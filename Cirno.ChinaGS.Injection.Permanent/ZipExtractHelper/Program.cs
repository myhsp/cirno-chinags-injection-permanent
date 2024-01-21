using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace ZipExtractHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string source, extract;
                source = args[0].Trim();
                extract = args[1].Trim();
                if (File.Exists(source) && Directory.Exists(extract))
                {
                    ZipFile.ExtractToDirectory(source, extract);
                }
                else
                {
                    Console.WriteLine("路径不存在");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("文件已存在");
            }
        }
    }
}
