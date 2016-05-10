''''''''''''''''''''
'mw - 03-30-2008
'mw - 07-27-2007
'mw - 06-27-2007
''''''''''''''''''''


Imports System.Text

Partial Class OrdSReq
  Inherits paraPageBase

#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

  End Sub
  Protected WithEvents txtSubmit As System.Web.UI.WebControls.TextBox
  Protected WithEvents tblSelect As System.Web.UI.WebControls.Table
  Protected WithEvents cmdNext As System.Web.UI.WebControls.Button
  Protected WithEvents frmOrdSea As System.Web.UI.HtmlControls.HtmlForm
  Protected WithEvents Label1 As System.Web.UI.WebControls.Label
  Protected WithEvents chkReq As System.Web.UI.WebControls.CheckBox
  Protected WithEvents chkShp As System.Web.UI.WebControls.CheckBox
  Protected WithEvents chkCst As System.Web.UI.WebControls.CheckBox

  'NOTE: The following placeholder declaration is required by the Web Form Designer.
  'Do not delete or move it.
  Private designerPlaceholderDeclaration As System.Object

  Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
    'CODEGEN: This method call is required by the Web Form Designer
    'Do not modify it using the code editor.
    InitializeComponent()

    MyBase.Page_Init(sender, e)
    txtLName.Text = Request.QueryString("N")
    LoadData()
  End Sub

#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        PageTitle = "Requestor Search"
        MyBase.PageMessage = "Click on Select to select an order for reference data."

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        ElseIf (MyBase.CurrentUser.CanLookupOrder = paraUser.LookupOrderType.None) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri
        End If
    End Sub


  Private Sub LoadData()
    LoadGrd()
  End Sub

  Private Function LoadGrd() As Boolean
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim dr As OleDb.OleDbDataReader
        Dim bSuccess As Boolean = False
        Dim sSQL As String

        Dim dsLocal As New dsLookupTableAdapters.LocalTableBillToAdapter
        Dim dsLocalTable As New dsLookup.LocalDataTable
        Dim dsOverall As New dsLookupTableAdapters.OverallTableBillToAdapter
        Dim dsOverallTable As New dsLookup.OverallDataTable

        Select Case MyBase.CurrentUser.CanLookupOrder
            Case paraUser.LookupOrderType.Overall
                Dim ds As New dsLookup.OverallDataTable

                'dsOverallTable = dsOverall.GetData(MyBase.CurrentUser.CustomerID, txtLName.Text & "%")
                'dsOverallTable = dsOverall.GetDataExact(MyBase.CurrentUser.CustomerID, txtLName.Text.Trim)
                dsOverallTable = dsOverall.GetDataExact(MyBase.CurrentUser.CustomerID, txtLName.Text & "%")

                Dim i As Integer = 0
                'For Each dr1 As dsLookup.OverallRow In dsOverallTable.Rows
                '    If i = 0 Or i + 1 = dsOverallTable.Rows.Count Then
                '        'Dim dr2 As dsLookup.OverallRow
                '        'dr2. = dr1
                '        'ds.Rows.Add(dr2)
                '    Else
                '        dr1.Delete()
                '    End If
                '    i = i + 1
                'Next

                grd.DataSource = dsOverallTable
                grd.DataBind()
                Return True
            Case paraUser.LookupOrderType.Local
                'dsLocalTable = dsLocal.GetData(MyBase.CurrentUser.AccessCodeID, txtLName.Text & "%")
                'dsLocalTable = dsLocal.GetDataExact(MyBase.CurrentUser.AccessCodeID, txtLName.Text.Trim)
                dsLocalTable = dsLocal.GetDataExact(MyBase.CurrentUser.AccessCodeID, txtLName.Text & "%")
                'Dim i As Integer = 0
                'For Each dr1 As dsLookup.LocalRow In dsLocalTable.Rows
                '    If i = 0 Or i + 1 = dsLocalTable.Rows.Count Then
                '        'Dim dr2 As dsLookup.OverallRow
                '        'dr2. = dr1
                '        'ds.Rows.Add(dr2)
                '    Else
                '        dr1.Delete()
                '    End If
                '    i = i + 1
                'Next

                grd.DataSource = dsLocalTable
                grd.DataBind()
                Return True
            Case Else
                Return False
        End Select
  End Function

  Private Function BuildSQL() As String
        Dim bShowActiveOnly As Boolean : bShowActiveOnly = True
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty
        Dim sGroup As String = String.Empty
        Dim sFilter As String = String.Empty
        Dim sName As String = txtLName.Text.ToString.Trim

        If (MyBase.CurrentUser.CanLookupOrder = True) Then
            sSelect = "SELECT " & _
                      "MAX([Order].ID) AS ID " & _
                      "FROM [Order]"
            sWhere = " ([Order].AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ")"
            If (sName.Length > 0) Then
                sWhere = sWhere & " AND ([Order].Requestor_LastName LIKE " & MyBase.MyData.SQLString(sName & "%") & ")"
            End If
            If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere
            sGroup = "GROUP BY " & _
                     "[Order].Requestor_LastName, [Order].Requestor_FirstName "
            sFilter = sSelect + sWhere + sGroup

            sSelect = "SELECT " & _
                      "[Order].ID, " & _
                      "CDATE(FORMAT([Order].RequestDate,'mm-dd-yyyy')) AS RequestDate, " & _
                      "[Order].Requestor_FirstName + ' ' + [Order].Requestor_LastName AS Requestor, " & _
                      "[Order].Requestor_Email AS ReqEmail " & _
                      "FROM [Order]"
            sWhere = " [Order].ID IN (" & sFilter & ")"
            If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere
            sOrder = sOrder & " ORDER BY [Order].Requestor_LastName, [Order].Requestor_FirstName, CDATE(FORMAT([Order].RequestDate,'mm-dd-yyyy')) DESC"
        End If

        sSQL = sSelect + sWhere + sOrder

        Return sSQL
  End Function

  Sub grd_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grd.ItemDataBound
    If (e.Item.ItemType = ListItemType.Item Or _
        e.Item.ItemType = ListItemType.AlternatingItem Or _
        e.Item.ItemType = ListItemType.SelectedItem Or _
        e.Item.ItemType = ListItemType.EditItem) Then
      Dim sID As String = e.Item.Cells(0).Text.ToString()
      Dim sLinks As String = String.Empty

      sLinks = "./OrdPLd.aspx?ED=" & sID & _
               "&LR=1&LS=0"
      sLinks = "<a class='grid' href='" & sLinks & "'>Select</a>"
      e.Item.Cells(1).Text = sLinks

      e.Item.Cells(2).Text = Format(Convert.ToDateTime(e.Item.Cells(2).Text.ToString.Trim), "MM-dd-yyyy")
    End If
  End Sub

  'Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  '  Dim sDestin As String = paraPageBase.PAG_OrdRequest.ToString()
  '  MyBase.PageDirect(sDestin, 0, 0)
  'End Sub
  Private Overloads Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click
    Dim sDestin As String = paraPageBase.PAG_OrdRequest.ToString()
    MyBase.PageDirect(sDestin, 0, 0)
  End Sub
End Class
