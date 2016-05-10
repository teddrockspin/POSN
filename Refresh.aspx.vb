Public Partial Class Refresh
    Inherits paraPageBase

    Private Sub Refresh_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MyBase.Page_Init(sender, e)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoginUser()
    End Sub
    Private Function LoginUser(Optional ByRef sMsg As String = "") As Boolean
        '    Dim odat As paraData : odat = New paraData
        Dim sCustCode As String
        Dim sPwd As String
        Dim sAccCode As String
        Dim bResult As Boolean



        Dim qa As New dsCustomerTableAdapters.QueriesTableAdapter
        sPwd = qa.GetPassword(CurrentUser.CustomerID)

        'Response.Write(CurrentUser.CustomerCode & "<BR>" & sPwd & "<BR>" & CurrentUser.AccessCode)
   

        bResult = MyBase.MyData.VerifyUser(CurrentUser.CustomerCode, sPwd, CurrentUser.AccessCode, sMsg)
        
        Response.Redirect("./OrdSummary.aspx")
        Return bResult
    End Function
End Class