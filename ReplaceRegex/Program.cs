//using CsvHelper;
//using CsvHelper.Configuration;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Text.RegularExpressions;

//namespace ReplaceRegex
//{
//    class Program
//    {
//        private const string StringRegex = "request\\.getParameter\\(\"*.*\"*|param\\[.*\\]\\)\\;";

//        static void Main(string[] args)
//        {
//            ReplaceConfig replaceConfig = ObterConfiguracao();

//            var regex = new Regex(StringRegex, RegexOptions.IgnorePatternWhitespace);
//            var logs = new List<ArquivoCsv>();
//            var arquivosParaProcessar = Directory.GetFiles(replaceConfig.Folder, $"*{replaceConfig.FileType}", SearchOption.AllDirectories);
//            bool arquivoModificado = false;

//            foreach (string file in arquivosParaProcessar)
//            {
//                long line_number = 1;

//                var linhas = File.ReadAllLines(file);
//                var existeAlgumaLinhaParaAlterar = linhas.Any(x => regex.IsMatch(x) && !x.Contains("ESAPI.encoder().encodeForHTML("));

//                if (existeAlgumaLinhaParaAlterar)
//                {
//                    linhas = null;
//                    using (StreamReader reader = new StreamReader(file))
//                    using (StreamWriter writer = new StreamWriter($"{file}{replaceConfig.FileTypeTemp}"))
//                    {
//                        string line = null;

//                        while ((line = reader.ReadLine()) != null)
//                        {
//                            if (regex.IsMatch(line) && !line.Contains("ESAPI.encoder().encodeForHTML("))
//                            {
//                                var match = Regex.Match(line, StringRegex).Value.Replace(";", "");


//                                var newContent = Regex.Replace(line, StringRegex, $"ESAPI.encoder().encodeForHTML({match});", RegexOptions.IgnorePatternWhitespace);
//                                writer.WriteLine(newContent);
//                                logs.Add(RetornaLinhaFormatada(file, line_number, line, newContent));
//                                arquivoModificado = true;
//                            }
//                            else
//                            {
//                                writer.WriteLine(line);
//                            }
//                            line_number++;
//                        }
//                    }
//                    if (arquivoModificado)
//                    {
//                        var srReader = new StreamReader($"{file}{replaceConfig.FileTypeTemp}");
//                        var strFileContents = srReader.ReadToEnd();
//                        srReader.Close();
//                        strFileContents = $"<%@ page import=\"org.owasp.esapi.*\"%>{Environment.NewLine}{strFileContents}";
//                        var swWriter = new StreamWriter($"{file}{replaceConfig.FileTypeTemp}", false);
//                        swWriter.Write(strFileContents);
//                        swWriter.Flush();
//                        arquivoModificado = false;
//                    }
//                }
//            }
//            File.AppendAllText($"{Directory.GetCurrentDirectory()}/log.json", JsonConvert.SerializeObject(logs));
//        }

//        private static ReplaceConfig ObterConfiguracao()
//        {
//            var builder = new ConfigurationBuilder()
//                 .SetBasePath(Directory.GetCurrentDirectory())
//                 .AddJsonFile("appsettings.json", optional: false);
//            IConfiguration config = builder.Build();
//            ReplaceConfig replaceConfig = config.GetSection("ReplaceConfig").Get<ReplaceConfig>();
//            return replaceConfig;
//        }

//        private static ArquivoCsv RetornaLinhaFormatada(string file, long line_number, string line, string newContent)
//        {
//            return new ArquivoCsv(@file, @line_number, @line, @newContent);
//        }
//    }
//}
