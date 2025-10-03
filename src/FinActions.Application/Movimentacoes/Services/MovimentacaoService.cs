using AutoMapper;
using FinActions.Application.Base.Responses;
using FinActions.Application.Movimentacoes.Contracts.Requests;
using FinActions.Application.Movimentacoes.Contracts.Responses;
using FinActions.Application.Movimentacoes.Services;
using FinActions.Application.Validations.Movimentacao;
using FinActions.Domain.Movimentacoes;
using FinActions.Domain.Shared.ContasBancarias;
using FinActions.Domain.Shared.Movimentacoes;
using FinActions.Infrastructure.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinActions.Application.Movimentacoes.Services;

public sealed class MovimentacaoService : IMovimentacaoService
{
    private readonly FinActionsDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<MovimentacaoService> _logger;
    private readonly IMovimentacaoValidator _validator;

    public MovimentacaoService(
        ILogger<MovimentacaoService> logger,
        FinActionsDbContext context,
        IMapper mapper,
        IMovimentacaoValidator validator
        )
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Results<NoContent, ProblemHttpResult>> Delete(Guid MovimentacaoId, Guid userId)
    {
        var Movimentacao = await _context.Movimentacoes
                                    .FirstOrDefaultAsync(x => x.UserId == userId
                                            && !x.IsDeleted
                                            && x.Id == MovimentacaoId);

        if (Movimentacao is null)
        {
            return TypedResults.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: MovimentacaoConsts.ErroMovimentacaoNaoEncontrada,
                    type: nameof(MovimentacaoConsts.ErroMovimentacaoNaoEncontrada));
        }

        Movimentacao.IsDeleted = true;
        Movimentacao.DataModificacao = DateTimeOffset.Now;
        await _context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public async Task<Results<Ok<MovimentacaoResponseDto>, ProblemHttpResult>> Insert(
        PostPutMovimentacaoRequestDto insertRequest,
        Guid userId)
    {
        var validation = _validator.ModelObject(insertRequest)
                                    .ValidateModel(out var isValid);

        if (!isValid)
            return TypedResults.Problem(validation);

        var movimentacao = _mapper.Map<PostPutMovimentacaoRequestDto, Movimentacao>(insertRequest);
        movimentacao.UserId = userId;
        var movimentacaoDb = (await _context.Movimentacoes.AddAsync(movimentacao)).Entity;
        await _context.SaveChangesAsync();

        return TypedResults.Ok(_mapper.Map<Movimentacao, MovimentacaoResponseDto>(movimentacaoDb));
    }

    public async Task<Results<Ok<PagedResultDto<MovimentacaoResponseDto>>, ProblemHttpResult>> ObterMovimentacaos(
        GetMovimentacaoRequestDto getRequest,
        Guid userId)
    {
        var query = _context.Movimentacoes
                        .AsNoTrackingWithIdentityResolution()
                        .Include(x => x.ContaBancaria)
                        .Include(x => x.Categoria)
                        .Where(x => x.UserId == userId && !x.IsDeleted);

        if (getRequest.TipoMovimentacao.HasValue)
        {
            query = query.Where(x => x.TipoMovimentacao == getRequest.TipoMovimentacao);
        }
        if (!string.IsNullOrEmpty(getRequest.Descricao))
        {
            query = query.Where(x => x.Descricao.ToUpper().Contains(getRequest.Descricao.ToUpper()));
        }
        if (!string.IsNullOrEmpty(getRequest.Tag))
        {
            query = query.Where(x => x.Tag.ToUpper().Contains(getRequest.Tag.ToUpper()));
        }
        if (getRequest.DataMovimentacaoInicio.HasValue)
        {
            query = query.Where(x => x.DataMovimentacao >= getRequest.DataMovimentacaoInicio);
        }
        if (getRequest.DataMovimentacaoFim.HasValue)
        {
            query = query.Where(x => x.DataMovimentacao <= getRequest.DataMovimentacaoFim);
        }
        if (getRequest.ContaBancariaId.HasValue)
        {
            query = query.Where(x => x.ContaBancariaId == getRequest.ContaBancariaId);
        }
        if (getRequest.CategoriaId.HasValue)
        {
            query = query.Where(x => x.CategoriaId == getRequest.CategoriaId);
        }

        var count = await query.Select(x => x.Id).CountAsync();
        var dbEntities = await query.Skip(getRequest.Skip).Take(getRequest.Take).ToListAsync();

        var result = new PagedResultDto<MovimentacaoResponseDto>
        {
            Items = _mapper.Map<List<Movimentacao>, List<MovimentacaoResponseDto>>(dbEntities),
            TotalCount = count
        };

        return TypedResults.Ok(result);
    }

    public async Task<Results<Ok<MovimentacaoResponseDto>, ProblemHttpResult>> ObterPorId(
        Guid movimentacaoId,
        Guid userId)
    {
        var Movimentacao = await _context.Movimentacoes
                                    .AsNoTrackingWithIdentityResolution()
                                    .Include(x => x.Categoria)
                                    .Include(x => x.ContaBancaria)
                                    .Where(x => x.UserId == userId
                                            && !x.IsDeleted
                                            && x.Id == movimentacaoId)
                                    .FirstOrDefaultAsync();

        if (Movimentacao is null)
        {
            return TypedResults.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: MovimentacaoConsts.ErroMovimentacaoNaoEncontrada,
                type: nameof(MovimentacaoConsts.ErroMovimentacaoNaoEncontrada));
        }

        return TypedResults.Ok(_mapper.Map<Movimentacao, MovimentacaoResponseDto>(Movimentacao));
    }

    public async Task<Results<Ok<MovimentacaoResponseDto>, ProblemHttpResult>> Update(
        Guid MovimentacaoId,
        Guid userId,
        PostPutMovimentacaoRequestDto updateRequest)
    {
        var validation = _validator.ModelObject(updateRequest)
                                    .ValidateModel(out var isValid);

        if (!isValid)
            return TypedResults.Problem(validation);

        var movimentacao = await _context.Movimentacoes
                                    .Where(x => x.UserId == userId
                                            && !x.IsDeleted
                                            && x.Id == MovimentacaoId)
                                    .FirstOrDefaultAsync();

        var entityValidation = _validator.DbEntityObject(movimentacao)
                                            .ApplyUpdateRules()
                                            .ValidateEntity(out var isEntityValid);

        if(!isEntityValid)
            return TypedResults.Problem(entityValidation);

        movimentacao.TipoMovimentacao = updateRequest.TipoMovimentacao;
        movimentacao.Descricao = updateRequest.Descricao;
        movimentacao.Tag = updateRequest.Tag;
        movimentacao.Cor = updateRequest.Cor;
        movimentacao.ValorMovimentado = updateRequest.ValorMovimentado;
        movimentacao.DataMovimentacao = updateRequest.DataMovimentacao;
        movimentacao.ContaBancariaId = updateRequest.ContaBancariaId;
        movimentacao.CategoriaId = updateRequest.CategoriaId;
        movimentacao.DataModificacao = DateTimeOffset.Now;
        await _context.SaveChangesAsync();

        return TypedResults.Ok(_mapper.Map<Movimentacao, MovimentacaoResponseDto>(movimentacao));
    }

}
