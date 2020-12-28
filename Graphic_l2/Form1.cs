using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Graphic_l2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //список полигонов для вывода
        private List<Point[]> windows;
        List<Brush> brushes;
        int currIndex;
        Graphics graphics;
        List<int> _colors;
        float x_move = 0, y_move = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            WarnockAlgo alg = new WarnockAlgo();
            double maxX = 0, maxY = 0;
            if (alg.readFromFile("input.txt", ref maxX, ref maxY) == true)
            {

                _colors = new List<int>();
                x_move = 0;
                y_move = 0;


                //набор кистей
                brushes = new List<Brush>();
                brushes.Add(Brushes.White);
                brushes.Add(Brushes.Blue);
                brushes.Add(Brushes.Yellow);
                brushes.Add(Brushes.Red);
                brushes.Add(Brushes.Aqua);

                //получение списка точек
                windows = alg.getImage(ref _colors, ref x_move, ref y_move);


                graphics = panelGraphic.CreateGraphics();
                int maxScale = 0;
                if (maxX > maxY)
                    maxScale = panelGraphic.Width / Convert.ToInt32(maxY);
                else
                    maxScale = panelGraphic.Height / Convert.ToInt32(maxX);
                for(int i = 0; i < windows.Count; i++)
                {
                    for(int j = 0; j < windows[i].Length; j++)
                    {
                        windows[i][j].X = (windows[i][j].X + (int)x_move) * maxScale;
                        windows[i][j].Y = (windows[i][j].Y + (int)y_move) * maxScale;
                    }
                }


                currIndex = 0;

                timer1.Interval = 5;
                timer1.Start();
                button2.Enabled = true;
            }
            else
                MessageBox.Show("Ошибка открытия файла!");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            for(int i = currIndex; i < windows.Count; i++)
                graphics.FillPolygon(brushes[_colors[i] + 1], windows[i]);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (currIndex < windows.Count)
            {
                graphics.FillPolygon(brushes[_colors[currIndex] + 1], windows[currIndex]);
                currIndex++;
            }
            else
                timer1.Stop();

        }
    }
}
