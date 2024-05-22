using Chart_WebApi.Model;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Chart_WebApi.Static_Data
{
	public class ChartDataFactory
	{
		private static readonly Random _random = new Random();
		private static readonly string[] _chartTypes = { "doughnut", "polarArea", "bar", "line", "radar", "pie" };
		private static readonly string[] _labels = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
		private static readonly string[] _datasetLabels = { "Sales", "Revenue", "Expenses", "Profit" };
		private static readonly string[] _colors = {
			"rgba(255, 99, 132, 0.2)",
			"rgba(54, 162, 235, 0.2)",
			"rgba(75, 192, 192, 0.2)",
			"rgba(153, 102, 255, 0.2)",
			"rgba(255, 159, 64, 0.2)",
			"rgba(255, 205, 86, 0.2)",
			"rgba(201, 203, 207, 0.2)",
			"rgba(0, 123, 255, 0.2)",
			"rgba(40, 167, 69, 0.2)",
			"rgba(255, 193, 7, 0.2)"
		};

		private static readonly string[] _borderColors = {
			"rgba(255, 99, 132, 1)",
			"rgba(54, 162, 235, 1)",
			"rgba(75, 192, 192, 1)",
			"rgba(153, 102, 255, 1)",
			"rgba(255, 159, 64, 1)",
			"rgba(255, 205, 86, 1)",
			"rgba(201, 203, 207, 1)",
			"rgba(0, 123, 255, 1)",
			"rgba(40, 167, 69, 1)",
			"rgba(255, 193, 7, 1)"
		};


		public ChartData CreateRandomChartData()
		{
			var chartType = GetRandomChartType();
			var numberOfLabels = chartType == "polarArea" ? 5 : _random.Next(2, 12);
			var selectedLabels = GetRandomLabels(numberOfLabels);
			var datasetData = GetRandomDatasetData(numberOfLabels);
			var datasetColors = GetRandomDatasetColors(numberOfLabels);

			return new ChartData
			{
				ChartType = chartType,
				Labels = JsonSerializer.Serialize(selectedLabels),
				DatasetLabel = GetRandomDatasetLabel(),
				DatasetData = JsonSerializer.Serialize(datasetData),
				DatasetBackgroundColor = JsonSerializer.Serialize(datasetColors.BackgroundColors),
				DatasetBorderWidth = _random.Next(1, 5),
				ChartOptions = GetChartOptions(chartType)
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

		private static (List<string> BackgroundColors, List<string> BorderColors) GetRandomDatasetColors(int numberOfLabels)
		{
			var backgroundColors = new List<string>();
			var borderColors = new List<string>();

			for (int i = 0; i < numberOfLabels; i++)
			{
				backgroundColors.Add(_colors[_random.Next(_colors.Length)]);
				borderColors.Add(_borderColors[_random.Next(_borderColors.Length)]);
			}

			return (backgroundColors, borderColors);
		}

		private static string GetChartOptions(string chartType)
		{
			switch (chartType)
			{
				case "bar":
				case "line":
					return @"{
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }";
				case "radar":
					return @"{
                scale: {
                    ticks: {
                        beginAtZero: true
                    },
                    reverse: false
                }
            }";
				case "doughnut":
				case "pie":
					return @"{
                responsive: true,
                maintainAspectRatio: false
            }";
				case "polarArea":
					return @"{
                scale: {
                    ticks: {
                        beginAtZero: true
                    },
                    reverse: false
                },
                scale: {
                    ticks: {
                        beginAtZero: true
                    },
                    reverse: false
                }
            }";
				default:
					return "{}";
			}
		}


	}
}
