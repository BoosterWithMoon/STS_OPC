﻿namespace Electric_Furnance_Monitoring_OPC_Included_
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.OPC_textBox1 = new System.Windows.Forms.TextBox();
            this.OPC_textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newOnlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSimulationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openIRDXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataLoggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previousRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.rOIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawROIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveROIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteROIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label_CurrentSteelKind = new System.Windows.Forms.ToolStrip();
            this.새로만들기ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.열기ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.PreviousRecord_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.NextRecord_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.KeepMoving_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.Pause_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.DrawPOI_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.MovePOI_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.DeletePOI_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.LogStart_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.LogStop_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.도움말ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.split_ViewToInfo = new System.Windows.Forms.SplitContainer();
            this.split_CamToCam = new System.Windows.Forms.SplitContainer();
            this.split_CAM1UpToDown = new System.Windows.Forms.SplitContainer();
            this.split_CAM1Info = new System.Windows.Forms.SplitContainer();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.split_CAM1ChartGrid = new System.Windows.Forms.SplitContainer();
            this.split_CAM2UpToDown = new System.Windows.Forms.SplitContainer();
            this.split_CAM2Info = new System.Windows.Forms.SplitContainer();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.split_CAM2ChartGrid = new System.Windows.Forms.SplitContainer();
            this.menuStrip1.SuspendLayout();
            this.label_CurrentSteelKind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_ViewToInfo)).BeginInit();
            this.split_ViewToInfo.Panel1.SuspendLayout();
            this.split_ViewToInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_CamToCam)).BeginInit();
            this.split_CamToCam.Panel1.SuspendLayout();
            this.split_CamToCam.Panel2.SuspendLayout();
            this.split_CamToCam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_CAM1UpToDown)).BeginInit();
            this.split_CAM1UpToDown.Panel1.SuspendLayout();
            this.split_CAM1UpToDown.Panel2.SuspendLayout();
            this.split_CAM1UpToDown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_CAM1Info)).BeginInit();
            this.split_CAM1Info.Panel1.SuspendLayout();
            this.split_CAM1Info.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_CAM1ChartGrid)).BeginInit();
            this.split_CAM1ChartGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_CAM2UpToDown)).BeginInit();
            this.split_CAM2UpToDown.Panel1.SuspendLayout();
            this.split_CAM2UpToDown.Panel2.SuspendLayout();
            this.split_CAM2UpToDown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_CAM2Info)).BeginInit();
            this.split_CAM2Info.Panel1.SuspendLayout();
            this.split_CAM2Info.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_CAM2ChartGrid)).BeginInit();
            this.split_CAM2ChartGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // OPC_textBox1
            // 
            this.OPC_textBox1.Location = new System.Drawing.Point(1370, 12);
            this.OPC_textBox1.Name = "OPC_textBox1";
            this.OPC_textBox1.Size = new System.Drawing.Size(100, 21);
            this.OPC_textBox1.TabIndex = 0;
            // 
            // OPC_textBox2
            // 
            this.OPC_textBox2.Location = new System.Drawing.Point(1568, 12);
            this.OPC_textBox2.Name = "OPC_textBox2";
            this.OPC_textBox2.Size = new System.Drawing.Size(100, 21);
            this.OPC_textBox2.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1476, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1683, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dataLoggingToolStripMenuItem,
            this.dataRecordToolStripMenuItem,
            this.rOIToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1904, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newOnlineToolStripMenuItem,
            this.newSimulationToolStripMenuItem,
            this.openIRDXToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newOnlineToolStripMenuItem
            // 
            this.newOnlineToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newOnlineToolStripMenuItem.Image")));
            this.newOnlineToolStripMenuItem.Name = "newOnlineToolStripMenuItem";
            this.newOnlineToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.newOnlineToolStripMenuItem.Text = "New: Online";
            this.newOnlineToolStripMenuItem.Click += new System.EventHandler(this.newOnlineToolStripMenuItem_Click);
            // 
            // newSimulationToolStripMenuItem
            // 
            this.newSimulationToolStripMenuItem.Name = "newSimulationToolStripMenuItem";
            this.newSimulationToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.newSimulationToolStripMenuItem.Text = "New: Simulation";
            // 
            // openIRDXToolStripMenuItem
            // 
            this.openIRDXToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openIRDXToolStripMenuItem.Image")));
            this.openIRDXToolStripMenuItem.Name = "openIRDXToolStripMenuItem";
            this.openIRDXToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.openIRDXToolStripMenuItem.Text = "Open IRDX";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // dataLoggingToolStripMenuItem
            // 
            this.dataLoggingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem});
            this.dataLoggingToolStripMenuItem.Name = "dataLoggingToolStripMenuItem";
            this.dataLoggingToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
            this.dataLoggingToolStripMenuItem.Text = "Data Logging";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("startToolStripMenuItem.Image")));
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.startToolStripMenuItem.Text = "Start";
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("stopToolStripMenuItem.Image")));
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            // 
            // dataRecordToolStripMenuItem
            // 
            this.dataRecordToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previousRecordToolStripMenuItem,
            this.nextRecordToolStripMenuItem,
            this.toolStripSeparator2,
            this.playToolStripMenuItem,
            this.stopToolStripMenuItem1});
            this.dataRecordToolStripMenuItem.Name = "dataRecordToolStripMenuItem";
            this.dataRecordToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.dataRecordToolStripMenuItem.Text = "Data Player";
            // 
            // previousRecordToolStripMenuItem
            // 
            this.previousRecordToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("previousRecordToolStripMenuItem.Image")));
            this.previousRecordToolStripMenuItem.Name = "previousRecordToolStripMenuItem";
            this.previousRecordToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.previousRecordToolStripMenuItem.Text = "Previous Record";
            // 
            // nextRecordToolStripMenuItem
            // 
            this.nextRecordToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("nextRecordToolStripMenuItem.Image")));
            this.nextRecordToolStripMenuItem.Name = "nextRecordToolStripMenuItem";
            this.nextRecordToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.nextRecordToolStripMenuItem.Text = "Next Record";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("playToolStripMenuItem.Image")));
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.playToolStripMenuItem.Text = "Play";
            // 
            // stopToolStripMenuItem1
            // 
            this.stopToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("stopToolStripMenuItem1.Image")));
            this.stopToolStripMenuItem1.Name = "stopToolStripMenuItem1";
            this.stopToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.stopToolStripMenuItem1.Text = "Stop";
            // 
            // rOIToolStripMenuItem
            // 
            this.rOIToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawROIToolStripMenuItem,
            this.moveROIToolStripMenuItem,
            this.deleteROIToolStripMenuItem});
            this.rOIToolStripMenuItem.Name = "rOIToolStripMenuItem";
            this.rOIToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.rOIToolStripMenuItem.Text = "ROI";
            // 
            // drawROIToolStripMenuItem
            // 
            this.drawROIToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("drawROIToolStripMenuItem.Image")));
            this.drawROIToolStripMenuItem.Name = "drawROIToolStripMenuItem";
            this.drawROIToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.drawROIToolStripMenuItem.Text = "Draw ROI";
            // 
            // moveROIToolStripMenuItem
            // 
            this.moveROIToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("moveROIToolStripMenuItem.Image")));
            this.moveROIToolStripMenuItem.Name = "moveROIToolStripMenuItem";
            this.moveROIToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.moveROIToolStripMenuItem.Text = "Move ROI";
            // 
            // deleteROIToolStripMenuItem
            // 
            this.deleteROIToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteROIToolStripMenuItem.Image")));
            this.deleteROIToolStripMenuItem.Name = "deleteROIToolStripMenuItem";
            this.deleteROIToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.deleteROIToolStripMenuItem.Text = "Delete ROI";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutToolStripMenuItem.Image")));
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            // 
            // label_CurrentSteelKind
            // 
            this.label_CurrentSteelKind.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.새로만들기ToolStripButton,
            this.열기ToolStripButton,
            this.toolStripSeparator3,
            this.PreviousRecord_toolStripButton,
            this.NextRecord_toolStripButton,
            this.KeepMoving_toolStripButton,
            this.Pause_toolStripButton,
            this.toolStripSeparator4,
            this.DrawPOI_toolStripButton,
            this.MovePOI_toolStripButton,
            this.DeletePOI_toolStripButton,
            this.toolStripSeparator5,
            this.LogStart_toolStripButton,
            this.LogStop_toolStripButton,
            this.toolStripSeparator6,
            this.도움말ToolStripButton});
            this.label_CurrentSteelKind.Location = new System.Drawing.Point(0, 24);
            this.label_CurrentSteelKind.Name = "label_CurrentSteelKind";
            this.label_CurrentSteelKind.Size = new System.Drawing.Size(1904, 25);
            this.label_CurrentSteelKind.TabIndex = 5;
            this.label_CurrentSteelKind.Text = "toolStrip1";
            // 
            // 새로만들기ToolStripButton
            // 
            this.새로만들기ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.새로만들기ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("새로만들기ToolStripButton.Image")));
            this.새로만들기ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.새로만들기ToolStripButton.Name = "새로만들기ToolStripButton";
            this.새로만들기ToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.새로만들기ToolStripButton.Text = "새로 만들기";
            this.새로만들기ToolStripButton.Click += new System.EventHandler(this.새로만들기ToolStripButton_Click);
            // 
            // 열기ToolStripButton
            // 
            this.열기ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.열기ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("열기ToolStripButton.Image")));
            this.열기ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.열기ToolStripButton.Name = "열기ToolStripButton";
            this.열기ToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.열기ToolStripButton.Text = "열기";
            this.열기ToolStripButton.Click += new System.EventHandler(this.열기ToolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // PreviousRecord_toolStripButton
            // 
            this.PreviousRecord_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PreviousRecord_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("PreviousRecord_toolStripButton.Image")));
            this.PreviousRecord_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PreviousRecord_toolStripButton.Name = "PreviousRecord_toolStripButton";
            this.PreviousRecord_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.PreviousRecord_toolStripButton.Text = "toolStripButton1";
            this.PreviousRecord_toolStripButton.Click += new System.EventHandler(this.PreviousRecord_toolStripButton_Click);
            // 
            // NextRecord_toolStripButton
            // 
            this.NextRecord_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NextRecord_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("NextRecord_toolStripButton.Image")));
            this.NextRecord_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NextRecord_toolStripButton.Name = "NextRecord_toolStripButton";
            this.NextRecord_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.NextRecord_toolStripButton.Text = "toolStripButton2";
            this.NextRecord_toolStripButton.Click += new System.EventHandler(this.NextRecord_toolStripButton_Click);
            // 
            // KeepMoving_toolStripButton
            // 
            this.KeepMoving_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.KeepMoving_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("KeepMoving_toolStripButton.Image")));
            this.KeepMoving_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.KeepMoving_toolStripButton.Name = "KeepMoving_toolStripButton";
            this.KeepMoving_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.KeepMoving_toolStripButton.Text = "toolStripButton3";
            this.KeepMoving_toolStripButton.Click += new System.EventHandler(this.KeepMoving_toolStripButton_Click);
            // 
            // Pause_toolStripButton
            // 
            this.Pause_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Pause_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("Pause_toolStripButton.Image")));
            this.Pause_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Pause_toolStripButton.Name = "Pause_toolStripButton";
            this.Pause_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.Pause_toolStripButton.Text = "toolStripButton4";
            this.Pause_toolStripButton.Click += new System.EventHandler(this.Pause_toolStripButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // DrawPOI_toolStripButton
            // 
            this.DrawPOI_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DrawPOI_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("DrawPOI_toolStripButton.Image")));
            this.DrawPOI_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DrawPOI_toolStripButton.Name = "DrawPOI_toolStripButton";
            this.DrawPOI_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.DrawPOI_toolStripButton.Text = "toolStripButton5";
            this.DrawPOI_toolStripButton.Click += new System.EventHandler(this.DrawPOI_toolStripButton_Click);
            // 
            // MovePOI_toolStripButton
            // 
            this.MovePOI_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MovePOI_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("MovePOI_toolStripButton.Image")));
            this.MovePOI_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MovePOI_toolStripButton.Name = "MovePOI_toolStripButton";
            this.MovePOI_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.MovePOI_toolStripButton.Text = "toolStripButton6";
            this.MovePOI_toolStripButton.Click += new System.EventHandler(this.MovePOI_toolStripButton_Click);
            // 
            // DeletePOI_toolStripButton
            // 
            this.DeletePOI_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeletePOI_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("DeletePOI_toolStripButton.Image")));
            this.DeletePOI_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeletePOI_toolStripButton.Name = "DeletePOI_toolStripButton";
            this.DeletePOI_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.DeletePOI_toolStripButton.Text = "toolStripButton7";
            this.DeletePOI_toolStripButton.Click += new System.EventHandler(this.DeletePOI_toolStripButton_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // LogStart_toolStripButton
            // 
            this.LogStart_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.LogStart_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("LogStart_toolStripButton.Image")));
            this.LogStart_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LogStart_toolStripButton.Name = "LogStart_toolStripButton";
            this.LogStart_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.LogStart_toolStripButton.Text = "toolStripButton8";
            this.LogStart_toolStripButton.Click += new System.EventHandler(this.LogStart_toolStripButton_Click);
            // 
            // LogStop_toolStripButton
            // 
            this.LogStop_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.LogStop_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("LogStop_toolStripButton.Image")));
            this.LogStop_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LogStop_toolStripButton.Name = "LogStop_toolStripButton";
            this.LogStop_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.LogStop_toolStripButton.Text = "toolStripButton9";
            this.LogStop_toolStripButton.Click += new System.EventHandler(this.LogStop_toolStripButton_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // 도움말ToolStripButton
            // 
            this.도움말ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.도움말ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("도움말ToolStripButton.Image")));
            this.도움말ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.도움말ToolStripButton.Name = "도움말ToolStripButton";
            this.도움말ToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.도움말ToolStripButton.Text = "도움말";
            this.도움말ToolStripButton.Click += new System.EventHandler(this.도움말ToolStripButton_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(0, 51);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(244, 978);
            this.propertyGrid1.TabIndex = 7;
            // 
            // split_ViewToInfo
            // 
            this.split_ViewToInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.split_ViewToInfo.IsSplitterFixed = true;
            this.split_ViewToInfo.Location = new System.Drawing.Point(250, 51);
            this.split_ViewToInfo.Name = "split_ViewToInfo";
            // 
            // split_ViewToInfo.Panel1
            // 
            this.split_ViewToInfo.Panel1.Controls.Add(this.split_CamToCam);
            this.split_ViewToInfo.Size = new System.Drawing.Size(1654, 978);
            this.split_ViewToInfo.SplitterDistance = 1363;
            this.split_ViewToInfo.TabIndex = 8;
            // 
            // split_CamToCam
            // 
            this.split_CamToCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_CamToCam.Location = new System.Drawing.Point(0, 0);
            this.split_CamToCam.Name = "split_CamToCam";
            // 
            // split_CamToCam.Panel1
            // 
            this.split_CamToCam.Panel1.Controls.Add(this.split_CAM1UpToDown);
            // 
            // split_CamToCam.Panel2
            // 
            this.split_CamToCam.Panel2.Controls.Add(this.split_CAM2UpToDown);
            this.split_CamToCam.Size = new System.Drawing.Size(1363, 978);
            this.split_CamToCam.SplitterDistance = 689;
            this.split_CamToCam.TabIndex = 0;
            // 
            // split_CAM1UpToDown
            // 
            this.split_CAM1UpToDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_CAM1UpToDown.IsSplitterFixed = true;
            this.split_CAM1UpToDown.Location = new System.Drawing.Point(0, 0);
            this.split_CAM1UpToDown.Name = "split_CAM1UpToDown";
            this.split_CAM1UpToDown.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split_CAM1UpToDown.Panel1
            // 
            this.split_CAM1UpToDown.Panel1.Controls.Add(this.split_CAM1Info);
            // 
            // split_CAM1UpToDown.Panel2
            // 
            this.split_CAM1UpToDown.Panel2.Controls.Add(this.split_CAM1ChartGrid);
            this.split_CAM1UpToDown.Size = new System.Drawing.Size(689, 978);
            this.split_CAM1UpToDown.SplitterDistance = 638;
            this.split_CAM1UpToDown.TabIndex = 0;
            // 
            // split_CAM1Info
            // 
            this.split_CAM1Info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_CAM1Info.IsSplitterFixed = true;
            this.split_CAM1Info.Location = new System.Drawing.Point(0, 0);
            this.split_CAM1Info.Name = "split_CAM1Info";
            this.split_CAM1Info.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split_CAM1Info.Panel1
            // 
            this.split_CAM1Info.Panel1.Controls.Add(this.textBox3);
            this.split_CAM1Info.Panel1.Controls.Add(this.textBox1);
            this.split_CAM1Info.Panel1.Controls.Add(this.label5);
            this.split_CAM1Info.Panel1.Controls.Add(this.label3);
            this.split_CAM1Info.Panel1.Controls.Add(this.label1);
            this.split_CAM1Info.Size = new System.Drawing.Size(689, 638);
            this.split_CAM1Info.SplitterDistance = 94;
            this.split_CAM1Info.TabIndex = 0;
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("맑은 고딕", 22F);
            this.textBox3.Location = new System.Drawing.Point(580, 13);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 40);
            this.textBox3.TabIndex = 7;
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("맑은 고딕", 22F);
            this.textBox1.Location = new System.Drawing.Point(343, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(95, 40);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("나눔바른고딕", 22F);
            this.label5.Location = new System.Drawing.Point(444, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(130, 34);
            this.label5.TabIndex = 4;
            this.label5.Text = "최고 온도";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("나눔바른고딕", 22F);
            this.label3.Location = new System.Drawing.Point(224, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 34);
            this.label3.TabIndex = 2;
            this.label3.Text = "강번 No.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 24F);
            this.label1.Location = new System.Drawing.Point(3, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "CAMERA #1";
            // 
            // split_CAM1ChartGrid
            // 
            this.split_CAM1ChartGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_CAM1ChartGrid.IsSplitterFixed = true;
            this.split_CAM1ChartGrid.Location = new System.Drawing.Point(0, 0);
            this.split_CAM1ChartGrid.Name = "split_CAM1ChartGrid";
            this.split_CAM1ChartGrid.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.split_CAM1ChartGrid.Size = new System.Drawing.Size(689, 336);
            this.split_CAM1ChartGrid.SplitterDistance = 234;
            this.split_CAM1ChartGrid.TabIndex = 0;
            // 
            // split_CAM2UpToDown
            // 
            this.split_CAM2UpToDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_CAM2UpToDown.IsSplitterFixed = true;
            this.split_CAM2UpToDown.Location = new System.Drawing.Point(0, 0);
            this.split_CAM2UpToDown.Name = "split_CAM2UpToDown";
            this.split_CAM2UpToDown.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split_CAM2UpToDown.Panel1
            // 
            this.split_CAM2UpToDown.Panel1.Controls.Add(this.split_CAM2Info);
            // 
            // split_CAM2UpToDown.Panel2
            // 
            this.split_CAM2UpToDown.Panel2.Controls.Add(this.split_CAM2ChartGrid);
            this.split_CAM2UpToDown.Size = new System.Drawing.Size(670, 978);
            this.split_CAM2UpToDown.SplitterDistance = 638;
            this.split_CAM2UpToDown.TabIndex = 0;
            // 
            // split_CAM2Info
            // 
            this.split_CAM2Info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_CAM2Info.IsSplitterFixed = true;
            this.split_CAM2Info.Location = new System.Drawing.Point(0, 0);
            this.split_CAM2Info.Name = "split_CAM2Info";
            this.split_CAM2Info.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split_CAM2Info.Panel1
            // 
            this.split_CAM2Info.Panel1.Controls.Add(this.textBox4);
            this.split_CAM2Info.Panel1.Controls.Add(this.textBox2);
            this.split_CAM2Info.Panel1.Controls.Add(this.label6);
            this.split_CAM2Info.Panel1.Controls.Add(this.label4);
            this.split_CAM2Info.Panel1.Controls.Add(this.label2);
            this.split_CAM2Info.Size = new System.Drawing.Size(670, 638);
            this.split_CAM2Info.SplitterDistance = 94;
            this.split_CAM2Info.TabIndex = 0;
            // 
            // textBox4
            // 
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Enabled = false;
            this.textBox4.Font = new System.Drawing.Font("맑은 고딕", 22F);
            this.textBox4.Location = new System.Drawing.Point(567, 13);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(100, 40);
            this.textBox4.TabIndex = 8;
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("맑은 고딕", 22F);
            this.textBox2.Location = new System.Drawing.Point(329, 13);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(100, 40);
            this.textBox2.TabIndex = 6;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("나눔바른고딕", 22F);
            this.label6.Location = new System.Drawing.Point(433, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 34);
            this.label6.TabIndex = 5;
            this.label6.Text = "최고 온도";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("나눔바른고딕", 22F);
            this.label4.Location = new System.Drawing.Point(200, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 34);
            this.label4.TabIndex = 3;
            this.label4.Text = "강번 No.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 24F);
            this.label2.Location = new System.Drawing.Point(3, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(199, 36);
            this.label2.TabIndex = 1;
            this.label2.Text = "CAMERA #2";
            // 
            // split_CAM2ChartGrid
            // 
            this.split_CAM2ChartGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_CAM2ChartGrid.IsSplitterFixed = true;
            this.split_CAM2ChartGrid.Location = new System.Drawing.Point(0, 0);
            this.split_CAM2ChartGrid.Name = "split_CAM2ChartGrid";
            this.split_CAM2ChartGrid.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.split_CAM2ChartGrid.Size = new System.Drawing.Size(670, 336);
            this.split_CAM2ChartGrid.SplitterDistance = 234;
            this.split_CAM2ChartGrid.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.split_ViewToInfo);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.label_CurrentSteelKind);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.OPC_textBox2);
            this.Controls.Add(this.OPC_textBox1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Electric Furnance Monitoring System";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.label_CurrentSteelKind.ResumeLayout(false);
            this.label_CurrentSteelKind.PerformLayout();
            this.split_ViewToInfo.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_ViewToInfo)).EndInit();
            this.split_ViewToInfo.ResumeLayout(false);
            this.split_CamToCam.Panel1.ResumeLayout(false);
            this.split_CamToCam.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_CamToCam)).EndInit();
            this.split_CamToCam.ResumeLayout(false);
            this.split_CAM1UpToDown.Panel1.ResumeLayout(false);
            this.split_CAM1UpToDown.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_CAM1UpToDown)).EndInit();
            this.split_CAM1UpToDown.ResumeLayout(false);
            this.split_CAM1Info.Panel1.ResumeLayout(false);
            this.split_CAM1Info.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_CAM1Info)).EndInit();
            this.split_CAM1Info.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_CAM1ChartGrid)).EndInit();
            this.split_CAM1ChartGrid.ResumeLayout(false);
            this.split_CAM2UpToDown.Panel1.ResumeLayout(false);
            this.split_CAM2UpToDown.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_CAM2UpToDown)).EndInit();
            this.split_CAM2UpToDown.ResumeLayout(false);
            this.split_CAM2Info.Panel1.ResumeLayout(false);
            this.split_CAM2Info.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_CAM2Info)).EndInit();
            this.split_CAM2Info.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_CAM2ChartGrid)).EndInit();
            this.split_CAM2ChartGrid.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox OPC_textBox1;
        public System.Windows.Forms.TextBox OPC_textBox2;
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.Button button2;
        public System.Windows.Forms.MenuStrip menuStrip1;
        public System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem newOnlineToolStripMenuItem;
        public System.Windows.Forms.ToolStrip label_CurrentSteelKind;
        public System.Windows.Forms.ToolStripMenuItem newSimulationToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem openIRDXToolStripMenuItem;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem dataLoggingToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem dataRecordToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem previousRecordToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem nextRecordToolStripMenuItem;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem rOIToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem drawROIToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem moveROIToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem deleteROIToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        public System.Windows.Forms.ToolStripButton 새로만들기ToolStripButton;
        public System.Windows.Forms.ToolStripButton 열기ToolStripButton;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.ToolStripButton 도움말ToolStripButton;
        public System.Windows.Forms.PropertyGrid propertyGrid1;
        public System.Windows.Forms.SplitContainer split_ViewToInfo;
        public System.Windows.Forms.SplitContainer split_CamToCam;
        public System.Windows.Forms.SplitContainer split_CAM1UpToDown;
        public System.Windows.Forms.SplitContainer split_CAM1Info;
        public System.Windows.Forms.SplitContainer split_CAM1ChartGrid;
        public System.Windows.Forms.SplitContainer split_CAM2UpToDown;
        public System.Windows.Forms.SplitContainer split_CAM2Info;
        public System.Windows.Forms.SplitContainer split_CAM2ChartGrid;
        public System.Windows.Forms.ToolStripButton PreviousRecord_toolStripButton;
        public System.Windows.Forms.ToolStripButton NextRecord_toolStripButton;
        public System.Windows.Forms.ToolStripButton KeepMoving_toolStripButton;
        public System.Windows.Forms.ToolStripButton Pause_toolStripButton;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        public System.Windows.Forms.ToolStripButton DrawPOI_toolStripButton;
        public System.Windows.Forms.ToolStripButton MovePOI_toolStripButton;
        public System.Windows.Forms.ToolStripButton DeletePOI_toolStripButton;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        public System.Windows.Forms.ToolStripButton LogStart_toolStripButton;
        public System.Windows.Forms.ToolStripButton LogStop_toolStripButton;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.TextBox textBox3;
        public System.Windows.Forms.TextBox textBox4;
        public System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    }
}

