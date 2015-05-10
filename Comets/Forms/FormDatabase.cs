﻿using Comets.Classes;
using Comets.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Comets.Forms
{
    public partial class FormDatabase : Form
    {
        #region Properties

        FilterCollection Filters;

        #endregion

        #region Constructor

        public FormDatabase()
        {
            InitializeComponent();
        }

        #endregion

        #region Form_Load

        private void FormDatabase_Load(object sender, EventArgs e)
        {
            SortList(FormMain.UserList);

            pnlDetails.Visible = true;
            pnlFilters.Visible = false;

            PopulateFilters(Filters);
        }

        #endregion

        #region lbxDatabase_SelectedIndexChanged

        private void lbxDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = lbxDatabase.SelectedIndex;
            Comet c = FormMain.UserList.ElementAt(ind);

            t_id.Text = c.id;
            t_name.Text = c.name;
            t_T.Text = c.Ty.ToString() + "-" + c.Tm.ToString("00") + "-" + c.Td.ToString("00") + "." + c.Th.ToString("0000");
            t_q1.Text = c.q.ToString("0.000000");
            t_e.Text = c.e.ToString("0.000000");
            t_i.Text = c.i.ToString("0.0000");
            t_N1.Text = c.N.ToString("0.0000");
            t_w.Text = c.w.ToString("0.0000");

            if (c.P > 10000 || c.e > 0.98)
            {
                t_P.Text = string.Empty;
                t_Q2.Text = string.Empty;
                t_a.Text = string.Empty;
                t_n2.Text = string.Empty;
            }
            else
            {
                t_P.Text = c.P.ToString("0.000000");
                t_Q2.Text = c.Q.ToString("0.000000");
                t_a.Text = c.a.ToString("0.000000");
                t_n2.Text = c.n.ToString("0.000000");
            }

            t_g.Text = c.g.ToString("0.0");
            t_k.Text = c.k.ToString("0.0");

            t_sortKey.Text = c.sortkey.ToString("0.00000000000");
            //t_sortKey.Text = c.idKey;

            tEquinox.Text = "2000.0";
        }

        #endregion

        #region lbxDatabase_MouseDoubleClick

        private void lbxDatabase_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            pnlFilters.Visible = false;
            pnlDetails.Visible = true;
        }

        #endregion

        #region Sort

        #region btnSort_Click

        private void btnSort_Click(object sender, EventArgs e)
        {
            contextSort.Show(this, new Point((sender as Button).Left + 1, (sender as Button).Top + (sender as Button).Height - 1));
        }

        #endregion

        #region menuItemSort_Click

        private void menuItemSort_Click(object sender, EventArgs e)
        {
            if (!(sender as MenuItem).Checked)
            {
                bool order = menuItemAsc.Checked;

                foreach (MenuItem item in contextSort.MenuItems)
                    item.Checked = false;

                (sender as MenuItem).Checked = true;

                menuItemAsc.Checked = order;
                menuItemDesc.Checked = !order;

                SortList(FormMain.UserList);
            }
        }

        private void menuItemAsc_Click(object sender, EventArgs e)
        {
            menuItemAsc.Checked = true;
            menuItemDesc.Checked = false;
            SortList(FormMain.UserList);
        }

        private void menuItemDesc_Click(object sender, EventArgs e)
        {
            menuItemAsc.Checked = false;
            menuItemDesc.Checked = true;
            SortList(FormMain.UserList);
        }

        #endregion

        #region SortList

        public void SortList(List<Comet> list)
        {
            List<Comet> tempList = list.ToList();
            FormMain.UserList.Clear();

            if (menuItemDesig.Checked && menuItemAsc.Checked)
                FormMain.UserList = tempList.OrderBy(x => x.sortkey).ToList();

            else if (menuItemDesig.Checked && menuItemDesc.Checked)
                FormMain.UserList = tempList.OrderByDescending(x => x.sortkey).ToList();

            else if (menuItemName.Checked && menuItemAsc.Checked)
                FormMain.UserList = tempList.OrderBy(x => x.name).ToList();

            else if (menuItemName.Checked && menuItemDesc.Checked)
                FormMain.UserList = tempList.OrderByDescending(x => x.name).ToList();

            else if (menuItemPerihDate.Checked && menuItemAsc.Checked)
                FormMain.UserList = tempList.OrderBy(x => x.T).ToList();

            else if (menuItemPerihDate.Checked && menuItemDesc.Checked)
                FormMain.UserList = tempList.OrderByDescending(x => x.T).ToList();

            else if (menuItemPerihDist.Checked && menuItemAsc.Checked)
                FormMain.UserList = tempList.OrderBy(x => x.q).ToList();

            else if (menuItemPerihDist.Checked && menuItemDesc.Checked)
                FormMain.UserList = tempList.OrderByDescending(x => x.q).ToList();

            else if (menuItemIncl.Checked && menuItemAsc.Checked)
                FormMain.UserList = tempList.OrderBy(x => x.i).ToList();

            else if (menuItemIncl.Checked && menuItemDesc.Checked)
                FormMain.UserList = tempList.OrderByDescending(x => x.i).ToList();

            else if (menuItemEcc.Checked && menuItemAsc.Checked)
                FormMain.UserList = tempList.OrderBy(x => x.e).ToList();

            else if (menuItemEcc.Checked && menuItemDesc.Checked)
                FormMain.UserList = tempList.OrderByDescending(x => x.e).ToList();

            else if (menuItemAscNode.Checked && menuItemAsc.Checked)
                FormMain.UserList = tempList.OrderBy(x => x.N).ToList();

            else if (menuItemAscNode.Checked && menuItemDesc.Checked)
                FormMain.UserList = tempList.OrderByDescending(x => x.N).ToList();

            else if (menuItemArgPeri.Checked && menuItemAsc.Checked)
                FormMain.UserList = tempList.OrderBy(x => x.w).ToList();

            else if (menuItemArgPeri.Checked && menuItemDesc.Checked)
                FormMain.UserList = tempList.OrderByDescending(x => x.w).ToList();

            else if (menuItemPeriod.Checked && menuItemAsc.Checked)
                FormMain.UserList = tempList.OrderBy(x => x.P).ToList();

            else if (menuItemPeriod.Checked && menuItemDesc.Checked)
                FormMain.UserList = tempList.OrderByDescending(x => x.P).ToList();

            //clear textboxes if no comets
            if (!FormMain.UserList.Any())
                foreach (Control c in gbDetails.Controls)
                    if (c is TextBox)
                        c.Text = string.Empty;

            lbxDatabase.DataSource = FormMain.UserList;
            lbxDatabase.DisplayMember = "full";
        }

        #endregion

        #endregion

        #region Filters

        #region btnFilters_Click

        private void btnFilters_Click(object sender, EventArgs e)
        {
            pnlDetails.Visible = !pnlDetails.Visible;
            pnlFilters.Visible = !pnlFilters.Visible;
        }

        #endregion

        #region CollectFilters

        private FilterCollection CollectFilters(FilterCollection fc)
        {
            if (fc == null)
                fc = new FilterCollection();

            fc.Name.Checked = cbxName.Checked;
            fc.Name.Text = txtName.Text.Trim().ToLower();
            fc.Name.Index = cboName.SelectedIndex;

            fc.PerihelionDate.Checked = cbxPerihelionDate.Checked;
            fc.PerihelionDate.Text = txtPerihelionDateD.Text.Trim().PadLeft(2, '0') + "." + txtPerihelionDateM.Text.Trim().PadLeft(2, '0') + "." + txtPerihelionDateY.Text.Trim();
            fc.PerihelionDate.Index = cboPerihelionDate.SelectedIndex;

            fc.PerihelionDistance.Checked = cbxPerihelionDistance.Checked;
            fc.PerihelionDistance.Text = txtPerihelionDistance.Text.Trim().ToLower();
            fc.PerihelionDistance.Index = cboPerihelionDistance.SelectedIndex;

            fc.Eccentricity.Checked = cbxEccentricity.Checked;
            fc.Eccentricity.Text = txtEccentricity.Text.Trim().ToLower();
            fc.Eccentricity.Index = cboEccentricity.SelectedIndex;

            fc.LongOfAscendingNode.Checked = cbxLongOfAscendingNode.Checked;
            fc.LongOfAscendingNode.Text = txtLongOfAscendingNode.Text.Trim().ToLower();
            fc.LongOfAscendingNode.Index = cboLongOfAscendingNode.SelectedIndex;

            fc.ArgumentOfPericenter.Checked = cbxArgumentOfPericenter.Checked;
            fc.ArgumentOfPericenter.Text = txtArgumentOfPericenter.Text.Trim().ToLower();
            fc.ArgumentOfPericenter.Index = cboArgumentOfPericenter.SelectedIndex;

            fc.Inclination.Checked = cbxInclination.Checked;
            fc.Inclination.Text = txtInclination.Text.Trim().ToLower();
            fc.Inclination.Index = cboInclination.SelectedIndex;

            fc.Period.Checked = cbxPeriod.Checked;
            fc.Period.Text = txtPeriod.Text.Trim().ToLower();
            fc.Period.Index = cboPeriod.SelectedIndex;

            return fc;
        }

        #endregion

        #region PopulateFilters

        private void PopulateFilters(FilterCollection Filters)
        {
            if (Filters == null)
            {
                cbxName.Checked = false;
                cboName.SelectedIndex = 0;
                txtName.Text = string.Empty;

                cbxPerihelionDate.Checked = false;
                cboPerihelionDate.SelectedIndex = 0;
                txtPerihelionDateD.Text = string.Empty;
                txtPerihelionDateM.Text = string.Empty;
                txtPerihelionDateY.Text = string.Empty;

                cbxPerihelionDistance.Checked = false;
                cboPerihelionDistance.SelectedIndex = 2;
                txtPerihelionDistance.Text = string.Empty;

                cbxEccentricity.Checked = false;
                cboEccentricity.SelectedIndex = 2;
                txtEccentricity.Text = string.Empty;

                cbxLongOfAscendingNode.Checked = false;
                cboLongOfAscendingNode.SelectedIndex = 2;
                txtLongOfAscendingNode.Text = string.Empty;

                cbxArgumentOfPericenter.Checked = false;
                cboArgumentOfPericenter.SelectedIndex = 2;
                txtArgumentOfPericenter.Text = string.Empty;

                cbxInclination.Checked = false;
                cboInclination.SelectedIndex = 2;
                txtInclination.Text = string.Empty;

                cbxPeriod.Checked = false;
                cboPeriod.SelectedIndex = 2;
                txtPeriod.Text = string.Empty;
            }
            else
            {
                cbxName.Checked = Filters.Name.Checked;
                cboName.SelectedIndex = Filter.GetIndexFromValueResolve(Filters.Name.ValueResolve);
                txtName.Text = Filters.Name.Text;

                cbxPerihelionDate.Checked = Filters.PerihelionDate.Checked;
                cboPerihelionDate.SelectedIndex = Filter.GetIndexFromValueResolve(Filters.PerihelionDate.ValueResolve);
                if (!string.IsNullOrEmpty(Filters.PerihelionDate.Text))
                {
                    string[] date = Filters.PerihelionDate.Text.Split('.');
                    txtPerihelionDateD.Text = date[0];
                    txtPerihelionDateM.Text = date[1];
                    txtPerihelionDateY.Text = date[2];
                }
                else
                {
                    txtPerihelionDateD.Text = string.Empty;
                    txtPerihelionDateM.Text = string.Empty;
                    txtPerihelionDateY.Text = string.Empty;
                }

                cbxPerihelionDistance.Checked = Filters.PerihelionDistance.Checked;
                cboPerihelionDistance.SelectedIndex = Filter.GetIndexFromValueResolve(Filters.PerihelionDistance.ValueResolve);
                txtPerihelionDistance.Text = Filters.PerihelionDistance.Text;

                cbxEccentricity.Checked = Filters.Eccentricity.Checked;
                cboEccentricity.SelectedIndex = Filter.GetIndexFromValueResolve(Filters.Eccentricity.ValueResolve);
                txtEccentricity.Text = Filters.Eccentricity.Text;

                cbxLongOfAscendingNode.Checked = Filters.LongOfAscendingNode.Checked;
                cboLongOfAscendingNode.SelectedIndex = Filter.GetIndexFromValueResolve(Filters.LongOfAscendingNode.ValueResolve);
                txtLongOfAscendingNode.Text = Filters.LongOfAscendingNode.Text;

                cbxArgumentOfPericenter.Checked = Filters.ArgumentOfPericenter.Checked;
                cboArgumentOfPericenter.SelectedIndex = Filter.GetIndexFromValueResolve(Filters.ArgumentOfPericenter.ValueResolve);
                txtArgumentOfPericenter.Text = Filters.ArgumentOfPericenter.Text;

                cbxInclination.Checked = Filters.Inclination.Checked;
                cboInclination.SelectedIndex = Filter.GetIndexFromValueResolve(Filters.Inclination.ValueResolve);
                txtInclination.Text = Filters.Inclination.Text;

                cbxPeriod.Checked = Filters.Period.Checked;
                cboPeriod.SelectedIndex = Filter.GetIndexFromValueResolve(Filters.Period.ValueResolve);
                txtPeriod.Text = Filters.Period.Text;
            }
        }

        #endregion

        #region btnFiltersApply_Click

        private void btnFiltersApply_Click(object sender, EventArgs e)
        {
            Filters = CollectFilters(Filters);

            if (Filter.ValidateFilters(Filters))
            {
                FormMain.UserList = Filter.FilterList(FormMain.MainList, Filters);

                SortList(FormMain.UserList);

                (this.Owner as FormMain).SetStatusCometsLabel(FormMain.UserList.Count, FormMain.MainList.Count);
            }
        }

        #endregion

        #region checkBoxCommon_CheckedChanged

        private void checkBoxCommon_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control c in (sender as CheckBox).Parent.Controls)
                c.Enabled = (sender as CheckBox).Checked;

            (sender as CheckBox).Enabled = true;
        }

        #endregion

        #region btnPerihelionDateNow_Click

        private void btnPerihelionDateNow_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now.AddDays(-20);
            txtPerihelionDateD.Text = "01";
            txtPerihelionDateM.Text = dt.Month.ToString("00");
            txtPerihelionDateY.Text = dt.Year.ToString();
        }

        #endregion

        #region txtFilters_KeyPress

        private void txtFilters_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Utils.ValidateKeyPress(sender, e, 3, 4);
        }

        #endregion

        #region pnlFilters_VisibleChanged

        private void pnlFilters_VisibleChanged(object sender, EventArgs e)
        {
            btnFilters.Text = pnlFilters.Visible ? "Filters ▲" : "Filters ▼";
        }

        #endregion

        #endregion
    }
}
