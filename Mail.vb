Imports System.Web.Mail
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security

''''''''''''''''''''
'mw - 05-27-2007
'mw - 08-09-2006
''''''''''''''''''''
'Note:  Emails sent here use C:\Inetpub\mailroot\Pickup\ Directory 
'       SMTP Service must be running to get picked up
''''''''''''''''''''


Public Class Mail

    'Const SMTP_SERVER = "mail.<sitename>.com"

    'Public Function SendMessageOld(ByVal sTo As String, ByVal sFrom As String, _
    '                          ByVal sSubject As String, ByVal sBody As String, _
    '                          Optional ByVal sCC As String = "", Optional ByVal sBCC As String = "", Optional ByVal sAttach As String = "", Optional ByRef sResult As String = "", Optional ByVal bAsHTML As Boolean = False) As Boolean
    '    Dim bSuccess As Boolean = False
    '    Dim sMsg As String

    '    '  'get the product name, version and description from the assembly
    '    '  sMsg = vbCrLf + vbCrLf + _
    '    '    System.Diagnostics.FileVersionInfo.GetVersionInfo( _
    '    'System.Reflection.Assembly.GetExecutingAssembly.Location).ProductName _
    '    '        + " v" + System.Diagnostics.FileVersionInfo.GetVersionInfo( _
    '    'System.Reflection.Assembly.GetExecutingAssembly.Location _
    '    '      ).ProductVersion + vbCrLf + _
    '    '   System.Diagnostics.FileVersionInfo.GetVersionInfo( _
    '    '   System.Reflection.Assembly.GetExecutingAssembly.Location _
    '    '        ).Comments + vbCrLf + vbCrLf

    '    Try
    '        Dim insMail As New MailMessage
    '        With insMail
    '            If (bAsHTML = True) Then
    '                .BodyFormat = MailFormat.Html
    '            Else
    '                .BodyFormat = MailFormat.Text
    '            End If

    '            .From = Trim(sFrom)
    '            .To = Trim(sTo)
    '            .Subject = Trim(sSubject)
    '            .Body = Trim(sBody)

    '            If (Len(Trim(sCC)) > 0) Then .Cc = Trim(sCC)
    '            If (Len(Trim(sBCC)) > 0) Then .Bcc = Trim(sBCC)

    '            If (Len(sAttach) > 0) Then
    '                Dim sFile As String
    '                Dim sAttachments() As String = Split(sAttach, ";")
    '                For Each sFile In sAttachments
    '                    .Attachments.Add(New MailAttachment(Trim(sFile)))
    '                Next
    '            End If
    '        End With

    '        SmtpMail.Send(insMail)

    '        sResult = "Successfully Sent Mail"
    '        bSuccess = True

    '    Catch err As Exception
    '        sResult = "EXCEPTION " + err.Message
    '        bSuccess = False
    '    End Try

    '    Return bSuccess
    'End Function


    Public Function SendMessage(ByVal sTo As String, ByVal sFrom As String, _
                              ByVal sSubject As String, ByVal sBody As String, _
                              Optional ByVal sCC As String = "", Optional ByVal sBCC As String = "", Optional ByVal sAttach As String = "", Optional ByRef sResult As String = "", Optional ByVal bAsHTML As Boolean = False) As Boolean

        Dim bln As Boolean = False

        Dim str As String
        Dim strArr() As String
        Dim count As Integer
        str = sTo
        strArr = str.Split(";")
        For count = 0 To strArr.Length - 1
            bln = SendMessage2(strArr(count), sFrom, sSubject, sBody, sCC, sBCC, sAttach, sResult, bAsHTML)
        Next
        Return bln

    End Function

    Private Function SendMessage2(ByVal sTo As String, ByVal sFrom As String, _
                              ByVal sSubject As String, ByVal sBody As String, _
                              Optional ByVal sCC As String = "", Optional ByVal sBCC As String = "", Optional ByVal sAttach As String = "", Optional ByRef sResult As String = "", Optional ByVal bAsHTML As Boolean = False) As Boolean
        Dim bSuccess As Boolean = False

        Try


            Dim o As New paraOrder
            Dim msLogFile As String
            msLogFile = ConfigurationSettings.AppSettings("LogFolder") & "Ordr-" & Format(Now, "yyyyMMdd") & ".log"
            o.LogActivity("  SendMessage - trying to send email to: " & sTo & ", Subject: " & sSubject, msLogFile)

            Dim oMsg As New System.Net.Mail.MailMessage()


            oMsg.From = New System.Net.Mail.MailAddress(sFrom)
            oMsg.[To].Add(New System.Net.Mail.MailAddress(sTo))
            oMsg.Subject = sSubject
            oMsg.Body = sBody


            If sAttach <> "" Then
                Dim oAttch As New System.Net.Mail.Attachment(sAttach)
                oMsg.Attachments.Add(oAttch)
            End If

            oMsg.IsBodyHtml = True

            Dim s As New System.Net.Mail.SmtpClient(ConfigurationSettings.AppSettings("smtpserver"))
            'Dim nc As New System.Net.NetworkCredential(ConfigurationSettings.AppSettings("smtpuser"), ConfigurationSettings.AppSettings("smtppassword"))
            's.EnableSsl = True
            's.UseDefaultCredentials = False
            's.Credentials = nc

            'ServicePointManager.ServerCertificateValidationCallback = Function(sender As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) True

            s.Send(oMsg)

            bSuccess = True

        Catch err As Exception
            sResult = "EXCEPTION " + err.Message
            bSuccess = False
            Dim msLogFile As String = ConfigurationSettings.AppSettings("LogFolder") & "Ordr-" & Format(Now, "yyyyMMdd") & ".log"
            LogError(" SendMessage >> " & err.Message.ToString)
        End Try

        Return bSuccess
    End Function
    Private Sub LogError(ByVal sMsg As String, Optional ByVal sFile As String = "")
        Dim moLog As Log
        Dim sLogFile As String = String.Empty

        Try
            If (sFile.Length > 0) Then
                sLogFile = sFile
            Else
                sLogFile = ConfigurationSettings.AppSettings("LogFolder") & "OrderErr.log"
            End If
            moLog = New Log
            moLog.Entry(sLogFile, "      " & Format(Now, "MM-dd-yyyy hh:mm:ss") & " **ERROR** ORDER:  " & sMsg)

        Catch ex As Exception

        End Try
        moLog = Nothing
    End Sub

End Class
