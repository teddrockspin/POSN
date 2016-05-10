''''''''''''''''''''
'mw - 12-09-2006
''''''''''''''''''''


Imports System.IO
Imports Microsoft.VisualBasic.ControlChars


Public Class Log

  Friend ReadOnly Property NewLine() As String
    Get
      Return vbCr & vbCrLf
    End Get
  End Property


  Friend Function Entry(ByVal sPath As String, ByVal sText As String) As Boolean
    Dim oWriter As StreamWriter

    If Not FileExists(sPath) Then
      oWriter = File.CreateText(sPath)
    Else
      oWriter = File.AppendText(sPath)
    End If
    oWriter.WriteLine(sText)
    oWriter.Flush()

    oWriter.Close()
    oWriter = Nothing
  End Function

  Public Function DirectoryExists(ByVal sPath As String) As Boolean
    Dim bResult As Boolean
    Dim f As New IO.DirectoryInfo(sPath)

    bResult = f.Exists
    If Not bResult Then
      f.Create()
      f.Refresh()
      bResult = f.Exists
    End If

    Return bResult
  End Function

  Public Function FileExists(ByVal sFilePath As String, Optional ByRef sFileSize As String = "") As Boolean
    Dim f As New IO.FileInfo(sFilePath)

    If (f.Exists) Then
      If (f.Length > 0) Then
        sFileSize = Format((f.Length / 1024), "0.0") & " kb"
      End If
    End If

    Return f.Exists
  End Function

End Class
