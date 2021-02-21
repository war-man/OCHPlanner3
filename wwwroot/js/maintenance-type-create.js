$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    $('select[name="SelectedProduct"]').select2();

    var tableSettings = {
        dom: 'Blrtip',
        select: {
            style: 'single',
            info: false
        },
        "bLengthChange": false,
        "aoColumnDefs": [
            {
                "aTargets": [0],
                "visible": false
            }
        ],
        "order": [[1, "asc"]],
        "paging": false,
        "bInfo": false,
        buttons: [
            //    {
            //        text: $('#hidNewButton').val(),
            //        action: function (e, dt, button, config) {
            //            location.href = ajaxUrl + '/MaintenanceType/Create/' + $('#hidSelectedGarageId').val();
            //        }
            //    },
            //    {
            //        extend: "selectedSingle",
            //        text: $('#hidEditButton').val(),
            //        action: function (e, dt, button, config) {
            //            var data = dt.row({ selected: true }).data();
            //            $('#editError').hide();
            //            $('#editForm').trigger('reset');

            //            $('#editForm input[name=id]').val(data.id);
            //            $('#editForm input[name=productNo]').val(data.productNo);
            //            $('#editForm input[name=description]').val(data.description);
            //            $('#editForm input[name=costPrice]').val(data.costPrice);
            //            $('#editForm input[name=retailPrice]').val(data.retailPrice);

            //            $('#editModal').modal({ backdrop: 'static' });
            //        }
            //    },
            {
                extend: "selectedSingle",
                text: $('#hidDeleteButton').val(),
                action: function (e, dt, button, config) {
                    var data = dt.row({ selected: true }).data();

                    $.ajax({
                        type: 'DELETE',
                        url: ajaxUrl + '/MaintenanceType/DeleteProduct',
                        data: { id: data.id },
                        success: function (response) {
                            $('#product-list').empty().html(response);
                            initTable();
                        },
                        error: function (xhr, status, error) {
                            alert('Error');
                        }
                    });

                }
            }
        ]
        //initComplete: function () {
        //    $('#MaintenanceTypeListTable_wrapper').find('div.dt-buttons').find('button').removeClass('dt-button').addClass('btn btn-outline-secondary btn-sm');
        //}
    };

    initTable();

    $(document).on("click", "#btnAddProduct", function () {
       
        var selected = $('#SelectedProduct').select2("val");

        //validation
        var hasError = false;
        if (selected === '') {
            hasError = true;
            $('#SelectedProduct-error').show();
        }
        if ($('input[name="Quantity"]').val() === '') {
            hasError = true;
            $('#Quantity-error').show();
        }

        if (!hasError) {
            var productToAdd = {
                Product: {
                    Id: selected
                },
                Quantity: $('input[name="Quantity"]').val()
            };

            $.ajax({
                url: ajaxUrl + '/MaintenanceType/AddProduct',
                type: "POST",
                data: {
                    product: productToAdd
                },
                //dataType: "json",
                success: function (response) {
                    $('#SelectedProduct-error').hide();
                    $('#Quantity-error').hide();
                    $('#product-list').empty().html(response);
                    resetAddProduct();
                    initTable();
                },
                error: function (xhr, status, error) {
                    alert('Error');
                }
            });
        }
    });

    $(document).on("click", "#btnSave", function () {

        var form = $('#createMaintenanceTypeForm');

        $('input[name="SelectedProduct"]').addClass('ignoreClass');
        $('input[name="Quantity"]').addClass('ignoreClass');

        form.validate({
            ignore: ".ignoreClass",
            rules: {
                'Code': {
                    required: true
                },
                'Name': {
                    required: true
                },
                'MaintenanceTotalPrice': {
                    required: true
                }
            },
            messages: {
                'Code': {
                    required: $('#hidCodeRequired').val()
                },
                'Name': {
                    required: $('#hidNameRequired').val()
                },
                'MaintenanceTotalPrice': {
                    required: $('#hidMaintenanceTotalPriceRequired').val()
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

            //Get selected products
            $.ajax({
                url: ajaxUrl + '/MaintenanceType/SelectedProducts',
                type: "GET",
                //dataType: "json",
                success: function (response) {

                    formData = formData + '&ProductString=' + response;

                    $.ajax({
                        url: ajaxUrl + '/MaintenanceType/Create',
                        type: "POST",
                        data: formData,
                        //dataType: "json",
                        success: function (response) {
                            location.href = ajaxUrl + "/MaintenanceType/" + $('#hidSelectedGarageId').val()
                        },
                        error: function (xhr, status, error) {
                            alert('Error');
                        }
                    });
                },
                error: function (xhr, status, error) {
                    alert('Error');
                }
            });

           
        }
    });

    function resetAddProduct() {
        $('#SelectedProduct').val('').trigger('change.select2');
        $('input[name="Quantity"]').val('')
    }

    function initTable() {

        if ($('#hidLanguage').val().toUpperCase() === "FR") {
            tableSettings.language = JSON.parse(datables_french());
        }

        var table = $('#SelectedProductTable').DataTable(tableSettings);
    }


});