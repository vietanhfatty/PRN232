USE HospitalManagementDB;
GO

-- ==========================================
-- 1. XÓA DỮ LIỆU CŨ VÀ RESET IDENTITY SEED
-- ==========================================
PRINT N'Đang dọn dẹp dữ liệu cũ...';

-- Tắt tạm thời các ràng buộc khóa ngoại để tránh lỗi khi xóa
ALTER TABLE Prescriptions NOCHECK CONSTRAINT ALL;
ALTER TABLE MedicalRecords NOCHECK CONSTRAINT ALL;
ALTER TABLE Appointments NOCHECK CONSTRAINT ALL;
ALTER TABLE Patients NOCHECK CONSTRAINT ALL;
ALTER TABLE Staffs NOCHECK CONSTRAINT ALL;
ALTER TABLE Accounts NOCHECK CONSTRAINT ALL;
ALTER TABLE Roles NOCHECK CONSTRAINT ALL;
ALTER TABLE Medicines NOCHECK CONSTRAINT ALL;

-- Xóa dữ liệu
DELETE FROM Prescriptions;
DELETE FROM MedicalRecords;
DELETE FROM Appointments;
DELETE FROM Patients;
DELETE FROM Staffs;
DELETE FROM Accounts;
DELETE FROM Roles;
DELETE FROM Medicines;

-- Reset seed cho các cột IDENTITY về 0
DBCC CHECKIDENT ('Prescriptions', RESEED, 0);
DBCC CHECKIDENT ('MedicalRecords', RESEED, 0);
DBCC CHECKIDENT ('Appointments', RESEED, 0);
DBCC CHECKIDENT ('Patients', RESEED, 0);
DBCC CHECKIDENT ('Staffs', RESEED, 0);
DBCC CHECKIDENT ('Accounts', RESEED, 0);
DBCC CHECKIDENT ('Roles', RESEED, 0);
DBCC CHECKIDENT ('Medicines', RESEED, 0);

-- Bật lại các ràng buộc khóa ngoại
ALTER TABLE Prescriptions CHECK CONSTRAINT ALL;
ALTER TABLE MedicalRecords CHECK CONSTRAINT ALL;
ALTER TABLE Appointments CHECK CONSTRAINT ALL;
ALTER TABLE Patients CHECK CONSTRAINT ALL;
ALTER TABLE Staffs CHECK CONSTRAINT ALL;
ALTER TABLE Accounts CHECK CONSTRAINT ALL;
ALTER TABLE Roles CHECK CONSTRAINT ALL;
ALTER TABLE Medicines CHECK CONSTRAINT ALL;

PRINT N'Dọn dẹp hoàn tất!';

-- ==========================================
-- 2. CHÈN DỮ LIỆU BẢNG Roles (Vai Trò)
-- ==========================================
PRINT N'Đang chèn dữ liệu bảng Roles...';
SET IDENTITY_INSERT Roles ON;
INSERT INTO Roles (RoleID, RoleName, Description) VALUES 
(1, 'Admin', N'Quản trị viên toàn quyền hệ thống'),
(2, 'Doctor', N'Bác sĩ chuyên khoa khám chữa bệnh'),
(3, 'Staff', N'Nhân viên tiếp tân, y tá và hỗ trợ hành chính');
SET IDENTITY_INSERT Roles OFF;

-- ==========================================
-- 3. CHÈN DỮ LIỆU BẢNG Accounts (Tài Khoản)
-- ==========================================
PRINT N'Đang chèn dữ liệu bảng Accounts...';
SET IDENTITY_INSERT Accounts ON;
INSERT INTO Accounts (AccountID, Username, PasswordHash, Email, IsActive, RoleID) VALUES
(1, 'admin01', 'hashed_password_123456', 'admin@hospital.com', 1, 1),
(2, 'doctor.tung', 'hashed_password_123456', 'tung.nguyen@hospital.com', 1, 2),
(3, 'doctor.huong', 'hashed_password_123456', 'huong.tran@hospital.com', 1, 2),
(4, 'doctor.minh', 'hashed_password_123456', 'minh.pham@hospital.com', 1, 2),
(5, 'doctor.lan', 'hashed_password_123456', 'lan.vo@hospital.com', 1, 2),
(6, 'staff.hoa', 'hashed_password_123456', 'hoa.le@hospital.com', 1, 3),
(7, 'staff.dung', 'hashed_password_123456', 'dung.hoang@hospital.com', 1, 3);
SET IDENTITY_INSERT Accounts OFF;

-- ==========================================
-- 4. CHÈN DỮ LIỆU BẢNG Staffs (Nhân Viên / Bác Sĩ)
-- ==========================================
PRINT N'Đang chèn dữ liệu bảng Staffs...';
SET IDENTITY_INSERT Staffs ON;
INSERT INTO Staffs (StaffID, FullName, Gender, DateOfBirth, PhoneNumber, Address, Position, Specialization, AccountID) VALUES
(1, N'Nguyễn Văn Admin', N'Nam', '1990-01-01', '0912345678', N'12 Đường 3/2, Quận 10, TP. HCM', N'Quản trị viên', NULL, 1),
(2, N'Nguyễn Thanh Tùng', N'Nam', '1985-05-12', '0987654321', N'45 Lê Lợi, Quận 1, TP. HCM', N'Bác sĩ', N'Nội tổng quát', 2),
(3, N'Trần Thị Hương', N'Nữ', '1988-10-24', '0918273645', N'789 Nguyễn Trãi, Quận 5, TP. HCM', N'Bác sĩ', N'Nhi khoa', 3),
(4, N'Phạm Hải Minh', N'Nam', '1982-03-15', '0909123456', N'120 CMT8, Quận 3, TP. HCM', N'Bác sĩ', N'Tim mạch', 4),
(5, N'Võ Hoàng Lan', N'Nữ', '1991-07-09', '0977889900', N'35 Bùi Thị Xuân, Quận Tân Bình, TP. HCM', N'Bác sĩ', N'Da liễu', 5),
(6, N'Lê Thị Hoa', N'Nữ', '1998-09-20', '0933445566', N'15 Trần Hưng Đạo, Quận 1, TP. HCM', N'Lễ tân', NULL, 6),
(7, N'Hoàng Văn Dũng', N'Nam', '1995-12-05', '0944556677', N'256 Nguyễn Văn Cừ, Quận 5, TP. HCM', N'Điều dưỡng', NULL, 7);
SET IDENTITY_INSERT Staffs OFF;

-- ==========================================
-- 5. CHÈN DỮ LIỆU BẢNG Patients (Bệnh Nhân)
-- ==========================================
PRINT N'Đang chèn dữ liệu bảng Patients...';
SET IDENTITY_INSERT Patients ON;
INSERT INTO Patients (PatientID, FullName, Gender, DateOfBirth, PhoneNumber, Address, HealthInsuranceCode) VALUES
(1, N'Trần Văn Nam', N'Nam', '1975-04-18', '0901234567', N'12 Phố Huế, Hai Bà Trưng, Hà Nội', 'GD47979102001'),
(2, N'Lê Thị Mai', N'Nữ', '1992-08-30', '0912345098', N'56 Hàng Bông, Hoàn Kiếm, Hà Nội', 'GD47979102002'),
(3, N'Phạm Quốc Bảo', N'Nam', '2015-11-12', '0989012345', N'102 Đê La Thành, Đống Đa, Hà Nội', 'GD47979102003'),
(4, N'Nguyễn Thị Thu', N'Nữ', '1960-03-25', '0932145678', N'88 Lạc Long Quân, Tây Hồ, Hà Nội', 'GD47979102004'),
(5, N'Hoàng Minh Triết', N'Nam', '1999-05-05', '0965432109', N'145 Giải Phóng, Thanh Xuân, Hà Nội', 'GD47979102005'),
(6, N'Vũ Thị Hồng', N'Nữ', '1987-02-14', '0945678901', N'22 Nguyễn Chí Thanh, Ba Đình, Hà Nội', NULL),
(7, N'Đỗ Hoàng Long', N'Nam', '1994-07-21', '0911223344', N'45 Cầu Giấy, Quận Cầu Giấy, Hà Nội', 'GD47979102007'),
(8, N'Bùi Minh Tuyết', N'Nữ', '2003-10-02', '0922334455', N'76 Láng Hạ, Đống Đa, Hà Nội', NULL),
(9, N'Phan Thanh Sơn', N'Nam', '1980-12-15', '0933445577', N'34 Bạch Mai, Hai Bà Trưng, Hà Nội', 'GD47979102009'),
(10, N'Ngô Bảo Châu', N'Nữ', '1996-01-20', '0955667788', N'90 Kim Mã, Ba Đình, Hà Nội', 'GD47979102010');
SET IDENTITY_INSERT Patients OFF;

-- ==========================================
-- 6. CHÈN DỮ LIỆU BẢNG Appointments (Lịch Hẹn)
-- ==========================================
PRINT N'Đang chèn dữ liệu bảng Appointments...';
SET IDENTITY_INSERT Appointments ON;
INSERT INTO Appointments (AppointmentID, PatientID, DoctorID, AppointmentDate, Reason, Status, CreatedAt) VALUES
-- Khám Nội tổng quát - Bác sĩ Tùng (StaffID = 2)
(1, 1, 2, '2026-05-10 08:30:00', N'Đau đầu kéo dài kèm chóng mặt', 'Completed', '2026-05-09 10:00:00'),
(2, 2, 2, '2026-05-12 09:15:00', N'Đau dạ dày sau khi ăn đồ cay nóng', 'Completed', '2026-05-11 14:00:00'),
(3, 6, 2, '2026-06-03 08:00:00', N'Tái khám định kỳ đường huyết', 'Pending', '2026-06-02 16:30:00'),
(4, 7, 2, '2026-06-04 10:30:00', N'Kiểm tra sức khỏe tổng quát', 'Confirmed', '2026-06-01 09:00:00'),

-- Khám Nhi khoa - Bác sĩ Hương (StaffID = 3)
(5, 3, 3, '2026-05-15 14:00:00', N'Trẻ sốt cao liên tục 2 ngày kèm ho', 'Completed', '2026-05-15 08:00:00'),
(6, 8, 3, '2026-06-03 15:30:00', N'Bé biếng ăn và nổi mẩn đỏ', 'Pending', '2026-06-02 11:00:00'),

-- Khám Tim mạch - Bác sĩ Minh (StaffID = 4)
(7, 4, 4, '2026-05-18 10:00:00', N'Tức ngực, khó thở khi vận động mạnh', 'Completed', '2026-05-17 07:30:00'),
(8, 9, 4, '2026-06-05 09:00:00', N'Nhịp tim nhanh không đều', 'Confirmed', '2026-06-02 15:00:00'),

-- Khám Da liễu - Bác sĩ Lan (StaffID = 5)
(9, 5, 5, '2026-05-20 16:00:00', N'Dị ứng da toàn thân sau khi ăn hải sản', 'Completed', '2026-05-20 12:00:00'),
(10, 10, 5, '2026-05-22 14:30:00', N'Nổi mụn trứng cá viêm nặng', 'Cancelled', '2026-05-21 08:00:00');
SET IDENTITY_INSERT Appointments OFF;

-- ==========================================
-- 7. CHÈN DỮ LIỆU BẢNG MedicalRecords (Bệnh Án)
-- ==========================================
PRINT N'Đang chèn dữ liệu bảng MedicalRecords...';
SET IDENTITY_INSERT MedicalRecords ON;
INSERT INTO MedicalRecords (RecordID, AppointmentID, Diagnosis, TreatmentPlan, DoctorNotes, UpdatedAt) VALUES
-- Hẹn 1 (Bệnh nhân 1 - Bác sĩ Tùng)
(1, 1, N'Thiếu máu não cục bộ nhẹ do căng thẳng kéo dài', N'Nghỉ ngơi hợp lý, uống thuốc tăng cường tuần hoàn não và tái khám sau 2 tuần.', N'Bệnh nhân làm việc văn phòng, thức khuya nhiều.', '2026-05-10 09:30:00'),

-- Hẹn 2 (Bệnh nhân 2 - Bác sĩ Tùng)
(2, 2, N'Viêm loét dạ dày - tá tràng cấp tính', N'Uống thuốc kháng axit dạ dày trước ăn, tránh đồ cay nóng, chất kích thích.', N'Bệnh nhân có thói quen ăn uống không đúng giờ.', '2026-05-12 10:00:00'),

-- Hẹn 5 (Bệnh nhân 3 - Bác sĩ Hương)
(3, 5, N'Viêm họng cấp kèm sốt siêu vi', N'Hạ sốt khi trên 38.5 độ, bổ sung nước oresol, uống kháng sinh theo đơn.', N'Theo dõi nhiệt độ thường xuyên, nếu sốt co giật phải đưa đi cấp cứu.', '2026-05-15 15:00:00'),

-- Hẹn 7 (Bệnh nhân 4 - Bác sĩ Minh)
(4, 7, N'Huyết áp cao độ 1 - Rối loạn nhịp tim nhẹ', N'Uống thuốc hạ huyết áp mỗi sáng, giảm muối trong khẩu phần ăn, tập thể dục nhẹ nhàng.', N'Huyết áp đo tại phòng khám: 145/90 mmHg.', '2026-05-18 11:00:00'),

-- Hẹn 9 (Bệnh nhân 5 - Bác sĩ Lan)
(5, 9, N'Mề đay cấp tính do dị ứng hải sản', N'Uống thuốc kháng histamin, bôi kem làm dịu da, kiêng hải sản trong 1 tháng.', N'Cơ địa dị ứng với tôm, cua.', '2026-05-20 17:00:00');
SET IDENTITY_INSERT MedicalRecords OFF;

-- ==========================================
-- 8. CHÈN DỮ LIỆU BẢNG Medicines (Danh Mục Thuốc)
-- ==========================================
PRINT N'Đang chèn dữ liệu bảng Medicines...';
SET IDENTITY_INSERT Medicines ON;
INSERT INTO Medicines (MedicineID, MedicineName, Unit, Price, StockQuantity, Description) VALUES
(1, 'Paracetamol 500mg', N'Viên', 1500.00, 5000, N'Thuốc giảm đau, hạ sốt'),
(2, 'Amoxicillin 500mg', N'Viên', 3000.00, 2000, N'Kháng sinh điều trị nhiễm khuẩn đường hô hấp'),
(3, 'Augmentin 1g', N'Hộp', 250000.00, 120, N'Kháng sinh mạnh phối hợp điều trị viêm họng, viêm xoang'),
(4, 'Piracetam 800mg', N'Viên', 5000.00, 1500, N'Thuốc bổ não, tăng cường tuần hoàn não'),
(5, 'Ginkgo Biloba 120mg', N'Viên', 4000.00, 3000, N'Hoạt huyết dưỡng não, giảm đau đầu chóng mặt'),
(6, 'Phosphalugel', N'Gói', 7500.00, 800, N'Thuốc trị đau dạ dày, kháng axit (thuốc chữ P)'),
(7, 'Omeprazole 20mg', N'Viên', 2500.00, 2500, N'Giảm tiết axit dịch vị dạ dày'),
(8, 'Hapacol 250mg', N'Gói', 3500.00, 1000, N'Thuốc hạ sốt cho trẻ em vị cam dễ uống'),
(9, 'Cephalexin 500mg', N'Viên', 2000.00, 1800, N'Kháng sinh điều trị nhiễm khuẩn nhẹ'),
(10, 'Amlodipine 5mg', N'Viên', 1800.00, 4000, N'Thuốc điều trị tăng huyết áp'),
(11, 'Panadol Extra', N'Viên', 2000.00, 6000, N'Giảm đau đầu, đau răng, hạ sốt nhanh chóng'),
(12, 'Fexofenadine 180mg', N'Viên', 8500.00, 1200, N'Thuốc chống dị ứng thế hệ mới không gây buồn ngủ'),
(13, 'Hydrocortisone 10g', N'Tuýp', 35000.00, 250, N'Kem bôi da trị dị ứng, mẩn ngứa, côn trùng cắn'),
(14, 'Oresol 4.1g', N'Gói', 2000.00, 2000, N'Bù nước và chất điện giải khi sốt hoặc tiêu chảy'),
(15, 'Decolgen Forte', N'Viên', 1200.00, 5000, N'Thuốc điều trị cảm cúm, sổ mũi, nghẹt mũi');
SET IDENTITY_INSERT Medicines OFF;

-- ==========================================
-- 9. CHÈN DỮ LIỆU BẢNG Prescriptions (Chi Tiết Đơn Thuốc)
-- ==========================================
PRINT N'Đang chèn dữ liệu bảng Prescriptions...';
SET IDENTITY_INSERT Prescriptions ON;
INSERT INTO Prescriptions (PrescriptionID, RecordID, MedicineID, Quantity, Dosage) VALUES
-- Đơn thuốc cho Record 1 (Thiếu máu não)
(1, 1, 4, 30, N'Ngày uống 2 lần, mỗi lần 1 viên sau ăn sáng/tối'),
(2, 1, 5, 30, N'Ngày uống 2 lần, mỗi lần 1 viên sau ăn sáng/tối'),

-- Đơn thuốc cho Record 2 (Viêm dạ dày)
(3, 2, 6, 20, N'Uống trước ăn 30 phút, mỗi lần 1 gói, ngày 2 lần'),
(4, 2, 7, 14, N'Uống 1 viên vào buổi sáng trước khi ăn 30 phút'),

-- Đơn thuốc cho Record 3 (Viêm họng trẻ em)
(5, 3, 8, 10, N'Uống 1 gói khi sốt trên 38.5 độ C, cách nhau ít nhất 4-6 tiếng'),
(6, 3, 14, 5, N'Hòa tan 1 gói với 200ml nước đun sôi để nguội, uống rải rác trong ngày'),

-- Đơn thuốc cho Record 4 (Huyết áp cao)
(7, 4, 10, 30, N'Uống 1 viên vào mỗi buổi sáng sau khi thức dậy'),

-- Đơn thuốc cho Record 5 (Dị ứng hải sản)
(8, 5, 12, 10, N'Uống 1 viên vào buổi tối sau ăn'),
(9, 5, 13, 1, N'Bôi một lớp mỏng lên vùng da dị ứng ngày 2 lần sáng và tối');
SET IDENTITY_INSERT Prescriptions OFF;

GO

PRINT N'Hoàn tất chèn tất cả dữ liệu mẫu chất lượng cao cho HospitalManagementDB!';
