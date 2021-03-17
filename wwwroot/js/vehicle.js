$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    $('select[name="OwnerName"]').select2({
        tags: true
    });

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

    $(document).on("click", "#btnCancel", function () {
        location.href = ajaxUrl + "/preventiveMaintenance";
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

            var favorite = [];
            $.each($("input[name='programs[]']:checked"), function () {
                var note = $("input[name='Note_" + $(this).val() + "']").val();
                favorite.push($(this).val() + ',' + note);
            });
            //alert("My favourite sports are: " + favorite.join("|"));
            formData = formData + '&SelectedPrograms=' + favorite.join("|");

            $.ajax({
                url: ajaxUrl + '/Vehicle/Save',
                type: "POST",
                data: formData,
                success: function (response) {
                    saveDone();
                },
                error: function (xhr, status, error) {
                    fail(xhr, status, error);
                }
            });

        }
    });

    $(document).on("blur", 'input[name="Odometer"]', function () {
        if ($('input[name="Odometer"]').val() != '' && parseInt($('input[name="Odometer"]').val()) > 0) {
            UpdateMonthlyMileage();
        }
    });

    $("#datetimepicker1").on("change.datetimepicker", ({ date, oldDate }) => {
        UpdateMonthlyMileage();
    })

    function UpdateMonthlyMileage() {
        var entryDate = $('input[name="EntryDate"]').val();
        var currentDate = moment();
        var mileage = $('input[name="Odometer"]').val();

        var entry = moment(entryDate).format('' + $('#hidDateFormat').val().toUpperCase() + '')

        var diff = currentDate.diff(entry, 'months') 

        $('input[name="MonthlyMileage"]').val(Math.round($('input[name="Odometer"]').val() / diff));
    }

    function saveDone(data, status, xhr) {
        Swal.fire({
            icon: 'success',
            title: $('#hidSaveSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 1000,
            timerProgressBar: true,
            onClose: () => {
                location.href = ajaxUrl + "/preventiveMaintenance/" + $('input[name="VinCode"]').val()
            }
        });
    }

    function fail(xhr, status, error) {
        alert(xhr.responseText || error);
    }
});