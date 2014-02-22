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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.mainTabs = new System.Windows.Forms.TabPage();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerTopMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerTopSourcesSetsCheckBoxes = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mtgPriceSourceCheckListBox = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.clearMTGSetsButton = new System.Windows.Forms.Button();
            this.recentMTGSetsButton = new System.Windows.Forms.Button();
            this.modernMTGSetsButton = new System.Windows.Forms.Button();
            this.standardMTGSetsButton = new System.Windows.Forms.Button();
            this.mtgSetsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.updateSetsButton = new System.Windows.Forms.Button();
            this.allMTGSetsButton = new System.Windows.Forms.Button();
            this.splitContainerBottomMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerGraphSetsCards = new System.Windows.Forms.SplitContainer();
            this.mtgSetsGraphListBox = new System.Windows.Forms.ListBox();
            this.mtgCardsGraphListBox = new System.Windows.Forms.ListBox();
            this.splitContainerBottomGraph = new System.Windows.Forms.SplitContainer();
            this.mtgPriceChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.windowStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.mainTab = new System.Windows.Forms.TabControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataFiltersCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.splitContainerCardInfo = new System.Windows.Forms.SplitContainer();
            this.pictureBoxCard = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblHighPrice = new System.Windows.Forms.Label();
            this.lblLowPrice = new System.Windows.Forms.Label();
            this.mainTabs.SuspendLayout();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.splitContainerTopMain.Panel1.SuspendLayout();
            this.splitContainerTopMain.Panel2.SuspendLayout();
            this.splitContainerTopMain.SuspendLayout();
            this.splitContainerTopSourcesSetsCheckBoxes.Panel1.SuspendLayout();
            this.splitContainerTopSourcesSetsCheckBoxes.Panel2.SuspendLayout();
            this.splitContainerTopSourcesSetsCheckBoxes.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.splitContainerBottomMain.Panel1.SuspendLayout();
            this.splitContainerBottomMain.Panel2.SuspendLayout();
            this.splitContainerBottomMain.SuspendLayout();
            this.splitContainerGraphSetsCards.Panel1.SuspendLayout();
            this.splitContainerGraphSetsCards.Panel2.SuspendLayout();
            this.splitContainerGraphSetsCards.SuspendLayout();
            this.splitContainerBottomGraph.Panel1.SuspendLayout();
            this.splitContainerBottomGraph.Panel2.SuspendLayout();
            this.splitContainerBottomGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mtgPriceChart)).BeginInit();
            this.windowStatusStrip.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.mainTab.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.splitContainerCardInfo.Panel1.SuspendLayout();
            this.splitContainerCardInfo.Panel2.SuspendLayout();
            this.splitContainerCardInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCard)).BeginInit();
            this.SuspendLayout();
            // 
            // mainTabs
            // 
            this.mainTabs.Controls.Add(this.splitContainerMain);
            this.mainTabs.Controls.Add(this.windowStatusStrip);
            this.mainTabs.Location = new System.Drawing.Point(4, 22);
            this.mainTabs.Name = "mainTabs";
            this.mainTabs.Padding = new System.Windows.Forms.Padding(3);
            this.mainTabs.Size = new System.Drawing.Size(1418, 752);
            this.mainTabs.TabIndex = 0;
            this.mainTabs.Text = "Price Analyzer";
            this.mainTabs.UseVisualStyleBackColor = true;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMain.IsSplitterFixed = true;
            this.splitContainerMain.Location = new System.Drawing.Point(3, 3);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.BackColor = System.Drawing.Color.DarkGray;
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerTopMain);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.BackColor = System.Drawing.Color.Silver;
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerBottomMain);
            this.splitContainerMain.Size = new System.Drawing.Size(1412, 724);
            this.splitContainerMain.SplitterDistance = 151;
            this.splitContainerMain.TabIndex = 0;
            // 
            // splitContainerTopMain
            // 
            this.splitContainerTopMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTopMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerTopMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTopMain.Name = "splitContainerTopMain";
            // 
            // splitContainerTopMain.Panel1
            // 
            this.splitContainerTopMain.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainerTopMain.Panel2
            // 
            this.splitContainerTopMain.Panel2.Controls.Add(this.splitContainerTopSourcesSetsCheckBoxes);
            this.splitContainerTopMain.Size = new System.Drawing.Size(1410, 149);
            this.splitContainerTopMain.SplitterDistance = 884;
            this.splitContainerTopMain.TabIndex = 8;
            // 
            // splitContainerTopSourcesSetsCheckBoxes
            // 
            this.splitContainerTopSourcesSetsCheckBoxes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTopSourcesSetsCheckBoxes.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerTopSourcesSetsCheckBoxes.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTopSourcesSetsCheckBoxes.Name = "splitContainerTopSourcesSetsCheckBoxes";
            // 
            // splitContainerTopSourcesSetsCheckBoxes.Panel1
            // 
            this.splitContainerTopSourcesSetsCheckBoxes.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainerTopSourcesSetsCheckBoxes.Panel2
            // 
            this.splitContainerTopSourcesSetsCheckBoxes.Panel2.Controls.Add(this.groupBox2);
            this.splitContainerTopSourcesSetsCheckBoxes.Size = new System.Drawing.Size(522, 149);
            this.splitContainerTopSourcesSetsCheckBoxes.SplitterDistance = 208;
            this.splitContainerTopSourcesSetsCheckBoxes.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mtgPriceSourceCheckListBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 149);
            this.groupBox1.TabIndex = 7;
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
            this.mtgPriceSourceCheckListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.mtgPriceSourceCheckListBox_ItemCheck);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.clearMTGSetsButton);
            this.groupBox2.Controls.Add(this.recentMTGSetsButton);
            this.groupBox2.Controls.Add(this.modernMTGSetsButton);
            this.groupBox2.Controls.Add(this.standardMTGSetsButton);
            this.groupBox2.Controls.Add(this.mtgSetsCheckedListBox);
            this.groupBox2.Controls.Add(this.updateSetsButton);
            this.groupBox2.Controls.Add(this.allMTGSetsButton);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(310, 149);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "MTG Sets";
            // 
            // clearMTGSetsButton
            // 
            this.clearMTGSetsButton.Location = new System.Drawing.Point(6, 77);
            this.clearMTGSetsButton.Name = "clearMTGSetsButton";
            this.clearMTGSetsButton.Size = new System.Drawing.Size(58, 23);
            this.clearMTGSetsButton.TabIndex = 9;
            this.clearMTGSetsButton.Text = "Clear";
            this.clearMTGSetsButton.UseVisualStyleBackColor = true;
            this.clearMTGSetsButton.Click += new System.EventHandler(this.clearMTGSetsButton_Click);
            // 
            // recentMTGSetsButton
            // 
            this.recentMTGSetsButton.Location = new System.Drawing.Point(70, 48);
            this.recentMTGSetsButton.Name = "recentMTGSetsButton";
            this.recentMTGSetsButton.Size = new System.Drawing.Size(58, 23);
            this.recentMTGSetsButton.TabIndex = 8;
            this.recentMTGSetsButton.Text = "Recent";
            this.recentMTGSetsButton.UseVisualStyleBackColor = true;
            this.recentMTGSetsButton.Click += new System.EventHandler(this.recentMTGSetsButton_Click);
            // 
            // modernMTGSetsButton
            // 
            this.modernMTGSetsButton.Location = new System.Drawing.Point(6, 48);
            this.modernMTGSetsButton.Name = "modernMTGSetsButton";
            this.modernMTGSetsButton.Size = new System.Drawing.Size(58, 23);
            this.modernMTGSetsButton.TabIndex = 7;
            this.modernMTGSetsButton.Text = "Modern";
            this.modernMTGSetsButton.UseVisualStyleBackColor = true;
            this.modernMTGSetsButton.Click += new System.EventHandler(this.modernMTGSetsButton_Click);
            // 
            // standardMTGSetsButton
            // 
            this.standardMTGSetsButton.Location = new System.Drawing.Point(70, 19);
            this.standardMTGSetsButton.Name = "standardMTGSetsButton";
            this.standardMTGSetsButton.Size = new System.Drawing.Size(58, 23);
            this.standardMTGSetsButton.TabIndex = 6;
            this.standardMTGSetsButton.Text = "Standard";
            this.standardMTGSetsButton.UseVisualStyleBackColor = true;
            this.standardMTGSetsButton.Click += new System.EventHandler(this.standardMTGSetsButton_Click);
            // 
            // mtgSetsCheckedListBox
            // 
            this.mtgSetsCheckedListBox.CheckOnClick = true;
            this.mtgSetsCheckedListBox.Location = new System.Drawing.Point(134, 16);
            this.mtgSetsCheckedListBox.Name = "mtgSetsCheckedListBox";
            this.mtgSetsCheckedListBox.Size = new System.Drawing.Size(169, 124);
            this.mtgSetsCheckedListBox.TabIndex = 2;
            this.mtgSetsCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.mtgSetsCheckedListBox_ItemCheck);
            // 
            // updateSetsButton
            // 
            this.updateSetsButton.Location = new System.Drawing.Point(6, 105);
            this.updateSetsButton.Name = "updateSetsButton";
            this.updateSetsButton.Size = new System.Drawing.Size(122, 34);
            this.updateSetsButton.TabIndex = 4;
            this.updateSetsButton.Text = "Update Set URLs";
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
            // splitContainerBottomMain
            // 
            this.splitContainerBottomMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerBottomMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerBottomMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerBottomMain.IsSplitterFixed = true;
            this.splitContainerBottomMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerBottomMain.Name = "splitContainerBottomMain";
            // 
            // splitContainerBottomMain.Panel1
            // 
            this.splitContainerBottomMain.Panel1.Controls.Add(this.splitContainerGraphSetsCards);
            // 
            // splitContainerBottomMain.Panel2
            // 
            this.splitContainerBottomMain.Panel2.Controls.Add(this.splitContainerBottomGraph);
            this.splitContainerBottomMain.Size = new System.Drawing.Size(1412, 569);
            this.splitContainerBottomMain.SplitterDistance = 232;
            this.splitContainerBottomMain.TabIndex = 0;
            // 
            // splitContainerGraphSetsCards
            // 
            this.splitContainerGraphSetsCards.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerGraphSetsCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerGraphSetsCards.Location = new System.Drawing.Point(0, 0);
            this.splitContainerGraphSetsCards.Name = "splitContainerGraphSetsCards";
            this.splitContainerGraphSetsCards.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerGraphSetsCards.Panel1
            // 
            this.splitContainerGraphSetsCards.Panel1.Controls.Add(this.mtgSetsGraphListBox);
            // 
            // splitContainerGraphSetsCards.Panel2
            // 
            this.splitContainerGraphSetsCards.Panel2.Controls.Add(this.mtgCardsGraphListBox);
            this.splitContainerGraphSetsCards.Size = new System.Drawing.Size(232, 569);
            this.splitContainerGraphSetsCards.SplitterDistance = 256;
            this.splitContainerGraphSetsCards.TabIndex = 0;
            // 
            // mtgSetsGraphListBox
            // 
            this.mtgSetsGraphListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtgSetsGraphListBox.FormattingEnabled = true;
            this.mtgSetsGraphListBox.Location = new System.Drawing.Point(0, 0);
            this.mtgSetsGraphListBox.Name = "mtgSetsGraphListBox";
            this.mtgSetsGraphListBox.Size = new System.Drawing.Size(230, 251);
            this.mtgSetsGraphListBox.TabIndex = 0;
            this.mtgSetsGraphListBox.SelectedIndexChanged += new System.EventHandler(this.mtgSetsGraphListBox_SelectedIndexChanged);
            // 
            // mtgCardsGraphListBox
            // 
            this.mtgCardsGraphListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtgCardsGraphListBox.FormattingEnabled = true;
            this.mtgCardsGraphListBox.Location = new System.Drawing.Point(0, 0);
            this.mtgCardsGraphListBox.Name = "mtgCardsGraphListBox";
            this.mtgCardsGraphListBox.Size = new System.Drawing.Size(230, 303);
            this.mtgCardsGraphListBox.TabIndex = 0;
            this.mtgCardsGraphListBox.SelectedIndexChanged += new System.EventHandler(this.mtgCardsGraphListBox_SelectedIndexChanged);
            // 
            // splitContainerBottomGraph
            // 
            this.splitContainerBottomGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerBottomGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerBottomGraph.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerBottomGraph.IsSplitterFixed = true;
            this.splitContainerBottomGraph.Location = new System.Drawing.Point(0, 0);
            this.splitContainerBottomGraph.Name = "splitContainerBottomGraph";
            // 
            // splitContainerBottomGraph.Panel1
            // 
            this.splitContainerBottomGraph.Panel1.Controls.Add(this.mtgPriceChart);
            // 
            // splitContainerBottomGraph.Panel2
            // 
            this.splitContainerBottomGraph.Panel2.Controls.Add(this.splitContainerCardInfo);
            this.splitContainerBottomGraph.Size = new System.Drawing.Size(1176, 569);
            this.splitContainerBottomGraph.SplitterDistance = 940;
            this.splitContainerBottomGraph.TabIndex = 0;
            // 
            // mtgPriceChart
            // 
            this.mtgPriceChart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea2.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea2.AxisX.IsLabelAutoFit = false;
            chartArea2.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea2.AxisX.Title = "Time";
            chartArea2.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea2.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea2.AxisY.Title = "Price";
            chartArea2.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea2.Name = "graphMainChartArea";
            this.mtgPriceChart.ChartAreas.Add(chartArea2);
            this.mtgPriceChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.mtgPriceChart.Legends.Add(legend2);
            this.mtgPriceChart.Location = new System.Drawing.Point(0, 0);
            this.mtgPriceChart.Name = "mtgPriceChart";
            this.mtgPriceChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            this.mtgPriceChart.Size = new System.Drawing.Size(938, 567);
            this.mtgPriceChart.TabIndex = 0;
            this.mtgPriceChart.Text = "Pricing Chart";
            title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title2.Name = "graphMainTitle";
            title2.Text = "Price History for <Please Select a Card>";
            this.mtgPriceChart.Titles.Add(title2);
            // 
            // windowStatusStrip
            // 
            this.windowStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.windowStatusStrip.Location = new System.Drawing.Point(3, 727);
            this.windowStatusStrip.Name = "windowStatusStrip";
            this.windowStatusStrip.Size = new System.Drawing.Size(1412, 22);
            this.windowStatusStrip.TabIndex = 1;
            this.windowStatusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripStatusLabel.Size = new System.Drawing.Size(1397, 17);
            this.toolStripStatusLabel.Spring = true;
            this.toolStripStatusLabel.Text = "Status: Idle";
            this.toolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1418, 752);
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
            this.panel1.Size = new System.Drawing.Size(1412, 746);
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
            // mainTab
            // 
            this.mainTab.Controls.Add(this.mainTabs);
            this.mainTab.Controls.Add(this.tabPage2);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.mainTab.Location = new System.Drawing.Point(0, 0);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(1426, 778);
            this.mainTab.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.mainTab.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataFiltersCheckedListBox);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox3.Location = new System.Drawing.Point(654, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(230, 149);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Data Filters";
            // 
            // dataFiltersCheckedListBox
            // 
            this.dataFiltersCheckedListBox.CheckOnClick = true;
            this.dataFiltersCheckedListBox.FormattingEnabled = true;
            this.dataFiltersCheckedListBox.Items.AddRange(new object[] {
            "Non-zero data only",
            "Within one standard deviation",
            "Average",
            "Future predictions (3 month)"});
            this.dataFiltersCheckedListBox.Location = new System.Drawing.Point(6, 13);
            this.dataFiltersCheckedListBox.Name = "dataFiltersCheckedListBox";
            this.dataFiltersCheckedListBox.Size = new System.Drawing.Size(218, 124);
            this.dataFiltersCheckedListBox.TabIndex = 0;
            this.dataFiltersCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.dataFiltersCheckedListBox_ItemCheck);
            // 
            // splitContainerCardInfo
            // 
            this.splitContainerCardInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerCardInfo.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerCardInfo.IsSplitterFixed = true;
            this.splitContainerCardInfo.Location = new System.Drawing.Point(0, 0);
            this.splitContainerCardInfo.Name = "splitContainerCardInfo";
            this.splitContainerCardInfo.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerCardInfo.Panel1
            // 
            this.splitContainerCardInfo.Panel1.Controls.Add(this.pictureBoxCard);
            // 
            // splitContainerCardInfo.Panel2
            // 
            this.splitContainerCardInfo.Panel2.Controls.Add(this.lblLowPrice);
            this.splitContainerCardInfo.Panel2.Controls.Add(this.lblHighPrice);
            this.splitContainerCardInfo.Panel2.Controls.Add(this.label3);
            this.splitContainerCardInfo.Panel2.Controls.Add(this.label2);
            this.splitContainerCardInfo.Size = new System.Drawing.Size(230, 567);
            this.splitContainerCardInfo.SplitterDistance = 319;
            this.splitContainerCardInfo.TabIndex = 1;
            // 
            // pictureBoxCard
            // 
            this.pictureBoxCard.Image = global::MTGUtils.Properties.Resources.MTG_Card_Back;
            this.pictureBoxCard.Location = new System.Drawing.Point(1, 1);
            this.pictureBoxCard.Name = "pictureBoxCard";
            this.pictureBoxCard.Size = new System.Drawing.Size(227, 320);
            this.pictureBoxCard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCard.TabIndex = 1;
            this.pictureBoxCard.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "High: $";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(107, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Low: $";
            // 
            // lblHighPrice
            // 
            this.lblHighPrice.AutoSize = true;
            this.lblHighPrice.Location = new System.Drawing.Point(50, 8);
            this.lblHighPrice.Name = "lblHighPrice";
            this.lblHighPrice.Size = new System.Drawing.Size(28, 13);
            this.lblHighPrice.TabIndex = 2;
            this.lblHighPrice.Text = "0.00";
            // 
            // lblLowPrice
            // 
            this.lblLowPrice.AutoSize = true;
            this.lblLowPrice.Location = new System.Drawing.Point(141, 8);
            this.lblLowPrice.Name = "lblLowPrice";
            this.lblLowPrice.Size = new System.Drawing.Size(28, 13);
            this.lblLowPrice.TabIndex = 3;
            this.lblLowPrice.Text = "0.00";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1426, 778);
            this.Controls.Add(this.mainTab);
            this.Name = "MainWindow";
            this.Text = "Jolly MTG";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.mainTabs.ResumeLayout(false);
            this.mainTabs.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerTopMain.Panel1.ResumeLayout(false);
            this.splitContainerTopMain.Panel2.ResumeLayout(false);
            this.splitContainerTopMain.ResumeLayout(false);
            this.splitContainerTopSourcesSetsCheckBoxes.Panel1.ResumeLayout(false);
            this.splitContainerTopSourcesSetsCheckBoxes.Panel2.ResumeLayout(false);
            this.splitContainerTopSourcesSetsCheckBoxes.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainerBottomMain.Panel1.ResumeLayout(false);
            this.splitContainerBottomMain.Panel2.ResumeLayout(false);
            this.splitContainerBottomMain.ResumeLayout(false);
            this.splitContainerGraphSetsCards.Panel1.ResumeLayout(false);
            this.splitContainerGraphSetsCards.Panel2.ResumeLayout(false);
            this.splitContainerGraphSetsCards.ResumeLayout(false);
            this.splitContainerBottomGraph.Panel1.ResumeLayout(false);
            this.splitContainerBottomGraph.Panel2.ResumeLayout(false);
            this.splitContainerBottomGraph.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mtgPriceChart)).EndInit();
            this.windowStatusStrip.ResumeLayout(false);
            this.windowStatusStrip.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.mainTab.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.splitContainerCardInfo.Panel1.ResumeLayout(false);
            this.splitContainerCardInfo.Panel2.ResumeLayout(false);
            this.splitContainerCardInfo.Panel2.PerformLayout();
            this.splitContainerCardInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage mainTabs;
        private System.Windows.Forms.StatusStrip windowStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainerTopMain;
        private System.Windows.Forms.SplitContainer splitContainerTopSourcesSetsCheckBoxes;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox mtgPriceSourceCheckListBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button clearMTGSetsButton;
        private System.Windows.Forms.Button recentMTGSetsButton;
        private System.Windows.Forms.Button modernMTGSetsButton;
        private System.Windows.Forms.Button standardMTGSetsButton;
        private System.Windows.Forms.CheckedListBox mtgSetsCheckedListBox;
        private System.Windows.Forms.Button updateSetsButton;
        private System.Windows.Forms.Button allMTGSetsButton;
        private System.Windows.Forms.SplitContainer splitContainerBottomMain;
        private System.Windows.Forms.SplitContainer splitContainerGraphSetsCards;
        private System.Windows.Forms.ListBox mtgSetsGraphListBox;
        private System.Windows.Forms.ListBox mtgCardsGraphListBox;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl mainTab;
        private System.Windows.Forms.SplitContainer splitContainerBottomGraph;
        private System.Windows.Forms.DataVisualization.Charting.Chart mtgPriceChart;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox dataFiltersCheckedListBox;
        private System.Windows.Forms.SplitContainer splitContainerCardInfo;
        private System.Windows.Forms.PictureBox pictureBoxCard;
        private System.Windows.Forms.Label lblLowPrice;
        private System.Windows.Forms.Label lblHighPrice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;

    }
}

