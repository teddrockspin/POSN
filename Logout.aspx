<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Logout.aspx.vb" Inherits="POSN.Logout" smartNavigation="False"%>
<td valign="top" align="center">
	<div dir="rtl" style='WIDTH: 100%; HEIGHT: 100%' runat='server' ID="Div1"><div dir="ltr" style="width: 100%; height: 100%">
			<form id="frmLogout" method="post" runat="server">
				<table width="100%" >
					<tr>
						<td height="180" background=''>
							<table width="100%" align="center">
								<TR>
									<TD align="center" colSpan="1">&nbsp;</TD>
									<td align="center" colSpan="2"><asp:label id="lblTitle" runat="server" CssClass="lblTitle" text="Logged out"></asp:label></td>
									<TD align="center" colSpan="1">&nbsp;</TD>
								</TR>
								<TR>
									<TD align="center" colSpan="6">&nbsp;<br>
									</TD>
								</TR>
								<TR>
									<TD align="center" colSpan="6">&nbsp;<br>
									</TD>
								</TR>
								<TR>
									<td align="center" colspan="6"><asp:label id="lblMsg" runat="server" CssClass="lblMediumLong" text="Thank you for visiting..."></asp:label></td>
								</TR>
								<TR>
									<TD align="center" colSpan="6">&nbsp;<br>
									</TD>
								</TR>
								<TR>
									<TD align="center" colSpan="6">&nbsp;<br>
									</TD>
								</TR>
							</table>
						</td>
					</tr>
					<tr>
						<td valign="bottom">
							<table width="100%" align="center" class="footercommon">
								<TR>
									<td width="350">&nbsp;</td>
									<td width="75"></td>
									<td align='right'><table><tr><td><A ID='lkLogIn' href='Login.aspx'><img src='images/btnLogin.png' alt='Login' border='0'/></A></td></tr></table></td>
								</TR>
							</table>
						</td>
					</tr>
				</table>
			</form>
		</div>
	</div>
</td>
