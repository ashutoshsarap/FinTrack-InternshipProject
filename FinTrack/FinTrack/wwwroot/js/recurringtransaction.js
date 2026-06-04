
document.querySelectorAll(".delete-recurringtransaction-form")
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
                        document
                            .getElementById(`recurringtransaction-${transactionId}`)
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