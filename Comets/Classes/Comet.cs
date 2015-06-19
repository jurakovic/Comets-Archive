﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Comets.Classes
{
	public class Comet
	{
		#region Const

		public static string[] Month = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

		#endregion

		#region Properties

		/// <summary>
		/// Full (e.g. 1P/Halley)
		/// </summary>
		public string full { get; set; }

		/// <summary>
		/// Name (e.g. Halley)
		/// </summary>
		public string name { get; set; }

		/// <summary>
		/// Id (e.g. 1P)
		/// </summary>
		public string id { get; set; }

		/// <summary>
		/// Epoch (Julian day)
		/// </summary>
		public double T { get; set; }

		/// <summary>
		/// Epoch year
		/// </summary>
		public int Ty { get; set; }

		/// <summary>
		/// Epoch month
		/// </summary>
		public int Tm { get; set; }

		/// <summary>
		/// Epoch day
		/// </summary>
		public int Td { get; set; }

		/// <summary>
		/// Epoch hour (#### format)
		/// </summary>
		public int Th { get; set; }

		/// <summary>
		/// Period
		/// </summary>
		public double P { get; set; }

		/// <summary>
		/// Perihelion distance
		/// </summary>
		public double q { get; set; }

		/// <summary>
		/// Eccentricity
		/// </summary>
		public double e { get; set; }

		/// <summary>
		/// Inclination
		/// </summary>
		public double i { get; set; }

		/// <summary>
		/// Longitude of the Ascending Node
		/// </summary>
		public double N { get; set; }

		/// <summary>
		/// Argument of Pericenter
		/// </summary>
		public double w { get; set; }

		/// <summary>
		/// Semimajor Axis
		/// </summary>
		public double a { get; set; }

		/// <summary>
		/// Aphelion distance
		/// </summary>
		public double Q { get; set; }

		/// <summary>
		/// Mean motion
		/// </summary>
		public double n { get; set; }

		/// <summary>
		/// Absolute magnitude
		/// </summary>
		public double g { get; set; }

		/// <summary>
		/// Slope parameter
		/// </summary>
		public double k { get; set; }

		/// <summary>
		/// Sortkey
		/// </summary>
		public double sortkey { get; set; }

		/// <summary>
		/// IdKey
		/// </summary>
		public string idKey { get; set; }

		#endregion

		#region Constructor

		public Comet()
		{

		}

		#endregion

		#region ToString

		public override string ToString()
		{
			return full;
		}

		#endregion

		#region Methods

		#region GetSortkey

		/// <summary>
		/// Calculates Sortkey
		/// </summary>
		/// <param name="id">ID</param>
		/// <returns></returns>
		public static double GetSortkey(string id)
		{
			double sort = 0.0;
			double v = 0.0;
			double offset = 2000.0;
			string fragm = String.Empty;

			//http://stackoverflow.com/questions/3720012/regular-expression-to-split-string-and-number
			Regex numAlpha = new Regex("(?<letters>[a-zA-Z]*)(?<digits>[0-9]*)");

			if (id.Contains('-') && id[2] != '-') // 128P-B, C/-146 P1
			{
				string[] fi = id.Split('-');
				id = fi[0];
				fragm = fi[1];

				var match = numAlpha.Match(fragm);

				string fragmLetters = match.Groups["letters"].Value;
				string fragmDigits = match.Groups["digits"].Value;

				for (int i = 0, divider = 1000000000; i < fragmLetters.Length; i++, divider *= 100)
				{
					v += (fragmLetters[i] - 64) / (double)divider;
				}

				if (fragmDigits != String.Empty)
					v += Convert.ToDouble(fragmDigits) / 10000000000000.0;
			}

			if (Char.IsDigit(id[0]))
			{
				sort = Convert.ToDouble(id.Substring(0, id.Length - 1));
			}
			else
			{
				string[] yc = id.Split(' ');
				sort = Convert.ToDouble(yc[0].Split('/')[1]) + offset; //da npr C/240 V1 ne bude isto kao i 240P/NEAT i slicno...

				string code = yc[1];

				var match = numAlpha.Match(code);

				string codeLetters = match.Groups["letters"].Value;
				string codeDigits = match.Groups["digits"].Value;

				// pretpostavka da mogu doci najvise 2 slova u id-u
				// 1. slovo dijelim sa 100
				// 2. slovo dijelim sa 10000
				// pretpostavka da se moze pojaviti najvise troznamenkasti broj
				// njega dijelim sa 10000000

				for (int i = 0, divider = 100; i < codeLetters.Length; i++, divider *= 100)
				{
					v += (codeLetters[i] - 64) / (double)divider;
				}

				if (codeDigits != String.Empty)
					v += Convert.ToDouble(codeDigits) / 10000000.0;
			}

			sort += v;

			return sort;
		}

		#endregion

		#region GetIdKey

		/// <summary>
		/// Returns IDKey
		/// </summary>
		/// <param name="id">ID</param>
		/// <returns></returns>
		public static string GetIdKey(string id)
		{
			string key = String.Empty;

			if (Char.IsDigit(id[0]))
			{
				key = id;

				for (int i = key.Length; i < 5; i++)
					key = '0' + key;
			}
			else
			{
				key = id.Remove(0, 2).Replace("-", String.Empty).Replace(" ", String.Empty);
			}

			return key;
		}

		#endregion

		#region GetPeriod

		/// <summary>
		/// Calculates Period (P)
		/// </summary>
		/// <param name="q">Perihelion distance (q)</param>
		/// <param name="e">Eccentricity (e)</param>
		/// <returns></returns>
		public static double GetPeriod(double q, double e)
		{
			if (e < 1.0)
				return Math.Pow((q / (1.0 - e)), 1.5);
			else if (e > 1.0)
				return Math.Pow((q / (e - 1.0)), 1.5);
			else //if (e == 1.0)
				return Math.Pow((q / (1 - 0.999999)), 1.5); //okvirno samo za sortiranje
		}

		#endregion

		#region GetSemimajorAxis

		/// <summary>
		/// Calculates Semimajor axis (a)
		/// </summary>
		/// <param name="q">Perihelion distance (q)</param>
		/// <param name="e">Eccentricity (e)</param>
		/// <returns></returns>
		public static double GetSemimajorAxis(double q, double e)
		{
			if (e < 1.0)
				return q / (1 - e);
			else if (e > 1.0)
				return -(q / (1 - e));
			else //if (e == 1.0)
				return q / (1 - 0.999999);
		}

		#endregion

		#region GetAphelionDistance

		/// <summary>
		/// Calculates Aphelion distance (Q)
		/// </summary>
		/// <param name="e">Eccentricity (e)</param>
		/// <param name="a">Semimajor axis (a)</param>
		/// <returns></returns>
		public static double GetAphelionDistance(double e, double a)
		{

			if (e < 1.0)
				return a * (1 + e);
			else if (e > 1.0)
				return a * (1 + (2 - e));
			else //if (e == 1.0) //koristi se zamo za sortiranje
				return a * (1 + 0.999999);
		}

		#endregion

		#region GetMeanMotion

		/// <summary>
		/// Calculates Mean motion (n)
		/// </summary>
		/// <param name="e">Eccentricity (e)</param>
		/// <param name="P">Period (P)</param>
		/// <returns></returns>
		public static double GetMeanMotion(double e, double P)
		{
			if (e < 1.0)
				return 0.9856076686 / P; // Gaussian gravitational constant (degrees)
			else
				return 0.0;
		}

		#endregion

		#region GetIdNameFromFull

		/// <summary>
		/// Sets Comet ID and Name from Full
		/// </summary>
		/// <param name="full">Full comet name</param>
		/// <returns></returns>
		public static string[] GetIdNameFromFull(string full)
		{
			string id = String.Empty, name = String.Empty;

			if (Char.IsDigit(full[0]))
			{
				if (full.Contains('/'))
				{
					string[] idname = full.Split('/');
					id = idname[0];
					name = idname[1];
				}
				else
				{
					id = full;
				}
			}
			else
			{
				if (full.Contains('('))
				{
					string[] idname = full.Split('(');
					id = idname[0].Trim();
					name = idname[1].TrimEnd(')');
				}
				else
				{
					id = full;
				}
			}

			return new string[] { id, name };
		}

		#endregion

		#region GetFullFromIdName

		/// <summary>
		/// Sets Full from ID and Name
		/// </summary>
		/// <param name="id">Comet ID</param>
		/// <param name="name">Comet Name</param>
		/// <returns></returns>
		public static string GetFullFromIdName(string id, string name)
		{
			string full = id;

			if (id.Contains('/'))
			{
				if (name != String.Empty)
				{
					full += " (" + name + ")";
				}
			}
			else
			{
				if (name != String.Empty)
				{
					full += "/" + name;
				}
			}

			return full;
		}

		#endregion

		#region GregorianToJulian

		/// <summary>
		/// Converts Gregorian date to Julian day
		/// </summary>
		/// <param name="y">Year</param>
		/// <param name="m">Month</param>
		/// <param name="d">Day</param>
		/// <param name="h">Hour (0000)</param>
		/// <returns></returns>
		public static double GregorianToJulian(int y, int m, int d, int h)
		{
			return 367 * y - (7 * (y + (m + 9) / 12)) / 4 - ((3 * (y + (m - 9) / 7)) / 100 + 1) / 4 + (275 * m) / 9 + d + 1721029 + ((double)h / 10000);
		}

		#endregion

		#endregion
	}
}
