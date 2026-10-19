
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AcademiaSistema.Models;

namespace AcademiaSistema.Services
{
    public class MenuMatriculas
    {
        public static void Executar(ArquivoService arquivo)
        {
            List<Aluno> alunos = arquivo.CarregarAlunos();
            List<Plano> planos = arquivo.CarregarPlanos();
            List<Matricula> matriculas = arquivo.CarregarMatriculas();

            int opcao = 0;

            while (opcao != 4)
            {
                Console.Clear();

                Console.WriteLine("=== ACADEMIA INFNET ===");
                Console.WriteLine("=== GESTÃO DE MATRÍCULAS ===");
                Console.WriteLine("1 - Nova matrícula");
                Console.WriteLine("2 - Listar matrículas");
                Console.WriteLine("3 - Alterar status");
                Console.WriteLine("4 - Voltar ao menu principal");
                Console.Write("Escolha: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Digite um número válido.");
                    Console.ReadLine();
                    opcao = 0;
                    continue;
                }

                switch (opcao)
                {
                    case 1:
                        CadastrarMatricula(
                            alunos, planos, matriculas, arquivo);
                        break;

                    case 2:
                        ListarMatriculas(alunos, planos, matriculas);
                        break;

                    case 3:
                        AlterarStatus(matriculas, arquivo);
                        break;

                    case 4:
                        break;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }

                if (opcao != 4)
                {
                    Console.WriteLine("\nPressione ENTER para continuar...");
                    Console.ReadLine();
                }
            }
        }

        private static void CadastrarMatricula(
            List<Aluno> alunos,
            List<Plano> planos,
            List<Matricula> matriculas,
            ArquivoService arquivo)
        {
            Console.Clear();
            Console.WriteLine("\n=== ACADEMIA INFNET ===");
            Console.WriteLine("=== NOVA MATRÍCULA ===");

            if (alunos.Count == 0)
            {
                Console.WriteLine("Cadastre um aluno primeiro.");
                return;
            }

            ListarAlunosDisponiveis(alunos);

            Console.Write("\nDigite o ID do aluno: ");

            if (!int.TryParse(Console.ReadLine(), out int idAluno))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var aluno = alunos.Find(a => a.IdAluno == idAluno);

            if (aluno == null)
            {
                Console.WriteLine("Aluno não encontrado.");
                return;
            }

            List<Plano> planosAtivos = planos
                .Where(p => p.Ativo)
                .ToList();

            if (planosAtivos.Count == 0)
            {
                Console.WriteLine("Não existem planos ativos.");
                return;
            }

            Console.WriteLine("\n=== ACADEMIA INFNET ===");
            Console.WriteLine("\n=== PLANOS DISPONÍVEIS ===");

            foreach (Plano plano in planosAtivos)
            {
                Console.WriteLine(
                    $"ID: {plano.IdPlano} | {plano.Nome} | " +
                    $"{plano.ValorMensal:C} por mês | " +
                    $"{plano.DuracaoMeses} mês(es)");
            }

            Console.Write("\nDigite o ID do plano: ");

            if (!int.TryParse(Console.ReadLine(), out int idPlano))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var planoEscolhido = planosAtivos
                .Find(p => p.IdPlano == idPlano);

            if (planoEscolhido == null)
            {
                Console.WriteLine("Plano não encontrado ou inativo.");
                return;
            }

            Console.Write("Data de início (dd/MM/aaaa): ");

            bool dataValida = DateTime.TryParseExact(
                Console.ReadLine(),
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime dataInicio);

            if (!dataValida || dataInicio.Date < DateTime.Today)
            {
                Console.WriteLine(
                    "A data de início não pode ser anterior a hoje.");
                return;
            }

            DateTime dataFim = dataInicio.AddMonths(
                planoEscolhido.DuracaoMeses);

            int novoId;

            if (matriculas.Count == 0)
            {
                novoId = 1;
            }
            else
            {
                novoId = matriculas.Max(p => p.IdMatricula) + 1;
            }


            Matricula matricula = new Matricula(
                novoId,
                aluno.IdAluno,
                planoEscolhido.IdPlano,
                dataInicio,
                dataFim,
                "ativo");

            matriculas.Add(matricula);
            arquivo.SalvarMatriculas(matriculas);

            Console.WriteLine("\nMatrícula cadastrada!");
            Console.WriteLine($"Número: {novoId}");
            Console.WriteLine($"Aluno: {aluno.Nome}");
            Console.WriteLine($"Plano: {planoEscolhido.Nome}");
            Console.WriteLine($"Início: {dataInicio:dd/MM/yyyy}");
            Console.WriteLine($"Término: {dataFim:dd/MM/yyyy}");
        }

        private static void ListarAlunosDisponiveis(
            List<Aluno> alunos)
        {
            Console.WriteLine("=== ALUNOS CADASTRADOS ===");

            foreach (Aluno aluno in alunos)
            {
                Console.WriteLine(
                    $"ID: {aluno.IdAluno} | {aluno.Nome}");
            }
        }

        private static void ListarMatriculas(
            List<Aluno> alunos,
            List<Plano> planos,
            List<Matricula> matriculas)
        {
            Console.Clear();
            Console.WriteLine("=== MATRÍCULAS CADASTRADAS ===");

            if (matriculas.Count == 0)
            {
                Console.WriteLine("Nenhuma matrícula cadastrada.");
                return;
            }

            foreach (Matricula matricula in matriculas)
            {
                var aluno = alunos.Find(
                    a => a.IdAluno == matricula.IdAluno);

                var plano = planos.Find(
                    p => p.IdPlano == matricula.IdPlano);

                Console.WriteLine("------------------------------");
                Console.WriteLine($"Matrícula: {matricula.IdMatricula}");
                Console.WriteLine(
                    $"Aluno: {aluno?.Nome ?? "Não encontrado"}");
                Console.WriteLine(
                    $"Plano: {plano?.Nome ?? "Não encontrado"}");
                Console.WriteLine(
                    $"Início: {matricula.DataInicio:dd/MM/yyyy}");
                Console.WriteLine(
                    $"Término: {matricula.DataFim:dd/MM/yyyy}");
                Console.WriteLine($"Status: {matricula.Status}");
            }
        }

        private static void AlterarStatus(
            List<Matricula> matriculas,
            ArquivoService arquivo)
        {
            Console.Clear();
            Console.WriteLine("=== ALTERAR STATUS ===");

            if (matriculas.Count == 0)
            {
                Console.WriteLine("Nenhuma matrícula cadastrada.");
                return;
            }

            foreach (Matricula matricula in matriculas)
            {
                Console.WriteLine(
                    $"ID: {matricula.IdMatricula} | " +
                    $"Status: {matricula.Status}");
            }

            Console.Write("\nID da matrícula: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var encontrada = matriculas.Find(
                m => m.IdMatricula == id);

            if (encontrada == null)
            {
                Console.WriteLine("Matrícula não encontrada.");
                return;
            }

            Console.WriteLine("\n1 - Ativo");
            Console.WriteLine("2 - Suspenso");
            Console.WriteLine("3 - Cancelado");
            Console.WriteLine("4 - Encerrado");
            Console.Write("Novo status: ");

            if (!int.TryParse(Console.ReadLine(), out int status))
            {
                Console.WriteLine("Opção inválida.");
                return;
            }

            switch (status)
            {
                case 1:
                    encontrada.Status = "ativo";
                    break;
                case 2:
                    encontrada.Status = "suspenso";
                    break;
                case 3:
                    encontrada.Status = "cancelado";
                    break;
                case 4:
                    encontrada.Status = "encerrado";
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    return;
            }

            arquivo.SalvarMatriculas(matriculas);

            Console.WriteLine("Status atualizado com sucesso.");
        }
    }
}
