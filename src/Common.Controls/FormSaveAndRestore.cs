using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Drawing;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Tiveria.Common.Controls
{
    public partial class FormSaveAndRestore : Component
    {
        private static string _GlobalPath = "";

        private Form ParentForm { get { return _ContainerControl as Form; } }
        private ContainerControl _ContainerControl = null;
        public ContainerControl ContainerControl
        {
            get { return _ContainerControl; }
            set
            {
                _ContainerControl = value;
                AttachEvents();
            }
        }
        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;
                if (value == null)
                {
                    return;
                }

                IDesignerHost host = value.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if (host != null)
                {
                    IComponent componentHost = host.RootComponent;
                    if (componentHost is ContainerControl)
                    {
                        ContainerControl = componentHost as ContainerControl;
                    }
                }
            }
        }
        public bool SaveSize { get; set; }

        [DefaultValue("Tiveria/SampleApp")]
        public string RegistryPath { get; set; }

        public FormSaveAndRestore()
        {
            InitializeComponent();
        }

        public FormSaveAndRestore(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

        }

        private void AttachEvents()
        {
            if (this.DesignMode || ParentForm == null) return;

            ParentForm.Load += _ContainerControl_Load;
            ParentForm.FormClosed += _ContainerControl_FormClosed;

        }

        void _ContainerControl_FormClosed(object sender, FormClosedEventArgs e)
        {
            StoreFormPosition();
        }

        void _ContainerControl_Load(object sender, EventArgs e)
        {
            SetGlobalPathFromFirstFormLoaded();
            SetRegistryPathFromGlobalIfEmpty();
            LoadFormPosition();
        }

        private void SetRegistryPathFromGlobalIfEmpty()
        {
            if (String.IsNullOrWhiteSpace(RegistryPath))
                RegistryPath = _GlobalPath;
        }

        private void SetGlobalPathFromFirstFormLoaded()
        {
            if (String.IsNullOrWhiteSpace(_GlobalPath))
                _GlobalPath = RegistryPath;
        }

        private void StoreFormPosition()
        {
            if (ParentForm.WindowState == FormWindowState.Minimized)
                return;

            RegistryKey key = Registry.CurrentUser.CreateSubKey(String.Format(@"Software\{0}\{1}", RegistryPath, _ContainerControl.Name));
            key.SetValue("X", ParentForm.DesktopBounds.X);
            key.SetValue("Y", ParentForm.DesktopBounds.Y);
            if (SaveSize)
            {
                key.SetValue("Width", ParentForm.DesktopBounds.Width);
                key.SetValue("Height", ParentForm.DesktopBounds.Height);
            }
            key.SetValue("Screen", Screen.FromControl(ParentForm).DeviceName);
        }

        private void LoadFormPosition()
        {
            int width = ParentForm.Width;
            int height = ParentForm.Height;
            RegistryKey key = Registry.CurrentUser.OpenSubKey(String.Format(@"Software\{0}\{1}", RegistryPath, _ContainerControl.Name));
            if (key == null)
                return;

            string screen = (string)key.GetValue("Screen", "");
            Screen found = Screen.AllScreens.Where(c => c.DeviceName == screen).FirstOrDefault();
            if (found == null)
                return;

            int x = (int)key.GetValue("X", ParentForm.DesktopBounds.X);
            int y = (int)key.GetValue("Y", ParentForm.DesktopBounds.Y);
            if (SaveSize)
            {
                width = (int)key.GetValue("Width", ParentForm.DesktopBounds.Width);
                height = (int)key.GetValue("Height", ParentForm.DesktopBounds.Height);
            }
            if (found.Bounds.Contains(x, y))
                ParentForm.DesktopBounds = new System.Drawing.Rectangle(x, y, width, height);
        }
    }
}
