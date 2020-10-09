$(document).ready(function () {


    $(document).on("click", "#btnPrint", function () {
        findDefaultPrinter(true);

        //printCommand();
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

    //replication for selected month
    $(document).on("change", 'select[name="SelectedMonth"]', function () {
        var month = $('select[name="SelectedMonth"] option:selected').text();
        var mileage = $('select[name="SelectedMileage"] option:selected').text();
        $('#datebox').val(month);
        $('input[name="unitvalue-preview"]').val(parseInt($('input[name="unitvalue"]').val()) + parseInt(mileage));
    });

    function printSimpleSticker() {
        var config = getUpdatedConfig();
        var lang = getUpdatedOptions().language; //print options not used with this flavor, just check language requested

        var printData;
        
        printData = [
            'N\n',
            'A90,157,0,3,1,1,N,"CARIGNAN ST-AMABLE"\n',
            'A116, 182, 0, 3, 1, 1, N, "(450) 922-8288"\n',
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