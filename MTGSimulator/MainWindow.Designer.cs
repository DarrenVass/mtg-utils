namespace MTGUtils
{
    partial class MainWindow
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
            this.mainTab = new System.Windows.Forms.TabControl();
            this.mainTabs = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Recent = new System.Windows.Forms.Button();
            this.modernMTGSetsButton = new System.Windows.Forms.Button();
            this.standardMTGSetsButton = new System.Windows.Forms.Button();
            this.mtgSetsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.updateSetsButton = new System.Windows.Forms.Button();
            this.allMTGSetsButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mtgPriceSourceCheckListBox = new System.Windows.Forms.CheckedListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSetsStatus = new System.Windows.Forms.Label();
            this.mainTab.SuspendLayout();
            this.mainTabs.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.mainTabs);
            this.mainTab.Controls.Add(this.tabPage2);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.mainTab.Location = new System.Drawing.Point(0, 0);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(1064, 762);
            this.mainTab.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.mainTab.TabIndex = 0;
            // 
            // mainTabs
            // 
            this.mainTabs.Controls.Add(this.splitContainer1);
            this.mainTabs.Location = new System.Drawing.Point(4, 22);
            this.mainTabs.Name = "mainTabs";
            this.mainTabs.Padding = new System.Windows.Forms.Padding(3);
            this.mainTabs.Size = new System.Drawing.Size(1056, 736);
            this.mainTabs.TabIndex = 0;
            this.mainTabs.Text = "Price Analyzer";
            this.mainTabs.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.DarkGray;
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Silver;
            this.splitContainer1.Size = new System.Drawing.Size(1050, 730);
            this.splitContainer1.SplitterDistance = 148;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblSetsStatus);
            this.groupBox2.Controls.Add(this.Recent);
            this.groupBox2.Controls.Add(this.modernMTGSetsButton);
            this.groupBox2.Controls.Add(this.standardMTGSetsButton);
            this.groupBox2.Controls.Add(this.mtgSetsCheckedListBox);
            this.groupBox2.Controls.Add(this.updateSetsButton);
            this.groupBox2.Controls.Add(this.allMTGSetsButton);
            this.groupBox2.Location = new System.Drawing.Point(199, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(309, 142);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "MTG Sets";
            // 
            // Recent
            // 
            this.Recent.Location = new System.Drawing.Point(70, 48);
            this.Recent.Name = "Recent";
            this.Recent.Size = new System.Drawing.Size(58, 23);
            this.Recent.TabIndex = 8;
            this.Recent.Text = "Recent";
            this.Recent.UseVisualStyleBackColor = true;
            // 
            // modernMTGSetsButton
            // 
            this.modernMTGSetsButton.Location = new System.Drawing.Point(6, 48);
            this.modernMTGSetsButton.Name = "modernMTGSetsButton";
            this.modernMTGSetsButton.Size = new System.Drawing.Size(58, 23);
            this.modernMTGSetsButton.TabIndex = 7;
            this.modernMTGSetsButton.Text = "Modern";
            this.modernMTGSetsButton.UseVisualStyleBackColor = true;
            // 
            // standardMTGSetsButton
            // 
            this.standardMTGSetsButton.Location = new System.Drawing.Point(70, 19);
            this.standardMTGSetsButton.Name = "standardMTGSetsButton";
            this.standardMTGSetsButton.Size = new System.Drawing.Size(58, 23);
            this.standardMTGSetsButton.TabIndex = 6;
            this.standardMTGSetsButton.Text = "Standard";
            this.standardMTGSetsButton.UseVisualStyleBackColor = true;
            // 
            // mtgSetsCheckedListBox
            // 
            this.mtgSetsCheckedListBox.Location = new System.Drawing.Point(134, 16);
            this.mtgSetsCheckedListBox.Name = "mtgSetsCheckedListBox";
            this.mtgSetsCheckedListBox.Size = new System.Drawing.Size(169, 124);
            this.mtgSetsCheckedListBox.TabIndex = 2;
            // 
            // updateSetsButton
            // 
            this.updateSetsButton.Location = new System.Drawing.Point(6, 80);
            this.updateSetsButton.Name = "updateSetsButton";
            this.updateSetsButton.Size = new System.Drawing.Size(122, 34);
            this.updateSetsButton.TabIndex = 4;
            this.updateSetsButton.Text = "Update Sets";
            this.updateSetsButton.UseVisualStyleBackColor = true;
            this.updateSetsButton.Click += new System.EventHandler(this.updateSetsButton_Click);
            // 
            // allMTGSetsButton
            // 
            this.allMTGSetsButton.Location = new System.Drawing.Point(6, 19);
            this.allMTGSetsButton.Name = "allMTGSetsButton";
            this.allMTGSetsButton.Size = new System.Drawing.Size(58, 23);
            this.allMTGSetsButton.TabIndex = 5;
            this.allMTGSetsButton.Text = "All";
            this.allMTGSetsButton.UseVisualStyleBackColor = true;
            this.allMTGSetsButton.Click += new System.EventHandler(this.allMTGSetsButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mtgPriceSourceCheckListBox);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(190, 140);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Price Sources from mtgprice.com";
            // 
            // mtgPriceSourceCheckListBox
            // 
            this.mtgPriceSourceCheckListBox.BackColor = System.Drawing.SystemColors.Window;
            this.mtgPriceSourceCheckListBox.CheckOnClick = true;
            this.mtgPriceSourceCheckListBox.FormattingEnabled = true;
            this.mtgPriceSourceCheckListBox.Items.AddRange(new object[] {
            "ABUGames",
            "Amazon US",
            "Cardhaus",
            "CCG House",
            "ChannelFireball",
            "Ebay",
            "Hotsauce Games",
            "Pastimes",
            "Starcity",
            "StrikeZone Online",
            "SuperGames Inc.",
            "TCGPlayer LOW",
            "Troll and Toad"});
            this.mtgPriceSourceCheckListBox.Location = new System.Drawing.Point(6, 14);
            this.mtgPriceSourceCheckListBox.Name = "mtgPriceSourceCheckListBox";
            this.mtgPriceSourceCheckListBox.Size = new System.Drawing.Size(178, 124);
            this.mtgPriceSourceCheckListBox.Sorted = true;
            this.mtgPriceSourceCheckListBox.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1056, 736);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "About";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1050, 730);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(395, 346);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "This Program created by \"JollyGuy\"";
            // 
            // lblSetsStatus
            // 
            this.lblSetsStatus.AutoSize = true;
            this.lblSetsStatus.Location = new System.Drawing.Point(13, 121);
            this.lblSetsStatus.Name = "lblSetsStatus";
            this.lblSetsStatus.Size = new System.Drawing.Size(60, 13);
            this.lblSetsStatus.TabIndex = 9;
            this.lblSetsStatus.Text = "Status: Idle";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 762);
            this.Controls.Add(this.mainTab);
            this.Name = "MainWindow";
            this.Text = "Jolly MTG";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.mainTab.ResumeLayout(false);
            this.mainTabs.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTab;
        private System.Windows.Forms.TabPage mainTabs;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox mtgPriceSourceCheckListBox;
        private System.Windows.Forms.CheckedListBox mtgSetsCheckedListBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button allMTGSetsButton;
        private System.Windows.Forms.Button updateSetsButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Recent;
        private System.Windows.Forms.Button modernMTGSetsButton;
        private System.Windows.Forms.Button standardMTGSetsButton;
        private System.Windows.Forms.Label lblSetsStatus;
    }
}

