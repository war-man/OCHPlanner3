$(document).ready(function () {

    $(document).on("click", "#btnPrint", function () {
        findDefaultPrinter(true);
        printCommand();
    });

    //replication for oil list value
    $(document).on("change", $('select[name="oillist"]'), function () {
        $('select[name="oillist-preview"]').val($('select[name="oillist"]').val());
    });

    //replication for comment
    $(document).on("keyup", $("#comment"), function () {
        $('input[name="comment-preview"]').val($('input[name="comment"]').val());
    });

    //replication for unitvalue
    $(document).on("keyup", $("#unitvalue"), function () {
        $('input[name="unitvalue-preview"]').val($('input[name="unitvalue"]').val());
    });
});