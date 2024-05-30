using StageUp.Models.POCO;

namespace StageUp.Models.Interfaces
{
    public interface IEmpresaDAL
    {
        public Task<Empresa> ConsultarPorCnpj(string cnpj);
        public Task<int> Cadastrar(Empresa empresa);
    }
}
