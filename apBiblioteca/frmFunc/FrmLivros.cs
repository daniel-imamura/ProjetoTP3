using System;
using System.IO;
using System.Windows.Forms;

namespace apBiblioteca
{
    public partial class FrmLivros : Form
    {
        VetorDados<Funcionario> osFunc;
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

            osFunc = new VetorDados<Funcionario>(50);  // instancia com vetor dados com 50 posições

            osFunc.LerDados("c:\\temp\\funcionarios.txt");

            btnInicio.PerformClick();
        }

        private void btnPrimeiro_Click(object sender, EventArgs e)
        {
            osFunc.PosicionarNoPrimeiro();
            AtualizarTela();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            osFunc.RetrocederPosicao();
            AtualizarTela();
        }

        private void btnProximo_Click(object sender, EventArgs e)
        {
            osFunc.AvancarPosicao();
            AtualizarTela();
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            osFunc.PosicionarNoUltimo();
            AtualizarTela();
        }

        private void AtualizarTela()
        {
            if (!osFunc.EstaVazio)
            {
                int indice = osFunc.PosicaoAtual;
                txtMatricula.Text = osFunc[indice].Matricula + "";
                txtNome.Text = osFunc[indice].Nome;
                txtSalario.Text = osFunc[indice].Salario.ToString();
                TestarBotoes();
                stlbMensagem.Text = "Mensagem: Registro " + (osFunc.PosicaoAtual + 1) +
                "/" + osFunc.Tamanho;
            }
        }

        private void LimparTela()
        {
            txtMatricula.Clear();
            txtNome.Clear();
            txtSalario.Clear();
        }

        private void TestarBotoes()
        {
            btnInicio.Enabled = true;
            btnAnterior.Enabled = true;
            btnProximo.Enabled = true;
            btnUltimo.Enabled = true;

            if (osFunc.EstaNoInicio)
            {
                btnInicio.Enabled = false;
                btnAnterior.Enabled = false;
            }

            if (osFunc.EstaNoFim)
            {
                btnProximo.Enabled = false;
                btnUltimo.Enabled = false;
            }
        }
        private void btnNovo_Click(object sender, EventArgs e)
        {
            txtMatricula.ReadOnly = false;
            osFunc.SituacaoAtual = Situacao.incluindo; // programa entrou no modo de inclusão
            LimparTela();  // limpamos os campos da tela para deixá-los prontos para digitação
            stlbMensagem.Text = "Mensagem: Digite a nova matrícula e pressione a tecla [Tab] para continuar";  // mensagem orientando o usuário
            txtMatricula.Focus(); // cursor é posicionado no txtMatricula para iniciar a digitação da matrícula
        }

        private void txtMatricula_Leave(object sender, EventArgs e)
        {
            int matricula = 0;
            if (int.TryParse(txtMatricula.Text, out matricula))  // se conseguiu converter a matricula digitada
            {
                Funcionario procurado = new Funcionario(matricula, "-", 10); // valores ficticios, o que importa é apenas a matricula digitada
                switch (osFunc.SituacaoAtual)
                {
                    case Situacao.incluindo:
                        {
                            if (osFunc.Existe(procurado, out posicaoDeInclusao))
                            {
                                MessageBox.Show("Matrícula já existe. Não pode ser incluída novamente!");
                                btnCancelar.PerformClick();  // cancela a operação de inclusão
                            }
                            else  // o parâmetro posicaoDeInclusao recebeu o índice de onde a nova matrícula deveria estar no vetor dados
                            {
                                txtNome.Focus();  // cursor é posicionado no campo de digitaçao do nome do funcionário
                                stlbMensagem.Text = "mensagem: Digite o nome do novo funcionário";
                                btnSalvar.Enabled = true;
                            }
                            break;
                        }
                    case Situacao.pesquisando:
                        {
                            int posicaoDoRegistro = -1;
                            if (!osFunc.Existe(procurado, out posicaoDoRegistro))
                                MessageBox.Show("Matrícula não existe!");
                            else                    // o parâmetro posicaoDoRegistro recebeu o índice de onde está a matrícula procurada
                                osFunc.PosicaoAtual = posicaoDoRegistro;

                            osFunc.SituacaoAtual = Situacao.navegando;  // retornamos ao modo de navegação 
                            AtualizarTela();                            // e reexibimos o registro que anteriormente esteva na tela
                            break;
                        }
                }
            }
            else
            {
                MessageBox.Show("Matrícula inválida. Digite corretamente!");
                txtMatricula.Focus();
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            switch (osFunc.SituacaoAtual)
            {
                case Situacao.incluindo:
                    var novoFunc = new Funcionario( int.Parse(txtMatricula.Text),
                                                    txtNome.Text,
                                                    double.Parse(txtSalario.Text));
                    osFunc.Incluir(novoFunc, posicaoDeInclusao);
                    osFunc.SituacaoAtual = Situacao.navegando;
                    btnCancelar.PerformClick();
                    break;
                case Situacao.editando:
                    osFunc[osFunc.PosicaoAtual].Nome = txtNome.Text;
                    osFunc[osFunc.PosicaoAtual].Salario = double.Parse(txtSalario.Text);
                    btnCancelar.PerformClick();
                    break;
            }
            txtMatricula.ReadOnly = true;  // usuário nao poderá mais digitasr nesese campo a menos que pressione [Novo]
            btnSalvar.Enabled = false; // desabilita novamente o btnSalvar até que se pressione [Novo] ou [Editar]
            osFunc.GravacaoEmDisco("C:\\temp\\funcionarios.txt");
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmFunc_FormClosing(object sender, FormClosingEventArgs e)
        {
            osFunc.GravacaoEmDisco("c:\\temp\\funcionarios.txt");
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente excluir?", 
                                "Exclusão",
                                MessageBoxButtons.YesNo, 
                                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                osFunc.Excluir(osFunc.PosicaoAtual);
                AtualizarTela();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            LimparTela();
            osFunc.SituacaoAtual = Situacao.pesquisando;
            txtMatricula.Focus();
            stlbMensagem.Text = "Mensagem: Digite a matrícula do funcionário desejado e pressione [Tab] para buscá-lo.";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            osFunc.SituacaoAtual = Situacao.navegando;  // desfaz o modo anterior do programa e volta ao modo de navegação
            AtualizarTela();                            // restaura na tela o registro que era exibido antes da operação que foi cancelada
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            osFunc.SituacaoAtual = Situacao.editando;
            txtMatricula.ReadOnly = true;               // não deixa usuário alterar a matrícula
            txtNome.Focus();
            btnSalvar.Enabled = true;
            stlbMensagem.Text = "Mensagem: Digite os dados atualizados pressione [Salvar] para registrá-los.";
        }
    }
}
