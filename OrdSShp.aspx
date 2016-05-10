<%@ Page Language="vb" AutoEventWireup="false" Codebehind="OrdSShp.aspx.vb" Inherits="POSN.OrdSShp"%>
<div dir="rtl" style='OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%' runat='server' ID="Div2"><div dir="ltr">
		<form id="frmAdmInvL" method="post" runat="server">
			<table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td background="">
						<table width="100%" align="center" bgColor="transparent">
							<tr>
								<td align="center" colSpan="2"><asp:label id="lblTitle" runat="server" text="Previous Shipping Locations" CssClass="lblTitle"></asp:label></td>
							</tr>
							<tr>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<td align="center">
									<asp:label id="lblLName" runat="server" text="Last Name:" CssClass="lblMedium" visible="false"></asp:label>
									<asp:textbox id="txtLName" runat="server" CssClass="txtMedium" MaxLength="20" visible="false"></asp:textbox>&nbsp;&nbsp;
									<asp:label id="lblCompany" runat="server" text="Company:" CssClass="lblMedium" visible="false"></asp:label>
									<asp:textbox id="txtCompany" runat="server" CssClass="txtMedium" MaxLength="20" visible="false"></asp:textbox>&nbsp;&nbsp;
									<asp:imagebutton id="cmdRefresh" ToolTip="Refresh Search" ImageURL="images/btnSearch.png" Runat="server"
										visible="false"></asp:imagebutton>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td vAlign="top" background="">
						<table width="100%" align="center">
							<tr>
								<td align="center" width="100%" colSpan="10"><asp:datagrid id="grd" runat="server" AutoGenerateColumns="False" ShowFooter="True" CellPadding="3"
										BorderWidth="1px" BorderColor="Black" Font-Size="Smaller" BorderStyle="None" OnItemDataBound="grd_ItemDataBound">
										<selecteditemstyle font-italic="True" cssclass="grdSelect"></selecteditemstyle>
										<itemstyle cssclass="grdBody"></itemstyle>
										<headerstyle font-italic="True" font-bold="True" cssclass="grdHead"></headerstyle>
										<columns>
											<asp:boundcolumn Visible="False" DataField="ID" HeaderText="ID"></asp:boundcolumn>
											<asp:templatecolumn Visible="True" HeaderText="Select">
												<headerstyle width="10%"></headerstyle>
											</asp:templatecolumn>
											<asp:boundcolumn DataField="RequestDate" HeaderText="Request Date">
												<headerstyle width="15%"></headerstyle>
											</asp:boundcolumn>
											<asp:boundcolumn DataField="Company" HeaderText="Company">
												<headerstyle width="25%"></headerstyle>
											</asp:boundcolumn>
											<asp:boundcolumn DataField="ShipTo" HeaderText="Ship To">
												<headerstyle width="15%"></headerstyle>
											</asp:boundcolumn>
											<asp:boundcolumn DataField="BAddress" HeaderText="Address">
												<headerstyle width="20%"></headerstyle>
											</asp:boundcolumn>
											<asp:boundcolumn DataField="BCity" HeaderText="City">
												<headerstyle width="20%"></headerstyle>
											</asp:boundcolumn>
											<asp:boundcolumn DataField="BState" HeaderText="State">
												<headerstyle width="1%"></headerstyle>
											</asp:boundcolumn>
											<asp:boundcolumn DataField="BZip" HeaderText="Zip">
												<headerstyle width="3%"></headerstyle>
											</asp:boundcolumn>
										</columns>
									</asp:datagrid></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="black">
					<td>
						<table width="100%" align="center">
							<tr>
								<td width="350"></td>
								<td width="75"></td>
								<td width="75">
									<asp:imagebutton id="cmdBack" runat="server" ToolTip="Return to Previously Viewed Screen" ImageUrl="images/btnBack.png"
										CausesValidation="False"></asp:imagebutton>
								</td>
								<td width="75"></td>
								<td width="75"></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</div>
</div>
