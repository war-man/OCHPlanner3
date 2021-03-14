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

                    $('#editForm input[name=id]').val(data.id);
                    $('#editForm input[name=productNo]').val(data.productNo);
                    $('#editForm input[name=description]').val(data.description);
                    $('#editForm input[name=costPrice]').val(data.costPrice);
                    $('#editForm input[name=retailPrice]').val(data.retailPrice);

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
                                url: ajaxUrl + '/Options/DeleteProduct',
                                data: { id: data.id }
                            })
                                .done(delDone)
                                .fail(delFail);
                        }
                    });
                }
            },
            'pageLength'
        ],
        initComplete: function () {
            $('#ProductListTable_wrapper').find('div.dt-buttons').find('button').removeClass('dt-button').addClass('btn btn-outline-secondary btn-sm');
        }
    };

    initTable();

    function initTable() {

        if ($('#hidLanguage').val().toUpperCase() === "FR") {
            tableSettings.language = JSON.parse(datables_french());
        }

        var table = $('#ProductListTable').DataTable(tableSettings);

       
    }

    $.validator.addMethod("ProductNotExist", function (value, element) {
        var result = false;

        $.ajax({
            url: ajaxUrl + '/Options/ProductNotExist',
            type: "GET",
            async: false,
            data: {
                productNo: value,
                garageId: $('#hidSelectedGarageId').val()
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

    $('#submitAddForm').on('click', function () {
        var form = $('#addForm');

        form.validate({
            rules: {
                'productNo': {
                    required: true,
                    noSpace: true,
                    ProductNotExist: true
                },
                'description': {
                    required: true,
                    noSpace: true
                },
                'cost': {
                    required: true,
                    noSpace: true
                },
                'retail': {
                    required: true,
                    noSpace: true
                }
            },
            messages: {
                'productNo': {
                    required: $('#hidProductNoRequired').val(),
                    noSpace: $('#hidProductNoRequired').val(),
                    ProductNotExist: $('#hidProductNotExist').val()
                },
                'description': {
                    required: $('#hidDescriptionRequired').val(),
                    noSpace: $('#hidDescriptionRequired').val()
                },
                'cost': {
                    required: $('#hidCostRequired').val(),
                    noSpace: $('#hidCostRequired').val()
                },
                'retail': {
                    required: $('#hidRetailRequired').val(),
                    noSpace: $('#hidRetailRequired').val()
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
                url: ajaxUrl + '/Options/CreateProduct',
                type: "POST",
                data: formData,
                success: function (response) {
                    addDone();
                },
                error: function (xhr, status, error) {
                    fail(xhr, status, error);
                }
            });
        }
    });

    $('#submitEditForm').on('click', function () {
        var form = $('#editForm');

        form.validate({
            rules: {
                'productNo': {
                    required: true,
                    noSpace: true
                },
                'description': {
                    required: true,
                    noSpace: true
                },
                'cost': {
                    required: true,
                    noSpace: true
                },
                'retail': {
                    required: true,
                    noSpace: true
                }
            },
            messages: {
                'productNo': {
                    required: $('#hidProductNoRequired').val(),
                    noSpace: $('#hidProductNoRequired').val()
                },
                'description': {
                    required: $('#hidDescriptionRequired').val(),
                    noSpace: $('#hidDescriptionRequired').val()
                },
                'cost': {
                    required: $('#hidCostRequired').val(),
                    noSpace: $('#hidCostRequired').val()
                },
                'retail': {
                    required: $('#hidRetailRequired').val(),
                    noSpace: $('#hidRetailRequired').val()
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
                url: ajaxUrl + '/Options/UpdateProduct',
                type: "POST",
                data: formData,
                success: function (response) {
                    editDone();
                },
                error: function (xhr, status, error) {
                   fail(xhr, status, error);
                }
            });
        }
    });

    function updateProductList(selectedGarage) {
        $.ajax({
            url: ajaxUrl + '/Options/Product/' + selectedGarage,
            type: "GET",
            dataType: "html",
            async: false,
            success: function (response) {
                $('#product-list').empty().html(response);
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
            timer: 1000,
            timerProgressBar: true,
            onClose: () => {
                updateProductList($('#hidSelectedGarageId').val());
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
                updateProductList($('#hidSelectedGarageId').val());
            }
        });
    }

    function fail(xhr, status, error) {
        alert(xhr.responseText || error);
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
                updateProductList($('#hidSelectedGarageId').val());
            }
        });
    }

    function delFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }
});