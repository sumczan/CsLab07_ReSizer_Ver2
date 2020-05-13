using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace CsLab07_photoReSizer
{
    public class Photo
    {
        public Bitmap photo { get; set; }
        public string name { get; set; }
    }

    class Program
    {
        static List<Photo>GetImages(string inPath, int resX, int resY)
        {
            List<Photo> photos = new List<Photo>();
            Size resolution = new Size(resX, resY);

            List<String> filename = new List<string>();
            var files = Directory.EnumerateFiles(inPath, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".png") || s.EndsWith("jpg") || s.EndsWith("bmp"));
            foreach (var item in files)
            {
                var image = new Bitmap(item);
                var photo = new Photo();

                image = new Bitmap(image, resolution);

                photo.photo = image;
                photo.name = Path.GetFileNameWithoutExtension(item);

                photos.Add(photo);
            }

            Console.WriteLine("Images loaded and resized");
            return photos;
        }

        static void SaveImages(List<Photo> images, string outPath)
        {
            string newName;
            foreach (var item in images)
            {
                int counter = 1;
                newName = item.name;
                var files = Directory.GetFiles(outPath, newName + ".*");
                while(files.Length != 0)
                {
                    newName = item.name + "_" + counter.ToString();
                    files = Directory.GetFiles(outPath, newName + ".*");
                    counter++;
                }
                item.photo.Save(outPath + "\\" + newName + ".png", ImageFormat.Png);
            }

            Console.WriteLine("images saved");
        }

        static void Main(string[] args)
        {
            int resX = 0;
            int resY = 0;
            string inPath = "";
            string outPath = "";
            string[] splitted;

            if (args.Length != 0)
            {
                Console.WriteLine(args[0]);
                splitted = args[0].Split('=');
                if (splitted[0] == "-res")
                {
                    splitted = splitted[1].Split('x');
                    Console.WriteLine(splitted[0], splitted[1]);
                    try
                    {
                        resX = Int32.Parse(splitted[0]);
                        resY = Int32.Parse(splitted[1]);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Podana rozdzielczosc ma zly format");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Console.WriteLine("Użyto nieznanego przełacznika 1");
                    Console.ReadKey();
                    Environment.Exit(0);
                }

                if (args.Length > 1)
                {
                    splitted = args[1].Split('=');
                    if (splitted[0] == "-inputdir")
                    {
                        inPath = Environment.CurrentDirectory + splitted[1];
                    }
                    else
                    {
                        Console.WriteLine("Użyto nieznanego przełącznika 2");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }

                    if (args.Length == 3)
                    {
                        splitted = args[2].Split('=');
                        if (splitted[0] == "-outputdir")
                        {
                            outPath = Environment.CurrentDirectory + splitted[1];
                        }
                        else
                        {
                            Console.WriteLine("Użyto nieznanego przełącznika 3");
                            Console.ReadKey();
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        outPath = Environment.CurrentDirectory;
                    }
                }
                else
                {
                    inPath = Environment.CurrentDirectory;
                    outPath = inPath;
                }

                Console.WriteLine("enwajerment current directory: " + Environment.CurrentDirectory);

                List<Photo> images = GetImages(inPath, resX, resY);

                SaveImages(images, outPath);
            }
            else
            {
                Console.WriteLine("Do obsługi tego programu potrzeba kilu wybranych parametrów");
            }
        }
    }
}
