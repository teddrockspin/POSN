''''''''''''''''''''
'mw - 03-17-2007
'mw - 01-20-2007
''''''''''''''''''''


Imports System.Text

Partial Class Logout
  Inherits paraPageBase
#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

  End Sub
  Protected WithEvents Label1 As System.Web.UI.WebControls.Label

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
    PageTitle = "Logout"
    'MyBase.Page_Load(sender, e)
    MyBase.PageMessage = ""

    ''Disable Back Button - to avoid confusion - Logout - then cannot go back
    'Dim pScript As New StringBuilder
    'pScript.Append("<script>" & _
    '        "history.forward();" & _
    '    "</script>")
    'Me.RegisterStartupScript("Startup", pScript.ToString)
    ''

    'If (Request.QueryString("STO") = "1") Then
    '  lblMsg.Text = "Timed Out...          Please login.          "
    'Else
    '  lblMsg.Text = ""
    'End If

    ''Seem to be recommended
    'Response.Cache.SetExpires(Now.AddSeconds(-1))
    'Response.Cache.SetCacheability(HttpCacheability.NoCache)
    'Response.Cache.SetNoStore()
    ''Response.AppendHeader("Pragma", "no-cache")
    'Response.AppendHeader("Pragma", "no-store")
    'Response.Cache.SetLastModified(Now)
    'Response.Cache.SetAllowResponseInBrowserHistory(False)

    '    MyBase.PageDirect(MyBase.PAG_Main)
    If Not Page.IsPostBack Then
      LogActivity()
      MyBase.SessionClear("Logout")
      'Part of SessionClear Now
      'Session.Abandon()
      'Response.Cookies.Add(New HttpCookie("ASP.NET_SessionId", ""))
      '
    End If
  End Sub

  Private Sub LogActivity()
    Dim moLog As Log
    Dim sLogFile As String = String.Empty

    Try
      sLogFile = ConfigurationSettings.AppSettings("LogFolder") & "Sess.log"
      moLog = New Log
      moLog.Entry(sLogFile, "      LogOut - Logout Request at " & Format(Now, "MM-dd-yyyy hh:mm:ss") & "  for " & Session.SessionID)
      moLog.Entry(sLogFile, "        Logout - Session Close at " & Format(Now, "MM-dd-yyyy hh:mm:ss") & "  for " & Session.SessionID)

    Catch ex As Exception

    End Try
    moLog = Nothing
  End Sub

End Class
