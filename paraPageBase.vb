
Imports System
Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Diagnostics

Public Class paraPageBase
    Inherits System.Web.UI.Page

    Protected Const COL_BACK As String = "#ffffff"    'Back Color / Main Menu Buttons
    Protected Const COL_LOGBAR As String = "#ffffff"  'Login Bar
    Protected Const COL_RBLUG As String = "#005DAA"   'Grid Heading Color
    Protected Const COL_MENU As String = "#f2efea"    'Menu Color
    Protected Const COL_BUTTON As String = "#4dbaed"  'Log Buttons / Selected Buttons
    Protected Const COL_DGRAY As String = "#696969"   'Button Border / Text
    Protected Const COL_BORD As String = "#ffffff"    'Bottom Border

    Protected Const WP_MENU As String = "" '"images/wpMenu.png"    'Menu Background

    Protected Const SEC_Req As Int16 = 1
    Protected Const SEC_Shp As Int16 = 2
    Protected Const SEC_Itm As Int16 = 3
    Protected Const SEC_Kit As Int16 = 4
    Protected Const SEC_Cst As Int16 = 5
    Protected Const SEC_Adm As Int16 = 6
    Protected Const SEC_AInv As Int16 = 7
    Protected Const SEC_AItm As Int16 = 8
    Protected Const SEC_AKit As Int16 = 9
    Protected Const SEC_ARsp As Int16 = 10
    Protected Const SEC_AUse As Int16 = 11
    Protected Const SEC_Crt As Int16 = 12
    Protected Const SEC_Hlp As Int16 = 13

    Protected Const PAG_Main As Int16 = 1
    Protected Const PAG_Login As Int16 = 2

    Protected Const PAG_OrdList As Int16 = 10
    Protected Const PAG_OrdPreview As Int16 = 11
    'Protected Const PAG_OrdVerify As Int16 = 11
    Protected Const PAG_OrdRequest As Int16 = 12
    Protected Const PAG_OrdCustom As Int16 = 13
    Protected Const PAG_OrdShip As Int16 = 14
    Protected Const PAG_OrdItems As Int16 = 15
    Protected Const PAG_OrdItemSelect As Int16 = 16
    Protected Const PAG_OrdKits As Int16 = 17
    Protected Const PAG_OrdKitSelect As Int16 = 18
    Protected Const PAG_OrdSubmit As Int16 = 19
    Protected Const PAG_OrdConfirm As Int16 = 20
    Protected Const PAG_OrdReqSearch As Int16 = 21
    Protected Const PAG_OrdShpSearch As Int16 = 22
    Protected Const PAG_OrdCart As Int16 = 23

    Protected Const PAG_AdmMenu As Int16 = 30
    Protected Const PAG_AdmKitList As Int16 = 31
    Protected Const PAG_AdmKitEdit As Int16 = 32
    Protected Const PAG_AdmKitItemSelect As Int16 = 33
    Protected Const PAG_AdmInvnList As Int16 = 35
    Protected Const PAG_AdmInvnEdit As Int16 = 36
    Protected Const PAG_AdmInvnPreview As Int16 = 37
    Protected Const PAG_AdmReview As Int16 = 38
    Protected Const PAG_AdmUsage As Int16 = 39
    Protected Const PAG_AdmUsageDetail As Int16 = 39
    Protected Const PAG_AdmUsagePreview As Int16 = 51
    Protected Const PAG_AdmRestockPlan As Int16 = 41
    Protected Const PAG_AdmRestockPlanPreview As Int16 = 42
    Protected Const PAG_AdmItemSelect As Int16 = 43
    Protected Const PAG_AdmItemEdit As Int16 = 44

    Protected Const PAG_ViewKit As Int16 = 50

    Protected Const PAG_ReviewItem As Int16 = 45

    Protected Const PAG_NotLoggedIn As Int16 = -2
    Protected Const PAG_Error As Int16 = -1
    Protected Const PAG_ErrorSpecified As Int16 = 0

    Private miSection As Integer = 0


    Private pPageTitle As String
    Private pPageMessage As String = String.Empty
    Private pPageDescription As String = String.Empty

    Private pPageKeywords As String = String.Empty

    Private pCurrentUser As paraUser = New paraUser
    Private pCurrentOrder As paraOrder = New paraOrder
    Private pCurrentKit As paraKit = New paraKit

    Friend MyData As paraData = New paraData

    Friend Property PageTitle() As String
        Get
            Return pPageTitle
        End Get
        Set(ByVal Value As String)
            pPageTitle = Value
        End Set
    End Property

    Friend Property PageMessage() As String
        Get
            Return pPageMessage.ToString()
        End Get
        Set(ByVal Value As String)
            pPageMessage = Value.ToString()
            Session("PMsg") = Value.ToString()
        End Set
    End Property

    Friend Property PageDescription() As String
        Get
            Return pPageDescription
        End Get
        Set(ByVal Value As String)
            pPageDescription = Value
        End Set
    End Property

    Friend ReadOnly Property CurrentUser() As paraUser
        Get
            Return pCurrentUser
        End Get
    End Property

    Friend ReadOnly Property CurrentOrder() As paraOrder
        Get
            Return pCurrentOrder
        End Get
    End Property

    Friend ReadOnly Property CurrentKit() As paraKit
        Get
            Return pCurrentKit
        End Get
    End Property

    Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            SessionReadBase(LCase(sender.ToString))
        Catch ex As Exception
            LogError("PAGE_INIT : " & LCase(sender.ToString) & " : " & ex.Message)
        End Try

        If (CurrentUser.State = MyData.STATE_LoggedOut) Then

            SessionInit(sender.ToString)
            If sender.ToString.Contains("login_aspx") = False Then
                Response.Redirect("login.aspx")
            End If


        ElseIf (CurrentUser.State >= MyData.STATE_LoggedIn) Then
            SessionReadDetail(sender.ToString)
        End If

        SetCurrentSender(LCase(sender.ToString))
    End Sub

    Private Sub SetCurrentSender(ByVal sSender As String)
        If (InStr(sSender, "ordreq") > 0) Then
            miSection = SEC_Req
        ElseIf (InStr(sSender, "ordsreq") > 0) Then
            miSection = SEC_Req
        ElseIf (InStr(sSender, "ordisel") > 0) Then
            miSection = SEC_Itm
        ElseIf (InStr(sSender, "itmsel") > 0) Then
            miSection = SEC_Itm
        ElseIf (InStr(sSender, "orditm") > 0) Then
            miSection = SEC_Itm
        ElseIf (InStr(sSender, "ordksel") > 0) Then
            miSection = SEC_Kit
        ElseIf (InStr(sSender, "kitsel") > 0) Then
            miSection = SEC_Kit
        ElseIf (InStr(sSender, "ordkit") > 0) Then
            miSection = SEC_Kit
        ElseIf (InStr(sSender, "ordshp") > 0) Then
            miSection = SEC_Shp
        ElseIf (InStr(sSender, "ordsshp") > 0) Then
            miSection = SEC_Shp
        ElseIf (InStr(sSender, "ordcst") > 0) Then
            miSection = SEC_Cst
        ElseIf (InStr(sSender, "ordcrt") > 0) Then
            miSection = SEC_Crt
        ElseIf (InStr(sSender, "adminv") > 0) Then
            miSection = SEC_AInv
        ElseIf (InStr(sSender, "admkit") > 0) Then
            miSection = SEC_AKit
        ElseIf (InStr(sSender, "admuse") > 0) Then
            miSection = SEC_AUse
        ElseIf (InStr(sSender, "admrsp") > 0) Then
            miSection = SEC_ARsp
        ElseIf (InStr(sSender, "admisel") > 0) Or (InStr(sSender, "admitm") > 0) Then
            miSection = SEC_AItm
        ElseIf (InStr(sSender, "adm") > 0) Then
            miSection = SEC_Adm
        End If
    End Sub

    Private Function GetMenuClass(ByVal iSender As Integer) As String
        Dim sClass As String = "menuLink"

        If miSection = iSender Then
            sClass = "menuLinkSelected"
        ElseIf iSender = SEC_Adm Then
            If (miSection = SEC_AInv) Or (miSection = SEC_AKit) Or (miSection = SEC_AUse) Or (miSection = SEC_ARsp) Or (miSection = SEC_AItm) Then
                sClass = "menuLinkSelected"
            End If
        End If

        Return sClass
    End Function

    Private Function GetMenuImage(ByVal iSender As Integer, ByVal sFile As String) As String
        Dim sImgFile As String = String.Empty

        'Base Image
        sImgFile = LCase(sFile & ".png")

        'Inactive Image
        sImgFile = Replace(sImgFile, ".png", "i.png")

        'Active Image
        If miSection = iSender Then
            sImgFile = Replace(sImgFile, "i.png", "a.png")
        ElseIf iSender = SEC_Adm Then
            If (miSection = SEC_AInv) Or (miSection = SEC_AKit) Or (miSection = SEC_AUse) Or (miSection = SEC_ARsp) Or (miSection = SEC_AItm) Then
                sImgFile = Replace(sImgFile, "i.png", "a.png")
            End If
        End If

        Return sImgFile
    End Function

    Private Function GetSubMenuClass(ByVal iSender As Integer) As String
        Dim sClass As String = "menuSubLink"

        If miSection = iSender Then sClass = "menuSubLinkSelected"

        Return sClass
    End Function

    Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            SessionReadBase(sender.ToString)
        Catch ex As Exception
            LogError("PAGE_LOAD : " & LCase(sender.ToString) & " : " & ex.Message)
        End Try

        If (CurrentUser.State = MyData.STATE_LoggedOut) Then
            SessionInit(sender.ToString)
        ElseIf (CurrentUser.State >= MyData.STATE_LoggedIn) Then
            SessionReadDetail(sender.ToString)
        End If

        SetCurrentSender(LCase(sender.ToString))


    End Sub

    Private Sub SessionReadBase(Optional ByVal sSender As String = "?")
        Dim sMsg As String = String.Empty

        Try
            pPageTitle = Session("PTitle").ToString()
            pPageMessage = Session("PMsg").ToString()
            pPageDescription = Session("PDescr").ToString()
            pCurrentUser = CType(Session("PUser"), paraUser)

            LogActivity("SessionReadBase   >> " & "Loaded Session Base - Sender " & sSender & "(" & Session.SessionID & ")")
        Catch ex As Exception
            sMsg = ex.Message.ToString
            LogError("SessionReadBase >> " & ex.Message.ToString)
        End Try
    End Sub

    Private Sub SessionInit(Optional ByVal sSender As String = "?")
        Dim sMsg As String = String.Empty

        Try
            With HttpContext.Current
                Session("PTitle") = String.Empty
                Session("PMsg") = String.Empty
                Session("PDescr") = String.Empty

                Session("PUser") = pCurrentUser
                Session("POrder") = pCurrentOrder
                Session("PKit") = pCurrentKit
                Session("ReviewResponse") = 0

                LogActivity("SessionInit       >> " & "Initialized Session - Sender " & sSender & "(" & Session.SessionID & ")")
            End With
        Catch ex As Exception
            sMsg = ex.Message.ToString
            LogError("SessionInit >> " & ex.Message.ToString)
        End Try
    End Sub
    Private Sub SessionReadDetail(Optional ByVal sSender As String = "?")
        Dim sMsg As String = String.Empty

        Try
            With HttpContext.Current
                pCurrentOrder = CType(Session("POrder"), paraOrder)
                pCurrentKit = CType(Session("PKit"), paraKit)

                LogActivity("SessionReadDetail >> " & "Loaded Session Details - Sender " & sSender & "(" & Session.SessionID & ")")
            End With
        Catch ex As Exception
            sMsg = ex.Message.ToString
            LogError("SessionReadDetail >> " & ex.Message.ToString)
        End Try
    End Sub
    Friend Sub SessionStore(Optional ByVal sSender As String = "?")
        Dim sMsg As String = String.Empty

        Try
            With HttpContext.Current
                LogActivity("SessionStore      >> " & "Stored Session Start  - Sender " & sSender & "(" & Session.SessionID & ")")

                Session("PUser") = pCurrentUser
                pCurrentOrder.ResetOrderNo()
                Session("POrder") = pCurrentOrder
                Session("PKit") = pCurrentKit

                LogActivity("SessionStore      >> " & "Stored Session Finish - Sender " & sSender & "(" & Session.SessionID & ")")
            End With
        Catch ex As Exception
            sMsg = ex.Message.ToString
            LogError("SessionStore >> " & ex.Message.ToString)
        End Try
    End Sub

    Friend Sub SessionClear(Optional ByVal sSender As String = "?")
        Dim sMsg As String = String.Empty

        Try
            With System.Web.HttpContext.Current
                MyData.KitTmpCleanup(pCurrentUser.AccessCodeID)
                Session.Remove("PUser")
                Session.Remove("POrder")
                Session.Remove("PKit")

                LogActivity("SessionClear      >> " & "Cleared Session (" & Session.SessionID & ") - " & Format(Now, "MM-dd-yyyy hh:mm:ss"))
            End With
            pCurrentUser.Init()
            pCurrentOrder.Init()
            pCurrentKit.Init()
            SessionInit(sSender)
        Catch ex As Exception
            sMsg = ex.Message.ToString
            LogError("SessionClear >> " & ex.Message.ToString)
        End Try

        Session.Clear()
        Session.RemoveAll()

        Session.Abandon()
        Response.Cookies("Filter").Values.Clear()
        Response.Cookies("Filter").Expires = DateTime.Now.AddDays(-1)
        Dim ckFilter As New HttpCookie("Filter")

        ckFilter.Expires = DateTime.Now.AddDays(-1)

        Response.Cookies.Add(ckFilter)

        Response.Cookies.Add(New HttpCookie("ASP.NET_SessionId", ""))
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        Dim bFrame As Boolean = False
        Dim bPopUp As Boolean = False

        bFrame = PageIsFrame(Page.ToString)
        bPopUp = PageIsPopUp(Page.ToString)
        If bFrame Then writer.Write(GetBasicTopSection(False))
        If bPopUp Then writer.Write(GetBasicTopSection(True))
        If (Not (bFrame Or bPopUp)) Then writer.Write(GetTopSection())
        MyBase.Render(writer)
        If (Not (bFrame Or bPopUp)) Then writer.Write(GetBottomSection())
    End Sub

    Private Function PageIsPopUp(ByVal sPage As String) As Boolean
        If (InStr(LCase(sPage), "ordsvep") > 0) Then
            Return True
        ElseIf (InStr(LCase(sPage), "adminvp") > 0) Then
            Return True
        ElseIf (InStr(LCase(sPage), "admusep") > 0) Then
            Return True
        ElseIf (InStr(LCase(sPage), "admrspp") > 0) Then
            Return True
        ElseIf ViewState("print") = 1 Then
            Return True
        Else
            Return False
        End If

    
    End Function

    Private Function PageIsFrame(ByVal sPage As String) As Boolean
        If (InStr(LCase(sPage), "itmvw") > 0) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function PageIsLogin(ByVal sPage As String) As Boolean
        If (InStr(LCase(sPage), "login") > 0) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function GetBasicTopSection(Optional ByVal bPrintOnly As Boolean = True) As String
        Dim s As StringBuilder = New StringBuilder(200)
        s.Append("<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">")
        s.Append("<html>")
        s.Append("<head>")
        s.Append("<META HTTP-EQUIV='Expires' CONTENT='-1'>")
        s.Append("<META HTTP-EQUIV='Pragma' CONTENT='no-store'>")
        s.Append("<META HTTP-EQUIV='CACH-CONTROL' CONTENT='NO-STORE'>")
        s.Append("<meta name='description' content='" + pPageDescription + "'>")
        s.Append("<meta name='keywords' content='" + pPageKeywords + "'>")
        s.Append("<title>" + pPageTitle + "</title>")
        If (bPrintOnly) Then
            s.Append("<link rel='stylesheet' type='text/css' href='" + "paraStylesP.css" + "'>")
        Else
            s.Append("<link rel='stylesheet' type='text/css' href='" + "paraStyles.css" + "'>")
        End If

        s.Append("</head>")
        s.Append("<table bgcolor='Transparent' border=0 cellspacing=0 cellpadding=0 id='tblMain'><tr>")

        Return s.ToString()
    End Function

    Private Function GetTopSection() As String
        Dim tString As String = ""

        tString = GetHTMLHeader()
        tString += GetSiteBorderOpen()
        tString += GetPageOpenSection()

        tString += GetMenu()
        tString += GetLoginSection()
        'tString += "<form/>" 'Finish off Header Section so as not to get mixed in pages brought up
        Return (tString)
    End Function

    Private Function GetBottomSection() As String
        Dim tString As String = ""

        tString = GetPageCloseSection()
        tString += GetSiteBorderClose()
        tString += GetFooter()

        Return (tString)
    End Function

    Protected Function GetHTMLHeader() As String
        Dim s As StringBuilder = New StringBuilder(200)

        s.Append("<html>")
        s.Append("<head>")
        s.Append("<META HTTP-EQUIV='Expires' CONTENT='-1'>")
        s.Append("<META HTTP-EQUIV='Pragma' CONTENT='no-store'>")
        s.Append("<META HTTP-EQUIV='CACH-CONTROL' CONTENT='NO-STORE'>")
        s.Append("<meta name='description' content='" + pPageDescription + "'>")
        s.Append("<meta name='keywords' content='" + pPageKeywords + "'>")
        s.Append("<title>" + pPageTitle + "</title>")
        s.Append("<link rel='stylesheet' type='text/css' href='" + "paraStyles.css" + "'>")
        s.Append("<META NAME='ROBOTS' CONTENT='NOINDEX,NOFOLLOW'>")
        s.Append("</head><body>")

        Return s.ToString()
    End Function

    Private Function GetSiteBorderOpen() As String
        Dim s As StringBuilder = New StringBuilder(200)
        s.Append("<center>")
        s.Append("<table class='pageBody' bgcolor='" & COL_BACK & "' border=0 cellspacing=0 cellpadding=0 ><tr>")                          'Start for Site Table
        s.Append("<td>")                                            'Start for Main Section

        Return s.ToString()
    End Function

    Private Function GetLogoHeader() As String
        Dim s As StringBuilder = New StringBuilder(200)

        'Option 1
        If (CurrentUser.State = MyData.STATE_LoggedOut) Then
            s.Append("<table width='100%' bgcolor='" & COL_BACK & "' border=0 cellspacing=0 cellpadding=0>")
            s.Append("<tr>")
            s.Append("<td align='center' colSpan='100%'><IMG src='logos/D3Logo.gif' border='0 style='BACKGROUND-COLOR:transparent;'></td>")
            s.Append("</tr>")
            s.Append("</table>")
        ElseIf (CurrentUser.State >= MyData.STATE_LoggedIn) Then
            s.Append("<table width='100%'>")
            s.Append("<tr>")
            s.Append("<td align='center' colSpan='100%' style='background-image: url(""./logos/" & Server.HtmlEncode(CurrentUser.LogoFile) & """);' class='logoheader'>" & _
                     "&nbsp</td>")
            'background-image: url('./logos/GE Energy1.jpg');
            '"><IMG src='./logos/" & CurrentUser.LogoFile & "' border='0 style='BACKGROUND-COLOR:transparent;'></td>")
            s.Append("</tr>")
            s.Append("</table>")
        End If

        Return s.ToString()
    End Function

    Private Function GetPageHeader() As String
        Dim s As StringBuilder = New StringBuilder(200)

        Return s.ToString()
    End Function

    Private Function GetMenu() As String
        Dim sLinks As String = String.Empty
        Dim s As StringBuilder = New StringBuilder(200)

        s.Append("<td background='" & WP_MENU & "' vAlign='top' align='left' class='menutd'>")
        s.Append("<table align='center' bgcolor='Transparent' border=0 cellspacing=0 cellpadding=0>")

        If (CurrentUser.State = MyData.STATE_LoggedOut) Then
            s.Append("<tr><td></td></tr>")
        ElseIf (CurrentUser.State >= MyData.STATE_LoggedIn) Then
            s.Append("<tr height='10'><td></td><td></td></tr>")

            Dim bShoppingCartLinks As Boolean = False

            If (pCurrentOrder.EditOrder) Then
                If (CurrentUser.CurrentUserAllowOrderEdit) And (pCurrentOrder.StatusID = 6) Then
                    bShoppingCartLinks = True
                End If
            Else
                bShoppingCartLinks = True
            End If

            If (bShoppingCartLinks) Then
                s.Append("<tr><td>&nbsp;</td><td valign='center'><right><A class='" & GetMenuClass(SEC_Itm) & "' href='./ItmSel.aspx?C=O'>Item Catalog</A></td>")
                s.Append("<td><A class='menuImgLink' href='./ItmSel.aspx?C=O'><img src='" & GetMenuImage(SEC_Itm, "images/mnuItm") & "' alt='Items' align='left' border='0'/></A></td></tr>")
                s.Append("<tr height='5'><td></td></tr>")
            End If

            If (CurrentUser.CanViewKits = True) And (bShoppingCartLinks) Then
                s.Append("<tr><td>&nbsp;</td><td valign='center'><right><A class='" & GetMenuClass(SEC_Kit) & "' href='./KitSel.aspx?C=O'>Kit Catalog</A></td>")
                s.Append("<td><A class='menuImgLink' href='./KitSel.aspx?C=O'><img src='" & GetMenuImage(SEC_Kit, "images/mnuKit") & "' alt='Kits' align='left' border='0'/></A></td></tr>")
                s.Append("<tr height='5'><td></td></tr>")
            End If

            If (bShoppingCartLinks) Then
                s.Append("<tr><td>&nbsp;</td><td valign='center'><right><A class='" & GetMenuClass(SEC_Crt) & "' href='./OrdCrt.aspx'>Shopping Cart</A></td>")
                s.Append("<td><A class='menuImgLink' href='./OrdCrt.aspx'><img src='" & GetMenuImage(SEC_Crt, "images/mnuCrt") & "' alt='Cart' align='left' border='0'/></A></td></tr>")
                s.Append("<tr height='5'><td></td></tr>")
            End If

            If (bShoppingCartLinks) Then
                s.Append("<tr><td>&nbsp;</td><td valign='center'><right><A class='" & GetMenuClass(SEC_Req) & "' href='./OrdReq.aspx'>Requestor Info</A></td>")
                s.Append("<td><A class='menuImgLink' href='./OrdReq.aspx'><img src='" & GetMenuImage(SEC_Req, "images/mnuReq") & "' alt='Requestor' align='left' border='0'/></A></td></tr>")
                s.Append("<tr height='5'><td></td></tr>")
            End If

            If (bShoppingCartLinks) Then
                s.Append("<tr><td>&nbsp;</td><td valign='center'><right><A class='" & GetMenuClass(SEC_Shp) & "' href='./OrdShp.aspx'>Shipping Info</A></td>")
                s.Append("<td><A class='menuImgLink' href='./OrdShp.aspx'><img src='" & GetMenuImage(SEC_Shp, "images/mnuShp") & "' alt='Shipping' align='left' border='0'/></A></td></tr>")
                s.Append("<tr height='5'><td></td></tr>")
            End If

            If (bShoppingCartLinks) Then
                s.Append("<tr><td>&nbsp;</td><td valign='center'><right><A class='" & GetMenuClass(SEC_Cst) & "' href='./OrdCst.aspx'>Custom Info</A></td>")
                s.Append("<td><A class='menuImgLink' href='./OrdCst.aspx'><img src='" & GetMenuImage(SEC_Cst, "images/mnuCst") & "' alt='Customization' align='left' border='0'/></A></td></tr>")
                s.Append("<tr height='5'><td></td></tr>")
            End If

            s.Append("<tr height='5'><td></td></tr>")
            s.Append("<tr height='5'><td></td></tr>")
            s.Append("<tr height='5'><td></td></tr>")
            s.Append("<tr height='5'><td></td></tr>")
            s.Append("<tr height='5'><td></td></tr>")

            If (CurrentUser.CanViewInventory = True) Or _
               (CurrentUser.CanRestockPlan = True) Or _
               (CurrentUser.ViewUsage = True) Or _
               (CurrentUser.CurrentUserReportHistoryVisibility <> "0") Or _
               (CurrentUser.CurrentUserExclusionsAccess > 0) Or _
               (CurrentUser.CanEditKits = True) Then

                s.Append("<tr><td>&nbsp;</td><td valign='center'><A class='" & GetMenuClass(SEC_Adm) & "'" & _
                              " onmouseover='divAdmin.style.display=" & """" & "block" & """" & "'" & _
                              " onclick='divAdmin.style.display=" & """" & "none" & """" & "'" & _
                              ">")
                s.Append("Admin Tools</A></td>")
                s.Append("<td valign='center'><center><A class='menuImgLink'><img src='" & GetMenuImage(SEC_Adm, "images/mnuAdm") & "' alt='Administration' align='left' border='0' " & _
                         " onmouseover='divAdmin.style.display=" & """" & "block" & """" & "'" & _
                         " onclick='divAdmin.style.display=" & """" & "none" & """" & "'" & _
                         "/>")
                s.Append("</A></center></td></tr>")
                s.Append("<tr><td></td><td>")
                s.Append("<div id='divAdmin'><table align='center' cellpadding=0 cellspacing=0>")

                If (CurrentUser.CanViewInventory = True) Then
                    s.Append("<tr><td align='left'><A class='" & GetSubMenuClass(SEC_AInv) & "' href='./AdmInvL.aspx'>Inventory</A></td></tr>")
                End If
                If (CurrentUser.ViewUsage = True) Then
                    s.Append("<tr><td align='left'><A id='lkUse' class='" & GetSubMenuClass(SEC_AUse) & "' href='./AdmUse.aspx'>Usage</A></td></tr>")
                End If
                If CurrentUser.CurrentUserReportHistoryVisibility <> "0" Then
                    s.Append("<tr><td align='left'><A id='lkOrders' class='" & GetSubMenuClass(1) & "' href='./OrdSummary.aspx'>Orders</A></td></tr>")
                End If

                If (CurrentUser.CanRestockPlan = True) Then
                    s.Append("<tr><td align='left'><A id='lkItm' class='" & GetSubMenuClass(SEC_AItm) & "' href='./ItmSel.aspx?C=I'>Items</A></td></tr>")
                    '
                End If
                If (CurrentUser.CanEditKits = True) Then
                    s.Append("<tr><td align='left'><A id='lkKit' class='" & GetSubMenuClass(SEC_AKit) & "' href='./AdmKitL.aspx'>Kits</A></td></tr>")
                End If
                If CurrentUser.CurrentUserExclusionsAccess > 0 Then
                    s.Append("<tr><td align='left'><A id='lkRsp' class='" & GetSubMenuClass(1) & "' href='./Exclusions.aspx'>Access Control</A></td></tr>")
                End If

                If (CurrentUser.CanRestockPlan = True) Then
                    s.Append("<tr><td align='left'><A id='lkRsp' class='" & GetSubMenuClass(SEC_ARsp) & "' href='./AdmRSP.aspx'>Restock</A></td></tr>")
                End If


                s.Append("</div></td></tr></table>")
                s.Append("</td></tr>")
                s.Append("<SCRIPT Language = 'Javascript'>" & _
                        "divAdmin.style.display='none';" & _
                        "</SCRIPT>")
            End If


            s.Append("<tr height='5'><td></td></tr>")
            s.Append("<tr height='5'><td></td></tr>")
            s.Append("<tr height='5'><td></td></tr>")
            s.Append("<tr height='5'><td></td></tr>")
            s.Append("<tr height='5'><td></td></tr>")
            s.Append("<td  class='logoutbuttontd' align='center' colspan='3'><A ID='lkLogOut' href='Logout.aspx'><img src='images/btnLogout.png' alt='Logout' border='0'/></A></td></tr>")
            's.Append("<tr><td>&nbsp;</td><td valign='center'><right><A class='" & GetMenuClass(SEC_Hlp) & "' href='./PDFs/POS_Manual.pdf' target='_blank'>Help</A></td>")
            's.Append("<td><A class='menuImgLink' href='./PDFs/POS_Manual.pdf' target='_blank'><img src='" & GetMenuImage(SEC_Hlp, "images/mnuHlp") & "' alt='Help' align='left' border='0'/></A></td></tr>")
            s.Append("<tr height='5'><td></td></tr>")
            End If
            s.Append("<tr><td><center>&nbsp;</center></td></tr>")
            s.Append("</table></td>")

            Return s.ToString()
    End Function

    Private Function GetLoginSection() As String
        Dim s As StringBuilder = New StringBuilder(200)

        s.Append("<td valign='top' width='100%'  style='height:100%' id='maintcontenttabletd'>")  'Original - detail

        s.Append("<table width='100%' height='100%' id='maintcontenttable'><tr><td>")
        s.Append("<table width='100%' cellspacing='0' cellpadding='0' align='center' bgcolor='" & COL_LOGBAR & "'><tr width='98%' bgcolor='" & COL_LOGBAR & "'>")

        If (CurrentUser.State = MyData.STATE_LoggedOut) Then
            s.Append("<td width=10% align='right'>&nbsp;</td><td width=80% class='pageMessage'><B>" & PageMessage & "</B></td>")

            'If (PageIsLogin(Page.ToString) = True) Then
            '    s.Append("<td width=10% align='right'>&nbsp;</td>")
            'Else
            's.Append("<td width=10% align='right'><table><tr><td><A ID='lkLogIn' href='Login.aspx'><img src='images/btnLogin.png' alt='Login' border='0'/></A></td></tr></table></td>")
            'End If
        ElseIf (CurrentUser.State = MyData.STATE_LoggedIn) Then
        s.Append("<td width=85% class='pageMessage'><B>" & PageMessage & "</B></td>")
        's.Append("<td width=10% align='right'><table><tr><td><A ID='lkLogOut' href='Logout.aspx'><img src='images/btnLogout.png' alt='Logout' border='0'/></A></td></tr></table></td>")

        If Request.ServerVariables("SERVER_NAME").ToLower = "localhost" Or Request.ServerVariables("SERVER_NAME").ToLower = "para.rmrdevelopment.com" Then
            s.Append("<a href=""./Refresh.aspx"" class='menuImgLink'>Refresh</a>")
        End If

        End If
     

        s.Append("</tr></table>")

        s.Append("</tr>")
        s.Append("<tr>")  'For remainder - details

        Return s.ToString()
    End Function


    Private Function GetPageOpenSection() As String
        Dim s As StringBuilder = New StringBuilder(200)

        s.Append("<table width='1280px' align='center' bgcolor='" & COL_BACK & "' border=0 cellspacing=0 cellpadding=0 >")
        s.Append("<tr><td>" & GetLogoHeader() & "</td></tr>")

        s.Append("<tr>")
        s.Append("<table width='1280px' align='center' bgcolor='Transparent' border=0 cellspacing=0 cellpadding=0 id='tablemain'><tr>")

        Return s.ToString()
    End Function

    Private Function GetPageCloseSection() As String
        Dim s As StringBuilder = New StringBuilder(200)

        's.Append("</tr></table>")
        's.Append("</tr></table>")
        's.Append("</tr>")
        's.Append("<tr>")

        's.Append("<td>")
        's.Append("<table width=100% align='center' border=0 cellspacing=0 cellpadding=0><tr>")
        's.Append("<td valign='bottom' align='right' width='8%'><IMG src='./logos/D3LogoS.gif' border='0 style='BACKGROUND-COLOR:transparent'></td>")
        's.Append("<td valign='center' align='left' width='17%' class='pageFooter'>Powered by D3 Secure server</td>")
        's.Append("<td width='75%'><center><A class='siteFooter'>&#169 Copyright " & Now.Year & ", Paragraphics,Inc. All rights reserved.</A></center></td>")
        's.Append("</tr></table></td>")

        's.Append("</tr>")

        's.Append("</table>")
        's.Append("</td></tr>")

        's.Append("</table>")                                    'End Main Table

        Return s.ToString()
    End Function

    Private Function GetFooter() As String
        Dim s As StringBuilder = New StringBuilder(200)
        s.Append("</center>")
        s.Append("</body>")
        s.Append("</html>")

        Return s.ToString()
    End Function

    Private Function GetSiteBorderClose() As String
        Dim s As StringBuilder = New StringBuilder(200)

        's.Append("</td>")                                     'Close for Main Section
        's.Append("</tr></table>")                             'Close for Site Table                         

        Return s.ToString()
    End Function
    Protected Sub PageDirectWithTarget(ByVal strTarget As String, Optional ByVal iParentID As Int32 = 0, Optional ByVal iEditID As Int32 = 0)
        Dim sLink As String = String.Empty
        Dim sQry As String = String.Empty
        Dim sStatus As String = String.Empty

        sQry = "?PD=" + iParentID.ToString() + "&ED=" + iEditID.ToString()

        Session("PMsg") = sStatus
        Response.Redirect(strTarget, False)
    End Sub
    Protected Sub PageDirect(Optional ByVal iDestinPageID As Int32 = PAG_Main, Optional ByVal iParentID As Int32 = 0, Optional ByVal iEditID As Int32 = 0)
        Dim sLink As String = String.Empty
        Dim sQry As String = String.Empty
        Dim sStatus As String = String.Empty

        sQry = "?PD=" + iParentID.ToString() + "&ED=" + iEditID.ToString()

        Select Case iDestinPageID
            Case PAG_Login
                sStatus = "Logged In"
                sLink = "./Login.aspx"

            Case PAG_NotLoggedIn
                sStatus = "Not Logged In"
                sLink = "./Main.aspx"

            Case PAG_Error
                sStatus = "ERROR"
                sLink = "./Main.aspx"

            Case PAG_ErrorSpecified
                sLink = "./Main.aspx"

            Case PAG_OrdList
                sStatus = "Order List"
                sLink = "./OrdL.aspx"

            Case PAG_OrdRequest
                sStatus = "Order Requestor Information"
                sLink = "./OrdReq.aspx"

            Case PAG_OrdShip
                sStatus = "Order Shipping Information"
                sLink = "./OrdShp.aspx"

            Case PAG_OrdItems
                sStatus = "Order Item Details"
                sLink = "./OrdItm.aspx"

            Case PAG_OrdItemSelect
                sStatus = "Order Item Selection"
                sLink = "./ItmSel.aspx?C=O"
                '
            Case PAG_OrdKits
                sStatus = "Order Kit Details"
                sLink = "./OrdKit.aspx"

            Case PAG_OrdKitSelect
                sStatus = "Order Kit Selection"
                sLink = "./KitSel.aspx?C=O"
                '
            Case PAG_OrdCustom
                sStatus = "Order Customize"
                sLink = "./OrdCst.aspx"

            Case PAG_OrdCart
                sStatus = "Shopping Cart"
                sLink = "./OrdCrt.aspx"

            Case PAG_OrdSubmit
                sStatus = "Order Submit / Confirm"
                sLink = "./OrdSve.aspx"

            Case PAG_OrdPreview
                sStatus = "Order Preview"
                sLink = "./OrdSveP.aspx"

            Case PAG_OrdConfirm
                sStatus = "Order Review / Confirmation"
                sLink = "./OrdCfm.aspx"

            Case PAG_OrdReqSearch
                sStatus = "Requestor Search"
                sLink = "./OrdSReq.aspx"

            Case PAG_OrdShpSearch
                sStatus = "Shipping Search"
                sLink = "./OrdSShp.aspx"

            Case PAG_AdmMenu
                sStatus = "Administration Menu"
                sLink = "./AdmMnu.aspx"

            Case PAG_AdmKitList
                sStatus = "Kit List"
                sLink = "./AdmKitL.aspx"

            Case PAG_AdmKitEdit
                sStatus = "Kit Edit"
                sLink = "./AdmKitE.aspx" & sQry

            Case PAG_AdmKitItemSelect
                sStatus = "Kit Item Select"
                sLink = "./ItmSel.aspx?C=K"

            Case PAG_AdmInvnList
                sStatus = "Inventory List"
                sLink = "./AdmInvL.aspx"

            Case PAG_AdmInvnEdit
                sStatus = "Inventory Review"
                sLink = "./AdmInvE.aspx"

            Case PAG_AdmInvnPreview
                sStatus = "Inventory Preview"
                sLink = "./AdmInvP.aspx"

            Case PAG_AdmUsage
                sStatus = "Usage"
                sLink = "./AdmUse.aspx"

            Case PAG_AdmRestockPlan
                sStatus = "Restock Planner"
                sLink = "./AdmRsp.aspx"

            Case PAG_AdmItemSelect
                sStatus = "Item Select"
                sLink = "./ItmSel.aspx?C=I"

            Case PAG_AdmItemEdit
                sStatus = "Item Maintenance"
                sLink = "./AdmItm.aspx"
           
            Case Else
                sLink = "./Main.aspx"
        End Select

        Session("PMsg") = sStatus
        Response.Redirect(sLink, False)
    End Sub

    Private Sub LogActivity(ByVal sMsg As String, Optional ByVal sFile As String = "")
        Dim moLog As Log
        Dim sLogFile As String = String.Empty

        Try
            If (sFile.Length > 0) Then
                sLogFile = sFile
            Else
                sLogFile = ConfigurationSettings.AppSettings("LogFolder") & "Sess.log"
            End If

            moLog = New Log
            moLog.Entry(sLogFile, "      " & Format(Now, "MM-dd-yyyy hh:mm:ss") & " - PAGE:  " & sMsg)

        Catch ex As Exception

        End Try
        moLog = Nothing
    End Sub
    Private Sub LogError(ByVal sMsg As String, Optional ByVal sFile As String = "")
        Dim moLog As Log
        Dim sLogFile As String = String.Empty

        Try
            If (sFile.Length > 0) Then
                sLogFile = sFile
            Else
                sLogFile = ConfigurationSettings.AppSettings("LogFolder") & "PageErr.log"
            End If
            moLog = New Log
            moLog.Entry(sLogFile, "      " & Format(Now, "MM-dd-yyyy hh:mm:ss") & " **ERROR** PAGE:  " & sMsg)

        Catch ex As Exception

        End Try
        moLog = Nothing
    End Sub

    Sub writeToEvenLog(ByVal strMessage As String, ByVal tEventType As EventLogEntryType)

        Dim myLog As EventLog = New EventLog()
        myLog.Source = "Posn"
        Try
            myLog.WriteEntry(strMessage, tEventType)
        Catch ex As Exception

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        MyData.Dispose()
        pCurrentUser = Nothing
        pCurrentOrder = Nothing
        MyBase.Finalize()
    End Sub



End Class




