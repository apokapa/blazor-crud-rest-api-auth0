using System;
using System.Collections.Generic;
using System.Text;

namespace Xample.Services.ToDo.Business.BusinessModels
{
    // Business Login aka Domain aka Service layer models.
    public class Todo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
