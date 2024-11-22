window.addEventListener("beforeunload", function () {
    document.getElementById("loader").style.display = "inline"; // Hiện loader khi chuẩn bị tải lại trang
});

window.addEventListener("load", function () {
    document.getElementById("loader").style.display = "none"; // Ẩn loader khi trang đã tải
});

