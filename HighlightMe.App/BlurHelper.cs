
namespace HighlightMe.App;

public static class BlurHelper
{
    public static Bitmap FastGaussianBlur(Bitmap src, int size)
    {
        var bxs = BoxesForGaussian(size, 3);
        Bitmap img = FastBoxBlur(src, bxs[0]);
        Bitmap img2 = FastBoxBlur(img, bxs[1]);
        Bitmap img3 = FastBoxBlur(img2, bxs[2]);
        return img3;
    }

    private static Bitmap FastBoxBlur(Bitmap img, int radius)
    {
        int kSize = radius;

        if (kSize % 2 == 0) kSize++;
        Bitmap hblur = (Bitmap) img.Clone();

        float avg = (float)1 / kSize;

        for (int j = 0; j < img.Height; j++)
        {
            float[] hSum = {
                0f, 0f, 0f, 0f
            };

            float[] iAvg = {
                0f, 0f, 0f, 0f
            };

            for (int x = 0; x < kSize; x++)
            {
                Color tmpColor = img.GetPixel(x, j);
                hSum[0] += tmpColor.A;
                hSum[1] += tmpColor.R;
                hSum[2] += tmpColor.G;
                hSum[3] += tmpColor.B;
            }

            iAvg[0] = hSum[0] * avg;
            iAvg[1] = hSum[1] * avg;
            iAvg[2] = hSum[2] * avg;
            iAvg[3] = hSum[3] * avg;
            for (int i = 0; i < img.Width; i++)
            {
                if (i - kSize / 2 >= 0 && i + 1 + kSize / 2 < img.Width) {
                    Color tmpPColor = img.GetPixel(i - kSize / 2, j);
                    hSum[0] -= tmpPColor.A;
                    hSum[1] -= tmpPColor.R;
                    hSum[2] -= tmpPColor.G;
                    hSum[3] -= tmpPColor.B;
                    Color tmpNColor = img.GetPixel(i + 1 + kSize / 2, j);
                    hSum[0] += tmpNColor.A;
                    hSum[1] += tmpNColor.R;
                    hSum[2] += tmpNColor.G;
                    hSum[3] += tmpNColor.B;
                    //
                    iAvg[0] = hSum[0] * avg;
                    iAvg[1] = hSum[1] * avg;
                    iAvg[2] = hSum[2] * avg;
                    iAvg[3] = hSum[3] * avg;
                }
                hblur.SetPixel(i, j, Color.FromArgb((int)iAvg[0], (int)iAvg[1], (int)iAvg[2], (int)iAvg[3]));
            }
        }

        Bitmap total = (Bitmap) hblur.Clone();
        for (int i = 0; i < hblur.Width; i++)
        {
            float[] tSum = {
                0f, 0f, 0f, 0f
            };
            float[] iAvg = {
                0f, 0f, 0f, 0f
            };
            for (int y = 0; y < kSize; y++)
            {
                Color tmpColor = hblur.GetPixel(i, y);
                tSum[0] += tmpColor.A;
                tSum[1] += tmpColor.R;
                tSum[2] += tmpColor.G;
                tSum[3] += tmpColor.B;
            }

            iAvg[0] = tSum[0] * avg;
            iAvg[1] = tSum[1] * avg;
            iAvg[2] = tSum[2] * avg;
            iAvg[3] = tSum[3] * avg;

            for (int j = 0; j < hblur.Height; j++)
            {
                if (j - kSize / 2 >= 0 && j + 1 + kSize / 2 < hblur.Height) {
                    Color tmpPColor = hblur.GetPixel(i, j - kSize / 2);
                    tSum[0] -= tmpPColor.A;
                    tSum[1] -= tmpPColor.R;
                    tSum[2] -= tmpPColor.G;
                    tSum[3] -= tmpPColor.B;
                    Color tmpNColor = hblur.GetPixel(i, j + 1 + kSize / 2);
                    tSum[0] += tmpNColor.A;
                    tSum[1] += tmpNColor.R;
                    tSum[2] += tmpNColor.G;
                    tSum[3] += tmpNColor.B;
                    //
                    iAvg[0] = tSum[0] * avg;
                    iAvg[1] = tSum[1] * avg;
                    iAvg[2] = tSum[2] * avg;
                    iAvg[3] = tSum[3] * avg;
                }
                total.SetPixel(i, j, Color.FromArgb((int)iAvg[0], (int)iAvg[1], (int)iAvg[2], (int)iAvg[3]));
            }
        }

        return total;
    }

    private static int[] BoxesForGaussian(double sigma, int n)
    {
        double wIdeal = Math.Sqrt((12 * sigma * sigma / n) + 1);
        double wl = Math.Floor(wIdeal);

        if (wl % 2 == 0) wl--;
        double wu = wl + 2;

        double mIdeal = (12 * sigma * sigma - n * wl * wl - 4 * n * wl - 3 * n) / (-4 * wl - 4);
        double m = Math.Round(mIdeal);

        int[] sizes = new int[n];
        for (int i = 0; i < n; i++)
        {
            if (i < m)
            {
                sizes[i] = (int)wl;
            }
            else
            {
                sizes[i] = (int)wu;
            }
        }

        return sizes;
    }
}