using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphic_l6
{
    public partial class ProgramForm : Form
    {
        //изображение
        Bitmap currImage;
        string imagePath;
        int imWidth, imHeight;

        //флаг наличия открытого изображения
        bool isOpen = false;



        private void showImage()
        {
            Graphics graphic = pictureBox1.CreateGraphics();
            graphic.DrawImage(currImage, new Rectangle(0, 0, 600, 480));

            imWidth = currImage.Width;
            imHeight = currImage.Height;

        }

        public ProgramForm()
        {
            InitializeComponent();
        }

        //сбросить изображение в стандартное
        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (imagePath == "")
                isOpen = false;
            else
                isOpen = true;

            //при успешном открытии изображения выводим его на экран
            if (isOpen == true)
            {
                currImage = new Bitmap(imagePath);
                showImage();
            }
            else
                MessageBox.Show("Выберите изображение!");
        }

        private void buttonAddNoise_Click(object sender, EventArgs e)
        {
            if(isOpen)
            {
                Graphics g = pictureBox1.CreateGraphics();
                g = Graphics.FromImage(currImage);

                //выбираем кисть для шума
                Pen noisePen = new Pen(Color.White, 1);
                Random rand = new Random();

                
                //количество шума в изображении
                int amountNoise = (imWidth * imHeight) / 100;
                
                /*
                int randVal;
                int newR, newG, newB;
                int randW, randH;
                Color r;
                //нанесение точек
                for (int i = 0; i < amountNoise; i++)
                {
                    randW = rand.Next(imWidth);
                    randH = rand.Next(imHeight);
                    r = currImage.GetPixel(randW, randH);
                    randVal = rand.Next(25);
                    newR = r.R + randVal;
                    if (newR > 255)
                        newR = 255;
                    newG = r.G + randVal;
                    if (newG > 255)
                        newG = 255;
                    newB = r.B + randVal;
                    if (newB > 255)
                        newB = 255;

                    g.DrawRectangle(new Pen(Color.FromArgb(newR, newG, newB), 1), randW, randH, 1, 1);
                }
                */
                
                
                Pen pen = new Pen(Color.White, 1);
                for (var i = 0; i < amountNoise; i++)
                {
                    g.DrawEllipse(pen, rand.Next(currImage.Width), rand.Next(currImage.Height), 1, 1);
                }


                //вывод изображения
                g = pictureBox1.CreateGraphics();
                g.DrawImage(currImage, new Rectangle(0, 0, 600, 480));

            }
        }

        private void buttonMedianFilter_Click(object sender, EventArgs e)
        {
            int appertSize = 3;
            Bitmap image1 = new Bitmap(imWidth + 2 * (appertSize / 2), imHeight + 2 * (appertSize / 2));
            Bitmap image2 = new Bitmap(imWidth, imHeight);

            Color color;
            for (int i = 0; i < imWidth; i++)
                for (int j = 0; j < imHeight; j++)
                {
                    color = currImage.GetPixel(i, j);
                    image1.SetPixel(i + (appertSize / 2), j + (appertSize / 2), color);
                }

            //fix up
            for (int i = appertSize / 2; i < imWidth + appertSize / 2; i++)
                for (int j = 0; j < appertSize / 2; j++)
                {
                    color = currImage.GetPixel(i - appertSize / 2, j + appertSize);
                    image1.SetPixel(i, j, color);
                }

            //fix down
            for (int i = appertSize / 2; i < imWidth + appertSize / 2; i++)
                for (int j = imHeight + appertSize / 2; j < imHeight + appertSize - 1; j++)
                {
                    color = currImage.GetPixel(i - appertSize / 2, j - appertSize);
                    image1.SetPixel(i, j, color);
                }

            //fix left
            for (int i = 0; i < appertSize / 2; i++)
                for (int j = 0; j < imHeight + appertSize - 1; j++)
                {
                    color = image1.GetPixel(i + appertSize / 2, j);
                    image1.SetPixel(i, j, color);
                }

            //fix right
            for (int i = imWidth + appertSize / 2; i < imWidth + appertSize - 1; i++)
                for (int j = 0; j < imHeight + appertSize - 1; j++)
                {
                    color = image1.GetPixel(i - appertSize / 2 - 1, j);
                    image1.SetPixel(i, j, color);
                }


            int index = 0;
            //последовательность которая подвергается сортировке
            Color[] mas = new Color[appertSize * appertSize];

            //перебор всех пикселей
            for (int i = appertSize / 2; i < imWidth + appertSize / 2; i++)
            {
                for (int j = appertSize / 2; j < imHeight + appertSize / 2; j++)
                {
                    //вычисление аппертуры для каждого из них
                    index = 0;
                    for (int x = -appertSize / 2; x < appertSize / 2 + 1; x++)
                        for (int y = -appertSize / 2; y < appertSize / 2 + 1; y++)
                            mas[index++] = image1.GetPixel(i + x, j + y);

                    //перекрашиваем пиксель в цвет среднего эл-та последовательности
                    image1.SetPixel(i, j, getMiddle(mas));
                }
            }


            //возвращение изображения к старым координатам - обрезание апертуры на краях
            for (int i = 0; i < imWidth; i++)
                for (int j = 0; j < imHeight; j++)
                {
                    color = image1.GetPixel(i + (appertSize / 2), j + (appertSize / 2));
                    image2.SetPixel(i, j, color);
                }

            Graphics g = pictureBox1.CreateGraphics();
            currImage = image2;
            g.DrawImage(currImage, new Rectangle(0, 0, 600, 480));
        }

        //сортировка и получение среднего элемента последовательности
        private Color getMiddle(Color[] e)
        {
            int size = e.Length;
            double[] brightness = new double[size];
            for (int i = 0; i < size; i++)
                brightness[i] = (0.299 * e[i].R + 0.5876 * e[i].G + 0.114 * e[i].B);
            Array.Sort(brightness, e);
            return e[size / 2];
        }
      
        //равномерное шумоподавление
        private void buttonUniformFilter_Click(object sender, EventArgs e)
        {
            double[] filter = new double[] {
                1.0f/9, 1.0f/9, 1.0f/9,  
                1.0f/9, 1.0f/9, 1.0f/9, 
                1.0f/9, 1.0f/9, 1.0f/9
            };
            performFilter(filter);
        }


        private void performFilter(double[] filter)
        {
            int appertSize = (int)Math.Sqrt(filter.Length);
            Bitmap image1 = new Bitmap(imWidth + 2 * (appertSize / 2), imHeight + 2 * (appertSize / 2));
            Bitmap image2 = new Bitmap(imWidth, imHeight);

            Color color;
            for (int i = 0; i < imWidth; i++)
                for (int j = 0; j < imHeight; j++)
                {
                    color = currImage.GetPixel(i, j);
                    image1.SetPixel(i + (appertSize / 2), j + (appertSize / 2), color);
                }

            //fix up
            for (int i = appertSize / 2; i < imWidth + appertSize / 2; i++)
                for (int j = 0; j < appertSize / 2; j++)
                {
                    color = currImage.GetPixel(i - appertSize / 2, j + appertSize);
                    image1.SetPixel(i, j, color);
                }

            //fix down
            for (int i = appertSize / 2; i < imWidth + appertSize / 2; i++)
                for (int j = imHeight + appertSize / 2; j < imHeight + appertSize - 1; j++)
                {
                    color = currImage.GetPixel(i - appertSize / 2, j - appertSize);
                    image1.SetPixel(i, j, color);
                }

            //fix left
            for (int i = 0; i < appertSize / 2; i++)
                for (int j = 0; j < imHeight + appertSize - 1; j++)
                {
                    color = image1.GetPixel(i + appertSize / 2, j);
                    image1.SetPixel(i, j, color);
                }

            //fix right
            for (int i = imWidth + appertSize / 2; i < imWidth + appertSize - 1; i++)
                for (int j = 0; j < imHeight + appertSize - 1; j++)
                {
                    color = image1.GetPixel(i - appertSize / 2 - 1, j);
                    image1.SetPixel(i, j, color);
                }


            int index = 0;
            double red, green, blue;
            double currAppert;

            //перебор всех пикселей
            for (int i = appertSize / 2; i < imWidth + appertSize / 2; i++)
            {
                for (int j = appertSize / 2; j < imHeight + appertSize / 2; j++)
                {
                    red = 0; green = 0; blue = 0;
                    //вычисление цвета для каждого из них
                    index = 0;
                    for (int x = -appertSize / 2; x < appertSize / 2 + 1; x++)
                        for (int y = -appertSize / 2; y < appertSize / 2 + 1; y++)
                        {
                            currAppert = filter[index++];
                            color = image1.GetPixel(i + x, j + y);
                            red += currAppert * color.R;
                            green += currAppert * color.G;
                            blue += currAppert * color.B;
                        }
                    if (red > 255) red = 255;
                    if (red < 0) red = 0;
                    if (green > 255) green = 255;
                    if (green < 0) green = 0;
                    if (blue > 255) blue = 255;
                    if (blue < 0) blue = 0;
                    //перекрашиваем пиксель в цвет среднего эл-та последовательности
                    image2.SetPixel(i - appertSize / 2, j - appertSize / 2, Color.FromArgb((int) red, (int) green, (int)blue));
                }
            }


            Graphics g = pictureBox1.CreateGraphics();
            currImage = image2;
            g.DrawImage(currImage, new Rectangle(0, 0, 600, 480));
        }

        //увеличение резкости
        private void buttonSharpness_Click(object sender, EventArgs e)
        {
            double[] filter = new double[] {
                -1.0f/4, -1.0f/4, -1.0f/4,
                -1.0f/4, 3.0f, -1.0f/4,
                -1.0f/4, -1.0f/4, -1.0f/4
            };
            performFilter(filter);
        }

        private void buttonWave_Click(object sender, EventArgs e)
        {
            buttonMedianFilter_Click(sender, e);
            buttonMedianFilter_Click(sender, e);
            buttonSharpness_Click(sender, e);
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
                showImage();
            }
            else
                MessageBox.Show("Выберите изображение!");
        }
    }
}
