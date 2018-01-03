using System;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;

namespace MVVMDemo
{
    public partial class MainForm : Form
    {
        Sample sourceObject;

        public MainForm()
        {
            InitializeComponent();
            sourceObject = new Sample();
            textBox1.DataBindings.Add("Text", sourceObject, "FirstName");
            textBox1.DataBindings.Add("Enabled", sourceObject, "Enabled");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var dlg = new NewPersonPresenter())
            {
                if (dlg.ShowNewPersonDialog())
                    label1.Text = dlg.Person.FullName;
                else
                    label1.Text = "Cancelled";
            }
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            sourceObject.FirstName = "Stack";
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
//            (new test(sourceObject)).Show();
        }

    }

    public class Sample : INotifyPropertyChanged
    {
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                InvokePropertyChanged(new PropertyChangedEventArgs("FirstName"));
            }
        }

        private bool myVar;

        public bool Enabled
        {
            get { return myVar; }
            set { myVar = value; InvokePropertyChanged(new PropertyChangedEventArgs("Enabled")); }
        }


        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }

        #endregion
    }

}
