CREATE DATABASE HospitalManagementDB;
GO
USE HospitalManagementDB;
GO

-- 1. Bảng Vai trò (Roles)
CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL UNIQUE, -- Admin, Doctor, Staff, Patient
    Description NVARCHAR(250)
);

-- 2. Bảng Tài khoản (Accounts)
CREATE TABLE Accounts (
    AccountID INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(50) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    IsActive BIT DEFAULT 1,
    RoleID INT FOREIGN KEY REFERENCES Roles(RoleID),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 3. Bảng Nhân viên / Bác sĩ (Staffs)
CREATE TABLE Staffs (
    StaffID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Gender NVARCHAR(10),
    DateOfBirth DATE,
    PhoneNumber VARCHAR(15),
    Address NVARCHAR(250),
    Specialization NVARCHAR(100), -- Chuyên khoa (nếu là Bác sĩ)
    Position NVARCHAR(50), -- Bác sĩ, Lễ tân, Kế toán...
    AccountID INT FOREIGN KEY REFERENCES Accounts(AccountID) ON DELETE SET NULL
);

-- 4. Bảng Bệnh nhân (Patients)
CREATE TABLE Patients (
    PatientID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    DateOfBirth DATE NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    Address NVARCHAR(250),
    HealthInsuranceCode VARCHAR(20), -- Mã bảo hiểm y tế
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 5. Bảng Lịch hẹn / Ca khám (Appointments)
CREATE TABLE Appointments (
    AppointmentID INT PRIMARY KEY IDENTITY(1,1),
    PatientID INT FOREIGN KEY REFERENCES Patients(PatientID) ON DELETE CASCADE,
    DoctorID INT FOREIGN KEY REFERENCES Staffs(StaffID), -- Bác sĩ phụ trách
    AppointmentDate DATETIME NOT NULL,
    Reason NVARCHAR(500), -- Lý do khám
    Status NVARCHAR(50) DEFAULT 'Pending', -- Pending, Confirmed, Completed, Cancelled
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 6. Bảng Hồ sơ bệnh án (MedicalRecords)
CREATE TABLE MedicalRecords (
    RecordID INT PRIMARY KEY IDENTITY(1,1),
    AppointmentID INT UNIQUE FOREIGN KEY REFERENCES Appointments(AppointmentID) ON DELETE CASCADE,
    Diagnosis NVARCHAR(500) NOT NULL, -- Chẩn đoán bệnh
    TreatmentPlan NVARCHAR(1000), -- Hướng điều trị
    DoctorNotes NVARCHAR(500),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- 7. Bảng Thuốc (Medicines)
CREATE TABLE Medicines (
    MedicineID INT PRIMARY KEY IDENTITY(1,1),
    MedicineName NVARCHAR(100) NOT NULL,
    Unit NVARCHAR(20) NOT NULL, -- Viên, Chai, Gói...
    Price DECIMAL(18,2) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    Description NVARCHAR(250)
);

-- 8. Chi tiết Đơn thuốc (Prescriptions)
CREATE TABLE Prescriptions (
    PrescriptionID INT PRIMARY KEY IDENTITY(1,1),
    RecordID INT FOREIGN KEY REFERENCES MedicalRecords(RecordID) ON DELETE CASCADE,
    MedicineID INT FOREIGN KEY REFERENCES Medicines(MedicineID),
    Quantity INT NOT NULL,
    Dosage NVARCHAR(250) NOT NULL -- Liều lượng dùng (Ví dụ: Ngày 2 lần, mỗi lần 1 viên sau ăn)
);
GO

-- CHÈN DỮ LIỆU MẪU MẶC ĐỊNH (SEED DATA)
INSERT INTO Roles (RoleName, Description) VALUES 
('Admin', 'Quản trị hệ thống'),
('Doctor', 'Bác sĩ chuyên khoa'),
('Staff', 'Nhân viên lễ tân / hành chính');

-- Mật khẩu mặc định demo là '123456' (Trong thực tế phải hash bằng BCrypt/Identity)
INSERT INTO Accounts (Username, PasswordHash, Email, RoleID) VALUES
('admin01', 'hashed_password_123456', 'admin@hospital.com', 1),
('doctor_tung', 'hashed_password_123456', 'tung.nguyen@hospital.com', 2),
('staff_hoa', 'hashed_password_123456', 'hoa.le@hospital.com', 3);

INSERT INTO Staffs (FullName, Gender, DateOfBirth, PhoneNumber, Position, Specialization, AccountID) VALUES
(N'Nguyễn Văn Admin', N'Nam', '1990-01-01', '0912345678', N'Quản trị viên', NULL, 1),
(N'Nguyễn Thanh Tùng', N'Nam', '1985-05-12', '0987654321', N'Bác sĩ', N'Nội khoa', 2),
(N'Lê Thị Hoa', N'Nữ', '1998-09-20', '0933445566', N'Lễ tân', NULL, 3);