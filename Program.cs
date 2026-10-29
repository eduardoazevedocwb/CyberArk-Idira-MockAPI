using CyberArk_Idira_MockApi.Application.Interfaces;
using CyberArk_Idira_MockApi.Application.Services;
using CyberArk_Idira_MockApi.Domain.Entities;
using CyberArk_Idira_MockApi.Infrastructure.Data;
using CyberArk_Idira_MockApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApiDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ISafeService, SafeService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ISafeMemberService, SafeMemberService>();
builder.Services.AddScoped<IPlatformService, PlatformService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IRoleService, RoleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
