using Codify;
using Microsoft.AspNetCore.Mvc;
using StageUp.Models;
using StageUp.Models.Interfaces;
using StageUp.Models.POCO;
using StageUp.Services;
using System.Data;

namespace StageUp.Controllers
{
    public class CadastroEmpresaController : Controller
    {
        private readonly IEmpresaDAL _EmpresaDAL;
        private readonly IApis _Apis;
        private readonly ISetorDAl _SetorDAl;
        private readonly IRamoDAL _ramoDAL;
        private readonly IEnderecoDAL _enderecoDAL;
        private readonly IUsuarioDAL _usuarioDAL;
        //private readonly string _json = "UURJNVVERXliSFJoPT09ZXlKSlpDSTZiblZzYkN3aVRtOXRaU0k2SW5KdlltVnpkbUZzWkc4aUxDSlRiMkp5Wlc1dmJXVWlPaUpEYjNOMFlTQmtZU0JUYVd4MllTSXNJa1Z0WVdsc0lqb2ljMjloY0M0eU9URXlRR2R0WVdsc0xtTnZiVElpTENKRFpXeDFiR0Z5SWpvaU1URTVOVEE1TXpFd01EZ2lMQ0pUWlc1b1lTSTZJa0F5T1ZBeE1teDBZU0lzSWtGMGFYWnZJanB1ZFd4c0xDSm1hMTlKWkY5RmJYQnlaWE5oSWpwdWRXeHNMQ0ptYTE5SlpGOUZiWEJ5WlhOaE1pSTZiblZzYkN3aVVtRnVaRzl0UzJWNUlqcHVkV3hzTENKbWExOUpaRjlGYm1SbGNtVmpieUk2Ym5Wc2JDd2lRMjl1Wmw5elpXNW9ZU0k2SWtBeU9WQXhNbXgwWVNJc0lrVnVaR1Z5WldOdklqcDdJa2xrSWpwdWRXeHNMQ0pNYjI1bmFYUjFaR1VpT2lJdE5EWXNOelV3TWpnNUlpd2lUR0YwYVhSMVpHVWlPaUl0TWpNc01qZzFNVE15TVNJc0lrTmxjQ0k2SWpBM09URXhNVFF3SWl3aVFtRnBjbkp2SWpvaVNtRnlaR2x0SUVGc1pYaGhibVJ5WVNJc0lsSjFZU0k2SWxKMVlTQkZjM1JjZFRBd1JURmphVzhnWkdVZ1UxeDFNREJGTVNJc0lsVm1Jam9pVTFBaUxDSkRhV1JoWkdVaU9pSkdjbUZ1WTJselkyOGdUVzl5WVhSdkluMHNJa1Z0Y0hKbGMyRWlPbTUxYkd4OQ==";
        //private readonly string _senha = "QDI5UDEybHRh";
        public CadastroEmpresaController(IEmpresaDAL empresaDAL, IApis apis, ISetorDAl setorDAl, IRamoDAL ramoDAL, IEnderecoDAL enderecoDAL, IUsuarioDAL usuarioDAL)
        {
            _EmpresaDAL = empresaDAL;
            _Apis = apis;
            _SetorDAl = setorDAl;
            _ramoDAL = ramoDAL;
            _EmpresaDAL = empresaDAL;
            _usuarioDAL = usuarioDAL;
            _enderecoDAL = enderecoDAL;


        }
        public async Task<IActionResult> Index(Empresa empresa)
        {
            HttpContext.Session.SetInt32("TipoCadastro", 1);
            empresa = await CarregaTabelas();
            return View(empresa);
        }
        public async Task<IActionResult> Cadastrar([FromForm] Empresa empresa)
        {
            // verifica todos os campos
            if (!VerificaCampos(empresa))
                //retorna se algum não estiver preenchido
                return RedirectToAction("Index", empresa);

            var tp = HttpContext.Session.GetInt32("TipoCadastro");

            //consulta empresa no banco de dados
            Empresa emp = await ConsultarEmpresaDAL(empresa.Cnpj);

            // se for cadastrar apenas a empresa
            if (tp == 1)
            {
                // se a empresa existir 
                if (emp != null)
                {
                    // mensagem de erro 
                    TratamentoDeErros(4);
                    return RedirectToAction("Index", empresa);
                }
                else if (await ConsultarEmpresaAPI(empresa.Cnpj) != null)
                {
                    CadastrarEmpresa(empresa);
                    return RedirectToAction("Index", "Login");
                }
                //se der qualquer outro erro
                else
                {
                    TratamentoDeErros(7);
                    return RedirectToAction("Index", "Login");
                }
            }

            // se também for cadastrar o usuario
            else if (tp == 2)
            {
                Usuario user = new Usuario();
                //le as infos do usuario
                user = LeSessionInfosUsuario();
                // Se a empresa existir no banco de dados
                if (emp == null)
                {
                    // se a empresa não existir da erro
                    if (await ConsultarEmpresaAPI(empresa.Cnpj) == null)
                    {
                        TratamentoDeErros(5);
                        return RedirectToAction("Index",empresa);
                    }
                    // cadastra a empresa no banco de dados 
                    user.fk_Id_Empresa = await CadastrarEmpresa(empresa);
                }

                else user.fk_Id_Empresa = emp.Id;

                // cadastra endereco
                user.fk_Id_Endereco = await CadastrarEndereco(user);

                // Caso a ação de cadastrar de errado 
                if (!await CadastrarUsuario(user))
                {
                    TratamentoDeErros(6);
                    return RedirectToAction("Index", empresa);
                }

            }
            return RedirectToAction("Index", "Loguin");
        }
        private async Task<int?> CadastrarEndereco(Usuario user)
        {
            //Se o endereco já existir no banco de dados
            if (user.fk_Id_Endereco != null)
            {
                return user.fk_Id_Endereco;
            }
            //Se não existir salva no bd e rtorna o id e depois cadastra o usuario
            else
            {
                return await _enderecoDAL.Cadastrar(user.Endereco);
            }

        }
        private async Task<bool> CadastrarUsuario(Usuario user)
        {
            return await _usuarioDAL.Cadastrar(user);
        }
        private async Task<int> CadastrarEmpresa(Empresa empresa)
        {
            return await _EmpresaDAL.Cadastrar(empresa);
        }
        private async Task<Empresa> CarregaTabelas()
        {
            Empresa empresa = new Empresa()
            {
                Ramo = new Ramo() { Ramos = await _ramoDAL.ConsultarTodos() },
                Setor = new Setor() { Setores = await _SetorDAl.ConsultarTodos() }
            };
            return empresa;
        }
        private Usuario LeSessionInfosUsuario()
        {
            // return new SerializadorJson().DesserializaUsuario(codifica.Descriptografar(HttpContext.Session.GetString("InfosUsuarioCadastro") as string, HttpContext.Session.GetString("Json") as string));
            return new SerializadorJson().DesserializaUsuario(codifica.Descriptografar(HttpContext.Session.GetString(""), HttpContext.Session.GetString("")));
        }
        [HttpPost]
        private async Task<Empresa> ConsultarEmpresa(string cnpj)
        {
            Empresa empresa = await ConsultarEmpresaDAL(cnpj);
            if (empresa != null)
                return empresa;
            empresa = await ConsultarEmpresaAPI(cnpj);
            if (empresa != null)
                return empresa;
            return null;
        }
        private bool VerificaCampos(Empresa empresa)
        {
            if (empresa.fk_Id_Ramo == -1 || empresa.fk_Id_Setor == -1)
            {
                TratamentoDeErros(1);
                return false;
            }
            if (string.IsNullOrWhiteSpace(empresa.Nome))
            {
                TratamentoDeErros(2);
                return false;
            }
            if (string.IsNullOrWhiteSpace(empresa.Cnpj))
            {
                TratamentoDeErros(3);
                return false;
            }
            return true;
        }
        //talvez fazer um switch case depois
        private void TratamentoDeErros(int erro)
        {
            //Formato do email inválido 
            if (erro == 1)
            {
                TempData["Mensagem"] = "Selecione ramo e setor da empresa.";
                TempData["Erro"] = 1;
                TempData.Save();
                return;
            }
            if (erro == 2)
            {
                TempData["Mensagem"] = "Coloque o nome da empresa.";
                TempData["Erro"] = 2;
                TempData.Save();
                return;
            }
            if (erro == 3)
            {
                TempData["Mensagem"] = "Coloque o CNPJ da empresa";
                TempData["Erro"] = 3;
                TempData.Save();
                return;
            }
            if (erro == 4)
            {
                TempData["Mensagem"] = "Empresa já cadastrada";
                TempData["Erro"] = 4;
                TempData.Save();
                return;
            }
            if (erro == 5)
            {
                TempData["Mensagem"] = "Cnpj inválido";
                TempData["Erro"] = 5;
                TempData.Save();
                return;
            }
            if (erro == 6)
            {
                TempData["Mensagem"] = "Não foi possivel cadastrar o usuario";
                TempData["Erro"] = 6;
                TempData.Save();
                return;
            }
            if (erro == 7)
            {
                TempData["Mensagem"] = "Não foi possivel cadastrar a empresa";
                TempData["Erro"] = 7;
                TempData.Save();
                return;
            }

        }
        [HttpGet]
        public async Task<JsonResult> ConsultaEmpresaJs(Empresa empresa)
        {
            Empresa _empresa = await ConsultarEmpresa(empresa.Cnpj);
            if (_empresa != null)
                return Json(new
                {
                    success = true,
                    nome = _empresa.Nome,
                    ramo = _empresa.Ramo,
                    setor = _empresa.Setor

                });
            return Json(new { success = false });
        }
        private async Task<Empresa> ConsultarEmpresaDAL(string cnpj)
        {
            return await _EmpresaDAL.ConsultarPorCnpj(cnpj);
        }
        private async Task<Empresa> ConsultarEmpresaAPI(string cnpj)
        {
            return await _Apis.ReceitaFerderalConsultaPorCnpj(cnpj);
        }
        public async Task<JsonResult> ConsultarSetoresPorId(Empresa empresa)
        {
            if (empresa.fk_Id_Ramo != null)
            {
                var setores = await _SetorDAl.ConsultarPorIdRamo((int)empresa.fk_Id_Ramo);
                return Json(new { setores = setores });
            }
            return null;
        }
        public async Task<JsonResult> ConsultarSetorPorRamoJs(Empresa empresa)
        {
            int id = (int)empresa.fk_Id_Ramo;


            if (empresa.fk_Id_Ramo != -1)
            {
                empresa.Setor = await _SetorDAl.ConsultarPorIdRamo(id);
                // Criando uma nova DataTable para armazenar os setores filtrados
                DataTable novaTabela = empresa.Setor.Setores.Tables[0].Clone();

                // Iterando sobre as linhas do DataSet original
                foreach (DataRow row in empresa.Setor.Setores.Tables[0].Rows)
                {
                    // Verificando se o valor na coluna "fk_Id_Ramo" é igual à variável de comparação
                    if (Convert.ToInt32(row["fk_Id_Ramo"]) == id)
                    {
                        // Adicionando a linha à nova DataTable
                        novaTabela.ImportRow(row);
                    }
                }
                // Substituindo a tabela original pela nova tabela filtrada
                empresa.Setor.Setores.Tables.Clear();
                empresa.Setor.Setores.Tables.Add(novaTabela);
            }
            else
            {
                if (empresa.Setor == null)
                    empresa.Setor = InicializaPOCO.Setor(empresa.Setor);
                empresa.Setor.Setores = await _SetorDAl.ConsultarTodos();
            }
            return Json(new { setores = empresa.Setor.Setores });
        }
    }
}

