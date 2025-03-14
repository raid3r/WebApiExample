using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using WebApiExample.Models;
using WebApiExample.Models.Rest;

namespace WebApiExample.Controllers
{
    [Route("api/v1/todo-list")]
    [Authorize]
    [ApiController]
    public class TodoListController(SiteContext context) : ControllerBase
    {
        private int GetUserId()
        {
            var id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return int.Parse(id);
        }   

        [HttpGet]
        public IEnumerable<ToDoListItemRest> GetList([FromQuery] ToToListItemFilter? filter)
        {
            // TODO use filter!
            var userId = GetUserId();
            
            return context.ToDoListItems
                .Include(x => x.User)
                .Where(x => x.User.Id == userId)
                .Select(x => new ToDoListItemRest
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    IsComplete = x.IsComplete,
                    CreatedAt = x.CreatedAt
                });
        }

        [HttpGet("{id}")]
        public ToDoListItem GetOne(int id)
        {
            return context.ToDoListItems
                .Include(x => x.User)
                .Where(x => x.User.Id == GetUserId())
                .First(x => x.Id == id)
                ;
        }

        [HttpPost("{id}")]
        public async Task<UpdateEntityResult> Update(int id, ToDoListItemRest item)
        {
            var userId = GetUserId();
            var itemToUpdate = await context.ToDoListItems
                .Include(x => x.User)
                .FirstAsync(x => x.Id == id && x.User.Id == userId);

            itemToUpdate.Title = item.Title;
            itemToUpdate.Description = item.Description;
            itemToUpdate.IsComplete = item.IsComplete;
            itemToUpdate.CreatedAt = item.CreatedAt;
            
            await context.SaveChangesAsync();

            return new UpdateEntityResult { Ok = true };
        }

        // create
        [HttpPut()]
        public async Task<ToDoListItem> Create(ToDoListItem item)
        {
            item.User = await context.Users.FirstAsync(x => x.Id == GetUserId());
            context.ToDoListItems.Add(item);
            await context.SaveChangesAsync();
            return item;
        }

        // delete
        [HttpDelete("{id}")]
        public async Task<UpdateEntityResult> Delete(int id)
        {
            var itemToDelete = await context.ToDoListItems
                .Include(x => x.User)
                .Where(x => x.User.Id == GetUserId())
                .FirstAsync(x => x.Id == id);
            context.ToDoListItems.Remove(itemToDelete);
            await context.SaveChangesAsync();
            return new UpdateEntityResult { Ok = true };
        }
    }
}
