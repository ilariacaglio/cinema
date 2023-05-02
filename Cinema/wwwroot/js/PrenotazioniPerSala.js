const ctx = document.getElementById('myChart');
var myChart = null;
let url = "https://localhost:7214/Admin/Statistiche/JsonPrenotazioniPerSala"

document.addEventListener("load", CaricaDati());

function CaricaDati() {
    const xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        obj = JSON.parse(this.responseText);
        let labelsArray = new Array();
        let dataArray = new Array();
        for (let i = 0; i < obj['data'].length; i++) {
            labelsArray.push(obj['data'][i]['idSala']);
            dataArray.push(obj['data'][i]['numeroPrenotazioni']);
        }
        myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labelsArray,
                datasets: [{
                    label: 'Numero',
                    data: dataArray
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false
            }
        });
    }
    xhttp.open("GET", url, true);
    xhttp.send();
}
