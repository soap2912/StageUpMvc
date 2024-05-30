using StageUp.Models.POCO;

namespace StageUp.Models
{
    public static class InicializaPOCO
    {
        public static Usuario Usuario(Usuario user)
        {
            if (user.Empresa == null)
                user.Empresa = Empresa(user.Empresa);
            if (user.Endereco == null)
                user.Endereco = Endereco(user.Endereco);
            return user;
        }
        public static Endereco Endereco(Endereco endereco)
        {
            if (endereco == null)
                endereco = new Endereco();
            return endereco;
        }
        public static Empresa Empresa(Empresa empresa)
        {
            if (empresa == null)
                empresa = new Empresa();
            if (empresa.Ramo == null)
                empresa.Ramo = Ramo(empresa.Ramo);
            if (empresa.Setor == null)
                empresa.Setor = Setor(empresa.Setor);
            return empresa;
        }
        public static Setor Setor(Setor setor)
        {
            if (setor == null)
                setor = new Setor();
            return setor;
        }

        public static Ramo Ramo(Ramo ramo)
        {
            if (ramo == null)
                ramo = new Ramo();
            return ramo;
        }



    }
}
