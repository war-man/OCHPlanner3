$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    $(document).on("blur", 'input[name="VinCode"]', function () {

        if ($(this).val().trim() === '')
            return false;

        $.ajax({
            url: ajaxUrl + '/MaintenancePlan/VINDecode',
            type: "GET",
            data: {
                vin: $(this).val()
            },
            async: false,
            success: function (response) {
                if (response.VIN == null) {
                    //Display VIN not found
                    swal.fire({
                        title: $('#hidVINNotFoundTitle').val(),
                        text: $('#hidVINNotFoundText').val(),
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#1E7E34',
                        confirmButtonText: $('#hidCreateVehicleButton').val()
                    }).then(function (result) {
                        if (result.value) {
                            location.href = ajaxUrl + "/Vehicle"
                           
                        }
                    });
                }

                $('input[name="Description"]').val(response.Description);
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });

    });

});