using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SwaggerIntegration.DAL.Contexts;
using SwaggerIntegration.DAL.Models;

namespace SwaggerIntegration.API.Controllers.v2
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion(V.v2_0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ToDoV2Controller : ControllerBase
    {
        private readonly TodoContext _todoContext;

        public ToDoV2Controller(TodoContext todoContext)
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
        [MapToApiVersion(V.v2_0)]
        [HttpGet]
        public async Task<IEnumerable<ToDoItem>> GetV2()
        {
            return (await _todoContext.Get()).ToList();
        }

    }
}