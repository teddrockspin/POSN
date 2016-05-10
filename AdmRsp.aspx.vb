''''''''''''''''''''
'mw - 04-26-2008
'mw - 09-21-2007
'mw - 08-31-2007
''''''''''''''''''''


Imports System.Text


Partial Class AdmRSP
    Inherits paraPageBase

    Private Const intHeaderHeight As Integer = 30

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents tblCst As System.Web.UI.WebControls.Table
    Protected WithEvents lblRed As System.Web.UI.WebControls.Label
    Protected WithEvents txtItmType As System.Web.UI.WebControls.TextBox

    Private Const TXT_NON = "None"
    Private Const TXT_ATT = "Attributes"
    Private Const TXT_ITM = "Item Type"
    Private Const TXT_PAR = "Part Number"
    Private Const TXT_VEN = "Vendor"
    Private Const TXT_URG = "Urgency"
    Protected WithEvents txtSort1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSort2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSort3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdPrintx As System.Web.UI.WebControls.Button
    Protected WithEvents cmdBackx As System.Web.UI.WebControls.Button
    Protected WithEvents tblRspH As System.Web.UI.WebControls.Table

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()

        MyBase.Page_Init(sender, e)
        '    LoadData()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        PageTitle = "Restock Planner"
        'MyBase.Page_Load(sender, e)
        MyBase.PageMessage = ""

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        ElseIf Not (MyBase.CurrentUser.CanRestockPlan = True) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri

            Try
                LoadBaseData()
            Catch ex As Exception
                Response.Write(ex.ToString())
            End Try


        End If 'End PostBack
    End Sub

    Private Function CurrentSort(ByVal indx As Integer) As String
        Dim sSort As String = String.Empty

        Select Case indx
            Case 1 : sSort = Trim$(cboSort1.SelectedItem.Text)
            Case 2 : sSort = Trim$(cboSort2.SelectedItem.Text)
            Case 3 : sSort = Trim$(cboSort3.SelectedItem.Text)
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

        If Not (cboItmType.SelectedItem Is Nothing) Then
            If Not (cboItmType.SelectedItem.Text = "<None>") Then
                lID = cboItmType.SelectedItem.Value()
            End If
        End If

        Return lID
    End Function

    Private Function CurrentItemType() As String
        Dim sText As String = String.Empty

        If Not (cboItmType.SelectedItem Is Nothing) Then
            If Not (cboItmType.SelectedItem.Text = "<None>") Then
                sText = cboItmType.SelectedItem.Text()
            End If
        End If

        Return sText
    End Function

    Private Function LoadBaseData()
        Dim bSuccess As Boolean = False

        'Try
        If (txtDays.Text.Length = 0) Then
            txtDays.Text = 30
        End If
        If (txtRound.Text.Length = 0) Then
            txtRound.Text = 50
        End If






        LoadItmTypes()
        LoadSortCbos()
        LoadHeader()

        bSuccess = True
        'Catch ex As Exception
        '    Session("PMsg") = "Unable to Load Data..."
        'Response.Write(ex.ToString())
        'End Try

        Return (bSuccess)
    End Function

    Private Function LoadData()
        Dim bSuccess As Boolean = False
        'TODO reenable try
        'Try
        LoadControls()

        ShowControls()

        bSuccess = True
        'Catch ex As Exception
        '    Session("PMsg") = "Unable to Load Data..."
        'End Try

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

            sSelect = "SELECT ActDate,InActDate, Customer_Document.ID AS DocID, " & _
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
                      "Customer_Document.Note AS Attr5, " & _
                      "[QtyOHLocP]+IIf([Customer].[InventoryStatusWarehouse2]='0',[Customer_Document_Fill].[QtyOHLocS],0)+IIf([Customer].[InventoryStatusWarehouse3]='0',[Customer_Document_Fill].[QtyOHLocW],0) AS QtyOH, " & _
                      "iif(ISNULL(Customer_Format.QtyBE),0,Customer_Format.QtyBE) AS QtyBE, " & _
                      "iif(ISNULL(Customer_Format.Price),0,Customer_Format.Price) AS FmtPrice, " & _
                      "Customer_document_fill.Price "

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
                   "on t1.DocID = t2.documentID " & vbCrLf '& _
            ' sOrder

        Else
            sSQL = sSelect + sFrom + sWhere '+ sOrder
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

        cmdRefresh.Visible = MyBase.CurrentUser.CanRestockPlan
    End Function

    Private Function BuildSQLItmType() As String
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty

        sSelect = "SELECT Customer_ItemType.ID, Customer_ItemType.ReferenceNo AS RefName " & _
                  "FROM Customer_ItemType "

        sWhere = "(Customer_ItemType.CustomerID = " & MyBase.CurrentUser.CustomerID & ")"
        If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

        sOrder = " ORDER BY Customer_ItemType.ReferenceNo, Customer_ItemType.Description "

        sSQL = sSelect + sWhere + sOrder

        Return sSQL
    End Function

    Private Function LoadItmTypes(Optional ByVal lID As Long = 0) As Long
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim dr As OleDb.OleDbDataReader
        Dim bSuccess As Boolean = False
        Dim sSQL As String
        Dim sID As String
        Dim sText As String

        cboItmType.Items.Clear()
        cboItmType.Items.Add(New ListItem("<None>", 0))
        cboItmType.Items(0).Attributes.Add("style", "color:" & MyBase.MyData.COLOR_NORMAL)

        sSQL = BuildSQLItmType()
        If (sSQL.Length > 0) Then
            MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
            Do While dr.Read()
                sID = dr("ID").ToString()
                sText = Trim(dr("RefName"))

                cboItmType.Items.Add(New ListItem(sText, sID))
                cboItmType.Items(cboItmType.Items.Count - 1).Attributes.Add("style", "color:" & MyBase.MyData.COLOR_NORMAL)

                If (sID = lID) Then
                    cboItmType.Items(cboItmType.Items.Count - 1).Selected = True
                    'cboItmType.SelectedIndex = cboItmType.Items.Count - 1
                End If
            Loop
            MyBase.MyData.ReleaseReader2(cnn, cmd, dr)

            'SELECT
            Dim indx As Integer
            Dim iMax As Integer

            iMax = cboItmType.Items.Count - 1
            For indx = 0 To iMax
                If (cboItmType.Items(indx).Value = lID) Then
                    cboItmType.SelectedIndex = indx
                End If
            Next

            bSuccess = True
        End If

        Return (bSuccess)
    End Function

    Private Function LoadSortCbos() As Boolean
        cboSort1.Items.Clear()
        cboSort2.Items.Clear()
        cboSort3.Items.Clear()

        cboSort1.Items.Add(New ListItem(TXT_NON, 1))
        cboSort2.Items.Add(New ListItem(TXT_NON, 1))
        cboSort3.Items.Add(New ListItem(TXT_NON, 1))

        cboSort1.Items.Add(New ListItem(TXT_ATT, 2))
        cboSort2.Items.Add(New ListItem(TXT_ATT, 2))
        cboSort3.Items.Add(New ListItem(TXT_ATT, 2))

        cboSort1.Items.Add(New ListItem(TXT_ITM, 3))
        cboSort2.Items.Add(New ListItem(TXT_ITM, 3))
        cboSort3.Items.Add(New ListItem(TXT_ITM, 3))

        cboSort1.Items.Add(New ListItem(TXT_PAR, 4))
        cboSort2.Items.Add(New ListItem(TXT_PAR, 4))
        cboSort3.Items.Add(New ListItem(TXT_PAR, 4))

        cboSort1.Items.Add(New ListItem(TXT_VEN, 5))
        cboSort2.Items.Add(New ListItem(TXT_VEN, 5))
        cboSort3.Items.Add(New ListItem(TXT_VEN, 5))

        cboSort1.Items.Add(New ListItem(TXT_URG, 6))
        cboSort2.Items.Add(New ListItem(TXT_URG, 6))
        cboSort3.Items.Add(New ListItem(TXT_URG, 6))
    End Function

    Private Function LoadHeader() As Boolean

        Dim intMeaningfulWeeks As Integer
        Dim ta As New dsCustomerTableAdapters.CustomerTableAdapter
        For Each dr As dsCustomer.CustomerRow In ta.GetByCustomerID(CurrentUser.CustomerID)
     
            If dr.IsMeaningfulUsageHistoryNull = False Then
                intMeaningfulWeeks = dr.MeaningfulUsageHistory
            End If

        Next

        'FIRST ROW OF THE HEADER:

        Dim tRow1 As TableRow

        tRow1 = New TableRow

        'Dim tCell1 As TableCell
        'tCell1 = GetCellHeader("", 1)
        'tCell1.ColumnSpan = 5
        'tCell1.RowSpan = 2
        'tRow1.Cells.Add(tCell1)

        Dim tCell1 As TableCell
        tCell1 = GetCellHeader("Type", 1)
        tCell1.Width = Unit.Percentage(5)
        tCell1.RowSpan = 2
        tRow1.Cells.Add(tCell1)

        tCell1 = GetCellHeader("Vendor", 1)
        tCell1.Width = Unit.Percentage(8)
        tCell1.RowSpan = 2
        tRow1.Cells.Add(tCell1)

        tCell1 = GetCellHeader("Status", 1)
        tCell1.Width = Unit.Percentage(2)
        tCell1.RowSpan = 2
        tRow1.Cells.Add(tCell1)


        tCell1 = GetCellHeader("Part Number", 1)
        tCell1.Width = Unit.Percentage(5)
        tCell1.RowSpan = 2
        tRow1.Cells.Add(tCell1)


        tCell1 = GetCellHeader("Description", 1)
        tCell1.Width = Unit.Percentage(5)
        tCell1.RowSpan = 2
        tRow1.Cells.Add(tCell1)

        Dim tCell2 As TableCell
        tCell2 = GetCellHeader("Specifications", 5, "HeaderStyle2")
        tCell2.ID = "tCell2Specifications"
        tRow1.Cells.Add(tCell2)

        Dim tCellUsageHeader As TableCell
        tCellUsageHeader = GetCellHeader("Usage (Prev. " & intMeaningfulWeeks & ") Wks", 5, "HeaderStyle3")
        tCellUsageHeader.ID = "tCellUsageHeader"
        tRow1.Cells.Add(tCellUsageHeader)

        tCell2 = GetCellHeader("Inventory", 4, "HeaderStyle4")

        tRow1.Cells.Add(tCell2)

        Dim tCell3 As TableCell
        tCell3 = GetCellHeader("Requirement", 3, "HeaderStyle5")
        tRow1.Cells.Add(tCell3)


        tblRsp.Rows.Add(tRow1)


        'SECOND ROW:

        Dim tRow As TableRow
        Dim tCell As TableCell

        Dim bSuccess As Boolean = False

        tRow = New TableRow

        ''tCell = GetCellHeader("Type", 1)
        ''tCell.Width = Unit.Percentage(5)
        ''tRow.Cells.Add(tCell)

        ''tCell = GetCellHeader("Vendor", 1)
        ''tCell.Width = Unit.Percentage(8)
        ''tRow.Cells.Add(tCell)

        ''tCell = GetCellHeader("Status", 1)
        ''tCell.Width = Unit.Percentage(2)
        ''tRow.Cells.Add(tCell)


        ''tCell = GetCellHeader("Part Number", 1)
        ''tCell.Width = Unit.Percentage(5)
        ''tRow.Cells.Add(tCell)


        ''tCell = GetCellHeader("Description", 1)
        ''tCell.Width = Unit.Percentage(5)
        ''tRow.Cells.Add(tCell)
        ''?
        ''Dim tCellType5 As TableCell
        ''tCellType5 = GetCellHeader("", 1)
        ''tCellType5.Width = Unit.Percentage(5)
        ''tRow.Cells.Add(tCellType5)

        'Dim tCell2 As TableCell
        'tCell2 = GetCellHeader("Specifications", 1)
        'tCell2.Width = Unit.Percentage(5)
        'tCell2.ColumnSpan = 5
        'tRow.Cells.Add(tCell2)


        tCell = GetCellHeader("Attr 1", 1, "HeaderStyle2")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)
        tCell = GetCellHeader("Attr 2", 1, "HeaderStyle2")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)
        tCell = GetCellHeader("Attr 3", 1, "HeaderStyle2")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)
        tCell = GetCellHeader("Attr 4", 1, "HeaderStyle2")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)
        tCell = GetCellHeader("Attr 5", 1, "HeaderStyle2")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)


        'tCell = GetCellHeaderTableDesc()
        'tCell.Width = Unit.Percentage(30) : tCell.Height = intHeaderHeight
        'tCell.BorderStyle = BorderStyle.None
        'tRow.Cells.Add(tCell)


        'tCell = GetCellHeaderTableUsage()
        'tCell.Width = Unit.Percentage(20) : tCell.Height = intHeaderHeight
        'tCell.BorderStyle = BorderStyle.None
        'tRow.Cells.Add(tCell)


        tCell = GetCellHeader("Active Date", 1, "HeaderStyle3")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Scheduled Obs Date", 1, "HeaderStyle3")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Qty", 1, "HeaderStyle3")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Requests", 1, "HeaderStyle3")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Per Week", 1, "HeaderStyle3")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Qty", 1, "HeaderStyle4")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)


        tCell = GetCellHeader("B.O.", 1, "HeaderStyle4")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Weeks", 1, "HeaderStyle4")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Value $", 1, "HeaderStyle4")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)

        'tCell = GetCellHeaderTableInvn()
        'tCell.Width = Unit.Percentage(20) : tCell.Height = intHeaderHeight
        'tCell.BorderStyle = BorderStyle.None
        'tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Qty Req", 1, "HeaderStyle5")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Method", 1, "HeaderStyle5")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Est Cost", 1, "HeaderStyle5")
        tCell.Width = Unit.Percentage(5)
        tRow.Cells.Add(tCell)

        tblRsp.Rows.Add(tRow)

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
        Dim ds As New DataSet
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
        Dim intMeaningfulWeeks As Integer
        Dim intMeaningfulOrderQty As Integer
        Dim intStockWarningPieces As Integer = -1


        Dim ta As New dsCustomerTableAdapters.CustomerTableAdapter


        'Select Case Combo(cStockWarningType).ListIndex
        '    Case 0
        '        lbl(32).Visible = True
        '        txt(28).Visible = True
        '    Case 1
        '        lbl(32).Visible = False
        '        txt(28).Visible = False
        'End Select

        For Each dr As dsCustomer.CustomerRow In ta.GetByCustomerID(CurrentUser.CustomerID)
            If dr.IsMeaningfulOrderQtyNull = False Then
                intMeaningfulOrderQty = dr.MeaningfulOrderQty
            End If
            If dr.IsMeaningfulUsageHistoryNull = False Then
                intMeaningfulWeeks = dr.MeaningfulUsageHistory
            End If
            '0 - weeks 1 - pieces
            Select Case dr.StockWarningType
                Case 0
                    If dr.IsStockWarningPiecesNull = False AndAlso IsNumeric(dr.StockWarningPieces) Then
                        intStockWarningPieces = dr.StockWarningPieces
                    End If
                Case 1
                    intStockWarningPieces = dr.StockWarn
            End Select

        Next


        Dim dtQtyUsage As Date = Now.Date.AddDays(-intMeaningfulWeeks * 7)


        Dim dt As New DataTable("Items")

        Dim dc As New DataColumn
        dc.DataType = System.Type.GetType("System.String")
        dc.ColumnName = "sType"
        dt.Columns.Add(dc)

        Dim dcV As New DataColumn
        dcV.DataType = System.Type.GetType("System.String")
        dcV.ColumnName = "sVendor"
        dt.Columns.Add(dcV)

        Dim dcS As New DataColumn
        dcS.DataType = System.Type.GetType("System.String")
        dcS.ColumnName = "sStatus"
        dt.Columns.Add(dcS)

        Dim dcLP As New DataColumn
        dcLP.DataType = System.Type.GetType("System.String")
        dcLP.ColumnName = "gCurrCost"
        dt.Columns.Add(dcLP)

        Dim dcP As New DataColumn
        dcP.DataType = System.Type.GetType("System.String")
        dcP.ColumnName = "sPartNumber"
        dt.Columns.Add(dcP)

        Dim dcDesc As New DataColumn
        dcDesc.DataType = System.Type.GetType("System.String")
        dcDesc.ColumnName = "sRefDesc"
        dt.Columns.Add(dcDesc)

        Dim dcA1 As New DataColumn
        dcA1.DataType = System.Type.GetType("System.String")
        dcA1.ColumnName = "sAttr1"
        dt.Columns.Add(dcA1)

        Dim dcA2 As New DataColumn
        dcA2.DataType = System.Type.GetType("System.String")
        dcA2.ColumnName = "sAttr2"
        dt.Columns.Add(dcA2)

        Dim dcA3 As New DataColumn
        dcA3.DataType = System.Type.GetType("System.String")
        dcA3.ColumnName = "sAttr3"
        dt.Columns.Add(dcA3)

        Dim dcA4 As New DataColumn
        dcA4.DataType = System.Type.GetType("System.String")
        dcA4.ColumnName = "sAttr4"
        dt.Columns.Add(dcA4)

        Dim dcA5 As New DataColumn
        dcA5.DataType = System.Type.GetType("System.String")
        dcA5.ColumnName = "sAttr5"
        dt.Columns.Add(dcA5)

        Dim dcInv As New DataColumn
        dcInv.DataType = System.Type.GetType("System.String")
        dcInv.ColumnName = "lCurrQty"
        dt.Columns.Add(dcInv)

        Dim dcDays As New DataColumn
        dcDays.DataType = System.Type.GetType("System.String")
        dcDays.ColumnName = "iCurrDays"
        dt.Columns.Add(dcDays)

        Dim dcD As New DataColumn
        dcD.DataType = System.Type.GetType("System.String")
        dcD.ColumnName = "gCurrVal"
        dt.Columns.Add(dcD)

        Dim dcQtyReq As New DataColumn
        dcQtyReq.DataType = System.Type.GetType("System.String")
        'dcQtyReq.DefaultValue = "0"
        dcQtyReq.ColumnName = "lReqQty"
        dt.Columns.Add(dcQtyReq)

        Dim dcEstPrice As New DataColumn
        dcEstPrice.DataType = System.Type.GetType("System.String")
        dcEstPrice.ColumnName = "sReqCostEach"
        dt.Columns.Add(dcEstPrice)


        Dim dcEstReplenish As New DataColumn
        dcEstReplenish.DataType = System.Type.GetType("System.String")
        dcEstReplenish.ColumnName = "sReqCostTotal"
        dt.Columns.Add(dcEstReplenish)


        Dim dcActDate As New DataColumn
        dcActDate.DataType = System.Type.GetType("System.DateTime")
        dcActDate.ColumnName = "ActDate"
        dt.Columns.Add(dcActDate)

        Dim dcSchedObsDate As New DataColumn
        dcSchedObsDate.DataType = System.Type.GetType("System.DateTime")
        dcSchedObsDate.ColumnName = "SchedObsDate"
        dt.Columns.Add(dcSchedObsDate)

        Dim dcQtyUsage As New DataColumn
        dcQtyUsage.DataType = System.Type.GetType("System.String")
        dcQtyUsage.ColumnName = "QtyUsage"
        dt.Columns.Add(dcQtyUsage)

        Dim dcRequests As New DataColumn
        dcRequests.DataType = System.Type.GetType("System.Int32")
        dcRequests.ColumnName = "Requests"
        dt.Columns.Add(dcRequests)


        Dim dcPerWeek As New DataColumn
        dcPerWeek.DataType = System.Type.GetType("System.String")
        dcPerWeek.ColumnName = "PerWeek"
        dt.Columns.Add(dcPerWeek)


        Dim dcQTYBCK As New DataColumn
        dcQTYBCK.DataType = System.Type.GetType("System.Int32")
        dcQTYBCK.ColumnName = "QTYBCK"
        dt.Columns.Add(dcQTYBCK)

        'Dim dcBwarn As New DataColumn
        'dcBwarn.DataType = System.Type.GetType("System.Boolean")
        'dcBwarn.DefaultValue = False
        'dcBwarn.ColumnName = "bwarn"
        'dt.Columns.Add(dcBwarn)

        ds.Tables.Add(dt)
        '---
        iReqDays = Convert.ToInt32(txtDays.Text)
        iQtyRnd = Convert.ToInt32(txtRound.Text)

        sSQL = BuildSQL()
        If (sSQL.Length > 0) Then
            MyBase.MyData.GetReader2(cnn, cmd, rcd, sSQL)
            Do While rcd.Read

                'Try


                lDocID = rcd("DocID")
                sType = rcd("ItmType").ToString
                sVendor = rcd("Vendor").ToString
                sStatus = rcd("Status").ToString



                If (ddlStatusFilter.SelectedIndex > 0 AndAlso sStatus = ddlStatusFilter.SelectedValue) Or ddlStatusFilter.SelectedIndex = 0 Then



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
                    'MyBase.MyData.RPGetInvnRequired(lDocID, lCurrQty, iReqDays, iQtyRnd, iCurrDays, lReqQty)
                    'iCurrDays = iCurrDays
                    If gCurrCost >= 0 Then
                        gCurrVal = iCurrDays * gCurrCost
                    Else
                        gCurrVal = -1
                    End If


                    lBEQty = rcd("QtyBE")
                    gFmtPrice = rcd("FmtPrice")

                    'MyBase.MyData.RPGetRcmdCosts(sStatus, lReqQty, lBEQty, gFmtPrice,  gCurrCost, sReqCostEach, sReqCostTotal, gTotCost, lDocID)



                    bShow = (chkExZeroQty.Checked = False) Or _
                            ((chkExZeroQty.Checked = True)) Or _
                             gCurrVal < 0

                    If rcd("InActDate") Is DBNull.Value = False Then
                        If DateDiff(DateInterval.Day, rcd("InActDate"), Now.Date) < 0 Then
                            bShow = False
                        End If
                    End If


                    If intStockWarningPieces >= 0 And intStockWarningPieces < lCurrQty And chkExZeroQty.Checked Then
                        bShow = False
                    End If

                    If (bShow = True) Then
                        '---------
                        Dim Dr As DataRow

                        Dr = dt.NewRow

                        Dr.Item("sType") = sType
                        Dr.Item("sVendor") = sVendor
                        Dr.Item("sStatus") = sStatus
                        If gCurrCost >= 0 Then
                            Dr.Item("gCurrCost") = Format(gCurrCost, "$ 0.00")
                        Else
                            Dr.Item("gCurrCost") = "N/A"
                        End If
                        Dr.Item("sPartNumber") = sRefNo
                        Dr.Item("sRefDesc") = sRefDesc
                        Dr.Item("sAttr1") = sAttr1
                        Dr.Item("sAttr2") = sAttr2
                        Dr.Item("sAttr3") = sAttr3
                        Dr.Item("sAttr4") = sAttr4
                        Dr.Item("sAttr5") = sAttr5

                        '--new fields:
                        Dr.Item("ActDate") = rcd("ActDate")
                        Dr.Item("SchedObsDate") = rcd("SchedObsDate")
                        Dim intQtyUsage As Integer = qa.GetQtyUsage(lDocID, dtQtyUsage)
                        Dim intRequests As Integer = qa.GetRequestCountMeaningful(lDocID, dtQtyUsage)
                        Dr.Item("QtyUsage") = intQtyUsage '& " " & lDocID & " " & dtQtyUsage
                        Dr.Item("Requests") = intRequests

                        'Per week
                        If rcd("ActDate") Is DBNull.Value = False AndAlso DateDiff(DateInterval.Day, rcd("ActDate"), Now.Date) < intMeaningfulWeeks * 7 Then
                            Dr.Item("PerWeek") = "Insufficient History"
                        ElseIf intQtyUsage = 0 Then
                            Dr.Item("PerWeek") = "No Usage"
                        ElseIf intRequests < intMeaningfulOrderQty Then
                            Dr.Item("PerWeek") = "Insufficient Data"
                        Else
                            Dr.Item("PerWeek") = Math.Round(intQtyUsage / intMeaningfulWeeks, 1) 'DateDiff(DateInterval.Day, rcd("ActDate"), Now.Date) & " " & rcd("ActDate")
                        End If

                        Dim sTest As Decimal = (2 * intQtyUsage / intMeaningfulWeeks * txtDays.Text / 7)

                        If CInt(lCurrQty) >= 0 Then
                            'FUL
                            Dr.Item("lCurrQty") = lCurrQty 'stays the same
                            Dr.Item("gCurrVal") = Format(gCurrVal, "Currency")

                            'If intStockWarningPieces >= 0 And intStockWarningPieces < lCurrQty And intQtyUsage = 0 And intRequests < intMeaningfulOrderQty And sStatus = "FUL" And lCurrQty > sTest Then
                            '    Dr.Item("iCurrDays") = "Sufficient Supply"
                            'Else

                            'If intQtyUsage > 0 And intRequests < intMeaningfulOrderQty And lCurrQty >= sTest Then
                            '    Dr.Item("iCurrDays") = "Sufficient Supply"
                            'ElseIf (sStatus = "FUL") And intRequests < intMeaningfulOrderQty And intQtyUsage > 0 And lCurrQty = 0 Then
                            '    Dr.Item("iCurrDays") = "Needs Urgent Evaluation"
                            'ElseIf (sStatus <> "FUL") And intRequests < intMeaningfulOrderQty And intQtyUsage > 0 And lCurrQty = 0 Then
                            '    Dr.Item("iCurrDays") = "Needs Evaluation"
                            'ElseIf (sStatus = "FUL" Or sStatus = "F/P") And (intRequests < intMeaningfulOrderQty Or intQtyUsage = 0) Then 'And lCurrQty = 0
                            '    Dr.Item("iCurrDays") = "Needs Evaluation"
                            'ElseIf intQtyUsage > 0 And intRequests < intMeaningfulOrderQty And lCurrQty < sTest Then
                            '    Dr.Item("iCurrDays") = "Needs Evaluation"
                            'ElseIf intQtyUsage > 0 And intRequests >= intMeaningfulOrderQty Then
                            '    Dr.Item("iCurrDays") = Math.Round(lCurrQty / (intQtyUsage / intMeaningfulWeeks), 1)
                            'Else
                            '    Dr.Item("iCurrDays") = "?"
                            'End If
                        Else
                            'POD
                            Dr.Item("lCurrQty") = "N/A"
                            Dr.Item("iCurrDays") = "N/A"
                            Dr.Item("gCurrVal") = "N/A"
                        End If

                        Dim pd As New paraData

                        'Response.Write(pd.GetSupply(lCurrQty, intQtyUsage, intRequests, intMeaningfulWeeks, intMeaningfulOrderQty, sStatus, intStockWarningPieces, txtDays.Text) & "<br>")

                        Dr.Item("iCurrDays") = pd.GetSupply(lCurrQty, intQtyUsage, intRequests, intMeaningfulWeeks, intMeaningfulOrderQty, sStatus, intStockWarningPieces, txtDays.Text)

                        Dr.Item("sReqCostEach") = "?"
                        Dr.Item("sReqCostTotal") = "?"

                        'QTY req
                        If intStockWarningPieces >= 0 And intStockWarningPieces < lCurrQty And intQtyUsage = 0 And intRequests < intMeaningfulOrderQty And (sStatus = "FUL" Or sStatus = "F/P") And lCurrQty > sTest Then
                            Dr.Item("lReqQty") = "0"
                            Dr.Item("iCurrDays") = "Sufficient Supply"
                        ElseIf intQtyUsage > 0 And intRequests < intMeaningfulOrderQty And lCurrQty >= sTest Then
                            Dr.Item("lReqQty") = 0
                        ElseIf intRequests < intMeaningfulOrderQty And intQtyUsage > 0 And lCurrQty = 0 And sStatus = "FUL" Then
                            Dr.Item("lReqQty") = "<font color=red>Needs Urgent Evaluation</font>"
                        ElseIf intRequests < intMeaningfulOrderQty And intQtyUsage > 0 And lCurrQty = 0 And sStatus <> "FUL" Then
                            Dr.Item("lReqQty") = "Needs Evaluation"
                        ElseIf (intRequests < intMeaningfulOrderQty Or intQtyUsage = 0) And sStatus = "FUL" Then 'And (lCurrQty = 0 Or lCurrQty = -1) 
                            Dr.Item("lReqQty") = "Needs Evaluation"
                        ElseIf (intRequests < intMeaningfulOrderQty Or intQtyUsage = 0) And sStatus <> "FUL" Then '(lCurrQty = 0 Or lCurrQty = -1) And 
                            Dr.Item("lReqQty") = 0
                        ElseIf intQtyUsage > 0 And intRequests < intMeaningfulOrderQty And lCurrQty < sTest Then
                            Dr.Item("lReqQty") = "Needs Evaluation"
                        ElseIf intQtyUsage > 0 And intRequests >= intMeaningfulOrderQty Then
                            Dim decTmp As Decimal
                            If rcd("SchedObsDate") Is DBNull.Value = False Then
                                If DateDiff(DateInterval.Day, rcd("SchedObsDate"), Now.Date.AddDays(txtDays.Text)) > 0 And DateDiff(DateInterval.Day, rcd("SchedObsDate"), Now.Date) < 0 Then
                                    decTmp = Math.Round(((DateDiff(DateInterval.Day, Now.Date, rcd("SchedObsDate"))) / 7) * intQtyUsage / intMeaningfulWeeks - lCurrQty)
                                Else
                                    decTmp = Math.Round((txtDays.Text / 7) * intQtyUsage / intMeaningfulWeeks - lCurrQty)
                                End If
                            Else
                                Try


                                    If rcd("Actdate") Is DBNull.Value = False AndAlso DateDiff(DateInterval.Day, rcd("ActDate"), Now.Date) < intMeaningfulWeeks * 7 Then
                                        decTmp = Math.Round((txtDays.Text / 7) * intQtyUsage / DateDiff(DateInterval.Day, rcd("ActDate"), Now.Date) * 7 - lCurrQty)
                                    Else
                                        decTmp = Math.Round((txtDays.Text / 7) * intQtyUsage / intMeaningfulWeeks - lCurrQty)
                                    End If
                                Catch ex As Exception
                                    Response.Write(ex.Message & "<BR>")
                                End Try

                            End If

                            If decTmp >= 0 Then
                                Dr.Item("lReqQty") = decTmp
                            Else
                                Dr.Item("lReqQty") = 0
                            End If
                        ElseIf (intRequests < intMeaningfulOrderQty Or intQtyUsage = 0) And (lCurrQty > 0) Then
                            Dr.Item("lReqQty") = "Needs Evaluation"
                        Else

                            Dr.Item("lReqQty") = "<font color=red>?</font>"
                        End If

                        If IsNumeric(Dr.Item("lReqQty")) And IsNumeric(txtRound.Text) Then
                            lReqQty = Dr.Item("lReqQty")
                            lReqQty = Math.Round(lReqQty / txtRound.Text) * txtRound.Text
                            Dr.Item("lReqQty") = lReqQty
                        End If

                        'IF pod then price is price of the format
                        'break even is in format 
                        'last cost is in document table
                        'POD price - format price 
                        Dim decLastCost As Decimal
                        If rcd("LastCost") Is DBNull.Value Then
                            decLastCost = 0
                        Else
                            decLastCost = rcd("LastCost")
                        End If

                        'ESTIMATED PRICE
                        If IsNumeric(Dr.Item("lReqQty")) = False Then
                            Dr.Item("sReqCostTotal") = 0
                        ElseIf IsNumeric(Dr.Item("lReqQty")) And Dr.Item("lReqQty") = 0 Then
                            Dr.Item("sReqCostTotal") = 0
                        ElseIf Dr.Item("lReqQty") > 0 And sStatus <> "FUL" Then
                            'Dr.Item("sReqCostTotal")  =
                            If Dr.Item("lReqQty") >= rcd("QtyBE") And rcd("FmtPrice") > 0 Then
                                Dr.Item("sReqCostTotal") = Math.Round(rcd("FmtPrice") * Dr.Item("lReqQty"), 2)
                            ElseIf Dr.Item("lReqQty") >= rcd("QtyBE") And decLastCost > 0 Then
                                Dr.Item("sReqCostTotal") = Math.Round(decLastCost * Dr.Item("lReqQty"), 2)
                            ElseIf Dr.Item("lReqQty") < rcd("QtyBE") Then
                                Dr.Item("sReqCostTotal") = Math.Round(rcd("FmtPrice") * Dr.Item("lReqQty"), 2)
                            ElseIf rcd("FmtPrice") = 0 Then
                                Dr.Item("sReqCostTotal") = 0
                            End If
                        ElseIf sStatus = "FUL" And Dr.Item("lReqQty") > 0 Then '  sstatus<>"POD"
                            If Dr.Item("lReqQty") >= rcd("QtyBE") And rcd("FmtPrice") > 0 Then
                                Dr.Item("sReqCostTotal") = Math.Round(rcd("FmtPrice") * Dr.Item("lReqQty"), 2)
                            ElseIf Dr.Item("lReqQty") >= rcd("QtyBE") And decLastCost > 0 Then
                                Dr.Item("sReqCostTotal") = Math.Round(decLastCost * Dr.Item("lReqQty"), 2)
                            Else
                                Dr.Item("sReqCostTotal") = 0
                            End If
                        End If
                        Dr.Item("sReqCostTotal") = Format(Dr.Item("sReqCostTotal"), "{0:C}")


                        Dim strSuggestedFormat As String
                        'suggested format:
                        If IsNumeric(Dr.Item("lReqQty")) = False Then
                            strSuggestedFormat = ""
                        ElseIf IsNumeric(Dr.Item("lReqQty")) And Dr.Item("lReqQty") = 0 Then
                            strSuggestedFormat = ""
                        ElseIf Dr.Item("lReqQty") > 0 And sStatus <> "FUL" Then
                            'Dr.Item("sReqCostTotal")  =
                            If Dr.Item("lReqQty") >= rcd("QtyBE") Then
                                strSuggestedFormat = "Offset"
                            Else
                                strSuggestedFormat = "POD"
                            End If
                        ElseIf sStatus = "FUL" And Dr.Item("lReqQty") > 0 Then '  sstatus<>"POD"
                            strSuggestedFormat = "Restock"
                        End If

                        Dr.Item("sReqCostEach") = strSuggestedFormat



                        Dim blnAddToTable As Boolean = False

                        'TODO EXCLUDE ITEMS:

                        'intStockWarningPieces >= 0 And intStockWarningPieces < lCurrQty And intQtyUsage = 0 And intRequests < intMeaningfulOrderQty And sStatus = "FUL" And lCurrQty > sTest Then


                        If (txtShowItems.Text <> "" AndAlso IsNumeric(txtShowItems.Text) AndAlso IsNumeric(Dr.Item("iCurrDays")) AndAlso Dr.Item("iCurrDays") <= CInt(txtShowItems.Text)) _
                           Or (Dr.Item("lReqQty") = "0" And Dr.Item("iCurrDays") = "Sufficient Supply" And (sStatus = "FUL" Or sStatus = "F/P")) Then
                            blnAddToTable = True
                        ElseIf IsNumeric(Dr.Item("iCurrDays")) = False AndAlso (Dr.Item("iCurrDays") = "Needs Evaluation" Or Dr.Item("iCurrDays") = "Needs Urgent Evaluation") Then
                            blnAddToTable = True
                        ElseIf txtShowItems.Text = "" Then
                            blnAddToTable = True
                        Else
                            blnAddToTable = False
                        End If

                        If chkExZeroQty.Checked = True And blnAddToTable = True Then
                            If (Dr.Item("iCurrDays") = "Sufficient Supply" And IsNumeric(Dr.Item("lReqQty")) = False And Dr.Item("lReqQty") <> "") Or (IsNumeric(Dr.Item("lReqQty")) AndAlso CDec(Dr.Item("lReqQty")) = 0) Then
                                blnAddToTable = False
                            Else
                                blnAddToTable = True
                            End If
                        End If




                        If blnAddToTable Then
                            Dim intQtyBck As Object = qa.GetQtyBackOrder(lDocID)
                            If intQtyBck Is DBNull.Value Then
                                Dr.Item("QTYBCK") = 0
                            Else
                                Dr.Item("QTYBCK") = CInt(intQtyBck)
                            End If


                            If IsNumeric(Dr.Item("sReqCostTotal")) Then
                                gTotCost += Dr.Item("sReqCostTotal")
                            End If

                            dt.Rows.Add(Dr)
                        End If

                    End If
                End If
                'Catch ex As Exception
                '    Dim Dr As DataRow
                '    Dr = dt.NewRow

                '    Dr.Item("sPartNumber") = ex.Message
                '    dt.Rows.Add(Dr)
                'End Try
            Loop
            MyBase.MyData.ReleaseReader2(cnn, cmd, rcd)
        End If


        '--get sort





        Dim sSort1 As String = GetSort(cboSort1.SelectedValue)
        Dim sSort2 As String = GetSort(cboSort2.SelectedValue)
        Dim sSort3 As String = GetSort(cboSort3.SelectedValue)

        Dim sSort As String = sSort1 & sSort2 & sSort3
        sSort = sSort.TrimEnd(" ")
        If sSort <> "" Then
            sSort = sSort.Remove(sSort.Length - 1)
        End If

        ds.Tables(0).DefaultView.Sort = sSort


        Dim dv As New DataView(dt)
        dv.Sort = sSort


        '-----


        For Each dr1 As DataRow In dv.ToTable.Rows   'ds.Tables(0).DefaultView.Table.Rows

            With dr1

                'Try

                tRow = New TableRow
                tCell = GetCellEntry(.Item("sType"), 1)
                tCell.Width = Unit.Percentage(5)
                tCell.Height = 15
                tRow.Cells.Add(tCell)

                tCell = GetCellEntry(.Item("sVendor"), 1)
                tCell.Width = Unit.Percentage(8)
                tRow.Cells.Add(tCell)

                tCell = GetCellEntry(.Item("sStatus"), 1)
                tCell.Width = Unit.Percentage(2)
                tRow.Cells.Add(tCell)

                'tCell = GetCellEntry(.Item("gcurrcost"), 1, , True)
                'tCell.Width = Unit.Percentage(5)
                'tRow.Cells.Add(tCell)

                tCell = GetCellEntry(.Item("sPartNumber"), 1)
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)

                tCell = GetCellEntry(.Item("sRefDesc"), 1, , , True)
                tCell.CssClass = "BorderRight"
                tCell.Width = Unit.Percentage(15)
                tRow.Cells.Add(tCell)

                'TODO temp disabled Scheduled for Obsolescence

                'If (bSchedObs = True) Then:C
                '    tCell = GetCellEntry("", 1, True)
                'Else
                '    tCell = GetCellEntry("", 1, False)
                'End If
                'tCell.Width = Unit.Percentage(2)
                'tRow.Cells.Add(tCell)

                'tCell = GetCellTableDesc(.Item("sRefDesc"), .Item("sAttr1"), .Item("sAttr2"), .Item("sAttr3"), .Item("sAttr4"), .Item("sAttr5"))
                'tCell.Width = Unit.Percentage(30) : tCell.Height = Unit.Percentage(100)
                'tRow.Cells.Add(tCell)

                tCell = GetCellEntry(.Item("sAttr1"), 1)
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)

                tCell = GetCellEntry(.Item("sAttr2"), 1)
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)

                tCell = GetCellEntry(.Item("sAttr3"), 1)
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)

                tCell = GetCellEntry(.Item("sAttr4"), 1)
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)

                tCell = GetCellEntry(.Item("sAttr5"), 1, , , True)
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)

                'TODO usage prev weeks:
                Dim dtSchedObsDate As String
                Try
                    dtSchedObsDate = .Item("SchedObsDate")
                Catch ex As Exception
                    dtSchedObsDate = ""
                End Try
                Dim dtActDate As String
                Try
                    dtActDate = .Item("ActDate")
                Catch ex As Exception
                    dtActDate = ""
                End Try

                'tCell = GetCellTableUsage(dtActDate, dtSchedObsDate.ToString, .Item("QtyUsage"), .Item("Requests"), .Item("PerWeek"))
                'tCell.Width = Unit.Percentage(30) : tCell.Height = Unit.Percentage(100)
                'tRow.Cells.Add(tCell)

                tCell = GetCellEntry(dtActDate, 1)
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)

                tCell = GetCellEntry(dtSchedObsDate.ToString, 1)
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)

                tCell = GetCellEntry(.Item("QtyUsage").ToString, 1)
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)


                tCell = GetCellEntry(.Item("Requests"), 1)
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)

                tCell = GetCellEntry(.Item("PerWeek"), 1, , , True)
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)

                'tCell = GetCellTableInvn(.Item("lCurrQty"), .Item("iCurrDays").ToString, .Item("gCurrVal"))
                'tCell.Width = Unit.Percentage(20) : tCell.Height = Unit.Percentage(100)
                'tRow.Cells.Add(tCell)

                tCell = GetCellEntry(.Item("lCurrQty"), 1)
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)

                tCell = GetCellEntry(.Item("QTYBCK"), 1)
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)

                tCell = GetCellEntry(.Item("iCurrDays").ToString, 1)
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)

                tCell = GetCellEntry(.Item("gCurrVal"), 1, , , True)
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)


                tCell = GetCellEntry(.Item("lReqQty"), 1, , True)
                tCell.Width = Unit.Percentage(5)
                tCell.CssClass = "DetailStyle5"
                tRow.Cells.Add(tCell)


                tCell = GetCellEntry(.Item("sReqCostEach"), 1, , True)
                tCell.Width = Unit.Percentage(5)
                tCell.CssClass = "DetailStyle5"
                tRow.Cells.Add(tCell)

                Dim strEstCost = Format(.Item("sReqCostTotal"), "Currency")
                tCell = GetCellEntry(strEstCost, 1, , , True)
                tCell.CssClass = "DetailStyle5"
                tCell.Width = Unit.Percentage(5)
                tRow.Cells.Add(tCell)

                tblRsp.Rows.Add(tRow)
                'Catch ex As Exception

                'End Try
            End With
        Next

        bSuccess = True

        txtTotCost.Text = Format(gTotCost, "$0.00")
        cmdRefresh.Visible = MyBase.CurrentUser.CanRestockPlan
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

    Function GetSort(ByVal intSortOption As Integer) As String
        '<select name="cboSort1" id="cboSort1" class="cboShort">
        '<option selected="selected" value="1">None</option>
        '<option value="2">Attributes</option>
        '<option value="3">Item Type</option>
        '<option value="4">Part Number</option>
        '<option value="5">Vendor</option>
        '<option value="6">Urgency</option>

        Dim strTmp As String

        Select Case intSortOption
            Case 1
                strTmp = ""
            Case 2
                strTmp = " sAttr1 , sattr2, sattr3, sattr4, sattr5 "
            Case 3
                strTmp = " sType "
            Case 4
                strTmp = " sPartNumber "
            Case 5
                strTmp = " sVendor "
            Case 6
                strTmp = " iCurrDays ASC"
            Case Else
                strTmp = ""
        End Select

        If strTmp <> "" Then
            strTmp += " , "
        End If

        Return strTmp
    End Function

    Private Function GetCellSpace() As TableCell
        Dim tCell As TableCell

        tCell = New TableCell
        tCell.ColumnSpan = 1

        Return tCell
    End Function

    Private Function GetCellHeader(ByVal sVal As String, ByVal iSpan As Integer, Optional ByVal strCssClass As String = "CellHeader") As TableCell
        Dim tCell As TableCell

        tCell = New TableCell

        tCell.ColumnSpan = iSpan
        tCell.CssClass = strCssClass
        tCell.Text = sVal

        Return tCell
    End Function

    Private Function GetCellEntry(ByVal sVal As String, ByVal iSpan As Integer, Optional ByVal bWarn As Boolean = False, Optional ByVal bNumeric As Boolean = False, Optional ByVal blnBorderRight As Boolean = False) As TableCell
        Dim tCell As TableCell

        tCell = New TableCell

        tCell.ColumnSpan = iSpan
        'If (bNumeric = True) Then
        '    tCell.HorizontalAlign = HorizontalAlign.Right
        'Else
        tCell.HorizontalAlign = HorizontalAlign.Center
        'End If
        If (bWarn = True) Then
            If blnBorderRight Then
                tCell.CssClass = "CellEntryWarn BorderRight"
            Else
                tCell.CssClass = "CellEntryWarn"
            End If

        Else
            If blnBorderRight Then
                tCell.CssClass = "CellEntry BorderRight"
            Else
                tCell.CssClass = "CellEntry"
            End If
        End If

   

        tCell.Text = sVal

        Return tCell
    End Function
#Region "header"
    Private Function GetCellHeaderTableDesc() As TableCell
        Dim tMainCell As TableCell
        Dim tTable As Table
        Dim tRow As TableRow
        Dim tCell As TableCell

        tMainCell = New TableCell
        tMainCell.Width = Unit.Percentage(100)
        tMainCell.Height = intHeaderHeight
        tTable = New Table
        tTable.BorderStyle = BorderStyle.None
        tTable.CellPadding = 0
        tTable.CellSpacing = 0
        tTable.Height = Unit.Percentage(100)
        tTable.Width = Unit.Percentage(100)

        tRow = New TableRow
        tCell = GetCellHeader("Specifications", 5)
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
        tMainCell.Height = intHeaderHeight
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

        tCell = GetCellHeader("Weeks", 1)
        tCell.Width = Unit.Percentage(33)
        tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Value $", 1)
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


    Private Function GetCellHeaderTableRequirement() As TableCell
        Dim tMainCell As TableCell
        Dim tTable As Table
        Dim tRow As TableRow
        Dim tCell As TableCell

        tMainCell = New TableCell
        tMainCell.Width = Unit.Percentage(100)
        tMainCell.Height = intHeaderHeight
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

        tCell = GetCellHeader("Weeks", 1)
        tCell.Width = Unit.Percentage(33)
        tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Value $", 1)
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

    Private Function GetCellHeaderTableUsage() As TableCell
        Dim tMainCell As TableCell
        Dim tTable As Table
        Dim tRow As TableRow
        Dim tCell As TableCell
        Dim intMeaningfulWeeks As Integer = 0
        Dim ta As New dsCustomerTableAdapters.CustomerTableAdapter

        For Each dr As dsCustomer.CustomerRow In ta.GetByCustomerID(CurrentUser.CustomerID)
            If dr.IsMeaningfulUsageHistoryNull = False Then
                intMeaningfulWeeks = dr.MeaningfulUsageHistory
            End If
        Next

        tMainCell = New TableCell



        tMainCell.Width = Unit.Percentage(100)
        tMainCell.Height = intHeaderHeight

        tTable = New Table

        tTable.ID = "tblUsage"

        tTable.BorderStyle = BorderStyle.None
        tTable.CellPadding = 0
        tTable.CellSpacing = 0
        tTable.Height = Unit.Percentage(100)
        tTable.Width = Unit.Percentage(100)

        tRow = New TableRow
        tCell = GetCellHeader("Usage (Prev. " & intMeaningfulWeeks & ") Wks", 5)
        tCell.Width = Unit.Percentage(100)
        tCell.ID = "tCellHeader1"
        tRow.Cells.Add(tCell)
        tTable.Rows.Add(tRow)

        tRow = New TableRow
        tCell = GetCellHeader("Active Date", 1)
        tCell.Width = Unit.Percentage(20)
        tCell.ID = "tCellHeade2"
        tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Inactive Date", 1)
        tCell.Width = Unit.Percentage(20)
        tCell.ID = "tCellHeade2"
        tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Qty", 1)
        tCell.Width = Unit.Percentage(20)
        tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Requests", 1)
        tCell.Width = Unit.Percentage(20)
        tRow.Cells.Add(tCell)

        tCell = GetCellHeader("Per Week", 1)
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
#End Region


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

        'tRow = New TableRow
        'tRow.Cells.Add(GetCellEntry(sRefDesc, 5))
        'tTable.Rows.Add(tRow)

        tRow = New TableRow

        tCell = GetCellEntry(sAttr1, 1)
        tCell.Width = Unit.Percentage(20)
        tCell.Height = Unit.Percentage(100)
        tCell.Height = intHeaderHeight
        tRow.Cells.Add(tCell)

        tCell = GetCellEntry(sAttr2, 1)
        tCell.Width = Unit.Percentage(20)
        tCell.Height = Unit.Percentage(100)
        tRow.Cells.Add(tCell)

        tCell = GetCellEntry(sAttr3, 1)
        tCell.Width = Unit.Percentage(20)
        tCell.Height = Unit.Percentage(100)
        tRow.Cells.Add(tCell)

        tCell = GetCellEntry(sAttr4, 1)
        tCell.Width = Unit.Percentage(20)
        tCell.Height = Unit.Percentage(100)
        tRow.Cells.Add(tCell)

        tCell = GetCellEntry(sAttr5, 1)
        tCell.Width = Unit.Percentage(20)
        tCell.Height = Unit.Percentage(100)
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


    Private Function GetCellTableUsage(ByVal sAttr1 As String, ByVal sAttr2 As String, ByVal sAttr3 As String, ByVal sAttr4 As String, ByVal sAttr5 As String) As TableCell
        Dim tMainCell As TableCell
        Dim tTable As Table
        Dim tRow As TableRow
        Dim tCell As TableCell

        tMainCell = New TableCell
        tMainCell.Width = Unit.Percentage(100)
        tTable = New Table

        tTable.ID = "tblUsage"


        tTable.BorderStyle = BorderStyle.None
        tTable.CellPadding = 0
        tTable.CellSpacing = 0
        tTable.Height = intHeaderHeight
        tTable.Width = Unit.Percentage(100)



        tRow = New TableRow

        tCell = GetCellEntry(sAttr1, 1)
        tCell.Width = Unit.Percentage(20)
        tCell.Height = intHeaderHeight
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


        'If CInt(sQty) >= 0 Then
        tCell = GetCellEntry(sQty, 1, , True)
        tCell.Width = Unit.Percentage(33)
        tCell.Height = intHeaderHeight
        tRow.Cells.Add(tCell)

        tCell = GetCellEntry(sDays, 1)
        tCell.Width = Unit.Percentage(33)
        tRow.Cells.Add(tCell)

        tCell = GetCellEntry(sAmt, 1, , True)
        tCell.Width = Unit.Percentage(34)
        tRow.Cells.Add(tCell)
        'Else

        '    tCell = GetCellEntry("N/A", 1, , True)
        '    tCell.Width = Unit.Percentage(33)
        '    tCell.Height = intHeaderHeight
        '    tRow.Cells.Add(tCell)

        '    tCell = GetCellEntry("N/A", 1, , True)
        '    tCell.Width = Unit.Percentage(33)
        '    tRow.Cells.Add(tCell)

        '    tCell = GetCellEntry("N/A", 1, , True)
        '    tCell.Width = Unit.Percentage(34)
        '    tRow.Cells.Add(tCell)
        'End If

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


    'Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '  'Response.Redirect("./AdmRspP.aspx" & _
    '  '                  "?DY=" & txtDays.Text & _
    '  '                  "&RN=" & txtRound.Text & _
    '  '                  "&IT=" & txtItmType.Text & _
    '  '                  "&PN=" & txtPartNo.Text & _
    '  '                  "&S1=" & cboSort1.SelectedItem.Text & _
    '  '                  "&S2=" & cboSort2.SelectedItem.Text & _
    '  '                  "&S3=" & cboSort3.SelectedItem.Text _
    '  '                  , False)
    '  Response.Redirect("./AdmRspP.aspx" & _
    '                    "?DY=" & txtDays.Text & _
    '                    "&RN=" & txtRound.Text & _
    '                    "&IT=" & CurrentItemType() & _
    '                    "&ITI=" & CurrentItemTypeID() & _
    '                    "&PN=" & txtPartNo.Text & _
    '                    "&XZQ=" & IIf(chkExZeroQty.Checked = True, "T", "F") & _
    '                    "&S1=" & cboSort1.SelectedItem.Text & _
    '                    "&S2=" & cboSort2.SelectedItem.Text & _
    '                    "&S3=" & cboSort3.SelectedItem.Text _
    '                    , False)
    'End Sub
    Private Overloads Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdPrint.Click
        'Response.Redirect("./AdmRspP.aspx" & _
        '                  "?DY=" & txtDays.Text & _
        '                  "&RN=" & txtRound.Text & _
        '                  "&IT=" & CurrentItemType() & _
        '                  "&ITI=" & CurrentItemTypeID() & _
        '                  "&PN=" & txtPartNo.Text & _
        '                  "&XZQ=" & IIf(chkExZeroQty.Checked = True, "T", "F") & _
        '                  "&S1=" & cboSort1.SelectedItem.Text & _
        '                  "&S2=" & cboSort2.SelectedItem.Text & _
        '                  "&S3=" & cboSort3.SelectedItem.Text _
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

        cmdPrint.Visible = False
        cmdBack.Visible = True

        'LoadBaseData()
        LoadData()
    End Sub
    Private Sub cmdBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBack.Click

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

        cmdBack.Visible = False
        cmdPrint.Visible = True

        'LoadBaseData()
        LoadData()
    End Sub
    'Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
    '  LoadData()
    'End Sub
    Private Overloads Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRefresh.Click
        LoadData()
    End Sub

    'Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '  'False here may help with lost session vars - states to not end request early
    '  Response.Redirect(MyBase.CurrentUser.PreviousPage, False)
    'End Sub

 
End Class
