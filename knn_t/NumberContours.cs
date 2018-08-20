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
                using (var gray_negaposi = new Mat())
                {
                    Cv2.Threshold(gray, gray, 245, 255, ThresholdTypes.Binary);
                    //Cv2.BitwiseNot(gray, gray_negaposi);

                    var points = Cv2.FindContoursAsArray(gray, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                    for (int i = 0; i < points.Length; i++)
                    {
                        gray.DrawContours(points, i, new Scalar(255, 255, 255), 5);
                    }

                    Cv2.ImShow("testttttttttttttttttttt", gray);
                    //Cv2.ImShow("gray_negaposiiiiiiiiiiiiiiiii", gray_negaposi);

                    break;
                }
            }

            Cv2.WaitKey();
        }
    }
}
