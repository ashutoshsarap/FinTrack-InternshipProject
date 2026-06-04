document.querySelectorAll(".delete-category-form")
    .forEach(form => {

        form.addEventListener("submit", function (e) {

            e.preventDefault();

            const categoryId = this.dataset.id;

            fetch(this.action, {
                method: "POST"
            })
                .then(response => response.json())
                .then(data => {

                    if (data.success) {

                        // Remove row from table
                        document
                            .getElementById(`category-${categoryId}`)
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