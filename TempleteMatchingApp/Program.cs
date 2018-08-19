using DSLab;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TempleteMatchingApp
{
    class Program
    {
        enum ProcModes
        {
            MovieFile,
            ImagesInFolder,
            CaptureDevice,
        }

        static ProcModes mode = ProcModes.MovieFile;
        static int deviceid = 0;
        static Mat templete;
        static int frame_count = 0;
        const string MATCHED_IMAGES = "MatchedImage";

        static void Main(string[] args)
        {
            Init();

            switch (mode)
            {
                case ProcModes.MovieFile:
                    ProcFromVideFile(args[0]);
                    break;
                case ProcModes.ImagesInFolder:
                    ProcFolder(args[0]);
                    break;
                case ProcModes.CaptureDevice:
                default:
                    ProcFromCaptureDevice(deviceid);
                    break;
            }
        }

        static void CheckArgment()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Skip(1).Any(arg => arg.Equals("-d")))
            {
                mode = ProcModes.CaptureDevice;
                SelectDeviceId();
            }
            else if (args.Length == 3)
            {
                if (Directory.Exists(args[1]))
                    mode = ProcModes.ImagesInFolder;
                else if (File.Exists(args[1]))
                    mode = ProcModes.MovieFile;
            }
            else
            {
                Console.WriteLine("Error. Invalid argments.");
                Console.WriteLine("Usage:");
                Console.WriteLine("  arg1:Target Movie file path.");
                Console.WriteLine("  arg2:Templete file path.");
                Environment.Exit(-1);
            }

            bool exists = false;
            for (int i = 1; i < args.Length; i++)
            {
                exists |= File.Exists(args[i]);
                exists |= Directory.Exists(args[i]);

                if (!exists)
                    Console.WriteLine($"Error. File or Directory is not Found. [{args[i]}]");
            }

            if (!exists)
                Environment.Exit(-2);
        }

        static void SelectDeviceId()
        {
            var category = new Guid(GUID.CLSID_VideoInputDeviceCategory);
            var filterInfos = Axi.GetFilterList(category);

            Console.WriteLine("Video Input Devices ({0})", filterInfos.Count);
            for (var i = 0; i < filterInfos.Count; i++)
            {
                // フィルタ情報:
                var filterInfo = filterInfos[i];
                //Console.WriteLine("|- {0}", iii);
                //Console.WriteLine("|  |- {0}", filterInfo.Name);
                Console.WriteLine($"[{i}] {filterInfo.Name}");
            }

            Console.Write("Please select device number:");

            try
            {
                int id = int.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error. Please correct device id.");
                Console.WriteLine(ex.ToString());
            }
            
        }

        static void Init()
        {
            CheckArgment();

            if (!Directory.Exists(MATCHED_IMAGES))
            {
                Directory.CreateDirectory(MATCHED_IMAGES);
            }

            var args = Environment.GetCommandLineArgs();
            templete = new Mat(args[2], ImreadModes.GrayScale);
        }

        static void ProcFromCaptureDevice(int deviceid)
        {
            using (var source = VideoCapture.FromCamera(CaptureDevice.Any, deviceid))
            {
                ProcCapture(source);
            }
        }

        static void ProcFromVideFile(string filename)
        {
            using (var source = VideoCapture.FromFile(filename))
            {
                ProcCapture(source);
            }
        }

        static void ProcCapture(VideoCapture source)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            while (true)
            {
                if (!source.IsOpened())
                    return;

                using (var mat = new Mat())
                {
                    frame_count++;
                    if (source.Read(mat))
                    {
                        if (mat.Empty())
                            break;

                        Matching(mat);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($"Process time:{sw.ElapsedMilliseconds}ms");
        }

        static void ProcFolder(string dir)
        {
            var files = Directory.EnumerateFiles(dir);

            foreach (var f in files)
            {
                frame_count++;
                using (var mat  = new Mat(f))
                {
                    if (mat.Empty())
                        continue;

                    Matching(mat);
                }
            }
        }

        static void Matching(Mat mat)
        {
            using (var target = InRange4ResultFailure(mat))
            //using (var target = InRange4ResultSuccess(mat))
            //using (var target = InRange4DetailResult(mat))
            {
                Composer c = new Composer(target, templete);
                var result = c.Compose();

                Console.WriteLine($"Frame[{frame_count:00000000}] Matched[{result}]");
                if (result)
                {
                    var name = $@"{MATCHED_IMAGES}\{frame_count:00000000}.bmp";
                    mat.ImWrite(name);

                    if (mode == ProcModes.CaptureDevice)
                    {
                        // 至近フレームのほぼ同一画像をスキップするためにスリープ
                        Thread.Sleep(5000);
                    }
                }
            }
        }

        static Mat InRange4DetailResult(Mat target)
        {
            Mat result = new Mat();

            //var rect = new Rect(new Point(0, 0), target.Size());
            var rect = new Rect(new Point(1035, 185), new Size(75, 712)); // 高速化のため矢印部分だけカット

            // 緑っぽいの(RGB=14/255/101)の画素範囲(個人の結果が書かれてるシーンの矢印検出用)
            Scalar scalar_low = new Scalar(50, 245, 0); // B,G,R いっつも忘れる
            Scalar scalar_high = new Scalar(120, 255, 30);

            using (var arrow_area = new Mat(target, rect))
            {
                //Cv2.InRange(color, scalar_low, scalar_high, yellow);
                Cv2.InRange(arrow_area, scalar_low, scalar_high, result);
            }

            return result;
        }

        static Mat InRange4ResultSuccess(Mat target)
        {
            Mat result = new Mat();

            //var rect = new Rect(new Point(0, 0), target.Size());
            var rect = new Rect(new Point(30, 68), new Size(219, 64)); // 高速化のため矢印部分だけカット

            // 緑っぽいの(RGB=14/255/101)の画素範囲(個人の結果が書かれてるシーンの矢印検出用)
            Scalar scalar_low = new Scalar(50, 245, 0); // B,G,R いっつも忘れる
            Scalar scalar_high = new Scalar(120, 255, 30);

            using (var result_area = new Mat(target, rect))
            {
                //Cv2.InRange(color, scalar_low, scalar_high, yellow);
                Cv2.InRange(result_area, scalar_low, scalar_high, result);
            }

            return result;
        }

        static Mat InRange4ResultFailure(Mat target)
        {
            Mat result = new Mat();

            //var rect = new Rect(new Point(0, 0), target.Size());
            var rect = new Rect(new Point(30, 68), new Size(219, 64)); // 高速化のため矢印部分だけカット

            // オレンジっぽいの(RGB=246/96/65)の画素範囲(個人の結果が書かれてるシーンの矢印検出用)
            Scalar scalar_low = new Scalar(50, 80, 220); // B,G,R いっつも忘れる
            Scalar scalar_high = new Scalar(70, 105, 255);

            using (var result_area = new Mat(target, rect))
            {
                //Cv2.InRange(color, scalar_low, scalar_high, yellow);
                Cv2.InRange(result_area, scalar_low, scalar_high, result);
            }

            return result;
        }
    }
}
