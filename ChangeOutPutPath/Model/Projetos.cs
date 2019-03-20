using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeOutPutPath.Model
{
    class Projetos
    {
        public Projetos(string nome, bool alterar, string outPutPathOriginal)
        {
            this.Nome = nome;
            this.Alterar = alterar;
            this.OutPutPathOriginal = outPutPathOriginal;
        }

        private string Nome;
        private bool Alterar;
        private string OutPutPathOriginal;
    }
}
