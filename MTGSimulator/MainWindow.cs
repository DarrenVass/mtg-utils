﻿using System;
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

            if (DM.GetSets().Count > 0)
            {
                updateMTGSetsCheckedListBox();
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            /* Check all the MTGPrice.com Sources */
            for (int i = 0; i < mtgPriceSourceCheckListBox.Items.Count; i++)
            {
                mtgPriceSourceCheckListBox.SetItemChecked(i, true);
            }

            mtgSetsCheckedListBox.Items.Clear();
            foreach (MTGSet set in DM.GetSets())
            {
                mtgSetsCheckedListBox.Items.Add(set.ToString());
            }
            if (mtgSetsCheckedListBox.Items.Count == 0)
            { 
                mtgSetsCheckedListBox.Items.Add("Need to \"Update Sets\"");
                mtgSetsCheckedListBox.Enabled = false;
            }
        }

        private void updateSetsButton_Click(object sender, EventArgs e)
        {
            updateSetsButton.Enabled = false;
            UpdateStatusLabel("Status: Getting Sets");

            DM.UpdateSets();
            List<MTGSet> sets = DM.GetSets();
            mtgSetsCheckedListBox.Items.Clear();
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

        

        private void updateMTGSetsCheckedListBox()
        {
            List<MTGSet> sets = DM.GetSets();
            mtgSetsCheckedListBox.BeginUpdate();
            mtgSetsGraphListBox.BeginUpdate();
            foreach (MTGSet set in sets)
            {
                mtgSetsGraphListBox.Items.Add(set.ToString());
                mtgSetsCheckedListBox.Items.Add(set.ToString());
            }
            mtgSetsGraphListBox.EndUpdate();
            mtgSetsGraphListBox.Enabled = true;
            mtgSetsCheckedListBox.EndUpdate();
            mtgSetsCheckedListBox.Enabled = true;

            allMTGSetsButton.Enabled = sets.Count > 0;
            standardMTGSetsButton.Enabled = sets.Count > 0;
            modernMTGSetsButton.Enabled = sets.Count > 0;
            recentMTGSetsButton.Enabled = sets.Count > 0;
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

        private void fromDateMTGSetsButtonHelper(DateTime fromDate)
        {
            List<MTGSet> currentSets = new List<MTGSet>();
            foreach (MTGSet set in DM.GetSets())
            {
                if (set.SetDate.CompareTo(fromDate) > 0)
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

        private void standardMTGSetsButton_Click(object sender, EventArgs e)
        {
            /* Sets within 2 years as a guesstimate. */
            DateTime twoYearsAgo = DateTime.Today;
            twoYearsAgo.AddYears(-2);

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

        /* TODO Use A status bar at the bottom of the window, more professional looking. */
        private void UpdateStatusLabel(string statusIn)
        {
            lblSetsStatus.Text = statusIn;
            lblSetsStatus.Refresh();
        }

        /* Uncheck all boxes in the mtgSetsCheckedListBOx */
        private void UnselectAllMTGSetsCheckedListBox()
        {
            foreach (int i in mtgSetsCheckedListBox.CheckedIndices)
            {
                mtgSetsCheckedListBox.SetItemChecked(i, false);
            }
        }

    }
}
