$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    //Detect ENTER key
    $(document).on("keypress", 'input[name="VinCode"]', function (e) {
        if (e.which == 13) {
            $('input[name="Description"]').focus();
        }
    });

    $(document).on("click", '#btnVehicleDetail', function (e) {
        if ($('input[name="VinCode"]').val() !== '') {
            location.href = ajaxUrl + "/Vehicle/" + $('input[name="VinCode"]').val()
        }
    });

    $(document).on("blur", 'input[name="VinCode"]', function () {

        if ($(this).val().trim() === '')
            return false;

        var VIN = $(this).val();

        $.ajax({
            url: ajaxUrl + '/MaintenancePlan/VINDecode',
            type: "GET",
            data: {
                vin: VIN
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
                            location.href = ajaxUrl + "/Vehicle/" + VIN
                           
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