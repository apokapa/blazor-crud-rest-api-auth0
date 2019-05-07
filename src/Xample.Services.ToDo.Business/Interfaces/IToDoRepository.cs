using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xample.Services.ToDo.Business.Interfaces
{
    // This is the Contract that any underlying data storage used by the todoService needs to implement. 
    public interface ITodoRepository
    {
        Task<List<BusinessModels.Todo>> GetAllTodos();
        Task<BusinessModels.Todo> GetTodo(Guid todoId);
        Task AddTodo(BusinessModels.Todo newTodo);
        Task UpdateTodo(Guid todoId, BusinessModels.Todo updatedToDo);
        Task<bool> DeleteTodo(Guid todoId);
    }
}
