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
                    location.href = ajaxUrl + '/MaintenancePlan/Create/' + $('#hidSelectedGarageId').val();
                }
            },
            {
                extend: "selectedSingle",
                text: $('#hidEditButton').val(),
                action: function (e, dt, button, config) {
                    var data = dt.row({ selected: true }).data();
                    location.href = ajaxUrl + '/MaintenancePlan/Edit/' + data.id;
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
                                url: ajaxUrl + '/MaintenancePlan/Delete',
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
            $('#MaintenancePlanListTable_wrapper').find('div.dt-buttons').find('button').removeClass('dt-button').addClass('btn btn-outline-secondary btn-sm');
        }
    };

    initTable();

    function initTable() {

        if ($('#hidLanguage').val().toUpperCase() === "FR") {
            tableSettings.language = JSON.parse(datables_french());
        }

        var table = $('#MaintenancePlanListTable').DataTable(tableSettings);
    }

    function updateMaintenanceList(selectedGarage) {
        $.ajax({
            url: ajaxUrl + '/MaintenancePlanList/' + selectedGarage,
            type: "GET",
            dataType: "html",
            async: false,
            success: function (response) {
                $('#maintenancePlan-list').empty().html(response);
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
                updateMaintenanceList($('#hidSelectedGarageId').val());
            }
        });
    }

    function ajaxFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }
});