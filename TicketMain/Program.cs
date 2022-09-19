using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Configuration;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicketMain
{
    internal class Program
    {
        enum menuPrincipal { menu_func = 1, menu_ticket = 2, relatorios = 3, sair = 4, }
        enum menuFunc { adicionar = 1, editar, voltar }
        enum menuTicket { adicionar = 1, editar, voltar }
        enum menuEditaTicket { editaQuantidade = 1, editaFuncionario, editaSituacao, voltar }
        enum menuRelatorio { totalFunc = 1, totalFuncPeriodo, totalGeralPeriodo, voltar }
        enum menuEditaFunc { nome = 1, cpf, situacao, voltar }
        enum menuEditaFuncSit { ativar = 1, inativar, voltar }

        static void Main(string[] args)
        {
            bool escolheuSair = false;

            while (!escolheuSair)
            {
                Cadastro cad = new Cadastro();
                Console.WriteLine("Menu:");
                Console.WriteLine("1 - Manutenção de Funcionários\n2 - Manutenção de Tickets\n3 - Relatórios\n4 - Sair");
                string menuEscolhido = Console.ReadLine();
                menuEscolhido = cad.trataEntrada(menuEscolhido);
                int index = int.Parse(menuEscolhido);
                menuPrincipal MenuSelecao = (menuPrincipal)index;

                switch (MenuSelecao)
                {
                    case menuPrincipal.menu_func:
                        ManutencaoFuncionarios();
                        break;

                    case menuPrincipal.menu_ticket:
                        ManutencaoTicket();
                        break;

                    case menuPrincipal.relatorios:
                        ManutencaoRelatorio();
                        break;

                    case menuPrincipal.sair:
                        Console.WriteLine("Até mais ;)");
                        Thread.Sleep(2000);
                        escolheuSair = true;
                        break;

                    default:
                        Console.WriteLine("Por favor, escolhe uma opção válida.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        break;
                }
            }

        }
        static void ManutencaoFuncionarios()
        {
            Console.WriteLine("Manutenção de Funcionários:");
            Console.WriteLine("1 - Adicionar novo\n2 - Editar\n3 - Voltar");
            string menuEscolhido = Console.ReadLine();
            Cadastro cad = new Cadastro();
            menuEscolhido = cad.trataEntrada(menuEscolhido);
            int idx = int.Parse(menuEscolhido);
            menuFunc MenuS = (menuFunc)idx;
            switch (MenuS)
            {
                case menuFunc.adicionar:
                    NovoFuncionario();
                    break;
                case menuFunc.editar:
                    EditaFunc();
                    break;
                case menuFunc.voltar:
                    break;
                default:
                    Console.WriteLine("Por favor, escolhe uma opção válida.");
                    Thread.Sleep(2000);
                    Console.Clear();
                    break;
            }
        }


        static void NovoFuncionario()
        {
            string situacao = "A";
            string dt_cad = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            Console.WriteLine("Informe o nome do novo Funcionário");
            string nome = Console.ReadLine();

            Console.WriteLine("Informe o seu CPF");
            string cpf = Console.ReadLine();
            Cadastro cad1 = new Cadastro();
            bool ValidaCPF = cad1.validaCpf(cpf);

            if (ValidaCPF == false)
            {
                //false = não tem no BD então libera o cadastro
                Cadastro cad = new Cadastro(nome, cpf, situacao, dt_cad);
            }
            else
                Console.WriteLine("O CPF '" + cpf + "' já está cadastrado para outro funcionário no Banco de Dados!" +
                    "\nInforme outro CPF por favor.");
        }

        static void EditaFunc()
        {
            Console.WriteLine("Edição de Funcionários:");
            Console.WriteLine("1 - Editar nome\n2 - Editar CPF\n3 - Situação\n4 - Voltar");
            string menuEscolhido = Console.ReadLine();
            Cadastro cad = new Cadastro();
            menuEscolhido = cad.trataEntrada(menuEscolhido);
            int idx1 = int.Parse(menuEscolhido);
            menuEditaFunc MenuX = (menuEditaFunc)idx1;

            switch (MenuX)
            {
                case menuEditaFunc.nome:
                    editaNomeFunc();
                    break;
                case menuEditaFunc.cpf:
                    editaCpfFunc();
                    break;
                case menuEditaFunc.situacao:
                    editaSituacao();
                    break;
                case menuEditaFunc.voltar:
                    ManutencaoFuncionarios();
                    break;
                default:
                    Console.WriteLine("Por favor, escolhe uma opção válida.");
                    Thread.Sleep(2000);
                    Console.Clear();
                    ManutencaoFuncionarios();
                    break;
            }
        }
        static void editaNomeFunc()
        {
            Console.WriteLine("Qual nome deseja realizar a edição?");
            string nome = Console.ReadLine();
            Console.WriteLine("Qual seu novo nome?");
            string nomeNovo = Console.ReadLine();
            Cadastro cad3 = new Cadastro();

            bool ValidaNome = cad3.validaNome(nome);

            if (ValidaNome == true)
            {
                //true = não tem no BD então libera o cadastro
                Cadastro cad = new Cadastro(nome, nomeNovo);
                Console.WriteLine("Cadastro alterado! O nome '" + nome + "' foi alterado para '" + nomeNovo + "'");
            }
            else
                Console.WriteLine("O Nome '" + nome + "' não foi encontrado no banco de dados\nRefaça o procedimento");
        }
        static void editaCpfFunc()
        {
            Console.WriteLine("Qual CPF deseja Editar?");
            string cpf1 = Console.ReadLine();
            Console.WriteLine("Qual seu novo CPF?");
            string novoCpf = Console.ReadLine();

            Cadastro cad1 = new Cadastro();
            bool ValidaCPF = cad1.validaCpf(cpf1);
            bool ValidaCPFNovo = cad1.validaCpf1(novoCpf);
            
            if (ValidaCPF == false )
            {
                //false = Já tem BD então não cadastra
                Console.WriteLine("Não foi encontrado o CPF '" + cpf1 + "'no banco de dados!");
            }
            if (ValidaCPFNovo == true)
            {                
                Console.WriteLine("O CPF '" + novoCpf + "' já está vinculado ha outro funcionário");
            }
            if (ValidaCPF == true & ValidaCPFNovo == false)
            {
                cad1.AlteracaoCpf(cpf1, novoCpf);
                Console.WriteLine("O CPF '" + cpf1 + "' foi alterado para '" + novoCpf + "' com sucesso!");
            } 
        }
        static void editaSituacao()
        {
            Console.WriteLine("A edição da sua situação cadastrada pode ser I para Inativo ou A para Ativo\n" +
                "Informe o CPF do funcionário que deseja realizar a alteração");
            string cpf = Console.ReadLine();
            Cadastro cad1 = new Cadastro();
            bool ValidaCPF = cad1.validaCpf(cpf);

            if (ValidaCPF == true)
            {
                //true = nome encontrato pode seguir              
                Console.WriteLine("Selecione uma opção - 1 Ativar Cadastro - 2 Inativar Funcionário - 3 Voltar");
                int idx = int.Parse(Console.ReadLine());
                menuEditaFuncSit MenuS = (menuEditaFuncSit)idx;
                switch (MenuS)
                {
                    case menuEditaFuncSit.ativar:
                        cad1.ativaSituacao(cpf);
                        break;
                    case menuEditaFuncSit.inativar:
                        cad1.desativaSituacao(cpf);
                        break;
                    case menuEditaFuncSit.voltar:
                        ManutencaoFuncionarios();
                        break;
                    default:
                        Console.WriteLine("Seleção inválida.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        break;
                }
            }
            else
                Console.WriteLine("O Nome '" + cpf + "' não foi encontrado no banco de dados\nRefaça o procedimento");
        }

        static void ManutencaoTicket()
        {
            Console.WriteLine("Manutenção de Tickets:");
            Console.WriteLine("1 - Adicionar novo\n2 - Editar\n3 - Voltar");
            string menuEscolhido = Console.ReadLine();
            Cadastro cad = new Cadastro();
            menuEscolhido = cad.trataEntrada(menuEscolhido);
            int idx = int.Parse(menuEscolhido);
            menuFunc MenuS = (menuFunc)idx;
            switch (MenuS)
            {
                case menuFunc.adicionar:
                    NovoTicket();
                    break;
                case menuFunc.editar:
                    EditaTicket();
                    break;
                case menuFunc.voltar:
                    break;
                default:
                    Console.WriteLine("Por favor, escolhe uma opção válida.");
                    Thread.Sleep(2000);
                    Console.Clear();
                    break;
            }
        }

        static void NovoTicket()
        {
            Console.WriteLine("Informe o cpf Funcionário esta recebendo o ticket");
            string cpf = Console.ReadLine();

            Console.WriteLine("Informe a Quantidade de Tickets entregue ao Funcionário");
            string qtdTicket = Console.ReadLine();

            Ticket tck = new Ticket();
            string msg = tck.validaCpf(cpf, qtdTicket);
            Console.WriteLine(msg);
        }

        static void EditaTicket()
        {            
            Console.WriteLine("Edição de Tickets:");
            Console.WriteLine("1 - Edição de quantidade entregue\n2 - Edição de funcionário que foi entregue\n3 - Edição de situação de Ticket\n 4 - Voltar");
            string menuEscolhido = Console.ReadLine();
            Cadastro cad = new Cadastro();
            menuEscolhido = cad.trataEntrada(menuEscolhido);
            int idx = int.Parse(menuEscolhido);
            menuEditaTicket MenuS = (menuEditaTicket)idx;
            switch (MenuS)
            {
                case menuEditaTicket.editaQuantidade:
                    EditaTicketQuantidade();
                    break;
                case menuEditaTicket.editaFuncionario:
                    EditaFuncionarioTktEntregue();
                    break;
                case menuEditaTicket.editaSituacao:
                    EditaFuncionarioTktSituacao();
                    break;
                case menuEditaTicket.voltar:
                    ManutencaoTicket();
                    break;
                default:
                    Console.WriteLine("Por favor, escolhe uma opção válida.");
                    Thread.Sleep(2000);
                    Console.Clear();
                    break;
            }

        }

        static void EditaTicketQuantidade()
        {
            Console.WriteLine("Qual o Identificador do Ticket que deseja alterar sua Quantidade?");
            string idTicket = Console.ReadLine();
            Console.WriteLine("Qual a Quantidade a ser atualizada?");
            string qtdTicketNew = Console.ReadLine();
            Ticket tck = new Ticket();
            string msg = tck.EditaQtdTicket(idTicket, qtdTicketNew);
            Console.WriteLine(msg);
        }

        static void EditaFuncionarioTktEntregue()
        {
            Console.WriteLine("Qual o Identificador do Ticket que deseja alterar o Funcionário que foi referenciado?");
            string idTicket = Console.ReadLine();
            Console.WriteLine("Qual o CPF do funcionário que deve ser lançado o Ticket?");
            string cpfFuncionario = Console.ReadLine();
            Ticket tck = new Ticket();
            string msg = tck.EditaFuncTktEntregue(idTicket, cpfFuncionario);
            Console.WriteLine(msg);
        }

        static void EditaFuncionarioTktSituacao()
        {
            Console.WriteLine("Qual o Identificador do Ticket que deseja alterar a sua situação?");
            string idTicket = Console.ReadLine();
            Console.WriteLine("A situação do Ticket é definido como ATIVO ou INATIVO\n 1 - Para ATIVAR 2 - Para INATIVAR");
            string situacao = Console.ReadLine();
            Ticket tck = new Ticket();  
            string msg = tck.EditaSitFuncTkt(idTicket, situacao);
            Console.WriteLine(msg);
        }

        static void ManutencaoRelatorio()
        {
            //enum menuRelatorio { totalFunc = 1, totalFuncPeriodo, totalGeral, totalGeralPeriodo, voltar }
            Console.WriteLine("Edição de Tickets:");
            Console.WriteLine("1 - Total por Funcionário\n2 - Total por Funcionário e período\n3 - Total Geral Periodo\n4 - Voltar");
            string menuEscolhido = Console.ReadLine();
            Cadastro cad = new Cadastro();
            menuEscolhido = cad.trataEntrada(menuEscolhido);
            int idx = int.Parse(menuEscolhido);
            menuRelatorio MenuS = (menuRelatorio)idx;
            switch (MenuS)
            {
                case menuRelatorio.totalFunc:
                    RelTotalFunc();
                    break;
                case menuRelatorio.totalFuncPeriodo:
                    RelTotalFuncPeriodo();
                    break;
                case menuRelatorio.totalGeralPeriodo:
                    RelTotalPeriodo();
                    break;
                case menuRelatorio.voltar:
                    ManutencaoRelatorio();
                    break;                
                default:
                    Console.WriteLine("Por favor, escolhe uma opção válida5.");
                    Thread.Sleep(2000);
                    Console.Clear();
                    break;
            }
        }
        static void RelTotalFunc()
        {
            Console.WriteLine("Qual o CPF do funcionário que deseja visualizar o total de Tickets já entregues");
            string cpf = Console.ReadLine();            
            Ticket tck = new Ticket();
            string msg = tck.RelatorioTotalPorFuncionario(cpf);            
            Console.WriteLine(msg);//tirar?
        }

        static void RelTotalFuncPeriodo()
        {
            Console.WriteLine("Qual o CPF do funcionário que deseja visualizar o total de Tickets já entregues");
            string cpf = Console.ReadLine();
            Console.WriteLine("Qual a data inicial de pesquisa? - Informe o dia/mês/ano Exemplo 01/12/2000");
            string dt = Console.ReadLine();
            string dia = dt.Substring(0, 3);
            string mes = dt.Substring(3, 3);
            string ano = dt.Substring(6, 4);
            string dataInicial = mes + dia + ano;   

            Console.WriteLine("Qual a data Final de pesquisa? - Informe o dia/mês/ano Exemplo 01/12/2000");            
            string dt1 = Console.ReadLine();
            string dia1 = dt1.Substring(0, 3);
            string mes1 = dt1.Substring(3, 3);
            string ano1 = dt1.Substring(6, 4);
            string dataFinal = mes1 + dia1 + ano1;

            Ticket tck = new Ticket();
            string msg = tck.RelatorioTotalPorFuncPeriodo(cpf, dataInicial, dataFinal);
            Console.WriteLine(msg);
        }

        static void RelTotalPeriodo()
        {
            Console.WriteLine("Qual a data inicial de pesquisa? - Informe o dia/mês/ano Exemplo 01/12/2000");
            string dt = Console.ReadLine();
            string dia = dt.Substring(0, 3);
            string mes = dt.Substring(3, 3);
            string ano = dt.Substring(6, 4);
            string dataInicial = mes + dia + ano;

            Console.WriteLine("Qual a data Final de pesquisa? - Informe o dia/mês/ano Exemplo 01/12/2000");
            string dt1 = Console.ReadLine();
            string dia1 = dt1.Substring(0, 3);
            string mes1 = dt1.Substring(3, 3);
            string ano1 = dt1.Substring(6, 4);
            string dataFinal = mes1 + dia1 + ano1;

            Ticket tck = new Ticket();
            string msg = tck.RelatorioTotalPeriodo(dataInicial, dataFinal);
            Console.WriteLine(msg);
        }

    }
}












