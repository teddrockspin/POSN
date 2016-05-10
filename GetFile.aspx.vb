Imports System.IO

Public Class GetFile
    Inherits paraPageBase
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        'InitializeComponent()

        MyBase.Page_Init(sender, e)
        '    LoadData()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageTitle = "Usage"
        'MyBase.Page_Load(sender, e)
        MyBase.PageMessage = "Use the date range to filter usage information."

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        ElseIf Not (MyBase.CurrentUser.ViewUsage = True) Then
            MyBase.PageDirect()
        End If

        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter

        Dim dnld As New dsDownloadsTableAdapters.DownloadLogTableAdapter
        If Request.QueryString("id") <> "" And Request.QueryString("fid") <> "" AndAlso IsNumeric(Request.QueryString("fid")) Then


            Dim dt As New DateTime
            dt = qa.GetDownloadExpirationDate(Request.QueryString("id"))
            'Response.Write(dt)
            If dt.AddDays(2) >= Now Then
                qa.IncreaseDownloadCounterByOne(Request.QueryString("id"))
                'dnld.AddDnldCount(Request.QueryString("id"), MyBase.CurrentUser.CustomerID, MyBase.CurrentUser.AccessCodeID, Date.Now)
                'dnld.AddDnldCountByDnldID(MyBase.CurrentUser.CustomerID, MyBase.CurrentUser.AccessCodeID, Date.Now, Convert.ToInt32(Request.QueryString("id")))
                SendFile(ConfigurationManager.AppSettings("dampath") & qa.GetDownloadableFile(Request.QueryString("id"), Request.QueryString("fid")))

            Else
                Response.Write("Your download has expired...")
            End If

        Else
            If Request.QueryString("docID") <> "" And Request.QueryString("f") <> "" Then
                Dim da As New dsCustomerDocumentTableAdapters.Customer_Document_Downloadable1TableAdapter
                qa.IncreaseDownloadCounterByOne(Request.QueryString("docID"))
                'dnld.AddDnldCount(Convert.ToInt32(Request.QueryString("docID")), MyBase.CurrentUser.CustomerID, MyBase.CurrentUser.AccessCodeID, Date.Now)
                dnld.AddDnldCountByDnldID(MyBase.CurrentUser.CustomerID, MyBase.CurrentUser.AccessCodeID, Date.Now, Convert.ToInt32(Request.QueryString("docID")))
                SendFile(ConfigurationManager.AppSettings("dampath") & da.GetDownloadableFile(Convert.ToInt32(Request.QueryString("docID")), Convert.ToInt32(Request.QueryString("f"))))
            Else
                Response.Write(Request.QueryString("id") & "<BR>" & Request.QueryString("fid"))
            End If
        End If

    End Sub
    Private Sub SendFile(ByVal strPath As System.String)

        Dim strServerPath As String
        Dim objSourceFileInfo As System.IO.FileInfo
        Dim strSuggestedName As String = Path.GetFileName(strPath)
        ' convert relative path to path on server machine
        strServerPath = strPath 'Me.Server.MapPath(strPath)

        ' get fileinfo of source file
        objSourceFileInfo = New System.IO.FileInfo(strServerPath)

        ' if the file exists
        If objSourceFileInfo.Exists Then

            With Me.Response
                Response.Clear()
                Response.ClearContent()
                Response.ClearHeaders()

                ' tell the browser what content type to expect
                If strSuggestedName.Contains("pdf") Then
                    .ContentType = "application/pdf"
                Else
                    .ContentType = "application/octet-stream"
                End If

                .AddHeader("Accept-Ranges", "bytes")
                ' tell the browser to save rather than display inline
                .AddHeader("Content-Disposition", "attachment; filename=" & strSuggestedName)

                ' tell the browser how big the file is
                .AddHeader("Content-Length", objSourceFileInfo.Length.ToString)

                ' send the file to the browser
                .WriteFile(objSourceFileInfo.FullName)
                .TransmitFile(objSourceFileInfo.FullName)



                ' make sure response is sent
                .Flush()
                .Close()
                ' end response
                .End()

            End With

            ' if the file does not exist
        Else

            ' show error page
            Response.Write("File Missing From Server")

        End If

    End Sub
End Class