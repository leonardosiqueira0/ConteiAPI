using System;

namespace ConteiAPI.Models
{
    public class ContagemEtiquetaModel
    {
        public Guid Id { get; set; }
        public Guid ContagemID { get; set; }
        public int EtiquetaID { get; set; }
        public string UsuarioID { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public int Quantidade { get; set; }
        public bool Status { get; set; } = true;
    }
}