using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace DesktopOrganizerWPF
{

    public partial class FolderOrganizer : ObservableObject
    {
        [ObservableProperty]
        public bool organize;

        [ObservableProperty]
        public string folderName;

        [ObservableProperty]
        public List<Extension> extensions = new List<Extension>();

        [ObservableProperty]
        public OrganizerType type;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum OrganizerType
        {
            audio,
            program,
            image,
            document,
            compressed
        }

        public FolderOrganizer(OrganizerType type)
        {
            FileExtensions fileExtensionNames = new FileExtensions();
            this.type = type;

            switch (type)
            {
                case OrganizerType.audio:
                    extensions.Add(fileExtensionNames.mp3);
                    extensions.Add(fileExtensionNames.wav);
                    extensions.Add(fileExtensionNames.m4a);
                    break;
                case OrganizerType.program:
                    extensions.Add(fileExtensionNames.lnk);
                    extensions.Add(fileExtensionNames.exe);
                    break;
                case OrganizerType.image:
                    extensions.Add(fileExtensionNames.png);
                    extensions.Add(fileExtensionNames.jpg);
                    break;
                case OrganizerType.document:
                    extensions.Add(fileExtensionNames.pdf);
                    extensions.Add(fileExtensionNames.docx);
                    extensions.Add(fileExtensionNames.xml);
                    extensions.Add(fileExtensionNames.xlsx);
                    extensions.Add(fileExtensionNames.xlsm);
                    break;
                case OrganizerType.compressed:
                    extensions.Add(fileExtensionNames.rar);
                    extensions.Add(fileExtensionNames.zip);
                    extensions.Add(fileExtensionNames.sevenzip);
                    break;
            }

        }

        [JsonConstructor]
        public FolderOrganizer(bool Organize, string FolderName, List<Extension> Extensions,OrganizerType Type)
        {
            this.Organize = Organize;
            this.FolderName = FolderName;
            this.Extensions = Extensions ?? new List<Extension>();
            type = Type;
        }
    }
    public partial class Extension : ObservableObject
    {
        [ObservableProperty]
        public string extensionName;

        [ObservableProperty]
        public bool isSelected;

        public Extension(string name)
        {
            extensionName = name;
        }
        [JsonConstructor]
        public Extension(string ExtensionName,bool IsSelected)
        {
            this.ExtensionName = ExtensionName;
            this.IsSelected = IsSelected;
        }
    }
    public class FileExtensions
    {
        public Extension mp3 = new Extension(".mp3");       
        public Extension wav = new Extension(".wav");       
        public Extension m4a = new Extension(".m4a");    
        
        public Extension exe = new Extension(".exe");       
        public Extension lnk = new Extension(".lnk");  
        
        public Extension png = new Extension(".png");       
        public Extension jpg = new Extension(".jpg"); 
        
        public Extension pdf = new Extension(".pdf");       
        public Extension docx = new Extension(".docx");       
        public Extension xml = new Extension(".xml");       
        public Extension xlsx = new Extension(".xlsx");       
        public Extension xlsm = new Extension(".xlsm"); 
        
        public Extension rar = new Extension(".rar");       
        public Extension zip = new Extension(".zip");       
        public Extension sevenzip = new Extension(".7z");       
    }

}

