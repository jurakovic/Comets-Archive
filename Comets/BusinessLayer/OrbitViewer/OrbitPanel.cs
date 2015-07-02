﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Linq;

namespace Comets.OrbitViewer
{
	public class OrbitPanel : Panel
	{
		#region Enums

		public enum CenteredObjectEnum
		{
			Sun = 0,
			CometAsteroid,
			Mercury,
			Venus,
			Earth,
			Mars,
			Jupiter,
			Saturn,
			Uranus,
			Neptune
			//Pluto
		}

		public enum OrbitsEnum
		{
			Default = 0,
			AllOrbits,
			NoOrbits,
			Space, //------
			CometAsteroid,
			Mercury,
			Venus,
			Earth,
			Mars,
			Jupiter,
			Saturn,
			Uranus,
			Neptune
			//Pluto
		}

		public enum OrbitDisplayEnum
		{
			CometAsteroid = 0,
			Mercury,
			Venus,
			Earth,
			Mars,
			Jupiter,
			Saturn,
			Uranus,
			Neptune
			//Pluto
		}

		#endregion

		#region Const

		private readonly int MaxNumberOfComets = 20;

		#endregion

		#region Properties

		private List<OVComet> Comets { get; set; }
		private List<CometOrbit> CometOrbits { get; set; }
		private List<Xyz> CometsPos { get; set; }
		public int SelectedIndex { get; set; }


		private ATime atime;
		public ATime ATime
		{
			get { return this.atime; }
			set
			{
				this.atime = value;
				UpdatePositions(atime);
			}
		}


		private PlanetOrbit[] PlanetOrbit { get; set; }
		private Xyz[] PlanetPos { get; set; }
		private double EpochPlanetOrbit { get; set; }


		public bool PaintEnabled { get; private set; }
		public Image Offscreen { get; set; }


		public int CenterObjectSelected { get; set; }
		private bool[] OrbitDisplay { get; set; }


		//settings
		private bool multipleMode;
		public bool MultipleMode
		{
			get { return multipleMode; }
			set
			{
				multipleMode = value;

				if (!multipleMode && PaintEnabled)
					ClearComets();
			}
		}
		public bool EclipticAxis { get; set; }
		public bool Antiliasing { get; set; }
		public bool ShowCometName { get; set; }
		public bool ShowPlanetName { get; set; }
		public bool ShowMagnitude { get; set; }
		public bool ShowDistance { get; set; }
		public bool ShowDate { get; set; }


		public double RotateHorz { get; set; }
		public double RotateVert { get; set; }
		public double Zoom { get; set; }
		private Matrix MtxToEcl { get; set; }
		private double EpochToEcl { get; set; }
		private Matrix MtxRotate { get; set; }
		private int X0 { get; set; }
		private int Y0 { get; set; }

		#endregion

		#region Colors

		protected Color ColorCometOrbitUpper = Color.FromArgb(0x00, 0xF5, 0xFF);
		protected Color ColorCometOrbitLower = Color.FromArgb(0x00, 0x00, 0xFF);
		protected Color ColorComet = Color.FromArgb(0x00, 0xFF, 0xFF);
		protected Color ColorCometName = Color.FromArgb(0x00, 0xcc, 0xcc);
		protected Color ColorPlanetOrbitUpper = Color.FromArgb(0xFF, 0xFF, 0xFF);
		protected Color ColorPlanetOrbitLower = Color.FromArgb(0x80, 0x80, 0x80);
		protected Color ColorPlanet = Color.FromArgb(0x00, 0xFF, 0x00);
		protected Color ColorPlanetName = Color.FromArgb(0x00, 0xaa, 0x00);
		protected Color ColorSun = Color.FromArgb(0xE5, 0x83, 0x17);
		protected Color ColorAxisPlus = Color.FromArgb(0xFF, 0xFF, 0x00);
		protected Color ColorAxisMinus = Color.FromArgb(0x55, 0x55, 0x00);
		protected Color ColorInformation = Color.FromArgb(0xFF, 0xFF, 0xFF);

		#endregion

		#region Fonts

		protected Font FontObjectName = new Font("Helvetica", 10, FontStyle.Regular);
		protected Font FontPlanetName = new Font("Helvetica", 10, FontStyle.Regular);
		protected Font FontInformation = new Font("Helvetica", 10, FontStyle.Bold);

		#endregion

		#region Consctructor

		public OrbitPanel()
		{
			this.DoubleBuffered = true;

			PlanetPos = new Xyz[9];
			OrbitDisplay = new bool[11];
			PlanetOrbit = new PlanetOrbit[9];

			Comets = new List<OVComet>();
			CometOrbits = new List<CometOrbit>();
			CometsPos = new List<Xyz>();

			ATime = atime;

			Offscreen = null;
			PaintEnabled = false;
		}

		#endregion

		#region LoadPanel

		public void LoadPanel(OVComet comet, ATime atime)
		{
			PaintEnabled = true;

			if (!MultipleMode)
			{
				Comets.Clear();
				CometOrbits.Clear();
			}

			if (comet != null && !Comets.Contains(comet))
			{
				Comets.Add(comet);
				CometOrbits.Add(new CometOrbit(comet, 1000));
			}

			if (Comets.Count > MaxNumberOfComets)
			{
				Comets.RemoveAt(0);
				CometOrbits.RemoveAt(0);
			}

			SelectedIndex = Comets.IndexOf(comet);

			this.atime = atime;

			UpdatePositions(atime);
			UpdatePlanetOrbit(atime);
			UpdateRotationMatrix(atime);
		}

		#endregion

		#region OnPaint

		protected override void OnPaint(PaintEventArgs e)
		{
			if (PaintEnabled)
			{
				if (Offscreen == null)
					Offscreen = new Bitmap(Size.Width, Size.Height);

				Update(e.Graphics);
			}
		}

		#endregion

		#region Update

		public void Update(Graphics g)
		{
			Point point3;
			Xyz xyz, xyz1;

			// Calculate Drawing Parameter
			Matrix mtxRotH = Matrix.RotateZ(RotateHorz * Math.PI / 180.0);
			Matrix mtxRotV = Matrix.RotateX(RotateVert * Math.PI / 180.0);
			MtxRotate = mtxRotV.Mul(mtxRotH);

			X0 = Size.Width / 2;
			Y0 = Size.Height / 2;

			if (Math.Abs(EpochToEcl - ATime.JD) > 365.2422 * 5)
			{
				UpdateRotationMatrix(ATime);
			}

			if (SelectedIndex >= 0 && CenterObjectSelected == (int)CenteredObjectEnum.CometAsteroid)
			{
				xyz = CometsPos[SelectedIndex].Rotate(MtxToEcl).Rotate(MtxRotate);
				point3 = GetDrawPoint(xyz);

				X0 = Size.Width - point3.X;
				Y0 = Size.Height - point3.Y;
			}
			else if (CenterObjectSelected >= (int)CenteredObjectEnum.Mercury)
			{
				xyz = PlanetPos[CenterObjectSelected - 2].Rotate(MtxRotate);
				point3 = GetDrawPoint(xyz);

				X0 = Size.Width - point3.X;
				Y0 = Size.Height - point3.Y;
			}

			using (Graphics graphics = Graphics.FromImage(Offscreen))
			{
				// Clear bacground
				SolidBrush sb = new SolidBrush(Color.Black);
				graphics.FillRectangle(sb, 0, 0, Size.Width, Size.Height);

				if (EclipticAxis)
					DrawEclipticAxis(graphics);

				// Draw Sun
				sb.Color = ColorSun;
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				graphics.FillPie(sb, X0 - 2, Y0 - 2, 5, 5, 0, 360);

				if (OrbitDisplay[(int)OrbitDisplayEnum.CometAsteroid])
				{
					DrawCometOrbit(graphics, CometOrbits);
				}

				DrawCometBody(graphics);

				//  Draw Orbit of Planets
				if (Math.Abs(EpochPlanetOrbit - ATime.JD) > 365.2422 * 5)
				{
					UpdatePlanetOrbit(ATime);
				}

				double zoom = 30.0;

				//if (Zoom * 39.5 >= zoom)
				//{
				//	if (OrbitDisplay[(int)OrbitDisplayEnum.Pluto])
				//		DrawPlanetOrbit(graphics, PlanetOrbit[Planet.PLUTO - 1]);

				//	DrawPlanetBody(graphics, FontPlanetName, PlanetPos[8], "Pluto");
				//}

				if (Zoom * 30.1 >= zoom)
				{
					if (OrbitDisplay[(int)OrbitDisplayEnum.Neptune])
						DrawPlanetOrbit(graphics, PlanetOrbit[Planet.NEPTUNE - 1]);

					DrawPlanetBody(graphics, FontPlanetName, PlanetPos[7], "Neptune");
				}

				if (Zoom * 19.2 >= zoom)
				{

					if (OrbitDisplay[(int)OrbitDisplayEnum.Uranus])
						DrawPlanetOrbit(graphics, PlanetOrbit[Planet.URANUS - 1]);

					DrawPlanetBody(graphics, FontPlanetName, PlanetPos[6], "Uranus");
				}

				if (Zoom * 9.58 >= zoom)
				{
					if (OrbitDisplay[(int)OrbitDisplayEnum.Saturn])
						DrawPlanetOrbit(graphics, PlanetOrbit[Planet.SATURN - 1]);

					DrawPlanetBody(graphics, FontPlanetName, PlanetPos[5], "Saturn");
				}

				if (Zoom * 5.2 >= zoom)
				{
					if (OrbitDisplay[(int)OrbitDisplayEnum.Jupiter])
						DrawPlanetOrbit(graphics, PlanetOrbit[Planet.JUPITER - 1]);

					DrawPlanetBody(graphics, FontPlanetName, PlanetPos[4], "Jupiter");
				}

				if (Zoom * 1.524 >= zoom)
				{
					if (OrbitDisplay[(int)OrbitDisplayEnum.Mars])
						DrawPlanetOrbit(graphics, PlanetOrbit[Planet.MARS - 1]);

					DrawPlanetBody(graphics, FontPlanetName, PlanetPos[3], "Mars");
				}

				if (Zoom * 1.0 >= zoom)
				{
					if (OrbitDisplay[(int)OrbitDisplayEnum.Earth])
						DrawEarthOrbit(graphics, PlanetOrbit[Planet.EARTH - 1]);

					DrawPlanetBody(graphics, FontPlanetName, PlanetPos[2], "Earth");
				}

				if (Zoom * 0.723 >= zoom)
				{
					if (OrbitDisplay[(int)OrbitDisplayEnum.Venus])
						DrawPlanetOrbit(graphics, PlanetOrbit[Planet.VENUS - 1]);

					DrawPlanetBody(graphics, FontPlanetName, PlanetPos[1], "Venus");
				}

				if (Zoom * 0.387 >= zoom)
				{
					if (OrbitDisplay[(int)OrbitDisplayEnum.Mercury])
						DrawPlanetOrbit(graphics, PlanetOrbit[Planet.MERCURY - 1]);

					DrawPlanetBody(graphics, FontPlanetName, PlanetPos[0], "Mercury");
				}

				// Information
				sb.Color = ColorInformation;

				// Object Name string
				int labelMargin = 8;
				double fontSize = (double)FontInformation.Size;

				Point point1 = new Point(labelMargin, labelMargin);

				if (SelectedIndex >= 0)
				{
					graphics.DrawString(Comets[SelectedIndex].Name, FontInformation, sb, point1.X, point1.Y);

					if (ShowDistance)
					{
						// Earth & Sun Distance, Magnitude
						double d, r, m;
						double xdiff, ydiff, zdiff;
						string dstr, rstr, mstr;

						xyz = CometsPos[SelectedIndex].Rotate(MtxToEcl).Rotate(MtxRotate);

						xyz1 = PlanetPos[2].Rotate(MtxRotate);
						r = Math.Sqrt((xyz.X * xyz.X) + (xyz.Y * xyz.Y) + (xyz.Z * xyz.Z)) + .0005;
						xdiff = xyz.X - xyz1.X;
						ydiff = xyz.Y - xyz1.Y;
						zdiff = xyz.Z - xyz1.Z;
						d = Math.Sqrt((xdiff * xdiff) + (ydiff * ydiff) + (zdiff * zdiff)) + .0005;

						m = Comets[SelectedIndex].g + 5 * Math.Log10(d) + 2.5 * Comets[SelectedIndex].k * Math.Log10(r);

						mstr = String.Format("Magnitude:       {0:#0.0}", m);
						dstr = String.Format("Earth Distance: {0:#0.0000} AU", d);
						rstr = String.Format("Sun Distance:   {0:#0.0000} AU", r);

						point1.Y = Size.Height - labelMargin - (int)(fontSize * 5.0);
						graphics.DrawString(mstr, FontInformation, sb, point1.X, point1.Y);

						point1.Y = Size.Height - labelMargin - (int)(fontSize * 3.5);
						graphics.DrawString(dstr, FontInformation, sb, point1.X, point1.Y);

						point1.Y = Size.Height - labelMargin - (int)(fontSize * 2.0);
						graphics.DrawString(rstr, FontInformation, sb, point1.X, point1.Y);
					}
				}

				if (ShowDate)
				{
					// Date string
					string strDate = String.Format("{0} {1}, {2}", ATime.MonthAbbr(ATime.Month), ATime.Day, ATime.Year);
					point1.X = Size.Width - (int)graphics.MeasureString(strDate, FontInformation).Width - labelMargin;
					point1.Y = Size.Height - labelMargin - (int)(fontSize * 2.0);
					graphics.DrawString(strDate, FontInformation, sb, point1.X, point1.Y);
				}
			}

			g.DrawImage(Offscreen, 0, 0);
		}

		#endregion

		#region + Methods

		#region UpdatePositions

		private void UpdatePositions(ATime atime)
		{
			if (PaintEnabled)
			{
				CometsPos.Clear();
				foreach (OVComet c in Comets)
					CometsPos.Add(c.GetPos(atime.JD));

				for (int i = 0; i < 9; i++)
				{
					PlanetPos[i] = Planet.GetPos(Planet.MERCURY + i, atime);
				}
			}
		}

		#endregion

		#region UpdatePlanetOrbit

		private void UpdatePlanetOrbit(ATime atime)
		{
			int nDivision = 300;

			for (int i = Planet.MERCURY; i <= Planet.PLUTO; i++)
			{
				PlanetOrbit[i - Planet.MERCURY] = new PlanetOrbit(i, atime, nDivision);
			}

			EpochPlanetOrbit = atime.JD;
		}

		#endregion

		#region UpdateRotationMatrix

		private void UpdateRotationMatrix(ATime atime)
		{
			Matrix mtxPrec = Matrix.PrecMatrix(Astro.JD2000, atime.JD);
			Matrix mtxEqt2Ecl = Matrix.RotateX(ATime.GetEp(atime.JD));
			MtxToEcl = mtxEqt2Ecl.Mul(mtxPrec);
			EpochToEcl = atime.JD;
		}

		#endregion

		#region GetDrawPoint

		private Point GetDrawPoint(Xyz xyz)
		{
			double mul = (Zoom * (double)this.MinimumSize.Width) / (1500.0 * (1.0 + xyz.Z / 625.0));
			int X = X0 + (int)Math.Round(xyz.X * mul);
			int Y = Y0 - (int)Math.Round(xyz.Y * mul);
			return new Point(X, Y);
		}

		#endregion

		#region DrawEclipticAxis

		private void DrawEclipticAxis(Graphics graphics)
		{
			graphics.SmoothingMode = Antiliasing ? SmoothingMode.AntiAlias : SmoothingMode.None;

			Pen pen = new Pen(ColorAxisMinus);
			Xyz xyz;
			Point point;
			double sizeAU = 50.0;

			// -X
			xyz = new Xyz(-sizeAU, 0.0, 0.0).Rotate(MtxRotate);
			point = GetDrawPoint(xyz);
			graphics.DrawLine(pen, X0, Y0, point.X, point.Y);

			// -Z
			xyz = new Xyz(0.0, 0.0, -sizeAU).Rotate(MtxRotate);
			point = GetDrawPoint(xyz);
			graphics.DrawLine(pen, X0, Y0, point.X, point.Y);

			pen.Color = ColorAxisPlus;

			// +X
			xyz = new Xyz(sizeAU, 0.0, 0.0).Rotate(MtxRotate);
			point = GetDrawPoint(xyz);
			graphics.DrawLine(pen, X0, Y0, point.X, point.Y);
			// +Z
			xyz = new Xyz(0.0, 0.0, sizeAU).Rotate(MtxRotate);
			point = GetDrawPoint(xyz);
			graphics.DrawLine(pen, X0, Y0, point.X, point.Y);
		}

		#endregion

		#region DrawCometOrbit

		private void DrawCometOrbit(Graphics graphics, List<CometOrbit> cometOrbits)
		{
			graphics.SmoothingMode = Antiliasing ? SmoothingMode.AntiAlias : SmoothingMode.None;

			for (int i = 0; i < Comets.Count; i++)
			{
				Xyz xyz = cometOrbits[i].GetAt(0).Rotate(MtxToEcl).Rotate(MtxRotate);
				Pen pen = new Pen(Color.White);
				Point point1, point2;
				point1 = GetDrawPoint(xyz);

				for (int j = 1; j <= cometOrbits[i].Division; j++)
				{
					xyz = cometOrbits[i].GetAt(j).Rotate(MtxToEcl);
					pen.Color = xyz.Z >= 0.0 ? ColorCometOrbitUpper : ColorCometOrbitLower;
					xyz = xyz.Rotate(MtxRotate);
					point2 = GetDrawPoint(xyz);
					graphics.DrawLine(pen, point1.X, point1.Y, point2.X, point2.Y);
					point1 = point2;
				}
			}
		}

		#endregion

		#region DrawCometBody

		private void DrawCometBody(Graphics graphics)
		{
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			SolidBrush sb = new SolidBrush(ColorComet);

			for (int i = 0; i < Comets.Count; i++)
			{
				Xyz xyz = CometsPos[i].Rotate(MtxToEcl).Rotate(MtxRotate);
				Point point1 = GetDrawPoint(xyz);
				graphics.FillPie(sb, point1.X - 2, point1.Y - 2, 5, 5, 0, 360);

				if (ShowCometName)
				{
					sb.Color = ColorCometName;
					graphics.DrawString(Comets[i].Name, FontObjectName, sb, point1.X + 5, point1.Y);
				}
			}
		}

		#endregion

		#region DrawPlanetOrbit

		private void DrawPlanetOrbit(Graphics graphics, PlanetOrbit planetOrbit)
		{
			graphics.SmoothingMode = Antiliasing ? SmoothingMode.AntiAlias : SmoothingMode.None;

			Pen pen = new Pen(ColorPlanetOrbitUpper);
			Point point1, point2;
			Xyz xyz = planetOrbit.GetAt(0).Rotate(MtxToEcl).Rotate(MtxRotate);

			point1 = GetDrawPoint(xyz);

			for (int i = 1; i <= planetOrbit.Division; i++)
			{
				xyz = planetOrbit.GetAt(i).Rotate(MtxToEcl);

				pen.Color = xyz.Z >= 0.0 ? ColorPlanetOrbitUpper : ColorPlanetOrbitLower;

				xyz = xyz.Rotate(MtxRotate);
				point2 = GetDrawPoint(xyz);
				graphics.DrawLine(pen, point1.X, point1.Y, point2.X, point2.Y);
				point1 = point2;
			}
		}

		#endregion

		#region DrawEarthOrbit

		private void DrawEarthOrbit(Graphics graphics, PlanetOrbit planetOrbit)
		{
			graphics.SmoothingMode = Antiliasing ? SmoothingMode.AntiAlias : SmoothingMode.None;

			Pen pen = new Pen(ColorPlanetOrbitUpper);
			Point point1, point2;
			Xyz xyz = planetOrbit.GetAt(0).Rotate(MtxToEcl).Rotate(MtxRotate);

			point1 = GetDrawPoint(xyz);

			for (int i = 1; i <= planetOrbit.Division; i++)
			{
				xyz = planetOrbit.GetAt(i).Rotate(MtxToEcl);
				xyz = xyz.Rotate(MtxRotate);
				point2 = GetDrawPoint(xyz);
				graphics.DrawLine(pen, point1.X, point1.Y, point2.X, point2.Y);
				point1 = point2;
			}
		}

		#endregion

		#region DrawPlanetBody

		private void DrawPlanetBody(Graphics graphics, Font font, Xyz planetPos, string name)
		{
			graphics.SmoothingMode = SmoothingMode.AntiAlias; //body always antialiased

			Xyz xyz = planetPos.Rotate(MtxRotate);
			Point point = GetDrawPoint(xyz);
			SolidBrush sb = new SolidBrush(ColorPlanet);

			graphics.FillPie(sb, point.X - 2, point.Y - 2, 5, 5, 0, 360);

			if (ShowPlanetName)
			{
				sb.Color = ColorPlanetName;
				graphics.DrawString(name, font, sb, point.X + 5, point.Y);
			}
		}

		#endregion

		#region SelectOrbits

		public void SelectOrbits(bool[] orbitDisplay)
		{
			for (int i = 0; i < orbitDisplay.Length; i++)
			{
				OrbitDisplay[i] = orbitDisplay[i];
			}
		}

		#endregion

		#region ClearComets

		public void ClearComets()
		{
			OVComet comet = Comets.ElementAt(SelectedIndex);

			Comets.Clear();
			CometOrbits.Clear();

			Comets.Add(comet);
			CometOrbits.Add(new CometOrbit(comet, 1000));

			SelectedIndex = 0;

			UpdatePositions(ATime);
			UpdatePlanetOrbit(ATime);
			UpdateRotationMatrix(ATime);
		}

		#endregion

		#endregion
	}
}