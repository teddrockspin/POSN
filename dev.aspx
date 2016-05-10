<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="dev.aspx.vb" Inherits="POSN.dev" %>

<%@ Register Assembly="APPortalUI" Namespace="APPortalUI.Web.UI" TagPrefix="apPortalUI" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>activePDF Portal Sample</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:DataGrid ID="grdItm" runat="server" CssClass="lblDSmall" Width="100%"
                GridLines="Horizontal" OnItemDataBound="grdItm_ItemDataBound" BorderStyle="None"
                Font-Size="Smaller" BorderColor="Black" BorderWidth="1px" CellPadding="0"
                AutoGenerateColumns="False"
                AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages" PagerStyle-Visible="False">
                <SelectedItemStyle CssClass="grdListSelectItm" BackColor="#FFFF80"></SelectedItemStyle>
                <PagerStyle Mode="NumericPages" Visible="False"></PagerStyle>

                <ItemStyle CssClass="grdListBody"></ItemStyle>
                <Columns>
                    <asp:BoundColumn DataField="ID" HeaderText="ID" Visible="False"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="Order" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <input id="inpID" runat="server" name="inpItmID" type="hidden"
                                value='<%# DataBinder.Eval(Container,"DataItem.ID")%>' />
                            <span class="tooltip1" title="tooltip">
                                <asp:CheckBox ID="chkSelect" runat="server" CssClass="chkItem" /></span>
                            <asp:Label ID="lblOrderQuantityLimit" runat="server" Style="display: none;"
                                Text='<%# DataBinder.Eval(Container,"DataItem.OrderQuantityLimit") %>'></asp:Label>
                            </input>
                        </ItemTemplate>
                        <HeaderStyle Width="1%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="&nbsp;Download" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" > 
                        <ItemTemplate>
                            <asp:HiddenField runat="server" ID="hfDownloadID" />
                            <span class="tooltip2" title="tooltip">
                                <asp:CheckBox ID="chkSelectDownload" runat="server" CssClass="chkItem" /></span>
                        </ItemTemplate>
                        <HeaderStyle Width="1%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="Status" Visible="False">
                        <HeaderStyle Width="1%" />
                    </asp:BoundColumn>
                    <asp:TemplateColumn>
                        <ItemTemplate>
                        </ItemTemplate>
                        <HeaderStyle Width="1%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Part No" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Button ID="btnItmNo" runat="server" CssClass="btnItmNoList"
                                Text='<%# DataBinder.Eval(Container.DataItem, "RefNo").ToString() & "  " %>' />
                        </ItemTemplate>
                        <HeaderStyle Width="18%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Description" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Button ID="btnItmName" runat="server" CssClass="btnItmNameList"
                                Text='<%# LEFT(DataBinder.Eval(Container.DataItem, "RefName").ToString(),65) & vbcrlf & MID(DataBinder.Eval(Container.DataItem, "RefName").ToString(),66) %>' />
                            <asp:Literal ID="ltDownloadables" runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle Width="70%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Style="display: none;"
                                Text='<%# DataBinder.Eval(Container, "DataItem.OrderQuantityLimit") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Button ID="btnStock" runat="server" Text='<%#getShowStock(DataBinder.Eval(Container, "DataItem.ID"))%>' CssClass="btnStockShow" />
                        </ItemTemplate>
                    </asp:TemplateColumn>

                </Columns>
                
                <HeaderStyle Font-Italic="True" Font-Bold="True" CssClass="grdListHead"></HeaderStyle>
            </asp:DataGrid>
        </div>
    </form>
</body>
</html>
