//function showContent(section) {
//    let content = document.getElementById('content');
//    switch (section) {
//        case 'hoc-phi':
//            content.innerHTML = '<h1>Nội dung Học Phí sẽ hiển thị ở đây.</h1>';
//            break;
//        case 'tuyen-sinh':
//            content.innerHTML = '<h1>Nội dung Tuyển Sinh sẽ hiển thị ở đây.</h1>';
//            break;
//        case 'video':
//            content.innerHTML = '<h1>Nội dung Video sẽ hiển thị ở đây.</h1>';
//            break;
//        case 'day-hoc':
//            content.innerHTML = '<h1>Nội dung Dạy Học Trực Tuyến sẽ hiển thị ở đây.</h1>';
//            break;
//        case 'hoat-dong':
//            content.innerHTML = `
//                

//            // Thêm sự kiện click cho từng card

//            break;
//        case 'tuyen-dung':
//            content.innerHTML = '<h1>Nội dung Tuyển Dụng sẽ hiển thị ở đây.</h1>';
//            break;
//        case 'tin-tuc':
//            content.innerHTML = '<h1>Nội dung Tin Tức sẽ hiển thị ở đây.</h1>';
//            break;
//        case 'lien-he':
//            content.innerHTML = '<h1>Nội dung Liên Hệ sẽ hiển thị ở đây.</h1>';
//            break;
//        case 'gioi-thieu':
//            content.innerHTML = '<h1>Nội dung Giới Thiệu sẽ hiển thị ở đây.</h1>';
//            break;
//        case 'home':
//            content.innerHTML = '<h1>Bổ sung sau nếu có hihi</h1>';
//            break;
//        default:
//            content.innerHTML = '<h1>Bổ sung sau nếu có hihi</h1>';
//    }
//}


/*===============================================TẠO BONG BÓNG BAY LÊN ================================================================*/
const loginnhe = document.getElementById('dangnhapnhe');
loginnhe.addEventListener('mouseover', function () {
    for (let i = 0; i < 6; i++) { // Tạo 6 bong bóng
        setTimeout(() => {
            const bubble = document.createElement('div');
            const size = Math.random() * 30 + 10; // Kích thước ngẫu nhiên
            bubble.style.width = `${size}px`;
            bubble.style.height = `${size}px`;
            bubble.classList.add('bubble');
            bubble.style.left = `${Math.random() * 100}%`; // Vị trí ngẫu nhiên
            const randomHue = Math.floor(Math.random() * 360);
            bubble.style.backgroundColor = `hsl(${randomHue}, 100%, 50%)`;
            loginnhe.appendChild(bubble);

            // Kết thúc hiệu ứng sau 1 giây
            setTimeout(() => {
                bubble.remove();
            }, 1000);
        }, i * 200); // Thời gian chờ 200ms giữa mỗi bong bóng
    }
});

