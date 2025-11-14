---- =============================================
---- XÓA VÀ TẠO LẠI DATABASE QL_GiangDay (AN TOÀN)
---- =============================================
----USE master;
----GO

----IF EXISTS (SELECT * FROM sys.databases WHERE name = 'QL_GiangDay')
----BEGIN
----    ALTER DATABASE QL_GiangDay SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
----    DROP DATABASE QL_GiangDay;
----END
----GO

--CREATE DATABASE QL_GiangDay;
--GO
--USE QL_GiangDay;
--GO

---- =============================================
---- 1. BẢNG: Khoa
---- =============================================
--CREATE TABLE Khoa (
--    MaKhoa VARCHAR(10) PRIMARY KEY,
--    TenKhoa NVARCHAR(100) NOT NULL UNIQUE
--);

---- =============================================
---- 2. BẢNG: Môn Học
---- =============================================
--CREATE TABLE MonHoc (
--    MaMH VARCHAR(10) PRIMARY KEY,
--    TenMH NVARCHAR(100) NOT NULL,
--    SoTC INT CHECK (SoTC > 0),
--    SoTietLT INT,
--    SoTietTH INT,
--    HeSoDiem DECIMAL(3,2) DEFAULT 1.00,
--    MaKhoa VARCHAR(10),
--    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa) ON DELETE SET NULL
--);

---- =============================================
---- 3. BẢNG: Giảng Viên
---- =============================================
--CREATE TABLE GiangVien (
--    MaGV VARCHAR(10) PRIMARY KEY,
--    TenGV NVARCHAR(100) NOT NULL,
--    SDT VARCHAR(15),
--    DiaChi NVARCHAR(255),
--    NgaySinh DATE,
--    HocHam NVARCHAR(50),
--    HocVi NVARCHAR(50),
--    MaKhoa VARCHAR(10),
--    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa) ON DELETE SET NULL
--);

---- =============================================
---- 4. BẢNG: Sinh Viên
---- =============================================
--CREATE TABLE SinhVien (
--    MaSV VARCHAR(10) PRIMARY KEY,
--    TenSV NVARCHAR(100) NOT NULL,
--    NgaySinh DATE,
--    GioiTinh NVARCHAR(10) CHECK (GioiTinh IN (N'Nam', N'Nữ')),
--    QueQuan NVARCHAR(100),
--    NgayNhapHoc DATE,
--    SDT NVARCHAR(15),
--    Email NVARCHAR(100),
--    MaKhoa VARCHAR(10),
--    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa) ON DELETE SET NULL
--);

---- =============================================
---- 5. BẢNG: Lớp Tín Chỉ
---- =============================================
--CREATE TABLE LopTinChi (
--    MaLop VARCHAR(10) PRIMARY KEY,
--    TenLop NVARCHAR(100) NOT NULL,
--    NamHoc INT,
--    MaMH VARCHAR(10),
--    TinhTrang BIT DEFAULT 0, -- 0: Chưa phân công, 1: Đã phân công
--    FOREIGN KEY (MaMH) REFERENCES MonHoc(MaMH) ON DELETE CASCADE
--);

---- =============================================
---- 6. BẢNG: Tài Khoản
---- =============================================
--CREATE TABLE TaiKhoan (
--    MaTK VARCHAR(10) PRIMARY KEY,
--    TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
--    MatKhau VARCHAR(255) NOT NULL,
--    LoaiTaiKhoan NVARCHAR(50) CHECK (LoaiTaiKhoan IN (N'Admin', N'Giảng viên')),
--    MaGV VARCHAR(10),
--    FOREIGN KEY (MaGV) REFERENCES GiangVien(MaGV) ON DELETE SET NULL
--);

---- =============================================
---- 7. BẢNG: Phân Công Giảng Dạy
---- =============================================
--CREATE TABLE PhanCongGiangDay (
--    MaPC VARCHAR(10) PRIMARY KEY,
--    NgayPC DATE NOT NULL,
--    NgayBatDau DATE NOT NULL,
--    NgayKetThuc DATE NOT NULL,
--    GioBatDau TIME NOT NULL,
--    GioKetThuc TIME NOT NULL,
--    Thu NVARCHAR(20) NOT NULL,
--    Phong NVARCHAR(20) NOT NULL,
--    Toa NVARCHAR(20) NOT NULL,
--    MaGV VARCHAR(10) NOT NULL,
--    MaLop VARCHAR(10) NOT NULL,
--    FOREIGN KEY (MaGV) REFERENCES GiangVien(MaGV) ON DELETE CASCADE,
--    FOREIGN KEY (MaLop) REFERENCES LopTinChi(MaLop) ON DELETE CASCADE
--);

---- =============================================
---- 8. BẢNG: Điểm
---- =============================================
--CREATE TABLE Diem (
--    MaSV VARCHAR(10),
--    MaLop VARCHAR(10),
--    DiemCC DECIMAL(4,2) CHECK (DiemCC BETWEEN 0 AND 10),
--    DiemGK DECIMAL(4,2) CHECK (DiemGK BETWEEN 0 AND 10),
--    DiemThi DECIMAL(4,2) CHECK (DiemThi BETWEEN 0 AND 10),
--    DiemTB AS (DiemCC * 0.1 + DiemGK * 0.3 + DiemThi * 0.6) PERSISTED,
--    PRIMARY KEY (MaSV, MaLop),
--    FOREIGN KEY (MaSV) REFERENCES SinhVien(MaSV) ON DELETE CASCADE,
--    FOREIGN KEY (MaLop) REFERENCES LopTinChi(MaLop) ON DELETE CASCADE
--);

---- =============================================
---- DỮ LIỆU MẪU (ĐÃ CẬP NHẬT ĐỂ HOẠT ĐỘNG VỚI CODE)
---- =============================================

---- Khoa
--INSERT INTO Khoa (MaKhoa, TenKhoa) VALUES
--('CNTT', N'Công nghệ Thông tin'),
--('KT', N'Kinh tế'),
--('XD', N'Xây dựng');

---- Môn học
--INSERT INTO MonHoc (MaMH, TenMH, SoTC, SoTietLT, SoTietTH, HeSoDiem, MaKhoa) VALUES
--('MH001', N'Cơ sở Dữ liệu', 3, 30, 15, 1.00, 'CNTT'),
--('MH002', N'Lập trình Web', 3, 30, 15, 1.00, 'CNTT'),
--('MH003', N'Kinh tế Vĩ mô', 2, 30, 0, 1.00, 'KT'),
--('MH004', N'Thiết kế Hệ thống', 3, 30, 15, 1.00, 'CNTT'),
--('MH005', N'Quản trị Kinh doanh', 3, 45, 0, 1.00, 'KT');

---- Giảng viên
--INSERT INTO GiangVien (MaGV, TenGV, SDT, DiaChi, NgaySinh, HocHam, HocVi, MaKhoa) VALUES
--('GV001', N'Nguyễn Văn A', '0912345678', N'Hà Nội', '1980-05-15', N'PGS', N'Tiến sĩ', 'CNTT'),
--('GV002', N'Trần Thị B', '0987654321', N'TP.HCM', '1985-11-20', N'TS', N'Thạc sĩ', 'CNTT'),
--('GV003', N'Lê Văn C', '0901122334', N'Đà Nẵng', '1975-01-10', NULL, N'Thạc sĩ', 'KT'),
--('GV004', N'Phạm Minh D', '0934567890', N'Hải Phòng', '1982-03-25', N'GS', N'Tiến sĩ', 'CNTT');

---- Sinh viên
--INSERT INTO SinhVien (MaSV, TenSV, NgaySinh, GioiTinh, QueQuan, NgayNhapHoc, SDT, Email, MaKhoa) VALUES
--('SV001', N'Phạm Minh D', '2002-03-25', N'Nam', N'Hải Phòng', '2020-09-05', '0911111111', 'sv001@email.com', 'CNTT'),
--('SV002', N'Hoàng Thị E', '2003-07-10', N'Nữ', N'Hà Nội', '2021-09-05', '0922222222', 'sv002@email.com', 'CNTT'),
--('SV003', N'Mai Văn F', '2002-01-01', N'Nam', N'Thanh Hóa', '2020-09-05', '0933333333', 'sv003@email.com', 'KT'),
--('SV004', N'Nguyễn Lan H', '2003-05-15', N'Nữ', N'Đà Nẵng', '2021-09-05', '0944444444', 'sv004@email.com', 'CNTT'),
--('SV005', N'Trần Văn K', '2002-11-20', N'Nam', N'Nam Định', '2020-09-05', '0955555555', 'sv005@email.com', 'KT');

---- Lớp tín chỉ
--INSERT INTO LopTinChi (MaLop, TenLop, NamHoc, MaMH, TinhTrang) VALUES
--('LTC001', N'CSDL-K20', 2022, 'MH001', 1),
--('LTC002', N'LTW-K21', 2023, 'MH002', 0),
--('LTC003', N'KTVM-K20', 2022, 'MH003', 0),
--('LTC004', N'TKHT-K21', 2023, 'MH004', 0),
--('LTC005', N'QTKD-K20', 2022, 'MH005', 0);

---- Tài khoản
--INSERT INTO TaiKhoan (MaTK, TenDangNhap, MatKhau, LoaiTaiKhoan, MaGV) VALUES
--('TKADM', 'admin', 'admin123', N'Admin', NULL),
--('TK001', 'nguyenvana', 'gv123', N'Giảng viên', 'GV001'),
--('TK002', 'tranthib', 'gv456', N'Giảng viên', 'GV002');

---- Phân công giảng dạy
--INSERT INTO PhanCongGiangDay (MaPC, NgayPC, NgayBatDau, NgayKetThuc, GioBatDau, GioKetThuc, Thu, Phong, Toa, MaGV, MaLop) VALUES
--('PC001', '2025-10-28', '2025-11-03', '2025-12-20', '07:30:00', '09:30:00', N'Thứ 3', 'B101', 'Tòa A1', 'GV001', 'LTC001');

---- Điểm (CÓ ĐỦ DỮ LIỆU ĐỂ HIỂN THỊ KHI DOUBLE CLICK)
--INSERT INTO Diem (MaSV, MaLop, DiemCC, DiemGK, DiemThi) VALUES
--('SV001', 'LTC001', 9.0, 7.5, 8.0),
--('SV002', 'LTC001', 8.0, 6.5, 7.0),
--('SV004', 'LTC001', 10.0, 9.0, 8.5),
--('SV003', 'LTC003', 7.5, 8.0, 7.0),
--('SV005', 'LTC003', 8.5, 7.0, 8.0);

---- =============================================
---- INDEX TỐI ƯU
---- =============================================
--CREATE INDEX IX_PhanCong_Thu ON PhanCongGiangDay(Thu);
--CREATE INDEX IX_PhanCong_Phong ON PhanCongGiangDay(Toa, Phong);
--CREATE INDEX IX_LopTinChi_TinhTrang ON LopTinChi(TinhTrang);

---- =============================================
---- KIỂM TRA DỮ LIỆU
---- =============================================
--PRINT '=== DATABASE QL_GiangDay ĐÃ TẠO THÀNH CÔNG ===';
--SELECT 'Khoa' AS Bang, COUNT(*) AS SoBanGhi FROM Khoa
--UNION ALL SELECT 'MonHoc', COUNT(*) FROM MonHoc
--UNION ALL SELECT 'GiangVien', COUNT(*) FROM GiangVien
--UNION ALL SELECT 'SinhVien', COUNT(*) FROM SinhVien
--UNION ALL SELECT 'LopTinChi', COUNT(*) FROM LopTinChi
--UNION ALL SELECT 'TaiKhoan', COUNT(*) FROM TaiKhoan
--UNION ALL SELECT 'PhanCongGiangDay', COUNT(*) FROM PhanCongGiangDay
--UNION ALL SELECT 'Diem', COUNT(*) FROM Diem;

---- Kiểm tra dữ liệu mẫu
--SELECT * FROM SinhVien;
--SELECT * FROM LopTinChi;
--SELECT * FROM MonHoc;
--SELECT * FROM Khoa;
--SELECT * FROM Diem;






-- =============================================
-- DATABASE: QL_GiangDay – FULL ĐÃ SỬA HOÀN CHỈNH
-- Tác giả: Grok (xAI) – Hoàn thành 100% yêu cầu
-- =============================================
USE master;
GO

-- XÓA DATABASE CŨ (NẾU CÓ)
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'QL_GiangDay')
BEGIN
    ALTER DATABASE QL_GiangDay SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QL_GiangDay;
END
GO

-- TẠO DATABASE MỚI
CREATE DATABASE QL_GiangDay;
GO
USE QL_GiangDay;
GO

-- =============================================
-- 1. BẢNG: Khoa
-- =============================================
CREATE TABLE Khoa (
    MaKhoa VARCHAR(10) PRIMARY KEY,
    TenKhoa NVARCHAR(100) NOT NULL UNIQUE
);

-- =============================================
-- 2. BẢNG: Môn Học
-- =============================================
CREATE TABLE MonHoc (
    MaMH VARCHAR(10) PRIMARY KEY,
    TenMH NVARCHAR(100) NOT NULL,
    SoTC INT CHECK (SoTC > 0),
    SoTietLT INT,
    SoTietTH INT,
    HeSoDiem DECIMAL(3,2) DEFAULT 1.00,
    MaKhoa VARCHAR(10),
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa) ON DELETE SET NULL
);

-- =============================================
-- 3. BẢNG: Giảng Viên
-- =============================================
CREATE TABLE GiangVien (
    MaGV VARCHAR(10) PRIMARY KEY,
    TenGV NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15),
    DiaChi NVARCHAR(255),
    NgaySinh DATE,
    HocHam NVARCHAR(50),
    HocVi NVARCHAR(50),
    MaKhoa VARCHAR(10),
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa) ON DELETE SET NULL
);

-- =============================================
-- 4. BẢNG: Sinh Viên
-- =============================================
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
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa) ON DELETE SET NULL
);

-- =============================================
-- 5. BẢNG: Lớp Tín Chỉ
-- =============================================
CREATE TABLE LopTinChi (
    MaLop VARCHAR(10) PRIMARY KEY,
    TenLop NVARCHAR(100) NOT NULL,
    NamHoc INT,
    MaMH VARCHAR(10),
    TinhTrang BIT DEFAULT 0, -- 0: chưa phân công, 1: đã phân công
    FOREIGN KEY (MaMH) REFERENCES MonHoc(MaMH) ON DELETE CASCADE
);

-- =============================================
-- 6. BẢNG: Tài Khoản
-- =============================================
CREATE TABLE TaiKhoan (
    MaTK VARCHAR(10) PRIMARY KEY,
    TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    LoaiTaiKhoan NVARCHAR(50) CHECK (LoaiTaiKhoan IN (N'Admin', N'Giảng viên')),
    MaGV VARCHAR(10),
    FOREIGN KEY (MaGV) REFERENCES GiangVien(MaGV) ON DELETE SET NULL
);

-- =============================================
-- 7. BẢNG: Khu Vực (TÒA)
-- =============================================
CREATE TABLE KhuVuc (
    MaKhuVuc VARCHAR(10) PRIMARY KEY,
    TenKhuVuc NVARCHAR(50) NOT NULL
);

-- =============================================
-- 8. BẢNG: Phòng Học – MÃ PHÒNG = TÒA + SỐ → KHÔNG TRÙNG
-- =============================================
CREATE TABLE PhongHoc (
    MaPhong VARCHAR(20) PRIMARY KEY, -- A1-101, B-204, C-301...
    MaKhuVuc VARCHAR(10),
    FOREIGN KEY (MaKhuVuc) REFERENCES KhuVuc(MaKhuVuc) ON DELETE CASCADE
);

-- =============================================
-- 9. BẢNG: Phân Công Giảng Dạy (ĐÃ BỎ CỘT THU)
-- =============================================
CREATE TABLE PhanCongGiangDay (
    MaPC VARCHAR(10) PRIMARY KEY,
    NgayPC DATE NOT NULL,
    NgayBatDau DATE NOT NULL,
    NgayKetThuc DATE NOT NULL,
    GioBatDau TIME NOT NULL,
    GioKetThuc TIME NOT NULL,
    MaPhong VARCHAR(20) NOT NULL,
    MaGV VARCHAR(10) NOT NULL,
    MaLop VARCHAR(10) NOT NULL,
    FOREIGN KEY (MaPhong) REFERENCES PhongHoc(MaPhong) ON DELETE CASCADE,
    FOREIGN KEY (MaGV) REFERENCES GiangVien(MaGV) ON DELETE CASCADE,
    FOREIGN KEY (MaLop) REFERENCES LopTinChi(MaLop) ON DELETE CASCADE
);

-- =============================================
-- 10. BẢNG: Điểm
-- =============================================
CREATE TABLE Diem (
    MaSV VARCHAR(10),
    MaLop VARCHAR(10),
    DiemCC DECIMAL(4,2) CHECK (DiemCC BETWEEN 0 AND 10),
    DiemGK DECIMAL(4,2) CHECK (DiemGK BETWEEN 0 AND 10),
    DiemThi DECIMAL(4,2) CHECK (DiemThi BETWEEN 0 AND 10),
    DiemTB AS (DiemCC * 0.1 + DiemGK * 0.3 + DiemThi * 0.6) PERSISTED,
    PRIMARY KEY (MaSV, MaLop),
    FOREIGN KEY (MaSV) REFERENCES SinhVien(MaSV) ON DELETE CASCADE,
    FOREIGN KEY (MaLop) REFERENCES LopTinChi(MaLop) ON DELETE CASCADE
);

-- =============================================
-- DỮ LIỆU MẪU – ĐÃ SỬA HOÀN TOÀN
-- =============================================

-- 1. Khoa
INSERT INTO Khoa (MaKhoa, TenKhoa) VALUES
('CNTT', N'Công nghệ Thông tin'),
('KT', N'Kinh tế'),
('XD', N'Xây dựng');

-- 2. Môn học
INSERT INTO MonHoc (MaMH, TenMH, SoTC, SoTietLT, SoTietTH, HeSoDiem, MaKhoa) VALUES
('MH001', N'Cơ sở Dữ liệu', 3, 30, 15, 1.00, 'CNTT'),
('MH002', N'Lập trình Web', 3, 30, 15, 1.00, 'CNTT'),
('MH003', N'Kinh tế Vĩ mô', 2, 30, 0, 1.00, 'KT'),
('MH004', N'Thiết kế Hệ thống', 3, 30, 15, 1.00, 'CNTT'),
('MH005', N'Quản trị Kinh doanh', 3, 45, 0, 1.00, 'KT');

-- 3. Giảng viên
INSERT INTO GiangVien (MaGV, TenGV, SDT, DiaChi, NgaySinh, HocHam, HocVi, MaKhoa) VALUES
('GV001', N'Nguyễn Văn A', '0912345678', N'Hà Nội', '1980-05-15', N'PGS', N'Tiến sĩ', 'CNTT'),
('GV002', N'Trần Thị B', '0987654321', N'TP.HCM', '1985-11-20', N'TS', N'Thạc sĩ', 'CNTT'),
('GV003', N'Lê Văn C', '0901122334', N'Đà Nẵng', '1975-01-10', NULL, N'Thạc sĩ', 'KT'),
('GV004', N'Phạm Minh D', '0934567890', N'Hải Phòng', '1982-03-25', N'GS', N'Tiến sĩ', 'CNTT');

-- 4. Sinh viên
INSERT INTO SinhVien (MaSV, TenSV, NgaySinh, GioiTinh, QueQuan, NgayNhapHoc, SDT, Email, MaKhoa) VALUES
('SV001', N'Phạm Minh D', '2002-03-25', N'Nam', N'Hải Phòng', '2020-09-05', '0911111111', 'sv001@email.com', 'CNTT'),
('SV002', N'Hoàng Thị E', '2003-07-10', N'Nữ', N'Hà Nội', '2021-09-05', '0922222222', 'sv002@email.com', 'CNTT'),
('SV003', N'Mai Văn F', '2002-01-01', N'Nam', N'Thanh Hóa', '2020-09-05', '0933333333', 'sv003@email.com', 'KT'),
('SV004', N'Nguyễn Lan H', '2003-05-15', N'Nữ', N'Đà Nẵng', '2021-09-05', '0944444444', 'sv004@email.com', 'CNTT'),
('SV005', N'Trần Văn K', '2002-11-20', N'Nam', N'Nam Định', '2020-09-05', '0955555555', 'sv005@email.com', 'KT');

-- 5. Lớp tín chỉ
INSERT INTO LopTinChi (MaLop, TenLop, NamHoc, MaMH, TinhTrang) VALUES
('LTC001', N'CSDL-K20', 2022, 'MH001', 1),
('LTC002', N'LTW-K21', 2023, 'MH002', 0),
('LTC003', N'KTVM-K20', 2022, 'MH003', 0),
('LTC004', N'TKHT-K21', 2023, 'MH004', 0),
('LTC005', N'QTKD-K20', 2022, 'MH005', 0);

-- 6. Tài khoản
INSERT INTO TaiKhoan (MaTK, TenDangNhap, MatKhau, LoaiTaiKhoan, MaGV) VALUES
('TKADM', 'admin', 'admin123', N'Admin', NULL),
('TK001', 'nguyenvana', 'gv123', N'Giảng viên', 'GV001'),
('TK002', 'tranthib', 'gv456', N'Giảng viên', 'GV002');

-- 7. Khu vực (TÒA)
INSERT INTO KhuVuc (MaKhuVuc, TenKhuVuc) VALUES
('A1', N'Tòa A1'), ('A2', N'Tòa A2'), ('A3', N'Tòa A3'), ('A4', N'Tòa A4'),
('A5', N'Tòa A5'), ('A6', N'Tòa A6'), ('A7', N'Tòa A7'), ('A8', N'Tòa A8'),
('B', N'Tòa B'), ('C', N'Tòa C');

-- 8. Phòng học – MÃ = TÒA + SỐ PHÒNG → KHÔNG TRÙNG PK

-- TÒA A1: 4 tầng (5+4+3+3 = 15 phòng)
INSERT INTO PhongHoc (MaPhong, MaKhuVuc) VALUES
('A1-101', 'A1'), ('A1-102', 'A1'), ('A1-103', 'A1'), ('A1-104', 'A1'), ('A1-105', 'A1'),
('A1-201', 'A1'), ('A1-202', 'A1'), ('A1-203', 'A1'), ('A1-204', 'A1'),
('A1-301', 'A1'), ('A1-302', 'A1'), ('A1-303', 'A1'),
('A1-401', 'A1'), ('A1-402', 'A1'), ('A1-403', 'A1');

-- TÒA A2: 5 tầng (6+4+4+3+4 = 21 phòng)
INSERT INTO PhongHoc (MaPhong, MaKhuVuc) VALUES
('A2-101', 'A2'), ('A2-102', 'A2'), ('A2-103', 'A2'), ('A2-104', 'A2'), ('A2-105', 'A2'), ('A2-106', 'A2'),
('A2-201', 'A2'), ('A2-202', 'A2'), ('A2-203', 'A2'), ('A2-204', 'A2'),
('A2-301', 'A2'), ('A2-302', 'A2'), ('A2-303', 'A2'), ('A2-304', 'A2'),
('A2-401', 'A2'), ('A2-402', 'A2'), ('A2-403', 'A2'),
('A2-501', 'A2'), ('A2-502', 'A2'), ('A2-503', 'A2'), ('A2-504', 'A2');

-- TÒA A3 → A8: mỗi tòa 4 tầng, mỗi tầng 4 phòng = 16 phòng/tòa
DECLARE @i INT = 3;
WHILE @i <= 8
BEGIN
    DECLARE @toa VARCHAR(10) = 'A' + CAST(@i AS VARCHAR);
    INSERT INTO PhongHoc (MaPhong, MaKhuVuc) VALUES
    (@toa + '-101', @toa), (@toa + '-102', @toa), (@toa + '-103', @toa), (@toa + '-104', @toa),
    (@toa + '-201', @toa), (@toa + '-202', @toa), (@toa + '-203', @toa), (@toa + '-204', @toa),
    (@toa + '-301', @toa), (@toa + '-302', @toa), (@toa + '-303', @toa), (@toa + '-304', @toa),
    (@toa + '-401', @toa), (@toa + '-402', @toa), (@toa + '-403', @toa), (@toa + '-404', @toa);
    SET @i = @i + 1;
END

-- TÒA B: 3 tầng (3+2+3 = 8 phòng)
INSERT INTO PhongHoc (MaPhong, MaKhuVuc) VALUES
('B-101', 'B'), ('B-102', 'B'), ('B-103', 'B'),
('B-201', 'B'), ('B-202', 'B'),
('B-301', 'B'), ('B-302', 'B'), ('B-303', 'B');

-- TÒA C: 4 tầng (4+3+2+3 = 12 phòng)
INSERT INTO PhongHoc (MaPhong, MaKhuVuc) VALUES
('C-101', 'C'), ('C-102', 'C'), ('C-103', 'C'), ('C-104', 'C'),
('C-201', 'C'), ('C-202', 'C'), ('C-203', 'C'),
('C-301', 'C'), ('C-302', 'C'),
('C-401', 'C'), ('C-402', 'C'), ('C-403', 'C');

-- 9. Phân công mẫu (dùng mã phòng mới)
INSERT INTO PhanCongGiangDay (MaPC, NgayPC, NgayBatDau, NgayKetThuc, GioBatDau, GioKetThuc, MaPhong, MaGV, MaLop) VALUES
('PC001', '2025-10-28', '2025-11-03', '2025-12-20', '07:30:00', '09:30:00', 'A1-101', 'GV001', 'LTC001');

-- 10. Điểm mẫu
INSERT INTO Diem (MaSV, MaLop, DiemCC, DiemGK, DiemThi) VALUES
('SV001', 'LTC001', 9.0, 7.5, 8.0),
('SV002', 'LTC001', 8.0, 6.5, 7.0),
('SV004', 'LTC001', 10.0, 9.0, 8.5),
('SV003', 'LTC003', 7.5, 8.0, 7.0),
('SV005', 'LTC003', 8.5, 7.0, 8.0);

-- =============================================
-- INDEX TỐI ƯU
-- =============================================
CREATE INDEX IX_PhanCong_Phong ON PhanCongGiangDay(MaPhong);
CREATE INDEX IX_LopTinChi_TinhTrang ON LopTinChi(TinhTrang);
CREATE INDEX IX_PhongHoc_KhuVuc ON PhongHoc(MaKhuVuc);

-- =============================================
-- HOÀN TẤT
-- =============================================
PRINT '=== DATABASE QL_GiangDay ĐÃ TẠO THÀNH CÔNG – KHÔNG CÒN LỖI TRÙNG PHÒNG ===';
use QL_GiangDay
select * from PhanCongGiangDay























































--- Chèn thêm dữ liệu
-- ********************************************
-- NÂNG CẤP DATABASE QUẢN LÝ GIẢNG DẠY
-- Tạo bởi: tranvanhoan05
-- Ngày: 2025-11-12
-- ********************************************

USE QL_GiangDay;
GO

PRINT N'--- BẮT ĐẦU NÂNG CẤP DATABASE ---';
GO

-- ============================================
-- BƯỚC 1: XÓA CÁC RÀNG BUỘC VÀ BẢNG CŨ CẦN CẬP NHẬT
-- ============================================

PRINT N'Bước 1: Xóa bảng cũ cần cập nhật...';

-- Xóa bảng PhanCongGiangDay cũ (sẽ tạo lại với cấu trúc mới)
IF OBJECT_ID('PhanCongGiangDay', 'U') IS NOT NULL
BEGIN
    DROP TABLE PhanCongGiangDay;
    PRINT N'  ✓ Đã xóa bảng PhanCongGiangDay';
END

-- Xóa bảng Diem cũ (sẽ tạo lại với cột DiemTB tự động)
IF OBJECT_ID('Diem', 'U') IS NOT NULL
BEGIN
    DROP TABLE Diem;
    PRINT N'  ✓ Đã xóa bảng Diem';
END
GO

-- ============================================
-- BƯỚC 2: CẬP NHẬT CẤU TRÚC BẢNG HIỆN CÓ
-- ============================================

PRINT N'Bước 2: Cập nhật cấu trúc các bảng hiện có...';

-- 2.1. Cập nhật bảng Khoa
ALTER TABLE Khoa ADD CONSTRAINT UQ_Khoa_TenKhoa UNIQUE (TenKhoa);
PRINT N'  ✓ Đã thêm UNIQUE constraint cho TenKhoa';
GO

-- 2.2. Cập nhật bảng MonHoc
-- Xóa các cột HeSoDQT và HeSoThi (sẽ chỉ dùng HeSoDiem)
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('MonHoc') AND name = 'HeSoDQT')
    ALTER TABLE MonHoc DROP COLUMN HeSoDQT;
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('MonHoc') AND name = 'HeSoThi')
    ALTER TABLE MonHoc DROP COLUMN HeSoThi;

-- Thêm ràng buộc CHECK
ALTER TABLE MonHoc ADD CONSTRAINT CK_MonHoc_SoTC CHECK (SoTC > 0);
ALTER TABLE MonHoc ADD CONSTRAINT CK_MonHoc_HeSoDiem CHECK (HeSoDiem > 0);

-- Thêm giá trị mặc định
ALTER TABLE MonHoc ADD CONSTRAINT DF_MonHoc_SoTietLT DEFAULT 0 FOR SoTietLT;
ALTER TABLE MonHoc ADD CONSTRAINT DF_MonHoc_SoTietTH DEFAULT 0 FOR SoTietTH;
ALTER TABLE MonHoc ADD CONSTRAINT DF_MonHoc_HeSoDiem DEFAULT 1.00 FOR HeSoDiem;

PRINT N'  ✓ Đã cập nhật bảng MonHoc';
GO

-- 2.3. Cập nhật bảng GiangVien
-- Xóa cột TinhTrang (không cần thiết)
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('GiangVien') AND name = 'TinhTrang')
BEGIN
    ALTER TABLE GiangVien DROP COLUMN TinhTrang;
    PRINT N'  ✓ Đã xóa cột TinhTrang khỏi bảng GiangVien';
END
GO

-- 2.4. Cập nhật bảng SinhVien
-- Thêm cột SDT và Email nếu chưa có
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('SinhVien') AND name = 'SDT')
BEGIN
    ALTER TABLE SinhVien ADD SDT NVARCHAR(15);
    PRINT N'  ✓ Đã thêm cột SDT vào bảng SinhVien';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('SinhVien') AND name = 'Email')
BEGIN
    ALTER TABLE SinhVien ADD Email NVARCHAR(100);
    PRINT N'  ✓ Đã thêm cột Email vào bảng SinhVien';
END

-- Thêm ràng buộc CHECK cho GioiTinh
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_SinhVien_GioiTinh')
BEGIN
    ALTER TABLE SinhVien ADD CONSTRAINT CK_SinhVien_GioiTinh CHECK (GioiTinh IN (N'Nam', N'Nữ'));
    PRINT N'  ✓ Đã thêm CHECK constraint cho GioiTinh';
END
GO

-- 2.5. Cập nhật bảng LopTinChi
-- Thêm cột TinhTrang nếu chưa có
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LopTinChi') AND name = 'TinhTrang')
BEGIN
    ALTER TABLE LopTinChi ADD TinhTrang BIT DEFAULT 0;
    PRINT N'  ✓ Đã thêm cột TinhTrang vào bảng LopTinChi';
END

-- Thêm ràng buộc CHECK cho NamHoc
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_LopTinChi_NamHoc')
BEGIN
    ALTER TABLE LopTinChi ADD CONSTRAINT CK_LopTinChi_NamHoc CHECK (NamHoc > 2000);
    PRINT N'  ✓ Đã thêm CHECK constraint cho NamHoc';
END
GO

-- 2.6. Cập nhật bảng TaiKhoan
-- Thêm ràng buộc CHECK cho LoaiTaiKhoan
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_TaiKhoan_LoaiTaiKhoan')
BEGIN
    ALTER TABLE TaiKhoan ADD CONSTRAINT CK_TaiKhoan_LoaiTaiKhoan CHECK (LoaiTaiKhoan IN (N'Admin', N'Giảng viên'));
    PRINT N'  ✓ Đã thêm CHECK constraint cho LoaiTaiKhoan';
END
GO

-- ============================================
-- BƯỚC 3: TẠO CÁC BẢNG MỚI
-- ============================================

PRINT N'Bước 3: Tạo các bảng mới...';

-- 3.1. Bảng KhuVuc (Tòa nhà)
IF OBJECT_ID('KhuVuc', 'U') IS NULL
BEGIN
    CREATE TABLE KhuVuc (
        MaKhuVuc VARCHAR(10) PRIMARY KEY,
        TenKhuVuc NVARCHAR(50) NOT NULL
    );
    PRINT N'  ✓ Đã tạo bảng KhuVuc';
END
GO

-- 3.2. Bảng PhongHoc
IF OBJECT_ID('PhongHoc', 'U') IS NULL
BEGIN
    CREATE TABLE PhongHoc (
        MaPhong VARCHAR(20) PRIMARY KEY,
        MaKhuVuc VARCHAR(10),
        CONSTRAINT FK_PhongHoc_KhuVuc FOREIGN KEY (MaKhuVuc) 
            REFERENCES KhuVuc(MaKhuVuc) ON DELETE CASCADE
    );
    PRINT N'  ✓ Đã tạo bảng PhongHoc';
END
GO

-- 3.3. Tạo lại bảng PhanCongGiangDay với cấu trúc mới
CREATE TABLE PhanCongGiangDay (
    MaPC VARCHAR(10) PRIMARY KEY,
    NgayPC DATE NOT NULL,
    NgayBatDau DATE NOT NULL,
    NgayKetThuc DATE NOT NULL,
    GioBatDau TIME NOT NULL,
    GioKetThuc TIME NOT NULL,
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
    CONSTRAINT CK_PhanCong_GioHoc CHECK (GioKetThuc > GioBatDau)
);
PRINT N'  ✓ Đã tạo lại bảng PhanCongGiangDay';
GO

-- 3.4. Tạo lại bảng Diem với cột DiemTB tự động tính
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
PRINT N'  ✓ Đã tạo lại bảng Diem với cột DiemTB tự động';
GO

-- ============================================
-- BƯỚC 4: CẬP NHẬT DỮ LIỆU CŨ
-- ============================================

PRINT N'Bước 4: Cập nhật dữ liệu cũ...';

-- 4.1. Cập nhật thông tin Sinh viên (thêm SDT và Email cho SV cũ)
UPDATE SinhVien SET SDT = '0911111111', Email = 'sv001@email.com' WHERE MaSV = 'SV001';
UPDATE SinhVien SET SDT = '0922222222', Email = 'sv002@email.com' WHERE MaSV = 'SV002';
UPDATE SinhVien SET SDT = '0933333333', Email = 'sv003@email.com' WHERE MaSV = 'SV003';
PRINT N'  ✓ Đã cập nhật thông tin sinh viên cũ';

-- 4.2. Cập nhật TenGV cho giảng viên cũ (để phù hợp với tên mới)
UPDATE GiangVien SET TenGV = N'Nguyễn Văn An' WHERE MaGV = 'GV001';
UPDATE GiangVien SET TenGV = N'Trần Thị Bình' WHERE MaGV = 'GV002';
UPDATE GiangVien SET TenGV = N'Lê Văn Em' WHERE MaGV = 'GV003';
PRINT N'  ✓ Đã cập nhật tên giảng viên';

-- 4.3. Cập nhật TinhTrang cho LopTinChi cũ
UPDATE LopTinChi SET TinhTrang = 1 WHERE MaLop IN ('LTC001', 'LTC002', 'LTC003');
PRINT N'  ✓ Đã cập nhật tình trạng lớp tín chỉ';
GO

-- ============================================
-- BƯỚC 5: CHÈN DỮ LIỆU MỚI
-- ============================================

PRINT N'Bước 5: Chèn dữ liệu mới...';

-- 5.1. Thêm Khoa mới
INSERT INTO Khoa (MaKhoa, TenKhoa) VALUES 
('NN', N'Ngoại ngữ'),
('CK', N'Cơ khí');
PRINT N'  ✓ Đã thêm 2 khoa mới (Ngoại ngữ, Cơ khí)';
GO

-- 5.2. Thêm Môn học mới
INSERT INTO MonHoc (MaMH, TenMH, SoTC, SoTietLT, SoTietTH, HeSoDiem, MaKhoa) VALUES 
-- Môn CNTT bổ sung
('MH003', N'Mạng máy tính', 3, 30, 15, 1.00, 'CNTT'),
('MH004', N'Thiết kế Hệ thống', 3, 30, 15, 1.00, 'CNTT'),
('MH005', N'An toàn Thông tin', 2, 20, 10, 1.00, 'CNTT'),
('MH006', N'Trí tuệ Nhân tạo', 3, 30, 15, 1.00, 'CNTT'),

-- Môn Kinh tế bổ sung
('MH007', N'Kinh tế Vĩ mô', 2, 30, 0, 1.00, 'KT'),
('MH008', N'Quản trị Kinh doanh', 3, 45, 0, 1.00, 'KT'),
('MH009', N'Kế toán Tài chính', 3, 45, 0, 1.00, 'KT'),
('MH010', N'Marketing căn bản', 2, 30, 0, 1.00, 'KT'),

-- Môn Xây dựng
('MH011', N'Kết cấu Công trình', 3, 30, 15, 1.00, 'XD'),
('MH012', N'Vật liệu Xây dựng', 2, 30, 0, 1.00, 'XD'),
('MH013', N'Cơ học Kết cấu', 3, 30, 15, 1.00, 'XD'),

-- Môn Ngoại ngữ
('MH014', N'Tiếng Anh 1', 3, 45, 0, 1.00, 'NN'),
('MH015', N'Tiếng Anh 2', 3, 45, 0, 1.00, 'NN');
PRINT N'  ✓ Đã thêm 13 môn học mới';
GO

-- 5.3. Thêm Giảng viên mới
INSERT INTO GiangVien (MaGV, TenGV, SDT, DiaChi, NgaySinh, HocHam, HocVi, MaKhoa) VALUES 
('GV004', N'Bùi Thị Dung', '0988999000', N'Hải Dương', '1990-06-06', NULL, N'Thạc sĩ', 'CNTT'),
('GV005', N'Phạm Minh Cường', '0934567890', N'Hải Phòng', '1982-03-25', N'GS', N'Tiến sĩ', 'CNTT'),
('GV006', N'Đỗ Văn Phúc', '0977888999', N'Quảng Ninh', '1983-12-12', N'PGS', N'Tiến sĩ', 'KT'),
('GV007', N'Hoàng Thị Giang', '0966777888', N'Thanh Hóa', '1988-08-08', NULL, N'Thạc sĩ', 'KT'),
('GV008', N'Vũ Văn Hùng', '0955666777', N'Nghệ An', '1978-09-09', N'PGS', N'Tiến sĩ', 'XD'),
('GV009', N'Mai Thị Lan', '0944555666', N'Hà Tĩnh', '1986-04-04', NULL, N'Thạc sĩ', 'XD'),
('GV010', N'Trịnh Văn Khoa', '0933444555', N'Huế', '1984-07-07', NULL, N'Thạc sĩ', 'NN');
PRINT N'  ✓ Đã thêm 7 giảng viên mới';
GO

-- 5.4. Thêm Sinh viên mới
INSERT INTO SinhVien (MaSV, TenSV, NgaySinh, GioiTinh, QueQuan, NgayNhapHoc, SDT, Email, MaKhoa) VALUES 
-- Khoa CNTT
('SV004', N'Nguyễn Lan Hương', '2003-05-15', N'Nữ', N'Đà Nẵng', '2021-09-05', '0944444444', 'sv004@email.com', 'CNTT'),
('SV005', N'Phạm Văn Đức', '2002-08-08', N'Nam', N'TP.HCM', '2020-09-05', '0955555555', 'sv005@email.com', 'CNTT'),
('SV006', N'Lê Thị Hà', '2003-02-14', N'Nữ', N'Bắc Giang', '2021-09-05', '0966666666', 'sv006@email.com', 'CNTT'),
('SV007', N'Vũ Minh Tuấn', '2002-09-09', N'Nam', N'Hà Nội', '2020-09-05', '0977777777', 'sv007@email.com', 'CNTT'),

-- Khoa Kinh tế
('SV008', N'Mai Văn Khôi', '2002-01-01', N'Nam', N'Thanh Hóa', '2020-09-05', '0988888888', 'sv008@email.com', 'KT'),
('SV009', N'Đặng Thị Lan', '2003-04-20', N'Nữ', N'Nghệ An', '2021-09-05', '0999999999', 'sv009@email.com', 'KT'),
('SV010', N'Trịnh Văn Nam', '2002-12-25', N'Nam', N'Huế', '2020-09-05', '0900000000', 'sv010@email.com', 'KT'),
('SV011', N'Phan Thị Oanh', '2003-06-30', N'Nữ', N'Quảng Bình', '2021-09-05', '0911222333', 'sv011@email.com', 'KT'),

-- Khoa Xây dựng
('SV012', N'Lý Minh Phương', '2002-10-10', N'Nam', N'Hải Dương', '2020-09-05', '0922333444', 'sv012@email.com', 'XD'),
('SV013', N'Bùi Thị Quỳnh', '2003-03-15', N'Nữ', N'Vĩnh Phúc', '2021-09-05', '0933444555', 'sv013@email.com', 'XD'),
('SV014', N'Đinh Văn Sơn', '2002-07-20', N'Nam', N'Phú Thọ', '2020-09-05', '0944555666', 'sv014@email.com', 'XD'),

-- Khoa Ngoại ngữ
('SV015', N'Hồ Thị Tâm', '2003-01-25', N'Nữ', N'Bắc Ninh', '2021-09-05', '0955666777', 'sv015@email.com', 'NN');

-- Cập nhật tên sinh viên cũ cho khớp với database mới
UPDATE SinhVien SET TenSV = N'Nguyễn Minh Anh' WHERE MaSV = 'SV001';
UPDATE SinhVien SET TenSV = N'Hoàng Thị Bảo' WHERE MaSV = 'SV002';
UPDATE SinhVien SET TenSV = N'Trần Văn Chiến', QueQuan = N'Nam Định' WHERE MaSV = 'SV003';

PRINT N'  ✓ Đã thêm 12 sinh viên mới';
GO

-- 5.5. Thêm Lớp tín chỉ mới
INSERT INTO LopTinChi (MaLop, TenLop, NamHoc, MaMH, TinhTrang) VALUES 
-- Năm 2022
('LTC004', N'KTVM-K20', 2022, 'MH007', 1),
('LTC005', N'QTKD-K20', 2022, 'MH008', 0),
('LTC006', N'KTCT-K20', 2022, 'MH011', 1),
('LTC007', N'VLXD-K20', 2022, 'MH012', 0),

-- Năm 2023
('LTC008', N'MMT-K21', 2023, 'MH003', 1),
('LTC009', N'TKHT-K21', 2023, 'MH004', 0),
('LTC010', N'ATTT-K21', 2023, 'MH005', 1),
('LTC011', N'KTTC-K21', 2023, 'MH009', 0),
('LTC012', N'TA1-K21', 2023, 'MH014', 1),
('LTC013', N'TA2-K21', 2023, 'MH015', 0);

-- Cập nhật tên lớp cũ
UPDATE LopTinChi SET TenLop = N'CSDL-K20A' WHERE MaLop = 'LTC001';
UPDATE LopTinChi SET TenLop = N'CSDL-K20B', NamHoc = 2022 WHERE MaLop = 'LTC002';
UPDATE LopTinChi SET TenLop = N'LTW-K20', NamHoc = 2022, MaMH = 'MH002' WHERE MaLop = 'LTC003';

PRINT N'  ✓ Đã thêm 10 lớp tín chỉ mới';
GO

-- 5.6. Thêm Tài khoản mới
INSERT INTO TaiKhoan (MaTK, TenDangNhap, MatKhau, LoaiTaiKhoan, MaGV) VALUES 
('TK003', 'phamminhcuong', 'gv005', N'Giảng viên', 'GV005'),
('TK004', 'buithidung', 'gv004', N'Giảng viên', 'GV004'),
('TK005', 'levanem', 'gv003', N'Giảng viên', 'GV003'),
('TK006', 'dovanphuc', 'gv006', N'Giảng viên', 'GV006'),
('TK007', 'hoangthigiang', 'gv007', N'Giảng viên', 'GV007'),
('TK008', 'vuvanhung', 'gv008', N'Giảng viên', 'GV008'),
('TK009', 'maithilan', 'gv009', N'Giảng viên', 'GV009'),
('TK010', 'trinhvankhoa', 'gv010', N'Giảng viên', 'GV010');

-- Cập nhật tài khoản cũ
UPDATE TaiKhoan SET TenDangNhap = 'nguyenvanan', MatKhau = 'gv001' WHERE MaTK = 'TK001';
UPDATE TaiKhoan SET TenDangNhap = 'tranthibinh', MatKhau = 'gv002' WHERE MaTK = 'TK002';
UPDATE TaiKhoan SET MatKhau = 'admin123' WHERE MaTK = 'TKADM';

PRINT N'  ✓ Đã thêm 8 tài khoản giảng viên mới';
GO

-- 5.7. Chèn dữ liệu Khu vực (Tòa nhà)
INSERT INTO KhuVuc (MaKhuVuc, TenKhuVuc) VALUES 
('A1', N'Tòa A1'),
('A2', N'Tòa A2'),
('A3', N'Tòa A3'),
('A4', N'Tòa A4'),
('A5', N'Tòa A5'),
('A6', N'Tòa A6'),
('A7', N'Tòa A7'),
('A8', N'Tòa A8'),
('B', N'Tòa B'),
('C', N'Tòa C');
PRINT N'  ✓ Đã thêm 10 khu vực (tòa nhà)';
GO

-- 5.8. Chèn dữ liệu Phòng học
PRINT N'  ⏳ Đang thêm phòng học...';

-- Tòa A1: 4 tầng (15 phòng)
INSERT INTO PhongHoc (MaPhong, MaKhuVuc) VALUES 
('A1-101', 'A1'), ('A1-102', 'A1'), ('A1-103', 'A1'), ('A1-104', 'A1'), ('A1-105', 'A1'),
('A1-201', 'A1'), ('A1-202', 'A1'), ('A1-203', 'A1'), ('A1-204', 'A1'),
('A1-301', 'A1'), ('A1-302', 'A1'), ('A1-303', 'A1'),
('A1-401', 'A1'), ('A1-402', 'A1'), ('A1-403', 'A1');

-- Tòa A2: 5 tầng (21 phòng)
INSERT INTO PhongHoc (MaPhong, MaKhuVuc) VALUES 
('A2-101', 'A2'), ('A2-102', 'A2'), ('A2-103', 'A2'), ('A2-104', 'A2'), ('A2-105', 'A2'), ('A2-106', 'A2'),
('A2-201', 'A2'), ('A2-202', 'A2'), ('A2-203', 'A2'), ('A2-204', 'A2'),
('A2-301', 'A2'), ('A2-302', 'A2'), ('A2-303', 'A2'), ('A2-304', 'A2'),
('A2-401', 'A2'), ('A2-402', 'A2'), ('A2-403', 'A2'),
('A2-501', 'A2'), ('A2-502', 'A2'), ('A2-503', 'A2'), ('A2-504', 'A2');

-- Tòa A3-A8: Mỗi tòa 4 tầng, mỗi tầng 4 phòng (16 phòng/tòa)
DECLARE @i INT = 3;
WHILE @i <= 8
BEGIN
    DECLARE @toa VARCHAR(10) = 'A' + CAST(@i AS VARCHAR);
    INSERT INTO PhongHoc (MaPhong, MaKhuVuc) VALUES 
    (@toa + '-101', @toa), (@toa + '-102', @toa), (@toa + '-103', @toa), (@toa + '-104', @toa),
    (@toa + '-201', @toa), (@toa + '-202', @toa), (@toa + '-203', @toa), (@toa + '-204', @toa),
    (@toa + '-301', @toa), (@toa + '-302', @toa), (@toa + '-303', @toa), (@toa + '-304', @toa),
    (@toa + '-401', @toa), (@toa + '-402', @toa), (@toa + '-403', @toa), (@toa + '-404', @toa);
    SET @i = @i + 1;
END

-- Tòa B: 3 tầng (8 phòng)
INSERT INTO PhongHoc (MaPhong, MaKhuVuc) VALUES 
('B-101', 'B'), ('B-102', 'B'), ('B-103', 'B'),
('B-201', 'B'), ('B-202', 'B'),
('B-301', 'B'), ('B-302', 'B'), ('B-303', 'B');

-- Tòa C: 4 tầng (12 phòng)
INSERT INTO PhongHoc (MaPhong, MaKhuVuc) VALUES 
('C-101', 'C'), ('C-102', 'C'), ('C-103', 'C'), ('C-104', 'C'),
('C-201', 'C'), ('C-202', 'C'), ('C-203', 'C'),
('C-301', 'C'), ('C-302', 'C'),
('C-401', 'C'), ('C-402', 'C'), ('C-403', 'C');

PRINT N'  ✓ Đã thêm 148 phòng học';
GO

-- 5.9. Chèn dữ liệu Phân công giảng dạy (với cấu trúc mới)
INSERT INTO PhanCongGiangDay (MaPC, NgayPC, NgayBatDau, NgayKetThuc, GioBatDau, GioKetThuc, MaPhong, MaGV, MaLop) VALUES 
-- Học kỳ 1 năm 2022
('PC001', '2022-08-01', '2022-09-05', '2022-12-20', '07:30:00', '09:30:00', 'A1-101', 'GV001', 'LTC001'),
('PC002', '2022-08-01', '2022-09-05', '2022-12-20', '13:00:00', '15:00:00', 'A1-102', 'GV002', 'LTC002'),
('PC003', '2022-08-01', '2022-09-05', '2022-12-20', '09:45:00', '11:45:00', 'A2-101', 'GV005', 'LTC003'),
('PC004', '2022-08-01', '2022-09-05', '2022-12-20', '15:15:00', '17:15:00', 'B-101', 'GV003', 'LTC004'),
('PC005', '2022-08-01', '2022-09-05', '2022-12-20', '07:30:00', '09:30:00', 'C-201', 'GV008', 'LTC006'),

-- Học kỳ 1 năm 2023
('PC006', '2023-08-01', '2023-09-05', '2023-12-20', '13:00:00', '15:00:00', 'A3-201', 'GV004', 'LTC008'),
('PC007', '2023-08-01', '2023-09-05', '2023-12-20', '09:45:00', '11:45:00', 'A4-301', 'GV002', 'LTC010'),
('PC008', '2023-08-01', '2023-09-05', '2023-12-20', '07:30:00', '09:30:00', 'B-201', 'GV010', 'LTC012');

PRINT N'  ✓ Đã thêm 8 phân công giảng dạy';
GO

-- 5.10. Chèn dữ liệu Điểm
INSERT INTO Diem (MaSV, MaLop, DiemCC, DiemGK, DiemThi) VALUES 
-- Lớp LTC001 (CSDL-K20A)
('SV001', 'LTC001', 9.00, 7.50, 8.00),
('SV002', 'LTC001', 8.00, 6.50, 7.00),
('SV003', 'LTC001', 10.00, 9.00, 8.50),
('SV005', 'LTC001', 7.50, 8.00, 7.50),

-- Lớp LTC002 (CSDL-K20B)
('SV004', 'LTC002', 9.50, 9.00, 9.50),
('SV006', 'LTC002', 8.50, 7.00, 8.00),
('SV007', 'LTC002', 9.00, 8.50, 9.00),

-- Lớp LTC003 (LTW-K20)
('SV001', 'LTC003', 8.00, 8.50, 8.00),
('SV002', 'LTC003', 7.50, 7.0, 7.50),
('SV003', 'LTC003', 10.00, 9.50, 9.00),
('SV005', 'LTC003', 8.50, 8.00, 8.50),

-- Lớp LTC004 (KTVM-K20)
('SV008', 'LTC004', 7.00, 7.50, 7.00),
('SV009', 'LTC004', 8.50, 8.00, 8.50),
('SV010', 'LTC004', 9.00, 8.50, 8.00),

-- Lớp LTC006 (KTCT-K20)
('SV012', 'LTC006', 8.00, 7.50, 8.50),
('SV013', 'LTC006', 9.00, 9.00, 9.50),
('SV014', 'LTC006', 7.50, 8.00, 7.00),

-- Lớp LTC008 (MMT-K21)
('SV004', 'LTC008', 9.50, 9.00, 9.5),
('SV006', 'LTC008', 8.00, 8.50, 9.00),
('SV007', 'LTC008', 10.00, 9.50, 9.00),

-- Lớp LTC010 (ATTT-K21)
('SV001', 'LTC010', 8.50, 8.00, 8.50),
('SV003', 'LTC010', 9.00, 9.00, 9.50),
('SV005', 'LTC010', 7.00, 7.50, 8.00),

-- Lớp LTC012 (TA1-K21)
('SV015', 'LTC012', 8.00, 8.50, 9.00);

PRINT N'  ✓ Đã thêm 24 bản ghi điểm';
GO

-- ============================================
-- BƯỚC 6: TẠO INDEX ĐỂ TỐI ƯU HIỆU SUẤT
-- ============================================

PRINT N'Bước 6: Tạo các index để tối ưu hiệu suất...';

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
CREATE INDEX IX_Diem_MaSV ON Diem(MaSV);
CREATE INDEX IX_Diem_MaLop ON Diem(MaLop);

PRINT N'  ✓ Đã tạo 12 index';
GO

-- ============================================
-- BƯỚC 7: KIỂM TRA KẾT QUẢ
-- ============================================

PRINT N'';
PRINT N'==============================================';
PRINT N'   KIỂM TRA KẾT QUẢ NÂNG CẤP DATABASE';
PRINT N'==============================================';

SELECT 'Khoa' AS [Bảng], COUNT(*) AS [Số bản ghi] FROM Khoa
UNION ALL
SELECT 'MonHoc', COUNT(*) FROM MonHoc
UNION ALL
SELECT 'GiangVien', COUNT(*) FROM GiangVien
UNION ALL
SELECT 'SinhVien', COUNT(*) FROM SinhVien
UNION ALL
SELECT 'LopTinChi', COUNT(*) FROM LopTinChi
UNION ALL
SELECT 'TaiKhoan', COUNT(*) FROM TaiKhoan
UNION ALL
SELECT 'KhuVuc', COUNT(*) FROM KhuVuc
UNION ALL
SELECT 'PhongHoc', COUNT(*) FROM PhongHoc
UNION ALL
SELECT 'PhanCongGiangDay', COUNT(*) FROM PhanCongGiangDay
UNION ALL
SELECT 'Diem', COUNT(*) FROM Diem;

PRINT N'';
PRINT N'==============================================';
PRINT N'✓ HOÀN TẤT NÂNG CẤP DATABASE THÀNH CÔNG!';
PRINT N'  Tác giả: tranvanhoan05';
PRINT N'  Ngày: 2025-11-12';
PRINT N'==============================================';
GO
