using System;
using System.Collections.Generic;
using System.Text;


namespace AcademiaSistema.Models
{
    public class Aluno
    {
        public int IdAluno { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        public Aluno(
            int idAluno,
            string nome,
            string cpf,
            DateTime dataNascimento,
            string email,
            string telefone)
        {
            IdAluno = idAluno;
            Nome = nome;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            Email = email;
            Telefone = telefone;
        }
    }
}
