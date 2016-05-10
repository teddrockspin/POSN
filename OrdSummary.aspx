<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OrdSummary.aspx.vb" Inherits="POSN.OrdSummary" %>

<%@ Register Assembly="PopupCalendar" Namespace="PopupCalendar" TagPrefix="cc1" %>
<link rel="stylesheet" type="text/css" href="ordSummary.css" />
<td valign="top" colspan="4">
    <div dir="rtl" style='width: 100%; height: 100%' runat='server' id="Div1">
        <form id="Form1" runat="server">


            <asp:hiddenfield runat="server" id="hfCustomField"></asp:hiddenfield>
            <div dir="ltr">
                <table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
                    <tbody>
                        <tr>
                            <td valign="top" background="">
            </div>
            <div id="divGrid" runat="server">
                <table width="100%" align="center">
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tbody>
                                    <tr>
                                        <th colspan="3">Order Summary<br />
                                            <asp:label runat="server" id="lblTest" visible="false"></asp:label>
                                            <asp:hiddenfield runat="server" id="hfSort" value="1"></asp:hiddenfield>
                                            <Asp:hiddenfield runat="server" id="hfSortField" value=""></Asp:hiddenfield>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <b>Order Date Range: </b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdStyle" width="100">From:
                                        </td>
                                        <td>
                                            <cc1:PopupCalendar ID="txtDate1" runat="server"> </cc1:PopupCalendar>
                                            <asp:regularexpressionvalidator runat="server" errormessage="enter valid date" controltovalidate="txtDate1"
                                                validationexpression="^[0,1]?\d{1}\/(([0-2]?\d{1})|([3][0,1]{1}))\/(([1]{1}[9]{1}[9]{1}\d{1})|([2-9]{1}\d{3}))$"
                                                validationgroup="Refresh" display="dynamic"></asp:regularexpressionvalidator>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdStyle">To:
                                        </td>
                                        <td>
                                            <cc1:PopupCalendar ID="txtDate2" runat="server"> </cc1:PopupCalendar>
                                            <asp:regularexpressionvalidator runat="server" errormessage="enter valid date" controltovalidate="txtDate2"
                                                validationexpression="^[0,1]?\d{1}\/(([0-2]?\d{1})|([3][0,1]{1}))\/(([1]{1}[9]{1}[9]{1}\d{1})|([2-9]{1}\d{3}))$"
                                                validationgroup="Refresh" display="dynamic"></asp:regularexpressionvalidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdStyle">Access code
                                        </td>
                                        <td>
                                            <asp:dropdownlist id="ddlAccessCode" runat="server" class="entryfield">
                                                    </asp:dropdownlist>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdStyle">Status
                                        </td>
                                        <td>
                                            <asp:dropdownlist id="ddlStatus" runat="server" class="entryfield">
                                                    </asp:dropdownlist>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdStyle">
                                            <asp:imagebutton runat="server" id="btnGo" imageurl="images/btnRefresh.png" validationgroup="Refresh"></asp:imagebutton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="alert">
                                                <div class="ui-widget">
                                                    <div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
                                                        <p>
                                                            <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
                                                            <strong>Clicking on "Details" will erase your current shopping cart contents</strong>
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:datagrid id="grd" runat="server" cellpadding="3" borderwidth="1px" bordercolor="Black"
                                font-size="Smaller" borderstyle="None" allowpaging="True" width="100%" autogeneratecolumns="false"
                                pagesize="50" allowsorting="True">
            <SelectedItemStyle Font-Italic="true" CssClass="grdSelect"></SelectedItemStyle>
            <PagerStyle Mode="NumericPages" CssClass="PagerView" />
            <ItemStyle CssClass="grdBody" Height="30" />
            <AlternatingItemStyle BackColor="#E8E8E8" />
            <HeaderStyle Font-Italic="true" Font-Bold="true" CssClass="grdHead"></HeaderStyle>
            <Columns>
                <asp:TemplateColumn Visible="false">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hfID" Value='<%#DataBinder.Eval(Container.DataItem , "ID")%>'>
                        </asp:HiddenField>
                    </ItemTemplate>
                    <HeaderTemplate>
                    </HeaderTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Review">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="lnkReview" Text="Details" CommandName="EditItem"
                            CommandArgument='<%#DataBinder.Eval(Container.DataItem , "ID")%>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="ID" HeaderText="Order Number" SortExpression="ID"></asp:BoundColumn>
                <asp:BoundColumn DataField="StatusDescription" HeaderText="Status" SortExpression="StatusDescription">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="RequestDate" HeaderText="Order Date" DataFormatString="{0:d}"
                    SortExpression="RequestDate"></asp:BoundColumn>
                <asp:BoundColumn DataField="ShipDate" HeaderText="Ship Date" DataFormatString="{0:d}"
                    SortExpression="ShipDate"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Ship Method" SortExpression="ShippingMethod">
                    <ItemTemplate>
                        <%--<%# getShipMethodName(DataBinder.Eval(Container.DataItem , "ActualShipID"))%>--%>
                        <%#DataBinder.Eval(Container.DataItem, "ShippingMethod")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="TrackNo" HeaderText="Tracking Number" SortExpression="TrackNo">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Code" HeaderText="Acc Code" SortExpression="Code"></asp:BoundColumn>
                <asp:BoundColumn DataField="RequestorName" HeaderText="Requestor Name" SortExpression="RequestorName">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="ShipToContactName" HeaderText="Ship Name" SortExpression="ShipToContactName">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="ShipTo_Name" HeaderText="Ship Company Name" SortExpression="ShipTo_Name">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="ShipTo_Address1" HeaderText="Ship Address 1" SortExpression="ShipTo_Address1">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="ShipTo_Address2" HeaderText="Ship Address 2" SortExpression="ShipTo_Address2">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="ShipTo_City" HeaderText="Ship City" SortExpression="ShipTo_City">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="ShipTo_State" HeaderText="Ship State" SortExpression="ShipTo_State">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="ShipTo_ZipCode" HeaderText="Ship Postal Code" SortExpression="ShipTo_ZipCode">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="ShipTo_Country" HeaderText="Ship Country" SortExpression="ShipTo_Country">
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Pieces">
                    <ItemTemplate>                     
                        <%#GetPieces(DataBinder.Eval(Container.DataItem, "ID"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                   <asp:TemplateColumn HeaderText="Kits">
                    <ItemTemplate>                     
                        <%#GetKitQty(DataBinder.Eval(Container.DataItem, "ID"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Order Cost" SortExpression="Cost">
                    <ItemTemplate>
                        <span id="Span1" runat="server" visible='<%#CheckVisibleDetails(DataBinder.Eval(Container.DataItem , "statusID"))%>'>
                            <%#String.Format("{0:C}", DataBinder.Eval(Container.DataItem, "Cost"))%></span>
                    </ItemTemplate>
                </asp:TemplateColumn>
         
            </Columns>
        </asp:datagrid>
                        </td>
                    </tr>
                    <tr>
                        <td align="center"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:textbox runat="server" id="txtSelectedOrder" visible="false"></asp:textbox>
                        </td>
                    </tr>
                </table>
            </div>
</td>
</tr>
                </tbody>
            </table>
        </form>
    </div>
    </div>
</td>
<link type="text/css" href="css/demos.css" rel="stylesheet" />

<script type="text/javascript" src="http://code.jquery.com/jquery-1.4.1.min.js"></script>

<script type="text/javascript" src="js/jquery-ui-1.7.2.custom.min.js"></script>

<link type="text/css" href="themes/base/ui.all.css" rel="stylesheet" />
