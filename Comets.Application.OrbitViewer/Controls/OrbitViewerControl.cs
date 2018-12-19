﻿using Comets.Core;
using Comets.Core.Extensions;
using Comets.Core.Managers;
using Comets.OrbitViewer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Object = Comets.OrbitViewer.Object;

namespace Comets.Application.OrbitViewer
{
	public partial class OrbitViewerControl : UserControl
	{
		#region Consts

		const int DefaultScrollVert = 40;
		const int DefaultScrollHorz = 75;
		const int DefaultScrollZoom = 145;

		readonly string[] TimeStepItems =
		{
			"1 Hour",
			"6 Hours",
			"12 Hours",
			"1 Day",
			"3 Days",
			"10 Days",
			"1 Month",
			"3 Months",
			"6 Months",
			"1 Year",
			"3 Years",
			"10 Years"
		};

		readonly ATimeSpan[] TimeStepSpan =
		{
			new ATimeSpan( 0, 0, 0, 1, 0, 0),
			new ATimeSpan( 0, 0, 0, 6, 0, 0),
			new ATimeSpan( 0, 0, 0,12, 0, 0),
			new ATimeSpan( 0, 0, 1, 0, 0, 0),
			new ATimeSpan( 0, 0, 3, 0, 0, 0),
			new ATimeSpan( 0, 0,10, 0, 0, 0),
			new ATimeSpan( 0, 1, 0, 0, 0, 0),
			new ATimeSpan( 0, 3, 0, 0, 0, 0),
			new ATimeSpan( 0, 6, 0, 0, 0, 0),
			new ATimeSpan( 1, 0, 0, 0, 0, 0),
			new ATimeSpan( 3, 0, 0, 0, 0, 0),
			new ATimeSpan(10, 0, 0, 0, 0, 0)
		};


		const string LabelPart = "Label";
		const string OrbitPart = "Orbit";
		const string CenterPart = "Center";

		#endregion

		#region Fields

		private bool IsLeftButtonMoving;
		private bool IsKeyboardScroll;
		private bool IsMouseWheelZoom;
		private Point StartDrag;

		private Timer Timer;
		private ATimeSpan TimeStep;

		private bool ValueChangedInternal;

		private CometCollection Comets;
		private FilterCollection Filters;
		private string SortProperty;
		private bool SortAscending;

		private List<OVComet> OVComets;

		private DateTime DefaultDateTime;

		#endregion

		#region Properties

		private OVComet SelectedComet
		{
			get { return OVComets?.ElementAtOrDefault(cboComet.SelectedIndex); }
			set { cboComet.SelectedIndex = OVComets.IndexOf(value); }
		}

		private DateTime _selectedDateTime;
		private DateTime SelectedDateTime
		{
			get { return _selectedDateTime; }
			set
			{
				bool isOutOfRange = FormDateTime.RangeDateTime(value, out _selectedDateTime);
				btnDate.Text = _selectedDateTime.ToString(DateTimeFormat.Full);

				if (isOutOfRange || (IsSimulationStarted && !ValueChangedInternal))
					StopSimulation();

				if (orbitPanel.IsPaintEnabled)
				{
					orbitPanel.ATime = new ATime(_selectedDateTime, _selectedDateTime.Timezone());
					RefreshPanel();
				}
			}
		}

		private bool _toolboxVisible = true;
		public bool ToolboxVisible
		{
			get { return _toolboxVisible; }
			private set { _toolboxVisible = value; }
		}

		private bool _isSimulationForward = true;
		private bool IsSimulationForward
		{
			get { return _isSimulationForward; }
			set { _isSimulationForward = value; }
		}

		private bool IsSimulationStarted
		{
			get { return Timer != null && Timer.Enabled; }
		}

		#endregion

		#region Constructor

		public OrbitViewerControl()
		{
			InitializeComponent();
		}

		#endregion

		#region LoadControl

		public void LoadControl(CometCollection comets, FilterCollection filters, string sortProperty, bool sortAscending)
		{
			txtFodSunDist.Tag = new ValNum(0.0, 99.99, 2);
			txtFodEarthDist.Tag = new ValNum(0.0, 99.99, 2);
			txtFodMagnitude.Tag = ValNum.VMagnitude;

			Timer = new Timer();
			Timer.Interval = 50;
			Timer.Tick += new EventHandler(this.timer_Tick);

			Comets = comets;
			Filters = filters;
			SortProperty = sortProperty;
			SortAscending = sortAscending;

			OVComets = TransformComets(comets);
			DefaultDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0, DateTimeKind.Local);

			ValueChangedInternal = true;

			scrollVert.Value = DefaultScrollVert;
			scrollHorz.Value = DefaultScrollHorz;
			scrollZoom.Value = DefaultScrollZoom;

			SelectedDateTime = DefaultDateTime;

			ValueChangedInternal = false;

			BindCollection();

			cboTimestep.DataSource = TimeStepItems;
			cboTimestep.SelectedIndex = 3;

			ATime atime = new ATime(SelectedDateTime, SelectedDateTime.Timezone());
			orbitPanel.LoadPanel(SelectedComet, atime);
			RefreshPanel();
		}

		#endregion

		#region +EventHandling

		#region OrbitPanel

		private void orbitPanel_Resize(object sender, EventArgs e)
		{
			orbitPanel.Image = new Bitmap(orbitPanel.Width, orbitPanel.Height);
			RefreshPanel();
		}

		#endregion

		#region + ToolBox

		#region Toggle

		private void lblCometToggle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Control linkLabel = sender as Control;
			GroupBox gbx = linkLabel.Parent as GroupBox;
			Panel pnl = gbx.Controls.OfType<Panel>().Single();

			int minHeight = 30;
			int maxHeight = gbx.Tag.ToString().Int();
			int offset = maxHeight - minHeight;

			pnl.Visible = !pnl.Visible;
			gbx.Height = pnl.Visible ? maxHeight : minHeight;

			if (!pnl.Visible)
				offset = -offset;

			foreach (Control c in pnlToolbox.Controls.OfType<GroupBox>())
				if (c.Top > gbx.Top)
					c.Top += offset;
		}

		#endregion

		#region Comet

		private void cboObject_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ValueChangedInternal)
			{
				orbitPanel.LoadPanel(SelectedComet, orbitPanel.ATime);
				SetFormText();
				RefreshPanel();
			}
		}

		private void btnFilter_Click(object sender, EventArgs e)
		{
			FilterComets();
		}

		private void btnAll_Click(object sender, EventArgs e)
		{
			LoadAllComets();
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			ClearComets();
		}

		private void comboBoxCommon_MouseHover(object sender, EventArgs e)
		{
			(sender as ComboBox).Focus();
		}

		private void FindComet()
		{
			using (FormFind ff = new FormFind(Comets))
			{
				ff.TopMost = this.ParentForm.TopMost;

				Point panelLocation = this.orbitPanel.PointToScreen(Point.Empty);
				Size margin = new Size(7, 7);
				ff.Location = panelLocation + margin;

				if (ff.ShowDialog() == DialogResult.OK && ff.SelectedComet != null)
					SelectedComet = OVComets.First(x => x.Name == ff.SelectedComet.full);
			}
		}

		private void FilterComets()
		{
			bool simStarted = IsSimulationStarted;

			string lastSelected = SelectedComet?.Name;

			using (FormDatabase fdb = new FormDatabase(CommonManager.MainCollection, Filters, SortProperty, SortAscending, true) { Owner = this.ParentForm })
			{
				fdb.TopMost = this.ParentForm.TopMost;

				StopSimulation();

				if (fdb.ShowDialog() == DialogResult.OK)
				{
					orbitPanel.ClearComets(true);

					Comets = fdb.CometsFiltered;
					Filters = fdb.Filters;
					SortProperty = fdb.SortProperty;
					SortAscending = fdb.SortAscending;

					OVComets = TransformComets(Comets);

					BindCollection(lastSelected);

					if (rbtnMultipleMode.Checked)
						orbitPanel.LoadPanel(OVComets.ToList(), orbitPanel.ATime, cboComet.SelectedIndex);

					SetFormText();
					RefreshPanel();
				}
			}

			if (simStarted)
				StartSimulation();
		}

		private void LoadAllComets()
		{
			if (OVComets.Count > 0 && orbitPanel.Comets.Count != OVComets.Count)
			{
				orbitPanel.LoadPanel(OVComets.ToList(), orbitPanel.ATime, cboComet.SelectedIndex);

				SetFormText();

				ValueChangedInternal = true;
				rbtnMultipleMode.Checked = true;
				ValueChangedInternal = false;

				RefreshPanel();
			}
		}

		private void ClearComets()
		{
			orbitPanel.ClearComets();
			SetFormText();
			RefreshPanel();
		}

		#endregion

		#region Mode

		private void rbtnMode_CheckedChanged(object sender, EventArgs e)
		{
			bool tempChanged = ValueChangedInternal;
			ValueChangedInternal = true;

			orbitPanel.MultipleMode = rbtnMultipleMode.Checked;

			ValueChangedInternal = tempChanged;

			SetFormText();

			if (!ValueChangedInternal)
				RefreshPanel();
		}

		#endregion

		#region Orbits, Labels, Center

		private void btnNoOrbits_Click(object sender, EventArgs e)
		{
			SetMultipleCheckBoxes(OrbitPart, false, true);
		}

		private void btnAllOrbits_Click(object sender, EventArgs e)
		{
			SetMultipleCheckBoxes(OrbitPart, true, true);
		}

		private void btnNoLabels_Click(object sender, EventArgs e)
		{
			SetMultipleCheckBoxes(LabelPart, false, true);
		}

		private void btnAllLabels_Click(object sender, EventArgs e)
		{
			SetMultipleCheckBoxes(LabelPart, true, true);
		}

		private void btnAllOrbitsLabels_Click(object sender, EventArgs e)
		{
			SetMultipleCheckBoxes(OrbitPart, true);
			SetMultipleCheckBoxes(LabelPart, true);
			RefreshPanel();
		}

		private void btnDefaultOrbitsLabels_Click(object sender, EventArgs e)
		{
			SetMultipleCheckBoxes(OrbitPart, true);
			SetMultipleCheckBoxes(LabelPart, true);

			ValueChangedInternal = true;
			cbxOrbitSaturn.Checked = false;
			cbxOrbitUranus.Checked = false;
			cbxOrbitNeptune.Checked = false;
			cbxOrbitComet.Checked = false;
			cbxLabelComet.Checked = false;
			rbtnCenterSun.Checked = true;
			cbxSelectedOrbit.Checked = true;
			cbxSelectedLabel.Checked = true;
			ValueChangedInternal = false;

			RefreshPanel();
		}

		private void SetMultipleCheckBoxes(string namePart, bool isChecked, bool refresh = false)
		{
			ValueChangedInternal = true;

			foreach (CheckBox c in pnlOrbitsLabelsCenter.Controls.OfType<CheckBox>())
			{
				if (c.Enabled && c.Name.Contains(namePart))
					c.Checked = isChecked;
			}

			ValueChangedInternal = false;

			if (refresh)
				RefreshPanel();
		}

		private void cbxOrbitCommon_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox cbx = sender as CheckBox;
			string name = cbx.Name.Replace("cbx" + OrbitPart, "");
			Object orbit = (Object)Enum.Parse(typeof(Object), name);

			if (cbx.Checked && !orbitPanel.OrbitDisplay.Contains(orbit))
				orbitPanel.OrbitDisplay.Add(orbit);
			else
				orbitPanel.OrbitDisplay.Remove(orbit);

			if (!ValueChangedInternal)
				RefreshPanel();
		}

		private void cbxLabelCommon_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox cbx = sender as CheckBox;
			string name = cbx.Name.Replace("cbx" + LabelPart, "");
			Object label = (Object)Enum.Parse(typeof(Object), name);

			if (cbx.Checked && !orbitPanel.LabelDisplay.Contains(label))
				orbitPanel.LabelDisplay.Add(label);
			else
				orbitPanel.LabelDisplay.Remove(label);

			if (!ValueChangedInternal)
				RefreshPanel();
		}

		private void rbtnCenterCommon_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton rbtn = sender as RadioButton;
			if (rbtn.Checked)
			{
				string name = rbtn.Name.Replace("rbtn" + CenterPart, "");
				Object centerObject = (Object)Enum.Parse(typeof(Object), name);
				orbitPanel.CenteredObject = centerObject;

				if (!ValueChangedInternal)
					RefreshPanel();
			}
		}

		private void cbxOrbit_CheckedChanged(object sender, EventArgs e)
		{
			orbitPanel.PreserveSelectedOrbit = cbxSelectedOrbit.Checked;

			if (!ValueChangedInternal)
				RefreshPanel();
		}

		private void cbxLabel_CheckedChanged(object sender, EventArgs e)
		{
			orbitPanel.PreserveSelectedLabel = cbxSelectedLabel.Checked;

			if (!ValueChangedInternal)
				RefreshPanel();
		}

		private void cbxMarker_CheckedChanged(object sender, EventArgs e)
		{
			orbitPanel.ShowMarker = cbxMarker.Checked;

			if (!ValueChangedInternal)
				RefreshPanel();
		}

		private void ChangeObjectDisplay(Object obj, bool control, bool shift)
		{
			ValueChangedInternal = true;

			if (!control && !shift)
				ChangeCenterObject(obj);
			else if (control && !shift)
				ChangeVisibleOrbit(obj);
			else if (shift && !control)
				ChangeVisibleLabel(obj);

			ValueChangedInternal = false;

			RefreshPanel();
		}

		private void ChangeVisibleOrbit(Object orbit)
		{
			CheckBox cbx = GetControlBoxFromObjectEnum(orbit, typeof(CheckBox), false) as CheckBox;

			if (cbx != null && cbx.Enabled)
				cbx.Checked = !orbitPanel.OrbitDisplay.Contains(orbit);
		}

		private void ChangeVisibleLabel(Object label)
		{
			CheckBox cbx = GetControlBoxFromObjectEnum(label, typeof(CheckBox)) as CheckBox;

			if (cbx != null && cbx.Enabled)
				cbx.Checked = !orbitPanel.LabelDisplay.Contains(label);
		}

		private void ChangeCenterObject(Object centeredObject)
		{
			RadioButton rbtn = GetControlBoxFromObjectEnum(centeredObject, typeof(RadioButton)) as RadioButton;

			if (rbtn != null && rbtn.Enabled)
				rbtn.Checked = true;
		}

		private Control GetControlBoxFromObjectEnum(Object obj, Type type, bool isLabel = true)
		{
			List<Control> controls = new List<Control>();
			Control control = null;

			foreach (Control c in pnlOrbitsLabelsCenter.Controls)
				if (c.Name.EndsWith(obj.ToString()))
					controls.Add(c);

			string name;

			if (type == typeof(CheckBox) && isLabel)
				name = LabelPart;
			else if (type == typeof(CheckBox) && !isLabel)
				name = OrbitPart;
			else //if(type == typeof(RadioButton))
				name = CenterPart;

			control = controls.First(x => x.GetType() == type && x.Name.Contains(name));

			return control;
		}

		#endregion

		#region Date and Time

		private void btnDate_Click(object sender, EventArgs e)
		{
			ShowDateTimeForm();
		}

		private void ShowDateTimeForm()
		{
			DateTime? perihelionDate = EphemerisManager.JDToLocalDateTimeSafe(SelectedComet.T);

			using (FormDateTime fdt = new FormDateTime(SelectedDateTime, DefaultDateTime, perihelionDate))
			{
				fdt.TopMost = this.ParentForm.TopMost;

				if (fdt.ShowDialog() == DialogResult.OK)
					SelectedDateTime = fdt.SelectedDateTime;
			}
		}

		private void btnNow_Click(object sender, EventArgs e)
		{
			SelectedDateTime = DateTime.Now;
		}

		private void btnPerihDate_Click(object sender, EventArgs e)
		{
			if (SelectedComet != null)
				SelectedDateTime = EphemerisManager.JDToDateTime(SelectedComet.T).ToLocalTime();
		}

		#endregion

		#region Simulation

		private void btnRevPlay_Click(object sender, EventArgs e)
		{
			StartSimulation(false);
		}

		private void btnRevStep_Click(object sender, EventArgs e)
		{
			StopSimulation();
			ChangeDate(false);
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			StopSimulation();
		}

		private void btnForStep_Click(object sender, EventArgs e)
		{
			StopSimulation();
			ChangeDate(true);
		}

		private void btnForPlay_Click(object sender, EventArgs e)
		{
			StartSimulation(true);
		}

		private void cboTimestep_SelectedIndexChanged(object sender, EventArgs e)
		{
			TimeStep = TimeStepSpan[cboTimestep.SelectedIndex];
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			ChangeDate(IsSimulationForward);
		}

		private void ChangeDate(bool isForward)
		{
			ATime atime = orbitPanel.ATime;
			atime.ChangeDate(TimeStep, isForward);

			ValueChangedInternal = true;
			SelectedDateTime = new DateTime(atime.Year, atime.Month, atime.Day, atime.Hour, atime.Minute, atime.Second, DateTimeKind.Utc);
			ValueChangedInternal = false;
		}

		private void StartSimulation(bool? isForward = null)
		{
			if (isForward != null)
				IsSimulationForward = isForward.Value;

			Timer.Start();
		}

		public void StopSimulation()
		{
			Timer.Stop();
		}

		private void FasterSimulation()
		{
			if (!IsSimulationStarted)
			{
				StartSimulation();
			}
			else
			{
				if (cboTimestep.SelectedIndex < cboTimestep.Items.Count - 1)
				{
					cboTimestep.SelectedIndex++;
				}
			}
		}

		private void SlowerSimulation()
		{
			if (!IsSimulationStarted)
			{
				StartSimulation();
			}
			else
			{
				if (cboTimestep.SelectedIndex > 0)
					cboTimestep.SelectedIndex--;
				else
					StopSimulation();
			}
		}

		#endregion

		#region Other

		private void cbxShowAxes_CheckedChanged(object sender, EventArgs e)
		{
			orbitPanel.ShowAxes = cbxShowAxes.Checked;
			RefreshPanel();
		}

		private void cbxAntialiasing_CheckedChanged(object sender, EventArgs e)
		{
			orbitPanel.Antialiasing = cbxAntialiasing.Checked;
			RefreshPanel();
		}

		private void cbxMagDist_CheckedChanged(object sender, EventArgs e)
		{
			orbitPanel.ShowDistance = cbxMagDist.Checked;
			RefreshPanel();
		}

		private void cbxDateTime_CheckedChanged(object sender, EventArgs e)
		{
			orbitPanel.ShowDate = cbxDateTime.Checked;
			RefreshPanel();
		}

		private void btnSaveImage_Click(object sender, EventArgs e)
		{
			Save();
		}

		#endregion

		#region Filter on date

		private void filterOnDateTxtCbxCommon_TextChangedCheckedChanged(object sender, EventArgs e)
		{
			if (!ValueChangedInternal)
			{
				TextBox txt = null;
				CheckBox cbx = null;

				bool isTxt = false;
				bool isCbx = false;

				string namePart = null;
				double? value = null;

				if (sender is TextBox)
				{
					txt = sender as TextBox;
					namePart = txt.Name.Replace("txt", String.Empty);
					cbx = pnlFilterOnDate.Controls.Find("cbx" + namePart, false).Single() as CheckBox;
					isTxt = true;
				}

				if (sender is CheckBox)
				{
					cbx = sender as CheckBox;
					namePart = cbx.Name.Replace("cbx", String.Empty);
					txt = pnlFilterOnDate.Controls.Find("txt" + namePart, false).Single() as TextBox;
					isCbx = true;
				}

				if (!String.IsNullOrEmpty(txt.Text) && (isTxt || (isCbx && cbx.Checked)))
					value = txt.Double();

				switch (namePart)
				{
					case "FodSunDist":
						orbitPanel.FilterOnDateSunDist = value;
						break;
					case "FodEarthDist":
						orbitPanel.FilterOnDateEarthDist = value;
						break;
					case "FodMagnitude":
						orbitPanel.FilterOnDateMagnitude = value;
						break;
					default:
						throw new NotImplementedException(txt.Name);
				}

				if (isTxt)
				{
					ValueChangedInternal = true;
					cbx.Checked = value != null;
					ValueChangedInternal = false;
				}

				RefreshPanel();
			}
		}

		private void txtFilterOnDateCommon_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = ValNumManager.HandleKeyPress(sender, e);
		}

		private void cbxWeakColor_CheckedChanged(object sender, EventArgs e)
		{
			orbitPanel.FilterOnDateShowInWeakColor = cbxWeakColor.Checked;
			RefreshPanel();
		}

		#endregion

		#endregion

		#region Keyboad shortcuts

		private void orbitPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Up:
				case Keys.Down:
				case Keys.Left:
				case Keys.Right:
					e.IsInputKey = true;
					break;
			}
		}

		public void OrbitViewerControl_KeyDown(object sender, KeyEventArgs e)
		{
			if ((txtFodSunDist.Focused || txtFodEarthDist.Focused || txtFodMagnitude.Focused)
				&& e.KeyCode.In(Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0, Keys.Back))
				return;

			bool handled = false;
			bool ctrl = (Control.ModifierKeys & Keys.Control) == Keys.Control;
			bool shift = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;

			switch (e.KeyCode)
			{
				case Keys.Menu:
					handled = false;
					break;
				case Keys.Left:
					if (!ctrl && !shift)
						handled = MoveScroll(scrollHorz, false);
					break;
				case Keys.Right:
					if (!ctrl && !shift)
						handled = MoveScroll(scrollHorz, true);
					break;

				case Keys.Up:
					if (!ctrl && !shift)
						handled = MoveScroll(scrollVert, false);
					break;
				case Keys.Down:
					if (!ctrl && !shift)
						handled = MoveScroll(scrollVert, true);
					break;

				case Keys.Add:
				case Keys.Q:
					if (!ctrl && !shift)
						handled = MoveScroll(scrollZoom, true, false);
					break;
				case Keys.Subtract:
					if (!ctrl && !shift)
						handled = MoveScroll(scrollZoom, false, false);
					break;
				case Keys.A:
					if (!ctrl && !shift)
						handled = MoveScroll(scrollZoom, false, false);
					else if (ctrl && shift)
						LoadAllComets();
					break;

				case Keys.D1:
					ChangeObjectDisplay(Object.Mercury, ctrl, shift);
					handled = true;
					break;

				case Keys.D2:
					ChangeObjectDisplay(Object.Venus, ctrl, shift);
					handled = true;
					break;

				case Keys.D3:
					ChangeObjectDisplay(Object.Earth, ctrl, shift);
					handled = true;
					break;

				case Keys.D4:
					ChangeObjectDisplay(Object.Mars, ctrl, shift);
					handled = true;
					break;

				case Keys.D5:
					ChangeObjectDisplay(Object.Jupiter, ctrl, shift);
					handled = true;
					break;

				case Keys.D6:
					ChangeObjectDisplay(Object.Saturn, ctrl, shift);
					handled = true;
					break;

				case Keys.D7:
					ChangeObjectDisplay(Object.Uranus, ctrl, shift);
					handled = true;
					break;

				case Keys.D8:
					ChangeObjectDisplay(Object.Neptune, ctrl, shift);
					handled = true;
					break;

				case Keys.D9:
					if (!ctrl && !shift)
					{
						ChangeObjectDisplay(Object.Comet, ctrl, shift);
						handled = true;
					}
					break;

				case Keys.C:
					if (!ctrl && !shift)
					{
						ChangeObjectDisplay(Object.Comet, ctrl, shift);
						handled = true;
					}
					else if (ctrl && shift)
					{
						ClearComets();
						handled = true;
					}
					break;

				case Keys.D0:
				case Keys.S:
					if (!ctrl && !shift)
					{
						ChangeCenterObject(Object.Sun);
						handled = true;
					}
					break;

				case Keys.Space:
				case Keys.P:
					if (!ctrl && !shift)
					{
						if (IsSimulationStarted)
							StopSimulation();
						else
							StartSimulation();

						handled = true;
					}
					break;

				case Keys.J:
					if (!ctrl && !shift)
					{
						// invert simulation
						IsSimulationForward = !IsSimulationForward;
						handled = true;
					}
					break;

				case Keys.K:
					if (!ctrl && !shift)
					{
						SlowerSimulation();
						handled = true;
					}
					break;

				case Keys.L:
					if (!ctrl && !shift)
					{
						FasterSimulation();
						handled = true;
					}
					break;

				case Keys.D:
					if (ctrl && !shift)
					{
						ShowDateTimeForm();
						handled = true;
					}
					break;

				case Keys.B:
					if (ctrl && !shift && SelectedComet != null)
					{
						SelectedDateTime = EphemerisManager.JDToDateTime(SelectedComet.T).ToLocalTime();
						handled = true;
					}
					break;

				case Keys.N:
					if (ctrl && !shift)
					{
						SelectedDateTime = DateTime.Now;
						handled = true;
					}
					break;

				case Keys.G:
					if (!ctrl && !shift)
					{
						cbxSelectedOrbit.Invert();
						handled = true;
					}
					break;

				case Keys.H:
					if (!ctrl && !shift)
					{
						cbxSelectedLabel.Invert();
						handled = true;
					}
					break;

				case Keys.M:
					if (!ctrl && !shift)
					{
						cbxMarker.Invert();
						handled = true;
					}
					break;

				case Keys.Enter:
					if (!ctrl && !shift && orbitPanel.MultipleMode)
					{
						if (SelectedComet != null)
							SelectedComet.IsMarked = !SelectedComet.IsMarked;

						if (orbitPanel.IsPaintEnabled && !IsSimulationStarted)
							RefreshPanel();

						handled = true;
					}
					break;

				case Keys.Back:
					if (!ctrl && !shift)
					{
						SelectedComet = null;
						handled = true;
					}
					break;

				case Keys.Delete:
					if (!ctrl && !shift && orbitPanel.MultipleMode)
					{
						OVComets.ForEach(x => x.IsMarked = false);

						if (orbitPanel.IsPaintEnabled && !IsSimulationStarted)
							RefreshPanel();

						handled = true;
					}
					break;

				case Keys.F:
					if (ctrl && !shift)
					{
						FindComet();
						handled = true;
					}
					else if (ctrl && shift)
					{
						FilterComets();
						handled = true;
					}
					break;

				case Keys.I:
					if (ctrl && !shift && SelectedComet != null)
					{
						Comet c = Comets.ElementAt(cboComet.SelectedIndex);
						CometManager.OpenJplInfo(c.id);
						handled = true;
					}
					break;
				default:
					handled = !(cboComet.Focused || cboTimestep.Focused);
					break;
			}

			e.Handled = handled;
		}

		private bool MoveScroll(ScrollBar scrollbar, bool isIncrement, bool continuous = true)
		{
			if (IsKeyboardScroll)
			{
				int value = scrollbar.LargeChange * (isIncrement ? 1 : -1);
				int newValue = scrollbar.Value + value;

				if (continuous)
				{
					while (newValue > scrollbar.Maximum)
						newValue = scrollbar.Minimum;

					while (newValue < scrollbar.Minimum)
						newValue = scrollbar.Maximum;
				}
				else
				{
					if (newValue > scrollbar.Maximum)
						newValue = scrollbar.Maximum;

					if (newValue < scrollbar.Minimum)
						newValue = scrollbar.Minimum;
				}

				scrollbar.Value = newValue;
			}

			return IsKeyboardScroll;
		}

		#endregion

		#region Mouse Controls

		private void orbitPanel_MouseEnter(object sender, EventArgs e)
		{
			IsMouseWheelZoom = true;
			IsKeyboardScroll = true;
		}

		private void orbitPanel_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				StartDrag = e.Location;
		}

		private void orbitPanel_MouseLeave(object sender, EventArgs e)
		{
			IsMouseWheelZoom = false;
			IsKeyboardScroll = false;
		}

		private void orbitPanel_MouseClick(object sender, MouseEventArgs e)
		{
			if (!IsLeftButtonMoving && e.Button == MouseButtons.Left && orbitPanel.MultipleMode)
			{
				string name = orbitPanel.SelectComet(e.Location);
				SelectedComet = OVComets.FirstOrDefault(x => x.Name == name);
			}
		}

		private void orbitPanel_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				string name = orbitPanel.SelectComet(e.Location);

				if (name != null)
				{
					ValueChangedInternal = true;
					rbtnCenterComet.Checked = true;

					bool isCometCentered = orbitPanel.CenterSelectedComet();

					if (!isCometCentered)
					{
						if (cbxSelectedOrbit.Checked && !cbxSelectedLabel.Checked)
						{
							cbxSelectedLabel.Checked = true;
						}
						else if (cbxSelectedLabel.Checked && !cbxSelectedOrbit.Checked)
						{
							cbxSelectedOrbit.Checked = true;
						}
						else
						{
							cbxSelectedOrbit.Invert();
							cbxSelectedLabel.Invert();
						}
					}

					ValueChangedInternal = false;
					RefreshPanel();
				}
			}
		}

		private void orbitPanel_MouseMove(object sender, MouseEventArgs e)
		{
			if (!orbitPanel.Focused)
				orbitPanel.Focus();

			IsLeftButtonMoving = e.Button == MouseButtons.Left;

			if (e.Button == MouseButtons.Right)
			{
				double xRatio = orbitPanel.MinimumSize.Width / scrollHorz.Maximum;
				double yRatio = orbitPanel.MinimumSize.Height / scrollVert.Maximum;

				int deltaX = e.X - StartDrag.X;
				int deltaY = e.Y - StartDrag.Y;

				double koef = 0.3;

				double newHv = xRatio * deltaX * koef;
				double newVv = yRatio * deltaY * koef;

				if (newHv > 0 && newHv < 1) newHv = 1;
				else if (newHv < 0 && newHv > -1) newHv = -1;
				else if (newVv > 0 && newVv < 1) newVv = 1;
				else if (newVv < 0 && newVv > -1) newVv = -1;

				int newHorizValue = scrollHorz.Value + (int)newHv;
				int newVertValue = scrollVert.Value + (int)newVv;

				while (newHorizValue > scrollHorz.Maximum)
					newHorizValue -= scrollHorz.Maximum;

				while (newHorizValue < scrollHorz.Minimum)
					newHorizValue += scrollHorz.Maximum;

				while (newVertValue > scrollVert.Maximum)
					newVertValue -= scrollVert.Maximum;

				while (newVertValue < scrollVert.Minimum)
					newVertValue += scrollVert.Maximum;

				scrollHorz.Value = newHorizValue;
				scrollVert.Value = newVertValue;

				StartDrag = e.Location;
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (IsMouseWheelZoom)
			{
				double koef = e.Delta < 0 ? 0.23 : 0.30;
				int value = (int)(scrollZoom.Value * koef);
				int newZvalue = scrollZoom.Value + (e.Delta < 0 ? -value : value);

				if (newZvalue < scrollZoom.Minimum)
					scrollZoom.Value = scrollZoom.Minimum;
				else if (newZvalue > scrollZoom.Maximum)
					scrollZoom.Value = scrollZoom.Maximum;
				else
					scrollZoom.Value = newZvalue;
			}
		}

		#endregion

		#region Scrollbars

		private void scrollVert_ValueChanged(object sender, EventArgs e)
		{
			orbitPanel.RotateVert = (double)(90 - scrollVert.Value);

			if (!ValueChangedInternal)
				RefreshPanel();
		}

		private void scrollHorz_ValueChanged(object sender, EventArgs e)
		{
			orbitPanel.RotateHorz = (double)(90 - scrollHorz.Value);

			if (!ValueChangedInternal)
				RefreshPanel();
		}

		private void scrollZoom_ValueChanged(object sender, EventArgs e)
		{
			orbitPanel.Zoom = (double)scrollZoom.Value;

			if (!ValueChangedInternal)
				RefreshPanel();
		}

		#endregion

		#endregion

		#region Methods

		private void BindCollection(string name = null)
		{
			ValueChangedInternal = true;

			cboComet.DisplayMember = "Name";
			cboComet.DataSource = OVComets;

			if (OVComets.Count > 0)
			{
				if (name != null && OVComets.Any(x => x.Name == name))
					SelectedComet = OVComets.First(x => x.Name == name);
				else
					SelectedComet = OVComets.OrderBy(x => Math.Abs(x.T - DateTime.Now.JD())).First(); //comet with nearest perihelion date
			}

			ValueChangedInternal = false;
		}

		private void RefreshPanel()
		{
			orbitPanel.Invalidate();
		}

		public void ShowToolbox(bool visible)
		{
			ToolboxVisible = visible;
			pnlToolbox.Visible = visible;
		}

		private List<OVComet> TransformComets(CometCollection comets)
		{
			List<OVComet> list = new List<OVComet>();
			comets.ForEach(x => list.Add(new OVComet(x)));
			return list;
		}

		public void Save()
		{
			using (SaveFileDialog sfd = new SaveFileDialog())
			{
				string lastExportDir = CommonManager.Settings.LastUsedExportDirectory;

				sfd.InitialDirectory = !String.IsNullOrEmpty(lastExportDir) ? lastExportDir : Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				sfd.FileName = "Comets_OrbitViewer_" + DateTime.Now.ToString(DateTimeFormat.Filename);
				sfd.Filter = "BMP (*.bmp)|*.bmp|" +
							"GIF (*.gif)|*.gif|" +
							"JPEG (*.jpg, *.jpeg, *.jpe, *.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif|" +
							"PNG (*.png)|*.png";
				sfd.FilterIndex = 4;

				if (sfd.ShowDialog() == DialogResult.OK)
				{
					ImageFormat format;

					switch (sfd.FilterIndex)
					{
						case 1:
							format = ImageFormat.Bmp;
							break;
						case 2:
							format = ImageFormat.Gif;
							break;
						case 3:
							format = ImageFormat.Jpeg;
							break;
						default:
							format = ImageFormat.Png;
							break;
					}

					Bitmap bmp = new Bitmap(this.orbitPanel.Width, this.orbitPanel.Height);
					this.orbitPanel.DrawToBitmap(bmp, this.orbitPanel.DisplayRectangle);
					bmp.Save(sfd.FileName, format);
					CommonManager.Settings.LastUsedExportDirectory = Path.GetDirectoryName(sfd.FileName);
					MessageBox.Show(String.Format("Orbit saved as {0}\t\t\t", sfd.FileName), "Comets", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		private void SetFormText()
		{
			string text = "Orbit Viewer";

			if (SelectedComet != null)
				text += " - " + SelectedComet.Name;

			if (orbitPanel.MultipleMode && orbitPanel.Comets.Count > 1)
				text += " (" + orbitPanel.Comets.Count + " comets)";

			this.ParentForm.Text = text;
		}

		#endregion
	}
}
