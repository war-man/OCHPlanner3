$(document).ready(function () {

    var ajaxUrl = $('#HidRootUrl').val();

    $('select[name="SelectedGarageId"]').select2();

    initTable();

    $('#submitAddForm').on('click', function () {
        var form = $('#addUserForm');

        var validator = form.validate({
            rules: {
                'username': {
                    required: true
                },
                'firstname': {
                    required: true
                },
                'lastname': {
                    required: true
                },
                'SelectedGarageId': {
                    required: true
                },
                'email': {
                    required: true
                },
                'roles[]': {
                    required: true
                }
            },
            messages: {
                'username': {
                    required: $('#hidUsernameRequired').val()
                },
                'firstname': {
                    required: $('#hidFirstnameRequired').val()
                },
                'lastname': {
                    required: $('#hidLastnameRequired').val()
                },
                'SelectedGarageId': {
                    required: $('#hidGarageRequired').val()
                },
                'email': {
                    required: $('#hidEmailRequired').val()
                },
                'roles[]': {
                    required: $('#hidRoleSelectionRequired').val()
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

            var disabled = form.find(':input:disabled').removeAttr('disabled');
            var formData = $(form).serialize();
            disabled.attr('disabled', 'disabled');

            // Submit the form using AJAX.
            $.ajax({
                url: ajaxUrl + '/CreateUser',
                type: "POST",
                dataType: "json",
                data: formData,
                async: false,
                success: function (response) {
                    validator.resetForm();
                    userDone();
                },
                error: function (xhr, status, error) {
                    validator.resetForm();
                    userFail(xhr, status, error);
                }
            });
        }
    });

    $('#submitEditForm').on('click', function () {

        var form = $('#editUserForm');

        var validator = form.validate({
            rules: {
                'username': {
                    required: true
                },
                'firstname': {
                    required: true
                },
                'lastname': {
                    required: true
                },
                'SelectedGarageId': {
                    required: true
                },
                'email': {
                    required: true
                },
                'roles[]': {
                    required: true
                }
            },
            messages: {
                'username': {
                    required: $('#hidUsernameRequired').val()
                },
                'firstname': {
                    required: $('#hidFirstnameRequired').val()
                },
                'lastname': {
                    required: $('#hidLastnameRequired').val()
                },
                'SelectedGarageId': {
                    required: $('#hidGarageRequired').val()
                },
                'email': {
                    required: $('#hidEmailRequired').val()
                },
                'roles[]': {
                    required: $('#hidRoleSelectionRequired').val()
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
            var disabled = form.find(':input:disabled').removeAttr('disabled');
            var formData = $(form).serialize();
            disabled.attr('disabled', 'disabled');

            // Submit the form using AJAX.
            $.ajax({
                url: ajaxUrl + '/UpdateUser',
                type: "POST",
                dataType: "json",
                data: formData,
                async: false,
                success: function (response) {
                    validator.resetForm();
                    editDone();
                },
                error: function (xhr, status, error) {
                    validator.resetForm();
                    editFail(xhr, status, error);
                }
            });
        }
    });

    $('#submitPwdForm').on('click', function () {
        var form = $('#pwdForm');

        var validator = form.validate({
            rules: {
                'password': {
                    required: true
                },
                'verify': {
                    required: true,
                    equalTo: "#password"
                }
            },
            messages: {
                'password': {
                    required: $('#hidPasswordRequired').val()
                },
                'verify': {
                    required: $('#hidVerifyPasswordRequired').val(),
                    equalTo: $('#hidPasswordNotEqual').val()
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
                url: ajaxUrl + '/ResetPassword',
                type: "POST",
                dataType: "json",
                data: formData,
                async: false,
                success: function (response) {
                    pwdDone();
                },
                error: function (xhr, status, error) {
                    pwdFail(xhr, status, error);
                }
            });
        }
    });

    function updateUserList() {
        $.ajax({
            url: ajaxUrl + '/Users/list',
            type: "GET",
            dataType: "html",
            async: false,
            success: function (response) {
                $('#user-list').empty().html(response);
                initTable();
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });
    }

    function initTable() {

        var tableSettings = {
            dom: 'Bfrtip',
            select: {
                style: 'single',
                info: false
            },
            "aoColumnDefs": [
                {
                    "aTargets": [0, 5, 8, 9, 10],
                    "visible": false
                },
                {
                    "aTargets": [7],
                    "className": 'text-center',
                    "width": "100px"
                }
            ],
            buttons: [
                {
                    text: $('#hidNewButton').val(),
                    action: function (e, dt, button, config) {
                        $('#userError').hide();
                        $('#addUserForm').trigger('reset');
                        $('#addModal').modal({ backdrop: 'static' });
                    }
                },
                {
                    extend: "selectedSingle",
                    text: $('#hidEditButton').val(),
                    action: function (e, dt, button, config) {
                        var data = dt.row({ selected: true }).data();

                        $('#editError').hide();
                        $('#userError').hide();
                        $('#editUserForm').trigger('reset');

                        $('#editUserForm input[name=username]').val(data.username);
                        $('#editUserForm input[name=email]').val(data.email);
                        $('#editUserForm input[name=firstname]').val(data.firstname);
                        $('#editUserForm input[name=lastname]').val(data.lastname);
                        $("#editUserForm select[name='SelectedGarageId']").val(data.garageId).trigger('change');

                        $('#editUserForm input[name=locked]').prop('checked', data.locked === 'True' ? true : false);
                        $('#editUserForm button[name=locked]').prop('checked', data.locked === 'True' ? true : false);
                        $('#editUserForm input[name=id]').val(data.id);

                        if (data.roles !== '') {
                            var array = data.roles.split('|');
                            $.each(array, function (index, value) {
                                $('#editUserForm :checkbox[value=' + "'" + value + "'" + ']').prop('checked', true);
                            });
                        }

                        $('#editModal').modal({ backdrop: 'static' });
                        //Trigger DDL change event to display current time
                        $('#editModal').on('shown.bs.modal', function () {
                            $("#editUserForm select[id='tz-selector-edit']").trigger('change');
                        });

                    }
                },
                {
                    extend: "selectedSingle",
                    text: $('#hidDeleteButton').val(),
                    action: function (e, dt, button, config) {
                        var data = dt.row({ selected: true }).data();

                        swal.fire({
                            title: $('#hidDeleteTitle').val(),
                            text: $('#hidDeleteText').val(),
                            icon: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#DD6B55',
                            confirmButtonText: $('#hidDeleteButton').val()
                        }).then(function (result) {

                            if (result.value) {
                                $.ajax({
                                    type: 'DELETE',
                                    url: ajaxUrl + '/DeleteUser',
                                    data: { id: data.id }
                                })
                                    .done(delDone)
                                    .fail(delFail);
                            }
                        });
                    }
                },
                {
                    extend: "selectedSingle",
                    text: $('#hidResetPasswordButton').val(),
                    action: function (e, dt, button, config) {
                        var data = dt.row({ selected: true }).data();
                        $('#pwdError').hide();
                        $('#pwdForm').trigger('reset');
                        $('#pwdForm input[name=id]').val(data.id);
                        $('#pwdModal').modal({ backdrop: 'static' });
                    }
                }
            ],
            initComplete: function () {
                $('#usersTable_wrapper').find('div.dt-buttons').find('button').removeClass('dt-button').addClass('btn btn-outline-secondary btn-sm');

            }
        };

        if ($('#hidLanguage').val().toUpperCase() === "FR") {
            tableSettings.language = JSON.parse(datables_french());
        }

        table = $('#usersTable').DataTable(tableSettings);

        // Disable ADD button if max number of user reached
        var remainingUsers = parseInt($('#hidRemainingUsers').val());

        if (remainingUsers === 0) {
            table.button(0).enable(false);
            $('#MaxUserWarning').removeClass('hidden');
        }
        else {
            table.button(0).enable(true);
            $('#MaxUserWarning').addClass('hidden');
        }
    }

    $('#addModal').on('hidden.bs.modal', function () {
        $("#addUserForm").validate().resetForm();

        // get errors that were created using jQuery.validate.unobtrusive
        $("#addUserForm").find(".is-invalid").removeClass('is-invalid');
    });

    $('#editModal').on('hidden.bs.modal', function () {
        $("#editUserForm").validate().resetForm();

        // get errors that were created using jQuery.validate.unobtrusive
        $("#editUserForm").find(".is-invalid").removeClass('is-invalid');
    });

    $('#pwdModal').on('hidden.bs.modal', function () {
        $("#pwdForm").validate().resetForm();

        // get errors that were created using jQuery.validate.unobtrusive
        $("#pwdForm").find(".is-invalid").removeClass('is-invalid');
    });

    function userDone(data, status, xhr) {
        $('#addModal').modal('hide');
        Swal.fire({
            icon: 'success',
            title: $('#hidCreateUserSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true
        });

        //Update remainingUsers
        var total = parseInt($('#hidRemainingUsers').val());
        $('#hidRemainingUsers').val(total-1);

        updateUserList();
    }

    function resendEmailDone(data, status, xhr) {
        $('#userModal').modal('hide');
        toastr.success($('#hidSendEmailSuccess').val());
    }

    function userFail(xhr, status, error) {
        $('#userError').html(xhr.responseText || error).fadeIn();
    }

    function editDone(data, status, xhr) {
        $('#editModal').modal('hide');
        Swal.fire({
            icon: 'success',
            title: $('#hidUpdateUserSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true
        });
        updateUserList();
    }

    function editFail(xhr, status, error) {
        $('#editError').html(xhr.responseText || error).fadeIn();
    }

    function delDone(data, status, xhr) {
        Swal.fire({
            icon: 'success',
            title: $('#hidDeleteUserSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true
        });
        updateUserList();
    }

    function delFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }

    function pwdDone(data, status, xhr) {
        Swal.fire({
            icon: 'success',
            title: $('#hidChangePasswordSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true
        });
        $('#pwdModal').modal('hide');
    }

    function pwdFail(xhr, status, error) {
        $('#pwdError').html(xhr.responseText || error).fadeIn();
    }
});
