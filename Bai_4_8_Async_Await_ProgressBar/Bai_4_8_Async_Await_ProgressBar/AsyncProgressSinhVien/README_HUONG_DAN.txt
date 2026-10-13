BÀI 4.8 - LẬP TRÌNH BẤT ĐỒNG BỘ (ASYNC/AWAIT) & PROGRESSBAR

CÁCH CHẠY
1. Giải nén file ZIP.
2. Mở thư mục AsyncProgressSinhVien.
3. Mở AsyncProgressSinhVien.csproj bằng Visual Studio 2022.
4. Cài workload ".NET desktop development" nếu chưa có.
5. Nhấn F5 để chạy.

CÁC YÊU CẦU ĐÃ CÀI ĐẶT
- Phương thức LoadStudentDataAsync(IProgress<int>) trả về Task<List<Student>>.
- Dùng Task.Run và Task.Delay để giả lập nạp 20 bản ghi dữ liệu.
- UI nhận tiến trình qua Progress<int>, cập nhật ProgressBar, Label phần trăm và StatusStrip.
- Trong lúc nạp, nút Nạp dữ liệu và Xóa danh sách bị khóa để tránh thao tác xung đột.
- Có try/catch/finally để xử lý lỗi và khôi phục trạng thái nút.
- Có menu Dữ liệu để nạp dữ liệu hoặc xóa danh sách.

LƯU Ý
- Dữ liệu hiện là dữ liệu giả lập, không kết nối CSDL thật.
- Quá trình nạp mất khoảng 3.6 giây; bạn vẫn có thể di chuyển cửa sổ và thao tác menu.
