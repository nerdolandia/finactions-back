using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FinActions.Domain.Usuarios;
using FinActions.Contracts.Response;
using FinActions.Contracts.Token;
using FinActions.Contracts.Usuario;
using FinActions.Contracts.Usuario.Papel;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace FinActions.Application.Usuarios;

public class UsuarioService : IUsuarioService
{
    private readonly IHttpContextAccessor _httpContextAcessor;
    private readonly IUsuarioRepository _usuarioRepository;
    private string? _userId;

    public UsuarioService(
        IHttpContextAccessor httpContextAcessor,
        IUsuarioRepository usuarioRepository)
    {
        _httpContextAcessor = httpContextAcessor;
        _usuarioRepository = usuarioRepository;
    }

    public Task<bool> EhEmailExistente(string email) => _usuarioRepository.EhEmailExistente(email);

    public Task<bool> EhUsuarioExistente(Guid id) => _usuarioRepository.EhUsuarioExistente(id);

    public async Task<UsuarioDto> Inserir(InsertUpdateUsuarioDto insertUsuario)
    {
        _userId = _httpContextAcessor.HttpContext!.User.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

        var salt = GerarSalt(UsuarioConsts.TamanhoSalt);
        var senhaCriptografada = CriptografarSenha(insertUsuario.Senha, salt);
        var newUsuario = await _usuarioRepository.Inserir(new Usuario()
        {
            Nome = insertUsuario.Nome,
            Senha = senhaCriptografada,
            Salt = Convert.ToHexString(salt),
            Email = insertUsuario.Email,
            CreatedBy = Guid.Parse(_userId!),
            CreationDate = DateTime.UtcNow
        });

        return new UsuarioDto(
                newUsuario.Id,
                newUsuario.Nome,
                newUsuario.Email,
                newUsuario.CreationDate,
                newUsuario.CreatedBy,
                newUsuario.EditedDate,
                newUsuario.EditedBy,
                newUsuario.Papeis.Select(x => new PapelDto(x.Id, x.Nome))
            );
    }

    public async Task<ResultadoPaginadoDto<UsuarioDto>> Obter(
        GetUsuarioDto parametros)
    {
        var filtros = (Usuario x) => true;

        if (!string.IsNullOrEmpty(parametros.Nome))
            filtros = x => x.Nome.Contains(parametros.Nome);
        if (!string.IsNullOrEmpty(parametros.Email))
            filtros = x => filtros(x) && x.Email.Contains(parametros.Email);

        var usuarios = (await _usuarioRepository.ObterPaginadoComFiltros(
                                                        parametros.SkipCount,
                                                        parametros.TakeCount,
                                                        filtros)).ToList();
        var dtos = new List<UsuarioDto>();
        usuarios.ForEach(x =>
        {
            var papeis = x.Papeis.Select(x => new PapelDto(x.Id, x.Nome));

            dtos.Add(new UsuarioDto(x.Id,
                                    x.Nome,
                                    x.Email,
                                    x.CreationDate,
                                    x.CreatedBy,
                                    x.EditedDate,
                                    x.EditedBy,
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
        _userId = _httpContextAcessor.HttpContext!.User.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
        var usuarioToUpdate = await _usuarioRepository.ObterPorId(id);

        if (!string.IsNullOrEmpty(usuarioToUpdate.Nome) && usuarioToUpdate.Nome != updateUsuario.Nome)
            usuarioToUpdate.Nome = updateUsuario.Nome;

        if (!string.IsNullOrEmpty(usuarioToUpdate.Email) && usuarioToUpdate.Email != updateUsuario.Email)
            usuarioToUpdate.Email = updateUsuario.Email;

        if (!await CompararSenhas(updateUsuario.Senha, usuarioToUpdate.Senha, Convert.FromHexString(usuarioToUpdate.Salt)))
        {
            var salt = GerarSalt(UsuarioConsts.TamanhoSalt);
            var novaSenha = CriptografarSenha(updateUsuario.Senha, salt);
            usuarioToUpdate.Salt = Convert.ToHexString(salt);
            usuarioToUpdate.Senha = updateUsuario.Senha;
        }

        usuarioToUpdate.EditedBy = Guid.Parse(_userId!);
        usuarioToUpdate.EditedDate = DateTime.UtcNow;

        var updatedUsuario = await _usuarioRepository.Atualizar(usuarioToUpdate);

        return new UsuarioDto(
                updatedUsuario.Id,
                updatedUsuario.Nome,
                updatedUsuario.Email,
                updatedUsuario.CreationDate,
                updatedUsuario.CreatedBy,
                updatedUsuario.EditedDate,
                updatedUsuario.EditedBy,
                updatedUsuario.Papeis.Select(x => new PapelDto(x.Id, x.Nome))
            );
    }

    public async Task<UsuarioDto> Excluir(Guid id)
    {
        _userId = _httpContextAcessor.HttpContext!.User.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
        var usuarioToUpdate = await _usuarioRepository.ObterPorId(id);

        usuarioToUpdate.DeletedDate = DateTime.UtcNow;
        usuarioToUpdate.DeletedBy = Guid.Parse(_userId!);
        usuarioToUpdate.IsDeleted = true;

        var updatedUsuario = await _usuarioRepository.Excluir(usuarioToUpdate);

        return new UsuarioDto(
                updatedUsuario.Id,
                updatedUsuario.Nome,
                updatedUsuario.Email,
                updatedUsuario.CreationDate,
                updatedUsuario.CreatedBy,
                updatedUsuario.EditedDate,
                updatedUsuario.EditedBy,
                updatedUsuario.Papeis.Select(x => new PapelDto(x.Id, x.Nome))
            );
    }

    public async Task<UsuarioDto> ObterPorId(Guid id)
    {
        var usuario = await _usuarioRepository.ObterPorId(id);
        return new UsuarioDto(
                usuario.Id,
                usuario.Nome,
                usuario.Email,
                usuario.CreationDate,
                usuario.CreatedBy,
                usuario.EditedDate,
                usuario.EditedBy,
                usuario.Papeis.Select(x => new PapelDto(x.Id, x.Nome))
            );
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

    public async Task<bool> SenhaEhIgual(PostTokenDto dadosLogin)
    {
        var usuario = await _usuarioRepository.ObterPorEmail(dadosLogin.Email);

        return await CompararSenhas(dadosLogin.Senha, usuario.Senha, Convert.FromHexString(usuario.Salt));
    }

    private static Task<bool> CompararSenhas(string senhaDigitada, string hashSenhaAtual, byte[] salt)
    {
        var senhaDigitadaHash = CriptografarSenha(senhaDigitada, salt);

        var result = CryptographicOperations
                        .FixedTimeEquals(Convert.FromHexString(senhaDigitadaHash),
                                        Convert.FromHexString(hashSenhaAtual));

        return Task.FromResult(result);
    }
}