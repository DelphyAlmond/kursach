
using Microsoft.OpenApi.Models;
using UniversityBusinessLogic.BusinessLogics;
using UniversityBusinessLogic.MailWorker;
using UniversityBusinessLogic.OfficePackage;
using UniversityBusinessLogic.OfficePackage.Implements;
using UniversityBusinessLogics.BusinessLogics;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicContracts;
using UniversityContracts.BusinessLogicsContracts;
using UniversityContracts.StorageContracts;
using UniversityDatabaseImplement.Implements;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddLog4Net("log4net.config");


// Add services to the container.
builder.Services.AddTransient<IUserStorageContract, UserStorageContract>();
builder.Services.AddTransient<IDisciplineStorageContract, DisciplineStorageContract>();
builder.Services.AddTransient<ITeacherStorageContract, TeacherStorageContract>();
builder.Services.AddTransient<IPlanOfStudyStorageContract, PlanOfStudyStorageContract>();
builder.Services.AddTransient<IAttestationStorage, AttestationStorageContract>();
builder.Services.AddTransient<IStatementStorageContract, StatementStorageContract>();
builder.Services.AddTransient<IStudentStorageContract, StudentStorageContract>();

builder.Services.AddTransient<AbstractSaveToExcelWorker, SaveToExcelWorker>();
builder.Services.AddTransient<AbstractSaveToWordWorker, SaveToWordWorker>();
builder.Services.AddTransient<AbstractSaveToPdfWorker, SaveToPdfWorker>();
builder.Services.AddTransient<AbstractSaveToExcelStorekeeper, SaveToExcelStorekeeper>();
builder.Services.AddTransient<AbstractSaveToWordStorekeeper, SaveToWordStorekeeper>();
builder.Services.AddTransient<AbstractSaveToPdfStorekeeper, SaveToPdfStorekeeper>();
builder.Services.AddTransient<IReportContract, ReportContract>();
builder.Services.AddTransient<IUserBusinessLogicContract, UserBusinessLogicContract>();
builder.Services.AddTransient<IDisciplineBusinessLogicContract, DisciplineBusinessLogicContract>();
builder.Services.AddTransient<ITeacherBusinessLogicContract, TeacherLogic>();
builder.Services.AddTransient<IPlanOfStudyBusinessLogicContract, PlanOfStudyBusinessLogicContract>();
builder.Services.AddTransient<IAttestationBusinessLogicContract, AttestationBusinessLogicContract>();
builder.Services.AddTransient<IStatementBusinessLogicContract, StatementBusinessLogicContract>();
builder.Services.AddTransient<IStudentBusinessLogicContract, StudentBusinessLogicContract>();
builder.Services.AddSingleton<AbstractMailWorker, MailKitWorker>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "UniversityWebApi",
        Version
    = "v1"
    });
});

var app = builder.Build();

var mailSender = app.Services.GetService<AbstractMailWorker>();

mailSender?.MailConfig(new MailConfigBindingModel
{
    MailLogin = builder.Configuration?.GetSection("MailLogin")?.Value?.ToString() ?? string.Empty,
    MailPassword = builder.Configuration?.GetSection("MailPassword")?.Value?.ToString() ?? string.Empty,
    SmtpClientHost = builder.Configuration?.GetSection("SmtpClientHost")?.Value?.ToString() ?? string.Empty,
    SmtpClientPort = Convert.ToInt32(builder.Configuration?.GetSection("SmtpClientPort")?.Value?.ToString()),
    PopHost = builder.Configuration?.GetSection("PopHost")?.Value?.ToString() ?? string.Empty,
    PopPort = Convert.ToInt32(builder.Configuration?.GetSection("PopPort")?.Value?.ToString())
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UniversityWebApi v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
