<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AdmKitL.aspx.vb" Inherits="POSN.AdmKitL" smartNavigation="True"%>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
<script type="text/javascript">
    $(function () {
       
        $('#maintcontenttable').height($('#maintcontenttabletd').height());

    });

</script>
<td valign="top">
	<div id="Div1" dir="rtl" style="WIDTH: 100%; HEIGHT: 100%" runat="server">
		<div dir="ltr" style="width: 100%; height: 100%">
			<form id="frmOrdL" method="post" runat="server">
				<table cellSpacing="0" cellPadding="0" width="100%"  height="100%"  align="center" border="0">
					<tr>
						<td vAlign="top" background="">
							<table width="100%" align="center">
								<TR>
									<td align="center" colSpan="4"><asp:label id="lblTitle" runat="server" text="Kit List" CssClass="lblTitle"></asp:label></td>
								</TR>
							</table>
						</td>
					</tr>
					<tr>
						<td vAlign="top" background="">
							<table width="100%" align="center">
								<TR>
									<TD align="center" width="100%" colSpan="4"><asp:datagrid id="grd" runat="server" AutoGenerateColumns="False" ShowFooter="True" CellPadding="3"
											BorderWidth="1px" BorderColor="Black" Font-Size="Smaller" BorderStyle="None" OnItemDataBound="grd_ItemDataBound">
											<SelectedItemStyle Font-Italic="True" CssClass="grdSelect"></SelectedItemStyle>
											<ItemStyle CssClass="grdBody"></ItemStyle>
											<HeaderStyle Font-Italic="True" Font-Bold="True" CssClass="grdHead"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
												<asp:TemplateColumn Visible="True" HeaderText="Review">
													<HeaderStyle Width="10%"></HeaderStyle>
												</asp:TemplateColumn>
												<asp:TemplateColumn Visible="True" HeaderText="Type">
													<HeaderStyle Width="10%"></HeaderStyle>
												</asp:TemplateColumn>
												<asp:BoundColumn Visible="false" DataField="Status" HeaderText="Status"></asp:BoundColumn>
												<asp:BoundColumn Visible="false" DataField="LocalOwn" HeaderText="LocalOwn"></asp:BoundColumn>
												<asp:BoundColumn Visible="false" DataField="InactiveDocCnt" HeaderText="InactiveDocCnt"></asp:BoundColumn>
												<asp:BoundColumn Visible="false" DataField="BackOrderDocCnt" HeaderText="BackOrderDocCnt"></asp:BoundColumn>
												<asp:BoundColumn DataField="RefNo" HeaderText="Reference Number">
													<HeaderStyle Width="15%"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="RefName" HeaderText="Description">
													<HeaderStyle Width="75%"></HeaderStyle>
												</asp:BoundColumn>
											</Columns>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD align="center" colSpan="4">&nbsp;</TD>
								</TR>
								<TR>
									<td align="center" colSpan="4"><font face="Verdana" size="-4">Note: Kits in <font face="Verdana" color="red" size="-4">
												red</font> contain inactive items that are no longer available. These Kits 
											should be modified to account for current item availability.</font></td>
								</TR>
								<TR>
									<td align="center" colSpan="4"><font face="Verdana" size="-4">Note: Kits in <font face="Verdana" color="#bd5304" size="-4">
												orange</font> contain backordered items that are not currently available.</font></td>
								</TR>
							</table>
						</td>
					</tr>
					<tr>
						<td valign="bottom">
							<TABLE width="100%" align="center"  class="footercommon">
								<TR>
									<td width="350"></td>
									<td width="75"><asp:imagebutton id="cmdBack" runat="server" CausesValidation="False" ImageUrl="images/btnBack.png"
											ToolTip="Disregard Changes and Return to Previously Viewed Screen"></asp:imagebutton></td>
									<td width="75"><asp:imagebutton id="cmdAdd" runat="server" CausesValidation="False" ImageUrl="images/btnAdd.png"
											ToolTip="Add New Kit Definition"></asp:imagebutton></td>
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
