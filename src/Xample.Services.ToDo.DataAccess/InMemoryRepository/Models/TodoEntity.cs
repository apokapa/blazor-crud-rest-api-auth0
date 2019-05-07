using System;

namespace Xample.Services.ToDo.DataAccess.InMemoryRepositoryModels
{
    public class TodoEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
