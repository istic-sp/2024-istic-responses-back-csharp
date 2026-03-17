using ISTIC.Responses.Core;
using ISTIC.Responses.Extensions;

namespace ISTIC.Responses.WebApi.Features.Responses.Methods;

public static class Detail
{
    public static ResponseOf<ProductRequest> Handle(Guid id, bool simulateUnauthorized)
    {
        if (simulateUnauthorized)
            return ErrorFactory.UnauthorizedError();

        if (id == Guid.Empty)
            return ErrorFactory.BadRequestError("O Id informado não é válido.").AddFieldErrors(("id", "O campo Id não pode ser vazio."));

        var product = new ProductRequest
        {
            Items = new List<ProductRequestItem>
            {
                new ProductRequestItem { Id = id, Name = "Notebook", Description = "Notebook Dell Inspiron", Price = 4500.00m }
            }
        };

        return product;
    }
}
