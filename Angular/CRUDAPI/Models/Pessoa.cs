namespace CRUDAPI.Models {
    public class Pessoa {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public enum Sexo {
            MASCULINO,
            FEMININO
        }

        public Sexo? sexo { get; set;}
    }
}