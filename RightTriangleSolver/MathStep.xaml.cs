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
using WpfMath.Controls;

namespace RightTriangleSolver
{
    /// <summary>
    /// Interaction logic for MathStep.xaml
    /// </summary>
    public partial class MathStep : UserControl
    {
        public MathStep(string formula, string description)
        {
            InitializeComponent();

            FormulaControl formulaControl = FindName("stepView") as FormulaControl;
            TextBlock descriptionBlock = FindName("stepDescription") as TextBlock;

            formulaControl.Formula = formula;
            descriptionBlock.Text = description;
        }
    }
}
