    document.addEventListener("DOMContentLoaded", function () {
        const deleteButtons = document.querySelectorAll(".btn-delete");
        const deleteForm = document.getElementById("globalDeleteForm");
        const deleteModal = new bootstrap.Modal(document.getElementById("globalDeleteModal"));

        deleteButtons.forEach(button => {
            button.addEventListener("click", function () {
                const actionUrl = this.getAttribute("data-action-url");
                deleteForm.setAttribute("action", actionUrl);
                deleteModal.show();
            });
        });
    });