''''''''''''''''''''
'mw - 08-12-2009
'mw - 07-20-2009
'mw - 01-31-2009
'mw - 10-30-2007
'mw - 09-14-2007
'mw - 02-10-2007
'mw - 11-18-2006
''''''''''''''''''''


Imports System.Data
Imports System.Data.OleDb
'Imports System.Data.Odbc
Imports System.Text
Imports System.Data.SqlClient


Public Class paraData
    Inherits System.Web.UI.Page




    Protected cnnMain As OleDbConnection

    'Friend STATE_LoggedOut As Integer = 0
    'Friend STATE_LoggedIn As Integer = 1
    'Friend STATE_LoggedINOIP As Integer = 2

    Friend Const STATE_None = 0
    Friend Const STATE_LoggedOut = 0
    Friend Const STATE_LoggedIn = 1
    '  Friend Const STATE_InOrder = 2
    Friend Const STATE_InAdmin = 3

    Friend Const STATUS_INAC = 0
    Friend Const STATUS_ACTV = 1
    Friend Const STATUS_BACK = 2
    Friend Const STATUS_CNCL = 4
    Friend Const STATUS_HOLD = 6
    Friend Const STATUS_FUT = 7
    'Friend Const STATUS_ACT = 1
    'Friend Const STATUS_HLD = 6

    Friend Const STATUS_NONE = 0
    Friend Const STATUS_PRT = 1

    Friend Const PM_FULFILL = 0
    Friend Const PM_PRINT = 1
    Friend Const PM_DOWNLOAD = 2

    Friend Const PERM_NONE = 0
    Friend Const PERM_LOCAL = 1
    Friend Const PERM_GLOBAL = 2
    'mw - 01-24-2009
    Friend Const PERM_TEMP = 3
    '

    Friend Const DT_FILLONLY = 0
    Friend Const DT_PRINTONLY = 1
    Friend Const DT_FILLPRINT = 2
    Friend Const DT_DOWNLOADONLY = 3

    Friend Const CST_DET = 0
    Friend Const CST_REQ = 1
    Friend Const CST_SHP = 2

    Friend Const COLOR_INACTIVE = "RED"
    'Friend Const COLOR_BACK = "GREEN"
    'Friend Const COLOR_BACK = "DarkOrange"
    Friend Const COLOR_BACK = "#bd5304"
    Friend Const COLOR_GLOBAL = "BLUE"
    'Friend Const COLOR_NORMAL = "#BLACK"
    Friend Const COLOR_NORMAL = "BLACK"
    'Friend Const COLOR_BLUE = "#000080"
    Friend Const COLOR_BASEKIT = "PURPLE"

    Friend Const PRO_REQ = 1
    Friend Const PRO_SHP = 2
    Friend Const PRO_ITM = 3
    Friend Const PRO_KIT = 4
    Friend Const PRO_CST = 5
    Friend Const PRO_CRT = 6



    'Protected Const SES_StyleSheet = "StyleSheet"
    'Protected Const SES_UState = "UState"
    'Protected Const SES_UAccRight = "UAccRight"
    'Protected Const SES_URL = "URL"
    'Protected Const SES_PageTitle = "PTitle"
    'Protected Const SES_PageHeader = "PHeader"
    'Protected Const SES_PageMsg = "PMsg"
    'Protected Const SES_PageStatus = "PStatus"
    'Protected Const SES_PageDescription = "PDescr"
    'Protected Const SES_PageKeywords = "PKeys"

    'Protected Const SES_CustID = "CustID"
    'Protected Const SES_CustCode = "CustCode"
    'Protected Const SES_CustName = "CustName"
    'Protected Const SES_ProcChg = "ProcessingChg"
    'Protected Const SES_MnthChg = "MonthlyChg"
    'Protected Const SES_FMkpChg = "FreightChg"
    'Protected Const SES_LnPChg = "LinePckChg"
    'Protected Const SES_LnKChg = "LineKitChg"

    'Protected Const SES_AccID = "AccCodeID"
    'Protected Const SES_AccCode = "AccCode"
    'Protected Const SES_AccRights = "WebSet"

    '''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Master Session List
    '''''''''''''''''''''
    'Session("UState")
    'Session("PMsg")

    'Session("CustomerID")
    'Session("CustomerCode")
    'Session("CustomerName")

    'Session("OrdSpec")
    'Session("OrdDocCart")
    'Session("OrdKitCart")

    'Session("ProcessingChg")
    'Session("MonthlyChg")
    'Session("FreightMarkupChg")
    'Session("LinePckChg")
    'Session("LineKitChg")

    'Session("AccessCodeID")
    'Session("AccessCode")
    'Session("UAccRight")
    '''''''''''''''''''''''''''''''''''''''''''''''''''''

    'Public Enum LoginState
    '  LoggedOut = 0
    '  LoggedIn = 1
    '  LoggedInOIP = 2
    'End Enum


    Protected ReadOnly Property paraCONN() As String
        Get


            Return ConfigurationManager.ConnectionStrings.Item("OrderConnectionString").ConnectionString
            'Return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & Server.MapPath("dat/Order.mdb") & "';Jet OLEDB:Database Password=pr1NT06;"

        End Get
    End Property

    Protected ReadOnly Property paraSQLCONN() As String
        Get


            Return ConfigurationManager.ConnectionStrings.Item("ParagraphicsConnectionString").ConnectionString
            'Return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & Server.MapPath("dat/Order.mdb") & "';Jet OLEDB:Database Password=pr1NT06;"

        End Get
    End Property

    Protected Function CleanString(ByVal sInText As String) As String
        Dim sText As String : sText = String.Empty

        sText = sInText
        'sText = sInText.Replace("<", "")
        'sText = sInText.Replace(">", "")
        'sText = sInText.Replace("/", "")
        'sText = sInText.Replace("{", "")
        'sText = sInText.Replace("}", "")
        'sText = sInText.Replace("|", "")
        'sText = sInText.Replace("~", "")
        sText = sText.Replace("<", "")
        sText = sText.Replace(">", "")
        'sText = sText.Replace("/", "")
        sText = sText.Replace("{", "")
        sText = sText.Replace("}", "")
        sText = sText.Replace("|", "")
        sText = sText.Replace("~", "")
        sText = sText.Replace("'", "''")
        'For spaces...  does not seem to make a difference later so may leave for now
        'sText = sText.Replace(Chr(13), " ")

        Return sText
    End Function

    Friend Function SQLString(ByVal sInText As Object) As String
        Dim sText As StringBuilder : sText = New StringBuilder(200)
        If sInText Is Nothing Then
            sText.Append("''")
        Else

            sText.Append("'")
            sText.Append(CleanString(sInText))
            sText.Append("'")
        End If


        Return sText.ToString
    End Function

    Friend Function GetDataSet(ByRef cnn As OleDbConnection, ByRef cmd As OleDbCommand, ByRef ds As DataSet, ByVal sSQL As String) As Boolean
        Dim da As OleDbDataAdapter
        'Dim ds As DataSet
        Dim bErr As Boolean

        Try
            cnn = New OleDbConnection(paraCONN)
            cnn.Open()

            cmd = New OleDbCommand(sSQL, cnn)
            da = New OleDbDataAdapter(cmd)
            ds = New DataSet
            da.Fill(ds, 0)

            If Not (cmd Is Nothing) Then
                If cmd.Connection Is Nothing Then cmd.Connection.Dispose()
                cmd.Dispose()
            End If
            If Not (cnn Is Nothing) Then
                If cnn.State <> ConnectionState.Closed Then cnn.Close()
                cnn.Dispose()
            End If
            'rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

        Catch ex As Exception
            bErr = True
            LogError("GetReader2 >> " & ex.Message.ToString & " >> " & sSQL)
        End Try

        Return Not bErr
    End Function

    Friend Function GetDataSetSql(ByRef cnn As SqlConnection, ByRef cmd As SqlCommand, ByRef ds As DataSet, ByVal sSQL As String) As Boolean
        Dim da As SqlDataAdapter
        'Dim ds As DataSet
        Dim bErr As Boolean

        Try
            cnn = New SqlConnection(paraSQLCONN)
            cnn.Open()

            cmd = New sqlCommand(sSQL, cnn)
            da = New SqlDataAdapter(cmd)
            ds = New DataSet
            da.Fill(ds, 0)

            If Not (cmd Is Nothing) Then
                If cmd.Connection Is Nothing Then cmd.Connection.Dispose()
                cmd.Dispose()
            End If
            If Not (cnn Is Nothing) Then
                If cnn.State <> ConnectionState.Closed Then cnn.Close()
                cnn.Dispose()
            End If
            'rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

        Catch ex As Exception
            bErr = True
            LogError("GetReader2 >> " & ex.Message.ToString & " >> " & sSQL)
        End Try

        Return Not bErr
    End Function

    Friend Function GetReaderSQL(ByRef cnn As SqlConnection, ByRef cmd As SqlCommand, ByRef rdr As SqlDataReader, ByVal sSQL As String, Optional ByRef strErrMsg As String = "") As Boolean
        Dim bErr As Boolean

        Try
            If Not IsNothing(cnn) AndAlso cnn.State <> ConnectionState.Open Then
                cnn = New SqlConnection(paraSQLCONN)
                cnn.Open()
            ElseIf IsNothing(cnn) Then
                cnn = New SqlConnection(paraSQLCONN)
                cnn.Open()
            End If


            cmd = New SqlCommand(sSQL, cnn)

            rdr = cmd.ExecuteReader()

        Catch ex As Exception
            bErr = True

            'If Request.ServerVariables("SERVER_NAME").ToLower = "localhost" Or Request.ServerVariables("SERVER_NAME").ToLower = "para.rmrdevelopment.com" Then
            strErrMsg = (ex.Message)
            'End If

            LogError("GetReader2 >> " & ex.Message.ToString & " >> " & sSQL)
        End Try

        Return Not bErr
    End Function

    Friend Function GetReader2(ByRef cnn As OleDbConnection, ByRef cmd As OleDbCommand, ByRef rdr As OleDbDataReader, ByVal sSQL As String, Optional ByRef strErrMsg As String = "") As Boolean
        Dim bErr As Boolean

        Try
            If Not IsNothing(cnn) AndAlso cnn.State <> ConnectionState.Open Then
                cnn = New OleDbConnection(paraCONN)
                cnn.Open()
            ElseIf IsNothing(cnn) Then
                cnn = New OleDbConnection(paraCONN)
                cnn.Open()
            End If


            cmd = New OleDbCommand(sSQL, cnn)

            rdr = cmd.ExecuteReader()

        Catch ex As Exception
            bErr = True

            'If Request.ServerVariables("SERVER_NAME").ToLower = "localhost" Or Request.ServerVariables("SERVER_NAME").ToLower = "para.rmrdevelopment.com" Then
            strErrMsg = (ex.Message)
            'End If

            LogError("GetReader2 >> " & ex.Message.ToString & " >> " & sSQL)
        End Try

        Return Not bErr
    End Function

    Friend Function ReleaseReaderSQL(ByRef cnn As SqlConnection, ByRef cmd As SqlCommand, ByRef rdr As SqlDataReader) As Boolean
        Dim bErr As Boolean

        Try
            If Not (rdr Is Nothing) Then
                If (Not rdr.IsClosed) Then rdr.Close()
                rdr = Nothing
            End If

            If Not (cmd Is Nothing) Then
                If Not (cmd.Connection Is Nothing) Then cmd.Connection.Dispose()
                cmd.Dispose()
            End If

            If Not (cnn Is Nothing) Then
                If cnn.State <> ConnectionState.Closed Then cnn.Close()
                cnn.Dispose()
            End If

        Catch ex As Exception
            bErr = True
            LogError("ReleaseReader2 >> " & ex.Message.ToString)
        End Try

        Return Not bErr
    End Function

    Friend Function ReleaseReader2(ByRef cnn As OleDbConnection, ByRef cmd As OleDbCommand, ByRef rdr As OleDbDataReader) As Boolean
        Dim bErr As Boolean

        Try
            If Not (rdr Is Nothing) Then
                If (Not rdr.IsClosed) Then rdr.Close()
                rdr = Nothing
            End If

            If Not (cmd Is Nothing) Then
                If Not (cmd.Connection Is Nothing) Then cmd.Connection.Dispose()
                cmd.Dispose()
            End If

            If Not (cnn Is Nothing) Then
                If cnn.State <> ConnectionState.Closed Then cnn.Close()
                cnn.Dispose()
            End If

        Catch ex As Exception
            bErr = True
            LogError("ReleaseReader2 >> " & ex.Message.ToString)
        End Try

        Return Not bErr
    End Function

    Friend Function GetConnection(ByRef cnn As OleDbConnection) As Boolean
        Dim bErr As Boolean

        Try
            cnn = New OleDbConnection(paraCONN)
            'cnn.mode = adModeReadWrite
            cnn.Open()

            '      Conn = Server.CreateObject("ADODB.Connection")
            '      Conn.Mode = 3 ' 3 = adModeReadWrite 
            '      Conn.Open("myDSN")
            '      Conn.Execute(SQL)
            '      Conn.Close()

        Catch ex As Exception
            bErr = True
            LogError("GetConnection >> " & ex.Message.ToString)
        End Try

        Return Not bErr
    End Function

    Friend Function GetConnectionSQL(ByRef cnn As SqlConnection) As Boolean
        Dim bErr As Boolean

        Try
            cnn = New SqlConnection(paraSQLCONN)
            'cnn.mode = adModeReadWrite
            cnn.Open()

            '      Conn = Server.CreateObject("ADODB.Connection")
            '      Conn.Mode = 3 ' 3 = adModeReadWrite 
            '      Conn.Open("myDSN")
            '      Conn.Execute(SQL)
            '      Conn.Close()

        Catch ex As Exception
            bErr = True
            LogError("GetConnection >> " & ex.Message.ToString)
        End Try

        Return Not bErr
    End Function

    Friend Function ReleaseConnection(ByRef cnn As OleDbConnection) As Boolean
        Dim bErr As Boolean

        Try
            If Not (cnn Is Nothing) Then
                If cnn.State <> ConnectionState.Closed Then cnn.Close()
                cnn.Dispose()
            End If

        Catch ex As Exception
            bErr = True
            LogError("ReleaseConnection >> " & ex.Message.ToString)
        End Try
        cnn = Nothing

        Return Not bErr
    End Function
    Friend Function ReleaseConnectionSQL(ByRef cnn As SqlConnection) As Boolean
        Dim bErr As Boolean

        Try
            If Not (cnn Is Nothing) Then
                If cnn.State <> ConnectionState.Closed Then cnn.Close()
                cnn.Dispose()
            End If

        Catch ex As Exception
            bErr = True
            LogError("ReleaseConnection >> " & ex.Message.ToString)
        End Try
        cnn = Nothing

        Return Not bErr
    End Function
    Friend Function SQLExecute(ByRef cnnE As OleDb.OleDbConnection, ByVal sSQL As String, Optional ByRef sMsg As String = "") As Boolean
        Dim cmd As OleDb.OleDbCommand
        Dim bErr As Boolean

        Try
            If cnnE.State = ConnectionState.Closed Then
                cnnE.Open()
            End If
            cmd = New OleDbCommand(sSQL & ";", cnnE) ' 
            cmd.ExecuteScalar()
            sMsg = "  Successful Execute"

        Catch ex As Exception
            bErr = True
            sMsg = ex.Message.ToString
            LogError("SQLExecute >> " & ex.Message.ToString & " >> " & sSQL)
        End Try
        cmd.Dispose()

        Return Not bErr
    End Function
    Friend Function SQLExecuteSQL(ByRef cnnE As SqlConnection, ByVal sSQL As String, Optional ByRef sMsg As String = "") As Boolean
        Dim cmd As SqlCommand
        Dim bErr As Boolean

        Try
            If cnnE.State = ConnectionState.Closed Then
                cnnE.Open()
            End If
            cmd = New SqlCommand(sSQL & ";", cnnE) ' 
            cmd.ExecuteScalar()
            sMsg = "  Successful Execute"

        Catch ex As Exception
            bErr = True
            sMsg = ex.Message.ToString
            LogError("SQLExecute >> " & ex.Message.ToString & " >> " & sSQL)
        End Try
        cmd.Dispose()

        Return Not bErr
    End Function
    '  Friend Function SQLExecuteIdentity(ByRef cnnE As OleDb.OleDbConnection, ByVal sSQL As String, Optional ByVal lID As Long = 0, Optional ByRef sMsg As String = "") As Boolean
    'dim ADODB.Connection adoConnection = new ADODB.ConnectionClass();
    'ADODB.Recordset adoRecordset = new ADODB.Recordset();
    'ADODB.Command adoCommand = new ADODB.CommandClass();

    '    Dim cmd As OleDb.OleDbCommand
    '    Dim rdr As OleDb.OleDbDataReader
    '    Dim bErr As Boolean

    '    Try
    '      cmd = New OleDbCommand(sSQL & "; SELECT @@IDENTITY AS ID;", cnnE)

    '      rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
    '      If (rdr.Read) Then
    '        lID = rdr("ID")
    '      End If
    '      sMsg = "  Successful Execute"

    '      rdr.Close()
    '      rdr = Nothing
    '      cmd.Dispose()
    '    Catch ex As Exception
    '      bErr = True
    '      sMsg = ex.Message.ToString
    '    End Try
    '    cmd.Dispose()

    '    Return Not bErr
    '  End Function


    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'LOGIN
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Friend Function VerifyUser(ByVal CustomerCode As String, ByVal Pwd As String, ByVal AccessCode As String, Optional ByRef sMsg As String = "") As Boolean
        Dim cnn As SqlConnection
        Dim cmd As SqlCommand
        Dim rcd As SqlDataReader
        Dim sSQL As String
        Dim bSuccess As Boolean
        Dim oMyUser As paraUser = New paraUser
        Dim oLog As Log = New Log


        sSQL = "SELECT Customer.ID AS CustomerID, Customer.Code AS CustomerCode, Customer.Name AS CustomerName, Customer.Email AS CustomerEmail, Customer.Phone AS CustomerPhone," & _
               "Customer_AccessCode.ID AS AccessCodeID, Customer_AccessCode.Code AS AccessCode, Customer_AccessCode.Email AS AccessCodeEmail, " & _
               "Customer_AccessCode.WebSet AS WebSettings, Customer_AccessCode.MaxQtyPerLineItem, Customer_AccessCode.MaxQtyPerOrder, " & _
               "Customer_AccessCode.CovLevel , AccountStatus, AllowOrderEdit, ReportHistoryVisibility,ExclusionsAccess,ExclusionsAllowEdit,DisableAllQuantityLimits, ShowPrice, " & _
               "viewusage, viewthumbnails " & _
               "FROM Customer INNER JOIN Customer_AccessCode " & _
               "ON Customer.ID = Customer_AccessCode.CustomerID " & _
               "WHERE (Customer.Code = " & SQLString(CustomerCode) & ") " & _
               "AND (Customer.Pwd = " & SQLString(Pwd) & " OR Customer.PreviousPwd = " & SQLString(Pwd) & ") " & _
               "AND (Customer_AccessCode.Code = " & SQLString(AccessCode) & ") "

        GetReaderSQL(cnn, cmd, rcd, sSQL, sMsg)

        If (rcd Is Nothing) Then

            bSuccess = False

            Session("PMsg") = "Unable to Login"
            sMsg = "Invalid Login...  Verify Caps Lock key is not set and re-enter information."

        ElseIf (rcd Is Nothing) = False Then

            bSuccess = True
            rcd.Read()
            If rcd.HasRows Then

                If CBool(rcd("AccountStatus")) Then

                    'TODO login user

                    oMyUser.State() = STATE_LoggedIn
                    oMyUser.CustomerID() = Convert.ToInt32(rcd("CustomerID").ToString())
                    oMyUser.CustomerCode() = rcd("CustomerCode").ToString
                    oMyUser.CustomerName() = rcd("CustomerName").ToString
                    oMyUser.CustomerEmail() = rcd("CustomerEmail").ToString
                    oMyUser.CustomerPhone() = rcd("CustomerPhone").ToString
                    oMyUser.AccessCodeID = Convert.ToInt32(rcd("AccessCodeID").ToString)
                    oMyUser.AccessCode = rcd("AccessCode").ToString
                    oMyUser.AccessCodeEmail = rcd("AccessCodeEmail").ToString
                    oMyUser.WebSet = rcd("WebSettings").ToString
                    oMyUser.DisableAllQuantityLimits = rcd("DisableAllQuantityLimits")


                    oMyUser.MaxQtyPerLineItem = rcd("MaxQtyPerLineItem").ToString
                    oMyUser.MaxQtyPerOrder = rcd("MaxQtyPerOrder").ToString

                    oMyUser.CoverSheetLevels = rcd("CovLevel").ToString
                    oMyUser.CurrentUserAccountStatus = CBool(rcd("AccountStatus").ToString)
                    oMyUser.CurrentUserAllowOrderEdit = CBool(rcd("AllowOrderEdit").ToString)
                    oMyUser.CurrentUserReportHistoryVisibility = rcd("ReportHistoryVisibility").ToString
                
                    If rcd("ViewUsage") Is DBNull.Value = False Then
                        oMyUser.ViewUsage = rcd("ViewUsage")
                    End If
                    If rcd("ViewThumbnails") Is DBNull.Value = False Then
                        oMyUser.ViewThumbnails = rcd("ViewThumbnails")
                    End If
                    'Set Initial Default Logo
                    If IsNumeric(rcd("ExclusionsAccess").ToString) Then
                        oMyUser.CurrentUserExclusionsAccess = rcd("ExclusionsAccess").ToString
                    Else
                        oMyUser.CurrentUserExclusionsAccess = 0
                    End If

                    If rcd("showprice") Is DBNull.Value = False Then
                        oMyUser.ShowPrice = CBool(rcd("showprice"))
                    End If

                    oMyUser.CurrentUserExclusionsAllowEdit = rcd("ExclusionsAllowEdit").ToString
                    oMyUser.LogoFile = "D3Logo.gif"
                    'Look for Customer Logo
                    'If oLog.FileExists(Server.MapPath("./logos/" & oMyUser.CustomerCode() & ".jpg")) Then
                    'oMyUser.LogoFile = oMyUser.CustomerCode() & ".jpg"
                    If oLog.FileExists(Server.MapPath("./logos/" & oMyUser.CustomerCode() & "-" & oMyUser.AccessCode() & ".png")) Then
                        oMyUser.LogoFile = oMyUser.CustomerCode() & "-" & oMyUser.AccessCode() & ".png"
                    ElseIf oLog.FileExists(Server.MapPath("./logos/" & oMyUser.CustomerCode() & "-" & oMyUser.AccessCode() & ".gif")) Then
                        oMyUser.LogoFile = oMyUser.CustomerCode() & "-" & oMyUser.AccessCode() & ".gif"
                    ElseIf oLog.FileExists(Server.MapPath("./logos/" & oMyUser.CustomerCode() & ".png")) Then
                        oMyUser.LogoFile = oMyUser.CustomerCode() & ".png"
                    ElseIf oLog.FileExists(Server.MapPath("./logos/" & oMyUser.CustomerCode() & ".gif")) Then
                        oMyUser.LogoFile = oMyUser.CustomerCode() & ".gif"
                    ElseIf oLog.FileExists(Server.MapPath("./logos/" & oMyUser.CustomerCode() & ".jpg")) Then
                        oMyUser.LogoFile = oMyUser.CustomerCode() & ".jpg"
                    End If
                    ReleaseReaderSQL(cnn, cmd, rcd)

                    'mw - 01-31-2009
                    KitTmpCleanup(oMyUser.AccessCodeID)
                    '
                    StartNewOrder(oMyUser)
                    StartAdmin()

                    'Session("UState") = STATE_LoggedIn
                    oMyUser.State = STATE_LoggedIn
                    Session("PUser") = oMyUser
                    oMyUser = Nothing
                    oLog = Nothing
                Else
                    bSuccess = False
                    Session("PMsg") = "Unable to Login"
                    sMsg = "Invalid Login...  Your account has been deactivated"
                    ReleaseReaderSQL(cnn, cmd, rcd)

                    'mw - 01-31-2009

                End If


            Else
                Session("PMsg") = "Unable to Login"
                bSuccess = False
                sMsg = "Invalid Login...  Verify Caps Lock key is not set and re-enter information."
            End If



        End If

        Return bSuccess
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'ORDERS
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Friend Function StartNewOrder(ByVal User As paraUser) As Boolean
        Dim bSuccess As Boolean

        If (User.CustomerID > 0) And (User.AccessCodeID > 0) Then
            bSuccess = OrderSpecsPrep(User.CustomerID, User.AccessCodeID, User.CanRememberOrder, User.CanEditCustomDataDetail)
            If (bSuccess = False) Then
                Session("PMsg") = "Unable to Start New Order - Order Specification Init"
            End If
        Else
            Session("PMsg") = "Unable to Start New Order"
        End If

        Return bSuccess
    End Function

    Private Function OrderSpecsPrep(ByVal lCustID As Long, ByVal lAccID As Long, ByVal bRemPrev As Boolean, ByVal bCustomDetail As Boolean) As Long
        Dim cnn As OleDbConnection
        Dim cmd As OleDbCommand
        Dim rcd As OleDbDataReader
        Dim cnn2 As OleDbConnection
        Dim cmd2 As OleDbCommand
        Dim rcd2 As OleDbDataReader
        Dim sSQL As String
        Dim bSuccess As Boolean
        Dim oMyOrder As paraOrder = New paraOrder
        Dim lOrderID As Long

        sSQL = "SELECT Customer.ID, Customer.Code, Customer.Name, Customer.Email, " & _
               "Customer.ProcessingChg, Customer.MonthlyChg, Customer.FreightMarkupChg, Customer.LinePckChg, Customer.LineKitChg, " & _
               "Customer_AccessCode.ID, Customer_AccessCode.Code, " & _
               "Customer_AccessCode.MaxQtyPerLineItem, Customer_AccessCode.MaxQtyPerOrder, " & _
               "(MID(Customer_AccessCode.WebSet,6,1)=1) AS CanExtendQtyLI, " & _
               "(MID(Customer_AccessCode.WebSet,7,1)=1) AS CanExtendQtyOR " & _
               "FROM Customer INNER JOIN Customer_AccessCode " & _
               "ON Customer.ID = Customer_AccessCode.CustomerID " & _
               "WHERE (Customer.ID = " & lCustID & ") " & _
               "AND (Customer_AccessCode.ID = " & lAccID & ") "



        'sSQL = "SELECT Customer.ID, Customer.Code, Customer.Name, Customer.Email, " & _
        '       "Customer.ProcessingChg, Customer.MonthlyChg, Customer.FreightMarkupChg, Customer.LinePckChg, Customer.LineKitChg, " & _
        '       "Customer_AccessCode.ID, Customer_AccessCode.Code, " & _
        '       "Customer_AccessCode.MaxQtyPerLineItem, Customer_AccessCode.MaxQtyPerOrder, " & _
        '       "(Substring(Customer_AccessCode.WebSet,6,1)=1) AS CanExtendQtyLI, " & _
        '       "(Substring(Customer_AccessCode.WebSet,7,1)=1) AS CanExtendQtyOR " & _
        '       "FROM Customer INNER JOIN Customer_AccessCode " & _
        '       "ON Customer.ID = Customer_AccessCode.CustomerID " & _
        '       "WHERE (Customer.ID = " & lCustID & ") " & _
        '       "AND (Customer_AccessCode.ID = " & lAccID & ") "

        GetReader2(cnn, cmd, rcd, sSQL)
        If Not (rcd Is Nothing) Then
            rcd.Read()
            bSuccess = False
            If rcd.HasRows Then
                bSuccess = True

                With oMyOrder
                    .Init()
                    'Customer
                    .CustomerEmail = rcd("Email").ToString()
                    'Order
                    .ProcessingChg() = rcd("ProcessingChg").ToString()
                    .MonthlyChg() = rcd("MonthlyChg").ToString()
                    .FreightMarkupChg = rcd("FreightMarkupChg").ToString()
                    .LinePckChg() = rcd("LinePckChg").ToString()
                    .LineKitChg() = rcd("LineKitChg").ToString()
                    .MaxQtyPerLineItem = rcd("MaxQtyPerLineItem").ToString()
                    .MaxQtyPerOrder = rcd("MaxQtyPerOrder").ToString()
                    .CanExtendQtyLI = rcd("CanExtendQtyLI").ToString()
                    .CanExtendQtyOR = rcd("CanExtendQtyOR").ToString()
                End With

                'If (bRemPrev = True) Then
                '  sSQL = "SELECT [Order].* FROM [Order] WHERE AccessCodeID = " & lAccID & " ORDER BY [Order].ID DESC"
                '  GetReader2(cnn2, cmd2, rcd2, sSQL)
                '  If Not (rcd2 Is Nothing) Then
                '    rcd2.Read()
                '    If rcd2.HasRows Then
                '      With oMyOrder
                '        .RequestorTitle = rcd2("Requestor_Title") & ""
                '        .RequestorFirstName = rcd2("Requestor_FirstName") & ""
                '        .RequestorLastName = rcd2("Requestor_LastName") & ""
                '        .RequestorEmail = rcd2("Requestor_Email") & ""

                '        .ShipContactFirstName = rcd2("ShipTo_ContactFirstName") & ""
                '        .ShipContactLastName = rcd2("ShipTo_ContactLastName") & ""
                '        .ShipCompany = rcd2("ShipTo_Name") & ""
                '        .ShipAddress1 = rcd2("ShipTo_Address1") & ""
                '        .ShipAddress2 = rcd2("ShipTo_Address2") & ""
                '        .ShipCity = rcd2("ShipTo_City") & ""
                '        .ShipState = rcd2("ShipTo_State") & ""
                '        .ShipZip = rcd2("ShipTo_ZipCode") & ""
                '        .ShipCountry = rcd2("ShipTo_Country") & ""
                '        .ShipPrefID = rcd2("PreferredShipID") & ""
                '        .CoverSheetID = rcd2("CoverSheetID") & ""
                '        .CoverSheetText = rcd2("CS_Content") & ""
                '      End With
                '    End If
                '  End If
                '  ReleaseReader2(cnn2, cmd2, rcd2)
                'End If

            Else
                Session("PMsg") = "Unable to Login"
            End If
            ReleaseReader2(cnn, cmd, rcd)

            oMyOrder.InitCustoms(lCustID, bCustomDetail)
            oMyOrder.InitDetails(lCustID)

            'mw - 06-30-2007
            If (bRemPrev = True) Then
                '  sSQL = "SELECT MAX([Order].ID) AS OrderID FROM [Order] WHERE AccessCodeID = " & lAccID
                '  GetReader2(cnn, cmd, rcd, sSQL)
                '  If Not (rcd Is Nothing) Then
                '    rcd.Read()
                '    If rcd.HasRows Then
                '      lorderid = rcd("OrderID") & ""
                '    End If
                '  End If
                '  ReleaseReader2(cnn, cmd, rcd)
                'End If
                oMyOrder.LoadFromPrior(Me, lAccID, 0, True, True, True, True, False)
            End If
            '
            Session("POrder") = oMyOrder
            oMyOrder = Nothing
        End If

        Return bSuccess
    End Function

    Friend Function ApproveOrder(ByVal lOrderID As Long) As Boolean
        Dim cnnE As OleDb.OleDbConnection
        Dim cmdE As OleDb.OleDbCommand
        Dim sSQLE As String
        Dim bSuccess As Boolean : bSuccess = False

        Try
            If (lOrderID > 0) Then
                GetConnection(cnnE)
                If Not (cnnE Is Nothing) Then
                    sSQLE = "UPDATE [Order] SET [Order].StatusID = " & STATUS_ACTV & " WHERE [Order].ID = " & lOrderID
                    SQLExecute(cnnE, sSQLE)
                End If
                bSuccess = True
            End If
        Catch ex As Exception
        End Try
        ReleaseConnection(cnnE)
        Return bSuccess
    End Function

    Friend Function RejectOrder(ByVal lOrderID As Long) As Boolean
        Dim cnnE As OleDb.OleDbConnection
        Dim cmdE As OleDb.OleDbCommand
        Dim sSQLE As String
        Dim bSuccess As Boolean : bSuccess = False

        Try
            If (lOrderID > 0) Then
                GetConnection(cnnE)
                If Not (cnnE Is Nothing) Then
                    sSQLE = "UPDATE [Order] SET [Order].StatusID = " & STATUS_CNCL & " WHERE [Order].ID = " & lOrderID
                    SQLExecute(cnnE, sSQLE)
                End If
                bSuccess = True
            End If
        Catch ex As Exception
        End Try
        ReleaseConnection(cnnE)
        Return bSuccess
    End Function
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'ADMIN
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Friend Function StartAdmin() As Boolean
        Dim bSuccess As Boolean
        Dim oMyKitSpec As paraKit = New paraKit

        oMyKitSpec.Init()
        Session("PKit") = oMyKitSpec

        bSuccess = True

        Return bSuccess
    End Function
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Cleanup
    Friend Function KitTmpCleanup(ByVal lAccID As Long) As Boolean
        Dim cnnE As SqlConnection
        Dim cmdE As SqlCommand
        Dim sSQLE As String
        Dim bSuccess As Boolean : bSuccess = False

        Try
            If (lAccID > 0) Then
                GetConnectionSQL(cnnE)
                If Not (cnnE Is Nothing) Then
                    sSQLE = "UPDATE Customer_Kit SET Customer_Kit.Status = " & STATUS_INAC & " WHERE Customer_Kit.Status = " & PERM_TEMP & " AND Customer_Kit.AccessCodeID = " & lAccID
                    SQLExecuteSQL(cnnE, sSQLE)
                End If
                bSuccess = True
            End If
        Catch ex As Exception
        End Try
        ReleaseConnectionSQL(cnnE)

        Return bSuccess
    End Function

    'Friend Sub LogoutUser()
    '  Session("PUser") = Nothing
    '  Session("POrder") = Nothing
    '  Session("PKit") = Nothing
    'End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Inventory
    'Friend Sub GetItemDetails(ByVal lItmID As Long, ByRef iStatus As Integer, ByRef sDesc As String)
    Friend Sub GetItemDetails(ByVal lItmID As Long, ByRef iStatus As Integer, ByRef sDesc As String, ByRef sActiveDate As String, ByRef sProjInActiveDate As String, ByRef gCost As Single)
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim dr As OleDb.OleDbDataReader
        Dim sSQL As String

        sSQL = "SELECT Customer_Document.Status, Customer_Document.ReferenceNo AS RefNo, Customer_Document.Description AS RefDesc, " & _
               "FORMAT(Customer_Document.ActDate,'m/d/yyyy') AS ActiveDate, FORMAT(Customer_Document.InActPrjDate,'m/d/yyyy') AS ProjInActiveDate, " & _
               "Customer_Document_Fill.Cost " & _
               "FROM Customer_Document LEFT JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID " & _
               "WHERE (Customer_Document.ID = " & lItmID & ") "
        GetReader2(cnn, cmd, dr, sSQL)
        If Not (dr Is Nothing) Then
            dr.Read()
            If dr.HasRows Then
                iStatus = dr("Status")
                sDesc = dr("RefNo") & " - " & dr("RefDesc")
                sActiveDate = dr("ActiveDate")
                sProjInActiveDate = dr("ProjInActiveDate")
                gCost = dr("Cost")
            End If
        End If
        ReleaseReader2(cnn, cmd, dr)
    End Sub

    Friend Function GetFirstUsage(ByVal lDocID As Long) As String
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim dr As OleDb.OleDbDataReader
        Dim sSQL As String
        Dim sDate As String = String.Empty

        sSQL = "SELECT TOP 1 FORMAT([Order].RequestDate,'m/d/yyyy') AS FirstDate " & _
               "FROM [Order] INNER JOIN Order_Document ON [Order].ID = Order_Document.OrderID " & _
               "WHERE (Order_Document.DocumentID = " & lDocID & ") " & _
               "ORDER BY RequestDate"
        GetReader2(cnn, cmd, dr, sSQL)
        If Not (dr Is Nothing) Then
            dr.Read()
            If dr.HasRows Then
                sDate = dr("FirstDate")
            End If
        End If
        ReleaseReader2(cnn, cmd, dr)

        Return sDate
    End Function

    'mw - 07-20-2009
    'Friend Function GetLastUsage(ByVal lDocID As Long) As String
    '  Dim cnn As OleDb.OleDbConnection
    '  Dim cmd As OleDb.OleDbCommand
    '  Dim dr As OleDb.OleDbDataReader
    '  Dim sSQL As String
    '  Dim sDate As String = String.Empty

    '  sSQL = "SELECT TOP 1 FORMAT([Order].RequestDate,'m/d/yyyy') AS LastDate " & _
    '         "FROM [Order] INNER JOIN Order_Document ON [Order].ID = Order_Document.OrderID " & _
    '         "WHERE (Order_Document.DocumentID = " & lDocID & ") " & _
    '         "ORDER BY RequestDate DESC"
    '  GetReader2(cnn, cmd, dr, sSQL)
    '  If Not (dr Is Nothing) Then
    '    dr.Read()
    '    If dr.HasRows Then
    '      sDate = dr("LastDate")
    '    End If
    '  End If
    '  ReleaseReader2(cnn, cmd, dr)

    '  Return sDate
    'End Function
    Friend Function GetLastUsage(ByVal lDocID As Long, ByRef sOrdDate As String, ByRef sOrdQty As String) As String
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim dr As OleDb.OleDbDataReader
        Dim sSQL As String
        Dim bSuccess As Boolean = False

        Try
            sSQL = "SELECT TOP 1 FORMAT([Order].RequestDate,'m/d/yyyy') AS LastDate " & _
                   ", Order_Document.QtyOrdered AS OrdQty " & _
                   "FROM [Order] INNER JOIN Order_Document ON [Order].ID = Order_Document.OrderID " & _
                   "WHERE (Order_Document.DocumentID = " & lDocID & ") " & _
                   "ORDER BY RequestDate DESC"
            GetReader2(cnn, cmd, dr, sSQL)
            If Not (dr Is Nothing) Then
                dr.Read()
                If dr.HasRows Then
                    sOrdDate = dr("LastDate")
                    sOrdQty = Val(dr("OrdQty").ToString)
                End If
            End If
            bSuccess = True
        Catch ex As Exception
        End Try

        ReleaseReader2(cnn, cmd, dr)
        Return bSuccess
    End Function

    Friend Function GetQtyRemainWarn(ByVal lDocID As Long, ByRef lQtyOH As Long, ByRef iStockWarn As Integer) As Boolean
        Dim ds As New dsInventoryTableAdapters.InventoryDetailTableAdapter
        Dim dt As New dsInventory.InventoryDetailDataTable

        ds.Fill(dt, lDocID)
        If (dt.Rows.Count > 0) Then
            lQtyOH = dt(0).QtyOH
            iStockWarn = dt(0).StockWarn
        End If

        Return (True)
    End Function

    'mw - 10-30-2007
    Friend Function GetAvgUsage(ByVal lDocID As Long, ByRef iMonthCnt As Integer, ByRef lAvgQtyMonth As Long, ByRef lAvgQtyWeek As Long, ByRef gAvgQtyDay As Single) As Boolean
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        'Dim dr As OleDb.OleDbDataReader
        Dim sSQL As String
        Dim lQtySum As Long = 0
        Dim iQtyPerPull As Integer = 0
        Dim bSuccess As Boolean = False

        Try
            iMonthCnt = 0
            lAvgQtyMonth = 0
            lAvgQtyWeek = 0
            gAvgQtyDay = 0




            Dim ta As New dsInventoryTableAdapters.GetAvgUsageTableAdapter
            For Each dr As dsInventory.GetAvgUsageRow In ta.GetAvgUsage(lDocID).Rows
                iQtyPerPull = dr("QtyPerPull")
                lQtySum = lQtySum + dr("QtySum")
                iMonthCnt = iMonthCnt + 1
            Next



            If (iMonthCnt > 0) Then
                lAvgQtyMonth = lQtySum / iMonthCnt
            Else
                lAvgQtyMonth = 0
            End If

            If (lAvgQtyMonth > 0) Then
                lAvgQtyWeek = Math.Round((lAvgQtyMonth / 30) * 7)
                gAvgQtyDay = Math.Round(lAvgQtyMonth / 30, 1)
            Else
                lAvgQtyWeek = 0
                gAvgQtyDay = 0
            End If
            '

            'Account for Qty Per Pull
            Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
            If qa.getDocumentPrintMethod(lDocID) <> 1 Then
                lAvgQtyMonth = lAvgQtyMonth * iQtyPerPull
                lAvgQtyWeek = lAvgQtyWeek * iQtyPerPull
                gAvgQtyDay = gAvgQtyDay * iQtyPerPull
            End If
            bSuccess = True

        Catch ex As Exception

        End Try
        'Return lAvgQtyMonth
        Return bSuccess
    End Function
    'Friend Function GetAvgUsage(ByVal lDocID As Long, ByRef iMonthCnt As Integer, ByRef lAvgQtyMonth As Long, ByRef lAvgQtyWeek As Long, ByRef gAvgQtyDay As Single) As Boolean
    '    Dim cnn As OleDb.OleDbConnection
    '    Dim cmd As OleDb.OleDbCommand
    '    Dim dr As OleDb.OleDbDataReader
    '    Dim sSQL As String
    '    Dim lQtySum As Long = 0
    '    Dim iQtyPerPull As Integer = 0
    '    Dim bSuccess As Boolean = False

    '    Try
    '        iMonthCnt = 0
    '        lAvgQtyMonth = 0
    '        lAvgQtyWeek = 0
    '        gAvgQtyDay = 0


    '        sSQL = "SELECT iif(cf.QtyPerPull IS NULL,0,cf.QtyPerPull) AS QtyPerPull, SUM(od.QtyOrdered) AS QtySum, FORMAT(o.RequestDate,'mm-yyyy') AS TheMonth " & _
    '               "FROM [Order] o INNER JOIN " & _
    '               "(Order_Document od LEFT JOIN Customer_Document_Fill cf ON od.DocumentID = cf.DocumentID) " & _
    '               " ON o.ID = od.OrderID  " & _
    '               "WHERE (od.DocumentID = " & lDocID & ") AND (DateDiff('m',o.RequestDate,Now)<=6)" & _
    '               "GROUP BY FORMAT(o.RequestDate,'mm-yyyy'), cf.QtyPerPull"

    '        GetReader2(cnn, cmd, dr, sSQL)
    '        If Not (dr Is Nothing) Then
    '            Do While dr.Read()
    '                iQtyPerPull = dr("QtyPerPull")
    '                lQtySum = lQtySum + dr("QtySum")
    '                iMonthCnt = iMonthCnt + 1
    '            Loop
    '            ReleaseReader2(cnn, cmd, dr)

    '            If (iMonthCnt > 0) Then
    '                lAvgQtyMonth = lQtySum / iMonthCnt
    '            Else
    '                lAvgQtyMonth = 0
    '            End If

    '            If (lAvgQtyMonth > 0) Then
    '                lAvgQtyWeek = Math.Round((lAvgQtyMonth / 30) * 7)
    '                gAvgQtyDay = Math.Round(lAvgQtyMonth / 30, 1)
    '            Else
    '                lAvgQtyWeek = 0
    '                gAvgQtyDay = 0
    '            End If
    '            '

    '            'Account for Qty Per Pull
    '            Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
    '            If qa.getDocumentPrintMethod(lDocID) <> 1 Then
    '                lAvgQtyMonth = lAvgQtyMonth * iQtyPerPull
    '                lAvgQtyWeek = lAvgQtyWeek * iQtyPerPull
    '                gAvgQtyDay = gAvgQtyDay * iQtyPerPull
    '            End If

    '        End If
    '        bSuccess = True

    '    Catch ex As Exception

    '    End Try
    '    'Return lAvgQtyMonth
    '    Return bSuccess
    'End Function
    'Friend Function GetWeekRemain(ByVal iAvgQtyPerWeek As Integer, ByVal lQtyRemain As Long, ByRef sMsg As String) As String
    Friend Function GetWeekRemain(ByVal lAvgQtyPerWeek As Long, ByVal lQtyRemain As Long, ByRef sMsg As String) As String
        Dim sRemain As String

        sRemain = 0
        If (lQtyRemain > 0) Then
            If (lAvgQtyPerWeek > 0) Then
                sRemain = Format((lQtyRemain / lAvgQtyPerWeek), "0.0")
            Else
                sMsg = "Insufficient History"
            End If
        End If

        Return sRemain
    End Function

    'Private Function GetMonthRemain(ByVal iAvgQtyPerMonth As Integer, ByVal lQtyRemain As Long, ByVal sMsg As String) As Integer
    '  'Dim sRemain

    '  'sRemain = 0
    '  'If (sQtyRemain > 0) Then
    '  '  If (sAvgQtyPerMonth > 0) Then
    '  '    sRemain = (sQtyRemain / sAvgQtyPerMonth)
    '  '    '      If (CINT(sRemain) > 0) Then
    '  '    '        sRemain = ((sRemain / 30) * 7)
    '  '    '      End If
    '  '  Else
    '  '    sMsg = "Insufficient History"
    '  '  End If
    '  'End If

    '  'GetMonthRemain = FormatNumber(sRemain, 1)
    '  Return 0
    'End Function

    Friend Function RPGetInvnRequired(ByVal lDocID As Long, ByVal lQtyRemain As Long, ByVal iReqDays As Integer, ByVal iQtyRound As Integer, _
                                    ByRef iRemainDays As Integer, ByRef lQtyRequired As Long) As Boolean
        Dim iMonthCnt As Integer
        Dim lAvgQtyPerMonth As Long
        Dim lAvgQtyPerWeek As Long
        'Dim iAvgQtyPerDay As Integer
        Dim gAvgQtyPerday As Single
        Dim iRoundFactor As Integer

        iRemainDays = 0
        lQtyRequired = 0

        ''''''''''''''''''
        'mw - 10-30-2007
        'GetAvgUsage(lDocID, iMonthCnt, iAvgQtyPerMonth, iAvgQtyPerWeek, iAvgQtyPerDay)
        'If (lQtyRemain > 0) And (iAvgQtyPerDay > 0) Then
        '  iRemainDays = Convert.ToInt32(lQtyRemain / iAvgQtyPerDay)
        'End If

        ''(Days Requested * AvgUsedPerDay)
        'lQtyRequired = iReqDays * iAvgQtyPerDay
        'GetAvgUsage(lDocID, iMonthCnt, iAvgQtyPerMonth, iAvgQtyPerWeek, gAvgQtyPerday)
        GetAvgUsage(lDocID, iMonthCnt, lAvgQtyPerMonth, lAvgQtyPerWeek, gAvgQtyPerday)

        If (lQtyRemain > 0) And (gAvgQtyPerday > 0) Then
            iRemainDays = Convert.ToInt32(lQtyRemain / gAvgQtyPerday)
        End If

        '(Days Requested * AvgUsedPerDay)
        lQtyRequired = iReqDays * gAvgQtyPerday
        ''''''''''''''''''

        'Compare to what remains
        If (lQtyRequired > lQtyRemain) And lQtyRemain >= 0 Then
            lQtyRequired = lQtyRequired - lQtyRemain
        ElseIf lQtyRemain < 0 Then
            'If iRemainDays > 0 And gAvgQtyPerday > 0 Then
            lQtyRequired = lQtyRequired 'iReqDays / (iRemainDays * gAvgQtyPerday)
            'End If

        Else
            lQtyRequired = 0
        End If

        'mw - 10-30-2007
        ''Round UP to round specification
        'If (lQtyRequired > 0) And (iQtyRound > 0) Then
        '  lQtyRequired = (Format(lQtyRequired / iQtyRound, "0") + IIf(lQtyRequired Mod iQtyRound > 0, 1, 0)) * iQtyRound
        'End If
        'Round normal - do not need to make sure inventory covered - ab - 10-24-2007
        lQtyRequired = MRound(lQtyRequired, iQtyRound)
        '

        Return True
    End Function

    'mw - 10-30-2007 - implemented new rounding - Excel function - MROUND
    Private Function MRound(ByVal lNo As Long, ByVal iRound As Integer) As Long
        Dim bRoundUp As Boolean = False
        Dim lBaseNoPrev As Long
        Dim lBaseNoNext As Long
        Dim lRetNo As Long

        If (lNo > 0) And (iRound > 0) Then
            'Excel's MRound Rule
            'Number   is the value to round.
            'Multiple   is the multiple to which you want to round number.
            'MROUND rounds up, away from zero, if the remainder of dividing number by multiple is greater than or equal to half the value of multiple.
            bRoundUp = (lNo Mod iRound) >= (iRound / 2)
            lBaseNoPrev = Math.DivRem(lNo, iRound, 0) * iRound
            lBaseNoNext = (Math.DivRem(lNo, iRound, 0) * iRound) + iRound
            If bRoundUp Then
                lRetNo = lBaseNoNext
            Else
                lRetNo = lBaseNoPrev
            End If
            'Guess at a way to do
            'Dim lBaseNo As Long
            'lBaseNo = Format(lNo / iRound, "0") * iRound
            'lBaseNoPrev = Format(lNo / iRound, "0") * iRound
            'lBaseNoNext = Format(lNo / iRound, "0") * (iRound + 1)
            ''Closer to Base Number or Closer to Next Number
            'If (Math.Abs(lBaseNo - lBaseNoPrev) < Math.Abs(lBaseNo - lBaseNoNext)) Then
            '  lRetNo = lBaseNoPrev
            'Else
            '  lRetNo = lBaseNoNext
            'End If
        End If

        MRound = lRetNo
    End Function
    '

    Friend Function RPGetRcmdCosts(ByVal sStatus As String, ByVal lQtyReq As Long, ByVal lQtyBE As Long, _
                                  ByVal gFmtPrice As Single, ByVal gCurrPrice As Single, _
                                  ByRef sReqCostEach As String, ByRef sReqCostTotal As String, ByRef gTotalCost As String, ByVal intDocID As Integer) As Boolean

        'If (sStatus = "F/P") And (lQtyReq < lQtyBE) Then
        If (sStatus = "F/P") And (lQtyReq > 0) And (lQtyReq < lQtyBE) Then
            'sReqCostEach = gFmtPrice                   'Estimated Each Price = Format Price


            If gFmtPrice > 0 Then
                sReqCostEach = Format(gFmtPrice, "$ 0.00")  'Estimated Each Price = Format Price
                sReqCostTotal = "POD"                       'Cost to Replenish = POD
            Else



                If lQtyReq < lQtyBE Then
                    sReqCostTotal = "POD"
                Else
                    sReqCostTotal = "OFFSET"
                End If

                sReqCostEach = "N/A"
            End If


        Else
            'sReqCostEach = gCurrPrice                              'Estimated Each Price = Last Cost/Current Cost
            'sReqCostTotal = gCurrPrice * lQtyReq                   'Cost to Replenish = Last Cost/Current Cost * lQtyReq
            If gCurrPrice > 0 Then
                sReqCostEach = Format(gCurrPrice, "$ 0.00")             'Estimated Each Price = Last Cost/Current Cost
            Else

                'sReqCostEach = "N/A"

                If lQtyReq < lQtyBE Then
                    sReqCostTotal = "POD"
                ElseIf gCurrPrice <= 0 And sStatus <> "FUL" Then
                    'TODO fix offset
                    sReqCostTotal = "OFFSET"
                Else
                    gCurrPrice = CSng(sReqCostEach)
                End If
            End If


            If gCurrPrice > 0 Then
                sReqCostTotal = Format(gCurrPrice * lQtyReq, "$ 0.00")  'Cost to Replenish = Last Cost/Current Cost * lQtyReq
            End If

            gTotalCost = gTotalCost + (gCurrPrice * lQtyReq)
        End If

        Return True
    End Function
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    ''''''''''''''
    'Misc
    Friend Function GetItmDetail(ByVal lCustomerID As Long, ByVal lItmID As Long, _
                              ByRef sItmRef As String, ByRef sItmDes As String, Optional ByRef sItmFile As String = "" _
                              , Optional ByRef strExtendedDescription As String = "") As Boolean
        Dim cnn As SqlConnection
        Dim cmd As SqlCommand
        Dim dr As SqlDataReader
        Dim bSuccess As Boolean = False
        Dim sSQL As String

        Try
            sSQL = "SELECT d.ReferenceNo AS RefNo, d.Description AS RefName, d.FileName, d.ExtendedDescription as ExtendedDescription " & _
                   "FROM Customer_Document d " & _
                   "WHERE CustomerID = " & lCustomerID & _
                   " AND (d.ID = " & lItmID & ") "
            GetReaderSQL(cnn, cmd, dr, sSQL)
            If dr.Read() Then
                sItmRef = dr("RefNo").ToString()
                sItmDes = dr("RefName").ToString()
                sItmFile = dr("FileName").ToString()

                If dr("ExtendedDescription") Is DBNull.Value = False Then
                    strExtendedDescription = dr("ExtendedDescription").ToString()
                End If

            End If
            ReleaseReaderSQL(cnn, cmd, dr)
            bSuccess = True

        Catch ex As Exception

        End Try

        Return bSuccess
    End Function

    

    Friend Function GetKitDetail(ByVal lCustomerID As Long, ByVal lKitID As Long, _
                              ByRef sKitRef As String, ByRef sKitDes As String _
                              ) As Boolean
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim dr As OleDb.OleDbDataReader
        Dim bSuccess As Boolean = False
        Dim sSQL As String

        Try
            sSQL = "SELECT k.ReferenceNo AS RefNo, k.Description AS RefName " & _
                   "FROM Customer_Kit k LEFT JOIN Customer_AccessCode ca ON k.AccessCodeID = ca.ID " & _
                   "WHERE CustomerID = " & lCustomerID & _
                   " AND (k.ID = " & lKitID & ") "
            GetReader2(cnn, cmd, dr, sSQL)
            If dr.Read() Then
                sKitRef = dr("RefNo").ToString()
                sKitDes = dr("RefName").ToString()
            End If
            ReleaseReader2(cnn, cmd, dr)
            bSuccess = True

        Catch ex As Exception

        End Try

        Return bSuccess
    End Function
    ''''''''''''''



    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'OLD NOTES - PROBABLY DON"T NEED AS OF 10-29-2006
    'CART NOTES
    'Friend Sub DocCartAdd(ByVal s As Object, ByVal e As EventArgs)

    '  objDT = Session("Cart")
    '  Dim Product = ddlProducts.SelectedItem.Text

    '  objDR = objDT.NewRow
    '  objDR("Quantity") = txtQuantity.Text
    '  objDR("Product") = ddlProducts.SelectedItem.Text
    '  objDR("Cost") = Decimal.Parse(ddlProducts.SelectedItem.Value)
    '  objDT.Rows.Add(objDR)
    '  Session("Cart") = objDT

    '  dg.DataSource = objDT
    '  dg.DataBind()

    'End Sub

    'Friend Sub DocCartDelete(ByVal s As Object, ByVal e As DataGridCommandEventArgs)

    '  objDT = Session("Cart")
    '  objDT.Rows(e.Item.ItemIndex).Delete()
    '  Session("Cart") = objDT

    '  dg.DataSource = objDT
    '  dg.DataBind()

    '  lblTotal.Text = "$" & GetItemTotal()

    'End Sub


    'Friend Sub KitCartAdd(ByVal s As Object, ByVal e As EventArgs)

    '  objDT = Session("Cart")
    '  Dim Product = ddlProducts.SelectedItem.Text

    '  objDR = objDT.NewRow
    '  objDR("Quantity") = txtQuantity.Text
    '  objDR("Product") = ddlProducts.SelectedItem.Text
    '  objDR("Cost") = Decimal.Parse(ddlProducts.SelectedItem.Value)
    '  objDT.Rows.Add(objDR)
    '  Session("Cart") = objDT

    '  dg.DataSource = objDT
    '  dg.DataBind()

    'End Sub

    'Friend Sub KitCartDelete(ByVal s As Object, ByVal e As DataGridCommandEventArgs)

    '  objDT = Session("Cart")
    '  objDT.Rows(e.Item.ItemIndex).Delete()
    '  Session("Cart") = objDT

    '  dg.DataSource = objDT
    '  dg.DataBind()

    '  lblTotal.Text = "$" & GetItemTotal()

    'End Sub
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    Private Sub LogError(ByVal sMsg As String, Optional ByVal sFile As String = "")
        Dim moLog As Log
        Dim sLogFile As String = String.Empty

        Try
            If (sFile.Length > 0) Then
                sLogFile = sFile
            Else
                sLogFile = ConfigurationSettings.AppSettings("LogFolder") & "DataErr.log"
            End If
            moLog = New Log
            moLog.Entry(sLogFile, "      " & Format(Now, "MM-dd-yyyy hh:mm:ss") & " **ERROR** ORDER:  " & sMsg)

        Catch ex As Exception

        End Try
        moLog = Nothing
    End Sub



    Public Function CleanUp(ByVal oMyUser As paraUser, ByVal oCurrentOrder As paraOrder)

        KitTmpCleanup(oMyUser.AccessCodeID)

        StartNewOrder(oMyUser)
        StartAdmin()
        oCurrentOrder.EditOrder = False
        oCurrentOrder.OrderID = 0
        'Session("UState") = STATE_LoggedIn
        oMyUser.State = STATE_LoggedIn
        Session("PUser") = oMyUser
    End Function

    Function GetSupply(ByVal lCurrQty As Integer, ByVal intQtyUsage As Integer, ByVal intRequests As Integer, ByVal intMeaningfulWeeks As Integer, ByVal intMeaningfulOrderQty As Integer, ByVal sStatus As String, ByVal intStockWarningPieces As Integer, ByVal intHorizonDays As Integer) As String
        Dim strCurrDays As String = ""
        Dim sTest As Decimal = (2 * intQtyUsage / intMeaningfulWeeks * intHorizonDays / 7)

        If CInt(lCurrQty) >= 0 Then
            'FUL


            'If intStockWarningPieces >= 0 And intStockWarningPieces < lCurrQty And intQtyUsage = 0 And intRequests < intMeaningfulOrderQty And sStatus = "FUL" And lCurrQty > sTest Then
            '    Dr.Item("iCurrDays") = "Sufficient Supply"
            'Else

            If intQtyUsage > 0 And intRequests < intMeaningfulOrderQty And lCurrQty >= sTest Then
                strCurrDays = "Sufficient Supply"
            ElseIf (sStatus = "FUL") And intRequests < intMeaningfulOrderQty And intQtyUsage > 0 And lCurrQty = 0 Then
                strCurrDays = "Needs Urgent Evaluation"
            ElseIf (sStatus <> "FUL") And intRequests < intMeaningfulOrderQty And intQtyUsage > 0 And lCurrQty = 0 Then
                strCurrDays = "Needs Evaluation"
            ElseIf (sStatus = "FUL" Or sStatus = "F/P") And (intRequests < intMeaningfulOrderQty Or intQtyUsage = 0) Then 'And lCurrQty = 0
                strCurrDays = "Needs Evaluation"
            ElseIf intQtyUsage > 0 And intRequests < intMeaningfulOrderQty And lCurrQty < sTest Then
                strCurrDays = "Needs Evaluation"
            ElseIf intQtyUsage > 0 And intRequests >= intMeaningfulOrderQty Then
                strCurrDays = Math.Round(lCurrQty / (intQtyUsage / intMeaningfulWeeks), 1)
            ElseIf intQtyUsage = 0 And intRequests = 0 Then
                strCurrDays = "No History"

            Else
                strCurrDays = "?"
            End If
        Else
            'POD

            strCurrDays = "N/A"
        End If


        If intStockWarningPieces >= 0 And intStockWarningPieces < lCurrQty And intQtyUsage = 0 And intRequests < intMeaningfulOrderQty And (sStatus = "FUL" Or sStatus = "F/P") And lCurrQty > sTest Then
            strCurrDays = "Sufficient Supply"
        End If

        Return strCurrDays
    End Function

    Function getShowStock(intItemID As Integer, blnShowStock As Boolean) As String
        Dim strTmp As String = ""
        If blnShowStock Then
            Dim ta As New dsInventoryTableAdapters.DataTable2TableAdapter
            Dim dr As dsInventory.DataTable2Row

            Try
                dr = ta.GetByDocID(intItemID).Rows(0)
                '1 - POD
                Select Case dr.PrintMethod
                    Case 1
                        Return "POD"
                    Case 0 'FUL
                        If dr.IsQtyOHNull = False AndAlso dr.QtyOH < 0 Then
                            Return "(" & Math.Abs(dr.QtyOH) & ")"
                        ElseIf dr.IsQtyOHNull = False Then
                            Return dr.QtyOH
                        Else
                            Return 0
                        End If
                    Case 2 'F/P
                        If dr.IsQtyOHNull = False AndAlso dr.QtyOH <= 0 Then
                            Return "POD"
                        ElseIf dr.IsQtyOHNull = False AndAlso dr.QtyOH > 0 Then
                            Return dr.QtyOH
                        Else
                            Return "POD"
                        End If
                    Case 3
                        Return "DNLD"
                End Select
            Catch ex As Exception
                Return "error : " & intItemID & " " & Err.Description
            End Try

        End If
        Return strTmp
    End Function

    Function GetKitStock(blnShowStock As Boolean, intKitID As Integer) As String
        Dim strTmp As String = "POD"
        Dim intLowestStock As Integer = -1
        Dim blnBackOrder As Boolean = False
        If blnShowStock Then
            Dim pd As New paraData
            Dim ta As New dsCustomerDocumentTableAdapters.Customer_Kit_DocumentTableAdapter
            Dim dt As New dsCustomerDocument.Customer_Kit_DocumentDataTable
            ta.Fill(dt, intKitID)

            If dt.Rows.Count > 0 Then
                For Each dr As dsCustomerDocument.Customer_Kit_DocumentRow In dt.Rows
                    strTmp = pd.getShowStock(dr.DocumentID, True)
                    If (IsNumeric(strTmp) AndAlso intLowestStock > Int(strTmp)) Or (IsNumeric(strTmp) And intLowestStock < 0) Then
                        intLowestStock = strTmp
                        If intLowestStock <= 0 Then
                            blnBackOrder = True
                        End If
                    End If
                Next
            Else
                strTmp = "-"
            End If

        End If
        If intLowestStock >= 0 Then
            Return intLowestStock
        ElseIf intLowestStock < 0 And blnBackOrder Then
            Return ""
        Else
            Return strTmp
        End If
    End Function

    Function GetKitContents(intKitID As Integer, strCssClass As String, blnShowStock As Boolean, Optional blnHighlightBackorder As Boolean = False) As String
        Dim pd As New paraData
        Dim ta As New dsCustomerDocumentTableAdapters.Customer_kit_with_documentsTableAdapter
        Dim dt As New dsCustomerDocument.Customer_kit_with_documentsDataTable
        Dim o As New paraData
        Dim strStock As String = ""
        Dim strBackOrderStyle As String = ""
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
        Dim intStatus As Integer = 0

        Dim strTmp As String = "<Table class='" & strCssClass & "' style='font-size:x-small;width:100%;'><tr><th colspan=10 align=left>Kit Contents:</th></tr>" & _
            "<tr><th>Seq</th><th>Qty</th><th align=left>Description</th><th>Stock</th></tr>"

        If blnShowStock = False Then
            strTmp = strTmp.Replace("Stock", "")
        End If
        ta.Fill(dt, intKitID)

        If dt.Rows.Count > 0 Then
            For Each dr As dsCustomerDocument.Customer_kit_with_documentsRow In dt.Rows

                If blnHighlightBackorder Then
                    strStock = o.getShowStock(dr.DocumentID, True)
                ElseIf blnShowStock Then
                    strStock = o.getShowStock(dr.DocumentID, blnShowStock)
                Else
                    strStock = ""
                End If

                If blnShowStock = False Then
                    strStock = ""
                End If
                intStatus = qa.GetDocumentStatusIDByDocumentID(dr.DocumentID)
                '2 backorder


                If (IsNumeric(strStock) AndAlso Int(strStock) <= 0) Or intStatus = 2 Then
                    strBackOrderStyle = "style='color: rgb(189, 83, 4);'"
                Else
                    strBackOrderStyle = ""
                End If

                strTmp += "<TR " & strBackOrderStyle & "><td>" & dr.Seq & "</td><td>" & dr.Qty & "</td><td>" & dr.ReferenceNo & " : " & dr.Description & "</td>" & _
                    "<td>" & strStock & "</td>" & _
                    "</tr>"
            Next
        Else
            strTmp = "-"
        End If

        Return strTmp & "</table>"
    End Function
End Class
