using System;

namespace Xample.Services.Todo.Api.ApiModels
{
    // Api layer models (What front end expects).
    public class TodoModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
