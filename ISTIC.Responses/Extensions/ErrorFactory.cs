using ISTIC.Responses.Core;
using System.Net;

namespace ISTIC.Responses.Extensions;

public static class ErrorFactory
{
    public static Error BadRequestError(string description = "Não foi possível realizar a requisição.", Dictionary<string, List<string>> fieldErrors = null)
        => new Error("Bad Request", description, fieldErrors)
            .SetStatusCode(HttpStatusCode.BadRequest);

    public static Error NotFoundError(string description = "O recurso solicitado não foi encontrado.", Dictionary<string, List<string>> fieldErrors = null)
        => new Error("Not Found", description, fieldErrors)
            .SetStatusCode(HttpStatusCode.NotFound);

    public static Error InternalServerError(string description = "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.", Dictionary<string, List<string>> fieldErrors = null)
        => new Error("Internal Server Error", description, fieldErrors)
            .SetStatusCode(HttpStatusCode.InternalServerError);

    public static Error UnauthorizedError(string description = "Você não foi autorizado para acessar este recurso.", Dictionary<string, List<string>> fieldErrors = null)
        => new Error("Unauthorized", description, fieldErrors)
            .SetStatusCode(HttpStatusCode.Unauthorized);

    public static Error ForbiddenError(string description = "Você não tem permissão para acessar este recurso.", Dictionary<string, List<string>> fieldErrors = null)
        => new Error("Forbidden", description, fieldErrors)
            .SetStatusCode(HttpStatusCode.Forbidden);

    public static Error GenericError(string description, HttpStatusCode statusCode, Dictionary<string, List<string>> fieldErrors = null)
        => new Error(statusCode.ToString(), description, fieldErrors)
            .SetStatusCode(statusCode);
}
