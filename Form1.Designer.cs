namespace ConvertToTIU
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
            this.btnLoadGroup = new System.Windows.Forms.Button();
            this.btn_loadTovars = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoadGroup
            // 
            this.btnLoadGroup.Location = new System.Drawing.Point(482, 12);
            this.btnLoadGroup.Name = "btnLoadGroup";
            this.btnLoadGroup.Size = new System.Drawing.Size(126, 23);
            this.btnLoadGroup.TabIndex = 0;
            this.btnLoadGroup.Text = "Перенести группы";
            this.btnLoadGroup.UseVisualStyleBackColor = true;
            this.btnLoadGroup.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btn_loadTovars
            // 
            this.btn_loadTovars.Location = new System.Drawing.Point(482, 59);
            this.btn_loadTovars.Name = "btn_loadTovars";
            this.btn_loadTovars.Size = new System.Drawing.Size(125, 30);
            this.btn_loadTovars.TabIndex = 1;
            this.btn_loadTovars.Text = "Перенести товары";
            this.btn_loadTovars.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 311);
            this.Controls.Add(this.btn_loadTovars);
            this.Controls.Add(this.btnLoadGroup);
            this.Name = "Form1";
            this.Text = "Загрузить товары на ТИУ";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadGroup;
        private System.Windows.Forms.Button btn_loadTovars;
    }
}

