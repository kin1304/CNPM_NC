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
    const parentId = document.getElementById("id_PH").textContent;

    fetch(`/api/PhuHuynhs/${parentId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Không thể lấy dữ liệu phụ huynh");
            }
            return response.json();
        })
        .then(data => {
            const gioiTinh = data.gioiTinh ? "Nam" : "Nữ";

            // Cập nhật nội dung vào form
            contentArea.innerHTML = `
                <div class="max-w-4xl mx-auto border border-gray-300 p-4">
                    <div class="flex">
                        <div class="w-1/4">
                            <img alt="Profile picture" class="rounded-full" height="100" src="https://storage.googleapis.com/a1aa/image/74PNLrMcK6ZDDF9oZ0uvDk62LAegmbp9zfgAqGEZJ91vopmTA.jpg" width="100"/>
                        </div>
                        <div class="w-3/4">
                            <div class="grid grid-cols-2 gap-4">
                                <div>
                                    <h2 class="text-red-600 font-bold">Thông tin phụ huynh</h2>
                                    <p><span class="font-bold">Họ tên:</span> ${data.hoTen}</p>
                                    <p><span class="font-bold">Địa chỉ:</span> ${data.diaChi}</p>
                                    <p><span class="font-bold">Giới tính:</span> ${gioiTinh}</p>
                                    <p><span class="font-bold">Nghề nghiệp:</span> ${data.ngheNghiep}</p>
                                    <p><span class="font-bold">Năm sinh:</span> ${data.namSinh}</p>
                                    <p><span class="font-bold">Email:</span> ${data.email}</p>
                                    <p><span class="font-bold">Số điện thoại:</span> ${data.sdt}</p>
                                </div>
                                <div>
                                    <h2 class="text-red-600 font-bold">Thông tin liên lạc</h2>
                                    <p><span class="font-bold">Dân tộc:</span> [...]</p>
                                    <p><span class="font-bold">Tôn giáo:</span> [...]</p>
                                    <p><span class="font-bold">Quốc gia:</span> [...]</p>
                                    <p><span class="font-bold">Tỉnh thành:</span> [...]</p>
                                    <p><span class="font-bold">Quận huyện:</span> [...]</p>

                                    <p><span class="font-bold">ĐT bàn:</span> [...]</p>
                                    <button class="mt-2 px-4 py-2 bg-gray-200 border border-gray-400">[Cập nhật thông tin cá nhân]</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="mt-4">
                        <h2 class="text-red-600 font-bold">Thông tin người thân khác</h2>
                        <p><span class="font-bold">Họ tên người liên hệ:</span> [...]</p>
                        <p><span class="font-bold">Địa chỉ người liên hệ:</span> [...]</p>
                        <p><span class="font-bold">Điện thoại người liên hệ:</span> [...]</p>
                    </div>
                </div>
            `;
        })
        .catch(error => {
            console.error("Error:", error);
            contentArea.innerHTML = `
                <p class="text-red-500">Có lỗi xảy ra: ${error.message}</p>
            `;
        });
});

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

btnLogout.addEventListener("click", function () {
    logoutModal.classList.remove("hidden");
});

cancelLogout.addEventListener("click", function () {
    logoutModal.classList.add("hidden");
});

confirmLogout.addEventListener("click", function () {
    window.location.href = "http://localhost:5005";
});
