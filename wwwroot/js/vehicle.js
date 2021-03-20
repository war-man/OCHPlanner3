$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    $('input[name = "OwnerPhone"]').mask('(000) 000-0000');
    $('input[name = "DriverPhone"]').mask('(000) 000-0000');
    $('input[name = "DriverCellphone"]').mask('(000) 000-0000');

    $('select[name="OwnerName"]').select2({
        tags: true,
        createTag: function (params) {
            var term = $.trim(params.term);

            if (term === '') {
                return null;
            }

            return {
                id: 0,
                text: term,
                newTag: true // add additional parameters
            }
        },
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

    $.validator.addMethod("noDuplicateOwner", function (value, element) {
        debugger
        var owner = $("select[name='OwnerName']").select2('data')
        if (!owner[0].newTag) return true;

        var result = false;

        $.ajax({
            url: ajaxUrl + '/Vehicle/DuplicateOwner',
            type: "GET",
            async: false,
            data: {
                name: owner[0].text,
                phone: $('input[name="OwnerPhone"]').val(),
                garageId: $('input[name="OwnerGarageId"]').val()
            },
            success: function (response) {
                result = response;
            },
            error: function (xhr, status, error) {
                return false;
            }
        });

        return result;
    }, "Required Field");

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
                    required: true,
                    minStrict: 1,
                },
                'OwnerName': {
                    noDuplicateOwner: true
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
                    required: $('#hidOdometerRequired').val(),
                    minStrict: $('#hidOdometerRequired').val()
                },
                'OwnerName': {
                    noDuplicateOwner: 'GGGG'
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

            formData = formData + '&SelectedPrograms=' + favorite.join("|");

            var owner = $("select[name='OwnerName']").select2('data')

            var res = formData.split("&");
            for (var i = 0; i < res.length; i++) {
                if (res[i].indexOf("OwnerName") == 0) {
                    res[i] = 'OwnerName=' + owner[0].text;
                }
                else if (res[i].indexOf("VehicleOwnerId") == 0) {
                    res[i] = 'VehicleOwnerId=' + owner[0].id;
                }
            }

            formData = res.join("&");    
            
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

    $('select[name="OwnerName"]').on('select2:select', function (e) {

        var owner = $("select[name='OwnerName']").select2('data')
        if (owner[0].newTag) {
            $('input[name="OwnerCompany"]').val('');
            $('input[name="OwnerAddress"]').val('');
            $('input[name="OwnerPhone"]').val('');
            $('input[name="OwnerEmail"]').val('');
            return;
        }

        $.ajax({
            url: ajaxUrl + '/Vehicle/GetOwner',
            type: "GET",
            data: {
                ownerId: $(this).val()
            },
            success: function (response) {
                $('input[name="OwnerName"]').val(response.Name);
                $('input[name="OwnerCompany"]').val(response.Company);
                $('input[name="OwnerAddress"]').val(response.Address);
                $('input[name="OwnerPhone"]').val(response.Phone);
                $('input[name="OwnerEmail"]').val(response.Email);
            },
            error: function (xhr, status, error) {
                fail(xhr, status, error);
            }
        });
    });

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