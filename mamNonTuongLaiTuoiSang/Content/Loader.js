window.addEventListener("beforeunload", function () {
    document.getElementById("loader").style.display = "inline-grid";
});

window.addEventListener("load", function () {
    document.getElementById("loader").style.display = "none";
});