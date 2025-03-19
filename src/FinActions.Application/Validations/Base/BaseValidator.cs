using FinActions.Application.Validations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FinActions.Application.Validations.Base;

public abstract class BaseValidator : IBaseValidator
{
    private bool _isValid { get; set; } = true;
    protected private object _validationObject { get; set; }
    protected private object _validationEntity { get; set; }
    protected abstract string ModelValidationTitle { get; init; }
    protected string EntityValidationTitle { get; init; } = "Erro de validação do banco de dados";
    private ModelValidationDto _modelValidationDto { get; set; } = new();
    private EntityValidationDto _entityValidationDto { get; set; } = new();


    protected private virtual IBaseValidator AddValidation<T>(Predicate<T> invalidConditions, string errorMessage, string campo = "")
    {
        if (invalidConditions((T)_validationObject))
        {
            _isValid = false;

            if (_modelValidationDto.Errors.ContainsKey(campo))
                _modelValidationDto.Errors[campo].Append(errorMessage);
            else
                _modelValidationDto.Errors.Add(campo, [errorMessage]);
        }

        return this;
    }

    protected private virtual IBaseValidator AddEntityValidation<T>(Predicate<T> invalidConditions, string errorMessage, string type, int statusCode)
    {
        if (invalidConditions((T)_validationEntity))
        {
            _isValid = false;
            _entityValidationDto = new EntityValidationDto(EntityValidationTitle, errorMessage, type, statusCode);
        }

        return this;
    }


    public virtual ValidationProblemDetails ValidateModel(out bool isValid)
    {
        isValid = _isValid;

        return new ValidationProblemDetails
        {
            Title = _modelValidationDto.Title,
            Status = StatusCodes.Status400BadRequest,
            Detail = $"Erro(s) de validação(ões) em {_modelValidationDto.Errors.Count} itens",
            Errors = _modelValidationDto.Errors
        };

    }

    public ValidationProblemDetails ValidateEntity(out bool isValid)
    {
        isValid = _isValid;

        return new ValidationProblemDetails
        {
            Title = _entityValidationDto.Title,
            Detail = _entityValidationDto.Description,
            Type = _entityValidationDto.Type,
            Status = _entityValidationDto.StatusCode
        };
    }
}
