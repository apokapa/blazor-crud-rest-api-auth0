using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xample.Services.ToDo.Business.BusinessModels;
using Xample.Services.ToDo.Business.Interfaces;

namespace Xample.Services.ToDo.Business
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        /* TodoService constructor expects injection of something that implements the repository interface
        This can be an slq, mongo, in memory or any other implementation as long as implements the repository interface contract.*/
        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        #region Todo Service implementation
        public async Task<Todo> AddTodo(Todo newTodo)
        {
            // Apply business layer logic (generate id)
            newTodo.Id = Guid.NewGuid();

            // Add
            await _todoRepository.AddTodo(newTodo);

            // Return the newTodo
            return newTodo;
        }

        public async Task<bool> DeleteTodo(Guid todoId)
        {
            var deletedResult = await _todoRepository.DeleteTodo(todoId);
            return deletedResult;
        }

        public async Task<List<Todo>> GetAllTodos()
        {
            var todos = await _todoRepository.GetAllTodos();
            return todos;
        }

        public async Task<Todo> GetTodo(Guid todoId)
        {
            var todo = await _todoRepository.GetTodo(todoId);
            return todo;
        }

        public async Task UpdateTodo(Guid todoId, Todo updatedToDo)
        {
            await _todoRepository.UpdateTodo(todoId, updatedToDo);
        }
        #endregion
    }
}
