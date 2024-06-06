using CommunityToolkit.Mvvm.ComponentModel;

namespace DesktopOrganizer.Model
{
    public partial class UIElementNames : ObservableObject
    {
        [ObservableProperty]
        public string settingsString;

        [ObservableProperty]
        public string targetFolderString;

        [ObservableProperty]
        public string extensionsString;

        [ObservableProperty]
        public string browseString;

        [ObservableProperty]
        public string organizeString;

        [ObservableProperty]
        public string autoOrganizeString;

        [ObservableProperty]
        public string folderNameString;

        [ObservableProperty]
        public string changeLangString;

        [ObservableProperty]
        public string organizeAudio, organizeProgram, organizeImage
            , organizeDocument, organizeCompressed;

        public UIElementNames()
        {
            settingsString = Properties.Resources.Settings;
            targetFolderString = Properties.Resources.TargetFolder;
            extensionsString = Properties.Resources.Extensions;
            browseString = Properties.Resources.Browse;
            organizeString = Properties.Resources.Organize;
            autoOrganizeString = Properties.Resources.AutoOrganize;
            folderNameString = Properties.Resources.FolderName;
            changeLangString = Properties.Resources.ChangeLanguage;

            organizeAudio = Properties.Resources.OrganizeAudio;
            organizeProgram = Properties.Resources.OrganizePrograms;
            organizeImage = Properties.Resources.OrganizeImages;
            organizeDocument = Properties.Resources.OrganizeDocuments;
            organizeCompressed = Properties.Resources.OrganizeCompressed;
        }
    }
}
