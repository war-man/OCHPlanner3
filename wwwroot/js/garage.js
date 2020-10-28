$(document).ready(function () {
    var ajaxUrl = $('#HidRootUrl').val();

    var tableSettings = {
        dom: 'Bfrtip',
        select: {
            style: 'single',
            info: false
        },
        //"aoColumnDefs": [
        //    {
        //        "aTargets": [0],
        //        "visible": false
        //    },
        //    {
        //        "aTargets": [2],
        //        "className": 'text-center'
        //    }
        //],
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
                                url: ajaxUrl + '/Roles/DeleteRole',
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
});