$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    if ($('#hidLanguage').val() === 'FR') {
        moment.locale('fr');
    }
    else {
        moment.locale('en');
    }

    $('.date').datetimepicker({
        format: $('#hidDateFormat').val().toUpperCase(),
        defaultDate: moment()
    });

    $(document).on("click", "#btnSave", function () {

        var form = $('#vehicleForm');

        $('input[name="SelectedProduct"]').addClass('ignoreClass');
        $('input[name="Quantity"]').addClass('ignoreClass');

        form.validate({
            ignore: ".ignoreClass",
            rules: {
                'VinCode': {
                    required: true
                },
                'Year': {
                    required: true
                },
                'Make': {
                    required: true
                },
                'Model': {
                    required: true
                },
                'Engine': {
                    required: true
                },
                'Odometer': {
                    required: true
                },
                'OwnerPhone': {
                    required: true
                }
            },
            messages: {
                'VinCode': {
                    required: $('#hidVinCodeRequired').val()
                },
                'Year': {
                    required: $('#hidYearRequired').val()
                },
                'Make': {
                    required: $('#hidMakeRequired').val()
                },
                'Model': {
                    required: $('#hidModelRequired').val()
                },
                'Engine': {
                    required: $('#hidEngineRequired').val()
                },
                'Odometer': {
                    required: $('#hidOdometerRequired').val()
                },
                'OwnerPhone': {
                    required: $('#hidOwnerPhoneRequired').val()
                }
            },
            errorElement: 'span',
            errorPlacement: function (error, element) {
                error.insertAfter(element);
                error.addClass('invalid-feedback');
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass('is-invalid');
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass('is-invalid');
            }
        });

        if (form.valid()) {
            var formData = $(form).serialize();

            var model = {
                Engine: formData.Engine
            };

            $.ajax({
                url: ajaxUrl + '/Vehicle/Save',
                type: "POST",
                data: formData,
                success: function (response) {
                    editDone();
                },
                error: function (xhr, status, error) {
                    fail(xhr, status, error);
                }
            });

            //$.ajax({
            //    url: ajaxUrl + '/Vehicle/Save',
            //    type: "POST",
            //    data: formData
            //})
            //    .done(addDone)
            //    .fail(ajaxFail);

            


        }
    });

    function addDone(data, status, xhr) {
        Swal.fire({
            icon: 'success',
            title: $('#hidSaveSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 1000,
            timerProgressBar: true,
            onClose: () => {
                location.href = ajaxUrl + "/MaintenanceType/" + $('#hidSelectedGarageId').val()
            }
        });
    }

    function ajaxFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }
});