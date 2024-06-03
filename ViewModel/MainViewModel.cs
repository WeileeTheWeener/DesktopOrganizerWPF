using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace DesktopOrganizerWPF.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private string previousAudioFolderName = "Audio";
        private string previousProgramsFolderName = "Programs";
        private string previousImagesFolderName = "Images";
        private string previouDocumentsFolderName = "Documents";
        private string previouCompressedFolderName = "Compressed";

        [ObservableProperty]
        private string targetFolder;

        [ObservableProperty]
        private bool autoOrganize;

        [ObservableProperty]
        private bool organizeAudio;

        [ObservableProperty]
        private string audioFolderName;

        [ObservableProperty]
        private bool organizePrograms;

        [ObservableProperty]
        private string programsFolderName;

        [ObservableProperty]
        private bool organizeImages;

        [ObservableProperty]
        private string imagesFolderName;

        [ObservableProperty]
        private bool organizeDocuments;

        [ObservableProperty]
        private string documentsFolderName;

        [ObservableProperty]
        private bool organizeCompressed;

        [ObservableProperty]
        private string compressedFolderName;

        public MainWindowViewModel()
        {
            AudioFolderName = "Audio";
            ProgramsFolderName = "Programs";
            ImagesFolderName = "Images";
            DocumentsFolderName = "Documents";
            compressedFolderName = "Compressed";
        }
        partial void OnAudioFolderNameChanged(string value)
        {
            if (Directory.Exists(TargetFolder))
            {
                RenameFolder(previousAudioFolderName, value);
                previousAudioFolderName = value;
            }
        }
        partial void OnProgramsFolderNameChanged(string value)
        {
            if (Directory.Exists(TargetFolder))
            {
                RenameFolder(previousProgramsFolderName, value);
                previousProgramsFolderName = value;
            }
        }
        partial void OnImagesFolderNameChanged(string value)
        {
            if (Directory.Exists(TargetFolder))
            {
                RenameFolder(previousImagesFolderName, value);
                previousImagesFolderName = value;
            }
        }
        partial void OnDocumentsFolderNameChanged(string value)
        {
            if (Directory.Exists(TargetFolder))
            {
                RenameFolder(previouDocumentsFolderName, value);
                previouDocumentsFolderName = value;
            }
        }
        partial void OnCompressedFolderNameChanged(string value)
        {
            if (Directory.Exists(TargetFolder))
            {
                RenameFolder(previouCompressedFolderName, value);
                previouCompressedFolderName = value;
            }
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

            if (OrganizeAudio)
            {
                OrganizeFilesByExtension(new[] { ".mp3", ".wav" ,".m4a" }, AudioFolderName);
            }
            if (OrganizePrograms)
            {
                OrganizeFilesByExtension(new[] { ".exe", ".lnk"}, ProgramsFolderName);
            }
            if (OrganizeImages)
            {
                OrganizeFilesByExtension(new[] { ".png", ".jpg" }, ImagesFolderName);
            }
            if (OrganizeDocuments)
            {
                OrganizeFilesByExtension(new[] { ".pdf", ".docx", ".xml", ".xlsx" }, DocumentsFolderName);
            }
            if (OrganizeCompressed)
            {
                OrganizeFilesByExtension(new[] { ".rar", ".zip"}, CompressedFolderName);
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

