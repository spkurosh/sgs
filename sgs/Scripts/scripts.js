document.getElementById("visibleOnSeccional").style.display = "none";

$("input[id='phone']").on("input", function () {
    $("input[id='phone']").val(destroyMask(this.value));
    this.value = createMask($("input[id='phone']").val());
});

function createMask(string) {
    console.log(string)
    return string.replace(/(\d{3})(\d{3})(\d{4})/, "$1-$2-$3");
};

function destroyMask(string) {
    console.log(string)
    return string.replace(/\D/g, '').substring(0,);
};

function onlyNumberKey(evt) {
    var ASCIICode = (evt.which) ? evt.which : evt.keyCode
    if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
        return false;
    return true;
};
$("#section").on('change', function () {
    GetDataSection($('#section').val());
});

function GetDataSection(id) {
    $.ajax({
        url: '/Component/GetDataSection/' + id,
        type: 'GET',
        datatype: 'JSON',
        success: function (data) {
            $("#distrito").val('');
            $("#municipio").val('');
            $("#colonia option").remove();
            $("#codigoPostal option").remove();
            if (data) {
                var valores = JSON.parse(data);
                $('#distrito').val(valores.distrito.nombre);
                $('#municipio').val(valores.municipio.nombre);

                $("#colonia").append($('<option></option>').val(0).html("Seleccionar..."));
                $("#codigoPostal").append($('<option></option>').val(0).html("Seleccionar..."));
                $.each(valores.colonias, function (i, item) {
                    $("#colonia").append($('<option></option>').val(item.id).html(item.nombre));
                });
                $.each(valores.codigoPostal, function (i, item) {
                    $("#codigoPostal").append($('<option></option>').val(item.id).html(item.nombre));
                });

            }
            else {
                alert("la sección ingresada es incorrecta.");
            }
        }
    });
};

$("#colonia").on("change", function () {
    $("#suburb").val($("#colonia :selected").val());
});

$("#codigoPostal").on("change", function () {
    $("#postalCode").val($("#codigoPostal :selected").val());
});

$(document).ready(function () {
    setTimeout(function () {
        $("#msg").fadeOut();
    }, 10000);
});


$("#email").on('change', function () {
    existsEmail($('#email').val());
});

function existsEmail(id) {
    $.post('/Component/ExistsEmail/', { id: id },
        function (returnedData) {
            if (returnedData) {
                document.getElementById("emailDuplicated").innerHTML = "Ya se registró ese Email";
                setTimeout(function () {
                    $("#email").val('');
                }, 1000);
                setTimeout(function () {
                    document.getElementById("emailDuplicated").innerHTML = '';
                }, 5000);
            }
        });
};

$("#VoterKey").on('change', function () {
    existsVoterKey($('#VoterKey').val());
});

function existsVoterKey(id) {
    $.post('/Component/ExistsVoterKey/', { id: id },
        function (returnedData) {
            if (returnedData) {
                document.getElementById("voterKeyDuplicated").innerHTML = "Ya se registró esa Clave de Elector";
                setTimeout(function () {
                    $("#VoterKey").val('');
                }, 1000);
                setTimeout(function () {
                    document.getElementById("voterKeyDuplicated").innerHTML = '';
                }, 5000);
            }
        });
};

$("#Role").on('change', function () {
    if ($('#Role').val() === "Seccional") {
        $('#visibleOnSeccional').show();
    }
    else {
        $('#visibleOnSeccional').hide();
    }
});

