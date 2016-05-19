using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SignalFilterDescretConvolution
{
    public partial class Form1 : Form
    {
        int M=65;

        public Form1()
        {
            InitializeComponent();
            chart1.Series.Clear();
            var data = ReadFromFile(@"C:\Users\Comp01\Downloads\Outlook.com (1)\filekr1.dat");
            chart1.Series.Add("Исходный сигнал");
            chart1.Series[0].ChartType = SeriesChartType.Line;
            data.ToList().ForEach(f => chart1.Series[0].Points.AddY(f));

            var coeffs = GetCoeffs();
            var result = Filter(data, coeffs);

            chart3.Series.Clear();
            chart3.Series.Add("После фильтрации");
            chart3.Series[0].ChartType = SeriesChartType.Line;
            
            result.ToList().ForEach(v => chart3.Series[0].Points.AddY(v));
        }

        private float[] ReadFromFile(string file)
        {
            
            using(StreamReader streamReader = new StreamReader(file))
            using (BinaryReader reader = new BinaryReader(streamReader.BaseStream))
            {
                float[] result = new float[reader.BaseStream.Length / 4];
                int i = 0;
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    result[i] = reader.ReadSingle();
                    i++;
                }
                return result;
            }
        }
        private double[] GetCoeffs()
        {
            double[] result = new double[M];
            double w = 2 * Math.PI * ((double)200 / 2000);
            double q = w / Math.PI;

            int n = M / 2;

            int j = n+1;

            for (int k = 0; k < n; k++)
            {
                if (k == 0)
                {
                    result[j] = q;
                }
                else
                {
                    result[j + k] = q * (Math.Sin(k * w) / k * w);
                    result[j - k] = q * (Math.Sin(k * w) / k * w);
                }
            }
            return result;
        }
        private double[] Filter(float[] input,double[] coeffs)
        {
            double[] result = new double[input.Length];
            for (int i = M / 2; i < input.Length - M / 2; i++)
            {
                result[i] = 0;
                for (int j = 0; j < M; j++)
                {
                    result[i] = result[i] + input[i - M / 2 + j] * coeffs[j];
                }
            }
            return result;
        }
    }
}
