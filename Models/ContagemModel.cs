namespace ConteiAPI.Models
{
    public class ContagemModel
    {
        public Guid Id { get; set; }
        public DateTime Data { get; set; }
        public string Deposito { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}