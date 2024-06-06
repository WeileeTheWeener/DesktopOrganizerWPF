using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.IO;
using System.Security.Permissions;
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

        [ObservableProperty]
        public string extensionCheckBoxText = "Extensions";

        private string settingsJsonFileName = "organizer_settings.json";

        private string welcomeString = "Welcome! Please Select A Target Folder";

        FileSystemWatcher fileSystemWatcher;

        public MainWindowViewModel()
        {
            fileSystemWatcher = new FileSystemWatcher();

            if (File.Exists(settingsJsonFileName))
            {
                string[] allStringLines = File.ReadAllLines(settingsJsonFileName);
                string[] settingsStringLines = allStringLines.Skip(2).ToArray();
                string settingsString = string.Join(Environment.NewLine, settingsStringLines);
                FoldersToOrganize = JsonSerializer.Deserialize<List<FolderOrganizer>>(settingsString);
                AutoOrganize = JsonSerializer.Deserialize<bool>(allStringLines[1]);

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
            if (File.Exists(settingsJsonFileName)) 
            {
                string[] targetString = File.ReadAllLines(settingsJsonFileName);

                if (targetString[0] != "null" && targetString[0].Trim('"') != welcomeString.Trim('"'))
                {
                    TargetFolder = JsonSerializer.Deserialize<string>(targetString[0]);
                    fileSystemWatcher.Path = TargetFolder;
                }
                else
                {
                    TargetFolder = welcomeString;
                }
            }
            else
            {
                TargetFolder = welcomeString;
            }

            Application.Current.MainWindow.Closed += MainWindow_Closed;

            fileSystemWatcher.EnableRaisingEvents = true;
            fileSystemWatcher.IncludeSubdirectories = false;
            fileSystemWatcher.Created += FileSystemWatcher_Created;
            fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes |
                                                NotifyFilters.CreationTime |
                                                NotifyFilters.FileName |
                                                NotifyFilters.LastAccess |
                                                NotifyFilters.LastWrite |
                                                NotifyFilters.Size |
                                                NotifyFilters.Security;

        }
        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            //MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;
            //MessageBox.Show($"File created: {e.Name}","title",MessageBoxButton.OK,MessageBoxImage.None,MessageBoxResult.None,options);

            if (AutoOrganize)
            {
                OrganizeFiles();
            }
        }
        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;

            string targetFolder = JsonSerializer.Serialize(TargetFolder,options);
            string autoOrganize = JsonSerializer.Serialize(AutoOrganize, options);
            string settings = JsonSerializer.Serialize(FoldersToOrganize, options);

            string jsonString = targetFolder + "\n" + autoOrganize + "\n" + settings;

            File.WriteAllText(settingsJsonFileName, jsonString);
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
            if(FoldersToOrganize.Where(ext => ext.Organize).Any())
            {
                foreach (var folder in FoldersToOrganize)
                {
                    if (folder.Organize)
                    {
                        //OrganizeFilesByExtension(folder.Extensions.ToArray(), folder.FolderName);
                        OrganizeFilesByExtension(folder.Extensions.Where(ext => ext.IsSelected)
                            .Select(ext => ext.ExtensionName).ToArray(), folder.FolderName);
                    }
                }

                MessageBox.Show("Files organized successfully!");
            }
            else
            {
                MessageBox.Show("Please select atleast one category for organization.");
            }
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

                    if(!File.Exists(destinationPath))
                    {
                        File.Move(file, destinationPath);
                    }
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

