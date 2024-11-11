$(document).ready(function () {
    $('#childSelect').on('change', async function () {
        var idHocSinh = $(this).val();

        // Gọi API để lấy chiều cao và cân nặng
        $.ajax({
            url: '/api/HocSinhs/ChieuCao/CanNang/' + idHocSinh,
            type: 'GET',
            success: function (data) {
                let boy_w_negav_2SD = [7.7, 9.7, 11.3, 12.7, 14.1, 15.9, 17.7, 19.5, 21.3, 23.2];
                let boy_w_posi_2SD = [12.0, 15.3, 18.3, 21.2, 24.2, 27.1, 30.7, 34.7, 39.4, 45];
                let boy_w_avg = [9.6, 12.2, 14.3, 16.3, 18.3, 20.5, 22.9, 25.4, 28.1, 31.2];
                let boy_h_negav_2SD = [71, 81, 88.7, 94.9, 100.7, 106.1, 111.2, 116.0, 120.5, 125];
                let boy_h_posi_2SD = [80.5, 93.2, 103.5, 111.7, 119.2, 125.8, 132.3, 138.6, 144.6, 150.5];
                let boy_h_avg = [75.7, 87.1, 96.1, 103.3, 110.0, 116.0, 121.7, 127.3, 132.6, 137.8];
                let girl_w_negav_2SD = [7, 9.0, 10.8, 12.3, 13.7, 15.3, 16.8, 18.6, 20.8, 23.3];
                let girl_w_posi_2SD = [11.5, 14.8, 18.1, 21.5, 24.9, 27.8, 31.4, 35.8, 41.0, 46.9];
                let girl_w_avg = [8.9, 11.5, 13.9, 16.1, 18.2, 20.2, 22.4, 25.0, 28.2, 31.9];
                let girl_h_negav_2SD = [68.9, 80, 87.4, 94.1, 99.9, 104.9, 109.9, 115.0, 120.3, 125.8];
                let girl_h_posi_2SD = [79.2, 92.9, 102.7, 111.3, 118.9, 125.4, 131.7, 138.2, 144.7, 151.4];
                let girl_h_avg = [74, 86.4, 95.1, 102.7, 109.4, 115.1, 120.8, 126.6, 132.5, 138.6];
                var chieuCao = data.chieuCao;
                var canNang = data.canNang;
                var tuoi = data.tuoi;
                var gioiTinh = data.gioiTinh;
                var trangthai = "";
                $('#hientai').html('Chiều Cao Và Cân Nặng Hiện tại của trẻ là ' + chieuCao + ' cm và ' + canNang + ' kg');
                var ans = (canNang / (chieuCao * chieuCao)).toFixed(2);
                if (isNaN(ans) || ans <= 0) {
                    document.getElementById("Bmi").style.display = "none";
                    document.getElementById("khongthay").style.display = "block";
                    return;
                }
                $('#chiso').html('Chỉ số BMI của trẻ em là: ' + ans);
                if (gioiTinh == 'Nam') {
                    if (chieuCao < boy_h_negav_2SD[tuoi - 1]) {
                        trangthai += "Trẻ bị thấp bé ";
                    }
                    else if (chieuCao == boy_h_avg[tuoi - 1]) {
                        trangthai += "Trẻ đạt chiều cao trung bình ";
                    }
                    else if (chieuCao < boy_h_posi_2SD[tuoi - 1]) {
                        trangthai += "Trẻ phát triên bỉnh thường ";
                    }
                    else {
                        trangthai += "Trẻ phát triển cao vượt trội ";
                    }
                    if (canNang < boy_w_negav_2SD[tuoi - 1]) {
                        trangthai += "và nhẹ cân ";
                    }
                    else if (canNang == boy_w_avg[tuoi - 1]) {
                        trangthai += "và cân nặng trung bình ";
                    }
                    else if (canNang < boy_w_posi_2SD[tuoi - 1]) {
                        trangthai += "và cân nặng bình thường ";
                    }
                    else {
                        trangthai += "và nặng cân ";
                    }
                    $('#trangthai').html(trangthai);
                    $('#lytuong').html('Cân nặng và chiều cao lý tưởng của trẻ là từ ' + boy_w_negav_2SD[tuoi - 1] + ' đến ' + boy_w_posi_2SD[tuoi - 1] + ' kg và từ ' + boy_h_negav_2SD[tuoi - 1] + ' đến ' + boy_h_posi_2SD[tuoi - 1] + ' m');
                    $('#toithieu').html('Cân nặng và chiều cao tối thiểu của trẻ là ' + boy_w_negav_2SD[tuoi - 1] + ' kg và ' + boy_h_negav_2SD[tuoi - 1] + ' m');
                    $('#toida').html('Cân nặng và chiều cao tối đa của trẻ là ' + boy_w_posi_2SD[tuoi - 1] + ' kg và '+ boy_h_posi_2SD[tuoi - 1] + ' m');
                }
                else {
                    if (chieuCao < girl_h_negav_2SD[tuoi - 1]) {
                        trangthai += "Trẻ bị thấp bé ";
                    }
                    else if (chieuCao == girl_h_avg[tuoi - 1]) {
                        trangthai += "Trẻ đạt chiều cao trung bình ";
                    }
                    else if (chieuCao < girl_h_posi_2SD[tuoi - 1]) {
                        trangthai += "Trẻ phát triên bỉnh thường ";
                    }
                    else {
                        trangthai += "Trẻ phát triển cao vượt trội ";
                    }
                    if (canNang < girl_w_negav_2SD[tuoi - 1]) {
                        trangthai += "và nhẹ cân ";
                    }
                    else if (canNang == girl_w_avg[tuoi - 1]) {
                        trangthai += "và cân nặng trung bình ";
                    }
                    else if (canNang < girl_w_posi_2SD[tuoi - 1]) {
                        trangthai += "và cân nặng bình thường ";
                    }
                    else {
                        trangthai += "và nặng cân ";
                    }
                    $('#trangthai').html(trangthai);
                    $('#lytuong').html('Cân nặng và chiều cao lý tưởng của trẻ là từ ' + girl_w_negav_2SD[tuoi - 1] + ' đến ' + girl_w_posi_2SD[tuoi - 1] + ' kg và từ ' + girl_h_negav_2SD[tuoi - 1] + ' đến ' + boy_h_posi_2SD[tuoi - 1] + ' m');
                    $('#toithieu').html('Cân nặng và chiều cao tối thiểu của trẻ là ' + girl_w_negav_2SD[tuoi - 1] + ' kg và ' + girl_h_negav_2SD[tuoi - 1] + ' m');
                    $('#toida').html('Cân nặng và chiều cao tối đa của trẻ là ' + girl_w_posi_2SD[tuoi - 1] + ' kg và ' + girl_h_posi_2SD[tuoi - 1] + ' m');
                }

                
                document.getElementById("Bmi").style.display = "block";
                updateArrowPosition(ans);
                updateArrowPosition(ans);

            },
            error: function () {
                document.getElementById("Bmi").style.display = "none";
            }
        });

        // Xóa thông tin nếu không chọn học sinh nào
        if (!idHocSinh) {
            $('#lytuong').html('Không Lấy được');
        }
    });
});
function updateArrowPosition(bmiValue) {
    const minBmi = 0; // Giá trị BMI tối thiểu để tính toán vị trí
    const maxBmi = 35; // Giá trị BMI tối đa để tính toán vị trí

    // Tính toán tỷ lệ phần trăm
    let percentage = (bmiValue - minBmi) / (maxBmi - minBmi);

    // Giới hạn giá trị tỷ lệ từ 0 đến 1
    percentage = Math.max(0, Math.min(1, percentage));

    // Tính toán vị trí của mũi tên
    const barWidth = document.querySelector('.relative').clientWidth; // Chiều rộng của thanh
    const arrowPosition = percentage * barWidth; // Vị trí mũi tên

    // Cập nhật vị trí mũi tên
    const arrow = document.getElementById('phantram');
    arrow.style.left = `${arrowPosition}px`;
}
