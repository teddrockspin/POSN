<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="POSN.Login"
    SmartNavigation="False" %>
<link type="text/css" href="css/demos.css" rel="stylesheet" />

<script type="text/javascript" src="http://code.jquery.com/jquery-1.4.1.min.js"></script>

<script type="text/javascript" src="js/jquery-ui-1.7.2.custom.min.js"></script>

<link type="text/css" href="themes/base/ui.all.css" rel="stylesheet" />
<script language="javascript">
    
</script>

<td valign="top" align="center">
    <div dir="rtl" style='width: 100%; height: 100%' runat='server' id="Div1">
        <div dir="ltr">
            <form id="frmMain" method="post" runat="server">

                <table width="100%">
                    <tr>
                        <td height="180px" background=''>
                            <table width="600px" align="center">
                                <tr>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                    <td align="center" colspan="2">
                                        <!--<B><font color=red>Under maintanence - site will be  back in 2 hours</font></b><BR>-->
                                        <asp:label id="lblTitle" runat="server" cssclass="lblTitle" text="Access Information - Login..."></asp:label>
                                    </td>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="6">
                                        <div class="ui-widget" style="width: 600px;" runat="server" id="divMessage">
                                            <div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
                                                <p>
                                                    <span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>
                                                    <strong>
                                                        <asp:label id="lblMsg" runat="server" cssclass="lblNoteInactive" text=""></asp:label>
                                                    </strong>
                                                </p>
                                            </div>
                                        </div>


                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="6">&nbsp;<br>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                    <td colspan="1" align="right">
                                        <asp:label id="lblCustCode" runat="server" cssclass="lblMedium" text="Customer Code:"></asp:label>
                                    </td>
                                    <td colspan="1" align="left">
                                        <asp:textbox id="txtCustCode" runat="server" maxlength="10" cssclass="txtMedium"></asp:textbox>
                                    </td>
                                    <td align="center" colspan="3">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                    <td colspan="1" align="right">
                                        <asp:label id="lblPwd" runat="server" cssclass="lblMedium" text="Password:"></asp:label>
                                    </td>
                                    <td colspan="1" align="left">
                                        <asp:textbox id="txtPwd" runat="server" textmode="Password" maxlength="10" cssclass="txtMedium"></asp:textbox>
                                    </td>
                                    <td align="center" colspan="3">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                    <td colspan="1" align="right">
                                        <asp:label id="lblAccCode" runat="server" cssclass="lblMedium" text="Access Code:"></asp:label>
                                    </td>
                                    <td colspan="1" align="left">
                                        <asp:textbox id="txtAccCode" runat="server" maxlength="20" cssclass="txtMedium"></asp:textbox>
                                    </td>
                                    <td align="center" colspan="3">&nbsp;
                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" align="center" class="footercommon">
                                <tr>
                                    <td width="350"></td>
                                    <td width="75">&nbsp;
                                    </td>
                                    <td width="75">
                                        <asp:imagebutton id="cmdNext" runat="server" imageurl='images/btnLogin.png' style="height: 18px"></asp:imagebutton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:linkbutton runat="server" id="btntestEmail" text="." visible="false" />
            </form>
        </div>
    </div>
</td>
