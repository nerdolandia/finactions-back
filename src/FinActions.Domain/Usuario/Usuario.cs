using FinActions.Domain.Usuario.Papel;

namespace FinActions.Domain.Usuario;

public class Usuario
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public DateTime CreationTime { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedTime { get; set; }
    public IEnumerable<Papel.Papel> Papeis { get; set; } = [];
}