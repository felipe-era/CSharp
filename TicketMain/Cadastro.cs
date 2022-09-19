using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace TicketMain
{
    public class Cadastro
    {
        Conexao conexao = new Conexao();
        SqlCommand cmd = new SqlCommand();
        public String msg = "";
        public bool temCpf = false;
        public bool temCpf2 = false;
        public bool temNome = false;
        SqlDataReader dr;
        public string dt_alter = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

        public Cadastro()
        {
        }

        public Cadastro(string nome, string nomeNovo)
        {
            //string dt_alter = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            cmd.CommandText = "select * from funcionario where nome = @nome;";
            Console.WriteLine(dt_alter);
            cmd.CommandText = "update funcionario set nome = @nomeNovo, dt_alter = @dt_alter where nome = @nome";
            cmd.Parameters.AddWithValue("@nome", nome);
            cmd.Parameters.AddWithValue("@nomeNovo", nomeNovo);
            cmd.Parameters.AddWithValue("@dt_alter", dt_alter);

            try
            {
                cmd.Connection = conexao.Conectar();
                cmd.ExecuteNonQuery();
                conexao.Desconectar();

                this.msg = "Editado!";
                Console.WriteLine(msg);
            }
            catch (SqlException)
            {
                this.msg = "erro ao se conectar";
                Console.WriteLine(msg);
            }
        }

        public Cadastro(string nome, string cpf, string situacao, string dt_cad)
        {

            cmd.CommandText = "insert into funcionario (nome, cpf, situacao, dt_cad) values (@nome, @cpf, @situacao, @dt_cad)";
            /*insert into funcionario (nome, cpf, situacao, dt_cad) VALUES ('joão', '12345678911', 'A', '01-22-2022 22:30:10')*/

            cmd.Parameters.AddWithValue("@nome", nome);
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
        public bool validaCpf(string cpf)
        {
            cmd.CommandText = "select * from funcionario where cpf = @cpf;";
            cmd.Parameters.AddWithValue("@cpf", cpf);
            try
            {
                cmd.Connection = conexao.Conectar();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    temCpf2 = true;
                }
            }
            catch (SqlException)
            {
                this.msg = "Erro de conexão/Sintaxe";
            }
            dr.Close();
            conexao.Desconectar();
            return temCpf2;
        }

        public bool validaCpf1(string novoCpf)
        {
            cmd.CommandText = "select * from funcionario where cpf = @cpf5;";
            cmd.Parameters.AddWithValue("@cpf5", novoCpf);
            try
            {
                cmd.Connection = conexao.Conectar();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    temCpf = true;
                }
            }
            catch (SqlException)
            {
                this.msg = "Erro de conexão/Sintaxe";
            }
            dr.Close();
            conexao.Desconectar();
            return temCpf;
        }

        public bool validaNome(string nome)
        {
            cmd.CommandText = "select * from funcionario where nome = @nome;";
            cmd.Parameters.AddWithValue("@nome", nome);

            try
            {
                cmd.Connection = conexao.Conectar();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    temNome = true;
                }
            }
            catch (SqlException)
            {
                this.msg = "Erro de conexão/Sintaxe";
            }
            
            dr.Close();
            return temNome;
        }

        public void AlteracaoCpf(string cpf1, string novoCpf)
        {
            cmd.CommandText = "update funcionario set cpf = @novoCpf, dt_alter = @dt_alter where cpf = @cpf1";
            cmd.Parameters.AddWithValue("@cpf1", cpf1);
            cmd.Parameters.AddWithValue("@novoCpf", novoCpf);
            cmd.Parameters.AddWithValue("@dt_alter", dt_alter);
            try
            {
                cmd.Connection = conexao.Conectar();
                cmd.ExecuteNonQuery();
                conexao.Desconectar();
                this.msg = "Editado!";
                Console.WriteLine(msg);
            }
            catch (SqlException)
            {
                this.msg = "Erro de conexão/Sintaxe";
                Console.WriteLine(msg);
            }
            return;
        }


        public void ativaSituacao(string cpf)
        {
            string dt_alter = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            cmd.CommandText = "update funcionario set situacao = 'A', dt_alter = @dt_alter where cpf = @cpfAltera";

            cmd.Parameters.AddWithValue("@cpfAltera", cpf);
            cmd.Parameters.AddWithValue("@dt_alter", dt_alter);
            try
            {
                cmd.Connection = conexao.Conectar();
                cmd.ExecuteNonQuery();
                conexao.Desconectar();
                this.msg = "Editado a situação do funcionário '" + cpf + "' para Ativo";
                Console.WriteLine(msg);
            }
            catch (SqlException)
            {
                this.msg = "Erro de conexão/Sintaxe";
                Console.WriteLine(msg);
            }
            return;

        }

        public void desativaSituacao(string cpf)
        {
            string dt_alter = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            cmd.CommandText = "update funcionario set situacao = 'I', dt_alter = @dt_alter where cpf = @cpfAlteraInativa";

            cmd.Parameters.AddWithValue("@cpfAlteraInativa", cpf);
            cmd.Parameters.AddWithValue("@dt_alter", dt_alter);


            try
            {
                cmd.Connection = conexao.Conectar();
                cmd.ExecuteNonQuery();
                conexao.Desconectar();
                this.msg = "Editado a situação do funcionário '" + cpf + "' para Inativo";
                Console.WriteLine(msg);
            }
            catch (SqlException)
            {
                this.msg = "Erro de conexão/Sintaxe";
                Console.WriteLine(msg);
            }
            return;

        }

        public string trataEntrada(string menuEscolhido)
        {            
            if (int.TryParse(menuEscolhido, out int value))
            {                
                Console.WriteLine(menuEscolhido);
            }
            else
            {
                menuEscolhido = "9";
            }
            return menuEscolhido;
        }

    }
}
