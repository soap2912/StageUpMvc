using StageUp.Models.POCO;

namespace StageUp.Models.Interfaces
{
    public interface IApis
    {
        public Task<Endereco> ViaCepConsultarPorCep(string cep);

        public Task<Empresa> ReceitaFerderalConsultaPorCnpj(string cnpj);
    }
}
