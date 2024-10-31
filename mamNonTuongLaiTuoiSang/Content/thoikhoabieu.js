
$(document).ready(function () {
    $('#childSelect').on('change', async function () {
        var idHocSinh = $(this).val();

        let mangA = await getSubjects();

        $.ajax({
            url: '/api/Tkbs/GetTkbByHocSinh/' + idHocSinh,
            type: 'GET',
            success: function (data) {
                var timetableHtml = '<div class="header">Ca Học</div>';
                timetableHtml += '<div class="header">Thứ 2</div>';
                timetableHtml += '<div class="header">Thứ 3</div>';
                timetableHtml += '<div class="header">Thứ 4</div>';
                timetableHtml += '<div class="header">Thứ 5</div>';
                timetableHtml += '<div class="header">Thứ 6</div>';
                var mang = [];
                var flag = false;
                console.log(mangA);
                data.forEach(function (row) {
                    mang.push([row.caHoc, row.ngay, row.idMh]);
                });
                // Đợi tất cả các `Promise` hoàn thành trước khi cập nhật HTML
                for (var ca = 1; ca <= 4; ca++) {
                    timetableHtml += '<div class="header"> ca ' + ca + '</div>';
                    ['Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu'].forEach(function (thu) {
                        for (var i = 0; i < mang.length; i++) {
                            if (mang[i][0] == ca && mang[i][1] == thu) {
                                mangA.forEach(function (row) {
                                    if (row.idMh == mang[i][2]) {
                                        timetableHtml += '<div class="cell"> ' + row.tenMh + '</div>';
                                        flag = true;
                                    }
                                });
                            }
                        }
                    });
                }
                if (flag) {
                    $('#timetableContainer').html(timetableHtml);
                }
                else {
                    $('#timetableContainer').html('<p>Không tìm thấy thời khóa biểu cho học sinh này </p>');
                }
                

            },
            error: function () {
                $('#timetableContainer').html('<p>Không tìm thấy thời khóa biểu cho học sinh này.</p>');
            }
        });

        $('#timetableContainer').html('Không Lấy được'); // Xóa thời khóa biểu nếu không chọn gì
    });
});

// Sửa đổi `getTenMonHoc` để trả về `Promise`
async function getSubjects() {
    try {
        // Gửi yêu cầu đến API
        const response = await fetch('/api/MonHocs');

        // Kiểm tra nếu yêu cầu thành công
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        // Chuyển đổi dữ liệu phản hồi thành JSON
        
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Có lỗi xảy ra:', error);
        return []; // Trả về mảng rỗng nếu có lỗi
    }
}