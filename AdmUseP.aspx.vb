Imports System.Data.SqlClient

''''''''''''''''''''
'mw - 05-17-2008
'mw - 08-25-2007
'mw - 04-12-2007
'mw - 03-17-2007
''''''''''''''''''''


Partial Class AdmUseP
    Inherits paraPageBase


    Private msFrom As String = Format(Now, "MM/dd/yyyy")
    Protected WithEvents lblFrom As System.Web.UI.WebControls.Label
    Protected WithEvents txtFrom As System.Web.UI.WebControls.TextBox
    Protected WithEvents vtxtFrom As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lblTo As System.Web.UI.WebControls.Label
    Protected WithEvents txtTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents vtxtTo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vCompare As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents cmdRefresh As System.Web.UI.WebControls.Button
    Protected WithEvents lblDates As System.Web.UI.WebControls.Label
    Private msTo As String = Format(Now, "MM/dd/yyyy")


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
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        PageTitle = "Usage Preview"
        'MyBase.Page_Load(sender, e)

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri

            msFrom = Request.QueryString("From").ToString()
            msTo = Request.QueryString("To").ToString()

            LoadData()
        End If 'End PostBack
    End Sub

    Private Sub LoadData()
        lblHead.Text = "Usage from " & msFrom & "  to  " & msTo
        LoadGrd(msFrom, msTo)
    End Sub

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

    'Private Function LoadGrd() As Boolean
    '    Dim cnn As OleDb.OleDbConnection
    '    Dim cmd As OleDb.OleDbCommand
    '    Dim dr As OleDb.OleDbDataReader
    '    Dim bSuccess As Boolean = False
    '    Dim sSQL As String

    '    If ((IsDate(msFrom)) And IsDate(msTo)) Then
    '        sSQL = BuildSQL()
    '        If (sSQL.Length > 0) Then
    '            MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
    '            grd.DataSource = dr
    '            grd.DataBind()
    '            MyBase.MyData.ReleaseReader2(cnn, cmd, dr)
    '            bSuccess = True
    '        End If
    '    End If

    '    Return (bSuccess)
    'End Function

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

        sFromDate = msFrom
        sToDate = msTo

        If (MyBase.CurrentUser.CanViewInventory = True) Then
            '  sSelect = "SELECT Customer_Document.ID, iif(Customer_Document.PrintMethod=0,'FUL',iif(Customer_Document.PrintMethod=1,'POD','F/P')) AS ItmType, Customer_Document.ReferenceNo + ' - ' + Customer_Document.Description AS RefDesc, SUM(IIF(ISNULL(Customer_Document_Fill.ID),1,Customer_Document_Fill.QtyPerPull) * Order_Document.QtyOrdered) AS SumOfQtyOrdered " & _
            '         "FROM [Order] LEFT JOIN ((Order_Document LEFT JOIN Customer_Document ON Order_Document.DocumentID = Customer_Document.ID) LEFT JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID) ON Order.ID = Order_Document.OrderID "

            '  sWhere = " (Customer_Document.CustomerID = " & MyBase.CurrentUser.CustomerID & ") " & _
            '           " AND (Order.StatusID <> 4) AND (Order.RequestDate>=#" & sFromDate & " 12:00 am#) AND (Order.RequestDate<=#" & sToDate & " 11:59 pm#) "
            '  If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

            '  sGroup = " GROUP BY Customer_Document.ID, iif(Customer_Document.PrintMethod=0,'FUL',iif(Customer_Document.PrintMethod=1,'POD','F/P')), Customer_Document.ReferenceNo, Customer_Document.Description"

            '  sOrder = " ORDER BY Customer_Document.ReferenceNo, Customer_Document.Description"
            'End If

            'sSQL = sSelect + sWhere + sGroup + sOrder

            'sSelect = "SELECT Customer_Document.ID, iif(Customer_Document.PrintMethod=0,'FUL',iif(Customer_Document.PrintMethod=1,'POD','F/P')) AS ItmType, " & _
            '          "Customer_Document.ReferenceNo + ' - ' + Customer_Document.Description AS RefDesc, " & _
            '          "iif(QtyUsed IS NULL,0,QtyUsed) AS QtyUsed, " & _
            '          "iif(QtyReceived IS NULL,0,QtyReceived) AS QtyReceived, " & _
            '          "iif(QtyDisposed IS NULL,0,QtyDisposed) AS QtyDisposed " & _
            '          "FROM "
            sSelect = "SELECT Customer_Document.ID, iif(Customer_Document.PrintMethod=0,'FUL',iif(Customer_Document.PrintMethod=1,'POD','F/P')) AS ItmType, " & _
                      "Customer_Document.ReferenceNo AS RefName, Customer_Document.Description AS RefDesc, " & _
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
            sOrder = "ORDER BY Customer_Document.ReferenceNo, Customer_Document.Description"
        End If

        sSQL = sSelect + sFromOrd + sJoinP1 + sFromAdj + sJoinP2 + sWhere + sOrder

        Return sSQL
    End Function

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Dim sDestin As String = paraPageBase.PAG_AdmUsage.ToString()

        MyBase.PageDirect(sDestin, 0, 0)
    End Sub

End Class
