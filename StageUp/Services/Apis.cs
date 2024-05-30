using StageUp.Models.POCO;
using System.Text.Json;
using Newtonsoft.Json;
using StageUp.Models.Interfaces;
using System.Text.RegularExpressions;

namespace StageUp.Services
{
    public class Apis : IApis
    {
        public async Task< Endereco> ViaCepConsultarPorCep(string cep)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"https://viacep.com.br/ws/{cep}/json/").Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    dynamic data = JsonConvert.DeserializeObject(json);

                    // Verifica se o CEP foi encontrado
                    if (data.erro != null && (bool)data.erro)
                        return null;

                    // Preenche os dados do endereço
                    Endereco endereco = new Endereco()
                    {
                        Cep = cep,
                        Rua = data.logradouro,
                        Bairro = data.bairro,
                        Cidade = data.localidade,
                        Uf = data.uf
                    };
                    endereco = endereco;
                    var _coordenadas = GmapConsultarCoordenadasPorCep(endereco.Cep);
                    endereco.Latitude = _coordenadas.Value.Latitude.ToString();
                    endereco.Longitude = _coordenadas.Value.Longitude.ToString();
                    endereco = endereco;
                    return endereco;
                }
                return null;
            }
        }
        public (double Latitude, double Longitude)? GmapConsultarCoordenadasPorCep(string endereco)
        {
            using (var client = new HttpClient())
            {
                var encodedAddress = Uri.EscapeDataString(endereco);
                var response = client.GetAsync($"https://maps.googleapis.com/maps/api/geocode/json?address={encodedAddress}&key=AIzaSyBEaBuIuQK4bcqHM_PZCPivSfar1l_NUQY").Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    dynamic data = JsonConvert.DeserializeObject(json);
                    if (data.status == "OK" && data.results.Count > 0)
                    {
                        double latitude = data.results[0].geometry.location.lat;
                        double longitude = data.results[0].geometry.location.lng;
                        return (latitude, longitude);
                    }
                }
                return null;
            }
        }

        public async Task<Empresa> ReceitaFerderalConsultaPorCnpj(string cnpj)
        {
            try
            {
                Empresa empresa = new Empresa();
                using (var client = new HttpClient())
                {
                    cnpj = Regex.Replace(cnpj, @"[^\d]", "");
                    var response = client.GetAsync($"https://www.receitaws.com.br/v1/cnpj/{cnpj}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        // Aqui você pode adicionar lógica adicional para verificar o conteúdo da resposta, se necessário
                        string json = response.Content.ReadAsStringAsync().Result;
                        dynamic data = JsonConvert.DeserializeObject(json);
                        if (data.erro != null && (bool)data.erro)
                            return null;
                        if (empresa == null)
                            cnpj = data.cnpj;
                        empresa.Nome = data.fantasia;
                        if (string.IsNullOrWhiteSpace(empresa.Nome))
                            empresa.Nome = data.nome;

                        if (string.IsNullOrWhiteSpace(cnpj) || string.IsNullOrWhiteSpace(empresa.Nome))
                            return null;
                        return empresa;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (AggregateException)
            {
                // Trate exceções de rede ou erros de solicitação aqui
                return null;
            }
        }
    }
}
