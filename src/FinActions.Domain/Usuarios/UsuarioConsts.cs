namespace FinActions.Domain.Usuarios;

public static class UsuarioConsts
{
    public const string ErroEmailJaCadastrado = "Email já cadastrado";
    public const string ErroJaExiste = "Usuário já existe";
    public const string ErroNaoExiste = "Usuário não existe";
    public const string ErroSenhaIncorreta = "Senha incorreta";
    public const int LimiteCampoEmail = 150;
    public const int TamanhoSalt = 64;
    public const int QtdeIteracoesCriptografiaSenha = 350000;
}