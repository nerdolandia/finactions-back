
using FinActions.Domain.Shared.DependencyInjection;
using FinActions.Application.Validations.Base;
namespace FinActions.Application.Validations.ContaBancaria;

public interface IContaBancariaValidator : IBaseValidator, ITransientDependency
{
    IContaBancariaValidator ModelObject(object validationObject);
    IContaBancariaValidator DbEntityObject(object validationDbEntity);
    IContaBancariaValidator ApplyInsertRules();
    IContaBancariaValidator ApplyGetByIdRules();
    IContaBancariaValidator ApplyUpdateRules();
    IContaBancariaValidator ApplyDeleteRules();
}
