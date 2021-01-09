$(document).ready(function () {

    var ajaxUrl = $('#HidRootUrl').val();

    $('select[name="carMakeList"]').select2();

    if ($('#hidLanguage').val() === 'FR') {
        moment.locale('fr');
    }
    else {
        moment.locale('en');
    }

    $('.date').datetimepicker({
        format: $('#hidDateFormat').val().toUpperCase(),
        defaultDate: moment()
    });

    $('.time').datetimepicker({
        format: 'HH:mm'
    });

    //calculate garage name center position
    centerTitle($('#garage-name-print').val());

    var nextunit = $('#HidNextUnitKm').val();
    var nextdate = $('#HidNextDate1').val();

    qz.websocket.connect().then(function () {
        console.log("Connected!");
    });

    //Detect ENTER key
    $(document).on("keypress", "input", function (e) {
        if (e.which == 13) {
            var inputVal = $(this).val();
            $('#btnPrint').focus();
        }
    });
        
    $(document).on("click", "#btnPrint", function () {
        //Warning if no printer selected
        if ($('#HidSelectedOilPrinter').val() === '') {
            Swal.fire({
                icon: 'error',
                title: $('#HidPrinterNotDefined').val(),
                text: $('#HidPrinterNotDefinedText').val(),
                showConfirmButton: true,
                allowOutsideClick: false
            }).then((result) => {
                if (result.isConfirmed) {
                    location.href = ajaxUrl + "/Options/Printer"
                }
            })
        }
        else {
            printSticker();
        }
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
        if ($("input[name='PrintChoices']:checked").val() === 'Choice4') {
            Choice4_Click();
        }
    });

    //CHOICE 1
    $(document).on("click", "#PrintChoice1", function () {
        Choice1_Click();
    });

    //Recommendation value to display
    $(document).on("change", 'select[name="SelectedRecommendationId"]', function () {

        $.ajax({
            url: ajaxUrl + '/Message/RecommendationMessageToDisplay',
            type: "GET",
            data: {
                recommendationId: $(this).val()
            },
            async: false,
            success: function (response) {
                $('#HidRecommandationValueToDisplay').val(response);
                $('#comment-preview').val(response);
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });

        //check choice 1
        selectChoice(1);
    });

    $(document).on("change", 'select[name="Choice1SelectedMileage"]', function () {
        UpdateMileageChoice1();

        //check choice 1
        selectChoice(1);
    });

    function Choice1_Click() {
        $('#choice4-preview').hide();
        $('#choice123-preview').show();

        nextdate = $('#HidNextDate1').val();

        UpdateDateChoice1();
        UpdateMileageChoice1();
        UpdateLabelUnitPreview();
        UpdateNextUnit();
    }

    $("#datetimepicker1").on("change.datetimepicker", ({ date, oldDate }) => {
        $('#datebox-preview').val(moment(date._d).format('' + $('#hidDateFormat').val().toUpperCase() + ''));
        //check choice 1
        selectChoice(1);
    })

    function UpdateDateChoice1() {
        var selectedDate = $('#datetimepicker1').datetimepicker('viewDate');
        $('#datebox-preview').val(moment(selectedDate).format('' + $('#hidDateFormat').val().toUpperCase() + ''));
        $('#label-datebox-preview').html($('#HidNextDateOrBefore').val() + '<span class="ml-3 small font-weight-bold">(' + PrintableDateFormat() + ')</span>');
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

    $(document).on("change", 'select[name="SelectedMaintenanceId"]', function () {
        $('#comment-preview').val($('select[name="SelectedMaintenanceId"] option:selected').text());

        //check choice 2
        selectChoice(2);
    });

    $(document).on("blur", 'input[name="UnitValueChoice2"]', function () {
        UpdateMileageChoice2();

        //check choice 2
        selectChoice(2);
    });

    $("#datetimepicker2").on("change.datetimepicker", ({ date, oldDate }) => {
        $('#datebox-preview').val(moment(date._d).format('' + $('#hidDateFormat').val().toUpperCase() + ''));

        //check choice 2
        selectChoice(2);
    })

    function Choice2_Click() {
        $('#choice4-preview').hide();
        $('#choice123-preview').show();

        UpdateDateChoice2();
        UpdateLabelUnitPreview();
        nextdate = $('#HidNextDate2').val();
        UpdateMileageChoice2();
        UpdateNextUnit();
    }

    function UpdateDateChoice2() {
        var selectedDate = $('#datetimepicker2').datetimepicker('viewDate');
        $('#datebox-preview').val(moment(selectedDate).format('' + $('#hidDateFormat').val().toUpperCase() + ''));
        $('#label-datebox-preview').html($('#HidLastService').val() + '<span class="ml-3 small font-weight-bold">(' + PrintableDateFormat() + ')</span>');
    }

    function UpdateMileageChoice2() {
        var mileage = $('input[name="UnitValueChoice2"]').val();
        $('input[name="unitvalue-preview"]').val(parseInt(mileage));
    }

    //CHOICE 3
    $(document).on("click", "#PrintChoice3", function () {
        Choice3_Click();
    });

    $(document).on("change", 'select[name="Choice3SelectedMileage"]', function () {
        UpdateMileageChoice3();

        //check choice 3
        selectChoice(3);
    });

    $(document).on("change", 'select[name="SelectedAppointmentId"]', function () {
        $('#comment-preview').val($('select[name="SelectedAppointmentId"] option:selected').text());

        //check choice 3
        selectChoice(3);
    });

    $("#datetimepicker3").on("change.datetimepicker", ({ date, oldDate }) => {
        $('#datebox-preview').val(moment(date._d).format('' + $('#hidDateFormat').val().toUpperCase() + ''));

        //check choice 3
        selectChoice(3);
    })

    $("#datetimepicker31").on("change.datetimepicker", ({ date, oldDate }) => {
        $('input[name="unitvalue-preview"]').val(moment(date._d).format('HH:mm'));
    })

    function Choice3_Click() {
        $('#choice4-preview').hide();
        $('#choice123-preview').show();

        UpdateDateChoice3();
        UpdateLabelUnitPreview();
        nextdate = $('#HidNextAppointDate').val();
        UpdateNextUnit();
    }

    function UpdateDateChoice3() {
        var selectedDate = $('#datetimepicker3').datetimepicker('viewDate');
        $('#datebox-preview').val(moment(selectedDate).format('' + $('#hidDateFormat').val().toUpperCase() + ''));
        $('#label-datebox-preview').html($('#HidNextAppointDateLabel').val() + '<span class="ml-3 small font-weight-bold">(' + PrintableDateFormat() + ')</span>');
    }

    //CHOICE 4
    $(document).on("click", "#PrintChoice4", function () {
        Choice4_Click();
    });

    function Choice4_Click() {
        $('#choice123-preview').hide();
        $('#choice4-preview').show();

    }

    //replication for car make list value
    $(document).on("change", 'select[name="carMakeList"]', function () {
        // Get Models
        $.ajax({
            url: ajaxUrl + '/Message/ModelSelectList',
            type: "GET",
            data: {
                make: $(this).val()
            },
            async: false,
            success: function (response) {
                $('select[name="carModelList"]').empty();


                var options = '';
                options += '<option value="Select">-- Select --</option>';
                for (var i = 0; i < response.length; i++) {
                    options += '<option value="' + response[i].Text + '">' + response[i].Text + '</option>';
                }

                $('select[name="carModelList"]').append(options);
                $('select[name="carModelList"]').select2();
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });

        //Update Preview
        $('#make-preview').val($(this).val());

        //check choice 4
        selectChoice(4);

    });

    function UpdateLabelUnitPreview() {

        var choice = $("input[name='PrintChoices']:checked").val()
        if (choice == 'Choice1') {
            if ($('input[name="SelectedUnit"]:checked').val() == 'KM') {
                $('#label-unit-preview').text($('#HidNextKm').val());
            }
            else if ($('input[name="SelectedUnit"]:checked').val() == 'MI') {
                $('#label-unit-preview').text($('#HidNextMiles').val());
            }
            else if ($('input[name="SelectedUnit"]:checked').val() == 'HM') {
                $('#label-unit-preview').text($('#HidNextHm').val());
            }
        }
        else if (choice == 'Choice2') {
            $('#label-unit-preview').text($('#label-unit').text());
        }
        else if (choice == 'Choice3') {
            $('#label-unit-preview').text($('#HidNextAppointTimeLabel').val());
        }
        
    }

    function UpdateNextUnit() {

        var choice = $("input[name='PrintChoices']:checked").val()
        if (choice == 'Choice1') {
            if ($('input[name="SelectedUnit"]:checked').val() == 'KM') {
                nextunit = $('#HidNextUnitKm').val();
            }
            else if ($('input[name="SelectedUnit"]:checked').val() == 'MI') {
                nextunit = $('#HidNextUnitMiles').val();
            }
            else if ($('input[name="SelectedUnit"]:checked').val() == 'HM') {
                nextunit = $('#HidNextUnitHrMotor').val();
            }
        }
        else if (choice == 'Choice2') {
            if ($('input[name="SelectedUnit"]:checked').val() == 'KM') {
                nextunit = $('#HidActualUnitKm').val();
            }
            else if ($('input[name="SelectedUnit"]:checked').val() == 'MI') {
                nextunit = $('#HidActualUnitMiles').val();
            }
            else if ($('input[name="SelectedUnit"]:checked').val() == 'HM') {
                nextunit = $('#HidActualUnitHrMotor').val();
            }
        }
        else if (choice == 'Choice3') {
            nextunit = $('#HidNextAppointTime').val();
        }

       


    }

    //replication for car model list value
    $(document).on("change", 'select[name="carModelList"]', function () {
        $('#model-preview').val($(this).val());

        //check choice 4
        selectChoice(4);
    });

    //replication for car color list value
    $(document).on("change", 'select[name="carColorList"]', function () {
        $('#color-preview').val($(this).val());

        //check choice 4
        selectChoice(4);
    });

    //replication for note
    $(document).on("blur", "#Note", function () {
        $('#comment-preview').val($(this).val());

        //check choice 4
        selectChoice(4);
    });

    //Reset Preview
    $(document).on("click", "#btnReset", function () {
        $('input[name="unitvalue"]').val('');
    });

    function selectChoice(choice) {
        if (choice === 1) {
            $("#PrintChoice1").prop("checked", true);
            Choice1_Click();
        }
        else if (choice === 2) {
            $("#PrintChoice2").prop("checked", true);
            Choice2_Click();
        }
        else if (choice === 3) {
            $("#PrintChoice3").prop("checked", true);
            Choice3_Click();
        }
        else if (choice === 4) {
            $("#PrintChoice4").prop("checked", true);
            Choice4_Click();
        }
    }

    function UpdateKM() {
        $('#label-unit').text($('#HidActualKm').val());
        UpdateLabelUnitPreview();
        UpdateNextUnit();
    }

    function UpdateMiles() {
        $('#label-unit').text($('#HidActualMiles').val());
        UpdateLabelUnitPreview();
        UpdateNextUnit();
    }

    function UpdateHM() {
        $('#label-unit').text($('#HidActualHm').val());
        UpdateLabelUnitPreview();
        UpdateNextUnit();
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

    function printSticker() {

        var selectedPrinter = $('#HidSelectedOilPrinter').val();

        qz.printers.find(selectedPrinter).then(function (printer) {
            console.log("Printer: " + printer);

            var config = qz.configs.create(printer);       // Create a default config for the found printer
            var printData = ($("input[name='PrintChoices']:checked").val() === 'Choice4' ? GetKeyMessage() : GetClientMessage());

            qz.print(config, printData);

            //Increment Print Count
            $.ajax({
                url: ajaxUrl + '/garage/IncrementPrintCounter',
                type: 'POST',
                success: function (response) {

                }
            });

        }).catch(function (e) { console.error(e); });
    }

    function GetClientMessage() {
        var oilOffsetXSlider = parseInt($('#HidOilOffsetX').val());
        var oilOffsetYSlider = parseInt($('#HidOilOffsetY').val());

        var printData1;

        printData1 = [
            'N\n',
            'Q400\n',
            'q440\n',
            'D12\n',
            ($('#HidPrinterRotation').val().toLowerCase() === 'true' ? 'ZB\n' : 'ZT\n')
        ];

        if ($('#HidPersonalizedSticker').val() === "False") {
            printData1.push('A' + (parseInt($('#HidCenterGarageNameOffset').val()) + oilOffsetXSlider) + ',' + (157 + oilOffsetYSlider) + ',0,3,1,1,N,"' + $('#garage-name-print').val() + '"\n');
            printData1.push('A' + (116 + oilOffsetXSlider) + ',' + (182 + oilOffsetYSlider) + ',0,3,1,1,N,"' + $('#garage-phone-print').val() + '"\n');
        }

        //Sticker Logo
        if ($('#HidStickerLogo').val() !== "") {
            printData1.push({ type: 'raw', format: 'image', flavor: 'file', data: $('#HidStickerLogo').val(), options: { dotDensity: 'single', language: 'EPL', xmlTag: 'v7:Image', pageHeight: '190', pageWidth: '300', x: '' + (75 + oilOffsetXSlider) + '', y: '' + (45 + oilOffsetYSlider) + '' } });
        }

        printData1.push('A' + (75 + oilOffsetXSlider) + ',' + (232 + oilOffsetYSlider) + ',0,4,1,1,N,"' + $('#comment-preview').val().substring(0, 17) + '"\n');
        printData1.push('A' + (75 + oilOffsetXSlider) + ',' + (262 + oilOffsetYSlider) + ',0,4,1,1,N,"' + $('#comment-preview').val().substring(17, 34) + '"\n');
        printData1.push('A' + (75 + oilOffsetXSlider) + ',' + (292 + oilOffsetYSlider) + ',0,4,1,1,N,"' + $('#comment-preview').val().substring(34, 51) + '"\n');

        printData1.push('A' + (75 + oilOffsetXSlider) + ',' + (342 + oilOffsetYSlider) + ',0,4,1,1,N,"' + nextdate + '"\n');

        if ($('#hidDateFormatPrint').val() === "True") {
            printData1.push('A' + (260 + oilOffsetXSlider) + ',' + (345 + oilOffsetYSlider) + ',0,2,1,1,N,"(' + PrintableDateFormat() + ')"\n');
        }

        var printData2 = [
            'A' + (75 + oilOffsetXSlider) + ',' + (372 + oilOffsetYSlider) + ',0,4,1,1,N,"' + $('input[name="datebox-preview"]').val().toUpperCase() + '"\n',
            'A' + (75 + oilOffsetXSlider) + ',' + (412 + oilOffsetYSlider) + ',0,4,1,1,N,"' + nextunit + '"\n',
            'A' + (75 + oilOffsetXSlider) + ',' + (437 + oilOffsetYSlider) + ',0,4,1,1,N,"' + $('input[name="unitvalue-preview"]').val() + '"\n',
            'P1,1\n'
        ];

        var printData = $.merge(printData1, printData2);

        return printData;
    }

    function GetKeyMessage() {
        var oilOffsetXSlider = parseInt($('#HidOilOffsetX').val());
        var oilOffsetYSlider = parseInt($('#HidOilOffsetY').val());
        var printData1;

        printData1 = [
            'N\n',
            'Q400\n',
            'q440\n',
            'D12\n',
            ($('#HidPrinterRotation').val().toLowerCase() === 'true' ? 'ZB\n' : 'ZT\n')
        ];

        if ($('#HidPersonalizedSticker').val() === "False") {
            printData1.push('A' + (parseInt($('#HidCenterGarageNameOffset').val()) + oilOffsetXSlider) + ',' + (157 + oilOffsetYSlider) + ',0,3,1,1,N,"' + $('#garage-name-print').val() + '"\n');
            printData1.push('A' + (116 + oilOffsetXSlider) + ',' + (182 + oilOffsetYSlider) + ',0,3,1,1,N,"' + $('#garage-phone-print').val() + '"\n');
        }

        //Sticker Logo
        if ($('#HidStickerLogo').val() !== "") {
            printData1.push({ type: 'raw', format: 'image', flavor: 'file', data: $('#HidStickerLogo').val(), options: { dotDensity: 'single', language: 'EPL', xmlTag: 'v7:Image', pageHeight: '190', pageWidth: '300', x: '' + (75 + oilOffsetXSlider) + '', y: '' + (45 + oilOffsetYSlider) + '' } });
        }

        printData1.push('A' + (75 + oilOffsetXSlider) + ',' + (232 + oilOffsetYSlider) + ',0,4,1,1,N,"' + $('#comment-preview').val().substring(0, 17) + '"\n');
        printData1.push('A' + (75 + oilOffsetXSlider) + ',' + (262 + oilOffsetYSlider) + ',0,4,1,1,N,"' + $('#comment-preview').val().substring(17, 34) + '"\n');

        printData1.push('A' + (75 + oilOffsetXSlider) + ',' + (290 + oilOffsetYSlider) + ',0,4,1,1,N,"' + $('#HidMake').val().toUpperCase() + '"\n');
        printData1.push('A' + (75 + oilOffsetXSlider) + ',' + (315 + oilOffsetYSlider) + ',0,4,1,1,N,"' + $('#make-preview').val().toUpperCase() + '"\n');
        printData1.push('A' + (75 + oilOffsetXSlider) + ',' + (350 + oilOffsetYSlider) + ',0,4,1,1,N,"' + $('#HidModel').val().toUpperCase() + '"\n');
        printData1.push('A' + (75 + oilOffsetXSlider) + ',' + (375 + oilOffsetYSlider) + ',0,4,1,1,N,"' + $('#model-preview').val().toUpperCase() + '"\n');
        printData1.push('A' + (75 + oilOffsetXSlider) + ',' + (410 + oilOffsetYSlider) + ',0,4,1,1,N,"' + $('#HidColor').val().toUpperCase() + '"\n');
        printData1.push('A' + (75 + oilOffsetXSlider) + ',' + (435 + oilOffsetYSlider) + ',0,4,1,1,N,"' + $('#color-preview').val().toUpperCase() + '"\n');

        printData1.push('P1,1\n');

        return printData1;
    }

    function centerTitle(word) {
        $.ajax({
            url: ajaxUrl + '/print/center',
            type: "GET",
            data: { word: word },
            async: false,
            success: function (response) {
                $('#HidCenterGarageNameOffset').val(response);
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });
    }
});