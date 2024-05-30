function VerificaCell(celular) {
    if (validarFormatoCelular(celular)) {
        $.ajax({
            url: '/CadastroUsuario/ConsultaCelularJs',
            type: 'GET',
            data: { 'Celular': celular },
            success: function (data) {
                if (data.existe) {
                    document.getElementById('SpanCell').textContent = 'Celular já cadastrado';
                    var inputCelular = document.getElementById("inputCelular");
                    inputCelular.style.backgroundColor = "rgba(255, 192, 203, 0.5)";
                } else {
                    inputCelular.style.backgroundColor = "white";
                }
            },
            error: function (error) {
                console.error('Erro na consulta de celular:', error);
                // Exibir mensagem de erro para o usuário ou registrar em log
            }
        });
    }
    else {
        inputCelular.style.backgroundColor = "white";
    }
}

function validarFormatoCelular(celular) {
    // Expressão regular para validar o formato do celular (exemplo)
    const regexCelular = /^\d{11}$/;
    return regexCelular.test(celular);
}

function ConsultaCep(cep) {
    if (validateCEP(cep)) {
        $.ajax({
            url: '/CadastroUsuario/ConsultarCepJs',
            type: 'GET',
            data: { 'Endereco.Cep': cep },
            success: function (data) {
                if (data.success) {
                    var rua = document.getElementById('inputRua');
                    var bairro = document.getElementById('inputBairro');
                    var cidade = document.getElementById('inputCidade');
                    var uf = document.getElementById('inputEstado');
                    rua.value = data.rua;
                    bairro.value = data.bairro;
                    cidade.value = data.cidade;
                    uf.value = data.uf;
                }
                else {
                    var rua = document.getElementById('inputRua');
                    var bairro = document.getElementById('inputBairro');
                    var cidade = document.getElementById('inputCidade');
                    var uf = document.getElementById('inputEstado');
                    rua.value = "";
                    bairro.value = "";
                    cidade.value = "";
                    uf.value = "";
                }
            }
        });
    }
}

function validateCEP(cep) {
    const allowedChars = /[0-9]/;
    let cepValue = cep;
    let cleanedCEP = '';
    for (let i = 0; i < cepValue.length; i++) {
        if (allowedChars.test(cepValue[i])) {
            cleanedCEP += cepValue[i];
        }
    }
    cleanedCEP = cleanedCEP.trim(); // Remove espaços em branco do início e do fim
    if (cleanedCEP.length === 8) {
        cep.value = cleanedCEP;
        return true;
    } else {
        return false;
    }
}

function VerificaSenhas() {
    var conf_Senha = document.getElementById("inputConf_Senha").value;
    var senha = document.getElementById("inputSenha").value;

    // Verifica se ambas as senhas estão preenchidas antes de continuar
    if (conf_Senha !== "" && senha !== "") {
        if (senha !== conf_Senha) {
            var inputSenha = document.getElementById("inputSenha");
            var inputConfSenha = document.getElementById("inputConf_Senha");

            // Define a cor de fundo personalizada com RGBA para uma cor mais clara e transparente
            inputSenha.style.backgroundColor = "rgba(255, 192, 203, 0.5)";
            inputConfSenha.style.backgroundColor = "rgba(255, 192, 203, 0.5)";

            var spanSenha = document.getElementById("SpanSenha");
            var spanConf_Senha = document.getElementById("SpanConf_Senha");
            spanConf_Senha.innerText = "Senhas não compatíveis";
        } else {
            // Se as senhas são iguais, limpa o span de aviso e a cor de fundo
            var inputSenha = document.getElementById("inputSenha");
            var inputConfSenha = document.getElementById("inputConf_Senha");

            inputSenha.style.backgroundColor = "";
            inputConfSenha.style.backgroundColor = "";

            var spanConf_Senha = document.getElementById("SpanConf_Senha");
            spanConf_Senha.innerText = "";
        }
    } else {
        // Se uma ou ambas as senhas estão vazias, limpa o span de aviso e a cor de fundo
        var inputSenha = document.getElementById("inputSenha");
        var inputConfSenha = document.getElementById("inputConf_Senha");

        inputSenha.style.backgroundColor = "";
        inputConfSenha.style.backgroundColor = "";

        var spanConf_Senha = document.getElementById("SpanConf_Senha");
        spanConf_Senha.innerText = "";
    }
}


