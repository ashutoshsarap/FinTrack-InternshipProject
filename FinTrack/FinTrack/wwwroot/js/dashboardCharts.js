const categoryLabels =
    categoryExpenseData.map(item => item.categoryName);

const categoryValues =
    categoryExpenseData.map(item => item.totalAmount);

const ctx =
    document.getElementById('expenseCategoryChart');

new Chart(ctx, {

    type: 'doughnut',

    data: {

        labels: categoryLabels,

        datasets: [{

            label: 'Amount Spent',

            data: categoryValues,

            borderWidth: 0,

            hoverOffset: 8,

            radius: '100%'

        }]
    },

    options: {

        responsive: true,

        maintainAspectRatio: false,

        cutout : '50%',

        plugins: {

            legend: {

                position: 'right',

                labels: {

                    usePointStyle: true,
                    pointStyle: 'circle',
                    padding: 20

                }
            }
        }
    }
});