document.querySelectorAll(".delete-transaction-form")
    .forEach(form => {

        form.addEventListener("submit", function (e) {

            e.preventDefault();

            const transactionId = this.dataset.id;

            fetch(this.action, {
                method: "POST"
            })
                .then(response => response.json())
                .then(data => {

                    if (data.success) {

                        // Remove row from table
                        document
                            .getElementById(`transaction-${transactionId}`)
                            .remove();
                        alert("Deleted")
                    } else {

                        alert(data.message);

                    }
                })
                .catch(error => {

                    console.error(error);
                    alert("Something went wrong");

                });

        });

    });

document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.getElementById('transactionSearch');
    if (!searchInput) return;

    searchInput.addEventListener('input', function () {
        const term = this.value.toLowerCase().trim();
        const rows = document.querySelectorAll('tbody tr');

        rows.forEach(function (row) {
            // columns: Date, Description, Category, PaymentMode, Amount, Actions
            const description = row.cells[1]?.textContent.toLowerCase() ?? '';
            const category = row.cells[2]?.textContent.toLowerCase() ?? '';
            const paymentMode = row.cells[3]?.textContent.toLowerCase() ?? '';
            const amount = row.cells[4]?.textContent.toLowerCase() ?? '';

            const matches = description.includes(term)
                || category.includes(term)
                || paymentMode.includes(term)
                || amount.includes(term);

            row.style.display = matches ? '' : 'none';
        });
    });
});
