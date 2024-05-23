using Chart_WebApi.Static_Data;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;
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
        
        canvas {{
            width: 100% !important;
            height: auto !important;
        }}
        h1 {{
            text-align: center;
            font-size: 20px;
            padding: 10px;
            background-color: #f0f0f0;
            
        }}
    </style>
</head>
<body>
    <div class='container'>
  <h1>{chartData.ChartType}</h1>
        <canvas id='myChart'></canvas> 
    </div>
    <script> 
        try {{
            var ctx = document.getElementById('myChart').getContext('2d');
            var myChart = new Chart(ctx, {{
                type: '{chartData.ChartType}',
                data: {{
                    labels: {chartData.Labels},
                    datasets: [{{
                        label: '{chartData.DatasetLabel}',
                        data: {chartData.DatasetData},
                        backgroundColor: {chartData.DatasetBackgroundColor},
                        borderColor: {chartData.DatasetBorderColor},
                        borderWidth: {chartData.DatasetBorderWidth}
                    }}]
                }},
                options: {{
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {{
                        yAxes: [{{
                            ticks: {{
                                beginAtZero: true
                            }}
                        }}]
                    }}
                }}
            }});
        }} catch (error) {{
            console.error('Error creating chart:', error);
        }}
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



