<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Exclusions.aspx.vb" Inherits="POSN.Exclusions" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<%@ Register assembly="System.Web.Extensions, Culture=neutral" namespace="System.Web.UI" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<head>
<link rel="stylesheet" type="text/css" href="ordSummary.css" />



    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</head>



<td valign="top">
	<div id="Div1" dir="rtl" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%" runat="server">
		<div dir="ltr">
			<form id="frmOrdShp" method="post" runat="server">
				<table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
					<tr>
						<td background="">
							<table width="100%" align="center">
								<tr>
									<td align="center" colSpan="1">&nbsp;</td>
									<td align="center" colSpan="2">
                                        <asp:label id="lblTitle" runat="server" 
                                            text="Access Control" CssClass="lblTitle"></asp:label></td>
									<td align="center" colSpan="1">&nbsp;</td>
								</tr>
								<tr>
									<td>
                                        <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
                                        </telerik:RadScriptManager>
                                    </td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td vAlign="top" background="" style="padding-left: 20px; padding-bottom: 20px;">
                            &nbsp;
                            <table cellpadding="2" cellspacing="0" id="bounding" style="width: 100%;">
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" class="style1">
                        <tr>
                            <td width="250">
                    <asp:Label ID="lblAccessCode0" runat="server" 
                        Text="Access Code" Font-Names="Arial" Font-Size="12pt"></asp:Label>
&nbsp;<asp:DropDownList ID="ddlAccessCode" runat="server" Width="130px" AutoPostBack="True">
                    </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="lstItemTypes" runat="server" 
                                    RepeatDirection="Horizontal" AutoPostBack="True">
                                    <asp:ListItem>Items</asp:ListItem>
                                    <asp:ListItem>Kits</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="33%">
																					&nbsp;&nbsp;</td>
                            <td width="33%" valign="Top">
                                &nbsp;</td>
                            <td align="Center" width="33%">
                                &nbsp;</td>
                        </tr>
                        </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td valign="top" width="33%">
                                <asp:Panel id="pnlItems" runat="server">
                                    <table cellpadding="0" cellspacing="0" 
    width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Documents4" runat="server" Font-Names="Arial" Font-Size="12pt" 
                                                    Text="Category/Tags"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                    <telerik:RadGrid ID="grdK1Rad" runat="server" AutoGenerateColumns="False" 
                                                        GridLines="None" Height="150px" Skin="Web20" Width="95%">
                                                        <MasterTableView>
                                                            <RowIndicatorColumn>
                                                                <HeaderStyle Width="20px" />
                                                            </RowIndicatorColumn>
                                                            <ExpandCollapseColumn>
                                                                <HeaderStyle Width="20px" />
                                                            </ExpandCollapseColumn>
                                                            <Columns>
                                                                <telerik:GridTemplateColumn UniqueName="TemplateColumn" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Labelk1" runat="server" 
                                                                            Text='<%# DataBinder.Eval(Container, "DataItem.Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Collateral Type" 
                                                                    UniqueName="TemplateColumn1">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="LinkButton10" runat="server" 
                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' 
                                                                            CommandName="SelectCatagory" 
                                                                            Text='<%# DataBinder.Eval(Container.DataItem, "Keyword") %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right" 
                                                                    UniqueName="TemplateColumn2">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEdit4" runat="server" 
                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' 
                                                                            CommandName="Add">Add &gt;&gt;&gt;</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn UniqueName="TemplateColumn3" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblColor" runat="server" Text="Label"></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                        <ClientSettings>
                                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                                        </ClientSettings>
                                                    </telerik:RadGrid>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                    <telerik:RadGrid ID="grdK2Rad" runat="server" AutoGenerateColumns="False" 
                                                        GridLines="None" Height="150px" Skin="Web20" Width="95%">
                                                        <MasterTableView>
                                                            <RowIndicatorColumn>
                                                                <HeaderStyle Width="20px" />
                                                            </RowIndicatorColumn>
                                                            <ExpandCollapseColumn>
                                                                <HeaderStyle Width="20px" />
                                                            </ExpandCollapseColumn>
                                                            <Columns>
                                                                <telerik:GridTemplateColumn UniqueName="TemplateColumn" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Labelk2" runat="server" 
                                                                            Text='<%# DataBinder.Eval(Container, "DataItem.Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Product Area" 
                                                                    UniqueName="TemplateColumn1">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="LinkButton9" runat="server" 
                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' 
                                                                            CommandName="SelectCatagory" 
                                                                            Text='<%# DataBinder.Eval(Container.DataItem, "Keyword") %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right" 
                                                                    UniqueName="TemplateColumn2">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEdit3" runat="server" 
                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' 
                                                                            CommandName="Add">Add &gt;&gt;&gt;</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn UniqueName="TemplateColumn3" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblColor" runat="server" Text="Label"></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>                                                                
                                                            </Columns>
                                                        </MasterTableView>
                                                        <ClientSettings>
                                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                                        </ClientSettings>
                                                    </telerik:RadGrid>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                            
                                                <telerik:RadGrid ID="grdK3Rad" runat="server" AutoGenerateColumns="False" 
                                                    GridLines="None" Height="150px" Skin="Web20" Width="95%">
                                                    <MasterTableView>
                                                        <RowIndicatorColumn>
                                                            <HeaderStyle Width="20px" />
                                                        </RowIndicatorColumn>
                                                        <ExpandCollapseColumn>
                                                            <HeaderStyle Width="20px" />
                                                        </ExpandCollapseColumn>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn UniqueName="TemplateColumn" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Labelk3" runat="server" 
                                                                        Text='<%# DataBinder.Eval(Container, "DataItem.Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Language" UniqueName="TemplateColumn1">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="LinkButton8" runat="server" 
                                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' 
                                                                        CommandName="SelectCatagory" 
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "Keyword") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right" 
                                                                UniqueName="TemplateColumn2">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkEdit2" runat="server" 
                                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' 
                                                                        CommandName="Add">Add &gt;&gt;&gt;</asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn UniqueName="TemplateColumn3" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblColor" runat="server" Text="Label"></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>                                                            
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings>
                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel id="pnlKits" runat="server">
                                    <table cellpadding="0" cellspacing="0" class="style1">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Documents5" runat="server" Font-Names="Arial" Font-Size="12pt" 
                                                    Text="Category/Tags"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadGrid ID="grdKitCategories" runat="server" 
                                                    AutoGenerateColumns="False" GridLines="None" Height="150px" Skin="Web20" 
                                                    Width="95%">
                                                    <MasterTableView>
                                                        <RowIndicatorColumn>
                                                            <HeaderStyle Width="20px" />
                                                        </RowIndicatorColumn>
                                                        <ExpandCollapseColumn>
                                                            <HeaderStyle Width="20px" />
                                                        </ExpandCollapseColumn>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn UniqueName="TemplateColumn" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Labelk6" runat="server" 
                                                                        Text='<%# DataBinder.Eval(Container, "DataItem.Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Collateral Type" 
                                                                UniqueName="TemplateColumn1">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="LinkButton11" runat="server" 
                                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' 
                                                                        CommandName="SelectCatagory" 
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "Keyword") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right" 
                                                                UniqueName="TemplateColumn2">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkEdit5" runat="server" 
                                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' 
                                                                        CommandName="Add">Add &gt;&gt;&gt;</asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="TemplateColumn3" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblColor0" runat="server" Text="Label"></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings>
                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>                                            
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                            <td valign="top" width="33%">
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                <asp:Label ID="lblDocuments" runat="server" Text="Items" Font-Names="Arial" 
                                                Font-Size="12pt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <div style="overflow:auto; height:488px; width:95%;">
																					<telerik:RadGrid ID="grdItmRad" runat="server" AutoGenerateColumns="False" 
                                                                                        GridLines="None" Height="704px" Skin="Web20" Width="100%">
<MasterTableView>
<RowIndicatorColumn>
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn>
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>
    <Columns>
        <telerik:GridTemplateColumn UniqueName="TemplateColumn" Visible="False">
            <ItemTemplate>
                <asp:Label ID="lblId" runat="server" 
                    Text='<%# DataBinder.Eval(Container, "DataItem.Id") %>'></asp:Label>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn UniqueName="TemplateColumn1" HeaderText="Item">
            <ItemTemplate>
                <asp:Label ID="lblRefNo" runat="server" 
                    Text='<%# DataBinder.Eval(Container, "DataItem.RefNo") %>'></asp:Label>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn UniqueName="TemplateColumn2" 
            HeaderText="Description">
            <ItemTemplate>
                <asp:Label ID="lblDescription" runat="server" 
                    Text='<%# DataBinder.Eval(Container, "DataItem.RefName") %>'></asp:Label>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn UniqueName="TemplateColumn3">
            <ItemTemplate>
                <asp:LinkButton ID="lnkEdit" runat="server" 
                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' 
                    CommandName="Add">Add &gt;&gt;&gt;</asp:LinkButton>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
        </telerik:GridTemplateColumn>
    </Columns>
</MasterTableView>
                                                                                    </telerik:RadGrid>
                                        </td>
                                    </tr>
                                </table>
                              </td>
                            <td valign="top" width="33%">
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                <asp:Label ID="Documents0" runat="server" 
                                    Text="Access Code Exclusions" Font-Names="Arial" Font-Size="12pt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <div style="overflow:auto; height:488px; width:95%;">
                                            <telerik:RadGrid ID="grdExclusionsRad" runat="server" 
                                                AutoGenerateColumns="False" GridLines="None" Height="704px" Skin="Web20" 
                                                Width="100%">
<MasterTableView>
<RowIndicatorColumn>
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn>
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>
    <Columns>
        <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Type">
        
            <ItemTemplate>
                <asp:Label ID="lblType" runat="server" 
                    Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>'></asp:Label>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn UniqueName="TemplateColumn1" 
            HeaderText="Description">
            <ItemTemplate>
                <asp:Label ID="lblDescription" runat="server" 
                    Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>'></asp:Label>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn UniqueName="TemplateColumn2">
            <ItemTemplate>
                <asp:LinkButton ID="lnkRemove" runat="server" 
                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' 
                    CommandName="Remove" Text="&lt;&lt;&lt; Remove"></asp:LinkButton>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn UniqueName="TemplateColumn3" Visible="False">
            <ItemTemplate>
                <asp:Label ID="lblAccessCode" runat="server" 
                    Text='<%# DataBinder.Eval(Container, "DataItem.AccessCodeId") %>'></asp:Label>
            </ItemTemplate>
            
        </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn UniqueName="TemplateColumn3" Visible="True" HeaderText="Access Code">
            <ItemTemplate>
              <%# GetAccessCode(DataBinder.Eval(Container, "DataItem.AccessCodeId")) %>
            </ItemTemplate>
            
        </telerik:GridTemplateColumn>
    </Columns>
</MasterTableView>
                                            </telerik:RadGrid>
                                        </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            </table>                              
                            
						</td>
					</tr>
					<tr bgColor="black">
						<td>
                            &nbsp;
						</td>
					</tr>
				</table>
			</form>
		</div>
	</div>
</td>