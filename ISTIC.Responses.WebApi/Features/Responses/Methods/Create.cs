using ISTIC.Responses.Core;
using ISTIC.Responses.WebApi.DTOs.Results;
using ISTIC.Responses.WebApi.DTOs.Results.Errors;
using System.Net;
using ErrorFactoryHelper = ISTIC.Responses.Extensions.ErrorFactory;

namespace ISTIC.Responses.WebApi.Features.Responses.Methods;

public static class Create
{
    public static CustomResponseOf<RegisterResult<Guid>, CustomErrorResult> Handle(CreateProductRequest request, bool simulateCustomError = false)
    {
        if (simulateCustomError)
            return ErrorFactoryHelper.CustomError("Já existe um produto com este nome.", HttpStatusCode.Conflict, null, new CustomErrorResult("Conflito", "Já existe um produto com este nome."));

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            var fieldErrors = new Dictionary<string, List<string>>
            {
                { "name", ["O campo Nome é obrigatório."] }
            };

            return ErrorFactoryHelper.CustomErrorWithoutData<CustomErrorResult>("Erro de validação ao criar o produto.", HttpStatusCode.BadRequest, fieldErrors);
        }

        if (request.Price <= 0)
        {
            var fieldErrors = new Dictionary<string, List<string>>
            {
                { "price", ["O preço deve ser maior que zero."] }
            };

            return ErrorFactoryHelper.CustomErrorWithoutData<CustomErrorResult>("Erro de validação ao criar o produto.", HttpStatusCode.BadRequest, fieldErrors);
        }

        return new RegisterResult<Guid> { Id = Guid.NewGuid() };
    }
}
