using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for TriangleData.xaml
    /// </summary>
    public partial class TriangleData : UserControl
    {
        public TriangleData()
        {
            InitializeComponent();
        }

		private double degToRad(double deg)
        {
			return deg * (Math.PI / 180.0);
        }

		private double radToDeg(double rad)
        {
			return rad * (180.0 / Math.PI);

		}

        private void calculate_Click(object sender, RoutedEventArgs e)
        {
            TextBox A = FindName("A") as TextBox;
            TextBox B = FindName("B") as TextBox;
            TextBox a = FindName("a") as TextBox;
            TextBox b = FindName("b") as TextBox;
            TextBox c = FindName("c") as TextBox;

			double? val_A = null;
			double? val_B = null;
			double? val_a = null;
			double? val_b = null;
			double? val_c = null;

			// Parse input
			try
			{ val_A = Convert.ToDouble(A.Text); }
			catch(Exception) {}
			try
			{ val_B = Convert.ToDouble(B.Text); }
			catch (Exception) { }
			try
			{ val_a = Convert.ToDouble(a.Text); }
			catch (Exception) { }
			try
			{ val_b = Convert.ToDouble(b.Text); }
			catch (Exception) { }
			try
			{ val_c = Convert.ToDouble(c.Text); }
			catch (Exception) { }

			// Check if any calculations can be performed
			int lengthConstants = ((val_a == null) ? 0 : 1) + ((val_b == null) ? 0 : 1) + ((val_c == null) ? 0 : 1);
			int angleConstants = ((val_A == null) ? 0 : 1) + ((val_B == null) ? 0 : 1);
			if ((angleConstants == 0 && lengthConstants < 2) || 
				(angleConstants == 2 && lengthConstants == 3))
			{
				MessageBox.Show("There is nothing to calculate!");
				return;
            }

			var results = new List<Tuple<char, List<Tuple<string, string>>>>();

			// Calculate angles
			if (val_A == null && val_B != null)
            {
				val_A = 90.0 - val_B;
				A.Text = val_A.ToString();

				results.Add(new Tuple<char, List<Tuple<string, string>>>(
					'A', new List<Tuple<string, string>>
					{
						new Tuple<string, string>(@"A=90-B", "Formula"),
						new Tuple<string, string>($@"A=90-{val_B}", "Substitute variables"),
						new Tuple<string, string>($@"A={val_A}", "Simplify")
					}
				));
			}

			if (val_B == null && val_A != null)
			{
				val_B = 90.0 - val_A;
				B.Text = val_B.ToString();

				results.Add(new Tuple<char, List<Tuple<string, string>>>(
					'B', new List<Tuple<string, string>>
					{
						new Tuple<string, string>(@"B=90-A", "Formula"),
						new Tuple<string, string>($@"B=90-{val_A}", "Substitute variables"),
						new Tuple<string, string>($@"B={val_B}", "Simplify")
					}
				));
			}

			// Calculate lengths
			// If it gets here then we either have
			// 0 angles and 2 lengths : Fill in third length
			// 2 angles and 1-2 lengths : Fill in the remaining lengths
			// 0 angles and 3 lengths : Get angles with arccos/arcsin/arctan
			if (lengthConstants == 2)
            {
				if (val_a == null)
                { 
					val_a = Math.Sqrt((double)(val_c * val_c - val_b * val_b));
					a.Text = val_a.ToString();

					results.Add(new Tuple<char, List<Tuple<string, string>>>(
						'a', new List<Tuple<string, string>>
						{
							new Tuple<string, string>(@"a=\sqrt{c^{2}-b^{2}}", "Formula"),
							new Tuple<string, string>(@"a=\sqrt{" + val_c + @"^{2}-" + val_b + @"^{2}}", "Substitute variables"),
							new Tuple<string, string>(@"a=\sqrt{" + (val_c * val_c) + @"-" + (val_b * val_b) + @"}", "Square numbers inside square root"),
							new Tuple<string, string>(@"a=\sqrt{" + (val_c * val_c - val_b * val_b) + @"}", "Simplify"),
							new Tuple<string, string>($@"a={val_a}", "Solve square root")
						}
					));
				}
				else if (val_b == null)
				{ 
					val_b = Math.Sqrt((double)(val_c * val_c - val_a * val_a));
					b.Text = val_b.ToString();

					results.Add(new Tuple<char, List<Tuple<string, string>>>(
						'b', new List<Tuple<string, string>>
						{
							new Tuple<string, string>(@"b=\sqrt{c^{2}-a^{2}}", "Formula"),
							new Tuple<string, string>(@"b=\sqrt{" + val_c + @"^{2}-" + val_a + @"^{2}}", "Substitute variables"),
							new Tuple<string, string>(@"b=\sqrt{" + (val_c * val_c) + @"-" + (val_a * val_a) + @"}", "Square numbers inside square root"),
							new Tuple<string, string>(@"b=\sqrt{" + (val_c * val_c - val_a * val_a) + @"}", "Simplify"),
							new Tuple<string, string>($@"b={val_b}", "Solve square root")
						}
					));
				}
				else if (val_c == null)
				{ 
					val_c = Math.Sqrt((double)(val_a * val_a + val_b * val_b)); 
					c.Text = val_c.ToString();

					results.Add(new Tuple<char, List<Tuple<string, string>>>(
						'c', new List<Tuple<string, string>>
						{
							new Tuple<string, string>(@"c=\sqrt{a^{2}+b^{2}}", "Formula"),
							new Tuple<string, string>(@"c=\sqrt{" + val_a + @"^{2}+" + val_b + @"^{2}}", "Substitute variables"),
							new Tuple<string, string>(@"c=\sqrt{" + (val_a * val_a) + @"+" + (val_b * val_b) + @"}", "Square numbers inside square root"),
							new Tuple<string, string>(@"c=\sqrt{" + (val_a * val_a + val_b * val_b) + @"}", "Simplify"),
							new Tuple<string, string>($@"c={val_c}", "Solve square root")
						}
					));
				}
			}

			if (lengthConstants == 1)
            {
				if (val_A != null)
                {
					// A is theta, a is adjacent and b is opposite
					if (val_a != null)
                    {
						// Adjacent is known
						val_c = (double)val_a / Math.Cos(degToRad((double)val_A));
						val_b = Math.Tan(degToRad((double)val_A)) * (double)val_a;
						c.Text = val_c.ToString();
						b.Text = val_b.ToString();

						results.Add(new Tuple<char, List<Tuple<string, string>>>(
							'c', new List<Tuple<string, string>>
							{
								new Tuple<string, string>(@"c=\frac{a}{\cos\left(A\right)}", "Formula"),
								new Tuple<string, string>(@"c=\frac{" + val_a + @"}{\cos\left(" + val_A + @"\right)}", "Substitute variables"),
								new Tuple<string, string>(@"c=\frac{" + val_a + @"}{" + Math.Cos(degToRad((double)val_A)) + @"}", "Solve cosine function"),
								new Tuple<string, string>($@"c={val_c}", "Simplify")
							}
						));
						results.Add(new Tuple<char, List<Tuple<string, string>>>(
							'b', new List<Tuple<string, string>>
							{
								new Tuple<string, string>(@"b=\tan\left(A\right)\cdot a", "Formula"),
								new Tuple<string, string>(@"b=\tan\left(" + val_A + @"\right)\cdot " + val_a, "Substitute variables"),
								new Tuple<string, string>(@"b=" + Math.Tan(degToRad((double)val_A)) + @"\cdot " + val_a, "Solve tangent function"),
								new Tuple<string, string>($@"b={val_b}", "Simplify")
							}
						));
					}
					if (val_b != null)
                    {
						// Opposite is known
						val_c = (double)val_b / Math.Sin(degToRad((double)val_A));
						val_a = (double)val_b / Math.Tan(degToRad((double)val_A));
						c.Text = val_c.ToString();
						a.Text = val_a.ToString();

						results.Add(new Tuple<char, List<Tuple<string, string>>>(
							'c', new List<Tuple<string, string>>
							{
								new Tuple<string, string>(@"c=\frac{b}{\sin\left(A\right)}", "Formula"),
								new Tuple<string, string>(@"c=\frac{" + val_b + @"}{\sin\left(" + val_A + @"\right)}", "Substitute variables"),
								new Tuple<string, string>(@"c=\frac{" + val_b + @"}{" + Math.Sin(degToRad((double)val_A)) + @"}", "Solve sine function"),
								new Tuple<string, string>($@"c={val_c}", "Simplify")
							}
						));
						results.Add(new Tuple<char, List<Tuple<string, string>>>(
							'a', new List<Tuple<string, string>>
							{
								new Tuple<string, string>(@"a=\frac{b}{\tan\left(A\right)}", "Formula"),
								new Tuple<string, string>(@"a=\frac{" + val_b + @"}{\tan\left(" + val_A + @"\right)}", "Substitute variables"),
								new Tuple<string, string>(@"a=\frac{" + val_b + @"}{" + Math.Tan(degToRad((double)val_A)) + @"}", "Solve sine function"),
								new Tuple<string, string>($@"a={val_a}", "Simplify")
							}
						));
					}
					if (val_c != null)
                    {
						// Hypotenuse is known
						val_b = Math.Sin(degToRad((double)val_A)) * (double)val_c;
						val_a = Math.Cos(degToRad((double)val_A)) * (double)val_c;
						b.Text = val_b.ToString();
						a.Text = val_a.ToString();

						results.Add(new Tuple<char, List<Tuple<string, string>>>(
							'b', new List<Tuple<string, string>>
							{
								new Tuple<string, string>(@"b=\sin\left(A\right)\cdot c", "Formula"),
								new Tuple<string, string>(@"b=\sin\left(" + val_A + @"\right)\cdot " + val_c, "Substitute variables"),
								new Tuple<string, string>(@"b=" + Math.Sin(degToRad((double)val_A)) + @"\cdot " + val_c, "Solve tangent function"),
								new Tuple<string, string>($@"b={val_b}", "Simplify")
							}
						));
						results.Add(new Tuple<char, List<Tuple<string, string>>>(
							'a', new List<Tuple<string, string>>
							{
								new Tuple<string, string>(@"a=\cos\left(A\right)\cdot c", "Formula"),
								new Tuple<string, string>(@"a=\cos\left(" + val_A + @"\right)\cdot " + val_c, "Substitute variables"),
								new Tuple<string, string>(@"a=" + Math.Cos(degToRad((double)val_A)) + @"\cdot " + val_c, "Solve tangent function"),
								new Tuple<string, string>($@"a={val_a}", "Simplify")
							}
						));
					}
				}
				else if (val_B != null)
                {
					// B is theta, b is adjacent and a is opposite
					if (val_a != null)
					{
						// Opposite is known
						val_c = (double)val_a / Math.Sin(degToRad((double)val_B));
						val_b = (double)val_a / Math.Tan(degToRad((double)val_B));
						c.Text = val_c.ToString();
						b.Text = val_b.ToString();

						results.Add(new Tuple<char, List<Tuple<string, string>>>(
							'c', new List<Tuple<string, string>>
							{
								new Tuple<string, string>(@"c=\frac{a}{\sin\left(B\right)}", "Formula"),
								new Tuple<string, string>(@"c=\frac{" + val_a + @"}{\sin\left(" + val_B + @"\right)}", "Substitute variables"),
								new Tuple<string, string>(@"c=\frac{" + val_a + @"}{" + Math.Sin(degToRad((double)val_B)) + @"}", "Solve sine function"),
								new Tuple<string, string>($@"c={val_c}", "Simplify")
							}
						));
						results.Add(new Tuple<char, List<Tuple<string, string>>>(
							'b', new List<Tuple<string, string>>
							{
								new Tuple<string, string>(@"b=\frac{a}{\tan\left(B\right)}", "Formula"),
								new Tuple<string, string>(@"b=\frac{" + val_a + @"}{\tan\left(" + val_B + @"\right)}", "Substitute variables"),
								new Tuple<string, string>(@"b=\frac{" + val_a + @"}{" + Math.Tan(degToRad((double)val_B)) + @"}", "Solve sine function"),
								new Tuple<string, string>($@"b={val_b}", "Simplify")
							}
						));
					}
					if (val_b != null)
					{
						// Adjacent is known
						val_c = (double)val_b / Math.Cos(degToRad((double)val_B));
						val_a = Math.Tan(degToRad((double)val_B)) * (double)val_b;
						c.Text = val_c.ToString();
						a.Text = val_a.ToString();

						results.Add(new Tuple<char, List<Tuple<string, string>>>(
							'c', new List<Tuple<string, string>>
							{
								new Tuple<string, string>(@"c=\frac{b}{\cos\left(B\right)}", "Formula"),
								new Tuple<string, string>(@"c=\frac{" + val_b + @"}{\cos\left(" + val_B + @"\right)}", "Substitute variables"),
								new Tuple<string, string>(@"c=\frac{" + val_b + @"}{" + Math.Cos(degToRad((double)val_B)) + @"}", "Solve cosine function"),
								new Tuple<string, string>($@"c={val_c}", "Simplify")
							}
						));
						results.Add(new Tuple<char, List<Tuple<string, string>>>(
							'a', new List<Tuple<string, string>>
							{
								new Tuple<string, string>(@"a=\tan\left(B\right)\cdot b", "Formula"),
								new Tuple<string, string>(@"a=\tan\left(" + val_B + @"\right)\cdot " + val_b, "Substitute variables"),
								new Tuple<string, string>(@"a=" + Math.Tan(degToRad((double)val_B)) + @"\cdot " + val_b, "Solve tangent function"),
								new Tuple<string, string>($@"a={val_a}", "Simplify")
							}
						));
					}
					if (val_c != null)
					{
						// Hypotenuse is known
						val_a = Math.Sin(degToRad((double)val_B)) * (double)val_c;
						val_b = Math.Cos(degToRad((double)val_B)) * (double)val_c;
						a.Text = val_a.ToString();
						b.Text = val_b.ToString();

						results.Add(new Tuple<char, List<Tuple<string, string>>>(
							'a', new List<Tuple<string, string>>
							{
								new Tuple<string, string>(@"a=\sin\left(B\right)\cdot c", "Formula"),
								new Tuple<string, string>(@"a=\sin\left(" + val_B + @"\right)\cdot " + val_c, "Substitute variables"),
								new Tuple<string, string>(@"a=" + Math.Sin(degToRad((double)val_B)) + @"\cdot " + val_c, "Solve tangent function"),
								new Tuple<string, string>($@"a={val_a}", "Simplify")
							}
						));
						results.Add(new Tuple<char, List<Tuple<string, string>>>(
							'b', new List<Tuple<string, string>>
							{
								new Tuple<string, string>(@"b=\cos\left(B\right)\cdot c", "Formula"),
								new Tuple<string, string>(@"b=\cos\left(" + val_B + @"\right)\cdot " + val_c, "Substitute variables"),
								new Tuple<string, string>(@"b=" + Math.Cos(degToRad((double)val_B)) + @"\cdot " + val_c, "Solve tangent function"),
								new Tuple<string, string>($@"b={val_b}", "Simplify")
							}
						));
					}
				}
			}

			if (angleConstants == 0)
            {
				// Get angle a
				val_A = radToDeg(Math.Acos((double)val_a / (double)val_c));
				val_B = 90.0 - val_A;
				A.Text = val_A.ToString();
				B.Text = val_B.ToString();

				results.Add(new Tuple<char, List<Tuple<string, string>>>(
					'A', new List<Tuple<string, string>>
					{
						new Tuple<string, string>(@"A=\arccos\left(\frac{a}{c}\right)", "Formula"),
						new Tuple<string, string>(@"A=\arccos\left(\frac{" + val_a + @"}{" + val_c + @"}\right)", "Substitute variables"),
						new Tuple<string, string>(@"A=\arccos\left(" + ((double)val_a / (double)val_c) + @"\right)", "Simplify fraction inside arccos function"),
						new Tuple<string, string>($@"A={val_A}", "Solve arccos function")
					}
				));

				results.Add(new Tuple<char, List<Tuple<string, string>>>(
					'B', new List<Tuple<string, string>>
					{
						new Tuple<string, string>(@"B=90-A", "Formula"),
						new Tuple<string, string>($@"B=90-{val_A}", "Substitute variables"),
						new Tuple<string, string>($@"B={val_B}", "Simplify")
					}
				));
			}

			ResultsWindow resultWindow = new ResultsWindow(results);
			resultWindow.Show();
		}
	}
}
