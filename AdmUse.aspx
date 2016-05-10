<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdmUse.aspx.vb" Inherits="POSN.AdmUse"
    SmartNavigation="True" %>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
<script type="text/javascript">
    $(function () {
        $('#maintcontenttable').height($('#maintcontenttabletd').height());
    });

</script>
<td valign="top">
    <div id="Div1" dir="rtl" style="width: 100%; height: 100%" runat="server">
        <div dir="ltr" style="width: 100%; height: 100%">
            <form id="frmAdmUse" method="post" runat="server">
            <table width="100%" height="100%" align="center" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" background="">
                        <table width="100%" align="center">
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:label id="lblTitle" runat="server" text="Usage" cssclass="lblTitle"></asp:label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="20%">
                                </td>
                                <td align="center" width="60%">
                                    <asp:label id="lblFrom" runat="server" text="From" cssclass="lblXShort"></asp:label>
                                    <asp:textbox id="txtFrom" runat="server" cssclass="txtShort" maxlength="10"></asp:textbox>
                                    <asp:requiredfieldvalidator id="vtxtFrom" runat="server" controltovalidate="txtFrom"
                                        errormessage="* You must enter a From Date" display="dynamic">*</asp:requiredfieldvalidator>
                                    <asp:label id="lblTo" runat="server" text="To" cssclass="lblXShort"></asp:label>
                                    <asp:textbox id="txtTo" runat="server" cssclass="txtShort" maxlength="10"></asp:textbox>
                                    <asp:requiredfieldvalidator id="vtxtTo" runat="server" controltovalidate="txtTo"
                                        errormessage="* You must enter a To Date" display="dynamic">*</asp:requiredfieldvalidator>
                                    <asp:comparevalidator id="vCompare" runat="server" controltovalidate="txtFrom" errormessage="* From Date must be Less than To Date"
                                        display="dynamic" controltocompare="txtTo" operator="LessThanEqual" type="Date">*</asp:comparevalidator>
                                    &nbsp;
                                </td>
                                <td align="right" width="10%">
                                    <asp:imagebutton id="cmdRefresh" causesvalidation="True" tooltip="Refresh Search"
                                        runat="server" imageurl="images/btnRefresh.png"></asp:imagebutton>
                                </td>
                                <td align="right" width="15%">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" background="">
                        <table width="100%" align="center">
                            <tr>
                                <td align="right" colspan="1">
                                </td>
                                <td align="center" colspan="2">
                                    <asp:datagrid id="grd" runat="server" borderstyle="None" font-size="Smaller" bordercolor="Black"
                                        borderwidth="1px" cellpadding="3" showfooter="True" autogeneratecolumns="False"
                                        allowsorting="True">
											<SelectedItemStyle Font-Italic="True" CssClass="grdSelect"></SelectedItemStyle>
											<ItemStyle CssClass="grdBody"></ItemStyle>
											<HeaderStyle Font-Italic="True" Font-Bold="True" CssClass="grdHead"></HeaderStyle>
											<Columns>
											   <asp:TemplateColumn Visible="True" HeaderText="Review">
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
                                <td align="right" colspan="2">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="bottom">
                        <TABLE width="100%"  align="center"  class="footercommon">
                            <tr>
                                <td width="350">
                                </td>
                                <td width="75">
                                    <asp:imagebutton id="cmdPrint" runat="server" tooltip="View Print Friendly Page"
                                        imageurl="images/btnPrint.png"></asp:imagebutton>
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
            </table>
            </form>
        </div>
    </div>
</td>
