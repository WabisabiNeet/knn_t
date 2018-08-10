using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleteMatchingApp
{
    class Composer
    {
        private Mat image;
        private Mat templete;

        public Composer(Mat img, Mat templete)
        {
            this.image = img;
            this.templete = templete;
        }

        public bool Compose()
        {
            bool matched = false;

            Mat refMat = image;
            Mat tplMat = templete;

            using (Mat res = new Mat(refMat.Rows - tplMat.Rows + 1, refMat.Cols - tplMat.Cols + 1, MatType.CV_32FC1))
            {
                //Convert input images to gray
                using (Mat gref = refMat.Clone())
                using (Mat gtpl = tplMat.Clone())
                {
                    Cv2.MatchTemplate(gref, gtpl, res, TemplateMatchModes.CCoeffNormed);
                    Cv2.Threshold(res, res, 0.8, 1.0, ThresholdTypes.Tozero);

                    while (true)
                    {
                        double minval, maxval, threshold = 0.8;
                        Point minloc, maxloc;
                        Cv2.MinMaxLoc(res, out minval, out maxval, out minloc, out maxloc);

                        if (maxval >= threshold)
                        {
                            matched = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                //Cv2.ImShow("Matches", refMat);
                Cv2.WaitKey();
            }

            return matched;
        }

        private void RunTemplateMatch(Mat reference, Mat tmp)
        {

        }
    }
}
