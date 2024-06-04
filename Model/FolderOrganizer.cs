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
        public List<string> extensions = new List<string>();

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
            FileExtensions fileExtensions = new FileExtensions();
            this.type = type;

            switch (type)
            {
                case OrganizerType.audio:
                    extensions.Add(fileExtensions.mp3);
                    extensions.Add(fileExtensions.wav);
                    extensions.Add(fileExtensions.m4a);
                    break;
                case OrganizerType.program:
                    extensions.Add(fileExtensions.lnk);
                    extensions.Add(fileExtensions.exe);
                    break;
                case OrganizerType.image:
                    extensions.Add(fileExtensions.png);
                    extensions.Add(fileExtensions.jpg);
                    break;
                case OrganizerType.document:
                    extensions.Add(fileExtensions.pdf);
                    extensions.Add(fileExtensions.docx);
                    extensions.Add(fileExtensions.xml);
                    extensions.Add(fileExtensions.xlsx);
                    break;
                case OrganizerType.compressed:
                    extensions.Add(fileExtensions.rar);
                    extensions.Add(fileExtensions.zip);
                    extensions.Add(fileExtensions.sevenzip);
                    break;
            }

        }
        [JsonConstructor]
        public FolderOrganizer(bool Organize, string FolderName, List<string> Extensions,OrganizerType Type)
        {
            this.Organize = Organize;
            this.FolderName = FolderName;
            this.Extensions = Extensions ?? new List<string>();
            type = Type;
        }
        public void AddExtension(string ex)
        {
            if(!Extensions.Contains(ex))
            {
                Extensions.Add(ex);
            }
        }
        public void RemoveExtension(string ex)
        {
            if (Extensions.Contains(ex))
            {
                Extensions.Remove(ex);
            }
        }
    }
    public class Extensions
    {
        public List<string> availableExtensions { get; set; }
        public bool IsSelected { get; set; }
    }
    public class FileExtensions
    {
        public string mp3 = ".mp3";
        public string wav = ".wav";
        public string m4a = ".m4a";

        public string exe = ".exe";
        public string lnk = ".lnk";

        public string png = ".png";
        public string jpg = ".jpg";

        public string pdf = ".pdf";
        public string docx = ".docx";
        public string xml = ".xml";
        public string xlsx = ".xlsx";
        
        public string rar = ".rar";
        public string zip = ".zip";
        public string sevenzip = ".7z";
    }

}

