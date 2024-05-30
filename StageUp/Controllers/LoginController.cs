using Microsoft.AspNetCore.Mvc;
using StageUp.DAL;
using StageUp.Models.Interfaces;
using StageUp.Models.POCO;
using StageUp.Services;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace StageUp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioDAL _usuarioDAL;

        public LoginController(IConfiguration configuration, IUsuarioDAL usuarioDAL)
        {
            _configuration = configuration;
            _usuarioDAL = usuarioDAL;
        }

        public IActionResult Index(Usuario user)
        {
            HttpContext.Session.SetInt32("TipoCadastro", 1);
            return View(user);
        }


        [HttpPost]
        public IActionResult Logar([FromForm] Usuario user, bool Log)
        {
            // Se algum dos campos estiver vazio
            if (!VerificaCampos(user.Email, user.Senha))
            {
                TratamentoDeErros(1);
                return RedirectToAction("Index");
            }
            //Verifica se o formato do email está correto
            if (!new EmailAddressAttribute().IsValid(user.Email))
            {
                TratamentoDeErros(2);
                return RedirectToAction("Index");
            }

            // Verifica se o usuario Existe no banco de dados 
            var _con = ConsultaUsuario(user).Result;

            if(!_con.Item1)
            {
                TratamentoDeErros(3);
                return RedirectToAction("Index");
            }
            user = _con.Item2;
            CriaSession(user,Log);
            return Ok(user);
        }
        private bool VerificaCampos(string email, string senha)
        {
            if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
                return false;
            return  true;
        }
        private void TratamentoDeErros(int erro)
        {
            if (erro == 1) 
            {
                TempData["Mensagem"] = "Preencha todos os campos";
                TempData["Erro"] = 1;
                TempData.Save();
                return;
            }
            //formato de email inválido
            if(erro == 2)
            {
                TempData["Erro"] = 2;
                TempData.Save();
                return;
            }
            if(erro ==3)
            {
                TempData["Mensagem"] = "Usuario não encontrado";
                TempData["Erro"] = 3;
                TempData.Save();
                return;
            }
        }
        private async Task<(bool, Usuario)> ConsultaUsuario(Usuario user)
        {
           Usuario _usuario = await _usuarioDAL.ConsultarInfos(user);

            // Se o usuário não existir, retorna false e um usuário nulo
            if (_usuario == null)
                return (false, null);

            return (true,_usuario);
        }
        private void CriaSession(Usuario user, bool Con)
        {
            if (Con)
                CriaCookie(user.RandomKey.ToString());
            HttpContext.Session.SetString("InfosUsuario" ,new SerializadorJson().SerializaUsuario(user));
        }
        private void CriaCookie(string key)
        {
            // Define a data de expiração para daqui a 15 dias
            DateTimeOffset dataExpiracao = DateTimeOffset.Now.AddDays(15);

            // Configura as opções do cookie
            CookieOptions options = new CookieOptions
            {
                Expires = dataExpiracao
            };

            // Adiciona o cookie com as opções configuradas
            Response.Cookies.Append("RandomKeyUser", key, options);
        }

    }
}
