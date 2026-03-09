using ISTIC.Responses.Core;
using ISTIC.Responses.WebApi.Features.ErrorFactory.Models;
using ErrorFactoryHelper = ISTIC.Responses.Extensions.ErrorFactory;

namespace ISTIC.Responses.WebApi.Features.ErrorFactory.Methods;

public static class GetAll
{
    private static readonly List<ProductModel> _products =
    [
        new() { Id = Guid.NewGuid(), Name = "Notebook", Description = "Notebook Dell Inspiron", Price = 4500.00m },
        new() { Id = Guid.NewGuid(), Name = "Mouse", Description = "Mouse Logitech G502", Price = 350.00m },
        new() { Id = Guid.NewGuid(), Name = "Teclado", Description = "Teclado Mecânico HyperX", Price = 600.00m }
    ];

    public static ResponseOf<List<ProductModel>> Handle(bool simulateError)
    {
        if (simulateError)
            return ErrorFactoryHelper.InternalServerError("Falha ao acessar o banco de dados para listar os produtos.");

        return _products;
    }
}
