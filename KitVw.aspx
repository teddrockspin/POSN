<%@ Page Language="vb" AutoEventWireup="false" Codebehind="KitVw.aspx.vb" Inherits="POSN.KitVw"%>
<td valign="top">
	<div id="Div1" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%" runat="server">
		<div dir="ltr">
			<form id="frmKitVw" method="post" runat="server">
                <asp:hiddenfield runat="server" id="hfShowStock" value="false"></asp:hiddenfield>
				<table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
					<tr>
						<td background="">
							<table width="100%" align="center">
								<tr>
									<td align="center" colSpan="4"><asp:label id="lblTitle" runat="server" text="Kit Specifications" CssClass="lblTitle"></asp:label></td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td vAlign="top" background="">
							<table width="100%" align="center">
								<tr>
									<TD align="center" colSpan="1">&nbsp;</TD>
									<td align="center" colSpan="1"><asp:label id="lblRefNo" runat="server" text="Reference Number:" CssClass="lblBTitle"></asp:label></td>
									<TD align="center" colSpan="3">&nbsp;</TD>
								</tr>
								<tr>
									<TD align="center" colSpan="1">&nbsp;</TD>
									<td align="center" colSpan="1"><asp:label id="lblRefName" runat="server" text="Description:" CssClass="lblBTitle"></asp:label></td>
									<TD align="center" colSpan="3">&nbsp;</TD>
								</tr>
								<tr>
									<td align="center" colSpan="6"><asp:table id="tblQty" runat="server" width="70%" HorizontalAlign="center" BackColor="Transparent"></asp:table></td>
								</tr>
								<TR>
									<TD align="center" colSpan="4">&nbsp;</TD>
								</TR>
								<TR>
									<td align="center" colspan="4"><font face="Verdana" size="-4">Note: Items in <font color="red" face="Verdana" size="-4">
												red</font> are inactive items that are no longer available.</font></td>
								</TR>
								<TR>
									<td align="center" colspan="4"><font face="Verdana" size="-4">Note: Items in <font color="#bd5304" face="Verdana" size="-4">
												orange</font> are on backorder and not available.</font></td>
								</TR>
							</table>
						</td>
					</tr>
					<tr>
						<td bgcolor="black">
							<table width="100%" align="center">
								<TR>
									<td width="350"></td>
									<td width="75"><asp:imagebutton id="cmdBack" runat="server" ToolTip="Return to Previously Viewed Screen" ImageUrl="images/btnBack.png"
											CausesValidation="False"></asp:imagebutton></td>
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
