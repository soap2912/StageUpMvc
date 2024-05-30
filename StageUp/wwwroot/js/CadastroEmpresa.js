const { ajax } = require("jquery");

function VerificaCnpj(cnpj) {
    cnpj = limparNumeros(cnpj);
    if (cnpj.length == 14) {
        $.ajax({
            url: '/CadastroEmpresa/ConsultaEmpresaJs',
            type: 'GET',
            data: { 'Cnpj': cnpj },
            success: function (data) {
                if (data.success) {
                    document.getElementById('inputNome').value = data.nome;
                    if (data.setor != null) {

                        document.getElementById('inputRamo').value = data.ramo
                        document.getElementById('inputSetor').value = data.setor
                        document.getElementById('inputRamo').readOnly = true;
                        document.getElementById('inputSetor').readOnly = true;
                        document.getElementById('inputNome').readOnly = true;
                    }
                } else {
                    inputCelular.style.backgroundColor = "white";
                }
            },
            error: function (error) {
                console.error('Erro na consulta de cnpj:', error);
                // Exibir mensagem de erro para o usuário ou registrar em log
            }
        });
    }
    else {
        document.getElementById('inputRamo').readOnly = false;
        document.getElementById('inputSetor').readOnly = false;
        document.getElementById('inputNome').readOnly = false;
    }
}
function limparNumeros(texto) {
    return texto.replace(/[^0-9]/g, "");
}

function ConsultaRamoSetor(ramo) {
    $.ajax({
        url: 'CadastroEmpresa/ConsultarSetorPorRamoJs',
        type: 'GET',
        data: { 'fk_Id_Ramo': ramo },
        success: function (data) {
            const dropdown = document.getElementById("inputSetor");
            dropdown.innerHTML = '';
            for (const item of data) {
                const option = document.createElement("option");
                option.value = item.value;
                option.textContent = item.text;
                dropdown.appendChild(option);
            }
        }
            
    });
}