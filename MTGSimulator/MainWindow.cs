using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using log4net;

namespace MTGUtils
{
    public partial class MainWindow : Form
    {
        DataManager DM;
        DataFunctions DF;

        private readonly ILog log;

        public MainWindow()
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            DM = new DataManager();
            DF = new DataFunctions();
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            List<int> checkedPriceSources = new List<int>();
            List<int> checkedMTGSets = new List<int>();
            List<int> checkedDataFilters = new List<int>();
            DM.GetAppState(ref checkedPriceSources, ref checkedMTGSets, ref checkedDataFilters);

            try
            {
                // Set all or only selected choices.
                if (checkedPriceSources == null || checkedPriceSources.Count == 0)
                {
                    for (int i = 0; i < mtgPriceSourceCheckListBox.Items.Count; i++)
                    {
                        mtgPriceSourceCheckListBox.SetItemChecked(i, true);
                    }
                }
                else
                {
                    foreach (int index in checkedPriceSources)
                        mtgPriceSourceCheckListBox.SetItemChecked(index, true);
                }
                ApplyPriceSourcesToChart();
            }
            catch (Exception err)
            {
                // This will typically mean the state is out of date somehow.
                log.Warn("Error updating mtgPriceSourceCheckListBox from stored state: " + err);
            }

            updateMTGSetsCheckedListBox();
            if (checkedMTGSets != null)
            {
                try
                {
                    if (mtgSetsCheckedListBox.Items.Count == 0)
                    {
                        mtgSetsCheckedListBox.Items.Add("Need to \"Update Sets\"");
                        mtgSetsCheckedListBox.Enabled = false;
                    }
                    else
                    {
                        foreach (int index in checkedMTGSets)
                            mtgSetsCheckedListBox.SetItemChecked(index, true);
                        updateMTGSetsGraphListBox();
                    }
                }
                catch (Exception err)
                {
                    // This will typically mean the state is out of date somehow.
                    log.Warn("Error updating mtgSetsCheckedListBox from stored state: " + err);
                }
            }

            if (checkedDataFilters != null)
            {
                try
                {
                    foreach (int index in checkedDataFilters)
                        dataFiltersCheckedListBox.SetItemChecked(index, true);
                }
                catch (Exception err)
                {
                    // This will typically mean the state is out of date somehow.
                    log.Warn("Error updating dataFiltersCheckedListBox from stored state: " + err);
                }
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<int> checkedPriceSources = new List<int>();
            foreach (int checkedIndex in mtgPriceSourceCheckListBox.CheckedIndices)
                checkedPriceSources.Add(checkedIndex);

            List<int> checkedMTGSets = new List<int>();
            foreach (int checkedIndex in mtgSetsCheckedListBox.CheckedIndices)
                checkedMTGSets.Add(checkedIndex);

            List<int> checkedDataFilters = new List<int>();
            foreach (int checkedIndex in dataFiltersCheckedListBox.CheckedIndices)
                checkedDataFilters.Add(checkedIndex);

            DM.UpdateAppState(checkedPriceSources, checkedMTGSets, checkedDataFilters);

            DM.Dispose();
        }

    /* For Handling the MTGSetsCheckedListBox */
        private void updateSetsButton_Click(object sender, EventArgs e)
        {
            updateSetsButton.Enabled = false;
            UpdateStatusLabel("Status: Getting Sets");

            DM.UpdateSetURLs();
            List<MTGSet> sets = DM.GetSets();
            
            if (sets.Count == 0)
            {
                mtgSetsCheckedListBox.Items.Add("Updating sets failed. See debug log");
            }
            else
            {
                updateMTGSetsCheckedListBox();
            }

            UpdateStatusLabel("Status: Idle");
            updateSetsButton.Enabled = true;
        }

        /*
         * Updates the elements of the MTGSetsCheckedListBox (will clear checked info)
         */
        private void updateMTGSetsCheckedListBox()
        {
            List<MTGSet> sets = DM.GetSets();
            mtgSetsCheckedListBox.BeginUpdate();
            mtgSetsCheckedListBox.Items.Clear();
            foreach (MTGSet set in sets)
            {
                mtgSetsCheckedListBox.Items.Add(set);
            }
            mtgSetsCheckedListBox.EndUpdate();
            mtgSetsCheckedListBox.Enabled = true;

            allMTGSetsButton.Enabled = sets.Count > 0;
            standardMTGSetsButton.Enabled = sets.Count > 0;
            modernMTGSetsButton.Enabled = sets.Count > 0;
            recentMTGSetsButton.Enabled = sets.Count > 0;
            clearMTGSetsButton.Enabled = sets.Count > 0;
        }

        private void fromDateMTGSetsButtonHelper(DateTime fromDate)
        {
            List<MTGSet> currentSets = new List<MTGSet>();
             foreach (MTGSet set in DM.GetSets())
            {
                if (set.SetDate.CompareTo(fromDate) >= 0)
                {
                    currentSets.Add(set);
                }
            }

            UnselectAllMTGSetsCheckedListBox();

            try
            {
                foreach (MTGSet set in currentSets)
                {
                    mtgSetsCheckedListBox.SetItemChecked(mtgSetsCheckedListBox.FindString(set.ToString()), true);
                }

                UpdateStatusLabel("Certain Sets: Complete");
            }
            catch (ArgumentOutOfRangeException err)
            {
                log.Warn("fromDateMTGSetsButtonHelper Error:", err);
                UpdateStatusLabel("Certain Sets: Error");
            }
        }

        private void allMTGSetsButton_Click(object sender, EventArgs e)
        {
            UnselectAllMTGSetsCheckedListBox();

            /* All sets checked. */
            for (int i = 0; i < mtgSetsCheckedListBox.Items.Count; i++)
            {
                mtgSetsCheckedListBox.SetItemChecked(i, true);
            }
            UpdateStatusLabel("All: Complete");
        }

        private void standardMTGSetsButton_Click(object sender, EventArgs e)
        {
            /* Sets within 2 years as a guesstimate. */
            DateTime twoYearsAgo = DateTime.Now.AddYears(-2);
 
            fromDateMTGSetsButtonHelper(twoYearsAgo);
        }

        private void modernMTGSetsButton_Click(object sender, EventArgs e)
        {
            /* Sets since 8th edition */
            List<MTGSet> sets = DM.GetSets();
            DateTime eighthDate = DateTime.Today;
            foreach(MTGSet set in sets)
            {
                if(set.ToString().CompareTo("8th Edition") == 0)
                {
                    eighthDate = set.SetDate;
                    break;
                }
            }
            
            fromDateMTGSetsButtonHelper(eighthDate);
        }

        private void recentMTGSetsButton_Click(object sender, EventArgs e)
        {
            /* Set with most recent release date */
            DateTime mostRecent = DateTime.MinValue;
            string setName = null;
            foreach (MTGSet set in DM.GetSets())
            {
                if (mostRecent.CompareTo(set.SetDate) < 0)
                {
                    mostRecent = set.SetDate;
                    setName = set.ToString();
                }
            }

            fromDateMTGSetsButtonHelper(mostRecent);
        }

        /* Uncheck all boxes in the mtgSetsCheckedListBOx */
        private void UnselectAllMTGSetsCheckedListBox()
        {
            foreach (int i in mtgSetsCheckedListBox.CheckedIndices)
            {
                mtgSetsCheckedListBox.SetItemChecked(i, false);
            }
        }

        private void clearMTGSetsButton_Click(object sender, EventArgs e)
        {
            UnselectAllMTGSetsCheckedListBox();
        }

        private void UpdateGraphWithPricePoints()
        {
            MTGCard curCard = DM.GetCurrentCard();
            List<PricePoint> curPP = DM.GetCurrentPricePoints();
            if (curCard == null || curPP == null)
            {
                // This can be called at the start to update sources when card/PP are null.
                return;
            }

            foreach (Series cs in mtgPriceChart.Series)
                cs.Points.Clear();

            UpdateStatusLabel("Status: Applying data filters.");
            // Apply any filters that are selected
            FilterTypes tempDataFilters = new FilterTypes();
            this.PopulateFilterTypes(ref tempDataFilters);
            curPP = DM.ApplyFilters(curPP, tempDataFilters);
            UpdateStatusLabel("Status: Complete");

            foreach (PricePoint pp in curPP)
            {
                // Only add points for selected retailers
                if (mtgPriceChart.Series.IsUniqueName(pp.Retailer) == false)
                {
                    mtgPriceChart.Series[pp.Retailer].Points.AddXY(pp.Date, (double)pp.Price / 100);
                }
            }
 
            UpdateCardInfoWindow(curCard, curPP);
        }

        private void UpdateCardInfoWindow(MTGCard CardIn, List<PricePoint> PPsIn)
        {
            UInt64 min = 0, max = 0;
            DF.GetMinMax(PPsIn, ref min, ref max);

            lblCurrentPrice.Text = DF.GetPriceFromUInt64(CardIn.Price);
            lblLowPrice.Text = DF.GetPriceFromUInt64(min);
            lblHighPrice.Text = DF.GetPriceFromUInt64(max);

            UInt64 Avg = 0, Avg3Day = 0, Avg7Day = 0, Avg30Day = 0;
            DM.CalculateAverages(PPsIn, ref Avg, ref Avg3Day, ref Avg7Day, ref Avg30Day);
            lblAveragePrice.Text = DF.GetPriceFromUInt64(Avg);
            lbl3DayAveragePrice.Text = DF.GetPriceFromUInt64(Avg3Day);
            lbl7DayAveragePrice.Text = DF.GetPriceFromUInt64(Avg7Day);
            lbl30DayAveragePrice.Text = DF.GetPriceFromUInt64(Avg30Day);
        }

        /*
         * Convert the dataFiltersCheckedListBox to FilterTypes struct.
         */
        private void PopulateFilterTypes(ref FilterTypes Filter)
        {
            Filter.StdDev = false;
            Filter.Future = false;
            Filter.Average = false;

            if (dataFiltersCheckedListBox.GetItemChecked(0))
                Filter.StdDev = true; ;
            if (dataFiltersCheckedListBox.GetItemChecked(1))
                Filter.Average = true; ;
            if (dataFiltersCheckedListBox.GetItemChecked(2))
                Filter.Future = true; ;
        }

        /*
         * Updates the elements of the mtgSetsGraphListBox based on checked state of mtgSetsCheckedListBox
         */
        private void updateMTGSetsGraphListBox()
        {
            mtgSetsGraphListBox.BeginUpdate();
            mtgSetsGraphListBox.Items.Clear();
            if (mtgSetsCheckedListBox.CheckedItems.Count > 0)
            {
                foreach (object checkedIndex in mtgSetsCheckedListBox.CheckedItems)
                {
                    MTGSet curSet = (MTGSet)checkedIndex;
                    mtgSetsGraphListBox.Items.Add(curSet);
                }
            }
            mtgSetsGraphListBox.EndUpdate();
        }

        /*
         * Updates the elements of the mtgCardsGraphListBox based on List<MTGCard>
         */
        private void updateMTGCardsGraphListBox(List<MTGCard> CurCards)
        {
            mtgCardsGraphListBox.BeginUpdate();
            mtgCardsGraphListBox.Items.Clear();
            if (CurCards.Count > 0)
            {
                foreach (MTGCard card in CurCards)
                {
                    mtgCardsGraphListBox.Items.Add(card);
                }
            }
            mtgCardsGraphListBox.EndUpdate();
        }

        /* Update the Series on the chart */
        private void ApplyPriceSourcesToChart()
        {
            mtgPriceChart.Series.Clear();
            foreach (string checkedIndex in mtgPriceSourceCheckListBox.CheckedItems)
            {
                mtgPriceChart.Series.Add(checkedIndex);
                mtgPriceChart.Series[checkedIndex].ChartType = SeriesChartType.Line;
                mtgPriceChart.Series[checkedIndex].XValueType = ChartValueType.DateTime;
                mtgPriceChart.Series[checkedIndex].BorderWidth = 5;

                mtgPriceChart.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd";
                mtgPriceChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
            }
            // Add average last so it's on top
            mtgPriceChart.Series.Add("Average");
            mtgPriceChart.Series["Average"].ChartType = SeriesChartType.Line;
            mtgPriceChart.Series["Average"].XValueType = ChartValueType.DateTime;
            mtgPriceChart.Series["Average"].Color = Color.Red;
            mtgPriceChart.Series["Average"].BorderWidth = 8;

            UpdateGraphWithPricePoints();
        }

        /* Fetch PricePoints if needed, update chart variables */
        private void ApplyPricePointsToChart()
        {
            if (mtgCardsGraphListBox.SelectedItem == null)
            {
                return;
            }

            MTGCard curCard = (MTGCard)mtgCardsGraphListBox.SelectedItem;
            if (curCard == null)
            {
                return;
            }

            // Populate the List of Currently selected retailers.
            List<string> RetailerList = new List<string>();
            foreach (string retailer in mtgPriceSourceCheckListBox.CheckedItems)
            {
                RetailerList.Add(retailer);
            }

            // Update the Title
            mtgPriceChart.Titles.Clear();
            mtgPriceChart.Titles.Add(curCard.ToString());

            // Fetch the Price Points if required
            UpdateStatusLabel("Status: Fetching info for " + curCard.ToString());
            List<PricePoint> PricePoints = DM.GetPricePointsForCard(curCard, RetailerList);
            UpdateStatusLabel("Status: Complete");

            if (PricePoints != null)
            {
                UpdateGraphWithPricePoints();
            }
            else
            {
                UpdateStatusLabel("Status: Error retrieving price points for card " + curCard.ToString());
            }

            try
            {
                pictureBoxCard.Load(curCard.CardImageURL);
            }
            catch (System.Net.WebException ex)
            {
                if (ex.Response is System.Net.HttpWebResponse)
                {
                    System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)ex.Response;
                    log.Error("Failed to load image " + curCard.CardImageURL + " with status code " + response.StatusCode + " (" + response.StatusDescription + ")");
                }

                pictureBoxCard.Image = global::MTGUtils.Properties.Resources.MTG_Card_Back;

            }
        }

        /* Simple function for updating the Status bar at the bottom of the window */
        private void UpdateStatusLabel(string statusIn)
        {
            toolStripStatusLabel.Text = statusIn;
            this.windowStatusStrip.Refresh();
        }

        /* When a different set is selected, grab URL for specific cards if required and populate mtgCardsGraphListBox */
        private void mtgSetsGraphListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mtgSetsGraphListBox.SelectedItem == null)
            {
                return;
            }
            MTGSet curSet = (MTGSet)mtgSetsGraphListBox.SelectedItem;
            UpdateStatusLabel("Status: Fetching info for " + curSet.ToString());
            List<MTGCard> Cards = DM.GetCardListForSet(curSet);
            UpdateStatusLabel("Status: Complete");

            if (Cards != null)
            {
                updateMTGCardsGraphListBox(Cards);
            }
            else
            {
                UpdateStatusLabel("Status: Error retrieving card list for set " + curSet.ToString());
            }
        }

        /* When a different card is selected update the chart. */
        private void mtgCardsGraphListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyPricePointsToChart();
        }

        /*
         * This event fires before the CheckedItems are updated. 
         * So disable event checker, update the CheckedItems and re-enable event checker and continue
         */
        private void mtgSetsCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox clb = (CheckedListBox)sender;
            clb.ItemCheck -= mtgSetsCheckedListBox_ItemCheck;
            clb.SetItemCheckState(e.Index, e.NewValue);
            clb.ItemCheck += mtgSetsCheckedListBox_ItemCheck;

            updateMTGSetsGraphListBox();
        }

        /*
         * This event fires before the CheckedItems are updated. 
         * So disable event checker, update the CheckedItems and re-enable event checker and continue
         */
        private void mtgPriceSourceCheckListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox clb = (CheckedListBox)sender;
            clb.ItemCheck -= mtgPriceSourceCheckListBox_ItemCheck;
            clb.SetItemCheckState(e.Index, e.NewValue);
            clb.ItemCheck += mtgPriceSourceCheckListBox_ItemCheck;

            ApplyPriceSourcesToChart();
            ApplyPricePointsToChart();
        }

        /*
         * This event fires before the CheckedItems are updated. 
         * So disable event checker, update the CheckedItems and re-enable event checker and continue
         */
        private void dataFiltersCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox clb = (CheckedListBox)sender;
            clb.ItemCheck -= dataFiltersCheckedListBox_ItemCheck;
            clb.SetItemCheckState(e.Index, e.NewValue);
            clb.ItemCheck += dataFiltersCheckedListBox_ItemCheck;

            UpdateGraphWithPricePoints();
        }

        /* Grab all selected sets and pass it to DataManager.ParseAllCards() */
        private void parseAllCardsButton_Click(object sender, EventArgs e)
        {
            if (mtgSetsGraphListBox.Items.Count == 0) { return; }
            List<MTGSet> setsToPass = new List<MTGSet>();
            foreach (MTGSet set in mtgSetsGraphListBox.Items)
            {
                setsToPass.Add(set);
            }
            UpdateStatusLabel("Status: Fetching info for selected sets");
            DM.ParseAllCards(setsToPass);
            UpdateStatusLabel("Status: Complete");
        }
    }
}
