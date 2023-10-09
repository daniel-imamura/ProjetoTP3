using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apBiblioteca
{
    public partial class FrmLeitores : Form
    {
        VetorDados<Livro> osLivros;
        VetorDados<Leitor> osLeitores;
        VetorDados<TipoLivro> osTipos;

        public FrmLeitores()
        {
            InitializeComponent();
        }
        private void AtualizarTela()
        {
            if (!osLeitores.EstaVazio)
            {
                int indice = osLeitores.PosicaoAtual;
                txtCodigoLeitor.Text = osLeitores[indice].CodigoLeitor + "";
                txtNomeLeitor.Text = osLeitores[indice].NomeLeitor;
                
                txtEndereco.Text = osLeitores[indice].EnderecoLeitor;                
                txtNomeLeitor.Text = "";
                
                TestarBotoes();
                stlbMensagem.Text = "Mensagem: Registro " + (osLeitores.PosicaoAtual + 1) +
                "/" + osLeitores.Tamanho;
            }
        }       
        private void TestarBotoes()
        {
            btnInicio.Enabled = true;
            btnAnterior.Enabled = true;
            btnProximo.Enabled = true;
            btnUltimo.Enabled = true;

            if (osLeitores.EstaNoInicio)
            {
                btnInicio.Enabled = false;
                btnAnterior.Enabled = false;
            }

            if (osLeitores.EstaNoFim)
            {
                btnProximo.Enabled = false;
                btnUltimo.Enabled = false;
            }
        }
        private void FrmLeitores_Load(object sender, EventArgs e)
        {            
            int indice = 0;
            foreach (ToolStripItem item in tsBotoes.Items)
                if (item is ToolStripButton) // se não é separador:
                    (item as ToolStripButton).ImageIndex = indice++;

            osLivros = new VetorDados<Livro>(150);  // instancia com vetor dados com 50 posições
            osLivros.LerDados("C:\\Windows\\Temp\\arquivosTexto\\livros.txt");

            osLeitores = new VetorDados<Leitor>(50);
            osLeitores.LerDados("C:\\Windows\\Temp\\arquivosTexto\\leitor.txt");

            osTipos = new VetorDados<TipoLivro>(50);
            osTipos.LerDados("C:\\Windows\\Temp\\arquivosTexto\\tiposLivros.txt");

            btnInicio.PerformClick();
        }

        private void btnInicio_Click_1(object sender, EventArgs e)
        {
            osLeitores.PosicionarNoPrimeiro();
            AtualizarTela();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            osLeitores.RetrocederPosicao();
            AtualizarTela();
        }

        private void btnProximo_Click(object sender, EventArgs e)
        {
            osLeitores.AvancarPosicao();
            AtualizarTela();
        }


        private void btnUltimo_Click(object sender, EventArgs e)
        {
            osLeitores.PosicionarNoUltimo();
            AtualizarTela();
        }

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            LimparTela();
            osLeitores.SituacaoAtual = Situacao.pesquisando;
            txtCodigoLeitor.Focus();
            stlbMensagem.Text = "Mensagem: Digite o código do livro desejado e pressione [Tab] para buscá-lo.";
        }

        private void LimparTela()
        {
            txtCodigoLeitor.Clear();
            txtEndereco.Clear();
            txtNomeLeitor.Clear();
            txtLivros.Clear();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {        
            osLeitores.SituacaoAtual = Situacao.incluindo; 
            LimparTela();  
            stlbMensagem.Text = "Mensagem: Digite o novo código de leitor e pressione a tecla [Tab] para continuar";  // mensagem orientando o usuário
            txtCodigoLeitor.Focus(); // cursor é posicionado para iniciar a digitação do código
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            osLeitores.SituacaoAtual = Situacao.navegando;  
            AtualizarTela();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente excluir?",
                                "Exclusão",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                osLeitores.Excluir(osLeitores.PosicaoAtual);
                AtualizarTela();
            }

        }
        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
