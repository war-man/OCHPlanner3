$(document).ready(function () {


    $(document).on("click", "#btnPrint", function () {
        printSimpleSticker();
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
    $(document).on("keyup", "#UnitValue", function () {
        $('input[name="unitvalue-preview"]').val($('input[name="unitvalue"]').val());
    });

    //CHOICE 1
    $(document).on("click", "#PrintChoice1", function () {
        console.log('Choice 1');
        $('#label-datebox-preview').text('Prochaine date ou avant le');
    });

    //replication for selected Period
    $(document).on("change", 'select[name="SelectedPeriod"]', function () {
        var month = parseInt($('select[name="SelectedPeriod"] option:selected').text()); 

        $('#datebox-preview').val(moment().add(month, 'M').format('MM/YYYY'));
    });

    //replication for selected mileage
    $(document).on("change", 'select[name="SelectedMileage"]', function () {
        var startMileage = $('input[name="unitvalue"]').val();
        if (startMileage === '') {
            startMileage = 0;
        }

        var mileage = $('select[name="SelectedMileage"] option:selected').text();
        $('input[name="unitvalue-preview"]').val(parseInt(startMileage) + parseInt(mileage));
    });


    //CHOICE 2
    $(document).on("click", "#PrintChoice2", function () {
        console.log('Choice 2');
        $('#label-datebox-preview').text('Entretien effectué le');
        $('#datebox-preview').val(moment().format('MM/YYYY'));
    });

    //replication for selected Mileage
    $(document).on("change", 'select[name="Choice2SelectedMileage"]', function () {
        var startMileage = $('input[name="unitvalue"]').val();
        if (startMileage === '') {
            startMileage = 0;
        }

        var mileage = $('select[name="Choice2SelectedMileage"] option:selected').text();
        $('input[name="unitvalue-preview"]').val(parseInt(startMileage) + parseInt(mileage));
    });

    //CHOICE 3
    $(document).on("click", "#PrintChoice3", function () {
        console.log('Choice 3');
        $('#label-datebox-preview').text('Prochaine date ou avant le');
    });

    $(document).on("change", 'select[name="Choice3SelectedMonth"]', function () {
        Choice3UpdatePreview();
    });

    $(document).on("change", 'select[name="Choice3SelectedYear"]', function () {
        Choice3UpdatePreview();
    });

    //replication for selected Mileage
    $(document).on("change", 'select[name="Choice3SelectedMileage"]', function () {
        var startMileage = $('input[name="unitvalue"]').val();
        if (startMileage === '') {
            startMileage = 0;
        }

        var mileage = $('select[name="Choice3SelectedMileage"] option:selected').text();
        $('input[name="unitvalue-preview"]').val(parseInt(startMileage) + parseInt(mileage));
    });

    function Choice3UpdatePreview() {
        var month = parseInt($('select[name="Choice3SelectedMonth"] option:selected').val());
        var year = parseInt($('select[name="Choice3SelectedYear"] option:selected').val());

        //pad zero if month length = 1
        if (month > 0 && year > 0) {
            $('#datebox-preview').val((month <= 9 ? '0' + month : month) + "/" + year);
        } 
    }

    function printSimpleSticker() {
        var config = getUpdatedConfig();
        findDefaultPrinter(true);

        var lang = getUpdatedOptions().language; //print options not used with this flavor, just check language requested

        var printData;
        
        printData = [
            'N\n',
            'Q500\n',
            'q440\n',
            'D12\n',
            'A90,157,0,3,1,1,N,"CARIGNAN ST-AMABLE"\n',
            'A116,182,0,3,1,1,N,"(450) 922-8288"\n',
            'A75,212,0,3,1,1,N,"' + $('input[name="comment-preview"]').val() + '"\n',
            'A75,242,0,4,1,1,N,"' + $('select[name="oillist-preview"] option:selected').text() + '"\n',
            'A75,272,0,4,1,1,N,"PROCH. DATE"\n',
            'A75,302,0,5,1,1,N,"' + $('input[name="datebox-preview"]').val().toUpperCase() + '"\n',
            'A75,362,0,4,1,1,N,"PROCH. KM"\n',
            'A75,387,0,5,1,1,N,"' + $('input[name="unitvalue-preview"]').val() + '"\n',
            'P1,1\n'
        ];
                
        qz.print(config, printData).catch(displayError);
    }
});