$(document).ready(function () {
    //hack to submit the logoutForm (had to do this because of styling)
    $('#logout').on('click', function () {
        $("#logoutForm").submit();
    });
});
