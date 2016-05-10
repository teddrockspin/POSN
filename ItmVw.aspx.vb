''''''''''''''''''''
'mw - 08-18-2009
''''''''''''''''''''


Partial Class ItmVw

  Inherits paraPageBase

  Private mlID As String

#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

  End Sub
  Protected WithEvents Div1 As System.Web.UI.HtmlControls.HtmlGenericControl

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
        PageTitle = "Item View"
        MyBase.PageMessage = ""

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            'MyBase.PageDirect()
            mlID = 0
        ElseIf Not (MyBase.CurrentUser.CanDownloadImages = True) And MyBase.CurrentUser.ViewThumbnails = False Then
            'MyBase.PageDirect()
            mlID = 0
        ElseIf (Page.IsPostBack = False) Then
            mlID = Val(Request.QueryString("I").ToString())
        End If

        If (Page.IsPostBack = False) Then
            LoadData()
            'qty()
        End If 'End 
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "scr", "showborder();", True)
    End Sub

  Private Sub LoadData()
    Dim bShow As Boolean = False

    If (mlID > 0) Then
            LoadImage(mlID)
            'GetDownloadableDetails(mlID)
      bShow = True
    Else
      ClearImage()
    End If
    If Not (pnlItm.Visible = bShow) Then pnlItm.Visible = bShow
    If (pnlNoItm.Visible = bShow) Then pnlNoItm.Visible = Not bShow
  End Sub

  Private Sub ClearImage()
    lblImgRef.Text = String.Empty
    lblImgDes.Text = String.Empty
    imgItem.ImageUrl = String.Empty
    lkLow.Text = String.Empty
    lkLow.NavigateUrl = String.Empty
    lkHigh.Text = String.Empty
    lkHigh.NavigateUrl = String.Empty
    lkBase.Text = String.Empty
    lkBase.NavigateUrl = String.Empty
  End Sub

  Private Sub LoadImage(ByVal lID As Long)
    Dim lItmID As Long = 0
    Dim sItmRef As String = String.Empty
    Dim sItmDes As String = String.Empty
    Dim sItmFile As String = String.Empty
    Dim sItem As String = String.Empty
    Dim sFile As String = String.Empty
    Dim sSize As String = String.Empty
    Dim bVisible As Boolean = False
    Dim LogLib As New Log
    Dim bCanDwnL As Boolean
    Dim bCanDwnS As Boolean
    Dim bCanDwnH As Boolean
        Dim strExtendedDescription As String

    bCanDwnL = MyBase.CurrentUser.CanDownloadImagesLowRes()
    bCanDwnS = MyBase.CurrentUser.CanDownloadImagesStandardRes()
    bCanDwnH = MyBase.CurrentUser.CanDownloadImagesHighRes()

    lItmID = lID
    'sItem = sItem
    'sItmRef = sRefNo
    'sItmDes = sDes
    'sItmFile = GetFileName(MyBase.CurrentUser.CustomerID, sItmRef)
        If MyBase.MyData.GetItmDetail(MyBase.CurrentUser.CustomerID, lID, sItmRef, sItmDes, sItmFile, strExtendedDescription) Then

            lblImgRef.Text = sItmRef
            lblImgDes.Text = sItmDes
            sItem = sItmFile

            sFile = "JPGs/" & MyBase.CurrentUser.CustomerCode & "/" & sItem & ".jpg"
            bVisible = LogLib.FileExists(Server.MapPath(sFile))
            If (bVisible = True) And MyBase.CurrentUser.ViewThumbnails = True Then
                imgItem.ImageUrl = sFile
            Else
                imgItem.ImageUrl = "./jpgs/NoImage.png"
            End If

            If (bCanDwnL = True) And MyBase.CurrentUser.CanDownloadImages = True Then
                sFile = "PDFs/" & MyBase.CurrentUser.CustomerCode & "/" & sItem & "_L.pdf"
                bVisible = LogLib.FileExists(Server.MapPath(sFile), sSize)
                If (bVisible = True) Then
                    lkLow.Text = "Viewable/Office Printer PDF (" & sSize & ")"
                    lkLow.ToolTip = "Open File/Download"
                    lkLow.NavigateUrl = "downloadfile.aspx?f=" & Server.UrlEncode(sFile) & "&id=" & lID
                    lkLow.Target = "_blank"
                Else
                    lkLow.Text = ""
                    lkLow.NavigateUrl = ""
                End If
            End If

            If (bCanDwnS = True) And MyBase.CurrentUser.CanDownloadImages = True Then
                sFile = "PDFs/" & MyBase.CurrentUser.CustomerCode & "/" & sItem & ".pdf"
                bVisible = LogLib.FileExists(Server.MapPath(sFile), sSize)
                If (bVisible = True) Then
                    lkBase.Text = "View PDF (" & sSize & ")"
                    lkBase.ToolTip = "Open File/Download"
                    lkBase.NavigateUrl = "downloadfile.aspx?f=" & Server.UrlEncode(sFile) & "&id=" & lID
                    lkBase.Target = "_blank"
                Else
                    lkBase.Text = ""
                    lkBase.NavigateUrl = ""
                End If
            End If

            If (bCanDwnH = True) And MyBase.CurrentUser.CanDownloadImages = True Then
                sFile = "PDFs/" & MyBase.CurrentUser.CustomerCode & "/" & sItem & "_H.pdf"
                bVisible = LogLib.FileExists(Server.MapPath(sFile), sSize)
                If (bVisible = True) Then
                    lkHigh.Text = "Commercial Print Ready PDF (" & sSize & ")"
                    lkHigh.ToolTip = "Open File/Download"
                    lkHigh.NavigateUrl = "downloadfile.aspx?f=" & Server.UrlEncode(sFile) & "&id=" & lID
                    lkHigh.Target = "_blank"
                Else
                    lkHigh.Text = ""
                    lkHigh.NavigateUrl = ""
                End If
            End If

            lblExtendedDescription.Text = strExtendedDescription
        Else
            ClearImage()
        End If

    LogLib = Nothing
  End Sub

  'Private Function GetItmDetails(ByVal lCustomerID As Long, ByVal lItmID As Long, _
  '                          ByRef sItmRef As String, ByRef sItmDes As String, ByRef sItmFile As String _
  '                          ) As Boolean
  '  Dim cnn As OleDb.OleDbConnection
  '  Dim cmd As OleDb.OleDbCommand
  '  Dim dr As OleDb.OleDbDataReader
  '  Dim bSuccess As Boolean = False
  '  Dim sSQL As String

  '  Try
  '    sSQL = "SELECT d.ReferenceNo AS RefNo, d.Description AS RefName, d.FileName " & _
  '           "FROM Customer_Document d " & _
  '           "WHERE CustomerID = " & lCustomerID & _
  '           " AND (d.ID = " & lItmID & ") "
  '    MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
  '    If dr.Read() Then
  '      sItmRef = dr("RefNo").ToString()
  '      sItmDes = dr("RefName").ToString()
  '      sItmFile = dr("FileName").ToString()
  '    End If
  '    MyBase.MyData.ReleaseReader2(cnn, cmd, dr)
  '    bSuccess = True

  '  Catch ex As Exception

  '  End Try

  '  Return bSuccess
  'End Function

  'Private Function GetFileName(ByVal lCustomerID As Long, ByVal sRefNo As String)
  '  Dim cnn As OleDb.OleDbConnection
  '  Dim cmd As OleDb.OleDbCommand
  '  Dim dr As OleDb.OleDbDataReader
  '  Dim bSuccess As Boolean = False
  '  Dim sSQL As String
  '  Dim sFile As String = String.Empty

  '  sSQL = "SELECT Customer_Document.FileName " & _
  '         "FROM Customer_Document " & _
  '         "WHERE CustomerID = " & lCustomerID & _
  '         " AND (Customer_Document.ReferenceNo = " & MyData.SQLString(sRefNo) & ") "
  '  MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
  '  If dr.Read() Then
  '    sFile = dr("FileName").ToString()
  '  End If
  '  MyBase.MyData.ReleaseReader2(cnn, cmd, dr)

  '  Return sFile
  'End Function


  '<asp:panel id="pnlItm" runat="server" visible="false">
  '</asp:panel>
  '<asp:panel id="pnlNoItm" runat="server" visible="false">
  '	<TABLE align="center" bgColor="transparent">
  '		<TR>
  '			<TD>
  '				<asp:label id="lblNoItm" runat="server" visible="false" text="Invalid Item..." CssClass="lblListB"></asp:label></TD>
  '		</TR>
  '	</TABLE>
  '</asp:panel>


  ' 	<div id="Div1" style="OVERFLOW: hidden; WIDTH: 100%; HEIGHT: 100%" runat="server">
  '	<div dir="ltr">
  '	</DIV>
  '</DIV>

    Sub GetDownloadableDetails(intDocumentID As Integer)
        Dim ta As New dsCustomerDocumentTableAdapters.Customer_Document_Downloadable1TableAdapter
        Dim dt As New dsCustomerDocument.Customer_Document_Downloadable1DataTable
        ta.FillByAccessCodeDoc(dt, intDocumentID, CurrentUser.AccessCodeID)

        If dt.Rows.Count > 0 Then
            rptDownloadables.DataSource = dt
            rptDownloadables.DataBind()
            'panelDownload.Visible = True
        Else
            panelDownload.Visible = False
        End If
    
    End Sub

    Function GetFileSize(sFile As String) As String
        Dim sSize As String = String.Empty
        Dim strPath As String = ConfigurationManager.AppSettings("dampath") & "\" & mlID & "\"
        Dim LogLib As New Log
        LogLib.FileExists(strPath & sFile, sSize)
        Return sSize
    End Function
End Class
