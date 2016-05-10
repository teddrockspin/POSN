<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdmRsp.aspx.vb" Inherits="POSN.AdmRSP"
    SmartNavigation="True" %>

<link rel="stylesheet" type="text/css" href="AdmRSP.css" />

<td valign="top">
    <div id="Div1" dir="rtl" runat="server" style='width: 100%; height: 100%'>
        <div dir="ltr">
            <form id="frmAdmRSP" method="post" runat="server">
            <table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" background="">
                        <table width="100%" align="center" border="0">
                            <tr>
                                <td align="center" colspan="6">
                                    <asp:label id="Label1" runat="server" text="Restock Planner" cssclass="lblTitle"></asp:label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="10%">
                                </td>
                                <td align="right" width="30%">
                                    <asp:label id="lblDays" runat="server" text="Restock level to:" cssclass="lblLong"></asp:label>
                                </td>
                                <td align="left" width="40%">
                                    <asp:textbox id="txtDays" runat="server" cssclass="txtNumber" maxlength="5"></asp:textbox>
                                    <asp:label id="lblDays2" runat="server" text="&nbsp;&nbsp;days" cssclass="lblXShort"></asp:label>
                                </td>
                                <td align="right" width="10%">
                                    &nbsp;
                                </td>
                                <td align="right" width="10%">
                                    <asp:imagebutton id="cmdRefresh" causesvalidation="True" tooltip="Refresh Search"
                                        runat="server" imageurl="images/btnRefresh.png" style="height: 16px"></asp:imagebutton>
                                </td>
                                <td align="right" width="10%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="10%">
                                </td>
                                <td align="right" width="20%">
                                    <asp:label id="lblShowItems" runat="server" text="Show Items with Inventory Below:"
                                        cssclass="lblLong"></asp:label>
                                </td>
                                <td align="left" width="40%">
                                    <asp:textbox id="txtShowItems" runat="server" cssclass="txtNumber" maxlength="5"></asp:textbox>
                                    <span class="lblLong">weeks supply</span>
                                </td>
                                <td align="left" width="10%">
                                    &nbsp;
                                </td>
                                <td align="left" width="10%">
                                    &nbsp;
                                </td>
                                <td align="right" width="20%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="10%">
                                </td>
                                <td align="right" width="20%">
                                    <asp:label id="lblRound" runat="server" text="Round Required Quantities to Nearest:"
                                        cssclass="lblLong"></asp:label>
                                </td>
                                <td align="left" width="40%">
                                    <asp:textbox id="txtRound" runat="server" cssclass="txtNumber" maxlength="5"></asp:textbox>
                                </td>
                                <td align="left" width="10%">
                                    &nbsp;
                                </td>
                                <td align="left" width="10%">
                                    &nbsp;
                                </td>
                                <td align="right" width="20%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="10%">
                                </td>
                                <td align="right" width="20%">
                                    <asp:label id="lblFilterItmType" runat="server" text="Filter By Item Type:" cssclass="lblLong"></asp:label>
                                </td>
                                <td align="left" width="40%">
                                    <asp:dropdownlist id="cboItmType" runat="server" cssclass="cboShort"></asp:dropdownlist>
                                </td>
                                <td align="left" width="10%">
                                    &nbsp;
                                </td>
                                <td align="left" width="10%">
                                    &nbsp;
                                </td>
                                <td align="right" width="20%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="10%">
                                </td>
                                <td align="right" width="20%">
                                    <asp:label id="lblFilterPart" runat="server" text="Filter By Part No:" cssclass="lblLong"></asp:label>
                                </td>
                                <td align="left" width="40%">
                                    <asp:textbox id="txtPartNo" runat="server" cssclass="txtMedium" maxlength="10"></asp:textbox>
                                </td>
                                <td align="left" width="10%">
                                    &nbsp;
                                </td>
                                <td align="left" width="10%">
                                    &nbsp;
                                </td>
                                <td align="right" width="20%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="10%">
                                </td>
                                <td align="right" width="20%">
                                    <asp:label id="lblFilterStatus" runat="server" text="Filter By Status:" cssclass="lblLong"></asp:label>
                                </td>
                                <td align="left" width="40%">
                                    <asp:dropdownlist runat="server" id="ddlStatusFilter" cssclass="cboShort">
                                        <asp:ListItem>ALL</asp:ListItem>
                                        <asp:ListItem>POD</asp:ListItem>
                                        <asp:ListItem>F/P</asp:ListItem>
                                        <asp:ListItem>FUL</asp:ListItem>
									
									</asp:dropdownlist>
                                </td>
                                <td align="left" width="10%">
                                    &nbsp;
                                </td>
                                <td align="left" width="10%">
                                    &nbsp;
                                </td>
                                <td align="right" width="20%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="10%">
                                </td>
                                <td align="right" width="20%">
                                    <asp:label id="lblSort" runat="server" text="Sort By:" cssclass="lblLong"></asp:label>
                                </td>
                                <td align="left" width="40%">
                                    <asp:dropdownlist id="cboSort1" runat="server" cssclass="cboShort"></asp:dropdownlist>
                                    <asp:label id="lblThen1" runat="server" text="&nbsp;&nbsp;then&nbsp;&nbsp;" cssclass="lblXShort"></asp:label>
                                    <asp:dropdownlist id="cboSort2" runat="server" cssclass="cboShort"></asp:dropdownlist>
                                    <asp:label id="lblThen2" runat="server" text="&nbsp;&nbsp;then&nbsp;&nbsp;" cssclass="lblXShort"></asp:label>
                                    <asp:dropdownlist id="cboSort3" runat="server" cssclass="cboShort"></asp:dropdownlist>
                                </td>
                                <td align="left" width="10%">
                                    &nbsp;
                                </td>
                                <td align="left" width="10%">
                                    &nbsp;
                                </td>
                                <td align="right" width="20%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                </td>
                                <td align="right" valign="middle">
                                    <asp:label runat="server" text="Show details" cssclass="lblLong"></asp:label>
                                </td>
                                <td align="left" valign="middle">
                                    <input type="checkbox" id="chkHide" />
                                </td>
                                <td align="right">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="right" valign="middle">
                                    <asp:label id="lblExZero" runat="server" text="Exclude Items that do NOT Require Additional Inventory:"
                                        cssclass="lblLong"></asp:label>
                                </td>
                                <td align="left" valign="middle">
                                    <asp:checkbox id="chkExZeroQty" runat="server" checked="True"></asp:checkbox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" background="">
                        <table width="100%" align="center">
                            <tr>
                                <td align="center" colspan="8">
                                    <asp:table id="tblRsp" runat="server" horizontalalign="center" bgcolor="Transparent"
                                        width="100%" cellspacing="0" cellpadding="0" cssclass="cellstohide"></asp:table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="1">
                                </td>
                                <td align="center" colspan="3">
                                </td>
                                <td align="right" colspan="2">
                                </td>
                                <tr>
                                    <td align="right" colspan="1">
                                    </td>
                                    <td align="left" colspan="3">
                                        <table style="display: none;">
                                            <tr>
                                                <td bgcolor="red">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:label id="lblNote" runat="server" text=" - Scheduled for Obsolescence" cssclass="lblLongSmall"></asp:label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="1">
                                    </td>
                                    <td align="left" colspan="4">
                                        <asp:label id="lblNote2" runat="server" text="FUL - Fulfill Only, POD - POD Only, F/P - Fulfill then POD as needed"
                                            cssclass="lblLongSmall"></asp:label>
                                    </td>
                                    <td align="right" colspan="1">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="1">
                                    </td>
                                    <td align="right" colspan="4">
                                        <asp:label id="lblTotCost" runat="server" text="Total Cost:" cssclass="lblMedium"></asp:label>
                                        <asp:label id="txtTotCost" runat="server" cssclass="txtNumberLong"></asp:label>
                                    </td>
                                    <td align="right" colspan="1">
                                        &nbsp;
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
                                    <asp:imagebutton id="cmdPrint" runat="server" tooltip="View Print Friendly Page"
                                        imageurl="images/btnPrint.png"></asp:imagebutton>
                                </td>
                                <td width="75">
                                    <asp:button id="cmdBack" runat="server" cssclass="cmdMedium" causesvalidation="False"
                                        tooltip="Return to Previously Viewed Screen" text="Back" visible="false"></asp:button>
                                </td>
                    </td>
                    <td width="75">
                    </td>
                </tr>
            </table>
</td>
</tr> </table> </form> </div> </div> </td>

<script type="text/javascript" src="js/jquery-1.3.2.min.js"></script>

<script type="text/javascript" src="js/jquery-ui-1.7.2.custom.min.js"></script>

<script type="text/javascript" src="js/jquery.column.min.js"></script>

<link type="text/css" href="themes/base/ui.all.css" rel="stylesheet" />

<script type="text/javascript">
    $(document).ready(function() {
        $('#chkHide').attr('checked', false);
        Hide();
    });


    $('#chkHide').click(function() {

        if ($('#chkHide').attr('checked') == false) {
            Hide();

        }
        else {
            Show();
            //            $('#tblUsage tbody td:nth-col(1)').show();
            //            $('#tblUsage tbody td:nth-col(2)').show();
            //            $('#tCellHeader1').hide();
        }
    });



    function Show() {
        $('#tblRsp tbody td:nth-col(6)').show();
        $('#tblRsp tbody td:nth-col(7)').show();
        $('#tblRsp tbody td:nth-col(8)').show();
        $('#tblRsp tbody td:nth-col(9)').show();
        $('#tblRsp tbody td:nth-col(10)').show();
        $('#tblRsp tbody td:nth-col(11)').show();
        $('#tblRsp tbody td:nth-col(12)').show();

        //$('#tCellUsageHeader').attr('colspan', 5);
        $('#tCellUsageHeader').each(function() { this.colSpan = 5; }); 
    }

    function Hide() {
        $('#tblRsp tbody td:nth-col(6)').hide();
        $('#tblRsp tbody td:nth-col(7)').hide();
        $('#tblRsp tbody td:nth-col(8)').hide();
        $('#tblRsp tbody td:nth-col(9)').hide();
        $('#tblRsp tbody td:nth-col(10)').hide();
        $('#tblRsp tbody td:nth-col(11)').hide();
        $('#tblRsp tbody td:nth-col(12)').hide();
        $('#tCellUsageHeader').show();
        //$('#tCellUsageHeader').attr('colspan', 3);
        $('#tCellUsageHeader').each(function() { this.colSpan = 3; }); 
      
    }
</script>

<%  If ViewState("print") = 1 Then%>

<script type="text/javascript">
    $(document).ready(function() {

        $('table').css('background-color', '#FFFFFF');
        $('input').attr("readonly", "true");
        $('select').attr("readonly", "true");
        $('#cmdRefresh').hide();
        $('#cmdBack').attr("disabled", "");
    });
</script>

<%  End If%>
