namespace Geo_Walle
{
    partial class Inicio
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Inicio));
            this.label1 = new System.Windows.Forms.Label();
            this.btn_comenzar = new System.Windows.Forms.Button();
            this.btn_salir = new System.Windows.Forms.Button();
            this.btn_inst = new System.Windows.Forms.Button();
            this.btn_info = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(666, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(555, 194);
            this.label1.TabIndex = 0;
            this.label1.Text = " Wall-E";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_comenzar
            // 
            this.btn_comenzar.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btn_comenzar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_comenzar.Font = new System.Drawing.Font("Microsoft Tai Le", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_comenzar.ForeColor = System.Drawing.Color.Black;
            this.btn_comenzar.Location = new System.Drawing.Point(84, 635);
            this.btn_comenzar.Name = "btn_comenzar";
            this.btn_comenzar.Size = new System.Drawing.Size(217, 50);
            this.btn_comenzar.TabIndex = 1;
            this.btn_comenzar.Text = "COMENZAR";
            this.btn_comenzar.UseVisualStyleBackColor = false;
            this.btn_comenzar.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_salir
            // 
            this.btn_salir.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btn_salir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_salir.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_salir.ForeColor = System.Drawing.Color.Black;
            this.btn_salir.Location = new System.Drawing.Point(1758, 971);
            this.btn_salir.Name = "btn_salir";
            this.btn_salir.Size = new System.Drawing.Size(127, 49);
            this.btn_salir.TabIndex = 2;
            this.btn_salir.Text = "SALIR";
            this.btn_salir.UseVisualStyleBackColor = false;
            this.btn_salir.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_inst
            // 
            this.btn_inst.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btn_inst.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_inst.Font = new System.Drawing.Font("Microsoft Tai Le", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_inst.ForeColor = System.Drawing.Color.Black;
            this.btn_inst.Location = new System.Drawing.Point(84, 736);
            this.btn_inst.Name = "btn_inst";
            this.btn_inst.Size = new System.Drawing.Size(217, 50);
            this.btn_inst.TabIndex = 3;
            this.btn_inst.Text = "Instrucciones";
            this.btn_inst.UseVisualStyleBackColor = false;
            this.btn_inst.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btn_info
            // 
            this.btn_info.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btn_info.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_info.Font = new System.Drawing.Font("Microsoft Tai Le", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_info.ForeColor = System.Drawing.Color.Black;
            this.btn_info.Location = new System.Drawing.Point(84, 841);
            this.btn_info.Name = "btn_info";
            this.btn_info.Size = new System.Drawing.Size(217, 50);
            this.btn_info.TabIndex = 4;
            this.btn_info.Text = "Informe";
            this.btn_info.UseVisualStyleBackColor = false;
            this.btn_info.Click += new System.EventHandler(this.btn_info_Click);
            // 
            // Inicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.BackgroundImage = global::Geo_Walle.Properties.Resources.IMG_20231203_WA0039;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1897, 1046);
            this.Controls.Add(this.btn_info);
            this.Controls.Add(this.btn_inst);
            this.Controls.Add(this.btn_salir);
            this.Controls.Add(this.btn_comenzar);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Inicio";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Geo_WallE";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Inicio_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_comenzar;
        private System.Windows.Forms.Button btn_salir;
        private System.Windows.Forms.Button btn_inst;
        private System.Windows.Forms.Button btn_info;
    }
}

