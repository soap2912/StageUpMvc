using Microsoft.Extensions.Configuration;
using StageUp.Models.Interfaces;
using StageUp.Models.POCO;
using System.Data.SqlClient;

namespace StageUp.DAL
{
    public class EmpresaDAL : IEmpresaDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _json = "UURJNVVERXliSFJoPT09ZXlKSlpDSTZiblZzYkN3aVRtOXRaU0k2SW5KdlltVnpkbUZzWkc4aUxDSlRiMkp5Wlc1dmJXVWlPaUpEYjNOMFlTQmtZU0JUYVd4MllTSXNJa1Z0WVdsc0lqb2ljMjloY0M0eU9URXlRR2R0WVdsc0xtTnZiVElpTENKRFpXeDFiR0Z5SWpvaU1URTVOVEE1TXpFd01EZ2lMQ0pUWlc1b1lTSTZJa0F5T1ZBeE1teDBZU0lzSWtGMGFYWnZJanB1ZFd4c0xDSm1hMTlKWkY5RmJYQnlaWE5oSWpwdWRXeHNMQ0ptYTE5SlpGOUZiWEJ5WlhOaE1pSTZiblZzYkN3aVVtRnVaRzl0UzJWNUlqcHVkV3hzTENKbWExOUpaRjlGYm1SbGNtVmpieUk2Ym5Wc2JDd2lRMjl1Wmw5elpXNW9ZU0k2SWtBeU9WQXhNbXgwWVNJc0lrVnVaR1Z5WldOdklqcDdJa2xrSWpwdWRXeHNMQ0pNYjI1bmFYUjFaR1VpT201MWJHd3NJa3hoZEdsMGRXUmxJanB1ZFd4c0xDSkRaWEFpT2lJd056a3hNVEUwTUNJc0lrSmhhWEp5YnlJNklrcGhjbVJwYlNCQmJHVjRZVzVrY21FaUxDSlNkV0VpT2lKU2RXRWdSWE4wWEhVd01FVXhZMmx2SUdSbElGTmNkVEF3UlRFaUxDSlZaaUk2SWxOUUlpd2lRMmxrWVdSbElqb2lSbkpoYm1OcGMyTnZJRTF2Y21GMGJ5SjlMQ0pGYlhCeVpYTmhJanB1ZFd4c2ZRPT0=";
        public EmpresaDAL(IConfiguration configuration) { _configuration = configuration; }

        public async Task<int> Cadastrar(Empresa empresa)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = conn,
                        CommandText = "INSERT INTO EMPRESA (Nome, Cnpj, fk_Id_Ramo, fk_Id_Setor) VALUES (@NOME,@CNPJ,@RAMO,@SETOR); SELECT SCOPE_IDENTITY();"
                    };
                    cmd.Parameters.AddWithValue("@NOME", empresa.Nome);
                    cmd.Parameters.AddWithValue("@CNPJ", empresa.Cnpj);
                    cmd.Parameters.AddWithValue("@RAMO", empresa.fk_Id_Ramo);
                    cmd.Parameters.AddWithValue("@SETOR", empresa.fk_Id_Setor);
                    conn.Open();
                    var _id = await cmd.ExecuteScalarAsync();
                    conn.Close();
                    return Convert.ToInt32(_id);
                }
                catch (Exception ex) { return -1; }
            }
        }

        public async Task<Empresa> ConsultarPorCnpj(string cnpj)
        {

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandText = "Select * from Empresa where Cnpj = @CNPJ"
                };
                conn.Open();
                cmd.Parameters.AddWithValue("@CNPJ", cnpj);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (!reader.Read())
                {
                    conn.Close();
                    return null;
                }
                Empresa empresa = new Empresa()
                {
                    Id = reader.GetInt32(0),
                    Cnpj = cnpj,
                    Nome = reader.GetString(1),
                    fk_Id_Ramo = reader.GetInt32(3),
                    fk_Id_Setor = reader.GetInt32(4),
                };
                conn.Close();
                return empresa;
            }
        }
    }
}
