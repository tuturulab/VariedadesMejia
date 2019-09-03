


var value1 = document.getElementById("monto1").value;
var value2 = document.getElementById("monto2").value;


data = {
    datasets: [{
        data: [value1, value2],
        backgroundColor: [ 'yellow', 'blue']
    }],

    // These labels appear in the legend and in the tooltips when hovering different arcs
    labels: [
        'Monto Total Esperado',
        'Monto Total Abonado'
    ]
};


var ctx = document.getElementById("pie");

var myPieChart = new Chart(ctx, {
    type: 'pie',
    data: data,
    //options: options
});


