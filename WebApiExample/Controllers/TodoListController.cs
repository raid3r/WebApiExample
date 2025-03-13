using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiExample.Models;

namespace WebApiExample.Controllers
{
    [Route("api/v1/todo-list")]
    [ApiController]
    public class TodoListController(SiteContext context) : ControllerBase
    {

        [HttpGet]
        public IEnumerable<ToDoListItem> GetList([FromQuery] ToToListItemFilter? filter)
        {
            // TODO use filter!
            
            return context.ToDoListItems
                .ToList()
                .Where(x => (filter?.CreatedAt) == null || x.CreatedAt == filter?.CreatedAt)
                .Where(x => (filter?.IsComplete) == null || x.IsComplete == filter?.IsComplete);
        }

        [HttpGet("{id}")]
        public ToDoListItem GetOne(int id)
        {
            return context.ToDoListItems.First(x => x.Id == id);
        }

        [HttpPost("{id}")]
        public async Task<UpdateEntityResult> Update(ToDoListItem item)
        {
            var itemToUpdate = await context.ToDoListItems.FirstAsync(x => x.Id == item.Id);
            itemToUpdate.Title = item.Title;
            itemToUpdate.Description = item.Description;
            itemToUpdate.IsComplete = item.IsComplete;
            
            await context.SaveChangesAsync();

            return new UpdateEntityResult { Ok = true };
        }

        // create
        [HttpPut()]
        public async Task<ToDoListItem> Create(ToDoListItem item)
        {
            context.ToDoListItems.Add(item);
            await context.SaveChangesAsync();
            return item;
        }

        // delete
        [HttpDelete("{id}")]
        public async Task<UpdateEntityResult> Delete(int id)
        {
            var itemToDelete = await context.ToDoListItems.FirstAsync(x => x.Id == id);
            context.ToDoListItems.Remove(itemToDelete);
            await context.SaveChangesAsync();
            return new UpdateEntityResult { Ok = true };
        }
    }
}
