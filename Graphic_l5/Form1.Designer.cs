namespace Graphic_l5
{
    partial class ProgramForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonBrightUp = new System.Windows.Forms.Button();
            this.buttonBrightDown = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonContrastDown = new System.Windows.Forms.Button();
            this.buttonContrastUp = new System.Windows.Forms.Button();
            this.buttonBinarization = new System.Windows.Forms.Button();
            this.buttonGreyShades = new System.Windows.Forms.Button();
            this.buttonNegative = new System.Windows.Forms.Button();
            this.openFileDialogImage = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.numericUpDownScale = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonHistogramScale = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownScale)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(12, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(600, 480);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(49, 2);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(138, 23);
            this.buttonOpen.TabIndex = 1;
            this.buttonOpen.Text = "Открыть изображение";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(432, 2);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(138, 23);
            this.buttonReset.TabIndex = 2;
            this.buttonReset.Text = "Сбросить эффекты";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonBrightUp
            // 
            this.buttonBrightUp.Location = new System.Drawing.Point(711, 69);
            this.buttonBrightUp.Name = "buttonBrightUp";
            this.buttonBrightUp.Size = new System.Drawing.Size(75, 23);
            this.buttonBrightUp.TabIndex = 3;
            this.buttonBrightUp.Text = "Увеличить";
            this.buttonBrightUp.UseVisualStyleBackColor = true;
            this.buttonBrightUp.Click += new System.EventHandler(this.buttonBrightUp_Click);
            // 
            // buttonBrightDown
            // 
            this.buttonBrightDown.Location = new System.Drawing.Point(782, 69);
            this.buttonBrightDown.Name = "buttonBrightDown";
            this.buttonBrightDown.Size = new System.Drawing.Size(75, 23);
            this.buttonBrightDown.TabIndex = 4;
            this.buttonBrightDown.Text = "Уменьшить";
            this.buttonBrightDown.UseVisualStyleBackColor = true;
            this.buttonBrightDown.Click += new System.EventHandler(this.buttonBrightDown_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(722, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Яркость изображения";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(708, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Контрастность изображения";
            // 
            // buttonContrastDown
            // 
            this.buttonContrastDown.Location = new System.Drawing.Point(782, 145);
            this.buttonContrastDown.Name = "buttonContrastDown";
            this.buttonContrastDown.Size = new System.Drawing.Size(75, 23);
            this.buttonContrastDown.TabIndex = 8;
            this.buttonContrastDown.Text = "Уменьшить";
            this.buttonContrastDown.UseVisualStyleBackColor = true;
            this.buttonContrastDown.Click += new System.EventHandler(this.buttonContrastDown_Click);
            // 
            // buttonContrastUp
            // 
            this.buttonContrastUp.Location = new System.Drawing.Point(711, 145);
            this.buttonContrastUp.Name = "buttonContrastUp";
            this.buttonContrastUp.Size = new System.Drawing.Size(75, 23);
            this.buttonContrastUp.TabIndex = 7;
            this.buttonContrastUp.Text = "Увеличить";
            this.buttonContrastUp.UseVisualStyleBackColor = true;
            this.buttonContrastUp.Click += new System.EventHandler(this.buttonContrastUp_Click);
            // 
            // buttonBinarization
            // 
            this.buttonBinarization.Location = new System.Drawing.Point(663, 208);
            this.buttonBinarization.Name = "buttonBinarization";
            this.buttonBinarization.Size = new System.Drawing.Size(82, 23);
            this.buttonBinarization.TabIndex = 9;
            this.buttonBinarization.Text = "Бинаризация";
            this.buttonBinarization.UseVisualStyleBackColor = true;
            this.buttonBinarization.Click += new System.EventHandler(this.buttonBinarization_Click);
            // 
            // buttonGreyShades
            // 
            this.buttonGreyShades.Location = new System.Drawing.Point(742, 208);
            this.buttonGreyShades.Name = "buttonGreyShades";
            this.buttonGreyShades.Size = new System.Drawing.Size(103, 23);
            this.buttonGreyShades.TabIndex = 10;
            this.buttonGreyShades.Text = "Оттенки серого";
            this.buttonGreyShades.UseVisualStyleBackColor = true;
            this.buttonGreyShades.Click += new System.EventHandler(this.buttonGreyShades_Click);
            // 
            // buttonNegative
            // 
            this.buttonNegative.Location = new System.Drawing.Point(841, 208);
            this.buttonNegative.Name = "buttonNegative";
            this.buttonNegative.Size = new System.Drawing.Size(85, 23);
            this.buttonNegative.TabIndex = 11;
            this.buttonNegative.Text = "Негатив";
            this.buttonNegative.UseVisualStyleBackColor = true;
            this.buttonNegative.Click += new System.EventHandler(this.buttonNegative_Click);
            // 
            // openFileDialogImage
            // 
            this.openFileDialogImage.FileName = "Choose your image";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(628, 288);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(528, 225);
            this.pictureBox2.TabIndex = 12;
            this.pictureBox2.TabStop = false;
            // 
            // numericUpDownScale
            // 
            this.numericUpDownScale.Location = new System.Drawing.Point(849, 260);
            this.numericUpDownScale.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numericUpDownScale.Name = "numericUpDownScale";
            this.numericUpDownScale.Size = new System.Drawing.Size(72, 20);
            this.numericUpDownScale.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(634, 262);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Максимальное  значение гистограммы";
            // 
            // buttonHistogramScale
            // 
            this.buttonHistogramScale.Location = new System.Drawing.Point(928, 260);
            this.buttonHistogramScale.Name = "buttonHistogramScale";
            this.buttonHistogramScale.Size = new System.Drawing.Size(44, 23);
            this.buttonHistogramScale.TabIndex = 15;
            this.buttonHistogramScale.Text = "ОК";
            this.buttonHistogramScale.UseVisualStyleBackColor = true;
            this.buttonHistogramScale.Click += new System.EventHandler(this.buttonHistogramScale_Click);
            // 
            // ProgramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(1184, 531);
            this.Controls.Add(this.buttonHistogramScale);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownScale);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.buttonNegative);
            this.Controls.Add(this.buttonGreyShades);
            this.Controls.Add(this.buttonBinarization);
            this.Controls.Add(this.buttonContrastDown);
            this.Controls.Add(this.buttonContrastUp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonBrightDown);
            this.Controls.Add(this.buttonBrightUp);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProgramForm";
            this.Text = "Graphic Lab5";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownScale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonBrightUp;
        private System.Windows.Forms.Button buttonBrightDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonContrastDown;
        private System.Windows.Forms.Button buttonContrastUp;
        private System.Windows.Forms.Button buttonBinarization;
        private System.Windows.Forms.Button buttonGreyShades;
        private System.Windows.Forms.Button buttonNegative;
        private System.Windows.Forms.OpenFileDialog openFileDialogImage;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.NumericUpDown numericUpDownScale;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonHistogramScale;
    }
}

