using Windows.Storage;

namespace TheManXS.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            string folderPath = ApplicationData.Current.LocalFolder.Path;
            LoadApplication(new TheManXS.App(new PathList(folderPath)));
        }
    }
}
