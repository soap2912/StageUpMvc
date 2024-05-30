using StageUp.Models.Interfaces;
using StageUp.Models.POCO;
using System.Data.SqlClient;

namespace StageUp.DAL
{
    public class EnderecoDAL : IEnderecoDAL
    {
        private readonly IConfiguration _configuration;
        public EnderecoDAL(IConfiguration configuration) { _configuration = configuration; }

        //public async Task<int> Cadastrar(Endereco endereco)
        //{
        //    string connectionString = _configuration.GetConnectionString("DefaultConnection");
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand()
        //            {
        //                Connection = conn,
        //                CommandText = "INSERT INTO ENDERECO (Rua, Bairro, Cidade, UF, Latitude, Longitude) VALUES (@RUA, @BAI, @CID, @UF, @LAT, @LON); SELECT SCOPE_IDENTITY();"
        //            };
        //            cmd.Parameters.AddWithValue("@RUA", endereco.Rua);
        //            cmd.Parameters.AddWithValue("@BAI", endereco.Bairro);
        //            cmd.Parameters.AddWithValue("@CID", endereco.Cidade);
        //            cmd.Parameters.AddWithValue("@UF", endereco.Uf);
        //            cmd.Parameters.AddWithValue("@LAT", endereco.Latitude);
        //            cmd.Parameters.AddWithValue("@LON", endereco.Longitude);
        //            conn.Open();
        //            var _id = await cmd.ExecuteScalarAsync();
        //            conn.Close();
        //            return Convert.ToInt32(_id);
        //        }
        //        catch (Exception ex) { return -1; }
        //    }
        //}
        public async Task<int> Cadastrar(Endereco endereco) { return 1; }

        public async Task<Endereco> ConsultarPorCep(string cep)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandText = "SELECT * FROM ENDERECO WHERE Cep = @CEP"
                };
                conn.Open();
                cmd.Parameters.AddWithValue("@CEP",cep);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (!reader.Read()) 
                    return null;
                Endereco endereco = new Endereco();

                endereco.Id = reader.GetInt32(0);
                endereco.Rua = reader.GetString(1);
                endereco.Bairro = reader.GetString(2);
                endereco.Cidade = reader.GetString(3);
                endereco.Uf = reader.GetString(4);
                endereco.Latitude = reader.GetString(5);
                endereco.Longitude = reader.GetString(6);
                endereco.Cep = reader.GetString(7);
                return endereco;
            }
        }
    }
}
