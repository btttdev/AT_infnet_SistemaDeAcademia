
using AcademiaSistema.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AcademiaSistema.Services
{
    public static class MenuAulas
    {
        private static readonly TimeOnly Abertura =
            new TimeOnly(6, 0);

        private static readonly TimeOnly Fechamento =
            new TimeOnly(22, 0);

        private static readonly string[] DiasSemana =
        {
            "Segunda-feira",
            "Terca-feira",
            "Quarta-feira",
            "Quinta-feira",
            "Sexta-feira",
            "Sabado",
            "Domingo"
        };

        public static void Executar(ArquivoService arquivo)
        {
            int opcao;

            do
            {
                Console.WriteLine("\n=== ACADEMIA INFNET ===");
                Console.WriteLine("\n=== GERENCIAR AULAS ===");
                Console.WriteLine("1 - Cadastrar aula");
                Console.WriteLine("2 - Listar aulas");
                Console.WriteLine("3 - Voltar ao menu principal");
                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    opcao = 0;
                }

                switch (opcao)
                {
                    case 1:
                        CadastrarAula(arquivo);
                        break;

                    case 2:
                        ListarAulas(arquivo);
                        break;

                    case 3:
                        Console.WriteLine(
                            "Voltando ao menu principal...");
                        break;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }

            } while (opcao != 3);
        }

        private static void CadastrarAula(ArquivoService arquivo)
        {
            List<Professor> professores =
                arquivo.CarregarProfessores();

            if (professores.Count == 0)
            {
                Console.WriteLine(
                    "\nNão há professores cadastrados.");
                Console.WriteLine(
                    "Cadastre um professor antes de criar uma aula.");
                return;
            }

            List<Aula> aulas = arquivo.CarregarAulas();

            Console.WriteLine("\n=== CADASTRO DE AULA ===");

            Console.Write("Nome da aula: ");
            string nome = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(nome) ||
                nome.Length > 50)
            {
                Console.WriteLine(
                    "Nome obrigatório, com até 50 caracteres.");
                return;
            }

            Console.Write("Descrição (opcional): ");
            string descricao = (Console.ReadLine() ?? "").Trim();

            if (descricao.Length > 250)
            {
                Console.WriteLine(
                    "A descrição deve ter até 250 caracteres.");
                return;
            }

            Console.WriteLine("\nEscolha o dia da semana:");

            for (int i = 0; i < DiasSemana.Length; i++)
            {
                Console.WriteLine(
                    $"{i + 1} - {DiasSemana[i]}");
            }

            Console.Write("Opção: ");

            if (!int.TryParse(Console.ReadLine(), out int dia) ||
                dia < 1 || dia > DiasSemana.Length)
            {
                Console.WriteLine("Dia da semana inválido.");
                return;
            }

            string diaSemana = DiasSemana[dia - 1];

            Console.Write("Horário de início (HH:mm): ");
            string entradaInicio =
                (Console.ReadLine() ?? "").Trim();

            if (!TimeOnly.TryParseExact(
                entradaInicio,
                "HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out TimeOnly horarioInicio))
            {
                Console.WriteLine(
                    "Horário de início inválido. Use HH:mm.");
                return;
            }

            Console.Write("Horário de término (HH:mm): ");
            string entradaFim =
                (Console.ReadLine() ?? "").Trim();

            if (!TimeOnly.TryParseExact(
                entradaFim,
                "HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out TimeOnly horarioFim))
            {
                Console.WriteLine(
                    "Horário de término inválido. Use HH:mm.");
                return;
            }

            if (horarioFim <= horarioInicio)
            {
                Console.WriteLine(
                    "O término deve ser posterior ao início.");
                return;
            }

            if (horarioInicio < Abertura ||
                horarioFim > Fechamento)
            {
                Console.WriteLine(
                    $"A aula deve ocorrer entre " +
                    $"{Abertura:HH:mm} e {Fechamento:HH:mm}.");
                return;
            }

            Console.WriteLine("\nProfessores cadastrados:");

            foreach (Professor professor in professores)
            {
                Console.WriteLine(
                    $"ID: {professor.IdProfessor} - " +
                    $"{professor.Nome} - CREF: {professor.Cref}");
            }

            Console.Write("Digite o ID do professor: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int idProfessor))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            Professor professorEscolhido =
                professores.FirstOrDefault(
                    p => p.IdProfessor == idProfessor)!;

            if (professorEscolhido == null)
            {
                Console.WriteLine(
                    "Professor não encontrado.");
                return;
            }

            int novoId;

            if (aulas.Count == 0)
            {
                novoId = 1;
            }
            else
            {
                novoId = aulas.Max(p => p.IdAula) + 1;
            }


            Aula novaAula = new Aula(
                novoId,
                nome,
                descricao,
                diaSemana,
                horarioInicio,
                horarioFim,
                professorEscolhido.IdProfessor
            );

            aulas.Add(novaAula);
            arquivo.SalvarAulas(aulas);

            Console.WriteLine(
                "\nAula cadastrada com sucesso!");
            Console.WriteLine($"ID da aula: {novoId}");
            Console.WriteLine(
                $"Professor responsável: {professorEscolhido.Nome}");
        }

        private static void ListarAulas(ArquivoService arquivo)
        {
            List<Aula> aulas = arquivo.CarregarAulas();
            List<Professor> professores =
                arquivo.CarregarProfessores();

            Console.WriteLine("\n=== Academia INFNET ===");
            Console.WriteLine("\n=== LISTA DE AULAS ===");

            if (aulas.Count == 0)
            {
                Console.WriteLine("Nenhuma aula cadastrada.");
                return;
            }

            foreach (Aula aula in aulas)
            {
                Professor professor = professores.FirstOrDefault(
                    p => p.IdProfessor == aula.IdProfessor)!;

                Console.WriteLine("------------------------------");
                Console.WriteLine($"ID: {aula.IdAula}");
                Console.WriteLine($"Nome: {aula.Nome}");
                Console.WriteLine(
                    $"Descrição: {aula.Descricao}");
                Console.WriteLine($"Dia: {aula.DiaSemana}");
                Console.WriteLine(
                    $"Início: {aula.HorarioInicio:HH:mm}");
                Console.WriteLine(
                    $"Término: {aula.HorarioFim:HH:mm}");
                Console.WriteLine(
                    $"Professor: {professor?.Nome ?? "Não encontrado"}");
            }

            Console.WriteLine("------------------------------");
        }
    }
}
