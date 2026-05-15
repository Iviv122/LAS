using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Humanizer;
using LAS.Lib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace LAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkItemsController : ControllerBase
    {
        private readonly LinkContext _context;

        public LinkItemsController(LinkContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems/{hash}
        [HttpGet("{hash}")]
        public async Task<ActionResult> GetTodoItem(string hash)
        {

            var todoItem = await _context.LinkItems.FindAsync(Base62.Decode(hash));

            if (todoItem == null)
            {
                return NotFound();
            }
            if (!IsAbsoluteUrl(todoItem.Url))
            {
                return BadRequest("Invalid response");
            }


            return Redirect(todoItem.Url);
        }
        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LinkItemDTO>> PostTodoItem(LinkItemDTO todoItemDTO)
        {
            LinkItem todoItem = DTOToItem(todoItemDTO);

            if (!IsAbsoluteUrl(todoItem.Url))
            {
                return BadRequest("No http:// or https://");
            }

            _context.LinkItems.Add(todoItem);
            await _context.SaveChangesAsync();

            var hash = Base62.Encode(todoItem.Id);

            todoItemDTO.Url = Url.Action(
                        nameof(GetTodoItem),
                        ControllerContext.RouteData.Values["controller"].ToString(),
                        new { hash },
                        Request.Scheme);



            return CreatedAtAction(
                nameof(GetTodoItem),
                new { hash = Base62.Encode(todoItem.Id) },
                todoItemDTO);
        }

        private static LinkItemDTO ItemToDTO(LinkItem todoItem) =>
            new LinkItemDTO
            {
                Url = todoItem.Url,
            };
        private static LinkItem DTOToItem(LinkItemDTO todoItem) =>
            new LinkItem
            {
                Url = todoItem.Url,
            };
        private bool IsAbsoluteUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }

    }
}
