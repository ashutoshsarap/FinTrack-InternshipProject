const categoryLabels = categoryExpenseData.map(item => item.categoryName);
const categoryValues = categoryExpenseData.map(item => item.totalAmount);

const ctx = document.getElementById('expenseCategoryChart');

const categoryExpenseChart = new Chart(ctx, {
    type: 'doughnut',

    data: {
        labels: categoryLabels,
        datasets: [{
            label: 'Amount spent',
            data: categoryValues
        }],
        hoverOffset: 4
    },

    options: {
        plugins: {
                    legend: {
                            position: 'right'
            }
        }
    }
);