using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace knn_t
{
    class Mask2
    {

        public static void exec()
        {
            var fname = "result_detail.bmp";
            var maskname = "2ti_yajirusi_inversion.bmp";

            Mat color = new Mat($@"image\{fname}");
            var rect = new Rect(685, 125, 60, 50);

            var sw = new Stopwatch();
            sw.Start();

            var color_small = new Mat(color, rect);

            // 緑っぽいの(RGB=14/255/101)の画素範囲(個人の結果が書かれてるシーンの矢印検出用)
            Scalar scalar_low = new Scalar(50, 245, 0); // B,G,R いっつも忘れる
            Scalar scalar_high = new Scalar(120, 255, 30);

            // 黄色画像の抽出(2値化)
            Mat yellow = new Mat();
            Cv2.InRange(color_small, scalar_low, scalar_high, yellow);

            var mask = new Mat($@"image\{maskname}", ImreadModes.GrayScale);

            var result = new Mat();
            Cv2.Add(yellow, mask, result);

            sw.Stop();
            Console.WriteLine($"ProcTime:{sw.ElapsedMilliseconds}");

            Cv2.ImShow("color_small", color_small);
            Cv2.ImShow("yellow", yellow);
            Cv2.ImShow("result", result);

            Cv2.WaitKey();

            
        }
    }
}
