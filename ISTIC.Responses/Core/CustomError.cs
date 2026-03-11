
namespace ISTIC.Responses.Core;

public class CustomError<T> : Error
{
    public T Data { get; set; }

    public CustomError(string name, string description, Dictionary<string, List<string>> fieldErrors = null, T data = default) : base(name, description, fieldErrors)
    {
        Data = data;
    }
}
