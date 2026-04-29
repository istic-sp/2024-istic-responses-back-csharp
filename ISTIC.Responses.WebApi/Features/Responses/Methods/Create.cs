using ISTIC.Responses.Core;
using ISTIC.Responses.Extensions;
using ISTIC.Responses.WebApi.DTOs.Results;
using ISTIC.Responses.WebApi.DTOs.Results.Errors;
using System.Net;

namespace ISTIC.Responses.WebApi.Features.Responses.Methods;

public static class Create
{
    public static CustomResponseOf<RegisterResult<Guid>, CustomErrorResult> Handle(CreateProductRequest request, bool simulateCustomError = false)
    {
        var fieldErrors = new Dictionary<string, List<string>>();

        if (simulateCustomError)
            return ErrorFactory.CustomError("Já existe um produto com este nome.", HttpStatusCode.Conflict, null, new CustomErrorResult("Erro customizado", "Um erro customizado."));

        if (string.IsNullOrWhiteSpace(request.Name))
            return ErrorFactory.CustomError<CustomErrorResult>("Erro de validação ao criar o produto.", HttpStatusCode.BadRequest).AddFieldErrors(("name", "O nome do produto é obrigatório."));

        if (request.Price <= 0)
            return ErrorFactory.CustomError<CustomErrorResult>("Erro de validação ao criar o produto.", HttpStatusCode.BadRequest).AddFieldErrors(("price", "O preço deve ser maior que zero."));

        return new RegisterResult<Guid> { Id = Guid.NewGuid() };
    }
}
