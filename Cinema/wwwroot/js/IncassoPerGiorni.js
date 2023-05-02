const ctx = document.getElementById('myChart');
var myChart = null;

document.addEventListener("load", CaricaDati());

function CaricaDati() {
    let dataInizio = new Date().toISOString().slice(0, 10);
    let dataFine = new Date().toISOString().slice(0, 10);
    let url = "https://localhost:7214/Admin/Statistiche/JsonIncassoPerGiorni?inizio=" + dataInizio + "&fine=" + dataFine;
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
                    data: dataArray,
                    borderWidth: 2,
                    borderRadius: Number.MAX_VALUE,
                    borderSkipped: false,
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    }
                }
            }
        });
    }
    xhttp.open("GET", url, true);
    xhttp.send();
}

function Grafico() {
    let dataInizio = document.getElementById('dataValueInizio').value;
    let dataFine = document.getElementById('dataValueFine').value;
    if (dataInizio == "") {
        dataInizio = new Date().toISOString().slice(0, 10);
    }
    if (dataFine == "") {
        dataFine = new Date().toISOString().slice(0, 10);
    }
    let url1 = "https://localhost:7214/Admin/Statistiche/JsonIncassoPerGiorni?inizio=" + dataInizio + "&fine=" + dataFine;
    console.log(url1);
    const xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        obj = JSON.parse(this.responseText);
        let labelsArray = new Array();
        let dataArray = new Array();
        for (let i = 0; i < obj['data'].length; i++) {
            labelsArray.push(obj['data'][i]['giorno']);
            dataArray.push(obj['data'][i]['totaleGiorno']);
        }
        console.log(labelsArray);
        console.log(dataArray);
        myChart.clear();
        myChart.destroy();
        myChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: labelsArray,
                datasets: [{
                    label: 'Incasso giornaliero',
                    data: dataArray,
                    cubicInterpolationMode: 'monotone',
                    pointStyle: 'circle',
                    pointRadius: 3,
                    pointHoverRadius: 5
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
