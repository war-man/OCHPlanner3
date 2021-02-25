$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();
    var table;

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
                    UpdateProductTotalCost();
                    UpdateTotalSection();
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
                    formData = formData + '&GarageId=' + $('#hidSelectedGarageId').val();

                    $.ajax({
                        url: ajaxUrl + '/MaintenanceType/Create',
                        type: "POST",
                        data: formData
                    })
                        .done(addDone)
                        .fail(ajaxFail);
                },
                error: function (xhr, status, error) {
                    alert('Error');
                }
            });


        }
    });

    $(document).on("blur", ".updTotal", function () {
        UpdateTotalSection();
    });
        
    $(document).on("blur", ".updWork", function () {
        UpdateWorkSection();
    });

    $(document).on("blur", ".updProfit", function () {
        UpdateProfit();
    });


    $(document).on("blur", ".toFixed", function () {
        var value = $(this).val();
        if (value !== '') {
            $(this).val(parseFloat(value).toFixed(2));
        }
    });

    function UpdateProductTotalCost() {
        var totalCost = 0;
        var totalRetail = 0;

        table.rows().eq(0).each(function (index) {
            var row = table.row(index);

            var data = row.data();
            totalCost = totalCost + (data.quantity * data.cost);
            totalRetail = totalRetail + (data.quantity * data.retail);
        });

        $('input[name="ProductCost"]').val(totalCost.toFixed(2));
        $('input[name="ProductRetail"]').val(totalRetail.toFixed(2));
    }

    function UpdateWorkSection() {
        var totalCost = 0;
        var totalRetail = 0;

        if ($('input[name="WorkTime"]').val() !== ''
            && $('input[name="HourlyRateCost"]').val()
            && $('input[name="HourlyRateBillable"]').val()) {
                totalCost = parseFloat($('input[name="WorkTime"]').val()) * parseFloat($('input[name="HourlyRateCost"]').val());
                totalRetail = parseFloat($('input[name="WorkTime"]').val()) * parseFloat($('input[name="HourlyRateBillable"]').val());

            $('input[name="WorkCost"]').val(totalCost.toFixed(2));
            $('input[name="WorkTotal"]').val(totalRetail.toFixed(2));
        }

        UpdateTotalSection();
    }

    function UpdateTotalSection() {
        var totalCost = 0;
        var totalRetail = 0;

        //cost
        totalCost = totalCost + ($('input[name="ProductCost"]').val() === '' ? parseFloat(0) : parseFloat($('input[name="ProductCost"]').val()));
        totalCost = totalCost + ($('input[name="MaterialCost"]').val() === '' ? parseFloat(0) : parseFloat($('input[name="MaterialCost"]').val()));
        totalCost = totalCost + ($('input[name="WorkCost"]').val() === '' ? parseFloat(0) : parseFloat($('input[name="WorkCost"]').val()));

        $('input[name="MaintenanceTotalCost"]').val(totalCost.toFixed(2));

        //retail
        totalRetail = totalRetail + ($('input[name="ProductRetail"]').val() === '' ? parseFloat(0) : parseFloat($('input[name="ProductRetail"]').val()));
        totalRetail = totalRetail + ($('input[name="MaterialRetail"]').val() === '' ? parseFloat(0) : parseFloat($('input[name="MaterialRetail"]').val()));
        totalRetail = totalRetail + ($('input[name="WorkTotal"]').val() === '' ? parseFloat(0) : parseFloat($('input[name="WorkTotal"]').val()));

        $('input[name="MaintenanceTotalRetail"]').val(totalRetail.toFixed(2));
        $('input[name="MaintenanceTotalPrice"]').val(totalRetail.toFixed(2));

        UpdateProfit();

    }

    function UpdateProfit() {
        var totalCost = 0;
        var totalRetail = 0;

        //cost
        totalCost = totalCost + ($('input[name="ProductCost"]').val() === '' ? parseFloat(0) : parseFloat($('input[name="ProductCost"]').val()));
        totalCost = totalCost + ($('input[name="MaterialCost"]').val() === '' ? parseFloat(0) : parseFloat($('input[name="MaterialCost"]').val()));
        totalCost = totalCost + ($('input[name="WorkCost"]').val() === '' ? parseFloat(0) : parseFloat($('input[name="WorkCost"]').val()));

        //Profit
        var salePrice = parseFloat($('input[name="MaintenanceTotalPrice"]').val());

        var profitAmount = (salePrice - totalCost).toFixed(2);
        $('input[name="ProfitAmount"]').val(profitAmount);

        var profitPercent = (((salePrice - totalCost) / totalCost) * 100).toFixed(2);
        $('input[name="ProfitPercentage"]').val(profitPercent);
    }

    function resetAddProduct() {
        $('#SelectedProduct').val('').trigger('change.select2');
        $('input[name="Quantity"]').val('')
    }

    function initTable() {

        if ($('#hidLanguage').val().toUpperCase() === "FR") {
            tableSettings.language = JSON.parse(datables_french());
        }

        table = $('#SelectedProductTable').DataTable(tableSettings);
    }

    function addDone(data, status, xhr) {
        Swal.fire({
            icon: 'success',
            title: $('#hidSaveSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 1000,
            timerProgressBar: true,
            onClose: () => {
                location.href = ajaxUrl + "/MaintenanceType/" + $('#hidSelectedGarageId').val()
            }
        });
    }

    function ajaxFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }
});