<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AdmInvP.aspx.vb" Inherits="POSN.AdmInvP"%>
<td valign="top">
	<form id="frmAdmInvP" method="post" runat="server">
		<table width="100%" align="center" border="0">
			<TBODY>
				<TR>
					<td height="190">
						<table width="100%" align="center">
							<TBODY>
								<TR>
									<TD align="center" width="80%">
										<asp:label id="lblHead" runat="server" text="" CssClass="lblDTitle"></asp:label>
									</TD>
								<TR>
					</td>
				</TR>
				<TR>
					<TD align="center" colspan="2"><asp:datagrid id="grd" runat="server" BorderStyle="None" Font-Size="Smaller" BorderColor="Black"
							BorderWidth="0" CellPadding="3" ShowFooter="True" AutoGenerateColumns="False">
							<SelectedItemStyle Font-Italic="True" CssClass="grdSelect"></SelectedItemStyle>
							<ItemStyle CssClass="grdBody"></ItemStyle>
							<HeaderStyle Font-Italic="True" Font-Bold="True" CssClass="grdHead"></HeaderStyle>
							<Columns>
								<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
								<asp:TemplateColumn Visible="False" HeaderText="Review">
									<HeaderStyle Width="0%"></HeaderStyle>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="Status" HeaderText="Status"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="DocStatus" HeaderText="Status"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="PrintMethod" HeaderText="PrintMethod"></asp:BoundColumn>
								<asp:TemplateColumn Visible="True" HeaderText="Status">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="QtyOH" HeaderText="Qty in Pieces">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RefNo" HeaderText="Reference Number">
									<HeaderStyle Width="17%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RefName" HeaderText="Description">
									<HeaderStyle Width="47%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<tr>
					<td>
						<table width="100%" align="center">
							<TR>
								<td align="center"><asp:button id="cmdBack" runat="server" CssClass="cmdMedium" ToolTip="Return to Previously Viewed Screen"
										Text="Back" CausesValidation="False"></asp:button></td>
							</TR>
						</table>
					</td>
				</tr>
			</TBODY>
		</table>
	</form>
</td>
</TR></TBODY></TABLE>
