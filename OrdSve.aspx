<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OrdSve.aspx.vb" Inherits="POSN.OrdSve"
    SmartNavigation="True" %>

<script type="text/javascript">
    function confirm_cancel() {
        if (confirm("Are you sure you want to cancel this order?") == true)
            return true;
        else
            return false;
    }
    function confirm_resubmit() {
        if (confirm("Are you sure you want to resubmit this order?") == true)
            return true;
        else
            return false;
    }
</script>

<td valign="top">
    <div dir="rtl" style="width: 100%; height: 100%" runat="server">
        <div dir="ltr">
            <form id="frmOrdSve" method="post" runat="server">
                <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                    <tr>
                        <td valign="top" background="">
                            <table width="100%" align="center">
                                <tbody>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="1">&nbsp;
                                        </td>
                                        <td align="center" colspan="2">
                                            <asp:label id="lblTitle" runat="server" cssclass="lblTitle" text="Confirmation"></asp:label>
                                        </td>
                                        <td align="center" colspan="1">&nbsp;
                                        </td>
                                    </tr>
                                    <tr runat="server" visible="true" id="divOrderMsg">
                                        <td align="center" colspan="1">&nbsp;
                                        </td>
                                        <td align="center" colspan="2">
                                            <div class="ui-widget" style="width:600px;">
                                                <div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
                                                    <p>
                                                        <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
                                                        <strong>
                                                            <asp:label id="lblOrderMessage" runat="server" text="text!"></asp:label>
                                                        </strong>
                                                    </p>
                                                </div>
                                            </div>

                                        </td>
                                        <td align="center" colspan="1">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="1">&nbsp;
                                        </td>
                                        <td align="center" colspan="2">
                                            <asp:label id="lblStatus" runat="server" cssclass="lblXLong" text="" visible="False"></asp:label>
                                        </td>
                                        <td align="center" colspan="1">&nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" background="">
                            <asp:table id="tblOrd" runat="server" bgcolor="Transparent" cellspacing="0" cellpadding="0"
                                align="center" width="100%">
								<asp:tablerow>
									<asp:tablecell>
										<table width="100%" align="center" cellpadding="0" cellspacing="0" bgColor="transparent">
											<tr>
												<td align="center" colSpan="6" valign="top">
                                                    <div class="ui-corner-all roundbox " >
													<table width="80%" align="center" border="0" cellpadding="0" cellspacing="0" bgColor="transparent">
														<tr>
															<td background="">
																<asp:table id="tblReqTtl" runat="server" HorizontalAlign="center"></asp:table>
															</td>
														</tr>
														<tr>
															<td background="">
																<asp:table id="tblRequest" runat="server" align="center" width="100%">
																	<asp:tablerow>
																		<asp:tablecell>
																			<asp:table id="tblReq" runat="server" bgColor="Transparent" HorizontalAlign="center"></asp:table>
																			<asp:table id="tblReqEdt" runat="server" bgColor="Transparent" HorizontalAlign="right">
																				<asp:tablerow>
																					<asp:tablecell width="80%">
																						<asp:imagebutton id="cmdEditReq" runat="server" ToolTip="Edit Requestor Information" ImageUrl="images/btnEdit.png"></asp:imagebutton>
																					</asp:tablecell>
																				</asp:tablerow>
																			</asp:table>
																		</asp:tablecell>
																	</asp:tablerow>
																</asp:table>
															</td>
														</tr>
													</table>
                                                        </div> 
												</td>
											</tr>
											<tr>
												<td>&nbsp;</td>
											</tr>
											<tr>
												<td align="center" colSpan="6" valign="top">
                                                    <div class="ui-corner-all roundbox" id="divShipBox" runat="server" >
													<table width="80%" align="center" border="0" cellpadding="0" cellspacing="0">
														<tr>
															<td background="">
																<asp:table id="tblShpTtl" runat="server" HorizontalAlign="center"></asp:table>
															</td>
														</tr>
														<tr>
															<td background="">
																<asp:table id="tblShip" runat="server" align="center" width="100%">
																	<asp:tablerow>
																		<asp:tablecell>
																			<asp:table id="tblShp" runat="server" bgColor="Transparent" HorizontalAlign="center"></asp:table>
																			<asp:table id="tblShpEdt" runat="server" bgColor="Transparent" HorizontalAlign="right">
																				<asp:tablerow>
																					<asp:tablecell width="80%">
																						<asp:imagebutton id="cmdEditShp" runat="server" ToolTip="Edit Shipping Information" ImageUrl="images/btnEdit.png"></asp:imagebutton>
																					</asp:tablecell>
																				</asp:tablerow>
																			</asp:table>
																		</asp:tablecell>
																	</asp:tablerow>
																	
																</asp:table>
															</td>
														</tr>
													</table>
                                                        </div> 
												</td>
											</tr>
											<tr>
												<td>&nbsp;</td>
											</tr>
											<tr>
												<td align="center" colSpan="6" valign="top">
                                                    <div class="ui-corner-all roundbox"  runat="server" id="divCustomBox">
													<table width="80%" align="center" border="0" cellpadding="0" cellspacing="0">
														<tr>
															<td background="">
																<asp:table id="tblCSTtl" runat="server" HorizontalAlign="center"></asp:table>
															</td>
														</tr>
														<tr>
															<td background="">
																<asp:table id="tblCustom" runat="server" align="center" width="100%">
																	<asp:tablerow>
																		<asp:tablecell>
																			<asp:table id="tblCS" runat="server" bgColor="Transparent" HorizontalAlign="center"></asp:table>
																			<asp:table id="tblCSEdt" runat="server" bgColor="Transparent" HorizontalAlign="right">
																				<asp:tablerow>
																					<asp:tablecell width="80%">
																						<asp:imagebutton id="cmdEditCS" runat="server" ToolTip="Edit Cover Sheet Information" ImageUrl="images/btnEdit.png"></asp:imagebutton>
																					</asp:tablecell>
																				</asp:tablerow>
																			</asp:table>
																		</asp:tablecell>
																	</asp:tablerow>
															
																</asp:table>
															</td>
														</tr>
													</table>
                                                        </div> 
												</td>
											</tr>
											<tr>
												<td>&nbsp;</td>
											</tr>
											<tr>
												<td align="center" colSpan="6" valign="top">
                                                    <div class="ui-corner-all roundbox " >
													<table width="80%" align="center" border="0" cellpadding="0" cellspacing="0">
														<tr>
															<td background="">
																<asp:table id="tblItmTtl" runat="server" HorizontalAlign="center"></asp:table>
															</td>
														</tr>
														<tr>
															<td background="">
																<asp:table id="tblDetail" runat="server" align="center" width="100%">
																<asp:tablerow>
																	  <asp:tablecell  align="center"> 
												                            <div class="alert" style="width: 600px"  runat="server" id="dUploadMessage" visible=false>
                                                                                <div>
                                                                                    <div class="ui-state-error ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
                                                                                        <p style="font-size: 13px !important;">
                                                                                            <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
                                                                                           <strong>Please note: some items in your shopping cart require document upload.<br />
                                                                    Please go back to the shopping cart to upload document(s). </strong>
                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                        </div></asp:tablecell>
																	</asp:tablerow>
																	<asp:tablerow>
																		<asp:tablecell>
																			<asp:table id="tblItm" runat="server" bgColor="Transparent" HorizontalAlign="center"  cellpadding="5"></asp:table>
																			<asp:table id="tblItmEdt" runat="server" bgColor="Transparent" HorizontalAlign="right">
																				<asp:tablerow>
																					<asp:tablecell width="80%">
																						<asp:imagebutton id="cmdEditItm" runat="server" ToolTip="Edit Item Details" ImageUrl="images/btnEdit.png"></asp:imagebutton>
																					</asp:tablecell>
																				</asp:tablerow>
																			</asp:table>
																		</asp:tablecell>
																	</asp:tablerow>
																	
																
																	<asp:tablerow>
																	  <asp:tablecell id="tcWarningEdit" visible="false" align=center> 
												                            <div class="alert" style="width:600px">
                                                                                <div class="ui-widget">
                                                                                    <div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
                                                                                        <p>
                                                                                            <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
                                                                                            <strong>Please note: warning messages are for the original order's Access Code.</strong></p>
                                                                                    </div>
                                                                                </div>
                                                                            </div></asp:tablecell>
																	</asp:tablerow>
																		
																</asp:table>
															</td>
														</tr>
													</table>
                                                        </div> 
												</td>
											</tr>
											
										</table>
									</asp:tablecell>
								</asp:tablerow>
							</asp:table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" background="">
                            <table cellspacing="0" cellpadding="0" width="100%" align="center" bgcolor="transparent">
                                <tr>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:label id="lblPMsg" runat="server" cssclass="lblNoteInactive" text="" visible="False"></asp:label>
                                    </td>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:label id="lblMsg" runat="server" cssclass="lblXLong" text="" visible="False"></asp:label>
                                    </td>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:label id="lblConfirm" runat="server" cssclass="lblXLongConf" text="" visible="False"></asp:label>
                                    </td>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:label id="lblMsgCont" runat="server" cssclass="lblXLong" text="" visible="False"></asp:label>
                                    </td>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:label id="lblCustomerName" runat="server" cssclass="lblXLong" text="" visible="False"></asp:label>
                                    </td>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:label id="lblCustomerPhone" runat="server" cssclass="lblXLong" text="" visible="False"></asp:label>
                                    </td>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:label id="lblCustomerEmail" runat="server" cssclass="lblXLong" text="" visible="False"></asp:label>
                                    </td>
                                    <td align="center" colspan="1">&nbsp;
                                    </td>
                                </tr>
                                <tr><td><br /></td></tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <TABLE width="100%" align="center"  class="footercommon" id="tblButtons">
                                <tbody>
                                    <tr>
                                        <td width="75">&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:imagebutton id="cmdPrint" runat="server" imageurl="images/btnPrint.png" tooltip="View Print Friendly Page"></asp:imagebutton>
                                        </td>
                                        <td width="75">&nbsp;
                                        </td>
                                        <td align="right">

                                            <asp:imagebutton id="cmdBackEdit" runat="server" visible="False" imageurl="images/btnresubmit.png"
                                                tooltip="Re-Submit"></asp:imagebutton>
                                            <asp:imagebutton id="cmdCancelOrder" runat="server" visible="False" imageurl="images/btnCancelOrder.png"
                                                tooltip="Cancel Order"></asp:imagebutton>
                                            <asp:imagebutton id="cmdBackCancel" runat="server" visible="False" imageurl="images/btnBackCancel.png"
                                                tooltip="Back"></asp:imagebutton>
                                            </asp:LinkButton><asp:imagebutton id="cmdBack" runat="server" visible="False" imageurl="images/btnBack.png"
                                                tooltip="Cancel"></asp:imagebutton>
                                            <asp:imagebutton id="cmdSubmit" runat="server" visible="False" imageurl="images/btnSubmit.png"
                                                tooltip="Submit Order" style="height: 18px"></asp:imagebutton>
                                            <asp:imagebutton id="cmdNew" runat="server" visible="False" imageurl="images/btnNew.png"
                                                tooltip="Start New Order" causesvalidation="False"></asp:imagebutton>
                                        </td>
                                        <td width="75"></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr id="trPrintBack">
                        <td>
                            <asp:button id="cmdPrintBack" runat="server" cssclass="cmdMedium" causesvalidation="False"
                                tooltip="Return to Previously Viewed Screen" text="Back"></asp:button>
                        </td>
                    </tr>
                </table>
            </form>
        </div>
    </div>
</td>
<link type="text/css" href="css/demos.css" rel="stylesheet" />

<script type="text/javascript" src="http://code.jquery.com/jquery-1.4.1.min.js"></script>

<script type="text/javascript" src="js/jquery-ui-1.7.2.custom.min.js"></script>

<link type="text/css" href="themes/base/ui.all.css" rel="stylesheet" />
<%  If ViewState("print") = "1" Then%>
<style type="text/css">
    .lbltitle {
        color: Black !important;
    }
</style>

<script type="text/javascript">
    $(document).ready(function () {
        $("#tblButtons").hide();
        $("#trPrintBack").show();
        $('.lbltitle').css('color', '#000000 !important');
        $('table').css('background-color', '#FFFFFF');
        $('td').removeAttr('background');
        $("input[type=image]").hide();
    });
</script>

<%  Else%>

<script type="text/javascript">
    $(document).ready(function () {
        $("#trPrintBack").hide();
    });
</script>

<%  End If%>