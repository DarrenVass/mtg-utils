using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using log4net;

namespace MTGUtils
{
    public partial class MainWindow : Form
    {
        DataManager DM;
        private readonly ILog log;

        public MainWindow()
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            DM = new DataManager();
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            List<int> checkedPriceSources = new List<int>();
            List<int> checkedMTGSets = new List<int>();
            DM.GetAppState(ref checkedPriceSources, ref checkedMTGSets);

            if (checkedPriceSources != null)
            {
                try
                {
                    // Set all or only selected choices.
                    if (checkedPriceSources.Count == 0)
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
                }
                catch (Exception err)
                {
                    // This will typically mean the state is out of date somehow.
                    log.Warn("Error updating mtgPriceSourceCheckListBox from stored state: " + err);
                }
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
                        log.Debug("Load Call");
                        updateMTGSetsGraphListBox();
                    }
                }
                catch (Exception err)
                {
                    // This will typically mean the state is out of date somehow.
                    log.Warn("Error updating mtgSetsCheckedListBox from stored state: " + err);
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

            DM.UpdateAppState(checkedPriceSources, checkedMTGSets);

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
                mtgSetsCheckedListBox.Items.Add(set.ToString());
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
                    mtgSetsGraphListBox.Items.Add(checkedIndex);
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
                    mtgCardsGraphListBox.Items.Add(card.ToPriceString());
                }
            }
            mtgCardsGraphListBox.EndUpdate();
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
            string SetName = mtgSetsGraphListBox.SelectedItem.ToString();
            UpdateStatusLabel("Status: Fetching info for " + SetName);
            List<MTGCard> Cards = DM.GetCardListForSet(SetName);
            UpdateStatusLabel("Status: Complete");

            if (Cards != null)
            {
                updateMTGCardsGraphListBox(Cards);
            }
            else
            {
                UpdateStatusLabel("Status: Error retrieving card list for set " + SetName);
            }
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

    }
}
