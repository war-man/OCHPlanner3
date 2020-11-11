$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    var oilOffsetXSlider = $("#oilOffsetX").slider();
   // oilOffsetXSlider.slider('setValue', 5);

    $("#oilOffsetX").on("slide", function (slideEvt) {
        $("#oilOffsetXSliderVal").text(slideEvt.value);
    });

    var oilOffsetYSlider = $("#oilOffsetY").slider();
    $("#oilOffsetY").on("slide", function (slideEvt) {
        $("#oilOffsetYSliderVal").text(slideEvt.value);
    });

    InitPage();

    $(document).on("click", '.list-group-item', function () {
        $(".list-group-item").removeClass("active");
        $(this).addClass("active");
    });

    $(document).on("click", '#btnSave', function () {
        var selectedPrinter = $('a.list-group-item.list-group-item-action.active')[0].innerText

        var printerConfig = {
            SelectedOilPrinter: selectedPrinter,
            OilOffsetX: oilOffsetXSlider.slider('getValue'),
            OilOffsetY: oilOffsetYSlider.slider('getValue')
        };

        $.ajax({
            traditional: true,
            url: ajaxUrl + '/options/printer/save',
            type: 'POST',
            data: printerConfig,
            success: function (response) {

                Swal.fire({
                    icon: 'success',
                    title: 'Sauvegardé avec succès!',
                    showCancelButton: false,
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true,
                   
                });
            }
        });
    });

    function InitPage() {
        qz.websocket.connect().then(function () {
            console.log("Connected!");
            displayAllPrinters();
        });

        oilOffsetXSlider.slider('setValue', $('#HidOilOffsetX').val());
        $("#oilOffsetXSliderVal").text($('#HidOilOffsetX').val());
        oilOffsetYSlider.slider('setValue', $('#HidOilOffsetY').val());
        $("#oilOffsetYSliderVal").text($('#HidOilOffsetY').val());

    }

    function displayAllPrinters(printers) {
        var printers = findPrinters();
    }

    function findPrinters() {
        //get active printer
        var activeOilPrinter = $('#HidSelectedOilPrinter').val();

        //list all printers
        qz.printers.find().then(function (data) {
            var list = '';
            for (var i = 0; i < data.length; i++) {
                if (data[i] === activeOilPrinter) {
                    list += "<a href=\"#\" class=\"list-group-item list-group-item-action active\">" + data[i] + "</a>";
                }
                else {
                    list += "<a href=\"#\" class=\"list-group-item list-group-item-action\">" + data[i] + "</a>";
                }
            }
            $('#printer-list').html(list);
        }).catch(displayError);
    }

    function displayError(err) {
        console.error(err);
        displayMessage(err, 'alert-danger');
    }


});