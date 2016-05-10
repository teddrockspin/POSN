<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OrdShp.aspx.vb" Inherits="POSN.OrdShp"
    SmartNavigation="True" %>
<%@ Register Assembly="PopupCalendar" Namespace="PopupCalendar" TagPrefix="cc1" %>
<td valign="top">
    <div dir="rtl" style="width: 100%; height: 100%" runat="server">
        <div dir="ltr">
            <form id="frmOrdShp" method="post" runat="server">          
            <table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td background="">
                        <table width="100%" align="center">
                            <tr>
                                <td align="center" colspan="1">
                                    &nbsp;
                                </td>
                                <td align="center" colspan="2">
                                    <asp:label id="lblTitle" runat="server" text="Shipping Information" cssclass="lblTitle"></asp:label>
                                </td>
                                <td align="center" colspan="1">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" background="">
                        <table width="100%" align="center">
                            <tr>
                                <td align="center" colspan="1">
                                    &nbsp;
                                </td>
                                <td align="center" colspan="2">
                                    <asp:label id="lblAuth" runat="server" text="" cssclass="lblXLongBack"></asp:label>
                                </td>
                                <td align="center" colspan="1">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="10">
                                    <asp:table id="tblCst" runat="server" width="100%" bgcolor="Transparent" horizontalalign="center">
											<asp:tablerow>
												<asp:tablecell colSpan="4">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" ColSpan="2">
													<asp:imagebutton id="cmdClear" ToolTip="Clear All" ImageURL="images/btnClear.png" Runat="server"></asp:imagebutton>&nbsp;
													<asp:imagebutton id="cmdSearch" ToolTip="Check Previous Ship To Locations (By Company and Last Name)"
														ImageURL="images/btnSearch.png" Runat="server" visible="false"></asp:imagebutton>																										
												</asp:tablecell>
												<asp:tablecell colSpan="1">&nbsp;</asp:tablecell>
											</asp:tablerow>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="lblFName" runat="server" CssClass="lblMedium" text="Given/First Name:"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1">
													<asp:textbox id="txtFName" runat="server" CssClass="txtMediumLong" MaxLength="20"></asp:textbox>
													<asp:label id="lblRFName" runat="server" text=" * " CssClass="lblRequired"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
											</asp:tablerow>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="lblLName" runat="server" CssClass="lblMedium" text="Family/Last Name:"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1">
													<asp:textbox id="txtLName" runat="server" CssClass="txtMediumLong" MaxLength="20"></asp:textbox>
													<asp:label id="lblRLName" runat="server" text=" * " CssClass="lblRequired"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
											</asp:tablerow>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="lblCompany" runat="server" CssClass="lblMedium" text="Company:"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1">
													<asp:textbox id="txtCompany" runat="server" CssClass="txtMediumLong" MaxLength="100"></asp:textbox>
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
											</asp:tablerow>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="lblAddress1" runat="server" CssClass="lblMedium" text="Address:"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1">
													<asp:textbox id="txtAddress1" runat="server" CssClass="txtMediumLong" MaxLength="50"></asp:textbox>
													<asp:label id="lblRAddress1" runat="server" text=" * " CssClass="lblRequired"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
											</asp:tablerow>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="lblAddress2" runat="server" CssClass="lblMedium" text=""></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1">
													<asp:textbox id="txtAddress2" runat="server" CssClass="txtMediumLong" MaxLength="50"></asp:textbox>
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
											</asp:tablerow>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="lblCity" runat="server" CssClass="lblMedium" text="City:"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1">
													<asp:textbox id="txtCity" runat="server" CssClass="txtMediumLong" MaxLength="50"></asp:textbox>
													<asp:label id="lblRCity" runat="server" text=" * " CssClass="lblRequired"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
											</asp:tablerow>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="lblState" runat="server" CssClass="lblMedium" text="State/Province:"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1">
													<asp:textbox id="txtState" runat="server" CssClass="txtMediumLong" MaxLength="30"></asp:textbox>
													<asp:label id="lblRState" runat="server" text=" * " CssClass="lblRequired"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
											</asp:tablerow>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="lblZip" runat="server" CssClass="lblMedium" text="Postal Code:"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1">
													<asp:textbox id="txtZip" runat="server" CssClass="txtMediumLong" MaxLength="20"></asp:textbox>
													<asp:label id="lblRZip" runat="server" text=" * " CssClass="lblRequired"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
											</asp:tablerow>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="lblCountry" runat="server" CssClass="lblMedium" text="Country:"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1"><asp:DropDownList ID="txtCountryDDL" runat="server" AutoPostBack="true"  AppendDataBoundItems="true">
                                                                        <asp:ListItem Enabled=true></asp:ListItem>                                                            
                                                          </asp:DropDownList>
													<asp:label id="lblRCountry" runat="server" text=" * " CssClass="lblRequired"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
											</asp:tablerow>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1"></asp:tablecell>
												<asp:tablecell align="left" colSpan="3">
													<asp:label id="lblCountryMsg" runat="server" CssClass="lblError" text=""></asp:label>
												</asp:tablecell>
											</asp:tablerow>
                                        <%-- <!-- new fields: -->--%>
                                       <asp:tablerow >
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="Label1" runat="server" CssClass="lblMedium" text="Recipient Phone Number:"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1">
													<asp:textbox id="txtPhoneNumber" runat="server" CssClass="txtMediumLong" MaxLength="20"></asp:textbox>
													<asp:label id="LabelPhone" runat="server" text=" * " CssClass="lblRequired"></asp:label><asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorPhone" ControlToValidate="txtPhoneNumber" ErrorMessage=""  Text=""  Display="None" validationgroup="checkout"></asp:RequiredFieldValidator>
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
											</asp:tablerow>
                                        <asp:tablerow>
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="Label3" runat="server" CssClass="lblMedium" text="Recipient Email:"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1">
													<asp:textbox id="txtEmail" runat="server" CssClass="txtMediumLong" ></asp:textbox>
													<asp:label id="LabelEmail" runat="server" text=" * " CssClass="lblRequired" visible="false"></asp:label><asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorEmail" ControlToValidate="txtEmail" ErrorMessage=""  Text=""  Display="None" validationgroup="checkout" enabled="false"></asp:RequiredFieldValidator>
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
											</asp:tablerow>
                                        <asp:tablerow>
                                            		<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1">
                                                     <Asp:checkbox runat="server" id="chkSendConfirmEmail" text="Send confirmation and shipping email to recipient"></Asp:checkbox><br /><br />
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
                                        </asp:tablerow>
                                       <%--  <!--/end new fields-->--%>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="lblShip" runat="server" CssClass="lblMedium" text="Preferred Shipping:"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1">
													<asp:dropdownlist id="cboShip" runat="server" CssClass="cboMediumlong" AutoPostBack="True"></asp:dropdownlist>
													<asp:label id="lblRShip" runat="server" text=" * " CssClass="lblRequired"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
											</asp:tablerow>
											
											
											<asp:tablerow id="trScheduledDelivery">
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="lblScheduledDelivery" runat="server" CssClass="lblMedium" text="Scheduled Delivery:"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1">
													<cc1:PopupCalendar ID="txtScheduledDate" runat="server"> </cc1:PopupCalendar>
													<asp:label id="lblRSchedule" runat="server" text=" * " CssClass="lblRequired"></asp:label>													
													<br /><input type=checkbox id="chkImmediate" /><asp:label id="lblImmediate" runat="server" CssClass="lblLong" text="Immediate"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
											</asp:tablerow>
											
											
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1"></asp:tablecell>
												<asp:tablecell align="left" colSpan="3">
													<asp:label id="lblShipTypeNote" runat="server" CssClass="lblLong" text=""></asp:label>
												</asp:tablecell>
											</asp:tablerow>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="1">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
												<asp:tablecell align="right" colSpan="1">
													<asp:label id="lblNote" runat="server" CssClass="lblMedium" text="Special Shipping Instructions:"></asp:label>
												</asp:tablecell>
												<asp:tablecell align="left" colSpan="1">
													<asp:textbox id="txtNote" runat="server" CssClass="txtXLongM" TextMode="MultiLine" MaxLength="200"
														style="OVERFLOW: hidden;" onKeyUp="Count(this,200)" onChange="Count(this,200)"></asp:textbox>
												</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;</asp:tablecell>
											</asp:tablerow>
											<asp:tablerow>
												<asp:tablecell align="center" colSpan="5">&nbsp;</asp:tablecell>
												<asp:tablecell align="center" colSpan="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:tablecell>
											</asp:tablerow>
										</asp:table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr >
                    <td>
                        <table width="100%" align="center" class=footercommon>
                            <tr>
                                <tr>
                                    <td width="350">
                                        <input id="txtSubmit" name="txtSubmit" runat="server" type="hidden">
                                        <input id="txtItmRow" name="txtItmRow" runat="server" type="hidden" value="0">
                                    </td>
                                    <td width="75">
                                        <asp:button id="cmdBack" runat="server" cssclass="cmdMedium" tooltip="Disregard Changes and Return to Previously Viewed Screen"
                                            text="Back" causesvalidation="False" visible="False"></asp:button>
                                    </td>
                                    <td width="75">
                                        <asp:imagebutton id="cmdNext" runat="server" tooltip="Save Information and Proceed with Order"
                                            imageurl="images/btnContChk.png"  validationgroup="checkout"></asp:imagebutton>
                                        <asp:imagebutton id="cmdEditBack" runat="server" visible="False" imageurl="images/btnSave.png"
                                            tooltip="Back"></asp:imagebutton>
                                    </td>
                                    <td width="75">
                                    </td>
                                </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </form>
        </div>
    </div>
</td>
<link type="text/css" href="<%=resolveURL("~/css/demos.css") %>" rel="stylesheet" />

<script type="text/javascript" src="http://code.jquery.com/jquery-1.4.1.min.js"></script>

<script type="text/javascript" src="js/jquery-ui-1.7.2.custom.min.js"></script>

<link type="text/css" href="<%=resolveURL("~/themes/base/ui.all.css")%>" rel="stylesheet" />

<script language="javascript">
    function Count(text, long) {
        var maxlength = new Number(long); // Change number to your max length.
        if (text.value.length > maxlength) {
            text.value = text.value.substring(0, maxlength);
            alert(" Special Shipping Instructions may only be " + long + " characters.");
        }
    }
</script>
<%  If blnShowpScheduledDelivery Then%>
<script type="text/javascript">
    $(document).ready(function() {
        //$("#<%=txtScheduledDate.clientID %>").datepicker();

        $("#chkImmediate").click(function() {
            if ($("#chkImmediate").attr("checked")) {
                //disable textbox and calendar
                var myDate = new Date();
                var prettyDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' +
        myDate.getFullYear();
                $("#txtScheduledDate").val(prettyDate);
                $("#txtScheduledDate").attr("readonly", "true");
                $('img[title~="calendar"]').hide();
            }
            else {
                //enable all
                $("#txtScheduledDate").val('');
                $("#txtScheduledDate").attr("readonly", "");
                $('img[title~="calendar"]').show();

            }
        });
    });
</script>
<%End If%>
