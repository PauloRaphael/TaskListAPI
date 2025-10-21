using TaskListAPI.Data;
using TaskListAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TaskListAPI.Model.Entities;

var builder = WebApplication.CreateBuilder(args);

// Adicionar servi�os ao container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Configurar Entity Framework Core com PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TaskListDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// 2. Registrar Reposit�rios
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));  
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Repeti��es para as outras entidades:
builder.Services.AddScoped<IGenericRepository<Categoria>, GenericRepository<Categoria>>();
builder.Services.AddScoped<IGenericRepository<Tarefa>, GenericRepository<Tarefa>>();
builder.Services.AddScoped<IGenericRepository<LoginSessao>, GenericRepository<LoginSessao>>();

var app = builder.Build();

// Configura��o do pipeline HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();