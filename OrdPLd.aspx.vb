''''''''''''''''''''
'mw - 06-27-2007
''''''''''''''''''''


Partial Class OrdPLd
  Inherits paraPageBase

#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

  End Sub
  Protected WithEvents lblID As System.Web.UI.WebControls.Label

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
    'Put user code to initialize the page here

    Dim sED As String : sED = 0
    '    Dim sT As String : sT = "R"
    Dim bReq As Boolean : bReq = 0
    Dim bShp As Boolean : bShp = 0
    '   Dim bCst As Boolean : bCst = 0
    Dim sDestin As String = paraPageBase.PAG_OrdRequest.ToString()

    sED = Request.QueryString("ED").ToString
    '  sT = Request.QueryString("T").ToString
    bReq = IIf(Request.QueryString("LR").ToString = "1", True, False)
    bShp = IIf(Request.QueryString("LS").ToString = "1", True, False)
    ' bCst = IIf(Request.QueryString("LC").ToString = "1", True, False)

    MyBase.CurrentOrder.LoadFromPrior(MyBase.MyData, MyBase.CurrentUser.AccessCodeID, sED, bReq, bShp, False, False)
    MyBase.SessionStore()

    If (bReq = True) Then
      sDestin = paraPageBase.PAG_OrdRequest.ToString
    ElseIf (bShp = True) Then
      sDestin = paraPageBase.PAG_OrdShip.ToString
    End If
    MyBase.PageDirect(sDestin, 0, 0)
  End Sub

End Class
