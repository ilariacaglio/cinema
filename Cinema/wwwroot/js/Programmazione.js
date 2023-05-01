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
            url: "/User/Home/ProgrammazioneGet"
        },
        columns: [
            { data: "titoloFilm", width: "30%" },
            { data: "genere", width: "10%" },
            {
                data: "dataInizio",
                render: function (data) {
                    if (data == "0001-01-01") {
                        return "Non ci sono spettacoli in programma";
                    } else {
                        return data;
                    }
                },
                width: "25%"
            },
            {
                data: "dataFine",
                render: function (data) {
                    if (data == "0001-01-01") {
                        return "";
                    } else {
                        return data;
                    }
                },
                width: "15%"
            },
            {
                data: "idFilm",
                render: function (data) {
                    return (`
                        <div class="w-100 text-center">
                                <a href="/Admin/Film/Details?id=${data}" class="btn btn-primary mx-1">
                                    <i class="bi bi-arrow-right-circle"></i>  Vedi tutti i dettagli</a>
                        </div>
                    `)
                },
                width: "20%",
            }
        ]
    });
}
