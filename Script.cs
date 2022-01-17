using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BadgeGifToPng
{
    public class Script
    {
        public static void init()
        {
            string path = Environment.CurrentDirectory + "/badges/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string outputExtraPath = "output/";

            if (!Directory.Exists(path + outputExtraPath))
                Directory.CreateDirectory(path + outputExtraPath);

            string[] fileEntries = Directory.GetFiles(Environment.CurrentDirectory + "/badges/");
            if (fileEntries.Length == 0)
            {
                Console.WriteLine("Don't have any images on the folder badges.");
                return;
            }

            byte[] imagebuffer;

            foreach (string fileName in fileEntries)
            {
                if (File.Exists(Path.Combine(path + outputExtraPath, fileName.Replace(".gif", ".png"))))
                    continue;

                using (FileStream fs = new FileStream(Path.Combine(path, fileName), FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        using (Image img = Image.FromStream(fs))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                img.Save(ms, ImageFormat.Png);
                                imagebuffer = ms.ToArray();
                                ms.Dispose();
                            }
                        }

                        File.WriteAllBytes(Path.Combine(path + outputExtraPath, fileName.Split('/')[2].Replace(".gif", "") + ".png"), imagebuffer);
                    }
                    catch (System.ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine(ex.ToString());
                        continue;
                    }
                    catch (System.ArgumentException ex)
                    {
                        Console.WriteLine("The followed badge can't be converted because is invalid: " + fileName.Split('/')[2]);
                        continue;
                    }
                }
            }

            Console.WriteLine("\nDone!");
        }
    }
}
