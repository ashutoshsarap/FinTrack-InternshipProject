const categoryLabels = categoryAnalysis.map(item => item.categoryName);
const currentMonthCategoryValues = categoryAnalysis.map(item => item.totalAmountSpentCurrentMonth);
const previousMonthCategoryValues = categoryAnalysis.map(item => item.totalAmountSpentPreviousMonth);

const ctx = document.getElementById('categoryAnalytics');

const categoryAnalyticsChart = new Chart(ctx, {
    type: 'bar',

    data: {
        labels: categoryLabels,
        datasets: [{
            label: 'Current Month',
            data: currentMonthCategoryValues
        },
        {
            label: 'Previous Month',
            data: previousMonthCategoryValues
        }]
    }
});