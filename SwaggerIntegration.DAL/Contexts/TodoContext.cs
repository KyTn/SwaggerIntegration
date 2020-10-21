using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SwaggerIntegration.DAL.Models;

namespace SwaggerIntegration.DAL.Contexts
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        private DbSet<ToDoItem> _todoItems { get; set; }


        public async Task<IEnumerable<ToDoItem>> Get()
        {
            return await _todoItems.ToListAsync();
        }


        public async Task<IEnumerable<ToDoItem>> Get(Guid id)
        {
            return await _todoItems.Where(x => x.Id == id).ToListAsync();
        }

        public async Task<ToDoItem> Create(ToDoItem item)
        {
            var response = await _todoItems.AddAsync(item);
            await this.SaveChangesAsync();
            return response.Entity;
        }

        public async Task<ToDoItem> Delete(Guid id)
        {
            var itemToRemove = (await Get(id)).FirstOrDefault();
            return itemToRemove == null ? null : _todoItems.Remove(itemToRemove).Entity;
        }

        public async Task<ToDoItem> Update(ToDoItem item)
        {
            var itemFound = (await Get(item.Id)).FirstOrDefault() != null;
            return itemFound ? _todoItems.Update(item).Entity : null;
        }
    }
}
