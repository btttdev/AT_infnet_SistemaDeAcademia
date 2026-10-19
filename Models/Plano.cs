
namespace AcademiaSistema.Models
{
    public class Plano
    {
        public int IdPlano { get; set; }
        public string Nome { get; set; }
        public decimal ValorMensal { get; set; }
        public int DuracaoMeses { get; set; }
        public bool Ativo { get; set; }

        public Plano(
            int idPlano,
            string nome,
            decimal valorMensal,
            int duracaoMeses,
            bool ativo)
        {
            IdPlano = idPlano;
            Nome = nome;
            ValorMensal = valorMensal;
            DuracaoMeses = duracaoMeses;
            Ativo = ativo;
        }
    }
}
