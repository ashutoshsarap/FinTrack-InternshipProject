// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", () => {

    const btn = document.getElementById("sidebar-toggle");

    if (btn) {

        btn.addEventListener("click", () => {

            document.body.classList.toggle("sidebar-collapsed");

        });

    }

});