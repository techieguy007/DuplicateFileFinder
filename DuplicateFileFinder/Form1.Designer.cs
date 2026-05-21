namespace DuplicateFileFinder
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCleanupDuplicates;
        private System.Windows.Forms.Button btnShowSimilar;
        private System.Windows.Forms.TextBox txtPattern;
        private System.Windows.Forms.Label lblPattern;
        private System.Windows.Forms.CheckBox chkPreview;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ColumnHeader colSelect;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colPath;
        private System.Windows.Forms.ColumnHeader colSize;
        private System.Windows.Forms.ColumnHeader colHash;
        private System.Windows.Forms.Button btnRemoveText;
        private System.Windows.Forms.TextBox txtRemoveText;
        private System.Windows.Forms.Label lblRemoveText;
        private System.Windows.Forms.Button btnLoadAll;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.CheckBox chkRecursive;
        private System.Windows.Forms.Button btnSuggestBySize;
        private System.Windows.Forms.CheckBox chkSkipSystemFiles;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtFolder = new TextBox();
            btnBrowse = new Button();
            btnScan = new Button();
            btnCancel = new Button();
            lvFiles = new ListView();
            colSelect = new ColumnHeader();
            colName = new ColumnHeader();
            colPath = new ColumnHeader();
            colSize = new ColumnHeader();
            colHash = new ColumnHeader();
            btnRename = new Button();
            btnDelete = new Button();
            btnCleanupDuplicates = new Button();
            btnShowSimilar = new Button();
            txtPattern = new TextBox();
            lblPattern = new Label();
            chkPreview = new CheckBox();
            progressBar = new ProgressBar();
            lblStatus = new Label();
            btnRemoveText = new Button();
            txtRemoveText = new TextBox();
            lblRemoveText = new Label();
            btnLoadAll = new Button();
            btnSelectAll = new Button();
            chkRecursive = new CheckBox();
            btnSuggestBySize = new Button();
            chkSkipSystemFiles = new CheckBox();
            SuspendLayout();
            // 
            // txtFolder
            // 
            txtFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtFolder.Location = new Point(12, 12);
            txtFolder.Name = "txtFolder";
            txtFolder.Size = new Size(799, 31);
            txtFolder.TabIndex = 0;
            // 
            // btnBrowse
            // 
            btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowse.Location = new Point(817, 11);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(97, 32);
            btnBrowse.TabIndex = 1;
            btnBrowse.Text = "Browse";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // btnScan
            // 
            btnScan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnScan.Location = new Point(923, 11);
            btnScan.Name = "btnScan";
            btnScan.Size = new Size(75, 32);
            btnScan.TabIndex = 2;
            btnScan.Text = "Scan";
            btnScan.UseVisualStyleBackColor = true;
            btnScan.Click += btnScan_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.Enabled = false;
            btnCancel.Location = new Point(1085, 11);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 32);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lvFiles
            // 
            lvFiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvFiles.Columns.AddRange(new ColumnHeader[] { colSelect, colName, colPath, colSize, colHash });
            lvFiles.FullRowSelect = true;
            lvFiles.Location = new Point(6, 79);
            lvFiles.Name = "lvFiles";
            lvFiles.Size = new Size(1154, 491);
            lvFiles.TabIndex = 4;
            lvFiles.UseCompatibleStateImageBehavior = false;
            lvFiles.View = View.Details;
            lvFiles.ItemChecked += lvFiles_ItemChecked;
            // 
            // colSelect
            // 
            colSelect.Text = "✓";
            colSelect.Width = 35;
            // 
            // colName
            // 
            colName.Text = "Name";
            colName.Width = 150;
            // 
            // colPath
            // 
            colPath.Text = "Path";
            colPath.Width = 350;
            // 
            // colSize
            // 
            colSize.Text = "Size";
            colSize.Width = 80;
            // 
            // colHash
            // 
            colHash.Text = "Hash";
            colHash.Width = 150;
            // 
            // btnRename
            // 
            btnRename.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRename.Location = new Point(1069, 660);
            btnRename.Name = "btnRename";
            btnRename.Size = new Size(91, 38);
            btnRename.TabIndex = 5;
            btnRename.Text = "Rename";
            btnRename.UseVisualStyleBackColor = true;
            btnRename.Click += btnRename_Click;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDelete.Location = new Point(964, 663);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(83, 38);
            btnDelete.TabIndex = 11;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnCleanupDuplicates
            // 
            btnCleanupDuplicates.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCleanupDuplicates.Location = new Point(14, 711);
            btnCleanupDuplicates.Name = "btnCleanupDuplicates";
            btnCleanupDuplicates.Size = new Size(135, 38);
            btnCleanupDuplicates.TabIndex = 12;
            btnCleanupDuplicates.Text = "Cleanup All";
            btnCleanupDuplicates.UseVisualStyleBackColor = true;
            btnCleanupDuplicates.Click += btnCleanupDuplicates_Click;
            // 
            // btnShowSimilar
            // 
            btnShowSimilar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnShowSimilar.Location = new Point(155, 711);
            btnShowSimilar.Name = "btnShowSimilar";
            btnShowSimilar.Size = new Size(135, 38);
            btnShowSimilar.TabIndex = 13;
            btnShowSimilar.Text = "Show Similar";
            btnShowSimilar.UseVisualStyleBackColor = true;
            btnShowSimilar.Click += btnShowSimilar_Click;
            // 
            // txtPattern
            // 
            txtPattern.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtPattern.Location = new Point(197, 623);
            txtPattern.Name = "txtPattern";
            txtPattern.Size = new Size(801, 31);
            txtPattern.TabIndex = 6;
            txtPattern.Text = "{name}_dup{n}";
            // 
            // lblPattern
            // 
            lblPattern.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblPattern.AutoSize = true;
            lblPattern.Location = new Point(14, 626);
            lblPattern.Name = "lblPattern";
            lblPattern.Size = new Size(137, 25);
            lblPattern.TabIndex = 7;
            lblPattern.Text = "Rename pattern";
            // 
            // chkPreview
            // 
            chkPreview.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkPreview.AutoSize = true;
            chkPreview.Location = new Point(884, 622);
            chkPreview.Name = "chkPreview";
            chkPreview.Size = new Size(98, 29);
            chkPreview.TabIndex = 8;
            chkPreview.Text = "Preview";
            chkPreview.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(12, 593);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(1154, 23);
            progressBar.TabIndex = 9;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(12, 525);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(60, 25);
            lblStatus.TabIndex = 10;
            lblStatus.Text = "Ready";
            // 
            // btnRemoveText
            // 
            btnRemoveText.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRemoveText.Location = new Point(1046, 711);
            btnRemoveText.Name = "btnRemoveText";
            btnRemoveText.Size = new Size(114, 38);
            btnRemoveText.TabIndex = 14;
            btnRemoveText.Text = "Remove Text";
            btnRemoveText.UseVisualStyleBackColor = true;
            btnRemoveText.Click += btnRemoveText_Click;
            // 
            // txtRemoveText
            // 
            txtRemoveText.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtRemoveText.Location = new Point(262, 667);
            txtRemoveText.Name = "txtRemoveText";
            txtRemoveText.PlaceholderText = "e.g., _old, -backup, copy";
            txtRemoveText.Size = new Size(696, 31);
            txtRemoveText.TabIndex = 15;
            // 
            // lblRemoveText
            // 
            lblRemoveText.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblRemoveText.AutoSize = true;
            lblRemoveText.Location = new Point(14, 667);
            lblRemoveText.Name = "lblRemoveText";
            lblRemoveText.Size = new Size(245, 25);
            lblRemoveText.TabIndex = 16;
            lblRemoveText.Text = "Text to remove from filename";
            // 
            // btnLoadAll
            // 
            btnLoadAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLoadAll.Location = new Point(1004, 12);
            btnLoadAll.Name = "btnLoadAll";
            btnLoadAll.Size = new Size(75, 32);
            btnLoadAll.TabIndex = 17;
            btnLoadAll.Text = "Load All";
            btnLoadAll.UseVisualStyleBackColor = true;
            btnLoadAll.Click += btnLoadAll_Click;
            // 
            // btnSelectAll
            // 
            btnSelectAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSelectAll.Location = new Point(296, 711);
            btnSelectAll.Name = "btnSelectAll";
            btnSelectAll.Size = new Size(134, 38);
            btnSelectAll.TabIndex = 18;
            btnSelectAll.Text = "Select All";
            btnSelectAll.UseVisualStyleBackColor = true;
            btnSelectAll.Click += btnSelectAll_Click;
            // 
            // chkRecursive
            // 
            chkRecursive.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkRecursive.AutoSize = true;
            chkRecursive.Checked = true;
            chkRecursive.CheckState = CheckState.Checked;
            chkRecursive.Location = new Point(987, 47);
            chkRecursive.Name = "chkRecursive";
            chkRecursive.Size = new Size(181, 29);
            chkRecursive.TabIndex = 19;
            chkRecursive.Text = "Search Recursively";
            chkRecursive.UseVisualStyleBackColor = true;
            // 
            // btnSuggestBySize
            // 
            btnSuggestBySize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSuggestBySize.Location = new Point(436, 711);
            btnSuggestBySize.Name = "btnSuggestBySize";
            btnSuggestBySize.Size = new Size(151, 38);
            btnSuggestBySize.TabIndex = 20;
            btnSuggestBySize.Text = "Suggest by Size";
            btnSuggestBySize.UseVisualStyleBackColor = true;
            btnSuggestBySize.Click += btnSuggestBySize_Click;
            // 
            // chkSkipSystemFiles
            // 
            chkSkipSystemFiles.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkSkipSystemFiles.AutoSize = true;
            chkSkipSystemFiles.Checked = true;
            chkSkipSystemFiles.CheckState = CheckState.Checked;
            chkSkipSystemFiles.Location = new Point(795, 49);
            chkSkipSystemFiles.Name = "chkSkipSystemFiles";
            chkSkipSystemFiles.Size = new Size(173, 29);
            chkSkipSystemFiles.TabIndex = 21;
            chkSkipSystemFiles.Text = "Skip System Files";
            chkSkipSystemFiles.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1178, 771);
            Controls.Add(chkSkipSystemFiles);
            Controls.Add(btnSuggestBySize);
            Controls.Add(chkRecursive);
            Controls.Add(btnSelectAll);
            Controls.Add(btnLoadAll);
            Controls.Add(btnRemoveText);
            Controls.Add(txtRemoveText);
            Controls.Add(lblRemoveText);
            Controls.Add(btnShowSimilar);
            Controls.Add(btnCleanupDuplicates);
            Controls.Add(btnDelete);
            Controls.Add(lblStatus);
            Controls.Add(progressBar);
            Controls.Add(chkPreview);
            Controls.Add(lblPattern);
            Controls.Add(txtPattern);
            Controls.Add(btnRename);
            Controls.Add(lvFiles);
            Controls.Add(btnCancel);
            Controls.Add(btnScan);
            Controls.Add(btnBrowse);
            Controls.Add(txtFolder);
            Name = "Form1";
            Text = "Duplicate File Finder";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}

