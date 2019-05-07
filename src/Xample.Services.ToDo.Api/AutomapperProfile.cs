using AutoMapper;
using BusinessModels = Xample.Services.ToDo.Business.BusinessModels;
using InMemoryRepositoryModels = Xample.Services.ToDo.DataAccess.InMemoryRepositoryModels;

namespace Xample.Services.Todo.Api
{
    // Automapper helps with converting models from one layer to another layer model so we dont have to it manually.
    public class AutomapperProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AutomapperProfile()
        {
            AddApiModelsToBusinessModelMappings();
            AddBusinessModelToApiModelMappings();
            AddBusinessModelToInMemoryRepositoryModelMappings();
            AddInMemoryRepositoryModelToBusinessModelMappings();
        }

        private void AddApiModelsToBusinessModelMappings()
        {
            // We ignore the id because this is getting assigned in the business layer. update and insert models dont have Id.
            // CreateMap < source model, destination model> ().ExtraOptions...
            CreateMap<ApiModels.TodoUpsertModel, BusinessModels.Todo>()
                .ForMember(dest => dest.Id, options => options.Ignore());
        }

        private void AddBusinessModelToApiModelMappings()
        {
            CreateMap<BusinessModels.Todo, ApiModels.TodoModel>();
        }

        private void AddBusinessModelToInMemoryRepositoryModelMappings()
        {
            CreateMap<BusinessModels.Todo, InMemoryRepositoryModels.TodoEntity>();
        }

        private void AddInMemoryRepositoryModelToBusinessModelMappings()
        {
            CreateMap<InMemoryRepositoryModels.TodoEntity, BusinessModels.Todo>();
        }
    }
}
