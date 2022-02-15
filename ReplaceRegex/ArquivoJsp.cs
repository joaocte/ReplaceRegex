using System;
using System.Collections.Generic;
using System.Text;

namespace ReplaceRegex
{
    public class ArquivoJsp
    {

        public ArquivoJsp(string arquivoOrigem)
        {
            ArquivoOrigem = arquivoOrigem;
            ArquivosInternos = new List<string>();
        }
        public string ArquivoOrigem { get; set; }

        public List<string> ArquivosInternos { get; set; }
    }
}
