BÀI 4.6 - QUẢN LÝ DỮ LIỆU VỚI BINDINGSOURCE & XỬ LÝ FILE (AVATAR / CSV)
Ứng dụng C# Windows Forms (.NET 8)

CÁCH MỞ VÀ CHẠY
1. Giải nén Bai_4_6_Quan_Ly_Sinh_Vien.zip.
2. Cài Visual Studio 2022 với workload ".NET desktop development".
3. Mở file QuanLySinhVien.csproj trong thư mục QuanLySinhVien.
4. Chọn cấu hình Debug và chạy bằng F5.

CHỨC NĂNG ĐÃ CÀI ĐẶT
- Lớp Student: StudentId, FullName, Age, Gender, AvatarPath.
- BindingList<Student> kết hợp BindingSource.
- DataGridView liên kết dữ liệu danh sách.
- TextBox và ComboBox liên kết hai chiều bằng DataBindings.Add.
- Chọn dòng trên bảng sẽ tự hiển thị ảnh đại diện nếu có.
- OpenFileDialog chọn ảnh JPG/JPEG/PNG/BMP.
- Thêm, xóa, cập nhật sinh viên; kiểm tra mã trùng và tuổi hợp lệ.
- SaveFileDialog xuất toàn bộ danh sách ra CSV UTF-8, có xử lý dấu phẩy/dấu nháy kép.
- Giao diện co giãn bằng TableLayoutPanel, Dock và Anchor.

LƯU Ý
- Chương trình có 2 bản ghi mẫu để dễ kiểm tra. Có thể xóa hoặc cập nhật.
- Ảnh được lưu bằng đường dẫn tệp trên máy, không nhúng ảnh vào CSV. Nếu chuyển máy,
  cần chọn lại ảnh hoặc đảm bảo đường dẫn ảnh vẫn tồn tại.
- File CSV có các cột StudentId, FullName, Age, Gender, AvatarPath.
