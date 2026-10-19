using System.Globalization;
using AcademiaSistema.Models;

namespace AcademiaSistema.Services
{
    public class ArquivoService
    {
        private readonly string caminhoArquivo;
        private readonly string caminhoPlanos;
        private readonly string caminhoMatriculas;
        private readonly string caminhoProfessores;
        private readonly string caminhoAulas;



        public ArquivoService()
        {
            string pasta = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Data"
            );

            Directory.CreateDirectory(pasta);

            caminhoArquivo = Path.Combine(
                pasta,
                "alunos.txt"
            );

            caminhoPlanos = Path.Combine(
                pasta,
                "planos.txt"
            );

            caminhoMatriculas = Path.Combine(
                pasta,
                "matriculas.txt"
            );

            caminhoProfessores = Path.Combine(
                pasta,
                "professores.txt"
            );

            caminhoAulas = Path.Combine(
                pasta,
                "aulas.txt"
            );


        }

        public List<Aluno> CarregarAlunos()
        {
            List<Aluno> alunos = new List<Aluno>();

            if (!File.Exists(caminhoArquivo))
            {
                return alunos;
            }

            string[] linhas = File.ReadAllLines(caminhoArquivo);

            foreach (string linha in linhas)
            {
                string[] dados = linha.Split('|');

                if (dados.Length != 6)
                {
                    continue;
                }

                bool idValido = int.TryParse(dados[0], out int id);

                bool dataValida = DateTime.TryParseExact(
                    dados[3],
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime dataNascimento
                );

                if (!idValido || !dataValida)
                {
                    continue;
                }

                Aluno aluno = new Aluno(
                    id,
                    dados[1],
                    dados[2],
                    dataNascimento,
                    dados[4],
                    dados[5]
                );

                alunos.Add(aluno);
            }

            return alunos;
        }

        public void SalvarAlunos(List<Aluno> alunos)
        {
            List<string> linhas = new List<string>();

            foreach (Aluno aluno in alunos)
            {
                string linha =
                    $"{aluno.IdAluno}|" +
                    $"{aluno.Nome}|" +
                    $"{aluno.Cpf}|" +
                    $"{aluno.DataNascimento:yyyy-MM-dd}|" +
                    $"{aluno.Email}|" +
                    $"{aluno.Telefone}";

                linhas.Add(linha);
            }

            File.WriteAllLines(caminhoArquivo, linhas);
        }


        public List<Plano> CarregarPlanos()
        {
            List<Plano> planos = new List<Plano>();

            if (!File.Exists(caminhoPlanos))
            {
                return planos;
            }

            string[] linhas = File.ReadAllLines(caminhoPlanos);

            foreach (string linha in linhas)
            {
                string[] dados = linha.Split('|');

                if (dados.Length != 5)
                {
                    continue;
                }

                bool idValido = int.TryParse(
                    dados[0], out int id);

                bool valorValido = decimal.TryParse(
                    dados[2],
                    System.Globalization.NumberStyles.Number,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out decimal valor);

                bool duracaoValida = int.TryParse(
                    dados[3], out int duracao);

                bool ativoValido = bool.TryParse(
                    dados[4], out bool ativo);

                if (!idValido || !valorValido ||
                    !duracaoValida || !ativoValido)
                {
                    continue;
                }

                Plano plano = new Plano(
                    id,
                    dados[1],
                    valor,
                    duracao,
                    ativo
                );

                planos.Add(plano);
            }

            return planos;
        }

        public void SalvarPlanos(List<Plano> planos)
        {
            List<string> linhas = new List<string>();

            foreach (Plano plano in planos)
            {
                string linha =
                    $"{plano.IdPlano}|" +
                    $"{plano.Nome}|" +
                    $"{plano.ValorMensal.ToString(
                        System.Globalization.CultureInfo.InvariantCulture)}|" +
                    $"{plano.DuracaoMeses}|" +
                    $"{plano.Ativo}";

                linhas.Add(linha);
            }

            File.WriteAllLines(caminhoPlanos, linhas);
        }

        public List<Matricula> CarregarMatriculas()
        {
            List<Matricula> matriculas = new List<Matricula>();

            if (!File.Exists(caminhoMatriculas))
            {
                return matriculas;
            }

            string[] linhas = File.ReadAllLines(caminhoMatriculas);

            foreach (string linha in linhas)
            {
                string[] dados = linha.Split('|');

                if (dados.Length != 6)
                {
                    continue;
                }

                bool idValido = int.TryParse(dados[0], out int id);
                bool alunoValido = int.TryParse(dados[1], out int idAluno);
                bool planoValido = int.TryParse(dados[2], out int idPlano);

                bool inicioValido = DateTime.TryParseExact(
                    dados[3],
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out DateTime dataInicio
                );

                bool fimValido = DateTime.TryParseExact(
                    dados[4],
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out DateTime dataFim
                );

                string status = dados[5];

                bool statusValido =
                    status == "ativo" ||
                    status == "suspenso" ||
                    status == "cancelado" ||
                    status == "encerrado";

                if (!idValido || !alunoValido || !planoValido ||
                    !inicioValido || !fimValido || !statusValido)
                {
                    continue;
                }

                Matricula matricula = new Matricula(
                    id,
                    idAluno,
                    idPlano,
                    dataInicio,
                    dataFim,
                    status
                );

                matriculas.Add(matricula);
            }

            return matriculas;
        }

        public void SalvarMatriculas(List<Matricula> matriculas)
        {
            List<string> linhas = new List<string>();

            foreach (Matricula matricula in matriculas)
            {
                string linha =
                    $"{matricula.IdMatricula}|" +
                    $"{matricula.IdAluno}|" +
                    $"{matricula.IdPlano}|" +
                    $"{matricula.DataInicio:yyyy-MM-dd}|" +
                    $"{matricula.DataFim:yyyy-MM-dd}|" +
                    $"{matricula.Status}";

                linhas.Add(linha);
            }

            File.WriteAllLines(caminhoMatriculas, linhas);
        }


        public List<Professor> CarregarProfessores()
        {
            List<Professor> professores = new List<Professor>();

            if (!File.Exists(caminhoProfessores))
            {
                return professores;
            }

            string[] linhas = File.ReadAllLines(caminhoProfessores);

            foreach (string linha in linhas)
            {
                string[] dados = linha.Split('|');

                if (dados.Length == 6 &&
                    int.TryParse(dados[0], out int idProfessor))
                {
                    Professor professor = new Professor(
                        idProfessor,
                        dados[1],
                        dados[2],
                        dados[3],
                        dados[4],
                        dados[5]
                    );

                    professores.Add(professor);
                }
            }

            return professores;
        }

        public void SalvarProfessores(List<Professor> professores)
        {
            List<string> linhas = new List<string>();

            foreach (Professor professor in professores)
            {
                string linha =
                    $"{professor.IdProfessor}|" +
                    $"{professor.Nome}|" +
                    $"{professor.Cpf}|" +
                    $"{professor.Cref}|" +
                    $"{professor.Email}|" +
                    $"{professor.Telefone}";

                linhas.Add(linha);
            }

            File.WriteAllLines(caminhoProfessores, linhas);
        }

        public List<Aula> CarregarAulas()
        {
            List<Aula> aulas = new List<Aula>();

            if (!File.Exists(caminhoAulas))
            {
                return aulas;
            }

            string[] linhas = File.ReadAllLines(caminhoAulas);

            foreach (string linha in linhas)
            {
                string[] dados = linha.Split('|');

                if (dados.Length == 7 &&
                    int.TryParse(dados[0], out int idAula) &&
                    TimeOnly.TryParseExact(
                        dados[4],
                        "HH:mm",
                        out TimeOnly horarioInicio) &&
                    TimeOnly.TryParseExact(
                        dados[5],
                        "HH:mm",
                        out TimeOnly horarioFim) &&
                    int.TryParse(dados[6], out int idProfessor))
                {
                    Aula aula = new Aula(
                        idAula,
                        dados[1],
                        dados[2],
                        dados[3],
                        horarioInicio,
                        horarioFim,
                        idProfessor
                    );

                    aulas.Add(aula);
                }
            }

            return aulas;
        }

        public void SalvarAulas(List<Aula> aulas)
        {
            List<string> linhas = new List<string>();

            foreach (Aula aula in aulas)
            {
                string linha =
                    $"{aula.IdAula}|" +
                    $"{aula.Nome}|" +
                    $"{aula.Descricao}|" +
                    $"{aula.DiaSemana}|" +
                    $"{aula.HorarioInicio.ToString("HH:mm")}|" +
                    $"{aula.HorarioFim.ToString("HH:mm")}|" +
                    $"{aula.IdProfessor}";

                linhas.Add(linha);
            }

            File.WriteAllLines(caminhoAulas, linhas);
        }


    }
}
