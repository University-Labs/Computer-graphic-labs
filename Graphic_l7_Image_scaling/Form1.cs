using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphic_l7
{
    public partial class ProgramForm : Form
    {
        //изображение
        Bitmap currImage;
        string imagePath;
        int imWidth, imHeight;

        //флаг наличия открытого изображения
        bool isOpen = false;

        public ProgramForm()
        {
            InitializeComponent();
        }

        private void showImage(PictureBox pic)
        {
            Graphics graphic = pic.CreateGraphics();
            imWidth = currImage.Width;
            imHeight = currImage.Height;

            graphic.DrawImage(currImage, new Rectangle(0,0, imWidth, imHeight));


        }

        //сбросить изображение в стандартное
        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (imagePath == "")
                isOpen = false;
            else
                isOpen = true;

            //при успешном открытии изображения выводим его на экран
            //и закрашиваем измененные изображения
            if (isOpen == true)
            {
                currImage = new Bitmap(imagePath);
                showImage(pictureBox1);
                Graphics g = pictureBox2.CreateGraphics();
                g.Clear(Color.White);

                g = pictureBox3.CreateGraphics();
                g.Clear(Color.White);
            }
            else
                MessageBox.Show("Выберите изображение!");
        }

        //метод ближайшего соседа
        private void buttonNearest_Click(object sender, EventArgs e)
        {
            //новые параметры изображения
            int newWidth =  (int)numericUpDownWidth.Value;
            int newHeight = (int)numericUpDownHeight.Value;

            double scaleX = (double)newWidth / imWidth;
            double scaleY = (double)newHeight / imHeight;

            //новое изображение
            Bitmap newImage = new Bitmap(newWidth, newHeight);

            int newI, newJ;

            //выбор ближайшего пикселя
            for (int i = 0; i < newWidth; i++)
                for (int j = 0; j < newHeight; j++)
                {
                    if (i / scaleX - (int)(i / scaleX) >= 0.5)
                        newI = (int)(i / scaleX) + 1;
                    else
                        newI = (int)(i / scaleX);

                    if (j / scaleY - (int)(j / scaleY) >= 0.5)
                        newJ = (int)(j / scaleY) + 1;
                    else
                        newJ = (int)(j / scaleY);

                    if (newI >= currImage.Width)
                        newI = (int)(i / scaleX);

                    if (newJ >= currImage.Height)
                        newJ = (int)(j / scaleY);
                    newImage.SetPixel(i, j, currImage.GetPixel(newI, newJ));
                }

            //вывод изображения
            Graphics g = pictureBox2.CreateGraphics();
            g.Clear(Color.White);

            newImage.Save("image_nearest.png", ImageFormat.Png);
            MessageBox.Show("Успешно произведена коррекция изображения методом Ближайшего соседа");

            g.DrawImage(newImage, 0, 0, newWidth, newHeight);

        }

        private double LinInterp(double beg, double end, double t)
        {
            return beg + (end - beg) * t;
        }

        //билинейное сглаживание
        private void buttonBilinear_Click(object sender, EventArgs e)
        {
            //новые параметры изображения
            int newWidth = (int)numericUpDownWidth.Value;
            int newHeight = (int)numericUpDownHeight.Value;

            double scaleX = (double)newWidth / currImage.Width;
            double scaleY = (double)newHeight / currImage.Height;
            //новое изображение
            Bitmap newImage = new Bitmap(newWidth, newHeight, currImage.PixelFormat);

            //проходим все пиксели
            Color lu, ld, ru, rd;
            double differX = (double)(imWidth - 1) / newWidth;
            double differY = (double)(imHeight - 1) / newHeight;
            for (int x = 0; x < newWidth; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    //вычисляем соседние по отношению к пикселю на новом изображении
                    //пиксели старого изображения
                    double gx = ((double)x) * differX;
                    double gy = ((double)y) * differY;
                    int gxi = (int)gx;
                    int gyi = (int)gy;

                    //берем соседние пиксели на старом изображении
                    lu = currImage.GetPixel(gxi, gyi);
                    ld = currImage.GetPixel(gxi + 1, gyi);
                    ru = currImage.GetPixel(gxi, gyi + 1);
                    rd = currImage.GetPixel(gxi + 1, gyi + 1);

                    //вычисление цветов
                    int red = (int)LinInterp(LinInterp(lu.R, ld.R, gx - gxi), LinInterp(ru.R, rd.R, gx - gxi), gy - gyi);
                    int green = (int)LinInterp(LinInterp(lu.G, ld.G, gx - gxi), LinInterp(ru.G, rd.G, gx - gxi), gy - gyi);
                    int blue = (int)LinInterp(LinInterp(lu.B, ld.B, gx - gxi), LinInterp(ru.B, rd.B, gx - gxi), gy - gyi);
                    
                    //нанесение пикселя
                    newImage.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }

        //вывод изображения
            Graphics g = pictureBox3.CreateGraphics();
            g.Clear(Color.White);

            newImage.Save("image_bilinear.png", ImageFormat.Png);
            MessageBox.Show("Успешно произведена коррекция изображения методом Билинейного сглаживания");

            g.DrawImage(newImage, 0, 0, newWidth, newHeight);
        }

        //открыть изображение
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            //открытие диалога и запись пути к изображению
            openFileDialogImage.ShowDialog();
            imagePath = openFileDialogImage.FileName;

            if (imagePath == "")
                isOpen = false;
            else
                isOpen = true;

            //при успешном открытии изображения выводим его на экран
            if (isOpen == true)
            {
                currImage = new Bitmap(imagePath);
                showImage(pictureBox1);
            }
            else
                MessageBox.Show("Выберите изображение!");
        }
    }
}
