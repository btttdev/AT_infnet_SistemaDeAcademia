
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AcademiaSistema.Models;

namespace AcademiaSistema.Services
{
    public class MenuPlanos
    {
        public static void Executar(ArquivoService arquivo)
        {
            List<Plano> planos = arquivo.CarregarPlanos();

            int opcao = 0;

            while (opcao != 4)
            {
                Console.Clear();

                Console.WriteLine("=== ACADEMIA INFNET ===");
                Console.WriteLine("=== GESTÃO DE PLANOS - ACADEMIA INFNET ===");
                Console.WriteLine("1 - Cadastrar plano");
                Console.WriteLine("2 - Listar planos");
                Console.WriteLine("3 - Ativar/Desativar plano");
                Console.WriteLine("4 - Voltar ao menu principal");
                Console.Write("Escolha: ");

                if (!int.TryParse(
                    Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Digite um número válido.");
                    Console.ReadLine();
                    opcao = 0;
                    continue;
                }

                switch (opcao)
                {
                    case 1:
                        CadastrarPlano(planos, arquivo);
                        break;

                    case 2:
                        ListarPlanos(planos);
                        break;

                    case 3:
                        AlterarStatus(planos, arquivo);
                        break;

                    case 4:
                        break;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }

                if (opcao != 4)
                {
                    Console.WriteLine(
                        "\nPressione ENTER para continuar...");
                    Console.ReadLine();
                }
            }
        }

        private static void CadastrarPlano(
            List<Plano> planos,
            ArquivoService arquivo)
        {
            Console.Clear();
            Console.WriteLine("\n=== ACADEMIA INFNET ===");
            Console.WriteLine("=== CADASTRAR PLANO ===");

            Console.Write("Nome do plano: ");
            string nome = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(nome) ||
                nome.Length > 50 ||
                nome.Contains('|'))
            {
                Console.WriteLine("Nome inválido.");
                return;
            }

            bool nomeExiste = planos.Exists(p =>
                p.Nome.Equals(
                    nome,
                    StringComparison.OrdinalIgnoreCase));

            if (nomeExiste)
            {
                Console.WriteLine(
                    "Já existe um plano com esse nome.");
                return;
            }

            Console.Write("Valor mensal (ex.: 119,90): ");

            bool valorValido = decimal.TryParse(
                Console.ReadLine(),
                NumberStyles.Number,
                CultureInfo.GetCultureInfo("pt-BR"),
                out decimal valor);

            if (!valorValido || valor <= 0)
            {
                Console.WriteLine(
                    "O valor deve ser maior que zero.");
                return;
            }

            Console.Write("Duração (1, 3, 6 ou 12 meses): ");

            bool duracaoValida = int.TryParse(
                Console.ReadLine(), out int duracao);

            if (!duracaoValida ||
                !new int[] { 1, 3, 6, 12 }.Contains(duracao))
            {
                Console.WriteLine("Duração inválida.");
                return;
            }

            int novoId;

            if (planos.Count == 0)
            {
                novoId = 1;
            }
            else
            {
                novoId = planos.Max(p => p.IdPlano) + 1;
            }

            Plano plano = new Plano(
                novoId,
                nome,
                valor,
                duracao,
                true
            );

            planos.Add(plano);
            arquivo.SalvarPlanos(planos);

            Console.WriteLine(
                $"\nPlano cadastrado! ID: {novoId}");
        }

        private static void ListarPlanos(List<Plano> planos)
        {
            Console.Clear();
            Console.WriteLine("\n=== ACADEMIA INFNET ===");
            Console.WriteLine("=== PLANOS CADASTRADOS ===");

            if (planos.Count == 0)
            {
                Console.WriteLine("Nenhum plano cadastrado.");
                return;
            }

            foreach (Plano plano in planos)
            {
                Console.Clear();

                Console.WriteLine("\n=== ACADEMIA INFNET ===");
                Console.WriteLine("-------------------------");
                Console.WriteLine($"ID: {plano.IdPlano}");
                Console.WriteLine($"Nome: {plano.Nome}");
                Console.WriteLine(
                    $"Valor mensal: {plano.ValorMensal:C}");
                Console.WriteLine(
                    $"Duração: {plano.DuracaoMeses} mês(es)");
                Console.WriteLine(
                    $"Status: {(plano.Ativo ? "Ativo" : "Inativo")}");
            }
        }

        private static void AlterarStatus(
            List<Plano> planos,
            ArquivoService arquivo)
        {
            ListarPlanos(planos);

            if (planos.Count == 0)
            {
                return;
            }

            Console.Write("\nDigite o ID do plano: ");

            if (!int.TryParse(
                Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var plano = planos.Find(p => p.IdPlano == id);

            if (plano == null)
            {
                Console.WriteLine("Plano não encontrado.");
                return;
            }

            plano.Ativo = !plano.Ativo;

            arquivo.SalvarPlanos(planos);

            Console.WriteLine(
                $"Plano agora está " +
                $"{(plano.Ativo ? "ativo" : "inativo")}.");
        }
    }
}
