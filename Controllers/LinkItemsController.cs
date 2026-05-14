using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
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
        public async Task<RedirectResult> GetTodoItem(string hash)
        {
            try
            {
                var todoItem = await _context.LinkItems.FindAsync(Base62.Decode(hash));

                if (todoItem == null)
                {
                    return Redirect(Request.PathBase);
                }

                return Redirect(todoItem.Url);
            }
            catch (ArgumentException)
            {
                return Redirect(Request.PathBase);
            }
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LinkItemDTO>> PostTodoItem(LinkItemDTO todoItemDTO)
        {
            LinkItem todoItem = DTOToItem(todoItemDTO);

            _context.LinkItems.Add(todoItem);
            await _context.SaveChangesAsync();

            var resultDto = ItemToDTO(todoItem);

            return CreatedAtAction(
                nameof(GetTodoItem),
                new {hash = Base62.Encode(todoItem.Id)},
                resultDto);
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
    }
}
