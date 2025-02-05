namespace GerencianetPix.Models
{
    public class PixSent
    {
        public string Valor { get; set; }
        
        public Pagador Pagador { get; set; }
        
        public Favorecido Favorecido { get; set; }
    }

    public class Pagador
    {
        public string Chave { get; set; }
        public string InfoPagador { get; set; }
    }

    public class Favorecido
    {
        public string Chave { get; set; }
    }
}