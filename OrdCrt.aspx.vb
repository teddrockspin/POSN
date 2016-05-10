''''''''''''''''''''
'mw - 08-12-2008
'mw - 05-23-2008
''''''''''''''''''''


Imports System.Text
Imports System.IO
Imports Telerik.Web
Imports Telerik.Web.UI

Partial Class OrdCrt

    Inherits paraPageBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents pnlKit As System.Web.UI.WebControls.Panel
    Protected WithEvents cmdShop As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblLILim As System.Web.UI.WebControls.Label
    Protected WithEvents lblLIReq As System.Web.UI.WebControls.Label
    Protected WithEvents lblORLim As System.Web.UI.WebControls.Label
    Protected WithEvents lblORReq As System.Web.UI.WebControls.Label
    Protected WithEvents tlbITtl As System.Web.UI.WebControls.Table

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Public ReadOnly Property AjaxRequest As Boolean
        Get
            If String.IsNullOrEmpty(Request.Params("ajax")) Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()

        MyBase.Page_Init(sender, e)

        If AjaxRequest Then
            If Request.Params("type") = "validateitems" Then
                Response.Clear()
                Response.ContentType = "text/HTML"
                Response.Write(GetAddToCartMessage(Request.Params("qty")))

                Response.End()

            End If
        End If

        LoadData()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        PageTitle = "Shopping Cart"
        'MyBase.Page_Load(sender, e)
        'MyBase.PageMessage = "Adjust quantities or remove an item, then click update.  Click Checkout to proced with the order."
        MyBase.PageMessage = "Adjust quantities or remove an item, then click update.  Click Continue with Checkout to proceed with the order."

        'If (MyBase.UState = MyBase.MyData.STATE_LoggedOut) Then
        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            'If (MyBase.UState = MyBase.MyData.STATE_None) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri.ToString

            If CurrentOrder.EditOrder = True Then
                cmdEditBack.Visible = True
                cmdNext.Visible = False

                If CurrentOrder.StatusID <> 6 Then
                    cmdEditBack.Visible = False
                    cmdUpdateItem.Visible = False
                    cmdUpdateKit.Visible = False
                End If

            End If


            MyBase.CurrentOrder.VisitMark(paraData.PRO_CRT)
            If (MyBase.CurrentOrder.VisitAll = True) Then
                Dim pScript As New StringBuilder
                pScript.Append("<script>" & _
                "function CheckSubmit () " & _
                "{ " & _
                "  document.frmOrdCrt.txtSubmit.value='Y';" & _
                "  return true;" & _
                "} " & _
                "</script>")
                Me.RegisterStartupScript("Startup", pScript.ToString)
                cmdNext.Attributes.Add("onclick", "return CheckSubmit(); ")
            End If

           

            ValidItemData()
            ValidKitData()
        End If

        ' Response.Write(Session.SessionID & "<BR>")
    End Sub

    Private Function BuildSQL_Itm(Optional ByVal sKeyword1IDs As String = "", Optional ByVal sKeyword2IDs As String = "", Optional ByVal sKeyword3IDs As String = "", Optional ByVal sKeyword4IDs As String = "", _
                                Optional ByVal sSearch As String = "", Optional ByVal sIDs As String = "") As String
        Dim bShowActiveOnly As Boolean : bShowActiveOnly = True
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sFrom As String = String.Empty
        Dim sKey1From As String = String.Empty
        Dim sKey2From As String = String.Empty
        Dim sKey3From As String = String.Empty
        Dim sKey4From As String = String.Empty
        Dim sKeyFrom As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty
        Dim qa As New dsAccessCodesTableAdapters.QueriesTableAdapter

        sSelect = "SELECT DISTINCT d.ID, d.[Status], d.ReferenceNo AS RefNo, d.[Description] AS RefName, " & _
                  "CASE d.[Status] WHEN " & MyBase.MyData.STATUS_BACK & " THEN 'True' ELSE 'False' End AS OnBackOrder, " & _
                  "CASE LEN(d.Prefix) WHEN 0 THEN 'ZZZZ' ELSE d.Prefix END as Prefix1, " & _
                  "OrderQuantityLimit, " & _
                  "ISNULL((Select (QtyOHLocP + QtyOHLocS + QtyOHLocW) from Customer_Document_Fill where DocumentID = d.ID), 999999) as CurrentStock, " & _
                  "d.PrintMethod "
        sFrom = "FROM Customer_Document d " & _
                "LEFT OUTER JOIN Customer_Document_DownloadableConnection ON d.ID = Customer_Document_DownloadableConnection.DocumentID_Downloadable " & _
                "INNER JOIN Customer_Document_Keyword dk ON d.ID = dk.DocumentID "

        If qa.CheckIfAllowedPurchaseDownloadableItems(CurrentUser.AccessCodeID) Then
            sWhere = " (d.CustomerID = " & MyBase.CurrentUser.CustomerID & " AND Customer_Document_DownloadableConnection.DocumentID_Main is null)"
        Else
            sWhere = " (d.CustomerID = " & MyBase.CurrentUser.CustomerID & " and d.PrintMethod<>3)"
        End If

        'By Keyword Tags
        If (Len(sKeyword1IDs) > 0) Or (Len(sKeyword2IDs) > 0) Or (Len(sKeyword3IDs) > 0) Or (Len(sKeyword4IDs) > 0) Then
            If (Len(sKeyword1IDs) > 0) Then
                sKey1From = "(SELECT DocumentID FROM Customer_Document_Keyword WHERE KeywordID IN (" & sKeyword1IDs & ")) K1 "
            Else
                sKey1From = "(SELECT DocumentID FROM Customer_Document_Keyword) K1 "
            End If
            If (Len(sKeyword2IDs) > 0) Then
                sKey2From = "(SELECT DocumentID FROM Customer_Document_Keyword WHERE KeywordID IN (" & sKeyword2IDs & ")) K2 "
            Else
                sKey2From = "(SELECT DocumentID FROM Customer_Document_Keyword) K2 "
            End If
            If (Len(sKeyword3IDs) > 0) Then
                sKey3From = "(SELECT DocumentID FROM Customer_Document_Keyword WHERE KeywordID IN (" & sKeyword3IDs & ")) K3 "
            Else
                sKey3From = "(SELECT DocumentID FROM Customer_Document_Keyword) K3 "
            End If
            If (Len(sKeyword4IDs) > 0) Then
                sKey4From = "(SELECT DocumentID FROM Customer_Document_Keyword WHERE KeywordID IN (" & sKeyword4IDs & ")) K4 "
            Else
                sKey4From = "(SELECT DocumentID FROM Customer_Document_Keyword) K4 "
            End If
            sKeyFrom = "SELECT DISTINCT K1.DocumentID FROM " & _
                        sKey1From & " INNER JOIN " & _
                        "(" & sKey2From & " INNER JOIN " & _
                        "(" & sKey3From & " INNER JOIN " & _
                        "" & sKey4From & _
                        " ON K3.DocumentID = K4.DocumentID " & _
                        ")ON K2.DocumentID = K3.DocumentID " & _
                        ")ON K1.DocumentID = K2.DocumentID " & _
                        ""
            sFrom = "FROM Customer_Document d " & _
                "LEFT OUTER JOIN Customer_Document_DownloadableConnection ON d.ID = Customer_Document_DownloadableConnection.DocumentID_Downloadable " & _
                "INNER JOIN (" & sKeyFrom & ") dk ON d.ID = dk.DocumentID "
        End If

        If (sSearch.Length > 0) Then
            sWhere = sWhere & "AND ( (d.ReferenceNo LIKE " & MyBase.MyData.SQLString("%" & sSearch & "%") & ") OR (d.[Description] LIKE " & MyBase.MyData.SQLString("%" & sSearch & "%") & ")) "
        End If

        If (bShowActiveOnly) Then
            sWhere = sWhere & " AND (d.[Status] >" & MyData.STATUS_INAC & ") "
        End If

        If sIDs.Length > 0 Then
            sWhere &= " AND (d.ID in (" & sIDs & "))"
        End If

        If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

        sOrder = sOrder & " ORDER BY Prefix1, d.ReferenceNo "

        sSQL = sSelect + sFrom & sWhere + sOrder

        'Response.Write(sSQL)

        Return sSQL
    End Function

    Private Function GetAddToCartMessage(ByVal QtyToOrder As Integer) As String
        Dim sScript, sSQL As String
        Dim cnn As SqlClient.SqlConnection
        Dim cmd As SqlClient.SqlCommand
        Dim ds As DataSet
        Dim dRow As DataRow
        Dim BackOrdered As Boolean = False
        Dim PartNumbers As String = ""
        Dim rcd As System.Data.DataTable
        Dim iMaxIndx As Integer = 0
        Dim indx As Integer = 0
        Dim sID As String
        Dim cartItems As String
        Dim sName As String
        Dim lQty As Long
        Dim sQty As String = String.Empty
        Dim intI As Integer = 0
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter

        rcd = MyBase.CurrentOrder.ItmCart
        iMaxIndx = rcd.Rows.Count

        Do While indx < iMaxIndx


            If rcd.Rows(indx).RowState <> DataRowState.Deleted Then
                Dim intPrintMethod As Integer = 0

                intPrintMethod = qa.getDocumentPrintMethod(rcd.Rows(indx).Item("ItmID").ToString())

                If intPrintMethod <> 3 Then
                    lQty = Val(rcd.Rows(indx).Item("ItmQty").ToString())
                    'intDocStatus = qa.GetDocumentStatusIDByDocumentID(rcd.Rows(indx).Item("ItmID").ToString())
                    With rcd.Rows(indx)


                        sID = .Item("ItmID").ToString()
                        sName = .Item("ItmName").ToString()
                        cartItems &= sID & ","
                      
                        'MyBase.MyData.GetItmDetail(MyBase.CurrentUser.CustomerID, sID, sName, sDesc)


                        intI += 1

                    End With
                End If
            End If
            indx = indx + 1
        Loop

        If cartItems <> "" then
            cartItems = cartItems.Remove(cartItems.Length -1)
        End If

        sSQL = BuildSQL_Itm("", "", "", "", "", cartItems)
        MyBase.MyData.GetDataSetSql(cnn, cmd, ds, sSQL)

        For Each dRow In ds.Tables(0).Rows
            If CBool(dRow("OnBackOrder")) = True Or (CInt(dRow("CurrentStock") < QtyToOrder And CInt(dRow("PrintMethod")) = 0)) Then
                BackOrdered = True
                PartNumbers &= dRow("RefNo") & ","
            End If
        Next

        If PartNumbers <> "" Then
            PartNumbers = PartNumbers.Remove(PartNumbers.Length - 1)
        End If

        If BackOrdered Then ' if one or more selected items is backordered then use this script
            sScript = "return confirm('Of the items you are about to update in the current order, the following are currently on Backorder or will be Backordered as a result of your request. Part Number(s) (" & PartNumbers & "). All items will be updated in your current order. Do you wish to continue?');"
        Else
            sScript = "return confirm('You are about to update items in the current order.  Do you wish to continue?');"
        End If

        Return sScript
    End Function

    Private Function LoadData() As Boolean
        Dim objDT As System.Data.DataTable
        Dim bSuccess As Boolean = False
        GetShowStockSetting()
        Try
            LoadControls()
            bSuccess = True
        Catch ex As Exception
            Session("PMsg") = "Unable to Load Data..."
        End Try

        Return (bSuccess)
    End Function

    Private Function LoadControls() As Boolean
        Dim bRequiresLI As Boolean = False
        Dim bRequiresOR As Boolean = False

        LoadItems()
        LoadDigitalItems()
        If (MyBase.CurrentUser.CanViewKits = True) Then
            LoadKits()
        End If
        LoadNotes()
        cmdKit.Visible = (MyBase.CurrentUser.CanViewKits = True)
        tblKTtl.Visible = (MyBase.CurrentUser.CanViewKits = True)
        tblKQty.Visible = (MyBase.CurrentUser.CanViewKits = True)
        tblKUp.Visible = (MyBase.CurrentUser.CanViewKits = True)
    End Function

    Private Function LoadItems() As Boolean
        Dim tRow As TableRow
        Dim tCell As TableCell
        Dim lLabel As New Label
        Dim tText As New TextBox
        Dim vCmp As New CompareValidator
        Dim cChk As New CheckBox
        Dim sValID As String
        Dim bButton As ImageButton

        Dim rcd As System.Data.DataTable
        Dim indx As Integer
        Dim iMaxIndx As Integer
        Dim iItemsCount As Integer = GetMaxIndxNonDownloadable()
        Dim bSuccess As Boolean = False

        Dim sID As String
        Dim sName As String
        Dim sDesc As String
        Dim sColor As String
        Dim lQty As Long
        Dim sQty As String = String.Empty
        Dim intI As Integer = 0
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
        Dim intDocStatus As Integer = 0
        Dim sScript As String

        tblITtl.Rows.Clear()
        tblIQty.Rows.Clear()

        rcd = MyBase.CurrentOrder.ItmCart
        iMaxIndx = rcd.Rows.Count
        indx = 0

        'Title
        'No Items Selected
        tRow = New TableRow
        tCell = New TableCell
        tCell.HorizontalAlign = HorizontalAlign.Center
        tCell.VerticalAlign = VerticalAlign.Middle
        tCell.ColumnSpan = 6
        lLabel = New Label
        lLabel.CssClass = "lblTitle"
        lLabel.ID = "lblITitle"
        If (iItemsCount = 0) Then
            lLabel.Text = "No Ordered Items"
        Else
            lLabel.Text = "Ordered Items"
        End If
        tCell.Controls.Add(lLabel)
        tRow.Cells.Add(tCell)
        'tblIQty.Rows.Add(tRow)
        tblITtl.Rows.Add(tRow)

        'Space
        tRow = New TableRow
        tCell = New TableCell
        tCell.Text = "&nbsp;"
        tRow.Cells.Add(tCell)
        'tblIQty.Rows.Add(tRow)
        tblITtl.Rows.Add(tRow)

        'Remove - Label
        If (iItemsCount > 0) Then
            tRow = New TableRow
            tCell = New TableCell : tRow.Cells.Add(tCell)
            tCell = New TableCell : tRow.Cells.Add(tCell)
            tCell = New TableCell : tRow.Cells.Add(tCell)
            tCell = New TableCell : tRow.Cells.Add(tCell)
            tCell = New TableCell
            tCell.HorizontalAlign = HorizontalAlign.Center
            tCell.VerticalAlign = VerticalAlign.Middle
            lLabel = New Label
            lLabel.CssClass = "lblTitle"
            lLabel.ID = "lblIRemove"
            lLabel.Text = "Remove"
            tCell.Controls.Add(lLabel)
            tRow.Cells.Add(tCell)
            tCell = New TableCell : tRow.Cells.Add(tCell)

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



            tblIQty.Rows.Add(tRow)
        End If

        Do While indx < iMaxIndx


            If rcd.Rows(indx).RowState <> DataRowState.Deleted Then
                Dim intPrintMethod As Integer = 0

                intPrintMethod = qa.getDocumentPrintMethod(rcd.Rows(indx).Item("ItmID").ToString())

                If intPrintMethod <> 3 Then
                    lQty = Val(rcd.Rows(indx).Item("ItmQty").ToString())
                    intDocStatus = qa.GetDocumentStatusIDByDocumentID(rcd.Rows(indx).Item("ItmID").ToString())

                    sScript = GetAddToCartMessage(lQty)

                    With rcd.Rows(indx)


                        sID = .Item("ItmID").ToString()
                        sName = .Item("ItmName").ToString()
                        sDesc = .Item("ItmDesc").ToString()
                        sColor = .Item("ItmColor").ToString()
                        'MyBase.MyData.GetItmDetail(MyBase.CurrentUser.CustomerID, sID, sName, sDesc)


                        intI += 1

                    End With
                    tRow = New TableRow

                    'ID - 0
                    tCell = New TableCell
                    tCell.Visible = False
                    tCell.Text = sID
                    tRow.Cells.Add(tCell)

                    'Label - 1
                    tCell = New TableCell
                    tCell.HorizontalAlign = HorizontalAlign.Left
                    tCell.VerticalAlign = VerticalAlign.Middle
                    lLabel = New Label
                    If (InStr(sColor, paraData.COLOR_INACTIVE) > 0) Or intDocStatus = 0 Then
                        lLabel.CssClass = "lblXLongInActive"
                    ElseIf (InStr(sColor, paraData.COLOR_BACK) > 0) Or intDocStatus = 2 Then
                        lLabel.CssClass = "lblXLongBack"
                    Else
                        lLabel.CssClass = "lblXLong"
                    End If
                    lLabel.ID = "lblI" & indx
                    lLabel.Text = sName & " : " & sDesc
                    tCell.Controls.Add(lLabel)
                    tRow.Cells.Add(tCell)

                    'Textbox - 2
                    tCell = New TableCell
                    tCell.HorizontalAlign = HorizontalAlign.Right
                    tCell.VerticalAlign = VerticalAlign.Middle
                    sValID = "txtI" & indx
                    tText = New TextBox
                    tText.CssClass = "txtNumber"
                    tText.ID = sValID
                    tText.Text = lQty
                    tText.MaxLength = 9

                    If intPrintMethod = 3 Then
                        tText.ReadOnly = True
                    End If
                    tCell.Controls.Add(tText)
                    tRow.Cells.Add(tCell)

                    'Validator - 3
                    If (MyBase.CurrentUser.CanExtendQtyLI) Then
                        sQty = 100000000
                    Else
                        sQty = MyBase.CurrentOrder.MaxQtyPerLineItem
                    End If
                    tCell = New TableCell
                    vCmp = New CompareValidator
                    vCmp.ID = "vI" & indx
                    vCmp.ControlToValidate = sValID
                    vCmp.Display = ValidatorDisplay.Dynamic
                    vCmp.ErrorMessage = "<p>Quantity cannot exceed " & sQty & "</p>"
                    vCmp.[Operator] = ValidationCompareOperator.LessThanEqual
                    vCmp.Type = ValidationDataType.Integer
                    vCmp.ValueToCompare = sQty
                    vCmp.CssClass = "lblInactive"

                    If CurrentUser.CanExtendQtyLI = False Then
                        'tCell.Controls.Add(vCmp)
                    End If


                    tRow.Cells.Add(tCell)

                    'Space - 4
                    tCell = New TableCell
                    tCell.Text = "&nbsp"
                    tRow.Cells.Add(tCell)

                    'Checkbox - 5
                    tCell = New TableCell
                    tCell.HorizontalAlign = HorizontalAlign.Center
                    tCell.VerticalAlign = VerticalAlign.Middle
                    cChk = New CheckBox
                    cChk.ID = "chkI" & indx
                    tCell.Controls.Add(cChk)
                    tRow.Cells.Add(tCell)

                    'Space - 6
                    tCell = New TableCell
                    tCell.Text = "&nbsp"
                    tRow.Cells.Add(tCell)



                    If CurrentUser.ShowPrice Then
                        Dim decItmPrice As Decimal = CurrentOrder.GetItemPrice(rcd.Rows(indx).Item("ItmID"), lQty)
                        tCell = New TableCell
                        tCell.Text = "<span class=""lblPrice"">$" & String.Format("{0:N5}", decItmPrice) & "</span>"
                        tCell.HorizontalAlign = HorizontalAlign.Right
                        tRow.Cells.Add(tCell)
                        tCell = New TableCell
                        tCell.HorizontalAlign = HorizontalAlign.Right
                        tCell.Text = "<span class=""lblPrice"">" & Format(decItmPrice * lQty, "Currency") & "</span>"
                        tRow.Cells.Add(tCell)
                    Else

                        tCell = New TableCell
                        tCell.Text = ""
                        tRow.Cells.Add(tCell)
                        tCell = New TableCell
                        tCell.Text = ""
                        tRow.Cells.Add(tCell)
                    End If

                    'upload document file:
                    If qa.CheckIfItemRequiresUpload(rcd.Rows(indx).Item("ItmID")) And CurrentOrder.EditOrder = False Then

                        If CheckIfFileUploaded(rcd.Rows(indx).Item("ItmID")) = False Then
                            Dim fileup As New RadUpload 'FileUpload
                            'fileup.TargetFolder = "~/Uploads"
                            fileup.ControlObjectsVisibility = ControlObjectsVisibility.ClearButtons
                            fileup.ID = "FileUpload" & rcd.Rows(indx).Item("ItmID")

                            tCell = New TableCell
                            tCell.ID = "UploadCell" & rcd.Rows(indx).Item("ItmID")
                            tCell.Controls.Add(fileup)
                            Dim strUploadInstructions As String = qa.GetUploadInstructions(rcd.Rows(indx).Item("ItmID"))
                            If strUploadInstructions <> "" Then
                                Dim lb As New LinkButton
                                lb.ID = "lb" & rcd.Rows(indx).Item("ItmID")
                                lb.OnClientClick = "return openDialog('" & GetJSString(strUploadInstructions) & "');"
                                lb.Text = "view upload file instructions"
                                lb.ToolTip = "view upload file instructions"
                                tCell.Controls.Add(lb)
                            End If


                            tRow.Cells.Add(tCell)
                            dUploadMessage.Visible = True
                            'cmdNext.Visible = False
                        Else
                            tCell = New TableCell
                            lLabel = New Label
                            lLabel.Text = "FILE UPLOADED"
                            lLabel.CssClass = "lblUploaded"
                            tCell.Controls.Add(lLabel)
                            tRow.Cells.Add(tCell)
                        End If
                    End If
                    '---


                    tblIQty.Rows.Add(tRow)
                End If
            End If
            indx = indx + 1
        Loop

        'Space
        tRow = New TableRow
        tCell = New TableCell
        tCell.Text = "&nbsp;"
        tRow.Cells.Add(tCell)
        tblIQty.Rows.Add(tRow)

        cmdUpdateItem.Visible = (iMaxIndx > 0)

        bSuccess = True


        If intI = 0 And tblIQty.FindControl("lblIRemove") IsNot Nothing Then
            tblIQty.FindControl("lblIRemove").Visible = False
        End If

        Page.Validate()



        cmdUpdateItem.Attributes.Add("onclick", sScript)

        Return (bSuccess)
    End Function

    Private Function LoadDigitalItems() As Boolean
        Dim tRow As TableRow
        Dim tCell As TableCell
        Dim lLabel As New Label
        Dim tText As New TextBox
        Dim vCmp As New CompareValidator
        Dim cChk As New CheckBox
        Dim sValID As String
        Dim bButton As ImageButton

        Dim rcd As System.Data.DataTable
        Dim indx As Integer
        Dim iMaxIndx As Integer
        Dim iItemCount As Integer = GetMaxIndxDownloadable()
        Dim bSuccess As Boolean = False

        Dim sID As String
        Dim sName As String
        Dim sDesc As String
        Dim sColor As String
        Dim lQty As Long
        Dim sQty As String = String.Empty
        Dim intI As Integer = 0
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter

        tblDnlTtl.Rows.Clear()
        tblDnlQty.Rows.Clear()

        rcd = MyBase.CurrentOrder.ItmCart
        iMaxIndx = rcd.Rows.Count
        indx = 0

        'Title
        'No Items Selected
        tRow = New TableRow
        tCell = New TableCell
        tCell.HorizontalAlign = HorizontalAlign.Center
        tCell.VerticalAlign = VerticalAlign.Middle
        tCell.ColumnSpan = 6
        lLabel = New Label
        lLabel.CssClass = "lblTitle"
        lLabel.ID = "lblITitleDigital"
        If (iItemCount = 0) Then
            lLabel.Text = "No Items Selected"
            tblDnlQty.Visible = False
            tblDnlTtl.Visible = False
            tblDnlUp.Visible = False
        Else
            lLabel.Text = "Downloadable Items Selected"
        End If
        tCell.Controls.Add(lLabel)
        tRow.Cells.Add(tCell)
        'tblDnlQty.Rows.Add(tRow)
        tblDnlTtl.Rows.Add(tRow)

        'Space
        tRow = New TableRow
        tCell = New TableCell
        tCell.Text = "&nbsp;"
        tRow.Cells.Add(tCell)
        'tblDnlQty.Rows.Add(tRow)
        tblDnlTtl.Rows.Add(tRow)

        'Remove - Label
        If (iItemCount > 0) Then
            tRow = New TableRow
            tCell = New TableCell : tRow.Cells.Add(tCell)
            tCell = New TableCell : tRow.Cells.Add(tCell)
            tCell = New TableCell : tRow.Cells.Add(tCell)
            tCell = New TableCell : tRow.Cells.Add(tCell)
            tCell = New TableCell
            tCell.HorizontalAlign = HorizontalAlign.Center
            tCell.VerticalAlign = VerticalAlign.Middle
            lLabel = New Label
            lLabel.CssClass = "lblTitle"
            lLabel.ID = "lblIRemoveDigital"
            lLabel.Text = "Remove"
            tCell.Controls.Add(lLabel)
            tRow.Cells.Add(tCell)
            tCell = New TableCell : tRow.Cells.Add(tCell)

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



            tblDnlQty.Rows.Add(tRow)
        End If

        Do While indx < iMaxIndx
            If rcd.Rows(indx).RowState <> DataRowState.Deleted Then
                Dim intPrintMethod As Integer = 0

                intPrintMethod = qa.getDocumentPrintMethod(rcd.Rows(indx).Item("ItmID").ToString())

                If intPrintMethod = 3 Then
                    rcd.Rows(indx).Item("ItmQty") = 1
                    lQty = 1



                    With rcd.Rows(indx)


                        sID = .Item("ItmID").ToString()
                        sName = .Item("ItmName").ToString()
                        sDesc = .Item("ItmDesc").ToString()
                        sColor = .Item("ItmColor").ToString()
                        'MyBase.MyData.GetItmDetail(MyBase.CurrentUser.CustomerID, sID, sName, sDesc)


                        intI += 1

                    End With
                    tRow = New TableRow

                    'ID - 0
                    tCell = New TableCell
                    tCell.Visible = False
                    tCell.Text = sID
                    tRow.Cells.Add(tCell)

                    'Label - 1
                    tCell = New TableCell
                    tCell.HorizontalAlign = HorizontalAlign.Left
                    tCell.VerticalAlign = VerticalAlign.Middle
                    lLabel = New Label
                    If (InStr(sColor, MyData.COLOR_INACTIVE) > 0) Then
                        lLabel.CssClass = "lblXLongInActive"
                    ElseIf (InStr(sColor, MyData.COLOR_BACK) > 0) Then
                        lLabel.CssClass = "lblXLongBack"
                    Else
                        lLabel.CssClass = "lblXLong"
                    End If
                    lLabel.ID = "lblI" & indx
                    lLabel.Text = sName & " : " & sDesc
                    tCell.Controls.Add(lLabel)
                    tRow.Cells.Add(tCell)

                    'Textbox - 2
                    tCell = New TableCell
                    tCell.HorizontalAlign = HorizontalAlign.Right
                    tCell.VerticalAlign = VerticalAlign.Middle
                    sValID = "txtI" & indx
                    tText = New TextBox
                    tText.CssClass = "txtNumber"
                    tText.ID = sValID
                    tText.Text = lQty
                    tText.MaxLength = 9
                    tText.Visible = False
                    If intPrintMethod = 3 Then
                        tText.ReadOnly = True
                    End If
                    tCell.Controls.Add(tText)
                    tRow.Cells.Add(tCell)

                    'Validator - 3
                    If (MyBase.CurrentUser.CanExtendQtyLI) Then
                        sQty = 100000000
                    Else
                        sQty = MyBase.CurrentOrder.MaxQtyPerLineItem
                    End If
                    tCell = New TableCell
                    vCmp = New CompareValidator
                    vCmp.ID = "vI" & indx
                    vCmp.ControlToValidate = sValID
                    vCmp.Display = ValidatorDisplay.Dynamic
                    vCmp.ErrorMessage = "<p>Quantity cannot exceed " & sQty & "</p>"
                    vCmp.[Operator] = ValidationCompareOperator.LessThanEqual
                    vCmp.Type = ValidationDataType.Integer
                    vCmp.ValueToCompare = sQty
                    vCmp.CssClass = "lblInactive"

                    If CurrentUser.CanExtendQtyLI = False Then
                        'tCell.Controls.Add(vCmp)
                    End If


                    tRow.Cells.Add(tCell)

                    'Space - 4
                    tCell = New TableCell
                    tCell.Text = "&nbsp"
                    tRow.Cells.Add(tCell)

                    'Checkbox - 5
                    tCell = New TableCell
                    tCell.HorizontalAlign = HorizontalAlign.Center
                    tCell.VerticalAlign = VerticalAlign.Middle
                    cChk = New CheckBox
                    cChk.ID = "chkI" & indx
                    tCell.Controls.Add(cChk)
                    tRow.Cells.Add(tCell)

                    'Space - 6
                    tCell = New TableCell
                    tCell.Text = "&nbsp"
                    tRow.Cells.Add(tCell)



                    If CurrentUser.ShowPrice Then
                        Dim decItmPrice As Decimal = CurrentOrder.GetItemPrice(rcd.Rows(indx).Item("ItmID"), lQty)
                        tCell = New TableCell
                        tCell.Text = "<span class=""lblPrice"">$" & String.Format("{0:N5}", decItmPrice) & "</span>"
                        tCell.HorizontalAlign = HorizontalAlign.Right
                        tRow.Cells.Add(tCell)
                        tCell = New TableCell
                        tCell.HorizontalAlign = HorizontalAlign.Right
                        tCell.Text = "<span class=""lblPrice"">" & Format(decItmPrice * lQty, "Currency") & "</span>"
                        tRow.Cells.Add(tCell)
                    Else

                        tCell = New TableCell
                        tCell.Text = ""
                        tRow.Cells.Add(tCell)
                        tCell = New TableCell
                        tCell.Text = ""
                        tRow.Cells.Add(tCell)
                    End If





                    tblDnlQty.Rows.Add(tRow)
                End If
            End If
            indx = indx + 1
        Loop

        'Space
        tRow = New TableRow
        tCell = New TableCell
        tCell.Text = "&nbsp;"
        tRow.Cells.Add(tCell)
        tblDnlQty.Rows.Add(tRow)

        'cmdUpdateItem.Visible = (iMaxIndx > 0)

        bSuccess = True


        If intI = 0 And tblDnlQty.FindControl("lblIRemoveDigital") IsNot Nothing Then
            tblDnlQty.FindControl("lblIRemoveDigital").Visible = False
        End If

        Page.Validate()

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
    Private Function LoadKits() As Boolean
        Dim tRow As TableRow
        Dim tCell As TableCell
        Dim lLabel As New Label
        Dim tText As New TextBox
        Dim cCbo As New DropDownList
        Dim vCmp As New CompareValidator
        Dim cChk As New CheckBox
        Dim sValID As String
        Dim bButton As ImageButton

        Dim rcd As System.Data.DataTable
        Dim indx As Integer
        Dim iMaxIndx As Integer
        Dim bSuccess As Boolean = False

        Dim sID As String
        Dim sName As String
        Dim sDesc As String
        Dim sColor As String
        Dim lQty As Long
        Dim sQty As String = String.Empty
        Dim intI As Integer = 0
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        Dim decLineKitChg As Decimal = qa.GetLineKitChg(CurrentUser.CustomerID)
        Dim strKitContents As String = ""


        tblKTtl.Rows.Clear()
        tblKQty.Rows.Clear()

        rcd = MyBase.CurrentOrder.KitCart
        iMaxIndx = rcd.Rows.Count
        indx = 0

        'Title
        tRow = New TableRow
        tCell = New TableCell
        tCell.HorizontalAlign = HorizontalAlign.Center
        tCell.VerticalAlign = VerticalAlign.Middle
        tCell.ColumnSpan = 6
        lLabel = New Label
        lLabel.CssClass = "lblTitle"
        lLabel.ID = "lblKTitle"
        If (iMaxIndx = 0) Then
            'No Kits Selected
            lLabel.Text = "No Ordered Kits"
        Else
            lLabel.Text = "Ordered Kits"
        End If
        tCell.Controls.Add(lLabel)
        tRow.Cells.Add(tCell)
        'tblKQty.Rows.Add(tRow)
        tblKTtl.Rows.Add(tRow)

        'Space
        tRow = New TableRow
        tCell = New TableCell
        tCell.Text = "&nbsp;"
        tRow.Cells.Add(tCell)
        'tblKQty.Rows.Add(tRow)


        tblKTtl.Rows.Add(tRow)

        'Remove - Label
        If (iMaxIndx > 0) Then
            tRow = New TableRow
            tCell = New TableCell : tRow.Cells.Add(tCell)
            tCell = New TableCell : tRow.Cells.Add(tCell)
            tCell = New TableCell : tRow.Cells.Add(tCell)
            tCell = New TableCell : tRow.Cells.Add(tCell)
            tCell = New TableCell
            tCell.HorizontalAlign = HorizontalAlign.Center
            tCell.VerticalAlign = VerticalAlign.Middle
            lLabel = New Label
            lLabel.CssClass = "lblTitle"
            lLabel.ID = "lblKRemove"
            lLabel.Text = "Remove"
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

            tblKQty.Rows.Add(tRow)
        End If

        Do While indx < iMaxIndx
            If rcd.Rows(indx).RowState <> DataRowState.Deleted Then
                With rcd.Rows(indx)

                    sID = .Item("KitID").ToString()
                    strKitContents = GetKitContents(sID, "")
                    sName = .Item("KitName").ToString()
                    sDesc = .Item("KitDesc").ToString()
                    sColor = .Item("KitColor").ToString()
                    'MyBase.MyData.GetKitDetail(MyBase.CurrentUser.CustomerID, Val(sID), sName, sDesc)
                    lQty = Val(.Item("KitQty").ToString())
                    intI += 1
                End With
                tRow = New TableRow

                'ID - 0
                tCell = New TableCell
                tCell.Visible = False
                tCell.Text = sID
                tRow.Cells.Add(tCell)

                'Label - 1
                tCell = New TableCell
                tCell.HorizontalAlign = HorizontalAlign.Left
                tCell.VerticalAlign = VerticalAlign.Middle
                lLabel = New Label
                If (InStr(sColor, MyData.COLOR_INACTIVE) > 0) Then
                    lLabel.CssClass = "lblXLongInActive"
                ElseIf (InStr(sColor, MyData.COLOR_BACK) > 0) Then
                    lLabel.CssClass = "lblXLongBack"
                Else
                    lLabel.CssClass = "lblXLong"
                End If
                lLabel.ID = "lblK" & indx
                lLabel.Text = sName & " : " & sDesc
                tCell.Controls.Add(lLabel)
                tRow.Cells.Add(tCell)

                'Textbox - 2
                tCell = New TableCell
                tCell.HorizontalAlign = HorizontalAlign.Right
                tCell.VerticalAlign = VerticalAlign.Middle
                sValID = "txtK" & indx
                tText = New TextBox
                tText.CssClass = "txtNumber"
                tText.ID = sValID
                tText.Text = lQty
                tText.MaxLength = 5
                tCell.Controls.Add(tText)
                tRow.Cells.Add(tCell)

                'Validator - 3
                If (MyBase.CurrentUser.CanExtendQtyLI) Then
                    sQty = 32767
                Else
                    sQty = MyBase.CurrentOrder.MaxQtyPerLineItem
                End If

                tCell = New TableCell
                vCmp = New CompareValidator
                vCmp.ID = "vK" & indx
                vCmp.ControlToValidate = sValID
                vCmp.Display = ValidatorDisplay.Dynamic
                vCmp.ErrorMessage = "<P>Quantity cannot exceed " & sQty & "</p>"
                vCmp.[Operator] = ValidationCompareOperator.LessThanEqual
                vCmp.Type = ValidationDataType.Integer
                vCmp.ValueToCompare = sQty
                vCmp.CssClass = "lblInactive"

                If CurrentUser.CanExtendQtyLI = False Then
                    ' tCell.Controls.Add(vCmp)
                End If

                tRow.Cells.Add(tCell)

                'Space - 4
                tCell = New TableCell
                tCell.Text = "&nbsp"
                tRow.Cells.Add(tCell)

                'Checkbox - 5
                tCell = New TableCell
                tCell.HorizontalAlign = HorizontalAlign.Center
                tCell.VerticalAlign = VerticalAlign.Middle
                cChk = New CheckBox
                cChk.ID = "chkK" & indx
                tCell.Controls.Add(cChk)
                tRow.Cells.Add(tCell)

                'Space - 6
                'tCell = New TableCell
                'tCell.Text = "&nbsp"
                'tRow.Cells.Add(tCell)


                If CurrentUser.ShowPrice Then

                    Dim decKitPrice As Decimal = CurrentOrder.GetKitPrice(sID, decLineKitChg, lQty)
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
                Else

                    tCell = New TableCell
                    tCell.Text = ""
                    tRow.Cells.Add(tCell)
                    tCell = New TableCell
                    tCell.Text = ""
                    tRow.Cells.Add(tCell)

                End If

                'upload document file:
                Dim ta As New dsCustomerDocumentTableAdapters.Customer_Kit_DocumentTableAdapter
                For Each dr As dsCustomerDocument.Customer_Kit_DocumentRow In ta.GetDataByKitID(sID).Rows
                    Dim qaInv As New dsInventoryTableAdapters.QueriesTableAdapter

                    If qaInv.CheckIfItemRequiresUpload(dr.DocumentID) And CurrentOrder.EditOrder = False Then

                        If CheckIfFileUploaded(dr.DocumentID) = False Then
                            Dim fileup As New RadUpload 'FileUpload
                            'fileup.TargetFolder = "~/Uploads"
                            fileup.ControlObjectsVisibility = ControlObjectsVisibility.ClearButtons
                            tCell = New TableCell
                            tCell.ID = "UploadCell" & dr.DocumentID
                            tCell.Controls.Add(fileup)
                            tRow.Cells.Add(tCell)


                            Dim strUploadInstructions As String = qaInv.GetUploadInstructions(dr.DocumentID)
                            If strUploadInstructions <> "" Then
                                Dim lb As New LinkButton
                                lb.ID = "lb" & dr.DocumentID
                                lb.OnClientClick = "return openDialog('" & GetJSString(strUploadInstructions) & "');"
                                lb.Text = "view upload file instructions"
                                lb.ToolTip = "view upload file instructions"
                                tCell.Controls.Add(lb)
                            End If

                            dUploadMessage.Visible = True
                            cmdNext.Visible = False
                        Else
                            tCell = New TableCell
                            lLabel = New Label
                            lLabel.Text = "FILE UPLOADED"
                            lLabel.CssClass = "lblUploaded"
                            tCell.Controls.Add(lLabel)
                            tRow.Cells.Add(tCell)
                        End If
                    End If
                Next

                tRow.Cells.Add(tCell)

                tblKQty.Rows.Add(tRow)

                'add kit contents row:
                Dim tc As New TableCell
                Dim tr As New TableRow
                tc.ColumnSpan = 10
                tc.Text = strKitContents
                tr.Cells.Add(tc)
                tblKQty.Rows.Add(tr)
            End If

            indx = indx + 1
        Loop

        'Space
        tRow = New TableRow
        tCell = New TableCell
        tCell.Text = "&nbsp;"
        tRow.Cells.Add(tCell)
        tblKQty.Rows.Add(tRow)

        cmdUpdateKit.Visible = (iMaxIndx > 0)
        If intI = 0 And tblKQty.FindControl("lblKRemove") IsNot Nothing Then
            tblKQty.FindControl("lblKRemove").Visible = False
        End If

        bSuccess = True

        Page.Validate()



        Return (bSuccess)
    End Function

    Private Sub LoadNotes()
        Dim bRequiresLI As Boolean = False
        Dim bRequiresOR As Boolean = False
        If CurrentUser.DisableAllQuantityLimits = False Then
            'Authorization Note...
            With MyBase.CurrentOrder

                .CurrentAuthorizationStatus(bRequiresLI, bRequiresOR, CurrentUser.DisableAllQuantityLimits)


                If (bRequiresLI) Then
                    lblLI.Text = "Requesting a quantity greater than " & .MaxQtyPerLineItem & " per line will require authorization."
                Else
                    lblLI.Text = "Your order is limited to " & .MaxQtyPerLineItem & " pieces per line item."
                End If
                If (bRequiresOR) Then
                    lblOR.Text = "Requesting a total quantity greater than " & .MaxQtyPerOrder & " per order will require authorization."
                Else
                    lblOR.Text = "Your order is limited to " & .MaxQtyPerOrder & " total pieces."
                End If
            End With
        End If

    End Sub

    Private Function ValidItemData() As Boolean
        Const COL_ID = 0
        Const COL_LAB = 1
        Const COL_QTY = 2
        Const COL_CHK = 5

        Dim bValid As Boolean = False
        Dim sMsg As String = String.Empty

        Dim indx As Integer
        Dim iMaxIndx As Integer

        Dim lID As Long
        Dim sDesc As String
        Dim lQty As Long
        Dim lLabel As Label
        Dim tText As TextBox
        Dim cChk As CheckBox

        Dim blnCanExtendQtyLi As Boolean
        'If CurrentOrder.EditOrder Then
        '    blnCanExtendQtyLi = CurrentOrder.CanExtendQtyLI()
        'Else
        blnCanExtendQtyLi = CurrentUser.CanExtendQtyLI()
        'End If


        Try
            bValid = True

            If bValid Then
                iMaxIndx = tblIQty.Rows.Count
                indx = 0
                Do While indx < iMaxIndx
                    lID = 0
                    lQty = 0

                    With tblIQty.Rows(indx)
                        lID = Val(.Cells(COL_ID).Text)
                        If (lID > 0) Then
                            lLabel = .Cells(COL_LAB).Controls(0)
                            sDesc = lLabel.Text
                            tText = .Cells(COL_QTY).Controls(0)
                            lQty = Val(tText.Text)
                            cChk = .Cells(COL_CHK).Controls(0)
                            If (cChk.Checked = True) Then lQty = 0
                            ''TO DO.....
                            '            If (iQty > MyBase.CurrentOrder.MaxQtyPerLineItem) Then
                            '              bValid = False
                            '            End If
                            '            If Not bValid Then sMsg = sDesc & " is greater than max qty per line item..."

                        End If
                    End With
                    indx = indx + 1
                Loop
            End If

            Dim dsInventoryDT As New dsInventory.OrderQuantityLimitByIDDataTable
            Dim dsInventoryTA As New dsInventoryTableAdapters.OrderQuantityLimitByIDTableAdapter
            Dim sPartNumber As String
            Dim sErrorMessage
            Dim dtCart As DataTable = MyBase.CurrentOrder.ItmCart

            For indx = 1 To tblIQty.Rows.Count - 2
                sPartNumber = GetPartNumber(CType(tblIQty.Rows(indx).Cells(COL_LAB).Controls(0), Label).Text)
                'dsInventoryTA.Fill(dsInventoryDT, sPartNumber)
                dsInventoryTA.Fill(dsInventoryDT, tblIQty.Rows(indx).Cells(COL_ID).Text)

                Dim intOrderQtyLimit As Integer = 0

                Select Case dsInventoryDT(0).OrderQuantityLimit
                    Case 999999, 999999999
                        intOrderQtyLimit = 0
                    Case Else
                        intOrderQtyLimit = dsInventoryDT(0).OrderQuantityLimit
                End Select



                If (CurrentOrder.MaxQtyPerLineItem < CType(CType(tblIQty.Rows(indx).Cells(COL_QTY).Controls(0), TextBox).Text, Integer) And intOrderQtyLimit <= CurrentOrder.MaxQtyPerLineItem) And blnCanExtendQtyLi = False Then
                    Dim lError As New Label

                    sErrorMessage = String.Format("<p>Quantity cannot exceed " & MyBase.CurrentOrder.MaxQtyPerLineItem & "</p>")

                    bValid = False
                    lError.Text = sErrorMessage
                    lError.CssClass = "lblInactive"
                    tblIQty.Rows(indx).Cells(COL_QTY + 1).Controls.Add(lError)

                    CType(tblIQty.Rows(indx).Cells(COL_QTY).Controls(0), TextBox).Text = dtCart.Rows(indx - 1).Item("itmQty").ToString
                    CType(tblIQty.Rows(indx).Cells(COL_QTY).Controls(0), TextBox).BackColor = Color.Yellow
                End If

                If (CType(CType(tblIQty.Rows(indx).Cells(COL_QTY).Controls(0), TextBox).Text, Integer) > intOrderQtyLimit) And blnCanExtendQtyLi = False And intOrderQtyLimit > 0 Then
                    Dim lError As New Label

                    sErrorMessage = String.Format("<p>There is an order limit of {0} for item {1}</p>", intOrderQtyLimit, sPartNumber)

                    bValid = False
                    lError.Text = sErrorMessage
                    lError.CssClass = "lblInactive"
                    tblIQty.Rows(indx).Cells(COL_QTY + 1).Controls.Add(lError)
                    CType(tblIQty.Rows(indx).Cells(COL_QTY).Controls(0), TextBox).Text = dtCart.Rows(indx - 1).Item("itmQty").ToString
                    CType(tblIQty.Rows(indx).Cells(COL_QTY).Controls(0), TextBox).BackColor = Color.Yellow
                End If
            Next

        Catch ex As Exception
            MyBase.PageMessage = "Error attempting to Validate Item Data..."
        End Try

        If Not bValid Then MyBase.PageMessage = sMsg
        Return (bValid)
    End Function

    Private Function GetPartNumber(ByVal sDescription As String) As String
        Const sDeliminator As String = " : "
        Return sDescription.Substring(0, InStr(sDescription, sDeliminator) - 1).Trim
    End Function
    Private Function SaveDigitalItemData() As Boolean
        'TODO save item data

        Const COL_ID = 0
        Const COL_LAB = 1
        Const COL_QTY = 2
        Const COL_CHK = 5

        Dim rcd As System.Data.DataTable
        Dim indx As Integer
        Dim iMaxIndx As Integer

        Dim lID As Int32
        Dim sName As String
        Dim sDesc As String
        Dim lQty As Long
        Dim sTmp As String
        Dim ipos As Integer
        Dim tText As TextBox
        Dim cChk As CheckBox

        Dim bSuccess As Boolean = False

        'Try
        iMaxIndx = tblDnlQty.Rows.Count
        indx = 0
        Do While indx < iMaxIndx
            lID = 0
            sName = ""
            sDesc = ""
            lQty = 0

            With tblDnlQty.Rows(indx)
                lID = Val(.Cells(COL_ID).Text)
                If (lID > 0) Then
                    tText = .Cells(COL_QTY).Controls(0)
                    lQty = Val(tText.Text)
                    cChk = .Cells(COL_CHK).Controls(0)
                    If (cChk.Checked = True) Then lQty = 0 '.Visible = False
                    MyBase.CurrentOrder.ItmSave(lQty, lID, sName, sDesc)
                End If
                indx = indx + 1
            End With
        Loop
        MyBase.SessionStore("OrdItm")

        bSuccess = True

        'Catch ex As Exception
        '    MyBase.PageMessage = "Error attempting to Save Item Data..."
        'End Try

        Return (bSuccess)
    End Function
    Private Function SaveItemData() As Boolean
        'TODO save item data

        Const COL_ID = 0
        Const COL_LAB = 1
        Const COL_QTY = 2
        Const COL_CHK = 5

        Dim rcd As System.Data.DataTable
        Dim indx As Integer
        Dim iMaxIndx As Integer

        Dim lID As Int32
        Dim sName As String
        Dim sDesc As String
        Dim lQty As Long
        Dim sTmp As String
        Dim ipos As Integer
        Dim tText As TextBox
        Dim cChk As CheckBox

        Dim bSuccess As Boolean = False

        'Try
        iMaxIndx = tblIQty.Rows.Count
        indx = 0
        Do While indx < iMaxIndx
            lID = 0
            sName = ""
            sDesc = ""
            lQty = 0

            With tblIQty.Rows(indx)
                lID = Val(.Cells(COL_ID).Text)
                If (lID > 0) Then
                    tText = .Cells(COL_QTY).Controls(0)
                    lQty = Val(tText.Text)
                    cChk = .Cells(COL_CHK).Controls(0)
                    If (cChk.Checked = True) Then lQty = 0 '.Visible = False
                    MyBase.CurrentOrder.ItmSave(lQty, lID, sName, sDesc)
                End If
                indx = indx + 1
            End With
        Loop
        MyBase.SessionStore("OrdItm")

        bSuccess = True

        'Catch ex As Exception
        '    MyBase.PageMessage = "Error attempting to Save Item Data..."
        'End Try

        Return (bSuccess)
    End Function

    Private Function ValidKitData() As Boolean
        Const COL_ID = 0
        Const COL_LAB = 1
        Const COL_QTY = 2
        Const COL_CHK = 5

        Dim bValid As Boolean = False
        Dim sMsg As String = String.Empty

        Dim indx As Integer
        Dim iMaxIndx As Integer

        Dim lID As Long
        Dim sDesc As String
        Dim lQty As Long
        Dim lLabel As Label
        Dim tText As TextBox
        Dim cChk As CheckBox
        Dim qaInv As New dsInventoryTableAdapters.QueriesTableAdapter
        Try
            bValid = True

            If bValid Then
                iMaxIndx = tblKQty.Rows.Count
                indx = 0
                Do While indx < iMaxIndx
                    lID = 0
                    lQty = 0

                    With tblKQty.Rows(indx)
                        lID = Val(.Cells(COL_ID).Text)
                        If (lID > 0) Then
                            lLabel = .Cells(COL_LAB).Controls(0)
                            sDesc = lLabel.Text
                            tText = .Cells(COL_QTY).Controls(0)
                            lQty = Val(tText.Text)
                            cChk = .Cells(COL_CHK).Controls(0)
                            If (cChk.Checked = True) Then lQty = 0

                        End If
                    End With
                    indx = indx + 1
                Loop
            End If

            Dim dsInventoryDT As New dsInventory.ComponentQuantityLimit2DataTable
            Dim dsInventoryTA As New dsInventoryTableAdapters.ComponentQuantityLimit2TableAdapter
            Dim dsInventoryRow As dsInventory.ComponentQuantityLimit2Row

            Dim blnCanExtendQtyLi As Boolean

            'If CurrentOrder.EditOrder Then
            '    blnCanExtendQtyLi = CurrentOrder.CanExtendQtyLI()
            'Else
            blnCanExtendQtyLi = CurrentUser.CanExtendQtyLI()
            'End If



            Dim sKit As String
            Dim sErrorMessage
            Dim dtCart As DataTable = MyBase.CurrentOrder.KitCart
            Dim iKitQuantity As Integer


            For indx = 1 To Me.tblKQty.Rows.Count - 2
                sKit = GetPartNumber(CType(tblKQty.Rows(indx).Cells(COL_LAB).Controls(0), Label).Text)
                'dsInventoryTA.Fill(dsInventoryDT, sKit)
                dsInventoryTA.Fill(dsInventoryDT, tblKQty.Rows(indx).Cells(COL_ID).Text)

                iKitQuantity = CType(CType(Me.tblKQty.Rows(indx).Cells(COL_QTY).Controls(0), TextBox).Text, Integer)
                For Each dsInventoryRow In dsInventoryDT.Rows
                    Dim intOrderQuantityLimit As Integer = dsInventoryRow.OrderQuantityLimit
                    If (iKitQuantity * dsInventoryRow.Qty > intOrderQuantityLimit) And blnCanExtendQtyLi = False Then
                        Dim lError As New Label
                        'TODO change error message:
                        sErrorMessage = String.Format("<P>There is an order limit of {0} for item {1} contained in this kit</p>", intOrderQuantityLimit, qaInv.GetItemRefNumber(dsInventoryRow.ItemNumber)) 'sKit

                        bValid = False
                        lError.Text = sErrorMessage
                        lError.CssClass = "lblInactive"
                        tblKQty.Rows(indx).Cells(COL_QTY + 1).Controls.Add(lError)
                        CType(tblKQty.Rows(indx).Cells(COL_QTY).Controls(0), TextBox).Text = dtCart.Rows(indx - 1).Item(1).ToString
                        CType(tblKQty.Rows(indx).Cells(COL_QTY).Controls(0), TextBox).BackColor = Color.Yellow
                    End If

                Next

                Dim sQty As Integer
                'Validator - 3
                If (blnCanExtendQtyLi) Then
                    sQty = 32767
                Else
                    sQty = MyBase.CurrentOrder.MaxQtyPerLineItem
                End If
                If sQty < iKitQuantity * dsInventoryRow.Qty And blnCanExtendQtyLi = False Then
                    Dim lError As New Label

                    sErrorMessage = String.Format("<P>Quantity cannot exceed " & sQty & "</p>")
                    bValid = False
                    lError.Text = sErrorMessage
                    lError.CssClass = "lblInactive"
                    tblKQty.Rows(indx).Cells(COL_QTY + 1).Controls.Add(lError)
                    CType(tblKQty.Rows(indx).Cells(COL_QTY).Controls(0), TextBox).Text = dtCart.Rows(indx - 1).Item(1).ToString
                    CType(tblKQty.Rows(indx).Cells(COL_QTY).Controls(0), TextBox).BackColor = Color.Yellow

                End If
            Next

        Catch ex As Exception
            'If Request.IsLocal Then
            '    MyBase.PageMessage = ex.ToString() & " " & ex.StackTrace
            'Else
            '    MyBase.PageMessage = "Error attempting to Validate Kit Data..."
            'End If
        End Try

        If Not bValid Then MyBase.PageMessage = sMsg
        Return (bValid)
    End Function

    Private Function SaveKitData() As Boolean


        Const COL_ID = 0
        Const COL_LAB = 1
        Const COL_QTY = 2
        Const COL_CHK = 5

        Dim rcd As System.Data.DataTable
        Dim indx As Integer
        Dim iMaxIndx As Integer

        Dim lID As Int32
        Dim sName As String
        Dim sDesc As String
        Dim lQty As Long
        Dim sTmp As String
        Dim ipos As Integer
        Dim tText As TextBox
        Dim cChk As CheckBox

        Dim bSuccess As Boolean = False

        Try
            iMaxIndx = tblKQty.Rows.Count
            indx = 0
            Do While indx < iMaxIndx
                lID = 0
                sName = ""
                sDesc = ""
                lQty = 0

                With tblKQty.Rows(indx)

                    lID = Val(.Cells(COL_ID).Text)
                    If (lID > 0) Then
                        tText = .Cells(COL_QTY).Controls(0)
                        lQty = Val(tText.Text)
                        cChk = .Cells(COL_CHK).Controls(0)
                        If (cChk.Checked = True) Then lQty = 0
                        MyBase.CurrentOrder.KitSave(lQty, lID, sName, sDesc)
                    End If
                End With
                indx = indx + 1
            Loop
            MyBase.SessionStore("OrdKit")

            bSuccess = True

        Catch ex As Exception
            MyBase.PageMessage = "Error attempting to Save Kit Data..."
        End Try

        Return (bSuccess)
    End Function

    Private Overloads Sub cmdUpdateItem_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdUpdateItem.Click
        If ValidItemData() Then
            'upload files
            UploadDocuments()
            SaveItemData()
            LoadItems()
            LoadNotes()

        End If
    End Sub
    Sub UploadDocuments()

        'upload from items:

        Dim filepath As String = ConfigurationManager.AppSettings("UploadsFolder").ToString & Session.SessionID & "/"
        Const COL_ID = 0
        Dim iMaxIndx As Integer = 0
        Dim indx = 0
        Dim lId As Integer
        iMaxIndx = tblIQty.Rows.Count
        indx = 0
        Do While indx < iMaxIndx
            lId = 0
            '6-7
            With tblIQty.Rows(indx)
                lId = Val(.Cells(COL_ID).Text)
                If (lId > 0) Then
                    If .Cells.Count > 7 Then


                        Dim i As Integer = 7
                        For i = 6 To .Cells.Count - 1
                            If .Cells(i).Controls.Count > 0 AndAlso TypeOf .Cells(i).Controls(0) Is RadUpload Then
                                Dim ctl As Control = .Cells(i).Controls(0)

                                'If TypeOf ctl Is FileUpload Then
                                If TypeOf ctl Is RadUpload Then
                                    Dim fileupload As RadUpload = ctl

                                    If fileupload.UploadedFiles.Count > 0 Then
                                        Dim strExt As String = System.IO.Path.GetExtension(fileupload.UploadedFiles(0).FileName)
                                        If Not Directory.Exists(Server.MapPath(filepath)) Then
                                            Directory.CreateDirectory(Server.MapPath(filepath))
                                        End If
                                        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
                                        Dim strRefNo As String = qa.GetItemRefNumber(lId)
                                        fileupload.UploadedFiles(0).SaveAs(Server.MapPath(filepath) & strRefNo & strExt)
                                    End If
                                End If
                            End If


                        Next





                    End If

                End If
                indx = indx + 1
            End With
        Loop


        'kits:
        iMaxIndx = 0
        indx = 0
        iMaxIndx = tblKQty.Rows.Count
        Do While indx < iMaxIndx
            lId = 0
            '6-7
            With tblKQty.Rows(indx)
                lId = Val(.Cells(COL_ID).Text)
                If (lId > 0) Then
                    If .Cells.Count > 7 Then


                        Dim i As Integer = 7
                        For i = 6 To .Cells.Count - 1
                            If .Cells(i).Controls.Count > 0 AndAlso TypeOf .Cells(i).Controls(0) Is FileUpload Then
                                Dim ctl As Control = .Cells(i).Controls(0)

                                If TypeOf ctl Is FileUpload Then
                                    Dim fileupload As FileUpload = ctl
                                    Dim strExt As String = System.IO.Path.GetExtension(fileupload.FileName)
                                    If fileupload.HasFile Then
                                        If Not Directory.Exists(Server.MapPath(filepath)) Then
                                            Directory.CreateDirectory(Server.MapPath(filepath))
                                        End If
                                        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
                                        Dim strRefNo As String = qa.GetItemRefNumber(lId)
                                        fileupload.SaveAs(Server.MapPath(filepath) & strRefNo & strExt)
                                    End If
                                End If
                            End If


                        Next





                    End If

                End If
                indx = indx + 1
            End With
        Loop

    End Sub
    Private Overloads Sub cmdUpdateKit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdUpdateKit.Click
        If ValidKitData() Then
            SaveKitData()
            LoadKits()
            LoadNotes()
        End If
    End Sub

    Private Overloads Sub cmdShop_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdShop.Click
        Dim sDestin As String = paraPageBase.PAG_OrdItemSelect.ToString()
        MyBase.PageDirect(sDestin, 0, 0)
    End Sub

    Private Overloads Sub cmdItem_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdItem.Click
        Dim sDestin As String = paraPageBase.PAG_OrdItemSelect.ToString()
        MyBase.PageDirect(sDestin, 0, 0)
    End Sub

    Private Sub cmdKit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdKit.Click
        Dim sDestin As String = paraPageBase.PAG_OrdKitSelect.ToString()
        MyBase.PageDirect(sDestin, 0, 0)
    End Sub

    Private Overloads Sub cmdNext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdNext.Click
        Dim sDestin As String = paraPageBase.PAG_OrdRequest.ToString()

        If (txtSubmit.Text = "Y") Then
            sDestin = paraPageBase.PAG_OrdSubmit.ToString()
        End If
        MyBase.PageDirect(sDestin, 0, 0)
    End Sub

    Protected Sub cmdEditBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdEditBack.Click


        Response.Redirect("OrdSve.aspx")
    End Sub
    Function GetJSString(ByVal strTmp As String) As String
        strTmp = Replace(strTmp, "'", "\'")
        Return strTmp
    End Function

    Private Sub cmdNext_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Load

    End Sub

    Function GetKitContents(intKitID As Integer, strCssClass As String) As String
        Dim o As New paraData
        Return o.GetKitContents(intKitID, strCssClass, False, True)
    End Function


    Sub GetShowStockSetting()
        Dim ta As New dsAccessCodesTableAdapters.Customer_AccessCode1TableAdapter
        Dim dr As dsAccessCodes.Customer_AccessCode1Row
        dr = ta.GetDataByAccessCodeID(CurrentUser.AccessCodeID).Rows(0)
        hfShowStock.Value = dr.ShowStock
    End Sub

    Function GetMaxIndxNonDownloadable() As Integer
        Dim indx As Integer = 0
        Dim iMaxIndx As Integer = 0
        Dim rcd As New DataTable
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
        Dim i As Integer = 0

        rcd = MyBase.CurrentOrder.ItmCart
        iMaxIndx = rcd.Rows.Count

        Do While indx < iMaxIndx


            If rcd.Rows(indx).RowState <> DataRowState.Deleted Then
                Dim intPrintMethod As Integer = 0
                intPrintMethod = qa.getDocumentPrintMethod(rcd.Rows(indx).Item("ItmID").ToString())
                If intPrintMethod <> 3 Then
                    i += 1
                End If
            End If
            indx = indx + 1
        Loop
        Return i
    End Function

    Function GetMaxIndxDownloadable() As Integer
        Dim indx As Integer = 0
        Dim iMaxIndx As Integer = 0
        Dim rcd As New DataTable
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
        Dim i As Integer = 0

        rcd = MyBase.CurrentOrder.ItmCart
        iMaxIndx = rcd.Rows.Count

        Do While indx < iMaxIndx


            If rcd.Rows(indx).RowState <> DataRowState.Deleted Then
                Dim intPrintMethod As Integer = 0
                intPrintMethod = qa.getDocumentPrintMethod(rcd.Rows(indx).Item("ItmID").ToString())
                If intPrintMethod = 3 Then
                    i += 1
                End If
            End If
            indx = indx + 1
        Loop
        Return i
    End Function

    Private Sub cmdUpdateItemDigital_Click(sender As Object, e As ImageClickEventArgs) Handles cmdUpdateItemDigital.Click
        If ValidItemData() Then
            
            SaveDigitalItemData()
            LoadDigitalItems()
            LoadNotes()

        End If
    End Sub
End Class
