using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace knn_t
{

    /// <summary>
    /// 左上のバイト結果抽出用
    /// </summary>
    class ConvImg4
    {
        public static void ConvertColor2Gray()
        {
            var fname = "result_detail_capture_001.bmp";

            Mat color = new Mat($@"C:\tagwork\test\knn_t\testdata\{fname}");
            var rect = new Rect(30, 68, 219, 64); // for 1080p

            var sw = new Stopwatch();
            sw.Start();

            var color_small = new Mat(color, rect);

            // 緑っぽいの(RGB=14/255/101)の画素範囲(個人の結果が書かれてるシーンの矢印検出用)
            Scalar scalar_low = new Scalar(50, 245, 0); // B,G,R いっつも忘れる
            Scalar scalar_high = new Scalar(120, 255, 30);

            // オレンジっぽいの(RGB=246/96/65)の画素範囲(個人の結果が書かれてるシーンの矢印検出用)
            //Scalar scalar_low = new Scalar(50, 80, 220); // B,G,R いっつも忘れる
            //Scalar scalar_high = new Scalar(70, 105, 255);

            Mat yellow = new Mat();
            //Cv2.InRange(color, scalar_low, scalar_high, yellow);
            Cv2.InRange(color_small, scalar_low, scalar_high, yellow);

            sw.Stop();
            Console.WriteLine($"ProcTime:{sw.ElapsedMilliseconds}");

            yellow.ImWrite($@"image\result_failure_1bit_1_movie.bmp");
            //color_small.ImWrite($@"image\green_yajirusi.bmp");
            Cv2.ImShow("color_small", color_small);
            Cv2.ImShow("yellow", yellow);

            Cv2.WaitKey();
        }
    }
}
