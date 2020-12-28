using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;

namespace Graphic_l2
{
    //структура простой точки
    class Point_u
    {
        public double x, y, z;

        public Point_u(double _x, double _y, double _z)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
        }
        public Point_u()
        {
            x = y = z = 0;
        }
    }
    
    //структура простой линии
    class Line
    {
        public Point_u first;
        public Point_u second;
        
        public Line(Point_u _first, Point_u _second)
        {
            this.first = _first;
            this.second = _second;
        }
        public Line()
        {
            first = new Point_u();
            second = new Point_u();
        }
    }

    //структура многоугольника
    class Figure
    {
        //линии многоугольника
        public List<Line> lines;
        
        //коэф-ты уравнения плоскости
        public double a, b, c, d;
        public Figure(List<Line> _lines)
        {
            this.lines = _lines;
        }
        public Figure()
        {
            lines = new List<Line>();
        }
    }

    //структура окна
    struct Window {
        public double xLeft, xRight, yUp, yDown; 
        public Window(double x_left, double x_right, double y_upper, double y_down) 
        { 
            xLeft = x_left; xRight = x_right; yUp = y_upper; yDown = y_down; 
        } 
    }

    class WarnockAlgo
    {
        //все фигуры
        private int amountFigures;
        private List<Figure> figures;
        private List<Point[]> windowMass;

        public bool readFromFile(string filename, ref double maxX, ref double maxY)
        {
            maxX = maxY = 0;
            string tmp;
            string[] mass_str;
            try
            {
                StreamReader inp_l = new System.IO.StreamReader(filename);

                //считывание количества фигур
                tmp = inp_l.ReadLine();
                amountFigures = Convert.ToInt32(tmp);

                //считывание фигур
                figures = new List<Figure>();
                for (int k = 0; k < amountFigures; k++)
                    figures.Add(new Figure());

                int index = 0;

                double x_first, x_second, y_first, y_second, z_first, z_second;
                while(index < amountFigures)
                {
                    mass_str = inp_l.ReadLine().Split(' ');
                    if (mass_str.Length > 1)
                    {
                        x_first = Convert.ToDouble(mass_str[0]); x_second = Convert.ToDouble(mass_str[3]);
                        y_first = Convert.ToDouble(mass_str[1]); y_second = Convert.ToDouble(mass_str[4]);
                        z_first = Convert.ToDouble(mass_str[2]); z_second = Convert.ToDouble(mass_str[5]);
                        figures[index].lines.Add(new Line(new Point_u(x_first, y_first, z_first),
                            new Point_u(x_second, y_second, z_second)));

                        if (x_first > maxX)
                            maxX = x_first;
                        if (y_first > maxY)
                            maxY = y_first;
                    }
                    else
                        index++;
                }
                return true;
            }
            catch(IOException e)
            {
                return false;
            }
        }

        public List<Point[]> getImage(ref List<int> colors, ref float x_move, ref float y_move)
        {
            windowMass = new List<Point[]>();
            colors.Clear();

            //получение коэффициентов  для уравнения плоскости
            for(int i = 0; i < figures.Count; i++)
            {
                Line line0 = figures[i].lines[0];
                Line line1 = figures[i].lines[1];
                figures[i].a = line0.first.y * (line1.first.z - line1.second.z) + line1.first.y * (line1.second.z - line0.first.z) + line1.second.y * (line0.first.z - line1.first.z);
                figures[i].b = line0.first.z * (line1.first.x - line1.second.x) + line1.first.z * (line1.second.x - line0.first.x) + line1.second.z * (line0.first.x - line1.first.x);
                figures[i].c = line0.first.x * (line1.first.y - line1.second.y) + line1.first.x * (line1.second.y - line0.first.y) + line1.second.x * (line0.first.y - line1.first.y);
                figures[i].d = - (line0.first.x * (line1.first.y * line1.second.z - line1.second.y * line1.first.z) + line1.first.x * (line1.second.y * line0.first.z - line0.first.y * line1.second.z) + line1.second.x * (line0.first.y * line1.first.z - line1.first.y * line0.first.z));
            }


            Stack<Window> stackWin = new Stack<Window>();

            Window win = getWin(ref figures, ref x_move, ref y_move);
            stackWin.Push(win);
            
            while (stackWin.Count > 0)
            {
                Window tmp = new Window();
                tmp = stackWin.Peek();

                List<int> list_cover = new List<int>();
                List<int> list_out = new List<int>();
                List<int> list_part = new List<int>();
                List<int> list_in = new List<int>();

                stackWin.Pop();

                for (int i = 0; i < figures.Count; ++i)
                {
                    double x_max = 0, x_min = 0, y_max = 0, y_min = 0;
                    getBorderFigure(ref x_max, ref x_min, ref y_max, ref y_min, figures[i]);

                    //внутренний
                    if (x_max <= tmp.xRight && x_min >= tmp.xLeft && y_max <= tmp.yUp && y_min >= tmp.yDown)
                        list_in.Add(i);
                    //пробная функция( стр.69 )
                    else if(probn(tmp, figures[i]))
                    {
                        if (is_area(figures[i], tmp.xLeft + (tmp.xRight - tmp.xLeft) / 2, tmp.yDown + (tmp.yUp - tmp.yDown) / 2))
                            list_cover.Add(i);
                        else
                            list_out.Add(i);
                    }
                    else
                        list_part.Add(i);
                }

                //если все внешние
                if (list_out.Count == amountFigures)
                {
                    windowMass.Add(new Point[] { new Point((int)tmp.xLeft, (int)tmp.yDown),
                                                new Point((int)tmp.xLeft, (int)tmp.yUp), 
                                                new Point((int)tmp.xRight, (int)tmp.yUp), 
                                                new Point((int)tmp.xRight, (int)tmp.yDown)});
                    colors.Add(-1);
                }
                else
                //один охватывающий
                if (list_cover.Count == 1 && list_in.Count == 0 && list_part.Count == 0)
                {
                    windowMass.Add(new Point[] { new Point((int)tmp.xLeft, (int)tmp.yDown),
                                                new Point((int)tmp.xLeft, (int)tmp.yUp),
                                                new Point((int)tmp.xRight, (int)tmp.yUp),
                                                new Point((int)tmp.xRight, (int)tmp.yDown)});
                    colors.Add(list_cover[0]);
                }
                else
                //один внутренний
                if (list_in.Count == 1 && list_part.Count == 0)
                {
                    int amountPoints = figures[list_in[0]].lines.Count;
                    Point[] mass = new Point[amountPoints];
                    for (int i = 0; i < amountPoints; i++)
                        mass[i] = new Point((int)figures[list_in[0]].lines[i].first.x, (int)figures[list_in[0]].lines[i].first.y);
                    
                    windowMass.Add(mass);
                    colors.Add(list_in[0]);

                }
                else
                if (list_cover.Count > 1)
                {
                    if (tmp.xRight - tmp.xLeft < 0.5)
                    {
                        int maxIndex = -1;
                        double maxZ = -100000000;
                        double curZ;
                        for(int i = 0; i < list_cover.Count; i++)
                        {
                            curZ = ((-figures[list_cover[i]].d - figures[list_cover[i]].a * tmp.xLeft - figures[list_cover[i]].b * tmp.yUp) / figures[list_cover[i]].c);
                            if(curZ > maxZ)
                            {
                                maxZ = curZ;
                                maxIndex = i;
                            }
                        }

                        windowMass.Add(new Point[] { new Point((int)tmp.xLeft, (int)tmp.yDown),
                                                new Point((int)tmp.xLeft, (int)tmp.yUp),
                                                new Point((int)tmp.xRight, (int)tmp.yUp),
                                                new Point((int)tmp.xRight, (int)tmp.yDown)});
                        if (maxIndex != -1)
                            colors.Add(list_cover[maxIndex]);
                        else
                            colors.Add(-1);
                    }
                    else
                    {
                        double del = (tmp.xRight - tmp.xLeft) / 2;
                        stackWin.Push(new Window(tmp.xLeft, tmp.xRight - del, tmp.yUp, tmp.yDown + del));
                        stackWin.Push(new Window(tmp.xLeft + del, tmp.xRight, tmp.yUp, tmp.yDown + del));
                        stackWin.Push(new Window(tmp.xLeft, tmp.xRight - del, tmp.yUp - del, tmp.yDown));
                        stackWin.Push(new Window(tmp.xLeft + del, tmp.xRight, tmp.yUp - del, tmp.yDown));
                    }
                }
                else

                //если пересекающие, несколько охватывающих или несколько внутренних
                if (list_in.Count > 1 || list_part.Count > 0)
                {

                    //размер менее 1 пиксела - ищем самый ближний из них
                    if (tmp.xRight - tmp.xLeft <= 0.5)
                    {
                        double zmax = -10000000;
                        int index = -1;
                        for (int i = 0; i < list_in.Count; i++)
                        {
                            double z = ((-figures[list_in[i]].d - figures[list_in[i]].a * tmp.xLeft - figures[list_in[i]].b * tmp.yDown) / figures[list_in[i]].c);
                            if (z > zmax) { zmax = z; index = list_in[i]; }
                        }
                        for (int i = 0; i < list_part.Count; i++)
                        {
                            double z = ((-figures[list_part[i]].d - figures[list_part[i]].a * tmp.xLeft - figures[list_part[i]].b * tmp.yDown) / figures[list_part[i]].c);
                            if (z > zmax) { zmax = z; index = list_part[i]; }
                        }
                        for (int i = 0; i < list_cover.Count; i++)
                        {
                            double z = ((-figures[list_cover[i]].d - figures[list_cover[i]].a * tmp.xLeft - figures[list_cover[i]].b * tmp.yDown) / figures[list_cover[i]].c);
                            if (z > zmax) { zmax = z; index = list_cover[i]; }
                        }

                        windowMass.Add(new Point[] { new Point((int)tmp.xLeft, (int)tmp.yDown),
                                                new Point((int)tmp.xLeft, (int)tmp.yUp),
                                                new Point((int)tmp.xRight, (int)tmp.yUp),
                                                new Point((int)tmp.xRight, (int)tmp.yDown)});
                        if (index != -1)
                            colors.Add(index);
                        else
                            colors.Add(-1);
                    }
                    
                    else
                    //иначе разбиваем на более мелкие окна
                    {
                        double del = (tmp.xRight - tmp.xLeft) / 2;
                        stackWin.Push(new Window(tmp.xLeft, tmp.xRight - del, tmp.yUp, tmp.yDown + del));
                        stackWin.Push(new Window(tmp.xLeft + del, tmp.xRight, tmp.yUp, tmp.yDown + del));
                        stackWin.Push(new Window(tmp.xLeft, tmp.xRight - del, tmp.yUp - del, tmp.yDown));
                        stackWin.Push(new Window(tmp.xLeft + del, tmp.xRight, tmp.yUp - del, tmp.yDown));
                    }
                }
            }

            return windowMass;
        }


        private Window getWin(ref List<Figure> figures, ref float x_move, ref float y_move)
        {
            double x_left = max_coord, x_right = -max_coord, y_uper = -max_coord, y_down = max_coord;
            foreach (var figure in figures)
            {
                foreach (var line in figure.lines)
                {
                    if (line.first.x > x_right)
                        x_right = line.first.x;
                    if (line.first.x < x_left) 
                        x_left = line.first.x;
                    if (line.second.x > x_right) 
                        x_right = line.second.x;
                    if (line.second.x < x_left) 
                        x_left = line.second.x; 

                    if (line.first.y > y_uper) 
                        y_uper = line.first.y; 
                    if (line.first.y < y_down)
                        y_down = line.first.y;
                    if (line.second.y > y_uper)
                        y_uper = line.second.y;
                    if (line.second.y < y_down) 
                        y_down = line.second.y;
                }
            }

            if (x_right - x_left > y_uper - y_down)
            {
                y_uper += Math.Abs(x_right - x_left - y_uper + y_down);
            }
            else if (x_right - x_left < y_uper - y_down)
            {
                x_right += Math.Abs(y_uper - y_down - x_right + x_left);
            }

            if (x_left < 0) 
                x_move = Math.Abs((float)x_left); 
            if (y_down < 0) 
                y_move = Math.Abs((float)y_down);

            return new Window(x_left, x_right, y_uper, y_down);
        }

        const double max_coord = 100000.0;
        private void getBorderFigure(ref double x_max, ref double x_min, ref double y_max, ref double y_min, Figure figure)
        {
            x_max = -max_coord; x_min = max_coord; y_max = -max_coord; y_min = max_coord;
            foreach (var line in figure.lines)
            {
                if (line.first.x > x_max) { x_max = line.first.x; }
                if (line.first.x < x_min) { x_min = line.first.x; }
                if (line.second.x > x_max) { x_max = line.second.x; }
                if (line.second.x < x_min) { x_min = line.second.x; }

                if (line.first.y > y_max) { y_max = line.first.y; }
                if (line.first.y < y_min) { y_min = line.first.y; }
                if (line.second.y > y_max) { y_max = line.second.y; }
                if (line.second.y < y_min) { y_min = line.second.y; }
            }
        }

        //Вычисление пробной функции
        //(для всех точек окна знак должен быть одинаков для каждой стороны многоугольника)
        public bool probn(Window wnd, Figure f)
        {
            List < Line > lines = f.lines;
            double m, b;
            int lt = 0, ld = 0, rt = 0, rd = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].first.y > wnd.yUp && lines[i].second.y > wnd.yUp
                    || lines[i].first.y < wnd.yDown && lines[i].second.y < wnd.yDown)
                {
                    continue;
                }
                if ((lines[i].second.x - lines[i].first.x) != 0)
                {
                    if (lines[i].first.x > wnd.xRight && lines[i].second.x > wnd.xRight
                    || lines[i].first.x < wnd.xLeft && lines[i].second.x < wnd.xLeft)
                    {
                        continue;
                    }

                    m = (lines[i].second.y - lines[i].first.y) / (lines[i].second.x - lines[i].first.x);
                    b = lines[i].second.y - m * lines[i].second.x;
                    lt = 0; ld = 0; rt = 0; rd = 0;
                    if ((wnd.yUp - m * wnd.xLeft - b) >= 0)
                        lt = 1;
                    else
                        lt = -1;

                    if ((wnd.yDown - m * wnd.xLeft - b) >= 0)
                        ld = 1;
                    else
                        ld = -1;

                    if ((wnd.yUp - m * wnd.xRight - b) >= 0)
                        rt = 1;
                    else
                        rt = -1;

                    if ((wnd.yDown - m * wnd.xRight - b) >= 0)
                        rd = 1;
                    else
                        rd = -1;

                    if (Math.Abs(lt + ld + rt + rd) != 4)
                        return false;
                }
                else
                {
                    //если x1=x2, то в знаменателе 0
                    m = -lines[i].first.x;
                    lt = 0; ld = 0; rt = 0; rd = 0;
                    if ((wnd.xRight + m) >= 0)
                        lt = 1;
                    else
                        lt = -1;

                    if ((wnd.xLeft + m) >= 0)
                        ld = 1;
                    else
                        ld = -1;

                    if (Math.Abs(lt + ld) != 2)
                        return false;
                }
            }
            return true;
        }

        //Лежит ли точка внутри многоугольника
        public bool is_area(Figure f, double x, double y)
        {
            bool result = false;
            foreach(Line line in f.lines)
            {
                if ((line.first.y < y && line.second.y >= y || line.second.y < y && line.first.y >= y) &&
                     (line.first.x + (y - line.first.y) / (line.second.y - line.first.y) * (line.second.x - line.first.x) < x))
                    result = !result;
            }
            return result;
        }
    }
}
