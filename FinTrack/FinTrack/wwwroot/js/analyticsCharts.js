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

const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec"]
const xValuesMonthlyLabels = monthlyExpenseTrends.map(months => monthNames[months.month-1]);
const yValuescurrentYearMonthly = monthlyExpenseTrends.map(item => item.totalExpense);

console.log(xValuesMonthlyLabels);
console.log(yValuescurrentYearMonthly);
console.log(monthlyExpenseTrends);  

const ctx2 = document.getElementById('monthlyExpenseTrend');

console.log(ctx2);

const monthlyExpenseTrendChart = new Chart(ctx2, {
    type: 'line',

    data: {
        labels: xValuesMonthlyLabels,
        datasets: [{
            label : 'Monthly expense',
            data: yValuescurrentYearMonthly,
            borderwidth : 2
        }]
    }
});