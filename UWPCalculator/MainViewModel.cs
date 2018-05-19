using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPCalculator
{
    public class MainViewModel : INotifyPropertyChanged
    {
        double accu = 0.0;
        double operand = 0.0;
        Func<double, double, double> queuedOperation;
        public IDictionary<string, Action<object, RoutedEventArgs>> Operations;
        private string _resultText = "0.0";
        public event PropertyChangedEventHandler PropertyChanged;
        public string Result
        {
            get => _resultText;
            set
            {
                _resultText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
            }
        }

        public MainViewModel()
        {
            Operations = new Dictionary<string, Action<object, RoutedEventArgs>>() {
            {"/", (sender, e) => calc((x, y) => x / y) },
            {"x", (sender, e) => calc((x, y) => x * y) },
            {"-", (sender, e) => calc((x, y) => x - y) },
            {"+", (sender, e) => calc((x, y) => x + y) },
            {"=", (sender, e) => calc((x, y) => x ) },
            {"c", (sender, e) => calc(null) }};
        }

        public void NumberClick(object sender, RoutedEventArgs e)
        {
            var value = int.Parse((sender as Button).Content as string);
            operand = operand * 10 + value;
            Result = operand.ToString();
        }

        public void calc(Func<double, double, double> operation)
        {
            if (operation == null) // C was pressed
            {
                accu = 0.0;
            }
            else
            {
                accu = queuedOperation != null ? queuedOperation(accu, operand) : operand;
            }
            queuedOperation = operation;
            operand = 0.0;
            var result = accu.ToString();
            Result = accu.ToString().Substring(0, Math.Min(10, result.Length));
        }
    }
}
