-- HỆ THỐNG QUẢN LÝ BỆNH VIỆN (HOSPITAL MANAGEMENT SYSTEM)
-- SCRIPT TẠO DATABASE SQL SERVER
-- Tự động cấu hình Khóa chính, Khóa ngoại và các ràng buộc dữ liệu

CREATE DATABASE HospitalManagementDB;
GO

USE HospitalManagementDB;
GO

-- ============================================================================
-- NHÓM 1: QUẢN LÝ NHÂN SỰ & PHÂN QUYỀN (Staff Management)
-- ============================================================================

-- 1. Bảng Vai trò
CREATE TABLE roles (
    role_id INT IDENTITY(1,1) CONSTRAINT PK_roles PRIMARY KEY,
    role_name NVARCHAR(50) NOT NULL CONSTRAINT UC_roles_name UNIQUE
);

-- 2. Bảng Nhân viên / Bác sĩ
CREATE TABLE staffs (
    staff_id INT IDENTITY(1,1) CONSTRAINT PK_staffs PRIMARY KEY,
    role_id INT NOT NULL,
    first_name NVARCHAR(50) NOT NULL,
    last_name NVARCHAR(50) NOT NULL,
    specialization NVARCHAR(100) NULL,
    phone VARCHAR(15) NOT NULL CONSTRAINT UC_staffs_phone UNIQUE,
    email VARCHAR(100) NULL CONSTRAINT UC_staffs_email UNIQUE,
    [status] NVARCHAR(20) DEFAULT 'Active' CONSTRAINT CK_staffs_status CHECK ([status] IN ('Active', 'Inactive')),
    CONSTRAINT FK_staffs_roles FOREIGN KEY (role_id) REFERENCES roles(role_id)
);

-- ============================================================================
-- NHÓM 2: QUẢN LÝ BỆNH NHÂN & BỆNH ÁN (Patient & Medical History)
-- ============================================================================

-- 3. Bảng Bệnh nhân
CREATE TABLE patients (
    patient_id INT IDENTITY(1,1) CONSTRAINT PK_patients PRIMARY KEY,
    first_name NVARCHAR(50) NOT NULL,
    last_name NVARCHAR(50) NOT NULL,
    gender NVARCHAR(10) NOT NULL CONSTRAINT CK_patients_gender CHECK (gender IN (N'Male', N'Female', N'Other')),
    dob DATE NOT NULL,
    phone VARCHAR(15) NOT NULL CONSTRAINT UC_patients_phone UNIQUE,
    [address] NVARCHAR(255) NULL,
    insurance_no VARCHAR(30) NULL CONSTRAINT UC_patients_insurance UNIQUE
);

-- 4. Bảng Lịch sử bệnh án (Lưu thông tin mỗi lần khám)
CREATE TABLE medical_records (
    record_id INT IDENTITY(1,1) CONSTRAINT PK_medical_records PRIMARY KEY,
    patient_id INT NOT NULL,
    doctor_id INT NOT NULL,
    visit_date DATETIME NOT NULL DEFAULT GETDATE(),
    symptoms NVARCHAR(MAX) NOT NULL,
    diagnosis NVARCHAR(MAX) NOT NULL,
    treatment_plan NVARCHAR(MAX) NULL,
    CONSTRAINT FK_records_patients FOREIGN KEY (patient_id) REFERENCES patients(patient_id),
    CONSTRAINT FK_records_staffs FOREIGN KEY (doctor_id) REFERENCES staffs(staff_id)
);

-- ============================================================================
-- NHÓM 3: LỊCH HẸN & TÁI KHÁM (Doctor Appointment Scheduling)
-- ============================================================================

-- 5. Bảng Lịch hẹn khám / Tái khám
CREATE TABLE appointments (
    appointment_id INT IDENTITY(1,1) CONSTRAINT PK_appointments PRIMARY KEY,
    patient_id INT NOT NULL,
    doctor_id INT NOT NULL,
    appointment_date DATETIME NOT NULL,
    [type] NVARCHAR(20) DEFAULT 'First Visit' CONSTRAINT CK_appointments_type CHECK ([type] IN ('First Visit', 'Follow-up')),
    [status] NVARCHAR(20) DEFAULT 'Scheduled' CONSTRAINT CK_appointments_status CHECK ([status] IN ('Scheduled', 'Completed', 'Cancelled')),
    reason NVARCHAR(MAX) NULL,
    CONSTRAINT FK_appointments_patients FOREIGN KEY (patient_id) REFERENCES patients(patient_id),
    CONSTRAINT FK_appointments_staffs FOREIGN KEY (doctor_id) REFERENCES staffs(staff_id)
);

-- ============================================================================
-- NHÓM 4: ĐƠN THUỐC & KHO DƯỢC (Prescription Management)
-- ============================================================================

-- 6. Bảng Danh mục thuốc
CREATE TABLE medicines (
    medicine_id INT IDENTITY(1,1) CONSTRAINT PK_medicines PRIMARY KEY,
    [name] NVARCHAR(100) NOT NULL,
    unit NVARCHAR(20) NOT NULL,
    price DECIMAL(10,2) NOT NULL CONSTRAINT CK_medicines_price CHECK (price >= 0),
    stock_quantity INT NOT NULL DEFAULT 0 CONSTRAINT CK_medicines_stock CHECK (stock_quantity >= 0)
);

-- 7. Bảng Đơn thuốc
CREATE TABLE prescriptions (
    prescription_id INT IDENTITY(1,1) CONSTRAINT PK_prescriptions PRIMARY KEY,
    record_id INT NOT NULL,
    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_prescriptions_records FOREIGN KEY (record_id) REFERENCES medical_records(record_id)
);

-- 8. Bảng Chi tiết đơn thuốc (Quan hệ n-n giữa Đơn thuốc và Thuốc)
CREATE TABLE prescription_details (
    prescription_id INT NOT NULL,
    medicine_id INT NOT NULL,
    quantity INT NOT NULL CONSTRAINT CK_details_quantity CHECK (quantity > 0),
    dosage NVARCHAR(255) NOT NULL,
    CONSTRAINT PK_prescription_details PRIMARY KEY (prescription_id, medicine_id),
    CONSTRAINT FK_details_prescriptions FOREIGN KEY (prescription_id) REFERENCES prescriptions(prescription_id),
    CONSTRAINT FK_details_medicines FOREIGN KEY (medicine_id) REFERENCES medicines(medicine_id)
);

-- ============================================================================
-- NHÓM 5: XÉT NGHIỆM (Lab Results Tracking)
-- ============================================================================

-- 9. Bảng Danh mục dịch vụ xét nghiệm
CREATE TABLE lab_tests (
    test_id INT IDENTITY(1,1) CONSTRAINT PK_lab_tests PRIMARY KEY,
    test_name NVARCHAR(100) NOT NULL,
    cost DECIMAL(10,2) NOT NULL CONSTRAINT CK_tests_cost CHECK (cost >= 0)
);

-- 10. Bảng Kết quả xét nghiệm của bệnh nhân
CREATE TABLE lab_results (
    result_id INT IDENTITY(1,1) CONSTRAINT PK_lab_results PRIMARY KEY,
    record_id INT NOT NULL,
    test_id INT NOT NULL,
    test_date DATETIME NOT NULL DEFAULT GETDATE(),
    result_summary NVARCHAR(MAX) NOT NULL,
    attachment_url VARCHAR(255) NULL,
    CONSTRAINT FK_results_records FOREIGN KEY (record_id) REFERENCES medical_records(record_id),
    CONSTRAINT FK_results_tests FOREIGN KEY (test_id) REFERENCES lab_tests(test_id)
);

-- ============================================================================
-- NHÓM 6: QUẢN LÝ GIƯỜNG BỆNH REAL-TIME (Bed/Room Availability)
-- ============================================================================

-- 11. Bảng Phòng bệnh
CREATE TABLE rooms (
    room_id INT IDENTITY(1,1) CONSTRAINT PK_rooms PRIMARY KEY,
    room_number VARCHAR(10) NOT NULL CONSTRAINT UC_rooms_number UNIQUE,
    room_type NVARCHAR(20) NOT NULL CONSTRAINT CK_rooms_type CHECK (room_type IN ('VIP', 'Normal', 'ICU')),
    daily_rate DECIMAL(10,2) NOT NULL CONSTRAINT CK_rooms_rate CHECK (daily_rate >= 0)
);

-- 12. Bảng Giường bệnh (Trạng thái Real-time phục vụ cập nhật liên tục)
CREATE TABLE beds (
    bed_id INT IDENTITY(1,1) CONSTRAINT PK_beds PRIMARY KEY,
    room_id INT NOT NULL,
    bed_number VARCHAR(10) NOT NULL,
    is_available BIT NOT NULL DEFAULT 1, -- 1: Trống (True), 0: Có bệnh nhân (False)
    CONSTRAINT FK_beds_rooms FOREIGN KEY (room_id) REFERENCES rooms(room_id)
);

-- 13. Bảng Phân phối giường bệnh (Lịch sử & Hiện trạng nội trú)
CREATE TABLE room_allocations (
    allocation_id INT IDENTITY(1,1) CONSTRAINT PK_room_allocations PRIMARY KEY,
    patient_id INT NOT NULL,
    bed_id INT NOT NULL,
    admission_date DATETIME NOT NULL DEFAULT GETDATE(),
    discharge_date DATETIME NULL,
    CONSTRAINT FK_allocations_patients FOREIGN KEY (patient_id) REFERENCES patients(patient_id),
    CONSTRAINT FK_allocations_beds FOREIGN KEY (bed_id) REFERENCES beds(bed_id),
    CONSTRAINT CK_allocations_date CHECK (discharge_date IS NULL OR discharge_date >= admission_date)
);

-- ============================================================================
-- NHÓM 7: HÓA ĐƠN & BẢO HIỂM (Billing & Insurance)
-- ============================================================================

-- 14. Bảng Hóa đơn thanh toán
CREATE TABLE bills (
    bill_id INT IDENTITY(1,1) CONSTRAINT PK_bills PRIMARY KEY,
    patient_id INT NOT NULL,
    total_amount DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    insurance_discount DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    tax_amount DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    net_amount DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    payment_status NVARCHAR(20) DEFAULT 'Unpaid' CONSTRAINT CK_bills_status CHECK (payment_status IN ('Unpaid', 'Paid', 'Partially Paid')),
    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_bills_patients FOREIGN KEY (patient_id) REFERENCES patients(patient_id)
);
GO

-- ============================================================================
-- THÊM DỮ LIỆU MẪU (MOCK DATA) ĐỂ TEST HỆ THỐNG
-- ============================================================================

-- Chèn dữ liệu vai trò
INSERT INTO roles (role_name) VALUES ('Admin'), ('Doctor'), ('Nurse'), ('Receptionist'), ('Accountant');

-- Chèn dữ liệu nhân viên (Bác sĩ, Tiếp tân)
INSERT INTO staffs (role_id, first_name, last_name, specialization, phone, email, [status]) VALUES 
(2, N'Nguyễn Văn', N'An', N'Tim Mạch', '0912345678', 'an.nguyen@hospital.com', 'Active'),
(2, N'Trần Thị', N'Bình', N'Nhi Khoa', '0987654321', 'binh.tran@hospital.com', 'Active'),
(4, N'Lê Hoàng', N'Nam', NULL, '0901112223', 'nam.le@hospital.com', 'Active');

-- Chèn dữ liệu bệnh nhân
INSERT INTO patients (first_name, last_name, gender, dob, phone, [address], insurance_no) VALUES 
(N'Phạm Minh', N'Đức', N'Male', '1995-05-20', '0933445566', N'123 Nguyễn Trãi, Hà Nội', 'GD47979102001'),
(N'Hoàng Ngọc', N'Linh', N'Female', '2000-11-12', '0944556677', N'456 Lê Lợi, TP.HCM', NULL);

-- Chèn danh mục phòng & giường bệnh
INSERT INTO rooms (room_number, room_type, daily_rate) VALUES 
('P101', 'Normal', 200000),
('P301', 'VIP', 1000000),
('ICU1', 'ICU', 1500000);

INSERT INTO beds (room_id, bed_number, is_available) VALUES 
(1, 'G1', 1), (1, 'G2', 1),
(2, 'VIP-G1', 1),
(3, 'ICU-G1', 0); -- Giường này đang bận (test realtime)

-- Chèn danh mục thuốc & dịch vụ xét nghiệm
INSERT INTO medicines (name, unit, price, stock_quantity) VALUES 
('Paracetamol 500mg', N'Viên', 1500, 5000),
('Amoxicillin 500mg', N'Viên', 3000, 2000),
('Augmentin 1g', N'Hộp', 250000, 150);

INSERT INTO lab_tests (test_name, cost) VALUES 
(N'Xét nghiệm máu tổng quát', 350000),
(N'Chụp X-Quang phổi', 150000),
(N'Siêu âm ổ bụng', 200000);
GO
