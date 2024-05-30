using System.Data;

namespace StageUp.Models.POCO
{
    public class Setor
    {
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public int? fk_Id_Ramo { get; set; }
        public DataSet? Setores { get; set; }
    }
}
