
namespace AcademiaSistema.Models
{
    public class Aula
    {
        public int IdAula { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string DiaSemana { get; set; }
        public TimeOnly HorarioInicio { get; set; }
        public TimeOnly HorarioFim { get; set; }
        public int IdProfessor { get; set; }

        public Aula(
            int idAula,
            string nome,
            string descricao,
            string diaSemana,
            TimeOnly horarioInicio,
            TimeOnly horarioFim,
            int idProfessor)
        {
            IdAula = idAula;
            Nome = nome;
            Descricao = descricao;
            DiaSemana = diaSemana;
            HorarioInicio = horarioInicio;
            HorarioFim = horarioFim;
            IdProfessor = idProfessor;
        }
    }
}
