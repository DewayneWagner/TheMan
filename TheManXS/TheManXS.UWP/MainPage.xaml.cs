using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TheManXS.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            string sqLiteDBName = "TheManXS_db.sqlite";
            string folderPath = ApplicationData.Current.LocalFolder.Path;
            string sqLiteFullPath = Path.Combine(folderPath, sqLiteDBName);

            string parameterBoundedFileName = "ParameterBounded.bin";
            string parameterConstantFileName = "ParameterConstant.bin";

            string boundedParameter = Path.Combine(folderPath, parameterBoundedFileName);
            string constantParameter = Path.Combine(folderPath, parameterConstantFileName);

            LoadApplication(new TheManXS.App(sqLiteFullPath,boundedParameter,constantParameter));
        }
    }
}
