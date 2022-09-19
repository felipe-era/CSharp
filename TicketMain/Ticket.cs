using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TicketMain
{
    public class Ticket
    {
        Conexao conexao = new Conexao();
        SqlCommand cmd = new SqlCommand();

        public string msg = "";
        SqlDataReader dr;


        public Ticket()
        {
        }

        public Ticket(string cpf, string qtdTicket)
        {
            string situacao = "A";
            string dt_cad = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            cmd.CommandText = "insert into ticket (funcionario, quantidade, situacao, dt_entrega) values (@nome, @cpf, @situacao, @dt_cad)";
            //INSERT INTO ticket(funcionario, quantidade, situacao, dt_entrega) values(2, 2, 'A', '12/03/2022 08:10:43');

            cmd.Parameters.AddWithValue("@funcionario", cpf);
            cmd.Parameters.AddWithValue("@cpf", cpf);
            cmd.Parameters.AddWithValue("@situacao", situacao);
            cmd.Parameters.AddWithValue("@dt_cad", dt_cad);

            try
            {
                cmd.Connection = conexao.Conectar();
                cmd.ExecuteNonQuery();
                conexao.Desconectar();

                this.msg = "Cadastro realizado com sucesso.";
                Console.WriteLine(msg);
            }
            catch (SqlException e)
            {
                this.msg = "Erro de conexão/Sintaxe";
                Console.WriteLine(msg);
            }

        }

        public string validaCpf(string cpf, string qtdTicket)
        {
            cmd.CommandText = "select * from funcionario where cpf = @cpf;";
            cmd.Parameters.AddWithValue("@cpf", cpf);
            try
            {
                cmd.Connection = conexao.Conectar();
                dr = cmd.ExecuteReader();
                if (dr.HasRows) //encontrou o cpf
                {
                    dr.Read();
                    //while (dr.Read()) { 
                    //Console.WriteLine("aqui >" + dr.GetValue(0));
                    string idfunc = dr.GetValue(0).ToString(); //pega o id para jogar no campo funcionario
                                                               //string b = dr.GetInt32(0).ToString();

                    //Console.WriteLine(cpf);
                    //Console.WriteLine(qtdTicket);
                    //Console.WriteLine("linha da tabela" + idfunc);

                    NovoTicket(cpf, qtdTicket, idfunc);

                    //}
                }
                else
                {
                    dr.Close();
                    this.msg = "O CPF INFORMADO NÂO EXISTE";
                    return msg;
                }

            }
            catch (SqlException)
            {
                this.msg = "Erro de conexão/Sintaxe";
            }
            this.msg = "Adicionado '" + qtdTicket + "' tickets para o CPF '" + cpf + "'";
            return msg; //por amsg aq

        }

        public string NovoTicket(string cpf, string qtdTicket, string idfunc)
        {
            dr.Close();
            string situacao = "A";
            string dt_cad = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            cmd.CommandText = "insert into ticket (funcionario, quantidade, situacao, dt_entrega) values (@idfunc, @qtdTicket, @situacao, @dt_cad)";
            //INSERT INTO ticket(funcionario, quantidade, situacao, dt_entrega) values(2, 2, 'A', '12/03/2022 08:10:43');


            cmd.Parameters.AddWithValue("@idfunc", idfunc);
            cmd.Parameters.AddWithValue("@qtdTicket", qtdTicket);
            cmd.Parameters.AddWithValue("@situacao", situacao);
            cmd.Parameters.AddWithValue("@dt_cad", dt_cad);


            cmd.Connection = conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();

            this.msg = "Cadastro realizado com sucesso.";
            Console.WriteLine(msg);
            return msg;
        }

        public string EditaQtdTicket(string idTicket, string qtdTicketNew)
        {
            cmd.CommandText = "select * from ticket where id_ticket = @idTicket;";
            cmd.Parameters.AddWithValue("@idTicket", idTicket);
            try
            {
                cmd.Connection = conexao.Conectar();
                dr = cmd.ExecuteReader();
                if (dr.HasRows) //encontrou o ticket
                {
                    NovaQuantidadeTicket(idTicket, qtdTicketNew);
                }
                else
                {
                    dr.Close();
                    this.msg = "O TICKET INFORMADO NÂO EXISTE";
                    //Console.WriteLine(msg);
                    return msg;
                }

            }
            catch (SqlException)
            {
                this.msg = "Erro de conexão/Sintaxe";
            }
            return msg;
        }


        public string NovaQuantidadeTicket(string idTicket, string qtdTicketNew)
        {
            dr.Close();
            cmd.CommandText = "update ticket set quantidade = @qtdTicketNew1 where id_ticket = @idTicket1";
            //update ticket set quantidade = 8 where id_ticket = 1

            //string idTicket1 = idTicket;
            cmd.Parameters.AddWithValue("@idTicket1", idTicket);
            cmd.Parameters.AddWithValue("@qtdTicketNew1", qtdTicketNew);

            cmd.Connection = conexao.Conectar();
            cmd.ExecuteNonQuery();
            conexao.Desconectar();

            this.msg = "Alterada a quantidade do Ticket com Identificador '" + idTicket + "' para '" + qtdTicketNew + "'";
            return msg;
        }


        public string EditaFuncTktEntregue(string idTicket, string cpfFuncionario)
        {
            cmd.CommandText = "select * from ticket where id_ticket = @idTicket;";
            cmd.Parameters.AddWithValue("@idTicket", idTicket);

            try
            {
                cmd.Connection = conexao.Conectar();
                dr = cmd.ExecuteReader();
                if (dr.HasRows) //encontrou o ticket
                {
                    Cadastro cad = new Cadastro();
                    Console.WriteLine(cad.validaCpf(cpfFuncionario));
                    if (cad.validaCpf(cpfFuncionario) == true) //se encontrou ID do ticket e o CPF libera
                    {
                        Console.WriteLine("o cpf existe");
                        dr.Close();
                        EdicaoTktEnd(idTicket, cpfFuncionario);

                    }
                    else
                    {
                        Console.WriteLine("o cpf não existe");
                    }


                    //string a = validaCpf(cpfFuncionario, idTicket );
                    //NovaQuantidadeTicket(idTicket, qtdTicketNew);

                }
                else
                {
                    dr.Close();
                    this.msg = "O TICKET INFORMADO NÂO EXISTE";
                    //Console.WriteLine(msg);
                    return msg;
                }

            }
            catch (SqlException)
            {
                this.msg = "Erro de conexão/Sintaxe";
            }
            return msg;
        }

        public string EdicaoTktEnd(string idTicket, string cpfFuncionario)
        {
            cmd.CommandText = "select * from funcionario where cpf = @cpfFuncionario;";
            cmd.Parameters.AddWithValue("@cpfFuncionario", cpfFuncionario);
            try
            {
                cmd.Connection = conexao.Conectar();
                dr = cmd.ExecuteReader();
                if (dr.HasRows) //encontrou o cpf
                {
                    dr.Read();
                    string idfunc = dr.GetValue(0).ToString(); //pega o id para jogar no campo funcionario
                                                               //string b = dr.GetInt32(0).ToString();
                                                               //Console.WriteLine(dr.GetValue(1).ToString());
                    dr.Close();
                    cmd.CommandText = "update ticket set funcionario = @idTicket4 where id_ticket = @idTicket3";
                    //update ticket set funcionario = 5 where id_ticket = 1

                    cmd.Parameters.AddWithValue("@idTicket3", idTicket);
                    cmd.Parameters.AddWithValue("@idTicket4", idfunc);

                    Console.WriteLine(idTicket);
                    Console.WriteLine(idfunc);

                    cmd.Connection = conexao.Conectar();
                    cmd.ExecuteScalar();
                    conexao.Desconectar();

                    this.msg = "O Ticket com Identificador '" + idTicket + "' foi corrigido para o CPF '" + cpfFuncionario + "' com sucesso";
                    return msg;
                }
            }
            catch (SqlException)
            {
                this.msg = "Erro de conexão/Sintaxe";
            }
            return msg;
        }


        public string EditaSitFuncTkt(string idTicket, string situacao)
        {
            cmd.CommandText = "select * from ticket where id_ticket = @idTicket;";

            cmd.Parameters.AddWithValue("@idTicket", idTicket);
            try
            {
                cmd.Connection = conexao.Conectar();
                dr = cmd.ExecuteReader();
                if (dr.HasRows) //encontrou o ticket
                {
                    if (situacao.Equals("1"))
                    {
                        dr.Close();
                        string situacaoFinal = "A";
                        cmd.CommandText = "update ticket set situacao = @situacao where id_ticket = @idTicket4";
                        //update ticket set situacao = A where id_ticket = 1

                        cmd.Parameters.AddWithValue("@idTicket4", idTicket);
                        cmd.Parameters.AddWithValue("@situacao", situacaoFinal);



                        cmd.Connection = conexao.Conectar();
                        cmd.ExecuteScalar();
                        conexao.Desconectar();

                        this.msg = "AAAAAA '" + idTicket + "' foi corrigido para o CPF '";
                        return msg;
                    }
                    else if (situacao.Equals("2"))
                    {
                        dr.Close();
                        string situacaoFinal = "I";
                        cmd.CommandText = "update ticket set situacao = @situacao where id_ticket = @idTicket4";
                        //update ticket set situacao = A where id_ticket = 1

                        cmd.Parameters.AddWithValue("@idTicket4", idTicket);
                        cmd.Parameters.AddWithValue("@situacao", situacaoFinal);



                        cmd.Connection = conexao.Conectar();
                        cmd.ExecuteScalar();
                        conexao.Desconectar();

                        this.msg = "IIIIII '" + idTicket + "' foi corrigido para o CPF '";
                        return msg;
                    }
                    else
                    {
                        Console.WriteLine("O valor informado é inválido\nAtenção: 1 para ATIVAR ou 2 para INATIVAR");
                    }

                }
                else
                {
                    dr.Close();
                    this.msg = "O TICKET INFORMADO NÂO EXISTE";
                    //Console.WriteLine(msg);
                    return msg;
                }

            }
            catch (SqlException)
            {
                this.msg = "Erro de conexão/Sintaxe";
            }
            return msg;
        }

        public string RelatorioTotalPorFuncionario(string cpf)
        {
            cmd.CommandText = "select * from funcionario where cpf = @cpfFuncionario1;";
            cmd.Parameters.AddWithValue("@cpfFuncionario1", cpf);
            try
            {
                cmd.Connection = conexao.Conectar();
                dr = cmd.ExecuteReader();
                if (dr.HasRows) //encontrou o cpf
                {
                    dr.Read();
                    string idfunc = dr.GetValue(0).ToString();
                    dr.Close();
                    lerValores(idfunc);
                    pegaQuantidade(idfunc);

                }
                else
                {
                    Console.WriteLine("nao existe esse cpf");
                }
            }
            catch (SqlException)
            {
                this.msg = "Erro de conexão/Sintaxe";
            }
            return msg;
        }

        public string RelatorioTotalPorFuncPeriodo(string cpf, string dataInicial, string dataFinal)
        {
            cmd.CommandText = "select * from funcionario where cpf = @cpfFuncionario1;";
            cmd.Parameters.AddWithValue("@cpfFuncionario1", cpf);
            try
            {
                cmd.Connection = conexao.Conectar();
                dr = cmd.ExecuteReader();
                if (dr.HasRows) //encontrou o cpf
                {
                    dr.Read();
                    string idfunc = dr.GetValue(0).ToString();
                    dr.Close();

                    lerValoresData(idfunc, dataInicial, dataFinal);
                    //pegaQuantidadeData(idfunc);

                }
                else
                {
                    Console.WriteLine("nao existe esse cpf");
                }
            }
            catch (SqlException)
            {
                this.msg = "Erro de conexão/Sintaxe";
            }
            return msg;
        }

        public void lerValores(string idfunc) //exibe todos os tickets entregue para o funcionário
        {
            cmd.CommandText = "select * from ticket where funcionario = @idfunc3";
            cmd.Parameters.AddWithValue("@idfunc3", idfunc);
            conexao.Conectar();
            SqlDataReader dr1 = cmd.ExecuteReader();
            while (dr1.Read())
            {
                Console.WriteLine("Número do Ticket: '" + dr1.GetValue(0) + "' Quantidade entregue: '" + dr1.GetValue(2) + "' Data de entrega: '" + dr1.GetValue(4) + "' Situação: (A = Ativo I = Inativo) '" + dr1.GetValue(3) + "'");
            }
        }

        public void pegaQuantidade(string idfunc)
        {
            conexao.Desconectar();
            cmd.CommandText = "select SUM(quantidade) from ticket where funcionario = @idfunc4 and situacao = 'A'";
            cmd.Parameters.AddWithValue("@idfunc4", idfunc);
            conexao.Conectar();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string qtde = dr.GetValue(0).ToString();
                Console.WriteLine("Tendo um total de '" + qtde + "' Tickets Ativos");
            }
        }

        public void lerValoresData(string idfunc, string dataInicial, string dataFinal)
        {
            cmd.CommandText = "select * from ticket where funcionario = @idfunc3 and dt_entrega between @dataInicial and @dataFinal";
            //SELECT* FROM ticket WHERE funcionario = 1 and dt_entrega between '09/16/2022' and '09/18/2022';

            cmd.Parameters.AddWithValue("@idfunc3", idfunc);
            cmd.Parameters.AddWithValue("@dataInicial", dataInicial);
            cmd.Parameters.AddWithValue("@dataFinal", dataFinal);

            conexao.Conectar();
            SqlDataReader dr1 = cmd.ExecuteReader();
            while (dr1.Read())
            {
                Console.WriteLine("Número do Ticket: '" + dr1.GetValue(0) + "' Quantidade entregue: '" + dr1.GetValue(2) + "' Data de entrega: '" + dr1.GetValue(4) + "' Situação: '" + dr1.GetValue(3) + "' (A = Ativo I = Inativo)");
            }
        }

        public void pegaQuantidadeData(string idfunc)
        {
            conexao.Desconectar();
            cmd.CommandText = "select SUM(quantidade) from ticket where funcionario = @idfunc4  and situacao = 'A'";
            cmd.Parameters.AddWithValue("@idfunc4", idfunc);
            conexao.Conectar();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string qtde = dr.GetValue(0).ToString();
                Console.WriteLine("Tendo um total de '" + qtde + "' Tickets Ativos");
            }
        }

        public string RelatorioTotalPeriodo(string dataInicial, string dataFinal)
        {
            cmd.CommandText = "select * from funcionario";
            try
            {
                cmd.Connection = conexao.Conectar();
                dr = cmd.ExecuteReader();
                if (dr.HasRows) //encontrou dados
                {
                    dr.Read();

                    dr.Close();

                    lerValoresDataPeriodo(dataInicial, dataFinal);
                    //pegaQuantidadeData(idfunc);

                }
                else
                {
                    Console.WriteLine("nao existe esse cpf");
                }
            }
            catch (SqlException)
            {
                this.msg = "Erro de conexão/Sintaxe";
            }
            return msg;
        }

        public void lerValoresDataPeriodo(string dataInicial, string dataFinal)
        {
            cmd.CommandText = "select * from ticket where dt_entrega between @dataInicial and @dataFinal";
            //SELECT* FROM ticket WHERE funcionario = 1 and dt_entrega between '09/16/2022' and '09/18/2022';

            cmd.Parameters.AddWithValue("@dataInicial", dataInicial);
            cmd.Parameters.AddWithValue("@dataFinal", dataFinal);

            conexao.Conectar();
            SqlDataReader dr1 = cmd.ExecuteReader();
            while (dr1.Read())
            {
                Console.WriteLine("Número do Ticket: '" + dr1.GetValue(0) + "' Quantidade entregue: '" + dr1.GetValue(2) + "' Data de entrega: '" + dr1.GetValue(4) + "' Situação: '" + dr1.GetValue(3) + "' (A = Ativo I = Inativo)");
            }
        }


    }
}
