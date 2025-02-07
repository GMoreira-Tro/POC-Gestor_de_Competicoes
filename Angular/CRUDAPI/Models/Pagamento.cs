using System;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Pagamentos")]
public class Pagamento
{
    public long Id { get; set; }  // Chave primária
    /// <summary>
    /// Identificador unico da transação.
    /// </summary>
    public string Txid { get; set; }
    /// <summary>
    /// Id do usuário que realizou o pagamento.
    /// </summary>
    [ForeignKey("Usuarios")]
    public long PagadorId { get; set; }

    /// <summary>
    /// Id do usuário que recebeu o pagamento.
    /// </summary>
    [ForeignKey("Usuarios")]
    public long FavorecidoId { get; set; }
    /// <summary>
    /// Informações sobre o pagamento.
    /// </summary>
    public string InfoPagamento { get; set; }
}
