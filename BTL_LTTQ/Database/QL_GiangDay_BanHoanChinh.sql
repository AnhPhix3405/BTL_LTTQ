-- ********************************************
-- DATABASE QUẢN LÝ GIẢNG DẠY - PHIÊN BẢN SAU KHI SỬA
-- Yêu cầu đã áp dụng cho PhanCongGiangDay:
--  - Bỏ GioBatDau, GioKetThuc
--  - Thêm CaHoc (1..5) với CHECK
--  - Thêm Thu (2..8) với CHECK, 2=Thứ Hai ... 8=Chủ Nhật
-- Tạo bởi: tranvanhoan05
-- Ngày: 2025-11-14
-- ********************************************

-- XÓA DATABASE CŨ (NẾU CÓ)
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'QL_GiangDay')
BEGIN
    ALTER DATABASE QL_GiangDay SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QL_GiangDay;
END
GO

-- TẠO DATABASE
CREATE DATABASE QL_GiangDay;
GO

USE QL_GiangDay;
GO

-- ============================================
-- 1. BẢNG: KHOA
-- ============================================
CREATE TABLE Khoa (
    MaKhoa VARCHAR(10) PRIMARY KEY,
    TenKhoa NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- ============================================
-- 2. BẢNG: MÔN HỌC
-- ============================================
CREATE TABLE MonHoc (
    MaMH VARCHAR(10) PRIMARY KEY,
    TenMH NVARCHAR(100) NOT NULL,
    SoTC INT CHECK (SoTC > 0) NOT NULL,
    SoTietLT INT DEFAULT 0,
    SoTietTH INT DEFAULT 0,
    HeSoDiem DECIMAL(3,2) DEFAULT 1.00 CHECK (HeSoDiem > 0),
    MaKhoa VARCHAR(10),
    CONSTRAINT FK_MonHoc_Khoa FOREIGN KEY (MaKhoa) 
        REFERENCES Khoa(MaKhoa) ON DELETE SET NULL
);
GO

-- ============================================
-- 3. BẢNG: GIẢNG VIÊN
-- ============================================
CREATE TABLE GiangVien (
    MaGV VARCHAR(10) PRIMARY KEY,
    TenGV NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15),
    DiaChi NVARCHAR(255),
    NgaySinh DATE,
    HocHam NVARCHAR(50),
    HocVi NVARCHAR(50),
    MaKhoa VARCHAR(10),
    CONSTRAINT FK_GiangVien_Khoa FOREIGN KEY (MaKhoa) 
        REFERENCES Khoa(MaKhoa) ON DELETE SET NULL
);
GO

-- ============================================
-- 4. BẢNG: SINH VIÊN
-- ============================================
CREATE TABLE SinhVien (
    MaSV VARCHAR(10) PRIMARY KEY,
    TenSV NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10) CHECK (GioiTinh IN (N'Nam', N'Nữ')),
    QueQuan NVARCHAR(100),
    NgayNhapHoc DATE,
    SDT NVARCHAR(15),
    Email NVARCHAR(100),
    MaKhoa VARCHAR(10),
    CONSTRAINT FK_SinhVien_Khoa FOREIGN KEY (MaKhoa) 
        REFERENCES Khoa(MaKhoa) ON DELETE SET NULL
);
GO

-- ============================================
-- 5. BẢNG: LỚP TÍN CHỈ
-- ============================================
CREATE TABLE LopTinChi (
    MaLop VARCHAR(10) PRIMARY KEY,
    TenLop NVARCHAR(100) NOT NULL,
    NamHoc INT CHECK (NamHoc > 2000),
    MaMH VARCHAR(10),
    TinhTrang BIT DEFAULT 0,  -- 0: chưa phân công, 1: đã phân công
    CONSTRAINT FK_LopTinChi_MonHoc FOREIGN KEY (MaMH) 
        REFERENCES MonHoc(MaMH) ON DELETE CASCADE
);
GO

-- ============================================
-- 6. BẢNG: TÀI KHOẢN
-- ============================================
CREATE TABLE TaiKhoan (
    MaTK VARCHAR(10) PRIMARY KEY,
    TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    LoaiTaiKhoan NVARCHAR(50) CHECK (LoaiTaiKhoan IN (N'Admin', N'Giảng viên')),
    MaGV VARCHAR(10),
    CONSTRAINT FK_TaiKhoan_GiangVien FOREIGN KEY (MaGV) 
        REFERENCES GiangVien(MaGV) ON DELETE SET NULL
);
GO

-- ============================================
-- 7. BẢNG: KHU VỰC (TÒA NHÀ)
-- ============================================
CREATE TABLE KhuVuc (
    MaKhuVuc VARCHAR(10) PRIMARY KEY,
    TenKhuVuc NVARCHAR(50) NOT NULL
);
GO

-- ============================================
-- 8. BẢNG: PHÒNG HỌC
-- ============================================
CREATE TABLE PhongHoc (
    MaPhong VARCHAR(20) PRIMARY KEY,  -- Ví dụ: A1-101, B-204, C-301
    MaKhuVuc VARCHAR(10),
    CONSTRAINT FK_PhongHoc_KhuVuc FOREIGN KEY (MaKhuVuc) 
        REFERENCES KhuVuc(MaKhuVuc) ON DELETE CASCADE
);
GO

-- ============================================
-- 9. BẢNG: PHÂN CÔNG GIẢNG DẠY (ĐÃ SỬA)
-- ============================================
CREATE TABLE PhanCongGiangDay (
    MaPC VARCHAR(10) PRIMARY KEY,
    NgayPC DATE NOT NULL,           
    NgayBatDau DATE NOT NULL,       
    NgayKetThuc DATE NOT NULL,      
    CaHoc TINYINT NOT NULL,         -- 1..5
    Thu TINYINT NOT NULL,           -- 2..8 (2=Thứ Hai ... 8=Chủ Nhật)
    MaPhong VARCHAR(20) NOT NULL,
    MaGV VARCHAR(10) NOT NULL,
    MaLop VARCHAR(10) NOT NULL,
    CONSTRAINT FK_PhanCong_PhongHoc FOREIGN KEY (MaPhong) 
        REFERENCES PhongHoc(MaPhong) ON DELETE CASCADE,
    CONSTRAINT FK_PhanCong_GiangVien FOREIGN KEY (MaGV) 
        REFERENCES GiangVien(MaGV) ON DELETE CASCADE,
    CONSTRAINT FK_PhanCong_LopTinChi FOREIGN KEY (MaLop) 
        REFERENCES LopTinChi(MaLop) ON DELETE CASCADE,
    CONSTRAINT CK_PhanCong_NgayHoc CHECK (NgayKetThuc >= NgayBatDau),
    CONSTRAINT CK_PhanCong_CaHoc CHECK (CaHoc BETWEEN 1 AND 5),
    CONSTRAINT CK_PhanCong_Thu CHECK (Thu BETWEEN 2 AND 8)
);
GO

-- ============================================
-- 10. BẢNG: ĐIỂM
-- ============================================
CREATE TABLE Diem (
    MaSV VARCHAR(10),
    MaLop VARCHAR(10),
    DiemCC DECIMAL(4,2) CHECK (DiemCC BETWEEN 0 AND 10),
    DiemGK DECIMAL(4,2) CHECK (DiemGK BETWEEN 0 AND 10),
    DiemThi DECIMAL(4,2) CHECK (DiemThi BETWEEN 0 AND 10),
    DiemTB AS (DiemCC * 0.1 + DiemGK * 0.3 + DiemThi * 0.6) PERSISTED,
    PRIMARY KEY (MaSV, MaLop),
    CONSTRAINT FK_Diem_SinhVien FOREIGN KEY (MaSV) 
        REFERENCES SinhVien(MaSV) ON DELETE CASCADE,
    CONSTRAINT FK_Diem_LopTinChi FOREIGN KEY (MaLop) 
        REFERENCES LopTinChi(MaLop) ON DELETE CASCADE
);
GO

-- ============================================
-- INDEXES
-- ============================================
CREATE INDEX IX_MonHoc_MaKhoa ON MonHoc(MaKhoa);
CREATE INDEX IX_GiangVien_MaKhoa ON GiangVien(MaKhoa);
CREATE INDEX IX_SinhVien_MaKhoa ON SinhVien(MaKhoa);
CREATE INDEX IX_LopTinChi_MaMH ON LopTinChi(MaMH);
CREATE INDEX IX_LopTinChi_TinhTrang ON LopTinChi(TinhTrang);
CREATE INDEX IX_TaiKhoan_MaGV ON TaiKhoan(MaGV);
CREATE INDEX IX_PhongHoc_KhuVuc ON PhongHoc(MaKhuVuc);
CREATE INDEX IX_PhanCong_Phong ON PhanCongGiangDay(MaPhong);
CREATE INDEX IX_PhanCong_GV ON PhanCongGiangDay(MaGV);
CREATE INDEX IX_PhanCong_Lop ON PhanCongGiangDay(MaLop);
CREATE INDEX IX_PhanCong_CaHoc ON PhanCongGiangDay(CaHoc);
CREATE INDEX IX_PhanCong_Thu ON PhanCongGiangDay(Thu);
CREATE INDEX IX_Diem_MaSV ON Diem(MaSV);
CREATE INDEX IX_Diem_MaLop ON Diem(MaLop);
GO

-- ============================================
-- DỮ LIỆU MẪU
-- ============================================

-- 1) KHOA
INSERT INTO Khoa (MaKhoa, TenKhoa) VALUES 
('CNTT', N'Công nghệ Thông tin'),
('KT',   N'Kinh tế'),
('XD',   N'Xây dựng'),
('NN',   N'Ngoại ngữ'),
('CK',   N'Cơ khí');
GO

-- 2) MÔN HỌC
INSERT INTO MonHoc (MaMH, TenMH, SoTC, SoTietLT, SoTietTH, HeSoDiem, MaKhoa) VALUES 
-- CNTT
('MH001', N'Cơ sở Dữ liệu',        3, 30, 15, 1.00, 'CNTT'),
('MH002', N'Lập trình Web',        3, 30, 15, 1.00, 'CNTT'),
('MH003', N'Mạng máy tính',        3, 30, 15, 1.00, 'CNTT'),
('MH004', N'Thiết kế Hệ thống',    3, 30, 15, 1.00, 'CNTT'),
('MH005', N'An toàn Thông tin',    2, 20, 10, 1.00, 'CNTT'),
('MH006', N'Trí tuệ Nhân tạo',     3, 30, 15, 1.00, 'CNTT'),
-- Kinh tế
('MH007', N'Kinh tế Vĩ mô',        2, 30,  0, 1.00, 'KT'),
('MH008', N'Quản trị Kinh doanh',  3, 45,  0, 1.00, 'KT'),
('MH009', N'Kế toán Tài chính',    3, 45,  0, 1.00, 'KT'),
('MH010', N'Marketing căn bản',    2, 30,  0, 1.00, 'KT'),
-- Xây dựng
('MH011', N'Kết cấu Công trình',   3, 30, 15, 1.00, 'XD'),
('MH012', N'Vật liệu Xây dựng',    2, 30,  0, 1.00, 'XD'),
('MH013', N'Cơ học Kết cấu',       3, 30, 15, 1.00, 'XD'),
-- Ngoại ngữ
('MH014', N'Tiếng Anh 1',          3, 45,  0, 1.00, 'NN'),
('MH015', N'Tiếng Anh 2',          3, 45,  0, 1.00, 'NN');
GO

-- 3) GIẢNG VIÊN
INSERT INTO GiangVien (MaGV, TenGV, SDT, DiaChi, NgaySinh, HocHam, HocVi, MaKhoa) VALUES 
-- CNTT
('GV001', N'Nguyễn Văn An',   '0912345678', N'Hà Nội',     '1980-05-15', N'PGS', N'Tiến sĩ',  'CNTT'),
('GV002', N'Trần Thị Bình',   '0987654321', N'TP.HCM',     '1985-11-20', N'TS',  N'Thạc sĩ',  'CNTT'),
('GV003', N'Phạm Minh Cường', '0934567890', N'Hải Phòng',  '1982-03-25', N'GS',  N'Tiến sĩ',  'CNTT'),
('GV004', N'Bùi Thị Dung',    '0988999000', N'Hải Dương',  '1990-06-06', NULL,   N'Thạc sĩ',  'CNTT'),
-- Kinh tế
('GV005', N'Lê Văn Em',       '0901122334', N'Đà Nẵng',    '1975-01-10', NULL,   N'Thạc sĩ',  'KT'),
('GV006', N'Đỗ Văn Phúc',     '0977888999', N'Quảng Ninh', '1983-12-12', N'PGS', N'Tiến sĩ',  'KT'),
('GV007', N'Hoàng Thị Giang', '0966777888', N'Thanh Hóa',  '1988-08-08', NULL,   N'Thạc sĩ',  'KT'),
-- Xây dựng
('GV008', N'Vũ Văn Hùng',     '0955666777', N'Nghệ An',    '1978-09-09', N'PGS', N'Tiến sĩ',  'XD'),
('GV009', N'Mai Thị Lan',     '0944555666', N'Hà Tĩnh',    '1986-04-04', NULL,   N'Thạc sĩ',  'XD'),
-- Ngoại ngữ
('GV010', N'Trịnh Văn Khoa',  '0933444555', N'Huế',        '1984-07-07', NULL,   N'Thạc sĩ',  'NN');
GO

-- 4) SINH VIÊN
INSERT INTO SinhVien (MaSV, TenSV, NgaySinh, GioiTinh, QueQuan, NgayNhapHoc, SDT, Email, MaKhoa) VALUES 
-- CNTT
('SV001', N'Nguyễn Minh Anh', '2002-03-25', N'Nam', N'Hải Phòng', '2020-09-05', '0911111111', 'sv001@email.com', 'CNTT'),
('SV002', N'Hoàng Thị Bảo',   '2003-07-10', N'Nữ',  N'Hà Nội',     '2021-09-05', '0922222222', 'sv002@email.com', 'CNTT'),
('SV003', N'Trần Văn Chiến',  '2002-11-20', N'Nam', N'Nam Định',   '2020-09-05', '0933333333', 'sv003@email.com', 'CNTT'),
('SV004', N'Nguyễn Lan Hương','2003-05-15', N'Nữ',  N'Đà Nẵng',    '2021-09-05', '0944444444', 'sv004@email.com', 'CNTT'),
('SV005', N'Phạm Văn Đức',    '2002-08-08', N'Nam', N'TP.HCM',     '2020-09-05', '0955555555', 'sv005@email.com', 'CNTT'),
('SV006', N'Lê Thị Hà',       '2003-02-14', N'Nữ',  N'Bắc Giang',  '2021-09-05', '0966666666', 'sv006@email.com', 'CNTT'),
('SV007', N'Vũ Minh Tuấn',    '2002-09-09', N'Nam', N'Hà Nội',     '2020-09-05', '0977777777', 'sv007@email.com', 'CNTT'),
-- Kinh tế
('SV008', N'Mai Văn Khôi',    '2002-01-01', N'Nam', N'Thanh Hóa',  '2020-09-05', '0988888888', 'sv008@email.com', 'KT'),
('SV009', N'Đặng Thị Lan',    '2003-04-20', N'Nữ',  N'Nghệ An',    '2021-09-05', '0999999999', 'sv009@email.com', 'KT'),
('SV010', N'Trịnh Văn Nam',   '2002-12-25', N'Nam', N'Huế',        '2020-09-05', '0900000000', 'sv010@email.com', 'KT'),
('SV011', N'Phan Thị Oanh',   '2003-06-30', N'Nữ',  N'Quảng Bình', '2021-09-05', '0911222333', 'sv011@email.com', 'KT'),
-- Xây dựng
('SV012', N'Lý Minh Phương',  '2002-10-10', N'Nam', N'Hải Dương',  '2020-09-05', '0922333444', 'sv012@email.com', 'XD'),
('SV013', N'Bùi Thị Quỳnh',   '2003-03-15', N'Nữ',  N'Vĩnh Phúc',  '2021-09-05', '0933444555', 'sv013@email.com', 'XD'),
('SV014', N'Đinh Văn Sơn',    '2002-07-20', N'Nam', N'Phú Thọ',    '2020-09-05', '0944555666', 'sv014@email.com', 'XD'),
-- Ngoại ngữ
('SV015', N'Hồ Thị Tâm',      '2003-01-25', N'Nữ',  N'Bắc Ninh',   '2021-09-05', '0955666777', 'sv015@email.com', 'NN');
GO

-- 5) LỚP TÍN CHỈ
INSERT INTO LopTinChi (MaLop, TenLop, NamHoc, MaMH, TinhTrang) VALUES 
-- 2022
('LTC001', N'CSDL-K20A', 2022, 'MH001', 1),
('LTC002', N'CSDL-K20B', 2022, 'MH001', 1),
('LTC003', N'LTW-K20',   2022, 'MH002', 1),
('LTC004', N'KTVM-K20',  2022, 'MH007', 1),
('LTC005', N'QTKD-K20',  2022, 'MH008', 0),
('LTC006', N'KTCT-K20',  2022, 'MH011', 1),
('LTC007', N'VLXD-K20',  2022, 'MH012', 0),
-- 2023
('LTC008', N'MMT-K21',   2023, 'MH003', 1),
('LTC009', N'TKHT-K21',  2023, 'MH004', 0),
('LTC010', N'ATTT-K21',  2023, 'MH005', 1),
('LTC011', N'KTTC-K21',  2023, 'MH009', 0),
('LTC012', N'TA1-K21',   2023, 'MH014', 1),
('LTC013', N'TA2-K21',   2023, 'MH015', 0);
GO

-- 6) TÀI KHOẢN
INSERT INTO TaiKhoan (MaTK, TenDangNhap, MatKhau, LoaiTaiKhoan, MaGV) VALUES 
('TKADM','admin',         'admin123', N'Admin',       NULL),
('TK001','nguyenvanan',   'gv001',    N'Giảng viên', 'GV001'),
('TK002','tranthibinh',   'gv002',    N'Giảng viên', 'GV002'),
('TK003','phamminhcuong', 'gv003',    N'Giảng viên', 'GV003'),
('TK004','buithidung',    'gv004',    N'Giảng viên', 'GV004'),
('TK005','levanem',       'gv005',    N'Giảng viên', 'GV005'),
('TK006','dovanphuc',     'gv006',    N'Giảng viên', 'GV006'),
('TK007','hoangthigiang', 'gv007',    N'Giảng viên', 'GV007'),
('TK008','vuvanhung',     'gv008',    N'Giảng viên', 'GV008'),
('TK009','maithilan',     'gv009',    N'Giảng viên', 'GV009'),
('TK010','trinhvankhoa',  'gv010',    N'Giảng viên', 'GV010');
GO

-- 7) KHU VỰC
INSERT INTO KhuVuc (MaKhuVuc, TenKhuVuc) VALUES 
('A1', N'Tòa A1'), ('A2', N'Tòa A2'), ('A3', N'Tòa A3'), ('A4', N'Tòa A4'),
('A5', N'Tòa A5'), ('A6', N'Tòa A6'), ('A7', N'Tòa A7'), ('A8', N'Tòa A8'),
('B',  N'Tòa B'),  ('C',  N'Tòa C');
GO

-- 8) PHÒNG HỌC
-- Tòa A1
INSERT INTO PhongHoc (MaPhong, MaKhuVuc) VALUES 
('A1-101','A1'),('A1-102','A1'),('A1-103','A1'),('A1-104','A1'),('A1-105','A1'),
('A1-201','A1'),('A1-202','A1'),('A1-203','A1'),('A1-204','A1'),
('A1-301','A1'),('A1-302','A1'),('A1-303','A1'),
('A1-401','A1'),('A1-402','A1'),('A1-403','A1');
-- Tòa A2
INSERT INTO PhongHoc (MaPhong, MaKhuVuc) VALUES 
('A2-101','A2'),('A2-102','A2'),('A2-103','A2'),('A2-104','A2'),('A2-105','A2'),('A2-106','A2'),
('A2-201','A2'),('A2-202','A2'),('A2-203','A2'),('A2-204','A2'),
('A2-301','A2'),('A2-302','A2'),('A2-303','A2'),('A2-304','A2'),
('A2-401','A2'),('A2-402','A2'),('A2-403','A2'),
('A2-501','A2'),('A2-502','A2'),('A2-503','A2'),('A2-504','A2');
-- Tòa A3-A8 (mỗi tòa 16 phòng)
DECLARE @i INT = 3;
WHILE @i <= 8
BEGIN
    DECLARE @toa VARCHAR(10) = 'A' + CAST(@i AS VARCHAR);
    INSERT INTO PhongHoc (MaPhong, MaKhuVuc) VALUES
    (@toa + '-101', @toa), (@toa + '-102', @toa), (@toa + '-103', @toa), (@toa + '-104', @toa),
    (@toa + '-201', @toa), (@toa + '-202', @toa), (@toa + '-203', @toa), (@toa + '-204', @toa),
    (@toa + '-301', @toa), (@toa + '-302', @toa), (@toa + '-303', @toa), (@toa + '-304', @toa),
    (@toa + '-401', @toa), (@toa + '-402', @toa), (@toa + '-403', @toa), (@toa + '-404', @toa);
    SET @i += 1;
END
-- Tòa B
INSERT INTO PhongHoc (MaPhong, MaKhuVuc) VALUES 
('B-101','B'),('B-102','B'),('B-103','B'),
('B-201','B'),('B-202','B'),
('B-301','B'),('B-302','B'),('B-303','B');
-- Tòa C
INSERT INTO PhongHoc (MaPhong, MaKhuVuc) VALUES 
('C-101','C'),('C-102','C'),('C-103','C'),('C-104','C'),
('C-201','C'),('C-202','C'),('C-203','C'),
('C-301','C'),('C-302','C'),
('C-401','C'),('C-402','C'),('C-403','C');
GO

-- 9) PHÂN CÔNG GIẢNG DẠY (DỮ LIỆU MẪU ĐÃ CHUYỂN SANG CaHoc/Thu)
-- Quy ước CaHoc: 1=07:30–09:30, 2=09:45–11:45, 3=13:00–15:00, 4=15:15–17:15, 5=khác
-- Thu: 2=Thứ Hai, 3=Thứ Ba, ..., 8=Chủ Nhật
INSERT INTO PhanCongGiangDay (MaPC, NgayPC, NgayBatDau, NgayKetThuc, CaHoc, Thu, MaPhong, MaGV, MaLop) VALUES
-- Học kỳ 1 năm 2022 (NgayBatDau 2022-09-05 là Thứ Hai -> Thu=2)
('PC001','2022-08-01','2022-09-05','2022-12-20', 1,2,'A1-101','GV001','LTC001'),
('PC002','2022-08-01','2022-09-05','2022-12-20', 3,2,'A1-102','GV002','LTC002'),
('PC003','2022-08-01','2022-09-05','2022-12-20', 2,2,'A2-101','GV003','LTC003'),
('PC004','2022-08-01','2022-09-05','2022-12-20', 4,2,'B-101',  'GV005','LTC004'),
('PC005','2022-08-01','2022-09-05','2022-12-20', 1,2,'C-201',  'GV008','LTC006'),
-- Học kỳ 1 năm 2023 (NgayBatDau 2023-09-05 là Thứ Ba -> Thu=3)
('PC006','2023-08-01','2023-09-05','2023-12-20', 3,3,'A3-201','GV004','LTC008'),
('PC007','2023-08-01','2023-09-05','2023-12-20', 2,3,'A4-301','GV002','LTC010'),
('PC008','2023-08-01','2023-09-05','2023-12-20', 1,3,'B-201',  'GV010','LTC012');
GO

-- 10) ĐIỂM
INSERT INTO Diem (MaSV, MaLop, DiemCC, DiemGK, DiemThi) VALUES 
-- LTC001
('SV001','LTC001',9.00,7.50,8.00),
('SV002','LTC001',8.00,6.50,7.00),
('SV003','LTC001',10.00,9.00,8.50),
('SV005','LTC001',7.50,8.00,7.50),
-- LTC002
('SV004','LTC002',9.50,9.00,9.50),
('SV006','LTC002',8.50,7.00,8.00),
('SV007','LTC002',9.00,8.50,9.00),
-- LTC003
('SV001','LTC003',8.00,8.50,8.00),
('SV002','LTC003',7.50,7.00,7.50),
('SV003','LTC003',10.00,9.50,9.00),
('SV005','LTC003',8.50,8.00,8.50),
-- LTC004
('SV008','LTC004',7.00,7.50,7.00),
('SV009','LTC004',8.50,8.00,8.50),
('SV010','LTC004',9.00,8.50,8.00),
-- LTC006
('SV012','LTC006',8.00,7.50,8.50),
('SV013','LTC006',9.00,9.00,9.50),
('SV014','LTC006',7.50,8.00,7.00),
-- LTC008
('SV004','LTC008',9.50,9.00,9.50),
('SV006','LTC008',8.00,8.50,9.00),
('SV007','LTC008',10.00,9.50,9.00),
-- LTC010
('SV001','LTC010',8.50,8.00,8.50),
('SV003','LTC010',9.00,9.00,9.50),
('SV005','LTC010',7.00,7.50,8.00),
-- LTC012
('SV015','LTC012',8.00,8.50,9.00);
GO

