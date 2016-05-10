Imports System.Web
Imports System.Web.SessionState

Public Class [Global]
    Inherits System.Web.HttpApplication

#Region " Component Designer Generated Code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

#End Region

  Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
    ' Fires when the application is started

    Dim moLog As Log
    Dim sLogFile As String = String.Empty

    Try
      sLogFile = ConfigurationSettings.AppSettings("LogFolder") & "App.log"
      moLog = New Log
      moLog.Entry(sLogFile, "    APPLICATION Start at " & Now & "  for " & Session.SessionID)

    Catch ex As Exception

    End Try
    moLog = Nothing
  End Sub

  Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
    Dim moLog As Log
    Dim sLogFile As String = String.Empty

    Try
      sLogFile = ConfigurationSettings.AppSettings("LogFolder") & "Sess.log"
      moLog = New Log
      moLog.Entry(sLogFile, "    SESSION Start at " & Now & "  for " & Session.SessionID)

      '01-20-2007
      'Session.Timeout = 60
      '

      ' Fires when the session is started
      'Session("UState") = 0
      'Requestor/Shipping/Items/Kits/Customize
      '                    RSIKC
      'Session("PTitle") = String.Empty
      'Session("PMsg") = String.Empty
      'Session("PDescr") = String.Empty

      'Session("PUser") = New paraUser
      'Session("POrder") = New paraOrder
      'Session("PKit") = New paraKit

      'Session("ReviewResponse") = String.Empty

      Session.Add("PTitle", String.Empty)
      Session.Add("PMsg", String.Empty)
      Session.Add("PDescr", String.Empty)

      Session.Add("PUser", New paraUser)
      Session.Add("POrder", New paraOrder)
      Session.Add("PKit", New paraKit)

      Session.Add("ReviewResponse", String.Empty)
      'Session.Add("PKit", pCurrentKit)

    Catch ex As Exception

    End Try
    moLog = Nothing
  End Sub

  Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
    ' Fires at the beginning of each request
  End Sub

  Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
    ' Fires upon attempting to authenticate the use
  End Sub

  Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
    Dim ex1 As Exception = Server.GetLastError().GetBaseException()
    Dim moLog As Log
    Dim sLogFile As String = String.Empty

    ' Fires when an error occurs
    Try
      sLogFile = ConfigurationSettings.AppSettings("LogFolder") & Format(Now, "yyyyMMdd-hhmm") & "-AppErr.log"
      moLog = New Log
      moLog.Entry(sLogFile, "")
      moLog.Entry(sLogFile, "      Error at " & Now) '& "  for " & Session.SessionID)
      moLog.Entry(sLogFile, "           Msg:  " & ex1.Message)
      moLog.Entry(sLogFile, "           Src:  " & ex1.Source)
      moLog.Entry(sLogFile, "        StkTrc:  " & ex1.StackTrace)
      moLog.Entry(sLogFile, "        ReqFrm:  " & Request.Form.ToString())
      moLog.Entry(sLogFile, "        ReqStr:  " & Request.QueryString.ToString() & moLog.NewLine)

    Catch ex As Exception
      moLog.Entry(sLogFile, "        Application_Error:  " & ex.Message & "(" & sender.ToString & ")")
    End Try
    moLog = Nothing
  End Sub

  Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
    Dim moLog As Log
    Dim sLogFile As String = String.Empty

    ' Fires when the session ends
    ''    System.Web.HttpContext.Current.Session.Remove("PURL")
    ''System.Web.HttpContext.Current.Session.Remove("PHeader")
    ''System.Web.HttpContext.Current.Session.Remove("PStatus")

    ''System.Web.HttpContext.Current.Session.Remove("UState")

    'Already removed...
    Try
      sLogFile = ConfigurationSettings.AppSettings("LogFolder") & "Sess.log"
      moLog = New Log
      moLog.Entry(sLogFile, "    SESSION End at " & Now & "  for " & Session.SessionID & " by " & sender.ToString())
      moLog.Entry(sLogFile, "")

      System.Web.HttpContext.Current.Session.Remove("PTitle")
      System.Web.HttpContext.Current.Session.Remove("PMsg")
      System.Web.HttpContext.Current.Session.Remove("PDescr")

      System.Web.HttpContext.Current.Session.Remove("PUser")
      System.Web.HttpContext.Current.Session.Remove("POrder")
      System.Web.HttpContext.Current.Session.Remove("PKit")

      System.Web.HttpContext.Current.Session.Remove("ReviewResponse")

    Catch ex As Exception

    End Try
    'Response.Redirect("./Main.aspx", False)
    moLog = Nothing
  End Sub

  Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
    ' Fires when the application ends

    Dim moLog As Log
    Dim sLogFile As String = String.Empty

    Try
      sLogFile = ConfigurationSettings.AppSettings("LogFolder") & "App.log"
      moLog = New Log
      moLog.Entry(sLogFile, "    APPLICATION End at " & Now & "  for " & Session.SessionID & " by " & sender.ToString())
      moLog.Entry(sLogFile, "")

    Catch ex As Exception

    End Try
    moLog = Nothing
  End Sub

End Class
