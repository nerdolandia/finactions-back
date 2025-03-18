namespace FinActions.Application.Validations.Models;

public sealed class EntityValidationDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public int StatusCode { get; set; }

    public EntityValidationDto() {}

    public EntityValidationDto(string title, string description, string type, int statusCode)
    {
        Title = title;
        Description = description;
        Type = type;
        StatusCode = statusCode;
    }
}
