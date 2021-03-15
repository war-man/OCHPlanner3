$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    var tableSettings = {
        dom: 'Bfrtip',
        select: {
            style: 'single',
            info: false
        },
        lengthMenu: [
            [10, 25, 50, -1],
            [$("#hid10Rows").val(), $('#hid25Rows').val(), $('#hid50Rows').val(), $('#hidAllRows').val()]
        ],
        "pageLength": 25,
        "aoColumnDefs": [
            {
                "aTargets": [0],
                "visible": false
            }
        ],
        "order": [[1, "asc"]],
        buttons: [
            {
                text: $('#hidNewButton').val(),
                action: function (e, dt, button, config) {
                    $('#addError').hide();
                    $('#addForm').trigger('reset');
                    $('#addModal').modal({ backdrop: 'static' });
                }
            },
            {
                extend: "selectedSingle",
                text: $('#hidEditButton').val(),
                action: function (e, dt, button, config) {
                    var data = dt.row({ selected: true }).data();
                    $('#editError').hide();
                    $('#tabs a:first').tab('show');
                    $('#editForm').trigger('reset');

                    $('#editForm input[name=name]').val(data.name);
                    $('#editForm input[name=id]').val(data.id);

                    $('#editModal').modal({ backdrop: 'static' });
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
                                url: ajaxUrl + '/Program/Delete',
                                data: { id: data.id }
                            })
                                .done(delDone)
                                .fail(ajaxFail);
                        }
                    });
                }
            },
            'pageLength'
        ],
        initComplete: function () {
            $('#ProgramListTable_wrapper').find('div.dt-buttons').find('button').removeClass('dt-button').addClass('btn btn-outline-secondary btn-sm');
        }
    };

    initTable();

    function initTable() {

        if ($('#hidLanguage').val().toUpperCase() === "FR") {
            tableSettings.language = JSON.parse(datables_french());
        }

        var table = $('#ProgramListTable').DataTable(tableSettings);
    }

    $('#submitAddForm').on('click', function () {
        var form = $('#addForm');

        form.validate({
            rules: {
                'name': {
                    required: true,
                    noSpace: true
                }
            },
            messages: {
                'name': {
                    required: $('#hidNameRequired').val(),
                    noSpace: $('#hidNameRequired').val()
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

            $.ajax({
                url: ajaxUrl + '/Program/Create',
                type: "POST",
                data: formData,
                success: function (response) {
                    addDone();
                },
                error: function (xhr, status, error) {
                    ajaxFail(xhr, status, error);
                }
            });
        }
    });

    $('#submitEditForm').on('click', function () {
        var form = $('#editForm');

        form.validate({
            rules: {
                'name': {
                    required: true,
                    noSpace: true
                }
            },
            messages: {
                'name': {
                    required: $('#hidNameRequired').val(),
                    noSpace: $('#hidNameRequired').val()
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

            $.ajax({
                url: ajaxUrl + '/Program/Update',
                type: "POST",
                data: formData,
                success: function (response) {
                    editDone();
                },
                error: function (xhr, status, error) {
                    ajaxFail(xhr, status, error);
                }
            });
        }
    });

    function addDone(data, status, xhr) {
        $('#addModal').modal('hide');
        Swal.fire({
            icon: 'success',
            title: $('#hidSaveSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 1000,
            timerProgressBar: true,
            onClose: () => {
                UpdateProgramList($('#hidSelectedGarageId').val());
            }
        });
       
    }

    function editDone(data, status, xhr) {
        $('#editModal').modal('hide');
        Swal.fire({
            icon: 'success',
            title: $('#hidUpdateSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 1000,
            timerProgressBar: true,
            onClose: () => {
                UpdateProgramList($('#hidSelectedGarageId').val());
            }
        });

    }
   
    function UpdateProgramList(selectedGarage) {
        $.ajax({
            url: ajaxUrl + '/ProgramList/' + selectedGarage,
            type: "GET",
            dataType: "html",
            async: false,
            success: function (response) {
                $('#program-list').empty().html(response);
                initTable();
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });
    }

    function delDone(data, status, xhr) {
        Swal.fire({
            icon: 'success',
            title: $('#hidDeleteSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 1000,
            timerProgressBar: true,
            onClose: () => {
                UpdateProgramList($('#hidSelectedGarageId').val());
            }
        });
    }

    function ajaxFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }
});