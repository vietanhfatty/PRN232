# 📝 BÁO CÁO ĐÁNH GIÁ VÀ QUYẾT ĐỊNH KỸ THUẬT (PROJECT REVIEW)
## HỆ THỐNG HOSPITAL MANAGEMENT SYSTEM - ASSM-1

---

## 1. PHÂN TÍCH VÀ ĐÁNH GIÁ CÁC QUYẾT ĐỊNH KIẾN TRÚC

### Kiến trúc 3 lớp (3-Tier Architecture)
* **Lý do lựa chọn:** Giúp phân tách rõ ràng các trách nhiệm (Separation of Concerns). Giao diện không can thiệp trực tiếp vào dữ liệu, logic nghiệp vụ được tập trung tại một nơi.
* **Đánh giá kết quả:** Việc bảo trì và mở rộng code trở nên dễ dàng. Khi có thay đổi về quy trình nghiệp vụ của bệnh viện, chúng ta chỉ cần chỉnh sửa lớp Business Logic mà không ảnh hưởng tới cấu trúc tầng dữ liệu hay mã nguồn ở Client.

### Mẫu thiết kế Repository Pattern
* **Lý do lựa chọn:** Trừu tượng hóa tầng truy cập dữ liệu thông qua các Interface (`IPatientRepository`, `IAppointmentRepository`). Lớp Controller của Web API sẽ không gọi trực tiếp EF Core `DbContext`.
* **Đánh giá kết quả:** Giúp code sạch, dễ viết Unit Test cho tầng điều hướng (Controller) nhờ kỹ thuật Mocking. Tránh tình trạng lặp lại các đoạn mã truy vấn LINQ phức tạp ở nhiều nơi.

### Mẫu thiết kế Singleton Pattern cho Data Access Layer
* **Lý do lựa chọn:** Đảm bảo duy nhất một thực thể quản lý kết nối cơ sở dữ liệu được khởi tạo và sử dụng xuyên suốt trong một phiên làm việc, tối ưu hóa tài nguyên hệ thống.
* **Đánh giá kết quả:** Quản lý tốt vòng đời của đối tượng truy cập, ngăn ngừa hiện tượng rò rỉ kết nối (Connection Leak) khi hệ thống có nhiều yêu cầu đồng thời.

---

## 2. REVIEW ĐÁNH GIÁ CÁC HÀNH ĐỘNG VÀ CHỨC NĂNG ĐÃ THỰC HIỆN

### A. Tích hợp ASP.NET Core Web API & OData
* **Hành động thực hiện:** Cấu hình thư viện OData vào đường dẫn API Endpoints.
* **Kết quả đạt được:** Giảm tải số lượng API cần viết một cách đáng kể. Client có thể tự gửi các câu lệnh lọc nâng cao, phân trang, chọn cột (`$filter`, `$skip`, `$top`, `$select`) trực tiếp từ URL mà Backend không cần viết riêng từng API cho từng bộ lọc tìm kiếm.

### B. Giải pháp Phân quyền (Authentication & Authorization)
* **Hành động thực hiện:** Triển khai cơ chế Cookie-Based Authentication tại Client và Role-based Authorization kiểm tra quyền tại từng API Controller bằng các thẻ Attribute `[Authorize(Roles = "Admin")]`.
* **Kết quả đạt được:** Đảm bảo an toàn dữ liệu tuyệt đối. Kể cả khi người dùng cố tình thay đổi giao diện Client bằng mã nguồn F12, API Backend vẫn từ chối thực thi nếu tài khoản đăng nhập là `Staff` nhưng cố thực hiện lệnh `DELETE`.

### C. Cơ chế trải nghiệm người dùng (Popup Dialog & Xác nhận xóa)
* **Hành động thực hiện:** Sử dụng Bootstrap Modals kết hợp AJAX để tải biểu mẫu Thêm/Sửa và kích hoạt popup xác nhận trước khi thực hiện hành động xóa.
* **Kết quả đạt được:** Tăng tốc độ phản hồi của ứng dụng Client do không phải tải lại toàn bộ trang (Page Reload). Hộp thoại xác nhận giúp ngăn chặn triệt để hành động vô tình bấm nhầm nút Xóa của nhân viên, bảo vệ tính toàn vẹn của dữ liệu bệnh án.

### D. Xác thực dữ liệu (Data Validation)
* **Hành động thực hiện:** Định nghĩa các quy tắc ràng buộc trực tiếp trên các thuộc tính của Model (Data Annotations) kết hợp kiểm tra trạng thái dữ liệu `ModelState.IsValid`.
* **Kết quả đạt được:** Dữ liệu rác hoàn toàn bị chặn trước khi chạm tới cơ sở dữ liệu. Ví dụ: Các trường bắt buộc như `Patient.FullName`, `Patient.PhoneNumber` luôn được đảm bảo có dữ liệu chính xác và đúng định dạng.

---

## 3. TỰ ĐÁNH GIÁ THEO TIÊU CHÍ CHẤM ĐIỂM (RUBRIC ASSESSMENT)

| STT | Nội dung yêu cầu | Trạng thái hoàn thành | Tự đánh giá đạt |
| :-: | :--- | :-: | :-: |
| 1 | Giao diện cơ bản User + Admin | Đã hoàn thành | **100%** |
| 2 | Tạo Web API (3 lớp, Repository, Singleton, OData, EF Core) | Đã hoàn thành | **100%** |
| 3 | Tương tác ứng dụng với Web API & Phân quyền Cookie/Session | Đã hoàn thành | **100%** |
| 4 | Tích hợp Tìm kiếm nâng cao (Tìm kiếm tương đối, đa điều kiện) | Đã hoàn thành | **100%** |

---

## 4. KẾT LUẬN & HƯỚNG CẢI TIẾN TRONG TƯƠNG LAI
* **Ưu điểm:** Hệ thống chạy ổn định, tuân thủ nghiêm ngặt mô hình kiến trúc được yêu cầu trong đề bài ASSM-1. Các chức năng popup và tìm kiếm nâng cao hoạt động mượt mà, phản hồi API nhanh nhờ OData.
* **Hướng cải tiến:** Trong phiên bản tiếp theo, có thể chuyển đổi cơ chế mã hóa từ Cookie sang **JWT Token (JSON Web Token)** để tăng tính bảo mật, đồng thời mở rộng thêm việc tích hợp SignalR giúp cập nhật lịch hẹn của bệnh nhân theo thời gian thực (Real-time).