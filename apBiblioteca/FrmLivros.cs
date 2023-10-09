using Microsoft.Build.Framework.XamlTypes;
using System;
using System.IO;
using System.Windows.Forms;

namespace apBiblioteca
{
    public partial class FrmLivros : Form
    {
        VetorDados<Livro> osLivros;
        VetorDados<Leitor> osLeitores;
        VetorDados<TipoLivro> osTipos;

        int posicaoDeInclusao = -1;
        public FrmLivros()
        {
            InitializeComponent();
        }
        private void FrmFunc_Load(object sender, EventArgs e)
        {
            tsBotoes.ImageList = imlBotoes;
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
        private void btnPrimeiro_Click(object sender, EventArgs e)
        {
            osLivros.PosicionarNoPrimeiro();
            AtualizarTela();
        }
        private void btnAnterior_Click(object sender, EventArgs e)
        {
            osLivros.RetrocederPosicao();
            AtualizarTela();
        }
        private void btnProximo_Click(object sender, EventArgs e)
        {
            osLivros.AvancarPosicao();
            AtualizarTela();
        }
        private void btnUltimo_Click(object sender, EventArgs e)
        {
            osLivros.PosicionarNoUltimo();
            AtualizarTela();
        }
        private void AtualizarTela()
        {
            if (!osLivros.EstaVazio)
            {
                int indice = osLivros.PosicaoAtual;
                txtCodigoLivro.Text = osLivros[indice].CodigoLivro + "";
                txtTituloLivro.Text = osLivros[indice].TituloLivro;                

                txtLeitorComLivro.Text = "000000";
                dtpDevolucao.Value = dtpDevolucao.MaxDate;
                txtNomeLeitor.Text = "";
                if (osLivros[indice].CodigoLeitorComLivro != "000000") // livro emprestado?
                {
                    int ondeLeitor = 0;
                    var leitorProc = new Leitor(osLivros[indice].CodigoLeitorComLivro);
                        //, "", "",
                        //                        1, new string[1]);
                    if (osLeitores.Existe(leitorProc, out ondeLeitor))
                    {
                        txtLeitorComLivro.Text = osLivros[indice].CodigoLeitorComLivro;
                        txtNomeLeitor.Text = osLeitores[ondeLeitor].NomeLeitor;
                        dtpDevolucao.Value = osLivros[indice].DataDevolucao;
                    }
                }
                TestarBotoes();
                stlbMensagem.Text = "Mensagem: Registro " + (osLivros.PosicaoAtual + 1) +
                "/" + osLivros.Tamanho;
            }
        }
        private void LimparTela()
        {
            txtCodigoLivro.Clear();
            txtTituloLivro.Clear();
            txtLeitorComLivro.Clear();
            txtNomeLeitor.Clear();
            dtpDevolucao.Value = dtpDevolucao.MaxDate;            
        }
        private void TestarBotoes()
        {
            btnInicio.Enabled = true;
            btnAnterior.Enabled = true;
            btnProximo.Enabled = true;
            btnUltimo.Enabled = true;

            if (osLivros.EstaNoInicio)
            {
                btnInicio.Enabled = false;
                btnAnterior.Enabled = false;
            }

            if (osLivros.EstaNoFim)
            {
                btnProximo.Enabled = false;
                btnUltimo.Enabled = false;
            }
        }
        private void btnNovo_Click(object sender, EventArgs e)
        {
            txtCodigoLivro.ReadOnly = false;
            osLivros.SituacaoAtual = Situacao.incluindo; // programa entrou no modo de inclusão
            LimparTela();  // limpamos os campos da tela para deixá-los prontos para digitação
            stlbMensagem.Text = "Mensagem: Digite o novo código de livro e pressione a tecla [Tab] para continuar";  // mensagem orientando o usuário
            txtCodigoLivro.Focus(); // cursor é posicionado para iniciar a digitação do código
        }
        private void txtCodigoLivro_Leave(object sender, EventArgs e)
        {
            if (txtCodigoLivro.Text != "")  // se conseguiu converter a matricula digitada
            {
                var procurado = new Livro(txtCodigoLivro.Text); // valores ficticios, o que importa é apenas a matricula digitada
                switch (osLivros.SituacaoAtual)
                {
                    case Situacao.incluindo:
                        {
                            if (osLivros.Existe(procurado, out posicaoDeInclusao))
                            {
                                MessageBox.Show("Código já existe. Não pode ser incluído novamente!");
                                btnCancelar.PerformClick();  // cancela a operação de inclusão
                            }
                            else  // o parâmetro posicaoDeInclusao recebeu o índice de onde a nova matrícula deveria estar no vetor dados
                            {
                                txtTituloLivro.Focus();  // cursor é posicionado no campo de digitaçao do nome do funcionário
                                stlbMensagem.Text = "mensagem: Digite o título do novo livro";
                                btnSalvar.Enabled = true;
                            }
                            break;
                        }
                    case Situacao.pesquisando:
                        {
                            int posicaoDoRegistro = -1;
                            if (!osLivros.Existe(procurado, out posicaoDoRegistro))
                                MessageBox.Show("Código não existe!");
                            else                    // o parâmetro posicaoDoRegistro recebeu o índice de onde está a matrícula procurada
                                osLivros.PosicaoAtual = posicaoDoRegistro;

                            osLivros.SituacaoAtual = Situacao.navegando;  // retornamos ao modo de navegação 
                            AtualizarTela();                            // e reexibimos o registro que anteriormente esteva na tela
                            break;
                        }
                }
            }
            else
            {
                MessageBox.Show("Código inválido. Digite corretamente!");
                txtCodigoLivro.Focus();
            }
        }
        
        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void FrmLivros_FormClosing(object sender, FormClosingEventArgs e)
        {
            osLivros.GravacaoEmDisco("C:\\Windows\\Temp\\livros\\livros.txt");
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente excluir?", 
                                "Exclusão",
                                MessageBoxButtons.YesNo, 
                                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                osLivros.Excluir(osLivros.PosicaoAtual);
                AtualizarTela();
            }
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            LimparTela();
            osLivros.SituacaoAtual = Situacao.pesquisando;
            txtCodigoLivro.Focus();
            stlbMensagem.Text = "Mensagem: Digite o código do livro desejado e pressione [Tab] para buscá-lo.";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            osLivros.SituacaoAtual = Situacao.navegando;  // desfaz o modo anterior do programa e volta ao modo de navegação
            AtualizarTela();                            // restaura na tela o registro que era exibido antes da operação que foi cancelada
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            osLivros.SituacaoAtual = Situacao.editando;
            txtCodigoLivro.ReadOnly = true;               // não deixa usuário alterar a matrícula
            txtTituloLivro.Focus();
            btnSalvar.Enabled = true;
            stlbMensagem.Text = "Mensagem: Digite os dados atualizados pressione [Salvar] para registrá-los.";
        }
    }
}
