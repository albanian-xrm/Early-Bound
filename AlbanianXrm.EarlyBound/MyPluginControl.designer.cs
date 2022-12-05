namespace AlbanianXrm.EarlyBound
{
    partial class MyPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Syncfusion.Windows.Forms.Tools.TreeNodeAdvStyleInfo treeNodeAdvStyleInfo1 = new Syncfusion.Windows.Forms.Tools.TreeNodeAdvStyleInfo();
            this.metadataTree = new Syncfusion.Windows.Forms.Tools.TreeViewAdv();
            this.mnuMetadataTree = new Syncfusion.Windows.Forms.Tools.ContextMenuStripEx();
            this.mnuGetMetadata = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelectNone = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelectGenerated = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.btnGetMetadata = new System.Windows.Forms.ToolStripButton();
            this.btnDownloadCLI = new System.Windows.Forms.ToolStripButton();
            this.btnGenerateEntities = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.splitContainer = new Syncfusion.Windows.Forms.Tools.SplitContainerAdv();
            this.tlpEntities = new System.Windows.Forms.TableLayoutPanel();
            this.flpEntityFilters = new System.Windows.Forms.FlowLayoutPanel();
            this.chkOnlySelected = new System.Windows.Forms.CheckBox();
            this.lblSelectEntity = new System.Windows.Forms.Label();
            this.cmbFindEntity = new Syncfusion.WinForms.ListView.SfComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbFindChild = new Syncfusion.WinForms.ListView.SfComboBox();
            this.splitContainerVertical = new Syncfusion.Windows.Forms.Tools.SplitContainerAdv();
            this.optionsGrid = new System.Windows.Forms.PropertyGrid();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.mnuOutput = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuCopyCommand = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.metadataTree)).BeginInit();
            this.mnuMetadataTree.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tlpEntities.SuspendLayout();
            this.flpEntityFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFindEntity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFindChild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerVertical)).BeginInit();
            this.splitContainerVertical.Panel1.SuspendLayout();
            this.splitContainerVertical.Panel2.SuspendLayout();
            this.splitContainerVertical.SuspendLayout();
            this.mnuOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // metadataTree
            // 
            this.metadataTree.AllowMouseBasedSelection = true;
            this.metadataTree.BackgroundImage = global::AlbanianXrm.EarlyBound.Properties.Resources.Logo;
            this.metadataTree.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            treeNodeAdvStyleInfo1.CheckBoxTickThickness = 1;
            treeNodeAdvStyleInfo1.CheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            treeNodeAdvStyleInfo1.EnsureDefaultOptionedChild = true;
            treeNodeAdvStyleInfo1.IntermediateCheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            treeNodeAdvStyleInfo1.OptionButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            treeNodeAdvStyleInfo1.SelectedOptionButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.metadataTree.BaseStylePairs.AddRange(new Syncfusion.Windows.Forms.Tools.StyleNamePair[] {
            new Syncfusion.Windows.Forms.Tools.StyleNamePair("Standard", treeNodeAdvStyleInfo1)});
            this.metadataTree.BeforeTouchSize = new System.Drawing.Size(1248, 601);
            this.metadataTree.ContextMenuStrip = this.mnuMetadataTree;
            this.metadataTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metadataTree.Enabled = false;
            // 
            // 
            // 
            this.metadataTree.HelpTextControl.BaseThemeName = null;
            this.metadataTree.HelpTextControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metadataTree.HelpTextControl.Location = new System.Drawing.Point(0, 0);
            this.metadataTree.HelpTextControl.Name = "helpText";
            this.metadataTree.HelpTextControl.Size = new System.Drawing.Size(71, 22);
            this.metadataTree.HelpTextControl.TabIndex = 0;
            this.metadataTree.HelpTextControl.Text = "help text";
            this.metadataTree.InactiveSelectedNodeForeColor = System.Drawing.SystemColors.ControlText;
            this.metadataTree.Indent = 24;
            this.metadataTree.ItemHeight = 27;
            this.metadataTree.LoadOnDemand = true;
            this.metadataTree.Location = new System.Drawing.Point(4, 67);
            this.metadataTree.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.metadataTree.MetroColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(165)))), ((int)(((byte)(220)))));
            this.metadataTree.Name = "metadataTree";
            this.metadataTree.SelectedNodeForeColor = System.Drawing.SystemColors.HighlightText;
            this.metadataTree.Size = new System.Drawing.Size(1248, 601);
            this.metadataTree.TabIndex = 0;
            this.metadataTree.Text = "treeViewAdv1";
            this.metadataTree.ThemeStyle.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.metadataTree.ThemeStyle.TreeNodeAdvStyle.CheckBoxTickThickness = 0;
            this.metadataTree.ThemeStyle.TreeNodeAdvStyle.CheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metadataTree.ThemeStyle.TreeNodeAdvStyle.EnsureDefaultOptionedChild = true;
            this.metadataTree.ThemeStyle.TreeNodeAdvStyle.IntermediateCheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metadataTree.ThemeStyle.TreeNodeAdvStyle.OptionButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metadataTree.ThemeStyle.TreeNodeAdvStyle.SelectedOptionButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            // 
            // 
            // 
            this.metadataTree.ToolTipControl.BackColor = System.Drawing.SystemColors.Info;
            this.metadataTree.ToolTipControl.BaseThemeName = null;
            this.metadataTree.ToolTipControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metadataTree.ToolTipControl.Location = new System.Drawing.Point(0, 0);
            this.metadataTree.ToolTipControl.Name = "toolTip";
            this.metadataTree.ToolTipControl.Size = new System.Drawing.Size(58, 22);
            this.metadataTree.ToolTipControl.TabIndex = 1;
            this.metadataTree.ToolTipControl.Text = "toolTip";
            this.metadataTree.BeforeExpand += new Syncfusion.Windows.Forms.Tools.TreeViewAdvCancelableNodeEventHandler(this.TreeViewAdv1_BeforeExpand);
            // 
            // mnuMetadataTree
            // 
            this.mnuMetadataTree.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mnuMetadataTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGetMetadata,
            this.mnuSelectAll,
            this.mnuSelectNone,
            this.mnuSelectGenerated});
            this.mnuMetadataTree.MetroColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(249)))));
            this.mnuMetadataTree.Name = "mnuMetadataTree";
            this.mnuMetadataTree.Size = new System.Drawing.Size(217, 169);
            this.mnuMetadataTree.Style = Syncfusion.Windows.Forms.Tools.ContextMenuStripEx.ContextMenuStyle.Default;
            this.mnuMetadataTree.Text = "Metadata";
            this.mnuMetadataTree.ThemeName = "Default";
            // 
            // mnuGetMetadata
            // 
            this.mnuGetMetadata.Name = "mnuGetMetadata";
            this.mnuGetMetadata.Size = new System.Drawing.Size(216, 32);
            this.mnuGetMetadata.Text = "Get Metadata";
            this.mnuGetMetadata.Click += new System.EventHandler(this.BtnGetMetadata_Click);
            // 
            // mnuSelectAll
            // 
            this.mnuSelectAll.Name = "mnuSelectAll";
            this.mnuSelectAll.Size = new System.Drawing.Size(216, 32);
            this.mnuSelectAll.Text = "Select All";
            this.mnuSelectAll.Click += new System.EventHandler(this.MnuSelectAll_Click);
            // 
            // mnuSelectNone
            // 
            this.mnuSelectNone.Name = "mnuSelectNone";
            this.mnuSelectNone.Size = new System.Drawing.Size(216, 32);
            this.mnuSelectNone.Text = "Select None";
            this.mnuSelectNone.Click += new System.EventHandler(this.MnuSelectNone_Click);
            // 
            // mnuSelectGenerated
            // 
            this.mnuSelectGenerated.Name = "mnuSelectGenerated";
            this.mnuSelectGenerated.Size = new System.Drawing.Size(216, 32);
            this.mnuSelectGenerated.Text = "Select Generated";
            this.mnuSelectGenerated.Click += new System.EventHandler(this.MnuSelectGenerated_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Image = null;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnGetMetadata,
            this.btnGenerateEntities,
            this.toolStripButton4,
            this.btnDownloadCLI});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Office12Mode = false;
            this.toolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(1791, 38);
            this.toolStrip.TabIndex = 6;
            // 
            // btnGetMetadata
            // 
            this.btnGetMetadata.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnGetMetadata.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGetMetadata.Name = "btnGetMetadata";
            this.btnGetMetadata.Size = new System.Drawing.Size(123, 33);
            this.btnGetMetadata.Text = "Get Metadata";
            this.btnGetMetadata.Click += new System.EventHandler(this.BtnGetMetadata_Click);
            // 
            // btnDownloadCLI
            // 
            this.btnDownloadCLI.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDownloadCLI.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDownloadCLI.Name = "btnDownloadCLI";
            this.btnDownloadCLI.Size = new System.Drawing.Size(311, 33);
            this.btnDownloadCLI.Text = "Get Microsoft.PowerApps.CLI installer";
            this.btnDownloadCLI.Click += new System.EventHandler(this.BtnDownloadCLI_Click);
            // 
            // btnGenerateEntities
            // 
            this.btnGenerateEntities.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnGenerateEntities.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGenerateEntities.Name = "btnGenerateEntities";
            this.btnGenerateEntities.Size = new System.Drawing.Size(86, 33);
            this.btnGenerateEntities.Text = "Generate";
            this.btnGenerateEntities.Click += new System.EventHandler(this.BtnGenerateEntities_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(122, 33);
            this.toolStripButton4.Text = "Save Settings";
            this.toolStripButton4.Click += new System.EventHandler(this.MnuSaveSettings_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.BeforeTouchSize = 7;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = Syncfusion.Windows.Forms.Tools.Enums.FixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(0, 38);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tlpEntities);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.splitContainerVertical);
            this.splitContainer.PanelToBeCollapsed = Syncfusion.Windows.Forms.Tools.Enums.CollapsedPanel.Panel1;
            this.splitContainer.Size = new System.Drawing.Size(1791, 673);
            this.splitContainer.SplitterDistance = 1256;
            this.splitContainer.TabIndex = 7;
            this.splitContainer.Text = "splitContainer";
            this.splitContainer.ThemeName = "None";
            // 
            // tlpEntities
            // 
            this.tlpEntities.ColumnCount = 1;
            this.tlpEntities.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpEntities.Controls.Add(this.metadataTree, 0, 1);
            this.tlpEntities.Controls.Add(this.flpEntityFilters, 0, 0);
            this.tlpEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEntities.Location = new System.Drawing.Point(0, 0);
            this.tlpEntities.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tlpEntities.Name = "tlpEntities";
            this.tlpEntities.RowCount = 2;
            this.tlpEntities.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tlpEntities.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpEntities.Size = new System.Drawing.Size(1256, 673);
            this.tlpEntities.TabIndex = 1;
            // 
            // flpEntityFilters
            // 
            this.flpEntityFilters.Controls.Add(this.chkOnlySelected);
            this.flpEntityFilters.Controls.Add(this.lblSelectEntity);
            this.flpEntityFilters.Controls.Add(this.cmbFindEntity);
            this.flpEntityFilters.Controls.Add(this.label1);
            this.flpEntityFilters.Controls.Add(this.cmbFindChild);
            this.flpEntityFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpEntityFilters.Location = new System.Drawing.Point(4, 5);
            this.flpEntityFilters.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flpEntityFilters.Name = "flpEntityFilters";
            this.flpEntityFilters.Size = new System.Drawing.Size(1248, 52);
            this.flpEntityFilters.TabIndex = 1;
            // 
            // chkOnlySelected
            // 
            this.chkOnlySelected.AutoSize = true;
            this.chkOnlySelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkOnlySelected.Location = new System.Drawing.Point(4, 5);
            this.chkOnlySelected.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkOnlySelected.Name = "chkOnlySelected";
            this.chkOnlySelected.Size = new System.Drawing.Size(141, 43);
            this.chkOnlySelected.TabIndex = 0;
            this.chkOnlySelected.Text = "Filter Selected ";
            this.chkOnlySelected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkOnlySelected.UseVisualStyleBackColor = true;
            // 
            // lblSelectEntity
            // 
            this.lblSelectEntity.AutoSize = true;
            this.lblSelectEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSelectEntity.Location = new System.Drawing.Point(153, 0);
            this.lblSelectEntity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelectEntity.Name = "lblSelectEntity";
            this.lblSelectEntity.Size = new System.Drawing.Size(84, 53);
            this.lblSelectEntity.TabIndex = 2;
            this.lblSelectEntity.Text = "Find Entity";
            this.lblSelectEntity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbFindEntity
            // 
            this.cmbFindEntity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbFindEntity.AutoCompleteSuggestMode = Syncfusion.WinForms.ListView.Enums.AutoCompleteSuggestMode.Contains;
            this.cmbFindEntity.DisplayMember = "Value";
            this.cmbFindEntity.Location = new System.Drawing.Point(245, 5);
            this.cmbFindEntity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbFindEntity.Name = "cmbFindEntity";
            this.cmbFindEntity.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.cmbFindEntity.Size = new System.Drawing.Size(300, 43);
            this.cmbFindEntity.TabIndex = 3;
            this.cmbFindEntity.ValueMember = "Key";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(553, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 53);
            this.label1.TabIndex = 4;
            this.label1.Text = "Find Child";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbFindChild
            // 
            this.cmbFindChild.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbFindChild.AutoCompleteSuggestMode = Syncfusion.WinForms.ListView.Enums.AutoCompleteSuggestMode.Contains;
            this.cmbFindChild.DisplayMember = "Value";
            this.cmbFindChild.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFindChild.Location = new System.Drawing.Point(640, 5);
            this.cmbFindChild.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbFindChild.Name = "cmbFindChild";
            this.cmbFindChild.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.cmbFindChild.Size = new System.Drawing.Size(300, 43);
            this.cmbFindChild.TabIndex = 5;
            this.cmbFindChild.ValueMember = "Key";
            // 
            // splitContainerVertical
            // 
            this.splitContainerVertical.BeforeTouchSize = 7;
            this.splitContainerVertical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerVertical.Location = new System.Drawing.Point(0, 0);
            this.splitContainerVertical.Name = "splitContainerVertical";
            this.splitContainerVertical.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // splitContainerVertical.Panel1
            // 
            this.splitContainerVertical.Panel1.Controls.Add(this.optionsGrid);
            // 
            // splitContainerVertical.Panel2
            // 
            this.splitContainerVertical.Panel2.Controls.Add(this.txtOutput);
            this.splitContainerVertical.Size = new System.Drawing.Size(528, 673);
            this.splitContainerVertical.SplitterDistance = 314;
            this.splitContainerVertical.TabIndex = 5;
            this.splitContainerVertical.Text = "splitContainerAdv1";
            this.splitContainerVertical.ThemeName = "None";
            // 
            // optionsGrid
            // 
            this.optionsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionsGrid.Location = new System.Drawing.Point(0, 0);
            this.optionsGrid.Name = "optionsGrid";
            this.optionsGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.optionsGrid.Size = new System.Drawing.Size(528, 314);
            this.optionsGrid.TabIndex = 0;
            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.Color.Black;
            this.txtOutput.ContextMenuStrip = this.mnuOutput;
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.ForeColor = System.Drawing.Color.White;
            this.txtOutput.Location = new System.Drawing.Point(0, 0);
            this.txtOutput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(528, 352);
            this.txtOutput.TabIndex = 1;
            this.txtOutput.Text = "";
            // 
            // mnuOutput
            // 
            this.mnuOutput.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mnuOutput.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCopyCommand});
            this.mnuOutput.Name = "mnuOutput";
            this.mnuOutput.Size = new System.Drawing.Size(216, 36);
            // 
            // mnuCopyCommand
            // 
            this.mnuCopyCommand.Name = "mnuCopyCommand";
            this.mnuCopyCommand.Size = new System.Drawing.Size(215, 32);
            this.mnuCopyCommand.Text = "Copy Command";
            this.mnuCopyCommand.Click += new System.EventHandler(this.MnuCopyCommand_Click);
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.toolStrip);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(1791, 711);
            this.OnCloseTool += new System.EventHandler(this.MyPluginControl_OnCloseTool);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metadataTree)).EndInit();
            this.mnuMetadataTree.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tlpEntities.ResumeLayout(false);
            this.flpEntityFilters.ResumeLayout(false);
            this.flpEntityFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFindEntity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFindChild)).EndInit();
            this.splitContainerVertical.Panel1.ResumeLayout(false);
            this.splitContainerVertical.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerVertical)).EndInit();
            this.splitContainerVertical.ResumeLayout(false);
            this.mnuOutput.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Syncfusion.Windows.Forms.Tools.TreeViewAdv metadataTree;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStrip;
        private Syncfusion.Windows.Forms.Tools.SplitContainerAdv splitContainer;
        private System.Windows.Forms.ToolStripButton btnGenerateEntities;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private Syncfusion.Windows.Forms.Tools.SplitContainerAdv splitContainerVertical;
        private System.Windows.Forms.PropertyGrid optionsGrid;
        internal System.Windows.Forms.ToolStripButton btnDownloadCLI;
        private System.Windows.Forms.RichTextBox txtOutput;
        private Syncfusion.Windows.Forms.Tools.ContextMenuStripEx mnuMetadataTree;
        private System.Windows.Forms.ToolStripMenuItem mnuGetMetadata;
        private System.Windows.Forms.ToolStripMenuItem mnuSelectAll;
        private System.Windows.Forms.ToolStripMenuItem mnuSelectNone;
        private System.Windows.Forms.ToolStripMenuItem mnuSelectGenerated;
        internal System.Windows.Forms.ToolStripButton btnGetMetadata;
        private System.Windows.Forms.ContextMenuStrip mnuOutput;
        private System.Windows.Forms.ToolStripMenuItem mnuCopyCommand;
        private System.Windows.Forms.TableLayoutPanel tlpEntities;
        private System.Windows.Forms.FlowLayoutPanel flpEntityFilters;
        private System.Windows.Forms.CheckBox chkOnlySelected;
        private System.Windows.Forms.Label lblSelectEntity;
        private Syncfusion.WinForms.ListView.SfComboBox cmbFindEntity;
        private System.Windows.Forms.Label label1;
        private Syncfusion.WinForms.ListView.SfComboBox cmbFindChild;
    }
}