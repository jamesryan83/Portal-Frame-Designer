using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System;
using System.Windows.Media;
using System.Linq;

namespace WpfMultiframeApp
{
    class DrawShedPreview
    {
        // Draw the shed in the drawing window thing
        public void DrawShed(HelixViewport3D view, Data data)
        {
            double toRadians = Math.PI/180;
            double apex = data.eaveHeight + ((data.span/2) * Math.Tan(data.roofPitch * toRadians));
            double highEave = data.eaveHeight + (data.span * Math.Tan(data.roofPitch * toRadians));
            double length = -data.baySpacing * data.numberOfBays;
            double z = 0;

            LinesVisual3D lines = new LinesVisual3D();
            
            lines.Thickness = 4;
            view.Children.Clear();
            view.Children.Add(new SunLight());

            // Draw portals
            for (int i = 0; i < (data.numberOfBays + 1); i++)
            {
                z = -data.baySpacing * i;

                lines.Points.Add(new Point3D(z, 0, 0));
                lines.Points.Add(new Point3D(z, 0, data.eaveHeight));

                if (data.roofType == "Gable")
                {
                    lines.Points.Add(new Point3D(z, 0, data.eaveHeight));
                    lines.Points.Add(new Point3D(z, (data.span / 2), apex));

                    lines.Points.Add(new Point3D(z, (data.span / 2), apex));
                    lines.Points.Add(new Point3D(z, data.span, data.eaveHeight));

                    lines.Points.Add(new Point3D(z, data.span, data.eaveHeight));
                    lines.Points.Add(new Point3D(z, data.span, 0));
                }
                else
                {
                    lines.Points.Add(new Point3D(z, 0, data.eaveHeight));
                    lines.Points.Add(new Point3D(z, data.span, highEave));

                    lines.Points.Add(new Point3D(z, data.span, highEave));
                    lines.Points.Add(new Point3D(z, data.span, 0));
                }
            }

            // Draw ridge and eave elements along building
            lines.Points.Add(new Point3D(0, 0, data.eaveHeight));
            lines.Points.Add(new Point3D(length, 0, data.eaveHeight));

            if (data.roofType == "Gable")
            {
                if (data.numberOfMullions == 0 || data.numberOfMullions == 2 || data.numberOfMullions == 4)
                {
                    lines.Points.Add(new Point3D(0, (data.span / 2), apex));
                    lines.Points.Add(new Point3D(length, (data.span / 2), apex));
                }

                lines.Points.Add(new Point3D(0, data.span, data.eaveHeight));
                lines.Points.Add(new Point3D(length, data.span, data.eaveHeight));
            }
            else
            {
                lines.Points.Add(new Point3D(0, data.span, highEave));
                lines.Points.Add(new Point3D(length, data.span, highEave));
            }


            // Draw knee braces
            double kneeX1 = 0, kneeY1 = 0, kneeX2 = 0, kneeY2 = 0, kneeX3 = 0, kneeY3 = 0, kneeX4 = 0, kneeY4 = 0;

            if (data.endKneeBraceType != "None" || data.midKneeBraceType != "None")
            {
                kneeX1 = 0;
                kneeY1 = data.eaveHeight - (data.eaveHeight * (data.kneeBracePercentEave / 100));
                kneeX2 = data.span * data.kneeBracePercentSpan / 100;
                kneeY2 = data.eaveHeight + (Math.Tan(data.roofPitch * toRadians) * (data.kneeBracePercentSpan / 100) * data.span);
                kneeX3 = data.span - kneeX2;
                kneeY3 = data.roofType == "Gable" ? kneeY2 : highEave - ((data.kneeBracePercentSpan / 100) * data.span * Math.Tan(data.roofPitch * toRadians));
                kneeX4 = data.span;
                kneeY4 = data.roofType == "Gable" ? kneeY1 : highEave - (highEave * (data.kneeBracePercentEave / 100));
            }

            if (data.endKneeBraceType != "None")
            {
                lines.Points.Add(new Point3D(0, kneeX1, kneeY1));
                lines.Points.Add(new Point3D(0, kneeX2, kneeY2));
                lines.Points.Add(new Point3D(0, kneeX3, kneeY3));
                lines.Points.Add(new Point3D(0, kneeX4, kneeY4));
                lines.Points.Add(new Point3D(length, kneeX1, kneeY1));
                lines.Points.Add(new Point3D(length, kneeX2, kneeY2));
                lines.Points.Add(new Point3D(length, kneeX3, kneeY3));
                lines.Points.Add(new Point3D(length, kneeX4, kneeY4));
                
            }

            if (data.midKneeBraceType != "None")
            {
                for (int i = 1; i < data.numberOfBays; i++)
                {
                    lines.Points.Add(new Point3D(-i * data.baySpacing, kneeX1, kneeY1));
                    lines.Points.Add(new Point3D(-i * data.baySpacing, kneeX2, kneeY2));
                    lines.Points.Add(new Point3D(-i * data.baySpacing, kneeX3, kneeY3));
                    lines.Points.Add(new Point3D(-i * data.baySpacing, kneeX4, kneeY4));
                    lines.Points.Add(new Point3D(-i * data.baySpacing, kneeX1, kneeY1));
                    lines.Points.Add(new Point3D(-i * data.baySpacing, kneeX2, kneeY2));
                    lines.Points.Add(new Point3D(-i * data.baySpacing, kneeX3, kneeY3));
                    lines.Points.Add(new Point3D(-i * data.baySpacing, kneeX4, kneeY4));
                }
            }

            // Draw apex braces
            if (data.roofType == "Gable")
            {
                double apexX1 = 0, apexY1 = 0, apexX2 = 0, apexY2 = 0;

                if (data.endApexBraceType != "None" || data.midApexBraceType != "None")
                {
                    apexX1 = data.span * (1.0 / 3.0);
                    apexY1 = data.eaveHeight + ((data.span / 3) * Math.Tan(data.roofPitch * toRadians));
                    apexX2 = data.span * (2.0 / 3.0);
                    apexY2 = apexY1;
                }

                if (data.endApexBraceType != "None")
                {
                    lines.Points.Add(new Point3D(0, apexX1, apexY1));
                    lines.Points.Add(new Point3D(0, apexX2, apexY2));
                    lines.Points.Add(new Point3D(length, apexX1, apexY1));
                    lines.Points.Add(new Point3D(length, apexX2, apexY2));
                }

                if (data.midApexBraceType != "None")
                {
                    for (int i = 1; i < data.numberOfBays; i++)
                    {
                        lines.Points.Add(new Point3D(-i * data.baySpacing, apexX1, apexY1));
                        lines.Points.Add(new Point3D(-i * data.baySpacing, apexX2, apexY2));
                    }
                }
            }


            // Draw Mullions & compression struts
            if (data.numberOfMullions > 0)
            {
                double[] xCoords = new double[data.numberOfMullions];
                double[] yCoords = new double[data.numberOfMullions];

                // Get x, y coords
                for (int i = 0; i < data.numberOfMullions; i++)
                    xCoords[i] = (data.span / (data.numberOfMullions + 1)) * (i + 1);

                if (data.roofType == "Skillion")
                {
                    for (int i = 0; i < data.numberOfMullions; i++)
                        yCoords[i] = data.eaveHeight + (xCoords[i] * Math.Tan(data.roofPitch * toRadians));
                }
                else
                {
                    for (int i = 0; i < data.numberOfMullions; i++)
                    {
                        if (xCoords[i] <= (data.span / 2))
                            yCoords[i] = data.eaveHeight + (xCoords[i] * Math.Tan(data.roofPitch * toRadians));
                        else
                            yCoords[i] = (data.eaveHeight + (xCoords[i] * Math.Tan(data.roofPitch * toRadians))) - ((2 * Math.Tan(data.roofPitch * toRadians)) * ((data.span / 2) - (data.span - xCoords[i])));
                    }
                }

                // Draw mullions
                for (int i = 0; i < data.numberOfMullions; i++)
                {
                    lines.Points.Add(new Point3D(0, xCoords[i], 0));
                    lines.Points.Add(new Point3D(0, xCoords[i], yCoords[i]));
                    lines.Points.Add(new Point3D(length, xCoords[i], 0));
                    lines.Points.Add(new Point3D(length, xCoords[i], yCoords[i]));
                }

                // Draw Compression Struts
                for (int i = 0; i < data.numberOfMullions; i++)
                {
                    for (int j = 0; j < data.numberOfBays; j++)
                    {
                        lines.Points.Add(new Point3D(-data.baySpacing * j, xCoords[i], yCoords[i]));
                        lines.Points.Add(new Point3D(-data.baySpacing * (j + 1), xCoords[i], yCoords[i]));
                    }
                }
            }
            
            

            // Add lines to view and update default camera
            view.Children.Add(lines);
        }

        
    }
}
