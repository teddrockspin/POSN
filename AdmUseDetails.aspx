<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdmUseDetails.aspx.vb"
    Inherits="POSN.AdmUseDetails" %>

<link type="text/css" href="css/demos.css" rel="stylesheet" />

<script type="text/javascript" src="http://code.jquery.com/jquery-1.4.1.min.js"></script>

<script type="text/javascript" src="js/jquery-ui-1.7.2.custom.min.js"></script>

<link type="text/css" href="themes/base/ui.all.css" rel="stylesheet" />


<script type="text/javascript">
    $(function () {
        $('#maintcontenttable').height($('#maintcontenttabletd').height());
    });

</script>
<td valign="top">
    <div id="Div1" dir="rtl" style="width: 100%; height: 100%" runat="server">
        <div dir="ltr" style="width: 100%; height: 100%">
            <form id="frmAdmUse" method="post" runat="server">
            <table width="100%"  height="100%"  align="center" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" background="">
                        <table width="100%" align="center">
                            <tr>
                                  <td align="right" colspan="1">
                                </td>
                                <td align="center" colspan="2" align="center">
                                    <div class="ui-widget">
                                        <div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
                                            <p>
                                                <strong><%=Request.QueryString("RefNumber")%></strong></p>
                                                <p><asp:Label runat=server id=lblDescrition></asp:Label></p>
                                        </div>
                                    </div>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="1">
                                </td>
                                <td align="center" colspan="2">
                                    <asp:datagrid id="grd" runat="server" borderstyle="None" font-size="Smaller" bordercolor="Black"
                                        borderwidth="1px" cellpadding="3" showfooter="True" autogeneratecolumns="true"
                                        allowsorting="True">
											<SelectedItemStyle Font-Italic="True" CssClass="grdSelect"></SelectedItemStyle>
											<ItemStyle CssClass="grdBody"></ItemStyle>
											<HeaderStyle Font-Italic="True" Font-Bold="True" CssClass="grdHead"></HeaderStyle>
											<Columns>
												<%--<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:BoundColumn DataField="ItmType" HeaderText="Status"  SortExpression="ItmType">
													<HeaderStyle HorizontalAlign="center" Width="10%"></HeaderStyle>
													<ItemStyle HorizontalAlign="center"></ItemStyle>
												</asp:BoundColumn>--%>
																						
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
                        <table width="100%" align="center" class="footercommon">
                            <tr>
                                <td width="350">
                                </td>
                                <td width="75">
                                    <asp:imagebutton id="cmdPrint" runat="server" tooltip="View Print Friendly Page"
                                        imageurl="images/btnPrint.png" visible="false"></asp:imagebutton>
                                    <td width="75">
                                        <asp:button id="cmdBack" runat="server" cssclass="cmdMedium" causesvalidation="False"
                                            tooltip="Return to Previously Viewed Screen" text="Back" visible="true"></asp:button>
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
