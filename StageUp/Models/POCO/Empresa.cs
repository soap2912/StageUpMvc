namespace StageUp.Models.POCO
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public int? fk_Id_Ramo { get; set; }
        public int? fk_Id_Setor { get; set; }
        public Ramo? Ramo { get; set; }
        public Setor? Setor { get; set; }
    }
}
