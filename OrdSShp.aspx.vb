''''''''''''''''''''
'mw - 04-03-2008
'mw - 07-27-2007
'mw - 06-28-2007
''''''''''''''''''''


Imports System.Text


Partial Class OrdSShp
  Inherits paraPageBase

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
    txtLName.Text = Request.QueryString("N")
    txtCompany.Text = Request.QueryString("C")
    LoadData()
  End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        PageTitle = "Ship To Search"
        MyBase.PageMessage = "Click on Select to select an order for reference data."

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        ElseIf (MyBase.CurrentUser.CanLookupOrder = paraUser.LookupOrderType.None) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri
        End If 'End PostBack
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

        Dim dsLocal As New dsLookupTableAdapters.LocalTableShipToTableAdapter
        Dim dsLocalTable As New dsLookup.LocalTableShipToDataTable
        Dim dsOverall As New dsLookupTableAdapters.OverallTableShipToTableAdapter
        Dim dsOverallTable As New dsLookup.OverallTableShipToDataTable

        Dim strShipTo As String = ""
        Dim strOldCompany As String = ""

        Select Case MyBase.CurrentUser.CanLookupOrder
            Case paraUser.LookupOrderType.Overall

                'dsOverallTable = dsOverall.GetDataExact(MyBase.CurrentUser.CustomerID, String.Format("{0}%", Me.txtLName.Text.Trim), String.Format("{0}%", Me.txtCompany.Text.Trim))
                'dsOverallTable = dsOverall.GetDataExact(MyBase.CurrentUser.CustomerID, Me.txtLName.Text.Trim, Me.txtCompany.Text.Trim)
                dsOverallTable = dsOverall.GetDataExact(MyBase.CurrentUser.CustomerID, String.Format("{0}%", Me.txtLName.Text.Trim), String.Format("{0}%", Me.txtCompany.Text.Trim))

                'Dim i As Integer = 0

                dsOverallTable.DefaultView.Sort = "  shipto, RequestDate desc"


                For Each dr1 As dsLookup.OverallTableShipToRow In dsOverallTable.DefaultView.Table.Rows
                    If dr1.ShipTo = strShipTo And dr1.Company = strOldCompany Then
                        
                        dr1.Delete()
                    Else

                        strShipTo = dr1.ShipTo
                        strOldCompany = dr1.Company
                        'dr1.Delete()
                    End If
                Next

                dsOverallTable.DefaultView.Sort = "  RequestDate desc"


                grd.DataSource = dsOverallTable.DefaultView
                grd.DataBind()

                'Response.Write("global")
                Return True
            Case paraUser.LookupOrderType.Local
                'dsLocalTable = dsLocal.GetData(MyBase.CurrentUser.AccessCodeID, String.Format("{0}%", Me.txtLName.Text.Trim), String.Format("{0}%", Me.txtCompany.Text.Trim))
                'dsLocalTable = dsLocal.GetDataExact(MyBase.CurrentUser.AccessCodeID, Me.txtLName.Text.Trim, Me.txtCompany.Text.Trim)
                dsLocalTable = dsLocal.GetDataExact(MyBase.CurrentUser.AccessCodeID, String.Format("{0}%", Me.txtLName.Text.Trim), String.Format("{0}%", Me.txtCompany.Text.Trim))


                dsLocalTable.DefaultView.Sort = "  shipto, RequestDate desc"
                For Each dr1 As dsLookup.LocalTableShipToRow In dsLocalTable.DefaultView.Table.Rows
                    If dr1.ShipTo = strShipTo And dr1.Company = strOldCompany Then
                        dr1.Delete()
                    Else
                        strShipTo = dr1.ShipTo
                        strOldCompany = dr1.Company
                    End If
                Next

                dsLocalTable.DefaultView.Sort = "  RequestDate desc"
                grd.DataSource = dsLocalTable.DefaultView.Table
                grd.DataBind()

                'Response.Write("local")
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
    Dim sCompany As String = txtCompany.Text.ToString.Trim

        If (True) Then
            sSelect = "SELECT " & _
                      "MAX([Order].ID) AS ID " & _
                      "FROM [Order]"
            sWhere = " ([Order].AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ")"
            If (sName.Length > 0) Then
                sWhere = sWhere & " AND ([Order].ShipTo_ContactLastName LIKE " & MyBase.MyData.SQLString(sName & "%") & ")"
            End If
            If (sCompany.Length > 0) Then
                sWhere = sWhere & " AND ([Order].ShipTo_Name LIKE " & MyBase.MyData.SQLString(sCompany & "%") & ")"
            End If
            If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere
            sGroup = "GROUP BY " & _
                     "[Order].ShipTo_Name, " & _
                     "[Order].ShipTo_ContactLastName, [Order].ShipTo_ContactFirstName "
            sFilter = sSelect + sWhere + sGroup

            sSelect = "SELECT " & _
                      "[Order].ID, " & _
                      "CDATE(FORMAT([Order].RequestDate,'mm-dd-yyyy')) AS RequestDate, " & _
                      "[Order].ShipTo_Name AS Company, " & _
                      "[Order].ShipTo_ContactFirstName + ' ' + [Order].ShipTo_ContactLastName AS ShipTo, " & _
                      "[Order].ShipTo_Address1 + IIF(LEN([Order].ShipTo_Address2)>0,' ' + [Order].ShipTo_Address2,'')  AS BAddress, " & _
                      "[Order].ShipTo_City AS BCity, [Order].ShipTo_State AS BState, [Order].ShipTo_ZipCode AS BZip " & _
                      "FROM [Order]"
            sWhere = " [Order].ID IN (" & sFilter & ")"
            If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere
            sOrder = sOrder & " ORDER BY ShipTo_Name, [Order].ShipTo_ContactLastName, [Order].ShipTo_ContactFirstName, CDATE(FORMAT([Order].RequestDate,'mm-dd-yyyy')) DESC"
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
               "&LR=0&LS=1"
      sLinks = "<a class='grid' href='" & sLinks & "'>Select</a>"
      e.Item.Cells(1).Text = sLinks

      e.Item.Cells(2).Text = Format(Convert.ToDateTime(e.Item.Cells(2).Text.ToString.Trim), "MM-dd-yyyy")
    End If
  End Sub

  'Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  '  Dim sDestin As String = paraPageBase.PAG_OrdShip.ToString()
  '  MyBase.PageDirect(sDestin, 0, 0)
  'End Sub
  Private Overloads Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click
    Dim sDestin As String = paraPageBase.PAG_OrdShip.ToString()
    MyBase.PageDirect(sDestin, 0, 0)
  End Sub
End Class
