using StageUp.Models.POCO;
using System.Data;

namespace StageUp.Models.Interfaces
{
    public interface ISetorDAl
    {
        public Task<DataSet> ConsultarTodos();
        public Task< Setor > ConsultarPorIdRamo(int id);  
    }
}
