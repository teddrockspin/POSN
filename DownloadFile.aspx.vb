Imports System.IO

Partial Public Class DownloadFile
    Inherits paraPageBase
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()

        MyBase.Page_Init(sender, e)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyBase.OnPreRender(e)

        Dim strFileName As String = ""


        strFileName = Path.GetFileName(MapPath(Server.UrlDecode(Request.QueryString("f")))) 'get filename from path)
        Dim strFilePath As String = MapPath(Server.UrlDecode(Request.QueryString("f")))

        SendFile(strFilePath, strFileName)

        'Response.Write("You are not authorized to download this file")


    End Sub

    Private Sub SendFile(ByVal strPath As System.String, ByVal strSuggestedName As System.String)

        Dim strServerPath As String
        Dim objSourceFileInfo As System.IO.FileInfo

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


                Record()
                ' make sure response is sent
                .Flush()
                .Close()
                ' end response
                .End()

            End With
            Record()
            ' if the file does not exist
        Else

            ' show error page
            Response.Write("File Missing From Server")

        End If

    End Sub
    Sub Record()
        Dim ta As New dsDownloadsTableAdapters.DownloadLogTableAdapter

        ta.AddDnldCount(Request.QueryString("ID"), MyBase.CurrentUser.CustomerID, MyBase.CurrentUser.AccessCodeID, Date.Now)
    End Sub

End Class