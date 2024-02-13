namespace FinActions.Contracts.Categorias;

public record CategoriaDTO
(
    Guid Id,
    string Nome,
    string Cor,
    DateTime CreationTime
);
