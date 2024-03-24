namespace FinActions.Domain.Usuarios.Papeis;

public class Papel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public IEnumerable<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
