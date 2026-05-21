using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace DuplicateFileFinder
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource? _cancellationTokenSource;
        private Stopwatch? _scanStopwatch;
        private Dictionary<string, List<string>>? _lastScanResults;

        public Form1()
        {
            InitializeComponent();
            InitializeContextMenu();
            // Enable checkboxes in ListView
            lvFiles.CheckBoxes = true;
            // Load and set the application icon
            LoadApplicationIcon();
        }

        private void LoadApplicationIcon()
        {
            try
            {
                string appDir = AppDomain.CurrentDomain.BaseDirectory;
                string icoPath = Path.Combine(appDir, "app.ico");

                if (File.Exists(icoPath))
                {
                    try
                    {
                        this.Icon = new Icon(icoPath);
                        System.Diagnostics.Debug.WriteLine("✓ Icon loaded from app.ico");
                        return;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to load app.ico: {ex.Message}");
                    }
                }

                // Fallback if app.ico not found
                this.Icon = SystemIcons.Application;
                System.Diagnostics.Debug.WriteLine("Using system application icon as fallback");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Icon load error: {ex.Message}");
                try
                {
                    this.Icon = SystemIcons.Application;
                }
                catch { /* Silent fail */ }
            }
        }

        private Icon CreatePlaceholderIcon()
        {
            // Create a simple blue square icon as placeholder
            // This demonstrates the icon loading mechanism
            Bitmap bitmap = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // Blue gradient background
                using (var brush = new SolidBrush(Color.FromArgb(37, 99, 235))) // #2563EB
                {
                    g.FillRectangle(brush, 0, 0, 32, 32);
                }

                // White circle in center (representing file/duplicate concept)
                using (var brush = new SolidBrush(Color.White))
                {
                    g.FillEllipse(brush, 8, 8, 16, 16);
                }

                // Orange circle (representing magnifying glass)
                using (var pen = new Pen(Color.FromArgb(245, 158, 11), 2)) // #F59E0B
                {
                    g.DrawEllipse(pen, 10, 10, 12, 12);
                }
            }

            IntPtr hIcon = bitmap.GetHicon();
            Icon icon = Icon.FromHandle(hIcon);
            return icon;
        }

        private void InitializeContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Open File", null, (s, e) => ContextMenu_OpenFile());
            contextMenu.Items.Add("Open File Location", null, (s, e) => ContextMenu_OpenFileLocation());
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("Copy Path", null, (s, e) => ContextMenu_CopyPath());
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("Rename", null, (s, e) => ContextMenu_Rename());
            contextMenu.Items.Add("Delete", null, (s, e) => ContextMenu_Delete());
            lvFiles.ContextMenuStrip = contextMenu;
        }

        private void ContextMenu_OpenFile()
        {
            if (lvFiles.SelectedItems.Count == 0) return;
            var path = lvFiles.SelectedItems[0].Tag as string;
            if (path != null && File.Exists(path))
            {
                try
                {
                    Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ContextMenu_OpenFileLocation()
        {
            if (lvFiles.SelectedItems.Count == 0) return;
            var path = lvFiles.SelectedItems[0].Tag as string;
            if (path != null && File.Exists(path))
            {
                try
                {
                    Process.Start(new ProcessStartInfo("explorer.exe", $"/select,\"{path}\""));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ContextMenu_CopyPath()
        {
            if (lvFiles.SelectedItems.Count == 0) return;
            var path = lvFiles.SelectedItems[0].Tag as string;
            if (path != null)
            {
                Clipboard.SetText(path);
                MessageBox.Show("Path copied to clipboard.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ContextMenu_Rename()
        {
            if (lvFiles.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a file to rename.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var path = lvFiles.SelectedItems[0].Tag as string;
            if (path == null || !File.Exists(path)) return;

            var fi = new FileInfo(path);
            var newName = PromptForNewName(fi.Name);
            if (newName == null) return;

            try
            {
                var newPath = Path.Combine(Path.GetDirectoryName(path)!, newName);
                File.Move(path, newPath);
                lvFiles.SelectedItems[0].SubItems[0].Text = newName;
                lvFiles.SelectedItems[0].SubItems[1].Text = newPath;
                lvFiles.SelectedItems[0].Tag = newPath;
                MessageBox.Show("File renamed successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error renaming file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ContextMenu_Delete()
        {
            if (lvFiles.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select files to delete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Delete {lvFiles.SelectedItems.Count} file(s)? This cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes) return;

            var errors = new List<string>();
            var itemsToRemove = new List<ListViewItem>();

            foreach (ListViewItem item in lvFiles.SelectedItems)
            {
                var path = item.Tag as string;
                if (path == null) continue;

                try
                {
                    File.Delete(path);
                    itemsToRemove.Add(item);
                }
                catch (Exception ex)
                {
                    errors.Add($"{Path.GetFileName(path)}: {ex.Message}");
                }
            }

            foreach (var item in itemsToRemove)
            {
                lvFiles.Items.Remove(item);
            }

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Files deleted successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string? PromptForNewName(string currentName)
        {
            var form = new Form
            {
                Text = "Rename File",
                Width = 400,
                Height = 150,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var label = new Label { Text = "New name:", Left = 20, Top = 20, Width = 360 };
            var textBox = new TextBox { Text = currentName, Left = 20, Top = 50, Width = 360 };
            var okButton = new Button { Text = "OK", Left = 200, Top = 80, Width = 80, DialogResult = DialogResult.OK };
            var cancelButton = new Button { Text = "Cancel", Left = 290, Top = 80, Width = 80, DialogResult = DialogResult.Cancel };

            form.Controls.Add(label);
            form.Controls.Add(textBox);
            form.Controls.Add(okButton);
            form.Controls.Add(cancelButton);
            form.AcceptButton = okButton;
            form.CancelButton = cancelButton;

            return form.ShowDialog() == DialogResult.OK ? textBox.Text : null;
        }

        private void btnBrowse_Click(object? sender, EventArgs e)
        {
            using var d = new FolderBrowserDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                txtFolder.Text = d.SelectedPath;
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                lblStatus.Text = "Cancelling scan...";
            }
        }

        private async void btnScan_Click(object? sender, EventArgs e)
        {
            lvFiles.Items.Clear();
            var folder = txtFolder.Text;
            if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
            {
                MessageBox.Show("Please select a valid folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnScan.Enabled = false;
            btnRename.Enabled = false;
            btnBrowse.Enabled = false;
            progressBar.Value = 0;

            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;
            _scanStopwatch = Stopwatch.StartNew();

            try
            {
                // Run on thread pool to avoid blocking UI
                var results = await Task.Run(
                    () => ScanFolderMultiThreaded(folder, chkRecursive.Checked, cancellationToken),
                    cancellationToken
                );

                if (!cancellationToken.IsCancellationRequested)
                {
                    DisplayResults(results);
                }
                else
                {
                    lblStatus.Text = "Scan cancelled.";
                }
            }
            catch (OperationCanceledException)
            {
                lblStatus.Text = "Scan cancelled.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during scan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Scan failed.";
            }
            finally
            {
                btnScan.Enabled = true;
                btnRename.Enabled = lvFiles.Items.Count > 0;
                btnBrowse.Enabled = true;
                _scanStopwatch?.Stop();
            }
        }

        private Dictionary<string, List<string>> ScanFolderMultiThreaded(string folder, bool recursive, CancellationToken cancellationToken)
        {
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var files = Directory.GetFiles(folder, "*", searchOption);
            var total = files.Length;
            var dict = new Dictionary<string, List<string>>();
            var lockObj = new object();
            int processed = 0;

            // Use Parallel.ForEach for multi-threaded processing
            var parallelOptions = new ParallelOptions
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            try
            {
                Parallel.ForEach(files, parallelOptions, (file) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    try
                    {
                        var fi = new FileInfo(file);
                        var hash = ComputeHash(file);
                        var key = $"{hash}:{fi.Length}";

                        lock (lockObj)
                        {
                            if (!dict.TryGetValue(key, out var list))
                            {
                                list = new List<string>();
                                dict[key] = list;
                            }
                            list.Add(file);

                            processed++;
                            if (total > 0)
                            {
                                var percentage = Math.Min(100, processed * 100 / total);
                                var speed = _scanStopwatch?.Elapsed.TotalSeconds > 0
                                    ? (processed / _scanStopwatch.Elapsed.TotalSeconds).ToString("F1")
                                    : "0";

                                // Update UI on main thread
                                Invoke((Action)(() =>
                                {
                                    progressBar.Value = percentage;
                                    lblStatus.Text = $"Processing: {processed}/{total} ({percentage}%) - {speed} files/sec";
                                }));
                            }
                        }
                    }
                    catch { }
                });
            }
            catch (OperationCanceledException)
            {
                throw;
            }

            return dict;
        }

        private void DisplayResults(Dictionary<string, List<string>> results)
        {
            _lastScanResults = results;
            foreach (var kv in results.Where(kv => kv.Value.Count > 1))
            {
                foreach (var path in kv.Value)
                {
                    var fi = new FileInfo(path);
                    var item = new ListViewItem(new[] { "", fi.Name, fi.FullName, fi.Length.ToString(), kv.Key.Split(':')[0] });
                    item.Tag = path;
                    lvFiles.Items.Add(item);
                }
            }

            lblStatus.Text = $"Found {lvFiles.Items.Count} files in duplicates. Scan completed in {_scanStopwatch?.Elapsed.TotalSeconds:F2} seconds.";
            progressBar.Value = 100;
        }

        private string ComputeHash(string path)
        {
            try
            {
                using var sha = SHA256.Create();
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                var hash = sha.ComputeHash(fs);
                return Convert.ToHexString(hash);
            }
            catch
            {
                return string.Empty;
            }
        }

        private Dictionary<long, List<string>> FindDuplicatesBySize(string folder, bool recursive)
        {
            try
            {
                var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                var files = Directory.GetFiles(folder, "*", searchOption);
                var dict = new Dictionary<long, List<string>>();

                foreach (var file in files)
                {
                    try
                    {
                        var fi = new FileInfo(file);
                        if (!dict.TryGetValue(fi.Length, out var list))
                        {
                            list = new List<string>();
                            dict[fi.Length] = list;
                        }
                        list.Add(file);
                    }
                    catch { }
                }

                return dict;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error scanning folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new Dictionary<long, List<string>>();
            }
        }

        private void DisplaySizeSuggestions(Dictionary<long, List<string>> results)
        {
            lvFiles.Items.Clear();
            int totalDuplicates = 0;

            foreach (var kv in results.Where(kv => kv.Value.Count > 1))
            {
                foreach (var path in kv.Value)
                {
                    var fi = new FileInfo(path);
                    var sizeKB = (fi.Length / 1024.0).ToString("F2");
                    var item = new ListViewItem(new[] { "", fi.Name, fi.FullName, fi.Length.ToString(), sizeKB + " KB" });
                    item.Tag = path;
                    lvFiles.Items.Add(item);
                    totalDuplicates++;
                }
            }

            int identicalSizeGroups = results.Count(kv => kv.Value.Count > 1);
            lblStatus.Text = $"Found {totalDuplicates} files in {identicalSizeGroups} size groups (identical by size). Verify manually for actual duplicates.";
            progressBar.Value = 100;
        }

        private void btnRename_Click(object? sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select files to rename.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var pattern = txtPattern.Text;
            bool preview = chkPreview.Checked;
            var conflicts = new List<string>();

            foreach (ListViewItem item in lvFiles.SelectedItems)
            {
                var path = item.Tag as string;
                if (path == null) continue;
                var dir = Path.GetDirectoryName(path)!;
                var name = Path.GetFileNameWithoutExtension(path);
                var ext = Path.GetExtension(path);
                int n = 1;
                string newName;
                do
                {
                    newName = pattern.Replace("{name}", name).Replace("{n}", n.ToString()) + ext;
                    n++;
                } while (File.Exists(Path.Combine(dir, newName)));

                var newPath = Path.Combine(dir, newName);
                if (preview)
                {
                    item.SubItems[0].Text = newName;
                    item.SubItems[1].Text = newPath;
                }
                else
                {
                    try
                    {
                        File.Move(path, newPath);
                        item.SubItems[0].Text = newName;
                        item.SubItems[1].Text = newPath;
                        item.Tag = newPath;
                    }
                    catch (Exception ex)
                    {
                        conflicts.Add($"{path} -> {newPath}: {ex.Message}");
                    }
                }
            }

            if (conflicts.Count > 0)
            {
                MessageBox.Show(string.Join("\n", conflicts), "Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!preview)
            {
                MessageBox.Show("Rename completed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lvFiles_ItemChecked(object? sender, ItemCheckedEventArgs e)
        {
            // When checkbox is checked/unchecked, also select/highlight the item
            // This provides visual feedback and makes the item visible in the list
            if (e.Item != null)
            {
                e.Item.Selected = e.Item.Checked;
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            var checkedItems = lvFiles.CheckedItems;
            if (checkedItems.Count == 0)
            {
                MessageBox.Show("Please select files to delete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Delete {checkedItems.Count} file(s)? This cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes) return;

            var errors = new List<string>();
            var itemsToRemove = new List<ListViewItem>();

            foreach (ListViewItem item in checkedItems)
            {
                var path = item.Tag as string;
                if (path == null) continue;

                try
                {
                    File.Delete(path);
                    itemsToRemove.Add(item);
                }
                catch (Exception ex)
                {
                    errors.Add($"{Path.GetFileName(path)}: {ex.Message}");
                }
            }

            foreach (var item in itemsToRemove)
            {
                lvFiles.Items.Remove(item);
            }

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show($"{itemsToRemove.Count} file(s) deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCleanupDuplicates_Click(object? sender, EventArgs e)
        {
            if (lvFiles.Items.Count == 0)
            {
                MessageBox.Show("No duplicates to clean up. Run a scan first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show(
                "This will keep the first file and delete all others in each duplicate group. Continue?",
                "Cleanup All Duplicates",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes) return;

            var duplicateGroups = new Dictionary<string, List<ListViewItem>>();

            // Group items by hash
            foreach (ListViewItem item in lvFiles.Items)
            {
                var hash = item.SubItems[4].Text; // Hash is in column 4 (after Select, Name, Path, Size)
                if (!duplicateGroups.TryGetValue(hash, out var group))
                {
                    group = new List<ListViewItem>();
                    duplicateGroups[hash] = group;
                }
                group.Add(item);
            }

            var deletedCount = 0;
            var errors = new List<string>();
            var itemsToRemove = new List<ListViewItem>();

            foreach (var group in duplicateGroups.Values)
            {
                if (group.Count <= 1) continue; // Skip if not a duplicate

                // Keep the first file, delete the rest
                for (int i = 1; i < group.Count; i++)
                {
                    var path = group[i].Tag as string;
                    if (path == null) continue;

                    try
                    {
                        File.Delete(path);
                        itemsToRemove.Add(group[i]);
                        deletedCount++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"{Path.GetFileName(path)}: {ex.Message}");
                    }
                }
            }

            foreach (var item in itemsToRemove)
            {
                lvFiles.Items.Remove(item);
            }

            var message = $"Cleanup completed. Deleted {deletedCount} file(s).";
            if (errors.Count > 0)
            {
                message += $"\n\nErrors ({errors.Count}):\n" + string.Join("\n", errors.Take(5));
                if (errors.Count > 5)
                    message += $"\n... and {errors.Count - 5} more errors.";
                MessageBox.Show(message, "Cleanup Completed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnShowSimilar_Click(object? sender, EventArgs e)
        {
            if (lvFiles.Items.Count == 0)
            {
                MessageBox.Show("No files to compare. Run a scan first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            lvFiles.Items.Clear();

            // Collect all file names from last scan results
            var allFiles = new List<(string Path, string Name)>();
            foreach (ListViewItem item in lvFiles.Items)
            {
                var path = item.Tag as string;
                if (path != null)
                {
                    allFiles.Add((path, Path.GetFileNameWithoutExtension(path).ToLower()));
                }
            }

            // Find similar names
            var similarGroups = new Dictionary<string, List<string>>();
            for (int i = 0; i < allFiles.Count; i++)
            {
                for (int j = i + 1; j < allFiles.Count; j++)
                {
                    if (AreNamesSimilar(allFiles[i].Name, allFiles[j].Name))
                    {
                        var key = string.Concat(allFiles[i].Name.OrderBy(c => c));
                        if (!similarGroups.TryGetValue(key, out var group))
                        {
                            group = new List<string>();
                            similarGroups[key] = group;
                        }
                        if (!group.Contains(allFiles[i].Path))
                            group.Add(allFiles[i].Path);
                        if (!group.Contains(allFiles[j].Path))
                            group.Add(allFiles[j].Path);
                    }
                }
            }

            // Display similar files
            lvFiles.Items.Clear();
            if (similarGroups.Count == 0)
            {
                MessageBox.Show("No similar file names found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var group in similarGroups.Values)
            {
                foreach (var path in group)
                {
                    if (File.Exists(path))
                    {
                        var fi = new FileInfo(path);
                        var item = new ListViewItem(new[] { "", fi.Name, fi.FullName, fi.Length.ToString(), "Similar" });
                        item.Tag = path;
                        lvFiles.Items.Add(item);
                    }
                }
            }

            lblStatus.Text = $"Found {lvFiles.Items.Count} files with similar names in {similarGroups.Count} group(s).";
        }

        private bool AreNamesSimilar(string name1, string name2)
        {
            // Calculate similarity using a simple algorithm
            // Names are similar if they share at least 60% of characters or are anagrams
            var minLen = Math.Min(name1.Length, name2.Length);
            if (minLen == 0) return false;

            // Check if they're anagrams (same letters in different order)
            var sorted1 = string.Concat(name1.OrderBy(c => c));
            var sorted2 = string.Concat(name2.OrderBy(c => c));

            if (sorted1 == sorted2) return true;

            // Calculate Levenshtein distance (simple implementation)
            int distance = CalculateLevenshteinDistance(name1, name2);
            int maxDistance = Math.Max(name1.Length, name2.Length) / 2;

            return distance <= maxDistance;
        }

        private int CalculateLevenshteinDistance(string s1, string s2)
        {
            var len1 = s1.Length;
            var len2 = s2.Length;
            var d = new int[len1 + 1, len2 + 1];

            for (int i = 0; i <= len1; i++)
                d[i, 0] = i;
            for (int j = 0; j <= len2; j++)
                d[0, j] = j;

            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    int cost = s1[i - 1] == s2[j - 1] ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[len1, len2];
        }

        private void btnRemoveText_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRemoveText.Text))
            {
                MessageBox.Show("Please enter text to remove from filenames.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var textToRemove = txtRemoveText.Text.Trim();
            var selectedItems = new List<ListViewItem>();

            // Get selected items or use checked items
            if (lvFiles.SelectedItems.Count > 0)
            {
                selectedItems.AddRange(lvFiles.SelectedItems.Cast<ListViewItem>());
            }
            else
            {
                selectedItems.AddRange(lvFiles.CheckedItems.Cast<ListViewItem>());
            }

            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Please select or check files to remove text from.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var preview = new List<string>();
            foreach (var item in selectedItems)
            {
                var path = item.Tag as string;
                if (path == null || !File.Exists(path)) continue;

                var fi = new FileInfo(path);
                var newName = fi.Name.Replace(textToRemove, "");
                preview.Add($"{fi.Name} → {newName}");
            }

            // Show preview
            if (preview.Count > 0)
            {
                var previewText = string.Join(Environment.NewLine, preview);
                var result = MessageBox.Show(
                    $"The following {preview.Count} file(s) will be renamed:\n\n{previewText}\n\nContinue?",
                    "Preview - Remove Text",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;
            }

            // Perform the actual rename
            int successCount = 0;
            var errors = new List<string>();

            foreach (var item in selectedItems)
            {
                var path = item.Tag as string;
                if (path == null || !File.Exists(path)) continue;

                try
                {
                    var fi = new FileInfo(path);
                    var newName = fi.Name.Replace(textToRemove, "");

                    if (newName == fi.Name)
                    {
                        continue; // No change needed
                    }

                    var newPath = Path.Combine(Path.GetDirectoryName(path)!, newName);

                    // Check if target file already exists
                    if (File.Exists(newPath))
                    {
                        errors.Add($"{fi.Name}: Target filename already exists");
                        continue;
                    }

                    File.Move(path, newPath);

                    // Update ListView
                    item.SubItems[1].Text = newName;  // File Name column
                    item.SubItems[2].Text = newPath;  // Path column
                    item.Tag = newPath;

                    successCount++;
                }
                catch (Exception ex)
                {
                    errors.Add($"{Path.GetFileName(path)}: {ex.Message}");
                }
            }

            // Show results
            var resultMsg = $"Successfully removed text from {successCount} file(s).";
            if (errors.Count > 0)
            {
                resultMsg += $"\n\nErrors ({errors.Count}):\n" + string.Join("\n", errors.Take(5));
                if (errors.Count > 5)
                    resultMsg += $"\n... and {errors.Count - 5} more";
            }

            MessageBox.Show(resultMsg, "Remove Text - Results", MessageBoxButtons.OK,
                errors.Count > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

            lblStatus.Text = $"Removed text from {successCount} file(s).";
        }

        private void btnLoadAll_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFolder.Text))
            {
                MessageBox.Show("Please select a folder first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var folderPath = txtFolder.Text;
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("Folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                lvFiles.Items.Clear();
                progressBar.Value = 0;
                lblStatus.Text = "Loading files...";
                Application.DoEvents();

                var searchOption = chkRecursive.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                var files = Directory.GetFiles(folderPath, "*.*", searchOption);
                int totalFiles = files.Length;

                for (int i = 0; i < files.Length; i++)
                {
                    try
                    {
                        var fi = new FileInfo(files[i]);
                        var item = new ListViewItem(new[] { "", fi.Name, fi.FullName, fi.Length.ToString(), "" });
                        item.Tag = files[i];
                        lvFiles.Items.Add(item);

                        // Update progress
                        int percentage = (int)((i + 1) * 100 / totalFiles);
                        progressBar.Value = percentage;
                        lblStatus.Text = $"Loading: {i + 1}/{totalFiles} files";
                        Application.DoEvents();
                    }
                    catch { }
                }

                progressBar.Value = 100;
                lblStatus.Text = $"Loaded {lvFiles.Items.Count} files from '{Path.GetFileName(folderPath)}'";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error loading files.";
            }
        }

        private void btnSelectAll_Click(object? sender, EventArgs e)
        {
            if (lvFiles.Items.Count == 0)
            {
                MessageBox.Show("No files loaded.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool allChecked = lvFiles.Items.Cast<ListViewItem>().All(item => item.Checked);

            if (allChecked)
            {
                // Uncheck all
                foreach (ListViewItem item in lvFiles.Items)
                {
                    item.Checked = false;
                }
                lblStatus.Text = "All files unchecked.";
            }
            else
            {
                // Check all
                foreach (ListViewItem item in lvFiles.Items)
                {
                    item.Checked = true;
                }
                lblStatus.Text = $"All {lvFiles.Items.Count} files checked.";
            }
        }

        private void btnSuggestBySize_Click(object? sender, EventArgs e)
        {
            var folder = txtFolder.Text;
            if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
            {
                MessageBox.Show("Please select a valid folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnSuggestBySize.Enabled = false;
            progressBar.Value = 0;
            lblStatus.Text = "Scanning for files with identical sizes...";
            Application.DoEvents();

            try
            {
                _scanStopwatch = Stopwatch.StartNew();

                // Find files with identical sizes
                var results = FindDuplicatesBySize(folder, chkRecursive.Checked);

                _scanStopwatch.Stop();

                if (results.Count == 0)
                {
                    MessageBox.Show("No files found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = "Ready";
                    return;
                }

                // Display suggestions
                DisplaySizeSuggestions(results);

                int suggestions = results.Count(kv => kv.Value.Count > 1);
                lblStatus.Text += $" Completed in {_scanStopwatch?.Elapsed.TotalSeconds:F2} seconds.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error scanning files.";
            }
            finally
            {
                btnSuggestBySize.Enabled = true;
            }
        }
    }
}
