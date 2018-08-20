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
    class NumberExtracting
    {
        public static void Exec()
        {
            var rect_1st_ikura_gold = new Rect(1490, 200, 85, 30); // 金イクラ for 1080p
            var rect_1st_ikura_red = new Rect(1490, 240, 85, 30); // 赤イクラ for 1080p
            var rect_1st_rescues = new Rect(1692, 200, 43, 30); // 助けた数 for 1080p
            var rect_1st_rescued = new Rect(1692, 240, 43, 30); // 助けられた数 for 1080p

            var rect_2nd_ikura_gold = new Rect(1490, 413, 85, 30); // 金イクラ for 1080p
            var rect_2nd_ikura_red = new Rect(1490, 453, 85, 30); // 赤イクラ for 1080p
            var rect_2nd_rescues = new Rect(1692, 413, 43, 30); // 助けた数 for 1080p
            var rect_2nd_rescued = new Rect(1692, 453, 43, 30); // 助けられた数 for 1080p

            var rect_3rd_ikura_gold = new Rect(1490, 625, 85, 30); // 金イクラ for 1080p
            var rect_3rd_ikura_red = new Rect(1490, 665, 85, 30); // 赤イクラ for 1080p
            var rect_3rd_rescues = new Rect(1692, 625, 43, 30); // 助けた数 for 1080p
            var rect_3rd_rescued = new Rect(1692, 665, 43, 30); // 助けられた数 for 1080p

            var rect_4th_ikura_gold = new Rect(1490, 838, 85, 30); // 金イクラ for 1080p
            var rect_4th_ikura_red = new Rect(1490, 878, 85, 30); // 赤イクラ for 1080p
            var rect_4th_rescues = new Rect(1692, 838, 43, 30); // 助けた数 for 1080p
            var rect_4th_rescued = new Rect(1692, 878, 43, 30); // 助けられた数 for 1080p

            var sw = new Stopwatch();
            sw.Start();

            foreach (var f in Directory.EnumerateFiles(@"C:\tagwork\test\salmon_testdata\result\"))
            {
                using (Mat gray = new Mat(f, ImreadModes.GrayScale))
                {
                    ExtractNumber(gray, rect_1st_ikura_gold, rect_1st_ikura_red, rect_1st_rescues, rect_1st_rescued);
                    ExtractNumber(gray, rect_2nd_ikura_gold, rect_2nd_ikura_red, rect_2nd_rescues, rect_2nd_rescued);
                    ExtractNumber(gray, rect_3rd_ikura_gold, rect_3rd_ikura_red, rect_3rd_rescues, rect_3rd_rescued);
                    ExtractNumber(gray, rect_4th_ikura_gold, rect_4th_ikura_red, rect_4th_rescues, rect_4th_rescued);
                }
            }
        }

        private static void ExtractNumber(Mat src, Rect gold, Rect red, Rect rescues, Rect rescued)
        {
            using (var img_ikura_gold = new Mat(src, gold))
            using (var img_ikura_red = new Mat(src, red))
            using (var img_rescues = new Mat(src, rescues))
            using (var img_rescued = new Mat(src, rescued))
            using (var img_binary_ikura_gold = new Mat())
            using (var img_binary_ikura_red = new Mat())
            using (var img_binary_rescues = new Mat())
            using (var img_binary_rescured = new Mat())
            {
                Cv2.Threshold(img_ikura_gold, img_binary_ikura_gold, 245, 255, ThresholdTypes.Binary);
                Cv2.Threshold(img_ikura_red, img_binary_ikura_red, 245, 255, ThresholdTypes.Binary);
                Cv2.Threshold(img_rescues, img_binary_rescues, 245, 255, ThresholdTypes.Binary);
                Cv2.Threshold(img_rescued, img_binary_rescured, 245, 255, ThresholdTypes.Binary);

                Cv2.ImWrite($@"C:\tagwork\test\salmon_testdata\result\Extracted\{Path.GetRandomFileName()}.bmp", img_binary_ikura_gold);
                Cv2.ImWrite($@"C:\tagwork\test\salmon_testdata\result\Extracted\{Path.GetRandomFileName()}.bmp", img_ikura_red);
                Cv2.ImWrite($@"C:\tagwork\test\salmon_testdata\result\Extracted\{Path.GetRandomFileName()}.bmp", img_rescues);
                Cv2.ImWrite($@"C:\tagwork\test\salmon_testdata\result\Extracted\{Path.GetRandomFileName()}.bmp", img_rescued);
            }

        }
    }
}
