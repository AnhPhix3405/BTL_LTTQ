-- ********************************************
-- TẠO DATABASE QUẢN LÝ GIẢNG DẠY
-- ********************************************

CREATE DATABASE QL_GiangDay;
go
USE QL_GiangDay;
go
-- 1. Khoa (Tạo trước vì các bảng khác tham chiếu)
CREATE TABLE Khoa (
    MaKhoa VARCHAR(10) PRIMARY KEY,
    TenKhoa NVARCHAR(100) NOT NULL
);

-- 2. Môn Học (Tham chiếu Khoa)
CREATE TABLE MonHoc (
    MaMH VARCHAR(10) PRIMARY KEY,
    TenMH NVARCHAR(100) NOT NULL,
    SoTC INT,
    SoTietLT INT,
    SoTietTH INT,
    HeSoDiem DECIMAL(3, 2),
    MaKhoa VARCHAR(10),
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);

-- 3. Giảng Viên (Tham chiếu Khoa)
CREATE TABLE GiangVien (
    MaGV VARCHAR(10) PRIMARY KEY,
    TenGV NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15),
    DiaChi NVARCHAR(255),
    NgaySinh DATE,
    TinhTrang NVARCHAR(50),
    HocHam NVARCHAR(50),
    HocVi NVARCHAR(50),
    MaKhoa VARCHAR(10),
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);

-- 4. Sinh Viên (Tham chiếu Khoa)
CREATE TABLE SinhVien (
    MaSV VARCHAR(10) PRIMARY KEY,
    TenSV NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    QueQuan NVARCHAR(100),
    NgayNhapHoc DATE,
    MaKhoa VARCHAR(10),
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);

-- 5. Lớp tín chỉ (Tham chiếu Môn Học)
CREATE TABLE LopTinChi (
    MaLop VARCHAR(10) PRIMARY KEY,
    TenLop NVARCHAR(100),
    NamHoc INT,
    MaMH VARCHAR(10),
    FOREIGN KEY (MaMH) REFERENCES MonHoc(MaMH)
);

-- 6. Tài Khoản (Tham chiếu Giảng Viên)
CREATE TABLE TaiKhoan (
    MaTK VARCHAR(10) PRIMARY KEY,
    TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    LoaiTaiKhoan NVARCHAR(50),
    MaGV VARCHAR(10),
    FOREIGN KEY (MaGV) REFERENCES GiangVien(MaGV)
);

-- 7. Phân công giảng dạy (Tham chiếu GV, Khoa, Lớp TC)
CREATE TABLE PhanCongGiangDay (
    MaPC VARCHAR(10) PRIMARY KEY,
    NgayPC DATE,
    BatDau TIME,
    KetThuc TIME,
    KhuVuc NVARCHAR(100),
    MaGV VARCHAR(10),
    MaKhoa VARCHAR(10),
    MaLop VARCHAR(10),
    
    FOREIGN KEY (MaGV) REFERENCES GiangVien(MaGV),
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa),
    FOREIGN KEY (MaLop) REFERENCES LopTinChi(MaLop)
);

-- 8. Điểm (Tham chiếu Sinh Viên, Lớp TC)
CREATE TABLE Diem (
    MaSV VARCHAR(10),
    MaLop VARCHAR(10),
    DiemCC DECIMAL(4, 2),
    DiemGK DECIMAL(4, 2),
    DiemThi DECIMAL(4, 2),
    DiemTB DECIMAL(4, 2),
    
    PRIMARY KEY (MaSV, MaLop),
    FOREIGN KEY (MaSV) REFERENCES SinhVien(MaSV),
    FOREIGN KEY (MaLop) REFERENCES LopTinChi(MaLop)
);

-- ********************************************
-- 3. CHÈN DỮ LIỆU MẪU (SAMPLE DATA)
-- ********************************************

-- 3.1. Khoa
INSERT INTO Khoa (MaKhoa, TenKhoa) VALUES
('CNTT', N'Công nghệ Thông tin'),
('KT', N'Kinh tế'),
('XD', N'Xây dựng');

-- 3.2. Giảng Viên
INSERT INTO GiangVien (MaGV, TenGV, SDT, DiaChi, NgaySinh, TinhTrang, HocHam, HocVi, MaKhoa) VALUES
('GV001', N'Nguyễn Văn A', '0912345678', N'Hà Nội', '1980-05-15', N'Đang làm việc', N'PGS', N'Tiến sĩ', 'CNTT'),
('GV002', N'Trần Thị B', '0987654321', N'TP.HCM', '1985-11-20', N'Đang làm việc', N'TS', N'Thạc sĩ', 'CNTT'),
('GV003', N'Lê Văn C', '0901122334', N'Đà Nẵng', '1975-01-10', N'Đang làm việc', NULL, N'Thạc sĩ', 'KT');

-- 3.3. Sinh Viên
INSERT INTO SinhVien (MaSV, TenSV, NgaySinh, GioiTinh, QueQuan, NgayNhapHoc, MaKhoa) VALUES
('SV001', N'Phạm Minh D', '2002-03-25', N'Nam', N'Hải Phòng', '2020-09-05', 'CNTT'),
('SV002', N'Hoàng Thị E', '2003-07-10', N'Nữ', N'Hà Nội', '2021-09-05', 'CNTT'),
('SV003', N'Mai Văn F', '2002-01-01', N'Nam', N'Thanh Hóa', '2020-09-05', 'KT');

-- 3.4. Môn Học
INSERT INTO MonHoc (MaMH, TenMH, SoTC, SoTietLT, SoTietTH, HeSoDiem, MaKhoa) VALUES
('MH001', N'Cơ sở Dữ liệu', 3, 30, 15, 1.00, 'CNTT'),
('MH002', N'Lập trình Web', 3, 30, 15, 1.00, 'CNTT'),
('MH003', N'Kinh tế Vĩ mô', 2, 30, 0, 1.00, 'KT');

-- 3.5. Lớp tín chỉ
INSERT INTO LopTinChi (MaLop, TenLop, NamHoc, MaMH) VALUES
('LTC001', N'CSDL-K20', 2022, 'MH001'),
('LTC002', N'LTW-K21', 2023, 'MH002'),
('LTC003', N'KTVM-K20', 2022, 'MH003');

-- 3.6. Phân công giảng dạy
INSERT INTO PhanCongGiangDay (MaPC, NgayPC, BatDau, KetThuc, KhuVuc, MaGV, MaLop) VALUES
('PC001', '2022-08-01', '07:30:00', '09:30:00', N'Phòng B101', 'GV001', 'LTC001'),
('PC002', '2023-01-15', '13:00:00', '15:00:00', N'Phòng C205', 'GV002', 'LTC002'),
('PC003', '2022-08-01', '09:45:00', '11:45:00', N'Phòng A101', 'GV003', 'LTC003');

-- 3.7. Điểm
INSERT INTO Diem (MaSV, MaLop, DiemCC, DiemGK, DiemThi, DiemTB) VALUES
('SV001', 'LTC001', 9.00, 7.50, 8.00, 8.13),
('SV002', 'LTC001', 8.00, 6.50, 7.00, 7.00),
('SV003', 'LTC003', 10.00, 9.00, 8.50, 9.13);

-- 3.8. Tài Khoản (Chỉ Admin và Giảng viên)
INSERT INTO TaiKhoan (MaTK, TenDangNhap, MatKhau, LoaiTaiKhoan, MaGV) VALUES
('TKADM', 'admin', 'passadmin', N'Admin', NULL),
('TK001', 'nguyenvana', 'matkhau123', N'Giảng viên', 'GV001'),
('TK002', 'tranthib', 'matkhau456', N'Giảng viên', 'GV002');