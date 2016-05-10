<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AdmItmE.aspx.vb" Inherits="POSN.AdmItmE"%>
<td valign="top">
	<div id="Div1" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%" runat="server">
		<div dir="ltr">
			<form id="frmAdmItmE" method="post" runat="server">
				<table width="100%" align="center" border="0">
					<TBODY>
						<tr>
							<td height="190" background="">
								<table width="100%" align="center">
									<TBODY>
										<TR>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="center" colSpan="3"><asp:label id="lblTitle" runat="server" text="Item Edit" CssClass="lblTitle"></asp:label></td>
											<TD align="center" colSpan="1">&nbsp;</TD>
										</TR>
										<TR>
											<TD align="center" colSpan="5">&nbsp;</TD>
										</TR>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1"><asp:label id="lblRefNo" runat="server" CssClass="lblMedium" text="Reference Number:"></asp:label></td>
											<td align="left" colSpan="1"><asp:label id="txtRefNo" runat="server" CssClass="lblMedium"></asp:label></td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1"><asp:label id="lblRefDesc" runat="server" CssClass="lblMedium" text="Description:"></asp:label></td>
											<td align="left" colSpan="1"><asp:label id="txtRefDesc" runat="server" CssClass="lblLong"></asp:label></td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1"><asp:label id="lblPrefix" runat="server" CssClass="lblMedium" text="Prefix:"></asp:label></td>
											<td align="left" colSpan="1"><asp:textbox id="txtPrefix" runat="server" CssClass="txtShort" MaxLength="4"></asp:textbox></td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1"><asp:label id="lblLastPurPrice" runat="server" CssClass="lblMedium" text="Last Purchase Price:">Last Purchase Price $:</asp:label></td>
											<td align="left" colSpan="1"><asp:textbox id="txtLastPurPrice" runat="server" CssClass="txtNumberLong" MaxLength="6" ToolTip="Enter as numeric - Do not include $"></asp:textbox><asp:comparevalidator id="vLastPurPrice" Runat="server" Type="Double" Operator="DataTypeCheck" Text="*"
													Display="Dynamic" ControlToValidate="txtLastPurPrice"></asp:comparevalidator><asp:rangevalidator id="v2LastPurPrice" runat="server" Type="Double" ControlToValidate="txtLastPurPrice"
													ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:rangevalidator></td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1"><asp:label id="lblVendor" runat="server" CssClass="lblMedium" text="Vendor:"></asp:label></td>
											<td align="left" colSpan="1"><asp:dropdownlist id="cboVendor" runat="server" CssClass="cboMedium"></asp:dropdownlist></td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1"><asp:label id="lblSchedObsDate" runat="server" CssClass="lblMedium" text="Known Obscolescence Date:"></asp:label></td>
											<td align="left" colSpan="1"><asp:textbox id="txtSchedObsDate" runat="server" CssClass="txtShort" MaxLength="10"></asp:textbox><asp:comparevalidator id="vSchedObsDate" Runat="server" Type="Date" Operator="DataTypeCheck" Text="*"
													Display="Dynamic" ControlToValidate="txtSchedObsDate"></asp:comparevalidator></td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1"><asp:label id="lblWarnLevel" runat="server" CssClass="lblMedium" text="Stock Warning Level (Wks):"></asp:label></td>
											<td align="left" colSpan="1"><asp:textbox id="txtWarnLevel" runat="server" CssClass="txtNumberLong" MaxLength="6"></asp:textbox><asp:comparevalidator id="vWarnLevel" Runat="server" Type="Integer" Operator="DataTypeCheck" Text="*"
													Display="Dynamic" ControlToValidate="txtWarnLevel"></asp:comparevalidator><asp:rangevalidator id="v2WarnLevel" runat="server" Type="Integer" ControlToValidate="txtWarnLevel"
													ErrorMessage="*" MinimumValue="0" MaximumValue="255"></asp:rangevalidator></td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1">
                                                <asp:label id="lblOrderQuantityLimit0" runat="server" 
                                                    CssClass="lblMedium" text="No Order Limit"></asp:label></td>
											<td align="left" colSpan="1">
                                                <asp:CheckBox ID="chkNoOrderLimit" runat="server" />
                                            </td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1"><asp:label id="lblOrderQuantityLimit" runat="server" 
                                                    CssClass="lblMedium" text="Order Quantity Limit:"></asp:label></td>
											<td align="left" colSpan="1"><asp:textbox id="txtOrderQuantityLimit" runat="server" 
                                                    CssClass="txtNumberLong" MaxLength="25"></asp:textbox></td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1"><asp:label id="lblItmType" runat="server" CssClass="lblMedium" text="Item Type:"></asp:label></td>
											<td align="left" colSpan="1"><asp:dropdownlist id="cboItmType" runat="server" CssClass="cboMedium" AutoPostBack="True"></asp:dropdownlist></td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1"><asp:label id="lblAttr1" runat="server" CssClass="lblMedium" text="Attribute 1:"></asp:label></td>
											<td align="left" colSpan="1"><asp:dropdownlist id="cboAttr1" runat="server" CssClass="cboMedium"></asp:dropdownlist></td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1"><asp:label id="lblAttr2" runat="server" CssClass="lblMedium" text="Attribute 2:"></asp:label></td>
											<td align="left" colSpan="1"><asp:dropdownlist id="cboAttr2" runat="server" CssClass="cboMedium"></asp:dropdownlist></td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1"><asp:label id="lblAttr3" runat="server" CssClass="lblMedium" text="Attribute 3:"></asp:label></td>
											<td align="left" colSpan="1"><asp:dropdownlist id="cboAttr3" runat="server" CssClass="cboMedium"></asp:dropdownlist></td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1"><asp:label id="lblAttr4" runat="server" CssClass="lblMedium" text="Attribute 4:"></asp:label></td>
											<td align="left" colSpan="1"><asp:dropdownlist id="cboAttr4" runat="server" CssClass="cboMedium"></asp:dropdownlist></td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1"><asp:label id="lblAttr5" runat="server" CssClass="lblMedium" text="Attribute 5 / Note:"></asp:label></td>
											<td align="left" colSpan="1"><asp:textbox id="txtAttr5" runat="server" CssClass="txtMedium" MaxLength="25"></asp:textbox></td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<tr>
											<TD align="center" colSpan="1">&nbsp;</TD>
											<td align="right" colSpan="1">&nbsp;</td>
											<td align="left" colSpan="1">&nbsp;</td>
											<TD align="center" colSpan="2">&nbsp;</TD>
										</tr>
										<TR>
											<TD align="center" colSpan="5">&nbsp;</TD>
										</TR>
									</TBODY>
								</table>
							</td>
						</tr>
						<tr>
							<td>
								<table width="100%" align="center" bgColor="black">
									<TR>
										<td width="350"></td>
										<td width="75"><asp:imagebutton id="cmdBack" runat="server" ToolTip="Disregard Changes and Return to Previously Viewed Screen"
												ImageUrl="images/btnBack.png" CausesValidation="False"></asp:imagebutton></td>
										<td width="75"><asp:imagebutton id="cmdSave" runat="server" ToolTip="Save Current Item Specification" ImageUrl="images/btnSave.png"></asp:imagebutton></td>
										<td width="75"></td>
									</TR>
								</table>
							</td>
						</tr>
					</TBODY>
				</table>
			</form>
		</div>
	</div>
</td>
</TR></TBODY></TABLE></TR></TBODY></TABLE></FORM>
<script language="javascript">
    function ShowOrderQuantityLimit(TextBox, Label, CheckBox, cMaxOrderLimit) {
        if (CheckBox.checked) {
            TextBox.style.display = 'none';
            Label.style.display = 'none';
        }
        else {
            TextBox.style.display = '';
            Label.style.display = '';
        }
        
        if (TextBox.value != "") {
            if (TextBox.value == cMaxOrderLimit) {
                TextBox.value = ""
            }
        }
    }
</script>