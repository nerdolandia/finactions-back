using FinActions.Application.Validations.Base;
using Microsoft.AspNetCore.Mvc;
using FinActions.Application.Categorias.Requests;
using Microsoft.AspNetCore.Http;
using DomainModel = FinActions.Domain.Categorias;
namespace FinActions.Application.Validations.Categoria;

public class CategoriaValidator : BaseValidator, ICategoriaValidator
{
    protected override string ModelValidationTitle { get; init; } = "Erro de validação da request de categorias";
    protected override string EntityValidationTitle { get; init; } = "Erro de validação do banco de dados";

    public ICategoriaValidator ModelObject(object validationObject)
    {
        _validationObject = validationObject;
        return this;
    }

    public ICategoriaValidator DbEntityObject(object validationDbEntity)
    {
        _validationEntity = validationDbEntity;
        return this;
    }

    public override ProblemDetails ValidateModel(out bool isValid)
    {
        if (_validationObject is GetCategoriaRequestDto getCategoriaRequest)
            ValidateGetRequest(getCategoriaRequest);
        else if (_validationObject is PostCategoriaRequestDto postCategoriaRequest)
            ValidatePostRequest(postCategoriaRequest);

        return base.ValidateModel(out isValid);
    }

    private void ValidateGetRequest(GetCategoriaRequestDto requestObject)
    {
        AddValidation<GetCategoriaRequestDto>
            (
                x => x.Nome.Length > 150,
                CategoriaValidatorConsts.ErroCategoriaJaExiste,
                nameof(requestObject.Nome)
            );
    }

    private void ValidatePostRequest(PostCategoriaRequestDto requestObject)
    {
        AddValidation<PostCategoriaRequestDto>(x => x.Nome.Length > 150,
                                                    CategoriaValidatorConsts.ErroTamanhoNome,
                                                    nameof(requestObject.Nome));

    }

    public ICategoriaValidator ApplyInsertRules()
        => AddEntityValidation<DomainModel.Categoria>
            (
                x => x is not null || !x.IsDeleted,
                CategoriaValidatorConsts.ErroCategoriaJaExiste,
                nameof(CategoriaValidatorConsts.ErroCategoriaJaExiste),
                StatusCodes.Status400BadRequest
            );

    public ICategoriaValidator ApplyGetByIdRules()
        => AddEntityValidation<DomainModel.Categoria>
            (
                x => x is null,
                CategoriaValidatorConsts.ErroNaoFoiEncontradoCategoria,
                nameof(CategoriaValidatorConsts.ErroNaoFoiEncontradoCategoria),
                StatusCodes.Status404NotFound
            );

    public ICategoriaValidator ApplyUpdateRules()
        => AddEntityValidation<DomainModel.Categoria>
             (
                 x => x is null || x.IsDeleted,
                 CategoriaValidatorConsts.ErroNaoFoiEncontradoCategoria,
                 nameof(CategoriaValidatorConsts.ErroNaoFoiEncontradoCategoria),
                 StatusCodes.Status404NotFound
             );

    public ICategoriaValidator ApplyDeleteRules()
        => AddEntityValidation<DomainModel.Categoria>
            (
                x => x is null || x.IsDeleted,
                CategoriaValidatorConsts.ErroNaoFoiEncontradoCategoria,
                nameof(CategoriaValidatorConsts.ErroNaoFoiEncontradoCategoria),
                StatusCodes.Status404NotFound
            );

    private protected override CategoriaValidator AddEntityValidation<T>(Predicate<T> predicate, string mensagemErro, string type, int statusCode)
    {
        base.AddEntityValidation(predicate, mensagemErro, type, statusCode);
        return this;
    }

    private protected override CategoriaValidator AddValidation<T>(Predicate<T> predicate, string mensagemErro, string campo = "")
    {
        base.AddValidation(predicate, mensagemErro, campo);
        return this;
    }
}
