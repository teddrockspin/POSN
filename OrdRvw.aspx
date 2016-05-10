<%@ Page Language="vb" AutoEventWireup="false" Codebehind="OrdRvw.aspx.vb" Inherits="POSN.OrdRvw" smartNavigation="True"%>
<td valign="top">
	<div dir="rtl" style='OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%' runat='server'><div dir="ltr">
			<form id="frmOrdRvw" method="post" runat="server">
				<table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
					<tr>
						<td background="">
							<table width="100%" align="center">
								<TR>
									<TD align="center" colSpan="1">&nbsp;</TD>
									<td align="center" colSpan="2"><asp:label id="lblTitle" runat="server" CssClass="lblTitle" text="Order - Approve / Respond"></asp:label></td>
									<TD align="center" colSpan="1">&nbsp;</TD>
								</TR>
							</table>
						</td>
					</tr>
					<tr>
						<td vAlign="top" background="">
							<table width="100%" align="center">
								<tr>
									<td align="center" colspan="4"><asp:table id="tblDetail" runat="server" HorizontalAlign="center" width="100%" BackColor="Transparent"></asp:table></td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td>
							<table width="100%" align="center" bgColor="black">
								<TR>
									<TD align="center" colSpan="5">
										&nbsp;
									</TD>
								</TR>
								<TR>
									<TD align="center" colSpan="5">
										<asp:imagebutton id="cmdApprove" runat="server" ToolTip="Approve as is / Order will be activated..."
											ImageUrl="images/btnApprove.png" CausesValidation="False" visible="false"></asp:imagebutton>
									</TD>
								</TR>
								<TR>
									<td align="right" colSpan="2"><asp:requiredfieldvalidator id="vREmail" runat="server" ControlToValidate="txtREmail" ErrorMessage="*"></asp:requiredfieldvalidator><asp:label id="lblREmail" runat="server" CssClass="lblDMedium" text="From (Return E-mail):"></asp:label></td>
									<td align="left" colSpan="3"><asp:textbox id="txtREmail" runat="server" CssClass="txtLong" MaxLength="50"></asp:textbox></td>
									<td><asp:textbox id="txtRCC" runat="server" CssClass="txtMedium" Width="1px" Visible="False"></asp:textbox></td>
								</TR>
								<TR>
									<td align="right" colSpan="2"><asp:requiredfieldvalidator id="vRContent" runat="server" ControlToValidate="txtRContent" ErrorMessage="*"></asp:requiredfieldvalidator><asp:label id="lblRContent" runat="server" CssClass="lblDMedium" text="How to Proceed with Request:"></asp:label></td>
									<td align="left" colSpan="3"><asp:textbox id="txtRContent" runat="server" CssClass="txtXLongM" TextMode="MultiLine" MaxLength="2000"></asp:textbox></td>
								</TR>
								<TR>
									<TD align="center" colSpan="5">
										<asp:imagebutton id="cmdRespond" runat="server" ToolTip="Reject and Respond as to how to proceed..."
											ImageUrl="images/btnReject.png" CausesValidation="False" visible="false"></asp:imagebutton>
									</TD>
								</TR>
								<TR>
									<TD align="center" colSpan="5">
										&nbsp;
									</TD>
								</TR>
							</table>
						</td>
					</tr>
				</table>
			</form>
		</div>
	</div>
</td>
