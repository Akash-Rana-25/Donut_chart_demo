using Chart_WebApi.Model;
using System.Text.Json;

namespace Chart_WebApi.Static_Data
{
    public  class ChartDataFactory
    {
        private static readonly Random _random = new Random();
        private static readonly string[] _chartTypes = { "bar", "line", "pie", "doughnut" };
        private static readonly string[] _labels = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        private static readonly string[] _datasetLabels = { "Sales", "Revenue", "Expenses", "Profit" };
        private static readonly string[] _colors = { "rgba(255, 99, 132, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(255, 159, 64, 0.2)" };
        private static readonly string[] _borderColors = { "rgba(255, 99, 132, 1)", "rgba(54, 162, 235, 1)", "rgba(75, 192, 192, 1)", "rgba(153, 102, 255, 1)", "rgba(255, 159, 64, 1)" };

        public  ChartData CreateRandomChartData()
        {
            var numberOfLabels = _random.Next(3, 7);
            var selectedLabels = GetRandomLabels(numberOfLabels);
            var datasetData = GetRandomDatasetData(numberOfLabels);

            return new ChartData
            {
                ChartType = GetRandomChartType(),
                Labels = JsonSerializer.Serialize(selectedLabels),
                DatasetLabel = GetRandomDatasetLabel(),
                DatasetData = JsonSerializer.Serialize(datasetData),
                DatasetBackgroundColor = GetRandomBackgroundColor(),
                DatasetBorderColor = GetRandomBorderColor(),
                DatasetBorderWidth = _random.Next(1, 5),
                ChartOptions = @"{
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }"
            };
        }

        private static string GetRandomChartType()
        {
            return _chartTypes[_random.Next(_chartTypes.Length)];
        }

        private static string[] GetRandomLabels(int count)
        {
            var selectedLabels = new List<string>();
            var availableLabels = new List<string>(_labels);

            for (int i = 0; i < count; i++)
            {
                var index = _random.Next(availableLabels.Count);
                selectedLabels.Add(availableLabels[index]);
                availableLabels.RemoveAt(index);
            }

            return selectedLabels.ToArray();
        }

        private static int[] GetRandomDatasetData(int count)
        {
            var data = new int[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = _random.Next(50, 150);
            }
            return data;
        }

        private static string GetRandomDatasetLabel()
        {
            return _datasetLabels[_random.Next(_datasetLabels.Length)];
        }

        private static string GetRandomBackgroundColor()
        {
            return _colors[_random.Next(_colors.Length)];
        }

        private static string GetRandomBorderColor()
        {
            return _borderColors[_random.Next(_borderColors.Length)];
        }
    }
}
