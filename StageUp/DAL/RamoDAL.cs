using Microsoft.Extensions.Configuration;
using StageUp.Models.Interfaces;
using StageUp.Models.POCO;
using System.Data;
using System.Data.SqlClient;

namespace StageUp.DAL
{
    public class RamoDAL : IRamoDAL
    {
        private readonly IConfiguration _configuration;
        public RamoDAL(IConfiguration configuration) { _configuration = configuration; }
        public async Task<DataSet> ConsultarTodos()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandText = "Select * from RAMO"
                };
                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (!r.Read())
                {
                    conn.Close();
                    return null;
                }
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                conn.Close();
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
        }
    }
}
