using Microsoft.AspNetCore.Mvc;
namespace FinActions.Application.Validations.Base;

public interface IBaseValidator
{
    ValidationProblemDetails ValidateModel(out bool isValid);
    ProblemDetails ValidateEntity(out bool isValid);
}
