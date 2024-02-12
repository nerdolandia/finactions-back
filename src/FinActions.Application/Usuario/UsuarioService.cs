using FinActions.Contracts.Response;
using FinActions.Contracts.Usuario;
using FinActions.Contracts.Usuario.Papel;
using FinActions.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FinActions.Application.Usuario;

public class UsuarioService : IUsuarioService
{
    private readonly FinActionsContext _context;

    public UsuarioService(FinActionsContext context)
    {
        _context = context;
    }

    public Task<bool> EhEmailExistente(string email)
    {
        return Task.FromResult(
            _context.Usuarios.Any(x => x.Email == email));
    }

    public Task<bool> EhUsuarioExistente(Guid id)
    {
        return Task.FromResult(
            _context.Usuarios.Any(x => x.Id == id));
    }

    public Task<UsuarioDto> Inserir(InsertUpdateUsuarioDto insertUsuario)
    {
        var newUsuario = _context.Usuarios.Add(new Domain.Usuario.Usuario()
        {
            Nome = insertUsuario.Nome,
            Senha = insertUsuario.Senha,
            Email = insertUsuario.Email,
            CreationTime = DateTime.UtcNow
        }).Entity;

        _context.SaveChanges();

        return Task.FromResult(
            new UsuarioDto(
                newUsuario.Id,
                newUsuario.Nome,
                newUsuario.Email,
                newUsuario.CreationTime,
                newUsuario.Papeis.Select(x => new PapelDto(x.Id, x.Name))
            ));
    }

    public async Task<ResultadoPaginadoDto<UsuarioDto>> Obter(
        GetUsuarioDto parametros)
    {
        var query = _context.Usuarios
                    .Include(x => x.Papeis)
                    .AsQueryable();

        if (!string.IsNullOrEmpty(parametros.Nome))
            query = query.Where(x => x.Nome.Contains(parametros.Nome));
        if (!string.IsNullOrEmpty(parametros.Email))
            query = query.Where(x => x.Email.Contains(parametros.Email));

        query = query.Skip(parametros.SkipCount)
                    .Take(parametros.TakeCount);

        var usuarios = await query.ToListAsync();
        var dtos = new List<UsuarioDto>();
        usuarios.ForEach(x =>
        {
            var papeis = x.Papeis.Select(x => new PapelDto(x.Id, x.Name));

            dtos.Add(new UsuarioDto(x.Id,
                                    x.Nome,
                                    x.Email,
                                    x.CreationTime,
                                    papeis));
        });

        return new ResultadoPaginadoDto<UsuarioDto>()
        {
            SkipCount = parametros.SkipCount,
            TakeCount = parametros.TakeCount,
            TotalCount = usuarios.Count,
            Items = dtos
        };
    }

    public Task<UsuarioDto> Atualizar(
        Guid id,
        InsertUpdateUsuarioDto insertUsuario)
    {
        var usuarioToUpdate = _context.Usuarios
                        .First(x => x.Id == id);

        usuarioToUpdate.Nome = insertUsuario.Nome;
        usuarioToUpdate.Email = insertUsuario.Email;
        usuarioToUpdate.Senha = insertUsuario.Senha;

        var updatedUsuario = _context.Usuarios.Update(usuarioToUpdate).Entity;

        _context.SaveChanges();

        return Task.FromResult(new UsuarioDto(
                updatedUsuario.Id,
                updatedUsuario.Nome,
                updatedUsuario.Email,
                updatedUsuario.CreationTime,
                updatedUsuario.Papeis.Select(x => new PapelDto(x.Id, x.Name))
            ));
    }

    public Task<UsuarioDto> Excluir(Guid id)
    {
        var usuarioToUpdate = _context.Usuarios
                        .First(x => x.Id == id);

        usuarioToUpdate.DeletedTime = DateTime.UtcNow;
        usuarioToUpdate.IsDeleted = true;

        var updatedUsuario = _context.Usuarios.Update(usuarioToUpdate).Entity;

        _context.SaveChanges();

        return Task.FromResult(new UsuarioDto(
                updatedUsuario.Id,
                updatedUsuario.Nome,
                updatedUsuario.Email,
                updatedUsuario.CreationTime,
                updatedUsuario.Papeis.Select(x => new PapelDto(x.Id, x.Name))
            ));
    }

    public Task<UsuarioDto> ObterPorId(Guid id)
    {
        var usuario = _context.Usuarios.First(x => x.Id == id);
        return Task.FromResult(new UsuarioDto(
                usuario.Id,
                usuario.Nome,
                usuario.Email,
                usuario.CreationTime,
                usuario.Papeis.Select(x => new PapelDto(x.Id, x.Name))
            ));
    }
}