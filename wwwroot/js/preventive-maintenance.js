﻿$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    $('select[name="VinCode"]').select2({
        tags: true
    });

    //Detect ENTER key
    $(document).on("keypress", 'input[name="VinCode"]', function (e) {
        if (e.which == 13) {
            $('input[name="Description"]').focus();
        }
    });

    $(document).on("click", '#btnVehicleDetail', function (e) {
        if ($('input[name="VinCode"]').val() !== '') {
            location.href = ajaxUrl + "/Vehicle/" + $('select[name="VinCode"]').select2('val')
        }
    });

    $('select[name="VinCode"]').on('select2:select', function (e) {
        if ($(this).val().trim() === '')
            return false;

        var VIN = $(this).val();

        $.ajax({
            url: ajaxUrl + '/Vehicle/GetVehicleByVIN',
            type: "GET",
            data: {
                vin: VIN
            },
            async: false,
            success: function (response) {
                if (response.VinCode == null || response.IsVINDecode) {
                    //Display VIN not found
                    swal.fire({
                        title: $('#hidVINNotFoundTitle').val(),
                        text: $('#hidVINNotFoundText').val(),
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#1E7E34',
                        confirmButtonText: $('#hidCreateVehicleButton').val()
                    }).then(function (result) {
                        if (result.value) {
                            location.href = ajaxUrl + "/Vehicle/" + VIN

                        }
                    });
                }

                UpdateUi(response);
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });

    });

    function UpdateUi(response) {
       
        $('input[name="Description"]').val(response.Description);
        $('input[name="LastServiceDate"]').val('');
        $('input[name="MonthlyMileage"]').val(response.MonthlyMileage);
        $('input[name="EntryDate"]').val(response.EntryDate);
        $('input[name="SelectedOilDisplay"]').val(response.SelectedOilDisplay);


        $('input[name="UnitNo"]').val(response.UnitNo);
        $('input[name="LicencePlate"]').val(response.LicencePlate);
        $('input[name="Color"]').val(response.Color);
        $('input[name="Odometer"]').val(response.Odometer);

        $('input[name="OwnerName"]').val(response.OwnerName);
        $('input[name="OwnerCompany"]').val(response.OwnerCompany);
        $('input[name="OwnerAddress"]').val(response.OwnerAddress);
        $('input[name="OwnerPhone"]').val(response.OwnerPhone);
        $('input[name="OwnerEmail"]').val(response.OwnerEmail);

        $('input[name="DriverName"]').val(response.DriverName);
        $('input[name="DriverPhone"]').val(response.DriverPhone);
        $('input[name="DriverCellphone"]').val(response.DriverCellphone);
        $('input[name="DriverEmail"]').val(response.DriverEmail);
        $('input[name="DriverNotes"]').val(response.DriverNotes);

        //Update programs
        $.ajax({
            url: ajaxUrl + '/Vehicle/Programs',
            type: "GET",
            data: {
                vehicleId: response.Id,
                displayOnlySelected: true
            },
            async: false,
            success: function (response) {
                $('#program-list').empty().html(response);
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });
    }

});