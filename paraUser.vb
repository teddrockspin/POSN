''''''''''''''''''''
'mw - 08-12-2009
'mw - 07-14-2009
'mw - 05-04-2009
'mw - 08-30-2007
'mw - 06-27-2007
'mw - 03-10-2007
'mw - 02-10-2007
'mw - 01-20-2007
''''''''''''''''''''

'<Serializable()> _
Public Class paraUser

    Public Enum LookupOrderType
        None
        Local
        Overall
    End Enum

    Private Const POS_INV = 1
    Private Const POS_KVW = 2
    Private Const POS_KED = 3
    Private Const POS_DWN = 4
    Private Const POS_REM = 5
    Private Const POS_XLI = 6
    Private Const POS_XOR = 7
    Private Const POS_CST = 8
    Private Const POS_COV = 9
    Private Const POS_EMA = 10
    Private Const POS_LKP = 11
    Private Const POS_RSP = 12

    'Private Const STATE_NONE = 0
    'Private Const STATE_LOGGEDOUT = 0
    'Private Const STATE_LOGGEDIN = 1
    'Private Const STATE_INORDER = 2
    'Private Const STATE_INADMIN = 3

    Private pState As Integer = 0

    Private pCustomerID As Long = 0
    Private pCustomerCode As String = String.Empty
    Private pCustomerName As String = String.Empty
    Private pCustomerPhone As String = String.Empty
    Private pCustomerEmail As String = String.Empty
    Private pAccessCodeID As Long = 0
    Private pAccessCode As String = String.Empty
    Private pAccessCodeEMail As String = String.Empty
    Private pWebSet As String = "00000000000000000000"
    Private pMaxQtyPerLineItem As Long = 0
    Private pMaxQtyPerOrder As Long = 0
    Private pCVLevels As String = String.Empty
    Private pLogoFile As String = String.Empty
    Private pPreviousPage As String = String.Empty
    Private pCurrentPage As String = String.Empty
    Private pCurrentK1Sels As String = String.Empty
    Private pCurrentK2Sels As String = String.Empty
    Private pCurrentK3Sels As String = String.Empty
    Private pCurrentK4Sels As String = String.Empty
    Private pCurrentItmSels(50) As String
    Private pCurrentK1KitSels As String = String.Empty
    Private pCurrentKitSels As String = String.Empty
    Private pCurrentItmScrolls As String = String.Empty
    Private pCurrentKitScrolls As String = String.Empty

    'NEW added by IB
    Private pCurrentUserAccountStatus As Boolean = False
    Private pCurrentUserReportHistoryVisibility As String = "N"
    Private pCurrentUserAllowOrderEdit As Boolean = False
    Private pCurrentUserExclusionsAllowEdit As Boolean = False
    Private pCurrentUserExclusionsAccess As Integer = 0
    Private pDisableAllQuantityLimits As Boolean = False
    'Private pCanExtendQtyLI As Boolean = (Mid(pWebSet, POS_XLI, 1) = 1)
    Private pShowPrice As Boolean = False

    Private pViewUsage As Boolean = False
    Private pViewThumbnails As Boolean = False



    Friend Property ViewUsage() As Boolean
        Get
            Return pViewUsage
        End Get
        Set(ByVal value As Boolean)
            pViewUsage = value
        End Set
    End Property

    Friend Property ViewThumbnails() As Boolean
        Get
            Return pViewThumbnails
        End Get
        Set(ByVal value As Boolean)
            pViewThumbnails = value
        End Set
    End Property

    Friend Property ShowPrice() As Boolean
        Get
            Return pShowPrice
        End Get
        Set(ByVal value As Boolean)
            pShowPrice = value
        End Set
    End Property

    Friend ReadOnly Property SendConfirmationEmail() As Boolean
        Get
            Dim bSet As Boolean : bSet = False

            'If Access Code set to View Kits...
            If (Len(pWebSet) > 0) Then
                bSet = (Mid(pWebSet, POS_EMA, 1) >= 1)
            End If

            Return bSet
        End Get
    End Property
    Friend Property DisableAllQuantityLimits() As Boolean
        Get
            Return pDisableAllQuantityLimits
        End Get
        Set(ByVal value As Boolean)
            pDisableAllQuantityLimits = value
        End Set
    End Property

    Friend Property CurrentUserExclusionsAllowEdit() As Boolean
        Get
            Return pCurrentUserExclusionsAllowEdit
        End Get
        Set(ByVal value As Boolean)
            pCurrentUserExclusionsAllowEdit = value
        End Set
    End Property

    Friend Property CurrentUserExclusionsAccess() As Integer
        Get
            Return pCurrentUserExclusionsAccess
        End Get
        Set(ByVal value As Integer)
            pCurrentUserExclusionsAccess = value
        End Set
    End Property
    Friend Property CurrentUserAllowOrderEdit() As Boolean
        Get
            Return pCurrentUserAllowOrderEdit
        End Get
        Set(ByVal Value As Boolean)
            pCurrentUserAllowOrderEdit = Value
        End Set
    End Property
    Friend Property CurrentUserReportHistoryVisibility() As String
        Get
            Return pCurrentUserReportHistoryVisibility
        End Get
        Set(ByVal Value As String)
            pCurrentUserReportHistoryVisibility = Value
        End Set
    End Property
    Friend Property CurrentUserAccountStatus() As Boolean
        Get
            Return pCurrentUserAccountStatus
        End Get
        Set(ByVal Value As Boolean)
            pCurrentUserAccountStatus = Value
        End Set
    End Property
    'Basic
    Friend Property State() As Integer
        Get
            Return pState
        End Get
        Set(ByVal Value As Integer)
            pState = Value
        End Set
    End Property

    Friend ReadOnly Property PreviousPage() As String
        Get
            If (pPreviousPage.Length = 0) Then
                Return ConfigurationSettings.AppSettings("HomeAddress") & "/" & ConfigurationSettings.AppSettings("SiteLocation") & "/Main.aspx"
            Else
                Return pPreviousPage
            End If
        End Get
    End Property

    Friend Property CurrentPage() As String
        Get
            If (pCurrentPage.Length = 0) Then
                Return ConfigurationSettings.AppSettings("HomeAddress") & "/" & ConfigurationSettings.AppSettings("SiteLocation") & "/Main.aspx"
            Else
                Return pCurrentPage
            End If
        End Get
        Set(ByVal Value As String)
            pPreviousPage = pCurrentPage
            pCurrentPage = Value
        End Set
    End Property

    Friend Property CurrentK1Sels() As String
        Get
            Return pCurrentK1Sels
        End Get
        Set(ByVal Value As String)
            pCurrentK1Sels = Value
        End Set
    End Property
    Friend Property CurrentK2Sels() As String
        Get
            Return pCurrentK2Sels
        End Get
        Set(ByVal Value As String)
            pCurrentK2Sels = Value
        End Set
    End Property
    Friend Property CurrentK3Sels() As String
        Get
            Return pCurrentK3Sels
        End Get
        Set(ByVal Value As String)
            pCurrentK3Sels = Value
        End Set
    End Property
    Friend Property CurrentK4Sels() As String
        Get
            Return pCurrentK4Sels
        End Get
        Set(ByVal Value As String)
            pCurrentK4Sels = Value
        End Set
    End Property
    Friend Property CurrentItmSels(ByVal indx As Integer) As String
        Get
            If (indx <= UBound(pCurrentItmSels)) Then
                Return pCurrentItmSels(indx)
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            If (indx <= UBound(pCurrentItmSels)) Then
                pCurrentItmSels(indx) = Value
            End If
        End Set
    End Property
    Friend ReadOnly Property CurrentItmSelPageCnt() As Integer
        Get
            Return UBound(pCurrentItmSels) - 1
        End Get
    End Property

    Friend Property CurrentK1KitSels() As String
        Get
            Return pCurrentK1KitSels
        End Get
        Set(ByVal Value As String)
            pCurrentK1KitSels = Value
        End Set
    End Property
    Friend Property CurrentKitSels() As String
        Get
            Return pCurrentKitSels
        End Get
        Set(ByVal Value As String)
            pCurrentKitSels = Value
        End Set
    End Property

    Friend Property CurrentItmScrolls() As String
        Get
            Return pCurrentItmScrolls
        End Get
        Set(ByVal Value As String)
            pCurrentItmScrolls = Value
        End Set
    End Property

    Friend Property CurrentKitScrolls() As String
        Get
            Return pCurrentKitScrolls
        End Get
        Set(ByVal Value As String)
            pCurrentKitScrolls = Value
        End Set
    End Property


    'Customer
    Friend Property CustomerID() As Long
        Get
            Return pCustomerID
        End Get
        Set(ByVal Value As Long)
            pCustomerID = Value
        End Set
    End Property

    Friend Property CustomerCode() As String
        Get
            Return pCustomerCode
        End Get
        Set(ByVal Value As String)
            pCustomerCode = Value
        End Set
    End Property

    Friend Property CustomerName() As String
        Get
            Return pCustomerName
        End Get
        Set(ByVal Value As String)
            pCustomerName = Value
        End Set
    End Property

    Friend Property CustomerEmail() As String
        Get
            Return pCustomerEmail
        End Get
        Set(ByVal Value As String)
            pCustomerEmail = Value
        End Set
    End Property

    Friend Property CustomerPhone() As String
        Get
            Return pCustomerPhone
        End Get
        Set(ByVal Value As String)
            pCustomerPhone = Value
        End Set
    End Property

    'Access Code
    Friend Property AccessCodeID() As Long
        Get
            Return pAccessCodeID
        End Get
        Set(ByVal Value As Long)
            pAccessCodeID = Value
        End Set
    End Property

    Friend Property AccessCode() As String
        Get
            Return pAccessCode
        End Get
        Set(ByVal Value As String)
            pAccessCode = Value
        End Set
    End Property

    Friend Property AccessCodeEmail() As String
        Get
            Return pAccessCodeEMail
        End Get
        Set(ByVal Value As String)
            pAccessCodeEMail = Value
        End Set
    End Property

    'Notes:  WebSet
    'Position 1:  Access to View Inventory
    'Position 2:  Access to View Kits
    'Position 3:  Access to Edit Kits
    'Position 4:  Access to Download Images (PDFs)
    ' NONE  - 0
    ' L     - 1
    ' S     - 2
    ' H     - 3
    ' LS    - 4
    ' LH    - 5
    ' SH    - 6
    ' LSH   - 7
    'Position 5:  Access to Recall last order upon start of order
    'Position 6:  Access to allow ordering past limits pending authorization - Qty Per Line Item
    'Position 7:  Access to allow ordering past limits pending authorization - Qty Per Order
    'Position 8:  Access to custom DETAILED data
    'Position 9:  Access to Cover Sheet Editing
    'Position 10:  Access to have Email sent upon Order Status changing to Shipped
    '*Important to remember to read in properly in Data.inc-VerifyUser when initializing on Web
    Friend WriteOnly Property WebSet() As String
        Set(ByVal Value As String)
            pWebSet = Value
        End Set
    End Property

    Friend ReadOnly Property CanViewInventory() As Boolean
        Get
            Dim bSet As Boolean : bSet = False

            'If Access Code set to View Inventory...
            If (Len(pWebSet) > 0) Then
                bSet = (Mid(pWebSet, POS_INV, 1) = 1)
            End If

            Return bSet
        End Get
    End Property

    Friend ReadOnly Property CanViewKits() As Boolean
        Get
            Dim bSet As Boolean : bSet = False

            'If Access Code set to View Kits...
            If (Len(pWebSet) > 0) Then
                bSet = (Mid(pWebSet, POS_KVW, 1) >= 1)
            End If

            Return bSet
        End Get
    End Property
    Friend ReadOnly Property KitViewPermissions() As Integer
        Get
            Dim iSet As Int16 : iSet = 0

            'If Access Code set to View Kits...
            If (Len(pWebSet) > 0) Then
                iSet = Mid(pWebSet, POS_KVW, 1)
            End If

            Return iSet
        End Get
    End Property

    Friend ReadOnly Property CanEditKits() As Boolean
        Get
            Dim bSet As Boolean : bSet = False

            'If Access Code set to Edit Kits...
            If (Len(pWebSet) > 0) Then
                bSet = (Mid(pWebSet, POS_KED, 1) >= 1)
            End If

            Return bSet
        End Get
    End Property
    Friend ReadOnly Property KitEditPermissions() As Integer
        Get
            Dim iSet As Int16 : iSet = 0

            'If Access Code set to View Kits...
            If (Len(pWebSet) > 0) Then
                iSet = Mid(pWebSet, POS_KED, 1)
            End If

            Return iSet
        End Get
    End Property

    Friend ReadOnly Property CanRememberOrder() As Boolean
        Get
            Dim bSet As Boolean : bSet = False

            'If Access Code set to Remember Last Order Base Info...
            If (Len(pWebSet) > 0) Then
                bSet = (Mid(pWebSet, POS_REM, 1) = 1)
            End If

            Return bSet
        End Get
    End Property

    Friend ReadOnly Property CanLookupOrder() As LookupOrderType
        Get
            Return Mid(pWebSet, POS_LKP, 1)
        End Get
    End Property

    Friend ReadOnly Property CanDownloadImages() As Boolean
        Get
            Dim bSet As Boolean : bSet = False

            'If Access Code set to have PDF downloads...
            If (Len(pWebSet) > 0) Then
                'bSet = (Mid(pWebSet, POS_DWN, 1) >= 1)
                bSet = (Mid(pWebSet, POS_DWN, 1) >= 1)
            End If

            Return bSet
        End Get
    End Property

    Friend ReadOnly Property CanDownloadImagesLowRes() As Boolean
        Get
            Dim bSet As Boolean : bSet = False

            'If Access Code set to have Low Resolution PDF downloads...
            If (Len(pWebSet) > 0) Then
                bSet = (Mid(pWebSet, POS_DWN, 1) = 1) Or _
                       (Mid(pWebSet, POS_DWN, 1) = 4) Or _
                       (Mid(pWebSet, POS_DWN, 1) = 5) Or _
                       (Mid(pWebSet, POS_DWN, 1) = 7)
            End If

            Return bSet
        End Get
    End Property

    Friend ReadOnly Property CanDownloadImagesStandardRes() As Boolean
        Get
            Dim bSet As Boolean : bSet = False

            'If Access Code set to have Standard Resolution PDF downloads...
            If (Len(pWebSet) > 0) Then
                bSet = (Mid(pWebSet, POS_DWN, 1) = 2) Or _
                       (Mid(pWebSet, POS_DWN, 1) = 4) Or _
                       (Mid(pWebSet, POS_DWN, 1) = 6) Or _
                       (Mid(pWebSet, POS_DWN, 1) = 7)
            End If

            Return bSet
        End Get
    End Property

    Friend ReadOnly Property CanDownloadImagesHighRes() As Boolean
        Get
            Dim bSet As Boolean : bSet = False

            'If Access Code set to have High Resolution PDF downloads...
            If (Len(pWebSet) > 0) Then
                'bSet = (Mid(pWebSet, POS_DWN, 1) = 2)
                bSet = (Mid(pWebSet, POS_DWN, 1) = 3) Or _
                       (Mid(pWebSet, POS_DWN, 1) = 5) Or _
                       (Mid(pWebSet, POS_DWN, 1) = 6) Or _
                       (Mid(pWebSet, POS_DWN, 1) = 7)
            End If

            Return bSet
        End Get
    End Property

    Friend ReadOnly Property CanExtendQtyLI() As Boolean
        Get
            Dim bSet As Boolean : bSet = False

            'If Access Code set to allow extended ordering per Line Item...
            If pDisableAllQuantityLimits Then
                bSet = True
            Else
                If (Len(pWebSet) > 0) Then
                    bSet = (Mid(pWebSet, POS_XLI, 1) = 1)
                End If
            End If


            Return bSet
        End Get

    End Property

    Friend ReadOnly Property CanExtendQtyOR() As Boolean
        Get
            Dim bSet As Boolean : bSet = False
            If pDisableAllQuantityLimits Then
                bSet = True
            Else
                'If Access Code set to allow extended ordering per Order...
                If (Len(pWebSet) > 0) Then
                    bSet = (Mid(pWebSet, POS_XOR, 1) = 1)
                End If
            End If
     

            Return bSet
        End Get
    End Property

    Friend ReadOnly Property CanEditCoverSheet() As Boolean
        Get
            Dim bSet As Boolean : bSet = False

            'If Access Code allowed to enter contents for cover sheet...
            If (Len(pWebSet) > 0) Then
                bSet = (Mid(pWebSet, POS_COV, 1) = 1)
            End If

            Return bSet
        End Get
    End Property

    Friend ReadOnly Property CanEditCustomDataDetail() As Boolean
        Get
            Dim bSet As Boolean : bSet = False

            'If Access Code allowed to enter detailed custom data...
            If (Len(pWebSet) > 0) Then
                bSet = (Mid(pWebSet, POS_CST, 1) = 1)
            End If

            Return bSet
        End Get
    End Property

    Friend ReadOnly Property CanRestockPlan() As Boolean
        Get
            Dim bSet As Boolean : bSet = False

            'If Access Code set to Use Restock Planning Features - Restocking Page, Item Maintenance Page...
            If (Len(pWebSet) > 0) Then
                bSet = (Mid(pWebSet, POS_RSP, 1) = 1)
            End If

            Return bSet
        End Get
    End Property

    Friend Property MaxQtyPerLineItem() As Long
        Get
            If pDisableAllQuantityLimits Then
                Return 9999999999
            Else
                Return pMaxQtyPerLineItem
            End If

        End Get
        Set(ByVal Value As Long)
            pMaxQtyPerLineItem = Value
        End Set
    End Property

    Friend Property MaxQtyPerOrder() As Long
        Get
            If pDisableAllQuantityLimits Then
                Return 9999999999
            Else
                Return pMaxQtyPerOrder
            End If

        End Get
        Set(ByVal Value As Long)
            pMaxQtyPerOrder = Value
        End Set
    End Property

    Friend Property CoverSheetLevels() As String
        Get
            Return pCVLevels
        End Get
        Set(ByVal Value As String)
            pCVLevels = Value
        End Set
    End Property

    Friend Property LogoFile() As String
        Get
            Return pLogoFile
        End Get
        Set(ByVal Value As String)
            pLogoFile = Value
        End Set
    End Property


    Friend Sub Init()
        Dim indx As Integer = 0

        pState = 0
        pCustomerEmail = String.Empty
        pCustomerPhone = String.Empty
        pCustomerID = 0
        pCustomerCode = String.Empty
        pCustomerName = String.Empty
        pAccessCodeID = 0
        pAccessCode = String.Empty
        pWebSet = "00000000000000000000"
        pMaxQtyPerLineItem = 0
        pMaxQtyPerOrder = 0
        pCVLevels = String.Empty
        pLogoFile = "D3LogoL.gif"
        ItmSelsInit()
        KitSelsInit()
    End Sub

    Friend Sub ItmSelsInit()
        Dim indx As Integer = 0

        For indx = 0 To UBound(pCurrentItmSels) - 1
            pCurrentItmSels(indx) = String.Empty
        Next indx
    End Sub

    Friend Sub KitSelsInit()
        pCurrentKitSels = String.Empty
    End Sub

End Class
