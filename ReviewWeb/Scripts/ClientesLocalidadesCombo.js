$(document).ready(function () {
    $("#idclientes").change(function () {
        var value = $("#idclientes option:selected").val();

        if (value !== "" || value !== undefined) {
            ListarLocais(value);
        }
    })

    var value2 = $("#idclientes option:selected").val();

    if (value2 !== "" || value2 !== undefined) {
        ListarLocais(value2);
    }
});

function ListarLocais(value) {
    var url = "/ReviewWeb/Contratos/LocaisList";
    //var url = "../LocaisList";
    var data = { idclientes: value, idcontratos: $("#IdContratos").val() }

    $("#idclientes_localidades").empty();

    $.ajax({
        url: url,
        type: "GET",
        datatype: "json",
        data: data
    }).done(function (data) {
        if (data.Resultado.length > 0) {
            var dadosGrid = data.Resultado;

            $("#idclientes_localidades").append('<option value="">--Selecione--</option>');

            $.each(dadosGrid, function (indice, item) {
                var opt = "";

                if (item["Selected"] == true) {
                    opt = '<option value="' + item["Value"] + '" selected="selected">' + item["Text"] + '</option>';
                }
                else {
                    opt = '<option value="' + item["Value"] + '">' + item["Text"] + '</option>';
                }

                $("#idclientes_localidades").append(opt);
            })
        }
    })
}