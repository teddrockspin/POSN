<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OrdCrt.aspx.vb" Inherits="POSN.OrdCrt"
    SmartNavigation="True" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<%--<script src="script/jquery-1.4.1.min.js" type="text/javascript"></script>--%>

<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.0/jquery.min.js"></script>
<script type="text/javascript" src="js/jquery-ui-1.7.2.custom.min.js"></script>

<link type="text/css" href="themes/base/ui.all.css" rel="stylesheet" />
<link rel="stylesheet" type="text/css" href="ordCrt.css" />



<script type="text/javascript">



    $(document).ready(function () {
        $("#dialog").dialog({
            modal: true,
            autoOpen: false,
            resizable: false,
            //        open: function(type, data) {
            //            $(this).parent().appendTo("form");
            //        }
            //                    ,
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                }
            }
        });

        $('#txtSubmit').hide();

        $("input.txtNumber").bind("change", function () {
            $.ajax({
                type: "GET",
                url: "OrdCrt.aspx",
                data: 'ajax=1&type=validateitems&qty=' + $(this).val(),
                success: function (msg) {
                    $(".cmdUpdateItem").attr("onclick", msg);
                },
                failure: function (r) {
                    console.log(r);
                }
            });
        });
    });

    function openDialog(s) {

        $("#divUploadInstructions").text(s);
        $("#dialog").dialog("open");

        return false;
    }

</script>



<div id="dialog" title="Upload Instructions">
    <div id="divUploadInstructions"></div>
</div>
<td align="center" valign="top">
    <div id="Div1" dir="rtl" style="width: 100%; height: 100%" runat="server">
        <div dir="ltr">
            <form id="frmOrdCrt" method="post" runat="server">
                <asp:hiddenfield runat="server" id="hfShowStock" value="false"></asp:hiddenfield>
                <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
                <table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td background="" valign="top">
                            <table width="100%" align="center">
                                <tr>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                    <td>
                                        <asp:textbox id="txtSubmit" runat="server" width="0" text=""></asp:textbox>
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:label id="lblTitle" runat="server" text="Shopping Cart Contents" cssclass="lblTitle"></asp:label>
                                    </td>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" background="">
                            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:label id="lblLI" runat="server" text="" cssclass="lblXLongBack"></asp:label>
                                    </td>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:label id="lblOR" runat="server" text="" cssclass="lblXLongBack"></asp:label>
                                    </td>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="6" valign="top">
                                        <div class="ui-corner-all roundbox" >
                                        <table width="85%" align="center" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="center" style="padding-bottom: 10px;">
                                                    <div class="alert" style="width: 600px" runat="server" id="dUploadMessage" visible="false">
                                                        <div>
                                                            <div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
                                                                <p style="font-size: 13px !important;">
                                                                    <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
                                                                    <strong>Note: items in your shopping cart require a file to be uploaded.<br />
                                                                        Browse for the correct file, then click "Update" to execute upload.</strong>
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6" align="center">
                                                    <telerik:RadProgressManager ID="Radprogressmanager1" runat="server" />
                                                    <telerik:RadProgressArea runat="server" ID="ProgressArea1" DisplayCancelButton="true">
                                                    </telerik:RadProgressArea>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td background="">
                                                    <asp:table id="tblITtl" runat="server" horizontalalign="center"></asp:table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td background="">
                                                    <asp:table id="tblIQty" runat="server" horizontalalign="center" cellpadding="5"></asp:table>
                                                    <asp:table id="tblIUp" runat="server" horizontalalign="right">
														<asp:tablerow>
															<asp:tablecell width="80%">
																<asp:imagebutton id="cmdUpdateItem" runat="server" CssClass="cmdUpdateItem" ToolTip="Save Item Updates" ImageUrl="images/btnUpdate.png"></asp:imagebutton>
															</asp:tablecell>
														</asp:tablerow>
													</asp:table>
                                                </td>
                                            </tr>
                                           
                                        </table>
                                            </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="6" valign="top">
                                        <div class="ui-corner-all roundbox" >
                                        <table width="85%" align="center" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td background="">
                                                    <asp:table id="tblDnlTtl" runat="server" horizontalalign="center"></asp:table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td background="">
                                                    <asp:table id="tblDnlQty" runat="server" horizontalalign="center" cellpadding="5"></asp:table>
                                                    <asp:table id="tblDnlUp" runat="server" horizontalalign="right">
														<asp:tablerow>
															<asp:tablecell width="80%">
																<asp:imagebutton id="cmdUpdateItemDigital" runat="server" ToolTip="Save Item Updates" ImageUrl="images/btnUpdate.png"></asp:imagebutton>
															</asp:tablecell>
														</asp:tablerow>
													</asp:table>
                                                </td>
                                            </tr>

                                        </table>
                                            </div> 
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="6" valign="top">
                                        <div class="ui-corner-all roundbox ">
                                        <table width="85%" align="center" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td background="">
                                                    <asp:table id="tblKTtl" runat="server" horizontalalign="center"></asp:table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td background="" align="center">
                                                    <asp:table id="tblKQty" runat="server" horizontalalign="center" cellpadding="5"></asp:table>
                                                    <div>
                                                    </div>
                                                    <asp:table id="tblKUp" runat="server" horizontalalign="right">
														<asp:tablerow>
															<asp:tablecell width="80%">
																<asp:imagebutton id="cmdUpdateKit" runat="server" ToolTip="Save Kit Updates" ImageUrl="images/btnUpdate.png"></asp:imagebutton>
															</asp:tablecell>
														</asp:tablerow>
													</asp:table>
                                                </td>
                                            </tr>
                                     
                                        </table>
                                            </div> 
                                    </td>
                                </tr>
                                       <tr>
                                                <td align="center" colspan="4" class="subnotes">Note: Items in <font color="#bd5304">
												orange</font> are on backorder and not available.</td>
                                            </tr>
                                         <%--<tr>
                                            <td align="center" colspan="4" class="subnotes">Note: Items in <font color="red" >
												red</font> are inactive items that are no longer available.</td>
                                          </tr>--%>
                                <tr><td>&nbsp;</td></tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <TABLE width="100%" align="center"  class="footercommon">
                                <tbody>
                                    <tr>
                                        <td width="250">&nbsp;
                                        </td>
                                        <td width="175" valign="middle" align="right">
                                            <asp:label id="Label1" runat="server" text="Continue Shopping" cssclass="lblDLong"></asp:label>
                                        </td>
                                        <td width="75">
                                            <asp:imagebutton id="cmdItem" runat="server" tooltip="View Item Catalog" imageurl="images/btnItmCat.png"
                                                causesvalidation="False"></asp:imagebutton>
                                        </td>
                                        <td width="75">
                                            <asp:imagebutton id="cmdKit" runat="server" tooltip="View Kit Catalog" imageurl="images/btnKitCat.png"
                                                causesvalidation="False"></asp:imagebutton>
                                        </td>
                                        <td width="75">&nbsp;
                                        </td>
                                        <td width="75">
                                            <asp:imagebutton id="cmdNext" runat="server" imageurl="images/btnContChk.png" tooltip="Proceed with Order"></asp:imagebutton>
                                            <asp:imagebutton id="cmdEditBack" runat="server" visible="False" imageurl="~/images/btnBack.png"
                                                tooltip="Back"></asp:imagebutton>
                                        </td>
                                        <td width="75">
                                            <asp:imagebutton id="cmdSubmit" runat="server" imageurl="images/btnSubmit.png" tooltip="Submit Order"
                                                visible="false"></asp:imagebutton>
                                        </td>
                                        <td width="75"></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </table>
            </form>
        </div>
    </div>
</td>

