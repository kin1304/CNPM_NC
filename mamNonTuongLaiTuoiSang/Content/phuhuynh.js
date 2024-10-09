// Lấy các nút trong Sidebar
const btnProfile = document.getElementById("btnProfile");
const btnNo1 = document.getElementById("btnNo1");
const btnNo2 = document.getElementById("btnNo2");
const btnNo3 = document.getElementById("btnNo3");
const btnNo4 = document.getElementById("btnNo4");
const btnNo5 = document.getElementById("btnNo5");
const btnLogout = document.getElementById("btnLogout");

// Lấy phần hiển thị nội dung
const contentArea = document.getElementById("contentArea");

// Modal logout
const logoutModal = document.getElementById("logoutModal");
const cancelLogout = document.getElementById("cancelLogout");
const confirmLogout = document.getElementById("confirmLogout");


btnProfile.addEventListener("click", function () {
    const parentId = "PH001"; 

    fetch(`/api/PhuHuynhs/${parentId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Không thể lấy dữ liệu phụ huynh");
            }
            return response.json();
        })
        .then(data => {
            const gioiTinh = data.gioiTinh ? "Nam" : "Nữ";

            contentArea.innerHTML = `
                <h2 class="text-2xl font-bold mb-4">Thông tin phụ huynh</h2>
                <p><strong>ID:</strong> ${data.idPH}</p>
                <p><strong>Họ và tên:</strong> ${data.hoVaTen}</p>
                <p><strong>Địa chỉ:</strong> ${data.diaChi}</p>
                <p><strong>Giới tính:</strong> ${gioiTinh}</p>
                <p><strong>Nghề nghiệp:</strong> ${data.ngheNghiep}</p>
                <p><strong>Năm sinh:</strong> ${data.namSinh}</p>
                <p><strong>Email:</strong> ${data.email}</p>
                <p><strong>Số điện thoại:</strong> ${data.sdt}</p>
            `;
        })
        .catch(error => {
            console.error("Error:", error);
            contentArea.innerHTML = `
                <p class="text-red-500">Có lỗi xảy ra: ${error.message}</p>
            `;
        });
});

// Sự kiện khi click vào các nút khác
btnNo1.addEventListener("click", function () {
    contentArea.innerHTML = `<h2 class="text-2xl font-bold mb-4">Chức năng số 1</h2>`;
});

btnNo2.addEventListener("click", function () {
    contentArea.innerHTML = `<h2 class="text-2xl font-bold mb-4">Chức năng số 2</h2>`;
});

btnNo3.addEventListener("click", function () {
    contentArea.innerHTML = `<h2 class="text-2xl font-bold mb-4">Chức năng số 3</h2>`;
});

btnNo4.addEventListener("click", function () {
    contentArea.innerHTML = `<h2 class="text-2xl font-bold mb-4">Chức năng số 4</h2>`;
});

btnNo5.addEventListener("click", function () {
    contentArea.innerHTML = `<h2 class="text-2xl font-bold mb-4">Chức năng số 5</h2>`;
});

// Sự kiện khi click vào nút Đăng xuất
btnLogout.addEventListener("click", function () {
    logoutModal.classList.remove("hidden");
});

// Hủy bỏ đăng xuất
cancelLogout.addEventListener("click", function () {
    logoutModal.classList.add("hidden");
});

// Xác nhận đăng xuất
confirmLogout.addEventListener("click", function () {
    window.location.href = "http://localhost:5005";
});
