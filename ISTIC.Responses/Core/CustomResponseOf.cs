using ISTIC.Responses.Interfaces;
using System.Net;

namespace ISTIC.Responses.Core;

public class CustomResponseOf<TResult, TError> : IResponse
{
    public CustomError<TError> CustomError { get; set; }
    public TResult Result { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public Error Error { get; set; }

    public CustomResponseOf() { }

    public CustomResponseOf(TResult result, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        Result = result;
        StatusCode = statusCode;
    }

    public CustomResponseOf(CustomError<TError> customError, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        CustomError = customError;
        StatusCode = customError.GetStatusCode().HasValue ? customError.GetStatusCode().Value : statusCode;
    }


    public static implicit operator CustomResponseOf<TResult, TError>(CustomResponse<TError> customResponse) => new CustomResponseOf<TResult, TError>
    {
        CustomError = customResponse.CustomError,
        StatusCode = customResponse.CustomError != null ? customResponse.CustomError.GetStatusCode().HasValue ? customResponse.CustomError.GetStatusCode().Value : customResponse.StatusCode : customResponse.StatusCode
    };

    public static implicit operator CustomResponseOf<TResult, TError>(TResult data) => new CustomResponseOf<TResult, TError>(data);

    public static implicit operator CustomResponseOf<TResult, TError>(CustomError<TError> error) => new CustomResponseOf<TResult, TError>(error);
}
