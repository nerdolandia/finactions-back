using System.ComponentModel.DataAnnotations;
using FinActions.Domain.Usuarios;

namespace FinActions.Contracts.Usuario;

public class InsertUpdateUsuarioDto
{
    [Required]
    public string Nome { get; set; } = null!;
    [Required]
    [MaxLength(UsuarioConsts.LimiteCampoEmail)]
    public string Email { get; set; } = null!;
    [Required]
    [MaxLength(30)]
    public string Senha { get; set; } = null!;
}