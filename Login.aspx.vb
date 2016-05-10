''''''''''''''''''''
'mw - 03-22-2008
'mw - 03-30-2007
'mw - 01-20-2007
''''''''''''''''''''


Imports System.Text
Imports System.IO

Partial Class Login
    Inherits paraPageBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmdBack As System.Web.UI.WebControls.Button
    Protected WithEvents cmdNext1 As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'If (Request.QueryString("x") <> "2") Then
        '  'Ensure a New Session on login
        '  Response.Cookies.Add(New HttpCookie("ASP.NET_SessionId", ""))
        '  Response.Redirect("Login.aspx?x=2")
        'Else

        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()

        'MyBase.Page_Init(sender, e)
        '    End If

        MyBase.Page_Init(sender, e)
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Protected override Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sDestin As String
        Dim sLinks As String
        Dim sScript As StringBuilder = New StringBuilder(200)
        Dim sCustCode As String = String.Empty
        Dim sAccCode As String = String.Empty
        Dim sPwd As String = String.Empty
        Dim sMsg As String = String.Empty

        PageTitle = "Login"
        MyBase.PageMessage = ""

        If lblMsg.Text <> "" Then
            divMessage.Visible = True
        Else
            divMessage.Visible = False
        End If
        If (Page.IsPostBack = False) Then

            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri

            sCustCode = Request.QueryString("C") & ""
            sAccCode = Request.QueryString("A") & ""
            sPwd = Request.QueryString("P") & ""
            If (sCustCode.Length > 0) And (sAccCode.Length > 0) And (sPwd.Length > 0) Then
                If (MyBase.MyData.VerifyUser(sCustCode, sPwd, sAccCode, sMsg) = True) Then

                    MoveOn()
                ElseIf (sMsg.Length > 0) Then
                    
                End If
            End If
        End If 'End PostBack
    End Sub
 
    Private Function LoginUser(Optional ByRef sMsg As String = "") As Boolean
        '    Dim odat As paraData : odat = New paraData
        Dim sCustCode As String
        Dim sPwd As String
        Dim sAccCode As String
        Dim bResult As Boolean
        Dim strErrMsg As String
        sCustCode = txtCustCode.Text
        sPwd = txtPwd.Text
        sAccCode = txtAccCode.Text

        bResult = MyBase.MyData.VerifyUser(sCustCode, sPwd, sAccCode, sMsg)


        If (sMsg.Length > 0) Then
            divMessage.Visible = True
            lblMsg.Text = sMsg
        End If

        '    odat.Dispose()
        Return bResult
    End Function


    Private Overloads Sub cmdNext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdNext.Click
        Dim sMsg As String = String.Empty

        If (LoginUser(sMsg)) Then
            If Request.QueryString("target") IsNot Nothing Then
                Response.Redirect(Request.QueryString("target"))
            Else
                MoveOn()
            End If


        Else
            MyBase.PageMessage = "Unable to Login... "
        End If
    End Sub


    Private Sub MoveOn()
        MyBase.PageDirect(MyBase.PAG_OrdItemSelect)
    End Sub



    Private Sub btntestEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btntestEmail.Click
        Dim myMail As New Mail
        myMail.SendMessage("festivenoodle@gmail.com", ConfigurationSettings.AppSettings("DefaultEmailAddress"), _
                                   "testing email", "testing", "", ConfigurationSettings.AppSettings("DefaultEmailCopy"), , , True)
    End Sub
End Class
