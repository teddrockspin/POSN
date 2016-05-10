''''''''''''''''''''
'mw - 03-17-2007
'mw - 10-29-2006
''''''''''''''''''''

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'If Decide to place Login Button in Logo...
'			<asp:ImageButton id="cmdLogin" style="Z-INDEX: 102; LEFT: 600px; POSITION: absolute; TOP: 320px"
'				runat="server" Width="272px" Height="35px" ImageUrl="images/Login.jpg"></asp:ImageButton>
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Partial Class Main
  Inherits paraPageBase

#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

  End Sub
  Protected WithEvents cmdLogin As System.Web.UI.WebControls.ImageButton
  Protected WithEvents lblTitle As System.Web.UI.WebControls.Label

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
    PageTitle = "Main"
    'MyBase.Page_Load(sender, e)
    MyBase.PageMessage = ""

    If Not Page.IsPostBack Then
      'Start with New Session when arrive
      If Not (Session.IsNewSession) Then
        Session.Abandon()
        Response.Cookies.Add(New HttpCookie("ASP.NET_SessionId", ""))
        'MyBase.SessionClear()
        Response.Redirect("Login.aspx")
        MyBase.PageMessage = "Session has ended - Starting new..."
      End If
    End If

    'If (MyBase.CurrentUser.State <> MyBase.MyData.STATE_LoggedIn) Then
    '  MyBase.PageMessage = "Welcome.  Please login."
    'End If
    '
    '    '  Response.Cookies.Add(New HttpCookie("ASP.NET_SessionId", ""))
    '    '  'If (Request.QueryString("STO") = "1") Then
    '    '  ''lblMsg.Text = "Timed Out...          Please login.          "
    '    '  ''LogActivity(False)
    '    '  ''MyBase.SessionClear()
    '    '  Response.Redirect("Login.aspx")
    '    '  'Else
    '    '  '  lblMsg.Text = ""
    '    '  'End If
    '  End If

    '  'LogActivity(True)
  End Sub

  Private Sub cmdLogin_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdLogin.Click
    MyBase.PageDirect(PAG_Login, 0, 0)
  End Sub
End Class
