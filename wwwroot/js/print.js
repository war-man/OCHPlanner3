var ajaxUrl = $('#HidRootUrl').val();

function centerTitle(word) {
    $.ajax({
        url: ajaxUrl + '/print/center',
        type: "GET",
        data: { word: word },
        async: false,
        success: function (response) {
            return response;
        },
        error: function (xhr, status, error) {
            alert('Error');
        }
    });
}

