using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTL_LTTQ.DAL.Subject;
using BTL_LTTQ.DTO;

namespace BTL_LTTQ.BLL.Subject
{
    public class SubjectBLL
    {
        private SubjectDAL subjectDAL;

        public SubjectBLL()
        {
            subjectDAL = new SubjectDAL();
        }

        // Lấy tất cả môn học
        public List<DTO.Subject> GetAllSubjects()
        {
            try
            {
                return subjectDAL.GetAllSubjects();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi lấy danh sách môn học: " + ex.Message);
            }
        }

        // Lấy môn học theo mã
        public DTO.Subject GetSubjectByCode(string maMH)
        {
            try
            {
                if (string.IsNullOrEmpty(maMH))
                {
                    throw new ArgumentException("Mã môn học không được để trống");
                }

                return subjectDAL.GetSubjectByCode(maMH.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi tìm môn học: " + ex.Message);
            }
        }

        // Tìm kiếm môn học theo tên
        public List<DTO.Subject> SearchSubjectsByName(string tenMH)
        {
            try
            {
                if (string.IsNullOrEmpty(tenMH))
                {
                    return GetAllSubjects();
                }

                return subjectDAL.SearchSubjectsByName(tenMH.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi tìm kiếm môn học: " + ex.Message);
            }
        }

        // Tìm kiếm môn học theo mã
        public List<DTO.Subject> SearchSubjectsByCode(string maMH)
        {
            try
            {
                if (string.IsNullOrEmpty(maMH))
                {
                    return GetAllSubjects();
                }

                return subjectDAL.SearchSubjectsByCode(maMH.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi tìm kiếm môn học theo mã: " + ex.Message);
            }
        }

        // Lấy môn học theo khoa
        public List<DTO.Subject> GetSubjectsByDepartment(string maKhoa)
        {
            try
            {
                if (string.IsNullOrEmpty(maKhoa))
                {
                    return GetAllSubjects();
                }

                return subjectDAL.GetSubjectsByDepartment(maKhoa.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi lấy môn học theo khoa: " + ex.Message);
            }
        }

        // Thêm môn học
        public bool AddSubject(DTO.Subject subject)
        {
            try
            {
                // Validate dữ liệu
                ValidateSubject(subject);

                // Kiểm tra trùng mã môn học
                if (subjectDAL.CheckSubjectExists(subject.MaMH))
                {
                    throw new Exception("Mã môn học đã tồn tại");
                }

                return subjectDAL.AddSubject(subject);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi thêm môn học: " + ex.Message);
            }
        }

        // Cập nhật môn học
        public bool UpdateSubject(DTO.Subject subject)
        {
            try
            {
                // Validate dữ liệu
                ValidateSubject(subject);

                // Kiểm tra môn học có tồn tại không
                if (!subjectDAL.CheckSubjectExists(subject.MaMH))
                {
                    throw new Exception("Môn học không tồn tại");
                }

                return subjectDAL.UpdateSubject(subject);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi cập nhật môn học: " + ex.Message);
            }
        }

        // Xóa môn học
        public bool DeleteSubject(string maMH)
        {
            try
            {
                if (string.IsNullOrEmpty(maMH))
                {
                    throw new ArgumentException("Mã môn học không được để trống");
                }

                // Kiểm tra môn học có tồn tại không
                if (!subjectDAL.CheckSubjectExists(maMH))
                {
                    throw new Exception("Môn học không tồn tại");
                }

                return subjectDAL.DeleteSubject(maMH.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi xóa môn học: " + ex.Message);
            }
        }

        // Kiểm tra mã môn học có tồn tại không
        public bool CheckSubjectExists(string maMH)
        {
            try
            {
                if (string.IsNullOrEmpty(maMH))
                {
                    return false;
                }

                return subjectDAL.CheckSubjectExists(maMH.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi kiểm tra môn học: " + ex.Message);
            }
        }

        // Lấy tất cả khoa
        public List<Department> GetAllDepartments()
        {
            try
            {
                return subjectDAL.GetAllDepartments();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi lấy danh sách khoa: " + ex.Message);
            }
        }

        // Validate dữ liệu môn học
        private void ValidateSubject(DTO.Subject subject)
        {
            if (subject == null)
            {
                throw new ArgumentException("Thông tin môn học không được để trống");
            }

            if (string.IsNullOrEmpty(subject.MaMH))
            {
                throw new ArgumentException("Mã môn học không được để trống");
            }

            if (string.IsNullOrEmpty(subject.TenMH))
            {
                throw new ArgumentException("Tên môn học không được để trống");
            }

            if (subject.SoTC <= 0)
            {
                throw new ArgumentException("Số tín chỉ phải lớn hơn 0");
            }

            if (subject.TietLT < 0)
            {
                throw new ArgumentException("Số tiết lý thuyết không được âm");
            }

            if (subject.TietTH < 0)
            {
                throw new ArgumentException("Số tiết thực hành không được âm");
            }

            if (string.IsNullOrEmpty(subject.MaKhoa))
            {
                throw new ArgumentException("Mã khoa không được để trống");
            }

            // Kiểm tra độ dài
            if (subject.MaMH.Length > 50)
            {
                throw new ArgumentException("Mã môn học không được vượt quá 50 ký tự");
            }

            if (subject.TenMH.Length > 255)
            {
                throw new ArgumentException("Tên môn học không được vượt quá 255 ký tự");
            }

            if (subject.MaKhoa.Length > 50)
            {
                throw new ArgumentException("Mã khoa không được vượt quá 50 ký tự");
            }
        }

        // Tìm kiếm môn học với nhiều điều kiện
        public List<DTO.Subject> SearchSubjects(string searchText = "", string maKhoa = "", bool searchByCode = false)
        {
            try
            {
                List<DTO.Subject> result = new List<DTO.Subject>();

                // Nếu có text để tìm kiếm
                if (!string.IsNullOrEmpty(searchText))
                {
                    if (searchByCode)
                    {
                        // Tìm theo mã môn học
                        result = SearchSubjectsByCode(searchText);
                    }
                    else
                    {
                        // Tìm theo tên môn học
                        result = SearchSubjectsByName(searchText);
                    }
                }
                else
                {
                    // Lấy tất cả môn học
                    result = GetAllSubjects();
                }

                // Lọc theo khoa nếu có
                if (!string.IsNullOrEmpty(maKhoa))
                {
                    result = result.Where(s => s.MaKhoa.Equals(maKhoa, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi tìm kiếm môn học: " + ex.Message);
            }
        }

        // Tìm kiếm môn học (backward compatibility)
        public List<DTO.Subject> SearchSubjects(string tenMH = "", string maKhoa = "")
        {
            return SearchSubjects(tenMH, maKhoa, false);
        }
    }
}
