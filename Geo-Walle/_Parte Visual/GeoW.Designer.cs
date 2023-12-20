namespace Geo_Walle
{
    partial class GeoW
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeoW));
            this.txtBox_codigo = new System.Windows.Forms.TextBox();
            this.Lienzo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_volver = new System.Windows.Forms.Button();
            this.btn_run = new System.Windows.Forms.Button();
            this.btn_reset = new System.Windows.Forms.Button();
            this.btn_arriba = new System.Windows.Forms.Button();
            this.btn_abajo = new System.Windows.Forms.Button();
            this.btn_derecha = new System.Windows.Forms.Button();
            this.btn_izquierda = new System.Windows.Forms.Button();
            this.Error_Box = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Lienzo)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBox_codigo
            // 
            this.txtBox_codigo.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtBox_codigo.Location = new System.Drawing.Point(73, 75);
            this.txtBox_codigo.Multiline = true;
            this.txtBox_codigo.Name = "txtBox_codigo";
            this.txtBox_codigo.Size = new System.Drawing.Size(292, 562);
            this.txtBox_codigo.TabIndex = 0;
            this.txtBox_codigo.TextChanged += new System.EventHandler(this.txtBox_codigo_TextChanged);
            // 
            // Lienzo
            // 
            this.Lienzo.BackColor = System.Drawing.Color.Gray;
            this.Lienzo.Location = new System.Drawing.Point(575, 75);
            this.Lienzo.Name = "Lienzo";
            this.Lienzo.Size = new System.Drawing.Size(1138, 562);
            this.Lienzo.TabIndex = 1;
            this.Lienzo.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(83, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(248, 35);
            this.label1.TabIndex = 2;
            this.label1.Text = "Escriba su código:";
            // 
            // btn_volver
            // 
            this.btn_volver.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btn_volver.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_volver.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_volver.ForeColor = System.Drawing.Color.Black;
            this.btn_volver.Location = new System.Drawing.Point(1758, 972);
            this.btn_volver.Name = "btn_volver";
            this.btn_volver.Size = new System.Drawing.Size(127, 49);
            this.btn_volver.TabIndex = 3;
            this.btn_volver.Text = "VOLVER";
            this.btn_volver.UseVisualStyleBackColor = false;
            this.btn_volver.Click += new System.EventHandler(this.btn_volver_Click);
            // 
            // btn_run
            // 
            this.btn_run.BackColor = System.Drawing.Color.White;
            this.btn_run.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_run.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_run.ForeColor = System.Drawing.Color.Black;
            this.btn_run.Location = new System.Drawing.Point(993, 774);
            this.btn_run.Name = "btn_run";
            this.btn_run.Size = new System.Drawing.Size(216, 58);
            this.btn_run.TabIndex = 4;
            this.btn_run.Text = "Dotnet Run";
            this.btn_run.UseVisualStyleBackColor = false;
            this.btn_run.Click += new System.EventHandler(this.btn_run_Click);
            // 
            // btn_reset
            // 
            this.btn_reset.BackColor = System.Drawing.Color.White;
            this.btn_reset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_reset.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_reset.ForeColor = System.Drawing.Color.Black;
            this.btn_reset.Location = new System.Drawing.Point(717, 774);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(216, 58);
            this.btn_reset.TabIndex = 6;
            this.btn_reset.Text = "RESET";
            this.btn_reset.UseVisualStyleBackColor = false;
            this.btn_reset.Click += new System.EventHandler(this.Btn_reset_Click);
            // 
            // btn_arriba
            // 
            this.btn_arriba.BackColor = System.Drawing.Color.DodgerBlue;
            this.btn_arriba.BackgroundImage = global::Geo_Walle.Properties.Resources.photo_2023_12_04_12_31_25;
            this.btn_arriba.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_arriba.Cursor = System.Windows.Forms.Cursors.PanNorth;
            this.btn_arriba.Location = new System.Drawing.Point(1518, 686);
            this.btn_arriba.Name = "btn_arriba";
            this.btn_arriba.Size = new System.Drawing.Size(65, 65);
            this.btn_arriba.TabIndex = 7;
            this.btn_arriba.UseVisualStyleBackColor = false;
            this.btn_arriba.Click += new System.EventHandler(this.btn_arriba_Click);
            // 
            // btn_abajo
            // 
            this.btn_abajo.BackColor = System.Drawing.Color.DodgerBlue;
            this.btn_abajo.BackgroundImage = global::Geo_Walle.Properties.Resources.photo_2023_12_04_12_31_06;
            this.btn_abajo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_abajo.Cursor = System.Windows.Forms.Cursors.PanSouth;
            this.btn_abajo.Location = new System.Drawing.Point(1518, 855);
            this.btn_abajo.Name = "btn_abajo";
            this.btn_abajo.Size = new System.Drawing.Size(65, 62);
            this.btn_abajo.TabIndex = 8;
            this.btn_abajo.UseVisualStyleBackColor = false;
            this.btn_abajo.Click += new System.EventHandler(this.btn_abajo_Click);
            // 
            // btn_derecha
            // 
            this.btn_derecha.BackColor = System.Drawing.Color.DodgerBlue;
            this.btn_derecha.BackgroundImage = global::Geo_Walle.Properties.Resources._5147733748861742228_120;
            this.btn_derecha.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_derecha.Cursor = System.Windows.Forms.Cursors.PanEast;
            this.btn_derecha.Location = new System.Drawing.Point(1608, 765);
            this.btn_derecha.Name = "btn_derecha";
            this.btn_derecha.Size = new System.Drawing.Size(65, 64);
            this.btn_derecha.TabIndex = 9;
            this.btn_derecha.UseVisualStyleBackColor = false;
            this.btn_derecha.Click += new System.EventHandler(this.btn_derecha_Click);
            // 
            // btn_izquierda
            // 
            this.btn_izquierda.BackColor = System.Drawing.Color.DodgerBlue;
            this.btn_izquierda.BackgroundImage = global::Geo_Walle.Properties.Resources.photo_2023_12_04_12_31_43;
            this.btn_izquierda.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_izquierda.Cursor = System.Windows.Forms.Cursors.PanWest;
            this.btn_izquierda.Location = new System.Drawing.Point(1434, 765);
            this.btn_izquierda.Name = "btn_izquierda";
            this.btn_izquierda.Size = new System.Drawing.Size(65, 64);
            this.btn_izquierda.TabIndex = 10;
            this.btn_izquierda.UseVisualStyleBackColor = false;
            this.btn_izquierda.Click += new System.EventHandler(this.btn_izquierda_Click);
            // 
            // Error_Box
            // 
            this.Error_Box.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Error_Box.Location = new System.Drawing.Point(73, 663);
            this.Error_Box.Multiline = true;
            this.Error_Box.Name = "Error_Box";
            this.Error_Box.Size = new System.Drawing.Size(292, 179);
            this.Error_Box.TabIndex = 11;
            // 
            // GeoW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.BackgroundImage = global::Geo_Walle.Properties.Resources.IMG_20231203_WA0039;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1897, 1046);
            this.Controls.Add(this.Error_Box);
            this.Controls.Add(this.btn_izquierda);
            this.Controls.Add(this.btn_derecha);
            this.Controls.Add(this.btn_abajo);
            this.Controls.Add(this.btn_arriba);
            this.Controls.Add(this.btn_reset);
            this.Controls.Add(this.btn_run);
            this.Controls.Add(this.btn_volver);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Lienzo);
            this.Controls.Add(this.txtBox_codigo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GeoW";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Geo_WallE";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.GeoW_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Lienzo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBox_codigo;
        private System.Windows.Forms.PictureBox Lienzo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_volver;
        private System.Windows.Forms.Button btn_run;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.Button btn_arriba;
        private System.Windows.Forms.Button btn_abajo;
        private System.Windows.Forms.Button btn_derecha;
        private System.Windows.Forms.Button btn_izquierda;
        private System.Windows.Forms.TextBox Error_Box;
    }
}