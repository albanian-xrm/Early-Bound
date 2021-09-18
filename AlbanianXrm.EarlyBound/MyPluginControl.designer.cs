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
            this.btnCoreTools = new System.Windows.Forms.ToolStripButton();
            this.btnGenerateEntities = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.splitContainer = new Syncfusion.Windows.Forms.Tools.SplitContainerAdv();
            this.tlpEntities = new System.Windows.Forms.TableLayoutPanel();
            this.flpEntityFilters = new System.Windows.Forms.FlowLayoutPanel();
            this.chkOnlySelected = new System.Windows.Forms.CheckBox();
            this.lblSelectEntity = new System.Windows.Forms.Label();
            this.cmbFindEntity = new Syncfusion.WinForms.ListView.SfComboBox();
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
            this.metadataTree.BeforeTouchSize = new System.Drawing.Size(621, 360);
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
            this.metadataTree.HelpTextControl.Size = new System.Drawing.Size(49, 15);
            this.metadataTree.HelpTextControl.TabIndex = 0;
            this.metadataTree.HelpTextControl.Text = "help text";
            this.metadataTree.InactiveSelectedNodeForeColor = System.Drawing.SystemColors.ControlText;
            this.metadataTree.Indent = 24;
            this.metadataTree.ItemHeight = 27;
            this.metadataTree.LoadOnDemand = true;
            this.metadataTree.Location = new System.Drawing.Point(3, 43);
            this.metadataTree.MetroColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(165)))), ((int)(((byte)(220)))));
            this.metadataTree.Name = "metadataTree";
            this.metadataTree.SelectedNodeForeColor = System.Drawing.SystemColors.HighlightText;
            this.metadataTree.Size = new System.Drawing.Size(621, 360);
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
            this.metadataTree.ToolTipControl.Size = new System.Drawing.Size(41, 15);
            this.metadataTree.ToolTipControl.TabIndex = 1;
            this.metadataTree.ToolTipControl.Text = "toolTip";
            this.metadataTree.BeforeExpand += new Syncfusion.Windows.Forms.Tools.TreeViewAdvCancelableNodeEventHandler(this.TreeViewAdv1_BeforeExpand);
            // 
            // mnuMetadataTree
            // 
            this.mnuMetadataTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGetMetadata,
            this.mnuSelectAll,
            this.mnuSelectNone,
            this.mnuSelectGenerated});
            this.mnuMetadataTree.MetroColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(249)))));
            this.mnuMetadataTree.Name = "mnuMetadataTree";
            this.mnuMetadataTree.Size = new System.Drawing.Size(163, 119);
            this.mnuMetadataTree.Style = Syncfusion.Windows.Forms.Tools.ContextMenuStripEx.ContextMenuStyle.Default;
            this.mnuMetadataTree.Text = "Metadata";
            this.mnuMetadataTree.ThemeName = "Default";
            // 
            // mnuGetMetadata
            // 
            this.mnuGetMetadata.Name = "mnuGetMetadata";
            this.mnuGetMetadata.Size = new System.Drawing.Size(162, 22);
            this.mnuGetMetadata.Text = "Get Metadata";
            this.mnuGetMetadata.Click += new System.EventHandler(this.BtnGetMetadata_Click);
            // 
            // mnuSelectAll
            // 
            this.mnuSelectAll.Name = "mnuSelectAll";
            this.mnuSelectAll.Size = new System.Drawing.Size(162, 22);
            this.mnuSelectAll.Text = "Select All";
            this.mnuSelectAll.Click += new System.EventHandler(this.MnuSelectAll_Click);
            // 
            // mnuSelectNone
            // 
            this.mnuSelectNone.Name = "mnuSelectNone";
            this.mnuSelectNone.Size = new System.Drawing.Size(162, 22);
            this.mnuSelectNone.Text = "Select None";
            this.mnuSelectNone.Click += new System.EventHandler(this.MnuSelectNone_Click);
            // 
            // mnuSelectGenerated
            // 
            this.mnuSelectGenerated.Name = "mnuSelectGenerated";
            this.mnuSelectGenerated.Size = new System.Drawing.Size(162, 22);
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
            this.btnCoreTools,
            this.btnGenerateEntities,
            this.toolStripButton4});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Office12Mode = false;
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(986, 25);
            this.toolStrip.TabIndex = 6;
            // 
            // btnGetMetadata
            // 
            this.btnGetMetadata.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnGetMetadata.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGetMetadata.Name = "btnGetMetadata";
            this.btnGetMetadata.Size = new System.Drawing.Size(82, 22);
            this.btnGetMetadata.Text = "Get Metadata";
            this.btnGetMetadata.Click += new System.EventHandler(this.BtnGetMetadata_Click);
            // 
            // btnCoreTools
            // 
            this.btnCoreTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCoreTools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCoreTools.Name = "btnCoreTools";
            this.btnCoreTools.Size = new System.Drawing.Size(84, 22);
            this.btnCoreTools.Text = "Get CoreTools";
            this.btnCoreTools.Click += new System.EventHandler(this.BtnCoreTools_Click);
            // 
            // btnGenerateEntities
            // 
            this.btnGenerateEntities.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnGenerateEntities.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGenerateEntities.Name = "btnGenerateEntities";
            this.btnGenerateEntities.Size = new System.Drawing.Size(58, 22);
            this.btnGenerateEntities.Text = "Generate";
            this.btnGenerateEntities.Click += new System.EventHandler(this.BtnGenerateEntities_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(80, 22);
            this.toolStripButton4.Text = "Save Settings";
            this.toolStripButton4.Click += new System.EventHandler(this.MnuSaveSettings_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.BeforeTouchSize = 7;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = Syncfusion.Windows.Forms.Tools.Enums.FixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(0, 25);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tlpEntities);
            this.splitContainer.Panel1.Margin = new System.Windows.Forms.Padding(2);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.splitContainerVertical);
            this.splitContainer.Panel2.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer.PanelToBeCollapsed = Syncfusion.Windows.Forms.Tools.Enums.CollapsedPanel.Panel1;
            this.splitContainer.Size = new System.Drawing.Size(986, 391);
            this.splitContainer.SplitterDistance = 627;
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
            this.tlpEntities.Name = "tlpEntities";
            this.tlpEntities.RowCount = 2;
            this.tlpEntities.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpEntities.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpEntities.Size = new System.Drawing.Size(627, 391);
            this.tlpEntities.TabIndex = 1;
            // 
            // flpEntityFilters
            // 
            this.flpEntityFilters.Controls.Add(this.chkOnlySelected);
            this.flpEntityFilters.Controls.Add(this.lblSelectEntity);
            this.flpEntityFilters.Controls.Add(this.cmbFindEntity);
            this.flpEntityFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpEntityFilters.Location = new System.Drawing.Point(3, 3);
            this.flpEntityFilters.Name = "flpEntityFilters";
            this.flpEntityFilters.Size = new System.Drawing.Size(621, 34);
            this.flpEntityFilters.TabIndex = 1;
            // 
            // chkOnlySelected
            // 
            this.chkOnlySelected.AutoSize = true;
            this.chkOnlySelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkOnlySelected.Location = new System.Drawing.Point(3, 3);
            this.chkOnlySelected.Name = "chkOnlySelected";
            this.chkOnlySelected.Size = new System.Drawing.Size(96, 28);
            this.chkOnlySelected.TabIndex = 0;
            this.chkOnlySelected.Text = "Filter Selected ";
            this.chkOnlySelected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkOnlySelected.UseVisualStyleBackColor = true;
            // 
            // lblSelectEntity
            // 
            this.lblSelectEntity.AutoSize = true;
            this.lblSelectEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSelectEntity.Location = new System.Drawing.Point(105, 0);
            this.lblSelectEntity.Name = "lblSelectEntity";
            this.lblSelectEntity.Size = new System.Drawing.Size(56, 34);
            this.lblSelectEntity.TabIndex = 2;
            this.lblSelectEntity.Text = "Find Entity";
            this.lblSelectEntity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbFindEntity
            // 
            this.cmbFindEntity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbFindEntity.AutoCompleteSuggestMode = Syncfusion.WinForms.ListView.Enums.AutoCompleteSuggestMode.Contains;
            this.cmbFindEntity.DisplayMember = "Value";
            this.cmbFindEntity.Location = new System.Drawing.Point(167, 3);
            this.cmbFindEntity.Name = "cmbFindEntity";
            this.cmbFindEntity.Size = new System.Drawing.Size(200, 28);
            this.cmbFindEntity.TabIndex = 3;
            this.cmbFindEntity.ValueMember = "Key";
            // 
            // splitContainerVertical
            // 
            this.splitContainerVertical.BeforeTouchSize = 7;
            this.splitContainerVertical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerVertical.Location = new System.Drawing.Point(0, 0);
            this.splitContainerVertical.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerVertical.Name = "splitContainerVertical";
            this.splitContainerVertical.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // splitContainerVertical.Panel1
            // 
            this.splitContainerVertical.Panel1.Controls.Add(this.optionsGrid);
            this.splitContainerVertical.Panel1.Margin = new System.Windows.Forms.Padding(2);
            // 
            // splitContainerVertical.Panel2
            // 
            this.splitContainerVertical.Panel2.Controls.Add(this.txtOutput);
            this.splitContainerVertical.Panel2.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerVertical.Size = new System.Drawing.Size(352, 391);
            this.splitContainerVertical.SplitterDistance = 183;
            this.splitContainerVertical.TabIndex = 5;
            this.splitContainerVertical.Text = "splitContainerAdv1";
            this.splitContainerVertical.ThemeName = "None";
            // 
            // optionsGrid
            // 
            this.optionsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionsGrid.Location = new System.Drawing.Point(0, 0);
            this.optionsGrid.Margin = new System.Windows.Forms.Padding(2);
            this.optionsGrid.Name = "optionsGrid";
            this.optionsGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.optionsGrid.Size = new System.Drawing.Size(352, 183);
            this.optionsGrid.TabIndex = 0;
            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.Color.Black;
            this.txtOutput.ContextMenuStrip = this.mnuOutput;
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.ForeColor = System.Drawing.Color.White;
            this.txtOutput.Location = new System.Drawing.Point(0, 0);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(352, 201);
            this.txtOutput.TabIndex = 1;
            this.txtOutput.Text = "";
            // 
            // mnuOutput
            // 
            this.mnuOutput.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCopyCommand});
            this.mnuOutput.Name = "mnuOutput";
            this.mnuOutput.Size = new System.Drawing.Size(163, 26);
            // 
            // mnuCopyCommand
            // 
            this.mnuCopyCommand.Name = "mnuCopyCommand";
            this.mnuCopyCommand.Size = new System.Drawing.Size(162, 22);
            this.mnuCopyCommand.Text = "Copy Command";
            this.mnuCopyCommand.Click += new System.EventHandler(this.MnuCopyCommand_Click);
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.toolStrip);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(986, 416);
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
        internal System.Windows.Forms.ToolStripButton btnCoreTools;
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
    }
}