public class PixCharge
{
    public Calendario Calendario { get; set; }
    public Devedor Devedor { get; set; }
    public Valor Valor { get; set; }
    public string Chave { get; set; }
    public string SolicitacaoPagador { get; set; }
}

public class Calendario
{
    public int Expiracao { get; set; }
}

public class Devedor
{
    public string Cpf { get; set; }
    public string Nome { get; set; }
}

public class Valor
{
    public string Original { get; set; }
}