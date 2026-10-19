
namespace AcademiaSistema.Models
{
    public class Matricula
    {
        public int IdMatricula { get; set; }
        public int IdAluno { get; set; }
        public int IdPlano { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Status { get; set; }

        public Matricula(
            int idMatricula,
            int idAluno,
            int idPlano,
            DateTime dataInicio,
            DateTime dataFim,
            string status)
        {
            IdMatricula = idMatricula;
            IdAluno = idAluno;
            IdPlano = idPlano;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Status = status;
        }
    }
}
