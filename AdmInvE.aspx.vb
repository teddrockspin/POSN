''''''''''''''''''''
'mw - 07-20-2009
'mw - 04-01-2009
'mw - 04-26-2008
'mw - 08-18-2007
'mw - 03-17-2007
'mw - 01-20-2007
''''''''''''''''''''


Partial Class AdmInvE
    Inherits paraPageBase


    Private mlID As Long = 0


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblTotVal As System.Web.UI.WebControls.Label
    Protected WithEvents txtTotVal As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()

        MyBase.Page_Init(sender, e)
        'LoadData()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        PageTitle = "Inventory Review"
        'MyBase.Page_Load(sender, e)
        MyBase.PageMessage = ""

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        ElseIf Not (MyBase.CurrentUser.CanViewInventory = True) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri
            LoadData()
            cmdBack.OnClientClick = "javascript:window.history.go(-1);return false;"
        End If 'End PostBack
    End Sub

    Private Sub LoadData()
        Dim iWeekRemain As Integer = 0
        Dim iStockWarn As Integer = 0
        Dim lQtyRemain As Long = 0
        Dim iMonthCnt As Integer = 0
        Dim iAvgQtyPerMonth As Integer = 0
        Dim iAvgQtyPerWeek As Integer = 0
        'Dim iAvgQtyPerDay As Integer = 0
        Dim gAvgQtyPerDay As Single = 0
        Dim sMsg As String = String.Empty
        Dim iStat As Integer = 0
        Dim sDesc As String = String.Empty
        Dim sActDate As String = String.Empty
        Dim sInActDate As String = String.Empty
        Dim gCost As Single = 0.0
        Dim sDate As String = String.Empty
        Dim sQty As String = String.Empty
        Dim strStockWarningType As String = 0
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter



        '--
        Dim intMeaningfulWeeks As Integer
        Dim intMeaningfulOrderQty As Integer
        Dim intStockWarningPieces As Integer = -1
        Dim ta As New dsCustomerTableAdapters.CustomerTableAdapter
        Dim decFmtPrice As Decimal
        Dim intStockWarningWeeks As Integer
        Dim intStockWarningType As Integer

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
                    End If
                    intStockWarningWeeks = dr.StockWarn
                Case 1
                    intStockWarningPieces = dr.StockWarn
            End Select

        Next

        txtMonthsRvw.Text = intMeaningfulWeeks
        '--

        strStockWarningType = qa.GetStockWarningType(CurrentUser.CustomerID)


        mlID = Val(Request.QueryString("ED").ToString())

        'MyBase.MyData.GetItemDetails(mlID, iStat, sDesc)
        MyBase.MyData.GetItemDetails(mlID, iStat, sDesc, sActDate, sInActDate, gCost)

        lblItm.Text = sDesc

        txtActDate.Text = sActDate
        txtInActDate.Text = sInActDate

        txtUVal.Text = Format(gCost, "$ #,###,###,##0.00")

        txtFirstUse.Text = MyBase.MyData.GetFirstUsage(mlID)
        'mw - 07-20-2009
        'txtLastUse.Text = MyBase.MyData.GetLastUsage(mlID)
        MyBase.MyData.GetLastUsage(mlID, sDate, sQty)
        txtLastUse.Text = sDate
        txtLastQty.Text = sQty
        '
        MyBase.MyData.GetQtyRemainWarn(mlID, lQtyRemain, iStockWarn)
        MyBase.MyData.GetAvgUsage(mlID, iMonthCnt, iAvgQtyPerMonth, iAvgQtyPerWeek, gAvgQtyPerDay)
        '
        iWeekRemain = MyData.GetWeekRemain(iAvgQtyPerWeek, lQtyRemain, sMsg)

        'txtMonthsRvw.Text = iMonthCnt
        txtQty.Text = lQtyRemain
        txtTVal.Text = Format(lQtyRemain * gCost, "$ #,###,###,##0.00")
        txtAvgWk.Text = iAvgQtyPerWeek
        txtWeeksRm.Text = iWeekRemain

        'mw - 09-13-2007 - ab - use from Item
        'iStockWarn = ConfigurationSettings.AppSettings("SafetyInventoryLevel")
        lblWarn.Visible = False
        Select Case strStockWarningType
            Case "0"
                If (iAvgQtyPerWeek > 0) And (iWeekRemain <= iStockWarn) Then
                    lblWarn.Text = "The Inventory Level for this Item is Below the ReOrder Point of " & iStockWarn & " Weeks Supply."
                    lblWarn.Visible = True
                    divWarn.Visible = True
                End If
            Case "1"
                If lQtyRemain <= iStockWarn Then
                    lblWarn.Text = "The Inventory Level for this Item is Below the ReOrder Point of " & iStockWarn & " Pieces."
                    lblWarn.Visible = True
                    divWarn.Visible = True
                End If
        End Select

        Dim taItemType As New dsInventoryTableAdapters.InventoryDetailTableAdapter
        Dim dtItemType As New dsInventory.InventoryDetailDataTable
        Dim sItemType As String

        dtItemType = taItemType.GetData(mlID)
        If (dtItemType.Rows.Count > 0) Then
            sItemType = dtItemType(0).PrintMethod
        Else
            sItemType = ""
        End If

        If (iStat = MyData.STATUS_INAC) Then
            If lblWarn.Text = "" Then
                lblWarn.Text = "Item Status is Inactive"
            Else
                lblWarn.Text = lblWarn.Text & "<br>" & "Item Status is Inactive"
            End If

            lblWarn.Visible = True
            divWarn.Visible = True
        ElseIf (lQtyRemain = 0) And (sItemType = MyData.DT_FILLONLY) Then
            If (lblWarn.Text = "") Then
                lblWarn.Text = "This Item is on Backorder and Not Available at this Time."
            Else
                lblWarn.Text = lblWarn.Text & "<br>" & "This Item is on Backorder and Not Available at this Time."
            End If

            lblWarn.Visible = True
            divWarn.Visible = True
        ElseIf (lQtyRemain = 0) And (sItemType = MyData.DT_FILLPRINT) Then
            If (lblWarn.Text = "") Then
                lblWarn.Text = "This Item has Depleted Inventory and has been Moved to POD Status."
            Else
                lblWarn.Text = lblWarn.Text & "<br>" & "This Item has Depleted Inventory and has been Moved to POD Status."
            End If

            lblWarn.Visible = True
            divWarn.Visible = True
        End If


    End Sub

    'Private Overloads Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click
    '    'Response.Redirect("AdmInvL.aspx")
    'End Sub


End Class
