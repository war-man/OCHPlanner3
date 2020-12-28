$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    var oilOffsetXSlider = $("#oilOffsetX").slider();
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
            OilOffsetY: oilOffsetYSlider.slider('getValue'),
            RotationSelected: $('input[name="rotation-active"]')[0].checked
        };

        $.ajax({
            traditional: true,
            url: ajaxUrl + '/options/printer/save',
            type: 'POST',
            data: printerConfig,
            success: function (response) {

                Swal.fire({
                    icon: 'success',
                    title: $('#HidSaveSuccess').val(),
                    showCancelButton: false,
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true,
                   
                });
            }
        });
    });

    $(document).on("click", '#btnTestOffset', function () {

        var selectedPrinter = $('a.list-group-item.list-group-item-action.active')[0].innerText;

        qz.printers.find(selectedPrinter).then(function (printer) {
            console.log("Printer: " + printer);

            var config = qz.configs.create(printer);       // Create a default config for the found printer
            var data = [
                'N\n',
                'Q400\n',
                'q440\n',
                'D12\n',
                'A' + (140 + oilOffsetXSlider.slider('getValue')) + ',' + (157 + oilOffsetYSlider.slider('getValue')) + ',0,3,1,1,N,"Test Garage"\n',
                'A' + (120 + oilOffsetXSlider.slider('getValue')) + ',' + (182 + oilOffsetYSlider.slider('getValue')) + ',0,3,1,1,N,"(514) 555-5555"\n',
                'A' + (75 + oilOffsetXSlider.slider('getValue')) + ',' + (212 + oilOffsetYSlider.slider('getValue')) + ',0,3,1,1,N,"Comment"\n',
                'A' + (75 + oilOffsetXSlider.slider('getValue')) + ',' + (242 + oilOffsetYSlider.slider('getValue')) + ',0,4,1,1,N,"10W30"\n',
                'A' + (75 + oilOffsetXSlider.slider('getValue')) + ',' + (272 + oilOffsetYSlider.slider('getValue')) + ',0,4,1,1,N,"PROCH. DATE"\n',
                'A' + (75 + oilOffsetXSlider.slider('getValue')) + ',' + (302 + oilOffsetYSlider.slider('getValue')) + ',0,5,1,1,N,"MAI 21"\n',
                'A' + (75 + oilOffsetXSlider.slider('getValue')) + ',' + (362 + oilOffsetYSlider.slider('getValue')) + ',0,4,1,1,N,"PROCH. KM"\n',
                'A' + (75 + oilOffsetXSlider.slider('getValue')) + ', ' + (387 + oilOffsetYSlider.slider('getValue')) + ',0,5,1,1,N,"125000"\n',
                'P1,1\n'
            ];
            return qz.print(config, data);

        }).catch(function (e) { console.error(e); });
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