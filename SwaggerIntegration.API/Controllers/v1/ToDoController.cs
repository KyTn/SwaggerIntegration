using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwaggerIntegration.DAL.Contexts;
using SwaggerIntegration.DAL.Models;

namespace SwaggerIntegration.API.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion(V.v1_0)]
    [ApiController]
    //[Route("[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly TodoContext _todoContext;

        public ToDoController(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        /// <summary>
        /// Get the list of all ToDoItems in memory
        /// </summary>
        /// <remarks>
        /// GET /Todo
        /// </remarks>
        /// <returns>List of ToDos</returns>
        [MapToApiVersion(V.v1_0)]
        [HttpGet]
        public async Task<IEnumerable<ToDoItem>> Get()
        {
            return (await _todoContext.Get()).ToList();
        }

        /// <summary>
        /// Get ToDoItems in memory with given Id
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
        /// Add a new ToDoItem
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
        public async Task<ActionResult<ToDoItem>> Create(ToDoItem todoItem)
        {
            var createdItem = await _todoContext.Create(todoItem);
            if (createdItem == null) return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = todoItem.Id }, todoItem);
        }


        /// <summary>
        /// Update an existing ToDoItem
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "fatherId": null,
        ///         "title": "Test",
        ///         "description": "New description",
        ///         "created": "2020-10-21T13:31:55.902Z",
        ///         "completed": true
        ///     }
        /// 
        /// </remarks>
        /// <param name="todoItem">ToDoItem to update</param>
        /// <returns></returns>
        /// <response code="200">Returns the updated item</response>
        /// <response code="400">If the item is null</response> 
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ToDoItem>> Update(ToDoItem todoItem)
        {
            var newItem = await _todoContext.Update(todoItem);
            if (newItem == null) return BadRequest();
            return Ok(newItem);
        }


        /// <summary>
        /// Delete an existing ToDoItem with the Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        /// 
        /// </remarks>
        /// <param name="todoItem">Id of the ToDoItem to delete</param>
        /// <returns></returns>
        /// <response code="200">Returns the deleted item</response>
        /// <response code="400">If the item is null</response> 
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ToDoItem>> Delete(Guid id)
        {
            var deletedItem = await _todoContext.Delete(id);
            if (deletedItem == null) return BadRequest();
            return Ok(deletedItem);
        }




    }

}
