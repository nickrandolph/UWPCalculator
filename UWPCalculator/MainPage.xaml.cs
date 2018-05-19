using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPCalculator
{
    public sealed partial class MainPage : Page
    {
        double accu = 0.0;
        double operand = 0.0;
        Func<double, double, double> queuedOperation;
        public IDictionary<string, Action<object, RoutedEventArgs>> Operations;
        public MainPage()
        {
            InitializeComponent();

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
            Result.Text = operand.ToString();
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
            Result.Text = accu.ToString().Substring(0, Math.Min(10, result.Length));
        }
    }

    //public class MainViewModel
    //{
    //    public Tuple<char, Color, Func<double, double, double>>[] Buttons = new Tuple<char, Color, Func<double, double, double>>[]
    //    {
    //       {'7',Color.White,(x,y)=>7 }
    //    };

    //}
}
