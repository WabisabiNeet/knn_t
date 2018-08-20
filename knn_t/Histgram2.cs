using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace knn_t
{
    class Histgram2
    {
        public static void exec()
        {
            //var image = new Mat(@"image\result_success.bmp", ImreadModes.GrayScale);
            var image_success = new Mat(@"image\result_success.bmp");
            var image_failure = new Mat(@"image\result_failure.bmp");

            var hsv_success = new Mat();
            var hsv_failure = new Mat();

            Cv2.CvtColor(image_success, hsv_success, ColorConversionCodes.BGR2HSV);
            Cv2.CvtColor(image_failure, hsv_failure, ColorConversionCodes.BGR2HSV);

            var hist_success = new Mat();
            foreach (var s in hsv_success.Split())
            {
                var h = GetHistogram4Hue(s);
                hist_success.Add(h);
                break;
            }

            var hist_failure = new Mat();
            foreach(var s in hsv_failure.Split())
            {
                var h = GetHistogram4Hue(s);
                hist_failure.Add(h);
                break;
            }

            Cv2.ImShow("success", hsv_success);
            Cv2.ImShow("failure", hsv_failure);
            Cv2.ImShow("hist_success", hist_success);
            Cv2.ImShow("hist_failure", hist_failure);

            Cv2.WaitKey();
        }

        // Draw Histogram from source image - for gray
        public static Mat GetHistogram(Mat source)
        {
            Mat hist = new Mat();
            int width = source.Cols/2, height = source.Rows/2;      // set Histogram same size as source image
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
                //Console.WriteLine($@"j:{j} P1: {j * binW},{render.Rows} P2:{(j + 1) * binW},{render.Rows - (int)hist.Get<float>(j)}");  //for Debug
                render.Rectangle(
                    new Point(j * binW, render.Rows - (int)hist.Get<float>(j)),
                    new Point((j + 1) * binW, render.Rows),
                    color,
                    -1);
            }
            return render;
        }

        public static Mat GetHistogram4Hue(Mat source)
        {
            Mat hist = new Mat();
            int width = source.Cols / 2, height = source.Rows / 2;      // set Histogram same size as source image
            const int histogramSize = 180;                      // you can change by urself
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
            //Scalar color = Scalar.All(100);
            //Scalar color = new Scalar(0, 0, 255);
            // Scales and draws histogram
            hist = hist * (maxVal != 0 ? height / maxVal : 0.0);
            int binW = width / dimensions[0];
            for (int j = 0; j < dimensions[0]; ++j)
            {
                Mat rgb = new Mat(); ;
                Mat hsv = new Mat(1, 1, MatType.CV_8UC3, new Scalar(j/2, 255, 255));
                Cv2.CvtColor(hsv, rgb, ColorConversionCodes.HSV2BGR);
                Scalar color = new Scalar((int)rgb.Get<Vec3b>(0, 0)[0], (int)rgb.Get<Vec3b>(0, 0)[1], (int)rgb.Get<Vec3b>(0, 0)[2]);
                Console.WriteLine($"color:{color}");

                //Console.WriteLine($@"j:{j} P1: {j * binW},{render.Rows} P2:{(j + 1) * binW},{render.Rows - (int)hist.Get<float>(j)}");  //for Debug
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
