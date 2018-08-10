using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace knn_t
{
    class TemplateMatching
    {
        public static void exec()
        {
            //var fname = "result_detail_capture_001.bmp";
            //var fname = "result_detail_movie_001.bmp";
            var fname = "result_detail_movie_002.bmp";
            //var template = "result_arrow_1bit_1st_movie.bmp";
            //var template = "result_arrow_1bit_2nd_1_movie.bmp";
            var template = "result_arrow_1bit_4th_capture.bmp";

            Mat color = new Mat($@"C:\tagwork\test\knn_t\testdata\{fname}");
            Mat tmp = new Mat($@"C:\tagwork\test\knn_t\testdata\{template}", ImreadModes.GrayScale);

            var sw = new Stopwatch();
            sw.Start();

            // 緑っぽいの(RGB=14/255/101)の画素範囲(個人の結果が書かれてるシーンの矢印検出用)
            Scalar scalar_low = new Scalar(50, 245, 0); // B,G,R いっつも忘れる
            Scalar scalar_high = new Scalar(120, 255, 30);

            Mat grayfromgreen = new Mat();
            //Cv2.InRange(color, scalar_low, scalar_high, yellow);
            Cv2.InRange(color, scalar_low, scalar_high, grayfromgreen);

            RunTemplateMatch(grayfromgreen, tmp);

            Cv2.WaitKey();
        }

        static void RunTemplateMatch(Mat reference, Mat tmp)
        {
            using (Mat refMat = reference)
            using (Mat tplMat = tmp)
            using (Mat res = new Mat(refMat.Rows - tplMat.Rows + 1, refMat.Cols - tplMat.Cols + 1, MatType.CV_32FC1))
            {
                //Convert input images to gray
                Mat gref = refMat.Clone();
                Mat gtpl = tplMat.Clone();

                Cv2.MatchTemplate(gref, gtpl, res, TemplateMatchModes.CCoeffNormed);
                Cv2.Threshold(res, res, 0.8, 1.0, ThresholdTypes.Tozero);

                while (true)
                {
                    double minval, maxval, threshold = 0.8;
                    Point minloc, maxloc;
                    Cv2.MinMaxLoc(res, out minval, out maxval, out minloc, out maxloc);

                    if (maxval >= threshold)
                    {
                        //Setup the rectangle to draw
                        Rect r = new Rect(new Point(maxloc.X, maxloc.Y), new Size(tplMat.Width, tplMat.Height));
                        Console.WriteLine($"MinVal={minval.ToString()} MaxVal={maxval.ToString()} MinLoc={minloc.ToString()} MaxLoc={maxloc.ToString()} Rect={r.ToString()}");

                        //Draw a rectangle of the matching area
                        Cv2.Rectangle(refMat, r, Scalar.White, 2);

                        //Fill in the res Mat so you don't find the same area again in the MinMaxLoc
                        Rect outRect;
                        Cv2.FloodFill(res, maxloc, new Scalar(0), out outRect, new Scalar(0.1), new Scalar(1.0), FloodFillFlags.Link4);
                    }
                    else
                        break;
                }

                Cv2.ImShow("Matches", refMat);
                Cv2.WaitKey();
            }
        }
    }
}
