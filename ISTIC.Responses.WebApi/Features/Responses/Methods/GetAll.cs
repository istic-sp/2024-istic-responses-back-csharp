using ISTIC.Responses.Core;
using ErrorFactoryHelper = ISTIC.Responses.Extensions.ErrorFactory;

namespace ISTIC.Responses.WebApi.Features.Responses.Methods;

public static class GetAll
{
    private static readonly ProductRequest _products =
    new ProductRequest
    {
        Items = new List<ProductRequestItem>
        {
            new ProductRequestItem { Id = Guid.NewGuid(), Name = "Notebook", Description = "Notebook Dell Inspiron", Price = 4500.00m },
            new ProductRequestItem { Id = Guid.NewGuid(), Name = "Mouse", Description = "Mouse Logitech G502", Price = 350.00m },
            new ProductRequestItem { Id = Guid.NewGuid(), Name = "Teclado", Description = "Teclado Mecânico HyperX", Price = 600.00m }
        }
    };

    public static ResponseOf<ProductRequest> Handle(bool simulateError)
    {
        if (simulateError)
            return ErrorFactoryHelper.InternalServerError("Falha ao acessar o banco de dados para listar os produtos.");

        return _products;
    }
}
