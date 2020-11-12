$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    $('#btnSave').on('click', function () {
        var form = $('#garage-edit');

        var validator = form.validate({
            rules: {
                'name': {
                    required: true
                },
                'address': {
                    required: true
                },
                'phone': {
                    required: true
                },
                'email': {
                    required: true
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
                    required: $('#HidRequired').val()
                },
                'address': {
                    required: $('#HidRequired').val()
                },
                'phone': {
                    required: $('#HidRequired').val()
                },
                'email': {
                    required: $('#HidRequired').val()
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
                            timer: 2000,
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
});