using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ControlsTestApp
{
    public partial class Form2 : Form
    {
        GraphicsPath _Path;
        float _Zoom;
        public Form2()
        {
            InitializeComponent();
            _Path = new GraphicsPath();
            _Path.AddRectangle(new Rectangle(10, 10, 20, 20));
            Log("INIT");
            _Zoom = 1f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Zoom(2f);
        }

        void Log(string Transform)
        {
            listBox1.Items.Add("After '" + Transform + "':");
            foreach (var pt in _Path.PathPoints)
                listBox1.Items.Add(pt.X + " / " + pt.Y);
        }

        private void bZoom3_Click(object sender, EventArgs e)
        {
            Zoom(3f);
        }

        private void Zoom(float fact)
        {
            if (fact == _Zoom)
            {
                Log("Ignored as identical");
                return;
            }

            var newfact = (1 / _Zoom) * fact;
            _Zoom = fact;
            using (var mx = new Matrix(newfact, 0f, 0f, newfact, 0f, 0f))
            {
                _Path.Transform(mx);
            }
            Log("Zoom " + fact);
        }

        private void bZoom1_Click(object sender, EventArgs e)
        {
            Zoom(1f);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            annotationPanel1.Shapes.Add(new Tiveria.Common.Controls.Annotations.Shapes.Rectangle(10, 10, 50, 25));

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            annotationPanel1.Zoom = (float)numericUpDown1.Value;
        }

        private void bInvalidate_Click(object sender, EventArgs e)
        {
            annotationPanel1.Invalidate();
        }
    }
}
