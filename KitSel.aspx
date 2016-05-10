<%@ Page Language="vb" AutoEventWireup="false" Codebehind="KitSel.aspx.vb" Inherits="POSN.KitSel"%>
<script src="script/jquery-1.4.1.min.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.2/themes/flick/jquery-ui.css" />

<form id="frmKitSel" onsubmit="javascript:SaveDocScroll();" method="post" runat="server">

<PRE lang="aspnet"></PRE>
<td valign="top">
	<div id="Div1" dir="ltr" runat="server">
		<div dir="ltr">
			
                 <asp:hiddenfield runat="server" id="hfShowStock" value="false"></asp:hiddenfield>
				<table cellSpacing="0" cellPadding="0"  align="center" border="0">
					<TBODY>
						<TR>
							<td background="">
								<table  align="center" border="0">
									<TBODY>
										<TR>
											<td align="center" colSpan="4"></td>
										</TR>
										<tr>
											<td vAlign="top" >
												<table  align="center">
													<TBODY>
														<TR>
															<TD align="left" colSpan="2"><asp:panel id="pnlK1" runat="server" Height="100px" visible="false">
																	<TABLE width="264px" cellSpacing="0"  align="center">
																		<TR>
																			<TD vAlign="middle" align="left" colSpan="1">
																				<asp:label id="lblK1" runat="server" text="Keyword Type 1" CssClass="lblListB"></asp:label></TD>
																			<TD align="right">
																				<asp:imagebutton id="ibtnK1Select" Runat="server" ImageURL="images/btnSelectAll.png" ToolTip="Select All"></asp:imagebutton></TD>
																		</TR>
																		<TR>
																			<TD colSpan="2">
																				<DIV id=divK1Grid 
                        style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; OVERFLOW: auto; BORDER-LEFT: gray 1px solid; WIDTH: 100%; BORDER-BOTTOM: gray 1px solid; HEIGHT: 139px; BACKGROUND-COLOR: white" 
                        onscroll="javascript:SaveDivScroll(this, <% =ScrollXPosK1.ClientID %>);">
																					<asp:datagrid id="grdK1" width="264" runat="server" AutoGenerateColumns="False"
																						ShowFooter="True" CellPadding="1" BorderWidth="1px" BorderColor="Black" Font-Size="Smaller"
																						BorderStyle="None" OnItemDataBound="grdK1_ItemDataBound" GridLines="Horizontal" ShowHeader="False"
																						>
																						<SelectedItemStyle CssClass="grdListSelect" BackColor="#FFFF80"></SelectedItemStyle>
																						<ItemStyle CssClass="grdListBody"></ItemStyle>
																						<HeaderStyle Font-Italic="True" Font-Bold="True" CssClass="grdListHead"></HeaderStyle>
																						<Columns>
																							<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
																							<asp:TemplateColumn HeaderText="Check">
																								<HeaderStyle Width="1%"></HeaderStyle>
																								<ItemStyle HorizontalAlign="Center"></ItemStyle>
																								<ItemTemplate>
																									<input id="chkSelect" runat="server" type="hidden" value="0" NAME="chkSelect">
																								</ItemTemplate>
																							</asp:TemplateColumn>
																							<asp:BoundColumn Visible="False" DataField="Keyword" HeaderText="Select Keyword">
																								<HeaderStyle Width="50%"></HeaderStyle>
																							</asp:BoundColumn>
																							<asp:TemplateColumn HeaderText="Keyword">
																								<HeaderStyle Width="95%"></HeaderStyle>
																								<ItemStyle HorizontalAlign="Left"></ItemStyle>
																								<ItemTemplate>
																									<ASP:BUTTON id="btnKeyword" Runat="server" cssClass="btnKeywordList" Text='<%# DataBinder.Eval(Container.DataItem, "Keyword").ToString() %>' CommandArgument='<%# DataBinder.Eval(Container,"DataItem.ID")%>' CommandName="Select">
																									</ASP:BUTTON>
																								</ItemTemplate>
																							</asp:TemplateColumn>
																						</Columns>
																					</asp:datagrid></DIV>
																			</TD>
																		</TR>
																	</TABLE>
																</asp:panel></TD>
														</TR>
														<tr>
															<td>&nbsp;</td>
														</tr>
														<tr>
															<td>&nbsp;</td>
														</tr>
														<tr>
															<td>&nbsp;</td>
														</tr>
														<tr>
															<td vAlign="middle" align="right"><asp:imagebutton id="ibtnRestart" Runat="server" ImageURL="images/btnRestart.png" ToolTip="Clear Tags Selected"></asp:imagebutton></td>
															<td vAlign="middle" align="left" colSpan="1"><asp:imagebutton id="ibtnGet" Runat="server" ImageURL="images/btnResult.png" ToolTip="Search By Tags Selected"></asp:imagebutton></td>
														</tr>
														<TR>
															<TD align="left" colSpan="2">&nbsp;</TD>
														</TR>
														<TR>
															<TD align="left" colSpan="2">&nbsp;</TD>
														</TR>
													</TBODY></table>
											</td>
							</td>
							<td vAlign="top" >
								<table  align="center">
									<TBODY>
										<TR>
											<TD align="left" colSpan="2">
												<table cellSpacing="0" cellPadding="1"  align="center">
													<tr>
														<td vAlign="middle" align="left" colSpan="1"><asp:label id="lblTitle" runat="server" text="Available Kits" CssClass="lblTitle"></asp:label><asp:image id="imgCheck" 
                                                                runat="server" ImageURL="images/Check.png" Visible="False"></asp:image>&nbsp;&nbsp;<asp:label id="lblSelect" runat="server" text="" CssClass="lblShortSmall"></asp:label><asp:label id="lblItem" runat="server" text="Items..." CssClass="lblListB" visible="false"></asp:label></td>
														<td vAlign="middle" align="right">&nbsp;</td>
														<td vAlign="middle" align="right">&nbsp;</td>
													</tr>
													<tr>
														<td colSpan="3">
															<div id=divItmGrid 
                        style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; OVERFLOW: auto; BORDER-LEFT: gray 1px solid; width:628px; BORDER-BOTTOM: gray 1px solid; HEIGHT: 308px; BACKGROUND-COLOR: white" 
                        onscroll="javascript:SaveDivScroll(this, <% =ScrollXPosItm.ClientID %>);" 
                        ><asp:datagrid id="grdItm" runat="server" OnItemCommand="grdItm_ItemCommand" AutoGenerateColumns="False" width="628px"
																	ShowFooter="True" CellPadding="0" BorderWidth="1px" BorderColor="Black" Font-Size="Smaller"
																	BorderStyle="None" OnItemDataBound="grdItm_ItemDataBound" GridLines="Horizontal" ShowHeader="True"
																	 BackColor="White">
																	<SelectedItemStyle CssClass="grdListSelectItm" BackColor="#FFFF80"></SelectedItemStyle>
																	<ItemStyle CssClass="grdListBody"></ItemStyle>
																	<HeaderStyle Font-Italic="True" Font-Bold="True" CssClass="grdListHead"></HeaderStyle>
																	<Columns>
																		<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
																		<asp:TemplateColumn Visible="true" HeaderText="Order" ItemStyle-VerticalAlign="Top">
																			<HeaderStyle Width="1%"></HeaderStyle>
																			<ItemStyle HorizontalAlign="Center"></ItemStyle>
																			<ItemTemplate>
																				<input id="inpID" runat="server" type="hidden" value='<%# DataBinder.Eval(Container,"DataItem.ID")%>' NAME="inpItmID">
																				<asp:checkbox runat="server" ID="chkSelect" CssClass="chkItem"></asp:checkbox>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:BoundColumn Visible="False" DataField="Status" ItemStyle-VerticalAlign="Top">
																			<HeaderStyle Width="1%"></HeaderStyle>
																		</asp:BoundColumn>
																		<asp:TemplateColumn ItemStyle-VerticalAlign="Top">
																			<HeaderStyle Width="1%"></HeaderStyle>
																			<ItemStyle HorizontalAlign="Center"></ItemStyle>
																			<ItemTemplate></ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:TemplateColumn HeaderText="Part No" ItemStyle-VerticalAlign="Top">
																			<HeaderStyle Width="18%"></HeaderStyle>
																			<ItemStyle HorizontalAlign="Center"></ItemStyle>
																			<ItemTemplate>
																				<ASP:BUTTON id="btnItmNo" Runat="server" cssClass="btnItmNoList" Text='<%# DataBinder.Eval(Container.DataItem, "RefNo").ToString() & "  " %>'>
																				</ASP:BUTTON>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:TemplateColumn HeaderText="Description" ItemStyle-VerticalAlign="Top">
																			<HeaderStyle Width="60%"></HeaderStyle>
																			<ItemStyle HorizontalAlign="left"></ItemStyle>
																			<ItemTemplate>
																				&nbsp;&nbsp;
																				<ASP:BUTTON id="btnItmName" Runat="server" cssClass="btnItmNameList" Text='<%# LEFT(DataBinder.Eval(Container.DataItem, "RefName").ToString(),65) & vbcrlf & MID(DataBinder.Eval(Container.DataItem, "RefName").ToString(),66) %>'>
																				</ASP:BUTTON>
																				<input id="inpItmNote" runat="server" type="hidden" value='<%# DataBinder.Eval(Container,"DataItem.KitNote")%>' NAME="inpItmNote">
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:BoundColumn Visible="False" DataField="KitNote">
																			<HeaderStyle Width="1%"></HeaderStyle>
																		</asp:BoundColumn>
																		<asp:BoundColumn Visible="False" DataField="InActiveDocCnt">
																			<HeaderStyle Width="1%"></HeaderStyle>
																		</asp:BoundColumn>
																		<asp:BoundColumn Visible="False" DataField="BackOrderDocCnt">
																			<HeaderStyle Width="1%"></HeaderStyle>
																		</asp:BoundColumn>
																		<asp:BoundColumn Visible="False" DataField="BaseKit">
																			<HeaderStyle Width="1%"></HeaderStyle>
																		</asp:BoundColumn>
																		<asp:BoundColumn Visible="False" DataField="LocalOwn">
																			<HeaderStyle Width="1%"></HeaderStyle>
																		</asp:BoundColumn>
                                                                         <asp:TemplateColumn headertext="Status">
                                                                            <ItemTemplate><asp:button id="btnStock" runat="server" text='<%#getShowStock(DataBinder.Eval(Container, "DataItem.ID"))%>' cssClass="btnStockShow" />
                                                                            </ItemTemplate>
                                                            </asp:TemplateColumn>
																	</Columns>
																</asp:datagrid></div>
														</td>
													</tr>
													<tr>
														<td colSpan="2" class="subnotes">* Kits in <font color="#bd5304">
																	orange</font> contain backordered items that are not currently available.</td>
														<td align="right" colSpan="1"><asp:imagebutton id="ibtnViewKit" Runat="server" ImageURL="images/btnKitCont.png" ToolTip="Kit Contents"></asp:imagebutton>&nbsp;</td>
													</tr>
													<tr>
														<td colSpan="2" class="subnotes">+ Kits in <font  color="purple">
																	purple</font> are base kits and may be modified.</td>
														<td align="right" colSpan="1"><asp:imagebutton id="ibtnEditKit" Runat="server" ImageURL="images/btnEditKit.png" ToolTip="Modify Temporary/Work Kit"></asp:imagebutton>&nbsp;</td>
													</tr>
												</table>
											</TD>
										</TR>
									</TBODY></table>
							</td>
							<td>
								<table width="256"  align="center">
									<TR>
										<TD align="left" colSpan="2">
											<table cellSpacing="0"  align="center">
												<TR>
													<TD align="center"><asp:label id="lblKitRef" runat="server" text="" CssClass="lblDSmall"></asp:label></TD>
												</TR>
												<TR>
													<TD align="center"><asp:label id="lblKitDes" runat="server" text="" CssClass="lblDSmallMulti"></asp:label></TD>
												</TR>
												<tr>
													<TD align="center"><asp:label id="lblKitNote" runat="server" text="" CssClass="lblDMediumMulti"></asp:label></TD>
												</tr>
												<tr>
													<td vAlign="bottom" align="center">&nbsp;</td>
												</tr>
												<TR>
													<TD vAlign="top" align="center">&nbsp;</TD>
												</TR>
											</table>
										</TD>
									</TR>
								</table>
							</td>
						</TR>
						<TR>
							<td vAlign="top" align="center" colSpan="100"></td>
						</TR>
					</TBODY></table>
</td>
</TR>
<TR>
	<td colSpan="1"><asp:panel id="pnlBtnOrd" runat="server" visible="false">
			<TABLE  align="center"  class="footercommon" border="0">
				<TR>
				
					<TD vAlign="middle" align="left" nowrap width="210">
						<asp:label id="lblQtyOrd" runat="server" text="Quantity of each Item to be Added" CssClass="lblDLong"></asp:label></td>
                    <td>
                        <asp:textbox id="txtQtyOrd" runat="server" CssClass="txtNumber" MaxLength="5" Text="1">1</asp:textbox>
                        
						</TD>
                    <td><asp:imagebutton id="ibtnAddOrd" runat="server" ToolTip="Add Selected Items" ImageUrl="images/btnAddCart.png"></asp:imagebutton></td>
					<TD width="75%">&nbsp;</TD>
					<TD vAlign="middle" width="75">
						<asp:imagebutton id="ibtnItmOrd" runat="server" ToolTip="View Item Catalog" ImageUrl="images/btnItmCat.png"
							CausesValidation="False"></asp:imagebutton></TD>
					<TD vAlign="middle" width="75">
						<asp:imagebutton id="ibtnCartOrd" runat="server" ToolTip="View Shopping Cart" ImageUrl="images/btnCart.png"></asp:imagebutton></TD>
					<TD width="75"></TD>
				</TR>
			</TABLE>
		</asp:panel></td>
</TR>
<TR class="ui-helper-hidden-accessible">
	<td colSpan="2">
		<TABLE  align="center" bgColor="white">
			<TBODY>
				<TR>
					<TD align="right" colSpan="1">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
					<td align="center" colSpan="2"><asp:comparevalidator id="vMaxQty" runat="server" CssClass="vldMedium" Text="*" ControlToValidate="txtQtyOrd"
							Display="Dynamic" Operator="LessThanEqual" Type="Integer" ErrorMessage="CompareValidator">*</asp:comparevalidator></td>
					<td align="center" colSpan="2">&nbsp;</td>
					<td><input id="txtSubmit" type="hidden" name="txtSubmit" runat="server"> <input id="txtItmRow" type="hidden" value="0" name="txtItmRow" runat="server">
						<input id="txtItmSelCnt" type="hidden" value="0" name="txtItmSelCnt" runat="server">
						<input id="txtItmSelFrom" type="hidden" name="txtItmSelFrom" runat="server"> <input id="ScrollXPosK1" type="hidden" name="ScrollXPosK1" runat="server">
						<input id="ScrollXPosItm" type="hidden" name="ScrollXPosItm" runat="server"> <input id="ScrollXPosDoc" type="hidden" name="ScrollXPosDoc" runat="server">
						<input id="ScrollYPosDoc" name="ScrollYPosDoc" runat="server" type="hidden">
					</td>
				</TR>
	</td>
</TR>
</TBODY>
    </TABLE></TD></TR></TBODY></TABLE></FORM></DIV></DIV></TD></form></table>
<script language="javascript">
window.onload = RestoreScrollPosition;

function RestoreScrollPosition() 
{   
  //document.body.scrollTop=document.frmKitSel.ScrollXPosDoc.value;
  //document.body.scrollLeft=document.frmKitSel.ScrollYPosDoc.value;
  //document.getElementById("divItmGrid").scrollTop=document.frmKitSel.ScrollXPosItm.value;
  //if (document.getElementById("divK1Grid") != null)
  //  document.getElementById("divK1Grid").scrollTop=document.frmKitSel.ScrollXPosK1.value;
  document.body.scrollTop=document.getElementById("ScrollXPosDoc").value;
  document.body.scrollLeft=document.getElementById("ScrollYPosDoc").value;
  document.getElementById("divItmGrid").scrollTop=document.getElementById("ScrollXPosItm").value;
  if (document.getElementById("divK1Grid") != null)
    document.getElementById("divK1Grid").scrollTop=document.getElementById("ScrollXPosK1").value;
}
function SaveDivScroll(itm, itmxholder) 
{
  itmxholder.value = itm.scrollTop;
}
function SaveDocScroll() 
{
  //document.frmKitSel.ScrollXPosDoc.value=document.body.scrollTop;
  //document.frmKitSel.ScrollYPosDoc.value=document.body.scrollLeft;
  document.getElementById("ScrollXPosDoc").value=document.body.scrollTop;
  document.getElementById("ScrollYPosDoc").value=document.body.scrollLeft;
}
</script>
<script language="javascript">
//Items
//Background color of selected
//Selected Count
//var rowsSelected = 0;
var iCntItmRow = 0;

//function LoadImage(ItmID)
//{ 
//  document.all.imgFrame.src = 'ItmVw.aspx?I=' + ItmID;
//}
//grdItm - color all rows when the main checkbox is clicked
function HighlighRowItm(srcElement) 
{
	//var cb = event.srcElement;
	var cb = srcElement;
	var curElement = cb;
	var iElement;
	var btnElement;
	var lID  = 0;
  var sRef = '';
  var sDes = '';
  var sNote = '';
  	
  //ColorSelectedItm(document.frmKitSel);
	ColorSelectedItm(document.getElementById('frmKitSel'));

	while (curElement && !(curElement.tagName.toLowerCase() == "tr")) 
	{
		//curElement = curElement.parentElement;
		curElement = curElement.parentNode;
	}
	if (!(curElement == cb) && (cb.name.indexOf('grdItm') > -1)) 
	{
		curElement.style.backgroundColor = "#FFFF80";
		iElement = curElement.getElementsByTagName('input')[0];
		lID = iElement.value;
		btnElement = curElement.getElementsByTagName('input')[2];
		btnElement.style.backgroundColor = "#FFFF80";
		sRef = btnElement.value;
		btnElement = curElement.getElementsByTagName('input')[3];
		btnElement.style.backgroundColor = "#FFFF80";
		sDes = btnElement.value;
		btnElement = curElement.getElementsByTagName('input')[4];
		btnElement.style.backgroundColor = "#FFFF80";
		sNote = btnElement.value;

		//document.getElementById("lblKitRef").innerText = sRef;
		//document.getElementById("lblKitDes").innerText = sDes;
		//document.getElementById("lblKitNote").innerText = sNote;
		document.getElementById("lblKitRef").innerHTML = sRef;
		document.getElementById("lblKitDes").innerHTML = sDes;
		document.getElementById("lblKitNote").innerHTML = sNote;
	  //LoadImage(lID);
	}
	//document.frmKitSel.txtItmSelCnt.value = iCntItmRow;
}
function SelectRowItm(srcElement) 
{
	//var cb = event.srcElement;
	var cb = srcElement;
	var curElement = cb;
	//var iElement;
	var btnElement;
	//var lID  = 0;
	
	while (curElement && !(curElement.tagName.toLowerCase() == "tr")) 
	{
		//curElement = curElement.parentElement;
		curElement = curElement.parentNode;
	}
	if (!(curElement == cb) && (cb.name.indexOf('grdItm') > -1)) 
	{
		if (cb.checked) 
		{
			curElement.style.backgroundColor = "#3DBC45";
			//curElement.style.backgroundColor = "#FFFF80";
			iCntItmRow = iCntItmRow + 1;
			//iElement = curElement.getElementsByTagName('input')[0];
			//lID = iElement.value;
			btnElement = curElement.getElementsByTagName('input')[2];
			btnElement.style.backgroundColor = "#3DBC45";
			//btnElement.style.backgroundColor = "#FFFF80";
			btnElement = curElement.getElementsByTagName('input')[3];
			btnElement.style.backgroundColor = "#3DBC45";
			//btnElement.style.backgroundColor = "#FFFF80";
	    //LoadImage(lID);
		}
		else 
		{
			curElement.style.backgroundColor = "white";
			iCntItmRow = iCntItmRow - 1;
			btnElement = curElement.getElementsByTagName('input')[2];
			btnElement.style.backgroundColor = "white";
			btnElement = curElement.getElementsByTagName('input')[3];
			btnElement.style.backgroundColor = "white";
		}
	}
	//document.frmKitSel.txtItmSelCnt.value = iCntItmRow;
	document.getElementById("txtItmSelCnt").value = iCntItmRow;
}
//grdItm - color all rows when the main checkbox is clicked
function SelectAllItm(form) 
{
	var thisNumRowsSelected = 0;
	var isChecked = document.all.chkItmSelectAll.checked;
	var btnElement;

  for (var i=0; i < form.elements.length; i++) 
  {
		if ((form.elements[i].name.indexOf('grdItm') > -1) && (form.elements[i].name.indexOf('ctl') > -1)) 
		{
			var curElement = form.elements[i];
			if (isChecked == true) 
			{
				curElement.checked = true;
				thisNumRowsSelected = thisNumRowsSelected + 1;
				while (!(curElement.tagName.toLowerCase() == "tr")) 
				{
					//curElement = curElement.parentElement;
					curElement = curElement.parentNode;
				}
				if (form.elements[i].name != "chkItmSelectAll") 
				{
					curElement.style.backgroundColor = "#3DBC45";
					btnElement = curElement.getElementsByTagName('input')[2];
					btnElement.style.backgroundColor = "#3DBC45";
					btnElement = curElement.getElementsByTagName('input')[3];
					btnElement.style.backgroundColor = "#3DBC45";
				}
			}
			else 
			{
				curElement.checked = false;
				while (!(curElement.tagName.toLowerCase() == "tr")) 
				{
					//curElement = curElement.parentElement;
					curElement = curElement.parentNode;
				}
				if (form.elements[i].name != "chkItmSelectAll") 
				{
					curElement.style.backgroundColor = "white";
					btnElement = curElement.getElementsByTagName('input')[2];
					btnElement.style.backgroundColor = "white";
					btnElement = curElement.getElementsByTagName('input')[3];
					btnElement.style.backgroundColor = "white";
				}
			}
		}
	}
	iCntItmRow = thisNumRowsSelected;
}
//grdItm - color checked rows when loaded
//ColorSelectedItm(document.frmKitSel);
ColorSelectedItm(document.getElementById('frmKitSel'));
function ColorSelectedItm(form) 
{
	var thisNumRowsSelected = 0;
	var btnElement;
	var chkElement;
	for (var i=0; i < form.elements.length; i++) 
	{
		if ((form.elements[i].name.indexOf('grdItm') > -1) && (form.elements[i].name.indexOf('ctl') > -1)) 
		{
			var curElement = form.elements[i];
			if (curElement.checked == true) 
			{
				thisNumRowsSelected = thisNumRowsSelected + 1;
				while (!(curElement.tagName.toLowerCase() == "tr")) 
				{
					//curElement = curElement.parentElement;
					curElement = curElement.parentNode;
					chkElement = curElement.getElementsByTagName('input')[1];
				}
				if ((form.elements[i].name != "chkItmSelectAll") && (curElement.style.backgroundColor != '#92ee8e')) 
				//&& (curElement.style.backgroundColor != '#ffff80')
				{
					curElement.style.backgroundColor = "#3DBC45";
					btnElement = curElement.getElementsByTagName('input')[2];
					btnElement.style.backgroundColor = "#3DBC45";
					btnElement = curElement.getElementsByTagName('input')[3];
					btnElement.style.backgroundColor = "#3DBC45";
				}
			}
			else
			{
				while (!(curElement.tagName.toLowerCase() == "tr")) 
				{
					//curElement = curElement.parentElement;
					curElement = curElement.parentNode;
					chkElement = curElement.getElementsByTagName('input')[1];
				}
				if ((curElement.style.backgroundColor != '#92ee8e')) 
				{
					//Selected for Cart
				  if (chkElement.checked)
				  {
						curElement.style.backgroundColor = "#3DBC45";
						btnElement = curElement.getElementsByTagName('input')[2];
						btnElement.style.backgroundColor = "#3DBC45";
						btnElement = curElement.getElementsByTagName('input')[3];
						btnElement.style.backgroundColor = "#3DBC45";
					}
					else
				  {
						//Must be in Cart
						if (chkElement.disabled == true)
						{
							curElement.style.backgroundColor = "#92ee8e";
							btnElement = curElement.getElementsByTagName('input')[2];
							btnElement.style.backgroundColor = "#92ee8e";
							btnElement = curElement.getElementsByTagName('input')[3];
							btnElement.style.backgroundColor = "#92ee8e";
						}
						//Nothing
						else
						{
							curElement.style.backgroundColor = "white";
							btnElement = curElement.getElementsByTagName('input')[2];
							btnElement.style.backgroundColor = "white";
							btnElement = curElement.getElementsByTagName('input')[3];
							btnElement.style.backgroundColor = "white";
						}
					}
				}			
			}
		}
	}
	iCntItmRow = thisNumRowsSelected;
}
</script>
<script language="javascript">
var iCntK1Row = 0;

function SelectRowKey(srcElement) 
{
	//var cb = event.srcElement;
	var cb = srcElement;
	var curElement = cb;
	var btnElement;
	var chkElement;
	
	while (curElement && !(curElement.tagName.toLowerCase() == "tr")) 
	{
		//curElement = curElement.parentElement;
		curElement = curElement.parentNode;
	}
	if (!(curElement == cb) && (cb.name.indexOf('grdK') > -1)) 
	{
		chkElement = curElement.getElementsByTagName('input')[0];
		btnElement = curElement.getElementsByTagName('input')[1];
		if (chkElement.value == "1") 
		{
			chkElement.value = "0";
			curElement.style.backgroundColor = "white";
			chkElement.style.backgroundColor = "white";
			btnElement.style.backgroundColor = "white";
			if (cb.name.indexOf('K1') > -1) iCntK1Row = iCntK1Row - 1;
		}
		else 
		{
			chkElement.value = "1";
			curElement.style.backgroundColor = "#FFFF80";
			chkElement.style.backgroundColor = "#FFFF80";
			btnElement.style.backgroundColor = "#FFFF80";
			if (cb.name.indexOf('K1') > -1) iCntK1Row = iCntK1Row + 1;
		}
	}
//if ((cb.name.indexOf('K1') > -1) && (document.getElementById("divK1Grid") != null)) document.getElementById('txtTestK1').value = parseInt(iCntK1Row);
	if (cb.name.indexOf('K1') > -1) SetClearButton("K1",iCntK1Row);
}
function SelectAllKey(form, sGrid) 
{
  var bVal = 0;
  
  if (sGrid.indexOf('K1') > -1) { if (iCntK1Row > 0) { bVal = 0; iCntK1Row = 0; } else { bVal = 1; } }
	
  for (var i=0; i < form.elements.length; i++) 
  {
		if ((form.elements[i].name.indexOf('grd' + sGrid) > -1) && (form.elements[i].name.indexOf('ctl') > -1) && (form.elements[i].name.indexOf('btnKeyword') > -1)) {
			var curElement = form.elements[i];

			if (bVal == 1) 
			{
				while (!(curElement.tagName.toLowerCase() == "tr")) 
				{
					//curElement = curElement.parentElement;
					curElement = curElement.parentNode;
				}
				curElement.style.backgroundColor = "#FFFF80";
 	      chkElement = curElement.getElementsByTagName('input')[0];
			  btnElement = curElement.getElementsByTagName('input')[1];
			  chkElement.value = bVal;
				btnElement.style.backgroundColor = "#FFFF80";
				if (form.elements[i].name.indexOf('K1') > -1) iCntK1Row = iCntK1Row + 1;
			}
			else 
			{
				while (!(curElement.tagName.toLowerCase() == "tr")) 
				{
					//curElement = curElement.parentElement;
					curElement = curElement.parentNode;
				}
				curElement.style.backgroundColor = "white";
 	      chkElement = curElement.getElementsByTagName('input')[0];
			  btnElement = curElement.getElementsByTagName('input')[1];
			  chkElement.value = bVal;
				btnElement.style.backgroundColor = "white";
			}
		}
	}
//if (document.getElementById("divK1Grid") != null) document.getElementById('txtTestK1').value = parseInt(iCntK1Row);
	if (document.getElementById("divK1Grid") != null) SetClearButton("K1",iCntK1Row);
}
//ColorSelectedKey(document.frmKitSel);
ColorSelectedKey(document.getElementById('frmKitSel'));
function ColorSelectedKey(form) {
	var btnElement;

//alert ('ColorSelectedKey:  ' + parseInt(iCntK1Row));
  iCntK1Row = 0;
  
	for (var i=0; i < form.elements.length; i++) 
	{
		if ((form.elements[i].name.indexOf('grdK') > -1) && (form.elements[i].name.indexOf('ctl') > -1)) 
		{
			var curElement = form.elements[i];
			if (curElement.value == '1') 
			{
				while (!(curElement.tagName.toLowerCase() == "tr")) 
				{
					//curElement = curElement.parentElement;
					curElement = curElement.parentNode;
				}
				curElement.style.backgroundColor = "#FFFF80";
				btnElement = curElement.getElementsByTagName('input')[1];
				btnElement.style.backgroundColor = "#FFFF80";
				
				if (form.elements[i].name.indexOf('K1') > -1) iCntK1Row = iCntK1Row + 1;
			}
		}
	}
//if (document.getElementById("divK1Grid") != null) document.getElementById('txtTestK1').value = parseInt(iCntK1Row);
	if (document.getElementById("divK1Grid") != null) SetClearButton("K1",iCntK1Row);
}
function SetClearButton(sGrid,iCnt)
{
  if (document.getElementById('div' + sGrid + 'Grid') != null) 
  {
		btnElement = document.getElementById('ibtn' + sGrid + 'Select');
		if (iCnt>0)
		{
			btnElement.src="images/btnDeselectAll.png"
			btnElement.title="Deselect All"
		}
		else
		{
			btnElement.src="images/btnSelectAll.png"
			btnElement.title="Select All"
		}
	}
}

function CheckComponentQuantityLimit(components, limits, arrayupperlimit, quantityrequested, checkbox) {
    var iCounter;
    for (iCounter=0;iCounter<=arrayupperlimit;iCounter++)
    {
        if (Number(limits[iCounter]) < Number(quantityrequested)) {
            alert('Too many ' + components[iCounter] + ' requested\nQuantity requested exceeds limit of ' + limits[iCounter] + ' pieces');
            checkbox.checked = false;
            return false;
        }
        else {
            return true;
        }
    }
}

function CheckOrderQuantities() {
    $('span.chkItem input:checked').each(function() {
        //$(this).click();
        this.onclick();
    });
}


</script>
