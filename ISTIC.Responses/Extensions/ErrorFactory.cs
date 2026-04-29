using ISTIC.Responses.Core;
using System.Net;

namespace ISTIC.Responses.Extensions;

/// <summary>
/// Error factory
/// </summary>
public static class ErrorFactory
{
    /// <summary>
    /// Retorna um erro do tipo "Bad Request" (400) com uma descrição padrão, podendo ser personalizada, e opcionalmente erros de campo específicos para cada propriedade que falhou na validação.
    /// </summary>
    /// <param name="description"></param>
    /// <param name="fieldErrors"></param>
    /// <returns></returns>
    public static Error BadRequestError(string description = "Não foi possível realizar a requisição.", Dictionary<string, List<string>> fieldErrors = null)
        => new Error("Bad Request", description, fieldErrors)
            .SetStatusCode(HttpStatusCode.BadRequest);

    /// <summary>
    /// Retorna um erro do tipo "Not Found" (404) com uma descrição padrão, podendo ser personalizada, e opcionalmente erros de campo específicos para cada propriedade que falhou na validação.
    /// </summary>
    /// <param name="description"></param>
    /// <param name="fieldErrors"></param>
    /// <returns></returns>
    public static Error NotFoundError(string description = "O recurso solicitado não foi encontrado.", Dictionary<string, List<string>> fieldErrors = null)
        => new Error("Not Found", description, fieldErrors)
            .SetStatusCode(HttpStatusCode.NotFound);

    /// <summary>
    /// Retorna um erro do tipo "Internal Server Error" (500) com uma descrição padrão, podendo ser personalizada, e opcionalmente erros de campo específicos para cada propriedade que falhou na validação.
    /// </summary>
    /// <param name="description"></param>
    /// <param name="fieldErrors"></param>
    /// <returns></returns>
    public static Error InternalServerError(string description = "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.", Dictionary<string, List<string>> fieldErrors = null)
        => new Error("Internal Server Error", description, fieldErrors)
            .SetStatusCode(HttpStatusCode.InternalServerError);

    /// <summary>
    /// Retorna um erro do tipo "Unauthorized" (401) com uma descrição padrão, podendo ser personalizada, e opcionalmente erros de campo específicos para cada propriedade que falhou na validação.
    /// </summary>
    /// <param name="description"></param>
    /// <param name="fieldErrors"></param>
    /// <returns></returns>
    public static Error UnauthorizedError(string description = "Você não foi autorizado para acessar este recurso.", Dictionary<string, List<string>> fieldErrors = null)
        => new Error("Unauthorized", description, fieldErrors)
            .SetStatusCode(HttpStatusCode.Unauthorized);

    /// <summary>
    /// Retorna um erro do tipo "Forbidden" (403) com uma descrição padrão, podendo ser personalizada, e opcionalmente erros de campo específicos para cada propriedade que falhou na validação.
    /// </summary>
    /// <param name="description"></param>
    /// <param name="fieldErrors"></param>
    /// <returns></returns>
    public static Error ForbiddenError(string description = "Você não tem permissão para acessar este recurso.", Dictionary<string, List<string>> fieldErrors = null)
        => new Error("Forbidden", description, fieldErrors)
            .SetStatusCode(HttpStatusCode.Forbidden);

    /// <summary>
    /// Retorna um erro personalizado, permitindo especificar a descrição, o código de status HTTP e opcionalmente erros de campo específicos para cada propriedade que falhou na validação.
    /// </summary>
    /// <param name="description"></param>
    /// <param name="statusCode"></param>
    /// <param name="fieldErrors"></param>
    /// <returns></returns>
    public static Error CustomError(string description, HttpStatusCode statusCode, Dictionary<string, List<string>> fieldErrors = null)
        => new Error(statusCode.ToString(), description, fieldErrors)
            .SetStatusCode(statusCode);
    /// <summary>
    /// Retorna um erro personalizado do tipo "CustomError&lt;T&gt;", permitindo especificar a descrição, o código de status HTTP, opcionalmente erros de campo específicos para cada propriedade que falhou na validação e um objeto de dados do tipo T para fornecer informações adicionais sobre o erro.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="description"></param>
    /// <param name="statusCode"></param>
    /// <param name="fieldErrors"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static CustomError<T> CustomError<T>(string description, HttpStatusCode statusCode, Dictionary<string, List<string>> fieldErrors = null, T data = default)
        => (CustomError<T>) new CustomError<T>(statusCode.ToString(), description, fieldErrors, data)
            .SetStatusCode(statusCode);   
}