$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    $('#Phone').mask('(000) 000-0000');

    //readonly everything if not SuperAdmin
    if ($('#HidIsSuperAdmin').val() == 'False') {
        var form = $('#garage-edit');
        form.find(':input').prop('readonly', true);
        $("#SelectedBannerId").prop('disabled', 'disabled');
        $("#SelectedDateFormatCode").prop('disabled', 'disabled');
        $("#SelectedLanguageCode").prop('disabled', 'disabled');
        $("#PersonalizedSticker").prop('disabled', 'disabled');
    }

    $(document).on("change", 'input[name="BrandingId"]', function () {
        if ($(this).val() === '1') {
            refreshBrandingPreview(1);
        }
        else if ($(this).val() === '2') {
            refreshBrandingPreview(2);
        }
    });

    $('.fa-trash-alt').on('click', function () {
        $.ajax(
            {
                type: "DELETE",
                url: ajaxUrl + '/garage/deleteLogo/',
                data: { garageId: $('#Id').val() },
                async: false,
                success: function (data) {
                    $('#file').val('');
                    $('#garage-logo').attr('src', 'https://ochplanner3.blob.core.windows.net/logos/blank.png');
                    $('#hidStickerLogo').val('');
                    $('.fa-trash-alt').hide();
                }
            }
        );
    });

    $(document).on("click", '#btnSave', function () {

        var form = $('#garage-edit');

        var validator = form.validate({
            rules: {
                'name': {
                    required: true,
                    noSpace: true
                },
                'address': {
                    required: true,
                    noSpace: true
                },
                'phone': {
                    required: true,
                    noSpace: true
                },
                'email': {
                    required: true,
                    noSpace: true
                },
                'SelectedLanguageCode': {
                    required: true
                },
                'SelectedDateFormatCode': {
                    required: true
                },
                'nbrUser': {
                    required: true
                }
            },
            messages: {
                'name': {
                    required: $('#HidRequired').val(),
                    noSpace: $('#HidRequired').val()
                },
                'address': {
                    required: $('#HidRequired').val(),
                    noSpace: $('#HidRequired').val()
                },
                'phone': {
                    required: $('#HidRequired').val(),
                    noSpace: $('#HidRequired').val()
                },
                'email': {
                    required: $('#HidRequired').val(),
                    noSpace: $('#HidRequired').val()
                },
                'SelectedLanguageCode': {
                    required: $('#HidRequired').val()
                },
                'SelectedDateFormatCode': {
                    required: $('#HidRequired').val()
                },
                'nbrUser': {
                    required: $('#HidRequired').val()
                }
            },
            errorElement: 'span',
            errorPlacement: function (error, element) {
                error.addClass('invalid-feedback');
                element.closest('.form-group').append(error);
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

            // Submit the form using AJAX.
            $.ajax({
                traditional: true,
                url: ajaxUrl + '/garage/edit',
                type: 'PUT',
                data: formData,
                success: function (response) {
                    if (response >= 1) {
                        Swal.fire({
                            icon: 'success',
                            title: $('#HidSaveSuccess').val(),
                            showCancelButton: false,
                            showConfirmButton: false,
                            timer: 1000,
                            timerProgressBar: true,
                            onClose: () => {
                                location.href = ajaxUrl + '/Garages';
                            }
                        });
                    }
                }
            });
        }
    });

    //boutons de gestion
    $(document).on("click", '#btnProduct', function () {
        location.href = ajaxUrl + '/Options/Products/' + $('#Id').val();
    });

    $(document).on("click", '#btnMaintenanceType', function () {
        location.href = ajaxUrl + '/MaintenanceType/' + $('#Id').val();
    });

    $(document).on("blur", '#CounterOrder', function () {

        var counter = $(this).val();

        swal.fire({
            title: "Mise à jour",
            text: "Voulez vous mettre à jour la quantité en stock?",
            icon: 'warning',
            showCancelButton: true,
            //confirmButtonColor: '#DD6B55',
            confirmButtonText: "Oui"
        }).then(function (result) {
            if (result.value) {
                $('#stock-span').text(counter);
                $('#UpdateCounterStock').val(true);
            }
        });
                
    });

    function refreshBrandingPreview(id) {
        $.ajax(
            {
                type: "GET",
                url: ajaxUrl + '/reference/branding',
                data: { brandingId: id },
                async: false,
                success: function (data) {
                    $('#branding-logo').attr('src', data.LogoUrl + '/' + id + '_logo.jpg');
                    $('#labelHelpLinkFr').text('Fr: ' + data.HelpLinkFr);
                    $('#labelHelpLinkEn').text('En: ' + data.HelpLinkEn);
                    $('#labelStoreLinkFr').text('Fr: ' + data.StoreLinkFr);
                    $('#labelStoreLinkEn').text('En: ' + data.StoreLinkEn);
                }
            }
        );
    }
});