namespace ControlsTestApp
{
    partial class ImageMapForm
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
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbShowRegions = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cbAutoZoom = new System.Windows.Forms.CheckBox();
            this.imageMap1 = new Tiveria.Common.Controls.ImageMap();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.numericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown1.Location = new System.Drawing.Point(0, 300);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(534, 20);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbEnabled);
            this.panel1.Controls.Add(this.cbAutoZoom);
            this.panel1.Controls.Add(this.cbShowRegions);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(534, 36);
            this.panel1.TabIndex = 3;
            // 
            // cbShowRegions
            // 
            this.cbShowRegions.AutoSize = true;
            this.cbShowRegions.Location = new System.Drawing.Point(118, 10);
            this.cbShowRegions.Name = "cbShowRegions";
            this.cbShowRegions.Size = new System.Drawing.Size(95, 17);
            this.cbShowRegions.TabIndex = 1;
            this.cbShowRegions.Text = "Show Regions";
            this.cbShowRegions.UseVisualStyleBackColor = true;
            this.cbShowRegions.CheckedChanged += new System.EventHandler(this.cbShowRegions_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Blink";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbAutoZoom
            // 
            this.cbAutoZoom.AutoSize = true;
            this.cbAutoZoom.Location = new System.Drawing.Point(220, 10);
            this.cbAutoZoom.Name = "cbAutoZoom";
            this.cbAutoZoom.Size = new System.Drawing.Size(75, 17);
            this.cbAutoZoom.TabIndex = 2;
            this.cbAutoZoom.Text = "AutoZoom";
            this.cbAutoZoom.UseVisualStyleBackColor = true;
            this.cbAutoZoom.CheckedChanged += new System.EventHandler(this.cbAutoZoom_CheckedChanged);
            // 
            // imageMap1
            // 
            this.imageMap1.AutoScroll = true;
            this.imageMap1.AutoScrollMinSize = new System.Drawing.Size(1024, 768);
            this.imageMap1.AutoZoom = false;
            this.imageMap1.BlinkBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.imageMap1.BlinkFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.imageMap1.ClickBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.imageMap1.ClickFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.imageMap1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageMap1.FillTransparency = ((byte)(128));
            this.imageMap1.HighlightBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.imageMap1.HighlightFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.imageMap1.Image = global::ControlsTestApp.Properties.Resources.Penguins;
            this.imageMap1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.imageMap1.Location = new System.Drawing.Point(0, 36);
            this.imageMap1.Name = "imageMap1";
            this.imageMap1.RegionCursor = System.Windows.Forms.Cursors.Hand;
            this.imageMap1.RegionsBorderColor = System.Drawing.Color.Yellow;
            this.imageMap1.ShowRegions = false;
            this.imageMap1.Size = new System.Drawing.Size(534, 264);
            this.imageMap1.TabIndex = 0;
            this.imageMap1.Text = "imageMap1";
            this.imageMap1.Zoom = 1F;
            // 
            // cbEnabled
            // 
            this.cbEnabled.AutoSize = true;
            this.cbEnabled.Location = new System.Drawing.Point(311, 10);
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbEnabled.TabIndex = 3;
            this.cbEnabled.Text = "Enabled";
            this.cbEnabled.UseVisualStyleBackColor = true;
            this.cbEnabled.CheckedChanged += new System.EventHandler(this.cbEnabled_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 320);
            this.Controls.Add(this.imageMap1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.numericUpDown1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Tiveria.Common.Controls.ImageMap imageMap1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cbShowRegions;
        private System.Windows.Forms.CheckBox cbAutoZoom;
        private System.Windows.Forms.CheckBox cbEnabled;
    }
}

