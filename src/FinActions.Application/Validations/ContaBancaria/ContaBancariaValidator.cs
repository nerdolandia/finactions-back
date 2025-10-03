
using FinActions.Application.Validations.Base;
using Microsoft.AspNetCore.Mvc;
using FinActions.Application.ContasBancarias.Contracts.Requests;
using Microsoft.AspNetCore.Http;
using DomainModel = FinActions.Domain.ContasBancarias;
using FinActions.Domain.Shared.ContasBancarias;
namespace FinActions.Application.Validations.ContaBancaria;

public class ContaBancariaValidator : BaseValidator, IContaBancariaValidator
{
    protected override string ModelValidationTitle { get; init; } = "Erro de validação da request de conta(s) bancária(s)";

    public override ValidationProblemDetails ValidateModel(out bool isValid)
    {
        if (_validationObject is PostPutContaBancariaRequestDto)
            ValidatePostPutModel();
        return base.ValidateModel(out isValid);
    }

    private void ValidatePostPutModel()
        => AddValidation<PostPutContaBancariaRequestDto>(
                x => string.IsNullOrWhiteSpace(x.Nome),
                ContaBancariaConsts.ErroContaBancariaNomeVazio,
                nameof(ContaBancariaConsts.ErroContaBancariaNomeVazio)
                )
            .AddValidation<PostPutContaBancariaRequestDto>(
                    x => x.Nome.Length > ContaBancariaConsts.NomeMaxLength,
                    ContaBancariaConsts.ErroContaBancariaNomeTamanhoMax,
                    nameof(ContaBancariaConsts.ErroContaBancariaNomeTamanhoMax)
                    )
            .AddValidation<PostPutContaBancariaRequestDto>(
                    x => x.Saldo < 0 || x.Saldo > decimal.MaxValue,
                    ContaBancariaConsts.ErroContaBancariaSaldoInvalido,
                    nameof(ContaBancariaConsts.ErroContaBancariaSaldoInvalido)
                    );

    public IContaBancariaValidator ApplyDeleteRules()
        => AddEntityValidation<DomainModel.ContaBancaria>(
                x => x == null,
                ContaBancariaConsts.ErroContaBancariaNaoEncontrada,
                nameof(ContaBancariaConsts.ErroContaBancariaNaoEncontrada),
                StatusCodes.Status404NotFound
                );

    public IContaBancariaValidator ApplyGetByIdRules()
        => AddEntityValidation<DomainModel.ContaBancaria>(
                x => x is null,
                ContaBancariaConsts.ErroContaBancariaNaoEncontrada,
                nameof(ContaBancariaConsts.ErroContaBancariaNaoEncontrada),
                StatusCodes.Status404NotFound
                );

    public IContaBancariaValidator ApplyUpdateRules()
        => AddEntityValidation<DomainModel.ContaBancaria>(
                x => x is null,
                ContaBancariaConsts.ErroContaBancariaNaoEncontrada,
                nameof(ContaBancariaConsts.ErroContaBancariaNaoEncontrada),
                StatusCodes.Status404NotFound
                );

    public IContaBancariaValidator ApplyInsertRules()
    {
        throw new NotImplementedException();
    }

    public IContaBancariaValidator DbEntityObject(object validationDbEntity)
    {
        _validationEntity = validationDbEntity;
        return this;
    }

    public IContaBancariaValidator ModelObject(object validationObject)
    {
        _validationObject = validationObject;
        return this;
    }

    private protected override ContaBancariaValidator AddEntityValidation<T>(Predicate<T> invalidConditions, string mensagemErro, string type, int statusCode)
    {
        base.AddEntityValidation(invalidConditions, mensagemErro, type, statusCode);
        return this;
    }

    private protected override ContaBancariaValidator AddValidation<T>(Predicate<T> invalidConditions, string mensagemErro, string campo = "")
    {
        base.AddValidation(invalidConditions, mensagemErro, campo);
        return this;
    }

}
