using Microsoft.Extensions.Configuration;
using StageUp.Models.Interfaces;
using StageUp.Models.POCO;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace StageUp.DAL
{
    public class SetorDAL : ISetorDAl
    {
        private readonly IConfiguration _configuration;
        public SetorDAL(IConfiguration configuration) { _configuration = configuration; }

        public async Task<Setor> ConsultarPorIdRamo(int id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    Setor setor = new Setor();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = conn,
                        CommandText = "Select * from SETOR WHERE fk_Id_Ramo = @RAMO"
                    };
                    cmd.Parameters.AddWithValue("@RAMO", id);
                    conn.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    conn.Close();
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    setor.Setores = dataSet;
                    return setor;
                }
            }
            catch { return null; }
        }

        public async Task<DataSet> ConsultarTodos()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    Setor setor = new Setor();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = conn,
                        CommandText = "Select * from SETOR"
                    };
                    conn.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    conn.Close();
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    setor.Setores = dataSet;
                    return dataSet;
                }
            }
            catch { return null; }
        }

    }
}
