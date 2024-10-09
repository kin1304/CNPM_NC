use master 
IF EXISTS (SELECT * FROM SYSDATABASES WHERE NAME = 'QLMamNon')
	Drop database QLMamNon
CREATE DATABASE QLMamNon
go
Use QLMamNon

create table ChucVu
(
	TenCV nvarchar (100),
	ViTri nvarchar(100),
	LuongCoBan int,
	HeSoLuong decimal,
	primary key(TenCV,ViTri)
)

create table NhanVien
(
	MaST char (10) primary key,
	HoTen nvarchar (100),
	MatKhau varchar (50),
	DiaChi nvarchar (100),
	NamSinh int,
	GioiTinh bit, 
	NamLam int,
	Email varchar (100),
	SDT varchar(15),
	TenCV nvarchar (100),
	ViTri nvarchar(100),

	CONSTRAINT FK_NV_CV FOREIGN KEY (TenCV, ViTri) REFERENCES CHUCVU(TenCV, ViTri)
)

	create table XeBus
	(
		IdXeBus char (10) primary key,
		BienSo varchar (20),
		ViTri nvarchar(100),
		MaST char(10),

		foreign key (MaST) references NhanVien(MaST)
	)

create table PhuHuynh
(
	IdPH char(10) primary key,
        HoTen nvarchar(30),
	DiaChi nvarchar (100),
	GioiTinh bit, 
	NgheNghiep nvarchar (100),
	NamSinh int,
	MatKhau char (50),
	Email varchar(100),
	SDT varchar(15)
)

create table HocSinh
(
	IdHS char(10) primary key,
	TenHS nvarchar(100),
	GioiTinh nvarchar(10),
	NamSinh int,
	IdPH char(10),
	QuanHe nvarchar(10),
	foreign key (IdPH) references PhuHuynh(IdPH)
)

create table DKXe
(
	IdHS char(10),
	IdXeBus char(10),
	DiaChiDonTra nvarchar(100),

	primary key (IdHS,IdXeBus),
	foreign key (IdXeBus) references XeBus(IdXeBus),
	foreign key (IdHS) references HocSinh(IdHS)
)

create table Lop
(
	IdLop char(10) primary key,
	TenLop nvarchar (100),
	SiSo int,
	MaST char(10),
	
	foreign key (MaST) references NhanVien(MaST)
)

create table MonHoc
(
	IdMH char(10) primary key,
	TenMH nvarchar(50)
)

create table TKB
(
	IdLop char(10),
	Ngay nvarchar(15),
	CaHoc nvarchar(15),
	IdMH char(10),

	primary key(IdLop,Ngay,CaHoc),
	foreign key (IdMH) references MonHoc(IdMH),
	foreign key (IdLop) references Lop(IdLop)
)

create table VacXin
(
	IdVacXin char(10) primary key,
	NuocSX nvarchar (50),
	LoaiVacXin nvarchar (50)
)

create table TiemChung
(
	IdVacXin char(10),
	IdHS char(10),
	NgayTiem date,

	primary key(IdVacXin,IdHS),
	foreign key (IdVacXin) references VacXin(IdVacXin),
	foreign key (IdHS) references HocSinh(IdHS)
)

create table KhoaHoc
(
	IdKH char(10) primary key,
	TenKH nvarchar(100),
	DonGia decimal(18,2),
	MoTa nvarchar(max),
	SoLuong int,
	ThoiHan nvarchar(20)
)

create table NgoaiKhoa
(
	IdNK char(10) primary key,
	TenNK nvarchar(100),
	NgayBatDau date,
	NgayKetThu date,
	MoTa nvarchar(max)
)

create table GiaoVien
(
	MaST char(10) primary key,
	TrinhDoChuyenMon nvarchar(100),
	SaoDanhGia decimal(2,1)
	foreign key (MaST) references NhanVien(MaST)
)

create table HoaDon
(
	IdHD char(10) primary key ,
	NgayLap date,
	NgayHetHan date,
	SoTien decimal(18,2),
	TrangThai nvarchar(20),
	MaST char(10),
	IdHS char(10),

	foreign key (MaST) references NhanVien(MaST),
	foreign key (IdHS) references HocSinh(IdHS)
)

create table DichVu
(
	IdDV char(10) primary key,
	TenDV nvarchar(100),
	DonGia decimal(18,2),
	MoTa nvarchar(max),
	ThoiHan nvarchar(50)
)

create table Voucher
(
	IdVoucher char(10) primary key,
	NgayTao date,
	NgayHetHan date,
	PhanTramGiam decimal(5,2),
	SoLuong int,
	MaST char(10),

	foreign key (MaST) references NhanVien(MaST)
)

create table VoucherCuaPH
(
	IdPH char(10),
	IdVoucher char(10),
	SoLuong int,

	primary key(IdPH,IdVoucher),
	foreign key (IdPH) references PhuHuynh(IdPH),
	foreign key (IdVoucher) references Voucher(IdVoucher)
)

create table TinTuc
(
	IdTinTuc char(10) primary key,
	TieuDe nvarchar(max),
	NoiDung nvarchar(max),
	Anh nvarchar(max),
	MaST char(10),

	foreign key (MaST) references NhanVien(MaST)
)

create table HoaDon_KhoaHoc
(
	IdKH char(10),
	IdHD char(10),
	SoLuong int,
	NgayDK date,
	NgayKT date,

	primary key(IdKH,IdHD),
	foreign key (IdKH) references KhoaHoc(IdKH),
	foreign key (IdHD) references HoaDon(IdHD)
)

create table NgoaiKhoa_GiaoVien
(
	IdNK char(10),
	MaST char(10),

	primary key (IdNK,MaST),
	foreign key (IdNK) references NgoaiKhoa(IdNK),
	foreign key (MaST) references GiaoVien(MaST)
)

create table HoaDon_DichVu
(
	IdHD char(10),
	IdDV char(10),
	SoLuong int,
	NgayDK date,
	NgayHetHan date,

	primary key(IdHD,IdDV),
	foreign key (IdHD) references HoaDon(IdHD),
	foreign key (IdDV) references DichVu(IdDV)
)

create table HocSinh_Lop
(
	IdHS char(10),
	IdLop char(10),
        DiemChuyenCan decimal,
	primary key(IdHS,IdLop),
	foreign key (IdHS) references HocSinh(IdHS),
	foreign key (IdLop) references Lop(IdLop)
)

CREATE TABLE DiemDanh
(
    IdDD char(10) PRIMARY KEY,  
    IdHS char(10),
    IdLop char(10),
    Ngay DATE,
    Thang INT,
    TrangThai TINYINT,          
    TrangThaiNghi TINYINT,      
    FOREIGN KEY (IdHS, IdLop) REFERENCES HocSinh_Lop(IdHS, IdLop)
);
INSERT INTO ChucVu (TenCV, ViTri, LuongCoBan, HeSoLuong)
VALUES 
(N'Giám đốc', N'Quản lý cấp cao', 50000000, 2.5),
(N'Trưởng phòng', N'Quản lý trung cấp', 30000000, 1.8),
(N'Nhân viên kinh doanh', N'Nhân viên', 15000000, 1.2),
(N'Nhân viên kỹ thuật', N'Nhân viên', 17000000, 1.3),
(N'Trợ lý', N'Nhân viên hỗ trợ', 12000000, 1.1),
(N'Admin', N'Nhân viên',12000000,1.1)

select * from ChucVu
delete ChucVu

INSERT INTO NhanVien (MaST, HoTen, MatKhau, DiaChi, NamSinh, GioiTinh, NamLam, Email, SDT, TenCV, ViTri)
VALUES
('ST001', N'Nguyễn Văn A', '123456', N'Số 1, Đường ABC, Hà Nội', 1985, 1, 2010, 'nguyenvana@gmail.com', '0901234567', N'Giám đốc', N'Quản lý cấp cao'),
('ST002', N'Trần Thị B', 'abcdef', N'Số 2, Đường DEF, Hà Nội', 1990, 0, 2012, 'tranthib@gmail.com', '0912345678', N'Trưởng phòng', N'Quản lý trung cấp'),
('ST003', N'Lê Văn C', '654321', N'Số 3, Đường GHI, Hà Nội', 1987, 1, 2011, 'levanc@gmail.com', '0923456789', N'Nhân viên kinh doanh', N'Nhân viên'),
('ST004', N'Phạm Thị D', 'ghijk', N'Số 4, Đường JKL, Hà Nội', 1992, 0, 2014, 'phamthid@gmail.com', '0934567890', N'Nhân viên kỹ thuật', N'Nhân viên'),
('ST005', N'Hoàng Văn E', '789xyz', N'Số 5, Đường MNO, Hà Nội', 1989, 1, 2013, 'hoangvane@gmail.com', '0945678901',N'Admin', N'Nhân viên');

select * from NhanVien
delete NhanVien

INSERT INTO XeBus (IdXeBus, BienSo, ViTri, MaST)
VALUES
('BUS00001', '29A-12345', N'Bến xe Mỹ Đình', 'ST001'),
('BUS00002', '30B-67890', N'Bến xe Giáp Bát', 'ST002'),
('BUS00003 ', '31C-54321', N'Bến xe Nước Ngầm', 'ST003'),
('BUS00004', '32D-98765', N'Bến xe Yên Nghĩa', 'ST004'),
('BUS00005', '33E-19283', N'Bến xe Gia Lâm', 'ST005');

select * from XeBus

INSERT INTO PhuHuynh (IdPH, HoTen, DiaChi, GioiTinh, NgheNghiep, NamSinh, MatKhau, Email, SDT)
VALUES
('PH001', N'Nguyễn Văn A' ,N'Số 10, Đường ABC, Hà Nội', 1, N'Kinh doanh', 1980, 'mk12345', 'ph001@gmail.com', '0912345678'),
('PH002', N'Trần Thị B' ,N'Số 20, Đường DEF, Đà Nẵng', 0, N'Giáo viên', 1982, 'mkabcde', 'ph002@gmail.com', '0923456789'),
('PH003',N'Lê Văn C', N'Số 30, Đường GHI, TP HCM', 1, N'Kỹ sư', 1975, 'mk54321', 'ph003@gmail.com', '0934567890'),
('PH004',N'Phạm Thị D', N'Số 40, Đường JKL, Hải Phòng', 0, N'Bác sĩ', 1978, 'mkqwerty', 'ph004@gmail.com', '0945678901'),
('PH005', N'Hoàng Văn E',N'Số 50, Đường MNO, Cần Thơ', 1, N'Luật sư', 1985, 'mkzxcvb', 'ph005@gmail.com', '0956789012');

select * from PhuHuynh

INSERT INTO HocSinh (IdHS, TenHS, GioiTinh, NamSinh,  IdPH, QuanHe)
VALUES
('HS000001', N'Trần Văn A', N'Nam', 2005, 'PH001', N'Con trai'),
('HS000002', N'Nguyễn Thị B', N'Nữ', 2006, 'PH002', N'Con gái'),
('HS000003', N'Lê Văn C', N'Nam', 2005,'PH003', N'Con trai'),
('HS000004', N'Phạm Thị D', N'Nữ', 2007, 'PH004', N'Con gái'),
('HS000005', N'Hoàng Văn E', N'Nam', 2006, 'PH005', N'Con trai');

select * from HocSinh

INSERT INTO DKXe (IdHS, IdXeBus, DiaChiDonTra)
VALUES
('HS000001', 'BUS00001', N'123 Đường ABC, Hà Nội'),
('HS000002', 'BUS00002', N'456 Đường DEF, TP.HCM'),
('HS000003', 'BUS00003', N'789 Đường GHI, Đà Nẵng'),
('HS000004', 'BUS00004', N'101 Đường JKL, Hải Phòng'),
('HS000005', 'BUS00005', N'202 Đường MNO, Cần Thơ');

select * from DKXe

INSERT INTO Lop (IdLop, TenLop, SiSo, MaST)
VALUES
('L00001', N'Lớp 1A', 30, 'ST001'),
('L00002', N'Lớp 2B', 28, 'ST002'),
('L00003', N'Lớp 3C', 32, 'ST003'),
('L00004', N'Lớp 4D', 25, 'ST004'),
('L00005', N'Lớp 5E', 29, 'ST005');

select * from Lop

INSERT INTO MonHoc (IdMH, TenMH)
VALUES
('MH00001', N'Toán học'),
('MH00002', N'Văn học'),
('MH00003', N'Mỹ thuật'),
('MH00004', N'Âm nhạc'),
('MH00005', N'Thể dục');
select * from MonHoc

INSERT INTO TKB (IdLop, Ngay, CaHoc, IdMH)
VALUES
('L00001', N'Thứ Hai', N'Sáng', 'MH00001'),
('L00002', N'Thứ Ba', N'Chiều', 'MH00002'),
('L00003', N'Thứ Tư', N'Sáng', 'MH00003'),
('L00004', N'Thứ Năm', N'Chiều', 'MH00004'),
('L00005', N'Thứ Sáu', N'Sáng', 'MH00005');
select * from TKB

INSERT INTO VacXin (IdVacXin, NuocSX, LoaiVacXin)
VALUES
('VX00001', N'Việt Nam', N'Sởi'),
('VX00002', N'Mỹ', N'Viêm gan B'),
('VX00003', N'Nhật Bản', N'Cúm'),
('VX00004', N'Pháp', N'Thủy đậu'),
('VX00005', N'Anh', N'Bại liệt');
select * from VacXin
delete VacXin
INSERT INTO TiemChung (IdVacXin, IdHS, NgayTiem)
VALUES
('VX00001', 'HS000001', '2024-01-10'),
('VX00002', 'HS000002', '2024-02-15'),
('VX00003', 'HS000003', '2024-03-20'),
('VX00004', 'HS000004', '2024-04-25'),
('VX00005', 'HS000005', '2024-05-30');
select * from TiemChung 
INSERT INTO KhoaHoc (IdKH, TenKH, DonGia, MoTa, SoLuong, ThoiHan)
VALUES
('KH00001', N'Khóa học Tiếng Anh', 1500000.00, N'Khóa học dành cho trẻ em từ 5-12 tuổi', 30, N'3 tháng'),
('KH00002', N'Khóa học Toán Nâng cao', 2000000.00, N'Khóa học giúp học sinh nâng cao kỹ năng toán học', 25, N'4 tháng'),
('KH00003', N'Khóa học Vẽ Nghệ Thuật', 1800000.00, N'Khóa học phát triển khả năng sáng tạo qua hội họa', 20, N'2 tháng'),
('KH00004', N'Khóa học Khoa Học', 1700000.00, N'Khóa học khám phá các lĩnh vực khoa học', 28, N'3 tháng'),
('KH00005', N'Khóa học Âm Nhạc', 1600000.00, N'Khóa học dạy các kỹ năng âm nhạc cơ bản', 15, N'2 tháng');
select * from KhoaHoc
INSERT INTO NgoaiKhoa (IdNK, TenNK, NgayBatDau, NgayKetThu, MoTa)
VALUES
('NK00001', N'Hội thảo Khoa học', '2024-01-15', '2024-01-16', N'Hội thảo về các vấn đề khoa học hiện đại'),
('NK00002', N'Trại hè Tiếng Anh', '2024-06-01', '2024-06-10', N'Trại hè giúp trẻ cải thiện kỹ năng tiếng Anh'),
('NK00003', N'Cuộc thi Vẽ', '2024-03-01', '2024-03-05', N'Cuộc thi dành cho tất cả học sinh yêu thích vẽ'),
('NK00004', N'Lễ hội Âm nhạc', '2024-04-20', '2024-04-21', N'Lễ hội trình diễn các tài năng âm nhạc của học sinh'),
('NK00005', N'Chương trình thể dục thể thao', '2024-05-01', '2024-05-30', N'Chương trình thể thao cho trẻ em');
select * from NgoaiKhoa
INSERT INTO GiaoVien (MaST, TrinhDoChuyenMon, SaoDanhGia)
VALUES
('ST001', N'Thạc sĩ Toán học', 4.5),
('ST002', N'Tiến sĩ Vật lý', 4.8),
('ST003', N'Thạc sĩ Hóa học', 4.3),
('ST004', N'Cử nhân Sinh học', 4.0),
('ST005', N'Thạc sĩ Ngữ văn', 4.7);
select * from GiaoVien
INSERT INTO HoaDon (IdHD, NgayLap, NgayHetHan, SoTien,TrangThai , MaST, IdHS)
VALUES
('HD00001', '2024-01-10', '2024-04-10', 1500000.00, N'chưa thanh toán' , 'ST001', 'HS000001'),
('HD00002', '2024-02-15', '2024-05-15', 2000000.00,N'đã thanh toán', 'ST002', 'HS000002'),
('HD00003', '2024-03-20', '2024-06-20', 1800000.00,N'đã thanh toán', 'ST003', 'HS000003'),
('HD00004', '2024-04-25', '2024-07-25', 1700000.00, N'chưa thanh toán' , 'ST004', 'HS000004'),
('HD00005', '2024-05-30', '2024-08-30', 1600000.00, N'chưa thanh toán' , 'ST005', 'HS000005');
select * from HoaDon
INSERT INTO DichVu (IdDV, TenDV, DonGia, MoTa, ThoiHan)
VALUES
('DV00001', N'Dịch vụ học thêm', 500000.00, N'Dịch vụ học thêm các môn học cơ bản', N'1 tháng'),
('DV00002', N'Chăm sóc sức khỏe', 700000.00, N'Chăm sóc sức khỏe cho học sinh', N'1 tháng'),
('DV00003', N'Dịch vụ tư vấn tâm lý', 600000.00, N'Tư vấn tâm lý cho học sinh', N'1 tháng'),
('DV00004', N'Đưa đón học sinh', 800000.00, N'Dịch vụ đưa đón học sinh hàng ngày', N'1 tháng'),
('DV00005', N'Lớp bồi dưỡng năng khiếu', 900000.00, N'Lớp bồi dưỡng cho học sinh có năng khiếu', N'2 tháng');
select * from DichVu

INSERT INTO Voucher (IdVoucher, NgayTao, NgayHetHan, PhanTramGiam, SoLuong, MaST)
VALUES
('VOU00001', '2024-01-01', '2024-12-31', 10.00, 100, 'ST001'),
('VOU00002', '2024-02-01', '2024-11-30', 15.00, 50, 'ST002'),
('VOU00003', '2024-03-01', '2024-10-31', 20.00, 75, 'ST003'),
('VOU00004', '2024-04-01', '2024-09-30', 5.00, 200, 'ST004'),
('VOU00005', '2024-05-01', '2024-08-31', 25.00, 30, 'ST005');
select * from Voucher
INSERT INTO VoucherCuaPH (IdPH, IdVoucher, SoLuong)
VALUES
('PH001', 'VOU00001', 5),
('PH002', 'VOU00002', 3),
('PH003', 'VOU00003', 10),
('PH004', 'VOU00004', 1),
('PH005', 'VOU00005', 4);
select * from VoucherCuaPH
INSERT INTO TinTuc (IdTinTuc, TieuDe, NoiDung, Anh, MaST)
VALUES
('TT00001', N'Tin tức về Khóa học mới', N'Chúng tôi xin thông báo về các khóa học mới.', 'link_to_image1.jpg', 'ST001'),
('TT00002', N'Thông báo Lễ hội Âm nhạc', N'Lễ hội Âm nhạc sẽ diễn ra vào cuối tháng này.', 'link_to_image2.jpg', 'ST002'),
('TT00003', N'Cập nhật chương trình học', N'Chương trình học đã được cập nhật.', 'link_to_image3.jpg', 'ST003'),
('TT00004', N'Khảo sát sự hài lòng', N'Chúng tôi cần ý kiến của bạn.', 'link_to_image4.jpg', 'ST004'),
('TT00005', N'Chương trình ngoại khóa sắp tới', N'Hãy đăng ký tham gia các chương trình ngoại khóa.', 'link_to_image5.jpg', 'ST005');
select * from TinTuc 

INSERT INTO HoaDon_KhoaHoc (IdKH, IdHD, SoLuong, NgayDK, NgayKT)
VALUES
('KH00001', 'HD00001', 1, '2024-01-10', '2024-04-10'),
('KH00002', 'HD00002', 2, '2024-02-15', '2024-05-15'),
('KH00003', 'HD00003', 1, '2024-03-20', '2024-06-20'),
('KH00004', 'HD00004', 3, '2024-04-25', '2024-07-25'),
('KH00005', 'HD00005', 1, '2024-05-30', '2024-08-30');
select * from HoaDon_KhoaHoc
INSERT INTO NgoaiKhoa_GiaoVien (IdNK, MaST)
VALUES
('NK00001', 'ST001'),
('NK00002', 'ST002'),
('NK00003', 'ST003'),
('NK00004', 'ST004'),
('NK00005', 'ST005');
select * from NgoaiKhoa_GiaoVien
INSERT INTO HoaDon_DichVu (IdHD, IdDV, SoLuong, NgayDK, NgayHetHan)
VALUES
('HD00001', 'DV00001', 2, '2024-01-10', '2024-02-10'),
('HD00002', 'DV00002', 1, '2024-02-15', '2024-03-15'),
('HD00003', 'DV00003', 3, '2024-03-20', '2024-04-20'),
('HD00004', 'DV00004', 1, '2024-04-25', '2024-05-25'),
('HD00005', 'DV00005', 2, '2024-05-30', '2024-06-30');
select * from HoaDon_DichVu
INSERT INTO HocSinh_Lop (IdHS, DiemChuyenCan , IdLop)
VALUES 
('HS000001', 8.5 ,'L00001'),
('HS000002', 9.0 ,'L00002'),
('HS000003', 7.5 ,'L00003'),
('HS000004', 10,'L00004'),
('HS000005', 9.5,'L00005');
select * from HocSinh_Lop
INSERT INTO DiemDanh (IdDD, IdHS, IdLop, Ngay, TrangThai, TrangThaiNghi)
VALUES 
('DD001', 'HS000001', 'L00001', '2024-10-01', 1, 0),
('DD002', 'HS000002', 'L00002', '2024-10-01', 0, 1),
('DD003', 'HS000003', 'L00003', '2024-10-01', 1, 0),
('DD004', 'HS000004', 'L00004', '2024-10-01', 1, 0),
('DD005', 'HS000005', 'L00005', '2024-10-01', 1, 1),
('DD006', 'HS000001', 'L00001', '2024-10-02', 0, 0),
('DD007', 'HS000002', 'L00002', '2024-10-02', 1, 0),
('DD008', 'HS000003', 'L00003', '2024-10-02', 1, 1),
('DD009', 'HS000004', 'L00004', '2024-10-02', 1, 0),
('DD010', 'HS000005', 'L00005', '2024-10-02', 0, 1);

