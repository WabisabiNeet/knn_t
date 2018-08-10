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
    /// 個人スコア表示の緑矢印抽出
    /// </summary>
    class ConvImg2
    {
        public static void ConvertColor2Gray()
        {
            var fname = "result_detail_movie_002.bmp";

            Mat color = new Mat($@"C:\tagwork\test\knn_t\testdata\{fname}");
            //var rect = new Rect(685, 125, 60, 50); // for 720p 1st
            //var rect = new Rect(1035, 185, 75, 75); // for 1080p 1st
            var rect = new Rect(1035, 397, 75, 75); // for 1080p 2nd
            //var rect = new Rect(1035, 822, 75, 75); // for 1080p 4th

            var sw = new Stopwatch();
            sw.Start();

            var color_small = new Mat(color, rect);

            // 緑っぽいの(RGB=14/255/101)の画素範囲(個人の結果が書かれてるシーンの矢印検出用)
            Scalar scalar_low = new Scalar(50, 245, 0); // B,G,R いっつも忘れる
            Scalar scalar_high = new Scalar(120, 255, 30);

            Mat yellow = new Mat();
            //Cv2.InRange(color, scalar_low, scalar_high, yellow);
            Cv2.InRange(color_small, scalar_low, scalar_high, yellow);

            sw.Stop();
            Console.WriteLine($"ProcTime:{sw.ElapsedMilliseconds}");

            yellow.ImWrite($@"image\result_arrow_1bit_2nd_1_movie.bmp");
            //color_small.ImWrite($@"image\green_yajirusi.bmp");
            Cv2.ImShow("color_small", color_small);
            Cv2.ImShow("yellow", yellow);

            Cv2.WaitKey();
        }
    }
}
