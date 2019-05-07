using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Xample.Services.ToDo.Business.Interfaces;
using RepositoryModels = Xample.Services.ToDo.DataAccess.InMemoryRepositoryModels;
using BusinessModels = Xample.Services.ToDo.Business.BusinessModels;

namespace Xample.Services.ToDo.DataAccess
{
    public class InMemoryRepository : ITodoRepository
    {
        private List<RepositoryModels.TodoEntity> _anInMemoryDatabaseOfTodos;
        private readonly IMapper _mapper;

        // InMemoryRepository class constructor
        public InMemoryRepository(IMapper mapper)
        {
            SeedSomeData();
            _mapper = mapper;
        }

        #region Repository interface implementation
        public async Task AddTodo(BusinessModels.Todo newTodo)
        {
            // Map the BusinessLayer model to database model
            var newTodoEntity = _mapper.Map<BusinessModels.Todo, RepositoryModels.TodoEntity>(newTodo);

            // Add to database, Wrapping in Task.FromResult( result ) to fake database async call.
            _anInMemoryDatabaseOfTodos.Add(newTodoEntity);
        }

        public async Task<bool> DeleteTodo(Guid todoId)
        {
            var todo = await Task.FromResult(_anInMemoryDatabaseOfTodos.Where(td => td.Id == todoId).FirstOrDefault());

            if (todo != null)
            {
                _anInMemoryDatabaseOfTodos.Remove(todo);
                return true;
            }

            return false;
        }

        public async Task<List<BusinessModels.Todo>> GetAllTodos()
        {
            // Wrapping in Task.FromResult( result ) to fake database async call.
            var todosFromDatabase = await Task.FromResult(_anInMemoryDatabaseOfTodos.ToList());

            // Map the database model to BusinessLayer model
            var todos = _mapper.Map<List<RepositoryModels.TodoEntity> ,List<BusinessModels.Todo>>(todosFromDatabase);

            //Return mapped todos
            return todos;
        }

        public async Task<BusinessModels.Todo> GetTodo(Guid todoId)
        {
            // Wrapping in Task.FromResult( result ) to fake database async call.
            var todoFromDatabase = await Task.FromResult(_anInMemoryDatabaseOfTodos.Where(td => td.Id == todoId).FirstOrDefault());

            // Map the database model to BusinessLayer model
            var todo = _mapper.Map<RepositoryModels.TodoEntity, BusinessModels.Todo>(todoFromDatabase);
            
            //Return mapped todos
            return todo;
        }

        public async Task UpdateTodo(Guid todoId, BusinessModels.Todo updatedToDo)
        {
            // Wrapping in Task.FromResult( result ) to fake database async call.
            var todoFromDatabase = await Task.FromResult(_anInMemoryDatabaseOfTodos.Where(td => td.Id == todoId).FirstOrDefault());

            // If exists update to the new name.
            if (todoFromDatabase != null)
            {
                todoFromDatabase.Name = updatedToDo.Name;
            }
        }
        #endregion

        //Seeds some data into the in memory todos collection when the application is deployed/started.
        private void SeedSomeData()
        {
            // Initialise the List with 3 todos.
            _anInMemoryDatabaseOfTodos = new List<RepositoryModels.TodoEntity>
            {
                new RepositoryModels.TodoEntity()
                {
                    Id = Guid.Parse("e0f36c5f-7b2d-45bd-a646-8bb1198879ae"),
                    Name = "ToDo1"
                },
                new RepositoryModels.TodoEntity()
                {
                    Id = Guid.Parse("a7f01520-537d-47fb-afbf-796607e95701"),
                    Name = "ToDo2"
                },
                new RepositoryModels.TodoEntity()
                {
                    Id = Guid.Parse("b58fef20-6b39-4e99-9eec-23f14caa052c"),
                    Name = "ToDo3"
                }
            };
        }
    }
}
