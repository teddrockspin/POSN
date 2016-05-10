''''''''''''''''''''
'mw - 08-27-2008
'mw - 06-18-2008
'mw - 06-07-2008
'mw - 08-18-2007
'mw - 04-25-2007
''''''''''''''''''''


Imports System.Text
Imports System.IO

Partial Class OrdSve
    Inherits paraPageBase
    Private Const MSG_RESP = "This message was sent from a notification-only address.  Please do not reply to this message."
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents tblLoad As System.Web.UI.WebControls.Table
    Protected WithEvents tblKit As System.Web.UI.WebControls.Table
    Protected WithEvents tblKitEdt As System.Web.UI.WebControls.Table
    Protected WithEvents tblRequestTtl As System.Web.UI.WebControls.Table
    Protected WithEvents tblCstTtl As System.Web.UI.WebControls.Table
    Protected WithEvents tblDetTtl As System.Web.UI.WebControls.Table
    Private strLimitMessage As String = ""
    Private blnEnableSubmit As Boolean = True
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()

        MyBase.Page_Init(sender, e)
        LoadData()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        LoadPage()


    End Sub

    Sub LoadPage()
        If MyBase.CurrentOrder.EditOrder Then
            PageTitle = "Order Details"
        Else
            PageTitle = "Submit / Confirm"
        End If
        If Request.QueryString("ordermsg") <> "" Then
            divOrderMsg.Visible = True
            lblOrderMessage.Text = Request.QueryString("ordermsg")
        Else
            divOrderMsg.Visible = False
        End If
        'MyBase.Page_Load(sender, e)
        MyBase.PageMessage = "Verify all information is accurate.  Click Submit Order to complete the order process."

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            'If (MyBase.UState = MyBase.MyData.STATE_None) Then
            'Response.Write("Logged out redirect<BR>")

            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Or ViewState("print") = 1 Then
            If (MyBase.CurrentOrder.ConfirmNo <= 0) Then
                'Get Review Data her...
                LoadControls()
            End If
        End If 'End PostBack




        If (Page.IsPostBack = False) Or ViewState("print") = 1 Then
            If MyBase.CurrentOrder.EditOrder Then
                cmdSubmit.Visible = False
                Dim blnCanEditOrder As Boolean
                Dim qa As New dsAccessCodesTableAdapters.QueriesTableAdapter

                blnCanEditOrder = qa.CheckIfCodeCanEditOrder(CurrentUser.AccessCode, CurrentUser.CustomerID)
                tcWarningEdit.Visible = True


                If CurrentOrder.StatusID = 6 And blnCanEditOrder Then
                    cmdBackCancel.Visible = True
                    If blnEnableSubmit Then
                        cmdBackEdit.Visible = True
                    End If

                    cmdCancelOrder.Visible = True
                Else

                   

                    cmdEditItm.Visible = False
                    cmdEditCS.Visible = False
                    cmdEditReq.Visible = False
                    cmdEditShp.Visible = False
                    cmdBackEdit.Visible = False
                    cmdBack.Visible = True
                End If
            Else
                Dim po As New paraOrder
                If po.CheckForDownloadOnly(MyBase.CurrentOrder.ItmCart, MyBase.CurrentOrder.KitCart) Then
                    tblShip.Visible = False
                    tblShpTtl.Visible = False
                    tblCSTtl.Visible = False
                    tblCustom.Visible = False

                    divCustomBox.Visible = False
                    divShipBox.Visible = False
                End If
            End If



            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri.ToString

            Dim pScript As New StringBuilder
            Dim pScripResubmit As New StringBuilder

            pScripResubmit.Append("<script>" & _
                 "function CheckSubmitRe () " & _
                 "{ " & _
                 "  return confirm('Do you wish to re-submit the current order at this time?');" & _
                 "} " & _
                 "</script>")

            'mw - 06-18-2008 - check before continuing
            pScript.Append("<script>" & _
            "function CheckSubmit () " & _
            "{ " & _
            "  return confirm('Do you wish to submit the current order at this time?');" & _
            "} " & _
            "</script>")
            Me.RegisterStartupScript("Startup", pScript.ToString)
            cmdSubmit.Attributes.Add("onclick", "return CheckSubmit(); ")
            cmdCancelOrder.Attributes.Add("onclick", "return confirm_cancel();")
            cmdBackEdit.Attributes.Add("onclick", "return confirm_resubmit();")
            'Me.RegisterStartupScript("Startup", pScripResubmit.ToString)
            'cmdBackEdit.Attributes.Add("onclick", "return CheckSubmitRe(); ")

            'Validating uploaded files:
            If Page.IsPostBack = False Then

            End If

        End If 'End PostBack
    End Sub
    Private Function LoadData()
        Dim sMsg As String = String.Empty
        Dim blnVisibleSubmit As Boolean = True

        Dim blnDisableAllQuantityLimits As String


        If CurrentOrder.EditOrder Then
            Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
            blnDisableAllQuantityLimits = qa.GetDisableAllQuantityLimitsByAccessCodeID(CurrentOrder.AccessCodeID)
            lblTitle.Text = "Order# " & CurrentOrder.OrderID
        Else
            blnDisableAllQuantityLimits = CurrentUser.DisableAllQuantityLimits
        End If



        If (MyBase.CurrentOrder.IsValid(blnDisableAllQuantityLimits, blnVisibleSubmit)) Then
            lblMsg.Text = ""

            'cmdSubmit.Enabled = True
            cmdSubmit.Visible = True
        Else
            If CurrentOrder.EditOrder Then
                If CurrentOrder.CanExtendQtyLI = False Then blnEnableSubmit = blnVisibleSubmit
            Else
                If CurrentUser.CanExtendQtyLI = False Then blnEnableSubmit = blnVisibleSubmit
            End If

            lblStatus.Text = "** Unable to place order as is.  Please correct the problem listed below and try placing again."
            lblPMsg.Text = MyBase.CurrentOrder.ProblemMsg
            lblPMsg.Visible = True
        End If
    End Function

    Private Function LoadControls() As Boolean
        Dim rcd As System.Data.DataTable
        Dim indx As Integer
        Dim iMaxIndx As Integer
        Dim bRequiresLI As Boolean = False
        Dim bRequiresOR As Boolean = False
        Dim bSuccess As Boolean = False


        Dim taShowStock As New dsAccessCodesTableAdapters.Customer_AccessCode1TableAdapter
        Dim drShowStock As dsAccessCodes.Customer_AccessCode1Row

        Dim blnShowStock As Boolean = False
        Try
            drShowStock = taShowStock.GetDataByAccessCodeID(CurrentUser.AccessCodeID).Rows(0)
            blnShowStock = drShowStock.ShowStock
        Catch ex As Exception

        End Try



        Dim taOrderLimit As New dsInventoryTableAdapters.OrderQuantityLimitByIDTableAdapter
        Dim dtOrderLimit As New dsInventory.OrderQuantityLimitByIDDataTable
        Dim taOrderLimitKit As New dsInventoryTableAdapters.ComponentQuantityLimit2TableAdapter
        Dim dtOrderLimitKit As New dsInventory.ComponentQuantityLimit2DataTable
        Dim drOrderLimitKit As dsInventory.ComponentQuantityLimit2Row
        Dim blnDisableAllLimits As Boolean = CurrentUser.DisableAllQuantityLimits
        Dim blnRequiresUpload As Boolean = False
        Dim blnBackorderItems As Boolean = False
        Dim sColor As String = ""

        If CurrentOrder.EditOrder Then
            blnDisableAllLimits = CurrentOrder.DisableAllQuantityLimits
        End If
        Dim qaInv As New dsInventoryTableAdapters.QueriesTableAdapter

        taOrderLimit.ClearBeforeFill = True
        taOrderLimitKit.ClearBeforeFill = True

        rcd = MyBase.CurrentOrder.CstCart
        iMaxIndx = rcd.Rows.Count

        Dim paraorder As New paraOrder
        Dim paradata As New paraData

        With MyBase.CurrentOrder

            Dim decOrderProc As Decimal = CurrentOrder.ProcessingChg
            If paraorder.CheckForDownloadOnly(MyBase.CurrentOrder.ItmCart) Then
                decOrderProc = 0
                CurrentOrder.ProcessingChg = decOrderProc
            End If
            AddHeaderRow(tblReqTtl, "<p class=""headerspacing"">Requestor Information</p>")
            'AddDetailRow(tblReq, "Requestor Title:", .RequestorTitle)
            AddDetailRow(tblReq, "Requestor First Name:", .RequestorFirstName)
            AddDetailRow(tblReq, "Requestor Last Name:", .RequestorLastName)
            AddDetailRow(tblReq, "Requestor E-mail:", .RequestorEmail)
            AddDetailRow(tblReq, "", "")
            indx = 0
            Do While indx < iMaxIndx
                If (rcd.Rows(indx).Item("FieldType") = MyBase.MyData.CST_REQ) Then
                    AddDetailRow(tblReq, rcd.Rows(indx).Item("FieldName").ToString() & ":", rcd.Rows(indx).Item("FieldValue").ToString())
                End If
                indx = indx + 1
            Loop
            'AddDetailRow("", "")
            AddHeaderRow(tblShpTtl, "<p class=""headerspacing"">Shipping Information</p>")
            AddDetailRow(tblShp, "Shipping Contact First Name:", .ShipContactFirstName)
            AddDetailRow(tblShp, "Shipping Contact Last Name:", .ShipContactLastName)
            AddDetailRow(tblShp, "Shipping Contact Company:", .ShipCompany)
            AddDetailRow(tblShp, "Shipping Address:", .ShipAddress1)
            AddDetailRow(tblShp, "", .ShipAddress2)
            AddDetailRow(tblShp, "", .ShipCity & ", " & .ShipState & " " & .ShipZip & "  " & .ShipCountry)
            If (.CountryExclusionCheck(MyBase.MyData, MyBase.CurrentUser.CustomerID, .ShipCountry) = True) Then
                AddNoteRow(tblShp, "Shipping country requires authorization.", True)
            End If
            'show recipient Phone/Email:
            AddDetailRow(tblShp, "Recipient Phone:", .RecipientPhone)
            AddDetailRow(tblShp, "Recipient Email:", .RecipientEmail)
            AddDetailRow(tblShp, "Preferred Shipping Method:", .ShipPrefDesc)

            If CurrentOrder.EditOrder Then
                Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
                If qa.CheckShippingMethodPermissions(CurrentOrder.AccessCodeID, CurrentOrder.ShipPrefID) = 2 Then
                    AddNoteRow(tblShp, "Shipping method requires authorization.", True)
                End If
                'If (Left(CurrentOrder.ShipPrefDesc, 1) = "*") Then AddNoteRow(tblShp, "Shipping method requires authorization.", True)
            Else
                If (.RequiresAuthorShip) Then AddNoteRow(tblShp, "Shipping method requires authorization.", True)
            End If

            AddDetailRow(tblShp, "Notes:", .ShipNote, 1)
            '
            If IsDate(CurrentOrder.ScheduledDelivery) AndAlso CurrentOrder.ScheduledDelivery.Year > 1900 Then
                AddDetailRow(tblShp, "Scheduled Delivery:", .ScheduledDelivery, 0)
            End If
            '
            indx = 0
            'Requestor custom fields Information:
            Do While indx < iMaxIndx
                If (rcd.Rows(indx).Item("FieldType") = MyBase.MyData.CST_SHP) Then
                    If paraorder.CheckIfCustomItemShows(CurrentOrder.ShipPrefID, rcd.Rows(indx).Item(1)) Then
                        AddDetailRow(tblShp, rcd.Rows(indx).Item("FieldName").ToString() & ":", rcd.Rows(indx).Item("FieldValue").ToString())
                    End If

                End If
                indx = indx + 1
            Loop
            'AddDetailRow("", "")
            AddHeaderRow(tblCSTtl, "<p class=""headerspacing"">Custom Information</p>")
            AddDetailRow(tblCS, "Cover Sheet:", .CoverSheetDesc)
            AddDetailRow(tblCS, "Cover Sheet Detail:", .CoverSheetText, 2)
            AddDetailRow(tblCS, "", "")
            rcd = MyBase.CurrentOrder.CstCart
            indx = 0
            Do While indx < iMaxIndx
                If (rcd.Rows(indx).Item("FieldType") = MyBase.MyData.CST_DET) Then
                    AddDetailRow(tblCS, rcd.Rows(indx).Item("FieldName").ToString() & ":", rcd.Rows(indx).Item("FieldValue").ToString())
                End If
                indx = indx + 1
            Loop
            AddHeaderRow(tblItmTtl, "<p class=""headerspacing"">Order Details</p>")

            rcd = MyBase.CurrentOrder.ItmCart
            iMaxIndx = rcd.Rows.Count
            indx = 0
            Dim blnAddSubHeader As Boolean = True

            Dim decItemTotals As Decimal = 0

            Dim sLineDescription As String
            .OverQuantityLimit = False
            Do While indx < iMaxIndx
                If rcd.Rows(indx).RowState <> DataRowState.Deleted Then
                    'regular items:
                    Dim intPrintMethod As Integer = 0
                    intPrintMethod = qaInv.getDocumentPrintMethod(rcd.Rows(indx).Item("ItmID").ToString())
                    If intPrintMethod <> 3 Then
                        If blnAddSubHeader Then
                            AddSubHeaderRow(tblItm, "Items")
                            blnAddSubHeader = False
                        End If
                        dtOrderLimit = taOrderLimit.GetData(rcd.Rows(indx).Item("ItmID"))
                        If (dtOrderLimit.Rows.Count > 0) Then
                            If (dtOrderLimit(0).OrderQuantityLimit < rcd.Rows(indx).Item("itmqty")) And blnDisableAllLimits = False Then
                                sLineDescription = rcd.Rows(indx).Item("ItmName").ToString() & " - " & rcd.Rows(indx).Item("ItmDesc").ToString() & String.Format("<span class=""lblNoteInactive""> - Over quantity limit of {0}. Requires authorization</ span>", dtOrderLimit(0).OrderQuantityLimit)
                                .OverQuantityLimit = True
                            Else
                                sLineDescription = rcd.Rows(indx).Item("ItmName").ToString() & " - " & rcd.Rows(indx).Item("ItmDesc").ToString()
                            End If
                        Else
                            sLineDescription = rcd.Rows(indx).Item("ItmName").ToString() & " - " & rcd.Rows(indx).Item("ItmDesc").ToString()
                        End If

                        sLineDescription += CheckLineItemLimit(rcd.Rows(indx).Item("ItmID"), rcd.Rows(indx).Item("itmqty"))

                        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
                        If qa.CheckIfItemRequiresUpload(rcd.Rows(indx).Item("ItmID")) And CurrentOrder.EditOrder = False Then
                            If CheckIfFileUploaded(rcd.Rows(indx).Item("ItmID")) = False Then
                                blnRequiresUpload = True
                            End If
                        End If
                        sColor = rcd.Rows(indx).Item("ItmColor").ToString()
                        If (InStr(sColor, MyData.COLOR_BACK) > 0) Then
                            blnBackorderItems = True
                        End If
                        'GetOrderQty(lLIMax, lLIQty)

                        'bValid = bValid And ((pMaxQtyPerLineItem >= lLIMax) Or _
                        '                     ((pMaxQtyPerLineItem < lLIMax) And pCanExtendQtyLI))
                        'If Not bValid Then
                        '    'pProblemMsg = "Exceeded Limits on Maximum Line Item Limit (" & iLIMax.ToString() & ")"
                        '    pProblemMsg = "Exceeded Limits on Maximum Line Item Limit (Limit = " & pMaxQtyPerLineItem.ToString() & ", Selection = " & lLIMax.ToString() & ")"
                        '    pProblemArea = PRO_ITM
                        'End If


                        AddDetailRow(tblItm, rcd.Rows(indx).Item("ItmQty").ToString() & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", sLineDescription, , rcd.Rows(indx).Item("itmqty"), rcd.Rows(indx).Item("ItmID"), 1, decItemTotals)
                    End If
                End If

                indx = indx + 1
            Loop
            AddDetailRow(tblItm, "", "")

            blnAddSubHeader = True
            'Downloadable items:

            rcd = MyBase.CurrentOrder.ItmCart
            iMaxIndx = rcd.Rows.Count
            indx = 0

            Do While indx < iMaxIndx
                If rcd.Rows(indx).RowState <> DataRowState.Deleted Then

                    Dim intPrintMethod As Integer = 0
                    intPrintMethod = qaInv.getDocumentPrintMethod(rcd.Rows(indx).Item("ItmID").ToString())
                    If intPrintMethod = 3 Then
                        If blnAddSubHeader Then
                            AddSubHeaderRow(tblItm, "Downloads")
                            blnAddSubHeader = False
                        End If
                        dtOrderLimit = taOrderLimit.GetData(rcd.Rows(indx).Item("ItmID"))
                        If (dtOrderLimit.Rows.Count > 0) Then
                            If (dtOrderLimit(0).OrderQuantityLimit < rcd.Rows(indx).Item("itmqty")) And blnDisableAllLimits = False Then
                                sLineDescription = rcd.Rows(indx).Item("ItmName").ToString() & " - " & rcd.Rows(indx).Item("ItmDesc").ToString() & String.Format("<span class=""lblNoteInactive""> - Over quantity limit of {0}. Requires authorization</ span>", dtOrderLimit(0).OrderQuantityLimit)
                                .OverQuantityLimit = True
                            Else
                                sLineDescription = rcd.Rows(indx).Item("ItmName").ToString() & " - " & rcd.Rows(indx).Item("ItmDesc").ToString()
                            End If
                        Else
                            sLineDescription = rcd.Rows(indx).Item("ItmName").ToString() & " - " & rcd.Rows(indx).Item("ItmDesc").ToString()
                        End If

                        sLineDescription += CheckLineItemLimit(rcd.Rows(indx).Item("ItmID"), rcd.Rows(indx).Item("itmqty"))

                        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
                        If qa.CheckIfItemRequiresUpload(rcd.Rows(indx).Item("ItmID")) And CurrentOrder.EditOrder = False Then
                            If CheckIfFileUploaded(rcd.Rows(indx).Item("ItmID")) = False Then
                                blnRequiresUpload = True
                            End If
                        End If


                        'GetOrderQty(lLIMax, lLIQty)

                        'bValid = bValid And ((pMaxQtyPerLineItem >= lLIMax) Or _
                        '                     ((pMaxQtyPerLineItem < lLIMax) And pCanExtendQtyLI))
                        'If Not bValid Then
                        '    'pProblemMsg = "Exceeded Limits on Maximum Line Item Limit (" & iLIMax.ToString() & ")"
                        '    pProblemMsg = "Exceeded Limits on Maximum Line Item Limit (Limit = " & pMaxQtyPerLineItem.ToString() & ", Selection = " & lLIMax.ToString() & ")"
                        '    pProblemArea = PRO_ITM
                        'End If


                        AddDetailRow(tblItm, rcd.Rows(indx).Item("ItmQty").ToString() & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", sLineDescription, , rcd.Rows(indx).Item("itmqty"), rcd.Rows(indx).Item("ItmID"), 1, decItemTotals)
                    End If
                End If

                indx = indx + 1
            Loop
            AddDetailRow(tblItm, "", "")


            'KITS:
            blnAddSubHeader = True

            rcd = MyBase.CurrentOrder.KitCart
            iMaxIndx = rcd.Rows.Count
            indx = 0

            Do While indx < iMaxIndx
                If rcd.Rows(indx).RowState <> DataRowState.Deleted Then
                    dtOrderLimitKit = taOrderLimitKit.GetData(rcd.Rows(indx).Item("KitID"))
                    sLineDescription = ""
                    For Each drOrderLimitKit In dtOrderLimitKit.Rows
                        If blnAddSubHeader Then
                            AddSubHeaderRow(tblItm, "Kits")
                            blnAddSubHeader = False
                        End If

                        dtOrderLimit = taOrderLimit.GetData(drOrderLimitKit.ItemNumber)
                        If (dtOrderLimit.Rows.Count > 0) Then
                            If (dtOrderLimit(0).OrderQuantityLimit < (rcd.Rows(indx).Item("KitQty") * drOrderLimitKit.Qty)) And blnDisableAllLimits = False Then
                                .OverQuantityLimit = True
                                sLineDescription = rcd.Rows(indx).Item("KitName").ToString() & " - " & rcd.Rows(indx).Item("KitDesc").ToString() & String.Format("<span class=""lblNoteInactive""> - Over quantity limit of {0} for item {1} in this kit. Requires authorization</ span>", dtOrderLimit(0).OrderQuantityLimit, dtOrderLimit(0).ReferenceNo)
                                Exit For
                            End If
                        End If
                    Next
                    If (sLineDescription = "") Then
                        sLineDescription = rcd.Rows(indx).Item("KitName").ToString() & " - " & rcd.Rows(indx).Item("KitDesc").ToString()
                    End If
                    AddDetailRow(tblItm, rcd.Rows(indx).Item("KitQty").ToString() & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", sLineDescription, , rcd.Rows(indx).Item("KitQty"), rcd.Rows(indx).Item("KitID"), 2, decItemTotals)
                    'add kit contents row:
                    Try
                        sColor = rcd.Rows(indx).Item("ItmColor").ToString()
                    Catch ex As Exception
                        sColor = ""
                    End Try

                    If (InStr(sColor, MyData.COLOR_BACK) > 0) Then
                        blnBackorderItems = True
                    End If

                    Dim tc As New TableCell
                    Dim tr As New TableRow
                    tc.ColumnSpan = 10
                    tc.Text = paradata.GetKitContents(rcd.Rows(indx).Item("KitID"), "", False, True)
                    tr.Cells.Add(tc)
                    tblItm.Rows.Add(tr)



                    'check for upload files:
                    Dim ta As New dsCustomerDocumentTableAdapters.Customer_Kit_DocumentTableAdapter
                    Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter

                    For Each dr As dsCustomerDocument.Customer_Kit_DocumentRow In ta.GetDataByKitID(CInt(rcd.Rows(indx).Item("KitID").ToString)).Rows
                        If qaInv.CheckIfItemRequiresUpload(dr.DocumentID) And CurrentOrder.EditOrder = False Then
                            If CheckIfFileUploaded(dr.DocumentID) = False Then
                                blnRequiresUpload = True
                            End If
                        End If

                    Next

                End If
                indx = indx + 1
            Loop




            If CurrentUser.ShowPrice Then
                AddFooter(decItemTotals)
            End If

            If blnBackorderItems Then
                AddNoteRow(tblItm, "Note: Items in <span style='color:#bd5304;'>orange</span> are on backorder and not available", False)
            End If
            'Authorization Note...
            If CurrentOrder.EditOrder Then
                .CurrentAuthorizationStatus(bRequiresLI, bRequiresOR, CurrentOrder.DisableAllQuantityLimits, CurrentOrder.MaxQtyPerLineItem, CurrentOrder.MaxQtyPerOrder)
            Else
                .CurrentAuthorizationStatus(bRequiresLI, bRequiresOR, CurrentUser.DisableAllQuantityLimits)
            End If

            'If (bRequiresLI) Then AddNoteRow(tblItm, "Quantity of line items requires authorization.", True)
            If (bRequiresOR) Then AddNoteRow(tblItm, "Note: Total quantity of items ordered requires authorization.", True)
            If (bRequiresLI) Then AddNoteRow(tblItm, strLimitMessage, True)
            'AddNoteRow("Confirmation will be sent to...  " & MyBase.CurrentOrder.RequestorEmail)
            lblConfirm.Text = "Confirmation will be sent to...  " & MyBase.CurrentOrder.RequestorEmail : lblConfirm.Visible = True
        End With
        rcd.Dispose() : rcd = Nothing
        bSuccess = True

        If blnRequiresUpload And CurrentOrder.EditOrder = False Then
            cmdSubmit.Visible = False
            cmdSubmit.Enabled = False
            dUploadMessage.Visible = True
        End If
        Return (bSuccess)
    End Function
    Public Function CheckIfFileUploaded(ByVal intItemNum As Integer) As Boolean
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
        Dim strRefNo As String = qa.GetItemRefNumber(intItemNum)
        Dim strPath As String = ConfigurationManager.AppSettings("UploadsFolder").ToString & Session.SessionID & "/"
        If Directory.Exists(Server.MapPath(strPath)) Then
            'strPath = strPath.Replace(HttpRuntime.AppDomainAppPath, "/").Replace(Path.DirectorySeparatorChar, "/")
            Dim dirInfo As New DirectoryInfo(Server.MapPath(strPath))
            Dim fileInfo As FileInfo() = dirInfo.GetFiles(strRefNo & ".*", SearchOption.TopDirectoryOnly)

            If fileInfo.Length > 0 Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function
    Sub AddFooter(ByVal decItemTotals As Decimal)
        Dim tRow As New TableRow
        Dim tCell As TableCell
        Dim po As New paraOrder
        Dim decOrderProc As Decimal = CurrentOrder.ProcessingChg
        If po.CheckForDownloadOnly(MyBase.CurrentOrder.ItmCart) Then
            decOrderProc = 0
            CurrentOrder.ProcessingChg = decOrderProc
        End If

        Dim decOtherCharge As Decimal

        tRow = New TableRow
        tCell = New TableCell

        tCell.Text = ""
        tRow.Cells.Add(tCell)

        tCell = New TableCell
        tCell.Text = ""
        tRow.Cells.Add(tCell)

        tCell = New TableCell
        tCell.Text = "Items/Kits Total:"
        tCell.CssClass = "lblTitle"
        tCell.ColumnSpan = 2
        tCell.HorizontalAlign = HorizontalAlign.Right
        tRow.Cells.Add(tCell)

        tCell = New TableCell
        tCell.Text = Format(decItemTotals, "Currency")
        tCell.CssClass = "lblTotal"
        tRow.Cells.Add(tCell)

        tblItm.Rows.Add(tRow)



        '-----
        tRow = New TableRow
        tCell = New TableCell

        tCell.Text = ""
        tRow.Cells.Add(tCell)

        tCell = New TableCell
        tCell.Text = ""
        tRow.Cells.Add(tCell)

        tCell = New TableCell
        tCell.Text = "Total Order Processing:"
        tCell.CssClass = "lblTitle"
        tCell.ColumnSpan = 2
        tCell.HorizontalAlign = HorizontalAlign.Right
        tRow.Cells.Add(tCell)

        tCell = New TableCell
        tCell.Text = Format(decOrderProc, "Currency")
        tCell.CssClass = "lblTotal"
        tRow.Cells.Add(tCell)

        tblItm.Rows.Add(tRow)

        '-----
        tRow = New TableRow
        tCell = New TableCell
        tCell.Text = ""
        tRow.Cells.Add(tCell)

        tCell = New TableCell
        tCell.Text = ""
        tRow.Cells.Add(tCell)

        tCell = New TableCell
        tCell.Text = "Total Other <small>(Determined at the time of shipping)</small>:"
        tCell.CssClass = "lblTitle"
        tCell.ColumnSpan = 2
        tCell.HorizontalAlign = HorizontalAlign.Right
        tRow.Cells.Add(tCell)

        tCell = New TableCell
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        Dim intOrderID As Integer = CurrentOrder.OrderID
        If intOrderID > 0 Then
            decOtherCharge = qa.GetOrderOtherCharge(intOrderID)
            If IsNumeric(decOtherCharge) Then
                tCell.Text = Format(decOtherCharge, "Currency")
            End If

        Else
            tCell.Text = Format(0, "Currency")
        End If
        tCell.CssClass = "lblTotal"
        tRow.Cells.Add(tCell)

        tblItm.Rows.Add(tRow)


        '-----
        tRow = New TableRow
        tCell = New TableCell
        tCell.Text = ""
        tRow.Cells.Add(tCell)

        tCell = New TableCell
        tCell.Text = ""
        tRow.Cells.Add(tCell)

        tCell = New TableCell
        tCell.Text = "Grand Total <small>(Excluding Shipping)</small>:"
        tCell.CssClass = "lblTitle"
        tCell.ColumnSpan = 2
        tCell.HorizontalAlign = HorizontalAlign.Right
        tRow.Cells.Add(tCell)

        tCell = New TableCell
        tCell.Text = Format(decItemTotals + decOrderProc + decOtherCharge, "Currency")
        tCell.CssClass = "lblTotal"
        tRow.Cells.Add(tCell)

        tblItm.Rows.Add(tRow)

    End Sub
    Function CheckLineItemLimit(ByVal intItemID As Integer, ByVal intQty As Integer) As String
        Dim strTmp As String = ""
        Dim strAccessCode As String
        If CurrentOrder.EditOrder Then
            strAccessCode = CurrentOrder.AccessCode
        Else
            strAccessCode = CurrentUser.AccessCode
        End If
        With CurrentUser
            If .DisableAllQuantityLimits = False Then

                Dim dsCustDoc As New dsCustomerDocumentTableAdapters.OrderQuantityLimitTableAdapter
                Dim drCustDocR As dsCustomerDocument.OrderQuantityLimitRow = dsCustDoc.GetData(intItemID).Rows(0)
                Dim dsAccessCode As New dsOrdersTableAdapters.Customer_AccessCodeTableAdapter
                Dim drAccessCode As dsOrders.Customer_AccessCodeRow = dsAccessCode.GetDataByCode(.CustomerID, strAccessCode).Rows(0)

                Dim intOrderQuantityLimit As Integer = drCustDocR.OrderQuantityLimit
                Dim intMaxQtyPerLineItem As Integer = drAccessCode.MaxQtyPerLineItem

                If (intMaxQtyPerLineItem < intQty And intOrderQuantityLimit <= intMaxQtyPerLineItem) And CurrentUser.DisableAllQuantityLimits = False Then
                    strTmp = "<P class=""lblNoteInactive"">Exceeded Limits on Maximum Line Item Limit (Limit = " & intMaxQtyPerLineItem.ToString() & ", Selection = " & intQty.ToString() & ") , Requires Authorization.</p>"
                End If
            End If
        End With

        'strLimitMessage += "<P>" & strTmp & "</p>"
        Return strTmp
    End Function
    Private Sub AddHeaderRow(ByRef tblLoad As Table, ByVal sLabel As String)
        Dim tRow As TableRow
        Dim tCell As TableCell
        Dim lLabel As Label

        tRow = New TableRow
        tCell = New TableCell
        tCell.ColumnSpan = 3
        tCell.HorizontalAlign = HorizontalAlign.Center
        tCell.VerticalAlign = VerticalAlign.Middle
        lLabel = New Label
        lLabel.CssClass = "lblXLong"
        lLabel.Font.Bold = True
        lLabel.Text = sLabel
        tCell.Controls.Add(lLabel)
        tRow.Cells.Add(tCell)

        tblLoad.Rows.Add(tRow)
    End Sub

    Private Sub AddSubHeaderRow(ByRef tblLoad As Table, ByVal sLabel As String)
        Dim tRow As TableRow
        Dim tCell As TableCell
        Dim lLabel As Label

        tRow = New TableRow
        tCell = New TableCell
        tCell.ColumnSpan = 3
        tCell.HorizontalAlign = HorizontalAlign.Left
        tCell.VerticalAlign = VerticalAlign.Middle
        lLabel = New Label
        lLabel.CssClass = "lblXLong"
        lLabel.Font.Bold = True
        lLabel.Text = sLabel
        tCell.Controls.Add(lLabel)
        tRow.Cells.Add(tCell)

        If CurrentUser.ShowPrice Then
            'each
            tCell = New TableCell
            tCell.HorizontalAlign = HorizontalAlign.Center
            tCell.Text = "Each"

            tCell.CssClass = "lblTitle"
            tRow.Cells.Add(tCell)
            'total
            tCell = New TableCell
            tCell.HorizontalAlign = HorizontalAlign.Center
            tCell.CssClass = "lblTitle"
            tCell.Text = "Total"

            tRow.Cells.Add(tCell)
        End If

        tblLoad.Rows.Add(tRow)
    End Sub

    Private Sub AddNoteRow(ByRef tblLoad As Table, ByVal sLabel As String, Optional ByVal bRed As Boolean = False)
        Dim tRow As TableRow
        Dim tCell As TableCell
        Dim lLabel As Label

        tRow = New TableRow
        tCell = New TableCell
        tCell.ColumnSpan = 3
        tCell.HorizontalAlign = HorizontalAlign.Center
        tCell.VerticalAlign = VerticalAlign.Middle
        lLabel = New Label
        If bRed Then
            lLabel.CssClass = "lblXLongDescriptorRed"
        Else
            lLabel.CssClass = "lblXLongDescriptor"
        End If

        If sLabel.Length > 0 Then
            lLabel.Text = "<P>" & sLabel & "</p>"
        End If

        'lLabel.Text += strLimitMessage
        tCell.Controls.Add(lLabel)
        tRow.Cells.Add(tCell)

        tblLoad.Rows.Add(tRow)
    End Sub

    Private Sub AddDetailRow(ByRef tblLoad As Table, ByVal sLabel As String, ByVal sText As String, Optional ByVal bMultiLine As Byte = 0, Optional ByVal lQty As Integer = 0, Optional ByVal ItmID As Integer = 0, Optional ByVal intItmOrKit As Integer = 0, Optional ByRef decVal As Decimal = 0)
        Dim tRow As TableRow
        Dim tCell As TableCell

        Dim qaS As New dsInventoryTableAdapters.QueriesTableAdapter



        tRow = New TableRow
        tCell = New TableCell
        tCell.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
        tRow.Cells.Add(tCell)
        Dim sColor As String = ""
        If ItmID > 0 Then
            Select Case qaS.GetDocumentStatusIDByDocumentID(ItmID)
                Case 0
                    'sColor = MyData.COLOR_INACTIVE
                Case 2 'back
                    sColor = MyData.COLOR_BACK
                Case 1 'normal
            End Select

        End If

        tCell = GetLabelCell(sLabel)
        tRow.Cells.Add(tCell)

        tCell = GetTextCell(sText, bMultiLine)
        tRow.Cells.Add(tCell)

        Dim lLabel As Label

        If CurrentUser.ShowPrice And lQty > 0 Then
            Select Case intItmOrKit
                Case 1
                    'items
                    Dim decItmPrice As Decimal
                    If CurrentOrder.StatusID <> 3 Then
                        decItmPrice = CurrentOrder.GetItemPrice(ItmID, lQty)
                    Else
                        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
                        decItmPrice = qa.GetItemPriceFromOrder(CurrentOrder.OrderID, ItmID)
                    End If
                    decItmPrice = CurrentOrder.GetItemPrice(ItmID, lQty)
                    tCell = New TableCell
                    tCell.Text = "<span class=""lblPrice"">$" & String.Format("{0:N5}", decItmPrice) & "</span>"
                    tCell.HorizontalAlign = HorizontalAlign.Right
                    tRow.Cells.Add(tCell)
                    tCell = New TableCell
                    tCell.HorizontalAlign = HorizontalAlign.Right
                    tCell.Text = "<span class=""lblPrice"">" & Format(decItmPrice * lQty, "Currency") & "</span>"
                    tRow.Cells.Add(tCell)
                    decVal += decItmPrice * lQty

                Case 2
                    'kits

                    Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
                    Dim decLineKitChg As Decimal = qa.GetLineKitChg(CurrentUser.CustomerID)
                    Dim decKitPrice As Decimal

                    If CurrentOrder.StatusID <> 3 Then
                        decKitPrice = CurrentOrder.GetKitPrice(ItmID, decLineKitChg, lQty)
                    Else
                        decKitPrice = qa.GetKitPriceFromOrder(CurrentOrder.OrderID, ItmID)
                    End If


                    'each
                    tCell = New TableCell
                    tCell.HorizontalAlign = HorizontalAlign.Right
                    lLabel = New Label
                    lLabel.Text = "$" & String.Format("{0:N5}", decKitPrice)
                    lLabel.CssClass = "lblPrice"
                    tCell.Controls.Add(lLabel)
                    tRow.Cells.Add(tCell)
                    'total
                    tCell = New TableCell
                    tCell.HorizontalAlign = HorizontalAlign.Right

                    lLabel = New Label
                    lLabel.Text = "$" & String.Format("{0:N2}", decKitPrice * lQty)
                    lLabel.CssClass = "lblPrice"
                    tCell.Controls.Add(lLabel)

                    tRow.Cells.Add(tCell)

                    decVal += decKitPrice * lQty
            End Select
        End If
        If sColor <> "" Then
            tRow.Style.Add("color", sColor & " !important;")
        End If

        tblLoad.Rows.Add(tRow)
    End Sub

    Private Function GetLabelCell(ByVal sText As String) As TableCell
        Dim tCell As TableCell
        Dim lLabel As Label

        tCell = New TableCell
        tCell.HorizontalAlign = HorizontalAlign.Right
        tCell.VerticalAlign = VerticalAlign.Middle
        lLabel = New Label
        lLabel.CssClass = "lblMediumLong"
        'If (Right(sText, 1) = ":") Then lLabel.Font.Bold = True
        lLabel.Text = sText & "&nbsp;&nbsp;&nbsp;"
        tCell.Controls.Add(lLabel)

        Return tCell
    End Function

    Private Function GetTextCell(ByVal sText As String, Optional ByVal bMultiLine As Byte = 0) As TableCell
        Dim tCell As TableCell
        Dim lLabel As Label
        Dim tText As TextBox

        tCell = New TableCell
        tCell.HorizontalAlign = HorizontalAlign.Left
        tCell.VerticalAlign = VerticalAlign.Middle
        If bMultiLine = 1 Or bMultiLine = 2 Then
            tText = New TextBox
            tText.TextMode = TextBoxMode.MultiLine
            tText.Style.Add("OVERFLOW", "HIDDEN")
            tText.ReadOnly = True
            tText.BorderStyle = BorderStyle.None
            If bMultiLine = 1 Then
                tText.CssClass = "lblXLongM"
            ElseIf bMultiLine = 2 Then
                tText.CssClass = "lblXLongL"
            End If
            tText.Text = sText
            tCell.Controls.Add(tText)
        Else
            lLabel = New Label
            lLabel.CssClass = "lblXLong"
            lLabel.Text = sText
            tCell.Controls.Add(lLabel)
        End If

        Return tCell
    End Function

    Private Function SaveData()
        Dim sMsg As String = String.Empty
        Dim bSuccess As Boolean
        Dim blnDisableAllQuantityLimits As String


        'If CurrentOrder.EditOrder Then
        '    Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        '    blnDisableAllQuantityLimits = qa.GetDisableAllQuantityLimitsByAccessCodeID(CurrentOrder.AccessCodeID)
        'Else
        blnDisableAllQuantityLimits = CurrentUser.DisableAllQuantityLimits
        'End If

        If (MyBase.CurrentOrder.IsValid(blnDisableAllQuantityLimits)) Then
            Dim filepath As String = ConfigurationManager.AppSettings("UploadsFolder").ToString & Session.SessionID & "/"
            bSuccess = MyBase.CurrentOrder.Submit(MyBase.CurrentUser, sMsg, Server.MapPath(filepath))
            If Not (bSuccess) Then
                lblPMsg.Text = sMsg

            Else
                MyBase.PageMessage = "Thank you for placing your order."
            End If
            sMsg += " ran"
        End If

        If (MyBase.CurrentOrder.ConfirmNo > 0) And bSuccess Then
            lblStatus.Text = "Order has been placed under Request # " & MyBase.CurrentOrder.ConfirmNo
            lblStatus.Visible = True
            lblMsg.Text = " Your order will be shipped on or before the next day of business.  If you have provided an email address, you will receive a confirmation via email." & _
                          " This email will contain the date of shipment, method of shipment, and tracking number, if applicable."
            lblMsg.Visible = True
            lblMsgCont.Text = " Any questions or concerns should be directed to "
            lblMsgCont.Visible = True
            lblCustomerName.Text = MyBase.CurrentUser.CustomerName
            lblCustomerName.Visible = True
            lblCustomerPhone.Text = MyBase.CurrentUser.CustomerPhone
            lblCustomerPhone.Visible = True
            lblCustomerEmail.Text = MyBase.CurrentUser.CustomerEmail
            lblCustomerEmail.Visible = True

            lblConfirm.Visible = False
            cmdPrint.Visible = False
            cmdSubmit.Visible = False
            '      cmdCorrect.Visible = False
            tblOrd.Visible = False
            cmdNew.Visible = True
        Else
            lblTitle.Visible = False
            lblStatus.Text = "<p><b><font color=red>D3 ENCOUNTERED AN ERROR.</font></b></p><p>Please click the shopping cart link on the left and resubmit your order.</p><p>" & sMsg & " " & bSuccess.ToString & "</p>"
            lblStatus.Visible = True
            lblMsg.Text = MyBase.CurrentOrder.ProblemMsg
            lblMsg.Visible = True
            tblOrd.Visible = False
            cmdSubmit.Visible = False
            lblConfirm.Visible = False
            cmdPrint.Visible = False
        End If
    End Function

    Private Overloads Sub cmdEditReq_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdEditReq.Click
        Dim sDestin As String = paraPageBase.PAG_OrdRequest.ToString()
        MyBase.PageDirect(sDestin, 0, 0)
    End Sub
    Private Overloads Sub cmdEditShp_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdEditShp.Click
        Dim sDestin As String = paraPageBase.PAG_OrdShip.ToString()
        MyBase.PageDirect(sDestin, 0, 0)
    End Sub
    Private Overloads Sub cmdEditCS_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdEditCS.Click
        Dim sDestin As String = paraPageBase.PAG_OrdCustom.ToString()
        MyBase.PageDirect(sDestin, 0, 0)
    End Sub
    Private Overloads Sub cmdEditItm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdEditItm.Click
        Dim sDestin As String = paraPageBase.PAG_OrdCart.ToString()
        MyBase.PageDirect(sDestin, 0, 0)
    End Sub

    Private Overloads Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdSubmit.Click
        SaveData()
    End Sub

    Private Overloads Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdNew.Click
        Dim sDestin As String = paraPageBase.PAG_OrdItemSelect.ToString()
        'If CurrentUser.CanRememberOrder = False Then
        Dim o As New paraData
        o.StartNewOrder(CurrentUser)
        'Else

        'End If
        MyBase.PageDirect(sDestin, 0, 0)
    End Sub

    Private Overloads Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdPrint.Click
        'Dim sDestin As String = paraPageBase.PAG_OrdPreview.ToString()
        'MyBase.PageDirect(sDestin, 0, 0)
        '                  , False)
        Select Case ViewState("print")
            Case 1
                ViewState("print") = 0
            Case Else
                If ViewState("print") Is DBNull.Value Then
                    ViewState.Add("print", 1)
                Else
                    ViewState("print") = 1
                End If

        End Select

        'cmdPrint.Visible = False
        'cmdBack.Visible = True

        'LoadBaseData()
        LoadPage()
    End Sub

    Private Sub cmdPrintBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrintBack.Click

        Select Case ViewState("print")
            Case 1
                ViewState("print") = 0
            Case Else
                If ViewState("print") Is DBNull.Value Then
                    ViewState.Add("print", 1)

                Else
                    ViewState("print") = 1

                End If

        End Select

        'cmdBack.Visible = False
        'cmdPrint.Visible = True

        'LoadBaseData()
        LoadPage()
    End Sub


#Region "update order"

    Protected Sub cmdEditBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBackEdit.Click
        
        'THIS updates order:

        Dim ta As New dsOrdersTableAdapters.OrderTableAdapter
        Dim drow As dsOrders.OrderRow
        drow = ta.GetOrderDataByID(CurrentOrder.OrderID).Rows(0)

        'Ord req
        drow.Requestor_FirstName = CurrentOrder.RequestorFirstName
        drow.Requestor_LastName = CurrentOrder.RequestorLastName
        drow.Requestor_Email = CurrentOrder.RequestorEmail

        'ord ship
        drow.ShipTo_ContactFirstName = CurrentOrder.ShipContactFirstName
        drow.ShipTo_ContactLastName = CurrentOrder.ShipContactLastName
        drow.ShipTo_Name = CurrentOrder.ShipCompany
        drow.ShipTo_Address1 = CurrentOrder.ShipAddress1
        drow.ShipTo_Address2 = CurrentOrder.ShipAddress2
        drow.ShipTo_City = CurrentOrder.ShipCity
        drow.ShipTo_State = CurrentOrder.ShipState
        drow.ShipTo_ZipCode = CurrentOrder.ShipZip
        drow.ShipTo_Country = CurrentOrder.ShipCountry
        drow.PreferredShipID = CurrentOrder.ShipPrefID
        drow.ShipNote = CurrentOrder.ShipNote
        drow.RecipientEmail = CurrentOrder.RecipientEmail
        drow.RecipientPhone = CurrentOrder.RecipientPhone
        'custom 



        drow.CoverSheetID = CurrentOrder.CoverSheetID

        drow.CS_Content = CurrentOrder.CoverSheetText


        'printDataset(CurrentOrder.CstCart)

        'update custom fields
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        For Each dr As DataRow In CurrentOrder.CstCart.Rows
            qa.UpdateOrderCustomField(dr("FieldValue").ToString, CurrentOrder.OrderID, dr("FieldID").ToString)
        Next


        Dim intScheduledDelivery As Integer = GetScheduledDelivery()

        If drow("RequiredDate") Is DBNull.Value = False Then
            If intScheduledDelivery > 0 And CurrentOrder.DateDiffB(Now.Date, drow.RequiredDate) > intScheduledDelivery Then
                'qa.UpdateOrderRequiredDateStatus(ScheduledDelivery, MyData.STATUS_FUT, intOrderID)
                drow.StatusID = MyData.STATUS_FUT
            ElseIf drow.IsRequestDateNull = False AndAlso intScheduledDelivery > 0 And CurrentOrder.DateDiffB(Now.Date, drow.RequiredDate) <= intScheduledDelivery Then
                drow.StatusID = 1
                drow.ActivationDate = Now.Date
            Else
                drow.StatusID = 1
            End If
        Else
            drow.ActivationDate = Now.Date & " " & Now.TimeOfDay.ToString
            drow.StatusID = 1
        End If



        ta.Update(drow)
        UpdateItems()
        SendApprovalEmail(CurrentOrder.OrderID)
        Dim paraD As New paraData
        Dim paraOrd As New paraOrder
        paraD.CleanUp(CurrentUser, CurrentOrder)
        paraOrd.UpdatePONumber(CurrentOrder.OrderID)


        Response.Redirect("ordsummary.aspx")
    End Sub
    Private Function GetScheduledDelivery() As Integer

        Dim intPShowpScheduledDelivery As Integer = 0

        Dim ta As New dsAccessCodesTableAdapters.Customer_AccessCode1TableAdapter


        For Each dr As dsAccessCodes.Customer_AccessCode1Row In ta.GetDataByCustomerIDandCode(CurrentUser.CustomerID, CurrentUser.AccessCode)
            If dr.IsScheduledDeliveryNull Then
                intPShowpScheduledDelivery = 0
            ElseIf dr.ScheduledDelivery > 0 Then
                intPShowpScheduledDelivery = dr.ScheduledDelivery
            Else
                intPShowpScheduledDelivery = 0
            End If
        Next
        Return intPShowpScheduledDelivery
    End Function
    Sub UpdateItems()
        'save changes
        'update kits: 
        Dim rcd As System.Data.DataTable

        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        rcd = MyBase.CurrentOrder.KitCart


        'KITS:
        rcd = MyBase.CurrentOrder.KitCart

        Dim taKits As New dsOrdersTableAdapters.dsOrderKitDetailsTableAdapter
        For Each dr As dsOrders.dsOrderKitDetailsRow In taKits.GetData(CurrentOrder.OrderID)

            'fix kit qty
            If CheckIfKitInOrder(dr.KitID, rcd, dr.KitQty) = False Then
                qa.DeleteKitFromOrder(CurrentOrder.OrderID, dr.KitID)
            End If
        Next
        CheckForNewKits(rcd)

        'ITEMS:
        rcd = MyBase.CurrentOrder.ItmCart

        Dim taItems As New dsOrdersTableAdapters.dsOrderDetailsTableAdapter
        For Each dr As dsOrders.dsOrderDetailsRow In taItems.GetByOrderID(CurrentOrder.OrderID)
            If CheckIfItemInCart(dr.ID, rcd, dr.QtyOrdered) = False Then
                qa.DeleteItemFromOrder(dr.ID)
            End If
        Next

        CheckForNewItems(rcd)
    End Sub

    Function CheckIfKitInOrder(ByVal intKitID As Integer, ByVal rcd As System.Data.DataTable, ByVal intOldKitQty As Integer) As Boolean
        Dim indx, iMaxIndx As Integer
        Dim intNewQty As Integer = 0
        Dim blnExists As Boolean = False

        iMaxIndx = rcd.Rows.Count


        Do While indx < iMaxIndx
            With rcd.Rows(indx)
                If rcd.Rows(indx).RowState <> DataRowState.Deleted Then
                    'sID = .Item("KitID").ToString()
                    'sName = .Item("KitName").ToString()
                    'sDesc = .Item("KitDesc").ToString()
                    'sColor = .Item("KitColor").ToString()
                    intNewQty = Val(.Item("KitQty").ToString())


                    If .Item("kitID").ToString() = intKitID Then
                        blnExists = True
                        If intNewQty <> intOldKitQty Then
                            'UPDATES to new qty
                            Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
                            qa.UpdateKitQtyInOrder(intNewQty, .Item("kitID").ToString(), CurrentOrder.OrderID)
                        End If
                        Return blnExists
                    End If

                End If

            End With



            indx = indx + 1
        Loop


        Return blnExists
    End Function
    Function CheckIfItemInCart(ByVal intItemID As Integer, ByVal rcd As System.Data.DataTable, ByVal intOldQty As Integer) As Boolean
        Dim indx, iMaxIndx As Integer
        Dim intNewQty As Integer = 0

        Dim blnExists As Boolean = False


        iMaxIndx = rcd.Rows.Count
        iMaxIndx = rcd.Rows.Count
        indx = 0

        Do While indx < iMaxIndx
            With rcd.Rows(indx)
                If rcd.Rows(indx).RowState <> DataRowState.Deleted Then
                    'sID = .Item("ItmID").ToString()
                    'sName = .Item("ItmName").ToString()
                    'sDesc = .Item("ItmDesc").ToString()
                    'sColor = .Item("ItmColor").ToString()
                    ''MyBase.MyData.GetItmDetail(MyBase.CurrentUser.CustomerID, sID, sName, sDesc)
                    intNewQty = Val(.Item("ItmQty").ToString())
                    If .Item("ID").ToString() = intItemID Then
                        blnExists = True
                        If intNewQty <> intOldQty Then
                            'UPDATES to new qty
                            Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
                            qa.UpdateItemQtyInOrder(intNewQty, GetActualPrintMethod(qa.GetDocumentIDFromOrder_DocumentID(.Item("ID").ToString()), intNewQty), intItemID)
                        End If
                        Return blnExists
                    End If

                End If

            End With

            indx = indx + 1

        Loop

        Return blnExists
    End Function
    Sub CheckForNewKits(ByVal rcd As System.Data.DataTable)

        Dim indx, iMaxIndx As Integer
        Dim intNewQty As Integer = 0
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        Dim qaInv As New dsInventoryTableAdapters.QueriesTableAdapter
        iMaxIndx = rcd.Rows.Count
        iMaxIndx = rcd.Rows.Count
        indx = 0
        Dim decLinekitCharge As Decimal = qa.GetLineKitChg(CurrentUser.CustomerID)
        'printDataset(rcd)



        'ID
        'KitQty()
        'KitID()
        'kitname()
        'kitDesc()
        'kitColor()

        Do While indx < iMaxIndx
            With rcd.Rows(indx)

                If rcd.Rows(indx).RowState <> Data.DataRowState.Deleted Then

                    ' DataRowState.Added Then

                    If qa.CheckIfKitInOrder(CurrentOrder.OrderID, .Item("KitID").ToString()) = 0 Then


                        Dim ta As New dsOrdersTableAdapters.KitItemsTableAdapter
                        For Each dr As dsOrders.dtKitItemsRow In ta.GetKitItems(.Item("KitID").ToString()).Rows
                            'add kit item to order
                            'qa.AddNewKitItemToOrder(CurrentOrder.OrderID, dr.DocumentID, .Item("KitID").ToString(), dr.Qty, dr.Price, _
                            '                         dr.PrintMethod, 0, Now.Date, "")
                            Dim decItemPrice As Decimal = CurrentOrder.GetItemPrice(dr.DocumentID, .Item("KitQty") * qaInv.GetItmQtyInKit(.Item("KitID").ToString(), dr.DocumentID)) + decLinekitCharge
                            qa.AddNewKitItemToOrder(CurrentOrder.OrderID, dr.DocumentID, .Item("KitID").ToString(), .Item("KitQty").ToString(), decItemPrice, _
                                                                           GetActualPrintMethod(dr.DocumentID, dr.Qty), 0, Now.Date, "")


                        Next
                    End If

                End If

            End With
            indx = indx + 1
        Loop

    End Sub
    Sub printDataset(ByVal rcd As DataTable)
        Dim i As Integer = 0
        Dim indx, iMaxIndx As Integer
        Dim intNewQty As Integer = 0

        iMaxIndx = rcd.Rows.Count
        iMaxIndx = rcd.Rows.Count
        indx = 0

        Response.Write("<table border=1>")
        Response.Write("<TR>")
        For Each f As DataColumn In rcd.Columns
            Response.Write("<TD>" & f.ColumnName)
        Next

        Do While indx < iMaxIndx
            With rcd.Rows(indx)
                Response.Write("<TR>")
                For i = 0 To rcd.Columns.Count - 1
                    Response.Write("<TD>" & .Item(i).ToString())
                Next
            End With
            indx = indx + 1
        Loop
        Response.Write("</table>")
    End Sub

    Sub CheckForNewItems(ByVal rcd As System.Data.DataTable)

        Dim indx, iMaxIndx As Integer
        Dim intNewQty As Integer = 0
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter

        iMaxIndx = rcd.Rows.Count
        iMaxIndx = rcd.Rows.Count
        indx = 0


        'ID
        'ItmQty()
        'ItmID()
        'ItmName()
        'ItmDesc()
        'ItmColor()
        'Price()
        'Note()

        Do While indx < iMaxIndx
            With rcd.Rows(indx)

                If rcd.Rows(indx).RowState <> Data.DataRowState.Deleted Then

                    If qa.CheckIfItemInOrder(.Item("ItmID").ToString(), CurrentOrder.OrderID) = 0 Then

                        'add new item



                        'qa.AddNewItemToOrder(CurrentOrder.OrderID, .Item("ItmID").ToString(), .Item("ItmQty").ToString(), _
                        ' GetItemPrice(.Item("ItmID").ToString()), GetPrintMethod(.Item("ItmID").ToString()), 0, Now.Date, "")

                        qa.AddNewItemToOrder(CurrentOrder.OrderID, .Item("ItmID").ToString(), .Item("ItmQty").ToString(), _
                         CurrentOrder.GetItemPrice(.Item("ItmID").ToString(), .Item("ItmQty").ToString()), GetActualPrintMethod(.Item("ItmID").ToString(), .Item("ItmQty").ToString()), 0, Now.Date, "")

                    End If

                End If
            End With
            indx = indx + 1
        Loop
    End Sub
    'Function GetItemPrice(ByVal intItemID As Integer) As Decimal

    '    Dim decPrice As Decimal = 0
    '    Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
    '    Try
    '        decPrice = qa.GetItemPrice(intItemID)
    '    Catch ex As Exception

    '    End Try

    '    Return decPrice
    'End Function
    Function GetPrintMethodOLD(ByVal intItemID As Integer) As Integer

        Dim intPrintMethod As Integer = 0
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        intPrintMethod = qa.GetItemPrintMethod(CurrentUser.CustomerID, intItemID)

        Return 0
    End Function

    Function GetActualPrintMethod(ByVal intItemID As Integer, ByVal intQty As Integer) As Integer
        Const COL_QTY = 1
        Const COL_ItmID = 2

        'Dim objDT As System.Data.DataTable
        Dim indx As Integer : indx = 0
        Dim iMaxIndx As Integer : iMaxIndx = 0
        Dim bSuccess As Boolean = False

        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String = String.Empty
        Dim sSQLE As String = String.Empty

        Dim sCurrentTime As String = String.Empty

        Dim lItmID As Long = 0
        'Dim iQty As Integer = 0
        Dim lQty As Long = 0
        Dim iItmType As Integer = 0
        Dim lQtyFillRemain As Long = 0
        'Dim iQtyToPrint As Integer = 0
        'Dim iQtyToFill As Integer = 0
        Dim lQtyToPrint As Long = 0
        Dim lQtyToFill As Long = 0
        Dim iQtyPerPkA As Integer = 0
        Dim iQtyPerPkB As Integer = 0
        'Dim iQtyPerCtn As Integer = 0
        Dim lQtyPerCtn As Long = 0
        Dim bPrePack As Boolean = False
        'Dim iCtnCnt As Integer = 0
        Dim lCtnCnt As Long = 0
        Dim iQtyPerPull As Integer = 0
        Dim gFillPrice As Single = 0


        Dim intPrintMethod As Integer = 0


        lItmID = intItemID 'objDT.Rows(indx).Item(COL_ItmID)

        lQty = intQty 'objDT.Rows(indx).Item(COL_QTY)

        If (lItmID > 0) And (lQty > 0) Then
            iItmType = 0
            lQtyFillRemain = 0
            lQtyToPrint = 0
            lQtyToFill = 0


            GetDocAvailability(MyData, lItmID, iItmType, lQtyFillRemain)

            If (iItmType = MyData.DT_PRINTONLY) Then
                'iQtyToPrint = iQty
                lQtyToPrint = lQty
            ElseIf (iItmType = MyData.DT_FILLONLY) Then
                'iQtyToFill = iQty
                lQtyToFill = lQty
            Else
                'If (iQty <= lQtyFillRemain) Then
                '  iQtyToFill = iQty
                If (lQty <= lQtyFillRemain) Then
                    lQtyToFill = lQty
                Else
                    'Do not want orders split between part fill/print - all or nothing
                    'iQtyToFill = 0
                    'iQtyToPrint = iQty
                    lQtyToFill = 0
                    lQtyToPrint = lQty
                End If
            End If

            If (lQtyToPrint > 0) Then

                intPrintMethod = MyData.PM_PRINT '& " AS PrintMethod, #" & Now() & "# AS ModifiedDate " & _

            End If

            'Add Fulfillment Documents
            'If (iQtyToFill > 0) Then
            If (lQtyToFill > 0) Then
                'Determine if Prepackaged Pick Charge or Non-PrePackaged Pick Charge to be used
                iQtyPerPkA = 0
                iQtyPerPkB = 0
                iQtyPerPull = 0
                lQtyPerCtn = 0
                gFillPrice = 0

                sSQLR = "SELECT Customer_Document_Fill.QtyPerPkA, Customer_Document_Fill.QtyPerPkB, Customer_Document_Fill.QtyPerPull, Customer_Document_Fill.QtyPerCtn, Customer_Document_Fill.Price " & _
                        "FROM Customer_Document_Fill WHERE DocumentID = " & lItmID
                MyData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
                If rcdR.Read() Then
                    'lOrderID = rcdR("OrderID")
                    iQtyPerPkA = rcdR("QtyPerPkA")
                    iQtyPerPkB = rcdR("QtyPerPkB")
                    iQtyPerPull = rcdR("QtyPerPull")
                    lQtyPerCtn = rcdR("QtyPerCtn")
                    'mw - 05-04-2009 - start considering Qty Per Pull
                    'gFillPrice = rcdR("Price")
                    gFillPrice = rcdR("Price") * iQtyPerPull
                End If
                MyData.ReleaseReader2(cnnR, cmdR, rcdR)

                'bPrePack = (iQty = 1) Or (iQty = iQtyPerPkA) Or (iQty = iQtyPerPkB)
                bPrePack = (lQty = 1) Or (lQty = iQtyPerPkA) Or (lQty = iQtyPerPkB)

                'determine how many cartons to charge for (do not charge for 1 carton - only over 1)
                If (lQtyPerCtn > 0) Then
                    'mw - 07-20-2009
                    ''mw - 05-04-2009
                    ''iCtnCnt = (iQty \ iQtyPerCtn) - 1
                    ''iCtnCnt = ((iQty * iQtyPerPull) \ iQtyPerCtn) - 1
                    'iCtnCnt = ((lQty * iQtyPerPull) \ iQtyPerCtn) - 1
                    'iCtnCnt = IIf(iCtnCnt < 0, 0, iCtnCnt)
                    lCtnCnt = ((lQty * iQtyPerPull) \ lQtyPerCtn) - 1
                    lCtnCnt = IIf(lCtnCnt < 0, 0, lCtnCnt)
                End If
                'If (iCtnCnt > 0) Then
                'sPrice = sPrice + ((sCtnChg * iCtnCnt) / iQty)

                If (bPrePack = True) Then

                    intPrintMethod = MyData.PM_FULFILL '& " AS PrintMethod, #" & Now() & "# AS ModifiedDate, " & _


                Else

                    intPrintMethod = MyData.PM_FULFILL '& " AS PrintMethod, #" & Now() & "# AS ModifiedDate, " & _
                End If

            End If
        End If


        Return intPrintMethod

        'mw - 06-01-207 - Test


    End Function


    Private Function GetDocAvailability(ByRef myData As paraData, ByVal lItmID As Long, ByRef iItmType As Integer, ByRef lQtyFillRemain As Long) As Boolean
        Dim bSuccess As Boolean : bSuccess = False
        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String = String.Empty
        Dim lQtyOH As Long = 0
        'Dim iQtyToFill As Integer = 0
        Dim lQtyToFill As Long = 0
        Dim iQtyPerPull As Integer = 0
        Dim iStockWarn As Integer = 0
        Dim QTY_XTRA As Integer = 0

        Try
            'If (cnnE Is Nothing) Then
            'LogActivity("      Attempt to Reset Cnn - GetDocAvailability", msLogFile)
            'myData.GetConnection(cnnE)
            'End If

            iItmType = -1
            lQtyFillRemain = 0

            sSQLR = "SELECT Customer_Document.PrintMethod " & _
                    "FROM Customer_Document WHERE ID = " & lItmID
            myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
            If rcdR.Read() Then
                iItmType = rcdR("PrintMethod")
            End If
            myData.ReleaseReader2(cnnR, cmdR, rcdR)
            If (iItmType <> myData.DT_PRINTONLY) Then
                sSQLR = "SELECT Customer_Document_Fill.QtyOH, Customer_Document_Fill.QtyPerPull, Customer_Document_Fill.StockWarn " & _
                        "FROM Customer_Document_Fill WHERE DocumentID = " & lItmID
                myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
                If rcdR.Read() Then
                    lQtyOH = rcdR("QtyOH")
                    iQtyPerPull = rcdR("QtyPerPull")
                    iStockWarn = rcdR("StockWarn")
                End If
                myData.ReleaseReader2(cnnR, cmdR, rcdR)

                'Consider if PerPull > 1
                If (iQtyPerPull > 0) Then lQtyOH = lQtyOH / iQtyPerPull

                'Consider having backup of at least an extra ...
                'mw - 09-13-2007 - ab use from item
                'QTY_XTRA = ConfigurationSettings.AppSettings("SafetyInventoryLevel")
                QTY_XTRA = iStockWarn
                '
                If (iQtyPerPull > 0) And (QTY_XTRA > 0) Then lQtyOH = lQtyOH - (QTY_XTRA / iQtyPerPull)

                sSQLR = "SELECT SUM(QtyOrdered) AS QtyToFill " & _
                        "FROM Order_Document INNER JOIN [Order] ON Order_Document.OrderID = [Order].ID " & _
                        "WHERE [Order].StatusID <> 3 " & _
                        " AND Order_Document.DocumentID = " & lItmID & " AND Order_Document.PrintMethod = " & myData.PM_FULFILL & _
                        " AND (Order_Document.Status<>" & myData.STATUS_PRT & ")"
                myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
                If rcdR.Read() Then
                    'iQtyToFill = Val(rcdR("QtyToFill") & "")
                    lQtyToFill = Val(rcdR("QtyToFill") & "")
                    'If (Len(iQtyToFill) = 0) Then iQtyToFill = 0
                End If
                myData.ReleaseReader2(cnnR, cmdR, rcdR)

                'If (lQtyOH > iQtyToFill) Then
                '  lQtyFillRemain = lQtyOH - iQtyToFill
                If (lQtyOH > lQtyToFill) Then
                    lQtyFillRemain = lQtyOH - lQtyToFill
                End If
            End If

            bSuccess = True

        Catch ex As Exception
            'LogError("GetAvailability >> " & ex.Message.ToString & " : " & sSQLR & msNewLine)
            myData.ReleaseReader2(cnnR, cmdR, rcdR)
        End Try

        Return bSuccess
    End Function

#End Region
    Protected Sub cmdBackCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBackCancel.Click, cmdBack.Click
        Dim paraD As New paraData
        paraD.CleanUp(CurrentUser, CurrentOrder)
        Response.Redirect("ordsummary.aspx")
    End Sub
    Protected Sub cmdCancelOrder_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdCancelOrder.Click
        Dim ta As New dsOrdersTableAdapters.OrderTableAdapter
        Dim drow As dsOrders.OrderRow
        drow = ta.GetOrderDataByID(CurrentOrder.OrderID).Rows(0)
        drow.StatusID = 4

        SendCancelEmails(CurrentOrder.OrderID)

        ta.Update(drow)


        Dim paraD As New paraData
        paraD.CleanUp(CurrentUser, CurrentOrder)
        Response.Redirect("ordsummary.aspx")
    End Sub


    Sub SendCancelEmails(ByVal intOrderID As Integer)
        Dim oOrder As New paraOrder
        Dim sBody As String = ""
        Dim myMail As Mail
        Dim sTo As String = ""
        Dim sSubject As String = ""
        Dim qa As New dsNotificationsTableAdapters.QueriesTableAdapter


        Dim ta As New dsAccessCodesTableAdapters.Customer_AccessCode1TableAdapter
        Dim dr As dsAccessCodes.Customer_AccessCode1Row = ta.GetDataByAccessCodeID(CurrentOrder.AccessCodeID).Rows(0)



        'Message - Start
        myMail = New Mail

        sSubject = "Order Request #" & intOrderID & " has been cancelled."

        sBody = "<CENTER>Order Request # " & intOrderID & " has been cancelled." & _
                "</CENTER>"

        'check for customer level permission to send email:
        If (Mid(dr.WebSet, 10, 1) >= 1) And CBool(qa.CheckForAuthorizationApprovalByCustomerID(CurrentUser.CustomerID, 4)) Then
            sTo = CurrentOrder.RequestorEmail

            If (Len(sTo) > 0) Then
                myMail.SendMessage(sTo, ConfigurationSettings.AppSettings("DefaultEmailAddress"), _
                                   sSubject, sBody, "", ConfigurationSettings.AppSettings("DefaultEmailCopy"), , , True)
                myMail = Nothing


            End If
        End If

        SendMatrixApprovalNotifications(sBody, sSubject)
    End Sub

    Private Sub SendApprovalEmail(ByVal intOrderID As Integer)

        Dim oOrder As New paraOrder
        Dim sBody As String = ""
        Dim myMail As Mail
        Dim sTo As String = ""
        Dim sSubject As String = ""
        Dim qa As New dsNotificationsTableAdapters.QueriesTableAdapter


        Dim ta As New dsAccessCodesTableAdapters.Customer_AccessCode1TableAdapter
        Dim dr As dsAccessCodes.Customer_AccessCode1Row = ta.GetDataByAccessCodeID(CurrentOrder.AccessCodeID).Rows(0)

        'check for customer level permission to send email:

        'Message - Start
        myMail = New Mail

        sSubject = "Order Request #" & intOrderID & " has been approved."

        sBody = "<CENTER>"
        sBody = sBody & "Order Request # " & intOrderID & " has been approved."

        sBody = sBody & "." & oOrder.GetEmailOrderBody(intOrderID)
        sBody = sBody & "<br>" & MSG_RESP & _
                "</CENTER>"


        If (Mid(dr.WebSet, 10, 1) >= 1) And CBool(qa.CheckForAuthorizationApprovalByCustomerID(CurrentUser.CustomerID, 4)) Then

            sTo = CurrentOrder.RequestorEmail

            If (Len(sTo) > 0) Then
                myMail.SendMessage(sTo, ConfigurationSettings.AppSettings("DefaultEmailAddress"), _
                                   sSubject, sBody, "", ConfigurationSettings.AppSettings("DefaultEmailCopy"), , , True)
                myMail = Nothing

            End If
        End If

        SendMatrixApprovalNotifications(sBody, sSubject)
    End Sub



    Sub SendMatrixApprovalNotifications(ByVal sBody As String, ByVal sSubject As String)

        Dim o As New paraOrder
        Dim msLogFile As String
        msLogFile = ConfigurationSettings.AppSettings("LogFolder") & "Ordr-" & Format(Now, "yyyyMMdd") & ".log"
        o.LogActivity("  SendMatrixApprovalNotifications for order# " & CurrentOrder.OrderID, msLogFile)


        Dim myMail As Mail
        myMail = New Mail




        '1	Primary Customer Contact
        '2	Secondary Customer Contact
        '3	Billing Group Contact
        '4: Requestor()
        '5	Para Sales Contact



        Dim strEmailTo As String = ""

        Dim ta As New dsNotificationsTableAdapters.DataTable1TableAdapter
        For Each dr As dsNotifications.DataTable1Row In ta.GetDataByCustomerID(CurrentUser.CustomerID).Rows
            Select Case dr.ContactType
                Case 1
                    If dr.AuthorizationApprovalNotify Then
                        strEmailTo = o.GetEmailForNotificationType(dr.ContactType, CurrentUser.CustomerID)
                        o.LogActivity("  SendMatrixApprovalNotifications - Primary - " & strEmailTo & " - Subj - " & sSubject, msLogFile)
                        myMail.SendMessage(strEmailTo, ConfigurationSettings.AppSettings("DefaultEmailAddress"), _
                                  sSubject, sBody, "", ConfigurationSettings.AppSettings("DefaultEmailCopy"), , , True)

                    End If
                Case 2
                    If dr.AuthorizationApprovalNotify Then
                        strEmailTo = o.GetEmailForNotificationType(dr.ContactType, CurrentUser.CustomerID)
                        o.LogActivity("  SendMatrixApprovalNotifications - Secondary - " & strEmailTo & " - Subj - " & sSubject, msLogFile)
                        myMail.SendMessage(strEmailTo, ConfigurationSettings.AppSettings("DefaultEmailAddress"), _
                                 sSubject, sBody, "", ConfigurationSettings.AppSettings("DefaultEmailCopy"), , , True)
                    End If
                Case 3
                Case 4
                Case 5
                    If dr.AuthorizationApprovalNotify Then

                        strEmailTo = o.GetEmailForNotificationType(dr.ContactType, CurrentUser.CustomerID)
                        o.LogActivity("  SendMatrixApprovalNotifications - Para Sales Contact - " & strEmailTo & " - Subj - " & sSubject, msLogFile)
                        myMail.SendMessage(strEmailTo, ConfigurationSettings.AppSettings("DefaultEmailAddress"), _
                                 sSubject, sBody, "", ConfigurationSettings.AppSettings("DefaultEmailCopy"), , , True)
                    End If
            End Select
        Next

        myMail = Nothing
    End Sub

End Class
