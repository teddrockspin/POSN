<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ItmSel.aspx.vb" Inherits="POSN.ItmSel" %>
<link rel="stylesheet" href="../../themes/base/jquery.ui.all.css">
	<script src="accordian/jquery-1.9.1.js"></script>
	<script src="accordian/jquery.ui.core.js"></script>
	<script src="accordian/jquery.ui.widget.js"></script>
	<script src="accordian/jquery.ui.button.js"></script>
	<script src="accordian/jquery.ui.tooltip.js"></script>


<link rel="stylesheet" type="text/css" href="ItmSel.css" />
<script src="./js/jquery.browser.min.js"></script>

<!--[if lt IE 9]>

<![endif]-->
<style>
    .ui-accordion .ui-accordion-header
{
    height:0px !important;
    visibility: hidden !important;
}
</style>
<script>

    var bBar;



    function accord() {


        $("[id^='accordion']").hide();
        

        $('#tooltip1').hide();
        $('#tooltip2').hide();

        $('.newWindow').button();

        if ($.browser.name == 'msie' && $.browser.versionNumber > 8) {
            $(document).tooltip(
            {
                items: ".tooltip1,.tooltip2",
                content: function () {
                    return $("#" + $(this).attr('class')).html();
                }


            }
    );
        }


        function openChild(href) {
            alert('openNewWidow');
            window.open('google.com', 'winname', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=400,height=350');
        };

        $('.newWindow').off().click(function (event) {
            var url = $(this).attr("data-href");

            var windowName = "popUp";//$(this).attr("name");
            var windowSize = "menubar=0,resizable=0";


            window.open(url, windowName, windowSize);

            event.preventDefault();

            return false;

        });

    }


    $(document).ready(function () {
        //alert('doc.ready triggered');
        if (document.documentElement.scrollHeight === document.documentElement.clientHeight) {
            bBar = false;

        } else {
            bBar = true;
        }
        accord();
    });
</script>


<form id="frmItmSel" onsubmit="javascript:SaveDocScroll();" method="post" runat="server">
   
<pre lang="aspnet"></pre>
<tr>
<td valign="top">
    <center>
    <div id="Div1" dir="ltr" style="height: 100%;" runat="server">
        <div dir="ltr" id="ltr">
                <asp:hiddenfield runat="server" id="hfShowStock" value="false"></asp:hiddenfield>
                <table cellspacing="0" cellpadding="0" align="center" border="0" id="tablecontent">
                    <tbody>
                        <tr valign="top">
                            <td>
                                <table align="center" border="0">
                                    <tbody>
                                        <tr>
                                            <td valign="top" align="center" colspan="4"></td>
                                        </tr>
                                        <tr>
                                            <td valign="top" >
                                                <table align="center">
                                                    <tbody>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <asp:panel id="pnlK1" runat="server" visible="false" height="100px">
																	<TABLE width="264px" cellSpacing="0" align="center">
																		<TR>
																			<TD vAlign="middle" align="left" colSpan="1">
																				<asp:label id="lblK1" runat="server" text="Keyword Type 1" CssClass="lblListB"></asp:label></TD>
																			<TD align="right">
																				<asp:imagebutton id="ibtnK1Select" Runat="server" ToolTip="Select All" ImageURL="images/btnSelectAll.png"></asp:imagebutton></TD>
																		</TR>
																		<TR>
																			<TD colSpan="2">
																				<DIV id=divK1Grid 
                        style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; OVERFLOW:auto; overflow-x:hidden; BORDER-LEFT: gray 1px solid; WIDTH: 100%; BORDER-BOTTOM: gray 1px solid; HEIGHT: 88px; BACKGROUND-COLOR: white" 
                        onscroll="javascript:SaveDivScroll(this, <% =ScrollXPosK1.ClientID %>);">
																					<asp:datagrid id="grdK1" runat="server" OnItemCommand="grdK1_ItemCommand" AutoGenerateColumns="False"
																						ShowFooter="True" CellPadding="1" BorderWidth="1px" BorderColor="Black" Font-Size="Smaller"
																						BorderStyle="None" OnItemDataBound="grdK1_ItemDataBound" GridLines="Horizontal" ShowHeader="False"
																						Width="100%">
																						<SelectedItemStyle CssClass="grdListSelect" BackColor="#FFFF80"></SelectedItemStyle>
																						<ItemStyle CssClass="grdListBody"></ItemStyle>
																						<HeaderStyle Font-Italic="True" Font-Bold="True" CssClass="grdListHead"></HeaderStyle>
																						<Columns>
																							<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
																							<asp:TemplateColumn HeaderText="Check">
																								<HeaderStyle Width="1%"></HeaderStyle>
																								<ItemStyle HorizontalAlign="Center"></ItemStyle>
																								<ItemTemplate>
																									<input id="chkSelect" runat="server" type="hidden" value="0" NAME="inpChkK1">
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
																</asp:panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <asp:panel id="pnlK2" runat="server" visible="false" height="100px">
																	<TABLE width="264px" cellSpacing="0"  align="center">
																		<TR>
																			<TD vAlign="middle" align="left" colSpan="1">
																				<asp:label id="lblK2" runat="server" text="Keyword Type 2" CssClass="lblListB"></asp:label></TD>
																			<TD align="right">
																				<asp:imagebutton id="ibtnK2Select" Runat="server" ToolTip="Select All" ImageURL="images/btnSelectAll.png"></asp:imagebutton></TD>
																		</TR>
																		<TR>
																			<TD colSpan="2">
																				<DIV id=divK2Grid 
                        style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; OVERFLOW: auto; overflow-x:hidden;BORDER-LEFT: gray 1px solid; WIDTH: 100%; BORDER-BOTTOM: gray 1px solid; HEIGHT: 88px; BACKGROUND-COLOR: white" 
                        onscroll="javascript:SaveDivScroll(this, <% =ScrollXPosK2.ClientID %>);">
																					<asp:datagrid id="grdK2" runat="server" OnItemCommand="grdK2_ItemCommand" AutoGenerateColumns="False"
																						ShowFooter="True" CellPadding="1" BorderWidth="1px" BorderColor="Black" Font-Size="Smaller"
																						BorderStyle="None" OnItemDataBound="grdK2_ItemDataBound" GridLines="Horizontal" ShowHeader="False"
																						Width="100%">
																						<SelectedItemStyle CssClass="grdListSelect" BackColor="#FFFF80"></SelectedItemStyle>
																						<ItemStyle CssClass="grdListBody"></ItemStyle>
																						<HeaderStyle Font-Italic="True" Font-Bold="True" CssClass="grdListHead"></HeaderStyle>
																						<Columns>
																							<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
																							<asp:TemplateColumn Visible="true" HeaderText="Check">
																								<HeaderStyle Width="1%"></HeaderStyle>
																								<ItemStyle HorizontalAlign="Center"></ItemStyle>
																								<ItemTemplate>
																									<input id="chkSelect" runat="server" type="hidden" value="0" NAME="inpChkK2">
																								</ItemTemplate>
																							</asp:TemplateColumn>
																							<asp:BoundColumn Visible="False" DataField="Keyword" HeaderText="Select Keyword">
																								<HeaderStyle Width="95%"></HeaderStyle>
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
																</asp:panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <asp:panel id="pnlK3" runat="server" visible="false" height="100px">
																	<TABLE width="264px" cellSpacing="0" align="center">
																		<TR>
																			<TD vAlign="middle" align="left" colSpan="1">
																				<asp:label id="lblK3" runat="server" text="Keyword Type 3" CssClass="lblListB"></asp:label></TD>
																			<TD align="right">
																				<asp:imagebutton id="ibtnK3Select" Runat="server" ToolTip="Select All" ImageURL="images/btnSelectAll.png"></asp:imagebutton></TD>
																		</TR>
																		<TR>
																			<TD colSpan="2">
																				<DIV id=divK3Grid 
                        style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; OVERFLOW: auto; overflow-x:hidden; BORDER-LEFT: gray 1px solid; WIDTH: 100%; BORDER-BOTTOM: gray 1px solid; HEIGHT: 88px; BACKGROUND-COLOR: white" 
                        onscroll="javascript:SaveDivScroll(this, <% =ScrollXPosK3.ClientID %>);">
																					<asp:datagrid id="grdK3" runat="server" OnItemCommand="grdK3_ItemCommand" AutoGenerateColumns="False"
																						ShowFooter="True" CellPadding="1" BorderWidth="1px" BorderColor="Black" Font-Size="Smaller"
																						BorderStyle="None" OnItemDataBound="grdK3_ItemDataBound" GridLines="Horizontal" ShowHeader="False"
																						Width="100%">
																						<SelectedItemStyle CssClass="grdListSelect" BackColor="#FFFF80"></SelectedItemStyle>
																						<ItemStyle CssClass="grdListBody"></ItemStyle>
																						<HeaderStyle Font-Italic="True" Font-Bold="True" CssClass="grdListHead"></HeaderStyle>
																						<Columns>
																							<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
																							<asp:TemplateColumn Visible="true" HeaderText="Check">
																								<HeaderStyle Width="1%"></HeaderStyle>
																								<ItemStyle HorizontalAlign="Center"></ItemStyle>
																								<ItemTemplate>
																									<input id="chkSelect" runat="server" type="hidden" value="0" NAME="inpChkK3">
																								</ItemTemplate>
																							</asp:TemplateColumn>
																							<asp:BoundColumn Visible="False" DataField="Keyword" HeaderText="Select Keyword">
																								<HeaderStyle Width="95%"></HeaderStyle>
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
																</asp:panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <asp:panel id="pnlK4" runat="server" visible="false" height="100px" style="margin: 0 0 15px 0;">
																	<TABLE  width="264px" cellSpacing="0" align="center">
																		<TR>
																			<TD vAlign="middle" align="left" colSpan="1">
																				<asp:label id="lblK4" runat="server" text="Keyword Type 4" CssClass="lblListB"></asp:label></TD>
																			<TD align="right">
																				<asp:imagebutton id="ibtnK4Select" Runat="server" ToolTip="Select All" ImageURL="images/btnSelectAll.png"></asp:imagebutton></TD>
																		</TR>
																		<TR>
																			<TD colSpan="2">
																				<DIV id=divK4Grid 
                        style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; OVERFLOW: auto; overflow-x:hidden; BORDER-LEFT: gray 1px solid; WIDTH: 100%; BORDER-BOTTOM: gray 1px solid; HEIGHT: 88px; BACKGROUND-COLOR: white" 
                        onscroll="javascript:SaveDivScroll(this, <% =ScrollXPosK4.ClientID %>);">
																					<asp:datagrid id="grdK4" runat="server" OnItemCommand="grdK4_ItemCommand" AutoGenerateColumns="False"
																						ShowFooter="True" CellPadding="1" BorderWidth="1px" BorderColor="Black" Font-Size="Smaller"
																						BorderStyle="None" OnItemDataBound="grdK4_ItemDataBound" GridLines="Horizontal" ShowHeader="False"
																						Width="100%">
																						<SelectedItemStyle CssClass="grdListSelect" BackColor="#FFFF80"></SelectedItemStyle>
																						<ItemStyle CssClass="grdListBody"></ItemStyle>
																						<HeaderStyle Font-Italic="True" Font-Bold="True" CssClass="grdListHead"></HeaderStyle>
																						<Columns>
																							<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
																							<asp:TemplateColumn Visible="true" HeaderText="Check">
																								<HeaderStyle Width="1%"></HeaderStyle>
																								<ItemStyle HorizontalAlign="Center"></ItemStyle>
																								<ItemTemplate>
																									<input id="chkSelect" runat="server" type="hidden" value="0" NAME="inpChkK4">
																								</ItemTemplate>
																							</asp:TemplateColumn>
																							<asp:BoundColumn Visible="False" DataField="Keyword" HeaderText="Select Keyword">
																								<HeaderStyle Width="95%"></HeaderStyle>
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
																</asp:panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="middle" align="left" colspan="1">
                                                                <asp:imagebutton id="ibtnRestart" imageurl="images/btnRestart.png" tooltip="Clear Tags Selected"
                                                                    runat="server"></asp:imagebutton>
                                                            </td>
                                                            <td valign="middle" align="right">
                                                                <asp:imagebutton id="ibtnGet" imageurl="images/btnResult.png" tooltip="Search By Tags Selected"
                                                                    runat="server"></asp:imagebutton>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td valign="top">
                                <table  align="center" border="0">
                                    <tbody>
                                        <tr>

                                            <td align="left" colspan="1">
                                                <table cellspacing="0" cellpadding="1" align="Left">
                                                    <tbody>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:label id="lblTitle" runat="server" cssclass="lblTitle" text="Available Items"></asp:label>
                                                            </td>
                                                            <td valign="middle" align="left" colspan="1">
                                                                <asp:image id="imgCheck" runat="server" imageurl="images/Check.png" visible="False"></asp:image>
                                                                &nbsp;&nbsp;<asp:label id="lblSelect" runat="server" cssclass="lblShortSmall" text=""></asp:label><asp:label
                                                                    id="lblItem" runat="server" cssclass="lblListB" text="Items..." visible="false"></asp:label>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>

                                            <td valign="middle" align="right" colspan="1">
                                                <asp:textbox id="txtSearch" runat="server" cssclass="txtMedium" maxlength="100"></asp:textbox>
                                                &nbsp;&nbsp;
                                            <asp:imagebutton id="ibtnSearch" imageurl="images/btnSearch.png" tooltip="Search By Text Entered"
                                                runat="server"></asp:imagebutton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div id="divItmGrid" style="width:628px; display:block; padding:0px;border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid; border-bottom: gray 1px solid; min-height: 473px; background-color: white"
                                                    onscroll="javascript:SaveDivScroll(this, <% =ScrollXPosItm.ClientID %>);">
                                                    <asp:datagrid id="grdItm" width="628px" runat="server" 
                                                        onitemdatabound="grdItm_ItemDataBound" 
                                                        font-size="Smaller" cellpadding="0"
                                                        autogeneratecolumns="False"
                                                        allowpaging="True" pagesize="15" pagerstyle-mode="NumericPages" pagerstyle-visible="False">
														<SelectedItemStyle CssClass="grdListSelectItm" BackColor="#FFFF80"></SelectedItemStyle>
                                                        <PagerStyle Mode="NumericPages" Visible="False"></PagerStyle>

														<ItemStyle ></ItemStyle>
														<Columns>
                                                            <asp:BoundColumn DataField="ID" HeaderText="ID" Visible="False">
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="&nbsp;Order for Delivery&nbsp;&nbsp;" ItemStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <input id="inpID" runat="server" name="inpItmID" type="hidden" 
                                                                        value='<%# DataBinder.Eval(Container,"DataItem.ID")%>' />
                                                                        <span class="tooltip1"><asp:CheckBox ID="chkSelect" runat="server" CssClass="chkItem" /></span>
                                                                        <asp:Label ID="lblOrderQuantityLimit" runat="server" style="display: none;"
                                                                            Text='<%# DataBinder.Eval(Container,"DataItem.OrderQuantityLimit") %>'></asp:Label>
                                                                    </input>
                                                                </ItemTemplate>
                                                                <HeaderStyle/>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateColumn>
                                                             <asp:TemplateColumn HeaderText="Digital Downloads" ItemStyle-VerticalAlign="Top"  HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="hdnDnld" HeaderStyle-CssClass="hdnDnld">
                                                                <ItemTemplate>
                                                                        <asp:HiddenField runat="server" ID="hfDownloadID" />
                                                                        <span class="tooltip2">
                                                                            <% If Me.qoEnabled Then%>
                                                                                <asp:Label ID="lblQDAvail" text="" CssClass="grdItmAvailable" runat="server"/>
                                                                            <%Else%>
                                                                            <asp:CheckBox ID="chkSelectDownload" runat="server" CssClass="chkItem" Width="60px"/>
                                                                            <%End If%>
                                                                        </span>
                                                                </ItemTemplate>
                                                                <HeaderStyle/>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="Status" Visible="False" ItemStyle-VerticalAlign="Top">
                                                                <HeaderStyle Width="1%" />
                                                            </asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText="Part No" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnItmNo" Runat="server" cssClass="btnItmNoList" 
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "RefNo").ToString()%>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="18%" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Description" ItemStyle-VerticalAlign="Top"  ItemStyle-HorizontalAlign="Left" >
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnItmName" Runat="server" cssClass="btnItmNameList" 
                                                                        Text='<%# LEFT(DataBinder.Eval(Container.DataItem, "RefName").ToString(),65) & vbcrlf & MID(DataBinder.Eval(Container.DataItem, "RefName").ToString(),66) %>' />
                                                                    <asp:literal id="ltDownloadables" runat="server"></asp:literal>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="70%" />
                                                                <ItemStyle HorizontalAlign="left" />
                                                            </asp:TemplateColumn>
                                                            <%--<asp:TemplateColumn >
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" style="display: none;"
                                                                        Text='<%# DataBinder.Eval(Container, "DataItem.OrderQuantityLimit") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>--%>
                                                             <asp:TemplateColumn  ItemStyle-VerticalAlign="Top" headertext="&nbsp;Status&nbsp;" ItemStyle-CssClass="hdn" HeaderStyle-CssClass="hdn">
                                                                <ItemTemplate><asp:button id="btnStock" runat="server" text='<%#getShowStock(DataBinder.Eval(Container, "DataItem.ID"))%>' cssClass="btnStockShow" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                         
                                                        </Columns>
														<HeaderStyle Font-Italic="True" Font-Bold="True" CssClass="grdListHead"></HeaderStyle>
													</asp:datagrid>
                                                </div>
                                                <center>
                                                <asp:linkbutton id="lbtnItmFirst" onclick="PagerMove" runat="server" visible="false"
                                                    text="<< 1st Page" commandargument="0" cssclass="lnkPagerButton"></asp:linkbutton>
                                                &nbsp;<asp:linkbutton id="lbtnItmPrev" onclick="PagerMove" runat="server" visible="false"
                                                    text="< Previous" commandargument="prev" cssclass="lnkPagerButton"></asp:linkbutton>
                                                &nbsp;<asp:linkbutton id="lbtnItmNext" onclick="PagerMove" runat="server" visible="false"
                                                    text="Next >" commandargument="next" cssclass="lnkPagerButton"></asp:linkbutton>
                                                &nbsp;<asp:linkbutton id="lbtnItmLast" onclick="PagerMove" runat="server" visible="false"
                                                    text="Last Page >>" commandargument="last" cssclass="lnkPagerButton"></asp:linkbutton>
                                                <asp:label id="lblItmPCnt" runat="server" width="100%" cssclass="lblPagerNote"></asp:label>
                                            </center>
                                                <input id="inpPageType" type="hidden" value="-" name="inpPageType" runat="server">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="subnotes" colspan="2">* Items in <font color="#bd5304">orange</font> are on backorder and not available.</font>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            
        </div>
    </div>
        </center>
</td>
<td valign="top">
    <table width="100%" align="center">
        <tr>
            <td align="left" colspan="2">
                <table cellspacing="0" width="100%" align="center" border="0">
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td background="">
                            <iframe id="imgFrame" hidefocus marginwidth="0" marginheight="0" src="ItmVw.aspx?I=0"
                                frameborder="0" width="256" scrolling="auto" height="425"></iframe>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</td>
</TR>
<tr>
    <td valign="top" align="center" colspan="2">&nbsp;
    </td>
</tr>
<tr>
    <td colspan="2">
        <asp:panel id="pnlBtnOrd" runat="server" visible="false">
			<TABLE align="center"  class="footercommon" border="0">
				<TR>
					
					<TD vAlign="middle" align="left" nowrap >
						<asp:label id="lblQtyOrd" runat="server" text="Quantity of each Item to be Added" CssClass="lblDLong"></asp:label>&nbsp;&nbsp;</TD>
                    <td width="1%"><asp:textbox id="txtQtyOrd" runat="server" CssClass="txtNumber" MaxLength="9" Text="1">1</asp:textbox></td>
                    <TD vAlign="middle" align="left"><asp:imagebutton id="ibtnAddOrd" runat="server" ToolTip="Add Selected Items" ImageUrl="images/btnAddCart.png"></asp:imagebutton></TD>
					<TD width="75%">&nbsp;</TD>
					<TD vAlign="middle" >
						<asp:imagebutton id="ibtnKitOrd" runat="server" ToolTip="View Kit Catalog" ImageUrl="images/btnKitCat.png"
							CausesValidation="False"></asp:imagebutton></TD>
					<TD vAlign="middle">
						<asp:imagebutton id="ibtnCartOrd" runat="server" ToolTip="View Shopping Cart" ImageUrl="images/btnCart.png"></asp:imagebutton></TD>
					<TD width="75"></TD>
				</TR>
			</TABLE>
		</asp:panel>
        <asp:panel id="pnlBtnItm" runat="server" visible="false">
			<TABLE width="100%" align="center"  class="footercommon">
				<TR>
					<TD align="center" width="350"></TD>
					<TD width="75"></TD>
					<TD width="75"></TD>
					<TD width="75">
						<asp:imagebutton id="ibtnBackItm" runat="server" ToolTip="Disregard Changes and Return to Previously Viewed Screen"
							ImageUrl="images/btnBack.png" CausesValidation="False"></asp:imagebutton></TD>
					<TD width="75">
						<asp:imagebutton id="ibtnEditItm" runat="server" ToolTip="Edit Selected Items" ImageUrl="images/btnEdit.png"></asp:imagebutton></TD>
				</TR>
			</TABLE>
		</asp:panel>
        <asp:panel id="pnlBtnKit" runat="server" visible="false">
			<TABLE width="100%" align="center" class="footercommon">
				<TR>
					<TD vAlign="middle" align="center" width="290">
						<asp:textbox id="txtQtyKit" runat="server" CssClass="txtNumber" MaxLength="4" Text="1">1</asp:textbox>
						<asp:label id="lblQtyKit" runat="server" text="Quantity of each Item to be Added" CssClass="lblDLong"></asp:label></TD>
					<TD align="left" width="155">
						<asp:imagebutton id="ibtnAddKit" runat="server" ToolTip="Add Selected Items" ImageUrl="images/btnAdd.png"></asp:imagebutton></TD>
					<TD align="left" width="155">&nbsp;</TD>
					<TD width="75">
						<asp:imagebutton id="ibtnBackKit" runat="server" ToolTip="Disregard Changes and Return to Previously Viewed Screen"
							ImageUrl="images/btnBack.png" CausesValidation="False"></asp:imagebutton></TD>
					<TD width="75">&nbsp;</TD>
					<TD width="75">&nbsp;</TD>
				</TR>
			</TABLE>
		</asp:panel>
    </td>
</tr>

<tr class="ui-helper-hidden-accessible">
    <td colspan="2">
        <table align="center" bgcolor="white">
            <tbody>
                <tr>
                    <td align="right" colspan="1">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="center" colspan="2">
                        <asp:comparevalidator id="vMaxQty" runat="server" cssclass="vldMedium" text="*" errormessage="CompareValidator"
                            type="Integer" operator="LessThanEqual" display="Dynamic" controltovalidate="txtQtyOrd">*</asp:comparevalidator>
                    </td>
                    <td align="center" colspan="2">&nbsp;
                    </td>
                    <td>
                        <input id="txtSubmit" name="txtSubmit" runat="server" type="hidden">
                        <input id="txtItmRow" name="txtItmRow" runat="server" type="hidden" value="0">
                        <input id="txtItmSelCnt" name="txtItmSelCnt" runat="server" value="0" type="hidden">
                        <input id="txtItmsSelected" name="txtItmsSelected" runat="server" value="" type="hidden" />
                        <input id="txtItmSelFrom" name="txtItmSelFrom" runat="server" type="hidden">
                        <input id="ScrollXPosK1" type="hidden" name="ScrollXPosK1" runat="server">
                        <input id="ScrollXPosK2" type="hidden" name="ScrollXPosK2" runat="server">
                        <input id="ScrollXPosK3" type="hidden" name="ScrollXPosK3" runat="server">
                        <input id="ScrollXPosK4" type="hidden" name="ScrollXPosK4" runat="server">
                        <input id="ScrollXPosItm" type="hidden" name="ScrollXPosItm" runat="server">
                        <input id="ScrollYPosItm" type="hidden" name="ScrollYPosItm" runat="server">
                        <input id="ScrollXPosDoc" type="hidden" name="ScrollXPosDoc" runat="server">
                        <input id="ScrollYPosDoc" type="hidden" name="ScrollYPosDoc" runat="server">
                        <br>
                        <div id="footer" style="font-size: xx-small; color: #F8F8F8;">
                            Load Time:
                            <asp:literal runat="server" id="LoadTime" />
                            <div id="tooltip1">
                                <h4>Order:</h4>
                                <p>to order for delivery</p>
                            </div>
<div id="tooltip2">
    <h4>Download:</h4>
    <p>to request download files</p>
</div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </td>
</tr>
    
<script lang="javascript">
    window.onload = RestoreScrollPosition;

    function RestoreScrollPosition() {
        document.body.scrollTop = document.getElementById("ScrollXPosDoc").value;
        document.body.scrollLeft = document.getElementById("ScrollYPosDoc").value;
        document.getElementById("divItmGrid").scrollTop = document.getElementById("ScrollXPosItm").value;
        if (document.getElementById("divK1Grid") != null)
            document.getElementById("divK1Grid").scrollTop = document.getElementById("ScrollXPosK1").value;
        if (document.getElementById("divK2Grid") != null)
            document.getElementById("divK2Grid").scrollTop = document.getElementById("ScrollXPosK2").value;
        if (document.getElementById("divK3Grid") != null)
            document.getElementById("divK3Grid").scrollTop = document.getElementById("ScrollXPosK3").value;
        if (document.getElementById("divK4Grid") != null)
            document.getElementById("divK4Grid").scrollTop = document.getElementById("ScrollXPosK4").value;
    }
    function SaveDivScroll(itm, itmxholder) {
        itmxholder.value = itm.scrollTop;
    }
    function SaveDocScroll() {
        document.getElementById("ScrollXPosDoc").value = document.body.scrollTop;
        document.getElementById("ScrollYPosDoc").value = document.body.scrollLeft;
    }
</script>

<script lang="javascript">
    $(document).ready(function () {
            $("#<%=txtQtyOrd.ClientID%>").on("change", function(){
                var selectedItems = document.frmItmSel.txtItmsSelected.value
                $.ajax({
                    type: "GET",
                    url: "ItmSel.aspx",
                    data: 'ajax=1&type=validateitems&data=' + selectedItems + '&qty=' + $(this).val(),
                    success: function (msg) {
                        //console.log(msg);
                        $("#<%= ibtnAddOrd.ClientID %>").attr("onclick", msg);

                    },
                    failure: function (r) {
                        console.log(r);
                    }
                });
            });
    });

    function ConfirmBackOrder(selectedItems, qty){
    
    }
    
    var iCntItmRow = 0;

    function LoadImage(ItmID) {
        document.getElementById('imgFrame').src = 'ItmVw.aspx?I=' + ItmID;
    }
    //grdItm - color all rows when the main checkbox is clicked
    function HighlighRowItm(srcElement) {
        //var cb = event.srcElement;
        var cb = srcElement;
        var curElement = cb;
        var iElement;
        var btnElement;
        var lID = 0;

        ColorSelectedItm(document.getElementById('frmItmSel'));
        while (curElement && !(curElement.tagName.toLowerCase() == "tr")) {
            //curElement = curElement.parentElement; - was commented out before
            curElement = curElement.parentNode;
        }

        if (!(curElement == cb) && (cb.name.indexOf('grdItm') > -1)) {
            curElement.style.backgroundColor = "#FFFF80";
            iElement = curElement.getElementsByTagName('input')[0];
            lID = iElement.value;

            btnElement = curElement.getElementsByTagName('input')[2];
            btnElement.style.backgroundColor = "#FFFF80";

            btnElement = curElement.getElementsByTagName('input')[3];
            btnElement.style.backgroundColor = "#FFFF80";

            btnElement = curElement.getElementsByTagName('input')[4];
            btnElement.style.backgroundColor = "#FFFF80";

            btnElement = curElement.getElementsByTagName('input')[5];
            if (typeof (btnElement) != "undefined") {
                btnElement.style.backgroundColor = "#FFFF80";
            }
           

            divElement = curElement.getElementsByTagName('div')[0];
            //alert(divElement.id);
            //accord();
            //alert($('#' + divElement.id).is(":visible"));
            if (divElement != null) {
                if ($('#' + divElement.id).is(":visible"))
                {
                    $("[id^='accordion']").hide('slide');
                }
                else
                {
                    $("[id^='accordion']").hide('slide');
                    $('#' + divElement.id).slideToggle();
                }
            }
            //divElement.trigger('click');
            //divElement.toggle();
            //alert(divElement.id);
            LoadImage(lID);
            //console.log('--in ' + cb.name);
        }
        else {
            //console.log('--' + cb.name);
        }

        //
    }
    function SelectRowItm(srcElement) {
        //var cb = event.srcElement;
        var cb = srcElement;
        var curElement = cb;
        //var iElement;
        var btnElement;
        //var lID  = 0;

        while (curElement && !(curElement.tagName.toLowerCase() == "tr")) {
            //curElement = curElement.parentElement;
            curElement = curElement.parentNode;
        }

        HighlighRowItm(srcElement);

        if (!(curElement == cb) && (cb.name.indexOf('grdItm') > -1)) {
            if (cb.checked) {
                curElement.style.backgroundColor = "#3DBC45";
                iCntItmRow = iCntItmRow + 1;
                btnElement = curElement.getElementsByTagName('input')[2];
                btnElement.style.backgroundColor = "#3DBC45";
                btnElement = curElement.getElementsByTagName('input')[3];
                btnElement.style.backgroundColor = "#3DBC45";
                btnElement = curElement.getElementsByTagName('input')[4];
                btnElement.style.backgroundColor = "#3DBC45";
                btnElement = curElement.getElementsByTagName('input')[5];
                if (typeof (btnElement) != "undefined") {
                    btnElement.style.backgroundColor = "#3DBC45";
                }

                //add selected item to hidden field
              document.frmItmSel.txtItmsSelected.value += curElement.getElementsByTagName('input')[0].value + ",";

              var selectedItems = document.frmItmSel.txtItmsSelected.value
                $.ajax({
                    type: "GET",
                    url: "ItmSel.aspx",
                    data: 'ajax=1&type=validateitems&data=' + selectedItems + '&qty=' + $("#<%= txtQtyOrd.ClientID%>").val(),
                    success: function (msg) {
                        //console.log(msg);
                        $("#<%= ibtnAddOrd.ClientID %>").attr("onclick", msg);
                
                    },
                    failure: function(r) {
                        console.log(r);
                    }
                });
            
           
            }
            else {
                curElement.style.backgroundColor = "white";
                iCntItmRow = iCntItmRow - 1;
                btnElement = curElement.getElementsByTagName('input')[2];
                btnElement.style.backgroundColor = "white";
                btnElement = curElement.getElementsByTagName('input')[3];
                btnElement.style.backgroundColor = "white";
                btnElement = curElement.getElementsByTagName('input')[4];
                btnElement.style.backgroundColor = "white";
                btnElement = curElement.getElementsByTagName('input')[5];
                if (typeof(btnElement) != "undefined") {
                    btnElement.style.backgroundColor = "white";
                }

                //remove unselected item from hidden field
                var str = document.frmItmSel.txtItmsSelected.value;
                str = str.replace(curElement.getElementsByTagName('input')[0].value + ",", "");
                document.frmItmSel.txtItmsSelected.value = str;
            }
        }

        
        document.frmItmSel.txtItmSelCnt.value = iCntItmRow;




    }

    function SelectRowItmDnld(srcElement) {
        //var cb = event.srcElement;
        var cb = srcElement;
        var curElement = cb;
        //var iElement;
        var btnElement;
        //var lID  = 0;

        while (curElement && !(curElement.tagName.toLowerCase() == "tr")) {
            //curElement = curElement.parentElement;
            curElement = curElement.parentNode;
        }

        HighlighRowItm(srcElement);

        if (!(curElement == cb) && (cb.name.indexOf('grdItm') > -1)) {
            if (cb.checked) {
                curElement.style.backgroundColor = "#3DBC45";
                iCntItmRow = iCntItmRow + 1;
                btnElement = curElement.getElementsByTagName('input')[2];
                btnElement.style.backgroundColor = "#3DBC45";
                btnElement = curElement.getElementsByTagName('input')[3];
                btnElement.style.backgroundColor = "#3DBC45";
                btnElement = curElement.getElementsByTagName('input')[4];
                btnElement.style.backgroundColor = "#3DBC45";
                btnElement = curElement.getElementsByTagName('input')[5];
               
                if (typeof (btnElement) != "undefined") {
                    btnElement.style.backgroundColor = "#3DBC45";
                }
               
            }
            else {
                curElement.style.backgroundColor = "";
                iCntItmRow = iCntItmRow - 1;
                btnElement = curElement.getElementsByTagName('input')[2];
                btnElement.style.backgroundColor = "";
                btnElement = curElement.getElementsByTagName('input')[3];
                btnElement.style.backgroundColor = "";
                btnElement = curElement.getElementsByTagName('input')[4];
                btnElement.style.backgroundColor = "";
                btnElement = curElement.getElementsByTagName('input')[5];

                if (typeof (btnElement) != "undefined") {
                    btnElement.style.backgroundColor = "";
                }
                
            }
        }

        document.frmItmSel.txtItmSelCnt.value = iCntItmRow;




    }

    //grdItm - color all rows when the main checkbox is clicked
    function SelectAllItm(form) {
        var thisNumRowsSelected = 0;
        var isChecked = document.all.chkItmSelectAll.checked;
        var btnElement;

        for (var i = 0; i < form.elements.length; i++) {
            if ((form.elements[i].name.indexOf('grdItm') > -1) && (form.elements[i].name.indexOf('ctl') > -1)) {
                var curElement = form.elements[i];
                if (isChecked == true) {
                    curElement.checked = true;
                    thisNumRowsSelected = thisNumRowsSelected + 1;
                    while (!(curElement.tagName.toLowerCase() == "tr")) {
                        //curElement = curElement.parentElement;
                        curElement = curElement.parentNode;
                    }
                    if (form.elements[i].name != "chkItmSelectAll") {
                        curElement.style.backgroundColor = "#3DBC45";
                        btnElement = curElement.getElementsByTagName('input')[2];
                        btnElement.style.backgroundColor = "#3DBC45";
                        btnElement = curElement.getElementsByTagName('input')[3];
                        btnElement.style.backgroundColor = "#3DBC45";
                        btnElement = curElement.getElementsByTagName('input')[4];
                        btnElement.style.backgroundColor = "#3DBC45";
                        btnElement = curElement.getElementsByTagName('input')[5];
                        if (typeof (btnElement) != "undefined") {
                            btnElement.style.backgroundColor = "#3DBC45";
                        }
                        
                    }
                }
                else {
                    curElement.checked = false;
                    while (!(curElement.tagName.toLowerCase() == "tr")) {
                        //curElement = curElement.parentElement;
                        curElement = curElement.parentNode;
                    }
                    if (form.elements[i].name != "chkItmSelectAll") {
                        curElement.style.backgroundColor = "white";
                        btnElement = curElement.getElementsByTagName('input')[2];
                        btnElement.style.backgroundColor = "white";
                        btnElement = curElement.getElementsByTagName('input')[3];
                        btnElement.style.backgroundColor = "white";
                        btnElement = curElement.getElementsByTagName('input')[4];
                        btnElement.style.backgroundColor = "white";
                        btnElement = curElement.getElementsByTagName('input')[5];
                        if (typeof (btnElement) != "undefined") {
                            btnElement.style.backgroundColor = "white";
                        }
                       
                    }
                }
            }
        }
        iCntItmRow = thisNumRowsSelected;
    }
    //grdItm - color checked rows when loaded
    //ColorSelectedItm(document.frmItmSel);
    ColorSelectedItm(document.getElementById('frmItmSel'));

    function ColorSelectedItm(form) {
        //here:

        var thisNumRowsSelected = 0;
        var btnElement;
        var chkElement;


        for (var i = 0; i < form.elements.length; i++) {

            if ((form.elements[i].name.indexOf('grdItm') > -1) && (form.elements[i].name.indexOf('ctl') > -1)) {
                var curElement = form.elements[i];

                if (curElement.checked == true) {

                    thisNumRowsSelected = thisNumRowsSelected + 1;
                    while (!(curElement.tagName.toLowerCase() == "tr")) {
                        //curElement = curElement.parentElement;

                        //if (form.elements[i].name.indexOf('chkSelectDownload') > -1) {
                        //    //console.log('-- inside ' + chkElement.name + ' ' + chkElement);
                        //    console.log('-- inside2 ' + curElement.name);
                        //    //chkElement = $('[name="' + curElement.name + '"]');
                        //    chkElement = curElement.getElementsByTagName('input')[1];
                        //    console.log('-- ' + chkElement);
                        //} else {
                        curElement = curElement.parentNode;
                        chkElement = curElement.getElementsByTagName('input')[1];

                        //}




                    }
                    if ((form.elements[i].name != "chkItmSelectAll") && (curElement.style.backgroundColor != '#92ee8e'))
                        //&& (curElement.style.backgroundColor != '#ffff80')
                    {
                        curElement.style.backgroundColor = "#3DBC45";
                        btnElement = curElement.getElementsByTagName('input')[2];
                        btnElement.style.backgroundColor = "#3DBC45";
                        btnElement = curElement.getElementsByTagName('input')[3];
                        btnElement.style.backgroundColor = "#3DBC45";
                        btnElement = curElement.getElementsByTagName('input')[4];
                        btnElement.style.backgroundColor = "#3DBC45";
                        btnElement = curElement.getElementsByTagName('input')[5];
                        if (typeof (btnElement) != "undefined") {
                            btnElement.style.backgroundColor = "#3DBC45";
                        }
                    }
                }
                else {
                    while (!(curElement.tagName.toLowerCase() == "tr")) {
                        //curElement = curElement.parentElement;
                        curElement = curElement.parentNode;
                        chkElement = curElement.getElementsByTagName('input')[1];
                    }
                    if ((curElement.style.backgroundColor != '#92ee8e')) {
                        //Selected for Cart
                        if (chkElement.checked) {
                            curElement.style.backgroundColor = "#3DBC45";
                            btnElement = curElement.getElementsByTagName('input')[2];
                            btnElement.style.backgroundColor = "#3DBC45";
                            btnElement = curElement.getElementsByTagName('input')[3];
                            btnElement.style.backgroundColor = "#3DBC45";
                            btnElement = curElement.getElementsByTagName('input')[4];
                            btnElement.style.backgroundColor = "#3DBC45";
                            btnElement = curElement.getElementsByTagName('input')[5];
                            if (typeof (btnElement) != "undefined") {
                                btnElement.style.backgroundColor = "#3DBC45";
                            }
                            

                        }
                        else {
                            //Must be in Cart
                            if (chkElement.disabled == true) {
                                curElement.style.backgroundColor = "#92ee8e";
                                btnElement = curElement.getElementsByTagName('input')[2];
                                btnElement.style.backgroundColor = "#92ee8e";
                                btnElement = curElement.getElementsByTagName('input')[3];
                                btnElement.style.backgroundColor = "#92ee8e";
                                btnElement = curElement.getElementsByTagName('input')[4];
                                btnElement.style.backgroundColor = "#92ee8e";
                                btnElement = curElement.getElementsByTagName('input')[5];
                                if (typeof (btnElement) != "undefined") {
                                    btnElement.style.backgroundColor = "#92ee8e";
                                }
                                
                            }
                                //Nothing
                            else {


                                curElement.style.backgroundColor = "white";
                                btnElement = curElement.getElementsByTagName('input')[2];

                                try {
                                    btnElement.style.backgroundColor = "white";
                                    btnElement = curElement.getElementsByTagName('input')[4];
                                    btnElement.style.backgroundColor = "white";
                                    btnElement = curElement.getElementsByTagName('input')[3];
                                    btnElement.style.backgroundColor = "white";
                                    btnElement = curElement.getElementsByTagName('input')[5];
                                    btnElement.style.backgroundColor = "white";
                                }
                                catch (err) {
                                }





                            }
                        }
                    }
                }
            }
        }
        iCntItmRow = thisNumRowsSelected;
    }
</script>

<script lang="javascript">
    var iCntK1Row = 0;
    var iCntK2Row = 0;
    var iCntK3Row = 0;
    var iCntK4Row = 0;

    function SelectRowKey(srcElement) {
        //var cb = event.srcElement;
        var cb = srcElement;
        var curElement = cb;
        var btnElement;
        var chkElement;

        //alert('Start : ' + cb.id);
        //alert(curElement.id);
        while (curElement && !(curElement.tagName.toLowerCase() == "tr")) {
            //curElement = curElement.parentElement;
            curElement = curElement.parentNode;
        }
        if (!(curElement == cb) && (cb.name.indexOf('grdK') > -1)) {
            chkElement = curElement.getElementsByTagName('input')[0];
            btnElement = curElement.getElementsByTagName('input')[1];
            //re(chkElement.id);
            //alert(btnElement.id);
            if (chkElement.value == "1") {
                chkElement.value = "0";
                curElement.style.backgroundColor = "white";
                chkElement.style.backgroundColor = "white";
                btnElement.style.backgroundColor = "white";
                if (cb.name.indexOf('K1') > -1) iCntK1Row = iCntK1Row - 1;
                if (cb.name.indexOf('K2') > -1) iCntK2Row = iCntK2Row - 1;
                if (cb.name.indexOf('K3') > -1) iCntK3Row = iCntK3Row - 1;
                if (cb.name.indexOf('K4') > -1) iCntK4Row = iCntK4Row - 1;
                //alert(chkElement.value);
            }
            else {
                chkElement.value = "1";
                curElement.style.backgroundColor = "#FFFF80";
                chkElement.style.backgroundColor = "#FFFF80";
                btnElement.style.backgroundColor = "#FFFF80";
                if (cb.name.indexOf('K1') > -1) iCntK1Row = iCntK1Row + 1;
                if (cb.name.indexOf('K2') > -1) iCntK2Row = iCntK2Row + 1;
                if (cb.name.indexOf('K3') > -1) iCntK3Row = iCntK3Row + 1;
                if (cb.name.indexOf('K4') > -1) iCntK4Row = iCntK4Row + 1;

            }
        }
        //if ((cb.name.indexOf('K1') > -1) && (document.getElementById("divK1Grid") != null)) document.getElementById('txtTestK1').value = parseInt(iCntK1Row);
        //if ((cb.name.indexOf('K2') > -1) && (document.getElementById("divK2Grid") != null)) document.getElementById('txtTestK2').value = parseInt(iCntK2Row);
        //if ((cb.name.indexOf('K3') > -1) && (document.getElementById("divK3Grid") != null)) document.getElementById('txtTestK3').value = parseInt(iCntK3Row);
        //if ((cb.name.indexOf('K4') > -1) && (document.getElementById("divK4Grid") != null)) document.getElementById('txtTestK4').value = parseInt(iCntK4Row);
        if (cb.name.indexOf('K1') > -1) SetClearButton("K1", iCntK1Row);
        if (cb.name.indexOf('K2') > -1) SetClearButton("K2", iCntK2Row);
        if (cb.name.indexOf('K3') > -1) SetClearButton("K3", iCntK3Row);
        if (cb.name.indexOf('K4') > -1) SetClearButton("K4", iCntK4Row);
        //alert('Done Selecting');
    }
    function SelectAllKey(form, sGrid) {
        var bVal = 0;

        if (sGrid.indexOf('K1') > -1) { if (iCntK1Row > 0) { bVal = 0; iCntK1Row = 0; } else { bVal = 1; } }
        if (sGrid.indexOf('K2') > -1) { if (iCntK2Row > 0) { bVal = 0; iCntK2Row = 0; } else { bVal = 1; } }
        if (sGrid.indexOf('K3') > -1) { if (iCntK3Row > 0) { bVal = 0; iCntK3Row = 0; } else { bVal = 1; } }
        if (sGrid.indexOf('K4') > -1) { if (iCntK4Row > 0) { bVal = 0; iCntK4Row = 0; } else { bVal = 1; } }

        for (var i = 0; i < form.elements.length; i++) {
            if ((form.elements[i].name.indexOf('grd' + sGrid) > -1) && (form.elements[i].name.indexOf('ctl') > -1) && (form.elements[i].name.indexOf('btnKeyword') > -1)) {
                var curElement = form.elements[i];

                if (bVal == 1) {
                    while (!(curElement.tagName.toLowerCase() == "tr")) {
                        //curElement = curElement.parentElement;
                        curElement = curElement.parentNode;
                    }
                    curElement.style.backgroundColor = "#FFFF80";
                    chkElement = curElement.getElementsByTagName('input')[0];
                    btnElement = curElement.getElementsByTagName('input')[1];
                    chkElement.value = bVal;
                    btnElement.style.backgroundColor = "#FFFF80";
                    if (form.elements[i].name.indexOf('K1') > -1) iCntK1Row = iCntK1Row + 1;
                    if (form.elements[i].name.indexOf('K2') > -1) iCntK2Row = iCntK2Row + 1;
                    if (form.elements[i].name.indexOf('K3') > -1) iCntK3Row = iCntK3Row + 1;
                    if (form.elements[i].name.indexOf('K4') > -1) iCntK4Row = iCntK4Row + 1;

                }
                else {
                    while (!(curElement.tagName.toLowerCase() == "tr")) {
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
        //if (document.getElementById("divK2Grid") != null) document.getElementById('txtTestK2').value = parseInt(iCntK2Row);
        //if (document.getElementById("divK3Grid") != null) document.getElementById('txtTestK3').value = parseInt(iCntK3Row);
        //if (document.getElementById("divK4Grid") != null) document.getElementById('txtTestK4').value = parseInt(iCntK4Row);
        if (document.getElementById("divK1Grid") != null) SetClearButton("K1", iCntK1Row);
        if (document.getElementById("divK2Grid") != null) SetClearButton("K2", iCntK2Row);
        if (document.getElementById("divK3Grid") != null) SetClearButton("K3", iCntK3Row);
        if (document.getElementById("divK4Grid") != null) SetClearButton("K4", iCntK4Row);
    }
    //ColorSelectedKey(document.frmItmSel);
    ColorSelectedKey(document.getElementById('frmItmSel'));

    function ColorSelectedKey(form) {
        var btnElement;
        var e;

        iCntK1Row = 0;
        iCntK2Row = 0;
        iCntK3Row = 0;
        iCntK4Row = 0;

        for (var i = 0; i < form.elements.length; i++) {
            if ((form.elements[i].name.indexOf('grdK') > -1) && (form.elements[i].name.indexOf('ctl') > -1)) {
                var curElement = form.elements[i];

                if (curElement.value == '1') {
                    while (!(curElement.tagName.toLowerCase() == "tr")) {
                        //curElement = curElement.parentElement;
                        curElement = curElement.parentNode;
                    }
                    curElement.style.backgroundColor = "#FFFF80";
                    btnElement = curElement.getElementsByTagName('input')[1];
                    btnElement.style.backgroundColor = "#FFFF80";

                    if (form.elements[i].name.indexOf('K1') > -1) iCntK1Row = iCntK1Row + 1;
                    if (form.elements[i].name.indexOf('K2') > -1) iCntK2Row = iCntK2Row + 1;
                    if (form.elements[i].name.indexOf('K3') > -1) iCntK3Row = iCntK3Row + 1;
                    if (form.elements[i].name.indexOf('K4') > -1) iCntK4Row = iCntK4Row + 1;
                }
            }
        }
        //if (document.getElementById("divK1Grid") != null) document.getElementById('txtTestK1').value = parseInt(iCntK1Row);
        //if (document.getElementById("divK2Grid") != null) document.getElementById('txtTestK2').value = parseInt(iCntK2Row);
        //if (document.getElementById("divK3Grid") != null) document.getElementById('txtTestK3').value = parseInt(iCntK3Row);
        //if (document.getElementById("divK4Grid") != null) document.getElementById('txtTestK4').value = parseInt(iCntK4Row);
        if (document.getElementById("divK1Grid") != null) SetClearButton("K1", iCntK1Row);
        if (document.getElementById("divK2Grid") != null) SetClearButton("K2", iCntK2Row);
        if (document.getElementById("divK3Grid") != null) SetClearButton("K3", iCntK3Row);
        if (document.getElementById("divK4Grid") != null) SetClearButton("K4", iCntK4Row);
    }
    function SetClearButton(sGrid, iCnt) {
        if (document.getElementById('div' + sGrid + 'Grid') != null) {
            btnElement = document.getElementById('ibtn' + sGrid + 'Select');
            if (iCnt > 0) {
                btnElement.src = "images/btnDeselectAll.png"
                btnElement.title = "Deselect All"
            }
            else {
                btnElement.src = "images/btnSelectAll.png"
                btnElement.title = "Select All"
            }
        }
    }
    function CheckOrderQuantityLimit(OrderQuantityLimit, QuantityRequested, CheckBox, PartNo) {

        var OQL = 0;
        var QR = 0;
        var iCount;

        OQL = parseInt(OrderQuantityLimit);
        QR = parseInt(QuantityRequested);

        if (OQL < QR) {
            if (CheckBox.checked == true) {
                alert('Quantity requested exceeds limit for this item\n' + PartNo);
                CheckBox.checked = false;
                iCntItmRow += 1;
            }
        }
        return true;
    }
    function CheckOrderQuantities() {
        var gvDrv = document.getElementById("<%= grdItm.ClientID %>");

        for (i = 0; i < gvDrv.rows.length; i++) {
            var cell = gvDrv.rows[i].cells;
            var HTML = cell[0].innerHTML;

            if (HTML.indexOf("chkSelect") != -1 && (HTML.indexOf("CHECKED") != -1 || HTML.indexOf("checked") != -1)) {
                CheckOrderQuantityLimit(cell[4].outerText, document.getElementById("<%= txtQtyOrd.ClientID %>").value, cell[0].childNodes[2].childNodes[0], cell[2].childNodes[0].value + ' - ' + cell[3].childNodes[1].value);
            }
        }
    }
</script>

</form>
</table>