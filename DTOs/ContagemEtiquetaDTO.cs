using ConteiAPI.Models;

namespace ConteiAPI.DTOs
{
    public class ContagemEtiquetaDTO
    {
        public Guid Id { get; set; }
        public Guid ContagemID { get; set; }
        public EtiquetaDTO Etiqueta { get; set; }
        public string UsuarioID { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public int Quantidade { get; set; }
        public bool Status { get; set; } = true;
    }
}
