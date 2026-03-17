using ISTIC.Responses.Core;
using ISTIC.Responses.Extensions;

namespace ISTIC.Responses.WebApi.Features.Responses.Methods;

public static class Delete
{
    public static Response Handle(Guid id, bool simulateInternalError)
    {
        if (simulateInternalError)
            return ErrorFactory.InternalServerError("Erro inesperado ao tentar deletar o produto.");

        if (id == Guid.Empty)
            return ErrorFactory.BadRequestError("O Id informado não é válido.").AddFieldErrors(("NomeDaPRop", "Não pode ser nulo"));

        var existingProductId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        if (id != existingProductId)
            return ErrorFactory.NotFoundError($"Produto com Id '{id}' não foi encontrado.");

        return Response.Success();
    }
}
