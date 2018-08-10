using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace knn_t
{
    class Histogram
    {
        public static void exec()
        {
            Mat src = Cv2.ImRead(@"image\lenna_grayscale.png", ImreadModes.GrayScale);

            // Histogram view
            const int Width = 260, Height = 200;
            Mat render = new Mat(new Size(Width, Height), MatType.CV_8UC3, Scalar.All(255));

            // Calculate histogram
            Mat hist = new Mat();
            int[] hdims = { 256 }; // Histogram size for each dimension
            Rangef[] ranges = { new Rangef(0, 256), }; // min/max 
            Cv2.CalcHist(
                new Mat[] { src },
                new int[] { 0 },
                null,
                hist,
                1,
                hdims,
                ranges);

            // Get the max value of histogram
            double minVal, maxVal;
            Cv2.MinMaxLoc(hist, out minVal, out maxVal);

            Scalar color = Scalar.All(100);
            // Scales and draws histogram
            hist = hist * (maxVal != 0 ? Height / maxVal : 0.0);
            for (int j = 0; j < hdims[0]; ++j)
            {
                int binW = (int)((double)Width / hdims[0]);
                render.Rectangle(
                    new Point(j * binW, render.Rows),
                    new Point((j + 1) * binW, render.Rows - (int)(hist.Get<float>(j))),
                    color,
                    -1);
            }

            using (new Window("Image", WindowMode.AutoSize | WindowMode.FreeRatio, src))
            using (new Window("Histogram", WindowMode.AutoSize | WindowMode.FreeRatio, render))
            {
                Cv2.WaitKey();
            }
        }

        public static void exec2()
        {
            string file = @"image\lenna_grayscale.png";
            //Mat src = new Mat(file);
            Mat gray = new Mat(file, ImreadModes.GrayScale);
            Mat hist = GetHistogram(gray);
            //using (new Window("src image", src))
            using (new Window("gray image", gray))
            using (new Window("hist", hist))
            {
                Cv2.WaitKey();
            }
        }

        // Draw Histogram from source image - for gray
        public static Mat GetHistogram(Mat source)
        {
            Mat hist = new Mat();
            int width = source.Cols, height = source.Rows;      // set Histogram same size as source image
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

            Mat render = new Mat(new Size(width, height), MatType.CV_8UC3, Scalar.All(255));
            double minVal, maxVal;
            Cv2.MinMaxLoc(hist, out minVal, out maxVal);
            Scalar color = Scalar.All(100);
            // Scales and draws histogram
            hist = hist * (maxVal != 0 ? height / maxVal : 0.0);
            int binW = width / dimensions[0];
            for (int j = 0; j < dimensions[0]; ++j)
            {
                Console.WriteLine($@"j:{j} P1: {j * binW},{render.Rows} P2:{(j + 1) * binW},{render.Rows - (int)hist.Get<float>(j)}");  //for Debug
                render.Rectangle(
                    new Point(j * binW, render.Rows - (int)hist.Get<float>(j)),
                    new Point((j + 1) * binW, render.Rows),
                    color,
                    -1);
            }
            return render;
        }
    }
}
