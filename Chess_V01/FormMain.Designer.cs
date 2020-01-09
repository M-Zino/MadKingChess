namespace Chess_V01
{
    partial class FormMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxDraw = new System.Windows.Forms.PictureBox();
            this.pictureBoxFrame = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDraw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxDraw
            // 
            this.pictureBoxDraw.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBoxDraw.BackgroundImage = global::Chess_V01.Properties.Resources.BGIn;
            this.pictureBoxDraw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxDraw.Location = new System.Drawing.Point(86, 77);
            this.pictureBoxDraw.Name = "pictureBoxDraw";
            this.pictureBoxDraw.Size = new System.Drawing.Size(600, 600);
            this.pictureBoxDraw.TabIndex = 1;
            this.pictureBoxDraw.TabStop = false;
            this.pictureBoxDraw.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxDraw_Paint);
            this.pictureBoxDraw.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDraw_MouseClick);
            // 
            // pictureBoxFrame
            // 
            this.pictureBoxFrame.BackgroundImage = global::Chess_V01.Properties.Resources.BGFrame;
            this.pictureBoxFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxFrame.Location = new System.Drawing.Point(12, 3);
            this.pictureBoxFrame.Name = "pictureBoxFrame";
            this.pictureBoxFrame.Size = new System.Drawing.Size(750, 750);
            this.pictureBoxFrame.TabIndex = 0;
            this.pictureBoxFrame.TabStop = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 753);
            this.Controls.Add(this.pictureBoxDraw);
            this.Controls.Add(this.pictureBoxFrame);
            this.Name = "FormMain";
            this.Text = "Chess";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDraw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFrame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxFrame;
        private System.Windows.Forms.PictureBox pictureBoxDraw;
    }
}

