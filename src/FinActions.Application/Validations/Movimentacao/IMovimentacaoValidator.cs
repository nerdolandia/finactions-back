using FinActions.Application.Validations.Base;
using FinActions.Domain.Shared.DependencyInjection;
namespace FinActions.Application.Validations.Movimentacao;

public interface IMovimentacaoValidator : IBaseValidator, ITransientDependency
{

    IMovimentacaoValidator ModelObject(object validationObject);
    IMovimentacaoValidator DbEntityObject(object validationDbEntity);
    IMovimentacaoValidator ApplyInsertRules();
    IMovimentacaoValidator ApplyGetByIdRules();
    IMovimentacaoValidator ApplyUpdateRules();
    IMovimentacaoValidator ApplyDeleteRules();
}
