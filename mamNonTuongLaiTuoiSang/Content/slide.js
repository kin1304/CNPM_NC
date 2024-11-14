let slideIndex = 0; // Khởi tạo chỉ số slide
showSlides(); // Hiện slide đầu tiên

function showSlides() {
    let slides = document.getElementsByClassName("mySlides");
    for (let i = 0; i < slides.length; i++) {
        slides[i].style.display = "none"; // Ẩn tất cả các slide
    }
    slideIndex++; // Tăng chỉ số slide
    if (slideIndex > slides.length) { slideIndex = 1 } // Quay lại slide đầu tiên
    slides[slideIndex - 1].style.display = "block"; // Hiện slide hiện tại
    setTimeout(showSlides, 3000); // Chuyển slide sau 3 giây
}

function plusSlides(n) {
    slideIndex += n; // Thay đổi chỉ số slide
    showSlides(); // Cập nhật slide hiển thị
}