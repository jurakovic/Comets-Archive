﻿namespace Comets.Application.ModulGraph
{
	partial class FormGraph
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(42004D, 0D);
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(42005D, 10D);
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(42006D, 8D);
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint4 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(42007D, 6D);
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint5 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(42008D, 2D);
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint6 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(42009D, 1D);
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint7 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(42010D, 3D);
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint8 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(42011D, 6D);
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint9 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(42012D, 9D);
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint10 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(42013D, 11D);
			System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint11 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(42009D, 0D);
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint12 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(42009D, 11D);
			System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.SuspendLayout();
			// 
			// chart1
			// 
			this.chart1.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.Text;
			chartArea1.AxisX2.IsLabelAutoFit = false;
			chartArea1.AxisX2.IsMarginVisible = false;
			chartArea1.AxisX2.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.None;
			chartArea1.AxisX2.LabelStyle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			chartArea1.AxisX2.LabelStyle.Format = "dd. MMMM yyyy";
			chartArea1.AxisX2.LabelStyle.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Months;
			chartArea1.AxisX2.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Months;
			chartArea1.AxisX2.LabelStyle.IsEndLabelVisible = false;
			chartArea1.AxisX2.MajorGrid.Enabled = false;
			chartArea1.AxisX2.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Months;
			chartArea1.AxisX2.ScaleBreakStyle.LineColor = System.Drawing.Color.Lime;
			chartArea1.AxisX2.Title = "Date";
			chartArea1.AxisX2.TitleAlignment = System.Drawing.StringAlignment.Far;
			chartArea1.AxisX2.TitleFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			chartArea1.AxisY.IsLabelAutoFit = false;
			chartArea1.AxisY.IsMarginVisible = false;
			chartArea1.AxisY.IsReversed = true;
			chartArea1.AxisY.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.None;
			chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			chartArea1.AxisY.MajorGrid.Enabled = false;
			chartArea1.AxisY.MajorGrid.Interval = 1D;
			chartArea1.AxisY.MajorTickMark.Size = 0.5F;
			chartArea1.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Rotated270;
			chartArea1.AxisY.Title = "Magnitude";
			chartArea1.AxisY.TitleAlignment = System.Drawing.StringAlignment.Far;
			chartArea1.AxisY.TitleFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			chartArea1.IsSameFontSizeForAllAxes = true;
			chartArea1.Name = "ChartAreaMagnitude";
			chartArea1.Position.Auto = false;
			chartArea1.Position.Height = 90F;
			chartArea1.Position.Width = 96F;
			chartArea1.Position.X = 1F;
			chartArea1.Position.Y = 8F;
			this.chart1.ChartAreas.Add(chartArea1);
			this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chart1.Location = new System.Drawing.Point(0, 0);
			this.chart1.Name = "chart1";
			series1.ChartArea = "ChartAreaMagnitude";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
			series1.Color = System.Drawing.Color.Red;
			series1.Legend = "Legend1";
			series1.Name = "SeriesMagnitude";
			dataPoint2.MarkerSize = 5;
			series1.Points.Add(dataPoint1);
			series1.Points.Add(dataPoint2);
			series1.Points.Add(dataPoint3);
			series1.Points.Add(dataPoint4);
			series1.Points.Add(dataPoint5);
			series1.Points.Add(dataPoint6);
			series1.Points.Add(dataPoint7);
			series1.Points.Add(dataPoint8);
			series1.Points.Add(dataPoint9);
			series1.Points.Add(dataPoint10);
			series1.XAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
			series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
			series2.ChartArea = "ChartAreaMagnitude";
			series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series2.Color = System.Drawing.Color.RoyalBlue;
			series2.Legend = "Legend1";
			series2.Name = "Series2";
			series2.Points.Add(dataPoint11);
			series2.Points.Add(dataPoint12);
			series2.ShadowColor = System.Drawing.Color.Transparent;
			series2.XAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
			series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
			this.chart1.Series.Add(series1);
			this.chart1.Series.Add(series2);
			this.chart1.Size = new System.Drawing.Size(1350, 729);
			this.chart1.TabIndex = 1;
			this.chart1.Text = "chart1";
			title1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			title1.Name = "ChartTitle";
			title1.Text = "Halley";
			this.chart1.Titles.Add(title1);
			// 
			// FormGraph
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1350, 729);
			this.Controls.Add(this.chart1);
			this.Name = "FormGraph";
			this.Text = "Magnitude Graph";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMagnitudeGraph_FormClosing);
			this.Load += new System.EventHandler(this.FormMagnitude_Load);
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
	}
}