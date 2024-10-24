function showContent(section) {
    let content = document.getElementById('content');
    switch (section) {
        case 'hoc-phi':
            content.innerHTML = '<h1>Nội dung Học Phí sẽ hiển thị ở đây.</h1>';
            break;
        case 'tuyen-sinh':
            content.innerHTML = '<h1>Nội dung Tuyển Sinh sẽ hiển thị ở đây.</h1>';
            break;
        case 'video':
            content.innerHTML = '<h1>Nội dung Video sẽ hiển thị ở đây.</h1>';
            break;
        case 'day-hoc':
            content.innerHTML = '<h1>Nội dung Dạy Học Trực Tuyến sẽ hiển thị ở đây.</h1>';
            break;
        case 'hoat-dong':
            content.innerHTML = `
                <div id="hoat-dong-section" style="background-image: url('image/anhnhatre.jpg'); background-size: cover; background-position: center; height: auto; display: flex; align-items: center; justify-content: center; color: white; position: relative; width: 100%; border-radius: 30px; overflow: hidden;">
                    <div id="hoat-dong-text" style="flex: 1; padding: 20px; background: rgba(0, 0, 0, 0.5); text-align: center;">
                        <h1 class="text-6xl font-bold">CÁC ĐỊA ĐIỂM HOẠT ĐỘNG NGOẠI KHÓA CỦA TRƯỜNG</h1>
                    </div>
                    <div style="display: flex; overflow-x: auto; padding: 20px; max-width: 100%; max-height: 600px;">
                        <div class="flex space-x-4">
                            <div class="card" id="baotang-chientich" style="background-color: rgba(128, 128, 128, 0.7); padding: 15px; border-radius: 15px; margin: 20px 0; cursor: pointer;">
                                <img alt="Bảo tàng chiến tích" class="card-image" src="image/baotangchientich.jpg" style="width: 100%; border-radius: 10px;">
                                <div class="card-title" style="color: white; margin-top: 10px; text-align: center;">Bảo tàng chiến tích</div>
                            </div>
                            <div class="card" id="baotang-lstsphcm" style="background-color: rgba(128, 128, 128, 0.7); padding: 15px; border-radius: 15px; margin: 20px 0; cursor: pointer;">
                                <img alt="Bảo tàng lịch sử thành phố Hồ Chí Minh" class="card-image" src="image/baotanglstphcm.jpg" style="width: 100%; border-radius: 10px;">
                                <div class="card-title" style="color: white; margin-top: 10px; text-align: center;">Bảo tàng lịch sử thành phố Hồ Chí Minh</div>
                            </div>
                            <div class="card" id="ben-nha-rong" style="background-color: rgba(128, 128, 128, 0.7); padding: 15px; border-radius: 15px; margin: 20px 0; cursor: pointer;">
                                <img alt="Bến nhà rồng" class="card-image" src="image/bennharong.jpg" style="width: 100%; border-radius: 10px;">
                                <div class="card-title" style="color: white; margin-top: 10px; text-align: center;">Bến nhà rồng</div>
                            </div>
                            <div class="card" id="dinh-doc-lap" style="background-color: rgba(128, 128, 128, 0.7); padding: 15px; border-radius: 15px; margin: 20px 0; cursor: pointer;">
                                <img alt="Dinh độc lập" class="card-image" src="image/dinhdoclap.jpg" style="width: 100%; border-radius: 10px;">
                                <div class="card-title" style="color: white; margin-top: 10px; text-align: center;">Dinh độc lập</div>
                            </div>
                        </div>
                    </div>
                </div>
            `;

            // Thêm sự kiện click cho từng card
            document.getElementById('baotang-chientich').addEventListener('click', function () {
                let hoatDongSection = document.getElementById('hoat-dong-section');
                let hoatDongText = document.getElementById('hoat-dong-text');

                // Thay đổi hình nền
                hoatDongSection.style.backgroundImage = "url('image/baotangchientich.jpg')";

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

                hoatDongSection.style.backgroundImage = "url('image/baotanglstphcm.jpg')";

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

                hoatDongSection.style.backgroundImage = "url('image/bennharong.jpg')";

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

                hoatDongSection.style.backgroundImage = "url('image/dinhdoclap.jpg')";

                hoatDongText.innerHTML = `
                    <h1 class="text-6xl font-bold">Dinh Độc Lập</h1>
                    <p style="margin-top: 20px; font-size: 1.2rem; color: white; text-align: justify;">
                        Dinh Độc Lập là biểu tượng của sự tự do và độc lập của đất nước. 
                        Tại đây, các bé sẽ được tham quan kiến trúc độc đáo và nghe những câu chuyện lịch sử về giai đoạn quan trọng của đất nước. 
                        Đây là dịp để các bé hiểu hơn về những gì đã xây dựng nên Tổ quốc chúng ta ngày hôm nay.
                    </p>
                `;
            });
            break;
        case 'tuyen-dung':
            content.innerHTML = '<h1>Nội dung Tuyển Dụng sẽ hiển thị ở đây.</h1>';
            break;
        case 'tin-tuc':
            content.innerHTML = '<h1>Nội dung Tin Tức sẽ hiển thị ở đây.</h1>';
            break;
        case 'lien-he':
            content.innerHTML = '<h1>Nội dung Liên Hệ sẽ hiển thị ở đây.</h1>';
            break;
        case 'gioi-thieu':
            content.innerHTML = '<h1>Nội dung Giới Thiệu sẽ hiển thị ở đây.</h1>';
            break;
        case 'home':
            content.innerHTML = '<h1>Bổ sung sau nếu có hihi</h1>';
            break;
        default:
            content.innerHTML = '<h1>Bổ sung sau nếu có hihi</h1>';
    }
}