using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tiveria.Common.Subversion;

namespace SimpeTestApp
{
    public partial class Form1 : Form
    {
        AsyncSvnTask task;

        public Form1()
        {
            InitializeComponent();
            task = new AsyncSvnTask("c:\\temp\\test\\", "https://dev.tiveria.de/svn/testwizard/trunk/Applications");
            task.SetSecurityCredentials("mgeissler", "ahNgeev4");
            task.TaskFinished += task_TaskFinished;
        }

        void task_TaskFinished(object sender, TaskFinishedEventArgs e)
        {
            ExecuteThreadSafeAndAsync(() =>
            {
                log.AppendText(String.Format("Callback received: {0}\r\n", e.Success));
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            task.UpdateToRepositoryRevision(555);
            log.AppendText("Task started\r\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            task.Cancel();
            log.AppendText("Cancel pressed\r\n");
        }


        public void ExecuteThreadSafeAndAsync(Action action)
        {
            if (InvokeRequired)
                BeginInvoke(action);
            else
                action.Invoke();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            task.LookIntoTask();
        }

    }
}
