document.getElementById('baotang-chientich').addEventListener('click', function () {
    let hoatDongSection = document.getElementById('hoat-dong-section');
    let hoatDongText = document.getElementById('hoat-dong-text');

    // Thay đổi hình nền
    hoatDongSection.style.backgroundImage = "url('../../Content/images/baotangchientich.jpg')";

    // Thay đổi nội dung văn bản
    hoatDongText.innerHTML = `
                    <h1 class="text-6xl font-bold">Bảo tàng Chiến tích Chiến tranh</h1>
                    <p style="margin-top: 20px; font-size: 1.2rem; color: white; text-align: justify;">
                        Đây là nơi tuyệt vời để các bé tìm hiểu về lịch sử dân tộc.
                        Bảo tàng mang đến những câu chuyện về hòa bình và sự đoàn kết,
                        giúp các bé hiểu thêm về giá trị của hòa bình trong cuộc sống.
                        Các bé sẽ được tham gia vào các hoạt động khám phá thú vị và học hỏi những bài học quý giá từ lịch sử.
                    </p>
                `;
});

document.getElementById('baotang-lstsphcm').addEventListener('click', function () {
    let hoatDongSection = document.getElementById('hoat-dong-section');
    let hoatDongText = document.getElementById('hoat-dong-text');

    hoatDongSection.style.backgroundImage = "url('../../Content/images/baotanglstphcm.jpg')";

    hoatDongText.innerHTML = `
                    <h1 class="text-6xl font-bold">Bảo tàng Lịch sử Thành phố Hồ Chí Minh</h1>
                    <p style="margin-top: 20px; font-size: 1.2rem; color: white; text-align: justify;">
                        Bảo tàng này là nơi trưng bày nhiều hiện vật phong phú về lịch sử và văn hóa của thành phố.
                        Tại đây, các bé sẽ được chiêm ngưỡng những bảo vật quý giá và có cơ hội tham gia các trò chơi học tập hấp dẫn,
                        giúp các bé nhận thức rõ hơn về nguồn cội văn hóa của dân tộc.
                    </p>
                `;
});

document.getElementById('ben-nha-rong').addEventListener('click', function () {
    let hoatDongSection = document.getElementById('hoat-dong-section');
    let hoatDongText = document.getElementById('hoat-dong-text');

    hoatDongSection.style.backgroundImage = "url('../../Content/images/bennharong.jpg')";

    hoatDongText.innerHTML = `
                    <h1 class="text-6xl font-bold">Bến Nhà Rồng</h1>
                    <p style="margin-top: 20px; font-size: 1.2rem; color: white; text-align: justify;">
                        Bến Nhà Rồng không chỉ là một địa điểm lịch sử mà còn là nơi gợi nhớ về hành trình của Bác Hồ.
                        Các bé sẽ được nghe kể những câu chuyện thú vị về cuộc sống và sự nghiệp của Bác,
                        từ đó khơi dậy lòng yêu nước và tự hào về quê hương trong các bé.
                    </p>
                `;
});

document.getElementById('dinh-doc-lap').addEventListener('click', function () {
    let hoatDongSection = document.getElementById('hoat-dong-section');
    let hoatDongText = document.getElementById('hoat-dong-text');

    hoatDongSection.style.backgroundImage = "url('../../Content/images/dinhdoclap.jpg')";

    hoatDongText.innerHTML = `
                    <h1 class="text-6xl font-bold">Dinh Độc Lập</h1>
                    <p style="margin-top: 20px; font-size: 1.2rem; color: white; text-align: justify;">
                        Dinh Độc Lập là biểu tượng của sự tự do và độc lập của đất nước.
                        Tại đây, các bé sẽ được tham quan kiến trúc độc đáo và nghe những câu chuyện lịch sử về giai đoạn quan trọng của đất nước.
                        Đây là dịp để các bé hiểu hơn về những gì đã xây dựng nên Tổ quốc chúng ta ngày hôm nay.
                    </p>
                `;
});