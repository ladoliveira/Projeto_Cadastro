<%@ Page Language="c#" AutoEventWireup="true" CodeBehind="CadastroClienteComBootstrap.aspx.cs" Inherits="ProjetoCadastros.CadastroClienteComBootstrap" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastro  de Cliente</title>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" />
    <style>
        /*gridview*/
        .botao {
            padding: 6px 16px;
            font-size: 18px;
            line-height: 1.3333333;
            border-radius: 6px;
        }

        .aumentaMargin {
            margin-bottom: 30px;
            margin-top: 15px;
        }

        .table table tbody tr td a,
        .table table tbody tr td span {
            position: relative;
            float: left;
            padding: 6px 12px;
            margin-left: -1px;
            line-height: 1.42857143;
            color: #337ab7;
            text-decoration: none;
            background-color: #fff;
            border: 1px solid #ddd;
        }

        .table table > tbody > tr > td > span {
            z-index: 3;
            color: #fff;
            cursor: default;
            background-color: #337ab7;
            border-color: #337ab7;
        }

        .table table > tbody > tr > td:first-child > a,
        .table table > tbody > tr > td:first-child > span {
            margin-left: 0;
            border-top-left-radius: 4px;
            border-bottom-left-radius: 4px;
        }

        .table table > tbody > tr > td:last-child > a,
        .table table > tbody > tr > td:last-child > span {
            border-top-right-radius: 4px;
            border-bottom-right-radius: 4px;
        }

        .table table > tbody > tr > td > a:hover,
        .table table > tbody > tr > td > span:hover,
        .table table > tbody > tr > td > a:focus,
        .table table > tbody > tr > td > span:focus {
            z-index: 2;
            color: #23527c;
            background-color: #eee;
            border-color: #ddd;
        }
        /*end gridview */
    </style>
</head>
<body>
    <script src="Scripts/jquery-3.4.1.min.js"></script>
    <script src="Scripts/jquery.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/jquery_mask.js"></script>
    <link href="Sweet2/sweetalert2.min.css" rel="stylesheet" />
    <script src="Sweet2/sweetalert2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(() => {
            $("#txtCpfCliente").mask("999.999.999-99");
            $("#txtCpfClienteNew").mask("999.999.999-99");
            $("#txtDataNascimentoCliente").mask("99/99/9999");
            $("#txtDataNascimentoCliente").mask("99/99/9999");
            $("#txtDataInicial").mask("99/99/9999");
            $("#txtDataFinal").mask("99/99/9999");
            $("#txtDataInclusao").mask("99/99/9999");
            $("#txtTelefoneCliente").mask("(99) 99999-9999");

            $('#btnModal1').on('click', () => {
                $('#txtCpfClienteNew').attr('required', false);
                $('#txtNomeCliente').attr('required', false);
                $('#txtDataNascimentoCliente').attr('required', false);
                $('#txtEnderecoCliente').attr('required', false);
                $('#txtTelefoneCliente').attr('required', false);
                $('#txtDataInclusao').attr('required', false);
            })

            $("#validarCampos").on("click", () => {
                var dataInicial = $("#txtDataInicial").val();
                var dataFinal = $("#txtDataFinal").val();

                if (dataInicial.length != 10 || dataInicial == "") {
                    $('#txtDataInicial').trigger("focus");
                    Swal.fire({ icon: 'error', title: 'Oops...', text: 'Por favor informe a data inicial.', footer: '' });
                    document.getElementById("txtDataInicial").value = "";
                    return false;
                }

                if (dataFinal.length != 10 || dataFinal == "") {
                    $('#txtDataFinal').trigger("focus");
                    Swal.fire({ icon: 'error', title: 'Oops...', text: 'Por favor informe a data final.', footer: '' });
                    document.getElementById("txtDataFinal").value = "";
                    return false;
                }

                if (dataInicial > dataFinal) {
                    $('#txtDataInicial').trigger("focus");
                    Swal.fire({ icon: 'error', title: 'Oops...', text: 'Data inicial deve ser menor que data final.', footer: '' });
                    document.getElementById("txtDataInicial").value = "";
                    return false;
                }
                $("#btnGerarRelatorio").click();
                event.preventDefault();
            });
        });

        function ExibirRelatorioModal() {
            $('#botaoFantasma').click();
        }
    </script>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sManager1" runat="server" EnableScriptGlobalization="true" />
        <br />
        <div class="container">
            <div class="form-group row">
                <label class="col-md-2 control-label" for="txtCpfCliente">
                    CPF
                </label>
                <div class="col-md-4">
                    <asp:TextBox ID="txtCpfCliente" name="txtCpfCliente" placeholder="" class="form-control input-md" type="text" runat="server" MaxLength="14"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <input type="button" id="btnPesquisar" runat="server" class="btn btn-warning" title="PESQUISAR" value="PESQUISAR" onserverclick="btnPesquisar_Click" />
                </div>

                <div class="col-md-2">
                    <button type="button" id="btnModal1" class="btn btn-info botao" data-toggle="modal" data-target="#modalFiltrarPeriodo">Relatório por Período</button>
                </div>
            </div>
            <div id="modalFiltrarPeriodo" class="modal fade" role="dialog" data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Gerar Relatório por Período</h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <label class="col-md-1 control-label" style="padding-left: 0px" for="txtDataInicial">
                                    INÍCIO
                                </label>
                                <div class="col-md-3">
                                    <input type="text" id="txtDataInicial" class="form-control input-md" runat="server" />
                                </div>
                                <label class="col-md-1 control-label" for="txtDataFinal">
                                    FIM
                                </label>
                                <div class="col-md-3">
                                    <input type="text" id="txtDataFinal" class="form-control input-md" runat="server" />
                                </div>
                            </div>
                            <br />
                        </div>
                        <br />
                        <div style="text-align: center">
                            <asp:Button ID="btnGerarRelatorio" class="btn btn-info btn-lg" Style="display: none" runat="server" Text="gerar relatório" OnClick="btnGerarRelatorio_Click" />
                            <asp:Button ID="validarCampos" class="btn btn-info btn-lg" runat="server" Text="Gerar Relatório" />
                        </div>
                        <br />
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div>
            <button type="button" style="display: none;" id="botaoFantasma" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#modalExibirRelatorio">Abrir Relatório</button>
        </div>
        <div class="modal fade" id="modalExibirRelatorio">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Relatório por Período</h4>
                    </div>
                    <div class="modal-body">
                        <div class="table-responsive">
                            <rsweb:ReportViewer ID="ReportViewer" CssClass="table" runat="server" ClientIDMode="AutoID" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226">
                                <LocalReport ReportPath="RelatorioCliente.rdlc">
                                    <DataSources>
                                        <rsweb:ReportDataSource Name="dsCliente" />
                                    </DataSources>
                                </LocalReport>
                            </rsweb:ReportViewer>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>
                    </div>
                </div>
            </div>
        </div>
        <hr style="margin-top:5px" />
        <div class="container">
            <div class="form-group row">
                <label class="col-md-2 control-label" for="txtCodigoCliente">CÓDIGO</label>
                <div class="col-md-4">
                    <input type="text" id="txtCodigoCliente" name="txtCodigoCliente" class="form-control input-md" runat="server" readonly="true" />
                </div>
            </div>
            <div class="form-group row">
                <label class="col-md-2 control-label" for="txtCpfClienteNew">
                    CPF
                <h11>*</h11>
                </label>
                <div class="col-md-4">
                    <input type="text" id="txtCpfClienteNew" name="txtCpfClienteNew" class="form-control input-md" runat="server" maxlength="14" required="required" />
                </div>
                <label class="col-md-2 control-label" for="txtNomeCliente">
                    NOME
                <h11>*</h11>
                </label>
                <div class="col-md-4">
                    <input type="text" id="txtNomeCliente" name="txtNomeCliente" class="form-control input-md" runat="server" required="required" />
                </div>
            </div>
            <div class="form-group row">
                <label class="col-md-2 control-label" for="txtDataNascimentoCliente">
                    DATA NASCIMENTO
                <h11>*</h11>
                </label>
                <div class="col-md-4">
                    <input type="text" id="txtDataNascimentoCliente" name="txtDataNascimentoCliente" class="form-control input-md" runat="server" required="required" />
                </div>
                <label class="col-md-2 control-label" for="txtEnderecoCliente">
                    ENDEREÇO
                <h11>*</h11>
                </label>
                <div class="col-md-4">
                    <input type="text" id="txtEnderecoCliente" name="txtEnderecoCliente" class="form-control input-md" runat="server" required="required" />
                </div>
            </div>
            <div class="form-group row">
                <label class="col-md-2 control-label" for="txtTelefoneCliente">
                    TELEFONE
                <h11>*</h11>
                </label>
                <div class="col-md-4">
                    <input type="text" id="txtTelefoneCliente" name="txtTelefoneCliente" class="form-control input-md" runat="server" required="required" />
                </div>
                <label class="col-md-2 control-label" for="txtTelefoneCliente">
                    DATA INCLUSÃO
                <h11>*</h11>
                </label>
                <div class="col-md-4">
                    <input type="text" id="txtDataInclusao" name="txtDataInclusao" class="form-control input-md" runat="server" required="required" />
                </div>
            </div>
        </div>
        <hr style="margin-top:5px" />
        <div class="container aumentaMargin">
            <asp:Button ID="btnInserir" runat="server" Text="INSERIR" class="btn btn-primary" OnClick="btnInserir_Click" />
            <asp:Button ID="btnAlterar" runat="server" Text="ALTERAR" class="btn btn-info" OnClick="btnAlterar_Click" />
            <asp:Button ID="btnExcluir" runat="server" Text="EXCLUIR" class="btn btn-danger" OnClientClick="javascript: return confirm('Tem certeza que deseja excluir esse registro?')" OnClick="btnExcluir_Click" />
            <asp:Button ID="btnLimparCampos" runat="server" Text="LIMPAR" class="btn btn-success" OnClick="btnLimparCampos_Click" />
        </div>
        <hr style="margin-top:-15px" />
        <div class="container">
            <asp:GridView ID="grvCliente" CssClass="table table-responsive table-striped" runat="server" AutoGenerateColumns="False" DataKeyNames="CodigoCliente" OnRowCancelingEdit="grvCliente_RowCancelingEdit" OnRowDataBound="grvCliente_RowDataBound"
                OnRowDeleting="grvCliente_RowDeleting" OnRowEditing="grvCliente_RowEditing" OnRowUpdating="grvCliente_RowUpdating" AutoGenerateSelectButton="True" OnSelectedIndexChanged="grvCliente_SelectedIndexChanged" PageSize="2" AllowPaging="True" CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="grvCliente_PageIndexChanging">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:TemplateField HeaderText="CÓDIGO">
                        <ItemTemplate><%#Eval("CodigoCliente") %>  </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="NOME">
                        <ItemTemplate><%#Eval("NomeCliente") %>    </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtNomeCliente" runat="server" Text='<%#Eval("NomeCliente") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="190px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="190px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CPF">
                        <ItemTemplate><%#Eval("CpfCliente") %>    </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCpfClienteNew" runat="server" Text='<%#Eval("CpfCliente") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="NASCIMENTO">
                        <ItemTemplate><%#Eval("DataNascimentoCliente") %>    </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDataNascimentoCliente" runat="server" Text='<%#Eval("DataNascimentoCliente") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ENDEREÇO">
                        <ItemTemplate><%#Eval("EnderecoCliente") %>    </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEnderecoCliente" runat="server" Text='<%#Eval("EnderecoCliente") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="190px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="190px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TELEFONE">
                        <ItemTemplate><%#Eval("TelefoneCliente") %>    </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtTelefoneCliente" runat="server" Text='<%#Eval("TelefoneCliente") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="190px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="190px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DATA INCLUSÃO">
                        <ItemTemplate><%#Eval("DataInclusao") %>    </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDataInclusao" runat="server" Text='<%#Eval("DataInclusao") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="true" ButtonType="Image"
                        EditImageUrl="Imagem/editar.jpg"
                        UpdateImageUrl="Imagem/aceitar.jpg"
                        CancelImageUrl="Imagem/erro.jpg" HeaderText="EDITAR">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                    </asp:CommandField>
                    <asp:CommandField ShowDeleteButton="true" ButtonType="Image" DeleteImageUrl="Imagem/erro.jpg" HeaderText="DELETAR">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                    </asp:CommandField>
                </Columns>
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                <SortedDescendingHeaderStyle BackColor="#820000" />
            </asp:GridView>
        </div>
        <br />
    </form>
</body>
</html>





