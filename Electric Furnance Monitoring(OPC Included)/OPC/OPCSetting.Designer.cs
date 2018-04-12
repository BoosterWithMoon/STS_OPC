namespace Electric_Furnance_Monitoring_OPC_Included_
{
    partial class OPCSetting
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
            this.label_nodeName = new System.Windows.Forms.Label();
            this.label_Channel = new System.Windows.Forms.Label();
            this.label_Device = new System.Windows.Forms.Label();
            this.textBox_endpoint = new System.Windows.Forms.TextBox();
            this.textBox_channel = new System.Windows.Forms.TextBox();
            this.textBox_device = new System.Windows.Forms.TextBox();
            this.button_Accept = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_nodeName
            // 
            this.label_nodeName.AutoSize = true;
            this.label_nodeName.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_nodeName.Location = new System.Drawing.Point(15, 89);
            this.label_nodeName.Name = "label_nodeName";
            this.label_nodeName.Size = new System.Drawing.Size(86, 20);
            this.label_nodeName.TabIndex = 7;
            this.label_nodeName.Text = "NodeName";
            // 
            // label_Channel
            // 
            this.label_Channel.AutoSize = true;
            this.label_Channel.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Channel.Location = new System.Drawing.Point(23, 21);
            this.label_Channel.Name = "label_Channel";
            this.label_Channel.Size = new System.Drawing.Size(66, 20);
            this.label_Channel.TabIndex = 5;
            this.label_Channel.Text = "Channel";
            // 
            // label_Device
            // 
            this.label_Device.AutoSize = true;
            this.label_Device.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Device.Location = new System.Drawing.Point(27, 55);
            this.label_Device.Name = "label_Device";
            this.label_Device.Size = new System.Drawing.Size(54, 20);
            this.label_Device.TabIndex = 6;
            this.label_Device.Text = "Device";
            // 
            // textBox_endpoint
            // 
            this.textBox_endpoint.Location = new System.Drawing.Point(119, 89);
            this.textBox_endpoint.Name = "textBox_endpoint";
            this.textBox_endpoint.Size = new System.Drawing.Size(246, 21);
            this.textBox_endpoint.TabIndex = 2;
            // 
            // textBox_channel
            // 
            this.textBox_channel.Location = new System.Drawing.Point(119, 22);
            this.textBox_channel.Name = "textBox_channel";
            this.textBox_channel.Size = new System.Drawing.Size(246, 21);
            this.textBox_channel.TabIndex = 0;
            // 
            // textBox_device
            // 
            this.textBox_device.Location = new System.Drawing.Point(119, 55);
            this.textBox_device.Name = "textBox_device";
            this.textBox_device.Size = new System.Drawing.Size(246, 21);
            this.textBox_device.TabIndex = 1;
            // 
            // button_Accept
            // 
            this.button_Accept.Location = new System.Drawing.Point(209, 121);
            this.button_Accept.Name = "button_Accept";
            this.button_Accept.Size = new System.Drawing.Size(75, 23);
            this.button_Accept.TabIndex = 3;
            this.button_Accept.Text = "Accept";
            this.button_Accept.UseVisualStyleBackColor = true;
            this.button_Accept.Click += new System.EventHandler(this.button_Accept_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(290, 121);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 4;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // OPCSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 159);
            this.ControlBox = false;
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Accept);
            this.Controls.Add(this.textBox_device);
            this.Controls.Add(this.textBox_channel);
            this.Controls.Add(this.textBox_endpoint);
            this.Controls.Add(this.label_Device);
            this.Controls.Add(this.label_Channel);
            this.Controls.Add(this.label_nodeName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OPCSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OPCSetting";
            this.Load += new System.EventHandler(this.OPCSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label label_nodeName;
        public System.Windows.Forms.Label label_Channel;
        public System.Windows.Forms.Label label_Device;
        public System.Windows.Forms.TextBox textBox_endpoint;
        public System.Windows.Forms.TextBox textBox_channel;
        public System.Windows.Forms.TextBox textBox_device;
        public System.Windows.Forms.Button button_Accept;
        public System.Windows.Forms.Button button_Cancel;
    }
}