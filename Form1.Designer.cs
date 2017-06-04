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
            this.btnBRPToTIU = new System.Windows.Forms.Button();
            this.tbLoginNethause = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbPassNethause = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbPassTIU = new System.Windows.Forms.TextBox();
            this.tbLoginTIU = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.btn_loadTovars.Click += new System.EventHandler(this.btn_loadTovars_Click);
            // 
            // btnBRPToTIU
            // 
            this.btnBRPToTIU.Location = new System.Drawing.Point(482, 108);
            this.btnBRPToTIU.Name = "btnBRPToTIU";
            this.btnBRPToTIU.Size = new System.Drawing.Size(126, 23);
            this.btnBRPToTIU.TabIndex = 2;
            this.btnBRPToTIU.Text = "BRP";
            this.btnBRPToTIU.UseVisualStyleBackColor = true;
            this.btnBRPToTIU.Click += new System.EventHandler(this.btnBRPToTIU_Click);
            // 
            // tbLoginNethause
            // 
            this.tbLoginNethause.Location = new System.Drawing.Point(6, 19);
            this.tbLoginNethause.Name = "tbLoginNethause";
            this.tbLoginNethause.Size = new System.Drawing.Size(100, 20);
            this.tbLoginNethause.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbPassNethause);
            this.groupBox1.Controls.Add(this.tbLoginNethause);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(219, 51);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Nethouse";
            // 
            // tbPassNethause
            // 
            this.tbPassNethause.Location = new System.Drawing.Point(112, 19);
            this.tbPassNethause.Name = "tbPassNethause";
            this.tbPassNethause.PasswordChar = '*';
            this.tbPassNethause.Size = new System.Drawing.Size(100, 20);
            this.tbPassNethause.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbPassTIU);
            this.groupBox2.Controls.Add(this.tbLoginTIU);
            this.groupBox2.Location = new System.Drawing.Point(12, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(219, 51);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "tiu";
            // 
            // tbPassTIU
            // 
            this.tbPassTIU.Location = new System.Drawing.Point(112, 19);
            this.tbPassTIU.Name = "tbPassTIU";
            this.tbPassTIU.PasswordChar = '*';
            this.tbPassTIU.Size = new System.Drawing.Size(100, 20);
            this.tbPassTIU.TabIndex = 4;
            // 
            // tbLoginTIU
            // 
            this.tbLoginTIU.Location = new System.Drawing.Point(6, 19);
            this.tbLoginTIU.Name = "tbLoginTIU";
            this.tbLoginTIU.Size = new System.Drawing.Size(100, 20);
            this.tbLoginTIU.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 311);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnBRPToTIU);
            this.Controls.Add(this.btn_loadTovars);
            this.Controls.Add(this.btnLoadGroup);
            this.Name = "Form1";
            this.Text = "Загрузить товары на ТИУ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadGroup;
        private System.Windows.Forms.Button btn_loadTovars;
        private System.Windows.Forms.Button btnBRPToTIU;
        private System.Windows.Forms.TextBox tbLoginNethause;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbPassNethause;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbPassTIU;
        private System.Windows.Forms.TextBox tbLoginTIU;
    }
}

