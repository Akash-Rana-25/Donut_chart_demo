using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Builder;
using Chart_WebApi.Model;
using Chart_WebApi.Static_Data;
using Razor.Templating.Core;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ChartDataFactory>();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
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

app.MapGet("Report", async (ChartDataFactory chartDataFactory) =>
{
    ChartData chartdata = chartDataFactory.CreateRandomChartData();
    var html = await RazorTemplateEngine.RenderAsync("View/chart.cshtml", chartdata);
    return Results.Ok(html);


});
    app.MapGet("Report1", async ( ) =>
    {
		List<DataPoint> dataPoints = new List<DataPoint>();

		dataPoints.Add(new DataPoint("Herbal Medicines", 41));
		dataPoints.Add(new DataPoint("Aroma Therapy", 22));
		dataPoints.Add(new DataPoint("Homeopathy", 9));
		dataPoints.Add(new DataPoint("Acupuncture", 7));
		dataPoints.Add(new DataPoint("Massage Therapy", 5));
		dataPoints.Add(new DataPoint("Reflexology", 6));
		dataPoints.Add(new DataPoint("Osteopathy", 5));
		dataPoints.Add(new DataPoint("Chiropractic", 5));

		var DataPoints = JsonConvert.SerializeObject(dataPoints);
		var html = await RazorTemplateEngine.RenderAsync("View/chart1.cshtml", DataPoints);

		

		return Results.Ok(html);

	});
app.Run();
