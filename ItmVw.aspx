<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ItmVw.aspx.vb" Inherits="POSN.ItmVw" %>

<script src="./js/jquery.min.js"></script>
<script type="text/javascript">
 

    function showborder() {
        if ( $('#tdMain').height() != null) {
            $("#divPreviewItem").addClass("ui-corner-all roundbox");
            //$('#divPreviewItem').height($('#tdMain').height());
            $('#divPreviewItem').width($('#tdMain').width());
            $("#imgItem").css("border", "thin solid #808080");
            $("#tblMain").css("width", "100%");
        }
      
    }
</script>

<td valign="top" background="">
     <div id="divPreviewItem" style="height:300px;">
    <form id="frmItmVw" method="post" runat="server">
      
        <table cellspacing="0" cellpadding="0" height="380" width="250px" align="left" border="0">
            <tbody>
                <tr valign="top">
                    <td background="" >
                        <asp:panel id="pnlItm" runat="server" visible="false">
                            
							<TABLE cellSpacing="0" width="100%" align="center" bgColor="transparent" border=0 id="tdMain">
								<TR>
									<TD align="center">
										<asp:label id="lblImgRef" runat="server" text="" CssClass="lblDSmall"></asp:label></TD>
								</TR>
								<TR>
									<TD align="center">
										<asp:label id="lblImgDes" runat="server" text="" CssClass="lblDSmallMulti"></asp:label></TD>
								</TR>
								<TR>
									<TD align="center">
                                        <div style="height:194px; width:150px;">
										    <asp:image id="imgItem" runat="server" style="max-width:100%; height:auto;" ImageUrl="./jpgs/Empty.png"></asp:image>
                                        </div>
                                    </TD>
								</TR>
								<TR>
									<TD vAlign="bottom" align="center">
									<div style="padding:10px;">
									<asp:label id="lblExtendedDescription" runat="server" visible="true" text="" CssClass="lblDSmall"></asp:label></div></TD>
								</TR>
								<TR>
									<TD vAlign="bottom" align="center">
										<asp:hyperlink id="lkHigh" runat="server" text="" CssClass="lblDSmall" ToolTip="View High Resolution"></asp:hyperlink>
										<asp:label id="lblHighSize" runat="server" visible="false" text="" CssClass="lblDSmall"></asp:label></TD>
								</TR>
								<TR>
									<TD vAlign="top" align="center">
										<asp:hyperlink id="lkBase" runat="server" text="" CssClass="lblDSmall" ToolTip="View"></asp:hyperlink>
										<asp:label id="lblBaseSize" runat="server" visible="false" text="" CssClass="lblDSmall"></asp:label>
										<asp:hyperlink id="lkLow" runat="server" text="" CssClass="lblDSmall" ToolTip="View Low Resolution"></asp:hyperlink>
										<asp:label id="lblLowSize" runat="server" visible="false" text="" CssClass="lblDSmall"></asp:label></TD>
								</TR>
                                <tr>
                                    <td valign="top" align="center">
                                      
              
                                                     <Asp:panel runat="server" id="panelDownload" cssclass="lblDSmall" visible="false"><B>Avaliable Downloadable Formats</B></Asp:panel>
                                    <Asp:gridview runat="server" id="rptDownloadables" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="0px" CellPadding="4" EnableModelValidation="True" ForeColor="Black" GridLines="None"  AutoGenerateColumns="False" cssclass="lblDSmall">
                                        <Columns>
                                            
                                            <asp:TemplateField>
                                                <itemtemplate><img src="<%#ResolveUrl("~/icons/")%><%#Eval("icon")%>" />                
                                              </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField datafield="FormatName" />   
                                            <Asp:templatefield>
                                                <itemtemplate>
                                                    <span class="lblDSmall">(<%#GetFileSize(eval("FileName")) %>)</span>
                                                </itemtemplate>
                                            </Asp:templatefield>
                                         </Columns> 
                                    </Asp:gridview>
                                        </table>
   
                     </asp:panel>
                        <asp:panel id="pnlNoItm" runat="server" visible="false">
							<TABLE align="center" bgColor="transparent">
								<TR>
									<TD>
										<asp:label id="lblNoItm" runat="server" visible="false" text="Invalid Item..." CssClass="lblListB"></asp:label></TD>
								</TR>
							</TABLE>
						</asp:panel>
</td>
</tr>
            </tbody>
        </table>
         
    </form>
</div> 
</td>
