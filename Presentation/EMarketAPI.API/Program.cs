using Microsoft.EntityFrameworkCore;
using EMarketAPI.Persistence.Context;
using EMarketAPI.Application.Mappings;
using EMarketAPI.Application.Abstractions.UnitOfWork;
using EMarketAPI.Persistence.Concretes.UnitOfWork;





var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));           //veritaban� ba�lant�s�.




builder.Services.AddAutoMapper(typeof(MappingProfile));  //automapper kullan�m� i�in.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();  // savechanges i�lemleri i�in.






var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
