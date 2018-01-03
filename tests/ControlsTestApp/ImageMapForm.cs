using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControlsTestApp
{
    public partial class ImageMapForm : Form
    {
        public ImageMapForm()
        {
            InitializeComponent();
            imageMap1.AddRectangle("Rectangle", 140, 20, 280, 60);
            imageMap1.AddElipse("Ellipse", 80, 100, 60);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            imageMap1.HighlightRegion("Ellipse", 100);

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            imageMap1.Zoom = (float)numericUpDown1.Value;
        }

        private void cbShowRegions_CheckedChanged(object sender, EventArgs e)
        {
            imageMap1.ShowRegions = cbShowRegions.Checked;
            imageMap1.Refresh();
        }

        private void cbAutoZoom_CheckedChanged(object sender, EventArgs e)
        {
            imageMap1.AutoZoom = cbAutoZoom.Checked;
        }

        private void cbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            imageMap1.Enabled = cbEnabled.Checked;
        }
    }
}
