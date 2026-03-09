using ISTIC.Responses.Core;
using ErrorFactoryHelper = ISTIC.Responses.Extensions.ErrorFactory;

namespace ISTIC.Responses.WebApi.Features.ErrorFactory.Methods;

public static class Delete
{
    public static Response Handle(Guid id, bool simulateInternalError)
    {
        if (simulateInternalError)
            return ErrorFactoryHelper.InternalServerError("Erro inesperado ao tentar deletar o produto.");

        if (id == Guid.Empty)
            return ErrorFactoryHelper.BadRequestError("O Id informado não é válido.", new Dictionary<string, List<string>>
            {
                { "id", ["O campo Id não pode ser vazio."] }
            });

        var existingProductId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        if (id != existingProductId)
            return ErrorFactoryHelper.NotFoundError($"Produto com Id '{id}' não foi encontrado.");

        return Response.Success();
    }
}
