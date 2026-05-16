using LAS.Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using TodoApi.Models;

namespace LAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkItemsController : ControllerBase
    {
        private readonly LinkContext _context;
        private readonly IDistributedCache _cache;
        private readonly ILogger<LinkItemsController> _logger;
        private const string LinkCacheKey = "link";

        public LinkItemsController(
            LinkContext context,
            IDistributedCache cache,
            ILogger<LinkItemsController> logger
            )
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        // GET: api/TodoItems/{hash}
        [HttpGet("{hash}")]
        public async Task<ActionResult> GetTodoItem(string hash, CancellationToken cancellationToken = default)
        {

            var cacheKey = $"link:{hash}";

            _logger.LogInformation($"Fetching data for key: {cacheKey}",cacheKey);

            var linkItem = await _cache.GetOrSetAsync(
                cacheKey,
                async () =>
                {
                    _logger.LogInformation($"Cache missing for key {cacheKey}",cacheKey);
                    return await _context.LinkItems.FindAsync(Base62.Decode(hash));
                },
                cancellationToken: cancellationToken);

            if (linkItem == null)
            {
                return NotFound();
            }
            if (!IsAbsoluteUrl(linkItem.Url))
            {
                return BadRequest("Invalid response");
            }

            return Redirect(linkItem.Url);
        }
        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LinkItemDTO>> PostTodoItem(LinkItemDTO todoItemDTO,CancellationToken cancellationToken =default)
        {
            LinkItem todoItem = DTOToItem(todoItemDTO);

            if (!IsAbsoluteUrl(todoItem.Url))
            {
                return BadRequest("No http:// or https://");
            }

            await _context.LinkItems.AddAsync(todoItem);
            await _context.SaveChangesAsync(cancellationToken);

            string hash = Base62.Encode(todoItem.Id);

#pragma warning disable 
            todoItemDTO.Url = Url.Action(
                        nameof(GetTodoItem),
                        ControllerContext.RouteData.Values["controller"].ToString(),
                        new { hash },
                        Request.Scheme);
#pragma warning restore 

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
#pragma warning disable
            return Uri.TryCreate(url, UriKind.Absolute, out result);
#pragma warning restore
        }

    }
}
