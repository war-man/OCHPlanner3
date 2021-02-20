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
            },
            {
                "aTargets": [4,7,8,9],
                "className": 'text-center'
            }
        ],
        buttons: [
            {
                text: $('#hidNewButton').val(),
                action: function (e, dt, button, config) {
                    location.href = ajaxUrl + '/Garage/Create';
                }
            },
            {
                extend: "selectedSingle",
                text: $('#hidEditButton').val(),
                action: function (e, dt, button, config) {
                    var data = dt.row({ selected: true }).data();
                    location.href = ajaxUrl + '/Garage/Edit/' + data.id;
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
                                url: ajaxUrl + '/Garage/Delete',
                                data: { garageId: data.id }
                            })
                            .done(delDone)
                            .fail(delFail);
                        }
                    });
                }
            },
            'pageLength'
        ],
        language: {
            buttons: {
                pageLength: '%d'
            }
        },
        rowCallback: function (row, data, index) {
            if (parseInt(data.stock) !== 0 && parseInt(data.alert) && parseInt(data.stock) <= parseInt(data.alert) ) {
                $(row).find('td:eq(11)').css('background-color', '#f55742');
            }
        },
        initComplete: function () {
            $('#garagesTable_wrapper').find('div.dt-buttons').find('button').removeClass('dt-button').addClass('btn btn-outline-secondary btn-sm');
        }
    };

    initTable();

    function initTable() {

        if ($('#hidLanguage').val().toUpperCase() === "FR") {
            tableSettings.language = JSON.parse(datables_french());
        }

        var table = $('#garagesTable').DataTable(tableSettings);

        table.on('select deselect', function (e, dt, type, indexes) {
            var rowData = table.rows(indexes).data().toArray();

            //Cannot delete SuperAdmin/Administrator/Mobile Role
            if (rowData[0]['name'] === 'SuperAdmin' || rowData[0]['name'] === 'Administrator') {
                table.button(1).enable(false);
                table.button(2).enable(false);
            }
            else if (rowData[0]['usercount'] > 0) //Cannot delete if user in group
                table.button(2).enable(false);
            else {
                table.button(1).enable(true);
                table.button(2).enable(true);
            }
        });
    }

    function updateGarageList() {
        $.ajax({
            url: ajaxUrl + '/Garage/list',
            type: "GET",
            dataType: "html",
            async: false,
            success: function (response) {
                $('#garage-list').empty().html(response);
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
            timerProgressBar: true
        });
        updateGarageList();
    }

    function delFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }

});