using Chart_WebApi.Model;
using Chart_WebApi.Static_Data;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.Reflection.Emit;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Razor.Templating.Core;
namespace Chart_WebApi.Controllers
{

    [Route("api/CreatePDF")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private IConverter _converter;
        public ChartController(IConverter converter)
        {
            _converter = converter;
        }


        [HttpGet]
        public async Task<IActionResult> CreatePDF()
        {


            // Download the browser if not already downloaded
            await new BrowserFetcher().DownloadAsync();
            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            using var page = await browser.NewPageAsync();
             
            ChartDataFactory chartdataFactory= new ChartDataFactory();
            // Generate chart data using factory
            var chartData = chartdataFactory.CreateRandomChartData();
			var htmlContent = $@"
<!DOCTYPE html>
<html>
<head>
    <title>Chart.js to PDF</title>
    <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 20px;
            height: 100vh; 
        }}
        .container {{
            max-width: 800px; 
            margin: auto;
            background: #fff;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            min-height: 70vh; 
            display: flex;
            align-items: center;
            justify-content: center; 
        }}
        canvas {{
            width: 100%; 
            max-width: 600px; 
            height: auto;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <canvas id='myChart' width='1000' height='1000'></canvas> 
    </div>
    <script> 
        var ctx = document.getElementById('myChart').getContext('2d');
        var myChart = new Chart(ctx, {{
            type: '{chartData.ChartType}',
            data: {{
                labels: {chartData.Labels},
                datasets: [{{
                    label: '{chartData.DatasetLabel}',
                    data: {chartData.DatasetData},
                    backgroundColor: {chartData.DatasetBackgroundColor},
                    borderWidth: {chartData.DatasetBorderWidth}
                }}]
            }},
            options: {chartData.ChartOptions}
        }});
    </script>
</body>
</html>";



			// Set HTML content in page
			await page.SetContentAsync(htmlContent);

            // Wait for chart canvas to be rendered
            await page.WaitForSelectorAsync("#myChart");

            // Generate PDF
            var pdfStream = await page.PdfStreamAsync(new PdfOptions
            {
                Format = PaperFormat.A4
            });

            // Return PDF as file
            return File(pdfStream, "application/pdf", "chart.pdf");
        }



    }
}



