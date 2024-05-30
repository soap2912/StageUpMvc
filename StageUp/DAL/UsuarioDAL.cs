using StageUp.Models.Interfaces;
using StageUp.Models.POCO;

using System.Data.SqlClient;

namespace StageUp.DAL
{
    public class UsuarioDAL : IUsuarioDAL
    {
        private readonly IConfiguration _configuration;
        public UsuarioDAL(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        /////////////// Consultas ///////////////////
        public async Task ConsultarPorEmailESenha( Usuario user) { }
        public async Task<Usuario> ConsultarInfos(Usuario usuario)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = conn,
                        CommandText = "SELECT * FROM USUARIO WHERE Email = @EMAIL AND Senha = @SENHA"
                    };
                    cmd.Parameters.AddWithValue("@EMAIL", usuario.Email);
                    cmd.Parameters.AddWithValue("@SENHA", usuario.Senha);
                    conn.Open();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    if (reader.Read())
                    {
                        usuario.Id = reader.GetInt32(0);
                        usuario.Nome = reader.GetString(1);
                        usuario.Sobrenome = reader.GetString(2);
                        usuario.Email = reader.GetString(3);
                        usuario.Celular = reader.GetString(4);
                        usuario.Ativo = 1;
                        usuario.fk_Id_Empresa = reader.GetInt32(6);
                        usuario.fk_Id_Endereco = reader.GetInt32(8);
                        usuario.RandomKey = reader.GetInt32(10);
                        if (!reader.IsDBNull(7))
                        {
                            usuario.fk_Id_Empresa2 = reader.GetInt32(7);
                        }
                        else
                        {
                            usuario.fk_Id_Empresa2 = null;
                        }
                        // Não está claro o que faz UsarioOnline, assumindo que é um método existente
                        //  UsarioOnline(usuario, true);
                        conn.Close();
                        return usuario;
                    }
                    else
                    {
                        conn.Close();
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    // Trate exceções de acordo com as necessidades do seu aplicativo
                    Console.WriteLine("Erro ao consultar o cadastro: " + ex.Message);
                    return null;
                }
            }
        }
        public async Task ConsultarPorId(Usuario user)
        {
           
        }
        public async Task<bool> ConsultarCelular(string cell)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandText = $"SELECT Celular FROM USUARIO WHERE Celular = @CELL"
                };
                cmd.Parameters.AddWithValue("@CELL", cell);
                conn.Open();
                SqlDataReader _reader = await cmd.ExecuteReaderAsync();
                if (_reader.Read())
                {
                    conn.Close();
                    return true;
                }
                conn.Close();
                return false;
            }
            }
        public async Task<bool> ConsultarEmail(string email)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandText = "SELECT Email FROM USUARIO WHERE Email = @EMAIL"
                };
                cmd.Parameters.AddWithValue("@EMAIL", email);
                conn.Open();
                SqlDataReader _reader = await cmd.ExecuteReaderAsync();
                if (_reader.Read())
                {
                    conn.Close();
                    return true;
                }
                conn.Close();
                return false;
            }
        }
        ///////////// Adiciona //////////////////
        public async Task Atualizar()
        {
        }
        public async Task Deletar()
        {
           
        }

        //public async Task<bool> Cadastrar(Usuario user)
        //{
        //    string connectionString = _configuration.GetConnectionString("DefaultConnection");
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand()
        //            {
        //                Connection = conn,
        //                CommandText = "INSERT INTO user(Nome, Sobrenome, Email, Celular, Senha, fk_Id_Empresa, fk_Id_Endereco, Ativo) VALUES (@NOME, @SOB, @EMAIL, @CELL, @SENHA, @FKE, @FKEN, 0);"
        //            };
        //            cmd.Parameters.AddWithValue("@NOME", user.Nome);
        //            cmd.Parameters.AddWithValue("@SOB", user.Sobrenome);
        //            cmd.Parameters.AddWithValue("@EMAIL", user.Email);
        //            cmd.Parameters.AddWithValue("@CELL", user.Celular);
        //            cmd.Parameters.AddWithValue("@SENHA", user.Senha);
        //            cmd.Parameters.AddWithValue("@FKE", user.fk_Id_Empresa);
        //            cmd.Parameters.AddWithValue("@FKEN", user.fk_Id_Endereco);
        //            conn.Open();
        //            cmd.ExecuteNonQuery();
        //            conn.Close();
        //            return true;

        //        }
        //        catch { return false; }
        //    }
        //}
        public async Task<bool> Cadastrar(Usuario user) { return false; }
    }
}
