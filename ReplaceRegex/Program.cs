using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace ReplaceRegex
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();

            ReplaceConfig replaceConfig = config.GetSection("ReplaceConfig").Get<ReplaceConfig>();
            var regex = new Regex("^(?!if\\()request\\.getParameter\\(\" *.* \"*|param\\[.*\\]\\)\\;", RegexOptions.IgnorePatternWhitespace);
            long line_number = 1;
            string line = null;
            var logs = new List<ArquivoCsv>();
            bool arquivoModificado = false;
            foreach (string file in Directory.GetFiles(replaceConfig.Folder, replaceConfig.FileType, SearchOption.AllDirectories))
            {
                using (StreamReader reader = new StreamReader(file))
                using (StreamWriter writer = new StreamWriter($"{file}_temp"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (regex.IsMatch(line) && !line.Contains("ESAPI.encoder().encodeForHTML("))
                        {
                            var newContent = Regex.Replace(line, "request\\.getParameter\\(\" *.* \"*|param\\[.*\\]\\)\\;", $"ESAPI.encoder().encodeForHTML({line.Replace(");", ")")});", RegexOptions.IgnorePatternWhitespace);
                            writer.WriteLine(newContent);
                            logs.Add(RetornaLinhaFormatada(file, line_number, line, newContent));
                            arquivoModificado = true;
                        }
                        else
                        {
                            writer.WriteLine(line);
                        }
                        line_number++;
                    }
                }
                if (arquivoModificado)
                {
                    var srReader = new StreamReader($"{file}_temp");
                    var strFileContents = srReader.ReadToEnd();
                    srReader.Close();
                    strFileContents = "<%@ page import=\"org.owasp.esapi.*\"%>" + Environment.NewLine + strFileContents;
                    var swWriter = new StreamWriter($"{file}_temp", false);
                    swWriter.Write(strFileContents);
                    swWriter.Flush();
                    arquivoModificado = false;
                }
            }
            File.WriteAllText($"{Directory.GetCurrentDirectory()}/log.json", JsonConvert.SerializeObject(logs));

        }


        private static ArquivoCsv RetornaLinhaFormatada(string file, long line_number, string line, string newContent)
        {
            return new ArquivoCsv(file, line_number, line, newContent);
        }
    }
}
