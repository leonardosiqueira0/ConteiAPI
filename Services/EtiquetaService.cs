using ConteiAPI.DTOs;
using ConteiAPI.Interfaces.Services;
using ConteiAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ConteiAPI.Services
{
    public class EtiquetaService : IEtiquetaService
    {
        private readonly SapService _sap;
        private readonly JsonSerializerOptions _jsonOptions;

        public EtiquetaService(SapService sapService)
        {
            _sap = sapService;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public EtiquetaDTO? GetEtiquetaByQRCode(string qrCode)
        {
            var response = _sap.SendRequestAsync(
                HttpMethod.Get,
                $"U_LGOIMPRESSCONF",
                queryParams: new Dictionary<string, string>
                {
                    { "$filter", $"U_QRCode eq '{qrCode}'" },
                    { "$select", "Code,U_CodigoProduto,U_Lote,U_QtdePallet,U_QRCode,U_Objeto,U_DocEntry,U_Sequencial" },
                },
                handlePagination: false
            ).Result;
            if (response is OkObjectResult okResult)
            {
                var jsonString = JsonSerializer.Serialize(okResult.Value);
                var jsonDocument = JsonDocument.Parse(jsonString);
                var listElement = jsonDocument.RootElement.GetProperty("value");
                var dtoList = JsonSerializer.Deserialize<List<EtiquetaSapDTO>>(listElement.GetRawText(), _jsonOptions);

                if (dtoList != null && dtoList.Count > 0)
                {
                    var dto = dtoList.First();

                    var produto = _sap.SendRequestAsync(
                        HttpMethod.Get,
                        $"Items('{dto.U_CodigoProduto}')",
                        queryParams: new Dictionary<string, string>
                        {
                            { "$select", "ItemCode,ItemName,BarCode,SalesItemsPerUnit,PurchaseItemsPerUnit,CountingItemsPerUnit,InventoryUOM,SalesPackagingUnit,U_LGO_QuantPallet" },
                        },
                        handlePagination: false
                    ).Result;

                    if (produto is OkObjectResult okProduto)
                    {
                        var produtoJson = JsonSerializer.Serialize(okProduto.Value);
                        var produtoDto = JsonSerializer.Deserialize<ProdutoSapDTO>(produtoJson, _jsonOptions);
                        if (produtoDto is ProdutoSapDTO)
                        {
                            return new EtiquetaDTO
                            {
                                EtiquetaID = dto.Code,
                                Produto = new ProdutoModel
                                {
                                    Codigo = produtoDto.ItemCode,
                                    Descricao = produtoDto.ItemName,
                                    CodigoBarras = produtoDto.BarCode,
                                    QuanidadeUnidadeVenda = produtoDto.SalesItemsPerUnit,
                                    UnidadeVenda = produtoDto.SalesPackagingUnit,
                                    QuantidadeUnidadeCompra = produtoDto.PurchaseItemsPerUnit,
                                    QuantidadePorItem = produtoDto.CountingItemsPerUnit,
                                    UnidadeEstoque = produtoDto.InventoryUOM,
                                    QuantidadePallet = produtoDto.U_LGO_QuantPallet,
                                },
                                Lote = dto.U_Lote,
                                QuantidadePallet = dto.U_QtdePallet,
                                QRCode = dto.U_QRCode,
                                TipoObjeto = dto.U_Objeto switch
                                {
                                    "202" => "OP",
                                    "LGDOS" => "OS",
                                    _ => dto.U_Objeto
                                },
                                NumeroDocumento = int.Parse(dto.U_DocEntry),
                                SequencialEtiqueta = int.Parse(dto.U_Sequencial)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public EtiquetaDTO? GetEtiquetaByID(int etiquetaID)
        {
            var response = _sap.SendRequestAsync(
                HttpMethod.Get,
                $"U_LGOIMPRESSCONF({etiquetaID})",
                queryParams: new Dictionary<string, string>
                {
                    { "$select", "Code,U_CodigoProduto,U_Lote,U_QtdePallet,U_QRCode,U_Objeto,U_DocEntry,U_Sequencial" },
                },
                handlePagination: false
            ).Result;
            if (response is OkObjectResult okResult)
            {
                var jsonString = JsonSerializer.Serialize(okResult.Value);
                var dto = JsonSerializer.Deserialize<EtiquetaSapDTO>(jsonString, _jsonOptions);

                if (dto is EtiquetaSapDTO)
                {
                    var produto = _sap.SendRequestAsync(
                        HttpMethod.Get,
                        $"Items('{dto.U_CodigoProduto}')",
                        queryParams: new Dictionary<string, string>
                        {
                            { "$select", "ItemCode,ItemName,BarCode,SalesItemsPerUnit,PurchaseItemsPerUnit,CountingItemsPerUnit,InventoryUOM,SalesPackagingUnit,U_LGO_QuantPallet" },
                        },
                        handlePagination: false
                    ).Result;

                    if (produto is OkObjectResult okProduto)
                    {
                        var produtoJson = JsonSerializer.Serialize(okProduto.Value);
                        var produtoDto = JsonSerializer.Deserialize<ProdutoSapDTO>(produtoJson, _jsonOptions);
                        if (produtoDto is ProdutoSapDTO)
                        {
                            return new EtiquetaDTO
                            {
                                EtiquetaID = dto.Code,
                                Produto = new ProdutoModel
                                {
                                    Codigo = produtoDto.ItemCode,
                                    Descricao = produtoDto.ItemName,
                                    CodigoBarras = produtoDto.BarCode,
                                    QuanidadeUnidadeVenda = produtoDto.SalesItemsPerUnit,
                                    UnidadeVenda = produtoDto.SalesPackagingUnit,
                                    QuantidadeUnidadeCompra = produtoDto.PurchaseItemsPerUnit,
                                    QuantidadePorItem = produtoDto.CountingItemsPerUnit,
                                    UnidadeEstoque = produtoDto.InventoryUOM,
                                    QuantidadePallet = produtoDto.U_LGO_QuantPallet
                                },
                                Lote = dto.U_Lote,
                                QuantidadePallet = dto.U_QtdePallet,
                                QRCode = dto.U_QRCode,
                                TipoObjeto = dto.U_Objeto switch
                                {
                                    "202" => "OP",
                                    "LGDOS" => "OS",
                                    _ => dto.U_Objeto
                                },
                                NumeroDocumento = int.Parse(dto.U_DocEntry),
                                SequencialEtiqueta = int.Parse(dto.U_Sequencial)
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
