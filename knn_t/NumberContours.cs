using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace knn_t
{
    class NumberContours
    {
        public static void Exec()
        {
            foreach (var f in Directory.EnumerateFiles(@"C:\tagwork\test\salmon_testdata\result\Extracted\"))
            {
                using (var gray = new Mat(f, ImreadModes.GrayScale))
                using (var gray2 = gray.Clone())
                {
                    var contours = Cv2.FindContoursAsArray(gray2, RetrievalModes.External, ContourApproximationModes.ApproxNone);
                    Console.WriteLine($"{f}:{contours.Length:00}");

                    foreach (var points in contours)
                    {
                        var minPoint = new Point(points.Select(p => p.X).Min(), points.Select(p => p.Y).Min());
                        var maxPoint = new Point(points.Select(p => p.X).Max(), points.Select(p => p.Y).Max());

                        //gray.Rectangle(minPoint, maxPoint, new Scalar(255));
                        var rect = new Rect(minPoint.X, minPoint.Y, maxPoint.X - minPoint.X, maxPoint.Y - minPoint.Y);
                        var numImg = new Mat(gray, rect);
                        Cv2.ImWrite($@"C:\tagwork\test\salmon_testdata\result\Extracted\Numbers\{Path.GetRandomFileName()}.bmp", numImg);
                    }
                }
            }

            Cv2.WaitKey();
        }
    }
}
