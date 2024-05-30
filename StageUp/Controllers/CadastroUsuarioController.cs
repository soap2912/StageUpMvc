using Microsoft.AspNetCore.Mvc;
using StageUp.Models.POCO;
using StageUp.DAL;
using StageUp.Services;
using StageUp.Models.Interfaces;
using StageUp.Models;
using Codify;

namespace StageUp.Controllers
{
    public class CadastroUsuarioController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IEnderecoDAL _enderecoDAL;
        private readonly IUsuarioDAL _usuarioDAL;
        public readonly IApis _apis;
        public CadastroUsuarioController(IConfiguration configuration, IEnderecoDAL enderecoDAL, IUsuarioDAL usuarioDAL, IApis apis)
        {
            _configuration = configuration;
            _enderecoDAL = enderecoDAL;
            _usuarioDAL = usuarioDAL;
            _apis = apis;
        }
        /////////////////////////////// VIEW /////////////////////////////////
        public IActionResult Index(Usuario user)
        {
            user = InicializaPOCO.Usuario(user);
            user = PegarInfosUsuarioTempdata();
            return View(user);
        }
        /////////////////////////////// FIM  /////////////////////////////////
        
        /////////////////////////////// PRINCIPAL ///////////////////////////////
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromForm] Usuario user)
        {
            //Verifica se os campos estão preenchidos 
            if (!VerificaCampos(user))
                return RedirectToAction("Index", user);
            //Verifica se endereço existe 
            Endereco _end = await ConsultarCep(user.Endereco.Cep);
            if (_end == null)
            {
                TratamentoDeErros(4);
                SalvarIndosUsuarioTempdata(user);
                return RedirectToAction("Index", user);
            }
            //Verifica se celular e email existem no banco de dados 
            if (!await ConsultaEmailCelularDAL(user))
                return RedirectToAction("Index");
            //Salva as infos do usuario em uma section e seta o tipo de cadastro em outra
            if(_end.Id != null)
                user.fk_Id_Endereco = _end.Id;
            if (user.Endereco.Latitude == null || user.Endereco.Longitude == null)
                user.Endereco = await ConsultarCep(user.Endereco.Cep);
            
            
            SalvarIndosUsuarioSession(user);
            // Redireciona para pagina de cadastro da empresa
            return RedirectToAction("Index", "CadastroEmpresa",user.Empresa);
        }
        /////////////////////////////// FIM  /////////////////////////////////

        /////////////////////////////// CONSULTAS  /////////////////////////////////

        //Verifica se celular já existe no banco de dados e retorna a informação para a view
        public async Task<JsonResult> ConsultaCelularJs(Usuario user)
        {
            bool _existe = await ConsultarCelularDAL(user.Celular);
            return Json(new { existe = _existe });
        }
        //Verifica se celular já existe no banco de dados
        private async Task<bool> ConsultarCelularDAL(string cell)
        {
            return await _usuarioDAL.ConsultarCelular(cell);
        }
        //Retorna informaçoes do endereço a partir do cep para a view
        public async Task<JsonResult> ConsultarCepJs(Usuario user)
        {
            string cep = user.Endereco.Cep;
            user.Endereco = await ConsultarCep(cep);
            if (user.Endereco == null)
            {
                return Json(new { success = false, });
            }
            return Json(new
            {
                success = true,
                rua = user.Endereco.Rua,
                bairro = user.Endereco.Bairro,
                cidade = user.Endereco.Cidade,
                uf = user.Endereco.Uf
            });
        }
        // Metodo para consultar o cep, pega informação de outros dois, verificando na DAL e com uma API
        public async Task<Endereco> ConsultarCep(string cep)
        {
            Endereco endereco = await ConsultarCepDAL(cep);
            if (endereco != null)
                return endereco;
            endereco = await ConsultarCepAPI(cep);
            if (endereco != null)
                return endereco;
            return null;
        }
        //Verifica se o cep existe na DAL, se sim retorna as informações dele
        private async Task<Endereco> ConsultarCepDAL(string cep)
        {
            // Pesquisa o endereço no banco de dados
            Endereco _end = await _enderecoDAL.ConsultarPorCep(cep);
            //se não existir retorna null
            if (_end == null)
                return null;
            return _end;
        }
        // Verifica se o Endereço existe a partir de uma api
        private async Task<Endereco> ConsultarCepAPI(string cep)
        {
            //Pesquisa informaçoes do endereço com base no cep
            Endereco end = await _apis.ViaCepConsultarPorCep(cep);
            // Se o cep for nulo
            if (end == null)
                return null;
            return end;
        }
        private async Task<bool> ConsultarEmailDAL(string email)
        {
            return await _usuarioDAL.ConsultarEmail(email);
        }
        private async Task<bool> ConsultaEmailCelularDAL(Usuario user)
        {
            if (await ConsultarEmailDAL(user.Email))
            {
                TratamentoDeErros(6);
                SalvarIndosUsuarioTempdata(user);
                return false;
            }
            if (await ConsultarCelularDAL(user.Celular))
            {
                TratamentoDeErros(7);
                SalvarIndosUsuarioTempdata(user);
                return false;
            }
            return true;
        }
        /////////////////////////////// FIM  /////////////////////////////////
        ///
        /////////////////////////////// VERIFICA OS CAMPOS  /////////////////////////////////
        //Verifica se todos os campos estão prenchidos corretamente
        private bool VerificaCampos(Usuario user)
        {

            List<string> campos = new List<string>()
            {
               user.Endereco.Rua,
                user.Endereco.Bairro,
                user.Endereco.Cidade,
                user.Endereco.Uf,
                user.Nome,
                user.Sobrenome,
                user.Senha,
                user.Conf_senha,
                user.Celular,
                user.Email,
            };
            if (campos.Any(string.IsNullOrWhiteSpace))
            {
                TratamentoDeErros(5);
                SalvarIndosUsuarioTempdata(user);
                return false;
            }
            if (!VerificaFormatoEmailSenha(user))
                return false;

            else return true;
        }
        // Verifica se o email e senha estão certos
        private bool VerificaFormatoEmailSenha(Usuario user)
        {
            if (!ValidaFormato.Email(user.Email))
            {
                TratamentoDeErros(1);
                return false;
            }
            if (!ValidaFormato.Senha(user.Senha))
            {
                TratamentoDeErros(2);
                return false;
            }
            if (user.Senha != user.Conf_senha)
            {
                TratamentoDeErros(3);
                return false;
            }
            return true;
        }
        /////////////////////////////// FIM  /////////////////////////////////
        
        /////////////////////////////// SALVA INFORMAÇÕES  /////////////////////////////////
        // Usado para tratamento de erros
        // Salva as informações do usuario para serem acessadas pela index em caso de retorno
        private void SalvarIndosUsuarioTempdata(Usuario user)
        {
            TempData["InfosUsuario"] = new SerializadorJson().SerializaUsuario(user);
            TempData.Save();
        }
        //Salva as informações do usuario e o tipo de cadastro numa session
        private void SalvarIndosUsuarioSession(Usuario user)
        {
            var senhaCod = codifica.Encode(user.Senha);
            var json = codifica.Criptografar(new SerializadorJson().SerializaUsuario(user),senhaCod);
            HttpContext.Session.SetString("InfosUsuarioCadastro", json);
            HttpContext.Session.SetString("Json", senhaCod);
            HttpContext.Session.SetInt32("TipoCadastro", 2);
        }
        // Le as informações do usuario
        private Usuario PegarInfosUsuarioTempdata()
        {
            Usuario usuario = new Usuario();
            if (TempData.ContainsKey("InfosUsuario"))
                usuario = new SerializadorJson().DesserializaUsuario(TempData["InfosUsuario"] as string);
            return usuario;
        }

        /////////////////////////////// TRATAMENTO DE ERROS  /////////////////////////////////
        private void TratamentoDeErros(int erro)
        {
            //Formato do email inválido 
            if (erro == 1)
            {
                TempData["Mensagem"] = "Formato do email invalido";
                TempData["Erro"] = 1;
                TempData.Save();
                return;
            }
            // Formato da Senha inválido
            if (erro == 2)
            {
                TempData["Mensagem"] = "Senha deve conter 8 caracteres, 1 especial, 1 letra maiuscula e 1 minuscula";
                TempData["Erro"] = 2;
                TempData.Save();
                return;
            }
            // Senha e Conf_senha nn batem
            if (erro == 3)
            {
                TempData["Mensagem"] = "Senhas incompativeis";
                TempData["Erro"] = 3;
                TempData.Save();
                return;
            }
            if (erro == 4)
            {
                TempData["Mensagem"] = "Cep inválido";
                TempData["Erro"] = 4;
                TempData.Save();
                return;
            }
            if (erro == 5)
            {
                TempData["Mensagem"] = "Preencha todos os campos";
                TempData["Erro"] = 5;
                TempData.Save();
                return;
            }
            if (erro == 6)
            {
                TempData["Mensagem"] = "Email já cadastrado";
                TempData["Erro"] = 6;
                TempData.Save();
                return;
            }
            if (erro == 7)
            {
                TempData["Mensagem"] = "Celular já cadastrado";
                TempData["Erro"] = 7;
                TempData.Save();
                return;
            }
        }
    }
}
