using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MTGUtils
{
    public partial class MainWindow : Form
    {
        DataManager DM;

        public MainWindow()
        {
            DM = new DataManager();
            InitializeComponent();
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
                mtgSetsCheckedListBox.Items.Add(set);
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
            lblSetsStatus.Text = "Status: Getting Sets";
            lblSetsStatus.Refresh();

            DM.UpdateSets();
            List<MTGSet> sets = DM.GetSets();
            mtgSetsCheckedListBox.Items.Clear();
            if (sets.Count == 0)
            {
                mtgSetsCheckedListBox.Items.Add("Updating sets failed. See debug log");
            }
            else
            {
                mtgSetsCheckedListBox.BeginUpdate();
                foreach (MTGSet set in sets)
                {
                    mtgSetsCheckedListBox.Items.Add(set.ToString());
                }
                mtgSetsCheckedListBox.EndUpdate();
                mtgSetsCheckedListBox.Enabled = true;
            }

            lblSetsStatus.Text = "Status: Idle";
            lblSetsStatus.Refresh();
            updateSetsButton.Enabled = true;
        }

        private void allMTGSetsButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < mtgSetsCheckedListBox.Items.Count; i++)
            {
                mtgSetsCheckedListBox.SetItemChecked(i, true);
            }
        }
    }
}
