using DanfolioBackend.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddInternalServices();
builder.Services.AddDataRepositories();
builder.AddLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
