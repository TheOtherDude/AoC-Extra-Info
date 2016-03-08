// reference:System.Core.dll

using Advanced_Combat_Tracker;
using AoCExtraInfo;
using MyExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;


[assembly: System.CLSCompliant(true)]
[assembly: AssemblyTitle("AoC Extra Info with PerkAlert™")]
[assembly: AssemblyDescription("Get more information from Age of Conan combat logs.")]
[assembly: AssemblyCompany("Lymain of Crom")]
[assembly: AssemblyVersion("1.0.0.6")]
[assembly: ComVisible(false)]

namespace AoCExtraInfo
{

    public class PluginTab : UserControl
    {
        private Panel pTaps;
        private Label lManaTap;
        private Label lStaminaTap;
        private Label lHealthTap;
        private Label lTaps;
        private Label lTotalMana;
        private Label lTotalStamina;
        private Label lTotalHealth;
        private Label lHTapPS;
        private Label lSTapPS;
        private Label lMTapPS;
        private ListBox lbEncounters;
        private Panel pEncDPS;
        private Label lEncDPS;
        private Panel pIncDPS;
        private Label lIncDPS;
        private Button bCritsOut;
        private Button bCritsInc;
        private Timer UIUpdateTimer;
        private System.ComponentModel.IContainer components;
        private GroupBox gbPerkAlert;
        private RichTextBox rtbPerkAlert;
        private Timer AlertLifeTimer;
        private FormMiniAlert fMiniAlert = new FormMiniAlert();
        private FormEditWatchList fEditWatch;
        private EncounterExtraData activeExtraEncounter;
        private List<EncounterExtraData> allExtraData = new List<EncounterExtraData>();
        private List<EncounterExtraData> selectedEncounters = new List<EncounterExtraData>();
        private EncounterExtraData mergedSelectedEncounters = new EncounterExtraData();
        private List<ActiveAlert> activeAlerts = new List<ActiveAlert>();
        SettingsSerializer xmlSettings;
        private Button bUnhideMini;
        private ContextMenuStrip cmsPerkAlert;
        private ToolStripMenuItem tsmiAddRemovePerks;
        private ToolStripMenuItem autoToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem bearShamanToolStripMenuItem;
        private ToolStripMenuItem priestOfMitraToolStripMenuItem;
        private ToolStripMenuItem tempestOfSetToolStripMenuItem;
        private ToolStripMenuItem mageToolStripMenuItem;
        private ToolStripMenuItem rogueToolStripMenuItem;
        private ToolStripMenuItem soldierToolStripMenuItem;
        private ToolStripMenuItem watchListToolStripMenuItem;
        private ToolStripMenuItem othersSoldierToolStripMenuItem;
        string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\AoC_Extra_Info.config.xml");



        #region Properties

        public List<string> PerksSelected
        {
            get
            {
                string playerClass;
                List<string> _perksSelected = new List<string>();

                if (bearShamanToolStripMenuItem.Checked)
                    _perksSelected.Add("Bear Shaman");
                if (priestOfMitraToolStripMenuItem.Checked)
                    _perksSelected.Add("Priest of Mitra");
                if (tempestOfSetToolStripMenuItem.Checked)
                    _perksSelected.Add("Tempest of Set");
                if (mageToolStripMenuItem.Checked)
                    _perksSelected.Add("Mage");
                if (rogueToolStripMenuItem.Checked)
                    _perksSelected.Add("Rogue");
                if (soldierToolStripMenuItem.Checked)
                    _perksSelected.Add("Soldier (You)");
                if (othersSoldierToolStripMenuItem.Checked)
                    _perksSelected.Add("Soldier (Others)");

                if (autoToolStripMenuItem.Checked)
                {
                    playerClass = ClassDetector.DetectedClassOrDefault;

                    switch (playerClass)
                    {
                        case "Assassin":
                        case "Barbarian":
                        case "Ranger":
                            playerClass = "Rogue";
                            break;
                        case "Conqueror":
                        case "Dark Templar":
                        case "Guardian":
                            playerClass = "Soldier (You)";
                            break;
                        case "Demonologist":
                        case "Herald of Xotli":
                        case "Necromancer":
                            playerClass = "Mage";
                            break;
                    }

                    if (!_perksSelected.Contains(playerClass))
                    {
                        _perksSelected.Add(playerClass);
                    }
                }
                return _perksSelected;
            }
        }

        public EncounterExtraData ActiveExtraEncounter
        {
            get { return activeExtraEncounter; }
        }

        public EncounterExtraData MergedSelectedEncounters
        {
            get { return mergedSelectedEncounters; }
        }

        public string TotalHealth
        {
            get { return lTotalHealth.Text; }
            set { lTotalHealth.Text = value; }
        }

        public string TotalStamina
        {
            get { return lTotalStamina.Text; }
            set { lTotalStamina.Text = value; }
        }

        public string TotalMana
        {
            get { return lTotalMana.Text; }
            set { lTotalMana.Text = value; }
        }

        public string HTapPS
        {
            get { return lHTapPS.Text; }
            set { lHTapPS.Text = value; }
        }

        public string STapPS
        {
            get { return lSTapPS.Text; }
            set { lSTapPS.Text = value; }
        }

        public string MTapPS
        {
            get { return lMTapPS.Text; }
            set { lMTapPS.Text = value; }
        }

        public string EncDPS
        {
            get { return lEncDPS.Text; }
            set { lEncDPS.Text = value; }
        }

        public string IncDPS
        {
            get { return lIncDPS.Text; }
            set { lIncDPS.Text = value; }
        }

        public string CritBonusOut
        {
            get { return bCritsOut.Text; }
            set { bCritsOut.Text = value; }
        }

        public string CritBonusInc
        {
            get { return bCritsInc.Text; }
            set { bCritsInc.Text = value; }
        }

        public bool AutoPerkChecked
        {
            get { return autoToolStripMenuItem.Checked; }
            set { autoToolStripMenuItem.Checked = value; }
        }

        public bool BSPerkChecked
        {
            get { return bearShamanToolStripMenuItem.Checked; }
            set { bearShamanToolStripMenuItem.Checked = value; }
        }

        public bool PoMPerkChecked
        {
            get { return priestOfMitraToolStripMenuItem.Checked; }
            set { priestOfMitraToolStripMenuItem.Checked = value; }
        }

        public bool ToSPerkChecked
        {
            get { return tempestOfSetToolStripMenuItem.Checked; }
            set { tempestOfSetToolStripMenuItem.Checked = value; }
        }

        public bool MagePerkChecked
        {
            get { return mageToolStripMenuItem.Checked; }
            set { mageToolStripMenuItem.Checked = value; }
        }

        public bool RoguePerkChecked
        {
            get { return rogueToolStripMenuItem.Checked; }
            set { rogueToolStripMenuItem.Checked = value; }
        }

        public bool SoldierPerkChecked
        {
            get { return soldierToolStripMenuItem.Checked; }
            set { soldierToolStripMenuItem.Checked = value; }
        }

        public bool OthersSoldierPerkChecked
        {
            get { return othersSoldierToolStripMenuItem.Checked; }
            set { othersSoldierToolStripMenuItem.Checked = value; }
        }

        public bool MiniAlertOpened
        {
            get { return FormMiniAlert.Opened; }
            set { FormMiniAlert.Opened = value; }
        }

        public bool MiniAlertTransparent
        {
            get { return FormMiniAlert.Transparent; }
            set { FormMiniAlert.Transparent = value; }
        }

        #endregion



        public PluginTab()
        {
            //System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = true;
            InitializeComponent();
        }

        internal void LoadSettings()
        {
            xmlSettings = new SettingsSerializer(this);

            // Add items to the xmlSettings object here...
            xmlSettings.AddBooleanSetting("AutoPerkChecked");
            xmlSettings.AddBooleanSetting("BSPerkChecked");
            xmlSettings.AddBooleanSetting("PoMPerkChecked");
            xmlSettings.AddBooleanSetting("ToSPerkChecked");
            xmlSettings.AddBooleanSetting("MagePerkChecked");
            xmlSettings.AddBooleanSetting("RoguePerkChecked");
            xmlSettings.AddBooleanSetting("SoldierPerkChecked");
            xmlSettings.AddBooleanSetting("OthersSoldierPerkChecked");
            xmlSettings.AddBooleanSetting("MiniAlertOpened");
            xmlSettings.AddBooleanSetting("MiniAlertTransparent");
            xmlSettings.AddControlSetting(fMiniAlert.Name, fMiniAlert);


            if (File.Exists(settingsFile))
            {
                FileStream fs = new FileStream(settingsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlTextReader xReader = new XmlTextReader(fs);

                try
                {
                    while (xReader.Read())
                    {
                        if (xReader.NodeType == XmlNodeType.Element && xReader.LocalName == "SettingsSerializer")
                        {
                            xmlSettings.ImportFromXml(xReader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AoCEI_Loader.PluginStatus = "Error loading settings: " + ex.Message;
                    throw;
                }
                xReader.Close();
            }

            fMiniAlert.ApplySettings();
        }

        internal void SaveSettings()
        {
            FileStream fs = new FileStream(this.settingsFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            XmlTextWriter xmlTextWriter = new XmlTextWriter(fs, Encoding.UTF8);
            xmlTextWriter.Formatting = Formatting.Indented;
            xmlTextWriter.Indentation = 1;
            xmlTextWriter.IndentChar = '\t';
            xmlTextWriter.WriteStartDocument(true);
            xmlTextWriter.WriteStartElement("Config");
            xmlTextWriter.WriteStartElement("SettingsSerializer");
            this.xmlSettings.ExportToXml(xmlTextWriter);
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndDocument();
            xmlTextWriter.Flush();
            xmlTextWriter.Close();
        }

        #region Designer Code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pTaps = new System.Windows.Forms.Panel();
            this.lHTapPS = new System.Windows.Forms.Label();
            this.lSTapPS = new System.Windows.Forms.Label();
            this.lMTapPS = new System.Windows.Forms.Label();
            this.lTaps = new System.Windows.Forms.Label();
            this.lTotalMana = new System.Windows.Forms.Label();
            this.lTotalStamina = new System.Windows.Forms.Label();
            this.lTotalHealth = new System.Windows.Forms.Label();
            this.lManaTap = new System.Windows.Forms.Label();
            this.lStaminaTap = new System.Windows.Forms.Label();
            this.lHealthTap = new System.Windows.Forms.Label();
            this.lbEncounters = new System.Windows.Forms.ListBox();
            this.pEncDPS = new System.Windows.Forms.Panel();
            this.bCritsOut = new System.Windows.Forms.Button();
            this.lEncDPS = new System.Windows.Forms.Label();
            this.pIncDPS = new System.Windows.Forms.Panel();
            this.bCritsInc = new System.Windows.Forms.Button();
            this.lIncDPS = new System.Windows.Forms.Label();
            this.UIUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.gbPerkAlert = new System.Windows.Forms.GroupBox();
            this.bUnhideMini = new System.Windows.Forms.Button();
            this.rtbPerkAlert = new System.Windows.Forms.RichTextBox();
            this.cmsPerkAlert = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAddRemovePerks = new System.Windows.Forms.ToolStripMenuItem();
            this.autoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bearShamanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.priestOfMitraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tempestOfSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rogueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soldierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.othersSoldierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.watchListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AlertLifeTimer = new System.Windows.Forms.Timer(this.components);
            this.pTaps.SuspendLayout();
            this.pEncDPS.SuspendLayout();
            this.pIncDPS.SuspendLayout();
            this.gbPerkAlert.SuspendLayout();
            this.cmsPerkAlert.SuspendLayout();
            this.SuspendLayout();
            // 
            // pTaps
            // 
            this.pTaps.BackColor = System.Drawing.Color.LightBlue;
            this.pTaps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pTaps.Controls.Add(this.lHTapPS);
            this.pTaps.Controls.Add(this.lSTapPS);
            this.pTaps.Controls.Add(this.lMTapPS);
            this.pTaps.Controls.Add(this.lTaps);
            this.pTaps.Controls.Add(this.lTotalMana);
            this.pTaps.Controls.Add(this.lTotalStamina);
            this.pTaps.Controls.Add(this.lTotalHealth);
            this.pTaps.Controls.Add(this.lManaTap);
            this.pTaps.Controls.Add(this.lStaminaTap);
            this.pTaps.Controls.Add(this.lHealthTap);
            this.pTaps.Location = new System.Drawing.Point(9, 245);
            this.pTaps.Name = "pTaps";
            this.pTaps.Size = new System.Drawing.Size(403, 140);
            this.pTaps.TabIndex = 2;
            // 
            // lHTapPS
            // 
            this.lHTapPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lHTapPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHTapPS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lHTapPS.Location = new System.Drawing.Point(271, 41);
            this.lHTapPS.Name = "lHTapPS";
            this.lHTapPS.Size = new System.Drawing.Size(117, 31);
            this.lHTapPS.TabIndex = 8;
            this.lHTapPS.Text = "0.0";
            this.lHTapPS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lSTapPS
            // 
            this.lSTapPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lSTapPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lSTapPS.ForeColor = System.Drawing.Color.DarkGreen;
            this.lSTapPS.Location = new System.Drawing.Point(271, 72);
            this.lSTapPS.Name = "lSTapPS";
            this.lSTapPS.Size = new System.Drawing.Size(117, 31);
            this.lSTapPS.TabIndex = 7;
            this.lSTapPS.Text = "0.0";
            this.lSTapPS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lMTapPS
            // 
            this.lMTapPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lMTapPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lMTapPS.ForeColor = System.Drawing.Color.Blue;
            this.lMTapPS.Location = new System.Drawing.Point(271, 103);
            this.lMTapPS.Name = "lMTapPS";
            this.lMTapPS.Size = new System.Drawing.Size(117, 31);
            this.lMTapPS.TabIndex = 6;
            this.lMTapPS.Text = "0.0";
            this.lMTapPS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lTaps
            // 
            this.lTaps.BackColor = System.Drawing.Color.Transparent;
            this.lTaps.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTaps.Location = new System.Drawing.Point(3, 9);
            this.lTaps.Name = "lTaps";
            this.lTaps.Size = new System.Drawing.Size(388, 31);
            this.lTaps.TabIndex = 3;
            this.lTaps.Text = "Taps          Total        Per sec";
            this.lTaps.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lTotalMana
            // 
            this.lTotalMana.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lTotalMana.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTotalMana.ForeColor = System.Drawing.Color.Blue;
            this.lTotalMana.Location = new System.Drawing.Point(132, 103);
            this.lTotalMana.Name = "lTotalMana";
            this.lTotalMana.Size = new System.Drawing.Size(133, 31);
            this.lTotalMana.TabIndex = 5;
            this.lTotalMana.Text = "0";
            this.lTotalMana.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lTotalStamina
            // 
            this.lTotalStamina.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTotalStamina.ForeColor = System.Drawing.Color.DarkGreen;
            this.lTotalStamina.Location = new System.Drawing.Point(132, 72);
            this.lTotalStamina.Name = "lTotalStamina";
            this.lTotalStamina.Size = new System.Drawing.Size(133, 31);
            this.lTotalStamina.TabIndex = 4;
            this.lTotalStamina.Text = "0";
            this.lTotalStamina.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lTotalHealth
            // 
            this.lTotalHealth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lTotalHealth.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTotalHealth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lTotalHealth.Location = new System.Drawing.Point(132, 41);
            this.lTotalHealth.Name = "lTotalHealth";
            this.lTotalHealth.Size = new System.Drawing.Size(133, 31);
            this.lTotalHealth.TabIndex = 3;
            this.lTotalHealth.Text = "0";
            this.lTotalHealth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lManaTap
            // 
            this.lManaTap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lManaTap.AutoSize = true;
            this.lManaTap.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lManaTap.ForeColor = System.Drawing.Color.Blue;
            this.lManaTap.Location = new System.Drawing.Point(3, 101);
            this.lManaTap.Name = "lManaTap";
            this.lManaTap.Size = new System.Drawing.Size(97, 31);
            this.lManaTap.TabIndex = 2;
            this.lManaTap.Text = "Mana -";
            // 
            // lStaminaTap
            // 
            this.lStaminaTap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lStaminaTap.AutoSize = true;
            this.lStaminaTap.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lStaminaTap.ForeColor = System.Drawing.Color.DarkGreen;
            this.lStaminaTap.Location = new System.Drawing.Point(3, 70);
            this.lStaminaTap.Name = "lStaminaTap";
            this.lStaminaTap.Size = new System.Drawing.Size(129, 31);
            this.lStaminaTap.TabIndex = 1;
            this.lStaminaTap.Text = "Stamina -";
            // 
            // lHealthTap
            // 
            this.lHealthTap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lHealthTap.AutoSize = true;
            this.lHealthTap.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHealthTap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lHealthTap.Location = new System.Drawing.Point(3, 41);
            this.lHealthTap.Name = "lHealthTap";
            this.lHealthTap.Size = new System.Drawing.Size(109, 31);
            this.lHealthTap.TabIndex = 0;
            this.lHealthTap.Text = "Health -";
            // 
            // lbEncounters
            // 
            this.lbEncounters.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbEncounters.FormattingEnabled = true;
            this.lbEncounters.Location = new System.Drawing.Point(1070, 0);
            this.lbEncounters.Name = "lbEncounters";
            this.lbEncounters.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbEncounters.Size = new System.Drawing.Size(269, 650);
            this.lbEncounters.TabIndex = 4;
            this.lbEncounters.SelectedIndexChanged += new System.EventHandler(this.lbEncounters_SelectedIndexChanged);
            this.lbEncounters.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbEncounters_KeyDown);
            // 
            // pEncDPS
            // 
            this.pEncDPS.BackColor = System.Drawing.Color.LightGreen;
            this.pEncDPS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pEncDPS.Controls.Add(this.bCritsOut);
            this.pEncDPS.Controls.Add(this.lEncDPS);
            this.pEncDPS.Location = new System.Drawing.Point(9, 3);
            this.pEncDPS.Name = "pEncDPS";
            this.pEncDPS.Size = new System.Drawing.Size(532, 60);
            this.pEncDPS.TabIndex = 5;
            // 
            // bCritsOut
            // 
            this.bCritsOut.BackColor = System.Drawing.Color.LightGreen;
            this.bCritsOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bCritsOut.Location = new System.Drawing.Point(347, 3);
            this.bCritsOut.Name = "bCritsOut";
            this.bCritsOut.Size = new System.Drawing.Size(180, 52);
            this.bCritsOut.TabIndex = 5;
            this.bCritsOut.Text = "Crit Bonus";
            this.bCritsOut.UseVisualStyleBackColor = false;
            this.bCritsOut.Click += new System.EventHandler(this.bCritsOut_Click);
            // 
            // lEncDPS
            // 
            this.lEncDPS.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lEncDPS.AutoSize = true;
            this.lEncDPS.BackColor = System.Drawing.Color.Transparent;
            this.lEncDPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lEncDPS.Location = new System.Drawing.Point(3, 13);
            this.lEncDPS.Name = "lEncDPS";
            this.lEncDPS.Size = new System.Drawing.Size(338, 31);
            this.lEncDPS.TabIndex = 4;
            this.lEncDPS.Text = "EncDPS - % Phys - % Mag";
            // 
            // pIncDPS
            // 
            this.pIncDPS.BackColor = System.Drawing.Color.LightCoral;
            this.pIncDPS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pIncDPS.Controls.Add(this.bCritsInc);
            this.pIncDPS.Controls.Add(this.lIncDPS);
            this.pIncDPS.Location = new System.Drawing.Point(9, 69);
            this.pIncDPS.Name = "pIncDPS";
            this.pIncDPS.Size = new System.Drawing.Size(532, 60);
            this.pIncDPS.TabIndex = 6;
            // 
            // bCritsInc
            // 
            this.bCritsInc.BackColor = System.Drawing.Color.LightCoral;
            this.bCritsInc.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bCritsInc.Location = new System.Drawing.Point(347, 2);
            this.bCritsInc.Name = "bCritsInc";
            this.bCritsInc.Size = new System.Drawing.Size(180, 52);
            this.bCritsInc.TabIndex = 6;
            this.bCritsInc.Text = "Crit Bonus";
            this.bCritsInc.UseVisualStyleBackColor = false;
            this.bCritsInc.Click += new System.EventHandler(this.bCritsInc_Click);
            // 
            // lIncDPS
            // 
            this.lIncDPS.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lIncDPS.AutoSize = true;
            this.lIncDPS.BackColor = System.Drawing.Color.Transparent;
            this.lIncDPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lIncDPS.Location = new System.Drawing.Point(3, 13);
            this.lIncDPS.Name = "lIncDPS";
            this.lIncDPS.Size = new System.Drawing.Size(328, 31);
            this.lIncDPS.TabIndex = 4;
            this.lIncDPS.Text = "IncDPS - % Phys - % Mag";
            // 
            // UIUpdateTimer
            // 
            this.UIUpdateTimer.Tick += new System.EventHandler(this.UIUpdateTimer_Tick);
            // 
            // gbPerkAlert
            // 
            this.gbPerkAlert.Controls.Add(this.bUnhideMini);
            this.gbPerkAlert.Controls.Add(this.rtbPerkAlert);
            this.gbPerkAlert.Location = new System.Drawing.Point(4, 136);
            this.gbPerkAlert.Name = "gbPerkAlert";
            this.gbPerkAlert.Size = new System.Drawing.Size(537, 103);
            this.gbPerkAlert.TabIndex = 7;
            this.gbPerkAlert.TabStop = false;
            this.gbPerkAlert.Text = "PerkAlert™";
            // 
            // bUnhideMini
            // 
            this.bUnhideMini.Location = new System.Drawing.Point(438, 77);
            this.bUnhideMini.Name = "bUnhideMini";
            this.bUnhideMini.Size = new System.Drawing.Size(75, 23);
            this.bUnhideMini.TabIndex = 1;
            this.bUnhideMini.Text = "Unhide Mini";
            this.bUnhideMini.UseVisualStyleBackColor = true;
            this.bUnhideMini.Click += new System.EventHandler(this.bUnhideMini_Click);
            // 
            // rtbPerkAlert
            // 
            this.rtbPerkAlert.BackColor = System.Drawing.Color.Black;
            this.rtbPerkAlert.ContextMenuStrip = this.cmsPerkAlert;
            this.rtbPerkAlert.DetectUrls = false;
            this.rtbPerkAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbPerkAlert.ForeColor = System.Drawing.Color.White;
            this.rtbPerkAlert.Location = new System.Drawing.Point(5, 19);
            this.rtbPerkAlert.Name = "rtbPerkAlert";
            this.rtbPerkAlert.ReadOnly = true;
            this.rtbPerkAlert.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbPerkAlert.Size = new System.Drawing.Size(403, 81);
            this.rtbPerkAlert.TabIndex = 0;
            this.rtbPerkAlert.Text = "Right-click for settings.\nClick to open/close mini-window.\nDouble-click to test a" +
    "n alert.";
            this.rtbPerkAlert.Click += new System.EventHandler(this.rtbPerkAlert_Click);
            this.rtbPerkAlert.DoubleClick += new System.EventHandler(this.rtbPerkAlert_DoubleClick);
            // 
            // cmsPerkAlert
            // 
            this.cmsPerkAlert.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAddRemovePerks,
            this.watchListToolStripMenuItem});
            this.cmsPerkAlert.Name = "cmsPerkAlert";
            this.cmsPerkAlert.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.cmsPerkAlert.Size = new System.Drawing.Size(137, 48);
            // 
            // tsmiAddRemovePerks
            // 
            this.tsmiAddRemovePerks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoToolStripMenuItem,
            this.toolStripSeparator1,
            this.bearShamanToolStripMenuItem,
            this.priestOfMitraToolStripMenuItem,
            this.tempestOfSetToolStripMenuItem,
            this.mageToolStripMenuItem,
            this.rogueToolStripMenuItem,
            this.soldierToolStripMenuItem,
            this.othersSoldierToolStripMenuItem});
            this.tsmiAddRemovePerks.Name = "tsmiAddRemovePerks";
            this.tsmiAddRemovePerks.Size = new System.Drawing.Size(136, 22);
            this.tsmiAddRemovePerks.Text = "Select Perks";
            // 
            // autoToolStripMenuItem
            // 
            this.autoToolStripMenuItem.Checked = true;
            this.autoToolStripMenuItem.CheckOnClick = true;
            this.autoToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoToolStripMenuItem.Name = "autoToolStripMenuItem";
            this.autoToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.autoToolStripMenuItem.Text = "Auto-Detected Class";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(179, 6);
            // 
            // bearShamanToolStripMenuItem
            // 
            this.bearShamanToolStripMenuItem.CheckOnClick = true;
            this.bearShamanToolStripMenuItem.Name = "bearShamanToolStripMenuItem";
            this.bearShamanToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.bearShamanToolStripMenuItem.Text = "Bear Shaman";
            // 
            // priestOfMitraToolStripMenuItem
            // 
            this.priestOfMitraToolStripMenuItem.CheckOnClick = true;
            this.priestOfMitraToolStripMenuItem.Name = "priestOfMitraToolStripMenuItem";
            this.priestOfMitraToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.priestOfMitraToolStripMenuItem.Text = "Priest of Mitra";
            // 
            // tempestOfSetToolStripMenuItem
            // 
            this.tempestOfSetToolStripMenuItem.CheckOnClick = true;
            this.tempestOfSetToolStripMenuItem.Name = "tempestOfSetToolStripMenuItem";
            this.tempestOfSetToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.tempestOfSetToolStripMenuItem.Text = "Tempest of Set";
            // 
            // mageToolStripMenuItem
            // 
            this.mageToolStripMenuItem.CheckOnClick = true;
            this.mageToolStripMenuItem.Name = "mageToolStripMenuItem";
            this.mageToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.mageToolStripMenuItem.Text = "Mage";
            // 
            // rogueToolStripMenuItem
            // 
            this.rogueToolStripMenuItem.CheckOnClick = true;
            this.rogueToolStripMenuItem.Name = "rogueToolStripMenuItem";
            this.rogueToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.rogueToolStripMenuItem.Text = "Rogue";
            // 
            // soldierToolStripMenuItem
            // 
            this.soldierToolStripMenuItem.CheckOnClick = true;
            this.soldierToolStripMenuItem.Name = "soldierToolStripMenuItem";
            this.soldierToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.soldierToolStripMenuItem.Text = "Soldier (You)";
            // 
            // othersSoldierToolStripMenuItem
            // 
            this.othersSoldierToolStripMenuItem.CheckOnClick = true;
            this.othersSoldierToolStripMenuItem.Name = "othersSoldierToolStripMenuItem";
            this.othersSoldierToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.othersSoldierToolStripMenuItem.Text = "Soldier (Others)";
            // 
            // watchListToolStripMenuItem
            // 
            this.watchListToolStripMenuItem.Name = "watchListToolStripMenuItem";
            this.watchListToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.watchListToolStripMenuItem.Text = "Watch List";
            this.watchListToolStripMenuItem.Click += new System.EventHandler(this.watchListToolStripMenuItem_Click);
            // 
            // AlertLifeTimer
            //
            this.AlertLifeTimer.Tick += new System.EventHandler(this.AlertLifeTimer_Tick);
            // 
            // PluginTab
            // 
            this.Controls.Add(this.lbEncounters);
            this.Controls.Add(this.gbPerkAlert);
            this.Controls.Add(this.pIncDPS);
            this.Controls.Add(this.pEncDPS);
            this.Controls.Add(this.pTaps);
            this.Name = "PluginTab";
            this.Size = new System.Drawing.Size(1339, 650);
            this.pTaps.ResumeLayout(false);
            this.pTaps.PerformLayout();
            this.pEncDPS.ResumeLayout(false);
            this.pEncDPS.PerformLayout();
            this.pIncDPS.ResumeLayout(false);
            this.pIncDPS.PerformLayout();
            this.gbPerkAlert.ResumeLayout(false);
            this.cmsPerkAlert.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion


        internal void oFormActMain_AfterCombatAction(bool isImport, CombatActionEventArgs actionInfo)
        {
            if (!isImport && AoCEI_Loader.oOptions.MinDashboardUpdateInterval.TotalSeconds == 0 && activeExtraEncounter.HasMe)
            {
                this.UIThread(() => RefreshDashboardUI(activeExtraEncounter));
            }

        }

        internal void oFormActMain_OnCombatStart(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            activeExtraEncounter = new EncounterExtraData(true);

            if (AoCEI_Loader.oOptions.MinDashboardUpdateInterval.TotalMilliseconds > 0)
            {
                this.UIThread(() => this.StartUITimer());
            }
        }



        internal void oFormActMain_OnCombatEnd(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            if (activeExtraEncounter.Duration.TotalSeconds > AoCEI_Loader.oOptions.MinEncounterDuration.TotalSeconds && activeExtraEncounter.HasMe)
            {
                allExtraData.Add(activeExtraEncounter);
                selectedEncounters.Clear();
                selectedEncounters.Add(activeExtraEncounter);
                MergeSelectedEncounters();
            }

            if (AoCEI_Loader.oOptions.MinDashboardUpdateInterval.TotalMilliseconds > 0)
            {
                this.UIThread(() => UIUpdateTimer.Enabled = false);
            }

            this.UIThreadInvoke(() => RefreshEncountersListBox()); //Not sure why I have to Invoke here. Shouldn't lock work?

        }

        private void StartUITimer()
        {
            UIUpdateTimer.Interval = (int)AoCEI_Loader.oOptions.MinDashboardUpdateInterval.TotalMilliseconds;
            UIUpdateTimer.Start();
        }

        private void UIUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (activeExtraEncounter.HasMe)
            {
                RefreshDashboardUI(activeExtraEncounter);
            }
        }

        private void RefreshDashboardUI(EncounterExtraData extraEncounter)
        {
            TotalHealth = extraEncounter.TotalHealth.ToString();
            TotalStamina = extraEncounter.TotalStamina.ToString();
            TotalMana = extraEncounter.TotalMana.ToString();
            HTapPS = extraEncounter.HTapPS.ToString("#0.#");
            STapPS = extraEncounter.STapPS.ToString("#0.#");
            MTapPS = extraEncounter.MTapPS.ToString("#0.#");
            EncDPS = extraEncounter.TotalDPSOut.ToString("#0.") + " - " + extraEncounter.PercentPhysicalOut + " - " + extraEncounter.PercentMagicalOut;
            IncDPS = extraEncounter.TotalDPSInc.ToString("#0.") + " - " + extraEncounter.PercentPhysicalInc + " - " + extraEncounter.PercentMagicalInc;
            CritBonusOut = extraEncounter.BonusCritDPSOut.ToString("#0.#");
            CritBonusInc = extraEncounter.BonusCritDPSInc.ToString("#0.#");
        }

        private void RefreshEncountersListBox()
        {
            lbEncounters.Items.Clear();

            lock (allExtraData) // Don't see exactly how, but I'm hoping this fixes an ArgumentNullException.
            {
                foreach (EncounterExtraData item in allExtraData)
                {
                    lbEncounters.Items.Add(item.EncounterName);
                }
            }
        }




        private void lbEncounters_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedEncounters.Clear();

            foreach (int index in lbEncounters.SelectedIndices)
            {
                selectedEncounters.Add(allExtraData[index]);
            }

            MergeSelectedEncounters();

            this.UIThread(() => RefreshDashboardUI(mergedSelectedEncounters));
        }

        private void MergeSelectedEncounters()
        {
            mergedSelectedEncounters = new EncounterExtraData();

            foreach (EncounterExtraData encounter in selectedEncounters)
            {
                mergedSelectedEncounters.Merge(encounter);
            }
        }




        private void lbEncounters_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && lbEncounters.Items.Count > 0)
            {
                int _selectedIndex = lbEncounters.SelectedIndex;

                foreach (EncounterExtraData encounter in selectedEncounters)
                {
                    allExtraData.Remove(encounter);
                }

                RefreshEncountersListBox();

                if (_selectedIndex < lbEncounters.Items.Count)
                {
                    lbEncounters.SetSelected(_selectedIndex, true);
                }
                else if (_selectedIndex > 0)
                {
                    lbEncounters.SetSelected(_selectedIndex - 1, true);
                }
            }
        }

        private void bCritsOut_Click(object sender, EventArgs e)
        {
            if (FormCritBonusReportOut.opened)
            {
                Form active = Application.OpenForms.OfType<FormCritBonusReportOut>().First();
                active.Close();
            }
            FormCritBonusReport fCritInfo = new FormCritBonusReportOut();
            fCritInfo.Show();
        }

        private void bCritsInc_Click(object sender, EventArgs e)
        {
            if (FormCritBonusReportInc.opened)
            {
                Form active = Application.OpenForms.OfType<FormCritBonusReportInc>().First();
                active.Close();
            }
            FormCritBonusReportInc fCritInfo = new FormCritBonusReportInc();
            fCritInfo.Show();
        }

        private void rtbPerkAlert_Click(object sender, EventArgs e)
        {
            if (FormMiniAlert.Opened)
            {
                fMiniAlert.Hide();
            }
            else
            {
                fMiniAlert.Show();
            }
        }

        private void rtbPerkAlert_DoubleClick(object sender, EventArgs e)
        {
            PerkAlert_Alert(this, new AlertEventArgs("TARGET", "PERK"));
        }

        internal void PerkAlert_Alert(object sender, AlertEventArgs e)
        {
            ActiveAlert alert = new ActiveAlert(e);
            alert.OnInactive += alert_OnInactive;
            lock (activeAlerts)
            {
                if (!activeAlerts.Select(x => x.Perk).Contains(e.Perk))
                {
                    ActGlobals.oFormActMain.TTS(e.Perk);
                }
                activeAlerts.Add(alert);
            }

            this.UIThread(() => ShowAlerts());
        }

        void alert_OnInactive(Object sender, EventArgs e)
        {
            ((ActiveAlert)sender).OnInactive -= this.alert_OnInactive;
            lock (activeAlerts)
            {
                activeAlerts.Remove((ActiveAlert)sender);

                this.UIThread(() => ShowAlerts());
                if (activeAlerts.Count == 0)
                {
                    this.UIThread(() => AlertLifeTimer_Tick(null, null));
                }
            }
        }

        private void ShowAlerts()
        {
            FormMiniAlert.AlertActive = true;
            rtbPerkAlert.Clear();
            fMiniAlert.MiniText = String.Empty;
            fMiniAlert.MiniHeight = (fMiniAlert.NormalHeight > (int)(activeAlerts.Count * 24.25F)) ? fMiniAlert.NormalHeight : (int)(activeAlerts.Count * 24.25F);
            

            lock (activeAlerts)
            {
                foreach (ActiveAlert alert in activeAlerts)
                {
                    rtbPerkAlert.Text += alert.Perk + " - " + alert.Target.ToUpperInvariant() + '\n';
                    fMiniAlert.MiniText += alert.Perk + " - " + alert.Target.ToUpperInvariant() + '\n';
                }
            }

            if (fMiniAlert != null)
            {
                fMiniAlert.MiniOpacity = 1.00;
            }

            AlertLifeTimer.Stop();
            AlertLifeTimer.Interval = (int)AoCEI_Loader.oOptions.MaxTextAlertDuration.TotalMilliseconds;
            AlertLifeTimer.Start();
        }

        private void AlertLifeTimer_Tick(object sender, EventArgs e)
        {
            if (fMiniAlert != null)
            {
                if (FormMiniAlert.Transparent)
                {
                    fMiniAlert.MiniOpacity = 0.01;
                }

                fMiniAlert.MiniText = String.Empty;
                fMiniAlert.MiniHeight = fMiniAlert.NormalHeight;
            }

            FormMiniAlert.AlertActive = false;
            rtbPerkAlert.Clear();
            AlertLifeTimer.Stop();
        }

        private void bUnhideMini_Click(object sender, EventArgs e)
        {
            if (fMiniAlert != null)
            {
                if (FormMiniAlert.Transparent)
                {
                    fMiniAlert.MakeVisible();
                }

                fMiniAlert.Show();
            }

        }

        private void watchListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fEditWatch = new FormEditWatchList();
            fEditWatch.Show();
        }

    }



    public class AoCEI_Loader : IActPluginV1
    {
        private static Label lStatus;
        private static TreeNode optionsNode;
        private static PluginTab _oPluginTab = new PluginTab();
        private static Options_AoC_Extra_Info _oOptionsTab = new Options_AoC_Extra_Info();

        public static string PluginStatus
        {
            get { return lStatus.Text; }
            set { lStatus.Text = value; }
        }

        public static PluginTab oPluginTab
        {
            get { return _oPluginTab; }
        }

        public static Options_AoC_Extra_Info oOptions
        {
            get { return _oOptionsTab; }
        }


        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatus)
        {
            lStatus = pluginStatus;
            pluginScreenSpace.Controls.Add(_oPluginTab);
            _oPluginTab.Dock = DockStyle.Fill;

            optionsNode = ActGlobals.oFormActMain.OptionsTreeView.Nodes.Add("AoC Extra Info");
            ActGlobals.oFormActMain.OptionsControlSets.Add("AoC Extra Info", new List<Control> { _oOptionsTab });

            ActGlobals.oFormActMain.AfterCombatAction += _oPluginTab.oFormActMain_AfterCombatAction;
            ActGlobals.oFormActMain.OnCombatStart += _oPluginTab.oFormActMain_OnCombatStart;
            ActGlobals.oFormActMain.OnCombatEnd += _oPluginTab.oFormActMain_OnCombatEnd;
            ActGlobals.oFormActMain.LogFileChanged += ClassDetector.oFormActMain_LogFileChanged;
            ActGlobals.oFormActMain.OnLogLineRead += PerkAlert.oFormActMain_OnLogLineRead;
            ActGlobals.oFormActMain.OnCombatStart += PerkAlert.oFormActMain_OnCombatStart;
            ActGlobals.oFormActMain.OnCombatEnd += PerkAlert.oFormActMain_OnCombatEnd;
            PerkAlert.Alert += _oPluginTab.PerkAlert_Alert;

            lStatus.Text = "Plugin Started \nUser Interface available in the AoC Extra Info plugin tab \nOptions available in ACT's Options tab";

            _oOptionsTab.LoadSettings();
            _oPluginTab.LoadSettings();
        }





        public void DeInitPlugin()
        {
            ActGlobals.oFormActMain.AfterCombatAction -= _oPluginTab.oFormActMain_AfterCombatAction;
            ActGlobals.oFormActMain.OnCombatStart -= _oPluginTab.oFormActMain_OnCombatStart;
            ActGlobals.oFormActMain.OnCombatEnd -= _oPluginTab.oFormActMain_OnCombatEnd;
            ActGlobals.oFormActMain.LogFileChanged -= ClassDetector.oFormActMain_LogFileChanged;
            ActGlobals.oFormActMain.OnLogLineRead -= PerkAlert.oFormActMain_OnLogLineRead;
            ActGlobals.oFormActMain.OnCombatStart -= PerkAlert.oFormActMain_OnCombatStart;
            PerkAlert.Alert -= _oPluginTab.PerkAlert_Alert;

            if (_oPluginTab.ActiveExtraEncounter != null)
            {
                _oPluginTab.ActiveExtraEncounter.MakeInactive();
            }

            if (optionsNode != null)
            {
                optionsNode.Remove();
                ActGlobals.oFormActMain.OptionsControlSets.Remove("AoC Extra Info");
            }

            _oOptionsTab.SaveSettings();
            _oPluginTab.SaveSettings();
            lStatus.Text = "Plugin Exited";
        }
    }


    public class FormMiniAlert : Form
    {
        private RichTextBox rtbMiniAlert;
        private readonly int _transparentMinHeight = 30;
        private readonly int _visibleMinHeight = 69;
        private Timer fadeOut;
        public static bool Opened { get; set; }
        public static bool Transparent { get; set; } // This allows me to reset to the original state after an alert
        public static bool AlertActive { get; set; }
        public int NormalHeight { get; set; }

        public int TransparentMinHeight
        {
            get { return _transparentMinHeight; }
        }

        public int VisibleMinHeight
        {
            get { return _visibleMinHeight; }
        }

        public string MiniText
        {
            get { return rtbMiniAlert.Text; }
            set { rtbMiniAlert.Text = value; }
        }

        public double MiniOpacity
        {
            get { return this.Opacity; }
            set { this.Opacity = value; }
        }

        public Color MiniBackground
        {
            get { return rtbMiniAlert.BackColor; }
            set { rtbMiniAlert.BackColor = value; }
        }

        public FormBorderStyle MiniBorders
        {
            get { return FormBorderStyle; }
            set { FormBorderStyle = value; }
        }

        public int MiniHeight
        {
            get { return this.ClientSize.Height; }
            set { this.ClientSize = new Size(this.ClientSize.Width, value); }
        }

        public int TitleBarHeight
        {
            get { return this.Height - this.ClientSize.Height; }
        }

        public FormMiniAlert()
        {
            InitializeComponent();
            NormalHeight = this.ClientSize.Height;
        }

        private void InitializeComponent()
        {
            rtbMiniAlert = new RichTextBox();
            this.SuspendLayout();
            // 
            // MiniAlert
            //
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MinimumSize = new Size(400, 69);
            this.ClientSize = new System.Drawing.Size(400, 69);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MiniAlert";
            this.Opacity = 100D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "MiniAlert";
            this.TopMost = true;
            this.Controls.Add(this.rtbMiniAlert);
            this.VisibleChanged += FormMiniAlert_VisibleChanged;
            this.FormClosing += FormMiniAlert_FormClosing;
            this.FormClosed += FormMiniAlert_FormClosed;
            this.ResizeEnd += FormMiniAlert_ResizeEnd;
            this.Shown += FormMiniAlert_Shown;
            //
            // rtbMiniAlert
            //
            this.rtbMiniAlert.BackColor = System.Drawing.Color.Black;
            this.rtbMiniAlert.DetectUrls = false;
            this.rtbMiniAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbMiniAlert.ForeColor = System.Drawing.Color.White;
            this.rtbMiniAlert.Location = new System.Drawing.Point(5, 19);
            this.rtbMiniAlert.Name = "rtbMiniAlert";
            this.rtbMiniAlert.ReadOnly = true;
            this.rtbMiniAlert.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbMiniAlert.Size = new System.Drawing.Size(400, 69);
            this.rtbMiniAlert.TabIndex = 0;
            this.rtbMiniAlert.Dock = DockStyle.Fill;
            this.rtbMiniAlert.Click += rtbMiniAlert_Click;
            this.rtbMiniAlert.SelectionAlignment = HorizontalAlignment.Center;
            this.rtbMiniAlert.Text = "Click to hide/unhide window. \nWindow will automatically unhide during alerts.";
            this.ResumeLayout(false);
        }

        void FormMiniAlert_Shown(object sender, EventArgs e)
        {
            NormalHeight = this.ClientSize.Height;
        }

        void FormMiniAlert_ResizeEnd(object sender, EventArgs e)
        {
            NormalHeight = this.ClientSize.Height;
        }

        void FormMiniAlert_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        // Should never close, but just in case...
        void FormMiniAlert_FormClosed(object sender, FormClosedEventArgs e)
        {
            Opened = false;
        }

        void FormMiniAlert_VisibleChanged(object sender, EventArgs e)
        {
            Opened = (this.Visible) ? true : false;
        }


        void rtbMiniAlert_Click(object sender, EventArgs e)
        {
            if (!AlertActive)
            {
                ToggleTransparency();
                
                if (fadeOut != null)
                {
                    fadeOut.Stop();
                    fadeOut.Tick -= fadeOut_Tick;
                    fadeOut.Dispose();
                }
            }
        }

        public void ToggleTransparency()
        {
            if (Transparent)
            {
                MakeVisible();
            }
            else
            {
                MakeTransparent();
            }
        }

        public void MakeVisible()
        {
            if (FormBorderStyle == FormBorderStyle.None)
            {
                FormBorderStyle = FormBorderStyle.SizableToolWindow;
                this.Location = new Point(this.Left, this.Top - TitleBarHeight);
                MiniHeight = NormalHeight;
            }
            this.MinimumSize = new Size(400, VisibleMinHeight);
            MiniOpacity = 1.00;
            Transparent = false;
        }

        public void MakeTransparent()
        {
            MiniOpacity = 0.01;
            Transparent = true;
            if (FormBorderStyle == FormBorderStyle.SizableToolWindow)
            {
                this.Location = new Point(this.Left, this.Top + TitleBarHeight);
                FormBorderStyle = FormBorderStyle.None;
            }
            this.MinimumSize = new Size(400, TransparentMinHeight);
            MiniHeight = NormalHeight;

        }

        public void ApplySettings()
        {
            if (Opened)
            {
                this.Show();
            }
            else
            {
                this.Hide();
            }
            
            
            if (Transparent)
            {
                this.NormalHeight = MiniHeight - TitleBarHeight;
                this.MinimumSize = new Size(400, TransparentMinHeight);
                this.MiniHeight = NormalHeight;
                FormBorderStyle = FormBorderStyle.None;
                fadeOut = new Timer();
                fadeOut.Interval = 50;
                fadeOut.Tick += fadeOut_Tick;
                fadeOut.Start();
            }
            else
            {
                this.MakeVisible();
            }
        }

        void fadeOut_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.03D;
            if (this.Opacity <= 0.01D)
            {
                fadeOut.Tick -= fadeOut_Tick;
                fadeOut.Stop();
                fadeOut.Dispose();
            }
        }
    }

    public class ListViewItemComparer : IComparer
    {
        private int _colSelect;
        public SortOrder Order { get; set; }

        public ListViewItemComparer()
        {
            _colSelect = 0;
            Order = SortOrder.Ascending;
        }

        public ListViewItemComparer(int column, SortOrder order)
        {
            _colSelect = column;
            Order = order;
        }

        public int Compare(object x, object y)
        {
            int xValue, yValue;
            string xColText = ((ListViewItem)x).SubItems[_colSelect].Text;
            string yColText = ((ListViewItem)y).SubItems[_colSelect].Text;
            char delim = '(';

            if (int.TryParse(xColText.Split(delim)[0], out xValue) && int.TryParse(yColText.Split(delim)[0], out yValue))
            {
                if (Order == SortOrder.Ascending)
                {
                    return (xValue.CompareTo(yValue));
                }
                else
                {
                    return -(xValue.CompareTo(yValue));
                }
            }
            else
            {
                if (Order == SortOrder.Ascending)
                {
                    return String.Compare(((ListViewItem)x).SubItems[_colSelect].Text, ((ListViewItem)y).SubItems[_colSelect].Text);
                }
                else
                {
                    return -(String.Compare(((ListViewItem)x).SubItems[_colSelect].Text, ((ListViewItem)y).SubItems[_colSelect].Text));
                }
            }
        }
    }

    public class FormEditWatchList : Form
    {
        private ListView _lvWatchList;
        private ColumnHeader _lvWatchItem;
        private ColumnHeader _lvPerk;
        private Panel pWatchListControls;
        private RadioButton rbUnbindingCharm;
        private RadioButton rbTaintedWeapons;
        private RadioButton rbFinelyHoned;
        private TextBox tbNewWatch;
        private Label lWatchName;
        private Button bResetWatchDefaults;
        private Button bApplyWatchChanges;
        private Button bRemoveWatch;
        private Button bAddWatch;
        private static int windowTop, windowLeft;
        private int sortedColumn = -1;
        private SortOrder order;

        protected ListView lvWatchList
        {
            get { return _lvWatchList; }
        }

        protected ColumnHeader lvWatchItem
        {
            get { return _lvWatchItem; }
        }

        public FormEditWatchList()
        {
            InitializeComponent();

            PopulateListView();

            this._lvWatchList.ListViewItemSorter = new ListViewItemComparer();
        }

        protected virtual void PopulateListView()
        {
            _lvWatchList.Items.Clear();

            foreach (KeyValuePair<string, string> watch in PerkAlert.WatchListRogue)
            {
                _lvWatchList.Items.Add(watch.Key).SubItems.Add(watch.Value);
            }

            foreach (string ability in PerkAlert.WatchListMage)
            {
                _lvWatchList.Items.Add(ability).SubItems.Add("Unbinding Charm");
            }
        }

        private void PopulateListView(Dictionary<string, string> rogueDict, List<string> mageList)
        {
            _lvWatchList.Items.Clear();

            foreach (KeyValuePair<string, string> watch in rogueDict)
            {
                _lvWatchList.Items.Add(watch.Key).SubItems.Add(watch.Value);
            }

            foreach (string ability in mageList)
            {
                _lvWatchList.Items.Add(ability).SubItems.Add("Unbinding Charm");
            }
        }

        protected virtual void ExportListView()
        {
            PerkAlert.WatchListMage.Clear();
            PerkAlert.WatchListRogue.Clear();

            foreach (ListViewItem item in _lvWatchList.Items)
            {
                if (item.SubItems[1].Text == "Finely Honed")
                {
                    PerkAlert.WatchListRogue[item.SubItems[0].Text] = "Finely Honed";
                }
                else if (item.SubItems[1].Text == "Tainted Weapons")
                {
                    PerkAlert.WatchListRogue[item.SubItems[0].Text] = "Tainted Weapons";
                }
                else if (item.SubItems[1].Text == "Unbinding Charm")
                {
                    PerkAlert.WatchListMage.Add(item.SubItems[0].Text);

                }
            }
        }

        //Consider restructuring for 1 exit.
        protected virtual void AddListItem(string item, string subItem)
        {
            foreach (ListViewItem watch in _lvWatchList.Items)
            {
                if (watch.Text == item)
                {
                    MessageBox.Show("That item is already on the watch list.", "No Duplicates");
                    return;
                }
            }

            _lvWatchList.Items.Add(item).SubItems.Add(subItem);
            this.bApplyWatchChanges.BackColor = Color.LightGreen;
        }

        protected virtual void RemoveListItem(int index)
        {
            _lvWatchList.Items[index].Remove();
            this.bApplyWatchChanges.BackColor = Color.LightGreen;
        }

        protected virtual void RemoveListItem(string item)
        {
            try
            {
                _lvWatchList.FindItemWithText(item).Remove();
                tbNewWatch.Text = String.Empty;
                this.bApplyWatchChanges.BackColor = Color.LightGreen;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Item not found. Please check for typos or just select the item from above.", "Error");
            }
        }

        protected virtual void lvWatchList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (sortedColumn == e.Column)
            {
                order = (order == SortOrder.Ascending) ? (SortOrder.Descending) : (SortOrder.Ascending);
            }
            sortedColumn = e.Column;
            this._lvWatchList.ListViewItemSorter = new ListViewItemComparer(e.Column, order);
        }

        private void InitializeComponent()
        {
            this._lvWatchList = new System.Windows.Forms.ListView();
            this._lvWatchItem = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._lvPerk = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pWatchListControls = new System.Windows.Forms.Panel();
            this.bAddWatch = new System.Windows.Forms.Button();
            this.bRemoveWatch = new System.Windows.Forms.Button();
            this.bApplyWatchChanges = new System.Windows.Forms.Button();
            this.bResetWatchDefaults = new System.Windows.Forms.Button();
            this.lWatchName = new System.Windows.Forms.Label();
            this.tbNewWatch = new System.Windows.Forms.TextBox();
            this.rbFinelyHoned = new System.Windows.Forms.RadioButton();
            this.rbTaintedWeapons = new System.Windows.Forms.RadioButton();
            this.rbUnbindingCharm = new System.Windows.Forms.RadioButton();
            this.pWatchListControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // _lvWatchList
            // 
            this._lvWatchList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._lvWatchItem,
            this._lvPerk});
            this._lvWatchList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lvWatchList.FullRowSelect = true;
            this._lvWatchList.GridLines = true;
            this._lvWatchList.Location = new System.Drawing.Point(0, 0);
            this._lvWatchList.MultiSelect = false;
            this._lvWatchList.Name = "_lvWatchList";
            this._lvWatchList.Size = new System.Drawing.Size(347, 260);
            this._lvWatchList.TabIndex = 0;
            this._lvWatchList.UseCompatibleStateImageBehavior = false;
            this._lvWatchList.View = System.Windows.Forms.View.Details;
            this._lvWatchList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvWatchList_ColumnClick);
            // 
            // _lvWatchItem
            // 
            this._lvWatchItem.Text = "Watch";
            this._lvWatchItem.Width = 183;
            // 
            // _lvPerk
            // 
            this._lvPerk.Text = "Perk";
            this._lvPerk.Width = 133;
            // 
            // pWatchListControls
            // 
            this.pWatchListControls.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pWatchListControls.Controls.Add(this.rbUnbindingCharm);
            this.pWatchListControls.Controls.Add(this.rbTaintedWeapons);
            this.pWatchListControls.Controls.Add(this.rbFinelyHoned);
            this.pWatchListControls.Controls.Add(this.tbNewWatch);
            this.pWatchListControls.Controls.Add(this.lWatchName);
            this.pWatchListControls.Controls.Add(this.bResetWatchDefaults);
            this.pWatchListControls.Controls.Add(this.bApplyWatchChanges);
            this.pWatchListControls.Controls.Add(this.bRemoveWatch);
            this.pWatchListControls.Controls.Add(this.bAddWatch);
            this.pWatchListControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pWatchListControls.Location = new System.Drawing.Point(0, 146);
            this.pWatchListControls.Name = "pWatchListControls";
            this.pWatchListControls.Size = new System.Drawing.Size(347, 114);
            this.pWatchListControls.TabIndex = 1;
            // 
            // bAddWatch
            // 
            this.bAddWatch.Location = new System.Drawing.Point(12, 4);
            this.bAddWatch.Name = "bAddWatch";
            this.bAddWatch.Size = new System.Drawing.Size(75, 23);
            this.bAddWatch.TabIndex = 0;
            this.bAddWatch.Text = "Add";
            this.bAddWatch.UseVisualStyleBackColor = true;
            this.bAddWatch.Click += bAddWatch_Click;
            // 
            // bRemoveWatch
            // 
            this.bRemoveWatch.Location = new System.Drawing.Point(93, 4);
            this.bRemoveWatch.Name = "bRemoveWatch";
            this.bRemoveWatch.Size = new System.Drawing.Size(75, 23);
            this.bRemoveWatch.TabIndex = 1;
            this.bRemoveWatch.Text = "Remove";
            this.bRemoveWatch.UseVisualStyleBackColor = true;
            this.bRemoveWatch.Click += bRemoveWatch_Click;
            // 
            // bApplyWatchChanges
            // 
            this.bApplyWatchChanges.Location = new System.Drawing.Point(174, 4);
            this.bApplyWatchChanges.Name = "bApplyWatchChanges";
            this.bApplyWatchChanges.Size = new System.Drawing.Size(75, 23);
            this.bApplyWatchChanges.TabIndex = 2;
            this.bApplyWatchChanges.Text = "Apply";
            this.bApplyWatchChanges.UseVisualStyleBackColor = true;
            this.bApplyWatchChanges.Click += bApplyWatchChanges_Click;
            // 
            // bResetWatchDefaults
            // 
            this.bResetWatchDefaults.Location = new System.Drawing.Point(255, 4);
            this.bResetWatchDefaults.Name = "bResetWatchDefaults";
            this.bResetWatchDefaults.Size = new System.Drawing.Size(75, 23);
            this.bResetWatchDefaults.TabIndex = 2;
            this.bResetWatchDefaults.Text = "Defaults";
            this.bResetWatchDefaults.UseVisualStyleBackColor = true;
            this.bResetWatchDefaults.Click += bResetWatchDefaults_Click;
            // 
            // lWatchName
            // 
            this.lWatchName.AutoSize = true;
            this.lWatchName.Location = new System.Drawing.Point(9, 47);
            this.lWatchName.Name = "lWatchName";
            this.lWatchName.Size = new System.Drawing.Size(41, 13);
            this.lWatchName.TabIndex = 3;
            this.lWatchName.Text = "Name: ";
            // 
            // tbNewWatch
            // 
            this.tbNewWatch.Location = new System.Drawing.Point(56, 44);
            this.tbNewWatch.Name = "tbNewWatch";
            this.tbNewWatch.Size = new System.Drawing.Size(274, 20);
            this.tbNewWatch.TabIndex = 4;
            // 
            // rbFinelyHoned
            // 
            this.rbFinelyHoned.AutoSize = true;
            this.rbFinelyHoned.BackColor = System.Drawing.SystemColors.Control;
            this.rbFinelyHoned.Location = new System.Drawing.Point(12, 73);
            this.rbFinelyHoned.Name = "rbFinelyHoned";
            this.rbFinelyHoned.Size = new System.Drawing.Size(87, 17);
            this.rbFinelyHoned.TabIndex = 5;
            this.rbFinelyHoned.TabStop = true;
            this.rbFinelyHoned.Text = "Finely Honed";
            this.rbFinelyHoned.UseVisualStyleBackColor = false;
            // 
            // rbTaintedWeapons
            // 
            this.rbTaintedWeapons.AutoSize = true;
            this.rbTaintedWeapons.Location = new System.Drawing.Point(105, 73);
            this.rbTaintedWeapons.Name = "rbTaintedWeapons";
            this.rbTaintedWeapons.Size = new System.Drawing.Size(110, 17);
            this.rbTaintedWeapons.TabIndex = 6;
            this.rbTaintedWeapons.TabStop = true;
            this.rbTaintedWeapons.Text = "Tainted Weapons";
            this.rbTaintedWeapons.UseVisualStyleBackColor = true;
            // 
            // rbUnbindingCharm
            // 
            this.rbUnbindingCharm.AutoSize = true;
            this.rbUnbindingCharm.Location = new System.Drawing.Point(221, 73);
            this.rbUnbindingCharm.Name = "rbUnbindingCharm";
            this.rbUnbindingCharm.Size = new System.Drawing.Size(106, 17);
            this.rbUnbindingCharm.TabIndex = 7;
            this.rbUnbindingCharm.TabStop = true;
            this.rbUnbindingCharm.Text = "Unbinding Charm";
            this.rbUnbindingCharm.UseVisualStyleBackColor = true;
            // 
            // WatchList
            //
            if (windowLeft > 0 || windowTop > 0)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new System.Drawing.Point(windowLeft, windowTop);
            }
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(347, 550);
            this.Controls.Add(this.pWatchListControls);
            this.Controls.Add(this._lvWatchList);
            this.Name = "WatchList";
            this.ShowIcon = false;
            this.Text = "Edit Watch List";
            this.pWatchListControls.ResumeLayout(false);
            this.pWatchListControls.PerformLayout();
            this.FormClosing += FormEditWatchList_FormClosing;
            this.ResumeLayout(false);

        }

        void bResetWatchDefaults_Click(object sender, EventArgs e)
        {
            PopulateListView(PerkAlert.DefaultWatchListRogue, PerkAlert.DefaultWatchListMage);
            if (PerkAlert.WatchListMage != PerkAlert.DefaultWatchListMage || PerkAlert.DefaultWatchListRogue != PerkAlert.WatchListRogue)
            {
                this.bApplyWatchChanges.BackColor = Color.LightGreen; // possibly just use event for color change?
            }
        }

        void bApplyWatchChanges_Click(object sender, EventArgs e)
        {
            ExportListView();
            PerkAlert.SaveWatchLists();
            this.bApplyWatchChanges.BackColor = DefaultBackColor;
        }


        void bRemoveWatch_Click(object sender, EventArgs e)
        {
            if (_lvWatchList.SelectedIndices.Count > 0)
            {
                RemoveListItem(_lvWatchList.SelectedIndices[0]);
            }
            else if (String.IsNullOrEmpty(tbNewWatch.Text))
            {
                RemoveListItem(tbNewWatch.Text);
            }
            else
            {
                MessageBox.Show("Please select an item or enter a name to remove.", "Remove what?");
            }
        }

        void bAddWatch_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(tbNewWatch.Text))
            {
                MessageBox.Show("Please enter a name to watch.", "Name Needed");
            }
            else if (!rbFinelyHoned.Checked && !rbTaintedWeapons.Checked && !rbUnbindingCharm.Checked)
            {
                MessageBox.Show("Please select a perk to associate with this watch.", "Perk Needed");
            }
            else
            {
                string perk;

                perk = (rbFinelyHoned.Checked) ? "Finely Honed" : (rbTaintedWeapons.Checked) ? "Tainted Weapons" : "Unbinding Charm";

                AddListItem(tbNewWatch.Text, perk);
                tbNewWatch.Text = String.Empty;
            }
        }

        private void FormEditWatchList_FormClosing(object sender, FormClosingEventArgs e)
        {
            windowTop = this.Top;
            windowLeft = this.Left;
            OnFormClosing();
        }

        virtual protected void OnFormClosing()
        {

        }
    }


    /*public class FormEditWatchListMage : FormEditWatchList
    {
        bool opened { get; set; }
        public FormEditWatchListMage() 
            :base()
        {

        }

        protected override void PopulateListView()
        {
            foreach (string ability in PerkAlert.WatchListMage)
            {
                lvWatchList.Items.Add(ability);
            }
        }
      
    }*/



    public abstract class FormCritBonusReport : Form
    {
        private ListView _lvCritReport;
        private ColumnHeader _lvAbilityName;
        private ColumnHeader _lvAbilityDamageAdded;
        private static int windowTop, windowLeft;
        private int sortedColumn = -1;
        private SortOrder order;

        protected ListView lvCritReport
        {
            get { return _lvCritReport; }
        }

        protected ColumnHeader lvAbilityName
        {
            get { return _lvAbilityName; }
        }

        protected ColumnHeader lvAbilityDamageAdded
        {
            get { return _lvAbilityDamageAdded; }
        }

        protected FormCritBonusReport()
        {
            InitializeComponent();

            PopulateListView();

            this._lvCritReport.ListViewItemSorter = new ListViewItemComparer();
        }

        protected abstract void PopulateListView();

        protected virtual void lvCritReport_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (sortedColumn == e.Column)
            {
                order = (order == SortOrder.Ascending) ? (SortOrder.Descending) : (SortOrder.Ascending);
            }
            sortedColumn = e.Column;
            this._lvCritReport.ListViewItemSorter = new ListViewItemComparer(e.Column, order);
        }

        private void InitializeComponent()
        {
            this._lvCritReport = new System.Windows.Forms.ListView();
            this._lvAbilityName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._lvAbilityDamageAdded = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvCritReport
            // 
            this._lvCritReport.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._lvAbilityName,
            this._lvAbilityDamageAdded});
            this._lvCritReport.FullRowSelect = true;
            this._lvCritReport.GridLines = true;
            this._lvCritReport.Location = new System.Drawing.Point(12, 12);
            this._lvCritReport.MultiSelect = false;
            this._lvCritReport.Name = "lvCritReport";
            this._lvCritReport.Size = new System.Drawing.Size(396, 236);
            this._lvCritReport.Sorting = System.Windows.Forms.SortOrder.None;
            this._lvCritReport.TabIndex = 0;
            this._lvCritReport.UseCompatibleStateImageBehavior = false;
            this._lvCritReport.View = System.Windows.Forms.View.Details;
            this._lvCritReport.Dock = DockStyle.Fill;
            this._lvCritReport.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvCritReport_ColumnClick);
            // 
            // lvAbilityName
            // 
            this._lvAbilityName.Text = "Ability (Damage Type)";
            this._lvAbilityName.Width = 264;
            // 
            // lvAbilityDamageAdded
            // 
            this._lvAbilityDamageAdded.Text = "Est. Damage Added (Crits)";
            this._lvAbilityDamageAdded.Width = 155;
            // 
            // FormCritBonusReport
            // 
            if (windowLeft > 0 || windowTop > 0)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new System.Drawing.Point(windowLeft, windowTop);
            }
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(425 + SystemInformation.VerticalScrollBarWidth, 260);
            this.Controls.Add(this._lvCritReport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "FormCritBonusReport";
            this.ShowIcon = false;
            this.ShowInTaskbar = true;
            this.Text = "Crit Bonus Report";
            this.FormClosing += FormCritBonusReport_FormClosing;
            this.ResumeLayout(false);

        }

        protected void FormCritBonusReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            windowTop = this.Top;
            windowLeft = this.Left;
            OnFormClosing();
        }

        virtual protected void OnFormClosing()
        {

        }
    }


    public class FormCritBonusReportInc : FormCritBonusReport
    {
        public static bool opened { get; set; }
        private static int _windowTop, _windowLeft;
        private ColumnHeader lvAbilityUser;

        public FormCritBonusReportInc()
            : base()
        {
            InitializeComponent();
            opened = true;
        }

        protected override void PopulateListView()
        {
            string abilityName;
            string attacker;

            foreach (KeyValuePair<string, Dtotal> ability in AoCEI_Loader.oPluginTab.MergedSelectedEncounters.AbilityCritBonusInc)
            {
                attacker = ability.Key.Substring(0, ability.Key.IndexOf('_'));
                abilityName = ability.Key.Substring(ability.Key.IndexOf('_') + 1);

                lvCritReport.Items.Add(attacker).SubItems.AddRange(new string[] { abilityName, ability.Value.Total.ToString("#0.") + " (" + ability.Value.NumHits.ToString() + ") " });
            }
        }

        private void InitializeComponent()
        {
            this.lvAbilityUser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));

            this.lvCritReport.Columns.Remove(lvAbilityDamageAdded);
            this.lvCritReport.Columns.Remove(lvAbilityName);

            this.lvCritReport.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.lvAbilityUser,
                    this.lvAbilityName,
                    this.lvAbilityDamageAdded});
            this.SuspendLayout();
            //
            //lvAbilityUser
            //               
            this.lvAbilityUser.Text = "Attacker";
            this.lvAbilityUser.Width = 154;
            // 
            // lvAbilityName
            // 
            this.lvAbilityName.Text = "Ability (Damage Type)";
            this.lvAbilityName.Width = 160;
            // 
            // lvAbilityDamageAdded
            // 
            this.lvAbilityDamageAdded.Text = "Est. Damage (Crits)";
            this.lvAbilityDamageAdded.Width = 105;
            //
            //Form
            //
            if (_windowLeft > 0 || _windowTop > 0)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new System.Drawing.Point(_windowLeft, _windowTop);
            }
            this.ResumeLayout();

        }

        protected override void OnFormClosing()
        {
            opened = false;
            _windowTop = this.Top;
            _windowLeft = this.Left;
        }
    }

    public class FormCritBonusReportOut : FormCritBonusReport
    {
        public static bool opened { get; set; }

        public FormCritBonusReportOut()
            : base()
        {
            opened = true;
        }

        protected override void PopulateListView()
        {
            string newKey;

            foreach (KeyValuePair<string, Dtotal> ability in AoCEI_Loader.oPluginTab.MergedSelectedEncounters.AbilityCritBonusOut)
            {
                newKey = ability.Key.Substring(ability.Key.IndexOf('_') + 1);
                this.lvCritReport.Items.Add(newKey).SubItems.Add(ability.Value.Total.ToString("#0.") + " (" + ability.Value.NumHits.ToString() + ") ");
            }
        }

        protected override void OnFormClosing()
        {
            opened = false;
        }
    }



    /*///////////////////////////////////////////////////////////////////////////////////////////////////////////////
     *
     * EncounterExtraData encapsulates the data and methods for all of my caclulations.
     * 
     * Subscribes to several ACT events if isActive = true. OnCombatEnd sets isActive = false and unsubsribes from all events.
     * Use MakeInactive() to manually unsubscribe from events if necessary.
     *  
     * /////////////////////////////////////////////////////////////////////////////////////////////////////////////*/




    public class EncounterExtraData
    {
        private int zoneIndex;      // Index to the corresponding encounter in ACT's ZoneList
        private string _encounterName;
        private int _totalHealth;
        private int _totalStamina;
        private int _totalMana;
        private TimeSpan _duration;
        private static Regex taps = new Regex(@"(?<amount>[0-9]+) (?:stamina |mana )?\((?<type>health|stamina|mana) tap\)", RegexOptions.Compiled);
        private bool isActive;
        private bool _hasMe;
        private long _totalDamageOut;
        private long _magicalDamageOut;
        private long _physicalDamageOut;
        private long _otherDamageOut;
        private double _criticalDamageOut;
        private long _totalDamageInc;
        private long _magicalDamageInc;
        private long _physicalDamageInc;
        private long _otherDamageInc;
        private double _criticalDamageInc;
        private Dictionary<string, List<Dnum>> outDamage = new Dictionary<string, List<Dnum>>();
        private Dictionary<string, Davg> avgNonCritOut = new Dictionary<string, Davg>();
        private Dictionary<string, Davg> avgCritOut = new Dictionary<string, Davg>();
        private Dictionary<string, List<Dnum>> incDamage = new Dictionary<string, List<Dnum>>();
        private Dictionary<string, Davg> avgNonCritInc = new Dictionary<string, Davg>();
        private Dictionary<string, Davg> avgCritInc = new Dictionary<string, Davg>();
        private Dictionary<string, Dtotal> _abilityCritDamageInc = new Dictionary<string, Dtotal>();
        private Dictionary<string, Dtotal> _abilityCritDamageOut = new Dictionary<string, Dtotal>();
        private int indexForIncDamage;

        #region Properties


        private EncounterData actActiveEncounter
        {
            get
            {
                if (isActive)
                {
                    try
                    {
                        return ActGlobals.oFormActMain.ZoneList[zoneIndex].ActiveEncounter;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        zoneIndex = ActGlobals.oFormActMain.ZoneList.FindIndex((zone) => zone.ActiveEncounter != null);
                        return ActGlobals.oFormActMain.ZoneList[zoneIndex].ActiveEncounter;
                    }
                    catch (NullReferenceException)
                    {
                        zoneIndex = ActGlobals.oFormActMain.ZoneList.FindIndex((zone) => zone.ActiveEncounter != null);
                        return ActGlobals.oFormActMain.ZoneList[zoneIndex].ActiveEncounter;
                    }
                }
                else
                {
                    throw new InvalidOperationException("Cannot get ACT's active encounter for an inactive EncounterExtraData object");
                }
            }
        }

        public string EncounterName
        {
            get
            {
                if (isActive)
                {
                    return actActiveEncounter.ToString();
                }
                else
                {
                    return this._encounterName;
                }
            }
        }

        public int TotalHealth
        {
            get { return _totalHealth; }
        }

        public int TotalStamina
        {
            get { return _totalStamina; }
        }

        public int TotalMana
        {
            get { return _totalMana; }
        }

        public double HTapPS
        {
            get { return _totalHealth / Duration.TotalSeconds; }
        }

        public double STapPS
        {
            get { return _totalStamina / Duration.TotalSeconds; }
        }

        public double MTapPS
        {
            get { return _totalMana / Duration.TotalSeconds; }
        }

        public TimeSpan Duration
        {
            get
            {
                if (isActive)
                {
                    return actActiveEncounter.Duration;
                }
                else
                {
                    return this._duration;
                }
            }
        }


        public bool HasMe
        {
            get
            {
                if (isActive)
                {
                    return actActiveEncounter.NumAllies > 0;
                }
                else
                {
                    return this._hasMe;
                }
            }
        }

        public long TotalDamageOut
        {
            get
            {
                if (isActive)
                {
                    return _physicalDamageOut + _magicalDamageOut + _otherDamageOut;
                }
                else
                {
                    return this._totalDamageOut;
                }
            }
        }

        public double TotalDPSOut
        {
            get { return TotalDamageOut / Duration.TotalSeconds; }
        }

        public long MagicalDamageOut
        {
            get { return this._magicalDamageOut; }
        }

        public long PhysicalDamageOut
        {
            get { return this._physicalDamageOut; }
        }

        public long OtherDamageOut
        {
            get { return this._otherDamageOut; }
        }

        public double BonusCriticalDamageOut
        {
            get { return this._criticalDamageOut; }
        }

        public string PercentMagicalOut
        {
            get
            {
                if (TotalDamageOut > 0)
                {
                    return ((double)this._magicalDamageOut / TotalDamageOut).ToString("p1");
                }
                else
                {
                    return 0.ToString("p1");
                }
            }
        }

        public string PercentPhysicalOut
        {
            get
            {
                if (TotalDamageOut > 0)
                {
                    return ((double)this._physicalDamageOut / TotalDamageOut).ToString("p1");
                }
                else
                {
                    return 0.ToString("p1");
                }
            }
        }

        public string PercentBonusCritDamageOut
        {
            get
            {
                if (TotalDamageOut > 0)
                {
                    return ((double)this._criticalDamageOut / (TotalDamageOut - _criticalDamageOut)).ToString("p1");
                }
                else
                {
                    return 0.ToString("p1");
                }
            }
        }

        public string PercentMagicalInc
        {
            get
            {
                if (TotalDamageInc > 0)
                {
                    return ((double)this._magicalDamageInc / TotalDamageInc).ToString("p1");
                }
                else
                {
                    return 0.ToString("p1");
                }
            }
        }

        public string PercentPhysicalInc
        {
            get
            {
                if (TotalDamageInc > 0)
                {
                    return ((double)this._physicalDamageInc / TotalDamageInc).ToString("p1");
                }
                else
                {
                    return 0.ToString("p1");
                }
            }
        }

        public string PercentBonusCritDamageInc
        {
            get
            {
                if (TotalDamageInc > 0)
                {
                    return (this._criticalDamageInc / (TotalDamageInc - _criticalDamageInc)).ToString("p1");
                }
                else
                {
                    return 0.ToString("p1");
                }
            }
        }

        public long TotalDamageInc
        {
            get
            {
                if (isActive)
                {
                    return _physicalDamageInc + _magicalDamageInc + _otherDamageInc;
                }
                else
                {
                    return this._totalDamageInc;
                }
            }
        }

        public double TotalDPSInc
        {
            get { return TotalDamageInc / Duration.TotalSeconds; }
        }

        public long MagicalDamageInc
        {
            get { return this._magicalDamageInc; }
        }

        public long PhysicalDamageInc
        {
            get { return this._physicalDamageInc; }
        }

        public long OtherDamageInc
        {
            get { return this._otherDamageInc; }
        }

        public double BonusCriticalDamageInc
        {
            get { return _criticalDamageInc; }
        }

        public Dictionary<string, Dtotal> AbilityCritBonusOut
        {
            get { return _abilityCritDamageOut; }
        }

        public Dictionary<string, Dtotal> AbilityCritBonusInc
        {
            get { return _abilityCritDamageInc; }
        }

        public double BonusCritDPSOut
        {
            get { return BonusCriticalDamageOut / Duration.TotalSeconds; }
        }

        public double BonusCritDPSInc
        {
            get { return BonusCriticalDamageInc / Duration.TotalSeconds; }
        }




        #endregion


        public EncounterExtraData()
        {
            this.isActive = false;
        }

        public EncounterExtraData(bool _isActive)
        {
            this.isActive = _isActive;

            if (this.isActive)
            {
                ActGlobals.oFormActMain.OnLogLineRead += this.oFormActMain_OnLogLineRead;
                ActGlobals.oFormActMain.BeforeCombatAction += this.oFormActMain_BeforeCombatAction;
                ActGlobals.oFormActMain.OnCombatEnd += this.oFormActMain_OnCombatEnd;

                zoneIndex = ActGlobals.oFormActMain.ZoneList.FindIndex((ZoneData zoned) => { return zoned.ActiveEncounter != null; });
            }
        }

        void oFormActMain_OnLogLineRead(bool isImport, LogLineEventArgs logInfo)
        {
            if (logInfo.detectedType != 0)
            {
                this.ParseTaps(logInfo.logLine);
            }
        }

        private void oFormActMain_BeforeCombatAction(bool isImport, CombatActionEventArgs actionInfo)
        {
            if (!isImport)
            {
                AnalyzeDamageInc();
            }
            AnalyzeDamageOut(actionInfo);
        }

        private void oFormActMain_OnCombatEnd(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            if (encounterInfo.encounter.NumAllies > 0)
            {
                this._hasMe = true;
                this._encounterName = encounterInfo.encounter.ToString();
                this._duration = encounterInfo.encounter.Duration;
                this._totalDamageOut = encounterInfo.encounter.GetCombatant(ActGlobals.charName).Damage;
                this._totalDamageInc = encounterInfo.encounter.GetCombatant(ActGlobals.charName).DamageTaken;
            }

            if (isImport)
            {
                AnalyzeDamageInc();
            }

            ActGlobals.oFormActMain.OnCombatEnd -= this.oFormActMain_OnCombatEnd;
            ActGlobals.oFormActMain.BeforeCombatAction -= this.oFormActMain_BeforeCombatAction;
            ActGlobals.oFormActMain.OnLogLineRead -= this.oFormActMain_OnLogLineRead;
            this.isActive = false;
        }


        static private void CategorizeDamage(string type, Dnum damage, ref long physical, ref long magical, ref long other)
        {
            if (type == "Holy" | type == "Unholy" | type == "Frost" | type == "Electrical" | type == "Fire")
            {
                magical += damage;
            }
            else if (type == "Slashing" | type == "Piercing" | type == "Crushing" | type == "Poison")
            {
                physical += damage;
            }
            else
            {
                other += damage;
            }
        }



        static private void updateAvg(string _theKey, Dnum damage, ref Dictionary<string, Davg> avgData)
        {
            Davg newData = new Davg(damage, 1);

            if (avgData.ContainsKey(_theKey))
            {
                avgData[_theKey] += newData;
            }
            else
            {
                avgData[_theKey] = new Davg(damage, 1);
            }

        }


        private void ParseTaps(string _curLine)
        {
            Match match = taps.Match(_curLine);

            if (match.Success)
            {
                if ("health" == match.Groups["type"].Value)
                {
                    this._totalHealth += int.Parse(match.Groups["amount"].Value);
                }
                else if ("stamina" == match.Groups["type"].Value)
                {
                    this._totalStamina += int.Parse(match.Groups["amount"].Value);
                }
                else if ("mana" == match.Groups["type"].Value)
                {
                    this._totalMana += int.Parse(match.Groups["amount"].Value);
                }
            }
        }

        public void Merge(EncounterExtraData subEncounter)
        {
            this._totalHealth += subEncounter.TotalHealth;
            this._totalStamina += subEncounter.TotalStamina;
            this._totalMana += subEncounter.TotalMana;
            this._totalDamageOut += subEncounter.TotalDamageOut;
            this._physicalDamageOut += subEncounter._physicalDamageOut;
            this._magicalDamageOut += subEncounter._magicalDamageOut;
            this._otherDamageOut += subEncounter._otherDamageOut;
            this._criticalDamageOut += subEncounter._criticalDamageOut;
            this._totalDamageInc += subEncounter._totalDamageInc;
            this._physicalDamageInc += subEncounter._physicalDamageInc;
            this._magicalDamageInc += subEncounter._magicalDamageInc;
            this._otherDamageInc += subEncounter._otherDamageInc;
            this._criticalDamageInc += subEncounter._criticalDamageInc;
            this._duration += subEncounter.Duration;

            this.outDamage.ConcatListValues(subEncounter.outDamage);
            this.incDamage.ConcatListValues(subEncounter.incDamage);
            this.avgNonCritInc.SumValues(subEncounter.avgNonCritInc);
            this.avgNonCritOut.SumValues(subEncounter.avgNonCritOut);
            this.avgCritInc.SumValues(subEncounter.avgCritInc);
            this.avgCritOut.SumValues(subEncounter.avgCritOut);
            this.AbilityCritBonusInc.SumValues(subEncounter.AbilityCritBonusInc);
            this.AbilityCritBonusOut.SumValues(subEncounter.AbilityCritBonusOut);
        }



        public void MakeInactive()
        {
            if (isActive)
            {
                ActGlobals.oFormActMain.OnLogLineRead -= this.oFormActMain_OnLogLineRead;
                ActGlobals.oFormActMain.BeforeCombatAction -= this.oFormActMain_BeforeCombatAction;
                ActGlobals.oFormActMain.OnCombatEnd -= this.oFormActMain_OnCombatEnd;

                this.isActive = false;
            }
        }

        static private void AnalyzeCritDamage(string attacker, string abilityName, string abilityType, Dnum abilityDamage, bool crit, float minCritMult, bool useObserved, bool disableMinCrit,
            ref double _criticalDamage, ref Dictionary<string, List<Dnum>> damageDict, ref Dictionary<string, Davg> avgNonCritDict, ref Dictionary<string, Davg> avgCritDict, ref Dictionary<string, Dtotal> abilityCritBonus)
        {
            float knownMinBonus = abilityDamage / (minCritMult / (minCritMult - 1));
            string _thisKey = attacker + "_" + abilityName + " (" + abilityType + ") ";
            float observedBonus;

            if (crit)
            {
                string _critKey = "Crit_" + _thisKey;

                if (damageDict.ContainsKey(_critKey))
                {
                    damageDict[_critKey].Add(abilityDamage);
                }
                else
                {
                    List<Dnum> forDamageDict = new List<Dnum>();
                    forDamageDict.Add(abilityDamage);
                    damageDict.Add(_critKey, forDamageDict);
                }

                if (!useObserved || !avgNonCritDict.ContainsKey(_thisKey) || ((observedBonus = abilityDamage - avgNonCritDict[_thisKey]) < knownMinBonus && !disableMinCrit))
                {
                    _criticalDamage += knownMinBonus;
                    if (abilityCritBonus.ContainsKey(_thisKey))
                    {
                        abilityCritBonus[_thisKey] += knownMinBonus;
                    }
                    else
                    {
                        abilityCritBonus.Add(_thisKey, new Dtotal(knownMinBonus, 1));
                    }
                }
                else
                {
                    _criticalDamage += observedBonus;
                    if (abilityCritBonus.ContainsKey(_thisKey))
                    {
                        abilityCritBonus[_thisKey] += observedBonus;
                    }
                    else
                    {
                        abilityCritBonus.Add(_thisKey, new Dtotal(observedBonus, 1));
                    }
                }

                updateAvg(_thisKey, abilityDamage, ref avgCritDict);
            }
            else
            {
                if (damageDict.ContainsKey(_thisKey))
                {
                    damageDict[_thisKey].Add(abilityDamage);
                }
                else
                {
                    List<Dnum> forDamageDict = new List<Dnum>();
                    forDamageDict.Add(abilityDamage);
                    damageDict.Add(_thisKey, forDamageDict);
                }

                updateAvg(_thisKey, abilityDamage, ref avgNonCritDict);
            }
        }


        private void AnalyzeDamageInc()
        {
            if (actActiveEncounter.Items.ContainsKey(ActGlobals.charName.ToUpperInvariant()) &&
                    actActiveEncounter.Items[ActGlobals.charName.ToUpperInvariant()].Items["Damage (Inc)"].Items.ContainsKey("All"))
            {
                for (; indexForIncDamage < actActiveEncounter.Items[ActGlobals.charName.ToUpperInvariant()].Items["Damage (Inc)"].Items["All"].Items.Count; indexForIncDamage++)
                {
                    string abilityUser = actActiveEncounter.Items[ActGlobals.charName.ToUpperInvariant()].Items["Damage (Inc)"].Items["All"].Items[indexForIncDamage].Attacker;
                    Dnum abilityDamage = actActiveEncounter.Items[ActGlobals.charName.ToUpperInvariant()].Items["Damage (Inc)"].Items["All"].Items[indexForIncDamage].Damage;
                    string abilityName = actActiveEncounter.Items[ActGlobals.charName.ToUpperInvariant()].Items["Damage (Inc)"].Items["All"].Items[indexForIncDamage].AttackType;
                    string abilityType = actActiveEncounter.Items[ActGlobals.charName.ToUpperInvariant()].Items["Damage (Inc)"].Items["All"].Items[indexForIncDamage].DamageType;
                    bool crit = actActiveEncounter.Items[ActGlobals.charName.ToUpperInvariant()].Items["Damage (Inc)"].Items["All"].Items[indexForIncDamage].Critical;

                    CategorizeDamage(abilityType, abilityDamage, ref this._physicalDamageInc, ref this._magicalDamageInc, ref this._otherDamageInc);
                    AnalyzeCritDamage(abilityUser, abilityName, abilityType, abilityDamage, crit, 1.5F, true, true,
                        ref this._criticalDamageInc, ref this.incDamage, ref this.avgNonCritInc, ref this.avgCritInc, ref this._abilityCritDamageInc);
                }
            }
        }


        private void AnalyzeDamageOut(CombatActionEventArgs actionInfo)
        {
            if (actionInfo.attacker == ActGlobals.charName && actionInfo.swingType == 0)
            {
                CategorizeDamage(actionInfo.theDamageType, actionInfo.damage, ref this._physicalDamageOut, ref this._magicalDamageOut, ref this._otherDamageOut);
                AnalyzeCritDamage(actionInfo.attacker, actionInfo.theAttackType, actionInfo.theDamageType, actionInfo.damage, actionInfo.critical,
                    AoCEI_Loader.oOptions.DefaultCritMult, AoCEI_Loader.oOptions.EstimateCritMultOut, AoCEI_Loader.oOptions.DisableMinCritMultOut,
                    ref this._criticalDamageOut, ref this.outDamage, ref this.avgNonCritOut, ref this.avgCritOut, ref this._abilityCritDamageOut);
            }
        }
    }

    public class AlertEventArgs : EventArgs
    {
        public string Target { get; set; }
        public string Perk { get; set; }

        public AlertEventArgs(string _target, string _perk)
        {
            Target = _target;
            Perk = _perk;
        }
    }

    public static class PerkAlert
    {
        private static Regex casts = new Regex(@"^.{11}(?<target>.*?) casts (?<ability>.*)", RegexOptions.Compiled);
        private static Regex afflicts = new Regex(@"afflicts (?<target>[A-Zy][a-z]+) with (?<ability>.*)", RegexOptions.Compiled);
        private static Regex heals = new Regex(@"^.{11}Stranger's (?<ability>.*?) heals (?<target>.*?) for", RegexOptions.Compiled);
        private static Dictionary<string, string> rogueWatchDefault = new Dictionary<string, string>()
        {
            {"Burn Skin", "Finely Honed"},
            {"Spirit Shield", "Finely Honed"},
            {"Taint of the Scholars", "Finely Honed"},
            {"Healing Glow", "Tainted Weapons"},
            {"Lesser Regeneration", "Tainted Weapons"},
            {"Commune Life", "Tainted Weapons"},
            {"Replenish", "Tainted Weapons"},
            {"Malicious Heal", "Tainted Weapons"},
            {"Ghostly Healing", "Tainted Weapons"},
            {"Regeneration of Nyarlathotep", "Tainted Weapons"},
            {"Protection of Nyarlathotep", "Finely Honed"},
            {"Regenerative Elixir", "Tainted Weapons"},
            {"Healing Wave", "Tainted Weapons"},
            {"Absorbing Corruption", "Finely Honed"},
            {"Dance of the Sisters", "Finely Honed"}
        };
        private static List<string> mageWatchDefault = new List<string> { "Enslaving Torment", "Deadweight", "Ghostly Torment" };
        private static Dictionary<string, string> rogueWatch = new Dictionary<string, string>();
        private static List<string> mageWatch = new List<string>();
        private static string _curLine;
        private static string _target;
        private static string _ability;
        private static List<string> selectedPerks;
        private static Match alertMatch;
        public static event EventHandler<AlertEventArgs> Alert;
        private static int zoneIndex;
        private static string iniFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\PerkAlert.ini");
        private static Dictionary<string, string> knownAllies = new Dictionary<string, string>();


        public static List<string> WatchListMage
        {
            get { return mageWatch; }

        }

        public static List<string> DefaultWatchListMage
        {
            get { return mageWatchDefault; }
        }

        public static Dictionary<string, string> WatchListRogue
        {
            get { return rogueWatch; }
        }

        public static Dictionary<string, string> DefaultWatchListRogue
        {
            get { return rogueWatchDefault; }
        }

        static PerkAlert()
        {
            InitializeWatchLists();
        }

        private static void InitializeWatchLists()
        {
            if (File.Exists(iniFile))
            {
                FileStream fs = new FileStream(iniFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader sReader = new StreamReader(fs);
                string perkName = String.Empty;
                string abilityName;
                string curLine;

                try
                {
                    while (sReader.Peek() > -1)
                    {
                        curLine = sReader.ReadLine();
                        if (curLine.StartsWith("["))
                        {
                            perkName = curLine;
                        }
                        else if (!String.IsNullOrWhiteSpace(curLine))
                        {
                            abilityName = curLine;

                            if (perkName == "[Unbinding Charm]")
                            {
                                mageWatch.Add(abilityName);
                            }
                            else if ((perkName == "[Finely Honed]" || perkName == "[Tainted Weapons]"))
                            {
                                //Using .Split to get perkName out of square brackets
                                rogueWatch[abilityName] = perkName.Split(new char[] { '[', ']' })[1];
                            }
                        }
                    }

                }
                catch (IOException)
                {
                    MessageBox.Show("Error loading " + iniFile + ". Loading default Watch List instead.", "Error");
                    rogueWatch = rogueWatchDefault;
                    mageWatch = mageWatchDefault;
                }

                sReader.Close();
            }
            else
            {
                rogueWatch = rogueWatchDefault;
                mageWatch = mageWatchDefault;
            }
        }

        internal static void SaveWatchLists()
        {
            if (!File.Exists(iniFile))
            {
                File.CreateText(iniFile);
            }

            FileStream fs = new FileStream(iniFile, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sWriter = new StreamWriter(fs);

            sWriter.WriteLine("[Finely Honed]");

            foreach (KeyValuePair<string, string> watch in rogueWatch)
            {
                if (watch.Value == "Finely Honed")
                {
                    sWriter.WriteLine(watch.Key);
                }
            }

            sWriter.WriteLine("[Tainted Weapons]");

            foreach (KeyValuePair<string, string> watch in rogueWatch)
            {
                if (watch.Value == "Tainted Weapons")
                {
                    sWriter.WriteLine(watch.Key);
                }
            }

            sWriter.WriteLine("[Unbinding Charm]");

            foreach (string ability in mageWatch)
            {
                sWriter.WriteLine(ability);
            }

            sWriter.Flush();
            sWriter.Close();
        }

        internal static void oFormActMain_OnLogLineRead(bool isImport, LogLineEventArgs logInfo)
        {
            if (!isImport)
            {
                _curLine = logInfo.logLine;
                selectedPerks = AoCEI_Loader.oPluginTab.PerksSelected; // Prolly can be more efficient with this

                //Checking for Ruin, Wrack, Torment, Unbinding Charm affects
                if (_curLine.IndexOf("afflicts", 11, StringComparison.Ordinal) >= 0)
                {
                    alertMatch = afflicts.Match(_curLine);
                    _target = alertMatch.Groups["target"].Value;
                    _ability = alertMatch.Groups["ability"].Value;

                    if (alertMatch.Success && IsAlly(_target))
                    {
                        if (selectedPerks.Contains("Mage") && ((AoCEI_Loader.oOptions.UCAlertThreshold == 2 && _ability.Contains("Torment")) || mageWatch.Contains(_ability)))
                        {
                            OnAlert(new AlertEventArgs(_target, "Unbinding Charm"));
                        }

                        if (selectedPerks.Contains("Bear Shaman") || selectedPerks.Contains("Priest of Mitra") || selectedPerks.Contains("Tempest of Set"))
                        {
                            if (_ability.Contains("Ruin"))
                            {
                                OnAlert(new AlertEventArgs(_target, "Steadfast Faith"));
                            }
                            else if (selectedPerks.Contains("Bear Shaman") && _ability.Contains("Physical"))
                            {
                                OnAlert(new AlertEventArgs(_target, "Steadfast Faith"));
                            }
                            else if (selectedPerks.Contains("Priest of Mitra") && _ability.Contains("Spiritual"))
                            {
                                OnAlert(new AlertEventArgs(_target, "Steadfast Faith"));
                            }
                            else if (selectedPerks.Contains("Tempest of Set") && _ability.Contains("Elemental"))
                            {
                                OnAlert(new AlertEventArgs(_target, "Steadfast Faith"));
                            }
                            else if (AoCEI_Loader.oOptions.UCAlertThreshold == 1 && selectedPerks.Contains("Mage") && _ability.Contains("Torment"))
                            {
                                OnAlert(new AlertEventArgs(_target, "Unbinding Charm"));
                            }
                        }

                        if (((selectedPerks.Contains("Soldier (You)") && _target == "you") || 
                            (selectedPerks.Contains("Soldier (Others)") && knownAllies.ContainsKey(_target.ToUpperInvariant()) && knownAllies[_target.ToUpperInvariant()] == "Soldier")) && 
                            _ability.Contains("Wrack"))
                        {
                            OnAlert(new AlertEventArgs(_target, "Resolve"));
                        }
                    }
                }//Checking for Finely Honed and Tainted Weapon affects
                else if (selectedPerks.Contains("Rogue"))
                {
                    if (_curLine.IndexOf("casts", 11, StringComparison.Ordinal) >= 0)
                    {
                        alertMatch = casts.Match(_curLine);

                        if (alertMatch.Success)
                        {
                            _target = alertMatch.Groups["target"].Value;
                            _ability = alertMatch.Groups["ability"].Value;

                            if (rogueWatch.ContainsKey(_ability) && !IsAlly(_target))
                            {
                                OnAlert(new AlertEventArgs(_target, rogueWatch[_ability]));
                            }
                        }
                    }
                    //Double checking for "Stranger's" heals for Tainted Weapons
                    else
                    {
                        alertMatch = heals.Match(_curLine);

                        if (alertMatch.Success)
                        {
                            _target = alertMatch.Groups["target"].Value;
                            _ability = alertMatch.Groups["ability"].Value;

                            if (rogueWatch.ContainsKey(_ability) && !IsAlly(_target))
                            {
                                OnAlert(new AlertEventArgs(_target, "Tainted Weapons"));
                            }
                        }
                    }
                }
            }
        }

        private static void OnAlert(AlertEventArgs alertEventArgs)
        {
            if (Alert != null)
            {
                Alert(null, alertEventArgs);
            }
        }


        private static bool IsAlly(string target)
        {
            try
            {
                if (ActGlobals.oFormActMain.ZoneList[zoneIndex].ActiveEncounter == null || ActGlobals.oFormActMain.ZoneList[zoneIndex].ActiveEncounter.NumAllies < 1)
                {
                    return false;
                }
                else
                {
                    return target == "you" || knownAllies.ContainsKey(target.ToUpperInvariant()) ||
                        (ActGlobals.oFormActMain.ZoneList[zoneIndex].ActiveEncounter.GetCombatant(ActGlobals.charName).Allies.ContainsKey(target.ToUpperInvariant())
                         && ActGlobals.oFormActMain.ZoneList[zoneIndex].ActiveEncounter.GetCombatant(ActGlobals.charName).Allies[target.ToUpperInvariant()] > 0);
                }
            }
            catch (IndexOutOfRangeException)
            {
                zoneIndex = ActGlobals.oFormActMain.ZoneList.FindIndex((ZoneData zoned) => { return zoned.ActiveEncounter != null; });

                if (ActGlobals.oFormActMain.ZoneList[zoneIndex].ActiveEncounter == null || ActGlobals.oFormActMain.ZoneList[zoneIndex].ActiveEncounter.NumAllies < 1)
                {
                    return false;
                }
                else
                {
                    return target == "you" || knownAllies.ContainsKey(target.ToUpperInvariant()) ||
                        (ActGlobals.oFormActMain.ZoneList[zoneIndex].ActiveEncounter.GetCombatant(ActGlobals.charName).Allies.ContainsKey(target.ToUpperInvariant())
                         && ActGlobals.oFormActMain.ZoneList[zoneIndex].ActiveEncounter.GetCombatant(ActGlobals.charName).Allies[target.ToUpperInvariant()] > 0);
                }
            }
        }

        internal static void oFormActMain_OnCombatStart(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            zoneIndex = ActGlobals.oFormActMain.ZoneList.FindIndex((ZoneData zoned) => { return zoned.ActiveEncounter != null; });
        }

        internal static void oFormActMain_OnCombatEnd(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            if (encounterInfo.encounter.NumAllies > 1)
            {
                foreach (string ally in encounterInfo.encounter.GetCombatant(ActGlobals.charName).Allies.Keys)
                {
                    if (!knownAllies.ContainsKey(ally) && encounterInfo.encounter.GetCombatant(ActGlobals.charName).Allies[ally] > 2
                        && ally != ActGlobals.charName.ToUpperInvariant() && !ally.Contains(' ') && ally != "STRANGER")
                    {
                        string _playerClass = ClassDetector.ClassString(encounterInfo, ally);

                        switch(_playerClass)
                        {
                            case "Assassin":
                            case "Barbarian":
                            case "Ranger":
                                _playerClass = "Rogue";
                                break;
                            case "Conqueror":
                            case "Dark Templar":
                            case "Guardian":
                                _playerClass = "Soldier";
                                break;
                            case "Demonologist":
                            case "Herald of Xotli":
                            case "Necromancer":
                                _playerClass = "Mage";
                                break;
                        }

                        knownAllies.Add(ally, _playerClass);
                    }
                }
            }
        }
    }


    public class ActiveAlert : IEquatable<ActiveAlert>, IDisposable
    {
        private string _target;
        private string _perk;
        private System.Timers.Timer LifeTimer;
        public event EventHandler OnInactive;
        bool Disposed = false;
        public string Target { get { return _target; } }
        public string Perk { get { return _perk; } }

        public ActiveAlert(AlertEventArgs e)
        {
            _target = e.Target;
            _perk = e.Perk;
            ActGlobals.oFormActMain.OnLogLineRead += oFormActMain_OnLogLineRead;
            LifeTimer = new System.Timers.Timer(AoCEI_Loader.oOptions.MaxTextAlertDuration.TotalMilliseconds);
            LifeTimer.Elapsed += LifeTimer_Elapsed;
            LifeTimer.Start();
        }


        void oFormActMain_OnLogLineRead(bool isImport, LogLineEventArgs logInfo)
        {
            if (logInfo.logLine.Contains(_perk))
            {
                if (_perk == "Steadfast Faith")
                    MakeInactive();
                else if (logInfo.logLine.Contains(_target))
                    MakeInactive();
                else if (_target == "you" && logInfo.logLine.Contains("You"))
                    MakeInactive();
            }
        }

        private void MakeInactive()
        {
            ActGlobals.oFormActMain.OnLogLineRead -= oFormActMain_OnLogLineRead;
            LifeTimer.Elapsed -= LifeTimer_Elapsed;
            LifeTimer.Close();
            GoingInactive();
        }

        private void LifeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            MakeInactive();
        }

        private void GoingInactive()
        {
            if (OnInactive != null)
            {
                OnInactive(this, EventArgs.Empty);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is ActiveAlert)
            {
                return Equals((ActiveAlert)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(ActiveAlert right)
        {
            return this.Target == right.Target && this.Perk == right.Perk;
        }

        public static bool operator ==(ActiveAlert left, ActiveAlert right)
        {
            return left.Target == right.Target && left.Perk == right.Perk;
        }

        public static bool operator !=(ActiveAlert left, ActiveAlert right)
        {
            return left.Target != right.Target && left.Perk != right.Perk;
        }

        public override string ToString()
        {
            return this.Perk + " - " + this.Target.ToUpperInvariant();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Target.GetHashCode();
                hash = hash * 23 + Perk.GetHashCode();
                return hash;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Disposed) return;

                if (LifeTimer != null)
                {
                    LifeTimer.Elapsed -= LifeTimer_Elapsed;
                    LifeTimer.Close();

                }

                Disposed = true;

            }
        }


    }


    public static class ClassDetector
    {
        private static string className = "None";
        public static event EventHandler ClassDetected;

        public static string ClassName
        {
            get { return className; }
        }

        public static string DetectedClassOrDefault
        {
            get
            {
                if (className != "None")
                {
                    return className;
                }
                else
                {
                    return AoCEI_Loader.oOptions.DefaultClass;
                }

            }
        }

        public static void OnClassDetected(EventArgs e)
        {
            if (ClassDetected != null)
            {
                ClassDetected(null, e);
            }
        }




        private static void GetClass(CombatActionEventArgs actionInfo)
        {
            if (actionInfo.attacker == ActGlobals.charName)
            {
                switch (actionInfo.theAttackType)
                {
                    case "Dread Venom":
                    case "Miasma: Face Stab":
                    case "Miasma: Lotus Coated Dart":
                    case "Miasma: Soul Strike":
                    case "Swift Strikes X":
                    case "Swift Strikes IX":
                    case "Swift Strikes VIII":
                    case "Swift Strikes VII":
                    case "Swift Strikes VI":
                    case "Swift Strikes V":
                    case "Swift Strikes IV":
                    case "Swift Strikes III":
                    case "Swift Strikes II":
                    case "Swift Strikes I":
                        className = "Assassin";
                        break;
                    case "Dance of Death":
                    case "Upheaval":
                    case "Butcher X":
                    case "Butcher IX":
                    case "Butcher VIII":
                    case "Butcher VII":
                    case "Butcher VI":
                    case "Butcher V":
                    case "Butcher IV":
                    case "Butcher III":
                    case "Butcher II":
                    case "Butcher I":
                    case "Cyclone of Steel X":
                    case "Cyclone of Steel IX":
                    case "Cyclone of Steel VIII":
                    case "Cyclone of Steel VII":
                    case "Cyclone of Steel VI":
                    case "Cyclone of Steel V":
                    case "Cyclone of Steel IV":
                    case "Cyclone of Steel III":
                    case "Cyclone of Steel II":
                    case "Cyclone of Steel I":
                        className = "Barbarian";
                        break;
                    case "Shrewd Blow VI":
                    case "Shrewd Blow V":
                    case "Shrewd Blow IV":
                    case "Shrewd Blow III":
                    case "Shrewd Blow II":
                    case "Shrewd Blow I":
                    case "Internal Bleed IV":
                    case "Internal Bleed III":
                    case "Internal Bleed II":
                    case "Internal Bleed I":
                    case "Renewal":
                        className = "Bear Shaman";
                        break;
                    case "Furious Inpiration (1)":
                    case "Furious Inpiration (2)":
                    case "Furious Inpiration (3)":
                    case "Furious Inpiration (4)":
                    case "Furious Inpiration (5)":
                    case "Furious Inspiration (10)":
                    case "Guard of Dancing Steel IV":
                    case "Guard of Dancing Steel III":
                    case "Guard of Dancing Steel II":
                    case "Guard of Dancing Steel I":
                    case "Bloodbath VII":
                    case "Bloodbath VI":
                    case "Bloodbath V":
                    case "Bloodbath IV":
                    case "Bloodbath III":
                    case "Bloodbath II":
                    case "Bloodbath I":
                        className = "Conqueror";
                        break;
                    case "Malacodor's Blight":
                    case "Blood for Aid V":
                    case "Blood for Aid IV":
                    case "Blood for Aid III":
                    case "Blood for Aid II":
                    case "Blood for Aid I":
                    case "Leech Life VI":
                    case "Leech Life V":
                    case "Leech Life IV":
                    case "Leech Life III":
                    case "Leech Life II":
                    case "Leech Life I":
                        className = "Dark Templar";
                        break;
                    case "Shock (Rank 5)":
                    case "Shock (Rank 4)":
                    case "Shock (Rank 3)":
                    case "Shock (Rank 2)":
                    case "Shock (Rank 1)":
                    case "Waves of Flames (Rank 4)":
                    case "Waves of Flames (Rank 3)":
                    case "Waves of Flames (Rank 2)":
                    case "Waves of Flames (Rank 1)":
                    case "Hellfire Stream (Rank 6)":
                    case "Hellfire Stream (Rank 5)":
                    case "Hellfire Stream (Rank 4)":
                    case "Hellfire Stream (Rank 3)":
                    case "Hellfire Stream (Rank 2)":
                    case "Hellfire Stream (Rank 1)":
                    case "Protection of Set (Rank 5)":
                    case "Protection of Set (Rank 4)":
                    case "Protection of Set (Rank 3)":
                    case "Protection of Set (Rank 2)":
                    case "Protection of Set (Rank 1)":
                        className = "Demonologist";
                        break;
                    case "Bloody Vengeance":
                    case "Disable III":
                    case "Disable II":
                    case "Disable I":
                    case "Flashing Arc IX":
                    case "Flashing Arc VIII":
                    case "Flashing Arc VII":
                    case "Flashing Arc VI":
                    case "Flashing Arc V":
                    case "Flashing Arc IV":
                    case "Flashing Arc III":
                    case "Flashing Arc II":
                    case "Flashing Arc I":
                    case "Plexus Strike III":
                    case "Plexus Strike II":
                    case "Plexus Strike I":
                    case "Counterweight":
                    case "Storm Strike":
                        className = "Guardian";
                        break;
                    case "Inferno":
                    case "Fire Lance":
                    case "Hellfire Breath (Rank 5)":
                    case "Hellfire Breath (Rank 4)":
                    case "Hellfire Breath (Rank 3)":
                    case "Hellfire Breath (Rank 2)":
                    case "Hellfire Breath (Rank 1)":
                    case "Contract of Protection (Rank 5)":
                    case "Contract of Protection (Rank 4)":
                    case "Contract of Protection (Rank 3)":
                    case "Contract of Protection (Rank 2)":
                    case "Contract of Protection (Rank 1)":
                        className = "Herald of Xotli";
                        break;
                    case "Chill (Rank 5)":
                    case "Chill (Rank 4)":
                    case "Chill (Rank 3)":
                    case "Chill (Rank 2)":
                    case "Chill (Rank 1)":
                    case "Flesh to Worms (Rank 5)":
                    case "Flesh to Worms (Rank 4)":
                    case "Flesh to Worms (Rank 3)":
                    case "Flesh to Worms (Rank 2)":
                    case "Flesh to Worms (Rank 1)":
                    case "Pestilential Blast (Rank 6)":
                    case "Pestilential Blast (Rank 5)":
                    case "Pestilential Blast (Rank 4)":
                    case "Pestilential Blast (Rank 3)":
                    case "Pestilential Blast (Rank 2)":
                    case "Pestilential Blast (Rank 1)":
                    case "Runed Flesh (Rank 5)":
                    case "Runed Flesh (Rank 4)":
                    case "Runed Flesh (Rank 3)":
                    case "Runed Flesh (Rank 2)":
                    case "Runed Flesh (Rank 1)":
                        className = "Necromancer";
                        break;
                    case "Cleansing Fire":
                    case "Lance of Mitra":
                    case "Emanation of Life (Rank 6)":
                    case "Emanation of Life (Rank 5)":
                    case "Emanation of Life (Rank 4)":
                    case "Emanation of Life (Rank 3)":
                    case "Emanation of Life (Rank 2)":
                    case "Emanation of Life (Rank 1)":
                    case "Smite (Rank 6)":
                    case "Smite (Rank 5)":
                    case "Smite (Rank 4)":
                    case "Smite (Rank 3)":
                    case "Smite (Rank 2)":
                    case "Smite (Rank 1)":
                        className = "Priest of Mitra";
                        break;
                    case "Advantage: Fire Attack":
                    case "Advantage: Shattering Attack":
                    case "Advantage: Focused Fire":
                    case "Advantage: Absolute Precision":
                    case "Advantage: Recuperation":
                    case "Advantage: Misdirection":
                    case "Armor Ripper III":
                    case "Armor Ripper II":
                    case "Armor Ripper I":
                    case "Penetrating Shot V":
                    case "Penetrating Shot IV":
                    case "Penetrating Shot III":
                    case "Penetrating Shot II":
                    case "Penetrating Shot I":
                        className = "Ranger";
                        break;
                    case "Storm Field":
                    case "Lightning Sparks":
                    case "Coils of the Serpent":
                    case "Healing Lotus (Rank 6)":
                    case "Healing Lotus (Rank 5)":
                    case "Healing Lotus (Rank 4)":
                    case "Healing Lotus (Rank 3)":
                    case "Healing Lotus (Rank 2)":
                    case "Healing Lotus (Rank 1)":
                        className = "Tempest of Set";
                        break;
                }

                if (className != "None")
                {
                    OnClassDetected(EventArgs.Empty);
                }
            }

        }

        internal static void oFormActMain_LogFileChanged(bool IsImport, string NewLogFileName)
        {
            className = "None";
            OnClassDetected(EventArgs.Empty);
            ActGlobals.oFormActMain.AfterCombatAction += oFormActMain_AfterCombatAction;
        }

        private static void oFormActMain_AfterCombatAction(bool isImport, CombatActionEventArgs actionInfo)
        {
            if (className == "None")
            {
                GetClass(actionInfo);
            }
            else
            {
                ActGlobals.oFormActMain.AfterCombatAction -= oFormActMain_AfterCombatAction;
            }
        }

        internal static string ClassString(CombatToggleEventArgs encounterInfo, string playerName)
        {
            CombatantData player = encounterInfo.encounter.GetCombatant(playerName);
            string playerClass = null;

            foreach (KeyValuePair<string, AttackType> ability in player.AllOut)
            {
                switch (ability.Key)
                {
                    case "Dread Venom":
                    case "Miasma: Face Stab":
                    case "Miasma: Lotus Coated Dart":
                    case "Miasma: Soul Strike":
                    case "Swift Strikes X":
                    case "Swift Strikes IX":
                    case "Swift Strikes VIII":
                    case "Swift Strikes VII":
                    case "Swift Strikes VI":
                    case "Swift Strikes V":
                    case "Swift Strikes IV":
                    case "Swift Strikes III":
                    case "Swift Strikes II":
                    case "Swift Strikes I":
                        playerClass = "Assassin";
                        break;
                    case "Dance of Death":
                    case "Upheaval":
                    case "Butcher X":
                    case "Butcher IX":
                    case "Butcher VIII":
                    case "Butcher VII":
                    case "Butcher VI":
                    case "Butcher V":
                    case "Butcher IV":
                    case "Butcher III":
                    case "Butcher II":
                    case "Butcher I":
                    case "Cyclone of Steel X":
                    case "Cyclone of Steel IX":
                    case "Cyclone of Steel VIII":
                    case "Cyclone of Steel VII":
                    case "Cyclone of Steel VI":
                    case "Cyclone of Steel V":
                    case "Cyclone of Steel IV":
                    case "Cyclone of Steel III":
                    case "Cyclone of Steel II":
                    case "Cyclone of Steel I":
                        playerClass = "Barbarian";
                        break;
                    case "Shrewd Blow VI":
                    case "Shrewd Blow V":
                    case "Shrewd Blow IV":
                    case "Shrewd Blow III":
                    case "Shrewd Blow II":
                    case "Shrewd Blow I":
                    case "Internal Bleed IV":
                    case "Internal Bleed III":
                    case "Internal Bleed II":
                    case "Internal Bleed I":
                    case "Renewal":
                        playerClass = "Bear Shaman";
                        break;
                    case "Furious Inpiration (1)":
                    case "Furious Inpiration (2)":
                    case "Furious Inpiration (3)":
                    case "Furious Inpiration (4)":
                    case "Furious Inpiration (5)":
                    case "Furious Inspiration (10)":
                    case "Guard of Dancing Steel IV":
                    case "Guard of Dancing Steel III":
                    case "Guard of Dancing Steel II":
                    case "Guard of Dancing Steel I":
                    case "Bloodbath VII":
                    case "Bloodbath VI":
                    case "Bloodbath V":
                    case "Bloodbath IV":
                    case "Bloodbath III":
                    case "Bloodbath II":
                    case "Bloodbath I":
                        playerClass = "Conqueror";
                        break;
                    case "Malacodor's Blight":
                    case "Blood for Aid V":
                    case "Blood for Aid IV":
                    case "Blood for Aid III":
                    case "Blood for Aid II":
                    case "Blood for Aid I":
                    case "Leech Life VI":
                    case "Leech Life V":
                    case "Leech Life IV":
                    case "Leech Life III":
                    case "Leech Life II":
                    case "Leech Life I":
                        playerClass = "Dark Templar";
                        break;
                    case "Shock (Rank 5)":
                    case "Shock (Rank 4)":
                    case "Shock (Rank 3)":
                    case "Shock (Rank 2)":
                    case "Shock (Rank 1)":
                    case "Waves of Flames (Rank 4)":
                    case "Waves of Flames (Rank 3)":
                    case "Waves of Flames (Rank 2)":
                    case "Waves of Flames (Rank 1)":
                    case "Hellfire Stream (Rank 6)":
                    case "Hellfire Stream (Rank 5)":
                    case "Hellfire Stream (Rank 4)":
                    case "Hellfire Stream (Rank 3)":
                    case "Hellfire Stream (Rank 2)":
                    case "Hellfire Stream (Rank 1)":
                    case "Protection of Set (Rank 5)":
                    case "Protection of Set (Rank 4)":
                    case "Protection of Set (Rank 3)":
                    case "Protection of Set (Rank 2)":
                    case "Protection of Set (Rank 1)":
                        playerClass = "Demonologist";
                        break;
                    case "Bloody Vengeance":
                    case "Disable III":
                    case "Disable II":
                    case "Disable I":
                    case "Flashing Arc IX":
                    case "Flashing Arc VIII":
                    case "Flashing Arc VII":
                    case "Flashing Arc VI":
                    case "Flashing Arc V":
                    case "Flashing Arc IV":
                    case "Flashing Arc III":
                    case "Flashing Arc II":
                    case "Flashing Arc I":
                    case "Plexus Strike III":
                    case "Plexus Strike II":
                    case "Plexus Strike I":
                    case "Counterweight":
                    case "Storm Strike":
                        playerClass = "Guardian";
                        break;
                    case "Inferno":
                    case "Fire Lance":
                    case "Hellfire Breath (Rank 5)":
                    case "Hellfire Breath (Rank 4)":
                    case "Hellfire Breath (Rank 3)":
                    case "Hellfire Breath (Rank 2)":
                    case "Hellfire Breath (Rank 1)":
                    case "Contract of Protection (Rank 5)":
                    case "Contract of Protection (Rank 4)":
                    case "Contract of Protection (Rank 3)":
                    case "Contract of Protection (Rank 2)":
                    case "Contract of Protection (Rank 1)":
                        playerClass = "Herald of Xotli";
                        break;
                    case "Chill (Rank 5)":
                    case "Chill (Rank 4)":
                    case "Chill (Rank 3)":
                    case "Chill (Rank 2)":
                    case "Chill (Rank 1)":
                    case "Flesh to Worms (Rank 5)":
                    case "Flesh to Worms (Rank 4)":
                    case "Flesh to Worms (Rank 3)":
                    case "Flesh to Worms (Rank 2)":
                    case "Flesh to Worms (Rank 1)":
                    case "Pestilential Blast (Rank 6)":
                    case "Pestilential Blast (Rank 5)":
                    case "Pestilential Blast (Rank 4)":
                    case "Pestilential Blast (Rank 3)":
                    case "Pestilential Blast (Rank 2)":
                    case "Pestilential Blast (Rank 1)":
                    case "Runed Flesh (Rank 5)":
                    case "Runed Flesh (Rank 4)":
                    case "Runed Flesh (Rank 3)":
                    case "Runed Flesh (Rank 2)":
                    case "Runed Flesh (Rank 1)":
                        playerClass = "Necromancer";
                        break;
                    case "Cleansing Fire":
                    case "Lance of Mitra":
                    case "Emanation of Life (Rank 6)":
                    case "Emanation of Life (Rank 5)":
                    case "Emanation of Life (Rank 4)":
                    case "Emanation of Life (Rank 3)":
                    case "Emanation of Life (Rank 2)":
                    case "Emanation of Life (Rank 1)":
                    case "Smite (Rank 6)":
                    case "Smite (Rank 5)":
                    case "Smite (Rank 4)":
                    case "Smite (Rank 3)":
                    case "Smite (Rank 2)":
                    case "Smite (Rank 1)":
                        playerClass = "Priest of Mitra";
                        break;
                    case "Advantage: Fire Attack":
                    case "Advantage: Shattering Attack":
                    case "Advantage: Focused Fire":
                    case "Advantage: Absolute Precision":
                    case "Advantage: Recuperation":
                    case "Advantage: Misdirection":
                    case "Armor Ripper III":
                    case "Armor Ripper II":
                    case "Armor Ripper I":
                    case "Penetrating Shot V":
                    case "Penetrating Shot IV":
                    case "Penetrating Shot III":
                    case "Penetrating Shot II":
                    case "Penetrating Shot I":
                        playerClass = "Ranger";
                        break;
                    case "Storm Field":
                    case "Lightning Sparks":
                    case "Coils of the Serpent":
                    case "Healing Lotus (Rank 6)":
                    case "Healing Lotus (Rank 5)":
                    case "Healing Lotus (Rank 4)":
                    case "Healing Lotus (Rank 3)":
                    case "Healing Lotus (Rank 2)":
                    case "Healing Lotus (Rank 1)":
                        playerClass = "Tempest of Set";
                        break;
                }

                if (!String.IsNullOrEmpty(playerClass))
                    break;
            }

            return playerClass;
        }

    }





    public class Options_AoC_Extra_Info : UserControl
    {
        private NumericUpDown nudDashboardUpdate;
        private Label lDashboardUpdate;
        private Label lDiscardEncounters;
        private NumericUpDown nudDiscardEncounters;
        private Label lDefaultClass;
        private ComboBox combDefaultClass;
        private Label lDetectedClass;
        private TextBox tbDetectedClass;
        private NumericUpDown nudCritMult;
        private Label lDefaultCritMult;
        private CheckBox cbCritEst;
        private CheckBox cbCritMin;
        private Panel pCritOptions;
        private Panel pClassDetector;
        private GroupBox gbPerkAlert;
        private NumericUpDown nudAlertDuration;
        private Label lAlertDuration;
        private Panel pUnbindingCharmAlerts;
        private ComboBox combUnbindingCharmAlerts;
        private Label lUnbindingCharmAlerts;
        SettingsSerializer xmlSettings;
        string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\AoC_Extra_Info.options.xml");



        #region Properties
        public TimeSpan MinDashboardUpdateInterval
        {
            get { return new TimeSpan(0, 0, 0, 0, (int)nudDashboardUpdate.Value * 1000); }
            set { nudDashboardUpdate.Value = (decimal)value.TotalMilliseconds / 1000; }
        }

        public TimeSpan MinEncounterDuration
        {
            get { return new TimeSpan(0, 0, (int)nudDiscardEncounters.Value); }
            set { nudDiscardEncounters.Value = (decimal)value.TotalSeconds; }
        }

        public string DefaultClass
        {
            get { return combDefaultClass.SelectedItem.ToString(); }
            set { combDefaultClass.SelectedItem = value; }
        }

        public string DetectedClass
        {
            get { return tbDetectedClass.Text; }
            set { tbDetectedClass.Text = value; }
        }

        public float DefaultCritMult
        {
            get { return (float)nudCritMult.Value; }
            set { nudCritMult.Value = (decimal)value; }
        }

        public bool EstimateCritMultOut
        {
            get { return cbCritEst.Checked; }
            set { cbCritEst.Checked = value; }
        }

        public bool DisableMinCritMultOut
        {
            get { return cbCritMin.Checked; }
            set { cbCritMin.Checked = value; }
        }

        public TimeSpan MaxTextAlertDuration
        {
            get { return new TimeSpan(0, 0, (int)nudAlertDuration.Value); }
            set { nudAlertDuration.Value = (decimal)value.TotalSeconds; }
        }

        public int UCAlertThreshold
        {
            get { return combUnbindingCharmAlerts.SelectedIndex; }
            set { combUnbindingCharmAlerts.SelectedIndex = value; } //IndexOutOfRange possible
        }

        #endregion


        public Options_AoC_Extra_Info()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.nudDashboardUpdate = new System.Windows.Forms.NumericUpDown();
            this.lDashboardUpdate = new System.Windows.Forms.Label();
            this.lDiscardEncounters = new System.Windows.Forms.Label();
            this.nudDiscardEncounters = new System.Windows.Forms.NumericUpDown();
            this.lDefaultClass = new System.Windows.Forms.Label();
            this.combDefaultClass = new System.Windows.Forms.ComboBox();
            this.lDetectedClass = new System.Windows.Forms.Label();
            this.tbDetectedClass = new System.Windows.Forms.TextBox();
            this.nudCritMult = new System.Windows.Forms.NumericUpDown();
            this.lDefaultCritMult = new System.Windows.Forms.Label();
            this.cbCritEst = new System.Windows.Forms.CheckBox();
            this.cbCritMin = new System.Windows.Forms.CheckBox();
            this.pCritOptions = new System.Windows.Forms.Panel();
            this.pClassDetector = new System.Windows.Forms.Panel();
            this.gbPerkAlert = new System.Windows.Forms.GroupBox();
            this.nudAlertDuration = new System.Windows.Forms.NumericUpDown();
            this.lAlertDuration = new System.Windows.Forms.Label();
            this.pUnbindingCharmAlerts = new System.Windows.Forms.Panel();
            this.lUnbindingCharmAlerts = new System.Windows.Forms.Label();
            this.combUnbindingCharmAlerts = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudDashboardUpdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiscardEncounters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCritMult)).BeginInit();
            this.pCritOptions.SuspendLayout();
            this.pClassDetector.SuspendLayout();
            this.gbPerkAlert.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlertDuration)).BeginInit();
            this.pUnbindingCharmAlerts.SuspendLayout();
            this.SuspendLayout();
            // 
            // nudDashboardUpdate
            // 
            this.nudDashboardUpdate.DecimalPlaces = 1;
            this.nudDashboardUpdate.Location = new System.Drawing.Point(250, 3);
            this.nudDashboardUpdate.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudDashboardUpdate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudDashboardUpdate.Name = "nudDashboardUpdate";
            this.nudDashboardUpdate.Size = new System.Drawing.Size(38, 20);
            this.nudDashboardUpdate.TabIndex = 0;
            // 
            // lDashboardUpdate
            // 
            this.lDashboardUpdate.AutoSize = true;
            this.lDashboardUpdate.Location = new System.Drawing.Point(3, 5);
            this.lDashboardUpdate.Name = "lDashboardUpdate";
            this.lDashboardUpdate.Size = new System.Drawing.Size(231, 13);
            this.lDashboardUpdate.TabIndex = 1;
            this.lDashboardUpdate.Text = "Minimum Dashboard Update Interval (seconds) ";
            this.lDashboardUpdate.MouseHover += new System.EventHandler(this.lDashboardUpdate_MouseHover);
            // 
            // lDiscardEncounters
            // 
            this.lDiscardEncounters.AutoSize = true;
            this.lDiscardEncounters.Location = new System.Drawing.Point(4, 31);
            this.lDiscardEncounters.Name = "lDiscardEncounters";
            this.lDiscardEncounters.Size = new System.Drawing.Size(214, 13);
            this.lDiscardEncounters.TabIndex = 5;
            this.lDiscardEncounters.Text = "Discard encounters shorter than X seconds ";
            this.lDashboardUpdate.MouseHover += new System.EventHandler(this.lDiscardEncounters_MouseHover);
            // 
            // nudDiscardEncounters
            // 
            this.nudDiscardEncounters.DecimalPlaces = 1;
            this.nudDiscardEncounters.Location = new System.Drawing.Point(224, 28);
            this.nudDiscardEncounters.Name = "nudDiscardEncounters";
            this.nudDiscardEncounters.Size = new System.Drawing.Size(38, 20);
            this.nudDiscardEncounters.TabIndex = 6;
            this.nudDiscardEncounters.Value = 6;
            // 
            // lDefaultClass
            // 
            this.lDefaultClass.AutoSize = true;
            this.lDefaultClass.Location = new System.Drawing.Point(3, 9);
            this.lDefaultClass.Name = "lDefaultClass";
            this.lDefaultClass.Size = new System.Drawing.Size(75, 13);
            this.lDefaultClass.TabIndex = 7;
            this.lDefaultClass.Text = "Default Class: ";
            this.lDefaultClass.MouseHover += new System.EventHandler(this.lDefaultClass_MouseHover);
            // 
            // combDefaultClass
            //
            this.combDefaultClass.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.combDefaultClass.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.combDefaultClass.FormattingEnabled = true;
            this.combDefaultClass.Items.AddRange(new object[] {
            "Assassin",
            "Barbarian",
            "Bear Shaman",
            "Conqueror",
            "Dark Templar",
            "Demonologist",
            "Guardian",
            "Herald of Xotli",
            "Necromancer",
            "Priest of Mitra",
            "Ranger",
            "Tempest of Set",
             });
            this.combDefaultClass.Location = new System.Drawing.Point(84, 5);
            this.combDefaultClass.Name = "cbDefaultClass";
            this.combDefaultClass.Size = new System.Drawing.Size(121, 21);
            this.combDefaultClass.TabIndex = 8;
            this.combDefaultClass.MouseHover += new System.EventHandler(this.combDefaultClass_MouseHover);
            // 
            // lDetectedClass
            // 
            this.lDetectedClass.AutoSize = true;
            this.lDetectedClass.Location = new System.Drawing.Point(211, 9);
            this.lDetectedClass.Name = "lDetectedClass";
            this.lDetectedClass.Size = new System.Drawing.Size(85, 13);
            this.lDetectedClass.TabIndex = 9;
            this.lDetectedClass.Text = "Detected Class: ";
            this.lDetectedClass.MouseHover += new System.EventHandler(this.lDetectedClass_MouseHover);
            // 
            // tbDetectedClass
            // 
            this.tbDetectedClass.Location = new System.Drawing.Point(302, 6);
            this.tbDetectedClass.Name = "tbDetectedClass";
            this.tbDetectedClass.ReadOnly = true;
            this.tbDetectedClass.Size = new System.Drawing.Size(100, 20);
            this.tbDetectedClass.TabIndex = 10;
            this.tbDetectedClass.Text = "None";
            this.tbDetectedClass.MouseHover += new System.EventHandler(this.tbDetectedClass_MouseHover);
            ClassDetector.ClassDetected += ClassDetector_ClassDetected;
            // 
            // nudCritMult
            // 
            this.nudCritMult.DecimalPlaces = 2;
            this.nudCritMult.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudCritMult.Location = new System.Drawing.Point(140, 3);
            this.nudCritMult.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCritMult.Name = "nudCritMult";
            this.nudCritMult.Size = new System.Drawing.Size(43, 20);
            this.nudCritMult.TabIndex = 3;
            this.nudCritMult.Value = new decimal(new int[] {
            150,
            0,
            0,
            131072});
            // 
            // lDefaultCritMult
            // 
            this.lDefaultCritMult.AutoSize = true;
            this.lDefaultCritMult.Location = new System.Drawing.Point(3, 7);
            this.lDefaultCritMult.Name = "lDefaultCritMult";
            this.lDefaultCritMult.Size = new System.Drawing.Size(122, 13);
            this.lDefaultCritMult.TabIndex = 2;
            this.lDefaultCritMult.Text = "Default Critical Multiplier ";
            this.lDefaultCritMult.MouseHover += new System.EventHandler(this.lDefaultCritMult_MouseHover);
            // 
            // cbCritEst
            // 
            this.cbCritEst.AutoSize = true;
            this.cbCritEst.Location = new System.Drawing.Point(6, 29);
            this.cbCritEst.Name = "cbCritEst";
            this.cbCritEst.Size = new System.Drawing.Size(250, 17);
            this.cbCritEst.TabIndex = 4;
            this.cbCritEst.Text = "Enable Outgoing Critical Bonus Damage Estimation";
            this.cbCritEst.UseVisualStyleBackColor = true;
            this.cbCritEst.MouseHover += new System.EventHandler(this.cbCritEst_MouseHover);
            this.cbCritEst.CheckedChanged += new System.EventHandler(this.cbCritEst_CheckedChanged);
            // 
            // cbCritMin
            // 
            this.cbCritMin.AutoSize = true;
            this.cbCritMin.Enabled = false;
            this.cbCritMin.Location = new System.Drawing.Point(19, 53);
            this.cbCritMin.Name = "cbNoMinCrit";
            this.cbCritMin.Size = new System.Drawing.Size(228, 17);
            this.cbCritMin.TabIndex = 5;
            this.cbCritMin.Text = "Disable Minimum Critical Multiplier for Estimations";
            this.cbCritMin.UseVisualStyleBackColor = true;
            this.cbCritMin.MouseHover += new System.EventHandler(this.cbCritMin_MouseHover);
            // 
            // pCritOptions
            // 
            this.pCritOptions.Controls.Add(this.cbCritMin);
            this.pCritOptions.Controls.Add(this.cbCritEst);
            this.pCritOptions.Controls.Add(this.lDefaultCritMult);
            this.pCritOptions.Controls.Add(this.nudCritMult);
            this.pCritOptions.AutoSize = true;
            this.pCritOptions.Location = new System.Drawing.Point(7, 85);
            this.pCritOptions.Name = "pCritOptions";
            this.pCritOptions.Size = new System.Drawing.Size(271, 100);
            this.pCritOptions.TabIndex = 4;
            // 
            // pClassDetector
            // 
            this.pClassDetector.Controls.Add(this.lDefaultClass);
            this.pClassDetector.Controls.Add(this.tbDetectedClass);
            this.pClassDetector.Controls.Add(this.combDefaultClass);
            this.pClassDetector.Controls.Add(this.lDetectedClass);
            this.pClassDetector.Location = new System.Drawing.Point(7, 50);
            this.pClassDetector.Name = "pClassDetector";
            this.pClassDetector.Size = new System.Drawing.Size(432, 32);
            this.pClassDetector.TabIndex = 11;
            // 
            // gbPerkAlert
            // 
            this.gbPerkAlert.Controls.Add(this.pUnbindingCharmAlerts);
            this.gbPerkAlert.Controls.Add(this.nudAlertDuration);
            this.gbPerkAlert.Controls.Add(this.lAlertDuration);
            this.gbPerkAlert.Location = new System.Drawing.Point(7, 170);
            this.gbPerkAlert.Name = "gbPerkAlert";
            this.gbPerkAlert.Size = new System.Drawing.Size(432, 73);
            this.gbPerkAlert.TabIndex = 12;
            this.gbPerkAlert.TabStop = false;
            this.gbPerkAlert.Text = "PerkAlert™";
            // 
            // nudAlertDuration
            // 
            this.nudAlertDuration.DecimalPlaces = 1;
            this.nudAlertDuration.Location = new System.Drawing.Point(214, 14);
            this.nudAlertDuration.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudAlertDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudAlertDuration.Name = "nudAlertDuration";
            this.nudAlertDuration.Size = new System.Drawing.Size(38, 20);
            this.nudAlertDuration.TabIndex = 1;
            this.nudAlertDuration.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // lAlertDuration
            // 
            this.lAlertDuration.AutoSize = true;
            this.lAlertDuration.Location = new System.Drawing.Point(6, 16);
            this.lAlertDuration.Name = "lAlertDuration";
            this.lAlertDuration.Size = new System.Drawing.Size(191, 13);
            this.lAlertDuration.TabIndex = 0;
            this.lAlertDuration.Text = "Maximum Text Alert Duration (seconds)";
            this.lAlertDuration.MouseHover += lAlertDuration_MouseHover;
            // 
            // pUnbindingCharmAlerts
            //
            this.pUnbindingCharmAlerts.Controls.Add(this.combUnbindingCharmAlerts);
            this.pUnbindingCharmAlerts.Controls.Add(this.lUnbindingCharmAlerts);
            this.pUnbindingCharmAlerts.Location = new System.Drawing.Point(0, 40);
            this.pUnbindingCharmAlerts.Name = "pUnbindingCharmAlerts";
            this.pUnbindingCharmAlerts.Size = new System.Drawing.Size(432, 26);
            this.pUnbindingCharmAlerts.TabIndex = 2;
            // 
            // lUnbindingCharmAlerts
            // 
            this.lUnbindingCharmAlerts.AutoSize = true;
            this.lUnbindingCharmAlerts.Location = new System.Drawing.Point(7, 4);
            this.lUnbindingCharmAlerts.Name = "lUnbindingCharmAlerts";
            this.lUnbindingCharmAlerts.Size = new System.Drawing.Size(123, 13);
            this.lUnbindingCharmAlerts.TabIndex = 0;
            this.lUnbindingCharmAlerts.Text = "Unbinding Charm Alerts: ";
            this.lUnbindingCharmAlerts.MouseHover += lUnbindingCharmAlerts_MouseHover;
            // 
            // combUnbindingCharmAlerts
            //
            this.combUnbindingCharmAlerts.DropDownStyle = ComboBoxStyle.DropDownList;
            this.combUnbindingCharmAlerts.FormattingEnabled = true;
            this.combUnbindingCharmAlerts.Items.AddRange(new object[] {
            "Watch List Only",
            "Watch List + Special Torments",
            "Watch List + All Torments"});
            this.combUnbindingCharmAlerts.Location = new System.Drawing.Point(136, 1);
            this.combUnbindingCharmAlerts.MaxDropDownItems = 3;
            this.combUnbindingCharmAlerts.Name = "combUnbindingCharmAlerts";
            this.combUnbindingCharmAlerts.Size = new System.Drawing.Size(167, 21);
            this.combUnbindingCharmAlerts.TabIndex = 1;
            this.combUnbindingCharmAlerts.SelectedIndex = 0;
            this.combUnbindingCharmAlerts.MouseHover += combUnbindingCharmAlerts_MouseHover;
            // 
            // Options_AoC_Extra_Info
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbPerkAlert);
            this.Controls.Add(this.pClassDetector);
            this.Controls.Add(this.nudDiscardEncounters);
            this.Controls.Add(this.lDiscardEncounters);
            this.Controls.Add(this.pCritOptions);
            this.Controls.Add(this.lDashboardUpdate);
            this.Controls.Add(this.nudDashboardUpdate);
            this.Name = "Options_AoC_Extra_Info";
            this.Size = new System.Drawing.Size(492, 389);
            ((System.ComponentModel.ISupportInitialize)(this.nudDashboardUpdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiscardEncounters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCritMult)).EndInit();
            this.pCritOptions.ResumeLayout(false);
            this.pCritOptions.PerformLayout();
            this.pClassDetector.ResumeLayout(false);
            this.pClassDetector.PerformLayout();
            this.gbPerkAlert.ResumeLayout(false);
            this.gbPerkAlert.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlertDuration)).EndInit();
            this.pUnbindingCharmAlerts.ResumeLayout(false);
            this.pUnbindingCharmAlerts.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void LoadSettings()
        {
            xmlSettings = new SettingsSerializer(this);

            // Add items to the xmlSettings object here...
            xmlSettings.AddControlSetting(this.nudDashboardUpdate.Name, this.nudDashboardUpdate);
            xmlSettings.AddControlSetting(this.nudDiscardEncounters.Name, this.nudDiscardEncounters);
            xmlSettings.AddControlSetting(this.combDefaultClass.Name, this.combDefaultClass);
            xmlSettings.AddControlSetting(this.nudCritMult.Name, this.nudCritMult);
            xmlSettings.AddControlSetting(this.cbCritEst.Name, this.cbCritEst);
            xmlSettings.AddControlSetting(this.cbCritMin.Name, this.cbCritMin);
            xmlSettings.AddControlSetting(this.nudAlertDuration.Name, this.nudAlertDuration);
            xmlSettings.AddIntSetting("UCAlertThreshold");
            
            
            if (File.Exists(settingsFile))
            {
                FileStream fs = new FileStream(settingsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlTextReader xReader = new XmlTextReader(fs);

                try
                {
                    while (xReader.Read())
                    {
                        if (xReader.NodeType == XmlNodeType.Element && xReader.LocalName == "SettingsSerializer")
                        {
                            xmlSettings.ImportFromXml(xReader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AoCEI_Loader.PluginStatus = "Error loading settings: " + ex.Message;
                    throw;
                }
                xReader.Close();
            }
        }

        public void SaveSettings()
        {
            FileStream fs = new FileStream(this.settingsFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            XmlTextWriter xmlTextWriter = new XmlTextWriter(fs, Encoding.UTF8);
            xmlTextWriter.Formatting = Formatting.Indented;
            xmlTextWriter.Indentation = 1;
            xmlTextWriter.IndentChar = '\t';
            xmlTextWriter.WriteStartDocument(true);
            xmlTextWriter.WriteStartElement("Config");
            xmlTextWriter.WriteStartElement("SettingsSerializer");
            this.xmlSettings.ExportToXml(xmlTextWriter);
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndDocument();
            xmlTextWriter.Flush();
            xmlTextWriter.Close();
        }

        private void lDashboardUpdate_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("Sets the frequency of dashboard updates in the AoC Extra Info plugin tab. The default value of 0 will update the dashboard whenever there is new data to display. A value less than 0 will disable automatic dashboard updates.");
        }

        private void lDiscardEncounters_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("Encounters shorter than this duration will not show in the encounter list in the AoC Extra Info plugin tab.");
        }


        private void cbCritMin_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("With this setting enabled, some crit bonus damage estimations will be extremely low (even negative). However, this setting is the only way that the sum of non-crit damage and critical bonus damage will equal total recorded damage. With this setting disabled, all outgoing crits are assumed to get at least the default critical multiplier.");

        }

        private void cbCritEst_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("Attempts to estimate the damage you gain from critical hits based on the difference between crits and normal hits. This is the method that is always used for incoming criticals. Unless your critical multiplier fluctates during combat, there is no reason to enable this setting.");
        }

        private void cbCritEst_CheckedChanged(object sender, EventArgs e)
        {
            cbCritMin.Enabled = (cbCritMin.Enabled == true) ? false : true;
            cbCritMin.Checked = false;
        }


        private void lDefaultCritMult_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("This number is used to calculate the bonus damage you gain from critical hits. Generally, it's 1.5 + Critical Damage Percent / 100.");
        }

        private void lDefaultClass_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("If the class dectector does not know which class you are, it assumes you're this class.");
        }

        private void combDefaultClass_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("If the class dectector does not know which class you are playing, it assumes you're playing this class.");
        }

        private void lDetectedClass_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("Class dectection is based on whether or not you have used certain abilities during this session. The list of abilities used for class dectection is not comprehensive. For example, it won't know you're a healer if you don't heal.");
        }

        private void tbDetectedClass_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("Class dectection is based on whether or not you have used certain abilities during this session. The list of abilities used for class dectection is not comprehensive. For example, it won't know you're a healer if you don't heal.");
        }

        void lAlertDuration_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("This controls how long alerts persist in the text and MiniAlert™ windows before resetting. If PerkAlert™ detects that an alert has been resolved, it will reset the alert early.");
        }

        void lUnbindingCharmAlerts_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("Watch List Only - Only abilities listed explicitly in the Watch List will trigger alerts. \r\n\r\nWatch List + Special Torments - In addition to abilities listed in the Watch List, any Torment effects besides the standard Elemental/Physical/Spiritual varieties will trigger an alert.\r\n\r\nWatch List + All Torments - In addition to abilities listed in the Watch List, any Torment effects will trigger an alert.");
        }

        void combUnbindingCharmAlerts_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("Watch List Only - Only abilities listed explicitly in the Watch List will trigger alerts. \r\n\r\nWatch List + Special Torments - In addition to abilities listed in the Watch List, any Torment effects besides the standard Elemental/Physical/Spiritual varieties will trigger an alert.\r\n\r\nWatch List + All Torments - In addition to abilities listed in the Watch List, any Torment effects will trigger an alert.");
        }

        private void ClassDetector_ClassDetected(object sender, EventArgs e)
        {
            this.UIThread(() => tbDetectedClass.Text = ClassDetector.ClassName);
        }


    }


    /// <summary>
    /// Represents a damage average stored as totalDamage / numHits.
    /// </summary>
    public struct Davg
    {

        private readonly int numHits;
        private readonly float totalDamage;

        public float Average
        {
            get { return totalDamage / numHits; }
        }

        public int NumHits
        {
            get { return numHits; }
        }

        public float TotalDamage
        {
            get { return totalDamage; }
        }

        public Davg(float dam, int hits)
        {
            totalDamage = dam;
            numHits = hits;
        }

        public static implicit operator float(Davg d)
        {
            return d.Average;
        }

        public static implicit operator double(Davg d)
        {
            return d.Average;
        }

        public static Davg ToDavg(Dtotal d)
        {
            return new Davg(d.Total, d.NumHits);
        }


        public static Davg operator +(Davg left, Davg right)
        {
            return new Davg(left.totalDamage + right.totalDamage, left.numHits + right.numHits);
        }

        public static Davg operator +(Davg left, float right)
        {
            return new Davg(left.totalDamage + right, left.numHits + 1);
        }

        public static Davg operator +(Davg left, int right)
        {
            return new Davg(left.totalDamage + right, left.numHits + 1);
        }

        public static Davg operator +(Davg left, long right)
        {
            return new Davg(left.totalDamage + right, left.numHits + 1);
        }

        public static bool operator ==(Davg left, Davg right)
        {
            return left.Average == right.Average;
        }

        public static bool operator !=(Davg left, Davg right)
        {
            return left.Average != right.Average;
        }

        public bool Equals(Davg right)
        {
            return this.Average == right.Average && this.numHits == right.numHits && this.totalDamage == right.totalDamage;
        }

        public override bool Equals(object right)
        {
            if (right == null || !(right is Davg))
            {
                return false;
            }
            return Equals((Davg)right);

        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + numHits.GetHashCode();
                hash = hash * 23 + totalDamage.GetHashCode();
                return hash;
            }
        }
    }

    /// <summary>
    /// Represents a damage total stored as average * numHits.
    /// </summary>
    public struct Dtotal
    {
        private readonly int numHits;
        private readonly float average;

        public float Total
        {
            get { return average * numHits; }
        }

        public int NumHits
        {
            get { return numHits; }
        }

        public float Average
        {
            get { return average; }
        }

        public Dtotal(float avg, int hits)
        {
            average = avg;
            numHits = hits;
        }

        public static implicit operator float(Dtotal d)
        {
            return d.Total;
        }

        public static implicit operator double(Dtotal d)
        {
            return d.Total;
        }

        public static implicit operator Dtotal(Davg d)
        {
            return new Dtotal(d.Average, d.NumHits);
        }

        public static Dtotal operator +(Dtotal left, Dtotal right)
        {
            return new Dtotal((left.Total + right.Total) / (left.numHits + right.numHits), left.numHits + right.numHits);
        }

        public static Dtotal operator +(Dtotal left, float right)
        {
            return new Dtotal((left.Total + right) / (left.numHits + 1), left.numHits + 1);
        }

        public static bool operator ==(Dtotal left, Dtotal right)
        {
            return left.Total == right.Total;
        }

        public static bool operator !=(Dtotal left, Dtotal right)
        {
            return left.Total != right.Total;
        }

        public bool Equals(Dtotal right)
        {
            return this.Total == right.Total && this.numHits == right.numHits && this.average == right.average;
        }

        public override bool Equals(object right)
        {
            if (right == null || !(right is Dtotal))
            {
                return false;
            }
            return Equals((Dtotal)right);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + numHits.GetHashCode();
                hash = hash * 23 + average.GetHashCode();
                return hash;
            }
        }

    }
}


namespace MyExtensions
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// Sums the values in the source dictionaries into the destination.
        /// </summary>
        /// <typeparam name="T">Dictionary with keys of type K and values of type Davg</typeparam>
        /// <typeparam name="K">The type of the keys in the dictionaries.</typeparam>
        /// <param name="Me">Destination. The sum goes here.</param>
        /// <param name="others">Source. The Dictionaries to add to the Destination.</param>
        public static void SumValues<T, K>(this Dictionary<K, Davg> Me, params T[] others)
        where T : Dictionary<K, Davg>
        {
            foreach (Dictionary<K, Davg> dictionary in others)
            {
                foreach (KeyValuePair<K, Davg> data in dictionary)
                {
                    if (Me.ContainsKey(data.Key))
                    {
                        Me[data.Key] += data.Value;
                    }
                    else
                    {
                        Me.Add(data.Key, data.Value);
                    }
                }
            }
        }



        /// <summary>
        /// For each KeyValuePair, Concatenates source List values into destination List values.
        /// </summary>
        /// <typeparam name="T">The type of dictionary with List values.</typeparam>
        /// <typeparam name="K">The type of keys in the Dictionary.</typeparam>
        /// <typeparam name="V">The type of items in the List.</typeparam>
        /// <param name="Me">The destination dictionary.</param>
        /// <param name="others">The source dictionaries.</param>
        public static void ConcatListValues<T, K, V>(this T Me, params Dictionary<K, List<V>>[] others)
            where T : Dictionary<K, List<V>>, IEnumerable
        {
            foreach (Dictionary<K, List<V>> dictionary in others)
            {
                foreach (KeyValuePair<K, List<V>> data in dictionary)
                {
                    if (Me.ContainsKey(data.Key))
                    {
                        Me[data.Key].AddRange(data.Value);
                    }
                    else
                    {
                        Me.Add(data.Key, data.Value);
                    }

                }
            }
        }


        /// <summary>
        /// Sums the values in the source dictionaries into the destination.
        /// </summary>
        /// <typeparam name="T">Dictionary with keys of type K and values of type Dtotal</typeparam>
        /// <typeparam name="K">The type of the keys in the dictionaries.</typeparam>
        /// <param name="Me">Destination. The sum goes here.</param>
        /// <param name="others">Source. The Dictionaries to add to the Destination.</param>
        public static void SumValues<T, K>(this T Me, params Dictionary<K, Dtotal>[] others)
        where T : Dictionary<K, Dtotal>
        {
            foreach (Dictionary<K, Dtotal> dictionary in others)
            {
                foreach (KeyValuePair<K, Dtotal> data in dictionary)
                {
                    if (Me.ContainsKey(data.Key))
                    {
                        Me[data.Key] += data.Value;
                    }
                    else
                    {
                        Me.Add(data.Key, data.Value);
                    }
                }
            }
        }
    }

    static class ControlExtensions
    {
        static public void UIThread(this Control control, Action code)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(code);
                return;
            }
            code();
        }

        static public void UIThreadInvoke(this Control control, Action code)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(code);
                return;
            }
            code();
        }

    }
}