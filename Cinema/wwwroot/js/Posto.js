var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    //per i dettagli si veda qui: https://datatables.net/manual/ajax
    dataTable = $('#tblData').DataTable({
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.2/i18n/it-IT.json"
        },
        ajax: {
            url: "/Admin/Posto/GetAll"
        },
        columns: [
            { data: "fila", width: "20%" },
            { data: "numero", width: "20%" },
            { data: "costo", width: "20%" },
            { data: "idSala", width: "20%" },
            {
                data: "id",
                render: function (data) {
                    return (`
                        <div class="w-100 text-center">
                                <a href="/Admin/Posto/Upsert?id=${data}" class="btn btn-primary mx-1">
                                    <i class="bi bi-pencil-square"></i>Edit</a>
                                <a onClick=Delete("/Admin/Posto/Delete?id=${data}") class="btn btn-danger mx-1">
                                    <i class="bi bi-trash-fill"></i>Delete</a>
                        </div>
                    `)
                },
                width: "20%",
            },
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Sicuro?',
        text: "L'azione è irreversibile!'",
        icon: 'info',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}