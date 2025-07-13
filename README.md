# 🏫 Hệ Thống Quản Lý Trường Mầm Non "Tương Lai Tươi Sáng"

## 📑 Mục lục

- [📋 Mô tả dự án](#-mô-tả-dự-án)
- [🚀 Tính năng chính](#-tính-năng-chính)
  - [👨‍💼 Quản lý nhân viên](#-quản-lý-nhân-viên)
  - [👩‍🏫 Quản lý giáo viên](#-quản-lý-giáo-viên)
  - [👨‍👩‍👧‍👦 Quản lý phụ huynh và học sinh](#-quản-lý-phụ-huynh-và-học-sinh)
  - [📚 Quản lý học tập](#-quản-lý-học-tập)
  - [💰 Quản lý tài chính](#-quản-lý-tài-chính)
  - [🚌 Quản lý vận chuyển](#-quản-lý-vận-chuyển)
  - [📢 Thông báo và tin tức](#-thông-báo-và-tin-tức)
- [🛠️ Công nghệ sử dụng](#️-công-nghệ-sử-dụng)
  - [Backend](#backend)
  - [Frontend](#frontend)
  - [Thư viện bổ sung](#thư-viện-bổ-sung)
- [📁 Cấu trúc dự án](#-cấu-trúc-dự-án)
- [🗄️ Cơ sở dữ liệu](#️-cơ-sở-dữ-liệu)
- [⚙️ Cài đặt và chạy](#️-cài-đặt-và-chạy)
  - [Yêu cầu hệ thống](#yêu-cầu-hệ-thống)
  - [Bước 1: Clone dự án](#bước-1-clone-dự-án)
  - [Bước 2: Cài đặt database](#bước-2-cài-đặt-database)
  - [Bước 3: Cài đặt dependencies](#bước-3-cài-đặt-dependencies)
  - [Bước 4: Chạy ứng dụng](#bước-4-chạy-ứng-dụng)
- [🔧 Cấu hình](#-cấu-hình)
  - [Connection String](#connection-string)
  - [Email Configuration](#email-configuration)
- [👥 Phân quyền người dùng](#-phân-quyền-người-dùng)
  - [Admin](#admin)
  - [Giáo viên](#giáo-viên)
  - [Phụ huynh](#phụ-huynh)
- [📊 Tính năng đặc biệt](#-tính-năng-đặc-biệt)
  - [Biểu đồ BMI](#biểu-đồ-bmi)
  - [Hệ thống voucher](#hệ-thống-voucher)
  - [Quản lý xe bus](#quản-lý-xe-bus)
- [🐛 Xử lý lỗi thường gặp](#-xử-lý-lỗi-thường-gặp)
  - [Lỗi kết nối database](#lỗi-kết-nối-database)
  - [Lỗi thư viện](#lỗi-thư-viện)
- [📝 Ghi chú](#-ghi-chú)
- [👨‍💻 Đóng góp](#-đóng-góp)
- [📄 License](#-license)

---

## 📋 Mô tả dự án

Đây là hệ thống quản lý toàn diện cho trường mầm non được phát triển bởi nhóm 2 trường HUFLIT. Hệ thống cung cấp các chức năng quản lý cho nhiều đối tượng khác nhau bao gồm: nhân viên, giáo viên, phụ huynh, học sinh và các hoạt động của trường.

## 🚀 Tính năng chính

### 👨‍💼 Quản lý nhân viên
- Quản lý thông tin nhân viên, chức vụ
- Hệ thống phân quyền và bảo mật
- Quản lý lương và hệ số lương

### 👩‍🏫 Quản lý giáo viên
- Thông tin giáo viên và trình độ chuyên môn
- Đánh giá và xếp hạng sao
- Quản lý lớp học và thời khóa biểu

### 👨‍👩‍👧‍👦 Quản lý phụ huynh và học sinh
- Thông tin phụ huynh và học sinh
- Quản lý mối quan hệ gia đình
- Theo dõi sức khỏe và tiêm chủng

### 📚 Quản lý học tập
- Quản lý lớp học và môn học
- Thời khóa biểu chi tiết
- Điểm danh học sinh
- Khóa học và hoạt động ngoại khóa

### 💰 Quản lý tài chính
- Hóa đơn và thanh toán
- Dịch vụ và khóa học
- Voucher và khuyến mãi
- Báo cáo thu nhập

### 🚌 Quản lý vận chuyển
- Quản lý xe bus đưa đón
- Đăng ký dịch vụ đưa đón
- Lịch trình và tuyến đường

### 📢 Thông báo và tin tức
- Hệ thống thông báo cho phụ huynh
- Tin tức và sự kiện trường
- Gửi email thông báo

## 🛠️ Công nghệ sử dụng

### Backend
- **.NET 6.0** - Framework chính
- **ASP.NET Core MVC** - Kiến trúc web
- **Entity Framework Core 6.0.33** - ORM
- **SQL Server** - Cơ sở dữ liệu

### Frontend
- **Bootstrap 4** - Framework CSS
- **jQuery** - JavaScript library
- **FontAwesome** - Icon library
- **Chart.js** - Biểu đồ và thống kê
- **DataTables** - Bảng dữ liệu

### Thư viện bổ sung
- **MailKit 4.8.0** - Gửi email
- **MimeKit 4.8.0** - Xử lý email
- **HtmlSanitizer 6.0.437** - Bảo mật HTML
- **Swashbuckle.AspNetCore 6.5.0** - API documentation

## 📁 Cấu trúc dự án

```
mamNonTuongLaiTuoiSang/
├── Areas/
│   └── Admin/                 # Khu vực quản trị
│       ├── Controllers/       # Controllers cho admin
│       ├── Models/           # DTO và models
│       └── Views/            # Views cho admin
├── Controllers/
│   ├── Parent/               # Controllers cho phụ huynh
│   ├── Teacher/              # Controllers cho giáo viên
│   └── *.cs                  # Controllers chính
├── Models/                   # Entity models
├── Views/                    # Views chính
├── Content/                  # Static files (CSS, JS, Images)
├── HelpersPython/           # Python helpers cho biểu đồ
└── App_Start/               # Cấu hình ứng dụng
```

## 🗄️ Cơ sở dữ liệu

Hệ thống sử dụng SQL Server với các bảng chính:
- **ChucVu, NhanVien** - Quản lý nhân viên
- **PhuHuynh, HocSinh** - Thông tin gia đình
- **Lop, MonHoc, TKB** - Quản lý học tập
- **KhoaHoc, NgoaiKhoa** - Khóa học và hoạt động
- **HoaDon, DichVu, Voucher** - Quản lý tài chính
- **XeBus, DKXe** - Quản lý vận chuyển
- **TinTuc, ThongBao** - Thông báo và tin tức

## ⚙️ Cài đặt và chạy

### Yêu cầu hệ thống
- .NET 6.0 SDK
- SQL Server (Express hoặc cao hơn)
- Visual Studio 2022 hoặc VS Code

### Bước 1: Clone dự án
```bash
git clone [repository-url]
cd CNPM_NC
```

### Bước 2: Cài đặt database
1. Mở SQL Server Management Studio
2. Chạy file `QLMamNon_db.sql` để tạo database
3. Cập nhật connection string trong `appsettings.json`

### Bước 3: Cài đặt dependencies
```bash
cd mamNonTuongLaiTuoiSang
dotnet restore
```

### Bước 4: Chạy ứng dụng
```bash
dotnet run
```

Ứng dụng sẽ chạy tại: `https://localhost:5001` hoặc `http://localhost:5000`

## 🔧 Cấu hình

### Connection String
Cập nhật connection string trong `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "MamNon": "Server=your-server;Database=QLMamNon;Integrated Security=True"
  }
}
```

### Email Configuration
Để sử dụng tính năng gửi email, cần cấu hình SMTP trong code.

## 👥 Phân quyền người dùng

### Admin
- Quản lý toàn bộ hệ thống
- Thêm/sửa/xóa nhân viên, học sinh, phụ huynh
- Quản lý tài chính và báo cáo

### Giáo viên
- Quản lý lớp học được phân công
- Điểm danh học sinh
- Xem thông tin học sinh và phụ huynh

### Phụ huynh
- Xem thông tin con em
- Đăng ký khóa học và dịch vụ
- Nhận thông báo từ trường

## 📊 Tính năng đặc biệt

### Biểu đồ BMI
- Sử dụng Python để tạo biểu đồ BMI cho trẻ em
- Theo dõi sự phát triển của trẻ

### Hệ thống voucher
- Tạo và quản lý voucher giảm giá
- Phân phối voucher cho phụ huynh

### Quản lý xe bus
- Đăng ký dịch vụ đưa đón
- Quản lý tuyến đường và lịch trình

## 🐛 Xử lý lỗi thường gặp

### Lỗi kết nối database
- Kiểm tra SQL Server đã chạy chưa
- Kiểm tra connection string
- Đảm bảo database đã được tạo

### Lỗi thư viện
```bash
dotnet restore
dotnet clean
dotnet build
```

## 📝 Ghi chú

- Dự án sử dụng Entity Framework Core với SQL Server
- Có tích hợp Python để tạo biểu đồ
- Hệ thống session để quản lý đăng nhập
- Sử dụng Bootstrap cho giao diện responsive

## 👨‍💻 Đóng góp

Dự án được phát triển bởi nhóm 2 trường HUFLIT. Mọi đóng góp và báo cáo lỗi đều được chào đón.

## 📄 License

Dự án này được phát triển cho mục đích học tập và nghiên cứu.

---

**Phiên bản:** 1.0  
**Cập nhật lần cuối:** 2024  
**Nhóm phát triển:** Nhóm 2 - HUFLIT
