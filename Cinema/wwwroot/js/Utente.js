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
            url: "/Admin/UtenteManagement/GetUtenti"
        },
        columns: [
            { data: "email", width: "20%" },
            { data: "ruolo", width: "20%" },
            {
                data: "utenteId",
                render: function (data) {
                    return (`
                        <div class="w-100 text-center">
                                <a href="/Admin/UtenteManagement/Edit?userId=${data}" class="btn btn-primary mx-1">
                                    <i class="bi bi-pencil-square"></i>  Modifica Ruolo</a>
                        </div>
                    `)
                },
                width: "20%",
            }
        ]
    });
}