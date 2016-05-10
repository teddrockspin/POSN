Imports System.Data.SqlClient

''''''''''''''''''''
'mw - 05-17-2008
'mw - 04-26-2008
'mw - 08-24-2007
'mw - 04-25-2007
'mw - 03-17-2007
'mw - 01-20-2007
''''''''''''''''''''


Partial Class AdmUse
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
        '    LoadData()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        PageTitle = "Usage"
        'MyBase.Page_Load(sender, e)
        MyBase.PageMessage = "Use the date range to filter usage information."

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        ElseIf Not (MyBase.CurrentUser.ViewUsage = True) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri
            LoadData()
        End If 'End PostBack
    End Sub

    Private Function LoadData(Optional ByVal sSortField As String = "")
        If (txtFrom.Text.Length = 0) Or (txtTo.Text.Length = 0) Then
            txtFrom.Text = Format(Now.AddMonths(-1), "MM/dd/yyyy")
            txtTo.Text = Format(Now, "MM/dd/yyyy")
        End If
        LoadGrd(txtFrom.Text, txtTo.Text, sSortField)
    End Function

    Private Function LoadGrd(ByVal sFromDate As String, ByVal sToDate As String, Optional ByVal sSortField As String = "") As Boolean
        Dim da As New SqlDataAdapter, ds As New DataSet
        Dim conn As New SqlConnection
		
        conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ParagraphicsConnectionString").ConnectionString
        da.SelectCommand = New SqlCommand
        da.SelectCommand.Connection = conn
        da.SelectCommand.CommandType = CommandType.StoredProcedure
        da.SelectCommand.CommandText = "dbo.ItemUsage"
        da.SelectCommand.Parameters.Add(New SqlParameter("@fromDate", sFromDate))
        da.SelectCommand.Parameters.Add(New SqlParameter("@toDate", sToDate))
        da.SelectCommand.Parameters.Add(New SqlParameter("@CustomerNumber", MyBase.CurrentUser.CustomerID))
        da.Fill(ds, "tbl1")

        Dim dv As New DataView(ds.Tables(0))

        If (sSortField <> "") Then
            If (sSortField = Session("Sort")) Then
                dv.Sort = sSortField & " DESC"
            Else
                dv.Sort = sSortField
            End If
        End If

        grd.DataSource = dv
        grd.DataBind()
    End Function
    'Private Function LoadGrd(Optional ByVal sSortField As String = "") As Boolean
    '    Dim cnn As OleDb.OleDbConnection
    '    Dim cmd As OleDb.OleDbCommand
    '    Dim dr As OleDb.OleDbDataReader
    '    Dim dnld As New dsDownloadsTableAdapters.DownloadLogTableAdapter
    '    Dim qa As New dsDownloadsTableAdapters.QueriesTableAdapter
    '    Dim bSuccess As Boolean = False
    '    Dim sSQL As String
    '    'check for details permissions
    '    Dim taUser As New dsAccessCodesTableAdapters.Customer_AccessCode1TableAdapter
    '    For Each dr1 As dsAccessCodes.Customer_AccessCode1Row In taUser.GetDataByCustomerIDandCode(CurrentUser.CustomerID, CurrentUser.AccessCode)
    '        If dr1.IsViewUsageNull = False Then
    '            Select Case dr1.ViewUsageDetails
    '                Case 0
    '                    grd.Columns(0).Visible = False
    '            End Select

    '        Else

    '            grd.Columns(0).Visible = False
    '        End If
    '    Next
    '    '--


    '    If ((IsDate(txtFrom.Text.ToString)) And IsDate(txtTo.Text.ToString)) Then
    '        sSQL = BuildSQL()
    '        If (sSQL.Length > 0) Then
    '            MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
    '            '--
    '            Dim dsUsage As New DataSet("Usage")


    '            Dim daUsage As New OleDb.OleDbDataAdapter(sSQL, cnn)
    '            daUsage.Fill(dsUsage, "Usage")
    '            '--
    '            MyBase.MyData.ReleaseReader2(cnn, cmd, dr)
    '            RemoveExcludedItems(dsUsage)
    '            dsUsage.AcceptChanges()
    '            Dim i As Integer = 0
    '            For Each dr1 As DataRow In dsUsage.Tables(0).Rows
    '                'Try
    '                'dr1("DownloadsCounter") = qa.GedDownloadedCount(dr1("ID"), txtTo.Text.ToString, txtFrom.Text.ToString)
    '                'Catch ex As Exception
    '                '    dr1("DownloadsCounter") = ""
    '                'End Try

    '                dr1("DownloadsCounter") = dnld.GetDnldCount(MyBase.CurrentUser.CustomerID, dr1("ID"), txtFrom.Text.ToString, txtTo.Text.ToString)



    '            Next

    '            Dim dv As New DataView(dsUsage.Tables(0))



    '            If (sSortField <> "") Then
    '                If (sSortField = Session("Sort")) Then
    '                    dv.Sort = sSortField & " DESC"
    '                Else
    '                    dv.Sort = sSortField
    '                End If
    '            End If




    '            grd.DataSource = dv




    '            grd.DataBind()
    '            Session("Sort") = dv.Sort


    '            grd.DataSource = dv
    '            'grd.DataSource = dr
    '            grd.DataBind()

    '            daUsage.Dispose()
    '            bSuccess = True
    '        End If
    '    End If

    '    Return (bSuccess)
    'End Function



    Private Sub RemoveExcludedItems(ByVal ds As DataSet)
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

        For iCounter = 0 To ds.Tables(0).Rows.Count - 1
            dRow = ds.Tables(0).Rows(iCounter)
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

    Private Function BuildSQL() As String
        Dim bShowActiveOnly As Boolean : bShowActiveOnly = True
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sFromOrd As String = String.Empty
        Dim sFromAdj As String = String.Empty
        Dim sJoinP1 As String = String.Empty
        Dim sJoinP2 As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty

        Dim sFromDate As String = String.Empty
        Dim sToDate As String = String.Empty

        sFromDate = txtFrom.Text.ToString
        sToDate = txtTo.Text.ToString

        'If (MyBase.CurrentUser.CanViewInventory = True) Then
        sSelect = "SELECT 0 as DownloadsCounter, Customer_Document.ID, iif(Customer_Document.PrintMethod=0,'FUL',iif(Customer_Document.PrintMethod=1,'POD','F/P')) AS ItmType, " & _
                  "Customer_Document.ReferenceNo AS RefName, Customer_Document.Description AS RefDesc, Prefix,  " & _
                  "iif(QtyUsed IS NULL,0,QtyUsed) AS QtyUsed, " & _
                  "iif(QtyReceived IS NULL,0,QtyReceived) AS QtyReceived, " & _
                  "iif(QtyDisposed IS NULL,0,QtyDisposed) AS QtyDisposed " & _
                  "FROM "
        sFromOrd = "(" & _
                   "SELECT Customer_Document.ID AS DocumentID, SUM(IIF(ISNULL(Customer_Document_Fill.ID),1,Customer_Document_Fill.QtyPerPull) * Order_Document.QtyOrdered) AS QtyUsed " & _
                   "FROM [Order] LEFT JOIN ((Order_Document LEFT JOIN Customer_Document ON Order_Document.DocumentID = Customer_Document.ID) LEFT JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID) ON Order.ID = Order_Document.OrderID " & _
                   "WHERE (Customer_Document.CustomerID = " & MyBase.CurrentUser.CustomerID & ") " & _
                   "AND (Order.StatusID <> 4) AND (Order.RequestDate>=#" & sFromDate & " 12:00 am#) AND (Order.RequestDate<=#" & sToDate & " 11:59 pm#) " & _
                   "GROUP BY Customer_Document.ID " & _
                   ") AS ord "
        sJoinP1 = " RIGHT JOIN (Customer_Document LEFT JOIN  "
        sFromAdj = "(" & _
                   "SELECT Customer_Document_History.DocumentID, " & _
                   "Sum(IIf(TranType='R',QtyLocP+QtyLocS+QtyLocW,0)) AS QtyReceived, " & _
                   "Sum(IIf(TranType='D',Abs(QtyLocP+QtyLocS+QtyLocW),0)) AS QtyDisposed " & _
                   "FROM Customer_Document_History " & _
                   "WHERE(Customer_Document_History.ModifiedDate >=#" & sFromDate & " 12:00 am#) And (Customer_Document_History.ModifiedDate <=#" & sToDate & " 11:59 pm#) " & _
                   "GROUP BY Customer_Document_History.DocumentID " & _
                   ") AS adj "
        sJoinP2 = " ON Customer_Document.ID = adj.DocumentID) ON ord.DocumentID=Customer_Document.ID "
        sWhere = "WHERE (Customer_Document.CustomerID = " & MyBase.CurrentUser.CustomerID & ") AND " & _
                 "(QtyUsed>0 OR QtyReceived>0 OR QtyDisposed>0) " & _
                 " AND ReferenceNo NOT LIKE '%DNLD' "
        sOrder = "ORDER BY Prefix, Customer_Document.ReferenceNo, Customer_Document.Description"
        'End If

        sSQL = sSelect + sFromOrd + sJoinP1 + sFromAdj + sJoinP2 + sWhere + sOrder
        Response.Write(sSQL)
        Return sSQL
    End Function


    Private Overloads Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdPrint.Click
        Response.Redirect("./AdmUseP.aspx?From=" & txtFrom.Text & "&To=" & txtTo.Text, False)
    End Sub

    Private Overloads Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRefresh.Click
        LoadData()
    End Sub

    Private Sub grd_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grd.ItemCommand

        If e.CommandName = "ViewDetails" Then
            Response.Redirect("AdmUseDetails.aspx?RefNumber=" & e.CommandArgument & "&dfrom=" & txtFrom.Text & "&dto=" & txtTo.Text)
        End If

    End Sub

    Private Sub grd_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles grd.SortCommand
        LoadData(e.SortExpression)
    End Sub
End Class