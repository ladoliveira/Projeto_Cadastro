using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Management;

namespace ProjetoCadastros
{
    public partial class CadastroClienteComBootstrap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //chama apenas na primeira carga da pagina
            if (!Page.IsPostBack)
            {
                preencheGrid();
            }
        }

        string conexao = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

        public void preencheGrid()
        {
            //cria uma conexão usando a string de conexão definida
            SqlConnection con = new SqlConnection(conexao);

            //abre a conexão
            con.Open();

            string sql = "SELECT * FROM CLIENTE";

            //Cria uma objeto do tipo comando passando como parametro a string sql e a string de conexão
            SqlCommand comando = new SqlCommand(sql, con);

            SqlDataReader dr = comando.ExecuteReader();

            //cria um datatable que conterá os dados
            DataTable dt = new DataTable();

            //carrega o datatable com os dados do datareader
            dt.Load(dr);

            if (dt.Rows.Count > 0)
            {
                //exibe os dados no GridView
                grvCliente.DataSource = dt;
                grvCliente.DataBind();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "radomtext;", "Swal.fire({icon: 'error', title: 'Oops...', text: 'Nenhum registro encontrado!', footer: ''});", true);

                //exibe os dados no GridView
                grvCliente.DataSource = dt;
                grvCliente.DataBind();
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            //cria uma conexão usando a string de conexão definida
            SqlConnection conn = new SqlConnection(conexao);

            //definição do comando sql
            string sql = "SELECT CodigoCliente, NomeCliente, CpfCliente, DataNascimentoCliente, EnderecoCliente, TelefoneCliente, DataInclusao FROM CLIENTE WHERE CpfCliente = @cpfCliente";

            //Cria uma objeto do tipo comando passando como parametro a string sql e a string de conexão
            SqlCommand comando = new SqlCommand(sql, conn);

            comando.Parameters.AddWithValue("@cpfCliente", txtCpfCliente.Text);

            //Abre a conexão
            conn.Open();

            //executa o comando com os parametros que foram adicionados acima

            IDataReader reader = comando.ExecuteReader();

            if (reader.Read())
            {
                txtCodigoCliente.Value = Convert.ToInt32(reader[0]).ToString();
                txtNomeCliente.Value = reader[1].ToString();
                txtCpfClienteNew.Value = reader[2].ToString();
                txtDataNascimentoCliente.Value = reader[3].ToString();
                txtEnderecoCliente.Value = reader[4].ToString();
                txtTelefoneCliente.Value = reader[5].ToString();
                txtDataInclusao.Value = reader[6].ToString();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "radomtext;", "Swal.fire({icon: 'error', title: 'Oops...', text: 'Nenhum registro encontrado!', footer: ''});", true);

                LimparCampos();
            }

            conn.Close();
        }

        protected void btnInserir_Click(object sender, EventArgs e)
        {
            //cria uma conexão usando a string de conexão definida
            SqlConnection conn = new SqlConnection(conexao);

            conn.Open();

            //definição do comando sql
            string sql = "SELECT CpfCliente FROM CLIENTE WHERE CpfCliente = @cpfCliente";

            //Cria uma objeto do tipo comando passando como parametro a string sql e a string de conexão
            SqlCommand comando = new SqlCommand(sql, conn);

            comando.Parameters.AddWithValue("@cpfCliente", txtCpfClienteNew.Value);

            SqlDataReader dr = comando.ExecuteReader();
            if (dr.Read())
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "radomtext;", "Swal.fire({icon: 'error', title: 'Oops...', text: 'CPF já cadastro, favor digitar outro!', footer: ''});", true);
            }
            else
            {
                //Abre a conexão
                conn.Close();

                //definição do comando sql
                string sqlInsert = "INSERT INTO CLIENTE (NomeCliente, CpfCliente, DataNascimentoCliente, EnderecoCliente, TelefoneCliente, DataInclusao )" +
                "VALUES(@NomeCliente, @CpfCliente, @DataNascimentoCliente, @EnderecoCliente, @TelefoneCliente, @dataInclusao)";

                //Cria uma objeto do tipo comando passando como parametro a string sql e a string de conexão
                SqlCommand comandoInsert = new SqlCommand(sqlInsert, conn);

                comandoInsert.Parameters.AddWithValue("@NomeCliente", txtNomeCliente.Value);
                comandoInsert.Parameters.AddWithValue("@cpfCliente", txtCpfClienteNew.Value);
                comandoInsert.Parameters.AddWithValue("@DataNascimentoCliente", txtDataNascimentoCliente.Value);
                comandoInsert.Parameters.AddWithValue("@EnderecoCliente", txtEnderecoCliente.Value);
                comandoInsert.Parameters.AddWithValue("@TelefoneCliente", txtTelefoneCliente.Value);
                comandoInsert.Parameters.AddWithValue("@dataInclusao", txtDataInclusao.Value);
                //Abre a conexão
                conn.Open();

                //Executa o comando com os parametros passados acima
                comandoInsert.ExecuteNonQuery();

                Page.ClientScript.RegisterStartupScript(this.GetType(), "radomtext;", "Swal.fire({icon: 'success', title: 'Sucesso!', text: 'Cadastro inserido com sucesso!', footer: ''});", true);

                conn.Close();
            }

            //Limpa os campos da tela
            LimparCampos();

            preencheGrid();
        }

        protected void btnAlterar_Click(object sender, EventArgs e)
        {
            if (txtCodigoCliente.Value == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "radomtext;", "Swal.fire({icon: 'error', title: 'Opss!', text: 'Por favor pesquise um registro para fazer a alteração', footer: ''});", true);
                LimparCampos();
            }
            else
            {
                //cria uma conexão usando a string de conexão definida
                SqlConnection conn = new SqlConnection(conexao);

                conn.Open();

                //definição do comando sql
                string sql = "UPDATE Cliente SET NomeCliente=@nomeCliente, CpfCliente=@cpfCliente, DataNascimentoCliente=@dataNascimentoCliente, EnderecoCliente=@enderecoCliente, TelefoneCliente=@telefoneCliente, DataInclusao = @dataInclusao WHERE CodigoCliente=@codigoCliente";

                //Cria uma objeto do tipo comando passando como parametro a string sql e a string de conexão
                SqlCommand comando = new SqlCommand(sql, conn);
                //Adicionando o valor das textBox nos parametros do comando
                comando.Parameters.AddWithValue("@cpfCliente", txtCpfClienteNew.Value);
                comando.Parameters.AddWithValue("@nomeCliente", txtNomeCliente.Value);
                comando.Parameters.AddWithValue("@dataNascimentoCliente", txtDataNascimentoCliente.Value);
                comando.Parameters.AddWithValue("@enderecoCliente", txtEnderecoCliente.Value);
                comando.Parameters.AddWithValue("@telefoneCliente", txtTelefoneCliente.Value);
                comando.Parameters.AddWithValue("@codigoCliente", txtCodigoCliente.Value);
                comando.Parameters.AddWithValue("@dataInclusao", txtDataInclusao.Value);

                comando.ExecuteNonQuery();

                //fecha a conexao
                conn.Close();

                Page.ClientScript.RegisterStartupScript(this.GetType(), "radomtext;", "Swal.fire({icon: 'success', title: 'Sucesso!', text: 'Cadastro atualizado com sucesso!', footer: ''});", true);

                //Minha função para limpar os textBox
                LimparCampos();

                preencheGrid();
            }
           
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            if (txtCodigoCliente.Value == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "radomtext;", "Swal.fire({icon: 'error', title: 'Opss!', text: 'Por favor pesquise um registro para fazer a exclusão', footer: ''});", true);
                LimparCampos();
            }
            else
            {
                //cria uma conexão usando a string de conexão definida
                SqlConnection conn = new SqlConnection(conexao);


                conn.Open();

                //definição do comando sql
                string sqlDelete = "DELETE FROM Usuario WHERE CodigoCliente=@codigoCliente";

                //Cria uma objeto do tipo comando passando como parametro a string sql e a string de conexão
                SqlCommand comando = new SqlCommand(sqlDelete, conn);

                //Adicionando o valor das textBox nos parametros do comando
                comando.Parameters.AddWithValue("@codigoCliente", txtCodigoCliente.Value);

                comando.ExecuteNonQuery();

                //fecha a conexao
                conn.Close();

                string sql = "DELETE FROM Cliente WHERE CpfCliente=@cpfCliente ";
                SqlConnection connDelete = new SqlConnection(conexao);
                SqlCommand comandoDelete = new SqlCommand(sql, connDelete);

                comandoDelete.Parameters.AddWithValue("@cpfCliente", txtCpfClienteNew.Value);
                comandoDelete.CommandType = CommandType.Text;
                connDelete.Open();
                comandoDelete.ExecuteNonQuery();
                connDelete.Close();

                Page.ClientScript.RegisterStartupScript(this.GetType(), "radomtext;", "Swal.fire({icon: 'success', title: 'Sucesso!', text: 'Cadastro excluído com sucesso!', footer: ''});", true);

                LimparCampos();

                preencheGrid();
            }
        }

        public void LimparCampos()
        {
            txtCodigoCliente.Value = "";
            txtNomeCliente.Value = "";
            txtCpfCliente.Text = "";
            txtCpfClienteNew.Value = "";
            txtDataNascimentoCliente.Value = "";
            txtEnderecoCliente.Value = "";
            txtTelefoneCliente.Value = "";
            txtDataInclusao.Value = "";
        }

        protected void btnLimparCampos_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        protected void grvCliente_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            grvCliente.EditIndex = -1;
            LimparCampos();
            preencheGrid();
        }
        protected void grvCliente_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            grvCliente.EditIndex = e.NewEditIndex;
            preencheGrid();

            //Obtem cada valor único do DataKeyNames
            int id = Convert.ToInt32(grvCliente.DataKeys[e.NewEditIndex].Value.ToString());

            //Obtem o valor do TextBox no EditItemTemplet da linha clicada
            txtCodigoCliente.Value = id.ToString();
            txtCpfClienteNew.Value = ((TextBox)grvCliente.Rows[e.NewEditIndex].FindControl("txtCpfClienteNew")).Text;
            txtNomeCliente.Value = ((TextBox)grvCliente.Rows[e.NewEditIndex].FindControl("txtNomeCliente")).Text;
            txtDataNascimentoCliente.Value = ((TextBox)grvCliente.Rows[e.NewEditIndex].FindControl("txtDataNascimentoCliente")).Text;
            txtEnderecoCliente.Value = ((TextBox)grvCliente.Rows[e.NewEditIndex].FindControl("txtEnderecoCliente")).Text;
            txtTelefoneCliente.Value = ((TextBox)grvCliente.Rows[e.NewEditIndex].FindControl("txtTelefoneCliente")).Text;
            txtDataInclusao.Value = ((TextBox)grvCliente.Rows[e.NewEditIndex].FindControl("txtDataInclusao")).Text;
        }
        protected void grvCliente_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            //Obtem cada valor único do DataKeyNames
            int id = Convert.ToInt32(grvCliente.DataKeys[e.RowIndex].Value.ToString());

            //Obtem o valor do TextBox no EditItemTemplet da linha clicada
            string cpf = ((TextBox)grvCliente.Rows[e.RowIndex].FindControl("txtCpfClienteNew")).Text;
            string nome = ((TextBox)grvCliente.Rows[e.RowIndex].FindControl("txtNomeCliente")).Text;
            string dataNascimento = ((TextBox)grvCliente.Rows[e.RowIndex].FindControl("txtDataNascimentoCliente")).Text;
            string endereco = ((TextBox)grvCliente.Rows[e.RowIndex].FindControl("txtEnderecoCliente")).Text;
            string telefone = ((TextBox)grvCliente.Rows[e.RowIndex].FindControl("txtTelefoneCliente")).Text;
            string dataInclusao = ((TextBox)grvCliente.Rows[e.RowIndex].FindControl("txtDataInclusao")).Text;

            // definição da string de conexão
            SqlConnection conn = new SqlConnection(conexao);

            conn.Open();

            //definição do comando sql
            string sql = "UPDATE Cliente SET NomeCliente=@nomeCliente, CpfCliente=@cpfCliente, DataNascimentoCliente=@dataNascimentoCliente, EnderecoCliente=@enderecoCliente, TelefoneCliente=@telefoneCliente, DataInclusao = @dataInclusao WHERE CodigoCliente=@codigoCliente";

            //Cria uma objeto do tipo comando passando como parametro a string sql e a string de conexão
            SqlCommand comando = new SqlCommand(sql, conn);

            //Adicionando o valor das textBox nos parametros do comando
            comando.Parameters.AddWithValue("@codigoCliente", id);
            comando.Parameters.AddWithValue("@cpfCliente", cpf);
            comando.Parameters.AddWithValue("@nomeCliente", nome);
            comando.Parameters.AddWithValue("@dataNascimentoCliente", dataNascimento);
            comando.Parameters.AddWithValue("@enderecoCliente", endereco);
            comando.Parameters.AddWithValue("@telefoneCliente", telefone);
            comando.Parameters.AddWithValue("@dataInclusao", dataInclusao);

            comando.ExecuteNonQuery();

            //fecha a conexao
            conn.Close();

            grvCliente.EditIndex = -1;

            Page.ClientScript.RegisterStartupScript(this.GetType(), "radomtext;", "Swal.fire({icon: 'success', title: 'Sucesso!', text: 'Cadastro alterado com sucesso!', footer: ''});", true);

            LimparCampos();

            //preenche o grid nomvanete
            preencheGrid();
        }
        protected void grvCliente_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            

            int id = Convert.ToInt32(grvCliente.DataKeys[e.RowIndex].Value.ToString());
            SqlConnection conn = new SqlConnection(conexao);
            conn.Open();

            //definição do comando sql
            string sql = "DELETE FROM Cliente WHERE CodigoCliente = @id";

            //Cria uma objeto do tipo comando passando como parametro a string sql e a string de conexão
            SqlCommand comando = new SqlCommand(sql, conn);

            comando.Parameters.AddWithValue("@id ", id);
            comando.ExecuteNonQuery();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "radomtext;", "Swal.fire({icon: 'success', title: 'Sucesso!', text: 'Cadastro excluído com sucesso!', footer: ''});", true);

            preencheGrid();
        }
        protected void grvCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate) | e.Row.RowState == DataControlRowState.Edit)
            {
                return;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Referencia ao linkbutton delete
                //ImageButton deleteButton = (ImageButton)e.Row.Cells[5].Controls[0];
                //deleteButton.OnClientClick = "if (!window.confirm('Confirma a exclusão deste registro ?')) return false;";
            }
        }

        protected void grvCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codigoCliente = Convert.ToInt32(grvCliente.SelectedValue);

            //cria uma conexão usando a string de conexão definida
            SqlConnection conn = new SqlConnection(conexao);

            //definição do comando sql
            string sql = "SELECT CodigoCliente, NomeCliente, CpfCliente, DataNascimentoCliente, EnderecoCliente, TelefoneCliente, DataInclusao FROM CLIENTE WHERE CodigoCliente = @codigoCliente";

            //Cria uma objeto do tipo comando passando como parametro a string sql e a string de conexão
            SqlCommand comando = new SqlCommand(sql, conn);

            comando.Parameters.AddWithValue("@codigoCliente", codigoCliente);

            //Abre a conexão
            conn.Open();

            //executa o comando com os parametros que foram adicionados acima

            IDataReader reader = comando.ExecuteReader();

            while (reader.Read())
            {
                txtCodigoCliente.Value = Convert.ToInt32(reader[0]).ToString();
                txtNomeCliente.Value = reader[1].ToString();
                txtCpfCliente.Text = reader[2].ToString();
                txtCpfClienteNew.Value = reader[2].ToString();
                txtDataNascimentoCliente.Value = reader[3].ToString();
                txtEnderecoCliente.Value = reader[4].ToString();
                txtTelefoneCliente.Value = reader[5].ToString();
                txtDataInclusao.Value = reader[6].ToString();
            };

            conn.Close();
        }

        protected void grvCliente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvCliente.PageIndex = e.NewPageIndex;

            preencheGrid();
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            string conString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            SqlConnection con = new SqlConnection(conString);

            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM CLIENTE WHERE convert(date,DataInclusao,103) BETWEEN convert(date,@dataInicial,103) AND convert(date,@dataFinal,103) ORDER BY CodigoCliente ASC", con);
            cmd.Parameters.AddWithValue("@dataInicial", txtDataInicial.Value);
            cmd.Parameters.AddWithValue("@dataFinal", txtDataFinal.Value);

            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();

            dt.Load(dr);



            if (dt.Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "radomtext;", "Swal.fire({icon: 'error', title: 'Oops...', text: 'Nenhum registro encontrado!', footer: ''});", true);
                con.Close();
            }
            else
            {
                con.Close();
                ReportViewer.ProcessingMode = ProcessingMode.Local;
                ReportViewer.LocalReport.ReportPath = Server.MapPath("~/RelatorioCliente.rdlc");
                ReportDataSource dataSource = new ReportDataSource("dsCliente", dt);
                ReportViewer.LocalReport.DataSources.Clear();
                ReportViewer.LocalReport.DataSources.Add(dataSource);

                ReportViewer.AsyncRendering = false;
                ReportViewer.SizeToReportContent = true;
                ReportViewer.ZoomMode = ZoomMode.FullPage;

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "ExibirRelatorioModal();", true);
            }
        }
    }
}