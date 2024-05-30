using StageUp.Models.POCO;

namespace StageUp.Models.Interfaces
{
    public interface IEnderecoDAL
    {
        public  Task<Endereco> ConsultarPorCep(string cep);
        public Task<int> Cadastrar(Endereco endereco);
    }
}