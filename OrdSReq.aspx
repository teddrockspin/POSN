<%@ Page Language="vb" AutoEventWireup="false" Codebehind="OrdSReq.aspx.vb" Inherits="POSN.OrdSReq"%>
<div dir="rtl" style='OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%' runat='server' ID="Div2"><div dir="ltr">
		<form id="frmAdmInvL" method="post" runat="server">
			<table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
				<TR>
					<td background="">
						<table width="100%" align="center" bgColor="transparent">
							<TR>
								<TD align="center" colSpan="2"><asp:label id="lblTitle" runat="server" text="Previous Requestors" CssClass="lblTitle"></asp:label></TD>
							</TR>
							<tr>
								<td>&nbsp;</td>
							</tr>
							<TR>
								<TD align="center">
									<asp:label id="lblLName" runat="server" text="Last Name:" CssClass="lblMedium" visible="false"></asp:label>
									<asp:textbox id="txtLName" runat="server" CssClass="txtMedium" MaxLength="20" visible="false"></asp:textbox>&nbsp;&nbsp;
									<asp:imagebutton id="cmdRefresh" ToolTip="Refresh Search" ImageURL="images/btnSearch.png" Runat="server"
										visible="false"></asp:imagebutton>
								</TD>
							</TR>
						</table>
					</td>
				</TR>
				<tr>
					<td vAlign="top" background="">
						<table width="100%" align="center">
							<TR>
								<TD align="center" width="100%" colSpan="10"><asp:datagrid id="grd" runat="server" AutoGenerateColumns="False" ShowFooter="True" CellPadding="3"
										BorderWidth="1px" BorderColor="Black" Font-Size="Smaller" BorderStyle="None" OnItemDataBound="grd_ItemDataBound">
										<SelectedItemStyle Font-Italic="True" CssClass="grdSelect"></SelectedItemStyle>
										<ItemStyle CssClass="grdBody"></ItemStyle>
										<HeaderStyle Font-Italic="True" Font-Bold="True" CssClass="grdHead"></HeaderStyle>
										<Columns>
											<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
											<asp:TemplateColumn Visible="True" HeaderText="Select">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="RequestDate" HeaderText="Request Date">
												<HeaderStyle Width="15%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Requestor" HeaderText="Requestor">
												<HeaderStyle Width="30%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="ReqEmail" HeaderText="E-mail">
												<HeaderStyle Width="25%"></HeaderStyle>
											</asp:BoundColumn>
										</Columns>
									</asp:datagrid></TD>
							</TR>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<table width="100%" align="center" bgColor="black">
							<TR>
								<td width="350"></td>
								<td width="75"></td>
								<td width="75"><asp:imagebutton id="cmdBack" runat="server" ToolTip="Return to Previously Viewed Screen" ImageUrl="images/btnBack.png"
										CausesValidation="False"></asp:imagebutton></td>
								<td width="75"></td>
								<td width="75"></td>
							</TR>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</div>
</div>
</TD>
