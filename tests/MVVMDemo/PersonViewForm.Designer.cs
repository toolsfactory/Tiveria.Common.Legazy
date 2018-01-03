namespace MVVMDemo
{
    partial class PersonViewForm
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
            this.components = new System.ComponentModel.Container();
            this.components = new System.ComponentModel.Container();
            this.eFirstname = new System.Windows.Forms.TextBox();
            this.eLastname = new System.Windows.Forms.TextBox();
            this.eBirthdate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lCombined = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.eEmail = new System.Windows.Forms.TextBox();
            this.bSave = new System.Windows.Forms.Button();
            this.ep = new System.Windows.Forms.ErrorProvider(this.components);
            this.bCancel = new System.Windows.Forms.Button();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ep)).BeginInit();
            this.SuspendLayout();
            // 
            // eFirstname
            // 
            this.eFirstname.Location = new System.Drawing.Point(80, 29);
            this.eFirstname.Name = "eFirstname";
            this.eFirstname.Size = new System.Drawing.Size(200, 20);
            this.eFirstname.TabIndex = 0;
            // 
            // eLastname
            // 
            this.eLastname.Location = new System.Drawing.Point(80, 55);
            this.eLastname.Name = "eLastname";
            this.eLastname.Size = new System.Drawing.Size(200, 20);
            this.eLastname.TabIndex = 1;
            // 
            // eBirthdate
            // 
            this.eBirthdate.Location = new System.Drawing.Point(80, 83);
            this.eBirthdate.Name = "eBirthdate";
            this.eBirthdate.Size = new System.Drawing.Size(200, 20);
            this.eBirthdate.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Firstname:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Lastname:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Birthdate:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 200);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Combined Name:";
            // 
            // lCombined
            // 
            this.lCombined.AutoSize = true;
            this.lCombined.Location = new System.Drawing.Point(107, 200);
            this.lCombined.Name = "lCombined";
            this.lCombined.Size = new System.Drawing.Size(16, 13);
            this.lCombined.TabIndex = 7;
            this.lCombined.Text = "   ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "eMail:";
            // 
            // eEmail
            // 
            this.eEmail.Location = new System.Drawing.Point(80, 109);
            this.eEmail.Name = "eEmail";
            this.eEmail.Size = new System.Drawing.Size(200, 20);
            this.eEmail.TabIndex = 8;
            // 
            // bSave
            // 
            this.bSave.Location = new System.Drawing.Point(289, 232);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(75, 23);
            this.bSave.TabIndex = 10;
            this.bSave.Text = "Save";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // ep
            // 
            this.ep.ContainerControl = this;
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(205, 232);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 11;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // cbEnabled
            // 
            this.cbEnabled.AutoSize = true;
            this.cbEnabled.Location = new System.Drawing.Point(25, 231);
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbEnabled.TabIndex = 12;
            this.cbEnabled.Text = "Enabled";
            this.cbEnabled.UseVisualStyleBackColor = true;
            this.cbEnabled.CheckedChanged += new System.EventHandler(this.cbEnabled_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(80, 227);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PersonViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 268);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbEnabled);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.eEmail);
            this.Controls.Add(this.lCombined);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.eBirthdate);
            this.Controls.Add(this.eLastname);
            this.Controls.Add(this.eFirstname);
            this.Name = "PersonViewForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.ep)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox eFirstname;
        private System.Windows.Forms.TextBox eLastname;
        private System.Windows.Forms.DateTimePicker eBirthdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lCombined;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox eEmail;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.ErrorProvider ep;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.CheckBox cbEnabled;
        private System.Windows.Forms.Button button1;
    }
}

