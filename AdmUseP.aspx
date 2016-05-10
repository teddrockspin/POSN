<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdmUseP.aspx.vb" Inherits="POSN.AdmUseP" %>

<td valign="top">
    <form id="frmAdmUseP" method="post" runat="server">
        <table width="100%" align="center" border="0">
            <tbody>
                <tr>
                    <td height="190">
                        <table width="100%" align="center">
                            <tbody>
                                <tr>
                                    <td align="center" width="80%">
                                        <asp:label id="lblHead" runat="server" text="" cssclass="lblDTitle"></asp:label>
                                    </td>
                                    <tr>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:datagrid id="grd" runat="server" borderstyle="None" font-size="Smaller" bordercolor="Black"
                            borderwidth="1px" cellpadding="3" showfooter="True" autogeneratecolumns="False"
                            allowsorting="True">
											<SelectedItemStyle Font-Italic="True" CssClass="grdSelect"></SelectedItemStyle>
											<ItemStyle CssClass="grdBody"></ItemStyle>
											<HeaderStyle Font-Italic="True" Font-Bold="True" CssClass="grdHead"></HeaderStyle>
											<Columns>
											   <asp:TemplateColumn Visible="false" HeaderText="Review">
                                                    <HeaderStyle Width="8%"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                     <ItemTemplate>
                                                       <asp:LinkButton runat=server id=lnkBtnDetails  CommandArgument='<%#Eval("ReferenceNo")%>' CommandName="ViewDetails">Details</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
												<asp:BoundColumn Visible="false" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:BoundColumn DataField="Status" HeaderText="Status"  SortExpression="Status">
													<HeaderStyle HorizontalAlign="center" Width="10%"></HeaderStyle>
													<ItemStyle HorizontalAlign="center"></ItemStyle>
												</asp:BoundColumn>												
												<asp:BoundColumn DataField="QTYUsed" HeaderText="Quantity Used"  SortExpression="QTYUsed">
													<HeaderStyle HorizontalAlign="center" Width="10%"></HeaderStyle>
													<ItemStyle HorizontalAlign="right"></ItemStyle>
												</asp:BoundColumn>
												
												<asp:BoundColumn DataField="QTYAdded" HeaderText="Quantity Added"  SortExpression="QTYAdded">
													<HeaderStyle HorizontalAlign="center" Width="10%"></HeaderStyle>
													<ItemStyle HorizontalAlign="right"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="QTYDisposed" HeaderText="Quantity Disposed"  SortExpression="QTYDisposed">
													<HeaderStyle HorizontalAlign="center" Width="10%"></HeaderStyle>
													<ItemStyle HorizontalAlign="right"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="FileDownloads" HeaderText="File Downloads"  SortExpression="FileDownloads">
													<HeaderStyle HorizontalAlign="center" Width="10%"></HeaderStyle>
													<ItemStyle HorizontalAlign="center"></ItemStyle>
												</asp:BoundColumn>
                                                <asp:BoundColumn DataField="Prefix" HeaderText="Prefix"  SortExpression="Prefix">
													<HeaderStyle Width="4%"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="ReferenceNo" HeaderText="Part Number"  SortExpression="ReferenceNo">
													<HeaderStyle Width="20%"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Description" HeaderText="Part Description"  SortExpression="Description" >
													<HeaderStyle Width="40%"></HeaderStyle>
												</asp:BoundColumn>	
                                                <asp:BoundColumn DataField="PrintMethod" HeaderText="Mode" SortExpression="PrintMethod">
                                                    <headerStyle width="20%" />
                                                </asp:BoundColumn>											
											</Columns>
										</asp:datagrid>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" align="center">
                            <tr>
                                <td align="center">
                                    <asp:button id="cmdBack" runat="server" cssclass="cmdMedium" tooltip="Return to Previously Viewed Screen"
                                        text="Back" causesvalidation="False"></asp:button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </form>
</td>
</TR></TBODY></TABLE>
