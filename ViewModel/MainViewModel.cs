using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace DesktopOrganizerWPF.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string targetFolder;

        [ObservableProperty]
        private bool autoOrganize;

        [ObservableProperty]
        private List<FolderOrganizer> foldersToOrganize = new List<FolderOrganizer>();

        [ObservableProperty]
        private FolderOrganizer audioOrganizer, programOrganizer, imageOrganizer
            ,documentOrganizer, compressedOrganizer;

        private string targetFolderJsonFilePath = "targetfolder.json"; 
        private string settingsJsonFilePath = "organizersettings.json"; 

        public MainWindowViewModel()
        {
            if (File.Exists(settingsJsonFilePath))
            {
                string settingsString = File.ReadAllText(settingsJsonFilePath);
                FoldersToOrganize = JsonSerializer.Deserialize<List<FolderOrganizer>>(settingsString);

                audioOrganizer = FoldersToOrganize.Find(fo => fo.Type == FolderOrganizer.OrganizerType.audio);
                programOrganizer = FoldersToOrganize.Find(fo => fo.Type == FolderOrganizer.OrganizerType.program);
                imageOrganizer = FoldersToOrganize.Find(fo => fo.Type == FolderOrganizer.OrganizerType.image);
                documentOrganizer = FoldersToOrganize.Find(fo => fo.Type == FolderOrganizer.OrganizerType.document);
                compressedOrganizer = FoldersToOrganize.Find(fo => fo.Type == FolderOrganizer.OrganizerType.compressed);
            }
            else
            {
                audioOrganizer = new FolderOrganizer(FolderOrganizer.OrganizerType.audio);
                programOrganizer = new FolderOrganizer(FolderOrganizer.OrganizerType.program);
                imageOrganizer = new FolderOrganizer(FolderOrganizer.OrganizerType.image);
                documentOrganizer = new FolderOrganizer(FolderOrganizer.OrganizerType.document);
                compressedOrganizer = new FolderOrganizer(FolderOrganizer.OrganizerType.compressed);

                FoldersToOrganize.Add(audioOrganizer);
                FoldersToOrganize.Add(programOrganizer);
                FoldersToOrganize.Add(imageOrganizer);
                FoldersToOrganize.Add(documentOrganizer);
                FoldersToOrganize.Add(compressedOrganizer);
            }

            if (File.Exists(targetFolderJsonFilePath)) 
            {
                string targetString = File.ReadAllText(targetFolderJsonFilePath);
                TargetFolder = JsonSerializer.Deserialize<string>(targetString);
            }
            else
            {
                TargetFolder = "Welcome! Please Select A Target Folder";
            }

            Application.Current.MainWindow.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            string targetFolder = JsonSerializer.Serialize(TargetFolder,options);
            string settings = JsonSerializer.Serialize(FoldersToOrganize, options);

            File.WriteAllText(targetFolderJsonFilePath, targetFolder);
            File.WriteAllText(settingsJsonFilePath, settings);
        }

        private void RenameFolder(string oldFolderName, string newFolderName)
        {
            string oldPath = Path.Combine(TargetFolder, oldFolderName);
            string newPath = Path.Combine(TargetFolder, newFolderName);

            if (Directory.Exists(oldPath) && !string.IsNullOrEmpty(newFolderName) && oldPath != newPath)
            {
                Directory.Move(oldPath, newPath);
            }
        }
        [RelayCommand]
        public void ChooseFolder()
        {
            var folderName = OpenFolderDialog("Select Target Folder");
            if (!string.IsNullOrEmpty(folderName))
            {
                TargetFolder = folderName;
                System.Diagnostics.Debug.WriteLine(TargetFolder);
            }
        }
        [RelayCommand]
        public void OrganizeFiles()
        {
            if (string.IsNullOrEmpty(TargetFolder) || !Directory.Exists(TargetFolder))
            {
                MessageBox.Show("Please select a valid target folder.");
                return;
            }
            foreach (var folder in FoldersToOrganize)
            {
                if(folder.Organize)
                {
                    OrganizeFilesByExtension(folder.Extensions.ToArray(), folder.FolderName);
                }
            }

            MessageBox.Show("Files organized successfully!");
        }
        private void OrganizeFilesByExtension(string[] extensions, string folderName)
        {
            string targetPath = Path.Combine(TargetFolder, folderName);

            if(!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }        

            var files = Directory.GetFiles(TargetFolder);

            foreach (var file in files)
            {
                if (extensions.Contains(Path.GetExtension(file)))
                {
                    string destinationPath = Path.Combine(targetPath, Path.GetFileName(file));
                    File.Move(file, destinationPath);
                }
            }
        }
        public string OpenFolderDialog(string title)
        {
            var dialog = new OpenFileDialog
            {
                Title = title,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Folder Selection."
            };

            if (dialog.ShowDialog() == true)
            {
                return Path.GetDirectoryName(dialog.FileName);
            }

            return null;
        }
    }
}

