using ConteiAPI.DTOs;

namespace ConteiAPI.Interfaces.Services
{
    public interface IEtiquetaService
    {
        EtiquetaDTO? GetEtiquetaByQRCode(string qrCode);
        EtiquetaDTO? GetEtiquetaByID(int etiquetaID);
    }
}
