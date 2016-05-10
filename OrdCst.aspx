<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OrdCst.aspx.vb" Inherits="POSN.OrdCst"
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
            <form id="frmOrdCst" method="post" runat="server">
            <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0" height='100%'>
                <tr>
                    <td background="">
                        <table width="100%" align="center" bgcolor="transparent">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:label id="lblTitle" runat="server" text="Custom Information" cssclass="lblTitle"></asp:label>
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
                    <td valign="top" background="">
                        <table width="100%" align="center">
                            <tr>
                                <td align="center" colspan="7">
                                    <asp:table id="tblCst" runat="server" width="100%" horizontalalign="center" bgcolor="Transparent">
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="6">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:tablecell>
											</asp:tablerow>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="3">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="lblCoverSheet" runat="server" text="Cover Sheet:" CssClass="lblMediumLong"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1">
													<asp:dropdownlist id="cboCVID" runat="server" CssClass="cboMedium" AutoPostBack="True"></asp:dropdownlist>
													<asp:label id="lblCoverSheetR" runat="server" text=" * " CssClass="lblRequired"></asp:label>
													<asp:HyperLink id="lkCS" runat="server" imageURL="images/btnPreview.png"></asp:HyperLink>
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
											</asp:tablerow>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="3">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="lblCVText" runat="server" text="Cover Sheet Detail:" CssClass="lblMediumLong"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="3">
													<asp:textbox id="txtCVText" runat="server" CssClass="txtXLongL" TextMode="MultiLine" MaxLength="255"
														style="OVERFLOW: hidden;" onKeyUp="Count(this,400)" onChange="Count(this,400)"></asp:textbox>
												</asp:tablecell>
											</asp:tablerow>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="5">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:tablecell>
											</asp:tablerow>
										</asp:table>
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
                                    <input id="txtSubmit" name="txtSubmit" runat="server" type="hidden">
                                    <input id="txtItmRow" name="txtItmRow" runat="server" type="hidden" value="0">
                                </td>
                                <td width="75">
                                    <asp:button id="cmdBack" runat="server" cssclass="cmdMedium" visible="False" causesvalidation="False"
                                        text="Back" tooltip="Disregard Changes and Return to Previously Viewed Screen"></asp:button>
                                </td>
                                <td width="75">
                                    <asp:imagebutton id="cmdNext" runat="server" tooltip="Save Information and Proceed with Order"
                                        imageurl="images/btnContChk.png"></asp:imagebutton>
                                    <asp:imagebutton id="cmdEditBack" runat="server" visible="False" imageurl="images/btnSave.png"
                                        tooltip="Back"></asp:imagebutton>
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

<script language="javascript">
function Count(text,long) 
{
	var maxlength = new Number(long); // Change number to your max length.
	if (text.value.length > maxlength){
		text.value = text.value.substring(0,maxlength);
		alert(" Cover Sheet Details may only be " + long + " characters.");
	}
}
</script>

