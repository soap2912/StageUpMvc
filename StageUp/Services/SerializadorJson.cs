using StageUp.Models;
using StageUp.Models.POCO;
using System.Data;
using System.Text.Json;

namespace StageUp.Services
{
    public class SerializadorJson
    {
        public string SerializaUsuario(Usuario usuario)
        {
            var json = JsonSerializer.Serialize(usuario);
            return json;
        }
        public Usuario DesserializaUsuario(string json)
        {
            Usuario usuario = JsonSerializer.Deserialize<Usuario>(json);
            return usuario;
        }
    }
}
