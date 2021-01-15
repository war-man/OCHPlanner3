$(document).ready(function () {

    $('#reset-form').validate({
        rules: {
            'Input.Email': {
                required: true,
                noSpace: true,
                email: true,
            },
            'Input.Password': {
                required: true,
                noSpace: true
            },
            'Input.ConfirmPassword': {
                required: true,
                noSpace: true
            }
        },
        messages: {
            'Input.Email': {
                required: "Please enter a email address",
                noSpace: "Please enter a email address",
                email: "Please enter a vaild email address"
            },
            'Input.Password': {
                required: "Please enter a password",
                noSpace: "Please enter a password"
            },
            'Input.ConfirmPassword': {
                required: "Please confirm your password",
                noSpace: "Please confirm your password"
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
});