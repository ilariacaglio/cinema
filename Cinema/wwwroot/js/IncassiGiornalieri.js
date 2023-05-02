const ctx = document.getElementById('myChart');
var myChart = null;
let url = "https://localhost:7214/Admin/Statistiche/JsonIncassiGiornalieri?data="

document.addEventListener("load", CaricaDati());

function CaricaDati() {
    const xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        obj = JSON.parse(this.responseText);
        let labelsArray = new Array();
        let dataArray = new Array();
        for (let i = 0; i < obj['data'].length; i++) {
            labelsArray.push(obj['data'][i]['titoloFilm']);
            dataArray.push(obj['data'][i]['incasso']);
        }
        myChart = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: labelsArray,
                datasets: [{
                    label: 'Incasso giornaliero',
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


function Grafico() {
    let dataIncassi = document.getElementById('dataValue').value;
    if (dataIncassi == "") {
        dataIncassi = new Date().toISOString().slice(0, 10);
    }
    let url1 = "https://localhost:7214/Admin/Statistiche/JsonIncassiGiornalieri?data=" + dataIncassi;
    const xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        obj = JSON.parse(this.responseText);
        let labelsArray = new Array();
        let dataArray = new Array();
        for (let i = 0; i < obj['data'].length; i++) {
            labelsArray.push(obj['data'][i]['titoloFilm']);
            dataArray.push(obj['data'][i]['incasso']);
        }
        myChart.clear();
        myChart.destroy();
        myChart = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: labelsArray,
                datasets: [{
                    label: 'Incasso giornaliero',
                    data: dataArray

                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false
            }
        });
    }
    xhttp.open("GET", url1, true);
    xhttp.send();
}