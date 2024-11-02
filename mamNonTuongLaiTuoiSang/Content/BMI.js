$(document).ready(function () {
    $('#childSelect').on('change', async function () {
        var idHocSinh = $(this).val();

        // Gọi API để lấy chiều cao và cân nặng
        $.ajax({
            url: '/api/HocSinhs/ChieuCao/CanNang/' + idHocSinh,
            type: 'GET',
            success: function (data) {
                var chieuCao = data.chieuCao;
                var canNang = data.canNang;
                var ans = (canNang / (chieuCao * chieuCao)).toFixed(2);
                if (isNaN(ans) || ans <= 0) {
                    document.getElementById("Bmi").style.display = "none";
                    document.getElementById("khongthay").style.display = "block";
                    return;
                }
                var canNangmin = (18.5 * chieuCao * chieuCao).toFixed(2);
                var canNangmax = (24.9 * chieuCao * chieuCao).toFixed(2);
                $('#chiso').html('Chỉ số BMI của trẻ em là: ' + ans);
                if (ans < 18.5) {
                    $('#trangthai').html('Trẻ đang bị thiếu dinh dưỡng');
                }
                else if (ans <= 24.9) {
                    $('#trangthai').html('Trẻ đang ở trạng thái bình thường');
                }
                else if (ans <= 29.9) {
                    $('#trangthai').html('Trẻ đang bị thừa cân');
                }
                else if (ans <= 34.9) {
                    $('#trangthai').html('Trẻ đang bị béo phì 1');
                }
                else{
                    $('#trangthai').html('Trẻ đang bị béo phì 2');
                }
                // Nếu dữ liệu trả về là một mảng
                
                // Thêm class mới
                $('#lytuong').html('Cân nặng lý tưởng của bạn là từ ' + canNangmin + ' đến ' + canNangmax + ' kg');
                $('#toithieu').html('Cân nặng tối thiểu của bạn là ' + canNangmin + ' kg');
                $('#toida').html('Cân nặng tối đa của bạn là ' + canNangmax + ' kg');
                document.getElementById("Bmi").style.display = "block";
                updateArrowPosition(ans);
                updateArrowPosition(ans);
                document.getElementById("khongthay").style.display = "none";

            },
            error: function () {
                document.getElementById("Bmi").style.display = "none";
                document.getElementById("khongthay").style.display = "block";
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
