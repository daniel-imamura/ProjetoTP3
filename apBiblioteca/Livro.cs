﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apBiblioteca
{
    class Livro : IRegistro, IComparable<Livro>
    {
        const int inicioCodigoLivro = 0;
        public const int tamanhoCodigoLivro = 7;
        const int inicioTitulo = inicioCodigoLivro + tamanhoCodigoLivro;
        const int tamanhoTitulo = 30;
        const int inicioTipo = inicioTitulo + tamanhoTitulo;
        const int tamanhoTipo = 3;
        const int inicioData = inicioTipo + tamanhoTipo;
        const int tamanhoData = 10;   // dd/mm/aaaa
        const int inicioCodigoLeitor = inicioData + tamanhoData;
        const int tamanhoCodigoLeitor = Leitor.tamanhoCodigoLeitor;

        string codigoLivro;
        string tituloLivro;
        int tipoLivro;
        DateTime dataDevolucao;
        string codigoLeitorComLivro;

        public Livro()
        {
        }
        public Livro(string cl)
        {
            CodigoLivro = cl;
        }
        public Livro(string livro, string titulo, int tipo, DateTime data, string leitor)
        {
            CodigoLivro = livro;
            TituloLivro = titulo;
            TipoLivro = tipo;
            DataDevolucao = data;
            CodigoLeitorComLivro = leitor;
        }
        public int CompareTo(Livro other)
        {
            return this.CodigoLivro.CompareTo(other.CodigoLivro);
        }

        public string FormatoDeArquivo()
        {
            return CodigoLivro.ToString() + TituloLivro + TipoLivro.ToString().PadLeft(tamanhoTipo, ' ') +
                   DataDevolucao.ToShortDateString() + CodigoLeitorComLivro;
        }

        public void LerRegistro(StreamReader arq)
        {
            if (!arq.EndOfStream)
            {
                String linha = arq.ReadLine();
                CodigoLivro = linha.Substring(inicioCodigoLivro, tamanhoCodigoLivro);
                TituloLivro = linha.Substring(inicioTitulo, tamanhoTitulo);
                TipoLivro   = int.Parse(linha.Substring(inicioTipo, tamanhoTipo));
                DataDevolucao = DateTime.Parse(linha.Substring(inicioData, tamanhoData));
                CodigoLeitorComLivro = linha.Substring(inicioCodigoLeitor, tamanhoCodigoLeitor);
            }
        }
        public string CodigoLivro
        {
            get => codigoLivro;
            set
            {
                if (value.Length > tamanhoCodigoLivro)
                    value = value.Substring(0, tamanhoCodigoLivro);
                codigoLivro = value.PadLeft(tamanhoCodigoLivro, '0');   
            }
        }
        public string TituloLivro
        {
            get => tituloLivro;
            set
            {
                if (value.Length > tamanhoTitulo)
                    value = value.Substring(0, tamanhoTitulo);
                tituloLivro = value.PadRight(tamanhoTitulo, ' ');
            }
        }
        public int TipoLivro  
        {
            get => tipoLivro;
            set
            {
                if (value > 0 && value < 1000)
                    tipoLivro = value;
            }
        }
        public DateTime DataDevolucao
        {
            get => dataDevolucao;
            set => dataDevolucao = value;
        }
        public string CodigoLeitorComLivro
        {
            get => codigoLeitorComLivro;
            set
            {
                if (value.Length > tamanhoCodigoLeitor)
                    value = value.Substring(0, tamanhoCodigoLeitor);
                codigoLeitorComLivro = value.PadLeft(tamanhoCodigoLeitor, '0');
            }
        }
        public override String ToString()
        {
            String resultado = CodigoLivro + " "+TituloLivro + " " + TipoLivro.ToString().PadLeft(tamanhoTipo, ' ');
            if (CodigoLeitorComLivro != "000000")
                resultado += " " + DataDevolucao.ToShortDateString() + " " + CodigoLeitorComLivro;
            return resultado;
        }
    }
}
