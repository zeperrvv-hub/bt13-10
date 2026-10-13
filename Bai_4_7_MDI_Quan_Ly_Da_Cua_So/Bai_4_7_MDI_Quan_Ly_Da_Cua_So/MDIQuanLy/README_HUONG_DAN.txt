BÀI 4.7 - THIẾT KẾ KHUNG ỨNG DỤNG MDI & QUẢN LÝ ĐA CỬA SỔ

CÔNG NGHỆ
- C# Windows Forms, .NET 8 (Windows).
- Mở MDIQuanLy.csproj bằng Visual Studio 2022.
- Cài workload ".NET desktop development", sau đó nhấn F5.

CÁC YÊU CẦU ĐÃ CÀI ĐẶT
1. FormMain đặt IsMdiContainer = true.
2. MenuStrip gồm Hệ thống (Đăng xuất, Thoát), Danh mục (Quản lý Sinh viên, Quản lý Sản phẩm),
   Cửa sổ (Cascade, Tile Horizontal, Tile Vertical).
3. ToolStrip có lối tắt mở nhanh các Form con.
4. StatusStrip hiển thị người dùng, trạng thái thao tác và đồng hồ hệ thống.
5. Hàm OpenChild<T> kiểm tra MDI Child đã tồn tại: nếu có thì Activate, nếu chưa thì tạo mới.
6. FormStudent là Form con, có bảng sinh viên, thao tác Thêm/Xóa dòng/Làm mới/Xem chi tiết.
7. ContextMenuStrip gắn vào DataGridView với Xóa dòng, Làm mới dữ liệu, Xem chi tiết.
8. FormStudent và FormProduct gửi thông điệp về FormMain bằng phương thức SetStatus.
9. FormProduct là Form minh họa, sẵn để phát triển thêm.

LƯU Ý
- Đăng xuất hiện là mô phỏng cho mục đích bài thực hành, chưa kết nối cơ sở dữ liệu/người dùng.
- FormStudent dùng dữ liệu mẫu trong bộ nhớ; đóng ứng dụng sẽ không lưu dữ liệu.
