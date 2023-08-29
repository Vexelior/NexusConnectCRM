var ctx = document.getElementById('employeeTaskProgressChart').getContext('2d');
if (ctx) {
    var taskProgressChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Completed', 'Pending'],
            datasets: [{
                data: [60, 40],
                backgroundColor: ['rgba(75, 192, 192, 0.8)', 'rgba(255, 99, 132, 0.8)']
            }],

        }
    });
}
