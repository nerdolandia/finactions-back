using FinActions.Domain.Usuarios.Papeis;

namespace FinActions.Domain.Usuarios;

public class UsuarioPapel
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public Guid PapelId { get; set; }
    public Papeis.Papel Papel { get; set; } = null!;
}