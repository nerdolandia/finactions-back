namespace FinActions.Application.Validations.Models;
public sealed class ModelValidationDto()
{
    public string Title { get; set; }
    public Dictionary<string, string[]> Errors { get; set;} = [];
}

