using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphic_l5
{
    public partial class ProgramForm : Form
    {
        //изображение
        Bitmap currImage;
        string imagePath;
        int imWidth, imHeight;

        //максимальное значение столбика в гистограмме
        long maxHistScale = 0;
        long[] hist;

        //флаг наличия открытого изображения
        bool isOpen = false;

        //флаг изменения яркости
        bool isBrightChanged = true;
        //средняя яркость изображения
        double lAB = 0;
        float coefficient = 2;

        public ProgramForm()
        {
            InitializeComponent();
        }

        private void showImage()
        {
            Graphics graphic = pictureBox1.CreateGraphics();
            graphic.DrawImage(currImage, new Rectangle(0, 0, 600, 480));

            imWidth = currImage.Width;
            imHeight = currImage.Height;

            //получаем гистограмму
            hist = getHistogram();
            graphic = pictureBox2.CreateGraphics();
            graphic.Clear(Color.White);
            maxHistScale = hist.Max();


            coefficient = 1f;

            //и отрисовываем её
            drawHistogram(hist, graphic);
        }

        //получение массива значений гистрограммы
        private long[] getHistogram()
        {
            hist = new long[256];
            Color pixelColor;
            int i;
            //проходим каждый пиксель и вычисляем для него яркость
            for (int x = 0; x < imWidth; x++)
                for (int y = 0; y < imHeight; y++)
                {
                    pixelColor = currImage.GetPixel(x, y);
                    i = (int)((0.299 * pixelColor.R + 0.5876 * pixelColor.G + 0.114 * pixelColor.B));
                    hist[i]++;
                }

            return hist;
        }

        //отрисовка гистрограммы
        private void drawHistogram(long[] hist, Graphics graphic)
        {
            if(maxHistScale > 0)
            {
                double maxHist = maxHistScale;
                int height = pictureBox2.Height;
                int width = pictureBox2.Width;

                for (int i = 0; i < 256; i++)
                {
                    float h = height * hist[i] / (float)maxHist;
                    graphic.FillRectangle(Brushes.Green, i * width / 256.0f, height - h, width / 256.0f, h);
                }
                numericUpDownScale.Value = maxHistScale;
            }
        }

        //выбор изображения из проводника
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


        private void buttonHistogramScale_Click(object sender, EventArgs e)
        {
            if (numericUpDownScale.Value > 0 && numericUpDownScale.Value <= 1000000000)
            {
                maxHistScale = (int)numericUpDownScale.Value;
                if (hist != null)
                {
                    Graphics graphic = pictureBox2.CreateGraphics();
                    graphic.Clear(Color.White);
                    drawHistogram(hist, graphic);
                }
            }
            else
            {
                numericUpDownScale.Value = maxHistScale;
                MessageBox.Show("Выберите другое Scale");
            }
        }

        private void buttonBrightUp_Click(object sender, EventArgs e)
        {
            if (isOpen)
            {
                for (int i = 0; i < imWidth; i++)
                {
                    for (int j = 0; j < imHeight; j++)
                    {
                        //изменение пикселей
                        byte red, green, blue;
                        Color color = currImage.GetPixel(i, j);
                        if (((color.R + 10 * 128 / 100) > 255)) 
                            red = 255; 
                        else 
                            red = (byte)((color.R + 10 * 128 / 100));
                        if (((color.G + 10 * 128 / 100) > 255)) 
                            green = 255; 
                        else 
                            green = (byte)((color.G + 10 * 128 / 100));
                        if (((color.B + 10 * 128 / 100) > 255)) 
                            blue = 255; 
                        else 
                            blue = (byte)((color.B + 10 * 128 / 100));

                        //нанесение нового пикселя
                        currImage.SetPixel(i, j, Color.FromArgb(red, green, blue));
                    }
                }

                isBrightChanged = true;

                Graphics graphic = pictureBox1.CreateGraphics();
                graphic.DrawImage(currImage, new Rectangle(0, 0, 600, 480));

                //обновление гистограммы
                hist = getHistogram();
                graphic = pictureBox2.CreateGraphics();
                graphic.Clear(Color.White);
                drawHistogram(hist, graphic);
            }
        }

        private void buttonBrightDown_Click(object sender, EventArgs e)
        {
            if (isOpen)
            {
                for (int i = 0; i < imWidth; i++)
                {
                    for (int j = 0; j < imHeight; j++)
                    {
                        //изменение пикселей
                        byte red, green, blue;
                        Color color = currImage.GetPixel(i, j);
                        if (((color.R - 10 * 128 / 100) < 0))
                            red = 0;
                        else
                            red = (byte)((color.R - 10 * 128 / 100));
                        if (((color.G - 10 * 128 / 100) < 0))
                            green = 0;
                        else
                            green = (byte)((color.G - 10 * 128 / 100));
                        if (((color.B - 10 * 128 / 100) < 0))
                            blue = 0;
                        else
                            blue = (byte)((color.B - 10 * 128 / 100));

                        //нанесение нового пикселя
                        currImage.SetPixel(i, j, Color.FromArgb(red, green, blue));
                    }
                }
                isBrightChanged = true;

                Graphics graphic = pictureBox1.CreateGraphics();
                graphic.DrawImage(currImage, new Rectangle(0, 0, 600, 480));

                //обновление гистограммы
                hist = getHistogram();
                graphic = pictureBox2.CreateGraphics();
                graphic.Clear(Color.White);
                drawHistogram(hist, graphic);
            }
        }


        private void buttonContrastUp_Click(object sender, EventArgs e)
        {
            if (coefficient < 1)
                coefficient = 1.25f;
            else
                coefficient *= 2f;
            if (isOpen)
            {
                Color currColor;
                //рассчитываем среднюю яркость изображения
                if (isBrightChanged == true)
                {
                    isBrightChanged = false;

                    for (int i = 0; i < imWidth; i++)
                    {
                        for (int j = 0; j < imHeight; j++)
                        {
                            currColor = currImage.GetPixel(i, j);
                            lAB += currColor.R * 0.299 + currColor.G * 0.587 + currColor.B * 0.114;
                        }
                    }
                    lAB /= imHeight * imWidth;
                }


                //находим отклонения от среднего значения  яркости и вычисляем новые значения цветов
                int red, green, blue;

                for (int i = 0; i < imWidth; i++)
                {
                    for (int j = 0; j < imHeight; j++)
                    {
                        currColor = currImage.GetPixel(i, j);

                        red = (int)(coefficient * ((int)currColor.R - lAB) + lAB);
                        if (red < 0)
                            red = 0;
                        if (red > 255)
                            red = 255;

                        green = (int)(coefficient * ((int)currColor.G - lAB) + lAB);
                        if (green < 0)
                            green = 0;
                        if (green > 255)
                            green = 255;

                        blue = (int)(coefficient * ((int)currColor.B - lAB) + lAB);
                        if (blue < 0)
                            blue = 0;
                        if (blue > 255)
                            blue = 255;

                        //наносим пиксель
                        currImage.SetPixel(i, j, Color.FromArgb(red, green, blue));
                    }
                }
                Graphics graphic = pictureBox1.CreateGraphics();
                graphic.DrawImage(currImage, new Rectangle(0, 0, 600, 480));

                //обновление гистограммы
                hist = getHistogram();
                graphic = pictureBox2.CreateGraphics();
                graphic.Clear(Color.White);
                drawHistogram(hist, graphic);
            }
        }

        private void buttonContrastDown_Click(object sender, EventArgs e)
        {
            if (coefficient > 1)
                coefficient = 0.8f;
            else
                coefficient /= 2;
            if (isOpen)
            {
                Color currColor;
                //рассчитываем среднюю яркость изображения
                if (isBrightChanged == true)
                {  
                    isBrightChanged = false;

                    for (int i = 0; i < imWidth; i++)
                    {
                        for (int j = 0; j < imHeight; j++)
                        {
                            currColor = currImage.GetPixel(i, j);
                            lAB += currColor.R * 0.299 + currColor.G * 0.5876 + currColor.B * 0.114;
                        }

                    }
                    lAB /= imHeight * imWidth;
                }

                //находим отклонения от среднего значения  яркости и вычисляем новые значения цветов
                int red, green, blue;

                for (int i = 0; i < imWidth; i++)
                {
                    for (int j = 0; j < imHeight; j++)
                    {
                        currColor = currImage.GetPixel(i, j);

                        red = (int)(coefficient * ((int)currColor.R - lAB) + lAB);
                        if (red < 0)
                            red = 0;
                        if (red > 255)
                            red = 255;

                        green = (int)(coefficient * ((int)currColor.G - lAB) + lAB);
                        if (green < 0)
                            green = 0;
                        if (green > 255)
                            green = 255;

                        blue = (int)(coefficient * ((int)currColor.B - lAB) + lAB);
                        if (blue < 0)
                            blue = 0;
                        if (blue > 255)
                            blue = 255;

                        //наносим пиксель
                        currImage.SetPixel(i, j, Color.FromArgb(red, green, blue));
                    }
                }
                Graphics graphic = pictureBox1.CreateGraphics();
                graphic.DrawImage(currImage, new Rectangle(0, 0, 600, 480));

                //обновление гистограммы
                hist = getHistogram();
                graphic = pictureBox2.CreateGraphics();
                graphic.Clear(Color.White);
                drawHistogram(hist, graphic);
            }
        }
        
        //эффект оттенков серого
        private void buttonGreyShades_Click(object sender, EventArgs e)
        {
            if (isOpen)
            {
                int averageBright = 0;
                Color currColor;
                for (int i = 0; i < imWidth; i++)
                {
                    for (int j = 0; j < imHeight; j++)
                    {
                        currColor = currImage.GetPixel(i, j);
                        averageBright = (int)(currColor.R * 0.2126 + currColor.G * 0.7152 + currColor.B * 0.0722);
                        currImage.SetPixel(i, j, Color.FromArgb(averageBright, averageBright, averageBright));
                    }
                }
                isBrightChanged = true;
                Graphics graphic = pictureBox1.CreateGraphics();
                graphic.DrawImage(currImage, new Rectangle(0, 0, 600, 480));

                //обновление гистограммы
                hist = getHistogram();
                graphic = pictureBox2.CreateGraphics();
                graphic.Clear(Color.White);
                drawHistogram(hist, graphic);
            }
        }

        //эффект негатива
        private void buttonNegative_Click(object sender, EventArgs e)
        {
            if(isOpen)
            {
                byte red, green, blue;
                Color currColor;
                for(int i = 0; i < imWidth; i++)
                    for(int j = 0; j < imHeight; j++)
                    {
                        //замена кажжого канала на его дополнение
                        currColor = currImage.GetPixel(i, j);
                        red = (byte)(255 - currColor.R);
                        green = (byte)(255 - currColor.G);
                        blue = (byte)(255 - currColor.B);

                        //нанесение пикселя
                        currImage.SetPixel(i, j, Color.FromArgb(red, green, blue));
                    }

                isBrightChanged = true;
                Graphics graphic = pictureBox1.CreateGraphics();
                graphic.DrawImage(currImage, new Rectangle(0, 0, 600, 480));

                //обновление гистограммы
                hist = getHistogram();
                graphic = pictureBox2.CreateGraphics();
                graphic.Clear(Color.White);
                drawHistogram(hist, graphic);
            }
        }

        private void buttonBinarization_Click(object sender, EventArgs e)
        {
            if (isOpen)
            {
                int averageBright = 0;
                Color currColor;
                for (int i = 0; i < imWidth; i++)
                {
                    for (int j = 0; j < imHeight; j++)
                    {
                        currColor = currImage.GetPixel(i, j);
                        averageBright = (int)(currColor.R * 0.299 + currColor.G * 0.5876 + currColor.B * 0.114);
                        if (averageBright > 128)
                            currImage.SetPixel(i, j, Color.White);
                        else
                            currImage.SetPixel(i, j, Color.Black);
                    }
                }
                isBrightChanged = true;
                Graphics graphic = pictureBox1.CreateGraphics();
                graphic.DrawImage(currImage, new Rectangle(0, 0, 600, 480));

                //обновление гистограммы
                hist = getHistogram();
                graphic = pictureBox2.CreateGraphics();
                graphic.Clear(Color.White);
                drawHistogram(hist, graphic);
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (imagePath == "")
                isOpen = false;
            else
                isOpen = true;

            //при успешном открытии изображения выводим его на экран
            if (isOpen == true)
            {
                isBrightChanged = true;
                currImage = new Bitmap(imagePath);
                showImage();
            }
            else
                MessageBox.Show("Выберите изображение!");
        }
    }
}
