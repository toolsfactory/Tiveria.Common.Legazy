using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlsTestApp
{
    public partial class MultiRegionsSelectForm : Form
    {
        public MultiRegionsSelectForm()
        {
            InitializeComponent();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            multiSelectRegionMap1.Zoom = (float)numericUpDown1.Value;
        }

        private void multiSelectRegionMap1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                multiSelectRegionMap1.DeleteSelectedRegion();
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            multiSelectRegionMap1.DeleteSelectedRegion();
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            multiSelectRegionMap1.AddRegion(new Rectangle(10, 10, 200, 50));
        }

        private void bInvalidate_Click(object sender, EventArgs e)
        {
            multiSelectRegionMap1.Invalidate();
        }
    }
}
