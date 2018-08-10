using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace knn_t
{
    class RecognitionFromMovie
    {
        public static void exec()
        {
            var fname = "base01.mp4";
            var maskname = "yellow_1bit_inversion.bmp";

            var videoCapture = VideoCapture.FromFile($@"image\{fname}");

            videoCapture.Set(CaptureProperty.FrameWidth, 1920);
            videoCapture.Set(CaptureProperty.FrameHeight, 1080);
            videoCapture.Set(CaptureProperty.FrameWidth, 1280);
            videoCapture.Set(CaptureProperty.FrameHeight, 720);

            var mat = new Mat();
            var mat_resize = new Mat();
            Mat yellow_hist, result_hist;
            if (videoCapture.IsOpened())
            {
                for (int i = 0; i < 10000; i++)
                {
                    var ret = videoCapture.Read(mat);

                    Cv2.Resize(mat, mat_resize, new Size(1280, 720));
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    var rect = new Rect(820, 30, 260, 80);
                    var startimg = new Mat(mat_resize, rect);

                    // 黄色の画素範囲
                    Scalar scalar_low = new Scalar(0, 240, 240);
                    Scalar scalar_high = new Scalar(20, 255, 255);

                    Mat yellow = new Mat();
                    Cv2.InRange(startimg, scalar_low, scalar_high, yellow);

                    var mask = new Mat($@"image\{maskname}", ImreadModes.GrayScale);
                    var result = new Mat();
                    Cv2.Add(yellow, mask, result);

                    result_hist = GetHistogram(result);
                    sw.Stop();
                    Console.WriteLine($"Elapsed:{sw.ElapsedMilliseconds}");
                    //Cv2.ImShow("movie", mat_resize);
                    Cv2.ImShow("result", result);
                    Cv2.ImShow("result_hist", result_hist);

                    Cv2.WaitKey(1);
                }
            }
        }

        static int cnt = 0;

        // Draw Histogram from source image - for gray
        private static Mat GetHistogram(Mat source)
        {
            Mat hist = new Mat();
            int width = source.Cols, height = source.Rows/*+500*/;      // set Histogram same size as source image
            const int histogramSize = 256;                      // you can change by urself
            int[] dimensions = { histogramSize };               // Histogram size for each dimension
            Rangef[] ranges = { new Rangef(0, histogramSize) }; // min/max

            Cv2.CalcHist(
                images: new[] { source },
                channels: new[] { 0 }, //The channel (dim) to be measured. In this case it is just the intensity (each array is single-channel) so we just write 0.
                mask: null,
                hist: hist,
                dims: 1, //The histogram dimensionality.
                histSize: dimensions,
                ranges: ranges);

            double minVal, maxVal;
            Cv2.MinMaxLoc(hist, out minVal, out maxVal);

            Mat render = new Mat(new Size(width, height), MatType.CV_8UC3, Scalar.All(255));
            Scalar color = Scalar.All(100);

            // Scales and draws histogram
            hist = hist * (maxVal != 0 ? height / maxVal : 0.0);
            int binW = width / dimensions[0];
            for (int j = 0; j < dimensions[0]; ++j)
            {
                //Console.WriteLine($@"j:{j} P1: {j * binW},{render.Rows} P2:{(j + 1) * binW},{render.Rows - (int)hist.Get<float>(j)}");  //for Debug
                var a = (int)hist.Get<float>(j);
                render.Rectangle(
                    new Point(j * binW, render.Rows - (int)hist.Get<float>(j)),
                    new Point((j + 1) * binW, render.Rows),
                    color,
                    -1);
            }

            var b = (int)hist.Get<float>(0);
            if (b < 700)
            {
                //Console.WriteLine($"baito start:{++cnt}");
                //source.SaveImage($@"image\start\{cnt}.bmp");
            }
            return render;
        }
    }
}
