using ISTIC.Responses.Core;
using ISTIC.Responses.WebApi.Features.ErrorFactory.Models;
using ErrorFactoryHelper = ISTIC.Responses.Extensions.ErrorFactory;

namespace ISTIC.Responses.WebApi.Features.ErrorFactory.Methods;

public static class GetById
{
    public static ResponseOf<ProductModel> Handle(Guid id, bool simulateUnauthorized)
    {
        if (simulateUnauthorized)
            return ErrorFactoryHelper.UnauthorizedError();

        if (id == Guid.Empty)
            return ErrorFactoryHelper.BadRequestError("O Id informado não é válido.", new Dictionary<string, List<string>>
            {
                { "id", ["O campo Id não pode ser vazio."] }
            });

        var product = new ProductModel
        {
            Id = id,
            Name = "Notebook",
            Description = "Notebook Dell Inspiron",
            Price = 4500.00m
        };

        return product;
    }
}
