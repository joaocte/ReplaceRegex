using System;
using System.Collections.Generic;
using System.Text;

namespace ReplaceRegex
{
   public class ArquivoCsv
    {
        public ArquivoCsv(string nomeArquivo, long linhaArquivo, string linhaAntiga, string linhaModificada)
        {
            NomeArquivo = nomeArquivo;
            LinhaArquivo = linhaArquivo;
            LinhaAntiga = linhaAntiga;
            LinhaModificada = linhaModificada;
        }

        public string NomeArquivo { get; set; }

        public long LinhaArquivo { get; set; }

        public string LinhaAntiga { get; set; }

        public string LinhaModificada { get; set; }
    }
}
