namespace StageUp.Models.POCO
{
    public class Usuario
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Senha { get; set; }
        public int? Ativo { get; set; }
        public int? fk_Id_Empresa { get; set; }
        public int? fk_Id_Empresa2 { get; set; }
        public int? RandomKey { get; set; }
        public int? fk_Id_Endereco { get; set; }
        public string Conf_senha { get; set; }
        public Endereco? Endereco { get; set; }
        public Empresa? Empresa { get; set; }


    }
}
