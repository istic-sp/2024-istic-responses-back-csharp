using ISTIC.Responses.Core;
using ErrorFactoryHelper = ISTIC.Responses.Extensions.ErrorFactory;

namespace ISTIC.Responses.WebApi.Features.Responses.Methods;

public static class Update
{
    public static Response Handle(Guid id, UpdateProductRequest request, bool simulateForbidden)
    {
        if (simulateForbidden)
            return ErrorFactoryHelper.ForbiddenError();

        if (id == Guid.Empty)
            return ErrorFactoryHelper.BadRequestError("O Id informado não é válido.", new Dictionary<string, List<string>>
            {
                { "id", ["O campo Id não pode ser vazio."] }
            });

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            var fieldErrors = new Dictionary<string, List<string>>
            {
                { "name", ["O campo Nome é obrigatório."] }
            };

            return ErrorFactoryHelper.BadRequestError("Erro de validação ao atualizar o produto.", fieldErrors);
        }

        var existingProductId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        if (id != existingProductId)
            return ErrorFactoryHelper.NotFoundError($"Produto com Id '{id}' não foi encontrado.");

        return Response.Success();
    }
}
