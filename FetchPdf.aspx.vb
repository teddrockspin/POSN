Public Class FetchPdf
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


     

        If Request.QueryString("ID") <> "" Then
            'Dim q As New dsDownloadsTableAdapters.QueriesTableAdapter
            Dim iID As Integer = Request.QueryString("ID") 'q.CheckIfDocumenthasDownloadableItem(Request.QueryString("ID"))

            SendFile(ConfigurationManager.AppSettings("dampath") & iID & "\preview\" & iID & ".pdf", iID & ".pdf")
        Else
            SendFile("C:\DAM\8437\preview\8437.pdf", "8437.pdf")
        End If


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



                ' make sure response is sent
                .Flush()
                .Close()
                ' end response
                .End()

            End With

            ' if the file does not exist
        Else

            ' show error page
            Response.Write("File Missing From Server <BR>" & strPath)

        End If

    End Sub
End Class