# 🏥 HỆ THỐNG QUẢN LÝ BỆNH VIỆN (HOSPITAL MANAGEMENT SYSTEM)
## TÀI LIỆU MÔ TẢ YÊU CẦU VÀ KIẾN TRÚC DỰ ÁN (ASSM-1)

---

## 1. TỔNG QUAN DỰ ÁN
Dự án tập trung xây dựng một hệ thống ứng dụng Web hoàn chỉnh phục vụ công tác quản lý bệnh viện (**HospitalManagementDB**). Hệ thống tách biệt hoàn toàn giữa hai thành phần: **ASP.NET Core Web API** (Backend cung cấp dữ liệu qua giao thức OData) và **Web Client** (Frontend tương tác qua API). Hệ thống phân quyền chặt chẽ dựa trên hai vai trò chính: **Admin** (Quản trị viên) và **Staff** (Nhân viên/Bác sĩ).

---

## 2. KIẾN TRÚC GIẢI PHÁP (PROJECT ARCHITECTURE)

Hệ thống được phát triển dựa trên **Kiến trúc 3 lớp (3-Tier Architecture)** kết hợp với **Repository Pattern** và **Singleton Pattern**.

**[ Web Client (MVC/Razor Pages) ]**
**│ (HTTP Requests / OData Query)**
**▼**
**[ ASP.NET Core Web API ]**
**│**
**▼**
**┌────────────────────────────────────────────────────────┐**
│ **BACKEND ARCHITECTURE (3-TIER)                          │**
│                                                        │
│ **1. Presentation Layer (Controllers / OData Endpoints)   │**
│        **│**                                               │
│        **▼**                                               │
│ **2. Business Logic Layer (Services / Repository)        │**
│        **│**                                               │
│        **▼**                                               │
│ **3. Data Access Layer (EF Core / DBContext / Singleton) │**
└────────────────────────────────────────────────────────┘
**│**
▼
**[ SQL Server (HospitalManagementDB) ]**


### Cấu trúc Solution trong Visual Studio:
* **HospitalManagement.DataAccess:** Lớp truy cập dữ liệu (Data Access Layer). Chứa `DbContext`, các Entity được sinh ra từ database thông qua EF Core Database First. Áp dụng **Singleton Pattern** cho lớp quản lý DB.
* **HospitalManagement.BusinessObject:** Chứa các mẫu thiết kế dữ liệu, DTOs (Data Transfer Objects), Fluent Validation hoặc Data Annotations phục vụ việc xác thực dữ liệu đầu vào.
* **HospitalManagement.Repository:** Lớp triển khai **Repository Pattern**. Định nghĩa các Interface và Class tương ứng nhằm trừu tượng hóa việc xử lý dữ liệu từ Data Access Layer, giúp Business dễ dàng tái sử dụng.
* **HospitalManagement.API:** Lớp xử lý logic ứng dụng và endpoints (Presentation Layer - Backend). Tích hợp cấu hình OData để hỗ trợ truy vấn nâng cao và lọc dữ liệu động từ Client.
* **HospitalManagement.Client:** Giao diện người dùng (Presentation Layer - Frontend) sử dụng ASP.NET Core MVC / Razor Pages. Giao tiếp với API bằng `HttpClient`.

---

## 3. PHÂN QUYỀN VÀ MÔ HÌNH VAI TRÒ (AUTHENTICATION & AUTHORIZATION)

Hệ thống áp dụng cơ chế **Cookie/Session Authentication** kết hợp với **Role-based Authorization**:
* **Admin (Quản trị viên):** Sở hữu toàn quyền trên hệ thống (`Full CRUD`). Có quyền Đọc, Thêm, Sửa, Xóa thông tin trên tất cả các thực thể (Tài khoản, Nhân viên, Bệnh nhân, Lịch hẹn, Bệnh án, Thuốc).
* **Staff (Nhân viên / Bác sĩ):** Chỉ có quyền **Đọc (Read)**, **Thêm mới (Create)** và **Cập nhật (Update)** lịch hẹn, hồ sơ bệnh án hoặc đơn thuốc. Quyền **Xóa (Delete) bị nghiêm cấm** đối với vai trò này.

---

## 4. CHI TIẾT ENDPOINTS WEB API (CÓ HỖ TRỢ ODATA)

Tất cả các API quản lý thực thể đều kế thừa cấu hình OData cho phép Client sử dụng các mệnh đề `$select`, `$filter`, `$expand`, `$orderby`, `$top`, `$skip`.

### 🔐 Authentication API
* `POST /api/auth/login`: Xác thực thông tin đăng nhập từ Client. Trả về thông tin User và Quyền (Role) để Client lưu vào Cookie/Session.
* `POST /api/auth/logout`: Xóa trạng thái đăng nhập.

### 👤 Accounts & Staffs API (Chỉ Admin truy cập)
* `GET /odata/Accounts`: Lấy danh sách tài khoản (Hỗ trợ OData).
* `POST /api/accounts`: Tạo tài khoản mới.
* `PUT /api/accounts/{id}`: Cập nhật thông tin tài khoản.
* `DELETE /api/accounts/{id}`: Xóa tài khoản.

### 🩺 Patients API (Admin & Staff)
* `GET /odata/Patients`: Lấy danh sách bệnh nhân (Tìm kiếm tương đối qua `$filter=contains(FullName, 'keyword')`).
* `POST /api/patients`: Thêm bệnh nhân mới (Yêu cầu kiểm tra tất cả các trường qua Data Annotation).
* `PUT /api/patients/{id}`: Sửa thông tin bệnh nhân.
* `DELETE /api/patients/{id}`: Xóa bệnh nhân (*Chỉ dành cho Admin*).

### 📅 Appointments & Medical Records API (Admin & Staff)
* `GET /odata/Appointments`: Lấy danh sách lịch hẹn (Hỗ trợ `$expand=Patient,Doctor`).
* `POST /api/appointments`: Đặt lịch hẹn mới.
* `PUT /api/appointments/{id}`: Thay đổi trạng thái lịch hẹn (`Pending` -> `Confirmed` -> `Completed`).
* `POST /api/medicalrecords`: Lập hồ sơ bệnh án mới khi ca khám hoàn thành.

---

## 5. MÔ TẢ CHỨC NĂNG VÀ CƠ CHẾ UI TẠI WEB CLIENT

Ứng dụng Client được thiết kế khoa học nhằm tối ưu hóa trải nghiệm người dùng và đáp ứng đúng tiêu chuẩn kỹ thuật đề ra:

### A. Xác thực dữ liệu (Data Validation)
* Tất cả các biểu mẫu (Forms) đầu vào tại Client đều được cấu hình các thuộc tính xác thực nghiêm ngặt (ví dụ: Họ tên không trống, Số điện thoại từ 10-15 ký tự, Email đúng định dạng). Việc kiểm tra tính hợp lệ được thực hiện song song tại Client (phản hồi ngay lập tức) và Backend (đảm bảo an toàn dữ liệu).

### B. Cơ chế hiển thị và Tương tác (Hộp thoại Bật lên & Xác nhận)
* **Hành động Đọc và Tìm kiếm:** Hiển thị danh sách dưới dạng bảng thông tin trực quan. Tích hợp thanh công cụ lọc nâng cao.
* **Hành động Tạo mới (Create) & Cập nhật (Update):** Khi người dùng nhấn nút "Thêm mới" hoặc "Chỉnh sửa", hệ thống **không chuyển trang** mà hiển thị một **Hộp thoại Bật lên (Popup Dialog / Modal Bootstrap)** đè lên màn hình hiện tại để người dùng nhập liệu, giúp tối ưu thời gian thao tác.
* **Hành động Xóa (Delete):** Khi bấm nút xóa, hệ thống kích hoạt một **Hộp thoại Xác nhận (Confirmation Modal)** với câu hỏi *"Bạn có chắc chắn muốn xóa mục này không?"*. Hành động xóa tại Backend chỉ được kích hoạt khi người dùng nhấn "Xác nhận".

### C. Tìm kiếm nâng cao và Tìm kiếm tương đối (Advanced Search)
* Hệ thống triển khai công cụ tìm kiếm kết hợp nhiều điều kiện thông qua việc biên dịch bộ lọc từ Client thành chuỗi truy vấn OData gửi lên API.
* **Tìm kiếm tương đối:** Tìm theo tên nhân viên hoặc tên bệnh nhân mà không cần khớp chính xác từng từ (`contains`).
* **Lọc nâng cao theo nhiều điều kiện đồng thời:**
    * Khoảng ngày (Tìm các lịch hẹn từ ngày `A` đến ngày `B`).
    * Trạng thái (Lọc các lịch hẹn đang ở trạng thái `Pending` hoặc `Completed`).
    * Tìm kiếm chính xác theo Mã định danh (ID) hoặc Mã bảo hiểm y tế.
   ###  Sử dụng Visual Studio.NET để tạo ứng dụng Web và dự án ASP.NET Core Web API (có hỗ trợ OData). 
* Thực hiện các hành động CRUD bằng Entity Framework Core. 

* **Áp dụng kiến ​​trúc 3 lớp để phát triển ứng dụng.**

* **Áp dụng mẫu Repository và mẫu Singleton trong một dự án.**

**Thêm các hành động CRUD và tìm kiếm vào ứng dụng Client bằng ASP.NET Core Web API.**

 **Áp dụng để xác thực kiểu dữ liệu cho tất cả các trường.** 

**Chạy dự án và kiểm tra các hành động của ứng dụng Client Web và ASP.NET Core Web API**

