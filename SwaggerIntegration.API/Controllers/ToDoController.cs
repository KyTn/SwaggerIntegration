using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwaggerIntegration.DAL.Contexts;
using SwaggerIntegration.DAL.Models;

namespace SwaggerIntegration.API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly TodoContext _todoContext;

        public ToDoController(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        /// <summary>
        /// Get the list of all Todo items in memory
        /// </summary>
        /// <remarks>
        /// GET /Todo
        /// </remarks>
        /// <returns>List of ToDos</returns>
        [HttpGet]
        public async Task<IEnumerable<ToDoItem>> Get()
        {
            return (await _todoContext.Get()).ToList();
        }

        /// <summary>
        /// Get Todo items in memory with given Id
        /// </summary>
        /// <remarks>
        /// Response example:
        /// After an POST example execution, GET /Todo/3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// 
        ///     {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "fatherId": null,
        ///         "title": "Test",
        ///         "description": "Test this",
        ///         "created": "2020-10-21T13:31:55.902Z",
        ///         "completed": true
        ///     }
        /// 
        /// </remarks>
        /// <returns>List of ToDos</returns>
        [HttpGet("{id}")]
        public async Task<IEnumerable<ToDoItem>> Get(Guid id)
        {
            return (await _todoContext.Get(id)).ToList();
        }

        /// <summary>
        /// Add a new aTodo item
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "fatherId": null,
        ///         "title": "Test",
        ///         "description": "Test this",
        ///         "created": "2020-10-21T13:31:55.902Z",
        ///         "completed": true
        ///     }
        /// 
        /// </remarks>
        /// <param name="todoItem">aTodo Item to add</param>
        /// <returns></returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>            
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ToDoItem>> PostTodoItem(ToDoItem todoItem)
        {
            await _todoContext.Create(todoItem);
            return CreatedAtAction(nameof(Get), new { id = todoItem.Id }, todoItem);
        }
    }

}
