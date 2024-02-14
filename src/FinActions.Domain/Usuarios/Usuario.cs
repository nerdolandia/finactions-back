using FinActions.Domain.Base;

namespace FinActions.Domain.Usuarios;

public class Usuario : BaseEntity
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public IEnumerable<Papeis.Papel> Papeis { get; set; } = new List<Papeis.Papel>();
    public string Salt { get; set; } = null!;
}