function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function MascaraCNPJ(cnpj) {
    if (mascaraInteiro(cnpj) === false) {
        event.returnValue = false;
    }
    return formataCampo(cnpj, '00.000.000/0000-00', event);
}

//adiciona mascara de cep
function MascaraCep(cep) {
    if (mascaraInteiro(cep) === false) {
        event.returnValue = false;
    }
    return formataCampo(cep, '00.000-000', event);
}

//adiciona mascara de data
function mascaraData(val) {
    var pass = val.value;
    var expr = /[0123456789]/;

    for (i = 0; i < pass.length; i++) {
        // charAt -> retorna o caractere posicionado no índice especificado
        var lchar = val.value.charAt(i);
        var nchar = val.value.charAt(i + 1);

        if (i === 0) {
            // search -> retorna um valor inteiro, indicando a posição do inicio da primeira
            // ocorrência de expReg dentro de instStr. Se nenhuma ocorrencia for encontrada o método retornara -1
            // instStr.search(expReg);
            if ((lchar.search(expr) !== 0) || (lchar > 3)) {
                val.value = "";
            }

        } else if (i === 1) {

            if (lchar.search(expr) !== 0) {
                // substring(indice1,indice2)
                // indice1, indice2 -> será usado para delimitar a string
                var tst1 = val.value.substring(0, (i));
                val.value = tst1;
                continue;
            }

            if ((nchar !== '/') && (nchar !== '')) {
                var tst1 = val.value.substring(0, (i) + 1);

                if (nchar.search(expr) !== 0)
                    var tst2 = val.value.substring(i + 2, pass.length);
                else
                    var tst2 = val.value.substring(i + 1, pass.length);

                val.value = tst1 + '/' + tst2;
            }

        } else if (i === 4) {

            if (lchar.search(expr) !== 0) {
                var tst1 = val.value.substring(0, (i));
                val.value = tst1;
                continue;
            }

            if ((nchar !== '/') && (nchar !== '')) {
                var tst1 = val.value.substring(0, (i) + 1);

                if (nchar.search(expr) !== 0)
                    var tst2 = val.value.substring(i + 2, pass.length);
                else
                    var tst2 = val.value.substring(i + 1, pass.length);

                val.value = tst1 + '/' + tst2;
            }
        }

        if (i >= 6) {
            if (lchar.search(expr) !== 0) {
                var tst1 = val.value.substring(0, (i));
                val.value = tst1;
            }
        }
    }

    if (pass.length > 10)
        val.value = val.value.substring(0, 10);
    return true;
}

//adiciona mascara ao telefone
function MascaraTelefone(tel) {
    if (mascaraInteiro(tel) === false) {
        event.returnValue = false;
        alert('Caractere invalido.');
    }
    return formataCampo(tel, '0000-0000', event);
}

//adiciona mascara ao CPF
function MascaraCPF(cpf) {
    if (mascaraInteiro(cpf) === false) {
        event.returnValue = false;
    }

    return formataCampo(cpf, '000.000.000-00', event);
}

//valida telefone
function ValidaTelefone(tel) {
    exp = /\d{4}\-\d{4}/;
    if (tel.value !== '') {
        if (!exp.test(tel.value)) {
            alert('Numero de Telefone Invalido!');
            document.getElementById('tel').focus();
        }
    }
    if (tel.value.length > 8) {
        alert('Telefone fixo possuem 8 digitos.');
        document.getElementById('tel').focus();
    }
}

//valida CEP
function ValidaCep(cep) {
    exp = /\d{2}\.\d{3}\-\d{3}/;
    if (!exp.test(cep.value))
        alert('Numero de Cep Invalido!');
}

//valida data
function ValidaData(data) {
    exp = /\d{2}\/\d{2}\/\d{4}/;
    if (!exp.test(data.value))
        alert('Data Invalida!');
}

function ComparaDatas(DataIni, DataFim) {
    var DtIni = document.getElementById(DataIni);
    var DtFim = document.getElementById(DataFim);

    var inicio = DtIni.value;
    var fim = DtFim.value;
    if (inicio.length != 10 || fim.length != 10) return false;

    if (gerarData(fim) >= gerarData(inicio)) return true;
    $("#errmsg").html("A data inicial não pode ser menor do que a data final").show().fadeOut(8000, "swing");
    DtIni.value = "";
    DtFim.value = "";

    return false;
}
function gerarData(str) {
    var partes = str.split("/");
    return new Date(partes[2], partes[1] - 1, partes[0]);
}


function DataValida(campo, valor) {
    var date = valor;
    var ardt = new Array;
    var ExpReg = new RegExp("(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}");
    ardt = date.split("/");
    erro = false;
    if (campo.value === "")
    {
        return true;
    }
    if (date.search(ExpReg) == -1) {
        erro = true;
    }
    else if (((ardt[1] == 4) || (ardt[1] == 6) || (ardt[1] == 9) || (ardt[1] == 11)) && (ardt[0] > 30))
        erro = true;
    else if (ardt[1] == 2) {
        if ((ardt[0] > 28) && ((ardt[2] % 4) != 0))
            erro = true;
        if ((ardt[0] > 29) && ((ardt[2] % 4) == 0))
            erro = true;
    }
    if (erro) {
        $("#errmsg").html("Data Inválida").show().fadeOut(8000, "swing");
        
        setTimeout(function () { campo.focus() }, 10);
        campo.value = "";
        return false;
    }
    return true;
}

function ValidarCPF(Objcpf)
{
    var cpf = Objcpf.value;
    cpf = cpf.toString().replace(/[^\d]+/g, '');
    if(cpf == '') return false; // Elimina CPFs invalidos conhecidos    
    if (cpf.length != 11 ||
        cpf == "00000000000" ||
        cpf == "11111111111" ||
        cpf == "22222222222" ||
        cpf == "33333333333" ||
        cpf == "44444444444" ||
        cpf == "55555555555" ||
        cpf == "66666666666" ||
        cpf == "77777777777" ||
        cpf == "88888888888" ||
        cpf == "99999999999")
    {
        alert('CPF inválido');
        return false;       // Valida 1o digito
    };
    add = 0;    
    for (i=0; i < 9; i ++)       
        add += parseInt(cpf.charAt(i)) * (10 - i);  rev = 11 - (add % 11);  
        if (rev == 10 || rev == 11)     rev = 0;    
        if (rev != parseInt(cpf.charAt(9))) {
            alert('CPF inválido');
            return false;       // Valida 2o digito 
        }
        add = 0; for (i = 0; i < 10; i++)
            add += parseInt(cpf.charAt(i)) * (11 - i);
        rev = 11 - (add % 11);
        if (rev == 10 || rev == 11)
            rev = 0;
        if (rev != parseInt(cpf.charAt(10))) {
            alert('CPF inválido');
            return false;
        }
        return true;
    }



//valida numero inteiro com mascara
function mascaraInteiro() {
    if (event.keyCode < 48 || event.keyCode > 57) {
        event.returnValue = false;
        return false;
    }
    return true;
}

//valida o CNPJ digitado
function ValidarCNPJ(ObjCnpj) {
    var cnpj = ObjCnpj.value;
    var valida = new Array(6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2);
    var dig1 = new Number;
    var dig2 = new Number;

    exp = /\.|\-|\//g
    cnpj = cnpj.toString().replace(exp, "");
    var digito = new Number(eval(cnpj.charAt(12) + cnpj.charAt(13)));

    for (i = 0; i < valida.length; i++) {
        dig1 += (i > 0 ? (cnpj.charAt(i - 1) * valida[i]) : 0);
        dig2 += cnpj.charAt(i) * valida[i];
    }
    dig1 = (((dig1 % 11) < 2) ? 0 : (11 - (dig1 % 11)));
    dig2 = (((dig2 % 11) < 2) ? 0 : (11 - (dig2 % 11)));

    if (((dig1 * 10) + dig2) != digito)
        alert('CNPJ Invalido!');

}

//formata de forma generica os campos
function formataCampo(campo, Mascara, evento) {
    var boleanoMascara;

    var Digitato = evento.keyCode;
    exp = /\-|\.|\/|\(|\)| /g
    campoSoNumeros = campo.value.toString().replace(exp, "");

    var posicaoCampo = 0;
    var NovoValorCampo = "";
    var TamanhoMascara = campoSoNumeros.length;;

    if (Digitato != 8) { // backspace 
        for (i = 0; i <= TamanhoMascara; i++) {
            boleanoMascara = ((Mascara.charAt(i) == "-") || (Mascara.charAt(i) == ".")
                                || (Mascara.charAt(i) == "/"))
            boleanoMascara = boleanoMascara || ((Mascara.charAt(i) == "(")
                                || (Mascara.charAt(i) == ")") || (Mascara.charAt(i) == " "))
            if (boleanoMascara) {
                NovoValorCampo += Mascara.charAt(i);
                TamanhoMascara++;
            } else {
                NovoValorCampo += campoSoNumeros.charAt(posicaoCampo);
                posicaoCampo++;
            }
        }
        campo.value = NovoValorCampo;
        return true;
    } else {
        return true;
    }
}

function ApenasNumero(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    return (charCode >= 48 && charCode <= 57 || charCode < 20);
}


function Mascara_Hora(Hora, campo) {
    var hora01 = '';
    hora01 = hora01 + Hora;
    var Digitado = event.keyCode;    
     
    if (Digitado != 8) {
        
        if (hora01.length == 2) {
            hora01 = hora01 + ':';
            campo.value = hora01;
        }
    }
    if (hora01.length == 5) {
        Verifica_Hora(campo);
    }
}

function Verifica_Hora(campo) {
    hrs = (campo.value.substring(0, 2));
    min = (campo.value.substring(3, 5));
    estado = "";
    if ((hrs < 00) || (hrs > 23) || (min < 00) || (min > 59)) {
        estado = "errada";
    }

    if (campo.value == "") {
        estado = "errada";
    }
    if (estado == "errada") {
        alert("Hora invalida!");
        campo.focus();
    }
}
/* Javascript tratamento Dropdownlist CheckBox*/
var color = 'White';

function changeColor(obj, ChkList) {
    var rowObject = getParentRow(obj);
    var parentTable =
      document.getElementById(ChkList.ClientID);

    if (color == '') {
        color = getRowColor();
    }

    if (obj.checked) {
        rowObject.style.backgroundColor = '#A3B1D8';
    }
    else {
        rowObject.style.backgroundColor = color;
        color = 'White';
    }

    // private method
    function getRowColor() {
        if (rowObject.style.backgroundColor == 'White')
            return parentTable.style.backgroundColor;
        else return rowObject.style.backgroundColor;
    }
}

// This method returns the parent row of the object
function getParentRow(obj) {
    do {
        obj = obj.parentElement;
    }
    while (obj.tagName != "TR")
    return obj;
}

function TurnCheckBoixGridView(id, ChkList) {
    var frm = document.forms[0];

    for (i = 0; i < frm.elements.length; i++) {
        if (frm.elements[i].type == "checkbox" &&
            frm.elements[i].id.indexOf(ChkList.ClientID) == 0) {
            frm.elements[i].checked =
              document.getElementById(id).checked;
        }
    }
}

function SelectAll(id, ChkList) {
    var parentTable = document.getElementById(ChkList.ClientID);
    var color

    if (document.getElementById(id).checked) {
        color = '#A3B1D8'
    }
    else {
        color = 'White'
    }

    for (i = 0; i < parentTable.rows.length; i++) {
        parentTable.rows[i].style.backgroundColor = color;
    }
    TurnCheckBoixGridView(id, ChkList);
}


/* Fim DropdownList CheckBox*/