using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace knn_t
{
    class Capture
    {
        static public void exec()
        {
            if (!Directory.Exists(@"image\capture"))
            {
                Directory.CreateDirectory(@"image\capture");
            }

            var start = DateTime.Now;
            //var videoCapture = VideoCapture.FromCamera(CaptureDevice.Any, 0);
            //var videoCapture = VideoCapture.FromFile(@"C:\tagwork\test\knn_t\testdata\2018-08-09 23-25-24.avi");
            var videoCapture = VideoCapture.FromFile(@"C:\tagwork\test\knn_t\testdata\2018-08-09 23-18-15.avi");

            //videoCapture.Set(CaptureProperty.FrameWidth, 1920);
            //videoCapture.Set(CaptureProperty.FrameHeight, 1080);
            //videoCapture.Set(CaptureProperty.FrameWidth, 1280);
            //videoCapture.Set(CaptureProperty.FrameHeight, 720);
            //videoCapture.Set(CaptureProperty.FrameWidth, 1920);
            //videoCapture.Set(CaptureProperty.FrameHeight, 1080);

            var mat = new Mat();
            if (videoCapture.IsOpened())
            {
                for (int i = 0; i < 24500; i++)
                {
                    var ret = videoCapture.Read(mat);
                    if (mat.Empty()) break;
                    Console.WriteLine($"roop:{i} ret:{ret}");
                    if (!ret)
                    {
                        break;
                    }
                    //Cv2.ImShow("test", mat);
                    Cv2.WaitKey(5);
                    Cv2.ImWrite($@"image\capture\1920x1080_{i:00000}.bmp", mat);
                }

                //for (int i = 0; i < 100; i++)
                //{
                //    var fname = $@"image\test{i}.bmp";
                //    Console.WriteLine(fname);
                //    mat = Cv2.ImRead(fname);
                //    Cv2.ImShow("test", mat.Clone());
                //    Cv2.WaitKey(10);
                //}
            }

            var end = DateTime.Now;
            Console.WriteLine($"{end - start}");
            videoCapture.Release();
            videoCapture.Dispose();
            //Console.ReadKey();
        }
    }
}
