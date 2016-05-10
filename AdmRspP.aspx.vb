''''''''''''''''''''
'mw - 09-21-2007
''''''''''''''''''''


Partial Class AdmRspP
  Inherits paraPageBase

#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

  End Sub
  Protected WithEvents Div1 As System.Web.UI.HtmlControls.HtmlGenericControl

  Private Const TXT_NON = "None"
  Private Const TXT_ATT = "Attributes"
  Private Const TXT_ITM = "Item Type"
  Private Const TXT_PAR = "Part Number"
  Private Const TXT_VEN = "Vendor"
  Private Const TXT_URG = "Urgency"

  'NOTE: The following placeholder declaration is required by the Web Form Designer.
  'Do not delete or move it.
  Private designerPlaceholderDeclaration As System.Object

  Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
    'CODEGEN: This method call is required by the Web Form Designer
    'Do not modify it using the code editor.
    InitializeComponent()

    MyBase.Page_Init(sender, e)
  End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        PageTitle = "Restock Planning Output"
        'MyBase.Page_Load(sender, e)
        MyBase.PageMessage = ""

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        ElseIf Not (MyBase.CurrentUser.CanRestockPlan = True) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri
            txtDays.Text = Request.QueryString("DY").ToString
            txtRound.Text = Request.QueryString("RN").ToString
            txtItmType.Text = Request.QueryString("IT").ToString
            txtItmTypeID.Text = Request.QueryString("ITI").ToString
            txtPartNo.Text = Request.QueryString("PN").ToString
            chkExZeroQty.Checked = IIf(Request.QueryString("XZQ").ToString = "T", True, False)
            cboSort1.Text = Request.QueryString("S1").ToString
            cboSort2.Text = Request.QueryString("S2").ToString
            cboSort3.Text = Request.QueryString("S3").ToString
            LoadData()
        End If 'End PostBack
    End Sub

  Private Function CurrentSort(ByVal indx As Integer) As String
    Dim sSort As String = String.Empty

    Select Case indx
      Case 1 : sSort = Trim$(cboSort1.Text)
      Case 2 : sSort = Trim$(cboSort2.Text)
      Case 3 : sSort = Trim$(cboSort3.Text)
    End Select

    Select Case sSort
      Case String.Empty : sSort = String.Empty
      Case TXT_NON : sSort = String.Empty
      Case TXT_ATT : sSort = "Customer_ItemType_Attribute_1.Description, Customer_ItemType_Attribute_2.Description, Customer_ItemType_Attribute_3.Description, Customer_ItemType_Attribute_4.Description"
      Case TXT_ITM : sSort = "Customer_ItemType.Description"
      Case TXT_PAR : sSort = "Customer_Document.ReferenceNo"
      Case TXT_VEN : sSort = "Customer_Vendor.Name"
        'Case TXT_URG : sSort = "iif(Customer_Document_Fill.QtyOH>0,Customer_Document_Fill.QtyOH/u.QtyPerWk,0)"
      Case TXT_URG : sSort = "iif(u.QtyPerWk>0,Customer_Document_Fill.QtyOH/u.QtyPerWk,0)"
    End Select

    Return sSort
  End Function

  Private Function CurrentItemTypeID() As Long
    Dim lID As Long = 0

    lID = Val(txtItmTypeID.Text)

    Return lID
  End Function

  Private Function LoadData()
    Dim bSuccess As Boolean = False

    Try
      LoadControls()

      ShowControls()

      bSuccess = True
    Catch ex As Exception
      Session("PMsg") = "Unable to Load Data..."
    End Try

    Return (bSuccess)
  End Function

  Private Function ShowControls()
  End Function

    Private Function BuildSQL() As String
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sFrom As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty
        'Dim sItmType As String = String.Empty
        Dim lItmTypeID As Long = 0
        Dim sPartNo As String = String.Empty
        Dim sSort1 As String = String.Empty
        Dim sSort2 As String = String.Empty
        Dim sSort3 As String = String.Empty
        Dim sUse As String = String.Empty

        If (MyBase.CurrentUser.CanRestockPlan = True) Then
            'sItmType = Trim$(txtItmType.Text)
            lItmTypeID = CurrentItemTypeID()
            sPartNo = Trim$(txtPartNo.Text)

            sSort1 = CurrentSort(1)
            sSort2 = CurrentSort(2)
            sSort3 = CurrentSort(3)

            sSelect = "SELECT Customer_Document.ID AS DocID, " & _
                      "Customer_ItemType.ReferenceNo AS ItmType, " & _
                      "Customer_Vendor.Name AS Vendor, " & _
                      "iif(Customer_Document.PrintMethod=0,'FUL',iif(Customer_Document.PrintMethod=1,'POD','F/P')) AS Status, " & _
                      "Customer_Document_Fill.Cost AS LastCost, " & _
                      "Customer_Document.InActPrjDate AS SchedObsDate, iif(ISNULL(Customer_Document.InActPrjDate),False,True) AS SchedObs, " & _
                      "Customer_Document.ReferenceNo AS PartNo, Customer_Document.Description AS PartDesc, " & _
                      "Customer_ItemType_Attribute_1.Description AS Attr1, " & _
                      "Customer_ItemType_Attribute_2.Description AS Attr2, " & _
                      "Customer_ItemType_Attribute_3.Description AS Attr3, " & _
                      "Customer_ItemType_Attribute_4.Description AS Attr4, " & _
                      "Customer_Document.Attr5 AS Attr5, " & _
                      "[QtyOHLocP]+IIf([Customer].[InventoryStatusWarehouse2]='0',[Customer_Document_Fill].[QtyOHLocS],0)+IIf([Customer].[InventoryStatusWarehouse3]='0',[Customer_Document_Fill].[QtyOHLocW],0) AS QtyOH, " & _
                      "iif(ISNULL(Customer_Format.QtyBE),0,Customer_Format.QtyBE) AS QtyBE, " & _
                      "iif(ISNULL(Customer_Format.Price),0,Customer_Format.Price) AS FmtPrice "

            sFrom = " FROM Customer " & _
                     "INNER JOIN (Customer_ItemType_Attribute AS Customer_ItemType_Attribute_4 " & _
                     "RIGHT JOIN (Customer_Format " & _
                     "RIGHT JOIN ((Customer_ItemType_Attribute AS Customer_ItemType_Attribute_3 " & _
                     "RIGHT JOIN (Customer_ItemType_Attribute AS Customer_ItemType_Attribute_2 " & _
                     "RIGHT JOIN (Customer_ItemType_Attribute AS Customer_ItemType_Attribute_1 " & _
                     "RIGHT JOIN ((Customer_Document " & _
                     "LEFT JOIN Customer_Vendor ON Customer_Document.VendorID = Customer_Vendor.ID) " & _
                     "LEFT JOIN Customer_ItemType " & _
                     "ON Customer_Document.ItemTypeID = Customer_ItemType.ID) ON Customer_ItemType_Attribute_1.ID = Customer_Document.Attr1ID) ON Customer_ItemType_Attribute_2.ID = Customer_Document.Attr2ID) ON Customer_ItemType_Attribute_3.ID = Customer_Document.Attr3ID) LEFT JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID) ON Customer_Format.ID = Customer_Document.FormatID) ON Customer_ItemType_Attribute_4.ID = Customer_Document.Attr4ID) ON Customer.ID = Customer_Document.CustomerID "

            'Only for Active, Fulfill or Fulfill/Print
            sWhere = " WHERE Customer_Document.CustomerID = " & MyBase.CurrentUser.CustomerID & _
                     " AND (Customer_Document.Status > 0) " ' & _
            '" AND (Customer_Document.PrintMethod <> 1) "
            'TODO enable for POD

            If (lItmTypeID > 0) Then
                sWhere = sWhere & " AND " & _
                         "Customer_Document.ItemTypeID = " & lItmTypeID
            End If
            If (sPartNo.Length > 0) Then
                sWhere = sWhere & " AND " & _
                         "Customer_Document.ReferenceNo LIKE " & MyBase.MyData.SQLString(sPartNo & "%")
            End If

            If (sSort1.Length > 0) Then
                If (Trim$(sOrder).Length > 0 And Right$(Trim$(sOrder), 1) <> ",") Then sOrder = sOrder & ", "
                sOrder = sOrder & sSort1
            End If
            If (sSort2.Length > 0) Then
                If (Trim$(sOrder).Length > 0 And Right$(Trim$(sOrder), 1) <> ",") Then sOrder = sOrder & ", "
                sOrder = sOrder & sSort2
            End If
            If (sSort3.Length > 0) Then
                If (Trim$(sOrder).Length > 0 And Right$(Trim$(sOrder), 1) <> ",") Then sOrder = sOrder & ", "
                sOrder = sOrder & sSort3
            End If
            If (Trim$(sOrder).Length > 0 And Right$(Trim$(sOrder), 1) <> ",") Then sOrder = sOrder & ", "
            sOrder = sOrder & " Customer_Document.ReferenceNo"
        End If
        sOrder = " ORDER BY " & sOrder

        If (InStr(sOrder, "u.") > 0) Then

            Dim strTmp As String = sSelect + sFrom + sWhere

            sUse = "SELECT Customer_Document.ID AS DocumentID, (" & _
                   "IIf(Sum(IIf(IsNull([Customer_Document_Fill].[ID]),1,[Customer_Document_Fill].[QtyPerPull])*[Order_Document].[QtyOrdered])>0," & _
                   "Sum(IIf(IsNull([Customer_Document_Fill].[ID]),1,[Customer_Document_Fill].[QtyPerPull])*[Order_Document].[QtyOrdered])/24 " & _
                   ",0)" & _
                   ") AS QtyPerWk " & _
                   "FROM ([Order] LEFT JOIN (Order_Document LEFT JOIN Customer_Document ON Order_Document.DocumentID = Customer_Document.ID) ON [Order].ID = Order_Document.OrderID) LEFT JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID " & _
                   "WHERE(Customer_Document.CustomerID = " & MyBase.CurrentUser.CustomerID & ") And ([Order].StatusID <> 4) And ([Order].RequestDate >= #" & Now.AddMonths(-6).Date & "#) And ([Order].RequestDate <= #" & Now.Date & " 11:59:00 PM#)" & _
                   "GROUP BY Customer_Document.ID"

            sFrom = " FROM Customer " & _
                     "INNER JOIN (Customer_ItemType_Attribute AS Customer_ItemType_Attribute_4 " & _
                     "RIGHT JOIN (Customer_Format " & _
                     "RIGHT JOIN ((Customer_ItemType_Attribute AS Customer_ItemType_Attribute_3 " & _
                     "RIGHT JOIN (Customer_ItemType_Attribute AS Customer_ItemType_Attribute_2 " & _
                     "RIGHT JOIN (Customer_ItemType_Attribute AS Customer_ItemType_Attribute_1 " & _
                     "RIGHT JOIN ((Customer_Document " & _
                     "LEFT JOIN Customer_Vendor ON Customer_Document.VendorID = Customer_Vendor.ID) " & _
                     "LEFT JOIN Customer_ItemType " & _
                     "ON Customer_Document.ItemTypeID = Customer_ItemType.ID) ON Customer_ItemType_Attribute_1.ID = Customer_Document.Attr1ID) ON Customer_ItemType_Attribute_2.ID = Customer_Document.Attr2ID) ON Customer_ItemType_Attribute_3.ID = Customer_Document.Attr3ID) LEFT JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID) ON Customer_Format.ID = Customer_Document.FormatID) ON Customer_ItemType_Attribute_4.ID = Customer_Document.Attr4ID) ON Customer.ID = Customer_Document.CustomerID "

            sOrder = Replace(sOrder, "u.", "")
            sOrder = Replace(sOrder, "Customer_Document.referenceno", "ItmType", 1, -1, CompareMethod.Text)
            sOrder = Replace(sOrder, "Customer_Document_Fill.", "t1.")
            sOrder = Replace(sOrder, "Customer_ItemType_Attribute_1.Description", "attr1")
            sOrder = Replace(sOrder, "Customer_ItemType_Attribute_2.Description", "attr2")
            sOrder = Replace(sOrder, "Customer_ItemType_Attribute_3.Description", "attr3")
            sOrder = Replace(sOrder, "Customer_ItemType_Attribute_4.Description", "attr4")
            sOrder = Replace(sOrder, "Customer_Vendor.Name", "t1.Vendor")
            sOrder = Replace(sOrder, "Customer_ItemType.Description", "ItmType")
            ' Customer_ItemType_Attribute_1.Description, Customer_ItemType_Attribute_2.Description, Customer_ItemType_Attribute_3.Description, Customer_ItemType_Attribute_4.Description,

            sSQL = "SELECT t1.*, t2.* FROM ( " & strTmp & ") as T1 " & vbCrLf & _
                   "LEFT JOIN (" & sUse & ") As T2 " & vbCrLf & _
                   "on t1.DocID = t2.documentID " & vbCrLf & _
                    sOrder

        Else
            sSQL = sSelect + sFrom + sWhere + sOrder
        End If







        'SELECT t1.*   FROM (

        'SELECT Customer_Document.ID AS DocID, Customer_ItemType.ReferenceNo AS ItmType, Customer_Vendor.Name AS Vendor, iif(Customer_Document.PrintMethod=0,'FUL',iif(Customer_Document.PrintMethod=1,'POD','F/P')) AS Status, Customer_Document_Fill.Cost AS LastCost, Customer_Document.InActPrjDate AS SchedObsDate, iif(ISNULL(Customer_Document.InActPrjDate),False,True) AS SchedObs, Customer_Document.ReferenceNo AS PartNo, Customer_Document.Description AS PartDesc, Customer_ItemType_Attribute_1.Description AS Attr1, Customer_ItemType_Attribute_2.Description AS Attr2, Customer_ItemType_Attribute_3.Description AS Attr3, Customer_ItemType_Attribute_4.Description AS Attr4, Customer_Document.Attr5 AS Attr5, [QtyOHLocP]+IIf([Customer].[InventoryStatusWarehouse2]='0',[Customer_Document_Fill].[QtyOHLocS],0)+IIf([Customer].[InventoryStatusWarehouse3]='0',[Customer_Document_Fill].[QtyOHLocW],0) AS QtyOH, iif(ISNULL(Customer_Format.QtyBE),0,Customer_Format.QtyBE) AS QtyBE, iif(ISNULL(Customer_Format.Price),0,Customer_Format.Price) AS FmtPrice  



        'FROM Customer INNER JOIN (Customer_ItemType_Attribute AS Customer_ItemType_Attribute_4 RIGHT JOIN (Customer_Format RIGHT JOIN ((Customer_ItemType_Attribute AS Customer_ItemType_Attribute_3 RIGHT JOIN (Customer_ItemType_Attribute AS Customer_ItemType_Attribute_2 RIGHT JOIN (Customer_ItemType_Attribute AS Customer_ItemType_Attribute_1 RIGHT JOIN ((Customer_Document LEFT JOIN Customer_Vendor ON Customer_Document.VendorID = Customer_Vendor.ID) LEFT JOIN Customer_ItemType ON Customer_Document.ItemTypeID = Customer_ItemType.ID) ON Customer_ItemType_Attribute_1.ID = Customer_Document.Attr1ID) ON Customer_ItemType_Attribute_2.ID = Customer_Document.Attr2ID) ON Customer_ItemType_Attribute_3.ID = Customer_Document.Attr3ID) LEFT JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID) ON Customer_Format.ID = Customer_Document.FormatID) ON Customer_ItemType_Attribute_4.ID = Customer_Document.Attr4ID) ON Customer.ID = Customer_Document.CustomerID  

        'WHERE Customer_Document.CustomerID = 14 AND (Customer_Document.Status > 0)  AND (Customer_Document.PrintMethod <> 1)  

        'ORDER BY Customer_Document.ReferenceNo) as T1

        ' LEFT JOIN 

        '(
        'SELECT Customer_Document.ID AS DocumentID, (IIf(Sum(IIf(IsNull([Customer_Document_Fill].[ID]),1,[Customer_Document_Fill].[QtyPerPull])*[Order_Document].[QtyOrdered])>0,Sum(IIf(IsNull([Customer_Document_Fill].[ID]),1,[Customer_Document_Fill].[QtyPerPull])*[Order_Document].[QtyOrdered])/24 ,0)) AS QtyPerWk 
        'FROM ([Order] LEFT JOIN (Order_Document LEFT JOIN Customer_Document ON Order_Document.DocumentID = Customer_Document.ID) ON [Order].ID = Order_Document.OrderID) LEFT JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID WHERE(Customer_Document.CustomerID = 14) And (Order.StatusID <> 4) And (Order.RequestDate >= #12/23/2009#) And (Order.RequestDate <= #6/23/2010 11:59:00 PM#)
        'GROUP BY Customer_Document.ID



        ') as T2

        'ON t1.docid =t2.documentID


        'order by   iif(QtyPerWk>0,QtyOH/QtyPerWk,0),  docid



        Return sSQL
    End Function

  Private Function LoadControls() As Boolean
    LoadHeader()
    LoadDetail()
  End Function

  Private Function LoadHeader() As Boolean

    Dim tRow As TableRow
    Dim tCell As TableCell

    Dim bSuccess As Boolean = False

    tRow = New TableRow

    tCell = GetCellHeader("Type", 1)
    tCell.Width = Unit.Percentage(5)
    tRow.Cells.Add(tCell)

    tCell = GetCellHeader("Vendor", 1)
    tCell.Width = Unit.Percentage(8)
    tRow.Cells.Add(tCell)

    tCell = GetCellHeader("Status", 1)
    tCell.Width = Unit.Percentage(2)
    tRow.Cells.Add(tCell)

    tCell = GetCellHeader("Last Price", 1)
    tCell.Width = Unit.Percentage(5)
    tRow.Cells.Add(tCell)

    tCell = GetCellHeader("Part Number", 2)
    tCell.Width = Unit.Percentage(5)
    tRow.Cells.Add(tCell)

    tCell = GetCellHeaderTableDesc()
    tCell.Width = Unit.Percentage(30) : tCell.Height = Unit.Percentage(100)
    tRow.Cells.Add(tCell)
    tCell = GetCellHeaderTableInvn()
    tCell.Width = Unit.Percentage(20) : tCell.Height = Unit.Percentage(100)
    tRow.Cells.Add(tCell)

    tCell = GetCellHeader("Qty Req", 1)
    tCell.Width = Unit.Percentage(5)
    tRow.Cells.Add(tCell)

    tCell = GetCellHeader("Est Price", 1)
    tCell.Width = Unit.Percentage(5)
    tRow.Cells.Add(tCell)

    tCell = GetCellHeader("Est Resplenish", 1)
    tCell.Width = Unit.Percentage(5)
    tRow.Cells.Add(tCell)

    tblRsp.Rows.Add(tRow)

    tCell.Dispose()
    tCell = Nothing
    tRow.Dispose()
    tRow = Nothing

    bSuccess = True
    Return (bSuccess)
  End Function

    Private Function LoadDetail() As Boolean
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim rcd As OleDb.OleDbDataReader
        Dim bSuccess As Boolean = False
        Dim sSQL As String

        Dim tRow As TableRow
        Dim tCell As TableCell

        Dim iReqDays As Integer
        Dim iAvailDays As Integer
        Dim iQtyRnd As Integer
        Dim sCount As String
        Dim lDocID As Long
        Dim sType As String
        Dim sVendor As String
        Dim sStatus As String
        Dim gCurrCost As Single
        Dim sRefNo As String
        Dim bSchedObs As Boolean
        Dim sRefDesc As String
        Dim sAttr1 As String
        Dim sAttr2 As String
        Dim sAttr3 As String
        Dim sAttr4 As String
        Dim sAttr5 As String
        Dim lCurrQty As Long
        Dim iCurrDays As Integer
        Dim gCurrVal As Single
        Dim lReqQty As Long
        Dim lBEQty As Long
        Dim gFmtPrice As Single
        Dim sReqCostEach As String
        Dim sReqCostTotal As String
        Dim gTotCost As Single = 0
        Dim bShow As Boolean = True

        iReqDays = Convert.ToInt32(txtDays.Text)
        iQtyRnd = Convert.ToInt32(txtRound.Text)

        sSQL = BuildSQL()
        If (sSQL.Length > 0) Then
            MyBase.MyData.GetReader2(cnn, cmd, rcd, sSQL)
            Do While rcd.Read
                lDocID = rcd("DocID")
                sType = rcd("ItmType").ToString
                sVendor = rcd("Vendor").ToString
                sStatus = rcd("Status").ToString
                If IsNumeric(rcd("LastCost").ToString) Then
                    gCurrCost = rcd("LastCost").ToString
                Else
                    gCurrCost = -1
                End If


                bSchedObs = Convert.ToBoolean(rcd("SchedObs"))
                If (Len(rcd("SchedObsDate") & "") > 0) Then
                    iAvailDays = (DateDiff(DateInterval.Day, Now, rcd("SchedObsDate")))
                    'if has not past already and avilable less than calculated days
                    If (iAvailDays > 0) And (iReqDays > iAvailDays) Then
                        iReqDays = iAvailDays
                    End If
                End If
                sRefNo = rcd("PartNo").ToString
                sRefDesc = rcd("PartDesc").ToString
                sAttr1 = rcd("Attr1").ToString
                sAttr2 = rcd("Attr2").ToString
                sAttr3 = rcd("Attr3").ToString
                sAttr4 = rcd("Attr4").ToString
                sAttr5 = rcd("Attr5").ToString
                If IsNumeric(rcd("QtyOH")) Then
                    lCurrQty = rcd("QtyOH")
                Else
                    lCurrQty = -1
                End If
                MyBase.MyData.RPGetInvnRequired(lDocID, lCurrQty, iReqDays, iQtyRnd, iCurrDays, lReqQty)
                'iCurrDays = iCurrDays
                If gCurrCost >= 0 Then
                    gCurrVal = iCurrDays * gCurrCost
                Else
                    gCurrVal = -1
                End If


                lBEQty = rcd("QtyBE")
                gFmtPrice = rcd("FmtPrice")

                MyBase.MyData.RPGetRcmdCosts(sStatus, lReqQty, lBEQty, gFmtPrice, gCurrCost, sReqCostEach, sReqCostTotal, gTotCost, lDocID)



                bShow = (chkExZeroQty.Checked = False) Or _
                        ((chkExZeroQty.Checked = True) And (lReqQty > 0)) Or _
                         gCurrVal < 0
                If (bShow = True) Then
                    tRow = New TableRow

                    tCell = GetCellEntry(sType, 1)
                    tCell.Width = Unit.Percentage(5)
                    tRow.Cells.Add(tCell)

                    tCell = GetCellEntry(sVendor, 1)
                    tCell.Width = Unit.Percentage(8)
                    tRow.Cells.Add(tCell)

                    tCell = GetCellEntry(sStatus, 1)
                    tCell.Width = Unit.Percentage(2)
                    tRow.Cells.Add(tCell)

                    If gCurrCost >= 0 Then
                        tCell = GetCellEntry(Format(gCurrCost, "$ 0.00"), 1, , True)
                    Else
                        tCell = GetCellEntry("N/A", 1, , True)
                    End If

                    tCell.Width = Unit.Percentage(5)
                    tRow.Cells.Add(tCell)

                    tCell = GetCellEntry(sRefNo, 1)
                    tCell.Width = Unit.Percentage(5)
                    tRow.Cells.Add(tCell)

                    If (bSchedObs = True) Then
                        tCell = GetCellEntry("", 1, True)
                    Else
                        tCell = GetCellEntry("", 1, False)
                    End If
                    tCell.Width = Unit.Percentage(2)
                    tRow.Cells.Add(tCell)

                    tCell = GetCellTableDesc(sRefDesc, sAttr1, sAttr2, sAttr3, sAttr4, sAttr5)
                    tCell.Width = Unit.Percentage(30) : tCell.Height = Unit.Percentage(100)
                    tRow.Cells.Add(tCell)

                    tCell = GetCellTableInvn(lCurrQty, iCurrDays.ToString, Format(gCurrVal, "$ 0.000"))
                    tCell.Width = Unit.Percentage(20) : tCell.Height = Unit.Percentage(100)
                    tRow.Cells.Add(tCell)

                    tCell = GetCellEntry(lReqQty, 1, , True)
                    tCell.Width = Unit.Percentage(5)
                    tRow.Cells.Add(tCell)


                    tCell = GetCellEntry(sReqCostEach, 1, , True)
                    tCell.Width = Unit.Percentage(5)
                    tRow.Cells.Add(tCell)

                    tCell = GetCellEntry(sReqCostTotal, 1, , True)
                    tCell.Width = Unit.Percentage(5)
                    tRow.Cells.Add(tCell)

                    tblRsp.Rows.Add(tRow)
                End If
            Loop
            MyBase.MyData.ReleaseReader2(cnn, cmd, rcd)
        End If
        bSuccess = True

        txtTotCost.Text = Format(gTotCost, "$ 0.00")
        'cmdRefresh.Visible = MyBase.CurrentUser.CanRestockPlan
        Try
            tCell.Dispose()
            tCell = Nothing
            tRow.Dispose()
            tRow = Nothing
        Catch ex As Exception

        End Try


        bSuccess = True
        Return (bSuccess)
    End Function

  Private Function GetCellSpace() As TableCell
    Dim tCell As TableCell

    tCell = New TableCell
    tCell.ColumnSpan = 1

    Return tCell
  End Function

  Private Function GetCellHeader(ByVal sVal As String, ByVal iSpan As Integer) As TableCell
    Dim tCell As TableCell

    tCell = New TableCell

    tCell.ColumnSpan = iSpan
    tCell.CssClass = "CellHeader"
    tCell.Text = sVal

    Return tCell
  End Function

  Private Function GetCellEntry(ByVal sVal As String, ByVal iSpan As Integer, Optional ByVal bWarn As Boolean = False, Optional ByVal bNumeric As Boolean = False) As TableCell
    Dim tCell As TableCell

    tCell = New TableCell

    tCell.ColumnSpan = iSpan
    If (bNumeric = True) Then
      tCell.HorizontalAlign = HorizontalAlign.Right
    Else
      tCell.HorizontalAlign = HorizontalAlign.Center
    End If
    If (bWarn = True) Then
      tCell.CssClass = "CellEntryWarn"
    Else
      tCell.CssClass = "CellEntry"
    End If

    tCell.Text = sVal

    Return tCell
  End Function

  Private Function GetCellHeaderTableDesc() As TableCell
    Dim tMainCell As TableCell
    Dim tTable As Table
    Dim tRow As TableRow
    Dim tCell As TableCell

    tMainCell = New TableCell
    tMainCell.Width = Unit.Percentage(100)
    tTable = New Table
    tTable.BorderStyle = BorderStyle.None
    tTable.CellPadding = 0
    tTable.CellSpacing = 0
    tTable.Height = Unit.Percentage(100)
    tTable.Width = Unit.Percentage(100)

    tRow = New TableRow
    tCell = GetCellHeader("Description", 5)
    tCell.Width = Unit.Percentage(100)
    tRow.Cells.Add(tCell)
    tTable.Rows.Add(tRow)

    tRow = New TableRow
    tCell = GetCellHeader("Attr 1", 1)
    tCell.Width = Unit.Percentage(20)
    tRow.Cells.Add(tCell)

    tCell = GetCellHeader("Attr 2", 1)
    tCell.Width = Unit.Percentage(20)
    tRow.Cells.Add(tCell)

    tCell = GetCellHeader("Attr 3", 1)
    tCell.Width = Unit.Percentage(20)
    tRow.Cells.Add(tCell)

    tCell = GetCellHeader("Attr 4", 1)
    tCell.Width = Unit.Percentage(20)
    tRow.Cells.Add(tCell)

    tCell = GetCellHeader("Attr 5", 1)
    tCell.Width = Unit.Percentage(20)
    tRow.Cells.Add(tCell)

    tTable.Rows.Add(tRow)

    tMainCell.Controls.Add(tTable)

    tCell.Dispose()
    tCell = Nothing
    tRow.Dispose()
    tRow = Nothing
    tTable.Dispose()
    tTable = Nothing

    Return tMainCell
  End Function

  Private Function GetCellHeaderTableInvn() As TableCell
    Dim tMainCell As TableCell
    Dim tTable As Table
    Dim tRow As TableRow
    Dim tCell As TableCell

    tMainCell = New TableCell
    tMainCell.Width = Unit.Percentage(100)
    tTable = New Table
    tTable.BorderStyle = BorderStyle.None
    tTable.CellPadding = 0
    tTable.CellSpacing = 0
    tTable.Height = Unit.Percentage(100)
    tTable.Width = Unit.Percentage(100)

    tRow = New TableRow
    tCell = GetCellHeader("Inventory", 3)
    tCell.Width = Unit.Percentage(20)
    tRow.Cells.Add(tCell)
    tTable.Rows.Add(tRow)

    tRow = New TableRow
    tCell = GetCellHeader("Qty", 1)
    tCell.Width = Unit.Percentage(33)
    tRow.Cells.Add(tCell)

    tCell = GetCellHeader("Days", 1)
    tCell.Width = Unit.Percentage(33)
    tRow.Cells.Add(tCell)

    tCell = GetCellHeader("$", 1)
    tCell.Width = Unit.Percentage(34)
    tRow.Cells.Add(tCell)
    tTable.Rows.Add(tRow)

    tMainCell.Controls.Add(tTable)

    tCell.Dispose()
    tCell = Nothing
    tRow.Dispose()
    tRow = Nothing
    tTable.Dispose()
    tTable = Nothing

    Return tMainCell
  End Function

  Private Function GetCellTableDesc(ByVal sRefDesc As String, ByVal sAttr1 As String, ByVal sAttr2 As String, ByVal sAttr3 As String, ByVal sAttr4 As String, ByVal sAttr5 As String) As TableCell
    Dim tMainCell As TableCell
    Dim tTable As Table
    Dim tRow As TableRow
    Dim tCell As TableCell

    tMainCell = New TableCell
    tMainCell.Width = Unit.Percentage(100)
    tTable = New Table
    tTable.BorderStyle = BorderStyle.None
    tTable.CellPadding = 0
    tTable.CellSpacing = 0
    tTable.Height = Unit.Percentage(100)
    tTable.Width = Unit.Percentage(100)

    tRow = New TableRow
    tRow.Cells.Add(GetCellEntry(sRefDesc, 5))
    tTable.Rows.Add(tRow)

    tRow = New TableRow

    tCell = GetCellEntry(sAttr1, 1)
    tCell.Width = Unit.Percentage(20)
    tRow.Cells.Add(tCell)

    tCell = GetCellEntry(sAttr2, 1)
    tCell.Width = Unit.Percentage(20)
    tRow.Cells.Add(tCell)

    tCell = GetCellEntry(sAttr3, 1)
    tCell.Width = Unit.Percentage(20)
    tRow.Cells.Add(tCell)

    tCell = GetCellEntry(sAttr4, 1)
    tCell.Width = Unit.Percentage(20)
    tRow.Cells.Add(tCell)

    tCell = GetCellEntry(sAttr5, 1)
    tCell.Width = Unit.Percentage(20)
    tRow.Cells.Add(tCell)

    tTable.Rows.Add(tRow)

    tMainCell.Controls.Add(tTable)

    tCell.Dispose()
    tCell = Nothing
    tRow.Dispose()
    tRow = Nothing
    tTable.Dispose()
    tTable = Nothing

    Return tMainCell
  End Function

  Private Function GetCellTableInvn(ByVal sQty As String, ByVal sDays As String, ByVal sAmt As String) As TableCell
    Dim tMainCell As TableCell
    Dim tTable As Table
    Dim tRow As TableRow
    Dim tCell As TableCell

    tMainCell = New TableCell
    tMainCell.Width = Unit.Percentage(100)
    tTable = New Table
    tTable.BorderStyle = BorderStyle.None
    tTable.CellPadding = 0
    tTable.CellSpacing = 0
    tTable.Height = Unit.Percentage(100)
    tTable.Width = Unit.Percentage(100)


    tRow = New TableRow

    tCell = GetCellEntry(sQty, 1, , True)
    tCell.Width = Unit.Percentage(33)
    tRow.Cells.Add(tCell)

    tCell = GetCellEntry(sDays, 1)
    tCell.Width = Unit.Percentage(33)
    tRow.Cells.Add(tCell)

    tCell = GetCellEntry(sAmt, 1, , True)
    tCell.Width = Unit.Percentage(34)
    tRow.Cells.Add(tCell)

    tTable.Rows.Add(tRow)

    tMainCell.Controls.Add(tTable)

    tCell.Dispose()
    tCell = Nothing
    tRow.Dispose()
    tRow = Nothing
    tTable.Dispose()
    tTable = Nothing

    Return tMainCell
  End Function

  Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
    Dim sDestin As String = paraPageBase.PAG_AdmRestockPlan.ToString()

    MyBase.PageDirect(sDestin, 0, 0)
  End Sub

End Class
