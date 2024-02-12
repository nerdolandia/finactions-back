namespace FinActions.Contracts.Usuario;

public class InsertUpdateUsuarioDto
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
}