using System.ComponentModel.DataAnnotations;

namespace Xample.Services.Todo.Api.ApiModels
{
    // Model used by api for updates and inserts
    public class TodoUpsertModel
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }
    }
}
