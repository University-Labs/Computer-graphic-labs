namespace Graphic_l6
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
            this.openFileDialogImage = new System.Windows.Forms.OpenFileDialog();
            this.buttonAddNoise = new System.Windows.Forms.Button();
            this.buttonUniformFilter = new System.Windows.Forms.Button();
            this.buttonMedianFilter = new System.Windows.Forms.Button();
            this.buttonSharpness = new System.Windows.Forms.Button();
            this.buttonWave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(12, 39);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(600, 480);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(53, 12);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(138, 23);
            this.buttonOpen.TabIndex = 2;
            this.buttonOpen.Text = "Открыть изображение";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(422, 12);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(138, 23);
            this.buttonReset.TabIndex = 3;
            this.buttonReset.Text = "Сбросить эффекты";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // openFileDialogImage
            // 
            this.openFileDialogImage.FileName = "Choose your image";
            // 
            // buttonAddNoise
            // 
            this.buttonAddNoise.Location = new System.Drawing.Point(720, 59);
            this.buttonAddNoise.Name = "buttonAddNoise";
            this.buttonAddNoise.Size = new System.Drawing.Size(99, 23);
            this.buttonAddNoise.TabIndex = 4;
            this.buttonAddNoise.Text = "Добавить шум";
            this.buttonAddNoise.UseVisualStyleBackColor = true;
            this.buttonAddNoise.Click += new System.EventHandler(this.buttonAddNoise_Click);
            // 
            // buttonUniformFilter
            // 
            this.buttonUniformFilter.Location = new System.Drawing.Point(679, 100);
            this.buttonUniformFilter.Name = "buttonUniformFilter";
            this.buttonUniformFilter.Size = new System.Drawing.Size(94, 35);
            this.buttonUniformFilter.TabIndex = 5;
            this.buttonUniformFilter.Text = "Равномерный фильтр";
            this.buttonUniformFilter.UseVisualStyleBackColor = true;
            this.buttonUniformFilter.Click += new System.EventHandler(this.buttonUniformFilter_Click);
            // 
            // buttonMedianFilter
            // 
            this.buttonMedianFilter.Location = new System.Drawing.Point(769, 100);
            this.buttonMedianFilter.Name = "buttonMedianFilter";
            this.buttonMedianFilter.Size = new System.Drawing.Size(94, 35);
            this.buttonMedianFilter.TabIndex = 6;
            this.buttonMedianFilter.Text = "Медианный фильтр";
            this.buttonMedianFilter.UseVisualStyleBackColor = true;
            this.buttonMedianFilter.Click += new System.EventHandler(this.buttonMedianFilter_Click);
            // 
            // buttonSharpness
            // 
            this.buttonSharpness.Location = new System.Drawing.Point(729, 153);
            this.buttonSharpness.Name = "buttonSharpness";
            this.buttonSharpness.Size = new System.Drawing.Size(75, 34);
            this.buttonSharpness.TabIndex = 7;
            this.buttonSharpness.Text = "Добавить резкости";
            this.buttonSharpness.UseVisualStyleBackColor = true;
            this.buttonSharpness.Click += new System.EventHandler(this.buttonSharpness_Click);
            // 
            // buttonWave
            // 
            this.buttonWave.Location = new System.Drawing.Point(720, 193);
            this.buttonWave.Name = "buttonWave";
            this.buttonWave.Size = new System.Drawing.Size(99, 23);
            this.buttonWave.TabIndex = 8;
            this.buttonWave.Text = "Акварелизация";
            this.buttonWave.UseVisualStyleBackColor = true;
            this.buttonWave.Click += new System.EventHandler(this.buttonWave_Click);
            // 
            // ProgramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(984, 531);
            this.Controls.Add(this.buttonWave);
            this.Controls.Add(this.buttonSharpness);
            this.Controls.Add(this.buttonMedianFilter);
            this.Controls.Add(this.buttonUniformFilter);
            this.Controls.Add(this.buttonAddNoise);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProgramForm";
            this.Text = "Graphic Lab6";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.OpenFileDialog openFileDialogImage;
        private System.Windows.Forms.Button buttonAddNoise;
        private System.Windows.Forms.Button buttonUniformFilter;
        private System.Windows.Forms.Button buttonMedianFilter;
        private System.Windows.Forms.Button buttonSharpness;
        private System.Windows.Forms.Button buttonWave;
    }
}

