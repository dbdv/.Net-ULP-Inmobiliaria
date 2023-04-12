// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function hideModal() {
  document
    .querySelector(".actions-section")
    .classList.add("actions-section-hide");
  document.querySelector(".form").classList.add("hidden");
  document.querySelector(".confirm-delete").classList.add("hidden");
  document.querySelector(".spinner").classList.remove("hidden");
}
