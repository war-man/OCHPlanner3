$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    $(document).on("blur", 'input[name="VinCode"]', function () {

        $.ajax({
            url: ajaxUrl + '/MaintenancePlan/VINDecode',
            type: "GET",
            data: {
                vin: $(this).val()
            },
            async: false,
            success: function (response) {
                $('input[name="Description"]').val(response.Description);
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });

    });

});