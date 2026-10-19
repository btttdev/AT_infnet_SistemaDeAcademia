
namespace AcademiaSistema.Models
{
    public class Professor
    {
        public int IdProfessor { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Cref { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        public Professor(
            int idProfessor,
            string nome,
            string cpf,
            string cref,
            string email,
            string telefone)
        {
            IdProfessor = idProfessor;
            Nome = nome;
            Cpf = cpf;
            Cref = cref;
            Email = email;
            Telefone = telefone;
        }
    }
}
