using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BTL_LTTQ.DTO;

namespace BTL_LTTQ.DAL.Subject
{
    public class SubjectDAL
    {
        // Lấy tất cả môn học
        public List<DTO.Subject> GetAllSubjects()
        {
            List<DTO.Subject> subjects = new List<DTO.Subject>();
            try
            {
                string query = @"SELECT mh.MaMH, mh.TenMH, mh.SoTC, mh.TietLT, mh.TietTH, mh.HeSoDiem, mh.MaKhoa, k.TenKhoa 
                                FROM MonHoc mh 
                                LEFT JOIN Khoa k ON mh.MaKhoa = k.MaKhoa";
                
                DataTable dt = DatabaseConnection.ExecuteQuery(query);
                
                foreach (DataRow row in dt.Rows)
                {
                    subjects.Add(new DTO.Subject
                    {
                        MaMH = row["MaMH"].ToString(),
                        TenMH = row["TenMH"].ToString(),
                        SoTC = Convert.ToInt32(row["SoTC"]),
                        TietLT = Convert.ToInt32(row["TietLT"]),
                        TietTH = Convert.ToInt32(row["TietTH"]),
                        HeSoDiem = row["HeSoDiem"]?.ToString() ?? "",
                        MaKhoa = row["MaKhoa"].ToString(),
                        TenKhoa = row["TenKhoa"]?.ToString() ?? ""
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách môn học: " + ex.Message);
            }
            
            return subjects;
        }

        // Lấy môn học theo mã
        public DTO.Subject GetSubjectByCode(string maMH)
        {
            try
            {
                string query = @"SELECT mh.MaMH, mh.TenMH, mh.SoTC, mh.TietLT, mh.TietTH, mh.HeSoDiem, mh.MaKhoa, k.TenKhoa 
                                FROM MonHoc mh 
                                LEFT JOIN Khoa k ON mh.MaKhoa = k.MaKhoa 
                                WHERE mh.MaMH = @MaMH";
                
                SqlParameter[] parameters = { new SqlParameter("@MaMH", maMH) };
                DataTable dt = DatabaseConnection.ExecuteQuery(query, parameters);
                
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];   
                    return new DTO.Subject
                    {
                        MaMH = row["MaMH"].ToString(),
                        TenMH = row["TenMH"].ToString(),
                        SoTC = Convert.ToInt32(row["SoTC"]),
                        TietLT = Convert.ToInt32(row["TietLT"]),
                        TietTH = Convert.ToInt32(row["TietTH"]),
                        HeSoDiem = row["HeSoDiem"]?.ToString() ?? "",
                        MaKhoa = row["MaKhoa"].ToString(),
                        TenKhoa = row["TenKhoa"]?.ToString() ?? ""
                    };
                }
                
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm môn học: " + ex.Message);
            }
        }

        // Tìm kiếm môn học theo tên
        public List<DTO.Subject> SearchSubjectsByName(string tenMH)
        {
            List<DTO.Subject> subjects = new List<DTO.Subject>();
            try
            {
                string query = @"SELECT mh.MaMH, mh.TenMH, mh.SoTC, mh.TietLT, mh.TietTH, mh.HeSoDiem, mh.MaKhoa, k.TenKhoa 
                                FROM MonHoc mh 
                                LEFT JOIN Khoa k ON mh.MaKhoa = k.MaKhoa 
                                WHERE mh.TenMH LIKE @TenMH";
                
                SqlParameter[] parameters = { new SqlParameter("@TenMH", "%" + tenMH + "%") };
                DataTable dt = DatabaseConnection.ExecuteQuery(query, parameters);
                
                foreach (DataRow row in dt.Rows)
                {
                    subjects.Add(new DTO.Subject
                    {
                        MaMH = row["MaMH"].ToString(),
                        TenMH = row["TenMH"].ToString(),
                        SoTC = Convert.ToInt32(row["SoTC"]),
                        TietLT = Convert.ToInt32(row["TietLT"]),
                        TietTH = Convert.ToInt32(row["TietTH"]),
                        HeSoDiem = row["HeSoDiem"]?.ToString() ?? "",
                        MaKhoa = row["MaKhoa"].ToString(),
                        TenKhoa = row["TenKhoa"]?.ToString() ?? ""
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm môn học: " + ex.Message);
            }
            
            return subjects;
        }

        // Tìm kiếm môn học theo mã
        public List<DTO.Subject> SearchSubjectsByCode(string maMH)
        {
            List<DTO.Subject> subjects = new List<DTO.Subject>();
            try
            {
                string query = @"SELECT mh.MaMH, mh.TenMH, mh.SoTC, mh.TietLT, mh.TietTH, mh.HeSoDiem, mh.MaKhoa, k.TenKhoa 
                                FROM MonHoc mh 
                                LEFT JOIN Khoa k ON mh.MaKhoa = k.MaKhoa 
                                WHERE mh.MaMH LIKE @MaMH";
                
                SqlParameter[] parameters = { new SqlParameter("@MaMH", "%" + maMH + "%") };
                DataTable dt = DatabaseConnection.ExecuteQuery(query, parameters);
                
                foreach (DataRow row in dt.Rows)
                {
                    subjects.Add(new DTO.Subject
                    {
                        MaMH = row["MaMH"].ToString(),
                        TenMH = row["TenMH"].ToString(),
                        SoTC = Convert.ToInt32(row["SoTC"]),
                        TietLT = Convert.ToInt32(row["TietLT"]),
                        TietTH = Convert.ToInt32(row["TietTH"]),
                        HeSoDiem = row["HeSoDiem"]?.ToString() ?? "",
                        MaKhoa = row["MaKhoa"].ToString(),
                        TenKhoa = row["TenKhoa"]?.ToString() ?? ""
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm môn học theo mã: " + ex.Message);
            }
            
            return subjects;
        }

        // Lấy môn học theo khoa
        public List<DTO.Subject> GetSubjectsByDepartment(string maKhoa)
        {
            List<DTO.Subject> subjects = new List<DTO.Subject>();
            try
            {
                string query = @"SELECT mh.MaMH, mh.TenMH, mh.SoTC, mh.TietLT, mh.TietTH, mh.HeSoDiem, mh.MaKhoa, k.TenKhoa 
                                FROM MonHoc mh 
                                LEFT JOIN Khoa k ON mh.MaKhoa = k.MaKhoa 
                                WHERE mh.MaKhoa = @MaKhoa";
                
                SqlParameter[] parameters = { new SqlParameter("@MaKhoa", maKhoa) };
                DataTable dt = DatabaseConnection.ExecuteQuery(query, parameters);
                
                foreach (DataRow row in dt.Rows)
                {
                    subjects.Add(new DTO.Subject
                    {
                        MaMH = row["MaMH"].ToString(),
                        TenMH = row["TenMH"].ToString(),
                        SoTC = Convert.ToInt32(row["SoTC"]),
                        TietLT = Convert.ToInt32(row["TietLT"]),
                        TietTH = Convert.ToInt32(row["TietTH"]),
                        HeSoDiem = row["HeSoDiem"]?.ToString() ?? "",
                        MaKhoa = row["MaKhoa"].ToString(),
                        TenKhoa = row["TenKhoa"]?.ToString() ?? ""
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy môn học theo khoa: " + ex.Message);
            }
            
            return subjects;
        }

        // Thêm môn học
        public bool AddSubject(DTO.Subject subject)
        {
            try
            {
                string query = "INSERT INTO MonHoc (MaMH, TenMH, SoTC, TietLT, TietTH, HeSoDiem, MaKhoa) VALUES (@MaMH, @TenMH, @SoTC, @TietLT, @TietTH, @HeSoDiem, @MaKhoa)";
                
                SqlParameter[] parameters = {
                    new SqlParameter("@MaMH", subject.MaMH),
                    new SqlParameter("@TenMH", subject.TenMH),
                    new SqlParameter("@SoTC", subject.SoTC),
                    new SqlParameter("@TietLT", subject.TietLT),
                    new SqlParameter("@TietTH", subject.TietTH),
                    new SqlParameter("@HeSoDiem", subject.HeSoDiem ?? (object)DBNull.Value),
                    new SqlParameter("@MaKhoa", subject.MaKhoa)
                };
                
                int result = DatabaseConnection.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm môn học: " + ex.Message);
            }
        }

        // Cập nhật môn học
        public bool UpdateSubject(DTO.Subject subject)
        {
            try
            {
                string query = "UPDATE MonHoc SET TenMH = @TenMH, SoTC = @SoTC, TietLT = @TietLT, TietTH = @TietTH, HeSoDiem = @HeSoDiem, MaKhoa = @MaKhoa WHERE MaMH = @MaMH";
                
                SqlParameter[] parameters = {
                    new SqlParameter("@MaMH", subject.MaMH),
                    new SqlParameter("@TenMH", subject.TenMH),
                    new SqlParameter("@SoTC", subject.SoTC),
                    new SqlParameter("@TietLT", subject.TietLT),
                    new SqlParameter("@TietTH", subject.TietTH),
                    new SqlParameter("@HeSoDiem", subject.HeSoDiem ?? (object)DBNull.Value),
                    new SqlParameter("@MaKhoa", subject.MaKhoa)
                };
                
                int result = DatabaseConnection.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật môn học: " + ex.Message);
            }
        }

        // Xóa môn học
        public bool DeleteSubject(string maMH)
        {
            try
            {
                string query = "DELETE FROM MonHoc WHERE MaMH = @MaMH";
                
                SqlParameter[] parameters = { new SqlParameter("@MaMH", maMH) };
                
                int result = DatabaseConnection.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa môn học: " + ex.Message);
            }
        }

        // Kiểm tra mã môn học có tồn tại không
        public bool CheckSubjectExists(string maMH)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM MonHoc WHERE MaMH = @MaMH";
                
                SqlParameter[] parameters = { new SqlParameter("@MaMH", maMH) };
                
                int count = Convert.ToInt32(DatabaseConnection.ExecuteScalar(query, parameters));
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kiểm tra môn học: " + ex.Message);
            }
        }

        // Lấy tất cả khoa
        public List<Department> GetAllDepartments()
        {
            List<Department> departments = new List<Department>();
            try
            {
                string query = "SELECT MaKhoa, TenKhoa FROM Khoa";
                DataTable dt = DatabaseConnection.ExecuteQuery(query);
                
                foreach (DataRow row in dt.Rows)
                {
                    departments.Add(new Department
                    {
                        MaKhoa = row["MaKhoa"].ToString(),
                        TenKhoa = row["TenKhoa"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách khoa: " + ex.Message);
            }
            
            return departments;
        }
    }
}