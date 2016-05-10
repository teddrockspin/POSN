<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AdmRspP.aspx.vb" Inherits="POSN.AdmRspP"%>
<td valign="top">
	<form id="frmAdmRspP" method="post" runat="server">
		<table width="100%" align="center" border="0">
			<TBODY>
				<TR>
					<td height="190">
						<table width="100%" align="center">
							<TBODY>
								<TR>
									<TD align="center" width="80%">
										<asp:label id="lblHead" runat="server" text="" CssClass="lblTitle"></asp:label>
									</TD>
								<TR>
					</td>
				</TR>
				<TR>
					<td height="190">
						<table width="100%" align="center">
							<TBODY>
								<TR>
									<TD align="right" width="10%"></TD>
									<TD align="right" width="30%"><asp:label id="lblDays" runat="server" text="Restock level to:" CssClass="lblLong"></asp:label></TD>
									<TD align="left" width="40%"><asp:label id="txtDays" runat="server" CssClass="lblNumber"></asp:label><asp:label id="lblDays2" runat="server" text="&nbsp;&nbsp;days" CssClass="lblXShort"></asp:label></TD>
									<TD align="right" width="10%">&nbsp;</TD>
									<TD align="right" width="10%">&nbsp;</TD>
									<TD align="right" width="10%">&nbsp;</TD>
								</TR>
								<TR>
									<TD align="right" width="10%"></TD>
									<TD align="right" width="20%"><asp:label id="lblRound" runat="server" text="Round Required Quantities to Nearest:" CssClass="lblLong"></asp:label></TD>
									<TD align="left" width="40%"><asp:label id="txtRound" runat="server" CssClass="lblNumber"></asp:label></TD>
									<TD align="left" width="10%">&nbsp;</TD>
									<TD align="left" width="10%">&nbsp;</TD>
									<TD align="right" width="20%">&nbsp;</TD>
								</TR>
								<TR>
									<TD align="right" width="10%"></TD>
									<TD align="right" width="20%"><asp:label id="lblFilterItmType" runat="server" text="Filter By Item Type:" CssClass="lblLong"></asp:label></TD>
									<TD align="left" width="40%"><asp:label id="txtItmType" runat="server" CssClass="lblMedium"></asp:label><asp:label id="txtItmTypeID" runat="server" visible="False"></asp:label></TD>
									<TD align="left" width="10%">&nbsp;</TD>
									<TD align="left" width="10%">&nbsp;</TD>
									<TD align="right" width="20%">&nbsp;</TD>
								</TR>
								<TR>
									<TD align="right" width="10%"></TD>
									<TD align="right" width="20%"><asp:label id="lblFilterPart" runat="server" text="Filter By Part No:" CssClass="lblLong"></asp:label></TD>
									<TD align="left" width="40%"><asp:label id="txtPartNo" runat="server" CssClass="lblMedium"></asp:label></TD>
									<TD align="left" width="10%">&nbsp;</TD>
									<TD align="left" width="10%">&nbsp;</TD>
									<TD align="right" width="20%">&nbsp;</TD>
								</TR>
								<TR>
									<TD align="right" width="10%"></TD>
									<TD align="right" width="20%"><asp:label id="lblExZero" runat="server" text="Exclude Items that do NOT Require Additional Inventory:"
											CssClass="lblLong"></asp:label></TD>
									<TD align="left" width="40%"><asp:checkbox id="chkExZeroQty" runat="server" enabled="False"></asp:checkbox></TD>
									<TD align="left" width="10%">&nbsp;</TD>
									<TD align="left" width="10%">&nbsp;</TD>
									<TD align="right" width="20%">&nbsp;</TD>
								</TR>
								<TR>
									<TD align="right" width="10%"></TD>
									<TD align="right" width="20%"><asp:label id="lblSort" runat="server" text="Sort By:" CssClass="lblLong"></asp:label></TD>
									<TD align="left" width="40%">
										<asp:label id="cboSort1" runat="server" CssClass="lblShort"></asp:label>
										<asp:label id="lblThen1" runat="server" text="&nbsp;&nbsp;then&nbsp;&nbsp;" CssClass="lblXShort"></asp:label>
										<asp:label id="cboSort2" runat="server" CssClass="lblShort"></asp:label>
										<asp:label id="lblThen2" runat="server" text="&nbsp;&nbsp;then&nbsp;&nbsp;" CssClass="lblXShort"></asp:label>
										<asp:label id="cboSort3" runat="server" CssClass="lblShort"></asp:label>
									</TD>
									<TD align="left" width="10%">&nbsp;</TD>
									<TD align="left" width="10%">&nbsp;</TD>
									<TD align="right" width="20%">&nbsp;</TD>
								</TR>
					</td>
				</TR>
				<TR>
					<td align="center" colSpan="8"><asp:table id="tblRsp" runat="server" HorizontalAlign="center" width="100%" CellSpacing="0"
							CellPadding="0"></asp:table></td>
				</TR>
				<TR>
					<TD align="right" colSpan="1"></TD>
					<TD align="center" colSpan="3"></TD>
					<TD align="right" colSpan="2"></TD>
				<TR>
					<TD align="right" colSpan="1"></TD>
					<TD align="left" colSpan="3"><table>
							<tr>
								<td bgcolor="black">&nbsp;</td>
								<td><asp:label id="lblNote" runat="server" text=" - Scheduled for Obsolescence" CssClass="lblLongSmall"></asp:label></td>
							</tr>
						</table>
					</TD>
					<TD align="right" colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD align="right" colSpan="1"></TD>
					<TD align="left" colSpan="4"><asp:label id="lblNote2" runat="server" text="FUL - Fulfill Only, POD - POD Only, F/P - Fulfill then POD as needed"
							CssClass="lblLongSmall"></asp:label></TD>
					<TD align="right" colSpan="1">&nbsp;</TD>
				</TR>
				<TR>
					<TD align="right" colSpan="1"></TD>
					<TD align="right" colSpan="4"><asp:label id="lblTotCost" runat="server" text="Total Cost:" CssClass="lblMedium"></asp:label>
						<asp:label id="txtTotCost" runat="server" CssClass="txtNumberLong"></asp:label></TD>
					<TD align="right" colSpan="1">&nbsp;</TD>
				</TR>
			</TBODY>
		</table>
</td>
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
</TBODY></TABLE></FORM></TD></TR></TBODY></TABLE>
