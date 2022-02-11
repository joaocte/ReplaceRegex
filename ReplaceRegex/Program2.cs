using Microsoft.Extensions.Configuration;
using System.IO;

namespace ReplaceRegex
{
    class Program2
    {
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();

            ReplaceConfig replaceConfig = config.GetSection("ReplaceConfig").Get<ReplaceConfig>();
            var arquivos = Directory.GetFiles(replaceConfig.Folder, "*", SearchOption.AllDirectories);
            foreach (string file in arquivos)
            {
                var fileName = Path.GetFileName(file);

                if (fileName.EndsWith(replaceConfig.FileType))
                {
                    var arquivoTemporario = fileName.Replace($"{replaceConfig.FileType}", $"{replaceConfig.FileType}{replaceConfig.FileTypeTemp}");
                    var diretorio = Path.GetDirectoryName(file);
                    if (File.Exists($"{diretorio}/{arquivoTemporario}"))
                    {
                        File.Delete(file);
                    }
                }
                else if (fileName.EndsWith(replaceConfig.FileTypeTemp))
                {
                    File.Move(file, Path.ChangeExtension(file, ""));

                }
            }

        }
    }
}


