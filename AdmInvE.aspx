<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdmInvE.aspx.vb" Inherits="POSN.AdmInvE"
    SmartNavigation="True" %>

<link type="text/css" href="css/demos.css" rel="stylesheet" />

<script type="text/javascript" src="http://code.jquery.com/jquery-1.4.1.min.js"></script>

<script type="text/javascript" src="js/jquery-ui-1.7.2.custom.min.js"></script>

<link type="text/css" href="themes/base/ui.all.css" rel="stylesheet" />
<td valign="top">
    <div dir="rtl" style='overflow: auto; width: 100%; height: 100%' runat='server' id="Div1">
        <div dir="ltr">
            <form id="frmAdmInvE" method="post" runat="server">
            <table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" background="">
                        <table width="100%" align="center">
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:label id="lblTitle" runat="server" cssclass="lblTitle" text="Inventory Summary"></asp:label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" background="">
                        <table width="100%" align="center">
                            <tr>
                                <td>
                                    <table width="100%" align="center" bgcolor="transparent">
                                        <tr>
                                            <td align="center" colspan="8">
                                                <asp:label id="lblItm" runat="server" cssclass="lblXLongDescriptor"></asp:label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="8">
                                                <asp:label id="lblItmWarn" runat="server" visible="False"></asp:label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="8">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                            <td align="right" colspan="4">
                                                <asp:label id="lblActDate" runat="server" cssclass="lblMediumLong" text="Active Date:"></asp:label>
                                            </td>
                                            <td align="left" colspan="2">
                                                <asp:label id="txtActDate" runat="server" cssclass="lblMedium"></asp:label>
                                            </td>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                            <td align="right" colspan="4">
                                                <asp:label id="lblInActDate" runat="server" cssclass="lblMediumLong" text="Projected Inactive Date:"></asp:label>
                                            </td>
                                            <td align="left" colspan="2">
                                                <asp:label id="txtInActDate" runat="server" cssclass="lblMedium"></asp:label>
                                            </td>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                            <td align="right" colspan="4">
                                                <asp:label id="lblUVal" runat="server" cssclass="lblMediumLong" text="Unit Value:"></asp:label>
                                            </td>
                                            <td align="left" colspan="2">
                                                <asp:label id="txtUVal" runat="server" cssclass="lblMedium"></asp:label>
                                            </td>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                            <td align="right" colspan="4">
                                                <asp:label id="lblFirstUse" runat="server" cssclass="lblMediumLong" text="First Usage Date:"></asp:label>
                                            </td>
                                            <td align="left" colspan="2">
                                                <asp:label id="txtFirstUse" runat="server" cssclass="lblMedium"></asp:label>
                                            </td>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                            <td align="right" colspan="4">
                                                <asp:label id="lblLastUse" runat="server" cssclass="lblMediumLong" text="Last Usage Date:"></asp:label>
                                            </td>
                                            <td align="left" colspan="2">
                                                <asp:label id="txtLastUse" runat="server" cssclass="lblMedium"></asp:label>
                                            </td>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                            <td align="right" colspan="4">
                                                <asp:label id="lblLastQty" runat="server" cssclass="lblMediumLong" text="Last Quantity Ordered:"></asp:label>
                                            </td>
                                            <td align="left" colspan="2">
                                                <asp:label id="txtLastQty" runat="server" cssclass="lblMedium"></asp:label>
                                            </td>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                            <td align="right" colspan="4">
                                                <asp:label id="lblMonthsRv" runat="server" cssclass="lblMediumLong" text="Weeks Reviewed:"></asp:label>
                                            </td>
                                            <td align="left" colspan="2">
                                                <asp:label id="txtMonthsRvw" runat="server" cssclass="lblMedium"></asp:label>
                                            </td>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                            <td align="right" colspan="4">
                                                <asp:label id="lblQty" runat="server" cssclass="lblMediumLong" text="Quantity Remaining:"></asp:label>
                                            </td>
                                            <td align="left" colspan="2">
                                                <asp:label id="txtQty" runat="server" cssclass="lblMedium"></asp:label>
                                            </td>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                            <td align="right" colspan="4">
                                                <asp:label id="lblTVal" runat="server" cssclass="lblMediumLong" text="Total Value:"></asp:label>
                                            </td>
                                            <td align="left" colspan="2">
                                                <asp:label id="txtTVal" runat="server" cssclass="lblMedium"></asp:label>
                                            </td>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                            <td align="right" colspan="4">
                                                <asp:label id="lblAvgWk" runat="server" cssclass="lblMediumLong" text="Average Per Week Usage:"></asp:label>
                                            </td>
                                            <td align="left" colspan="2">
                                                <asp:label id="txtAvgWk" runat="server" cssclass="lblMedium"></asp:label>
                                            </td>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                            <td align="right" colspan="4">
                                                <asp:label id="lblWeeksRm" runat="server" cssclass="lblMediumLong" text="Weeks Remaining:"></asp:label>
                                            </td>
                                            <td align="left" colspan="2">
                                                <asp:label id="txtWeeksRm" runat="server" cssclass="lblMedium"></asp:label>
                                            </td>
                                            <td align="center" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="8">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="8">
                                                <div class="ui-widget" style="width:400px;" runat=server id=divWarn visible=false>
                                                    <div class="ui-state-error ui-corner-all" style="padding: 0 .7em;">
                                                        <p>
                                                            <span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>
                                                            <strong>
                                                                <asp:label id="lblWarn" runat="server" cssclass="lblNoteInactive" visible="False"></asp:label>
                                                            </strong>
                                                        </p>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" align="center" bgcolor="black">
                            <tr>
                                <td width="350">
                                </td>
                                <td width="75">
                                    <asp:imagebutton id="cmdBack" runat="server" tooltip="Return to Previously Viewed Screen"
                                        imageurl="images/btnBack.png"></asp:imagebutton>
                                    <%--<a href="#" onclick="history.go(-1);return false;">Back</a>--%>
                                </td>
                                <td width="75">
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
