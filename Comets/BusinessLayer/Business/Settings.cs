﻿using System;
using System.Collections.Generic;

namespace Comets.BusinessLayer.Business
{
	public class Settings
	{
		#region Properties

		//General
		public bool AutomaticUpdate { get; set; }
		public int UpdateInterval { get; set; }
		public DateTime? LastUpdateDate { get; set; }
		public bool RememberWindowPosition { get; set; }
		public bool NewVersionOnStartup { get; set; }
		public bool ExitWithoutConfirm { get; set; }
		public bool ShowStatusBar { get; set; }

		//Window
		public bool Maximized { get; set; }
		public int Left { get; set; }
		public int Top { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }

		//Network
		public bool UseProxy { get; set; }
		public string Domain { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Proxy { get; set; }
		public int Port { get; set; }

		// Location
		public Location Location { get; set; }

		//Programs
		public List<ExternalProgram> ExternalPrograms { get; set; }
		public string LastUsedImportDirectory { get; set; }
		public string LastUsedExportDirectory { get; set; }

		public bool HasErrors { get; set; }
		public bool IsSettingsChanged { get; set; }

		#endregion

		#region Constructor

		public Settings()
		{
			AutomaticUpdate = true;
			UpdateInterval = 7;
			LastUpdateDate = null;
			NewVersionOnStartup = false;
			RememberWindowPosition = true;
			ExitWithoutConfirm = false;
			ShowStatusBar = true;

			Maximized = false;
			Width = 0;
			Height = 0;

			UseProxy = false;
			Domain = String.Empty;
			Username = String.Empty;
			Password = String.Empty;
			Proxy = String.Empty;
			Port = 0;

			Location = new Location();

			ExternalPrograms = new List<ExternalProgram>();
			LastUsedImportDirectory = String.Empty;
			LastUsedExportDirectory = String.Empty;

			HasErrors = false;
			IsSettingsChanged = false;
		}

		#endregion
	}
}
