using HospitalManagement.DataAccess.Entities;
using HospitalManagement.Repository;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<HospitalManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Server=(localdb)\\MSSQLLocalDB;Database=HospitalManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"));

// Add Repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();

// Configure OData
builder.Services.AddControllers()
    .AddOData(options => options
        .Select()
        .Filter()
        .Expand()
        .OrderBy()
        .SetMaxTop(100)
        .Count()
        .AddRouteComponents("odata", GetEdmModel()));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<Account>("Accounts");
    builder.EntitySet<Role>("Roles");
    builder.EntitySet<Patient>("Patients");
    builder.EntitySet<Staff>("Staffs");
    builder.EntitySet<Appointment>("Appointments");
    
    var medicalRecord = builder.EntitySet<MedicalRecord>("MedicalRecords");
    medicalRecord.EntityType.HasKey(m => m.RecordId);
    
    builder.EntitySet<Medicine>("Medicines");
    
    var prescription = builder.EntitySet<Prescription>("Prescriptions");
    prescription.EntityType.HasKey(p => p.PrescriptionId);
    
    return builder.GetEdmModel();
}
