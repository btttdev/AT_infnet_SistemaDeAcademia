
using AcademiaSistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace AcademiaSistema.Services
{
    public static class MenuProfessores
    {
        public static void Executar(ArquivoService arquivo)
        {
            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine("=== ACADEMIA INFNET ===");
                Console.WriteLine("\n=== GERENCIAR PROFESSORES ===");
                Console.WriteLine("1 - Cadastrar professor");
                Console.WriteLine("2 - Listar professores");
                Console.WriteLine("3 - Voltar ao menu principal");
                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    opcao = 0;
                }

                switch (opcao)
                {
                    case 1:
                        CadastrarProfessor(arquivo);
                        break;

                    case 2:
                        ListarProfessores(arquivo);
                        break;

                    case 3:
                        Console.WriteLine("Voltando ao menu principal...");
                        break;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }

            } while (opcao != 3);
        }

        private static void CadastrarProfessor(ArquivoService arquivo)
        {
            List<Professor> professores =
                arquivo.CarregarProfessores();

            Console.Clear();
            Console.WriteLine("\n=== ACADEMIA INFNET ===");
            Console.WriteLine("\n=== CADASTRO DE PROFESSOR ===");

            Console.Write("Nome: ");
            string nome = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(nome) || nome.Length > 100)
            {
                Console.WriteLine(
                    "Nome obrigatório. Limite de 100 caracteres.");
                return;
            }

            Console.Write("CPF (somente números): ");
            string cpf = (Console.ReadLine() ?? "").Trim();

            if (cpf.Length != 11 ||
                !cpf.All(char.IsDigit) ||
                professores.Exists(a => a.Cpf == cpf))
            {
                Console.WriteLine("CPF inválido ou já cadastrado.");
                return;
            }

            Console.Write("CREF: ");
            string cref = (Console.ReadLine() ?? "").Trim();

            if (cref.Length != 11 ||
                !cref.All(char.IsDigit) ||
                professores.Exists(a => a.Cref == cref))
            {
                Console.WriteLine("Cref inválido ou já cadastrado.");
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

            Console.Write("Telefone (DDD + número, 11 dígitos): ");
            string telefone = (Console.ReadLine() ?? "").Trim();

            if (telefone.Length != 11 ||
                !telefone.All(char.IsDigit))
            {
                Console.WriteLine(
                    "Telefone inválido. Digite os 11 números.");
                return;
            }

            int novoId;
            if (professores.Count == 0)
            {
                novoId = 1;
            }
            else
            {
                novoId = professores.Max(p => p.IdProfessor) + 1;
            }

            Professor professor = new Professor(
                novoId,
                nome,
                cpf,
                cref,
                email,
                telefone
            );

            professores.Add(professor);
            arquivo.SalvarProfessores(professores);

            Console.WriteLine(
                $"\nProfessor cadastrado com sucesso! ID: {novoId}");
        }

        private static void ListarProfessores(ArquivoService arquivo)
        {
            List<Professor> professores =
                arquivo.CarregarProfessores();

            Console.Clear();
            Console.WriteLine("\n=== Academia INFNET ===");
            Console.WriteLine("\n=== LISTA DE PROFESSORES ===");

            if (professores.Count == 0)
            {
                Console.WriteLine(
                    "Nenhum professor cadastrado.");
                return;
            }

            foreach (Professor professor in professores)
            {
                Console.WriteLine("------------------------------");
                Console.WriteLine($"ID: {professor.IdProfessor}");
                Console.WriteLine($"Nome: {professor.Nome}");
                Console.WriteLine($"CPF: {professor.Cpf}");
                Console.WriteLine($"CREF: {professor.Cref}");
                Console.WriteLine(
                    $"E-mail: {professor.Email}");
                Console.WriteLine(
                    $"Telefone: {professor.Telefone}");
            }

            Console.WriteLine("------------------------------");
        }
    }
}
