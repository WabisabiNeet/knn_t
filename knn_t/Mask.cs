using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace knn_t
{
    class Mask
    {

        public static void exec()
        {
            var fname = "01バイト開始.bmp";
            var maskname = "yellow_1bit_inversion.bmp";

            Mat color = new Mat($@"image\{fname}");
            var rect = new Rect(820, 30, 260, 80);

            var sw = new Stopwatch();
            sw.Start();

            var color_small = new Mat(color, rect);

            // 黄色の画素範囲
            Scalar scalar_low = new Scalar(0, 240, 240);
            Scalar scalar_high = new Scalar(20, 255, 255);

            // 黄色画像の抽出(2値化)
            Mat yellow = new Mat();
            Cv2.InRange(color_small, scalar_low, scalar_high, yellow);

            var mask = new Mat($@"image\{maskname}", ImreadModes.GrayScale);

            var result = new Mat();
            Cv2.Add(yellow, mask, result);

            sw.Stop();
            Console.WriteLine($"ProcTime:{sw.ElapsedMilliseconds}");

            Cv2.ImShow("yellow", yellow);
            Cv2.ImShow("result", result);

            Cv2.WaitKey();

            
        }
    }
}
