using ISTIC.Responses.Interfaces;
using System.Net;

namespace ISTIC.Responses.Core;

public class CustomResponse<T> : IResponse
{
    public CustomError<T> CustomError { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public static CustomResponse<T> Success(HttpStatusCode statusCode = HttpStatusCode.OK) => new() { StatusCode = statusCode };
    public static CustomResponse<T> ErrorHandle(string name, string description, HttpStatusCode statusCode = HttpStatusCode.BadRequest, T data = default) => Throw(new CustomError<T>(name, description, data: data), statusCode);
    private static CustomResponse<T> Throw(CustomError<T> customError, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new()
    {
        CustomError = customError,
        StatusCode = customError.GetStatusCode().HasValue ? customError.GetStatusCode().Value : statusCode
    };

    public static implicit operator CustomResponse<T>(CustomError<T> customError) => Throw(customError);
}
