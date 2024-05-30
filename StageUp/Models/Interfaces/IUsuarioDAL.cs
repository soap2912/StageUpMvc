using StageUp.Models.POCO;

namespace StageUp.Models.Interfaces
{
    public interface IUsuarioDAL
    {
        public Task ConsultarPorEmailESenha(Usuario user);
        public Task<Usuario> ConsultarInfos(Usuario usuario);
        public Task<bool> ConsultarCelular(string cell);
        public Task<bool> ConsultarEmail(string email);
        public Task<bool> Cadastrar(Usuario user);

    }
}
