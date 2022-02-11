//using Microsoft.Extensions.Configuration;
//using System.IO;

//namespace ReplaceRegex
//{
//    class Program2
//    {
//        static void Main(string[] args)
//        {

//            var builder = new ConfigurationBuilder()
//                 .SetBasePath(Directory.GetCurrentDirectory())
//                 .AddJsonFile("appsettings.json", optional: false);
//            IConfiguration config = builder.Build();

//            ReplaceConfig replaceConfig = config.GetSection("ReplaceConfig").Get<ReplaceConfig>();
//            var arquivos = Directory.GetFiles(replaceConfig.Folder, $"*{replaceConfig.FileTypeTemp}", SearchOption.AllDirectories);
//            foreach (string file in arquivos)
//            {
//                var fileName = Path.GetFileName(file);

//                    var arquivoOriginal = fileName.Replace($"{replaceConfig.FileTypeTemp}", "");
//                    var diretorio = Path.GetDirectoryName(file);

//                    if (File.Exists($"{diretorio}/{arquivoOriginal}"))
//                    {
//                        File.Delete(($"{diretorio}/{arquivoOriginal}"));
//                    }
//                    File.Move(file, Path.ChangeExtension(file, ""));

//            }

//        }
//    }
//}


