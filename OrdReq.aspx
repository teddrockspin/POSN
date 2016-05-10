<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OrdReq.aspx.vb" Inherits="POSN.OrdReq"
    SmartNavigation="True" %>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
<script type="text/javascript">
    $(function () {
        $('#maintcontenttable').height($('#maintcontenttabletd').height());
    });
    
</script>
<form id="frmOrdReq" method="post" runat="server">\
<td valign="top">
    <div dir="rtl" style="width: 100%; height: 100%">
        <div dir="ltr">
            
            <table width="100%" height="100%" align="center" border="0" cellpadding="0" cellspacing="0"   style="height:100%;">
                <tr>
                    <td>
                        <table width="100%" align="center" >
                            <tr>
                                <td align="center" colspan="1">
                                    &nbsp;
                                </td>
                                <td align="center" colspan="2">
                                    <asp:label id="lblTitle" runat="server" text="Requestor Information" cssclass="lblTitle"></asp:label>
                                </td>
                                <td align="center" colspan="1">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table width="100%" style="height:100%;" height="100%"  align="center">
                            <tr>
                                <td align="center" colspan="10">
                                    <asp:table id="tblCst" runat="server" width="100%" horizontalalign="center" backcolor="Transparent">
											<asp:TableRow>
												<asp:TableCell colSpan="4">&nbsp;</asp:TableCell>
												<asp:TableCell align="center" ColSpan="2">
													<asp:imagebutton id="cmdClear" ToolTip="Clear All" ImageURL="images/btnClear.png" Runat="server"></asp:imagebutton>&nbsp;
													<asp:imagebutton id="cmdSearch" ToolTip="Check Previous Requestor List (By Last Name)" ImageURL="images/btnSearch.png"
														Runat="server" visible="true"></asp:imagebutton>
												</asp:TableCell>
												<asp:TableCell colSpan="1">&nbsp;</asp:TableCell>
											</asp:TableRow>
											<asp:TableRow>
												<asp:TableCell align="center" colSpan="1">&nbsp;</asp:TableCell>
												<asp:TableCell align="center" colSpan="2">&nbsp;</asp:TableCell>
												<asp:TableCell colspan="1" align="right">
													<asp:Label id="lblRqFName" runat="server" CssClass="lblMediumLong" text="Given/First Name:"></asp:Label>
												</asp:TableCell>
												<asp:TableCell colspan="1" align="left">
													<asp:textbox id="txtRqFName" runat="server" MaxLength="20" CssClass="txtMediumLong"></asp:textbox>
													<asp:label id="lblRqFNameR" runat="server" text=" * " CssClass="lblRequired"></asp:label>
												</asp:TableCell>
												<asp:TableCell align="center" colSpan="2">&nbsp;</asp:TableCell>
											</asp:TableRow>
											<asp:TableRow>
												<asp:TableCell align="center" colSpan="1">&nbsp;</asp:TableCell>
												<asp:TableCell align="center" colSpan="2">&nbsp;</asp:TableCell>
												<asp:TableCell colspan="1" align="right">
													<asp:Label id="lblRqLName" runat="server" CssClass="lblMediumLong" text="Family/Last Name:"></asp:Label>
												</asp:TableCell>
												<asp:TableCell colspan="1" align="left">
													<asp:textbox id="txtRqLName" runat="server" MaxLength="20" CssClass="txtMediumLong"></asp:textbox>
													<asp:label id="lblRqLNameR" runat="server" text=" * " CssClass="lblRequired"></asp:label>
												</asp:TableCell>
												<asp:TableCell align="center" colSpan="2">&nbsp;</asp:TableCell>
											</asp:TableRow>
											<asp:TableRow>
												<asp:TableCell align="center" colSpan="1">&nbsp;</asp:TableCell>
												<asp:TableCell align="center" colSpan="2">&nbsp;</asp:TableCell>
												<asp:TableCell colspan="1" align="right" height="27">
													<asp:Label id="lblRqEmail" runat="server" CssClass="lblMediumLong" text="E-mail:"></asp:Label>
												</asp:TableCell>
												<asp:TableCell colspan="1" align="left" height="27">
													<asp:textbox id="txtRqEmail" runat="server" MaxLength="50" CssClass="txtMediumLong"></asp:textbox>
													<asp:label id="lblRqEmailR" runat="server" text=" * " CssClass="lblRequired"></asp:label>
												</asp:TableCell>
												<asp:TableCell align="center" colSpan="2">&nbsp;</asp:TableCell>
											</asp:TableRow>
                                            <asp:TableRow id="trConfirmEmail" runat="server">
												<asp:TableCell align="center" colSpan="1">&nbsp;</asp:TableCell>
												<asp:TableCell align="center" colSpan="2">&nbsp;</asp:TableCell>
												<asp:TableCell colspan="1" align="right" height="27">
													<asp:Label id="Label1" runat="server" CssClass="lblMediumLong" text="Confirm E-mail:"></asp:Label>
												</asp:TableCell>
												<asp:TableCell colspan="1" align="left" height="27">
													<asp:textbox id="txtEmailConfirm" runat="server" MaxLength="50" CssClass="txtMediumLong"></asp:textbox>
													<asp:CompareValidator ID="CompareValidator1"  runat="server" ErrorMessage=" Emails do not match" ValidationGroup="reqinfo" Display="Dynamic"  ControlToValidate="txtEmailConfirm" ControlToCompare="txtRqEmail" CssClass="lblRequired"></asp:CompareValidator><asp:RequiredFieldValidator runat="server" ID="rfval" ErrorMessage=" Please confirm your email" ValidationGroup="reqinfo" Display="Dynamic" ControlToValidate="txtEmailConfirm" CssClass="lblRequired"></asp:RequiredFieldValidator></asp:TableCell>
												<asp:TableCell align="center" colSpan="2">&nbsp;</asp:TableCell>
											</asp:TableRow>
											<asp:TableRow>
												<asp:TableCell align="center" colSpan="5">&nbsp;</asp:TableCell>
												<asp:TableCell align="center" colSpan="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:TableCell>
											</asp:TableRow>
										</asp:table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
               
            </table>
            
        </div>
    </div>
</td>
</tr>
 <tr>
                    <td valign="bottom">
                        <TABLE width="100%" align="center"  class="footercommon">
                            <tr>
                                <td width="350">
                                    <input id="txtSubmit" name="txtSubmit" runat="server" type="hidden">
                                    <input id="txtItmRow" name="txtItmRow" runat="server" type="hidden" value="0">
                                </td>
                                <td width="75">
                                    <asp:button id="cmdBack" runat="server" cssclass="cmdMedium" tooltip="Disregard Changes and Return to Previously Viewed Screen"
                                        text="Back" causesvalidation="False" visible="False"></asp:button>
                                </td>
                                <td width="75">
                                    <asp:imagebutton id="cmdNext" runat="server" tooltip="Save Information and Proceed with Order"
                                        imageurl="images/btnContChk.png"  validationgroup="reqinfo"></asp:imagebutton>
                                    <asp:imagebutton id="cmdEditBack" runat="server" visible="False" imageurl="images/btnSave.png"
                                        tooltip="Back"></asp:imagebutton>
                                </td>
                                <td width="75">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

</form>
    </table>
