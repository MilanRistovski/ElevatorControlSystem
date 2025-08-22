using ElevatorControl.Application.Abstractions;
using ElevatorControl.Application.Options;
using ElevatorControl.Application.Services;
using ElevatorControl.Application.Services.cs;
using ElevatorControl.Infrastructure.Logging;
using ElevatorControl.Infrastructure.State;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Configure<ElevatorOptions>(builder.Configuration.GetSection("ElevatorOptions"));

builder.Services.AddSingleton<IElevatorState>(sp => {
	var options = sp.GetRequiredService<IOptions<ElevatorOptions>>().Value;
	return new ElevatorState(options.FloorCount, options.CarCount, options.InitialFloor);
});
builder.Services.AddSingleton<IEventLog, InMemoryLog>();
builder.Services.AddSingleton<IElevatorDispatcher, CarDispatcher>();
builder.Services.AddSingleton<CarMover>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Elevator}/{action=Index}/{id?}");

app.Run();
