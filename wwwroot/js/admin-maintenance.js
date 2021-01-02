$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    $('select[name="SelectedGarageId"]').select2();

    $('select[name="SelectedGarageId"]').on('change', function () {
        var selectedGarage = $(this).val();
        $('#hidSelectedGarageId').val(selectedGarage);
        $('input[name="selectedGarageId"]').val(selectedGarage);
        updateMaintenanceList(selectedGarage);
    });

    var tableSettings = {
        dom: 'Bfrtip',
        select: {
            style: 'single',
            info: false
        },
        "aoColumnDefs": [
            {
                "aTargets": [0],
                "visible": false
            }
        ],
        buttons: [
            {
                text: $('#hidNewButton').val(),
                action: function (e, dt, button, config) {
                    $('#maintenanceError').hide();
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
                                url: ajaxUrl + '/Options/DeleteMaintenance',
                                data: { id: data.id }
                            })
                                .done(delDone)
                                .fail(delFail);
                        }
                    });
                }
            }
        ],
        initComplete: function () {
            $('#MaintenanceListTable_wrapper').find('div.dt-buttons').find('button').removeClass('dt-button').addClass('btn btn-outline-secondary btn-sm');
        }
    };

    initTable();

    function initTable() {

        if ($('#hidLanguage').val().toUpperCase() === "FR") {
            tableSettings.language = JSON.parse(datables_french());
        }

        var table = $('#MaintenanceListTable').DataTable(tableSettings);

        table.on('select deselect', function (e, dt, type, indexes) {
            var rowData = table.rows(indexes).data().toArray();

            //Cannot delete base options
            if (parseInt(rowData[0]['id']) >= 250000) {
                table.button(1).enable(false);
                table.button(2).enable(false);
            }
            else {
                table.button(1).enable(true);
                table.button(2).enable(true);
            }
        });
    }

    $('#submitAddForm').on('click', function () {
        var form = $('#addForm');

        form.validate({
            rules: {
                'name': {
                    required: true
                }
            },
            messages: {
                'name': {
                    required: $('#hidNameRequired').val()
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

            formData = formData;

            $.ajax({
                url: ajaxUrl + '/Options/CreateMaintenance',
                type: "POST",
                dataType: "json",
                data: formData,
                async: false,
                success: function (response) {
                    addDone();
                },
                error: function (xhr, status, error) {
                    maintenanceFail(xhr, status, error);
                }
            });
        }
    });

    $('#submitEditForm').on('click', function () {
        var form = $('#editForm');

        form.validate({
            rules: {
                'name': {
                    required: true
                }
            },
            messages: {
                'name': {
                    required: $('#hidNameRequired').val()
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
                url: ajaxUrl + '/Options/UpdateMaintenance',
                type: "POST",
                dataType: "json",
                data: formData,
                async: false,
                success: function (response) {
                    editDone();
                },
                error: function (xhr, status, error) {
                    maintenanceFail(xhr, status, error);
                }
            });
        }
    });

    function updateMaintenanceList(selectedGarage) {
        $.ajax({
            url: ajaxUrl + '/Options/Maintenance/' + selectedGarage,
            type: "GET",
            dataType: "html",
            async: false,
            success: function (response) {
                $('#maintenance-list').empty().html(response);
                initTable();
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });
    }

    function addDone(data, status, xhr) {
        $('#addModal').modal('hide');
        Swal.fire({
            icon: 'success',
            title: $('#hidSaveSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true,
            onClose: () => {
                updateMaintenanceList($('#hidSelectedGarageId').val());
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
            timer: 2000,
            timerProgressBar: true,
            onClose: () => {
                updateMaintenanceList($('#hidSelectedGarageId').val());
            }
        });
    }

    function maintenanceFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }

    function delDone(data, status, xhr) {
        Swal.fire({
            icon: 'success',
            title: $('#hidDeleteSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true,
            onClose: () => {
                updateMaintenanceList($('#hidSelectedGarageId').val());
            }
        });
    }

    function delFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }
});