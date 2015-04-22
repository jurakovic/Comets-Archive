﻿using Comets.Classes;
using Comets.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using ImportType = Comets.Classes.ElementTypes.Type;

namespace Comets.Forms
{
    public partial class FormImport : Form
    {
        const string url = "http://www.minorplanetcenter.net/iau/Ephemerides/Comets/Soft00Cmt.txt";

        string downloadFilename;
        string localFilename;
        string importFilename;

        int importType = (int)ImportType.Unknown;

        FormMain formMain = null;

        public FormImport(FormMain formMain)
        {
            InitializeComponent();
            this.formMain = formMain;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (downloadFilename != null)
            {
                File.Delete(downloadFilename);
                progressDownload.Value = 0;
            }

            downloadFilename = FormMain.Settings.Downloads + "\\Soft00Cmt_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";

            bwDownload.RunWorkerAsync();
        }

        private void bwDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            if (progressDownload.InvokeRequired)
            {
                progressDownload.Invoke((MethodInvoker)delegate()
                {
                    bwDownload_DoWork(sender, e);
                });
            }
            else
            {
                progressDownload.Visible = true;

                WebClient Client = new WebClient();

                if (FormMain.Settings.UseProxy)
                {
                    WebProxy proxy = new WebProxy(FormMain.Settings.Proxy, FormMain.Settings.Port);
                    proxy.Credentials = new NetworkCredential(FormMain.Settings.Username, FormMain.Settings.Password, FormMain.Settings.Domain);
                    Client.Proxy = proxy;
                }

                Client.DownloadProgressChanged += Client_DownloadProgressChanged;
                Client.DownloadFileCompleted += Client_DownloadFileCompleted;
                Uri uri = new Uri(url);

                try
                {
                    Client.DownloadFileAsync(uri, downloadFilename);
                }
                catch
                {
                    //nothing...
                }
            }
        }

        void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressDownload.Value = e.ProgressPercentage;
        }

        void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (File.Exists(downloadFilename) && new FileInfo(downloadFilename).Length == 0)
            {
                progressDownload.Visible = false;
                File.Delete(downloadFilename);
                MessageBox.Show(e.Error.Message, "Comets", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                importFilename = downloadFilename;
                setImportStatus();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (formMain.ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtLocalFile.Text = formMain.ofd.FileName; // txtImportFilename_TextChanged()
            }
        }

        private void setImportStatus()
        {
            if (importFilename == null)
            {
                if (localFilename == null && downloadFilename != null)
                    importFilename = downloadFilename;

                else if (downloadFilename == null && localFilename != null)
                    importFilename = localFilename;
            }

            importType = (int)ImportHelper.GetImportType(importFilename);

            switch (importType)
            {
                case (int)ImportType.NoFileSelected:
                    lblImportFormat.Text = "(no file selected)";
                    labelDetectedComets.Text = "-";
                    break;

                case (int)ImportType.FileNotFound:
                    lblImportFormat.Text = "(file not found)";
                    labelDetectedComets.Text = "-";
                    break;

                case (int)ImportType.EmptyFile:
                    lblImportFormat.Text = "(empty file)";
                    labelDetectedComets.Text = "-";
                    break;

                case (int)ImportType.Unknown:
                    lblImportFormat.Text = "(unknown)";
                    labelDetectedComets.Text = "-";
                    break;

                default:
                    lblImportFormat.Text = ElementTypes.TypeName[importType];
                    labelDetectedComets.Text = ImportHelper.GetNumberOfComets(importFilename, importType).ToString();
                    break;
            }
        }

        private void txtImportFilename_TextChanged(object sender, EventArgs e)
        {
            localFilename = txtLocalFile.Text.Trim().Trim('"');

            if (localFilename.Length == 0)
            {
                localFilename = null;
                importFilename = null;
            }

            else
                importFilename = localFilename;

            setImportStatus();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (importType >= (int)ImportType.NoFileSelected)
            {
                return;
            }

            List<Comet> list = ImportHelper.ImportMain(importType, importFilename);

            if (list.Count == 0)
            {
                MessageBox.Show("Something wrong happened. Zero comets imported.\t\t\t", "Comets", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                formMain.isDataChanged = true;
                FormMain.mainList = list;
                FormMain.userList = list;
                this.formMain.SetStatusCometsLabel(list.Count);
                MessageBox.Show("Import complete.\t\t\t", "Comets", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnClose.Focus();
            }
        }
    }
}
