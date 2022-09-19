using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketMain
{
    public class Conexao
    {

        SqlConnection con = new SqlConnection();   

        public Conexao()
        {
            con.ConnectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=teste01;Integrated Security=True";
            //("Data Source=PC\\SERVIDOR;Initial Catalog=BANCO DE DADOS;User ID=USUARIO;Password=SENHA"));
        }

        public SqlConnection Conectar()
        {
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            return con;
        }

        public void Desconectar()
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

        }
    }
}
