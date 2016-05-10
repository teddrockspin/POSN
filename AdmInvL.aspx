<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdmInvL.aspx.vb" Inherits="POSN.AdmInvL"
    SmartNavigation="True" %>

<td valign="top">
    <div dir="rtl" style='width: 100%; height: 100%' runat='server' id="Div1">
        <div dir="ltr" style="width: 100%; height: 100%">
            <form id="frmAdmInvL" method="post" runat="server">
            <table width="100%" height="100%" align="center" border="0" cellpadding="0" cellspacing="0" class="changebg"
                <tbody>
                    <tr>
                        <td valign="top" background="">
                            <table width="100%" align="center">
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:label id="lblTitle" runat="server" text="Inventory" cssclass="lblTitle"></asp:label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <font face="Verdana" size="-4">Note: Items shown in <font face="Verdana" color="green"
                                            size="-4">GREEN</font> have depleted inventories and have been moved to POD status.</font>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <font face="Verdana" size="-4">Note: Items shown in <font face="Verdana" color="#bd5304"
                                            size="-4">ORANGE</font> are on backorder and not available at this time.</font>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <font face="Verdana" size="-4">Note: Items shown in <font face="Verdana" color="darkred"
                                            size="-4">BURGUNDY</font> have an inventory below the designated restocking level.</font>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" background="">
                            <table width="100%" align="center">
                                <tr>
                                    <td align="center" width="100%" colspan="4">
                                        <asp:datagrid id="grd" runat="server" autogeneratecolumns="False" showfooter="True"
                                            cellpadding="3" borderwidth="1px" bordercolor="Black" font-size="Smaller" borderstyle="None"
                                            onitemdatabound="grd_ItemDataBound" allowsorting="True">
            <SelectedItemStyle Font-Italic="True" CssClass="grdSelect"></SelectedItemStyle>
            <ItemStyle CssClass="grdBody"></ItemStyle>
            <HeaderStyle Font-Italic="True" Font-Bold="True" CssClass="grdHead"></HeaderStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="ID" Visible="False" >
                    <ItemTemplate>
                        <asp:Label ID="lblId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn Visible="True" HeaderText="Review">
                    <HeaderStyle Width="8%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Status" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="PrintMethod" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblPrintMethod" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PrintMethod") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                 <asp:TemplateColumn HeaderText="Status" Visible="True">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Status1") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn Visible="True" HeaderText="Supply (Wks)"  SortExpression="Supply1">
                    <ItemTemplate>
                        <Asp:label runat=server id=lblSweeks text='<%#DataBinder.Eval(Container, "DataItem.supply1")%>'></Asp:label>
                    </ItemTemplate>
                    <HeaderStyle Width="20%" HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateColumn>
                   <asp:TemplateColumn Visible="False" HeaderText="Bwarn">
                    <ItemTemplate>
                        <Asp:label runat=server id=lblBwarn text='<%#DataBinder.Eval(Container, "DataItem.bwarn")%>'></Asp:label>
                    </ItemTemplate>
              
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Qty in Pieces" SortExpression="QtyOH">
                    <ItemTemplate>
                        <asp:Label ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.QtyOH") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Qty BackOrdered" SortExpression="QTYBCK">
                    <ItemTemplate>
                        <asp:Label ID="lblQTYBCK" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.QTYBCK") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="TotalValue" HeaderText="Value $"  SortExpression="TotalValue" DataFormatString="{0:C}">
                    <HeaderStyle Width="10%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundColumn>
                 <asp:TemplateColumn HeaderText="Prefix" SortExpression="Prefix">
                    <ItemTemplate>
                        <asp:Label ID="lblPrefix" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Prefix") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Part Number" SortExpression="RefNo">
                    <ItemTemplate>
                        <asp:Label ID="lblPartNumber" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RefNo") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Description"  SortExpression="RefName">
                    <ItemTemplate>
                        <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RefName") %>'></asp:Label>
                        
                    </ItemTemplate>
                </asp:TemplateColumn>
            
            </Columns>
        </asp:datagrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:label id="lblValue" runat="server" cssclass="lblXLong" text=""></asp:label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <TABLE width="100%" align="center"  class="footercommon" class="changebg">
                                <tr>
                                    <td width="350">
                                    </td>
                                    <td width="75">
                                        <asp:imagebutton id="cmdPrint" runat="server" tooltip="View Print Friendly Page"
                                            imageurl="images/btnPrint.png"></asp:imagebutton>
                                    </td>
                                    <td width="75">
                                        <asp:button id="cmdBack" runat="server" cssclass="cmdMedium" causesvalidation="False"
                                            tooltip="Return to Previously Viewed Screen" text="Back" visible="false"></asp:button>
                                    </td>
                                    <td width="75">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            </form>
        </div>
    </div>
</td>
<script type="text/javascript" src="js/jquery-1.3.2.min.js"></script>
<%  If ViewState("print") = "1" Then%>
        <script type="text/javascript">
            $(document).ready(function() {

            $('.changebg').css('background-color', '#FFFFFF');
            $('table.changebg td').css("background-image", "none");
        //        $('input').attr("readonly", "true");
        //        $('select').attr("readonly", "true");
        //        $('#cmdRefresh').hide();
        //        $('#cmdBack').attr("disabled", "");       
                
                
            });
        </script>
<%  End If%>
