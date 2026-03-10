using ISTIC.Responses.Core;
using Microsoft.AspNetCore.Mvc;

namespace ISTIC.Responses.WebApi.Features.Responses;

[ApiController]
[Route("api/[controller]")]
public class ResponseController : ControllerBase
{
    /// <summary>
    /// Lista todos os produtos.
    /// Envie simulateError=true para simular um InternalServerError.
    /// </summary>
    [HttpGet]
    public async Task<ResponseOf<List<ProductRequest>>> GetAll([FromQuery] bool simulateError = false)
    {
        return await Task.FromResult(Methods.GetAll.Handle(simulateError));
    }

    /// <summary>
    /// Busca um produto pelo Id.
    /// Envie um Guid vazio para simular BadRequest.
    /// Envie simulateUnauthorized=true para simular UnauthorizedError.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ResponseOf<ProductRequest>> GetById(Guid id, [FromQuery] bool simulateUnauthorized = false)
    {
        return await Task.FromResult(Methods.GetById.Handle(id, simulateUnauthorized));
    }

    /// <summary>
    /// Cria um novo produto.
    /// Envie Name vazio para simular BadRequest de validação.
    /// Envie Price menor ou igual a 0 para simular BadRequest de validação.
    /// Envie Name="Duplicado" para simular Conflict (409).
    /// </summary>
    [HttpPost]
    public async Task<ResponseOf<RegisterResult<Guid>>> Create([FromBody] CreateProductRequest request)
    {
        return await Task.FromResult(Methods.Create.Handle(request));
    }

    /// <summary>
    /// Atualiza um produto existente.
    /// Envie um Guid vazio para simular BadRequest.
    /// Envie um Id diferente de 00000000-0000-0000-0000-000000000001 para simular NotFound.
    /// Envie simulateForbidden=true para simular ForbiddenError.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<Response> Update(Guid id, [FromBody] UpdateProductRequest request, [FromQuery] bool simulateForbidden = false)
    {
        return await Task.FromResult(Methods.Update.Handle(id, request, simulateForbidden));
    }

    /// <summary>
    /// Deleta um produto pelo Id.
    /// Envie um Guid vazio para simular BadRequest.
    /// Envie um Id diferente de 00000000-0000-0000-0000-000000000001 para simular NotFound.
    /// Envie simulateInternalError=true para simular InternalServerError.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<Response> Delete(Guid id, [FromQuery] bool simulateInternalError = false)
    {
        return await Task.FromResult(Methods.Delete.Handle(id, simulateInternalError));
    }
}
