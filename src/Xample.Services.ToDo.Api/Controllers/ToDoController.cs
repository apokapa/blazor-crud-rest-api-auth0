using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xample.Services.Todo.Api.ApiModels;
using Xample.Services.ToDo.Business.Interfaces;
using BusinessModels = Xample.Services.ToDo.Business.BusinessModels;

namespace Xample.Services.ToDo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITodoService _todoService;

        public ToDoController(IMapper mapper, ITodoService todoService)
        {
            _mapper = mapper;
            _todoService = todoService;
        }

        [HttpGet]
        [Authorize()]
        public async Task<IActionResult> GetAll()
        {
            // Example of getting a custom claim from access token
            // var username = User.Claims.First(x => x.Type == "CustomClaimName").Value;

            var todosFromService = await _todoService.GetAllTodos();

            // Map todos into the api model that is expected from the api clients (a front-end app, or another service)
            var todos = _mapper.Map<List<BusinessModels.Todo>, List<TodoModel>>(todosFromService);

            // Return OK result with the todos.
            return Ok(todos);
        }

        [HttpGet("{id}")]
        [Authorize()]
        public async Task<IActionResult> Get(Guid id)
        {
            // Get todos from service
            var todoFromService = await _todoService.GetTodo(id);

            // Map todos into the api model that is expected from the front end client
            var todo = _mapper.Map<BusinessModels.Todo, TodoModel>(todoFromService);

            // if null , not found.
            if (todo == null)
            {
                return NotFound();
            }

            // Return OK result with the todos.
            return Ok(todo);
        }

        [HttpPost]
        [Authorize()]
        public async Task<IActionResult> Post([FromBody] TodoUpsertModel todoUpsertModel)
        {
            var todo = _mapper.Map<TodoUpsertModel, BusinessModels.Todo>(todoUpsertModel);

            // Add newTodo
            var newTodoFromService = await _todoService.AddTodo(todo);

            // Map newTodo to apiModel
            var todoApiModel = _mapper.Map<BusinessModels.Todo, TodoModel>(newTodoFromService);

            // Return newTodo to client with the uri of the resource and created 201 result.
            return CreatedAtAction(nameof(Get), new { todoApiModel.Id }, todoApiModel);
        }

        [HttpPut("{id}")]
        [Authorize()]
        public async Task<IActionResult> Put(Guid id, [FromBody] TodoUpsertModel todoUpsertModel)
        {
            var todo = _mapper.Map<TodoUpsertModel, BusinessModels.Todo>(todoUpsertModel);
            await _todoService.UpdateTodo(id, todo);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleteResult = await _todoService.DeleteTodo(id);

            // If deleteResult (false) unsuccessful then nothing was found to delete
            if (deleteResult != true)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
