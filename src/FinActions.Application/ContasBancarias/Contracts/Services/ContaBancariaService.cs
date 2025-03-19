using AutoMapper;
using FinActions.Application.Base.Responses;
using FinActions.Application.ContasBancarias.Contracts.Requests;
using FinActions.Application.ContasBancarias.Contracts.Responses;
using FinActions.Application.Validations.ContaBancaria;
using FinActions.Domain.ContasBancarias;
using FinActions.Domain.Shared.ContasBancarias;
using FinActions.Infrastructure.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinActions.Application.ContasBancarias.Contracts.Services;

public sealed class ContaBancariaService : IContaBancariaService
{
    private readonly FinActionsDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ContaBancariaService> _logger;
    private readonly IContaBancariaValidator _validator;

    public ContaBancariaService(
        ILogger<ContaBancariaService> logger,
        FinActionsDbContext context,
        IMapper mapper,
        IContaBancariaValidator validator)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Results<NoContent, ProblemHttpResult>> Delete(Guid contaBancariaId, Guid userId)
    {
        var contaBancaria = await _context.ContasBancarias
                                    .Where(x => x.UserId == userId
                                            && !x.IsDeleted
                                            && x.Id == contaBancariaId)
                                    .FirstOrDefaultAsync();

        var validation = _validator.DbEntityObject(contaBancaria)
                                    .ApplyDeleteRules()
                                    .ValidateEntity(out var isValid);
        
        if(!isValid)
            return TypedResults.Problem(validation);

        contaBancaria.IsDeleted = true;
        contaBancaria.DataModificacao = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public async Task<Results<Ok<ContaBancariaResponseDto>, ProblemHttpResult>> Insert(
        PostPutContaBancariaRequestDto insertRequest,
        Guid userId)
    {
        insertRequest.UserId = userId;

        var validation = _validator.ModelObject(insertRequest)
                                    .ValidateModel(out bool isValid);

        if(!isValid)
            return TypedResults.Problem(validation);

        var contaBancaria = _mapper.Map<PostPutContaBancariaRequestDto, ContaBancaria>(insertRequest);
        var contaBancariaDb = (await _context.ContasBancarias.AddAsync(contaBancaria)).Entity;
        await _context.SaveChangesAsync();

        return TypedResults.Ok(_mapper.Map<ContaBancaria, ContaBancariaResponseDto>(contaBancariaDb));
    }

    public async Task<Results<Ok<PagedResultDto<ContaBancariaResponseDto>>, ProblemHttpResult>> ObterContaBancarias(
        GetContaBancariaRequestDto getRequest,
        Guid userId)
    {
        var query = _context.ContasBancarias
                        .AsNoTracking()
                        .Where(x => x.UserId == userId && !x.IsDeleted);

        if (!string.IsNullOrEmpty(getRequest.Nome))
        {
            query = query.Where(x => x.Nome.Contains(getRequest.Nome, StringComparison.CurrentCultureIgnoreCase));
        }
        if (getRequest.TipoConta.HasValue)
        {
            query = query.Where(x => x.TipoConta.Equals(getRequest.TipoConta));
        }

        var count = await query.Select(x => x.Id).CountAsync();
        var dbEntities = await query.Skip(getRequest.Skip).Take(getRequest.Take).ToListAsync();

        var result = new PagedResultDto<ContaBancariaResponseDto>
        {
            Items = _mapper.Map<List<ContaBancaria>, List<ContaBancariaResponseDto>>(dbEntities),
            TotalCount = count
        };

        return TypedResults.Ok(result);
    }

    public async Task<Results<Ok<ContaBancariaResponseDto>, ProblemHttpResult>> ObterPorId(
        Guid contaBancariaId,
        Guid userId)
    {
        var contaBancaria = await _context.ContasBancarias
                                    .AsNoTracking()
                                    .Where(x => x.UserId == userId
                                            && !x.IsDeleted
                                            && x.Id == contaBancariaId)
                                    .FirstOrDefaultAsync();

        var entityValidation = _validator.DbEntityObject(contaBancaria)
                                            .ApplyGetByIdRules()
                                            .ValidateEntity(out var isValid);

        if(!isValid)
            return TypedResults.Problem(entityValidation);

        return TypedResults.Ok(_mapper.Map<ContaBancaria, ContaBancariaResponseDto>(contaBancaria));
    }

    public async Task<Results<Ok<ContaBancariaResponseDto>, ProblemHttpResult>> Update(
        Guid contaBancariaId,
        Guid userId,
        PostPutContaBancariaRequestDto updateRequest)
    {
        var validation = _validator.ModelObject(updateRequest)
                                    .ValidateModel(out var isValid);

        if(!isValid)
            return TypedResults.Problem(validation);

        var contaBancaria = await _context.ContasBancarias
                                    .Where(x => x.UserId == userId
                                            && !x.IsDeleted
                                            && x.Id == contaBancariaId)
                                    .FirstOrDefaultAsync();

        var entityValidation = _validator.DbEntityObject(contaBancaria)
                                            .ApplyUpdateRules()
                                            .ValidateEntity(out var isEntityValid);

        if(!isEntityValid)
            return TypedResults.Problem(entityValidation);

        contaBancaria.Nome = updateRequest.Nome;
        contaBancaria.TipoConta = updateRequest.TipoConta;
        contaBancaria.DataModificacao = DateTimeOffset.Now;
        await _context.SaveChangesAsync();

        return TypedResults.Ok(_mapper.Map<ContaBancaria, ContaBancariaResponseDto>(contaBancaria));
    }

}
