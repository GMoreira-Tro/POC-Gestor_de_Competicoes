namespace WebApi.Models
{
    public class Pessoa 
    {
        public enum Sexo
        {
            MASCULINO,
            FEMININO
        };
        public int ID;
        public string nome;
        public string sobrenome;
    }
}