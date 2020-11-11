$(document).ready(function () {

    var ajaxUrl = $('#HidRootUrl').val();

    moment.locale('fr');

    var nextunit = 'PROCH. KM';
    var nextdate = 'PROCH. DATE';

    qz.websocket.connect().then(function () {
        console.log("Connected!");
    });

    //Warning if no printer selected
    if ($('#HidSelectedOilPrinter').val() === '') {
        Swal.fire({
            icon: 'error',
            title: 'Imprimante non défini',
            text: 'Aucune configuration d\'imprimante trouvée',
            showConfirmButton: true,
            allowOutsideClick: false
        }).then((result) => {
            if (result.isConfirmed) {
                location.href = ajaxUrl + "/Options/Printer"
            }
        })
    }

    //Initial Setup
    InitialSetup();

    $(document).on("click", "#btnPrint", function () {
        printSimpleSticker();
    });

    $(document).on("change", 'input[name="SelectedUnit"]', function () {
        if ($(this).val() === 'KM') {
            UpdateKM();
            refreshIntervalSelectList(1);
        }
        else if ($(this).val() === 'MI') {
            UpdateMiles();
            refreshIntervalSelectList(2);
        }
        else if ($(this).val() === 'HM') {
            UpdateHM();
            refreshIntervalSelectList(3);
        }
    });

    //replication for oil list value
    $(document).on("change", 'select[name="oillist"]', function () {
        $('select[name="oillist-preview"]').val($('select[name="oillist"]').val());
    });

    //replication for comment
    $(document).on("keyup", "#Comment", function () {
        $('input[name="comment-preview"]').val($('input[name="comment"]').val());
    });

    //replication for unitvalue
    $(document).on("blur", "#UnitValue", function () {
        if ($("input[name='PrintChoices']:checked").val() === 'Choice1') {
            Choice1_Click();
        }
        if ($("input[name='PrintChoices']:checked").val() === 'Choice2') {
            Choice2_Click();
        }
        if ($("input[name='PrintChoices']:checked").val() === 'Choice3') {
            Choice3_Click();
        }  
        
    });

    //CHOICE 1
    $(document).on("click", "#PrintChoice1", function () {
        Choice1_Click();
    });

    //replication for selected Period
    $(document).on("change", 'select[name="SelectedPeriodChoice1"]', function () {
        UpdateMonthChoice1();
    });

    //replication for selected mileage
    $(document).on("change", 'select[name="Choice1SelectedMileage"]', function () {
        UpdateMileageChoice1()
    });

    function Choice1_Click() {
        nextdate = 'PROCH. DATE';

        UpdateMonthChoice1();
        UpdateMileageChoice1();
    }

    function UpdateMonthChoice1() {
        var month = parseInt($('select[name="SelectedPeriodChoice1"] option:selected').text());
        $('#datebox-preview').val(moment().add(month, 'M').format('' + $('#hidDateFormat').val().toUpperCase() + ''));
        $('#label-datebox-preview').html('Prochaine date ou avant le <span class="ml-3 small font-weight-bold">(' + PrintableDateFormat() + ')</span>');
    }

    function UpdateMileageChoice1() {
        var startMileage = $('input[name="unitvalue"]').val();
        if (startMileage === '') {
            startMileage = 0;
        }

        var mileage = $('select[name="Choice1SelectedMileage"] option:selected').text();
        $('input[name="unitvalue-preview"]').val(parseInt(startMileage) + parseInt(mileage));
    }

    //CHOICE 2
    $(document).on("click", "#PrintChoice2", function () {
        Choice2_Click();
    });

    //replication for selected Mileage
    $(document).on("change", 'select[name="Choice2SelectedMileage"]', function () {
        UpdateMileageChoice2();
    });

    function Choice2_Click() {
        $('#label-datebox-preview').html('Entretien effectué le <span class="ml-3 small font-weight-bold">(' + PrintableDateFormat() + ')</span>');
        $('#datebox-preview').val(moment().format('' + $('#hidDateFormat').val().toUpperCase() + ''));
        nextdate = 'EFFECTUE LE';
        UpdateMileageChoice2();
    }

    function UpdateMileageChoice2() {
        var startMileage = $('input[name="unitvalue"]').val();
        if (startMileage === '') {
            startMileage = 0;
        }

        var mileage = $('select[name="Choice2SelectedMileage"] option:selected').text();
        $('input[name="unitvalue-preview"]').val(parseInt(startMileage) + parseInt(mileage));
    }

    //CHOICE 3
    $(document).on("click", "#PrintChoice3", function () {
        Choice3_Click();
    });

    $(document).on("change", 'select[name="Choice3SelectedMonth"]', function () {
        Choice3UpdatePreview();
    });

    $(document).on("change", 'select[name="Choice3SelectedYear"]', function () {
        Choice3UpdatePreview();
    });

    function Choice3_Click() {
        $('#label-datebox-preview').text('Prochaine visite');
        nextdate = 'PROCH. VISITE';
        Choice3UpdatePreview();
        UpdateMileageChoice3();
    }

    //Update Date
    function Choice3UpdatePreview() {
        var month = parseInt($('select[name="Choice3SelectedMonth"] option:selected').val());
        var year = parseInt($('select[name="Choice3SelectedYear"] option:selected').val());

        $('#datebox-preview').val(moment(year + "/" + (month <= 9 ? '0' + month : month) + "/01").format("MMMYY").toUpperCase());
    }

    //replication for selected Mileage
    $(document).on("change", 'select[name="Choice3SelectedMileage"]', function () {
        UpdateMileageChoice3();
    });

    function UpdateMileageChoice3() {
        var startMileage = $('input[name="unitvalue"]').val();
        if (startMileage === '') {
            startMileage = 0;
        }

        var mileage = $('select[name="Choice3SelectedMileage"] option:selected').text();
        $('input[name="unitvalue-preview"]').val(parseInt(startMileage) + parseInt(mileage));
    }

    function InitialSetup() {
        if ($('input[name="SelectedUnit"]:checked').val() === 'KM') {
            UpdateKM();
        }
        if ($('input[name="SelectedUnit"]:checked').val() === 'MI') {
            UpdateMiles();
        }
        if ($('input[name="SelectedUnit"]:checked').val() === 'HM') {
            UpdateHM();
        }
        $('input[name="comment-preview"]').val($('input[name="comment"]').val());
        if ($("input[name='PrintChoices']:checked").val() === 'Choice1') {
            Choice1_Click();
        }
        if ($("input[name='PrintChoices']:checked").val() === 'Choice2') {
            Choice2_Click();
        }
        if ($("input[name='PrintChoices']:checked").val() === 'Choice3') {
            Choice3_Click();
        }  

    }

    //Reset Preview
    $(document).on("click", "#btnReset", function () {
        $('input[name="unitvalue"]').val('');
        InitialSetup();
    });

    //Save default values
    $(document).on("click", '#btnSave', function () {
        var defaultValue = {
            SelectedUnit: $('input[name="SelectedUnit"]:checked').val(),
            Comment: $('input[name="comment"]').val(),
            SelectedOil: $('select[name="oillist"]').val(),
            SelectedChoice: $("input[name='PrintChoices']:checked").val(),
            Choice1SelectedMonth: $('select[name="SelectedPeriodChoice1"] option:selected').val(),
            Choice1SelectedMileage: $('select[name="Choice1SelectedMileage"] option:selected').val(),
            Choice2SelectedMileage: $('select[name="Choice2SelectedMileage"] option:selected').val(),
            Choice3SelectedMonth: $('select[name="Choice3SelectedMonth"] option:selected').val(),
            Choice3SelectedYear: $('select[name="Choice3SelectedYear"] option:selected').val(),
            Choice3SelectedMileage: $('select[name="Choice3SelectedMileage"] option:selected').val()
        };

        $.ajax({
            traditional: true,
            url: ajaxUrl + '/simple/save',
            type: 'POST',
            data: defaultValue,
            success: function (response) {
                if (response == 1) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Sauvegardé avec succès!',
                        showCancelButton: false,
                        showConfirmButton: false,
                        timer: 2000,
                        timerProgressBar: true
                    });
                }
            }
        });  
    });

    function UpdateKM() {
        $('#label-unit').text('Kilomètres actuel');
        $('#label-unit-preview').text('Prochain Kilomètres');
        nextunit = 'PROCH. KM';
    }

    function UpdateMiles() {
        $('#label-unit').text('Miles actuel');
        $('#label-unit-preview').text('Prochain Miles');
        nextunit = 'PROCH. MILES';
    }

    function UpdateHM() {
        $('#label-unit').text('Heures moteur actuel');
        $('#label-unit-preview').text('Prochain Heures moteur');
        nextunit = 'PROCH. HR. MOTEUR';
    }

    function PrintableDateFormat() {
        var dateFormat = $('#hidDateFormat').val();
        var language = $('#hidLanguage').val().toUpperCase();
        if (language === "FR") {
            return dateFormat.replace('dd', 'j').replace('MM', 'm').replace('yy', 'a').toUpperCase();
        }
        else {
            return dateFormat.replace('dd', 'd').replace('MM', 'm').replace('yy', 'y').toUpperCase();
        }

    }

    function refreshIntervalSelectList(mileageType) {
        $.ajax({
            url: ajaxUrl + '/reference/intervalSelectList/' + mileageType,
            type: "GET",
            async: false,
            success: function (response) {
                $('select[name="Choice1SelectedMileage"]').empty();
                $('select[name="Choice2SelectedMileage"]').empty();
                $('select[name="Choice3SelectedMileage"]').empty();

                var options = '';
                options += '<option value="Select">-- Select --</option>';
                for (var i = 0; i < response.length; i++) {
                    options += '<option value="' + response[i].Id + '">' + response[i].Name + '</option>';
                }

                $('select[name="Choice1SelectedMileage"]').append(options);
                $('select[name="Choice2SelectedMileage"]').append(options);
                $('select[name="Choice3SelectedMileage"]').append(options);
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });
    }

    function printSimpleSticker() {
        var oilOffsetXSlider = parseInt($('#HidOilOffsetX').val());
        var oilOffsetYSlider = parseInt($('#HidOilOffsetY').val());

        var selectedPrinter = $('#HidSelectedOilPrinter').val();

        qz.printers.find(selectedPrinter).then(function (printer) {
            console.log("Printer: " + printer);

            var config = qz.configs.create(printer);       // Create a default config for the found printer
            var printData1;

            printData1 = [
                'N\n',
                'Q400\n',
                'q440\n',
                'D12\n',
                'A' + (90 + oilOffsetXSlider) + ',' + (157 + oilOffsetYSlider) + ',0,3,1,1,N,"' + $('#garage-name-print').val() + '"\n',
                'A' + (116 + oilOffsetXSlider) + ',' + (182 + oilOffsetYSlider) + ',0,3,1,1,N,"' + $('#garage-phone-print').val() + '"\n',
                'A' + (75 + oilOffsetXSlider) + ',' + (212 + oilOffsetYSlider) + ',0,3,1,1,N,"' + $('input[name="comment-preview"]').val() + '"\n',
                'A' + (75 + oilOffsetXSlider) + ',' + (242 + oilOffsetYSlider) + ',0,4,1,1,N,"' + $('select[name="oillist-preview"] option:selected').text() + '"\n',
                'A' + (75 + oilOffsetXSlider) + ', ' + (272 + oilOffsetYSlider) + ',0,4,1,1,N,"' + nextdate + '"\n',
            ];

            if ($('#hidDateFormatPrint').val() === "True" &&
                ($("input[name='PrintChoices']:checked").val() === 'Choice1') ||
                $("input[name='PrintChoices']:checked").val() === 'Choice2') {
                printData1.push('A' + (260 + oilOffsetXSlider) + ',' + (275 + oilOffsetYSlider) + ',0,1,1,1,N,"(' + PrintableDateFormat() + ')"\n');
            }

            var printData2 = [
                'A' + (75 + oilOffsetXSlider) + ',' + (302 + oilOffsetYSlider) + ',0,5,1,1,N,"' + $('input[name="datebox-preview"]').val().toUpperCase() + '"\n',
                'A' + (75 + oilOffsetXSlider) + ',' + (362 + oilOffsetYSlider) + ',0,4,1,1,N,"' + nextunit + '"\n',
                'A' + (75 + oilOffsetXSlider) + ',' + (387 + oilOffsetYSlider) + ',0,5,1,1,N,"' + $('input[name="unitvalue-preview"]').val() + '"\n',
                'P1,1\n'
            ];

            var printData = $.merge(printData1, printData2);

            return qz.print(config, printData);

        }).catch(function (e) { console.error(e); });


        //qz.websocket.connect().then(function () {
        //    return qz.printers.find($('#HidSelectedOilPrinter').val());             
        //}).then(function (printer) {
        //    var config = qz.configs.create(printer);      

        //    var printData1;

        //    printData1 = [
        //        'N\n',
        //        'Q400\n',
        //        'q440\n',
        //        'D12\n',
        //        'A' + (90 + oilOffsetXSlider) + ',' + (157 + oilOffsetYSlider) + ',0,3,1,1,N,"' + $('#garage-name-print').val() + '"\n',
        //        'A' + (116 + oilOffsetXSlider) + ',' + (182 + oilOffsetYSlider) + ',0,3,1,1,N,"' + $('#garage-phone-print').val() + '"\n',
        //        'A' + (75 + oilOffsetXSlider) + ',' + (212 + oilOffsetYSlider) + ',0,3,1,1,N,"' + $('input[name="comment-preview"]').val() + '"\n',
        //        'A' + (75 + oilOffsetXSlider) + ',' + (242 + oilOffsetYSlider) + ',0,4,1,1,N,"' + $('select[name="oillist-preview"] option:selected').text() + '"\n',
        //        'A' + (75 + oilOffsetXSlider) + ', ' + (272 + oilOffsetYSlider) + ',0,4,1,1,N,"' + nextdate + '"\n',
        //    ];

        //    if ($('#hidDateFormatPrint').val() === "True" &&
        //        ($("input[name='PrintChoices']:checked").val() === 'Choice1') ||
        //        $("input[name='PrintChoices']:checked").val() === 'Choice2') {
        //        printData1.push('A' + (260 + oilOffsetXSlider) + ',' + (275 + oilOffsetYSlider) + ',0,1,1,1,N,"(' + PrintableDateFormat() + ')"\n');
        //    }

        //    var printData2 = [
        //        'A' + (75 + oilOffsetXSlider) + ',' + (302 + oilOffsetYSlider) + ',0,5,1,1,N,"' + $('input[name="datebox-preview"]').val().toUpperCase() + '"\n',
        //        'A' + (75 + oilOffsetXSlider) + ',' + (362 + oilOffsetYSlider) + ',0,4,1,1,N,"' + nextunit + '"\n',
        //        'A' + (75 + oilOffsetXSlider) + ',' + (387 + oilOffsetYSlider) + ',0,5,1,1,N,"' + $('input[name="unitvalue-preview"]').val() + '"\n',
        //        'P1,1\n'
        //    ];

        //    var printData = $.merge(printData1, printData2);

        //    return qz.print(config, printData);
        //}).catch(function (e) { console.error(e); });
    }
});