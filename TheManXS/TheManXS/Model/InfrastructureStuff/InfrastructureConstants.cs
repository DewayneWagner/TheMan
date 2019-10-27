using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.InfrastructureStuff
{
    public class InfrastructureConstants
    {
        public static double RoadFromTopOfSQRatio = 0.4;
        public static double RailFromTopOfSQRatio = 0.5;
        public static double PipelineFromTopOfSQRatio = 0.6;

        public static Color RoadColor = Color.Black;
        public static Color RailColor = Color.Violet;
        public static Color PipeLineColor = Color.DarkSlateGray;

        public static double LengthStraight = ((QC.SqSize / 2)*1.1);
        public static double LengthLateral = Math.Sqrt(((LengthStraight) * (LengthStraight))
            + ((LengthStraight) * (LengthStraight)));
        public static double Width = QC.SqSize * 0.05;
        public static double CornerRadius = QC.SqSize * 0.025;

        
    }
}
