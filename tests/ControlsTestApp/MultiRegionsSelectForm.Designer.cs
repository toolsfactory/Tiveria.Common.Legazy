namespace ControlsTestApp
{
    partial class MultiRegionsSelectForm
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
            this.bInvalidate = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.bDelete = new System.Windows.Forms.Button();
            this.bAdd = new System.Windows.Forms.Button();
            this.multiSelectRegionMap1 = new Tiveria.Common.Controls.MultiSelectRegionMap();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // bInvalidate
            // 
            this.bInvalidate.Location = new System.Drawing.Point(194, 9);
            this.bInvalidate.Name = "bInvalidate";
            this.bInvalidate.Size = new System.Drawing.Size(75, 23);
            this.bInvalidate.TabIndex = 9;
            this.bInvalidate.Text = "Invalidate";
            this.bInvalidate.UseVisualStyleBackColor = true;
            this.bInvalidate.Click += new System.EventHandler(this.bInvalidate_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown1.Location = new System.Drawing.Point(123, 12);
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
            this.numericUpDown1.TabIndex = 8;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // bDelete
            // 
            this.bDelete.Location = new System.Drawing.Point(12, 9);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(75, 23);
            this.bDelete.TabIndex = 11;
            this.bDelete.Text = "Delete";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bAdd
            // 
            this.bAdd.Location = new System.Drawing.Point(275, 9);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(75, 23);
            this.bAdd.TabIndex = 12;
            this.bAdd.Text = "Add";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // multiSelectRegionMap1
            // 
            this.multiSelectRegionMap1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.multiSelectRegionMap1.AutoScroll = true;
            this.multiSelectRegionMap1.AutoScrollMinSize = new System.Drawing.Size(1024, 768);
            this.multiSelectRegionMap1.AutoZoom = false;
            this.multiSelectRegionMap1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.multiSelectRegionMap1.Image = global::ControlsTestApp.Properties.Resources.Penguins;
            this.multiSelectRegionMap1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.multiSelectRegionMap1.Location = new System.Drawing.Point(12, 38);
            this.multiSelectRegionMap1.Name = "multiSelectRegionMap1";
            this.multiSelectRegionMap1.Size = new System.Drawing.Size(874, 660);
            this.multiSelectRegionMap1.TabIndex = 10;
            this.multiSelectRegionMap1.Text = "multiSelectRegionMap1";
            this.multiSelectRegionMap1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.multiSelectRegionMap1_KeyUp);
            // 
            // MultiRegionsSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 710);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.bDelete);
            this.Controls.Add(this.multiSelectRegionMap1);
            this.Controls.Add(this.bInvalidate);
            this.Controls.Add(this.numericUpDown1);
            this.Name = "MultiRegionsSelectForm";
            this.Text = "MultiRegionsSelectForm";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bInvalidate;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private Tiveria.Common.Controls.MultiSelectRegionMap multiSelectRegionMap1;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Button bAdd;
    }
}