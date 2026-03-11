using ISTIC.Responses.Core;
using ErrorFactoryHelper = ISTIC.Responses.Extensions.ErrorFactory;

namespace ISTIC.Responses.WebApi.Features.Responses.Methods;

public static class Detail
{
    public static ResponseOf<ProductRequest> Handle(Guid id, bool simulateUnauthorized)
    {
        if (simulateUnauthorized)
            return ErrorFactoryHelper.UnauthorizedError();

        if (id == Guid.Empty)
            return ErrorFactoryHelper.BadRequestError("O Id informado não é válido.", new Dictionary<string, List<string>>
            {
                { "id", ["O campo Id não pode ser vazio."] }
            });

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
