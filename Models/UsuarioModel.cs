namespace ConteiAPI.Models
{
    public class UsuarioModel
    {
        public required Guid Id { get; set; }
        public required string Nome { get; set; }
        public required string Usuario { get; set; }
        public string Senha { get; set; } = string.Empty;
        public int? DepartamentoID { get; set; }
        public string? SapID { get; set; }
        public string? Label { get; set; }
    }
}
                        