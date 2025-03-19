using FinActions.Application.Movimentacoes.Contracts.Requests;
using FinActions.Application.Validations.Base;
using FinActions.Domain.Shared.Movimentacoes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DomainModel = FinActions.Domain.Movimentacoes;

namespace FinActions.Application.Validations.Movimentacao;
public class MovimentacaoValidator : BaseValidator, IMovimentacaoValidator
{
    protected override string ModelValidationTitle { get; init; } = "Erro de validação da request de movimentação(ões)";

    public override ValidationProblemDetails ValidateModel(out bool isValid)
    {
        if (_validationObject is PostPutMovimentacaoRequestDto request)
            ValidatePost(request);

        return base.ValidateModel(out isValid);
    }

    private void ValidatePost(PostPutMovimentacaoRequestDto requestDto)
        => AddValidation<PostPutMovimentacaoRequestDto>(
                    x => string.IsNullOrWhiteSpace(x.Descricao),
                    MovimentacaoConsts.ErroMovimentacaoDescricaoVazio,
                    nameof(requestDto.Descricao)
                )
            .AddValidation<PostPutMovimentacaoRequestDto>(
                    x => x.Descricao.Length > MovimentacaoConsts.DescricaoMaxLength,
                    MovimentacaoConsts.ErroMovimentacaoNomeTamanhoMax,
                    nameof(requestDto.Descricao)
                    )
            .AddValidation<PostPutMovimentacaoRequestDto>(
                    x => x.ValorMovimentado <= 0 || x.ValorMovimentado > decimal.MaxValue,
                    MovimentacaoConsts.ErroValorMovimentadoInvalido,
                    nameof(requestDto.ValorMovimentado)
                    )
            .AddValidation<PostPutMovimentacaoRequestDto>(
                    x => x.DataMovimentacao > DateTimeOffset.MaxValue || x.DataMovimentacao < DateTimeOffset.MinValue,
                    MovimentacaoConsts.ErroDataMovimentacaoInvalida,
                    nameof(requestDto.DataMovimentacao)
                    );



    public IMovimentacaoValidator ApplyDeleteRules()
        => AddEntityValidation<DomainModel.Movimentacao>(
                x => x is null,
                MovimentacaoConsts.ErroMovimentacaoNaoEncontrada,
                nameof(MovimentacaoConsts.ErroMovimentacaoNaoEncontrada),
                StatusCodes.Status404NotFound
                );

    public IMovimentacaoValidator ApplyGetByIdRules()
        => AddEntityValidation<DomainModel.Movimentacao>(
                x => x is null,
                MovimentacaoConsts.ErroMovimentacaoNaoEncontrada,
                nameof(MovimentacaoConsts.ErroMovimentacaoNaoEncontrada),
                StatusCodes.Status404NotFound
                );

    //TODO: REGRAS NO VALIDATEMODEL AAAAAAA
    public IMovimentacaoValidator ApplyInsertRules()
    {
        throw new NotImplementedException();
    }

    public IMovimentacaoValidator ApplyUpdateRules()
    => AddEntityValidation<DomainModel.Movimentacao>(
            x => x is null,
            MovimentacaoConsts.ErroMovimentacaoNaoEncontrada,
            nameof(MovimentacaoConsts.ErroMovimentacaoNaoEncontrada),
            StatusCodes.Status404NotFound
            );

    public IMovimentacaoValidator DbEntityObject(object validationDbEntity)
    {
        _validationEntity = validationDbEntity;
        return this;
    }

    public IMovimentacaoValidator ModelObject(object validationObject)
    {
        _validationObject = validationObject;
        return this;
    }

    private protected override MovimentacaoValidator AddEntityValidation<T>(Predicate<T> invalidConditions, string errorMessage, string type, int statusCode)
    {
        base.AddEntityValidation(invalidConditions, errorMessage, type, statusCode);
        return this;
    }

    private protected override MovimentacaoValidator AddValidation<T>(Predicate<T> invalidConditions, string errorMessage, string campo = "")
    {
        base.AddValidation(invalidConditions, errorMessage, campo);
        return this;
    }
}
