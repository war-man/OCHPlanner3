$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    InitPage();

    $("#oilOffsetX").slider();
    $("#oilOffsetX").on("slide", function (slideEvt) {
        $("#oilOffsetXSliderVal").text(slideEvt.value);
    });

    $("#oilOffsetY").slider();
    $("#oilOffsetY").on("slide", function (slideEvt) {
        $("#oilOffsetYSliderVal").text(slideEvt.value);
    });

    $(document).on("click", '.list-group-item', function () {
        $(".list-group-item").removeClass("active");
        $(this).addClass("active");
    });

    function InitPage() {
        qz.websocket.connect().then(function () {
            console.log("Connected!");
            displayAllPrinters();
        });

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