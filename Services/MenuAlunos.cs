
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AcademiaSistema.Models;
using AcademiaSistema.Services;

namespace AcademiaSistema.Services
{
    public class MenuAlunos
    {
        public static void Executar(ArquivoService arquivo)
        {
            List<Aluno> alunos = arquivo.CarregarAlunos();

            int opcao = 0;

            while (opcao != 3)
            {
                Console.Clear();

                Console.WriteLine("=== ACADEMIA INFNET ===");
                Console.WriteLine("=== GESTÃO DE ALUNOS ===");
                Console.WriteLine("1 - Cadastrar aluno");
                Console.WriteLine("2 - Listar alunos");
                Console.WriteLine("3 - Voltar ao menu principal");
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
                        CadastrarAluno(alunos, arquivo);
                        break;

                    case 2:
                        ListarAlunos(alunos);
                        break;

                    case 3:
                        Console.WriteLine("Voltando ao menu...");
                        break;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }

                if (opcao != 3)
                {
                    Console.WriteLine("\nPressione ENTER para continuar...");
                    Console.ReadLine();
                }
            }
        }

        static void CadastrarAluno(
            List<Aluno> alunos,
            ArquivoService arquivo)
        {
            Console.Clear();
            Console.WriteLine("=== CADASTRAR ALUNO ===");

            Console.Write("Nome completo: ");
            string nome = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(nome) ||
                nome.Length > 100 ||
                nome.Contains('|'))
            {
                Console.WriteLine("Nome inválido.");
                return;
            }

            Console.Write("CPF (somente números): ");
            string cpf = Console.ReadLine() ?? "";

            if (cpf.Length != 11 ||
                !cpf.All(char.IsDigit) ||
                alunos.Exists(a => a.Cpf == cpf))
            {
                Console.WriteLine("CPF inválido ou já cadastrado.");
                return;
            }

            Console.Write("Data de nascimento (dd/MM/aaaa): ");
            string entradaData = Console.ReadLine() ?? "";

            bool dataValida = DateTime.TryParseExact(
                entradaData,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime dataNascimento
            );

            if (!dataValida ||
                dataNascimento > DateTime.Today.AddYears(-14))
            {
                Console.WriteLine(
                    "Data inválida. O aluno deve ter pelo menos 14 anos."
                );
                return;
            }

            Console.Write("E-mail (opcional): ");
            string email = (Console.ReadLine() ?? "").Trim();

            if (email.Length > 100 ||
                email.Contains('|') ||
                (email != "" &&
                 (!email.Contains('@') ||
                  !email.Contains('.') ||
                  email.Contains(' '))))
            {
                Console.WriteLine("E-mail inválido.");
                return;
            }

            Console.Write("Telefone com DDD (11 dígitos): ");
            string telefone = Console.ReadLine() ?? "";

            if (telefone.Length != 11 ||
                !telefone.All(char.IsDigit))
            {
                Console.WriteLine("Telefone inválido.");
                return;
            }

            int novoId;

            if (alunos.Count == 0)
            {
                novoId = 1;
            }
            else
            {
                novoId = alunos.Max(p => p.IdAluno) + 1;
            }


            Aluno aluno = new Aluno(
                novoId,
                nome,
                cpf,
                dataNascimento,
                email,
                telefone
            );

            alunos.Add(aluno);
            arquivo.SalvarAlunos(alunos);

            Console.WriteLine(
                $"\nAluno {nome} cadastrado com sucesso! ID: {novoId}"
            );
        }

        static void ListarAlunos(List<Aluno> alunos)
        {
            Console.Clear();
            Console.WriteLine("=== ALUNOS CADASTRADOS ===");

            if (alunos.Count == 0)
            {
                Console.WriteLine("Nenhum aluno cadastrado.");
                return;
            }

            foreach (Aluno aluno in alunos)
            {
                Console.WriteLine("------------------------------");
                Console.WriteLine($"ID: {aluno.IdAluno}");
                Console.WriteLine($"Nome: {aluno.Nome}");
                Console.WriteLine($"CPF: {aluno.Cpf}");
                Console.WriteLine(
                    $"Nascimento: {aluno.DataNascimento:dd/MM/yyyy}"
                );
                Console.WriteLine(
                    $"E-mail: {aluno.Email}"
                );
                Console.WriteLine($"Telefone: {aluno.Telefone}");
            }
        }
    }
}
