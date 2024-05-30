using StageUp.Models.POCO;
using System.Data;

namespace StageUp.Models.Interfaces
{
    public interface IRamoDAL
    {
        public Task<DataSet> ConsultarTodos();


    }
}
