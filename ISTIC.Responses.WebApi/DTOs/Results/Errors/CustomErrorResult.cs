namespace ISTIC.Responses.WebApi.DTOs.Results.Errors;

public class CustomErrorResult
{
    public string Title { get; set; }
    public string Description { get; set; }

    public CustomErrorResult(string title, string description)
    {
        Title = title;
        Description = description;
    }
}