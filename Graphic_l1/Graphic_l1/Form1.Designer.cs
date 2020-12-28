namespace Graphic_l1
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.drawBox = new System.Windows.Forms.PictureBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.timerChangeObj = new System.Windows.Forms.Timer(this.components);
            this.buttonAnimationCntrl = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.drawBox)).BeginInit();
            this.SuspendLayout();
            // 
            // drawBox
            // 
            this.drawBox.BackColor = System.Drawing.SystemColors.Info;
            this.drawBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.drawBox.Enabled = false;
            this.drawBox.Location = new System.Drawing.Point(22, 27);
            this.drawBox.Name = "drawBox";
            this.drawBox.Size = new System.Drawing.Size(350, 350);
            this.drawBox.TabIndex = 0;
            this.drawBox.TabStop = false;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(141, 202);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Начать";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(378, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(254, 166);
            this.label2.TabIndex = 3;
            this.label2.Text = resources.GetString("label2.Text");
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timerChangeObj
            // 
            this.timerChangeObj.Tick += new System.EventHandler(this.timerChangeObj_Tick);
            // 
            // buttonAnimationCntrl
            // 
            this.buttonAnimationCntrl.Location = new System.Drawing.Point(429, 331);
            this.buttonAnimationCntrl.Name = "buttonAnimationCntrl";
            this.buttonAnimationCntrl.Size = new System.Drawing.Size(135, 46);
            this.buttonAnimationCntrl.TabIndex = 4;
            this.buttonAnimationCntrl.Text = "Запустить/Остановить анимацию";
            this.buttonAnimationCntrl.UseVisualStyleBackColor = true;
            this.buttonAnimationCntrl.Click += new System.EventHandler(this.buttonAnimationCntrl_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(429, 27);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 5;
            this.buttonReset.Text = "Сбросить";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 401);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonAnimationCntrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.drawBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Graphic 1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.drawBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox drawBox;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timerChangeObj;
        private System.Windows.Forms.Button buttonAnimationCntrl;
        private System.Windows.Forms.Button buttonReset;
    }
}

