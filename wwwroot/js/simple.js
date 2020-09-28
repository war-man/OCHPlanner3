$(document).ready(function () {

    $(document).on("click", "#btnPrint", function () {
        findDefaultPrinter(true);
        printCommand();
    });
});