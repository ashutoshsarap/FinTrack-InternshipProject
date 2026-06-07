const categoryLabels =
    categoryExpenseData.map(item => item.categoryName);

const categoryValues =
    categoryExpenseData.map(item => item.totalAmount);

// Modern dashboard color palette
const colorPalette = [
    '#3b82f6', // blue
    '#10b981', // emerald
    '#f59e0b', // amber
    '#ef4444', // red
    '#8b5cf6', // violet
    '#06b6d4', // cyan
    '#ec4899', // pink
    '#84cc16', // lime
    '#f97316', // orange
    '#64748b'  // slate
];

const categoryColors =
    categoryExpenseData.map((_, index) =>
        colorPalette[index % colorPalette.length]
    );

// Center text plugin
const centerTextPlugin = {
    id: 'centerText',
    afterDraw(chart) {

        const { ctx } = chart;

        const meta = chart.getDatasetMeta(0);

        if (!meta.data.length) return;

        const centerX = meta.data[0].x;
        const centerY = meta.data[0].y;

        const total =
            chart.data.datasets[0].data.reduce(
                (sum, value) => sum + Number(value),
                0
            );

        ctx.save();

        ctx.textAlign = 'center';
        ctx.textBaseline = 'middle';

        // Amount
        ctx.font = 'bold 24px Inter, sans-serif';
        ctx.fillStyle = '#1f2937';

        ctx.fillText(
            `₹${total.toLocaleString()}`,
            centerX,
            centerY - 10
        );

        // Subtitle
        ctx.font = '13px Inter, sans-serif';
        ctx.fillStyle = '#6b7280';

        ctx.fillText(
            'Total Spent',
            centerX,
            centerY + 18
        );

        ctx.restore();
    }
};

const ctx =
    document.getElementById('expenseCategoryChart');

new Chart(ctx, {

    type: 'doughnut',

    plugins: [centerTextPlugin],

    data: {

        labels: categoryLabels,

        datasets: [{

            label: 'Amount Spent',

            data: categoryValues,

            backgroundColor: categoryColors,

            borderColor: '#ffffff',

            borderWidth: 3,

            hoverOffset: 12
        }]
    },

    options: {

        responsive: true,

        maintainAspectRatio: false,

        cutout: '60%',

        plugins: {

            legend: {

                position: 'right',

                labels: {

                    usePointStyle: true,

                    pointStyle: 'circle',

                    padding: 18,

                    font: {
                        size: 13
                    }
                }
            },

            tooltip: {

                callbacks: {

                    label: function (context) {

                        const total =
                            context.dataset.data.reduce(
                                (a, b) => a + Number(b),
                                0
                            );

                        const value = context.raw;

                        const percentage =
                            ((value / total) * 100)
                                .toFixed(1);

                        return `${context.label}: ₹${value.toLocaleString()} (${percentage}%)`;
                    }
                }
            }
        }
    }
});