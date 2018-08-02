using System;
using System.Data.SqlClient;

namespace CSharpAdoNet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Informe a opção desejada:");
            Console.WriteLine("1 - Cadastrar");
            Console.WriteLine("2 - Alterar");
            Console.WriteLine("3 - Excluir");
            Console.WriteLine("4 - Listar");
            Console.WriteLine("5 - Relatório");
            int selecao = Convert.ToInt16(Console.ReadLine());
            switch (selecao)
            {
                case 1: 
                    Console.WriteLine("Cadastran cliente");
                    Console.Write("Informe o nome do cliente que deseja cadastrar:");
                    string nome = Console.ReadLine();
                    Console.Write("Informe o email do cliente:");
                    string email = Console.ReadLine();
                    SalvarCliente(nome, email);
                    break;
                case 2:
                    Console.WriteLine("Alterando cliente");
                    ListarClientes();
                    Console.Write("Informe a id do cliente que deseja alterar:");
                    int id = Convert.ToInt32(Console.ReadLine());
                    (int _id, string _nome, string _email) = SelecionarCliente(id);
                    Console.Clear();
                    Console.WriteLine($"Alterando o cliente {_nome} com e-mail {_email}");
                    Console.Write($"Informe o novo nome para o cliente {_nome}:");
                    _nome = Console.ReadLine();
                    Console.Write($"Informe o novo email:");
                    _email = Console.ReadLine();
                    SalvarCliente(_nome, _email, _id);
                    break;
                case 3:
                    Console.WriteLine("Exclusão de cliente");
                    ListarClientes();
                    Console.Write("Informe a id do cliente que deseja excluir:");
                    int delid = Convert.ToInt32(Console.ReadLine());
                    (int _delid, string _delnome, string _delemail) = SelecionarCliente(delid);
                    Console.Clear();
                    Console.WriteLine($"Excluindo o cliente {_delnome}");
                    Console.Write("Realmente deseja excluir este cliente?(S/N)");
                    if (Console.ReadLine().ToUpper().Equals("S"))
                        DeletarCliente(delid);
                    break;
                case 4:
                    Console.WriteLine("Listagem de cliente");
                    ListarClientes();
                    Console.Write("Informe a id do cliente que deseja listar:");
                    int listid = Convert.ToInt32(Console.ReadLine());
                    (int _listid, string _listnome, string _listemail) = SelecionarCliente(listid);
                    Console.Clear();
                    Console.WriteLine($"Id: {_listid}");
                    Console.WriteLine($"Nome: {_listnome}");
                    Console.WriteLine($"E-mail: {_listemail}");
                    break;
                case 5:
                    Console.WriteLine("Relatório de cliente");
                    ListarClientes();
                    break;
                default:
                    Console.WriteLine("Opção inválida");
                    break;
            }
            Console.ReadLine();
        }

        static void ListarClientes()
        {
            using (SqlConnection conn = new SqlConnection(getStringConn()))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = " select *" +
                                  " from clientes" +
                                  " order by id";
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Console.WriteLine($"ID: {dr["id"]}");
                        Console.WriteLine($"Nome: {dr["nome"]}");
                    }
                }

            }

        }

        static void SalvarCliente(string nome, string email)
        {
           
            using (SqlConnection conn = new SqlConnection(getStringConn()))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = " insert into clientes (nome, email) values (@nome, @email)";
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.ExecuteNonQuery();
            }

        }

        static void SalvarCliente(string nome, string email, int id)
        {

            using (SqlConnection conn = new SqlConnection(getStringConn()))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = " update clientes set nome = @nome, email = @email where id = @id";
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

        }

        static void DeletarCliente(int id)
        {

            using (SqlConnection conn = new SqlConnection(getStringConn()))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = " delete clientes where id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

        }

        static (int, string, string) SelecionarCliente(int id)
        {
            using (SqlConnection conn = new SqlConnection(getStringConn()))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = " select *" +
                                  " from clientes" +
                                  " where id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dr.Read();
                    return (Convert.ToInt32(dr["id"]), dr["nome"].ToString(), dr["email"].ToString());
                }
            }
        }


        static string getStringConn()
        {
            return "Server=RAFA\\SQLEXPRESS;Database=CSharpAdoNET;User Id=sa;Password=TESTE123";
        }
    }

}
