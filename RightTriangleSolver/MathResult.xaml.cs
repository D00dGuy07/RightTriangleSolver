using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RightTriangleSolver
{
    /// <summary>
    /// Interaction logic for MathResult.xaml
    /// </summary>
    public partial class MathResult : UserControl
    {
        Tuple<char, List<Tuple<string, string>>> m_Data;

        public MathResult(Tuple<char, List<Tuple<string, string>>> result)
        {
            InitializeComponent();

            m_Data = result;

            TextBlock varName = FindName("variableName") as TextBlock;
            varName.Text = m_Data.Item1.ToString();

            StackPanel stepsStack = FindName("stepsStack") as StackPanel;
            foreach (var stepData in result.Item2)
            {
                MathStep mathStep = new MathStep(stepData.Item1, stepData.Item2);
                stepsStack.Children.Add(mathStep);
            }
        }
    }
}
