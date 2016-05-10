<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AdmKitE.aspx.vb" Inherits="POSN.AdmKitE" smartNavigation="True"%>
<script type="text/javascript">
    $(function () {

        $('#maintcontenttable').height($('#maintcontenttabletd').height());

    });

</script>
<td valign="top">
	<div id="Div1" style="WIDTH: 100%; HEIGHT: 100%" runat="server">
		<div dir="ltr" style="width: 100%; height: 100%">
			<form id="frmAdmKitE" method="post" runat="server">
				<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
					<tr>
						<td background="">
							<table width="100%" align="center">
								<TR>
									<TD align="center" colSpan="1">&nbsp;</TD>
									<td align="center" colSpan="4"><asp:label id="lblTitle" runat="server" text="Kit Edit" CssClass="lblTitle"></asp:label></td>
									<TD align="center" colSpan="1">&nbsp;</TD>
								</TR>
								<TR>
									<TD align="center" colSpan="6">&nbsp;</TD>
								</TR>
								<tr>
									<td align="center" colSpan="4"><asp:label id="lblNoteTmp" runat="server" text="Note:  To modify the current kit, please enter a new description.  Click on Add item to select new items to add to the kit.  Click Save to temporarily add your modified kit to the Kit Catalog and/or Add to Cart to add to your order.  This kit will stay active only for the current session."
											CssClass="lblNote" visible="false"></asp:label></td>
								</tr>
								<tr>
									<TD align="center" colSpan="1">&nbsp;</TD>
									<td align="right" colSpan="1"><asp:label id="lblCat" runat="server" text="Main Category:" CssClass="lblMedium"></asp:label></td>
									<td align="left" colSpan="1"><asp:dropdownlist id="cboCat" runat="server" CssClass="cboMedium"></asp:dropdownlist></td>
									<TD align="center" colSpan="3">&nbsp;</TD>
								</tr>
								<tr>
									<TD align="center" colSpan="1">&nbsp;</TD>
									<td align="right" colSpan="1"><asp:label id="lblRefNo" runat="server" text="Reference Number:" CssClass="lblMedium"></asp:label></td>
									<td align="left" colSpan="1"><asp:textbox id="txtRefNo" runat="server" CssClass="txtMedium" MaxLength="15"></asp:textbox><asp:requiredfieldvalidator id="vRefNo" runat="server" ErrorMessage="*" ControlToValidate="txtRefNo"></asp:requiredfieldvalidator></td>
									<TD align="center" colSpan="3">&nbsp;</TD>
								</tr>
								<tr>
									<TD align="center" colSpan="1">&nbsp;</TD>
									<td align="right" colSpan="1"><asp:label id="lblRefName" runat="server" text="Description:" CssClass="lblMedium"></asp:label></td>
									<td align="left" colSpan="1"><asp:textbox id="txtRefName" runat="server" CssClass="txtLong" MaxLength="50"></asp:textbox></td>
									<TD align="center" colSpan="3">&nbsp;</TD>
								</tr>
								<tr>
									<TD align="center" colSpan="1">&nbsp;</TD>
									<td align="right" colSpan="1"><asp:label id="lblGlobal" runat="server" text="Define Globally:" CssClass="lblMedium"></asp:label></td>
									<td align="left" colSpan="1"><asp:checkbox id="chkGlobal" runat="server" Enabled="False"></asp:checkbox></td>
									<TD align="center" colSpan="3">&nbsp;</TD>
								</tr>
								<tr>
									<TD align="center" colSpan="1">&nbsp;</TD>
									<td align="right" colSpan="1"><asp:label id="lblAbleToModify" runat="server" text="Able To Modify:" CssClass="lblMedium"></asp:label></td>
									<td align="left" colSpan="1"><asp:checkbox id="chkEnModify" runat="server"></asp:checkbox></td>
									<TD align="center" colSpan="3">&nbsp;</TD>
								</tr>
								<tr>
									<TD align="center" colSpan="1">&nbsp;</TD>
									<td align="right" colSpan="1"><asp:label id="lblNote" runat="server" text="Description:" CssClass="lblMedium"></asp:label></td>
									<td align="left" colSpan="3"><asp:textbox id="txtNote" onkeyup="Count(this,255)" style="OVERFLOW: hidden" runat="server" CssClass="txtXLongM"
											MaxLength="255" onChange="Count(this,255)" TextMode="MultiLine"></asp:textbox></td>
									<TD align="center" colSpan="1">&nbsp;</TD>
								</tr>
                                <tr>
                                    <TD align="center" colSpan="1">&nbsp;</TD>
									<td align="right" colSpan="1"><asp:label id="lblAssemblyInstructions" runat="server" text="Assembly Instructions:" CssClass="lblMedium"></asp:label></td>
									<td align="left" colSpan="3"><asp:textbox id="txtAssemblyInstructions" onkeyup="Count(this,255)" style="OVERFLOW: hidden" runat="server" CssClass="txtXLongM"
											MaxLength="255" onChange="Count(this,255)" TextMode="MultiLine"></asp:textbox></td>
									<TD align="center" colSpan="1">&nbsp;</TD>
                                </tr>
							</table>
						</td>
					</tr>
					<tr>
						<td vAlign="top" background="">
							<table width="100%" align="center">
								<TR>
									<td align="center" colSpan="6"><asp:table id="tblQty" runat="server" width="70%" HorizontalAlign="center"></asp:table></td>
								</TR>
								<TR>
									<TD align="center" colSpan="4">&nbsp;</TD>
								</TR>
								<TR>
									<td align="center" colSpan="4"><asp:label id="lblNSeq" runat="server" text="Note:  Enter number for sequencing.  C represents a Container Item.  0 represents no sequence."
											CssClass="lblNote"></asp:label></td>
								</TR>
								<TR>
									<td align="center" colSpan="4"><font face="Verdana" size="-4">Note: Items in <font face="Verdana" color="red" size="-4">
												red</font> are inactive and no longer available.</font></td>
								</TR>
								<TR>
									<td align="center" colSpan="4"><font face="Verdana" size="-4">Note: Items in <font face="Verdana" color="#bd5304" size="-4">
												orange</font> are on backorder and not available.</font></td>
								</TR>
							</table>
						</td>
					</tr>
					<tr>
						<td valign="bottom">
							<table width="100%" align="center" class="footercommon">
								<TR>
									<td vAlign="middle" align="center" width="100">&nbsp;</td>
									<td valign="middle" width="75">
										<asp:imagebutton id="cmdAdd" runat="server" ToolTip="Add Selected Kits" ImageUrl="images/btnAddCart.png" Visible="false"></asp:imagebutton>
									</td>
									<td valign="middle" align="left" width="370">
										<asp:textbox id="txtQty" runat="server" CssClass="txtNumber" MaxLength="5" Text="1">1</asp:textbox>&nbsp;&nbsp;
										<asp:label id="lblDQty" runat="server" text="Quantity of Kit to be Added" CssClass="lblDLong"></asp:label>
									</td>
									<td valign="middle" width="75"><asp:imagebutton id="cmdBack" runat="server" CausesValidation="False" ImageUrl="images/btnBack.png"
											ToolTip="Disregard Changes and Return to Previously Viewed Screen"></asp:imagebutton></td>
									<td valign="middle" width="75"><asp:imagebutton id="cmdItem" runat="server" CausesValidation="False" ImageUrl="images/btnItmAdd.png"
											ToolTip="Make Additional Selections"></asp:imagebutton></td>
									<td valign="middle" width="75"><asp:imagebutton id="cmdDelete" runat="server" CausesValidation="False" ImageUrl="images/btnDelete.png"
											ToolTip="Remove Current Kit Specification"></asp:imagebutton></td>
									<td valign="middle" width="75"><asp:imagebutton id="cmdSave" runat="server" CausesValidation="False" ImageUrl="images/btnSave.png"
											ToolTip="Save Current Kit Specification"></asp:imagebutton></td>
									<td width="75"></td>
								</TR>
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
		alert(" Notes may only be " + long + " characters.");
	}
}
</script>
