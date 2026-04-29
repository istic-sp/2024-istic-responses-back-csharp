using System.Net;

namespace ISTIC.Responses.Interfaces;

public interface IResponse
{
    public HttpStatusCode StatusCode { get; set; }
}
