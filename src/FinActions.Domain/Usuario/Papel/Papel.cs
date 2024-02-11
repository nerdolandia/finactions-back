namespace FinActions.Domain.Usuario.Papel;

public class Papel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public IEnumerable<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public IEnumerable<UsuarioPapel> UsuariosPapeis { get; set; } = new List<UsuarioPapel>();
}