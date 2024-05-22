using Telegram.Bot;
using Telegram.Bot.Polling;
using WebApplication1.Services;
using WebApplication1.Services.Handler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<BkService>();

builder.Services.AddSingleton<IUpdateHandler, BotUpdateHandler>();
builder.Services.AddSingleton(new TelegramBotClient("6726665639:AAEzGElAxUHe7js1j5qJTMVDRaXUexIFfvI"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
