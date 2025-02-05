using System;
using System.ComponentModel.DataAnnotations.Schema;

[Table("PixPayments")]
public class PixPayment
{
    public long Id { get; set; }  // Chave primária
    public string Txid { get; set; }  // Identificador único da transação
    public int UserId { get; set; }  // ID do usuário que fez o pagamento
}
