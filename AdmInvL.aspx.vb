''''''''''''''''''''
'mw - 04-01-2009
'mw - 08-19-2008
'mw - 06-07-2008
'mw - 08-18-2007
'mw - 04-28-2007
'mw - 01-20-2007
''''''''''''''''''''


Partial Class AdmInvL
    Inherits paraPageBase

    Private Enum StockWarningType
        Weeks
        Pieces
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
        PageTitle = "Inventory"
        'MyBase.Page_Load(sender, e)
        MyBase.PageMessage = "Click on Detail for more information on individual items."

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        ElseIf Not (MyBase.CurrentUser.CanViewInventory = True) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri
        End If 'End PostBack
    End Sub

    Private Sub LoadData()
        LoadGrd()
        'LoadValue()
    End Sub

    Private Function LoadGrd(Optional ByVal sSortField As String = "") As Boolean
        Dim ds As New dsInventoryTableAdapters.InventoryTableAdapter
        Dim dt As New dsInventory.InventoryDataTable
        Dim decTotalValue As Decimal = 0

        ds.Fill(dt, MyBase.CurrentUser.CustomerID, MyData.DT_FILLONLY, MyBase.CurrentUser.CustomerID, MyData.DT_FILLPRINT)

        RemoveExcludedItems(dt.DefaultView)

        Dim dc As New DataColumn
        dc.DataType = System.Type.GetType("System.String")
        dc.DefaultValue = "supply1"
        dc.ColumnName = "supply1"
        dt.Columns.Add(dc)


        Dim dcInt As New DataColumn
        dcInt.DataType = System.Type.GetType("System.Int32")
        dcInt.DefaultValue = "0"
        dcInt.ColumnName = "QTYBCK"
        dt.Columns.Add(dcInt)


        Dim dcBwarn As New DataColumn
        dcBwarn.DataType = System.Type.GetType("System.Boolean")
        dcBwarn.DefaultValue = False
        dcBwarn.ColumnName = "bwarn"
        dt.Columns.Add(dcBwarn)

        Dim dcTotalValue As New DataColumn
        dcTotalValue.DataType = System.Type.GetType("System.Int32")
        dcTotalValue.DefaultValue = 0
        dcTotalValue.ColumnName = "TotalValue"
        dt.Columns.Add(dcTotalValue)

        Dim dcStatus As New DataColumn
        dcStatus.DataType = System.Type.GetType("System.String")
        dcStatus.DefaultValue = ""
        dcStatus.ColumnName = "Status1"
        dt.Columns.Add(dcStatus)


        'NEW SUPPLY ITEMS:
        Dim intMeaningfulWeeks As Integer
        Dim intMeaningfulOrderQty As Integer
        Dim intStockWarningPieces As Integer = -1

        Dim ta As New dsCustomerTableAdapters.CustomerTableAdapter
        Dim decFmtPrice As Decimal
        Dim intStockWarningWeeks As Integer
        Dim intStockWarningType As Integer
        Dim intStockWarningWeeksCustomer As Integer = 0
        Dim intStockWarningPiecesCustomer As Integer = -1

        For Each dr As dsCustomer.CustomerRow In ta.GetByCustomerID(CurrentUser.CustomerID)
            If dr.IsMeaningfulOrderQtyNull = False Then
                intMeaningfulOrderQty = dr.MeaningfulOrderQty
            End If
            If dr.IsMeaningfulUsageHistoryNull = False Then
                intMeaningfulWeeks = dr.MeaningfulUsageHistory
            End If
            '0 - weeks 1 - pieces
            intStockWarningType = dr.StockWarningType

            Select Case dr.StockWarningType
                Case 0
                    If dr.IsStockWarningPiecesNull = False AndAlso IsNumeric(dr.StockWarningPieces) Then
                        intStockWarningPieces = dr.StockWarningPieces
                        intStockWarningPiecesCustomer = dr.StockWarningPieces
                    End If
                    intStockWarningWeeks = dr.StockWarn
                    intStockWarningWeeksCustomer = dr.StockWarn
                Case 1
                    intStockWarningPieces = dr.StockWarn
                    intStockWarningPiecesCustomer = dr.StockWarn
            End Select

        Next



        Dim dtQtyUsage As Date = Now.Date.AddDays(-intMeaningfulWeeks * 7)

        For Each dr As dsInventory.InventoryRow In dt.Rows

            If dr.RowState <> DataRowState.Deleted Then
                intStockWarningWeeks = intStockWarningWeeksCustomer
                intStockWarningPieces = intStockWarningPiecesCustomer


                Dim sID As String = dr.ID
                Dim sLinks As String = String.Empty
                Dim intWeeksLeft As Integer = 0
                Dim iStatus As Int16 = dr.Status
                Dim sDesc As String = dr.RefNo & " - " & dr.RefName

                'Weeks
                Dim iType As Integer = dr.PrintMethod
                Dim sWeeks As String = String.Empty
                Dim bWarn As Boolean = False
                Dim iQuantityOnHand As Integer
                Dim iStockWarningLevel As Integer

                Dim iMonthsLeft As Integer
                Dim iAverageQuantityPerMonth As Integer
                Dim iAverageQuantityPerWeek As Integer
                Dim iAverageQuantityPerDay As Integer
                Dim pd As New paraData
                Dim sStatus As String
                'stock warning type weeks/pieces
                Dim strStockWarningType As String = 0
                Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
                strStockWarningType = qa.GetStockWarningType(CurrentUser.CustomerID)

                MyBase.MyData.GetQtyRemainWarn(sID, iQuantityOnHand, iStockWarningLevel)
                If iStockWarningLevel <> 0 Then
                    intStockWarningWeeks = iStockWarningLevel
                    intStockWarningPieces = iStockWarningLevel
                End If


                MyBase.MyData.GetAvgUsage(sID, iMonthsLeft, iAverageQuantityPerMonth, iAverageQuantityPerWeek, iAverageQuantityPerDay)

                If (iType = MyData.DT_PRINTONLY) Then
                    sWeeks = "POD"
                Else

                    If dr.PrintMethod = 0 Then
                        sStatus = "FUL"
                    ElseIf dr.PrintMethod = 1 Then
                        sStatus = "POD"
                    Else
                        sStatus = "F/P"
                    End If


                    If dr.QtyOH = 0 And sStatus = "FUL" Then
                        sWeeks = "Back Ordered"
                    Else

                        '"iif(Customer_Document.PrintMethod=0,'FUL',iif(Customer_Document.PrintMethod=1,'POD','F/P')) AS Status, " & _

                        Dim intQtyUsage As Integer = qa.GetQtyUsage(dr.ID, dtQtyUsage) '* dr.QtyPerPull
                        Dim intRequests As Integer = qa.GetRequestCountMeaningful(dr.ID, dtQtyUsage)

                        sWeeks = pd.GetSupply(dr.QtyOH, intQtyUsage, intRequests, intMeaningfulWeeks, intMeaningfulOrderQty, sStatus, intStockWarningPieces, 365)
                    End If

                    'sWeeks = GetUsageNote(Convert.ToInt32(sID), iType, bWarn, intWeeksLeft, strStockWarningType)
                End If

                'iif(ISNULL(Customer_Format.Price),0,Customer_Format.Price) AS FmtPrice

                'first price then cost
                Dim decPrice As Decimal
                decPrice = qa.GetPriceFromDocumentFill(dr.ID)
                If decPrice <= 0 Then
                    decPrice = qa.GetCostFromDocumentFill(dr.ID)
                End If
                dr.Item("TotalValue") = decPrice * dr.QtyOH
                decTotalValue += dr.Item("TotalValue")
                If dr.QtyOH = 0 And sStatus = "F/P" Then
                    'GREEN:
                End If
                decFmtPrice = 0
                '0 - weeks 1 - pieces




                Select Case intStockWarningType
                    Case 1
                        If intStockWarningPieces > dr.QtyOH Then
                            bWarn = True
                        End If
                    Case 0
                        If IsNumeric(sWeeks) AndAlso sWeeks < intStockWarningWeeks Then
                            bWarn = True
                        End If


                        If (sStatus = "FUL" Or sStatus = "F/P") AndAlso sWeeks.Contains("Evaluation") Then
                            bWarn = True
                        End If

                End Select

                'dr.Item("RefName") = dr("")
                dr.Item("supply1") = sWeeks
                dr.Item("bwarn") = bWarn
                dr.Item("status1") = sStatus

                'If dr("RefNo") = "14950A" Then
                '    Response.Write(dr("supply1") & "2<BR>")
                'End If



                Dim intTmp As Object = qa.GetQtyBackOrder(dr.ID)

                If IsNothing(intTmp) = False AndAlso IsNumeric(intTmp) Then
                    dr.Item("QTYBCK") = CInt(intTmp)
                End If
            End If

        Next


        Dim dv As New DataView(dt)



        If (sSortField <> "") Then
            If (sSortField = Session("Sort")) Then
                dv.Sort = sSortField & " DESC"
            Else
                dv.Sort = sSortField
            End If
        End If

        grd.DataSource = dv


        grd.DataBind()


        lblValue.Text = "Current Value in Inventory:  " & Format(decTotalValue, "Currency")

        Return (True)
    End Function
    Private Sub RemoveExcludedItems(ByVal ds As DataView)
        Dim taExclusions As New dsExclusionsTableAdapters.ExclusionsByTypeTableAdapter
        Dim dtExclusions As New dsExclusions.ExclusionsByTypeDataTable
        Dim drExclusions() As dsExclusions.ExclusionsByTypeRow

        Dim taExclusionsKeyword As New dsExclusionsTableAdapters.GetExclusionsByKeywordTableAdapter
        Dim dtExclusionsKeyword As New dsExclusions.GetExclusionsByKeywordDataTable
        Dim drExclusionsKeyword() As dsExclusions.GetExclusionsByKeywordRow

        Dim dRow As DataRow
        Dim iCounter As Integer

        taExclusions.Fill(dtExclusions, 5, CurrentUser.AccessCodeID, 5, CurrentUser.CustomerID)
        taExclusionsKeyword.Fill(dtExclusionsKeyword, CurrentUser.CustomerID, CurrentUser.CustomerID, CurrentUser.AccessCodeID)

        For iCounter = 0 To ds.Table.Rows.Count - 1
            dRow = ds.Table.Rows(iCounter)
            Try
                drExclusions = dtExclusions.Select(String.Format("Description={0}", dRow("Id")))
                If (drExclusions.GetUpperBound(0) > -1) Then
                    dRow.Delete()
                Else
                    drExclusionsKeyword = dtExclusionsKeyword.Select(String.Format("Id={0}", dRow("Id")))
                    If (drExclusionsKeyword.GetUpperBound(0) > -1) Then
                        dRow.Delete()
                    End If
                End If
            Catch ex As Exception
            End Try
        Next
    End Sub

    Sub LoadValue2()
        lblValue.Text = "Current Value in Inventory:  " & GetInvnVal()
    End Sub

    Function GetInvnVal() As String
        Dim bShowActiveOnly As Boolean : bShowActiveOnly = True
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim dr As OleDb.OleDbDataReader
        Dim sSQL As String
        Dim sWhere As String
        Dim sVal As String : sVal = "$ 0.00"

        sSQL = "SELECT Format(Sum(Format(([QtyOHLocP]+IIf([Customer].[InventoryStatusWarehouse2]='0',[Customer_Document_Fill].[QtyOHLocS],0)+IIf([Customer].[InventoryStatusWarehouse3]='0',[Customer_Document_Fill].[QtyOHLocW],0))*[Customer_Document_Fill].[Cost],'$ 0.00')),'$ #,##0.00') AS TotVal " & _
                "FROM Customer INNER JOIN (Customer_Document INNER JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID) ON Customer.ID = Customer_Document.CustomerID "

        sWhere = " (Customer_Document.CustomerID = " & MyBase.CurrentUser.CustomerID & ") AND (Customer_Document.PrintMethod IN (" & MyData.DT_FILLONLY & ", " & MyData.DT_FILLPRINT & ")) "
        If (bShowActiveOnly) Then
            sWhere = sWhere & " AND (Status >" & MyData.STATUS_INAC & ") "
        End If
        If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

        sSQL = sSQL & sWhere
        MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
        If dr.Read() Then
            sVal = dr("TotVal").ToString()
        End If
        MyBase.MyData.ReleaseReader2(cnn, cmd, dr)

        Return sVal
    End Function

    Sub grd_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grd.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or _
            e.Item.ItemType = ListItemType.AlternatingItem Or _
            e.Item.ItemType = ListItemType.SelectedItem Or _
            e.Item.ItemType = ListItemType.EditItem) Then

            Dim sID As String = CType(e.Item.FindControl("lblId"), Label).Text
            Dim sLinks As String = String.Empty
            '' '' ''Weeks
            Dim iType As Integer = CType(e.Item.FindControl("lblPrintMethod"), Label).Text
            Dim bWarn As Boolean = CType(e.Item.FindControl("lblBwarn"), Label).Text
            Dim sStatus As String = CType(e.Item.FindControl("lblStatus1"), Label).Text
            Dim intQtyOH As Integer = CType(e.Item.FindControl("lblQuantity"), Label).Text
            Dim sWeeks As String = CType(e.Item.FindControl("lblSweeks"), Label).Text

            If (sStatus = "F/P") And intQtyOH = 0 Then
                e.Item.BackColor = Color.LightGreen
            ElseIf (sWeeks.ToUpper = "Back Ordered".ToUpper) Then
                e.Item.BackColor = Color.Orange
            ElseIf ((sStatus = "FUL") And (bWarn = True)) Or ((sStatus = "F/P") And (bWarn = True)) Then '
                e.Item.ForeColor = Color.White
                e.Item.BackColor = Color.DarkRed
            End If
            '

            sLinks = "./AdmInvE.aspx?ED=" + sID
            sLinks = "<a class='grid' href='" & sLinks & "'>Detail</a>"
            e.Item.Cells(1).Text = sLinks
        End If
    End Sub

    Private Function GetUsageNote(ByVal lDocID As Long, ByVal iDocType As Integer, ByRef bWarn As Boolean, ByRef gWeekRemain As Single, ByVal sStockWarningType As StockWarningType) As String
        Dim lQtyRemain As Long = 0
        Dim iStockWarn As Integer = 0
        Dim iMonthCnt As Integer = 0
        Dim iAvgQtyPerMonth As Integer = 0
        Dim iAvgQtyPerWeek As Integer = 0
        'Dim iAvgQtyPerDay As Integer = 0
        Dim gAvgQtyPerDay As Single = 0
        'Dim gWeekRemain As Single = 0
        Dim sMsg As String = String.Empty
        Dim sNote As String = String.Empty

        sNote = ""
        bWarn = False

        MyBase.MyData.GetQtyRemainWarn(lDocID, lQtyRemain, iStockWarn)
        MyBase.MyData.GetAvgUsage(lDocID, iMonthCnt, iAvgQtyPerMonth, iAvgQtyPerWeek, gAvgQtyPerDay)
        '
        gWeekRemain = MyBase.MyData.GetWeekRemain(iAvgQtyPerWeek, lQtyRemain, sMsg)


        If sStockWarningType = StockWarningType.Weeks Then
            If (iAvgQtyPerWeek = 0) Then
                sNote = "No History"
            Else
                sNote = gWeekRemain & " weeks remain"
                If (gWeekRemain <= iStockWarn) And (iAvgQtyPerWeek > 0) Then
                    bWarn = True
                End If
            End If
        Else
            Dim taDetail As New dsInventoryTableAdapters.InventoryDetailTableAdapter
            Dim dtDetail As New dsInventory.InventoryDetailDataTable

            If (iAvgQtyPerWeek = 0) Then
                sNote = "No History"
            Else
                sNote = gWeekRemain & " weeks remain"
            End If

            dtDetail = taDetail.GetData(lDocID)
            If (dtDetail.Rows.Count > 0) Then
                If (dtDetail(0).QtyOH <= dtDetail(0).StockWarn) Then
                    bWarn = True
                End If
            End If
        End If

        If (lQtyRemain = 0) And (iDocType = MyData.DT_FILLONLY) Then
            bWarn = True : sNote = "Back Ordered"
        ElseIf (lQtyRemain = 0) And (iDocType = MyData.DT_FILLPRINT) Then
            bWarn = True : sNote = "POD/Back Ordered"
        End If

        Return sNote
    End Function

    'Private Overloads Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdPrint.Click
    '    Dim sDestin As String = paraPageBase.PAG_AdmInvnPreview.ToString()
    '    MyBase.PageDirect(sDestin, 0, 0)
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
                If ViewState("print") Is DBNull.Value Or ViewState("print") Is Nothing Then
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

    Private Sub grd_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles grd.SortCommand
        LoadGrd(e.SortExpression)
    End Sub
End Class
