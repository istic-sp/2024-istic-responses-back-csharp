using ISTIC.Responses.Core;
using ISTIC.Responses.WebApi.Features.ErrorFactory.Models;
using System.Net;
using ErrorFactoryHelper = ISTIC.Responses.Extensions.ErrorFactory;

namespace ISTIC.Responses.WebApi.Features.ErrorFactory.Methods;

public static class Create
{
    public static ResponseOf<RegisterResult<Guid>> Handle(CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            var fieldErrors = new Dictionary<string, List<string>>
            {
                { "name", ["O campo Nome é obrigatório."] }
            };

            return ErrorFactoryHelper.BadRequestError("Erro de validação ao criar o produto.", fieldErrors);
        }

        if (request.Price <= 0)
        {
            var fieldErrors = new Dictionary<string, List<string>>
            {
                { "price", ["O preço deve ser maior que zero."] }
            };

            return ErrorFactoryHelper.BadRequestError("Erro de validação ao criar o produto.", fieldErrors);
        }

        if (request.Name.Equals("Duplicado", StringComparison.OrdinalIgnoreCase))
            return ErrorFactoryHelper.GenericError("Já existe um produto com este nome.", HttpStatusCode.Conflict);

        return new RegisterResult<Guid> { Id = Guid.NewGuid() };
    }
}
