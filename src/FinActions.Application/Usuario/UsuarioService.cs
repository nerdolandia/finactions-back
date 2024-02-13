using System.Security.Cryptography;
using System.Text;
using FinActions.Contracts.Response;
using FinActions.Contracts.Token;
using FinActions.Contracts.Usuario;
using FinActions.Contracts.Usuario.Papel;
using FinActions.Domain.Usuarios;
using FinActions.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FinActions.Application.Usuario;

public class UsuarioService : IUsuarioService
{
    private readonly FinActionsContext _context;
    private readonly IHttpContextAccessor _httpContextAcessor;

    public UsuarioService(
        FinActionsContext context,
        IHttpContextAccessor httpContextAcessor)
    {
        _context = context;
        _httpContextAcessor = httpContextAcessor;
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
        var creatorId = _httpContextAcessor.HttpContext!
                            .User.Claims.First(x => x.Type == "Id").Value;

        var salt = GerarSalt(UsuarioConsts.TamanhoSalt);
        var senhaCriptografada = CriptografarSenha(insertUsuario.Senha, salt);
        var newUsuario = _context.Usuarios.Add(new Domain.Usuarios.Usuario()
        {
            Nome = insertUsuario.Nome,
            Senha = senhaCriptografada,
            Salt = Convert.ToHexString(salt),
            Email = insertUsuario.Email,
            CreationTime = DateTime.UtcNow,
            CreatedBy = Guid.Parse(creatorId)
        }).Entity;

        _context.SaveChanges();

        return Task.FromResult(
            new UsuarioDto(
                newUsuario.Id,
                newUsuario.Nome,
                newUsuario.Email,
                newUsuario.CreationTime,
                newUsuario.CreatedBy,
                newUsuario.Papeis.Select(x => new PapelDto(x.Id, x.Nome))
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
            var papeis = x.Papeis.Select(x => new PapelDto(x.Id, x.Nome));

            dtos.Add(new UsuarioDto(x.Id,
                                    x.Nome,
                                    x.Email,
                                    x.CreationTime,
                                    x.CreatedBy,
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

    public async Task<UsuarioDto> Atualizar(
        Guid id,
        InsertUpdateUsuarioDto updateUsuario)
    {
        var usuarioToUpdate = _context.Usuarios
                        .First(x => x.Id == id);

        if (!string.IsNullOrEmpty(usuarioToUpdate.Nome) && usuarioToUpdate.Nome != updateUsuario.Nome)
            usuarioToUpdate.Nome = updateUsuario.Nome;

        if (!string.IsNullOrEmpty(usuarioToUpdate.Email) && usuarioToUpdate.Email != updateUsuario.Email)
            usuarioToUpdate.Email = updateUsuario.Email;

        if (!await CompararSenhas(updateUsuario.Senha, usuarioToUpdate.Senha, Encoding.ASCII.GetBytes(usuarioToUpdate.Salt)))
        {
            var salt = GerarSalt(UsuarioConsts.TamanhoSalt);
            var novaSenha = CriptografarSenha(updateUsuario.Senha, salt);
            usuarioToUpdate.Salt = Convert.ToHexString(salt);
            usuarioToUpdate.Senha = updateUsuario.Senha;
        }

        var updatedUsuario = _context.Usuarios.Update(usuarioToUpdate).Entity;

        _context.SaveChanges();

        return new UsuarioDto(
                updatedUsuario.Id,
                updatedUsuario.Nome,
                updatedUsuario.Email,
                updatedUsuario.CreationTime,
                updatedUsuario.CreatedBy,
                updatedUsuario.Papeis.Select(x => new PapelDto(x.Id, x.Nome))
            );
    }

    public Task<UsuarioDto> Excluir(Guid id)
    {
        var usuarioToUpdate = _context.Usuarios
                        .First(x => x.Id == id);

        usuarioToUpdate.DeletedDate = DateTime.UtcNow;
        usuarioToUpdate.IsDeleted = true;

        var updatedUsuario = _context.Usuarios.Update(usuarioToUpdate).Entity;

        _context.SaveChanges();

        return Task.FromResult(new UsuarioDto(
                updatedUsuario.Id,
                updatedUsuario.Nome,
                updatedUsuario.Email,
                updatedUsuario.CreationTime,
                updatedUsuario.CreatedBy,
                updatedUsuario.Papeis.Select(x => new PapelDto(x.Id, x.Nome))
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
                usuario.CreatedBy,
                usuario.Papeis.Select(x => new PapelDto(x.Id, x.Nome))
            ));
    }

    private static byte[] GerarSalt(int tamanho)
    {
        return RandomNumberGenerator.GetBytes(tamanho);
    }

    private static string CriptografarSenha(string senha, byte[] salt)
    {
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(senha),
            salt,
            UsuarioConsts.QtdeIteracoesCriptografiaSenha,
            HashAlgorithmName.SHA512,
            UsuarioConsts.TamanhoSalt);
        return Convert.ToHexString(hash);
    }

    public Task<bool> SenhaEhIgual(PostTokenDto dadosLogin)
    {
        var usuario = _context.Usuarios.First(x => x.Email == dadosLogin.Email);

        return CompararSenhas(dadosLogin.Senha, usuario.Senha, Convert.FromHexString(usuario.Salt));
    }

    private Task<bool> CompararSenhas(string senhaDigitada, string hashSenhaAtual, byte[] salt)
    {
        var senhaDigitadaHash = CriptografarSenha(senhaDigitada, salt);

        var result = CryptographicOperations
                        .FixedTimeEquals(Convert.FromHexString(senhaDigitadaHash),
                                        Convert.FromHexString(hashSenhaAtual));

        return Task.FromResult(result);
    }
}