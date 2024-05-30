namespace StageUp.Models.POCO
{
    public class Endereco
    {
        public int? Id { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }
        public string Uf { get; set; }
        public string Cidade { get; set; }
    }
}
