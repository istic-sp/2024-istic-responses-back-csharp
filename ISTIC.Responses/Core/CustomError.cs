namespace ISTIC.Responses.Core;

public class CustomError<T> : Error
{
    public T Data { get; set; }

    public CustomError(string name, string description, Dictionary<string, List<string>> fieldErrors = null, T data = default) : base(name, description, fieldErrors)
    {
        Data = data;
    }

    /// <summary>
    /// Add field errors
    /// </summary>
    /// <param name="keyValues"></param>
    /// <returns></returns>
    public new CustomError<T> AddFieldErrors(params (string Key, string Value)[] keyValues)
    {
        foreach (var (key, value) in keyValues)
            FieldErrors.Add(key, value);

        return this;
    }
}
