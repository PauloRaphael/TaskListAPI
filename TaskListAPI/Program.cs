using TaskListAPI.Data;
using TaskListAPI.Repository;
using Microsoft.EntityFrameworkCore;
using TaskListAPI.Model.Entities;
using TaskListAPI.Services; // Adicione este using

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Configurar Entity Framework Core com PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TaskListDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// 2. Registrar Repositórios
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Repetições para as outras entidades:
builder.Services.AddScoped<IGenericRepository<Categoria>, GenericRepository<Categoria>>();
builder.Services.AddScoped<IGenericRepository<Tarefa>, GenericRepository<Tarefa>>();
builder.Services.AddScoped<IGenericRepository<LoginSessao>, GenericRepository<LoginSessao>>();

// 3. Registrar o Serviço de Autenticação (Correção para o erro anterior)
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAllOrigins", // You can name this anything
        policy =>
        {
            policy.AllowAnyOrigin() // Allows requests from any domain/origin
                  .AllowAnyHeader()  // Allows any type of request header
                  .AllowAnyMethod(); // Allows any HTTP method (GET, POST, PUT, DELETE, etc.)
        });
});

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

// Adicionar Autenticação e Autorização na ordem correta
app.UseAuthentication(); // Adicione se estiver implementando autenticação
app.UseAuthorization();

app.MapControllers();
app.Run();