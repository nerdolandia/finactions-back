using FinActions.Contracts.Usuario.Papel;

namespace FinActions.Contracts.Usuario;

public record UsuarioDto(
    Guid Id,
    string Nome,
    string Email,
    DateTime CreationDate,
    Guid CreatedBy,
    DateTime? EditedDate,
    Guid? EditedBy,
    IEnumerable<PapelDto> Papeis
);