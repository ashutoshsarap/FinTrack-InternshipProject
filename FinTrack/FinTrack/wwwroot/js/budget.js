
document.querySelectorAll(".delete-budget-form")
    .forEach(form => {

        form.addEventListener("submit", function (e) {

            e.preventDefault();
            const budgetId = this.dataset.id;

            fetch(this.action, {
                method: "POST"
            })
                .then(response => response.json())
                .then(data => {

                    if (data.success) {
                        console.log(budgetId);
                        console.log(document.getElementById(`budget-${budgetId}`));
                        document
                            .getElementById(`budget-${budgetId}`)
                            .remove();

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