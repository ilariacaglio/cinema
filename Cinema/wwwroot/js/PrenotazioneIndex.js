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
            url: "/User/Prenotazione/GetAll"
        },
        columns: [
            { data: "emailUtente", width: "20%" },
            { data: "titoloFilm", width: "20%" },
            { data: "idSala", width: "5%" },
            { data: "dataS", width: "10%" },
            { data: "oraS", width: "10%" },
            {
                data: "pagato",
                render: function (data) {
                    if (data == true) {
                        return "Sì"
                    }
                    else {
                        return "No"
                    }
                },
                width: "5%"
            },
            { data: "prezzoTot", width: "9%" },
            {
                data: "id",
                render: function (data, type, row, meta) {
                    return (`
                        <div class="w-100 text-center">
                                <a href="/User/Prenotazione/Edit?prenotazioneId=${data}" class="btn btn-primary mx-1">
                                    <i class="bi bi-cash-coin"></i>  Pagato</a>
                                <a onClick=Delete("/User/Prenotazione/Delete?id=${data}") class="btn btn-danger mx-1">
                                    <i class="bi bi-trash-fill"></i>Delete</a>
                        </div>
                    `)
                },
                width: "20%"
            }
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