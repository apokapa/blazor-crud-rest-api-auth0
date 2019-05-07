using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessModels = Xample.Services.ToDo.Business.BusinessModels;

namespace Xample.Services.ToDo.Business.Interfaces
{
    // This is the Contract that any service used by the api needs to implement. 
    public interface ITodoService
    {
        Task<List<BusinessModels.Todo>> GetAllTodos();
        Task<BusinessModels.Todo> GetTodo(Guid todo);
        Task<BusinessModels.Todo> AddTodo(BusinessModels.Todo newTodo);
        Task UpdateTodo(Guid todoId, BusinessModels.Todo updatedToDo);
        Task<bool> DeleteTodo(Guid todoId);    
    }
}
