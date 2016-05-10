''''''''''''''''''''
'mw - 06-17-2007
'mw - 03-17-2007
''''''''''''''''''''


Partial Class AdmInvP
  Inherits paraPageBase

#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

  End Sub
  Protected WithEvents tblLoad As System.Web.UI.WebControls.Table
  Protected WithEvents lblDates As System.Web.UI.WebControls.Label

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

  Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    PageTitle = "Inventory Preview"
    'MyBase.Page_Load(sender, e)

    If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
      MyBase.PageDirect()
    End If

    If (Page.IsPostBack = False) Then
      MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri

      LoadData()
    End If 'End PostBack
  End Sub

  Private Function LoadData()
    lblHead.Text = "Inventory as of " & Format(Now, "MM/dd/yyyy")
    LoadGrd()
  End Function

  Private Function LoadGrd() As Boolean
    Dim cnn As OleDb.OleDbConnection
    Dim cmd As OleDb.OleDbCommand
    Dim dr As OleDb.OleDbDataReader
    Dim bSuccess As Boolean = False
    Dim sSQL As String

    sSQL = BuildSQL()
    If (sSQL.Length > 0) Then
      MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
      grd.DataSource = dr
      grd.DataBind()
      MyBase.MyData.ReleaseReader2(cnn, cmd, dr)
      bSuccess = True
    End If

    Return (bSuccess)
  End Function

  Private Function BuildSQL() As String
    Dim bShowActiveOnly As Boolean : bShowActiveOnly = True
    Dim sSQL As String = String.Empty
    Dim sSelect As String = String.Empty
    Dim sWhere As String = String.Empty
    Dim sOrder As String = String.Empty

    If (MyBase.CurrentUser.CanViewInventory = True) Then
      sSelect = "SELECT Customer_Document.ID, Customer_Document.Status, Customer_Document.PrintMethod, Customer_Document.ReferenceNo AS RefNo, Customer_Document.Description AS RefName, QtyOH, QtyPerCtn, CtnPerSkid, " & _
                "IIf([Customer_Document].[Status]=2,'Backorder',IIf([Customer_Document].[Status]=0,'Inactive','Active')) AS DocStatus, " & _
                "IIf([Customer_Document].PrintMethod=1,10,PrintMethod) AS InvSort " & _
                "FROM Customer_Document INNER JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID"

      sWhere = " (Customer_Document.CustomerID = " & MyBase.CurrentUser.CustomerID & ") AND (Customer_Document.PrintMethod IN (" & MyData.DT_FILLONLY & ", " & MyData.DT_FILLPRINT & ")) "

      If (bShowActiveOnly) Then
        sWhere = sWhere & " AND (Status >" & MyData.STATUS_INAC & ") "
      End If
      If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

      'Display Order >> FulFillOnly, FulFill/POD
      sOrder = sOrder & " ORDER BY IIf([Customer_Document].PrintMethod=1,10,PrintMethod), Customer_Document.ReferenceNo "
    End If

    sSQL = sSelect + sWhere + sOrder

    Return sSQL
  End Function

  Sub grd_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grd.ItemDataBound
    If (e.Item.ItemType = ListItemType.Item Or _
        e.Item.ItemType = ListItemType.AlternatingItem Or _
        e.Item.ItemType = ListItemType.SelectedItem Or _
        e.Item.ItemType = ListItemType.EditItem) Then
      Dim sDestin As String = paraPageBase.PAG_AdmKitEdit.ToString()
      Dim sID As String = e.Item.Cells(0).Text.ToString()
      Dim sLinks As String = String.Empty

      Dim iStatus As Int16 = Convert.ToInt16(e.Item.Cells(2).Text.ToString)
      Dim sDesc As String = e.Item.Cells(6).Text.ToString & " - " & e.Item.Cells(7).Text.ToString
      'Weeks
      Dim iType As Int16 = Convert.ToInt16(e.Item.Cells(4).Text.ToString)
      Dim sWeeks As String = String.Empty
      Dim bWarn As Boolean = False

      If (iType = MyData.DT_PRINTONLY) Then
        sWeeks = "POD"
      Else
        sWeeks = GetUsageNote(Convert.ToInt32(sID), iType, bWarn)
      End If
      e.Item.Cells(5).Text = sWeeks

      If (bWarn = True) Then
        'Inventory Warning
        'e.Item.ForeColor = Color.Red
        e.Item.ForeColor = Color.Gray
      End If

      sLinks = "./AdmInvE.aspx?ED=" + sID
      sLinks = "<a class='grid' href='" & sLinks & "'>Detail</a>"
      e.Item.Cells(1).Text = sLinks
    End If
  End Sub

  'Inventory Warning Section - Start
  'Private Function GetUsageNote(ByVal lDocID As Int32, ByRef sWarn As String)
  Private Function GetUsageNote(ByVal lDocID As Long, ByVal iDocType As Integer, ByRef bWarn As Boolean)
    Dim lQtyRemain As Long = 0
    Dim iStockWarn As Integer = 0
    Dim iMonthCnt As Integer = 0
    Dim iAvgQtyPerMonth As Integer = 0
    Dim iAvgQtyPerWeek As Integer = 0
    'Dim iAvgQtyPerDay As Integer = 0
    Dim gAvgQtyPerDay As Single = 0
    Dim gWeekRemain As Single = 0
    Dim sMsg As String = String.Empty
    Dim sNote As String = String.Empty

    sNote = ""
    bWarn = False

    MyBase.MyData.GetQtyRemainWarn(lDocID, lQtyRemain, iStockWarn)
    'mw - 10-30-2007
    'MyBase.MyData.GetAvgUsage(lDocID, iMonthCnt, iAvgQtyPerMonth, iAvgQtyPerWeek, iAvgQtyPerDay)
    MyBase.MyData.GetAvgUsage(lDocID, iMonthCnt, iAvgQtyPerMonth, iAvgQtyPerWeek, gAvgQtyPerDay)
    '
    gWeekRemain = MyBase.MyData.GetWeekRemain(iAvgQtyPerWeek, lQtyRemain, sMsg)

    If (iAvgQtyPerWeek = 0) Then
      sNote = "No History"
    Else
      sNote = gWeekRemain & " weeks remain"
      If (gWeekRemain <= iStockWarn) And (iAvgQtyPerWeek > 0) Then
        bWarn = True
      End If
    End If

    If (lQtyRemain = 0) And (iDocType = MyData.DT_FILLONLY) Then
      bWarn = True : sNote = "Back Ordered"
    ElseIf (lQtyRemain = 0) And (iDocType = MyData.DT_FILLPRINT) Then
      bWarn = True : sNote = "POD/Back Ordered"
    End If

    Return sNote
  End Function

  Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
    Dim sDestin As String = paraPageBase.PAG_AdmInvnList.ToString()

    MyBase.PageDirect(sDestin, 0, 0)
  End Sub

End Class
