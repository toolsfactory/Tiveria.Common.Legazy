namespace ControlsTestApp
{
    partial class Form2
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
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.bZoom3 = new System.Windows.Forms.Button();
            this.bZoom1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.bInvalidate = new System.Windows.Forms.Button();
            this.annotationPanel1 = new Tiveria.Common.Controls.AnnotationPanel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(96, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Zoom 2x";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(16, 64);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(155, 355);
            this.listBox1.TabIndex = 1;
            // 
            // bZoom3
            // 
            this.bZoom3.Location = new System.Drawing.Point(177, 21);
            this.bZoom3.Name = "bZoom3";
            this.bZoom3.Size = new System.Drawing.Size(75, 23);
            this.bZoom3.TabIndex = 2;
            this.bZoom3.Text = "Zoom 3x";
            this.bZoom3.UseVisualStyleBackColor = true;
            this.bZoom3.Click += new System.EventHandler(this.bZoom3_Click);
            // 
            // bZoom1
            // 
            this.bZoom1.Location = new System.Drawing.Point(16, 21);
            this.bZoom1.Name = "bZoom1";
            this.bZoom1.Size = new System.Drawing.Size(75, 23);
            this.bZoom1.TabIndex = 3;
            this.bZoom1.Text = "Zoom 1x";
            this.bZoom1.UseVisualStyleBackColor = true;
            this.bZoom1.Click += new System.EventHandler(this.bZoom1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(345, 21);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Add Rectangle";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown1.Location = new System.Drawing.Point(441, 24);
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
            this.numericUpDown1.Size = new System.Drawing.Size(65, 20);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // bInvalidate
            // 
            this.bInvalidate.Location = new System.Drawing.Point(512, 21);
            this.bInvalidate.Name = "bInvalidate";
            this.bInvalidate.Size = new System.Drawing.Size(75, 23);
            this.bInvalidate.TabIndex = 7;
            this.bInvalidate.Text = "Invalidate";
            this.bInvalidate.UseVisualStyleBackColor = true;
            this.bInvalidate.Click += new System.EventHandler(this.bInvalidate_Click);
            // 
            // annotationPanel1
            // 
            this.annotationPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.annotationPanel1.AutoScroll = true;
            this.annotationPanel1.AutoScrollMinSize = new System.Drawing.Size(1024, 768);
            this.annotationPanel1.AutoZoom = false;
            this.annotationPanel1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.annotationPanel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.annotationPanel1.Image = global::ControlsTestApp.Properties.Resources.Penguins;
            this.annotationPanel1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            this.annotationPanel1.Location = new System.Drawing.Point(177, 64);
            this.annotationPanel1.Name = "annotationPanel1";
            this.annotationPanel1.Size = new System.Drawing.Size(476, 365);
            this.annotationPanel1.TabIndex = 4;
            this.annotationPanel1.Text = "annotationPanel1";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 441);
            this.Controls.Add(this.bInvalidate);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.annotationPanel1);
            this.Controls.Add(this.bZoom1);
            this.Controls.Add(this.bZoom3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form2";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button bZoom3;
        private System.Windows.Forms.Button bZoom1;
        private Tiveria.Common.Controls.AnnotationPanel annotationPanel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button bInvalidate;
    }
}