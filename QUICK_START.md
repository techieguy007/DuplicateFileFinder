# Quick Start Guide - New Features

## 🚀 Progress Bar & Speed Indicator

**What's New:**
- Real-time progress showing files processed and speed (files/sec)
- Total scan time displayed when complete
- Progress bar updates smoothly without freezing UI

**Example Output:**
```
Processing: 1234/5000 (24%) - 42.5 files/sec
Found 156 files in duplicates. Scan completed in 117.45 seconds.
```

---

## ⚡ Multi-Threading (Faster Scans)

**What's New:**
- Automatic parallel processing on all CPU cores
- 4-8x faster scanning on modern computers
- Cancellable at any time with the new Cancel button

**How It Works:**
- 1 system with 4 cores = scan ~4 folders simultaneously
- 1 system with 8 cores = scan ~8 folders simultaneously
- Your UI stays responsive throughout

**Cancel Button:**
- Click "Cancel" to stop scanning anytime
- Gracefully stops all operations
- No data loss or corruption

---

## 📋 Context Menu (Right-Click Options)

**Access:** Right-click any file in the results list

**Available Options:**

### 1. **Open File**
- Double-click or right-click → "Open File"
- Opens file with its default application
- Ex: .txt with Notepad, .jpg with Photo Viewer

### 2. **Open File Location** 
- Opens Windows Explorer at the file's directory
- Highlights the file in the folder view
- Useful for quick navigation

### 3. **Copy Path**
- Copies the full file path to clipboard
- Can be pasted into emails, scripts, terminal, etc.
- Example: `C:\Users\John\Documents\photo.jpg`

### 4. **Rename** (Single File)
- Shows a dialog to rename the selected file
- Only works on one file at a time
- Cannot rename to a name that already exists

### 5. **Delete** (Multiple Files Supported)
- Shows confirmation dialog before deletion
- Can delete multiple selected files at once
- Action cannot be undone!
- Removes deleted files from results list

---

## 🔄 Batch Rename Pattern (Existing Feature Enhanced)

**How to Use:**
1. Select multiple files from the list
2. Enter rename pattern: `{name}_dup{n}`
3. Check "Preview" to see the changes first
4. Click "Rename" to apply

**Pattern Variables:**
- `{name}` = Original filename (without extension)
- `{n}` = Sequential number (1, 2, 3, ...)
- Example: `photo.jpg` → `photo_dup1.jpg`, `photo_dup2.jpg`

---

## 🎯 Button Reference

| Button | Function | When Available |
|--------|----------|-----------------|
| **Browse** | Select folder to scan | Always, except during scan |
| **Scan** | Start duplicate detection | Always, except during scan |
| **Cancel** | Stop current scan | Only during active scan |
| **Rename** | Batch rename selected files | When files are selected (not during scan) |

---

## 💡 Pro Tips

1. **Large Folder Scans:**
   - Watch the progress bar with speed indicator
   - Typical speed: 40-100 files/sec depending on disk type
   - SSD = faster, HDD = slower

2. **Quick Cleanup:**
   - Right-click → Delete for immediate removal
   - Select multiple files → use Batch Rename

3. **Safe Operations:**
   - Use "Preview" mode before actual rename
   - Confirmation dialog before deletion
   - Files in use are skipped safely

4. **Integration:**
   - Copy paths for use in scripts
   - Open file location to verify duplicates manually
   - Open file to inspect content

---

## ⚠️ Important Notes

- **Deletion is permanent** - No undo, no recycle bin by default
- **Multi-threading speeds up scanning** by utilizing all CPU cores
- **Cancellation is safe** - Can stop anytime without data corruption
- **Progress speed varies** based on disk speed and file sizes
- **Context menu is always available** after scan completes

---

## Example Workflow

```
1. Click "Browse" → Select C:\Downloads
2. Click "Scan" → Watch progress bar (42.5 files/sec)
3. Wait for completion → 156 duplicates found in 120 seconds
4. Right-click duplicate → "Open File Location" to verify
5. Multi-select duplicates → "Delete" → Confirm → Done!
```

---

Need help? All operations have error dialogs that explain what went wrong!
