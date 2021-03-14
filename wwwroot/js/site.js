$(document).ready(function () {
    //hack to submit the logoutForm (had to do this because of styling)
    $('#logout').on('click', function () {
        $("#logoutForm").submit();
    });

    $('.restrict').keydown(function (e) {
        var k = e.which;
        var ok = k >= 65 && k <= 90 || // A-Z
            k >= 96 && k <= 105 || // a-z
            k >= 35 && k <= 40 || // arrows
            k == 8 || // Backspaces
            k == 32 || // Spaces
            k == 189 || // -
            k == 222 || //#
            k >= 48 && k <= 57; // 0-9

        if (!ok) {
            e.preventDefault();
            e.stopPropagation();
        }
    });

});
