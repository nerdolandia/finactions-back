using FinActions.Domain.Usuario.Papel;

namespace FinActions.Domain.Usuario;

public class UsuarioPapel
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public Guid PapelId { get; set; }
    public Papel.Papel Papel { get; set; } = null!;
}