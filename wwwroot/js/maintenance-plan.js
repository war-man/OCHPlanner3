$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    $(document).on("blur", 'input[name="VinCode"]', function () {
        alert($(this).val());

        $.ajax({
            url: ajaxUrl + '/MaintenancePlan/VINDecode',
            type: "GET",
            data: {
                vin: $(this).val()
            },
            async: false,
            success: function (response) {
                alert(response);
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });

    });

});