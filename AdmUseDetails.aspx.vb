Public Partial Class AdmUseDetails
    Inherits paraPageBase

    Private Sub AdmUseDetails_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        InitializeComponent()

        MyBase.Page_Init(sender, e)
    End Sub
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageTitle = "Usage"
        'MyBase.Page_Load(sender, e)


        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        ElseIf Not (MyBase.CurrentUser.ViewUsage = True) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri


            Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
            lblDescrition.Text = qa.GetDocumentDescription(Request.QueryString("RefNumber"), CurrentUser.CustomerID)

            LoadData()


        End If 'End PostBack

    End Sub
    Sub LoadData(Optional ByVal strSortExpression As String = "Order Number Desc")



        Dim strRefNumber As String = Request.QueryString("RefNumber")
        Dim ta As New dsOrdersTableAdapters.dtUsageByRefNumInOrdersTableAdapter
        Dim dt As DataTable
        Dim dFrom As Date = Request.QueryString("dfrom")
        Dim dTo As Date = Request.QueryString("dto")

        Dim taUser As New dsAccessCodesTableAdapters.Customer_AccessCode1TableAdapter

        For Each dr As dsAccessCodes.Customer_AccessCode1Row In taUser.GetDataByCustomerIDandCode(CurrentUser.CustomerID, CurrentUser.AccessCode)
            If dr.IsViewUsageNull = False Then
                Select Case dr.ViewUsageDetails
                    Case 0


                    Case 1
                       
                        MyBase.PageMessage = "This detail usage information shown for <B>" & CurrentUser.AccessCode & "</b> access code only"
                        dt = ta.GetDataByRefNumber(strRefNumber, CurrentUser.CustomerID, CurrentUser.AccessCode, dFrom, dTo)
                        dt.Columns("Order Number").SetOrdinal(0)
                    Case 2
                        MyBase.PageMessage = "Detail usage information shown for all access codes"
                        dt = ta.GetDataByRefNumber(strRefNumber, CurrentUser.CustomerID, "%%", dFrom, dTo)
                        dt.Columns("Order Number").SetOrdinal(0)
                End Select


                If (strSortExpression <> "") AndAlso IsNothing(dt) = False Then
                    If (strSortExpression = Session("UserDetailSort")) And strSortExpression.ToUpper.Contains(" DESC") = False Then
                        dt.DefaultView.Sort = strSortExpression & " DESC"
                    Else
                        dt.DefaultView.Sort = strSortExpression
                    End If
                End If

                grd.DataSource = dt

                grd.DataBind()
                If IsNothing(dt) = False Then
                    Session("UserDetailSort") = dt.DefaultView.Sort




                End If

                grd.DataSource = dt
                grd.DataBind()

                For Each dc As DataGridColumn In grd.Columns
                    If dc.HeaderText = "" Then

                    End If
                Next
            End If
        Next


    End Sub

    Private Sub cmdBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Response.Redirect("AdmUse.aspx")
    End Sub

    Private Sub grd_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grd.ItemDataBound
        e.Item.Cells(11).Visible = False
    End Sub

    Private Sub grd_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles grd.SortCommand
        LoadData(e.SortExpression)
    End Sub
End Class