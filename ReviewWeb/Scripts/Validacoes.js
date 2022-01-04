// Aqui adicionamos os metodos respeitando os nomes gerado em nossa classe atributo
// os nomes devem estar em caixa baixa tanto na integração como aqui no JavaScript (Nome atributo, parametro)
$.validator.unobtrusive.adapters.addSingleVal('emailbrasil', 'emailrequerido');

//Aqui temos a funcao que Valida do lado do cliente o e-mail digitado utilizando uma expressão regular
$.validator.addMethod('emailbrasil',
function (value, element, emailrequerido) {

    // Retira espaços do final da literal
    value = value.trim();

    // Valida caso o valor não tenha sido preenchido
    if (value.length == 0) {
        if (emailrequerido.toString() == "True") {
            return false;
        }
        else {
            return true;
        }
    }

    // Atribui expressão
    var expReg = /^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*\s+<(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})>$|^(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})$/;

    // Valida Expressao
    var result = value.match(expReg);

    // Retorna false caso a expressao nao seja valida
    if (result != null) {
        return true;
    }

    return false;
});



// Adicionamos o método date para sobrepor o método padrão e atribuindo sempre um retorno verdadeiro para garantir 
// que o JQuery não vai tratar as data exibindo mensagens indesejadas. 
$.validator.addMethod('date',
   function (value, element) {
       return true;
   });

// Aqui adicionamos os métodos respeitando os nomes gerado em nossa classe atributo
// os nomes devem estar em caixa baixa tanto na integração como aqui no JavaScript (Nome atributo, parametro)
$.validator.unobtrusive.adapters.addSingleVal("databrasil", "datarequerida");

//Aqui temos a funcao que Valida do lado do cliente as datas digitadas utilizando uma expressão regular
$.validator.addMethod('databrasil',
function (value, element, datarequerida) {

    // Verificamos se o valor foi digitado
    if (value.length == 0) {
        // Se a data é requerida retorna erro senão retorna com sucesso
        if (datarequerida.toString() == 'True') {
            return false;
        }
        else {
            return true;
        }
    }

    /// Expressão Regular para tratar datas, dd/MM/yyyy, validando também ano Bisexto.
    var expReg = /^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$/;

    // Valida a expressão, se for compatível vai retornar validando o campo, caso contrario exibe a mensagem de erro informada no atributo;
    return value.match(expReg);
});