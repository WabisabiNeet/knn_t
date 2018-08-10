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
    class ContainsImage
    {
        public static void exec()
        {
            var base_images = Directory.EnumerateFiles(@"C:\tagwork\test\knn_t\testdata\success");

            var sw = new Stopwatch();
            sw.Start();

            foreach (var i in base_images)
            {
                Compare(i);
            }

            sw.Stop();
            Console.WriteLine($"ProcTime:{sw.ElapsedMilliseconds} Compare Count:{base_images.Count()}");
        }

        private static void Compare(string img)
        {
            var maskname = "2ti_yajirusi_inversion.bmp";
            var rect = new Rect(685, 125, 60, 50);

            using (Mat color = new Mat($@"{img}"))
            using (var color_small = new Mat(color, rect))
            using (Mat yellow = new Mat())
            using (var mask = new Mat($@"image\{maskname}", ImreadModes.GrayScale))
            using (var result = new Mat())
            {
                // 緑っぽいの(RGB=14/255/101)の画素範囲(個人の結果が書かれてるシーンの矢印検出用)
                Scalar scalar_low = new Scalar(50, 245, 0); // B,G,R いっつも忘れる
                Scalar scalar_high = new Scalar(120, 255, 30);

                // 黄色画像の抽出(2値化)
                Cv2.InRange(color_small, scalar_low, scalar_high, yellow);
                Cv2.Add(yellow, mask, result);

                using (var hist = GetHistogram(result))
                {
                    int cnt = (int)hist.Get<float>(0);
                    if (cnt < 10)
                    {
                        Console.WriteLine($"img:[{img}] black_count:[{hist.Get<float>(0)}] white_count:[{hist.Get<float>(255)}]");
                    }
                }
            }
        }

        public static Mat GetHistogram(Mat source)
        {
            using(var hist = new Mat())
            {
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

                return hist.Clone();
            }
        }
    }
}
