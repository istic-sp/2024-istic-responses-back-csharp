using ISTIC.Responses.Core;
using ISTIC.Responses.Extensions;

namespace ISTIC.Responses.WebApi.Features.Responses.Methods;

public static class Update
{
    public static Response Handle(Guid id, UpdateProductRequest request, bool simulateForbidden)
    {
        if (simulateForbidden)
            return ErrorFactory.ForbiddenError();

        if (id == Guid.Empty)
            return ErrorFactory.BadRequestError("O Id informado não é válido.").AddFieldErrors(("id", "O campo Id não pode ser vazio."));

        if (string.IsNullOrWhiteSpace(request.Name))
            return ErrorFactory.BadRequestError("Erro de validação ao atualizar o produto.").AddFieldErrors(("name", "O campo Nome é obrigatório."));

        var existingProductId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        if (id != existingProductId)
            return ErrorFactory.NotFoundError($"Produto com Id '{id}' não foi encontrado.");

        return Response.Success();
    }
}
