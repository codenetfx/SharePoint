// -----------------------------------------------------------------------
// <copyright file="Form1.Designer.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace TestEngagementsService_WinForms
{
    /// <summary>
    /// Form 1 partial class
    /// </summary>
    public partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// The status strip1
        /// </summary>
        private System.Windows.Forms.StatusStrip statusStrip1;

        /// <summary>
        /// The tool strip status label1
        /// </summary>
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;

        /// <summary>
        /// The tab engagements
        /// </summary>
        private System.Windows.Forms.TabControl tabEngagements;

        /// <summary>
        /// The tab engagement
        /// </summary>
        private System.Windows.Forms.TabPage tabEngagement;

        /// <summary>
        /// The tab processes
        /// </summary>
        private System.Windows.Forms.TabPage tabProcesses;

        /// <summary>
        /// The label2
        /// </summary>
        private System.Windows.Forms.Label label2;

        /// <summary>
        /// The label1
        /// </summary>
        private System.Windows.Forms.Label label1;
        
        /// <summary>
        /// The label7
        /// </summary>
        private System.Windows.Forms.Label label7;
        
        /// <summary>
        /// The label6
        /// </summary>
        private System.Windows.Forms.Label label6;
        
        /// <summary>
        /// The label5
        /// </summary>
        private System.Windows.Forms.Label label5;
        
        /// <summary>
        /// The label4
        /// </summary>
        private System.Windows.Forms.Label label4;
        
        /// <summary>
        /// The label3
        /// </summary>
        private System.Windows.Forms.Label label3;
        
        /// <summary>
        /// The label11
        /// </summary>
        private System.Windows.Forms.Label label11;
        
        /// <summary>
        /// The label10
        /// </summary>
        private System.Windows.Forms.Label label10;
        
        /// <summary>
        /// The label9
        /// </summary>
        private System.Windows.Forms.Label label9;
        
        /// <summary>
        /// The label8
        /// </summary>
        private System.Windows.Forms.Label label8;
        
        /// <summary>
        /// The DateTime Picker for status datum of the offer
        /// </summary>
        private System.Windows.Forms.DateTimePicker datAuftragStatusDatum;
        
        /// <summary>
        /// The TXT staff
        /// </summary>
        private System.Windows.Forms.TextBox txtStaff;
        
        /// <summary>
        /// Textbox for offer status
        /// </summary>
        private System.Windows.Forms.TextBox txtAuftragStatus;
        
        /// <summary>
        /// Textbox for txtBezeichnung
        /// </summary>
        private System.Windows.Forms.TextBox txtBezeichnung;
        
        /// <summary>
        /// The TXT niederlasdsung
        /// </summary>
        private System.Windows.Forms.TextBox txtNiederlasdsung;
        
        /// <summary>
        /// The TXT concurring partner
        /// </summary>
        private System.Windows.Forms.TextBox txtConcurringPartner;
        
        /// <summary>
        /// The TXT company
        /// </summary>
        private System.Windows.Forms.TextBox txtCompany;
        
        /// <summary>
        /// The TXT opportunity number
        /// </summary>
        private System.Windows.Forms.TextBox txtOpportunityNumber;
        
        /// <summary>
        /// The TXT engagement manager
        /// </summary>
        private System.Windows.Forms.TextBox txtEngagementManager;
        
        /// <summary>
        /// The TXT engagement partner
        /// </summary>
        private System.Windows.Forms.TextBox txtEngagementPartner;
        
        /// <summary>
        /// The TXT wb number
        /// </summary>
        private System.Windows.Forms.TextBox txtWbNumber;
        
        /// <summary>
        /// The context menu strip1
        /// </summary>
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        
        /// <summary>
        /// The BTN load
        /// </summary>
        private System.Windows.Forms.Button btnLoad;
        
        /// <summary>
        /// The BTN create engagement
        /// </summary>
        private System.Windows.Forms.Button btnCreateEngagement;
        
        /// <summary>
        /// The TXT output
        /// </summary>
        private System.Windows.Forms.TextBox txtOutput;
        
        /// <summary>
        /// The BTN get status
        /// </summary>
        private System.Windows.Forms.Button btnGetStatus;
        
        /// <summary>
        /// The TXT engagement status
        /// </summary>
        private System.Windows.Forms.TextBox txtEngagementStatus;
        
        /// <summary>
        /// The BTN reopen
        /// </summary>
        private System.Windows.Forms.Button btnReopen;
        
        /// <summary>
        /// The BTN close
        /// </summary>
        private System.Windows.Forms.Button btnClose;
        
        /// <summary>
        /// The TXT process site
        /// </summary>
        private System.Windows.Forms.TextBox txtProcessSite;

        /// <summary>
        /// The label12
        /// </summary>
        private System.Windows.Forms.Label label12;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabEngagements = new System.Windows.Forms.TabControl();
            this.tabEngagement = new System.Windows.Forms.TabPage();
            this.btnUpdateOpportunity = new System.Windows.Forms.Button();
            this.btnUpdateEngagement = new System.Windows.Forms.Button();
            this.btnCreateOpportunity = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnCreateEngagement = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.datAuftragStatusDatum = new System.Windows.Forms.DateTimePicker();
            this.txtStaff = new System.Windows.Forms.TextBox();
            this.txtAuftragStatus = new System.Windows.Forms.TextBox();
            this.txtBezeichnung = new System.Windows.Forms.TextBox();
            this.txtNiederlasdsung = new System.Windows.Forms.TextBox();
            this.txtConcurringPartner = new System.Windows.Forms.TextBox();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.txtOpportunityNumber = new System.Windows.Forms.TextBox();
            this.txtEngagementManager = new System.Windows.Forms.TextBox();
            this.txtEngagementPartner = new System.Windows.Forms.TextBox();
            this.txtWbNumber = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabProcesses = new System.Windows.Forms.TabPage();
            this.btnConvertToEng = new System.Windows.Forms.Button();
            this.btnGetStatus = new System.Windows.Forms.Button();
            this.txtEngagementStatus = new System.Windows.Forms.TextBox();
            this.btnReopen = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtProcessSite = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.epEngagementPartner = new System.Windows.Forms.ErrorProvider(this.components);
            this.epEngagementManager = new System.Windows.Forms.ErrorProvider(this.components);
            this.epConcurringPartner = new System.Windows.Forms.ErrorProvider(this.components);
            this.epStaff = new System.Windows.Forms.ErrorProvider(this.components);
            this.epWbNumber = new System.Windows.Forms.ErrorProvider(this.components);
            this.statusStrip1.SuspendLayout();
            this.tabEngagements.SuspendLayout();
            this.tabEngagement.SuspendLayout();
            this.tabProcesses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.epEngagementPartner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epEngagementManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epConcurringPartner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epStaff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epWbNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(551, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(62, 17);
            this.toolStripStatusLabel1.Text = "statusLabel";
            // 
            // tabEngagements
            // 
            this.tabEngagements.Controls.Add(this.tabEngagement);
            this.tabEngagements.Controls.Add(this.tabProcesses);
            this.tabEngagements.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabEngagements.Location = new System.Drawing.Point(0, 0);
            this.tabEngagements.Name = "tabEngagements";
            this.tabEngagements.SelectedIndex = 0;
            this.tabEngagements.Size = new System.Drawing.Size(551, 540);
            this.tabEngagements.TabIndex = 1;
            // 
            // tabEngagement
            // 
            this.tabEngagement.Controls.Add(this.btnUpdateOpportunity);
            this.tabEngagement.Controls.Add(this.btnUpdateEngagement);
            this.tabEngagement.Controls.Add(this.btnCreateOpportunity);
            this.tabEngagement.Controls.Add(this.txtOutput);
            this.tabEngagement.Controls.Add(this.btnCreateEngagement);
            this.tabEngagement.Controls.Add(this.btnLoad);
            this.tabEngagement.Controls.Add(this.datAuftragStatusDatum);
            this.tabEngagement.Controls.Add(this.txtStaff);
            this.tabEngagement.Controls.Add(this.txtAuftragStatus);
            this.tabEngagement.Controls.Add(this.txtBezeichnung);
            this.tabEngagement.Controls.Add(this.txtNiederlasdsung);
            this.tabEngagement.Controls.Add(this.txtConcurringPartner);
            this.tabEngagement.Controls.Add(this.txtCompany);
            this.tabEngagement.Controls.Add(this.txtOpportunityNumber);
            this.tabEngagement.Controls.Add(this.txtEngagementManager);
            this.tabEngagement.Controls.Add(this.txtEngagementPartner);
            this.tabEngagement.Controls.Add(this.txtWbNumber);
            this.tabEngagement.Controls.Add(this.label11);
            this.tabEngagement.Controls.Add(this.label10);
            this.tabEngagement.Controls.Add(this.label9);
            this.tabEngagement.Controls.Add(this.label8);
            this.tabEngagement.Controls.Add(this.label7);
            this.tabEngagement.Controls.Add(this.label6);
            this.tabEngagement.Controls.Add(this.label5);
            this.tabEngagement.Controls.Add(this.label4);
            this.tabEngagement.Controls.Add(this.label3);
            this.tabEngagement.Controls.Add(this.label2);
            this.tabEngagement.Controls.Add(this.label1);
            this.tabEngagement.Location = new System.Drawing.Point(4, 22);
            this.tabEngagement.Name = "tabEngagement";
            this.tabEngagement.Padding = new System.Windows.Forms.Padding(3);
            this.tabEngagement.Size = new System.Drawing.Size(543, 514);
            this.tabEngagement.TabIndex = 0;
            this.tabEngagement.Text = "Create Sites";
            this.tabEngagement.UseVisualStyleBackColor = true;
            // 
            // btnUpdateOpportunity
            // 
            this.btnUpdateOpportunity.Location = new System.Drawing.Point(11, 473);
            this.btnUpdateOpportunity.Name = "btnUpdateOpportunity";
            this.btnUpdateOpportunity.Size = new System.Drawing.Size(166, 23);
            this.btnUpdateOpportunity.TabIndex = 30;
            this.btnUpdateOpportunity.Text = "Update Opportunity";
            this.btnUpdateOpportunity.UseVisualStyleBackColor = true;
            this.btnUpdateOpportunity.Click += new System.EventHandler(this.btnUpdateOpportunity_Click);
            // 
            // btnUpdateEngagement
            // 
            this.btnUpdateEngagement.Location = new System.Drawing.Point(352, 473);
            this.btnUpdateEngagement.Name = "btnUpdateEngagement";
            this.btnUpdateEngagement.Size = new System.Drawing.Size(166, 23);
            this.btnUpdateEngagement.TabIndex = 29;
            this.btnUpdateEngagement.Text = "Update Engagement";
            this.btnUpdateEngagement.UseVisualStyleBackColor = true;
            this.btnUpdateEngagement.Click += new System.EventHandler(this.btnUpdateEngagement_Click);
            // 
            // btnCreateOpportunity
            // 
            this.btnCreateOpportunity.Location = new System.Drawing.Point(11, 444);
            this.btnCreateOpportunity.Name = "btnCreateOpportunity";
            this.btnCreateOpportunity.Size = new System.Drawing.Size(166, 23);
            this.btnCreateOpportunity.TabIndex = 28;
            this.btnCreateOpportunity.Text = "Create Opportunity";
            this.btnCreateOpportunity.UseVisualStyleBackColor = true;
            this.btnCreateOpportunity.Click += new System.EventHandler(this.btnCreateOpportunity_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtOutput.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtOutput.Location = new System.Drawing.Point(11, 397);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(507, 41);
            this.txtOutput.TabIndex = 27;
            this.txtOutput.WordWrap = false;
            // 
            // btnCreateEngagement
            // 
            this.btnCreateEngagement.Location = new System.Drawing.Point(352, 444);
            this.btnCreateEngagement.Name = "btnCreateEngagement";
            this.btnCreateEngagement.Size = new System.Drawing.Size(166, 23);
            this.btnCreateEngagement.TabIndex = 26;
            this.btnCreateEngagement.Text = "Create Engagement";
            this.btnCreateEngagement.UseVisualStyleBackColor = true;
            this.btnCreateEngagement.Click += new System.EventHandler(this.btnCreateEngagement_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(453, 9);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 25;
            this.btnLoad.Text = "Load fields";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // datAuftragStatusDatum
            // 
            this.datAuftragStatusDatum.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datAuftragStatusDatum.Location = new System.Drawing.Point(285, 277);
            this.datAuftragStatusDatum.Name = "datAuftragStatusDatum";
            this.datAuftragStatusDatum.Size = new System.Drawing.Size(233, 20);
            this.datAuftragStatusDatum.TabIndex = 23;
            this.datAuftragStatusDatum.Value = new System.DateTime(2012, 9, 17, 12, 31, 34, 0);
            // 
            // txtStaff
            // 
            this.txtStaff.Location = new System.Drawing.Point(11, 317);
            this.txtStaff.Multiline = true;
            this.txtStaff.Name = "txtStaff";
            this.txtStaff.Size = new System.Drawing.Size(507, 61);
            this.txtStaff.TabIndex = 21;
            this.txtStaff.WordWrap = false;
            this.txtStaff.Validating += new System.ComponentModel.CancelEventHandler(this.txtStaff_Validating);
            // 
            // txtAuftragStatus
            // 
            this.txtAuftragStatus.Location = new System.Drawing.Point(11, 277);
            this.txtAuftragStatus.Name = "txtAuftragStatus";
            this.txtAuftragStatus.Size = new System.Drawing.Size(233, 20);
            this.txtAuftragStatus.TabIndex = 19;
            this.txtAuftragStatus.Text = "open";
            // 
            // txtBezeichnung
            // 
            this.txtBezeichnung.Location = new System.Drawing.Point(11, 210);
            this.txtBezeichnung.Name = "txtBezeichnung";
            this.txtBezeichnung.Size = new System.Drawing.Size(507, 20);
            this.txtBezeichnung.TabIndex = 18;
            this.txtBezeichnung.Text = "My engagement\'s description";
            // 
            // txtNiederlasdsung
            // 
            this.txtNiederlasdsung.Location = new System.Drawing.Point(285, 172);
            this.txtNiederlasdsung.Name = "txtNiederlasdsung";
            this.txtNiederlasdsung.Size = new System.Drawing.Size(233, 20);
            this.txtNiederlasdsung.TabIndex = 17;
            this.txtNiederlasdsung.Text = "Frankfurt";
            // 
            // txtConcurringPartner
            // 
            this.txtConcurringPartner.Location = new System.Drawing.Point(11, 171);
            this.txtConcurringPartner.Name = "txtConcurringPartner";
            this.txtConcurringPartner.Size = new System.Drawing.Size(233, 20);
            this.txtConcurringPartner.TabIndex = 16;
            this.txtConcurringPartner.Validating += new System.ComponentModel.CancelEventHandler(this.txtConcurringPartner_Validating);
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(285, 126);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(233, 20);
            this.txtCompany.TabIndex = 15;
            this.txtCompany.Text = "Company AG";
            // 
            // txtOpportunityNumber
            // 
            this.txtOpportunityNumber.Location = new System.Drawing.Point(11, 126);
            this.txtOpportunityNumber.Name = "txtOpportunityNumber";
            this.txtOpportunityNumber.Size = new System.Drawing.Size(233, 20);
            this.txtOpportunityNumber.TabIndex = 14;
            this.txtOpportunityNumber.Text = "111111";
            // 
            // txtEngagementManager
            // 
            this.txtEngagementManager.Location = new System.Drawing.Point(285, 80);
            this.txtEngagementManager.Name = "txtEngagementManager";
            this.txtEngagementManager.Size = new System.Drawing.Size(233, 20);
            this.txtEngagementManager.TabIndex = 13;
            this.txtEngagementManager.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngagementManager_Validating);
            // 
            // txtEngagementPartner
            // 
            this.txtEngagementPartner.Location = new System.Drawing.Point(11, 80);
            this.txtEngagementPartner.Name = "txtEngagementPartner";
            this.txtEngagementPartner.Size = new System.Drawing.Size(233, 20);
            this.txtEngagementPartner.TabIndex = 12;
            this.txtEngagementPartner.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngagementPartner_Validating);
            // 
            // txtWbNumber
            // 
            this.txtWbNumber.Location = new System.Drawing.Point(84, 16);
            this.txtWbNumber.Name = "txtWbNumber";
            this.txtWbNumber.Size = new System.Drawing.Size(233, 20);
            this.txtWbNumber.TabIndex = 11;
            this.txtWbNumber.Validating += new System.ComponentModel.CancelEventHandler(this.txtWbNumber_Validating);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 320);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Staff:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(282, 261);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(132, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "WB-Auftrag Status Datum:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 261);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "WB-Auftrag Status:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 194);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Bezeichnung:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(282, 156);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Niederlassung:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Concurring partner:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(282, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Account:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Opportunity number:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(282, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Engagement Manager::";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Engagement Partner:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "WB Nummer:";
            // 
            // tabProcesses
            // 
            this.tabProcesses.Controls.Add(this.btnConvertToEng);
            this.tabProcesses.Controls.Add(this.btnGetStatus);
            this.tabProcesses.Controls.Add(this.txtEngagementStatus);
            this.tabProcesses.Controls.Add(this.btnReopen);
            this.tabProcesses.Controls.Add(this.btnClose);
            this.tabProcesses.Controls.Add(this.txtProcessSite);
            this.tabProcesses.Controls.Add(this.label12);
            this.tabProcesses.Location = new System.Drawing.Point(4, 22);
            this.tabProcesses.Name = "tabProcesses";
            this.tabProcesses.Padding = new System.Windows.Forms.Padding(3);
            this.tabProcesses.Size = new System.Drawing.Size(543, 514);
            this.tabProcesses.TabIndex = 1;
            this.tabProcesses.Text = "Processes";
            this.tabProcesses.UseVisualStyleBackColor = true;
            // 
            // btnConvertToEng
            // 
            this.btnConvertToEng.Location = new System.Drawing.Point(20, 134);
            this.btnConvertToEng.Name = "btnConvertToEng";
            this.btnConvertToEng.Size = new System.Drawing.Size(172, 23);
            this.btnConvertToEng.TabIndex = 30;
            this.btnConvertToEng.Text = "Convert Opportunity To Engagement";
            this.btnConvertToEng.UseVisualStyleBackColor = true;
            // 
            // btnGetStatus
            // 
            this.btnGetStatus.Location = new System.Drawing.Point(20, 96);
            this.btnGetStatus.Name = "btnGetStatus";
            this.btnGetStatus.Size = new System.Drawing.Size(172, 23);
            this.btnGetStatus.TabIndex = 29;
            this.btnGetStatus.Text = "Get Current Status";
            this.btnGetStatus.UseVisualStyleBackColor = true;
            this.btnGetStatus.Click += new System.EventHandler(this.btnGetStatus_Click);
            // 
            // txtEngagementStatus
            // 
            this.txtEngagementStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtEngagementStatus.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtEngagementStatus.Location = new System.Drawing.Point(20, 288);
            this.txtEngagementStatus.Multiline = true;
            this.txtEngagementStatus.Name = "txtEngagementStatus";
            this.txtEngagementStatus.Size = new System.Drawing.Size(507, 202);
            this.txtEngagementStatus.TabIndex = 28;
            this.txtEngagementStatus.WordWrap = false;
            // 
            // btnReopen
            // 
            this.btnReopen.Location = new System.Drawing.Point(212, 134);
            this.btnReopen.Name = "btnReopen";
            this.btnReopen.Size = new System.Drawing.Size(172, 23);
            this.btnReopen.TabIndex = 15;
            this.btnReopen.Text = "Reopen";
            this.btnReopen.UseVisualStyleBackColor = true;
            this.btnReopen.Click += new System.EventHandler(this.btnReopen_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(212, 96);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(172, 23);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtProcessSite
            // 
            this.txtProcessSite.Location = new System.Drawing.Point(93, 44);
            this.txtProcessSite.Name = "txtProcessSite";
            this.txtProcessSite.Size = new System.Drawing.Size(233, 20);
            this.txtProcessSite.TabIndex = 13;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 47);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "WB Nummer:";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // epEngagementPartner
            // 
            this.epEngagementPartner.ContainerControl = this;
            // 
            // epEngagementManager
            // 
            this.epEngagementManager.ContainerControl = this;
            // 
            // epConcurringPartner
            // 
            this.epConcurringPartner.ContainerControl = this;
            // 
            // epStaff
            // 
            this.epStaff.ContainerControl = this;
            // 
            // epWbNumber
            // 
            this.epWbNumber.ContainerControl = this;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(551, 562);
            this.Controls.Add(this.tabEngagements);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Text = "Engagements Test Console";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabEngagements.ResumeLayout(false);
            this.tabEngagement.ResumeLayout(false);
            this.tabEngagement.PerformLayout();
            this.tabProcesses.ResumeLayout(false);
            this.tabProcesses.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.epEngagementPartner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epEngagementManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epConcurringPartner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epStaff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epWbNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion        

        private System.Windows.Forms.Button btnCreateOpportunity;
        private System.Windows.Forms.Button btnUpdateOpportunity;
        private System.Windows.Forms.Button btnUpdateEngagement;
        private System.Windows.Forms.Button btnConvertToEng;
        private System.Windows.Forms.ErrorProvider epEngagementPartner;
        private System.Windows.Forms.ErrorProvider epEngagementManager;
        private System.Windows.Forms.ErrorProvider epConcurringPartner;
        private System.Windows.Forms.ErrorProvider epStaff;
        private System.Windows.Forms.ErrorProvider epWbNumber;

    }
}
