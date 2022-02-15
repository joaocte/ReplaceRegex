using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReplaceRegex
{
    class Program2
    {
        private const string StringRegex = "\\w+\\.jsp";
        private static Regex regex = new Regex(StringRegex, RegexOptions.IgnorePatternWhitespace);
        private static ReplaceConfig replaceConfig = ObterConfiguracao();
        private static List<string> arquivosJsp = Directory.GetFiles(replaceConfig.Folder, $"*{replaceConfig.FileType}", SearchOption.AllDirectories).ToList();
        private static List<ArquivoJsp> retorno = new List<ArquivoJsp>();

        static void Main(string[] args)
        {

            var arquivosApontados = new List<string>
            {
                "msg_bloqueadoc.jsp",
            };


            foreach (var item in arquivosApontados)
            {

                ObterArquivosInternos(item);
            }
           retorno.Reverse();

            Console.WriteLine(JsonConvert.SerializeObject(retorno));
            File.AppendAllText($"{Directory.GetCurrentDirectory()}/ArquivosInternos.json", JsonConvert.SerializeObject(retorno));
        }

        private static void ObterArquivosInternos(string arquivoApontado)
        {
            var arquivosComNomesApontados = arquivosJsp.Where(x => x.Contains(arquivoApontado)).ToList();

            foreach (var arquivo in arquivosComNomesApontados)
            {
                var linhasDoArquivo = File.ReadAllLines(arquivo);

                if (!linhasDoArquivo.Any(x => regex.IsMatch(x)))
                    continue;

                var arquivoInterno = new ArquivoJsp(arquivo);
                foreach (var linha in linhasDoArquivo)
                {
                    if (regex.IsMatch(linha))
                    {
                        var linhaJsp = ObterArquivoJspDaLinha(linha);
                        arquivoInterno.ArquivosInternos.Add(linhaJsp);
                        ObterArquivosInternos(linhaJsp);
                    }
                }

                if (arquivoInterno.ArquivosInternos.Any())
                    retorno.Add(arquivoInterno);
            }
        }



        private static string ObterArquivoJspDaLinha(string linha)
        {

            return Path.GetFileName(Regex.Match(linha, StringRegex).Value);
        }

        private static ReplaceConfig ObterConfiguracao()
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            ReplaceConfig replaceConfig = config.GetSection("ReplaceConfig").Get<ReplaceConfig>();
            return replaceConfig;
        }

        private static ArquivoCsv RetornaLinhaFormatada(string file, long line_number, string line, string newContent)
        {
            return new ArquivoCsv(@file, @line_number, @line, @newContent);
        }
    }
}
