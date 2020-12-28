using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections.Specialized;

namespace Graphic_l1
{
    public partial class Form1 : Form
    {
        //количество точек и соединительных линий
        const int amount_points = 56;
        const int amount_lines = 88;


        //Структура отдельной точки
        struct Point_gr
        {
            public double x;
            public double y;
            public double z;
            public double h;
            public Point_gr(double x, double y, double z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.h = 1.5;
            }
        }
        
        //Структура линии
        struct Line_gr
        {
            public int from;
            public int to;

            public Line_gr(int a, int b)
            {
                this.from = a;
                this.to = b;
            }
        }
        
        //Координаты точек фигуры
        Point_gr[] points;
        Line_gr[] lines;

        Graphics g;

        /*Рисование фигур*/
        void DrawPoints()
        {
            drawBox.Refresh();

            //Если точки уже были отрисованы
            //то по-новому проецируем их
            if (points != null)
            {
                //матрица косоугольного проецирования
                double[,] projectionMatrix =
                {
                    {1, 0, 0, 0},
                    {0, 1, 0, 0},
                    {-0.25 * Math.Sqrt(2), -0.25 * Math.Sqrt(2), 0, 0},
                    {0, 0, 0, 1}
                };

                //Изменённые координаты точек
                Point_gr[] trans_points = new Point_gr[amount_points];

                double[] matrix;
                for (int i = 0; i < amount_points; i++)
                {
                    double[] point_coord = { points[i].x, points[i].y, points[i].z, points[i].h };
                    //перемножение координат точки на матрицу проектирования
                    matrix = MatrixMulty(projectionMatrix, point_coord);
                    trans_points[i] = new Point_gr(matrix[0] / matrix[3], matrix[1] / matrix[3], matrix[2] / matrix[3]);
                }

                Bitmap myBitmap = new Bitmap(drawBox.Width, drawBox.Height);

                g = Graphics.FromImage(myBitmap);

                //рисование осей координат
                Point_gr start_point = new Point_gr(0, 0, 0);
                Point_gr x = new Point_gr(300, 0, 0), x_trans;
                Point_gr y = new Point_gr(0, 300, 0), y_trans;
                Point_gr z = new Point_gr(0, 0, 300), z_trans;

                double[] point_coord_x = { x.x, x.y, x.z, x.h };
                matrix = MatrixMulty(projectionMatrix, point_coord_x);
                x_trans = new Point_gr(matrix[0] / matrix[3], matrix[1] / matrix[3], matrix[2] / matrix[3]);

                double[] point_coord_y = { y.x, y.y, y.z, y.h };
                matrix = MatrixMulty(projectionMatrix, point_coord_y);
                y_trans = new Point_gr(matrix[0] / matrix[3], matrix[1] / matrix[3], matrix[2] / matrix[3]);

                double[] point_coord_z = { z.x, z.y, z.z, z.h };
                matrix = MatrixMulty(projectionMatrix, point_coord_z);
                z_trans = new Point_gr(matrix[0] / matrix[3], matrix[1] / matrix[3], matrix[2] / matrix[3]);

                g.DrawLine(new Pen(Color.Green, 2), new Point(175, 175), new Point(175 + Convert.ToInt32(x_trans.x), 175 + Convert.ToInt32(x_trans.y)));
                g.DrawLine(new Pen(Color.Green, 2), new Point(175, 175), new Point(175 + Convert.ToInt32(y_trans.x), 175 + Convert.ToInt32(y_trans.y)));
                g.DrawLine(new Pen(Color.Green, 2), new Point(175, 175), new Point(125 + Convert.ToInt32(z_trans.x), 125 + Convert.ToInt32(z_trans.y)));

                //рисование линий 3д объекта
                foreach (Line_gr line in lines)
                { 
                    g.DrawLine(Pens.Red, Convert.ToInt32(trans_points[line.from].x + 175), Convert.ToInt32(trans_points[line.from].y + 175),
                        Convert.ToInt32(trans_points[line.to].x + 175), Convert.ToInt32(trans_points[line.to].y + 175));
                }
                drawBox.Image = myBitmap;
            }
            //иначе считываем точки из файлов
            else
            {
                //56 точек и 88 линий соединения
                points = new Point_gr[amount_points + 3];
                lines = new Line_gr[amount_lines];

                //Файлы, из которых берутся точки
                System.IO.StreamReader inp_p = new System.IO.StreamReader("points.txt");
                System.IO.StreamReader inp_l = new System.IO.StreamReader("lines.txt");
                string tmp;
                string[] mass_str;

                //Получение списка точек
                int i = 0;
                do
                {
                    tmp = inp_p.ReadLine();
                    mass_str = tmp.Split(' ');
                    points[i] = new Point_gr(Convert.ToDouble(mass_str[0]), Convert.ToDouble(mass_str[1]),
                        Convert.ToDouble(mass_str[2]));
                    i++;
                } while (inp_p.Peek() != -1);

                //Получение отрезков
                i = 0;
                do
                {
                    tmp = inp_l.ReadLine();
                    mass_str = tmp.Split(' ');
                    lines[i] = new Line_gr(Convert.ToInt32(mass_str[0]), Convert.ToInt32(mass_str[1]));
                    i++;
                } while (inp_l.Peek() != -1);
                //Отрисовывание полученной фигуры
                DrawPoints();
            }
        }

        //Перемножение матриц 4*4 и 4*1
        double [] MatrixMulty(double [,] matrix, double [] points)
        {
            double[] result = { 0, 0, 0, 0 };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    result[i] += matrix[j, i] * points[j];
            }
            return result;
        }
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buttonAnimationCntrl.Enabled = false;
        }

        //Отрисовка буквы по нажатию на кнопку
        private void buttonStart_Click(object sender, EventArgs e)
        {
            drawBox.Enabled = true;
            buttonStart.Visible = false;
            buttonAnimationCntrl.Enabled = true;
            DrawPoints();

        }
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //обрабатываются нажатия клавиш
            //по ним вызываются соответствующие преобразования
            if(points != null)
            {
                if (e.Shift && e.Control && (e.KeyCode == Keys.X || e.KeyCode == Keys.Y || e.KeyCode == Keys.Z))
                    ShiftFigure(e.KeyCode, -1);
                else if (e.Shift && (e.KeyCode == Keys.X || e.KeyCode == Keys.Y || e.KeyCode == Keys.Z))
                    ShiftFigure(e.KeyCode, 1);
                else if (e.Alt && e.Control && (e.KeyCode == Keys.X || e.KeyCode == Keys.Y || e.KeyCode == Keys.Z))
                    RotateFigure(e.KeyCode, -1);
                else if (e.Alt && (e.KeyCode == Keys.X || e.KeyCode == Keys.Y || e.KeyCode == Keys.Z))
                    RotateFigure(e.KeyCode, 1);
                else if (e.Control && (e.KeyCode == Keys.X || e.KeyCode == Keys.Y || e.KeyCode == Keys.Z))
                    ScaleFigure(e.KeyCode, -1);
                else if (e.KeyCode == Keys.X || e.KeyCode == Keys.Y || e.KeyCode == Keys.Z)
                    ScaleFigure(e.KeyCode, 1);
            }
        }

        /*Перемещение фигуры*/
        private void ShiftFigure(Keys keycode, int direction)
        {
            //Матрица перемещения
            double[,] shift_matrix =
            {
                {1, 0, 0, 0},
                {0, 1, 0, 0},
                {0, 0, 1, 0},
                {0, 0, 0, 1}
            };

            if (direction > 0)
                direction = 1;
            else
                direction = -1;

            if (keycode == Keys.X)
                shift_matrix[3, 0] = 2 * direction;
            else if (keycode == Keys.Y)
            {
                shift_matrix[3, 1] = 2 * direction * (-1);
            }
            else if (keycode == Keys.Z)
                shift_matrix[3, 2] = 2 * direction;

            //Само перемещение фигуры
            double [] matrix;
            for (int i = 0; i < amount_points + 3; i++)
            {
                if (i == amount_points + 1)
                    continue;
                double[] point_coord = { points[i].x, points[i].y, points[i].z, points[i].h};
                matrix = MatrixMulty(shift_matrix, point_coord);
                points[i].x = matrix[0];
                points[i].y = matrix[1];
                points[i].z = matrix[2];
                points[i].h = matrix[3];
            }
            DrawPoints();
        }
        
        /*Вращение фигуры*/
        private void RotateFigure(Keys keycode, int direction)
        {
            // Матрица поворота
            double[,] rotate_matrix =
            {
                {1, 0, 0, 0},
                {0, 1, 0, 0},
                {0, 0, 1, 0},
                {0, 0, 0, 1}
            };
            if (direction > 0)
                direction = 1;
            else direction = -1;
            double cos = Math.Cos(5 * direction * Math.PI / 180);
            double sin = Math.Sin(5 * direction * Math.PI / 180);
            if (keycode == Keys.X)
            {
                rotate_matrix[1, 1] = cos;
                rotate_matrix[1, 2] = sin;
                rotate_matrix[2, 1] = -sin;
                rotate_matrix[2, 2] = cos;
            }
            else if (keycode == Keys.Y)
            {
                rotate_matrix[0, 0] = cos;
                rotate_matrix[0, 2] = -sin;
                rotate_matrix[2, 0] = sin;
                rotate_matrix[2, 2] = cos;
            }
            else if (keycode == Keys.Z)
            {
                rotate_matrix[0, 0] = cos;
                rotate_matrix[0, 1] = sin;
                rotate_matrix[1, 0] = -sin;
                rotate_matrix[1, 1] = cos;
            }
            //Сам поворот
            double[] matrix;
            for (int i = 0; i < amount_points + 3; i++)
            {
                double[] point_coord = { points[i].x, points[i].y, points[i].z, points[i].h };
                matrix = MatrixMulty(rotate_matrix, point_coord);
                points[i].x = matrix[0];
                points[i].y = matrix[1];
                points[i].z = matrix[2];
                points[i].h = matrix[3];
            }
            DrawPoints();
        }
        
        /*Масштабирование фигуры*/
        private void ScaleFigure(Keys keycode, double direction)
        {
            //матрица растяжения (сжатия)
            double[,] scale_matrix =
            {
                {1, 0, 0, 0},
                {0, 1, 0, 0},
                {0, 0, 1, 0},
                {0, 0, 0, 1}
            };
            //Если положительное - увеличение масштабирования,
            // иначе уменьшение
            if (direction > 0)
                direction = 1.1;
            else
                direction = 0.9;
            if (keycode == Keys.X)
                scale_matrix[0, 0] = direction;
            else if (keycode == Keys.Y)
                scale_matrix[1, 1] = direction;
            else if (keycode == Keys.Z)
                scale_matrix[2, 2] = direction;

            //Само растяжение
            double[] matrix;
            for (int i = 0; i < amount_points + 3; i++)
            {
                double[] point_coord = { points[i].x, points[i].y, points[i].z, points[i].h };
                matrix = MatrixMulty(scale_matrix, point_coord);
                points[i].x = matrix[0];
                points[i].y = matrix[1];
                points[i].z = matrix[2];
                points[i].h = matrix[3];
            }
            DrawPoints();
        }

        //Обработка анимации
        private Random rnd = new Random();
        private void timerChangeObj_Tick(object sender, EventArgs e)
        {
            //Вызов по таймеру изменения положения точки
            changeVertexPosition();
            DrawPoints();
        }

        //индекс текущей изменяемой точки
        private int currChangeIndex = -1;
        //в каком направлении идет изменение (от настоящей координаты или к ней)
        private bool direction = false;

        //Точность совпадения точки
        private const double epsilon = 0.01;
       
        /*Изменение позиции вершины*/
        private void changeVertexPosition()
        {
            //Если ни одна вершина еще не изменяется, то выбираем вершину для изменения
            //И запоминаем её старые координаты, вектор перемещения
            if(currChangeIndex == -1)
            {
                currChangeIndex = rnd.Next(amount_points);
                direction = true;
                points[amount_points] = points[currChangeIndex];
                points[amount_points + 1] = new Point_gr(Convert.ToDouble(rnd.Next(-100, 100)), Convert.ToDouble(rnd.Next(-100, 100)),
                    Convert.ToDouble(rnd.Next(-100, 100)));
                points[amount_points + 2] = new Point_gr(points[amount_points].x + points[amount_points + 1].x, points[amount_points].y + points[amount_points + 1].y,
                    points[amount_points].z + points[amount_points + 1].z);
            }

            //изменяем положение точки в направлении -  от настоящей координаты
            if(direction == true)
            {
                points[currChangeIndex].x += points[amount_points + 1].x / 10;
                points[currChangeIndex].y += points[amount_points + 1].y / 10;
                points[currChangeIndex].z += points[amount_points + 1].z / 10;

                //Если текущее положение точки достаточно близко к тому, которое должно быть получено расчетами
                //то меняем направление движения и для точности устанавливаем точку в расчетное положение
                if (points[amount_points + 2].x + epsilon > points[currChangeIndex].x && points[amount_points + 2].x - epsilon < points[currChangeIndex].x
                    && points[amount_points + 2].y + epsilon > points[currChangeIndex].y && points[amount_points + 2].y - epsilon < points[currChangeIndex].y
                    && points[amount_points + 2].z + epsilon > points[currChangeIndex].z && points[amount_points + 2].z - epsilon < points[currChangeIndex].z)
                {
                    direction = false;
                    points[currChangeIndex] = points[amount_points + 2];
                }
            }
            //изменяем  положение точки в направлении -  к настоящей координате
            else
            {
                points[currChangeIndex].x -= points[amount_points + 1].x / 10;
                points[currChangeIndex].y -= points[amount_points + 1].y / 10;
                points[currChangeIndex].z -= points[amount_points + 1].z / 10;

                //Если текущее положение точки достаточно близко к её изначальному, 
                //то изменения для этой вершины окончены, выставляем сигнал того, что ни одна точка не изменяется
                if (points[amount_points].x + epsilon > points[currChangeIndex].x && points[amount_points].x - epsilon < points[currChangeIndex].x
                   && points[amount_points].y + epsilon > points[currChangeIndex].y && points[amount_points].y - epsilon < points[currChangeIndex].y
                   && points[amount_points].z + epsilon > points[currChangeIndex].z && points[amount_points].z - epsilon < points[currChangeIndex].z)

                {
                    points[currChangeIndex] = points[amount_points];
                    currChangeIndex = -1;
                }
            }
        }

        private void buttonAnimationCntrl_Click(object sender, EventArgs e)
        {
            if (timerChangeObj.Enabled == false)
                timerChangeObj.Start();
            else
                timerChangeObj.Stop();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            //сброс положения объекта
            points = null;
            lines = null;
            currChangeIndex = -1;
            timerChangeObj.Stop();
            DrawPoints();
        }
    }
}
