using FinActions.Contracts.Usuario.Papel;

namespace FinActions.Contracts.Usuario;

public record UsuarioDto(
    Guid Id,
    string Nome,
    string Email,
    DateTime CreationTime,
    IEnumerable<PapelDto> Papeis
);