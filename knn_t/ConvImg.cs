using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace knn_t
{
    class ConvImg
    {
        public static void ConvertColor2Gray()
        {
            var fname = "01バイト開始.bmp";

            Mat color = new Mat($@"image\{fname}");
            var rect = new Rect(820, 30, 260, 80);

            var sw = new Stopwatch();
            sw.Start();

            var color_small = new Mat(color, rect);

            // 黄色の画素範囲
            Scalar scalar_low = new Scalar(0, 240, 240); // B,G,R いっつも忘れる
            Scalar scalar_high = new Scalar(20, 255, 255);

            Mat yellow = new Mat();
            //Cv2.InRange(color, scalar_low, scalar_high, yellow);
            Cv2.InRange(color_small, scalar_low, scalar_high, yellow);

            sw.Stop();
            Console.WriteLine($"ProcTime:{sw.ElapsedMilliseconds}");

            yellow.ImWrite($@"image\yellow.bmp");
            Cv2.ImShow("yellow", yellow);

            Cv2.WaitKey();
        }
    }
}
