using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace apBiblioteca
{
    class TipoLivro : IRegistro, IComparable<TipoLivro>
    {
        const int tamanhoTipoLivro = 3;
        const int tamanhoDescricao = 20;

        const int inicioTipoLivro = 0;
        const int inicioDescricao = inicioTipoLivro + tamanhoTipoLivro;                

        int tipoDoLivro;
        string descricaoTipo;

        public TipoLivro()
        {
        }
        public TipoLivro(int tp)
        {
            TipoDoLivro = tp;
        }
        public TipoLivro(int tipo, string descricao)
        {
            TipoDoLivro = tipo;
            DescricaoTipo = descricao;
        }
        public string DescricaoTipo
        {
            get => descricaoTipo;

            set
            {
                if (value.Length > tamanhoDescricao)
                    value = value.Substring(0, tamanhoDescricao);

                descricaoTipo = value.PadLeft(tamanhoDescricao, '0');
            }
        }
        public int TipoDoLivro
        {
            get => tipoDoLivro;

            set
            {
                if (value > 0 && value < 10)
                    tipoDoLivro = value;
            }
        }
        public List<TipoLivro> Lista()
        {
            var lista = new List<TipoLivro>();
            var l1 = new TipoLivro();

            l1.TipoDoLivro = 1;
            l1.DescricaoTipo = "dajdos";

            lista.Add(l1);

            return lista;
        }

        public int CompareTo(TipoLivro outro)
        {
            return tipoDoLivro.CompareTo(outro.tipoDoLivro);
        }
        public override String ToString()
        {
            return TipoDoLivro.ToString() + " " + DescricaoTipo;
        }
        public void LerRegistro(StreamReader arq)
        {
            if (!arq.EndOfStream)
            {
                String linha = arq.ReadLine();
                TipoDoLivro = int.Parse(linha.Substring(inicioTipoLivro, tamanhoTipoLivro));
                DescricaoTipo = linha.Substring(inicioDescricao, tamanhoDescricao);                                
            }
        }
        public string FormatoDeArquivo()
        {
            return TipoDoLivro.ToString() + DescricaoTipo;
        }
    }
}
