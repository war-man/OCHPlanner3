$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    $('.fa-trash-alt').on('click', function () {
        $.ajax(
            {
                type: "DELETE",
                url: ajaxUrl + '/garage/deleteLogo/',
                data: { garageId: '99999' },
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

    $('#Phone').mask('(000) 000-0000');

    $(document).on("change", 'input[name="BrandingId"]', function () {
        if ($(this).val() === '1') {
            refreshBrandingPreview(1);
        }
        else if ($(this).val() === '2') {
            refreshBrandingPreview(2);
        }
    });

    $('#btnSave').on('click', function () {
        var form = $('#garage-create');

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
                url: ajaxUrl + '/garage/create',
                type: 'POST',
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