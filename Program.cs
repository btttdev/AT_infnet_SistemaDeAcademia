
using AcademiaSistema.Services;
using System;

class Program
{
    static void Main()
    {
        ArquivoService arquivo = new ArquivoService();
        int opcao = 0;

        while (opcao != 6)
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("  SISTEMA DE GESTÃO - ACADEMIA INFNET");
            Console.WriteLine("================================");
            Console.WriteLine("1 - Gerenciar alunos");
            Console.WriteLine("2 - Gerenciar planos");
            Console.WriteLine("3 - Gerenciar matrículas");
            Console.WriteLine("4 - Gerenciar aulas");
            Console.WriteLine("5 - Gerenciar professores");
            Console.WriteLine("6 - Sair");
            Console.WriteLine("================================");
            Console.Write("Escolha uma opção: ");

            string entrada = Console.ReadLine() ?? "";

            if (int.TryParse(entrada, out opcao))
            {
                switch (opcao)
                {
                    case 1:
                        MenuAlunos.Executar(arquivo);
                        break;

                    case 2:
                        MenuPlanos.Executar(arquivo);
                        break;

                    case 3:
                        MenuMatriculas.Executar(arquivo);
                        break;

                    case 4:
                        MenuAulas.Executar(arquivo);
                        break;

                    case 5:
                        MenuProfessores.Executar(arquivo);
                        break;

                    case 6:
                        Console.WriteLine("Sistema encerrado.");
                        break;

                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Digite um número válido.");
                opcao = 0;
            }

            if (opcao != 6)
            {
                Console.WriteLine();
                Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
            }
        }
    }
}
