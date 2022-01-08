using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfMath.Controls;

namespace RightTriangleSolver
{
    /// <summary>
    /// Interaction logic for ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        private List<Tuple<char, List<Tuple<string, string>>>> m_Work;

        public ResultsWindow(List<Tuple<char, List<Tuple<string, string>>>> work)
        {
            InitializeComponent();

            m_Work = work;

            StackPanel formulas = FindName("formulaStack") as StackPanel;
            foreach (var resultData in work)
            {
                MathResult mathResult = new MathResult(resultData);
                formulas.Children.Add(mathResult);
            }
        }
    }
}
