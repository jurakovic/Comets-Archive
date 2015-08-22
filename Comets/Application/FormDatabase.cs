﻿using Comets.BusinessLayer.Business;
using Comets.BusinessLayer.Extensions;
using Comets.BusinessLayer.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PropertyEnum = Comets.BusinessLayer.Business.Comet.PropertyEnum;

namespace Comets.Application
{
	public partial class FormDatabase : Form
	{
		#region Properties

		public CometCollection Comets { get; private set; }

		public Comet SelectedComet
		{
			get
			{
				Comet comet = null;

				if (lbxDatabase.SelectedIndex >= 0)
					comet = Comets.ElementAt(lbxDatabase.SelectedIndex);

				return comet;
			}
		}

		public FilterCollection Filters { get; private set; }
		public string SortProperty { get; private set; }
		public bool SortAscending { get; private set; }

		private Timer Timer { get; set; }
		private EphemerisResult PreviousEphemeris { get; set; }

		#endregion

		#region Constructor

		public FormDatabase(CometCollection collection, FilterCollection filters, string sortProperty, bool sortAscending, bool isForFiltering)
		{
			InitializeComponent();

			this.mnuDesig.Tag = PropertyEnum.sortkey.ToString();
			this.mnuDiscoverer.Tag = PropertyEnum.name.ToString();
			this.mnuPerihDate.Tag = PropertyEnum.Tn.ToString();
			this.mnuPerihDist.Tag = PropertyEnum.q.ToString();
			this.mnuPerihEarthDist.Tag = PropertyEnum.PerihEarthDist.ToString();
			this.mnuPerihMag.Tag = PropertyEnum.PerihMag.ToString();
			this.mnuCurrSunDist.Tag = PropertyEnum.CurrentSunDist.ToString();
			this.mnuCurrEarthDist.Tag = PropertyEnum.CurrentEarthDist.ToString();
			this.mnuCurrMag.Tag = PropertyEnum.CurrentMag.ToString();
			this.mnuPeriod.Tag = PropertyEnum.P.ToString();
			this.mnuAphDistance.Tag = PropertyEnum.Q.ToString();
			this.mnuSemiMajorAxis.Tag = PropertyEnum.a.ToString();
			this.mnuEcc.Tag = PropertyEnum.e.ToString();
			this.mnuIncl.Tag = PropertyEnum.i.ToString();
			this.mnuAscNode.Tag = PropertyEnum.N.ToString();
			this.mnuArgPeri.Tag = PropertyEnum.w.ToString();

			Comets = new CometCollection(collection);
			Filters = filters;

			SortProperty = sortProperty;
			SortAscending = sortAscending;

			pnlDetails.Visible = !isForFiltering;
			pnlFilters.Visible = isForFiltering;

			Timer = new Timer();
			Timer.Interval = 1000;
			Timer.Tick += Timer_Tick;
		}

		#endregion

		#region FormDatabase_Load

		private void FormDatabase_Load(object sender, EventArgs e)
		{
			PopulateFilters(Filters);
			SetSortItems(SortAscending);
			SortCollection(Comets);
		}

		#endregion

		#region FormDatabase_KeyDown

		private void FormDatabase_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				if (pnlFilters.Visible)
					InvertPanelsVisibility();
				else
					this.Close();
			}
		}

		#endregion

		#region FormDatabase_FormClosing

		private void FormDatabase_FormClosing(object sender, FormClosingEventArgs e)
		{
			Filters = CollectFilters();
		}

		#endregion

		#region lbxDatabase_SelectedIndexChanged

		private void lbxDatabase_SelectedIndexChanged(object sender, EventArgs e)
		{
			Timer.Stop();

			lblEphemSunDistIndicator.Text = String.Empty;
			lblEphemSunDistIndicator.Text = String.Empty;
			lblEphemEarthDistIndicator.Text = String.Empty;
			lblEphemMagIndicator.Text = String.Empty;
			lblEphemRaIndicator.Text = String.Empty;
			lblEphemDecIndicator.Text = String.Empty;
			lblEphemAltIndicator.Text = String.Empty;
			lblEphemAzIndicator.Text = String.Empty;
			lblEphemElongationIndicator.Text = String.Empty;

			string format6 = "0.000000";
			string format4 = "0.0000";
			int minPeriod = 1000;

			Comet c = SelectedComet;
			PreviousEphemeris = EphemerisManager.GetEphemeris(c, DateTime.Now.AddMinutes(-1).JD(), FormMain.Settings.Location);

			string commonName = c.full;
			string commonPerihDist = c.q.ToString(format6);
			string commonPeriod = c.P < minPeriod ? c.P.ToString(format6) : String.Empty;
			string commonAphDist = c.P < minPeriod ? c.Q.ToString(format6) : String.Empty;

			//ephemeris
			txtInfoName.Text = commonName;

			txtInfoNextPerihDate.Text = Utils.JDToDateTime(c.Tn).ToLocalTime().ToString(FormMain.DateTimeFormat);
			txtInfoPeriod.Text = commonPeriod;
			txtInfoAphSunDist.Text = commonAphDist;

			txtInfoPerihDist.Text = commonPerihDist;
			txtInfoPerihEarthDist.Text = c.PerihEarthDist.ToString(format6);
			txtInfoPerihMag.Text = c.PerihMag.ToString("0.00");

			CalculateEphemeris();
			Timer.Start();

			//orbital elements
			txtElemName.Text = commonName;

			txtElemPerihDate.Text = String.Format("{0}-{1:00}-{2:00}.{3:0000}", c.Ty, c.Tm, c.Td, c.Th);
			txtElemPeriod.Text = commonPeriod;
			txtElemMeanMotion.Text = c.P < minPeriod ? c.n.ToString(format6) : String.Empty;

			txtElemPerihDist.Text = commonPerihDist;
			txtElemAphDist.Text = commonAphDist;
			txtElemSemiMajorAxis.Text = c.P < minPeriod ? c.a.ToString(format6) : String.Empty;

			txtElemEcc.Text = c.e.ToString(format6);
			txtElemAscNode.Text = c.N.ToString(format4);
			txtElemMagG.Text = c.g.ToString("0.0");
			txtElemMagK.Text = c.k.ToString("0.0");

			txtElemIncl.Text = c.i.ToString(format4);
			txtElemArgPeri.Text = c.w.ToString(format4);
			txtElemEquinox.Text = "2000.0";

			//t_sortKey.Text = c.sortkey.ToString("0.00000000000");
			//t_sortKey.Text = c.idKey;
		}

		#endregion

		#region lbxDatabase_MouseDoubleClick

		private void lbxDatabase_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (pnlFilters.Visible)
				InvertPanelsVisibility();
			else
				InvertTabs();
		}

		#endregion

		#region btnResetAllFilters_Click

		private void btnResetAllFilters_Click(object sender, EventArgs e)
		{
			Filters = null;
			PopulateFilters(Filters);

			SortProperty = FormMain.DefaultSortProperty;
			SortAscending = FormMain.DefaultSortAscending;

			SetSortItems(SortAscending);
			ApplyFilters();
		}

		#endregion

		#region InvertTabs

		private void InvertTabs()
		{
			if (tbcDetails.SelectedIndex == 0)
				tbcDetails.SelectedIndex = 1;
			else
				tbcDetails.SelectedIndex = 0;

			lbxDatabase.Focus();
		}

		#endregion

		#region InvertPanelsVisibility

		private void InvertPanelsVisibility()
		{
			pnlDetails.Visible = !pnlDetails.Visible;
			pnlFilters.Visible = !pnlFilters.Visible;

			lbxDatabase.Focus();
		}

		#endregion

		#region Timer

		void Timer_Tick(object sender, EventArgs e)
		{
			CalculateEphemeris();
		}

		private void CalculateEphemeris()
		{
			if (SelectedComet != null)
			{
				Comet c = SelectedComet;
				EphemerisResult er = EphemerisManager.GetEphemeris(c, DateTime.Now.JD(), FormMain.Settings.Location);

				txtInfoCurrSunDist.Text = er.SunDist.ToString("0.000000");
				txtInfoCurrEarthDist.Text = er.EarthDist.ToString("0.000000");
				txtInfoCurrMag.Text = er.Magnitude.ToString("0.00");

				txtEphemRA.Text = (EphemerisManager.HMSString(er.RA / 15.0)).Trim();
				txtEphemDec.Text = (EphemerisManager.AngleString(er.Dec, false, true)).Trim();
				txtEphemAlt.Text = er.Alt.ToString("0.00") + "°";
				txtEphemAz.Text = er.Az.ToString("0.00") + "°";
				txtEphemElongation.Text = er.Elongation.ToString("0.00") + "°" + (er.PositionAngle >= 180 ? " W" : " E");

				bool rHigher = er.SunDist >= PreviousEphemeris.SunDist;
				bool dHigher = er.EarthDist >= PreviousEphemeris.EarthDist;
				bool mHigher = er.Magnitude >= PreviousEphemeris.Magnitude;
				bool raHigher = er.RA >= PreviousEphemeris.RA;
				bool decHigher = er.Dec >= PreviousEphemeris.Dec;
				bool altHigher = er.Alt >= PreviousEphemeris.Alt;
				bool azHigher = er.Az >= PreviousEphemeris.Az;
				bool eloHigher = er.Elongation >= PreviousEphemeris.Elongation;

				lblEphemSunDistIndicator.Text = rHigher ? "▲" : "▼";
				lblEphemSunDistIndicator.ForeColor = rHigher ? Color.Red : Color.Green;

				lblEphemEarthDistIndicator.Text = dHigher ? "▲" : "▼";
				lblEphemEarthDistIndicator.ForeColor = dHigher ? Color.Red : Color.Green;

				lblEphemMagIndicator.Text = mHigher ? "▲" : "▼";
				lblEphemMagIndicator.ForeColor = mHigher ? Color.Red : Color.Green;

				lblEphemRaIndicator.Text = raHigher ? "▲" : "▼";
				lblEphemRaIndicator.ForeColor = Color.Black;

				lblEphemDecIndicator.Text = decHigher ? "▲" : "▼";

				if (FormMain.Settings.Location.Latitude >= 0)
					lblEphemDecIndicator.ForeColor = decHigher ? Color.Green : Color.Red;
				else //for southern hemisphere it is better if dec is lower -> higher on sky
					lblEphemDecIndicator.ForeColor = decHigher ? Color.Red : Color.Green;

				lblEphemAltIndicator.Text = altHigher ? "▲" : "▼";
				lblEphemAltIndicator.ForeColor = altHigher ? Color.Green : Color.Red;

				lblEphemAzIndicator.Text = azHigher ? "▲" : "▼";
				lblEphemAzIndicator.ForeColor = Color.Black;

				lblEphemElongationIndicator.Text = eloHigher ? "▲" : "▼";
				lblEphemElongationIndicator.ForeColor = eloHigher ? Color.Green : Color.Red;

				PreviousEphemeris = er;
			}
		}

		#endregion

		#region + Sort

		#region SetSortItems

		private void SetSortItems(bool isAscending)
		{
			foreach (MenuItem menuitem in contextSort.MenuItems)
			{
				if (menuitem.Tag as string == SortProperty)
					menuitem.Checked = true;
				else
					menuitem.Checked = false;
			}

			mnuAsc.Checked = isAscending;
			mnuDesc.Checked = !isAscending;
		}

		#endregion

		#region btnSort_Click

		private void btnSort_Click(object sender, EventArgs e)
		{
			Button btn = sender as Button;
			contextSort.Show(this, new Point(btn.Left + 1, btn.Top + btn.Height - 1));
		}

		#endregion

		#region menuItemSort_Click

		private void menuItemSortCommon_Click(object sender, EventArgs e)
		{
			MenuItem mni = sender as MenuItem;

			if (!mni.Checked)
			{
				SortAscending = mnuAsc.Checked;

				foreach (MenuItem item in contextSort.MenuItems)
					item.Checked = false;

				mni.Checked = true;
				SortProperty = mni.Tag as string;

				mnuAsc.Checked = SortAscending;
				mnuDesc.Checked = !SortAscending;

				SortCollection(Comets);
			}
		}

		private void menuItemSortAscDesc_Click(object sender, EventArgs e)
		{
			mnuAsc.Checked = !mnuAsc.Checked;
			mnuDesc.Checked = !mnuDesc.Checked;
			SortAscending = mnuAsc.Checked;
			SortCollection(Comets);
		}

		#endregion

		#region SortCollection

		public void SortCollection(CometCollection collection)
		{
			CometCollection temp = new CometCollection(collection);
			Comets.Clear();

			PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(Comet)).Find(SortProperty, false);

			if (prop == null)
				throw new ArgumentException(String.Format("Unknown SortPropertyName: \"{0}\"", SortProperty));

			if (SortAscending)
				Comets = new CometCollection(temp.OrderBy(x => prop.GetValue(x)));
			else
				Comets = new CometCollection(temp.OrderByDescending(x => prop.GetValue(x)));

			//clear textboxes if no comets
			if (Comets.Count == 0)
			{
				foreach (TabPage t in tbcDetails.TabPages)
					foreach (Control c in t.Controls)
						if (c is TextBox)
							c.Text = String.Empty;
						else if (c is Label && c.Name.EndsWith("Indicator"))
							c.Text = String.Empty;

				Timer.Stop();
			}

			lbxDatabase.DataSource = Comets;
			lbxDatabase.DisplayMember = "full";

			lblTotal.Text = "Comets: " + Comets.Count;
		}

		#endregion

		#endregion

		#region + Filters

		#region AddInitialFilters

		private void AddInitialFilters()
		{
			List<Control> toRemove = new List<Control>();

			foreach (Control p in pnlFilters.Controls.OfType<Panel>())
				toRemove.Add(p);

			foreach (Control p in toRemove)
				pnlFilters.Controls.Remove(p);

			btnNewFilter.Location = new Point(20, 7);
			AddNewFilterPanel(null, PropertyEnum.full);
			AddNewFilterPanel(null, PropertyEnum.Tn);
			AddNewFilterPanel(null, PropertyEnum.q);
			AddNewFilterPanel(null, PropertyEnum.CurrentMag);
		}

		#endregion

		#region AddNewFilterPanel

		private void AddNewFilterPanel(Filter filter, PropertyEnum? property)
		{
			Point location = new Point(1, 11);

			int id = 0;
			int offset = 31;

			foreach (Panel p in pnlFilters.Controls.OfType<Panel>())
			{
				int pid = p.Name.Int();
				if (pid > id)
					id = pid;

				offset = p.Height + 6; //margin
				location.Y += offset;
			}

			FilterManager.CreateFilterPanel(pnlFilters, ++id, location, FormMain.DefaultDateStart, filter, property);

			btnNewFilter.Location = new Point(btnNewFilter.Location.X, btnNewFilter.Location.Y + offset);
			btnNewFilter.Visible = id < 10;
		}

		#endregion

		#region btnNewFilter_Click

		private void btnNewFilter_Click(object sender, EventArgs e)
		{
			AddNewFilterPanel(null, PropertyEnum.full);
		}

		#endregion

		#region btnFilters_Click

		private void btnFilters_Click(object sender, EventArgs e)
		{
			InvertPanelsVisibility();
		}

		#endregion

		#region CollectFilters

		private FilterCollection CollectFilters()
		{
			FilterCollection filters = new FilterCollection();

			foreach (Panel p in pnlFilters.Controls.OfType<Panel>())
			{
				bool isChecked = false;
				int propertyIndex = 0;
				int compareIndex = 0;
				string txtStr = String.Empty;
				string txtVal = String.Empty;
				DateTime dt = DateTime.Now;

				foreach (Control c in p.Controls)
				{
					if (c is CheckBox)
						isChecked = (c as CheckBox).Checked;
					else if (c is ComboBox && c.Name == FilterManager.PropertyName)
						propertyIndex = (c as ComboBox).SelectedIndex;
					else if (c is ComboBox && c.Name == FilterManager.CompareName)
						compareIndex = (c as ComboBox).SelectedIndex;
					else if (c is TextBox && c.Name == FilterManager.StringName)
						txtStr = (c as TextBox).Text.Trim();
					else if (c is TextBox && c.Name == FilterManager.ValueName)
						txtVal = (c as TextBox).Text.Trim();
					else if (c is Button && c.Name == FilterManager.DateName)
						dt = (DateTime)(c as Button).Tag;
				}

				if (propertyIndex >= 0)
				{
					PropertyEnum property = (PropertyEnum)propertyIndex;

					Filter.DataTypeEnum dataType = FilterManager.GetDataType(property);

					string text;

					if (dataType == Filter.DataTypeEnum.String)
						text = txtStr;
					else if (property == PropertyEnum.Tn)
						text = dt.JD().ToString();
					else
						text = txtVal;

					Filter f = new Filter(property, dataType, isChecked, text, compareIndex);
					filters.Add(f);
				}
			}

			return filters;
		}

		#endregion

		#region PopulateFilters

		private void PopulateFilters(FilterCollection filters)
		{
			if (filters == null)
			{
				AddInitialFilters();
			}
			else
			{
				foreach (Filter f in filters)
				{
					AddNewFilterPanel(f, null);
				}
			}
		}

		#endregion

		#region btnFiltersApply_Click

		private void btnFiltersApply_Click(object sender, EventArgs e)
		{
			ApplyFilters();
		}

		#endregion

		#region  ApplyFilters

		private void ApplyFilters()
		{
			Filters = CollectFilters();

			if (FilterManager.ValidateFilters(Filters))
			{
				if (Filters.Any(x => x.Checked))
					Comets = FilterManager.FilterCollection(FormMain.MainCollection, Filters);
				else
					Comets = new CometCollection(FormMain.MainCollection);

				SortCollection(Comets);
			}
		}

		#endregion

		#region pnlFilters_VisibleChanged

		private void pnlFilters_VisibleChanged(object sender, EventArgs e)
		{
			this.btnFilters.Text = pnlFilters.Visible ? "Filters ▲" : "Filters ▼";
			this.btnOk.TabStop = this.btnCancel.TabStop = pnlDetails.Visible;
			this.AcceptButton = pnlFilters.Visible ? btnFiltersApply : btnOk;
		}

		#endregion

		#region CreateParams

		protected override CreateParams CreateParams
		{
			//http://stackoverflow.com/questions/2612487/how-to-fix-the-flickering-in-user-controls
			//http://social.msdn.microsoft.com/forums/en-US/winforms/thread/aaed00ce-4bc9-424e-8c05-c30213171c2c/

			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
				return cp;
			}
		}

		#endregion

		#endregion
	}
}
