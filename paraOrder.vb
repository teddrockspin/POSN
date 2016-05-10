''''''''''''''''''''
'mw - 07-20-2009
'mw - 05-04-2009
'mw - 08-28-2008
'mw - 05-21-2008
'mw - 03-07-2008
'mw - 08-18-2007
'mw - 07-15-2007
'mw - 05-25-2007
'mw - 02-16-2007
'mw - 12-09-2006
''''''''''''''''''''


Imports System.Text
Imports System.Data.SqlClient
Imports System.IO
Imports POSN.dsCustomerTableAdapters


Public Class paraOrder

    Private Const MSG_RESP = "This message was sent from a notification-only address.  Please do not reply to this message."

    Private Const PRO_REQ = 1
    Private Const PRO_SHP = 2
    Private Const PRO_ITM = 3
    Private Const PRO_KIT = 4
    Private Const PRO_CST = 5
    Private Const PRO_CRT = 6

    Private pCustomerEmail As String = String.Empty
    Private pProcessingChg As Single = 0
    Private pMonthlyChg As Single = 0
    Private pFreightMarkupChg As Single = 0
    Private pLinePckChg As Single = 0
    Private pLineKitChg As Single = 0
    Private pMaxQtyPerLineItem As Long = 0
    Private pMaxQtyPerOrder As Long = 0
    Private pCanExtendQtyLI As Boolean = False
    Private pCanExtendQtyOR As Boolean = False
    Private pRequestorTitle As String = String.Empty
    Private pRequestorFirstName As String = String.Empty
    Private pRequestorLastName As String = String.Empty
    Private pRequestorEmail As String = String.Empty

    Private bOverQuantityLimit As Boolean = False

    Private pShipContactFirstName As String = String.Empty
    Private pShipContactLastName As String = String.Empty
    Private pShipCompany As String = String.Empty
    Private pShipAddress1 As String = String.Empty
    Private pShipAddress2 As String = String.Empty
    Private pShipCity As String = String.Empty
    Private pShipState As String = String.Empty
    Private pShipCountry As String = String.Empty
    Private pShipZip As String = String.Empty
    Private pShipPrefID As Int32 = 0
    Private pShipPrefDesc As String = String.Empty
    Private pShipNote As String = String.Empty
    Private pCoverSheetID As Int32 = 0
    Private pCoverSheetDesc As String = String.Empty
    Private pCoverSheetText As String = String.Empty

    Private pCurrentCstCart As New System.Data.DataTable
    Private pCurrentItmCart As New System.Data.DataTable
    Private pCurrentKitCart As New System.Data.DataTable

    Private pID As Long = 0
    Private pProblemMsg As String = "Order has not been submitted..."
    Private pProblemArea As Integer = 0
    Private pVisited As String = String.Empty 'RSIKC - Requestor/Shipping/Items/Kits/Customize
    Private pEditOrder As Boolean = False
    Private pOrderID As Integer = 0
    'Private moLog As Log
    Private msLogFile As String = String.Empty
    Private msNewLine As String = vbCr & vbCrLf
    Private pStatusID As Integer = 0
    Private pAccessCode As String = ""
    Private pAccessCodeID As Integer = 0
    Private pDisableAllQuantityLimits As Boolean = False
    Private pScheduledDelivery As Date
    Private pRecipientEmail As String
    Private pRecipientPhone As String
    Private pSendRecipientConfirmEmail As Boolean

#Region "properties"
    Friend Property SendRecipientConfirmEmail As Boolean
        Get
            Return pSendRecipientConfirmEmail
        End Get
        Set(value As Boolean)
            pSendRecipientConfirmEmail = value
        End Set
    End Property

    Friend Property RecipientPhone() As String
        Get
            Return pRecipientPhone
        End Get
        Set(ByVal Value As String)
            pRecipientPhone = Value
        End Set
    End Property
    Friend Property RecipientEmail() As String
        Get
            Return pRecipientEmail
        End Get
        Set(ByVal Value As String)
            pRecipientEmail = Value
        End Set
    End Property
    Friend Property ScheduledDelivery() As Date
        Get
            Return pScheduledDelivery
        End Get
        Set(ByVal Value As Date)
            pScheduledDelivery = Value
        End Set
    End Property

    Friend Property DisableAllQuantityLimits() As Boolean
        Get
            Return pDisableAllQuantityLimits
        End Get
        Set(ByVal Value As Boolean)
            pDisableAllQuantityLimits = Value
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

    Friend Property AccessCodeID() As Integer
        Get
            Return pAccessCodeID
        End Get
        Set(ByVal Value As Integer)
            pAccessCodeID = Value
        End Set
    End Property
    Friend Property StatusID() As Integer
        Get
            Return pStatusID
        End Get
        Set(ByVal Value As Integer)
            pStatusID = Value
        End Set
    End Property

    Public Property OverQuantityLimit() As Boolean
        Get
            Return bOverQuantityLimit
        End Get
        Set(ByVal value As Boolean)
            bOverQuantityLimit = value
        End Set
    End Property

    Friend Property OrderID() As Integer
        Get
            Return pOrderID
        End Get
        Set(ByVal Value As Integer)
            pOrderID = Value
        End Set
    End Property
    Friend Property EditOrder() As Boolean
        Get
            Return pEditOrder
        End Get
        Set(ByVal Value As Boolean)
            pEditOrder = Value
        End Set
    End Property
    'Customer
    Friend Property CustomerEmail() As String
        Get
            Return pCustomerEmail
        End Get
        Set(ByVal Value As String)
            pCustomerEmail = Value
        End Set
    End Property

    'Order
    Friend Property ProcessingChg() As Single
        Get
            Return pProcessingChg
        End Get
        Set(ByVal Value As Single)
            pProcessingChg = Value
        End Set
    End Property

    Friend Property MonthlyChg() As Single
        Get
            Return pMonthlyChg
        End Get
        Set(ByVal Value As Single)
            pMonthlyChg = Value
        End Set
    End Property

    Friend Property FreightMarkupChg() As Single
        Get
            Return pFreightMarkupChg
        End Get
        Set(ByVal Value As Single)
            pFreightMarkupChg = Value
        End Set
    End Property

    Friend Property LinePckChg() As Single
        Get
            Return pLinePckChg
        End Get
        Set(ByVal Value As Single)
            pLinePckChg = Value
        End Set
    End Property

    Friend Property LineKitChg() As Single
        Get
            Return pLineKitChg
        End Get
        Set(ByVal Value As Single)
            pLineKitChg = Value
        End Set
    End Property

    Friend Property MaxQtyPerLineItem() As Long
        Get
            Return pMaxQtyPerLineItem
        End Get
        Set(ByVal Value As Long)
            pMaxQtyPerLineItem = Value
        End Set
    End Property

    Friend Property MaxQtyPerOrder() As Long
        Get
            Return pMaxQtyPerOrder
        End Get
        Set(ByVal Value As Long)
            pMaxQtyPerOrder = Value
        End Set
    End Property

    Friend Property CanExtendQtyLI() As Boolean
        Get
            Return pCanExtendQtyLI
        End Get
        Set(ByVal Value As Boolean)
            pCanExtendQtyLI = Value
        End Set
    End Property

    Friend Property CanExtendQtyOR() As Boolean
        Get
            Return pCanExtendQtyOR
        End Get
        Set(ByVal Value As Boolean)
            pCanExtendQtyOR = Value
        End Set
    End Property

    'Requestor
    Friend Property RequestorTitle() As String
        Get
            Return pRequestorTitle
        End Get
        Set(ByVal Value As String)
            pRequestorTitle = Value
        End Set
    End Property

    Friend Property RequestorFirstName() As String
        Get
            Return pRequestorFirstName
        End Get
        Set(ByVal Value As String)
            pRequestorFirstName = Value
        End Set
    End Property

    Friend Property RequestorLastName() As String
        Get
            Return pRequestorLastName
        End Get
        Set(ByVal Value As String)
            pRequestorLastName = Value
        End Set
    End Property

    Friend Property RequestorEmail() As String
        Get
            Return pRequestorEmail
        End Get
        Set(ByVal Value As String)
            pRequestorEmail = Value
        End Set
    End Property

    Friend Property ShipContactFirstName() As String
        Get
            Return pShipContactFirstName
        End Get
        Set(ByVal Value As String)
            pShipContactFirstName = Value
        End Set
    End Property

    Friend Property ShipContactLastName() As String
        Get
            Return pShipContactLastName
        End Get
        Set(ByVal Value As String)
            pShipContactLastName = Value
        End Set
    End Property

    Friend Property ShipCompany() As String
        Get
            Return pShipCompany
        End Get
        Set(ByVal Value As String)
            pShipCompany = Value
        End Set
    End Property

    Friend Property ShipAddress1() As String
        Get
            Return pShipAddress1
        End Get
        Set(ByVal Value As String)
            pShipAddress1 = Value
        End Set
    End Property

    Friend Property ShipAddress2() As String
        Get
            Return pShipAddress2
        End Get
        Set(ByVal Value As String)
            pShipAddress2 = Value
        End Set
    End Property

    Friend Property ShipCity() As String
        Get
            Return pShipCity
        End Get
        Set(ByVal Value As String)
            pShipCity = Value
        End Set
    End Property

    Friend Property ShipState() As String
        Get
            Return pShipState
        End Get
        Set(ByVal Value As String)
            pShipState = Value
        End Set
    End Property

    Friend Property ShipCountry() As String
        Get
            Return pShipCountry
        End Get
        Set(ByVal Value As String)
            pShipCountry = Value
        End Set
    End Property

    Friend Property ShipZip() As String
        Get
            Return pShipZip
        End Get
        Set(ByVal Value As String)
            pShipZip = Value
        End Set
    End Property

    Friend Property ShipPrefID() As String
        Get
            Return pShipPrefID
        End Get
        Set(ByVal Value As String)
            pShipPrefID = Value
        End Set
    End Property

    Friend Property ShipPrefDesc() As String
        Get
            Return pShipPrefDesc
        End Get
        Set(ByVal Value As String)
            pShipPrefDesc = Value
        End Set
    End Property

    Friend Property ShipNote() As String
        Get
            Return pShipNote
        End Get
        Set(ByVal Value As String)
            pShipNote = Value
        End Set
    End Property

    Friend Property CoverSheetID() As String
        Get
            Return pCoverSheetID
        End Get
        Set(ByVal Value As String)
            pCoverSheetID = Value
        End Set
    End Property

    Friend Property CoverSheetDesc() As String
        Get
            Return pCoverSheetDesc
        End Get
        Set(ByVal Value As String)
            pCoverSheetDesc = Value
        End Set
    End Property

    Friend Property CoverSheetText() As String
        Get
            Return pCoverSheetText
        End Get
        Set(ByVal Value As String)
            pCoverSheetText = Value
        End Set
    End Property

    Friend Property CstCart() As System.Data.DataTable
        Get
            Return pCurrentCstCart
        End Get
        Set(ByVal Value As System.Data.DataTable)
            pCurrentCstCart = Value
        End Set
    End Property

    Friend Property ItmCart() As System.Data.DataTable
        Get
            Return pCurrentItmCart
        End Get
        Set(ByVal Value As System.Data.DataTable)
            pCurrentItmCart = Value
        End Set
    End Property

    Friend Property KitCart() As System.Data.DataTable
        Get
            Return pCurrentKitCart
        End Get
        Set(ByVal Value As System.Data.DataTable)
            pCurrentKitCart = Value
        End Set
    End Property

    Friend ReadOnly Property ConfirmNo() As Long
        Get
            Return pID
        End Get
    End Property

    Friend ReadOnly Property ProblemMsg() As String
        Get
            Return pProblemMsg
        End Get
    End Property

    Friend ReadOnly Property ProblemArea() As Integer
        Get
            Return pProblemArea
        End Get
    End Property

    Friend ReadOnly Property RequiresAuthorShip() As Boolean
        Get
            Return (Left(pShipPrefDesc, 1) = "*")
        End Get
    End Property

    Friend Function VisitMark(ByVal iVal As Integer) As String
        pVisited = Left$(pVisited, iVal - 1) & "1" & Mid$(pVisited, iVal + 1)
    End Function

    Friend ReadOnly Property VisitAll() As Boolean
        Get
            Return (pVisited = "111111")
        End Get
    End Property
#End Region
    'Shipping
    'Session("Sh_FName") = ""
    'Session("Sh_LName") = ""
    'Session("Sh_Company") = ""
    'Session("Sh_Address1") = ""
    'Session("Sh_Address2") = ""
    'Session("Sh_City") = ""
    'Session("Sh_State") = ""
    'Session("Sh_ZipCode") = ""
    'Session("Sh_PrefID") = ""
    'Session("Sh_PrefName") = ""
    ''Cover Sheet
    'Session("Cv_ID") = ""
    'Session("Cv_Name") = ""
    'Session("Cv_Content") = ""

    Friend Sub Init()
        pCustomerEmail = String.Empty
        pProcessingChg = 0
        pMonthlyChg = 0
        pFreightMarkupChg = 0
        pLinePckChg = 0
        pLineKitChg = 0
        pMaxQtyPerLineItem = 0
        pMaxQtyPerOrder = 0
        pCanExtendQtyLI = False
        pCanExtendQtyOR = False

        pRequestorTitle = String.Empty
        pRequestorFirstName = String.Empty
        pRequestorLastName = String.Empty
        pRequestorEmail = String.Empty

        pShipContactFirstName = String.Empty
        pShipContactLastName = String.Empty
        pShipCompany = String.Empty
        pShipAddress1 = String.Empty
        pShipAddress2 = String.Empty
        pShipCity = String.Empty
        pShipState = String.Empty
        pShipCountry = String.Empty
        pShipZip = String.Empty
        pShipPrefID = 0
        pShipNote = String.Empty
        pCoverSheetText = String.Empty

        pID = 0
        pProblemMsg = "Order has not been submitted..."
        pProblemArea = 0
        pVisited = "000100" 'RSIKCC - do not have to visit kits - may not have permission
    End Sub

    Friend Sub ResetOrderNo()
        pID = 0
    End Sub

    Friend Sub InitCustoms(ByVal lCustID As Long, ByVal bCustomDetail As Boolean)
        CstCartPrep(lCustID, bCustomDetail)
    End Sub

    'mw - 07-15-2007 - want to remember custom fields between orders
    'Friend Sub InitDetails(ByVal lCustID As Long, ByVal bCustomDetail As Boolean)
    Friend Sub InitDetails(ByVal lCustID As Long)
        'Need here when starting another order
        pVisited = "00010" 'RSIKC - do not have to visit kits - may not have permission

        'CstCartPrep(lCustID, bCustomDetail)
        'TODO this is where items/kits added to new order
        ItmCartPrep()
        KitCartPrep()
    End Sub

    Private Sub CstCartPrep(ByVal sCustomerID As String, ByVal bCustomDetail As Boolean)
        Dim objDT As System.Data.DataTable
        Dim objDR As System.Data.DataRow

        objDT = New System.Data.DataTable("CstCart")
        objDT.Columns.Add("ID", GetType(Integer))
        objDT.Columns("ID").AutoIncrement = True
        objDT.Columns("ID").AutoIncrementSeed = 1

        objDT.Columns.Add("FieldID", GetType(Integer))
        objDT.Columns.Add("FieldRequire", GetType(Integer))
        objDT.Columns.Add("FieldEntry", GetType(Integer))
        objDT.Columns.Add("FieldName", GetType(String))
        objDT.Columns.Add("FieldValue", GetType(String))
        objDT.Columns.Add("UserEntry", GetType(String))
        objDT.Columns.Add("FieldType", GetType(String))


        Dim myData As paraData
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim dr As OleDb.OleDbDataReader
        Dim sSQL As String
        Dim indx As Integer
        Dim sID As String
        Dim sName As String
        Dim bRequire As Boolean
        Dim bEntry As Boolean
        Dim sEntry As String
        Dim iType As Int16

        myData = New paraData
        '    sSQL = "SELECT Customer_CustomField.ID, Customer_CustomField.Name, Customer_CustomField.Require, Customer_CustomField.DetailData, Customer_CustomField.Entry, Customer_CustomField.DetailData " & _
        '           "FROM Customer INNER JOIN Customer_CustomField ON Customer.ID = Customer_CustomField.CustomerID " & _
        '           "WHERE Customer_CustomField.CustomerID = " & sCustomerID & _
        'sSQL = "SELECT Customer_CustomField.ID, Customer_CustomField.Name, Customer_CustomField.Require, Customer_CustomField.DetailData, Customer_CustomField.Entry, Customer_CustomField.DetailData, iif(lkp.CustomFieldID is NULL,0,lkp.Cnt) AS SelectCnt " & _
        '       "FROM (Customer INNER JOIN Customer_CustomField ON Customer.ID = Customer_CustomField.CustomerID) LEFT JOIN " & _
        '       "(SELECT COUNT(ID) AS Cnt, CustomFieldID FROM Lkp_CustomField GROUP BY CustomFieldID) lkp " & _
        '       "ON Customer_CustomField.ID =lkp.CustomFieldID " & _
        '       "WHERE Customer_CustomField.CustomerID = " & sCustomerID
        sSQL = "SELECT Customer_CustomField.ID, Customer_CustomField.Name, Customer_CustomField.Require, Customer_CustomField.CustomType, Customer_CustomField.Entry, iif(lkp.CustomFieldID is NULL,0,lkp.Cnt) AS SelectCnt " & _
               "FROM (Customer INNER JOIN Customer_CustomField ON Customer.ID = Customer_CustomField.CustomerID) LEFT JOIN " & _
               "(SELECT COUNT(ID) AS Cnt, CustomFieldID FROM Lkp_CustomField GROUP BY CustomFieldID) lkp " & _
               "ON Customer_CustomField.ID =lkp.CustomFieldID " & _
               "WHERE Customer_CustomField.Active=1 AND Customer_CustomField.CustomerID = " & sCustomerID

        If (bCustomDetail = False) Then
            'sSQL = sSQL & " AND  Customer_CustomField.DetailData = False "
            '0-Detail / 1-Requestor / 2-Shipping
            sSQL = sSQL & " AND  Customer_CustomField.CustomType > 0 "
        End If
        'sSQL = sSQL & " ORDER BY DetailData"
        sSQL = sSQL & " ORDER BY Customer_CustomField.CustomType, Customer_CustomField.SeqNo, Customer_CustomField.Name"

        If (sSQL.Length > 0) Then
            myData.GetReader2(cnn, cmd, dr, sSQL)
            Do While dr.Read()
                indx = indx + 1

                sID = dr("ID").ToString()
                sName = dr("Name").ToString()
                bRequire = Convert.ToBoolean(dr("Require").ToString())
                bEntry = Convert.ToBoolean(dr("Entry").ToString())
                sEntry = IIf(dr("SelectCnt") > 0, "Select", "Type")
                iType = dr("CustomType")

                objDR = objDT.NewRow()
                objDR("FieldID") = sID
                objDR("FieldRequire") = bRequire
                objDR("FieldEntry") = bEntry
                objDR("FieldName") = sName
                objDR("FieldValue") = ""
                objDR("UserEntry") = sEntry
                objDR("FieldType") = iType

                objDT.Rows.Add(objDR)
            Loop
        End If
        pCurrentCstCart = objDT

        myData.ReleaseReader2(cnn, cmd, dr)
        myData = Nothing
    End Sub

    Private Sub ItmCartPrep()
        Dim objDT As System.Data.DataTable
        Dim objDR As System.Data.DataRow

        objDT = New System.Data.DataTable("ItmCart")
        objDT.Columns.Add("ID", GetType(Integer))
        objDT.Columns("ID").AutoIncrement = True
        objDT.Columns("ID").AutoIncrementSeed = 1

        'objDT.Columns.Add("ItmQty", GetType(Integer))
        objDT.Columns.Add("ItmQty", GetType(Long))
        objDT.Columns.Add("ItmID", GetType(String))
        objDT.Columns.Add("ItmName", GetType(String))
        objDT.Columns.Add("ItmDesc", GetType(String))
        objDT.Columns.Add("ItmColor", GetType(String))

        pCurrentItmCart = objDT
    End Sub

    Private Sub KitCartPrep()
        Dim objDT As System.Data.DataTable
        Dim objDR As System.Data.DataRow

        objDT = New System.Data.DataTable("KitCart")
        objDT.Columns.Add("ID", GetType(Integer))
        objDT.Columns("ID").AutoIncrement = True
        objDT.Columns("ID").AutoIncrementSeed = 1

        'objDT.Columns.Add("KitQty", GetType(Integer))
        objDT.Columns.Add("KitQty", GetType(Long))
        objDT.Columns.Add("KitID", GetType(String))
        objDT.Columns.Add("KitName", GetType(String))
        objDT.Columns.Add("KitDesc", GetType(String))
        objDT.Columns.Add("KitColor", GetType(String))

        pCurrentKitCart = objDT
    End Sub
   
    Friend Sub CstSave(ByVal lID As Long, ByVal sValue As String)
        Const COL_FLDID = 1
        Const COL_FLDVAL = 5

        Dim objDT As System.Data.DataTable
        Dim objDR As System.Data.DataRow
        Dim indx As Integer : indx = 0
        Dim iMaxIndx As Integer : iMaxIndx = 0
        Dim iRow As Integer : iRow = -1

        objDT = pCurrentCstCart

        iMaxIndx = objDT.Rows.Count - 1
        For indx = 0 To iMaxIndx
            If (lID = objDT.Rows(indx).Item(COL_FLDID)) Then
                iRow = indx
            End If
        Next indx

        If (lID > 0) And (iRow >= 0) Then

            objDT.Rows(iRow).Item(COL_FLDVAL) = sValue
        End If

        pCurrentCstCart = objDT

        objDT = Nothing
    End Sub

    Friend Sub ItmSave(ByVal lQty As Long, ByVal lID As Long, ByVal sName As String, ByVal sDesc As String, Optional ByVal sColor As String = "NORMAL")
        Const COL_ID = 0
        Const COL_QTY = 1
        Const COL_ITMID = 2
        Const COL_NAME = 3
        Const COL_DESC = 4
        Const COL_COLOR = 5

        Dim objDT As System.Data.DataTable
        Dim objDR As System.Data.DataRow
        Dim indx As Integer : indx = 0
        Dim iMaxIndx As Integer : iMaxIndx = 0
        Dim iRow As Integer : iRow = -1

        objDT = pCurrentItmCart
        'TODO itemid order
        iMaxIndx = objDT.Rows.Count - 1
        For indx = 0 To iMaxIndx
            If objDT.Rows(indx).RowState <> DataRowState.Deleted Then
                If (lID = objDT.Rows(indx).Item(COL_ITMID)) Then
                    iRow = indx
                End If
            End If

        Next indx

        'If (iQty <= 0) And (iRow >= 0) Then
        If (lQty <= 0) And (iRow >= 0) Then
            'Delete
            objDT.Rows(iRow).Delete()

            'ElseIf (lID > 0) And (iQty > 0) And (iRow < 0) Then
        ElseIf (lID > 0) And (lQty > 0) And (iRow < 0) Then
            'Add
            objDR = objDT.NewRow()
            'objDR.Item(COL_QTY) = iQty
            objDR.Item(COL_QTY) = lQty
            objDR.Item(COL_ITMID) = lID
            objDR.Item(COL_NAME) = sName
            objDR.Item(COL_DESC) = sDesc
            objDR.Item(COL_COLOR) = sColor
            objDT.Rows.Add(objDR)
            objDR = Nothing

        Else
            'Update
            'objDT.Rows(iRow).Item(COL_QTY) = iQty
            objDT.Rows(iRow).Item(COL_QTY) = lQty
        End If

        pCurrentItmCart = objDT

        objDT = Nothing
    End Sub

    Friend Sub KitSave(ByVal lQty As Long, ByVal lID As Long, ByVal sName As String, ByVal sDesc As String, Optional ByVal sColor As String = "NORMAL")
        Const COL_ID = 0
        Const COL_QTY = 1
        Const COL_KITID = 2
        Const COL_NAME = 3
        Const COL_DESC = 4
        Const COL_COLOR = 5

        Dim objDT As System.Data.DataTable
        Dim objDR As System.Data.DataRow
        Dim indx As Integer : indx = 0
        Dim iMaxIndx As Integer : iMaxIndx = 0
        Dim iRow As Integer : iRow = -1

        objDT = pCurrentKitCart

        iMaxIndx = objDT.Rows.Count - 1
        For indx = 0 To iMaxIndx
            If objDT.Rows(indx).RowState <> DataRowState.Deleted Then
                If (lID = objDT.Rows(indx).Item(COL_KITID)) Then
                    iRow = indx
                End If
            End If

        Next indx

        'If (iQty <= 0) And (iRow >= 0) Then
        If (lQty <= 0) And (iRow >= 0) Then
            'Delete
            objDT.Rows(iRow).Delete()

            'ElseIf (lID > 0) And (iQty > 0) And (iRow < 0) Then
        ElseIf (lID > 0) And (lQty > 0) And (iRow < 0) Then
            'Add
            objDR = objDT.NewRow()
            'objDR.Item(COL_QTY) = iQty
            objDR.Item(COL_QTY) = lQty
            objDR.Item(COL_KITID) = lID
            objDR.Item(COL_NAME) = sName
            objDR.Item(COL_DESC) = sDesc
            objDR.Item(COL_COLOR) = sColor
            objDT.Rows.Add(objDR)
            objDR = Nothing

        Else
            'Add
            'objDT.Rows(iRow).Item(COL_QTY) = iQty
            objDT.Rows(iRow).Item(COL_QTY) = lQty
        End If

        pCurrentKitCart = objDT

        objDT = Nothing
    End Sub

    Friend Function ItmCartString() As String
        Const COL_ItmID = 2

        Dim objDT As System.Data.DataTable
        Dim objDR As System.Data.DataRow
        Dim indx As Integer : indx = 0
        Dim iMaxIndx As Integer : iMaxIndx = 0
        Dim iRow As Integer : iRow = -1
        Dim lID As Long
        Dim sList As String = String.Empty

        pCurrentItmCart.AcceptChanges()
        objDT = pCurrentItmCart

        iMaxIndx = objDT.Rows.Count - 1
        For indx = 0 To iMaxIndx
            lID = objDT.Rows(indx).Item(COL_ItmID)
            If (sList.Length > 0) Then sList = sList & ","
            sList = sList & lID
        Next indx

        objDT = Nothing

        Return sList
    End Function

    Friend Function KitCartString() As String
        Const COL_KITID = 2

        Dim objDT As System.Data.DataTable
        Dim objDR As System.Data.DataRow
        Dim indx As Integer : indx = 0
        Dim iMaxIndx As Integer : iMaxIndx = 0
        Dim iRow As Integer : iRow = -1
        Dim lID As Long
        Dim sList As String = String.Empty

        pCurrentKitCart.AcceptChanges()
        objDT = pCurrentKitCart

        iMaxIndx = objDT.Rows.Count - 1
        For indx = 0 To iMaxIndx
            lID = objDT.Rows(indx).Item(COL_KITID)
            If (sList.Length > 0) Then sList = sList & ","
            sList = sList & lID
        Next indx

        objDT = Nothing

        Return sList
    End Function

    Friend Function IsValid(ByVal DisableAllQuantityLimits As Boolean, Optional ByRef blnVisible As Boolean = True) As Boolean
        Dim bValid As Boolean = True
        'Dim iLIMax As Integer = 0
        'Dim iLIQty As Integer = 0
        Dim lLIMax As Long = 0
        Dim lLIQty As Long = 0
        Dim bCustomFieldsOK As Boolean = False
        Dim sField As String = ""
        Dim iPA As Integer = 0
        'Private Const PRO_REQ = 0
        'Private Const PRO_SHP = 1
        'Private Const PRO_ITM = 2
        'Private Const PRO_KIT = 3
        'Private Const PRO_CST = 4

        pProblemMsg = ""
        pProblemArea = PRO_REQ
        Try
            'Requestor
            If bValid Then
                bValid = bValid And (pRequestorFirstName.Length > 0)
                If Not bValid Then
                    pProblemMsg = "Requestor Information"
                    pProblemArea = PRO_REQ
                End If
            End If
            If bValid Then
                bValid = bValid And (pRequestorLastName.Length > 0)
                If Not bValid Then
                    pProblemMsg = "Requestor Information"
                    pProblemArea = PRO_REQ
                End If
            End If
            If bValid Then
                bValid = bValid And (pRequestorEmail.Length > 0)
                If Not bValid Then
                    pProblemMsg = "Requestor Information"
                    pProblemArea = PRO_REQ
                End If
            End If
            'Shipping
            If bValid Then
                bValid = bValid And (pShipContactFirstName.Length > 0)
                If Not bValid Then
                    pProblemMsg = "Ship To Information"
                    pProblemArea = PRO_SHP
                End If
            End If
            If bValid Then
                bValid = bValid And (pShipContactLastName.Length > 0)
                If Not bValid Then
                    pProblemMsg = "Ship To Information"
                    pProblemArea = PRO_SHP
                End If
            End If
            'If bValid Then
            '  bValid = bValid And (pShipCompany.Length > 0)
            '  If Not bValid Then
            '    pProblemMsg = "Ship To Information"
            '    pProblemArea = PRO_SHP
            '  End If
            'End If
            If bValid Then
                bValid = bValid And (pShipAddress1.Length > 0)
                If Not bValid Then
                    pProblemMsg = "Validate Check Error"
                    pProblemArea = PRO_SHP
                End If
            End If
            If bValid Then
                bValid = bValid And (pShipCity.Length > 0)
                If Not bValid Then
                    pProblemMsg = "Ship To Information"
                    pProblemArea = PRO_SHP
                End If
            End If
            If bValid Then
                bValid = bValid And (pShipState.Length > 0)
                If Not bValid Then
                    pProblemMsg = "Ship To Information"
                    pProblemArea = PRO_SHP
                End If
            End If
            If bValid Then
                bValid = bValid And (pShipZip.Length > 0)
                If Not bValid Then
                    pProblemMsg = "Ship To Information"
                    pProblemArea = PRO_SHP
                End If
            End If
            If bValid Then
                bValid = bValid And (pShipCountry.Length > 0)
                If Not bValid Then
                    pProblemMsg = "Ship To Information"
                    pProblemArea = PRO_SHP
                End If
            End If
            If bValid Then
                bValid = bValid And (pShipPrefID > 0)
                If Not bValid Then
                    pProblemMsg = "Ship To Information"
                    pProblemArea = PRO_SHP
                End If
            End If
            'Cover Sheet
            If bValid Then
                bValid = bValid And (pCoverSheetID > 0)
                If Not bValid Then
                    pProblemMsg = "Cover Sheet Information"
                    pProblemArea = PRO_CST
                End If
            End If
            'Check Custom Field Requirements
            If bValid Then
                bCustomFieldsOK = GetCustomFieldsValid(sField, iPA, pShipPrefID)
                bValid = bValid And bCustomFieldsOK
                If Not bValid Then
                    pProblemMsg = "Custom Field Entries required... " & sField
                    pProblemArea = iPA 'PRO_CST
                End If
            End If
            'Check Quantities per line item
            If bValid Then
                'GetOrderQty(iLIMax, iLIQty)


                GetOrderQty(lLIMax, lLIQty)
                If DisableAllQuantityLimits = False Then
                    Dim blnIdividual As Boolean

                    bValid = bValid And (ValidateLineItemsInCart(pMaxQtyPerLineItem, blnIdividual))

                    'bValid = bValid And ((pMaxQtyPerLineItem >= lLIMax) Or _
                    '                     ((pMaxQtyPerLineItem < lLIMax) And pCanExtendQtyLI))
                    If Not bValid Then
                        'pProblemMsg = "Exceeded Limits on Maximum Line Item Limit (" & iLIMax.ToString() & ")"
                        If blnIdividual = False Then
                            pProblemMsg = "Exceeded Limits on Maximum Line Item Limit (Limit = " & pMaxQtyPerLineItem.ToString() & ", Selection = " & lLIMax.ToString() & ")"
                        End If
                        blnVisible = False
                        pProblemArea = PRO_ITM
                    End If
                Else
                    bValid = True
                End If

            End If

            'Check Quantities per order
            If bValid Then





                bValid = bValid And ((pMaxQtyPerOrder >= lLIQty) Or _
                                     ((pMaxQtyPerOrder < lLIQty) And pCanExtendQtyOR))
                If DisableAllQuantityLimits = False Then
                    If Not bValid Then
                        'TODO exceeded pieces per order
                        blnVisible = False
                        pProblemMsg = "Exceeded Limits on Maximum Per Order Limit (Limit = " & pMaxQtyPerOrder.ToString() & ", Selection = " & lLIQty.ToString() & ")"
                        pProblemArea = PRO_ITM
                    End If
                Else
                    bValid = True
                End If
            End If
            ' Mike Mike Mike Mike
            'At least one item ordered
            If bValid Then
                'bValid = bValid And (iLIMax > 0)
                bValid = bValid And (lLIMax > 0)
                If Not bValid Then
                    pProblemMsg = "No Items Selected..."
                    pProblemArea = PRO_ITM
                End If
            End If

            'Show Messages to user on Quantity change screens so they are aware of 
            '1-inactive items in kits
            '2-backorder items in kits or items

        Catch ex As Exception
            'Page Message Here for Status to user...
        End Try

        Return bValid
    End Function
    Function ValidateLineItemsInCart(ByVal intMaxPerLineItem As Integer, ByRef blnIdividual As Boolean) As Boolean
        Dim bln As Boolean = True
        If pCanExtendQtyLI = False Then
            Const COL_QTY = 1

            Dim objDT As System.Data.DataTable
            Dim indx As Integer : indx = 0
            Dim iMaxIndx As Integer : iMaxIndx = 0
            'Dim iQty As Integer = 0
            Dim lQty As Long = 0
            Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
            blnIdividual = False

            objDT = pCurrentItmCart
            iMaxIndx = objDT.Rows.Count - 1
            'CHECKING for line item max
            For indx = 0 To iMaxIndx
                If objDT.Rows(indx).RowState <> DataRowState.Deleted Then
                    'iQty = objDT.Rows(indx).Item(COL_QTY)
                    'lLIQty = lLIQty + iQty
                    'If (iQty > lLIMax) Then lLIMax = iQty
                    lQty = objDT.Rows(indx).Item(COL_QTY)
                    Dim intMaxPerItem As Integer = qa.GetItemOrderQuantityLimit(objDT.Rows(indx).Item(2))

                    If (lQty > intMaxPerLineItem And intMaxPerLineItem > intMaxPerItem) Or (lQty > intMaxPerItem And intMaxPerItem > 0) Then

                        If intMaxPerItem > intMaxPerLineItem Then
                            blnIdividual = True
                        End If

                        Return False
                    End If
                End If
            Next indx
            objDT = Nothing

            objDT = pCurrentKitCart
            iMaxIndx = objDT.Rows.Count - 1

            'checking for order total max:
            For indx = 0 To iMaxIndx
                'iQty = objDT.Rows(indx).Item(COL_QTY)
                'lLIQty = lLIQty + iQty
                'If (iQty > lLIMax) Then lLIMax = iQty
                If objDT.Rows(indx).RowState <> DataRowState.Deleted Then
                    lQty = objDT.Rows(indx).Item(COL_QTY)

                    If (lQty > intMaxPerLineItem) Then 'Or lQty > qa.GetItemOrderQuantityLimit(objDT.Rows(indx).Item(2))
                        Return False
                    End If
                End If

            Next indx
            objDT = Nothing
        End If

        Return bln
    End Function
    Private Function GetCustomFieldsValid(ByRef sField As String, ByRef iProbArea As Integer, intShipMethod As Integer) As Boolean
        Const COL_REQ = 2
        Const COL_NAM = 4
        Const COL_VAL = 5
        Const COL_TYP = 7

        Const CST_DET = 0
        Const CST_REQ = 1
        Const CST_SHP = 2

        Dim objDT As System.Data.DataTable
        Dim indx As Integer : indx = 0
        Dim iMaxIndx As Integer : iMaxIndx = 0
        Dim bReq As Boolean = False
        Dim sName As String = ""
        Dim sValue As String = ""
        Dim bValid As Boolean = True

        objDT = pCurrentCstCart
        iMaxIndx = objDT.Rows.Count - 1


        For indx = 0 To iMaxIndx
            bReq = objDT.Rows(indx).Item(COL_REQ)
            sName = objDT.Rows(indx).Item(COL_NAM)
            sValue = objDT.Rows(indx).Item(COL_VAL)

            If CheckIfCustomItemShows(intShipMethod, objDT.Rows(indx).Item(1)) Then
                If (bReq = True) And (sValue.Length = 0) Then
                    sField = sName
                    bValid = False
                    If (objDT.Rows(indx).Item(COL_TYP) = CST_DET) Then
                        iProbArea = PRO_CST
                    ElseIf (objDT.Rows(indx).Item(COL_TYP) = CST_REQ) Then
                        iProbArea = PRO_REQ
                    ElseIf (objDT.Rows(indx).Item(COL_TYP) = CST_SHP) Then
                        iProbArea = PRO_SHP
                    End If
                End If
            End If


        Next indx
        objDT = Nothing
        Return bValid
    End Function

    Private Sub GetOrderQty(ByRef lLIMax As Long, ByRef lLIQty As Long)
        lLIMax = 0
        lLIQty = 0

        Const COL_QTY = 1

        Dim objDT As System.Data.DataTable
        Dim indx As Integer : indx = 0
        Dim iMaxIndx As Integer : iMaxIndx = 0
        'Dim iQty As Integer = 0
        Dim lQty As Long = 0
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter

        objDT = pCurrentItmCart
        iMaxIndx = objDT.Rows.Count - 1
        'CHECKING for line item max
        For indx = 0 To iMaxIndx
            If objDT.Rows(indx).RowState <> DataRowState.Deleted Then
                'iQty = objDT.Rows(indx).Item(COL_QTY)
                'lLIQty = lLIQty + iQty
                'If (iQty > lLIMax) Then lLIMax = iQty
                lQty = objDT.Rows(indx).Item(COL_QTY)
                lLIQty = lLIQty + lQty
                If (lQty > lLIMax) Then lLIMax = lQty 'And qa.GetItemOrderQuantityLimit(objDT.Rows(indx).Item(2)) <= lLIMax
            End If

        Next indx
        objDT = Nothing

        objDT = pCurrentKitCart
        iMaxIndx = objDT.Rows.Count - 1

        'checking for order total max:
        For indx = 0 To iMaxIndx
            'iQty = objDT.Rows(indx).Item(COL_QTY)
            'lLIQty = lLIQty + iQty
            'If (iQty > lLIMax) Then lLIMax = iQty
            If objDT.Rows(indx).RowState <> DataRowState.Deleted Then
                lQty = objDT.Rows(indx).Item(COL_QTY)
                lLIQty = lLIQty + lQty
                If (lQty > lLIMax) Then lLIMax = lQty ' And qa.GetItemOrderQuantityLimit(objDT.Rows(indx).Item(2)) <= lLIMax
            End If

        Next indx
        objDT = Nothing
    End Sub

    'Check Qtys
    Friend Sub CurrentAuthorizationStatus(ByRef bRequiresLI As Boolean, ByRef bRequiresOR As Boolean, ByVal blnDisalbeAllLimits As Boolean, Optional ByVal intMaxQtyPerLineItem As Integer = -1, Optional ByVal intMaxQtyPerOrder As Integer = -1)
        Dim lMaxQtyLI As Long = 0
        Dim lMaxQtyOR As Long = 0
        If blnDisalbeAllLimits = False Then
            'TODO pass pmax related to the order if editing 
            GetOrderQty(lMaxQtyLI, lMaxQtyOR)
            If intMaxQtyPerLineItem > 0 And intMaxQtyPerOrder > 0 Then
                bRequiresLI = (lMaxQtyLI > intMaxQtyPerLineItem) And pCanExtendQtyLI
                bRequiresOR = (lMaxQtyOR > intMaxQtyPerOrder) And pCanExtendQtyOR
            Else
                bRequiresLI = (lMaxQtyLI > pMaxQtyPerLineItem) And pCanExtendQtyLI
                bRequiresOR = (lMaxQtyOR > pMaxQtyPerOrder) And pCanExtendQtyOR
            End If

        Else
            bRequiresLI = False
            bRequiresOR = False

        End If

    End Sub

    Friend Function Submit(ByVal oUser As paraUser, ByRef sMsg As String, ByVal strSessionID As String) As Boolean
        Dim bSuccess As Boolean : bSuccess = False
        'TODO this is where order submit
        Try
            bSuccess = OrderCreate(oUser, sMsg, strSessionID)
            If bSuccess Then
                InitDetails(oUser.CustomerID)
            Else

                sMsg = " Unable to Create Order...  " & sMsg
            End If
        Catch ex As Exception
            sMsg = " Unable to Create Order...  " & ex.InnerException.ToString
        End Try

        Return bSuccess
    End Function
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Order Save Section - Start
    Private Function OrderCreate(ByRef myUser As paraUser, ByRef sMsg As String, ByVal strPath As String) As Boolean
        Dim dbAccessCodeID As Long = 0
        Dim dbProcessingChg As Single = 0
        Dim dbMonthlyChg As Single = 0
        Dim dbFreightMarkupChg As Single = 0
        Dim dbCoverSheet_ID As Long
        Dim dbCoverSheet_Content As String = String.Empty
        Dim dbRequestor_Title As String = String.Empty
        Dim dbRequestor_FirstName As String = String.Empty
        Dim dbRequestor_LastName As String = String.Empty
        Dim dbRequestor_Email As String = String.Empty
        Dim dbShipTo_ContactFirstName As String = String.Empty
        Dim dbShipTo_ContactLastName As String = String.Empty
        Dim dbShipTo_Name As String = String.Empty
        Dim dbShipTo_Address1 As String = String.Empty
        Dim dbShipTo_Address2 As String = String.Empty
        Dim dbShipTo_City As String = String.Empty
        Dim dbShipTo_State As String = String.Empty
        Dim dbShipTo_ZipCode As String = String.Empty
        Dim dbShipTo_Country As String = String.Empty
        Dim dbShipTo_ShipPreferred_ID As Long = 0
        Dim dbShipTo_Note As String = String.Empty
        Dim dbShipTo_HdChg As String = 0
        Dim dbRecipientEmail As String = String.Empty
        Dim dbRecipientPhone As String = String.Empty
        Dim dbSendRecipientEmail As Boolean = False

        Dim myData As paraData
        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String
        Dim cnnE As OleDb.OleDbConnection
        Dim cmdE As OleDb.OleDbCommand
        Dim sSQLE As String
        Dim cnSql As SqlConnection

        'Dim sResult As String = String.Empty
        Dim bSuccess As Boolean
        Dim bRequiresAuthorization As Boolean = False

        Dim lOrderID As Long = 0
        Dim sCurrentTime As String = String.Empty
        Dim sErrMsg As String = String.Empty
        Dim iTry As Integer = 0


        '    moLog = New Log
        msLogFile = ConfigurationSettings.AppSettings("LogFolder") & "Ordr-" & Format(Now, "yyyyMMdd") & ".log"

        Try
            'moLog.Entry(msLogFile, "  OrderCreate Start...  " & Now & moLog.NewLine)
            'moLog.Entry(msLogFile, "    Base Order Start..." & moLog.NewLine)
            LogActivity("  OrderCreate Start...  ", msLogFile)
            LogActivity("    Base Order Start...", msLogFile)

            dbAccessCodeID = myUser.AccessCodeID
            dbProcessingChg = pProcessingChg
            dbMonthlyChg = pMonthlyChg
            dbFreightMarkupChg = pFreightMarkupChg

            'Gather Cover Sheet - Begin
            dbCoverSheet_ID = pCoverSheetID
            dbCoverSheet_Content = pCoverSheetText
            'Gather Cover Sheet - End

            'Gather Requestor - Begin
            dbRequestor_Title = pRequestorTitle
            dbRequestor_FirstName = pRequestorFirstName
            dbRequestor_LastName = pRequestorLastName
            dbRequestor_Email = pRequestorEmail
            'Gather Requestor - End

            'Gather Ship To - Begin
            dbShipTo_ContactFirstName = pShipContactFirstName
            dbShipTo_ContactLastName = ShipContactLastName
            dbShipTo_Name = ShipCompany
            dbShipTo_Address1 = ShipAddress1
            dbShipTo_Address2 = ShipAddress2
            dbShipTo_City = ShipCity
            dbShipTo_State = ShipState
            dbShipTo_ZipCode = ShipZip
            dbShipTo_Country = ShipCountry
            dbShipTo_Note = ShipNote
            dbShipTo_ShipPreferred_ID = ShipPrefID
            dbRecipientEmail = RecipientEmail
            dbRecipientPhone = RecipientPhone
            'mw - 08-13-2008 - TO ADD for Cost Model Changes
            dbShipTo_HdChg = 0
            dbSendRecipientEmail = SendRecipientConfirmEmail
            'Gather Ship To - End

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            LogActivity("      Base Variables Loaded...", msLogFile)

            'Create Order Record
            myData = New paraData
            myData.GetConnection(cnnE)
            myData.GetConnectionSQL(cnSql)
            If Not (cnnE Is Nothing) Then
                LogActivity("    Connected...", msLogFile)
                If (dbAccessCodeID > 0) Then
                    LogActivity("    Access Code ID:  " & dbAccessCodeID.ToString, msLogFile)
                    sCurrentTime = Now()
                    'mw - 08-13-2008 - TO ADD for Cost Model Changes
                    dbShipTo_HdChg = GetShipHandleCharge(myData, dbShipTo_ShipPreferred_ID, sErrMsg)
                    Dim strGUIDTMP As String = Guid.NewGuid().ToString
                    sSQLE = "INSERT INTO [Order] (StatusID, AccessCodeID, RequestDate, Requestor_Title, Requestor_FirstName, Requestor_LastName, Requestor_Email, " & _
                          "CoverSheetID, CS_Content, ProcessingChg, MonthlyChg, FreightMarkupChg, PreferredShipID, ActualShipID, ShipHdChg, ShipNote, " & _
                          "ShipTo_Name, ShipTo_Address1, ShipTo_Address2, ShipTo_City, ShipTo_State, ShipTo_ZipCode, ShipTo_Country, ShipTo_ContactFirstName, ShipTo_ContactLastName, ModifiedDate,TempGUID,RecipientPhone,RecipientEmail ,SendRecipientConfirmEmail ) " & _
                          "VALUES (" & myData.STATUS_HOLD & ", " & dbAccessCodeID & ", '" & sCurrentTime & "', " & myData.SQLString(dbRequestor_Title) & ", " & myData.SQLString(dbRequestor_FirstName) & ", " & myData.SQLString(dbRequestor_LastName) & ", " & myData.SQLString(dbRequestor_Email) & ", " & _
                          dbCoverSheet_ID & ", " & myData.SQLString(dbCoverSheet_Content) & ", " & dbProcessingChg & ", " & dbMonthlyChg & ", " & dbFreightMarkupChg & ", " & dbShipTo_ShipPreferred_ID & ", " & dbShipTo_ShipPreferred_ID & ", " & dbShipTo_HdChg & ", " & myData.SQLString(dbShipTo_Note) & ", " & _
                          myData.SQLString(dbShipTo_Name) & ", " & myData.SQLString(dbShipTo_Address1) & ", " & myData.SQLString(dbShipTo_Address2) & ", " & myData.SQLString(dbShipTo_City) & ", " & myData.SQLString(dbShipTo_State) & ", " & _
                          myData.SQLString(dbShipTo_ZipCode) & ", " & myData.SQLString(dbShipTo_Country) & ", " & myData.SQLString(dbShipTo_ContactFirstName) & ", " & myData.SQLString(dbShipTo_ContactLastName) & ",'" & Now() & "','" & strGUIDTMP & "', " & myData.SQLString(dbRecipientPhone) & ", " & myData.SQLString(dbRecipientEmail) & ", " & myData.SQLString(dbSendRecipientEmail) & ") "

                    If (cnSql Is Nothing) Then myData.GetConnection(cnnE)
                    myData.SQLExecuteSQL(cnSql, sSQLE, sErrMsg)
                    myData.ReleaseConnection(cnnE)
                    LogActivity("      Inserted Base Record:  " & sSQLE, msLogFile)
                    LogActivity("        Message:  " & sErrMsg, msLogFile)

                    cnSql.Close()



                    'sSQLR = "SELECT ID AS OrderID FROM [Order] WHERE ((AccessCodeID = " & dbAccessCodeID & ") AND (RequestDate = #" & sCurrentTime & "#))"

                    sSQLR = "SELECT ID AS OrderID FROM [Order] WHERE ((AccessCodeID = " & dbAccessCodeID & ") AND (TempGUID = '" & strGUIDTMP & "'))"

                    LogActivity("    Reading Order ID...  " & dbAccessCodeID.ToString & " AND " & sCurrentTime, msLogFile)
                    LogActivity("      " & sSQLR, msLogFile)

                    myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
                    If rcdR.Read() Then
                        lOrderID = rcdR("OrderID")
                        LogActivity("      Located Order ID...  " & lOrderID.ToString, msLogFile)
                    Else
                        LogActivity("      Unable to locate new Order after " & iTry.ToString & " tries ...  " & dbAccessCodeID.ToString & " AND " & sCurrentTime, msLogFile)
                    End If
                    myData.ReleaseReader2(cnnR, cmdR, rcdR)
                End If


                If (lOrderID = 0) And Not (cnnE Is Nothing) Then
                    LogActivity("      Unable to Locate Order ...  " & lOrderID.ToString(), msLogFile)
                ElseIf (lOrderID > 0) Then
                    'sResult = "Saved"
                    LogActivity("      Order ID:  " & lOrderID, msLogFile)
                    LogActivity("    Base Order End", msLogFile)

                    LogActivity("    Saving Custom Start...", msLogFile)
                    bSuccess = OrderCreate_Custom(cnnE, myData, lOrderID, sMsg)
                    LogActivity("    Saved Custom End " & bSuccess, msLogFile)

                    'try few times to insert order items
                    Dim i As Integer = 0

                    For i = 0 To 5

                        LogActivity("    Saving Items Start...", msLogFile)
                        bSuccess = OrderCreate_Itms(cnnE, myData, lOrderID, sMsg)
                        LogActivity("    Saved Items End " & bSuccess, msLogFile)


                        If bSuccess Then Exit For
                    Next

                    If bSuccess Then
                        For i = 0 To 5

                            LogActivity("    Saving Kits Start...", msLogFile)

                            bSuccess = OrderCreate_Kits(cnnE, myData, lOrderID, sMsg)
                            LogActivity("    Saved Kits End " & bSuccess, msLogFile)

                            If bSuccess Then Exit For
                        Next

                    End If

                    If bSuccess Then
                        bRequiresAuthorization = AuthorizationCheck(myData, lOrderID, myUser)
                        LogActivity("    Requires Authorization:  " & bRequiresAuthorization.ToString, msLogFile)
                    End If

                    If bSuccess And Not (bRequiresAuthorization) Then
                        OrderActivate(cnnE, myData, lOrderID)
                        LogActivity("    Order Activated:  " & (Not bRequiresAuthorization).ToString, msLogFile)
                    End If

                 
                    'TODO update order status to FUT if required date is over 
                    UpdateRequiredShipDate(myUser, lOrderID)

                    Dim qa As New dsNotificationsTableAdapters.QueriesTableAdapter

                    'Note:  ....need to send confirmation email out here
                    If myUser.SendConfirmationEmail And bSuccess = True AndAlso CBool(qa.CheckForOrderPlacementNotify(myUser.CustomerID, 4)) Then
                        ConfirmationCheck(myData, lOrderID, bRequiresAuthorization, dbRequestor_Email, myUser.DisableAllQuantityLimits)
                    End If


                    If CheckIfHasDownloadItems(pCurrentItmCart) Then
                        SendDownloadLinks(ActivateDownloadableItems(lOrderID, myUser.AccessCodeID), lOrderID, dbRequestor_Email)

                        'update order status to partial if there is more items to be fulfilled.
                        If Not CheckForDownloadOnly(pCurrentItmCart) Then
                            OrderPartial(cnnE, myData, lOrderID)
                        End If

                    End If


                    SendNewOrderNotifications(myUser, myData, lOrderID, bRequiresAuthorization)
                End If

                LogActivity("  OrderCreate End... " & bSuccess & "" & Now & msNewLine, msLogFile)
            End If
        Catch ex As Exception
            'Page Message Here for Status to user...
            LogActivity("  ERROR:  " & msNewLine & ex.Message.ToString & msNewLine, msLogFile)
            LogError("OrderCreate >> " & ex.Message.ToString, msLogFile)
            myData.ReleaseConnection(cnnE)


            If lOrderID > 0 Then
                Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
                DeleteOrder(lOrderID)
            End If


        End Try
        myData.ReleaseConnection(cnnE)
        myData = Nothing

        LogActivity("  Closing Connection", msLogFile)
        LogActivity("  OrderID = " & lOrderID.ToString, msLogFile)
        LogActivity("  OrderCreate >> Success ? " & bSuccess.ToString & msNewLine, msLogFile)

        pID = lOrderID
        If lOrderID > 0 And bSuccess = True Then
            UpdatePONumber(lOrderID)
            UpdateUploadedFolder(lOrderID, strPath, myUser)
        End If

        If lOrderID > 0 And bSuccess = False Then
            Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
            DeleteOrder(lOrderID)

        End If




        Return bSuccess
    End Function

    Sub SendNewOrderAuthorizationNotifications(ByRef myUser As paraUser, ByVal myData As paraData, ByVal lOrderID As Long, ByVal sSubject As String, ByVal sBody As String)
        Dim o As New paraOrder
        Dim msLogFile As String
        msLogFile = ConfigurationSettings.AppSettings("LogFolder") & "Ordr-" & Format(Now, "yyyyMMdd") & ".log"
        o.LogActivity("  SendNewOrderAuthorizationNotifications for order# " & lOrderID, msLogFile)

        '1	Primary Customer Contact
        '2	Secondary Customer Contact
        '3	Billing Group Contact
        '4: Requestor()
        '5	Para Sales Contact
        Dim myMail As Mail
        myMail = New Mail
        Dim strEmailTo As String = ""

        Dim ta As New dsNotificationsTableAdapters.DataTable1TableAdapter
        For Each dr As dsNotifications.DataTable1Row In ta.GetDataByCustomerID(myUser.CustomerID).Rows



            Select Case dr.ContactType
                Case 1
                    If dr.AuthorizationRequestNotify Then

                        strEmailTo = GetEmailForNotificationType(dr.ContactType, myUser.CustomerID)
                        o.LogActivity("    Send Auth Notifications -  Primary - " & strEmailTo, msLogFile)

                        myMail.SendMessage(strEmailTo, ConfigurationSettings.AppSettings("DefaultEmailAddress"), _
                                  sSubject, sBody, "", ConfigurationSettings.AppSettings("DefaultEmailCopy"), "", "", True)

                    End If
                Case 2
                    If dr.AuthorizationRequestNotify Then

                        strEmailTo = GetEmailForNotificationType(dr.ContactType, myUser.CustomerID)
                        o.LogActivity("  Send Auth Notifications - Secondary - " & strEmailTo, msLogFile)
                        myMail.SendMessage(strEmailTo, ConfigurationSettings.AppSettings("DefaultEmailAddress"), _
                                 sSubject, sBody, "", ConfigurationSettings.AppSettings("DefaultEmailCopy"), "", "", True)
                    End If
                Case 3
                Case 4
                Case 5



                    If dr.AuthorizationRequestNotify Then
                        strEmailTo = GetEmailForNotificationType(dr.ContactType, myUser.CustomerID)
                        o.LogActivity("  Send Auth Notifications - Para Sales Contact - " & strEmailTo, msLogFile)
                        myMail.SendMessage(strEmailTo, ConfigurationSettings.AppSettings("DefaultEmailAddress"), _
                                  sSubject, sBody, "", ConfigurationSettings.AppSettings("DefaultEmailCopy"), "", "", True)


                    End If
            End Select

        Next

        myMail = Nothing
    End Sub

    Sub SendNewOrderNotifications(ByRef myUser As paraUser, ByVal myData As paraData, ByVal lOrderID As Long, ByVal bRequiresAuthorization As Boolean)

        LogActivity("  SendNewOrderNotifications for order# " & lOrderID, msLogFile)

        '1	Primary Customer Contact
        '2	Secondary Customer Contact
        '3	Billing Group Contact
        '4: Requestor()
        '5	Para Sales Contact


        Dim strEmailTo As String = ""

        Dim ta As New dsNotificationsTableAdapters.DataTable1TableAdapter
        For Each dr As dsNotifications.DataTable1Row In ta.GetDataByCustomerID(myUser.CustomerID).Rows
            Select Case dr.ContactType
                Case 1
                    If dr.OrderPlacementNotify Then

                        strEmailTo = GetEmailForNotificationType(dr.ContactType, myUser.CustomerID)
                        LogActivity("  SendNewOrderNotifications - Primary - " & strEmailTo, msLogFile)
                        ConfirmationCheck(myData, lOrderID, bRequiresAuthorization, strEmailTo, myUser.DisableAllQuantityLimits)
                    End If
                Case 2
                    If dr.OrderPlacementNotify Then
                        LogActivity("  SendNewOrderNotifications - Secondary - " & strEmailTo, msLogFile)
                        strEmailTo = GetEmailForNotificationType(dr.ContactType, myUser.CustomerID)
                        ConfirmationCheck(myData, lOrderID, bRequiresAuthorization, strEmailTo, myUser.DisableAllQuantityLimits)
                    End If
                Case 3
                Case 4
                Case 5
                    If dr.OrderPlacementNotify Then
                        LogActivity("  SendNewOrderNotifications - Para Sales Contact - " & strEmailTo, msLogFile)
                        strEmailTo = GetEmailForNotificationType(dr.ContactType, myUser.CustomerID)
                        ConfirmationCheck(myData, lOrderID, bRequiresAuthorization, strEmailTo, myUser.DisableAllQuantityLimits)
                    End If
            End Select

        Next

        'send confirmation email to recipient:
        If SendRecipientConfirmEmail Then
            LogActivity("  SendNewOrderNotifications - Recipient Email" & strEmailTo, msLogFile)
            ConfirmationCheck(myData, lOrderID, bRequiresAuthorization, RecipientEmail, myUser.DisableAllQuantityLimits)
        End If
    End Sub

    Function GetEmailForNotificationType(ByVal intNotificationType As Integer, ByVal intCustomerID As Integer) As String
        '1	Primary Customer Contact
        '2	Secondary Customer Contact
        '3	Billing Group Contact
        '4: Requestor()
        '5	Para Sales Contact

        Dim ta As New dsCustomerTableAdapters.CustomerTableAdapter
        Dim dr As dsCustomer.CustomerRow = ta.GetByCustomerID(intCustomerID).Rows(0)
        Select Case intNotificationType
            Case 1
                Return dr.Email
            Case 2
                Return dr.SecondaryContact
            Case 5
                Return dr.StockWarnEmail
        End Select
    End Function
    Private Sub UpdateUploadedFolder(ByVal intOrderNumber As Integer, ByVal strPath As String, ByVal user As paraUser)


        If Directory.Exists(strPath) Then
            If Directory.Exists(ConfigurationManager.AppSettings("FtpFolder").ToString & intOrderNumber & "\") = False Then
                Directory.CreateDirectory(ConfigurationManager.AppSettings("FtpFolder").ToString & intOrderNumber & "\")
            End If


            'send email:
            Dim sEmailAuthor As String = String.Empty
            Dim sEmailCopy As String = String.Empty
            Dim sSubject As String = String.Empty
            Dim sBody As String = String.Empty
            Dim myMail As Mail
            Dim strTo As String = "" 'TODO find to email
            Dim iCount As Integer = 0

            Dim ta As New dsCustomerTableAdapters.CustomerTableAdapter
            For Each dr As dsCustomer.CustomerRow In ta.GetByCustomerID(user.CustomerID).Rows
                strTo = dr.StockWarnEmail
            Next

            'Try

            'Message - Start
            myMail = New Mail

            sSubject = "D3 Upload Request# " & intOrderNumber

            sBody = "<table><TR>" & _
                        "<TD>Date Submitted</td><td>" & Now.ToString & "</td></tr>" & _
                        "<TR><TD>Request #</td><td>" & intOrderNumber & "</td></tr>" & _
                        "<TR><TD>Requestor Name</td><td>" & RequestorFirstName & " " & RequestorLastName & "</td></tr>" & _
                        "<TR><TD>Requestor Email</td><td>" & RequestorEmail & "</td></tr>" & _
                        "<TR><TD colspan=2>A file has been uploaded for production of</td></tr>"            '

            Dim f1() As String = Directory.GetFiles(strPath)
            For i As Integer = 0 To UBound(f1)
                sBody = sBody & "<tr><TD>" & Path.GetFileNameWithoutExtension(f1(i)) & "<TD><TD>" & fileNameWithoutThePath(f1(i)) & "</td></tr>"
                iCount += 1
            Next


            sBody = sBody & "<TR><TD>\\Ftp\ftppub\d3\" & intOrderNumber & "\</td></tr><table>"

            If iCount > 0 Then
                myMail.SendMessage(strTo, ConfigurationSettings.AppSettings("DefaultEmailAddress"), _
                                             sSubject, sBody, sEmailCopy, ConfigurationSettings.AppSettings("DefaultEmailCopy"), , , True)
                myMail = Nothing

                Dim f() As String = Directory.GetFiles(strPath)
                For i As Integer = 0 To UBound(f)
                    File.Move(f(i), ConfigurationManager.AppSettings("FtpFolder").ToString & intOrderNumber & "\" & fileNameWithoutThePath(f(i)))
                Next
            End If


            'Directory.Delete(strPath)


            'Catch ex As Exception
            '    LogError("UpdateUploadedFolder >> " & ex.Message.ToString & msNewLine)
            'End Try

        End If


    End Sub

    Private Function fileNameWithoutThePath(ByVal b As String) As String
        Dim j As Int16

        j = Convert.ToInt16(b.LastIndexOf("\"))
        Return b.Substring(j + 1)

    End Function
    Private Sub UpdateRequiredShipDate(ByVal myUser As paraUser, ByVal intOrderID As Integer)
        Dim myData As paraData
        Dim intPShowpScheduledDelivery As Integer = 0
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        Dim ta As New dsAccessCodesTableAdapters.Customer_AccessCode1TableAdapter

        For Each dr As dsAccessCodes.Customer_AccessCode1Row In ta.GetDataByCustomerIDandCode(myUser.CustomerID, myUser.AccessCode)
            If dr.IsScheduledDeliveryNull Then
                intPShowpScheduledDelivery = 0
            ElseIf dr.ScheduledDelivery > 0 Then
                intPShowpScheduledDelivery = dr.ScheduledDelivery
            Else
                intPShowpScheduledDelivery = 0
            End If
        Next

        Dim intStatusID As Integer = qa.GetOrderStatusID(intOrderID)

        If intPShowpScheduledDelivery > 0 And DateDiffB(Now.Date, ScheduledDelivery) > intPShowpScheduledDelivery And intStatusID = myData.STATUS_ACTV Then
            qa.UpdateOrderRequiredDateStatus(ScheduledDelivery, myData.STATUS_FUT, intOrderID)
        ElseIf intPShowpScheduledDelivery > 0 And DateDiffB(Now.Date, ScheduledDelivery) <= intPShowpScheduledDelivery And intStatusID = myData.STATUS_ACTV Then
            qa.UpdateOrderRequiredActivationStatus(ScheduledDelivery, myData.STATUS_ACTV, Now.Date & " " & Now.TimeOfDay.ToString, intOrderID)
        ElseIf intPShowpScheduledDelivery = 0 Then
            Dim taOrder As New dsOrdersTableAdapters.OrderTableAdapter
            For Each dr As dsOrders.OrderRow In taOrder.GetOrderDataByID(intOrderID)
                If dr.StatusID = myData.STATUS_ACTV Then
                    dr.ActivationDate = Now.Date & " " & Now.TimeOfDay.ToString
                    taOrder.Update(dr)
                End If

            Next
        End If
    End Sub
    Private Function GetShipHandleCharge(ByRef myData As paraData, ByVal lShipID As Long, ByRef sMsg As String) As Single
        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String = String.Empty
        Dim sVal As Single = 0

        Try
            sSQLR = "SELECT Customer_ShippingMethod.HdChg " & _
                "FROM Customer_ShippingMethod WHERE ID = " & lShipID
            myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
            If rcdR.Read() Then
                sVal = rcdR("HdChg")
            End If
            myData.ReleaseReader2(cnnR, cmdR, rcdR)

        Catch ex As Exception
            LogError("GetShipHandleCharge >> " & ex.Message.ToString & " : " & sSQLR & msNewLine)
            myData.ReleaseReader2(cnnR, cmdR, rcdR)
        End Try

        Return sVal
    End Function

    Private Function OrderCreate_Custom(ByRef cnnE As OleDb.OleDbConnection, ByRef myData As paraData, ByVal lOrderID As Long, ByRef sMsg As String) As Boolean
        'Create Order Custom Data Records
        Const COL_ID = 1
        Const COL_VAL = 5

        Dim objDT As System.Data.DataTable
        Dim indx As Integer : indx = 0
        Dim iMaxIndx As Integer : iMaxIndx = 0
        Dim lCFID As Long = 0
        Dim sCFVal As String = ""
        Dim sSQLE As String = String.Empty
        Dim bSuccess As Boolean = False

        Try
            If (cnnE Is Nothing) Then
                LogActivity("      Attempt to Reset Cnn - OrderCreate_Custom", msLogFile)
                myData.GetConnection(cnnE)
            End If

            objDT = pCurrentCstCart
            iMaxIndx = objDT.Rows.Count - 1
            For indx = 0 To iMaxIndx
                lCFID = objDT.Rows(indx).Item(COL_ID)
                'sCFVal = objDT.Rows(indx).Item(COL_VAL)
                sCFVal = Left(objDT.Rows(indx).Item(COL_VAL), 50)
                If (lCFID > 0) And (sCFVal.Length > 0) Then
                    sSQLE = "INSERT INTO Order_CustomField (OrderID, CustomFieldID, [Value], ModifiedDate) " & _
                            "VALUES (" & lOrderID & ", " & lCFID & ", " & myData.SQLString(sCFVal) & ", #" & Now() & "#) "
                    myData.SQLExecute(cnnE, sSQLE)
                End If
            Next indx
            objDT = Nothing
            bSuccess = True

            'mw - 06-01-207 - Test
            myData.ReleaseConnection(cnnE)
            '

        Catch ex As Exception
            LogError("OrderCreate_Custom >> " & ex.Message.ToString & " : " & sSQLE & msNewLine)
            sMsg = "Error Creating Custom..."
            'mw - 06-01-207 - Test
            myData.ReleaseConnection(cnnE)
            '
        End Try

        Return bSuccess
    End Function

    
    '           

    Private Function OrderCreate_Itms(ByRef cnnE As OleDb.OleDbConnection, ByRef myData As paraData, ByVal lOrderID As Long, ByRef sMsg As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Ordering Documents:
        '
        '1-Order Printable only Documents - price determined by format
        '
        '2-Order Non-Printable/Printable Documents
        '
        ' a-determine qty available considering orders in progress not yet completed
        '  - (Fulfillment qty - documents ordered by fulfillment not yet marked w/ Print Date)
        '
        ' b-place order for available documents
        '  - determine Pick Charge
        '  -  if prepackaged qty (exactly) - use prepackaged Pick Charge price spread over qty
        '  -  if not prepackaged qty (exactly) - use Pick Charge price spread over qty
        '  - determine Carton Handling Charge
        '  -   if (qty*qtyperpull) exceeds 1 carton - use Carton Handling Charge * Carton Count spread over qty
        '  - determine Fulfillment Price
        '  - Price per Document = (Pick Charge / qty) + (Fulfillment Per Piece Price * Qty Per Pull)
        '
        ' c-place order for non-available documents
        '  - determine format price
        '  - Price per Document = Format Price
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'Create Order Item Records
        Const COL_QTY = 1
        Const COL_ItmID = 2

        Dim objDT As System.Data.DataTable
        Dim indx As Integer : indx = 0
        Dim iMaxIndx As Integer : iMaxIndx = 0
        Dim bSuccess As Boolean = False

        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String = String.Empty
        Dim sSQLE As String = String.Empty

        Dim sCurrentTime As String = String.Empty

        Dim lItmID As Long = 0
        'Dim iQty As Integer = 0
        Dim lQty As Long = 0
        Dim iItmType As Integer = 0
        Dim lQtyFillRemain As Long = 0
        'Dim iQtyToPrint As Integer = 0
        'Dim iQtyToFill As Integer = 0
        Dim lQtyToPrint As Long = 0
        Dim lQtyToFill As Long = 0
        Dim iQtyPerPkA As Integer = 0
        Dim iQtyPerPkB As Integer = 0
        'Dim iQtyPerCtn As Integer = 0
        Dim lQtyPerCtn As Long = 0
        Dim bPrePack As Boolean = False
        'Dim iCtnCnt As Integer = 0
        Dim lCtnCnt As Long = 0
        Dim iQtyPerPull As Integer = 0
        Dim gFillPrice As Single = 0

        'myData.GetCommand(cnnE, cmde)

        Try
            If (cnnE Is Nothing) Then
                LogActivity("      Attempt to Reset Cnn - OrderCreate_Itms", msLogFile)
                myData.GetConnection(cnnE)
            End If

            objDT = pCurrentItmCart
            iMaxIndx = objDT.Rows.Count - 1
            For indx = 0 To iMaxIndx
                If objDT.Rows(indx).RowState <> DataRowState.Deleted Then

                    lItmID = objDT.Rows(indx).Item(COL_ItmID)
                    'iQty = objDT.Rows(indx).Item(COL_QTY)
                    lQty = objDT.Rows(indx).Item(COL_QTY)
                    'If (iQty > lLIMax) Then lLIMax = iQty

                    'If (lItmID > 0) And (iQty > 0) Then
                    If (lItmID > 0) And (lQty > 0) Then
                        iItmType = 0
                        lQtyFillRemain = 0
                        'iQtyToPrint = 0
                        'iQtyToFill = 0
                        lQtyToPrint = 0
                        lQtyToFill = 0

                        'GetDocAvailability(cnnE, myData, lItmID, iItmType, lQtyFillRemain)
                        GetDocAvailability(myData, lItmID, iItmType, lQtyFillRemain)



                        If (iItmType = myData.DT_PRINTONLY) Then
                            'iQtyToPrint = iQty
                            lQtyToPrint = lQty
                        ElseIf (iItmType = myData.DT_FILLONLY) Then
                            'iQtyToFill = iQty
                            lQtyToFill = lQty
                        ElseIf iItmType = myData.DT_DOWNLOADONLY Then
                            sSQLE = "INSERT INTO Order_Document " & _
                                   "SELECT " & lOrderID & " AS OrderID, " & lItmID & " AS DocumentID, " & _
                                   lQty & " AS QtyOrdered, Price, " & _
                                   myData.PM_DOWNLOAD & " AS PrintMethod, #" & Now() & "# AS ModifiedDate " & _
                                   "FROM Customer_Document LEFT JOIN Customer_Format ON Customer_Document.FormatID = Customer_Format.ID " & _
                                   "WHERE Customer_Document.ID = " & lItmID & " AND Customer_Document.Status > " & myData.STATUS_INAC
                            myData.SQLExecute(cnnE, sSQLE)
                        Else
                            'If (iQty <= lQtyFillRemain) Then
                            '  iQtyToFill = iQty
                            If (lQty <= lQtyFillRemain) Then
                                'like FUL
                                lQtyToFill = lQty
                            Else
                                'Do not want orders split between part fill/print - all or nothing
                                'iQtyToFill = 0
                                'iQtyToPrint = iQty
                                'like POD
                                lQtyToFill = 0
                                lQtyToPrint = lQty
                            End If
                        End If

                        'Add Printable Documents
                        'If (iQtyToPrint > 0) Then
                        '  sSQLE = "INSERT INTO Order_Document " & _
                        '          "SELECT " & lOrderID & " AS OrderID, " & lItmID & " AS DocumentID, " & _
                        '          iQtyToPrint & " AS QtyOrdered, Price, " & _
                        '          myData.PM_PRINT & " AS PrintMethod, #" & Now() & "# AS ModifiedDate " & _
                        '          "FROM Customer_Document LEFT JOIN Customer_Format ON Customer_Document.FormatID = Customer_Format.ID " & _
                        '          "WHERE Customer_Document.ID = " & lItmID & " AND Customer_Document.Status > " & myData.STATUS_INAC
                        If (lQtyToPrint > 0) Then
                            sSQLE = "INSERT INTO Order_Document " & _
                                    "SELECT " & lOrderID & " AS OrderID, " & lItmID & " AS DocumentID, " & _
                                    lQtyToPrint & " AS QtyOrdered, Price, " & _
                                    myData.PM_PRINT & " AS PrintMethod, #" & Now() & "# AS ModifiedDate " & _
                                    "FROM Customer_Document LEFT JOIN Customer_Format ON Customer_Document.FormatID = Customer_Format.ID " & _
                                    "WHERE Customer_Document.ID = " & lItmID & " AND Customer_Document.Status > " & myData.STATUS_INAC
                            myData.SQLExecute(cnnE, sSQLE)
                        End If

                        'Add Fulfillment Documents
                        'If (iQtyToFill > 0) Then
                        If (lQtyToFill > 0) Then
                            'Determine if Prepackaged Pick Charge or Non-PrePackaged Pick Charge to be used
                            iQtyPerPkA = 0
                            iQtyPerPkB = 0
                            iQtyPerPull = 0
                            lQtyPerCtn = 0
                            gFillPrice = 0

                            sSQLR = "SELECT Customer_Document_Fill.QtyPerPkA, Customer_Document_Fill.QtyPerPkB, Customer_Document_Fill.QtyPerPull, Customer_Document_Fill.QtyPerCtn, Customer_Document_Fill.Price " & _
                                    "FROM Customer_Document_Fill WHERE DocumentID = " & lItmID
                            myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
                            If rcdR.Read() Then
                                'lOrderID = rcdR("OrderID")
                                iQtyPerPkA = rcdR("QtyPerPkA")
                                iQtyPerPkB = rcdR("QtyPerPkB")
                                iQtyPerPull = rcdR("QtyPerPull")
                                lQtyPerCtn = rcdR("QtyPerCtn")
                                'mw - 05-04-2009 - start considering Qty Per Pull
                                'gFillPrice = rcdR("Price")
                                gFillPrice = rcdR("Price") * iQtyPerPull
                            End If
                            myData.ReleaseReader2(cnnR, cmdR, rcdR)

                            'bPrePack = (iQty = 1) Or (iQty = iQtyPerPkA) Or (iQty = iQtyPerPkB)
                            bPrePack = (lQty = 1) Or (lQty = iQtyPerPkA) Or (lQty = iQtyPerPkB)

                            'determine how many cartons to charge for (do not charge for 1 carton - only over 1)
                            If (lQtyPerCtn > 0) Then
                                'mw - 07-20-2009
                                ''mw - 05-04-2009
                                ''iCtnCnt = (iQty \ iQtyPerCtn) - 1
                                ''iCtnCnt = ((iQty * iQtyPerPull) \ iQtyPerCtn) - 1
                                'iCtnCnt = ((lQty * iQtyPerPull) \ iQtyPerCtn) - 1
                                'iCtnCnt = IIf(iCtnCnt < 0, 0, iCtnCnt)
                                lCtnCnt = ((lQty * iQtyPerPull) \ lQtyPerCtn) - 1
                                lCtnCnt = IIf(lCtnCnt < 0, 0, lCtnCnt)
                            End If
                            'If (iCtnCnt > 0) Then
                            'sPrice = sPrice + ((sCtnChg * iCtnCnt) / iQty)

                            If (bPrePack = True) Then
                              
                                sSQLE = "INSERT INTO Order_Document " & _
                                        "SELECT " & lOrderID & " AS OrderID, " & lItmID & " AS DocumentID, " & _
                                        lQtyToFill & " AS QtyOrdered, " & _
                                        gFillPrice & " + (Customer.LinePckChgPak/" & lQtyToFill & ") + iif(" & lCtnCnt & ">0, (" & lCtnCnt & "*Customer.CtnHdChg)/" & lQtyToFill & ",0) AS Price, " & _
                                        myData.PM_FULFILL & " AS PrintMethod, #" & Now() & "# AS ModifiedDate, " & _
                                        "iif(Customer_Document.Status=" & myData.STATUS_BACK & "," & myData.STATUS_BACK & "," & myData.STATUS_NONE & ") AS Status " & _
                                        "FROM Customer_Document INNER JOIN Customer ON Customer_Document.CustomerID = Customer.ID " & _
                                        "WHERE Customer_Document.ID = " & lItmID & " AND Customer_Document.Status > " & myData.STATUS_INAC
                                '
                            Else
                             
                                sSQLE = "INSERT INTO Order_Document " & _
                                        "SELECT " & lOrderID & " AS OrderID, " & lItmID & " AS DocumentID, " & _
                                        lQtyToFill & " AS QtyOrdered, " & _
                                        gFillPrice & " + (Customer.LinePckChg/" & lQtyToFill & ") + iif(" & lCtnCnt & ">0, (" & lCtnCnt & "*Customer.CtnHdChg)/" & lQtyToFill & ",0) AS Price, " & _
                                        myData.PM_FULFILL & " AS PrintMethod, #" & Now() & "# AS ModifiedDate, " & _
                                        "iif(Customer_Document.Status=" & myData.STATUS_BACK & "," & myData.STATUS_BACK & "," & myData.STATUS_NONE & ") AS Status " & _
                                        "FROM Customer_Document INNER JOIN Customer ON Customer_Document.CustomerID = Customer.ID " & _
                                        "WHERE Customer_Document.ID = " & lItmID & " AND Customer_Document.Status > " & myData.STATUS_INAC
                                '
                            End If
                            myData.SQLExecute(cnnE, sSQLE)
                        End If
                    End If
                End If
            Next indx
            objDT = Nothing
            bSuccess = True

            'mw - 06-01-207 - Test
            myData.ReleaseConnection(cnnE)
            '

        Catch ex As Exception
            LogError("OrderCreate_Itms >> " & ex.Message.ToString & " : " & sSQLE & msNewLine)
            sMsg = "Error Creating Items..."
            'mw - 06-01-207 - Test
            myData.ReleaseConnection(cnnE)
            '
        End Try

        Return bSuccess
    End Function

    Private Function OrderCreate_Kits(ByRef cnnE As OleDb.OleDbConnection, ByRef myData As paraData, ByVal lOrderID As Long, ByRef sMsg As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Ordering Kits:
        '
        '1-Order Printable only Documents - price determined by format
        '
        ' a-determine qty available considering orders in progress not yet completed
        '  - (Fulfillment qty - documents ordered by fulfillment not yet marked w/ Print Date)
        '
        ' b-place order for available documents
        '  - determine Kit Charge
        '  - determine Pick Charge
        '  -  if prepackaged qty (exactly) - use prepackaged Pick Charge price spread over qty
        '  -  if not prepackaged qty (exactly) - use Pick Charge price spread over qty
        '  - determine Carton Handling Charge
        '  -   if (qty*qtyperpull) exceeds 1 carton - use Carton Handling Charge * Carton Count spread over qty
        '  - determine Fulfillment Price
        '  - Price per Document = Kit Charge + (Pick Charge / qty) + (Fulfillment Per Piece Price * Qty Per Pull)
        '
        ' c-place order for non-available documents
        '  - determine Kit Charge
        '  - determine format price
        '  - Price per Document = Kit Charge + Format Price
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'TODO kits added to new order
        'Create Order Item Records
        Const COL_QTY = 1
        Const COL_KitID = 2

        Dim objDT As System.Data.DataTable
        Dim indx As Integer : indx = 0
        Dim iMaxIndx As Integer : iMaxIndx = 0
        Dim bSuccess As Boolean = False

        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String = String.Empty
        Dim cnnR2 As OleDb.OleDbConnection
        Dim cmdR2 As OleDb.OleDbCommand
        Dim rcdR2 As OleDb.OleDbDataReader
        Dim sSQLR2 As String = String.Empty
        Dim sSQLE As String = String.Empty

        Dim sCurrentTime As String = String.Empty

        'Gather Kits - Begin
        Dim lKitID As Long = 0
        Dim iKitQty As Integer = 0
        Dim lItmID As Long = 0
        Dim iItmType As Integer = 0
        'Dim iItmQty As Integer = 0
        Dim lItmQty As Long = 0
        Dim iQtyFillRemain As Integer = 0
        'Dim iQtyToPrint As Integer = 0
        'Dim iQtyToFill As Integer = 0
        Dim lQtyToPrint As Long = 0
        Dim lQtyToFill As Long = 0
        Dim iQtyPerPkA As Integer = 0
        Dim iQtyPerPkB As Integer = 0
        Dim lQtyPerCtn As Long = 0
        Dim gFillPrice As Single = 0
        'Dim iCtnCnt As Integer = 0
        Dim lCtnCnt As Long = 0
        Dim iQtyPerPull As Integer = 0
        Dim bPrePack As Boolean = False

        'Create Order Kit-Documents Records
        Try
            If IsNothing(cnnE) Then
                LogActivity("      Attempt to Reset Cnn - OrderCreate_Kits", msLogFile)
                myData.GetConnection(cnnE)
            End If

            objDT = pCurrentKitCart
            iMaxIndx = objDT.Rows.Count - 1
            For indx = 0 To iMaxIndx
                If objDT.Rows(indx).RowState <> DataRowState.Deleted Then


                    lKitID = objDT.Rows(indx).Item(COL_KitID)
                    iKitQty = objDT.Rows(indx).Item(COL_QTY)

                    If (lKitID > 0) And (iKitQty > 0) Then
                        'Kit Details
                        sSQLR = "SELECT Customer_Document.ID AS ItmID, iif(Customer_Document.Status >" & myData.STATUS_INAC & ",True,False) AS Active, Customer_Kit_Document.Qty " & _
                                "FROM Customer_Kit_Document INNER JOIN Customer_Document ON Customer_Kit_Document.DocumentID = Customer_Document.ID " & _
                                "WHERE (Customer_Kit_Document.KitID = " & lKitID & ") " & _
                                "ORDER BY Customer_Document.ReferenceNo "
                        myData.GetReader2(cnnE, cmdR, rcdR, sSQLR)
                        Do While rcdR.Read()

                            lItmID = rcdR("ItmID")
                            'iItmQty = iKitQty * rcdR("Qty")
                            lItmQty = iKitQty * rcdR("Qty")

                            iQtyFillRemain = 0
                            'iQtyToPrint = 0
                            'iQtyToFill = 0
                            lQtyToPrint = 0
                            lQtyToFill = 0
                            'GetDocAvailability(cnnE, myData, lItmID, iItmType, iQtyFillRemain)
                            GetDocAvailability(myData, lItmID, iItmType, iQtyFillRemain)

                            If (iItmType = myData.DT_PRINTONLY) Then
                                'iQtyToPrint = iItmQty
                                lQtyToPrint = lItmQty
                            ElseIf (iItmType = myData.DT_FILLONLY) Then
                                'iQtyToFill = iItmQty
                                lQtyToFill = lItmQty
                            Else
                                'If (iItmQty <= iQtyFillRemain) Then
                                'iQtyToFill = iItmQty
                                If (lItmQty <= iQtyFillRemain) Then
                                    lQtyToFill = lItmQty
                                Else
                                    'Do not want orders split between part fill/print - all or nothing
                                    'iQtyToFill = iQtyFillRemain
                                    'iQtyToPrint = iQty - iQtyFillRemain
                                    'iQtyToFill = 0
                                    'iQtyToPrint = iItmQty
                                    lQtyToFill = 0
                                    lQtyToPrint = lItmQty
                                End If
                            End If
                            'TODO get kit price:
                            'Add Printable Documents
                            'If (iQtyToPrint > 0) Then
                            '  sSQLE = "INSERT INTO Order_Document " & _
                            '          "SELECT " & lOrderID & " AS OrderID, " & lKitID & " AS KitID, " & lItmID & " AS DocumentID, " & _
                            '          iQtyToPrint & " AS QtyOrdered, Customer_Format.Price + " & pLineKitChg & " AS Price, " & _
                            '          myData.PM_PRINT & " AS PrintMethod, #" & Now() & "# AS ModifiedDate " & _
                            '          "FROM Customer_Document LEFT JOIN Customer_Format ON Customer_Document.FormatID = Customer_Format.ID " & _
                            '          "WHERE Customer_Document.ID = " & lItmID & " AND Status > " & myData.STATUS_INAC
                            If (lQtyToPrint > 0) Then
                                sSQLE = "INSERT INTO Order_Document " & _
                                        "SELECT " & lOrderID & " AS OrderID, " & lKitID & " AS KitID, " & lItmID & " AS DocumentID, " & _
                                        lQtyToPrint & " AS QtyOrdered, Customer_Format.Price + " & pLineKitChg & " AS Price, " & _
                                        myData.PM_PRINT & " AS PrintMethod, #" & Now() & "# AS ModifiedDate " & _
                                        "FROM Customer_Document LEFT JOIN Customer_Format ON Customer_Document.FormatID = Customer_Format.ID " & _
                                        "WHERE Customer_Document.ID = " & lItmID & " AND Status > " & myData.STATUS_INAC
                                myData.SQLExecute(cnnE, sSQLE)
                            End If
                            '
                            'Add Fulfillment Documents
                            'If (iQtyToFill > 0) Then
                            If (lQtyToFill > 0) Then
                                'Get Fulfillment Price - Determine if Prepackaged Pick Charge or Non-PrePackaged Pick Charge to be used
                                iQtyPerPkA = 0
                                iQtyPerPkB = 0
                                iQtyPerPull = 0
                                lQtyPerCtn = 0
                                gFillPrice = 0

                                sSQLR2 = "SELECT Customer_Document_Fill.QtyPerPkA, Customer_Document_Fill.QtyPerPkB, Customer_Document_Fill.QtyPerPull, Customer_Document_Fill.QtyPerCtn, Customer_Document_Fill.Price " & _
                                         "FROM Customer_Document_Fill WHERE DocumentID = " & lItmID
                                myData.GetReader2(cnnR2, cmdR2, rcdR2, sSQLR2)
                                If (rcdR2.Read()) Then
                                    iQtyPerPkA = rcdR2("QtyPerPkA")
                                    iQtyPerPkB = rcdR2("QtyPerPkB")
                                    iQtyPerPull = rcdR2("QtyPerPull")
                                    lQtyPerCtn = rcdR2("QtyPerCtn")
                                    'mw - 05-04-2009 - start considering Qty Per Pull
                                    'gFillPrice = rcdR("Price")
                                    gFillPrice = rcdR2("Price") * iQtyPerPull
                                End If
                                myData.ReleaseReader2(cnnR2, cmdR2, rcdR2)
                                'bPrePack = (iItmQty = 1) Or (iItmQty = iQtyPerPkA) Or (iItmQty = iQtyPerPkB)
                                bPrePack = (lItmQty = 1) Or (lItmQty = iQtyPerPkA) Or (lItmQty = iQtyPerPkB)

                                'determine how many cartons to charge for (do not charge for 1 carton - only over 1)
                                If (lQtyPerCtn > 0) Then
                                    'mw - 07-20-2009
                                    ''mw - 05-04-2009
                                    ''iCtnCnt = (iItmQty \ iQtyPerCtn) - 1
                                    ''iCtnCnt = ((iItmQty * iQtyPerPull) \ iQtyPerCtn) - 1
                                    'iCtnCnt = ((lItmQty * iQtyPerPull) \ iQtyPerCtn) - 1
                                    'iCtnCnt = IIf(iCtnCnt < 0, 0, iCtnCnt)
                                    lCtnCnt = ((lItmQty * iQtyPerPull) \ lQtyPerCtn) - 1
                                    lCtnCnt = IIf(lCtnCnt < 0, 0, lCtnCnt)
                                End If
                                'If (iCtnCnt > 0) Then
                                'sPrice = sPrice + ((sCtnChg * iCtnCnt) / iItmQty)

                                If (bPrePack = True) Then
                                    'mw - 07-20-2009
                                    ''sSQLE = "INSERT INTO Order_Document " & _
                                    ''        "SELECT " & lOrderID & " AS OrderID, " & lKitID & " AS KitID, " & lItmID & " AS DocumentID, " & _
                                    ''        iQtyToFill & " AS QtyOrdered, " & _
                                    ''        gFillPrice & " + (Customer.LinePckChgPak/" & iQtyToFill & ") + iif(" & iCtnCnt & ">0, (" & iCtnCnt & "*Customer.CtnHdChg)/" & iQtyToFill & ",0) + " & pLineKitChg & " AS Price, " & _
                                    ''        myData.PM_FULFILL & " AS PrintMethod, #" & Now() & "# AS ModifiedDate, " & _
                                    ''        "iif(Customer_Document.Status=" & myData.STATUS_BACK & "," & myData.STATUS_BACK & "," & myData.STATUS_NONE & ") AS Status " & _
                                    ''        "FROM Customer_Document INNER JOIN Customer ON Customer_Document.CustomerID = Customer.ID " & _
                                    ''        "WHERE Customer_Document.ID = " & lItmID & " AND Customer_Document.Status > " & myData.STATUS_INAC
                                    'sSQLE = "INSERT INTO Order_Document " & _
                                    '        "SELECT " & lOrderID & " AS OrderID, " & lKitID & " AS KitID, " & lItmID & " AS DocumentID, " & _
                                    '        lQtyToFill & " AS QtyOrdered, " & _
                                    '        gFillPrice & " + (Customer.LinePckChgPak/" & lQtyToFill & ") + iif(" & iCtnCnt & ">0, (" & iCtnCnt & "*Customer.CtnHdChg)/" & lQtyToFill & ",0) + " & pLineKitChg & " AS Price, " & _
                                    '        myData.PM_FULFILL & " AS PrintMethod, #" & Now() & "# AS ModifiedDate, " & _
                                    '        "iif(Customer_Document.Status=" & myData.STATUS_BACK & "," & myData.STATUS_BACK & "," & myData.STATUS_NONE & ") AS Status " & _
                                    '        "FROM Customer_Document INNER JOIN Customer ON Customer_Document.CustomerID = Customer.ID " & _
                                    '        "WHERE Customer_Document.ID = " & lItmID & " AND Customer_Document.Status > " & myData.STATUS_INAC
                                    sSQLE = "INSERT INTO Order_Document " & _
                                            "SELECT " & lOrderID & " AS OrderID, " & lKitID & " AS KitID, " & lItmID & " AS DocumentID, " & _
                                            lQtyToFill & " AS QtyOrdered, " & _
                                            gFillPrice & " + (Customer.LinePckChgPak/" & lQtyToFill & ") + iif(" & lCtnCnt & ">0, (" & lCtnCnt & "*Customer.CtnHdChg)/" & lQtyToFill & ",0) + " & pLineKitChg & " AS Price, " & _
                                            myData.PM_FULFILL & " AS PrintMethod, #" & Now() & "# AS ModifiedDate, " & _
                                            "iif(Customer_Document.Status=" & myData.STATUS_BACK & "," & myData.STATUS_BACK & "," & myData.STATUS_NONE & ") AS Status " & _
                                            "FROM Customer_Document INNER JOIN Customer ON Customer_Document.CustomerID = Customer.ID " & _
                                            "WHERE Customer_Document.ID = " & lItmID & " AND Customer_Document.Status > " & myData.STATUS_INAC
                                    '
                                Else
                                    'mw - 07-20-2009
                                    ''sSQLE = "INSERT INTO Order_Document " & _
                                    ''        "SELECT " & lOrderID & " AS OrderID, " & lKitID & " AS KitID, " & lItmID & " AS DocumentID, " & _
                                    ''        iQtyToFill & " AS QtyOrdered, " & _
                                    ''        gFillPrice & " + (Customer.LinePckChg/" & iQtyToFill & ") + iif(" & iCtnCnt & ">0, (" & iCtnCnt & "*Customer.CtnHdChg)/" & iQtyToFill & ",0) + " & pLineKitChg & " AS Price, " & _
                                    ''        myData.PM_FULFILL & " AS PrintMethod, #" & Now() & "# AS ModifiedDate, " & _
                                    ''        "iif(Customer_Document.Status=" & myData.STATUS_BACK & "," & myData.STATUS_BACK & "," & myData.STATUS_NONE & ") AS Status " & _
                                    ''        "FROM Customer_Document INNER JOIN Customer ON Customer_Document.CustomerID = Customer.ID " & _
                                    ''        "WHERE Customer_Document.ID = " & lItmID & " AND Customer_Document.Status > " & myData.STATUS_INAC
                                    'sSQLE = "INSERT INTO Order_Document " & _
                                    '        "SELECT " & lOrderID & " AS OrderID, " & lKitID & " AS KitID, " & lItmID & " AS DocumentID, " & _
                                    '        lQtyToFill & " AS QtyOrdered, " & _
                                    '        gFillPrice & " + (Customer.LinePckChg/" & lQtyToFill & ") + iif(" & iCtnCnt & ">0, (" & iCtnCnt & "*Customer.CtnHdChg)/" & lQtyToFill & ",0) + " & pLineKitChg & " AS Price, " & _
                                    '        myData.PM_FULFILL & " AS PrintMethod, #" & Now() & "# AS ModifiedDate, " & _
                                    '        "iif(Customer_Document.Status=" & myData.STATUS_BACK & "," & myData.STATUS_BACK & "," & myData.STATUS_NONE & ") AS Status " & _
                                    '        "FROM Customer_Document INNER JOIN Customer ON Customer_Document.CustomerID = Customer.ID " & _
                                    '        "WHERE Customer_Document.ID = " & lItmID & " AND Customer_Document.Status > " & myData.STATUS_INAC
                                    sSQLE = "INSERT INTO Order_Document " & _
                                            "SELECT " & lOrderID & " AS OrderID, " & lKitID & " AS KitID, " & lItmID & " AS DocumentID, " & _
                                            lQtyToFill & " AS QtyOrdered, " & _
                                            gFillPrice & " + (Customer.LinePckChg/" & lQtyToFill & ") + iif(" & lCtnCnt & ">0, (" & lCtnCnt & "*Customer.CtnHdChg)/" & lQtyToFill & ",0) + " & pLineKitChg & " AS Price, " & _
                                            myData.PM_FULFILL & " AS PrintMethod, #" & Now() & "# AS ModifiedDate, " & _
                                            "iif(Customer_Document.Status=" & myData.STATUS_BACK & "," & myData.STATUS_BACK & "," & myData.STATUS_NONE & ") AS Status " & _
                                            "FROM Customer_Document INNER JOIN Customer ON Customer_Document.CustomerID = Customer.ID " & _
                                            "WHERE Customer_Document.ID = " & lItmID & " AND Customer_Document.Status > " & myData.STATUS_INAC
                                End If
                                myData.SQLExecute(cnnE, sSQLE)
                                '
                            End If
                        Loop 'End - Looping through Items in a Kit
                        myData.ReleaseReader2(cnnR, cmdR, rcdR)
                    End If 'End - Checking a Kit Item
                End If
            Next 'End - Looping through Kits
            'If (iMaxIndx > 0) Then myData.ReleaseReader2(cnnR, cmdR, rcdR)

            bSuccess = True

            'mw - 06-01-207 - Test
            myData.ReleaseConnection(cnnE)
            '

        Catch ex As Exception
            LogError("OrderCreate_Kits >> " & ex.Message.ToString & " : " & sSQLE & msNewLine)
            sMsg = "Error Creating Kits..."
            'mw - 06-01-207 - Test
            myData.ReleaseConnection(cnnE)
            '
        End Try

        Return bSuccess
    End Function
    '''''''''''''''''

    'Private Function GetDocAvailability(ByRef cnnE As OleDb.OleDbConnection, ByRef myData As paraData, ByVal lItmID As Long, ByRef iItmType As Integer, ByRef lQtyFillRemain As Long)
    Private Function GetDocAvailability(ByRef myData As paraData, ByVal lItmID As Long, ByRef iItmType As Integer, ByRef lQtyFillRemain As Long) As Boolean
        Dim bSuccess As Boolean : bSuccess = False
        Dim cnnR As SqlConnection
        Dim cmdR As SqlCommand
        Dim rcdR As SqlDataReader
        Dim sSQLR As String = String.Empty
        Dim lQtyOH As Long = 0
        'Dim iQtyToFill As Integer = 0
        Dim lQtyToFill As Long = 0
        Dim iQtyPerPull As Integer = 0
        Dim iStockWarn As Integer = 0
        Dim QTY_XTRA As Integer = 0

        Try
            'If (cnnE Is Nothing) Then
            'LogActivity("      Attempt to Reset Cnn - GetDocAvailability", msLogFile)
            'myData.GetConnection(cnnE)
            'End If

            iItmType = -1
            lQtyFillRemain = 0

            sSQLR = "SELECT Customer_Document.PrintMethod " & _
                    "FROM Customer_Document WHERE ID = " & lItmID
            myData.GetReaderSQL(cnnR, cmdR, rcdR, sSQLR)
            If rcdR.Read() Then
                iItmType = rcdR("PrintMethod")
            End If
            myData.ReleaseReaderSQL(cnnR, cmdR, rcdR)
            If (iItmType <> myData.DT_PRINTONLY) Then


                If iItmType = myData.DT_DOWNLOADONLY Then
                    'DOWNLOAD ONLY!
                Else
                    sSQLR = "SELECT Customer_Document_Fill.QtyOH, Customer_Document_Fill.QtyPerPull, Customer_Document_Fill.StockWarn " & _
                      "FROM Customer_Document_Fill WHERE DocumentID = " & lItmID
                    myData.GetReaderSQL(cnnR, cmdR, rcdR, sSQLR)
                    If rcdR.Read() Then
                        lQtyOH = rcdR("QtyOH")
                        iQtyPerPull = rcdR("QtyPerPull")
                        iStockWarn = rcdR("StockWarn")
                    End If
                    myData.ReleaseReaderSQL(cnnR, cmdR, rcdR)

                    'Consider if PerPull > 1
                    If (iQtyPerPull > 0) Then lQtyOH = lQtyOH / iQtyPerPull

                    'Consider having backup of at least an extra ...
                    'mw - 09-13-2007 - ab use from item
                    'QTY_XTRA = ConfigurationSettings.AppSettings("SafetyInventoryLevel")
                    QTY_XTRA = iStockWarn
                    '
                    If (iQtyPerPull > 0) And (QTY_XTRA > 0) Then lQtyOH = lQtyOH - (QTY_XTRA / iQtyPerPull)

                    sSQLR = "SELECT SUM(QtyOrdered) AS QtyToFill " & _
                            "FROM Order_Document INNER JOIN [Order] ON Order_Document.OrderID = [Order].ID " & _
                            "WHERE [Order].StatusID in (1,5,6,7) " & _
                            " AND Order_Document.DocumentID = " & lItmID & " AND Order_Document.PrintMethod = " & myData.PM_FULFILL & _
                            " AND (Order_Document.Status<>" & myData.STATUS_PRT & ")"
                    myData.GetReaderSQL(cnnR, cmdR, rcdR, sSQLR)
                    If rcdR.Read() Then
                        'iQtyToFill = Val(rcdR("QtyToFill") & "")
                        lQtyToFill = Val(rcdR("QtyToFill") & "")
                        'If (Len(iQtyToFill) = 0) Then iQtyToFill = 0
                    End If
                    myData.ReleaseReaderSQL(cnnR, cmdR, rcdR)

                    'If (lQtyOH > iQtyToFill) Then
                    '  lQtyFillRemain = lQtyOH - iQtyToFill
                    If (lQtyOH > lQtyToFill) Then
                        lQtyFillRemain = lQtyOH - lQtyToFill
                    End If
                End If

              
            End If

            bSuccess = True

        Catch ex As Exception
            LogError("GetAvailability >> " & ex.Message.ToString & " : " & sSQLR & msNewLine)
            myData.ReleaseReaderSQL(cnnR, cmdR, rcdR)
        End Try

        Return bSuccess
    End Function


    Private Function AuthorizationCheck(ByRef myData As paraData, ByVal lOrderID As Long, ByRef myUser As paraUser) As Boolean
        'todo checks for autorization
        Dim bRequiresAuthor As Boolean = False
        Dim bRequiresShipCtyAuthor As Boolean = False
        Dim bRequiresShipMtdAuthor As Boolean = False
        Dim bRequiresQtyLIAuthor As Boolean = False
        Dim bRequiresQtyORAuthor As Boolean = False
        Dim sEmailAuthor As String = String.Empty
        Dim sEmailCopy As String = String.Empty
        Dim sSubject As String = String.Empty
        Dim sBody As String = String.Empty
        Dim sKey As String = String.Empty
        Dim myMail As Mail
        Dim str1 As String
        Dim str2 As String
        Try
            bRequiresAuthor = IsAuthorRequired(myData, lOrderID, myUser, bRequiresShipCtyAuthor, bRequiresShipMtdAuthor, bRequiresQtyLIAuthor, bRequiresQtyORAuthor, sEmailAuthor, sEmailCopy, sKey)

            'Message - Start
            If bRequiresAuthor And myUser.DisableAllQuantityLimits = False Then
                myMail = New Mail

                sSubject = "Order Authorization Check"

                Dim strLink As String = ConfigurationSettings.AppSettings("HomeAddress") & "/" & ConfigurationSettings.AppSettings("SiteLocation") & "/ordReviewLanding.aspx?KE=" & sKey & "&RNUMB=" & lOrderID


                strLink = "<a href=""" & strLink & """>" & strLink & "</a>"

                sBody = "Order Request # " & lOrderID & " has been placed on hold until authorization has been received." & _
                          "  Order may be reviewed and authorized by following the link below..." & vbCrLf & vbCrLf & _
                          strLink

                If bRequiresShipCtyAuthor Then
                    sBody = sBody & vbCrLf & vbCrLf & "Note:  Shipping country requires authorization."
                End If
                If bRequiresShipMtdAuthor Then
                    sBody = sBody & vbCrLf & vbCrLf & "Note:  Shipping method requires authorization."
                End If
                If bRequiresQtyLIAuthor Then
                    sBody = sBody & vbCrLf & vbCrLf & "Note:  Quantity of line items requires authorization."
                End If
                If bRequiresQtyORAuthor Then
                    sBody = sBody & vbCrLf & vbCrLf & "Note:  Total quantity of items ordered requires authorization."
                End If
                sBody = sBody & vbCrLf & vbCrLf & MSG_RESP

                'SendMessage(sEmailAuthor, sEmailCopy, ConfigurationSettings.AppSettings("DefaultEmailAddress"), sSubject, sBody)
                myMail.SendMessage(sEmailAuthor, ConfigurationSettings.AppSettings("DefaultEmailAddress"), _
                                   sSubject, sBody, sEmailCopy, ConfigurationSettings.AppSettings("DefaultEmailCopy"), str1, str2, True)
                myMail = Nothing
                Try
                    SendNewOrderAuthorizationNotifications(myUser, myData, lOrderID, sSubject, sBody)
                Catch ex As Exception
                    LogError("SendNewOrderAuthorizationNotifications >> " & ex.Message.ToString & msNewLine)
                End Try

            End If
            'Message - End

        Catch ex As Exception
            LogError("AuthorizationCheck >> " & ex.Message.ToString & msNewLine)
        End Try

        Return bRequiresAuthor
    End Function

    Private Function IsAuthorRequired(ByRef myData As paraData, ByVal lOrderID As Long, ByRef myUser As paraUser, _
                                      ByRef bRequiresShipCountryAuthor As Boolean, ByRef bRequiresShipMethodAuthor As Boolean, ByRef bRequiresQtyLIAuthor As Boolean, ByRef bRequiresQtyORAuthor As Boolean, _
                                      ByRef sEmailAuthor As String, ByRef sEmailCopy As String, ByRef sKey As String) As Boolean
        Dim bSuccess As Boolean : bSuccess = False
        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String = String.Empty
        Dim lCustomerID As Long = 0
        Dim lShipID As Long = 0
        Dim sCountry As String = String.Empty

        bRequiresShipCountryAuthor = False
        bRequiresShipMethodAuthor = False
        bRequiresQtyLIAuthor = False
        bRequiresQtyORAuthor = False
        sEmailAuthor = String.Empty
        sEmailCopy = String.Empty
        sKey = String.Empty





        Try
            'Look up Details
            sSQLR = "SELECT [Order].PreferredShipID AS ShipID, [Order].ShipTo_Country, FORMAT([Order].RequestDate,'mmddyyyy-hhnnss') AS OrderKey, " & _
                    "Customer_AccessCode.EmailAuthor, " & _
                    "Customer.ID AS CustomerID, Customer.Email " & _
                    "FROM [Order] INNER JOIN (Customer_AccessCode INNER JOIN Customer ON Customer_AccessCode.CustomerID = Customer.ID) ON [Order].AccessCodeID = Customer_AccessCode.ID " & _
                    "WHERE [Order].ID = " & lOrderID
            myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
            If rcdR.Read() Then
                lShipID = rcdR("ShipID")
                sCountry = rcdR("ShipTo_Country")
                sKey = rcdR("OrderKey")
                sEmailAuthor = rcdR("EmailAuthor")
                lCustomerID = rcdR("CustomerID")
                sEmailCopy = rcdR("Email")
            End If
            myData.ReleaseReader2(cnnR, cmdR, rcdR)

            'Verify Country against Exclusion List
            bRequiresShipCountryAuthor = CountryExclusionCheck(myData, lCustomerID, sCountry)

            'Look up Shipping Method
            sSQLR = "SELECT Customer_ShippingPerm.Permit " & _
                   "FROM Customer_ShippingPerm " & _
                   "WHERE AccessCodeID = " & myUser.AccessCodeID & " AND ShippingMethodID = " & lShipID
            myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
            If rcdR.Read() Then
                bRequiresShipMethodAuthor = CBool(rcdR("Permit") = 2)
            End If
            myData.ReleaseReader2(cnnR, cmdR, rcdR)

            If CheckForDownloadOnly(pCurrentItmCart, pCurrentKitCart) Then
                bRequiresShipMethodAuthor=False
            End If

            If myUser.DisableAllQuantityLimits = False Then


                'Allowed to exceed w/ authorization
                If (myUser.CanExtendQtyLI = True) Then
                    'Look up qty on each
                    sSQLR = "SELECT iif(MAX(QtyOrdered)is null,0,MAX(QtyOrdered)) AS Qty " & _
                            "FROM Order_Document " & _
                            "WHERE OrderID = " & lOrderID
                    myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
                    If rcdR.Read() Then
                        bRequiresQtyLIAuthor = CBool(rcdR("Qty") > myUser.MaxQtyPerLineItem)
                    End If
                    myData.ReleaseReader2(cnnR, cmdR, rcdR)
                End If
            Else
                bRequiresQtyLIAuthor = False
            End If

            If (Not bRequiresQtyLIAuthor) And myUser.DisableAllQuantityLimits = False Then
                bRequiresQtyLIAuthor = bOverQuantityLimit
            End If

            If myUser.DisableAllQuantityLimits = False Then
                'Allowed to exceed w/ authorization
                If (myUser.CanExtendQtyOR = True) Then
                    'Look up qty sum
                    sSQLR = "SELECT iif(SUM(QtyOrdered) is null,0,SUM(QtyOrdered)) AS Qty " & _
                            "FROM Order_Document " & _
                            "WHERE OrderID = " & lOrderID
                    myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
                    If rcdR.Read() Then
                        bRequiresQtyORAuthor = CBool(rcdR("Qty") > myUser.MaxQtyPerOrder)
                    End If
                    myData.ReleaseReader2(cnnR, cmdR, rcdR)
                End If
            Else
                bRequiresQtyORAuthor = False
            End If

            myData.ReleaseReader2(cnnR, cmdR, rcdR)

        Catch ex As Exception
            LogError("IsAuthorRequired >> " & ex.Message.ToString & msNewLine)
            myData.ReleaseReader2(cnnR, cmdR, rcdR)
        End Try



        Return bRequiresShipCountryAuthor Or bRequiresShipMethodAuthor Or bRequiresQtyLIAuthor Or bRequiresQtyORAuthor
    End Function

    Function CountryExclusionCheck(ByRef myData As paraData, ByVal lCustomerID As Long, ByVal sCountry As String, Optional ByRef sMsg As String = "") As Boolean
        Dim bMatch As Boolean = False
        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String = String.Empty
        Dim sMatchCountry As String = String.Empty

        Try
            sMsg = String.Empty

            'Exact Match Check
            ' ''sSQLR = "SELECT Customer_Match.MatchText AS CountryMatch " & _
            ' ''        "FROM Customer_Match " & _
            ' ''        "WHERE CustomerID = " & lCustomerID & _
            ' ''        " AND Customer_Match.MatchType = 1 " & _
            ' ''        " AND Customer_Match.MatchText = " & myData.SQLString(sCountry)
            ' ''myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
            ' ''If rcdR.Read() Then
            ' ''    bMatch = True
            ' ''    sMatchCountry = rcdR("CountryMatch")
            ' ''    sMsg = "The system has determined that the Ship To Country entered is an exact or near match to the country of " & sMatchCountry & ".  You will be able to finalize and submit your order, but it will be held pending authorization."
            ' ''End If
            ' ''myData.ReleaseReader2(cnnR, cmdR, rcdR)

            Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
            If qa.CheckForCountry(sCountry, lCustomerID) = 1 Or qa.CheckForCountry(sCountry, lCustomerID) = 2 Then
                bMatch = True
                sMatchCountry = sCountry
                sMsg = "The system has determined that the Ship To Country entered is an exact or near match to the country of " & sMatchCountry & ".  You will be able to finalize and submit your order, but it will be held pending authorization."

            End If

            '

            'Close Match Check - Method 1
            '' LEFT 50% + 1 positions AND RIGHT 1 position : Note:  NO rounding so, use +0.5 instead of +1
            '' RIGHT 50% + 1 positions AND LEFT 1 position : Note:  NO rounding so, use +0.5 instead of +1
            'sSQLR = "SELECT Customer_Match.MatchText AS CountryMatch " & _
            '        "FROM Customer_Match " & _
            '        "WHERE CustomerID = " & lCustomerID & _
            '        " AND Customer_Match.MatchType = 2 " & _
            '        " AND " & _
            '        " ( (( LEFT(Customer_Match.MatchText, ROUND((LEN(Customer_Match.MatchText) * 0.5) + 0.5, 0)) " & _
            '        "    = LEFT(" & myData.SQLString(sCountry) & ", ROUND((LEN(Customer_Match.MatchText) * 0.5) + 0.5, 0))" & _
            '        "    ) AND " & _
            '        "    ( RIGHT(Customer_Match.MatchText, 1) " & _
            '        "    = RIGHT(" & myData.SQLString(sCountry) & ",1)" & _
            '        "    ))" & _
            '        "      ) OR (" & _
            '        "   (( RIGHT(Customer_Match.MatchText, ROUND((LEN(Customer_Match.MatchText) * 0.5) + 0.5, 0)) " & _
            '        "    = RIGHT(" & myData.SQLString(sCountry) & ", ROUND((LEN(Customer_Match.MatchText) * 0.5) + 0.5, 0))" & _
            '        "    ) AND " & _
            '        "    ( LEFT(Customer_Match.MatchText, 1) " & _
            '        "    = LEFT(" & myData.SQLString(sCountry) & ",1)" & _
            '        "    ))" & _
            '        " )"
            '
            'Close Match Check - Method 2
            ' LEFT 50% + 1 positions of MatchText AND RIGHT 1 position of MatchText is present : Note:  NO rounding so, use +0.5 instead of +1
            ' RIGHT 50% + 1 positions of MatchText AND LEFT 1 position of MatchText is present : Note:  NO rounding so, use +0.5 instead of +1
            '' '' ''sSQLR = "SELECT Customer_Match.MatchText AS CountryMatch " & _
            '' '' ''        "FROM Customer_Match " & _
            '' '' ''        "WHERE CustomerID = " & lCustomerID & _
            '' '' ''        " AND Customer_Match.MatchType = 2 " & _
            '' '' ''        " AND " & _
            '' '' ''        " ( (( LEFT(Customer_Match.MatchText, ROUND((LEN(Customer_Match.MatchText) * 0.5) + 0.5, 0)) " & _
            '' '' ''        "    = LEFT(" & myData.SQLString(sCountry) & ", ROUND((LEN(Customer_Match.MatchText) * 0.5) + 0.5, 0))" & _
            '' '' ''        "    ) AND " & _
            '' '' ''        "    (INSTR(" & myData.SQLString(sCountry) & ", RIGHT(Customer_Match.MatchText,1))>0" & _
            '' '' ''        "    ))" & _
            '' '' ''        "      ) OR (" & _
            '' '' ''        "   (( RIGHT(Customer_Match.MatchText, ROUND((LEN(Customer_Match.MatchText) * 0.5) + 0.5, 0)) " & _
            '' '' ''        "    = RIGHT(" & myData.SQLString(sCountry) & ", ROUND((LEN(Customer_Match.MatchText) * 0.5) + 0.5, 0))" & _
            '' '' ''        "    ) AND " & _
            '' '' ''        "    (INSTR(" & myData.SQLString(sCountry) & ", LEFT(Customer_Match.MatchText,1))>0" & _
            '' '' ''        "    ))" & _
            '' '' ''        " )"
            ' '' '' ''
            '' '' ''myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
            '' '' ''If rcdR.Read() Then
            '' '' ''    bMatch = True
            '' '' ''    sMatchCountry = rcdR("CountryMatch")
            '' '' ''    sMsg = "The system has determined that the Ship To Country entered is an exact or near match to the country of " & sMatchCountry & ".  You will be able to finalize and submit your order, but it will be held pending authorization."
            '' '' ''End If
            myData.ReleaseReader2(cnnR, cmdR, rcdR)
            '

        Catch ex As Exception
            LogError("CountryExclusionCheck >> " & ex.Message.ToString & msNewLine)
            myData.ReleaseReader2(cnnR, cmdR, rcdR)
        End Try

        Return bMatch
    End Function

    ''''''''''''''''''

    Function OrderActivate(ByRef cnn As OleDb.OleDbConnection, ByRef myData As paraData, ByVal lOrderID As Long) As Boolean
        Dim sSQL As String = String.Empty
        Dim bSuccess As Boolean : bSuccess = False

        Try
            If (cnn Is Nothing) Then myData.GetConnection(cnn)

            If Not (cnn Is Nothing) Then
                sSQL = "UPDATE [Order] SET StatusID = " & myData.STATUS_ACTV & " WHERE Order.ID = " & lOrderID
                myData.SQLExecute(cnn, sSQL)
            End If

            bSuccess = True

            'mw - 06-01-207 - Test
            myData.ReleaseConnection(cnn)
            '

        Catch ex As Exception
            LogError("OrderActivate >> " & ex.Message.ToString & msNewLine)
            'mw - 06-01-207 - Test
            myData.ReleaseConnection(cnn)
            '
        End Try

        Return bSuccess
    End Function

    Public Function OrderPartial(ByRef cnn As OleDb.OleDbConnection, ByRef myData As paraData, ByVal lOrderID As Long) As Boolean
        Dim sSQL As String = String.Empty
        Dim bSuccess As Boolean : bSuccess = False

        Try
            If (cnn Is Nothing) Then myData.GetConnection(cnn)

            If Not (cnn Is Nothing) Then
                sSQL = "UPDATE [Order] SET StatusID = 5 WHERE Order.ID = " & lOrderID
                myData.SQLExecute(cnn, sSQL)
            End If

            bSuccess = True

            'mw - 06-01-207 - Test
            myData.ReleaseConnection(cnn)
            '

        Catch ex As Exception
            LogError("OrderPartial >> " & ex.Message.ToString & msNewLine)
            'mw - 06-01-207 - Test
            myData.ReleaseConnection(cnn)
            '
        End Try

        Return bSuccess
    End Function

    Private Function ConfirmationCheck(ByRef myData As paraData, ByVal lOrderID As Long, ByVal bRequiresAuthor As Boolean, ByVal sTo As String, ByVal blnDisableAllQuanitityLimits As Boolean) As Boolean
        Dim bSuccess As Boolean = False
        Dim sEmailAuthor As String = String.Empty
        Dim sEmailCopy As String = String.Empty
        Dim sSubject As String = String.Empty
        Dim sBody As String = String.Empty
        Dim myMail As Mail

        Try
            If (Len(sTo) > 0) Then
                'Message - Start
                myMail = New Mail

                sSubject = "Order Confirmation"

                sBody = "<CENTER>"
                sBody = sBody & "Order Request # " & lOrderID & " has been placed"
                If (bRequiresAuthor) Then
                    sBody = sBody & "on hold until authorization has been received"
                End If
                sBody = sBody & "." & GetConfirmationBody(myData, lOrderID, blnDisableAllQuanitityLimits)
                sBody = sBody & "<br>" & MSG_RESP & _
                        "</CENTER>"
                'TODO: disable: 
                'LogActivity("    Email Text: ...." & sBody, msLogFile)
                myMail.SendMessage(sTo, ConfigurationSettings.AppSettings("DefaultEmailAddress"), _
                                   sSubject, sBody, sEmailCopy, ConfigurationSettings.AppSettings("DefaultEmailCopy"), , , True)
                myMail = Nothing


            End If
            'Message - End

        Catch ex As Exception
            LogError("ConfirmationCheck >> " & ex.Message.ToString & msNewLine)
        End Try

        Return bSuccess
    End Function


    Function SendDownloadLinks(strEmailBody As String, strOrderNumber As String, sTo As String) As Boolean
        Dim bSuccess As Boolean = False
        Dim sSubject As String = String.Empty

        Dim myMail As Mail

        Try
            If (Len(sTo) > 0) Then
                'Message - Start
                myMail = New Mail

                sSubject = "Request Number " & strOrderNumber


                'LogActivity(" Download Links Email Text: ...." & strEmailBody, msLogFile)
                myMail.SendMessage(sTo, ConfigurationSettings.AppSettings("DefaultEmailAddress"), _
                                   sSubject, strEmailBody, "", ConfigurationSettings.AppSettings("DefaultEmailCopy"), , , True)
                myMail = Nothing


            End If
            'Message - End

        Catch ex As Exception
            LogError("ConfirmationCheck >> " & ex.Message.ToString & msNewLine)
        End Try

        Return bSuccess
    End Function
    Private Function GetConfirmationBody(ByRef myData As paraData, ByVal lOrderID As Long, ByVal blnDisableAllQuantityLimits As Boolean) As String
        Dim s As StringBuilder = New StringBuilder(200)
        'Dim cnnR As OleDb.OleDbConnection
        'Dim cmdR As OleDb.OleDbCommand
        'Dim rcdR As OleDb.OleDbDataReader

        Dim cnnR As SqlClient.SqlConnection
        Dim cmdR As SqlClient.SqlCommand
        Dim rcdR As SqlClient.SqlDataReader

        Dim sSQLR As String = String.Empty
        Dim bRequiresLI As Boolean = False
        Dim bRequiresOR As Boolean = False

        Dim sRequestorTitle As String = String.Empty
        Dim sRequestorFirstName As String = String.Empty
        Dim sRequestorLastName As String = String.Empty
        Dim sRequestorEmail As String = String.Empty
        Dim sShipContactFirstName As String = String.Empty
        Dim sShipContactLastName As String = String.Empty
        Dim sShipCompany As String = String.Empty
        Dim sShipAddress1 As String = String.Empty
        Dim sShipAddress2 As String = String.Empty
        Dim sShipCity As String = String.Empty
        Dim sShipState As String = String.Empty
        Dim sShipZip As String = String.Empty
        Dim sShipCountry As String = String.Empty
        Dim sShipPrefDesc As String = String.Empty
        Dim sShipNote As String = String.Empty
        Dim sCoverSheetDesc As String = String.Empty
        Dim sCoverSheetText As String = String.Empty
        Dim strRequiredDate As String = String.Empty
        Dim sRecipientPhone As String = String.Empty
        Dim sRecipientEmail As String = String.Empty
        Dim paradata As New paraData





        Dim sPrevKit As String = String.Empty

        'Order - Base
        sSQLR = "SELECT Requestor_Title, Requestor_FirstName, Requestor_LastName, Requestor_Email, " & _
                "ShipTo_ContactFirstName, ShipTo_ContactLastName, ShipTo_Name, ShipTo_Address1, ShipTo_Address2, ShipTo_City, ShipTo_State, ShipTo_ZipCode, ShipTo_Country, " & _
                "Customer_ShippingMethod.Description AS ShipTo_PrefDescr, ShipNote, " & _
                "Customer_CoverSheet.Name AS CS_Desc, CS_Content, RequiredDate , RecipientPhone , RecipientEmail " & _
                "FROM Customer_ShippingMethod RIGHT JOIN ([Order] LEFT JOIN Customer_CoverSheet ON [Order].CoverSheetID = Customer_CoverSheet.ID) ON Customer_ShippingMethod.ID = [Order].PreferredShipID " & _
                "WHERE [Order].ID = " & lOrderID
        myData.GetReaderSQL(cnnR, cmdR, rcdR, sSQLR)

        If rcdR.Read() Then
            sRequestorTitle = rcdR("Requestor_Title")
            sRequestorFirstName = rcdR("Requestor_FirstName")
            sRequestorLastName = rcdR("Requestor_LastName")
            sRequestorEmail = rcdR("Requestor_Email")

            sShipContactFirstName = rcdR("ShipTo_ContactFirstName")
            sShipContactLastName = rcdR("ShipTo_ContactLastName")
            sShipCompany = rcdR("ShipTo_Name")
            sShipAddress1 = rcdR("ShipTo_Address1")
            sShipAddress2 = rcdR("ShipTo_Address2")
            sShipCity = rcdR("ShipTo_City")
            sShipState = rcdR("ShipTo_State")
            sShipZip = rcdR("ShipTo_ZipCode")
            sShipCountry = rcdR("ShipTo_Country")
            sShipPrefDesc = rcdR("ShipTo_PrefDescr")
            sShipNote = rcdR("ShipNote")
            sRecipientEmail = rcdR("RecipientEmail")
            sRecipientPhone = rcdR("RecipientPhone")

            sCoverSheetDesc = rcdR("CS_Desc")
            sCoverSheetText = rcdR("CS_Content")

            If rcdR("RequiredDate") Is DBNull.Value = False Then
                strRequiredDate = rcdR("RequiredDate")
            End If
        End If
        myData.ReleaseReaderSQL(cnnR, cmdR, rcdR)

        s.Append("<br><br>")
        s.Append("<table>")
        s.Append("<tr><td align='right'>" & "Requestor:" & "</td><td>&nbsp;</td><td>" & sRequestorTitle & "</td></tr>")
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & sRequestorFirstName & " " & sRequestorLastName & "</td></tr>")
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & sRequestorEmail & "</td></tr>")
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td></td></tr>")

        s.Append("<tr><td align='right'>" & "Shipping:" & "</td><td>&nbsp;</td><td>" & sShipContactFirstName & " " & sShipContactLastName & "</td></tr>")
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & pShipCompany & "</td></tr>")
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & pShipAddress1 & "</td></tr>")
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & pShipAddress2 & "</td></tr>")
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & pShipCity & ", " & pShipState & " " & pShipZip & " " & pShipCountry & "</td></tr>")
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & pShipPrefDesc & "</td></tr>")
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & pShipNote & "</td></tr>")

        s.Append("<tr><td align='right'></td><td>Recipient Email:</td><td>" & pRecipientEmail & "</td></tr>")
        s.Append("<tr><td align='right'></td><td>Recipient Phone:</td><td>" & pRecipientPhone & "</td></tr>")

        If strRequiredDate <> "" Then
            s.Append("<tr><td align='right'></td><td>Scheduled Delivery:</td><td>" & strRequiredDate & "</td></tr>")
        End If

        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td></td></tr>")

        s.Append("<tr><td align='right'>" & "Cover Sheet:" & "</td><td>&nbsp;</td><td>" & pCoverSheetDesc & "</td></tr>")
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & pCoverSheetText & "</td></tr>")
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td></td></tr>")
        'Order - Custom
        s.Append("<tr><td align='right'>" & "Custom:" & "</td><td>&nbsp;</td><td></td></tr>")
        sSQLR = "SELECT Customer_CustomField.Name AS FieldName,  " & _
                "Order_CustomField.Value AS FieldValue " & _
                "FROM Order_CustomField LEFT JOIN Customer_CustomField ON Order_CustomField.CustomFieldID = Customer_CustomField.ID " & _
                "WHERE Order_CustomField.OrderID = " & lOrderID & " " & _
                "ORDER BY Customer_CustomField.Name "
        myData.GetReaderSQL(cnnR, cmdR, rcdR, sSQLR)
        Do While rcdR.Read()
            s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & rcdR("FieldName").ToString() & ":&nbsp;&nbsp;" & rcdR("FieldValue").ToString() & "</td></tr>")
        Loop
        myData.ReleaseReaderSQL(cnnR, cmdR, rcdR)
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td></td></tr>")

        'Order - Item
        s.Append("<tr><td align='right'>" & "Details:" & "</td><td>&nbsp;</td><td></td></tr>")
        sSQLR = "SELECT Customer_Document.ReferenceNo AS ItmName, Customer_Document.Description AS ItmDesc, " & _
                "Order_Document.QtyOrdered AS ItmQty, " & _
                "CASE [Customer_Document].[Status]  " & _
                "WHEN 2 THEN 1  " & _
                "ELSE 0 " & _
                "END  AS OnBackOrder  " & _
                "FROM Order_Document LEFT JOIN Customer_Document ON Order_Document.DocumentID = Customer_Document.ID " & _
                "WHERE Order_Document.OrderID = " & lOrderID & " AND Order_Document.KitID IS NULL " & _
                "ORDER BY Customer_Document.ReferenceNo "
        'sSQLR = "SELECT Customer_Document.ReferenceNo AS ItmName, Customer_Document.Description AS ItmDesc, " & _
        '"Order_Document.QtyOrdered AS ItmQty, " & _
        '"iif([Customer_Document].Status=" & myData.STATUS_BACK & ",True,False) AS OnBackOrder " & _
        '"FROM Order_Document LEFT JOIN Customer_Document ON Order_Document.DocumentID = Customer_Document.ID " & _
        '"WHERE Order_Document.OrderID = " & lOrderID & " AND Order_Document.KitID IS NULL " & _
        '"ORDER BY Customer_Document.ReferenceNo "
        myData.GetReaderSQL(cnnR, cmdR, rcdR, sSQLR)
        Do While rcdR.Read()
            If (Convert.ToBoolean(rcdR("OnBackOrder")) = True) Then
                s.Append("<tr><td align='right'></td><td>&nbsp;*B*</td><td>" & rcdR("ItmQty").ToString() & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & rcdR("ItmName").ToString() & " - " & rcdR("ItmDesc").ToString() & "</td></tr>")
            Else
                s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & rcdR("ItmQty").ToString() & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & rcdR("ItmName").ToString() & " - " & rcdR("ItmDesc").ToString() & "</td></tr>")
            End If
        Loop
        myData.ReleaseReaderSQL(cnnR, cmdR, rcdR)
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td></td></tr>")

        'Order - Kit
        sPrevKit = String.Empty
        sSQLR = "SELECT Customer_Document.ReferenceNo AS ItmName, Customer_Document.Description AS ItmDesc, KitID,  " & _
                "Order_Document.QtyOrdered AS ItmQty, " & _
                "CASE [Customer_Document].[Status]  " & _
                "WHEN 2 THEN 1  " & _
                "ELSE 0 " & _
                "END  AS OnBackOrder,  " & _
                "Customer_Kit.ReferenceNo AS KitName " & _
                "FROM Customer_Kit RIGHT JOIN (Order_Document LEFT JOIN Customer_Document ON Order_Document.DocumentID = Customer_Document.ID) ON Customer_Kit.ID = Order_Document.KitID " & _
                "WHERE Order_Document.OrderID = " & lOrderID & " AND NOT(Order_Document.KitID IS NULL) " & _
                "ORDER BY Customer_Kit.ReferenceNo, Customer_Document.ReferenceNo "


        'sSQLR = "SELECT Customer_Document.ReferenceNo AS ItmName, Customer_Document.Description AS ItmDesc, " & _
        '       "Order_Document.QtyOrdered AS ItmQty, " & _
        '       "iif([Customer_Document].Status=" & myData.STATUS_BACK & ",True,False) AS OnBackOrder, " & _
        '       "Customer_Kit.ReferenceNo AS KitName " & _
        '       "FROM Customer_Kit RIGHT JOIN (Order_Document LEFT JOIN Customer_Document ON Order_Document.DocumentID = Customer_Document.ID) ON Customer_Kit.ID = Order_Document.KitID " & _
        '       "WHERE Order_Document.OrderID = " & lOrderID & " AND NOT(Order_Document.KitID IS NULL) " & _
        '       "ORDER BY Customer_Kit.ReferenceNo, Customer_Document.ReferenceNo "

        myData.GetReaderSQL(cnnR, cmdR, rcdR, sSQLR)
        Do While rcdR.Read()
            If (sPrevKit <> rcdR("KitName").ToString()) Then
                s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>KIT - " & rcdR("KitName").ToString() & "</td></tr>")
                s.Append("<tr><td colspan=10>" & paradata.GetKitContents(rcdR("KitID"), "", False, True) & "</td></tr>")
            End If
            If (Convert.ToBoolean(rcdR("OnBackOrder")) = True) Then
                s.Append("<tr><td align='right'></td><td>&nbsp;*B*</td><td>" & rcdR("ItmQty").ToString() & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & rcdR("ItmName").ToString() & " - " & rcdR("ItmDesc").ToString() & "</td></tr>")
            Else
                s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & rcdR("ItmQty").ToString() & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & rcdR("ItmName").ToString() & " - " & rcdR("ItmDesc").ToString() & "</td></tr>")
            End If
            sPrevKit = rcdR("KitName").ToString()
        Loop

        myData.ReleaseReaderSQL(cnnR, cmdR, rcdR)
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td></td></tr>")

        CurrentAuthorizationStatus(bRequiresLI, bRequiresOR, blnDisableAllQuantityLimits)
        If (bRequiresLI) Then s.Append("<tr><td colspan='100%' align='center'></td>" & "Quantity of line items requires authorization." & "</td></tr>")
        If (bRequiresOR) Then s.Append("<tr><td colspan='100%' align='center'></td>" & "Total quantity of items ordered requires authorization." & "</td></tr>")

        s.Append("</table>")
        s.Append("<br>")
        'rcd.Dispose() : rcd = Nothing

        Return s.ToString()
    End Function


    Public Function GetEmailOrderBody(ByVal lOrderID As Long) As String
        Dim s As StringBuilder = New StringBuilder(200)
        'Dim cnnR As OleDb.OleDbConnection
        'Dim cmdR As OleDb.OleDbCommand
        'Dim rcdR As OleDb.OleDbDataReader

        Dim cnnR As SqlClient.SqlConnection
        Dim cmdR As SqlClient.SqlCommand
        Dim rcdR As SqlClient.SqlDataReader

        Dim sSQLR As String = String.Empty
        Dim bRequiresLI As Boolean = False
        Dim bRequiresOR As Boolean = False

        Dim sRequestorTitle As String = String.Empty
        Dim sRequestorFirstName As String = String.Empty
        Dim sRequestorLastName As String = String.Empty
        Dim sRequestorEmail As String = String.Empty
        Dim sShipContactFirstName As String = String.Empty
        Dim sShipContactLastName As String = String.Empty
        Dim sShipCompany As String = String.Empty
        Dim sShipAddress1 As String = String.Empty
        Dim sShipAddress2 As String = String.Empty
        Dim sShipCity As String = String.Empty
        Dim sShipState As String = String.Empty
        Dim sShipZip As String = String.Empty
        Dim sShipCountry As String = String.Empty
        Dim sShipPrefDesc As String = String.Empty
        Dim sShipNote As String = String.Empty
        Dim sCoverSheetDesc As String = String.Empty
        Dim sCoverSheetText As String = String.Empty
        Dim strRequiredDate As String = String.Empty
        Dim objData As New paraData
        Dim sPrevKit As String = String.Empty

        'Order - Base
        sSQLR = "SELECT Requestor_Title, Requestor_FirstName, Requestor_LastName, Requestor_Email, " & _
                "ShipTo_ContactFirstName, ShipTo_ContactLastName, ShipTo_Name, ShipTo_Address1, ShipTo_Address2, ShipTo_City, ShipTo_State, ShipTo_ZipCode, ShipTo_Country, " & _
                "Customer_ShippingMethod.Description AS ShipTo_PrefDescr, ShipNote, " & _
                "Customer_CoverSheet.Name AS CS_Desc, CS_Content, RequiredDate " & _
                "FROM Customer_ShippingMethod RIGHT JOIN ([Order] LEFT JOIN Customer_CoverSheet ON [Order].CoverSheetID = Customer_CoverSheet.ID) ON Customer_ShippingMethod.ID = [Order].PreferredShipID " & _
                "WHERE [Order].ID = " & lOrderID
        objData.GetReaderSQL(cnnR, cmdR, rcdR, sSQLR)

        If rcdR.Read() Then
            sRequestorTitle = rcdR("Requestor_Title")
            sRequestorFirstName = rcdR("Requestor_FirstName")
            sRequestorLastName = rcdR("Requestor_LastName")
            sRequestorEmail = rcdR("Requestor_Email")

            sShipContactFirstName = rcdR("ShipTo_ContactFirstName")
            sShipContactLastName = rcdR("ShipTo_ContactLastName")
            sShipCompany = rcdR("ShipTo_Name")
            sShipAddress1 = rcdR("ShipTo_Address1")
            sShipAddress2 = rcdR("ShipTo_Address2")
            sShipCity = rcdR("ShipTo_City")
            sShipState = rcdR("ShipTo_State")
            sShipZip = rcdR("ShipTo_ZipCode")
            sShipCountry = rcdR("ShipTo_Country")
            sShipPrefDesc = rcdR("ShipTo_PrefDescr")
            sShipNote = rcdR("ShipNote")

            sCoverSheetDesc = rcdR("CS_Desc")
            sCoverSheetText = rcdR("CS_Content")

            If rcdR("RequiredDate") Is DBNull.Value = False Then
                strRequiredDate = rcdR("RequiredDate")
            End If
        End If
        objData.ReleaseReaderSQL(cnnR, cmdR, rcdR)

        s.Append("<br><br>")
        s.Append("<table>")
        s.Append("<tr><td align='right'>" & "Requestor:" & "</td><td>&nbsp;</td><td>" & sRequestorTitle & "</td></tr>")
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & sRequestorFirstName & " " & sRequestorLastName & "</td></tr>")
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & sRequestorEmail & "</td></tr>")
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td></td></tr>")

        'TODO check if order need to show ship info:

        If CheckForDownloadOnlyExistingOrder(lOrderID) = False Then
            s.Append("<tr><td align='right'>" & "Shipping:" & "</td><td>&nbsp;</td><td>" & sShipContactFirstName & " " & sShipContactLastName & "</td></tr>")
            s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & sShipCompany & "</td></tr>")
            s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & sShipAddress1 & "</td></tr>")
            s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & sShipAddress2 & "</td></tr>")
            s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & sShipCity & ", " & sShipState & " " & sShipZip & " " & sShipCountry & "</td></tr>")
            s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & sShipPrefDesc & "</td></tr>")
            s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & sShipNote & "</td></tr>")
            If strRequiredDate <> "" Then
                s.Append("<tr><td align='right'></td><td>Scheduled Delivery:</td><td>" & strRequiredDate & "</td></tr>")
            End If



            s.Append("<tr><td align='right'></td><td>&nbsp;</td><td></td></tr>")

            s.Append("<tr><td align='right'>" & "Cover Sheet:" & "</td><td>&nbsp;</td><td>" & sCoverSheetDesc & "</td></tr>")
            s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & sCoverSheetText & "</td></tr>")


        End If

        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td></td></tr>")
        'Order - Custom
        s.Append("<tr><td align='right'>" & "Custom:" & "</td><td>&nbsp;</td><td></td></tr>")
        sSQLR = "SELECT Customer_CustomField.Name AS FieldName,  " & _
                "Order_CustomField.Value AS FieldValue " & _
                "FROM Order_CustomField LEFT JOIN Customer_CustomField ON Order_CustomField.CustomFieldID = Customer_CustomField.ID " & _
                "WHERE Order_CustomField.OrderID = " & lOrderID & " " & _
                "ORDER BY Customer_CustomField.Name "
        objData.GetReaderSQL(cnnR, cmdR, rcdR, sSQLR)
        Do While rcdR.Read()
            s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & rcdR("FieldName").ToString() & ":&nbsp;&nbsp;" & rcdR("FieldValue").ToString() & "</td></tr>")
        Loop
        objData.ReleaseReaderSQL(cnnR, cmdR, rcdR)
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td></td></tr>")

        'Order - Item
        s.Append("<tr><td align='right'>" & "Details:" & "</td><td>&nbsp;</td><td></td></tr>")
        sSQLR = "SELECT Customer_Document.ReferenceNo AS ItmName, Customer_Document.Description AS ItmDesc, " & _
                "Order_Document.QtyOrdered AS ItmQty, " & _
                "CASE [Customer_Document].[Status]  " & _
                "WHEN 2 THEN 1  " & _
                "ELSE 0 " & _
                "END  AS OnBackOrder  " & _
                "FROM Order_Document LEFT JOIN Customer_Document ON Order_Document.DocumentID = Customer_Document.ID " & _
                "WHERE Order_Document.OrderID = " & lOrderID & " AND Order_Document.KitID IS NULL " & _
                "ORDER BY Customer_Document.ReferenceNo "
        'sSQLR = "SELECT Customer_Document.ReferenceNo AS ItmName, Customer_Document.Description AS ItmDesc, " & _
        '"Order_Document.QtyOrdered AS ItmQty, " & _
        '"iif([Customer_Document].Status=" & myData.STATUS_BACK & ",True,False) AS OnBackOrder " & _
        '"FROM Order_Document LEFT JOIN Customer_Document ON Order_Document.DocumentID = Customer_Document.ID " & _
        '"WHERE Order_Document.OrderID = " & lOrderID & " AND Order_Document.KitID IS NULL " & _
        '"ORDER BY Customer_Document.ReferenceNo "
        objData.GetReaderSQL(cnnR, cmdR, rcdR, sSQLR)
        Do While rcdR.Read()
            If (Convert.ToBoolean(rcdR("OnBackOrder")) = True) Then
                s.Append("<tr><td align='right'></td><td>&nbsp;*B*</td><td>" & rcdR("ItmQty").ToString() & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & rcdR("ItmName").ToString() & " - " & rcdR("ItmDesc").ToString() & "</td></tr>")
            Else
                s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & rcdR("ItmQty").ToString() & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & rcdR("ItmName").ToString() & " - " & rcdR("ItmDesc").ToString() & "</td></tr>")
            End If
        Loop
        objData.ReleaseReaderSQL(cnnR, cmdR, rcdR)
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td></td></tr>")

        'Order - Kit
        sPrevKit = String.Empty
        sSQLR = "SELECT Customer_Document.ReferenceNo AS ItmName, Customer_Document.Description AS ItmDesc, " & _
                "Order_Document.QtyOrdered AS ItmQty, " & _
                "CASE [Customer_Document].[Status]  " & _
                "WHEN 2 THEN 1  " & _
                "ELSE 0 " & _
                "END  AS OnBackOrder,  " & _
                "Customer_Kit.ReferenceNo AS KitName " & _
                "FROM Customer_Kit RIGHT JOIN (Order_Document LEFT JOIN Customer_Document ON Order_Document.DocumentID = Customer_Document.ID) ON Customer_Kit.ID = Order_Document.KitID " & _
                "WHERE Order_Document.OrderID = " & lOrderID & " AND NOT(Order_Document.KitID IS NULL) " & _
                "ORDER BY Customer_Kit.ReferenceNo, Customer_Document.ReferenceNo "


        'sSQLR = "SELECT Customer_Document.ReferenceNo AS ItmName, Customer_Document.Description AS ItmDesc, " & _
        '       "Order_Document.QtyOrdered AS ItmQty, " & _
        '       "iif([Customer_Document].Status=" & myData.STATUS_BACK & ",True,False) AS OnBackOrder, " & _
        '       "Customer_Kit.ReferenceNo AS KitName " & _
        '       "FROM Customer_Kit RIGHT JOIN (Order_Document LEFT JOIN Customer_Document ON Order_Document.DocumentID = Customer_Document.ID) ON Customer_Kit.ID = Order_Document.KitID " & _
        '       "WHERE Order_Document.OrderID = " & lOrderID & " AND NOT(Order_Document.KitID IS NULL) " & _
        '       "ORDER BY Customer_Kit.ReferenceNo, Customer_Document.ReferenceNo "

        objData.GetReaderSQL(cnnR, cmdR, rcdR, sSQLR)
        Do While rcdR.Read()
            If (sPrevKit <> rcdR("KitName").ToString()) Then
                s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>KIT - " & rcdR("KitName").ToString() & "</td></tr>")
            End If
            If (Convert.ToBoolean(rcdR("OnBackOrder")) = True) Then
                s.Append("<tr><td align='right'></td><td>&nbsp;*B*</td><td>" & rcdR("ItmQty").ToString() & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & rcdR("ItmName").ToString() & " - " & rcdR("ItmDesc").ToString() & "</td></tr>")
            Else
                s.Append("<tr><td align='right'></td><td>&nbsp;</td><td>" & rcdR("ItmQty").ToString() & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & rcdR("ItmName").ToString() & " - " & rcdR("ItmDesc").ToString() & "</td></tr>")
            End If
            sPrevKit = rcdR("KitName").ToString()
        Loop
        objData.ReleaseReaderSQL(cnnR, cmdR, rcdR)
        s.Append("<tr><td align='right'></td><td>&nbsp;</td><td></td></tr>")

        'CurrentAuthorizationStatus(bRequiresLI, bRequiresOR, blnDisableAllQuantityLimits)
        'If (bRequiresLI) Then s.Append("<tr><td colspan='100%' align='center'></td>" & "Quantity of line items requires authorization." & "</td></tr>")
        'If (bRequiresOR) Then s.Append("<tr><td colspan='100%' align='center'></td>" & "Total quantity of items ordered requires authorization." & "</td></tr>")

        s.Append("</table>")
        s.Append("<br>")
        'rcd.Dispose() : rcd = Nothing

        Return s.ToString()
    End Function

    'Order Save Section - End
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''


    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '''Inventory Section - Start
    ''Function GetFulfilledInventory(ByVal bShowActiveOnly)
    ''  Dim CustomerID
    ''  Dim sPath
    ''  Dim sCnn
    ''  Dim cnn
    ''  Dim rcd

    ''  CustomerID = CLng(Session("CustomerID"))


    ''  If AccessInventory Then
    ''    sPath = Server.MapPath(GetDBPath())
    ''    sCnn = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & sPath
    ''    cnn = Server.CreateObject("ADODB.Connection")
    ''    On Error Resume Next : cnn.open(sCnn)

    ''    If Not (cnn Is Nothing) Then
    ''      rcd = Server.CreateObject("ADODB.Recordset")

    ''      'sSQL = "SELECT Customer_Document.ID, Customer_Document.ReferenceNo, Customer_Document.Description, QtyOH, QtyPerCtn, CtnPerSkid " & _
    ''      '       "FROM Customer_Document INNER JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID" & _
    ''      '       " WHERE (Customer_Document.CustomerID = " & CustomerID & ") AND (Customer_Document.Printable=False) "
    ''      'sSQL = "SELECT Customer_Document.ID, Customer_Document.ReferenceNo, Customer_Document.Description, QtyOH, QtyPerCtn, CtnPerSkid " & _
    ''      '       "FROM Customer_Document INNER JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID" & _
    ''      '       " WHERE (Customer_Document.CustomerID = " & CustomerID & ") AND (Customer_Document.PrintMethod IN (" & DT_FILLONLY & ", " & DT_FILLPRINT  & ")) "
    ''      sSQL = "SELECT Customer_Document.ID, Customer_Document.ReferenceNo, Customer_Document.Description, QtyOH, QtyPerCtn, CtnPerSkid, " & _
    ''             "IIf([Customer_Document].[Status]=2,'Backorder',IIf([Customer_Document].[Status]=0,'Inactive','')) AS DocStatus " & _
    ''             "FROM Customer_Document INNER JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID" & _
    ''             " WHERE (Customer_Document.CustomerID = " & CustomerID & ") AND (Customer_Document.PrintMethod IN (" & DT_FILLONLY & ", " & DT_FILLPRINT & ")) "

    ''      If (bShowActiveOnly) Then
    ''        sSQL = sSQL & " AND (Status >" & STATUS_INAC & ") "
    ''      End If

    ''      sSQL = sSQL & "ORDER BY Customer_Document.ReferenceNo "

    ''      rcd.Open(sSQL, cnn)

    ''      'cnn.close - using disconnected
    ''    End If
    ''    cnn = Nothing
    ''  End If

    ''  GetFulfilledInventory = rcd
    ''End Function


    ''Function GetUsage(ByVal sFromDate, ByVal sToDate)
    ''  'Find all Items Used in Orders (NOT cancelled) within date range

    ''  Dim CustomerID
    ''  Dim sPath
    ''  Dim sCnn
    ''  Dim cnn
    ''  Dim rcd

    ''  CustomerID = CLng(Session("CustomerID"))

    ''  If AccessInventory Then
    ''    sPath = Server.MapPath(GetDBPath())
    ''    sCnn = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & sPath
    ''    cnn = Server.CreateObject("ADODB.Connection")
    ''    On Error Resume Next : cnn.open(sCnn)

    ''    If Not (cnn Is Nothing) Then
    ''      rcd = Server.CreateObject("ADODB.Recordset")

    ''      ''      sSQL = "SELECT Customer_Document.ReferenceNo, Customer_Document.Description, SUM(Customer_Document_Fill.QtyPerPull * Order_Document.QtyOrdered) AS SumOfQtyOrdered " & _
    ''      ''             "FROM [Order] LEFT JOIN ((Order_Document LEFT JOIN Customer_Document ON Order_Document.DocumentID = Customer_Document.ID) LEFT JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID) ON Order.ID = Order_Document.OrderID " & _
    ''      ''             " WHERE (Customer_Document.CustomerID = " & CustomerID & ") AND (Customer_Document.Printable=False) " & _
    ''      ''             " AND (Order.StatusID <> 4) AND (Order.RequestDate>=#" & sFromDate & " 12:00 am#) AND (Order.RequestDate<=#" & sToDate & " 11:59 pm#) " & _
    ''      ''             " GROUP BY Customer_Document.ID, Customer_Document.ReferenceNo, Customer_Document.Description"
    ''      sSQL = "SELECT Customer_Document.ReferenceNo, Customer_Document.Description, SUM(IIF(ISNULL(Customer_Document_Fill.ID),1,Customer_Document_Fill.QtyPerPull) * Order_Document.QtyOrdered) AS SumOfQtyOrdered " & _
    ''             "FROM [Order] LEFT JOIN ((Order_Document LEFT JOIN Customer_Document ON Order_Document.DocumentID = Customer_Document.ID) LEFT JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID) ON Order.ID = Order_Document.OrderID " & _
    ''             " WHERE (Customer_Document.CustomerID = " & CustomerID & ") " & _
    ''             " AND (Order.StatusID <> 4) AND (Order.RequestDate>=#" & sFromDate & " 12:00 am#) AND (Order.RequestDate<=#" & sToDate & " 11:59 pm#) " & _
    ''             " GROUP BY Customer_Document.ID, Customer_Document.ReferenceNo, Customer_Document.Description"
    ''      sSQL = sSQL & " ORDER BY Customer_Document.ReferenceNo, Customer_Document.Description"

    ''      rcd.Open(sSQL, cnn)

    ''      'cnn.close - using disconnected
    ''    End If
    ''    cnn = Nothing
    ''  End If

    ''  GetUsage = rcd
    ''End Function
    '''Inventory Section - End
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'mw - 06-30-2007
    Friend Function LoadFromPrior(ByRef oData As paraData, ByVal lAccID As Long, ByVal lOrderID As Long, ByVal bReq As Boolean, ByVal bShp As Boolean, ByVal bCst As Boolean, ByVal bCV As Boolean, Optional bCustomFields As Boolean = True)
        Dim myData As paraData
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim rcd As OleDb.OleDbDataReader
        Dim sSQL As String
        Dim sType As String
        Dim lID As Integer
        Dim sValue As String
        Dim bSuccess As Boolean

        Try
            If (oData Is Nothing) Then
                myData = New paraData
            Else
                myData = oData
            End If

            If (lOrderID = 0) Then
                sSQL = "SELECT MAX([Order].ID) AS OrderID FROM [Order] WHERE AccessCodeID = " & lAccID
                myData.GetReader2(cnn, cmd, rcd, sSQL)
                If Not (rcd Is Nothing) Then
                    rcd.Read()
                    If rcd.HasRows AndAlso IsNumeric(rcd("OrderID")) Then
                        lOrderID = rcd("OrderID") & ""
                    End If
                End If
                myData.ReleaseReader2(cnn, cmd, rcd)
            End If

            If (lOrderID > 0) Then
                'Lookup Order Data
                sSQL = "SELECT [Order].* FROM [Order] WHERE [Order].ID = " & lOrderID
                myData.GetReader2(cnn, cmd, rcd, sSQL)
                If Not (rcd Is Nothing) Then
                    rcd.Read()
                    If rcd.HasRows Then
                        If (bReq = True) Then
                            pRequestorTitle = rcd("Requestor_Title") & ""
                            pRequestorFirstName = rcd("Requestor_FirstName") & ""
                            pRequestorLastName = rcd("Requestor_LastName") & ""
                            pRequestorEmail = rcd("Requestor_Email") & ""
                        End If

                        If (bShp = True) Then
                            pShipContactFirstName = rcd("ShipTo_ContactFirstName") & ""
                            pShipContactLastName = rcd("ShipTo_ContactLastName") & ""
                            pShipCompany = rcd("ShipTo_Name") & ""
                            pShipAddress1 = rcd("ShipTo_Address1") & ""
                            pShipAddress2 = rcd("ShipTo_Address2") & ""
                            pShipCity = rcd("ShipTo_City") & ""
                            pShipState = rcd("ShipTo_State") & ""
                            pShipZip = rcd("ShipTo_ZipCode") & ""
                            pShipCountry = rcd("ShipTo_Country") & ""
                            pShipPrefID = rcd("PreferredShipID") & ""

                            'wipe out special ship instructions:
                            'pShipNote = rcd("ShipNote") & ""
                            pShipNote = ""
                            If (bCV = True) Then
                                pCoverSheetID = rcd("CoverSheetID") & ""
                                'wipe out special ship instructions:
                                'pCoverSheetText = rcd("CS_Content") & ""
                                pCoverSheetText = ""

                            End If
                        End If

                        bSuccess = True
                    End If
                End If
                myData.ReleaseReader2(cnn, cmd, rcd)

                If bCustomFields Then
                    'Lookup Order Custom Data
                    sSQL = "SELECT IIF([Customer_CustomField].CustomType=1,'R',IIF([Customer_CustomField].CustomType=2,'S','C')) AS CustomType, " & _
                           "[Order_CustomField].* " & _
                           "FROM [Order_CustomField] INNER JOIN [Customer_CustomField] ON [Order_CustomField].CustomFieldID = [Customer_CustomField].ID " & _
                           "WHERE [Order_CustomField].OrderID = " & lOrderID
                    myData.GetReader2(cnn, cmd, rcd, sSQL)
                    If Not (rcd Is Nothing) Then
                        Do While rcd.Read()
                            sType = rcd("CustomType").ToString
                            lID = Convert.ToInt32(rcd("CustomFieldID").ToString)
                            sValue = rcd("Value").ToString
                            Select Case sType
                                Case "R"
                                    If (bReq = True) Then
                                        LoadFromPrior_CustomValue(sType, lID, sValue)
                                    End If
                                Case "S"
                                    If (bShp = True) Then
                                        LoadFromPrior_CustomValue(sType, lID, sValue)
                                    End If
                                Case "C"
                                    If (bCst = True) Then
                                        LoadFromPrior_CustomValue(sType, lID, sValue)
                                    End If
                            End Select
                        Loop
                    End If
                    myData.ReleaseReader2(cnn, cmd, rcd)
                End If

            End If
            If (oData Is Nothing) Then myData.Dispose()

            myData = Nothing
            Return bSuccess

        Catch ex As Exception
            If (oData Is Nothing) Then myData.Dispose()
            myData = Nothing
        End Try
    End Function

    Private Function LoadFromPrior_CustomValue(ByVal sType As String, ByVal lID As Integer, ByVal sValue As String)
        Const COL_FLDID = 1
        Const COL_FLDVAL = 5

        Dim objDT As System.Data.DataTable
        Dim objDR As System.Data.DataRow
        Dim indx As Integer : indx = 0
        Dim iMaxIndx As Integer : iMaxIndx = 0
        Dim iRow As Integer : iRow = -1
        Dim bSuccess As Boolean

        Try
            objDT = pCurrentCstCart

            iMaxIndx = objDT.Rows.Count - 1
            For indx = 0 To iMaxIndx
                If (lID = objDT.Rows(indx).Item(COL_FLDID)) Then
                    iRow = indx
                End If
            Next indx

            If (lID > 0) And (iRow >= 0) Then
                'Update
                objDT.Rows(iRow).Item(COL_FLDVAL) = sValue
            End If

            pCurrentCstCart = objDT

            objDT = Nothing

            bSuccess = True
            Return bSuccess
        Catch ex As Exception
        End Try
    End Function
    '
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


    Public Sub LogActivity(ByVal sMsg As String, Optional ByVal sFile As String = "")
        Dim moLog As Log
        Dim sLogFile As String = String.Empty

        Try
            If (sFile.Length > 0) Then
                sLogFile = sFile
            Else
                sLogFile = ConfigurationSettings.AppSettings("LogFolder") & "Order.log"
            End If

            moLog = New Log
            moLog.Entry(sLogFile, "      " & Format(Now, "MM-dd-yyyy hh:mm:ss") & " - ORDER:  " & sMsg)

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
                sLogFile = ConfigurationSettings.AppSettings("LogFolder") & "OrderErr.log"
            End If
            moLog = New Log
            moLog.Entry(sLogFile, "      " & Format(Now, "MM-dd-yyyy hh:mm:ss") & " **ERROR** ORDER:  " & sMsg)

        Catch ex As Exception

        End Try
        moLog = Nothing
    End Sub


    Protected Overrides Sub Finalize()
        pCurrentCstCart = Nothing
        pCurrentItmCart = Nothing
        pCurrentKitCart = Nothing
        MyBase.Finalize()
    End Sub


    '---------
    Sub PopulateOrderData(ByVal intOrderID As Integer, ByVal currentUser As paraUser)
        Dim ta As New dsOrdersTableAdapters.dsOrdersWithAccessCodesTableAdapter
        Dim taCS As New dsOrdersTableAdapters.Customer_CoverSheetTableAdapter
        Dim taKits As New dsOrdersTableAdapters.dsOrderKitDetailsTableAdapter
        Dim taDetails As New dsOrdersTableAdapters.dsOrderDetailsCustomTableAdapter
        Dim taOrderCustomFields As New dsOrdersTableAdapters.dtOrderCustomFieldsTableAdapter
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        Dim strWebSet As String = ""

        'gvOrderItems.DataSource = taDetails.GetByOrderID(intOrderID)

        'gvOrderItems.DataBind()

        'hfOrderID.Value = intOrderID
        For Each dr As dsOrders.dsOrdersWithAccessCodesRow In ta.GetByOrderID(intOrderID)
            With Me

                For Each drCS As dsOrders.Customer_CoverSheetRow In taCS.GetByCoverSheetID(dr.CoverSheetID)

                    .CoverSheetDesc = drCS.Name
                    .CoverSheetID = dr.CoverSheetID
                    .CoverSheetText = drCS.Note
                Next

                .CustomerEmail = dr.Email
                .RequestorEmail = dr.Requestor_Email
                .RequestorFirstName = dr.Requestor_FirstName
                .RequestorLastName = dr.Requestor_LastName

                Try

                Catch ex As Exception
                    .RequestorTitle = dr.Requestor_Title
                End Try
                Try
                    .ShipAddress1 = dr.ShipTo_Address1
                Catch ex As Exception

                End Try
                Try
                    .ShipAddress2 = dr.ShipTo_Address2
                Catch ex As Exception

                End Try

                .ShipCity = dr.ShipTo_City
                .ShipCompany = dr.ShipTo_Name
                .ShipContactFirstName = dr.ShipTo_ContactFirstName
                .ShipContactLastName = dr.ShipTo_ContactLastName
                .ShipCountry = dr.ShipTo_Country
                Try
                    .ShipNote = dr.ShipNote
                Catch ex As Exception

                End Try

                .ShipState = dr.ShipTo_State
                .ShipZip = dr.ShipTo_ZipCode
                .ShipPrefID = dr.PreferredShipID
                .ShipPrefDesc = qa.GetShippingMethodName(currentUser.CustomerID, dr.PreferredShipID)
                .EditOrder = True
                .ItmCart = taDetails.GetData(intOrderID)
                .KitCart = taKits.GetData(intOrderID)
                .OrderID = intOrderID
                Try
                    .CoverSheetText = dr.CS_Content()
                Catch ex As Exception

                End Try

                .StatusID = dr.StatusID
                .CstCart = taOrderCustomFields.GetData(intOrderID)
                .AccessCodeID = dr.AccessCodeID
                .AccessCode = qa.GetAccessCode(dr.AccessCodeID)
                .DisableAllQuantityLimits = qa.GetDisableAllQuantityLimitsByAccessCodeID(dr.AccessCodeID)

                strWebSet = qa.GetWebSet(dr.AccessCodeID)

                If .DisableAllQuantityLimits Then
                    .CanExtendQtyLI = True
                    .CanExtendQtyOR = True
                Else
                    If (Len(strWebSet) > 0) Then
                        .CanExtendQtyLI = (Mid(strWebSet, 6, 1) = 1)
                        .CanExtendQtyOR = (Mid(strWebSet, 7, 1) = 1)
                    End If
                End If


                .MaxQtyPerOrder = qa.GetMaxQtyPerOrder(dr.AccessCodeID)
                .MaxQtyPerLineItem = qa.GetMaxQtyPerLineItem(dr.AccessCodeID)
            End With




        Next
    End Sub
    Public Sub UpdatePONumber(ByVal intOrderNumber As Integer)
        Dim ta As New dsOrdersTableAdapters.OrderTableAdapter
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        Dim dr As dsOrders.OrderRow

        Dim strPO As Object = qa.GetPurchaseReferenceByOrderNumber(intOrderNumber)

        If strPO Is Nothing = False Then
            dr = ta.GetOrderDataByID(intOrderNumber).Rows(0)
            dr.PurchaseReference = strPO
            ta.Update(dr)
        End If



    End Sub
    Public Sub DeleteOrder(ByVal lOrderID As Long)
        Dim qa As New dsStatusesTableAdapters.QueriesTableAdapter
        qa.DeleteCustomFieldsFromOrder(lOrderID)
        qa.DeleteOrderDocumentsFromOrder(lOrderID)
        qa.DeleteOrderHeader(lOrderID)
    End Sub

    Public Function DateDiffB(ByVal date1 As Date, ByVal date2 As Date) As Integer
        Dim startDate As Date = IIf(date1 < date2, date1, date2)
        Dim endDate As Date = IIf(date1 > date2, date1, date2)
        Dim difference As TimeSpan = endDate - startDate
        Dim totalDays As Integer = difference.Days
        Dim weeks As Integer = totalDays \ 7
        Dim days As Integer = totalDays Mod 7
        Dim businessDays As Integer = weeks * 5

        For offset As Integer = 1 To days
            Select Case startDate.AddDays(offset).DayOfWeek
                Case DayOfWeek.Monday, _
                     DayOfWeek.Tuesday, _
                     DayOfWeek.Wednesday, _
                     DayOfWeek.Thursday, _
                     DayOfWeek.Friday
                    businessDays += 1
            End Select
        Next
        If DateDiff(DateInterval.Day, date1, date2) < 0 Then
            businessDays = -businessDays
        End If
        Return businessDays
    End Function
    Public Function GetKitPrice(ByVal intKitID As Integer, ByVal decLineKitChg As Decimal, ByVal intKitQty As Integer) As Decimal
        Dim ta As New dsOrdersTableAdapters.KitItemsTableAdapter
        Dim decPrice As Decimal = 0
        For Each dr As dsOrders.dtKitItemsRow In ta.GetKitItems(intKitID).Rows
            decPrice += (GetItemPrice(dr.DocumentID, dr.Qty * intKitQty) + decLineKitChg) * dr.Qty
        Next
        Return decPrice
    End Function
    Public Function GetItemPrice(ByVal intItemID As Integer, ByVal intReqQty As Integer) As Decimal
        Try


            Dim ta As New dsInventoryTableAdapters.dtPriceTableAdapter
            For Each dr As dsInventory.dtPriceRow In ta.GetDataByID(intItemID).Rows
                Select Case dr.PrintMethod.ToUpper
                    Case "DNL"
                        Return dr.PricePerDownload
                    Case "FUL"
                        'get FUL price
                        Return GetFULPrice(dr, intItemID, intReqQty)
                    Case "POD"
                        If dr.Item("FormatPrice") Is DBNull.Value = False Then
                            Return dr.FormatPrice
                        Else
                            Return 0
                        End If

                    Case Else


                        'If (lQty <= lQtyFillRemain) Then
                        '    lQtyToFill = lQty
                        'Else
                        '    'Do not want orders split between part fill/print - all or nothing           
                        '    lQtyToFill = 0
                        '    lQtyToPrint = lQty
                        'End If
                        Dim lQtyFillRemain As Long
                        Dim myData As New paraData


                        GetDocAvailability(myData, intItemID, 2, lQtyFillRemain)


                        If intReqQty <= lQtyFillRemain Then
                            'FUL
                            Return GetFULPrice(dr, intItemID, intReqQty)
                        Else
                            'POD
                            If dr.Item("FormatPrice") Is DBNull.Value = False Then
                                Return dr.FormatPrice
                            Else
                                Return 0
                            End If
                        End If

                End Select
            Next
        Catch ex As Exception
            Return -1
        End Try
    End Function
    Private Function GetFULPrice(ByVal dr As dsInventory.dtPriceRow, ByVal intItemID As Integer, ByVal intReqQty As Integer) As Decimal
        Try


            Dim iQtyPerPkA As Integer = dr.QtyPerPkA
            Dim iQtyPerPkB As Integer = dr.QtyPerPkB
            Dim iQtyPerPull As Integer = dr.QtyPerPull
            Dim bPrePack As Boolean
            Dim gFillPrice As Decimal
            Dim decPrice As Decimal
            Dim lCtnCnt As Long
            Dim decLinePckChg As Decimal
            Dim decCtnHdChg As Decimal
            Dim decLinePckChgPak As Decimal

            If dr.Item("CtnHdChg") Is DBNull.Value = False Then
                decCtnHdChg = dr.CtnHdChg
            End If

            If dr.Item("LinePckChgPak") Is DBNull.Value = False Then
                decLinePckChgPak = dr.LinePckChgPak
            End If

            If dr.Item("LinePckChg") Is DBNull.Value = False Then
                decLinePckChg = dr.LinePckChg
            End If


            bPrePack = (intReqQty = 1) Or (intReqQty = iQtyPerPkA) Or (intReqQty = iQtyPerPkB)
            gFillPrice = dr.QtyPerPull * dr.FillPrice


            If (dr.QtyPerCtn > 0) Then

                lCtnCnt = ((intReqQty * iQtyPerPull) \ dr.QtyPerCtn) - 1
                lCtnCnt = IIf(lCtnCnt < 0, 0, lCtnCnt)
            End If

            If bPrePack Then

                ' gFillPrice & " + (Customer.LinePckChgPak/" & lQtyToFill & ") + iif(" & lCtnCnt & ">0, (" & lCtnCnt & "*Customer.CtnHdChg)/" & lQtyToFill & ",0)"
                decPrice = gFillPrice + decLinePckChgPak / intReqQty
                If lCtnCnt > 0 Then
                    decPrice += lCtnCnt * decCtnHdChg / intReqQty
                End If
            Else
                '+ iif(" & lCtnCnt & ">0, (" & lCtnCnt & "*Customer.CtnHdChg)/" & lQtyToFill & ",0) 
                decPrice = gFillPrice + (decLinePckChg / intReqQty)
                If lCtnCnt > 0 Then
                    decPrice += (lCtnCnt * decCtnHdChg) / intReqQty
                End If
            End If
            'Return dr.DocumentCost

            Return decPrice
        Catch ex As Exception
            Return -2
        End Try
    End Function


    Function GetCustomFields(ByVal sCustomerID As String, ByVal bCustomDetail As Boolean, ByVal intOrderID As Integer) As DataTable
        Dim objDT As System.Data.DataTable
        Dim objDR As System.Data.DataRow
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        objDT = New System.Data.DataTable("CstCart")
        objDT.Columns.Add("ID", GetType(Integer))
        objDT.Columns("ID").AutoIncrement = True
        objDT.Columns("ID").AutoIncrementSeed = 1

        objDT.Columns.Add("FieldID", GetType(Integer))
        objDT.Columns.Add("FieldRequire", GetType(Integer))
        objDT.Columns.Add("FieldEntry", GetType(Integer))
        objDT.Columns.Add("FieldName", GetType(String))
        objDT.Columns.Add("FieldValue", GetType(String))
        objDT.Columns.Add("UserEntry", GetType(String))
        objDT.Columns.Add("FieldType", GetType(String))


        Dim myData As paraData
        Dim cnn As SqlConnection
        Dim cmd As SqlCommand
        Dim dr As SqlDataReader
        Dim sSQL As String
        Dim indx As Integer
        Dim sID As String
        Dim sName As String
        Dim bRequire As Boolean
        Dim bEntry As Boolean
        Dim sEntry As String
        Dim iType As Int16

        myData = New paraData

        'sSQL = "SELECT Customer_CustomField.ID, Customer_CustomField.Name, Customer_CustomField.Require, Customer_CustomField.CustomType, Customer_CustomField.Entry, iif(lkp.CustomFieldID is NULL,0,lkp.Cnt) AS SelectCnt " & _
        '       "FROM (Customer INNER JOIN Customer_CustomField ON Customer.ID = Customer_CustomField.CustomerID) LEFT JOIN " & _
        '       "(SELECT COUNT(ID) AS Cnt, CustomFieldID FROM Lkp_CustomField GROUP BY CustomFieldID) lkp " & _
        '       "ON Customer_CustomField.ID =lkp.CustomFieldID " & _
        '       "WHERE Customer_CustomField.CustomerID = " & sCustomerID

        sSQL = "SELECT Customer_CustomField.ID, Customer_CustomField.Name, Customer_CustomField.Require, Customer_CustomField.CustomType, Customer_CustomField.[Entry], CASE   WHEN lkp.CustomFieldID is NULL THEN 0  ELSE lkp.Cnt END as SelectCnt " & _
       "FROM (Customer INNER JOIN Customer_CustomField ON Customer.ID = Customer_CustomField.CustomerID) LEFT JOIN " & _
       "(SELECT COUNT(ID) AS Cnt, CustomFieldID FROM Lkp_CustomField GROUP BY CustomFieldID) lkp " & _
       "ON Customer_CustomField.ID =lkp.CustomFieldID " & _
       "WHERE Customer_CustomField.Active= 1 AND Customer_CustomField.CustomerID = " & sCustomerID

        If (bCustomDetail = False) Then
            'sSQL = sSQL & " AND  Customer_CustomField.DetailData = False "
            '0-Detail / 1-Requestor / 2-Shipping
            sSQL = sSQL & " AND  Customer_CustomField.CustomType > 0 "
        End If
        'sSQL = sSQL & " ORDER BY DetailData"
        sSQL = sSQL & " ORDER BY Customer_CustomField.CustomType, Customer_CustomField.SeqNo, Customer_CustomField.Name"

        If (sSQL.Length > 0) Then
            myData.GetReaderSQL(cnn, cmd, dr, sSQL)
            Do While dr.Read()
                indx = indx + 1

                sID = dr("ID").ToString()
                sName = dr("Name").ToString()
                bRequire = Convert.ToBoolean(dr("Require").ToString())
                bEntry = Convert.ToBoolean(dr("Entry").ToString())
                sEntry = IIf(dr("SelectCnt") > 0, "Select", "Type")
                iType = dr("CustomType")

                objDR = objDT.NewRow()
                objDR("FieldID") = sID
                objDR("FieldRequire") = bRequire
                objDR("FieldEntry") = bEntry
                objDR("FieldName") = sName
                objDR("FieldValue") = qa.GetCustomFieldValue(intOrderID, sID)
                objDR("UserEntry") = sEntry
                objDR("FieldType") = iType

                objDT.Rows.Add(objDR)
            Loop
        End If


        myData.ReleaseReaderSQL(cnn, cmd, dr)
        myData = Nothing

        Return objDT
    End Function
    Function CheckIfCustomItemShows(intValueID As Integer, intCustomFieldID As Integer) As Boolean
        Dim ta As New Customer_CustomFields_SwitchTableAdapter

        If ta.GetDataByCustomFieldIDValueID(intCustomFieldID, intValueID).Rows.Count > 0 Then
            Dim dr As dsCustomer.Customer_CustomFields_SwitchRow
            dr = ta.GetDataByCustomFieldIDValueID(intCustomFieldID, intValueID).Rows(0)
            Return dr.Show
        Else
            Return True
        End If

    End Function

    Function CheckIfKitOnlyOrder(ByVal currentOrder As paraOrder) As Boolean
        Dim bKitOnly As Boolean = False

        If currentOrder.KitCart.Rows.Count > 0 And currentOrder.ItmCart.Rows.Count = 0 Then
            bKitOnly = True
        End If

        Return bKitOnly
    End Function

    Function CheckForDownloadOnlyKit(rcd As System.Data.DataTable) As Boolean
        Dim indx As Integer
        Dim iMaxIndx As Integer
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
        Dim kitID As Integer = 0

        Dim pd As New paraData
        Dim ta As New dsCustomerDocumentTableAdapters.Customer_kit_with_documentsTableAdapter
        Dim dt As New dsCustomerDocument.Customer_kit_with_documentsDataTable
        Dim o As New paraData
        Dim blnDownloadOnly As Boolean = True

        iMaxIndx = rcd.Rows.Count
        indx = 0
        Do While indx < iMaxIndx

            If rcd.Rows(indx).RowState <> DataRowState.Deleted Then
                Dim intPrintMethod As Integer = 0

                With rcd.Rows(indx)

                    kitID = .Item("KitID").ToString()

                    ta.Fill(dt, kitID)

                    If dt.Rows.Count > 0 Then
                        For Each dr As dsCustomerDocument.Customer_kit_with_documentsRow In dt.Rows

                            intPrintMethod = qa.getDocumentPrintMethod(dr.DocumentID)

                            If intPrintMethod <> 3 Then
                                Return False
                            End If
                        Next
                    End If
                End With

            End If

            indx = indx + 1
        Loop

        If indx = 0 Then
            Return False
        Else
            Return blnDownloadOnly
        End If

    End Function

    Function CheckForDownloadOnly(rcd As System.Data.DataTable, Optional ByVal kitrcd As System.Data.DataTable = Nothing) As Boolean

        Dim indx As Integer
        Dim iMaxIndx As Integer
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
        Dim blnDownloadOnly As Boolean = True
        Dim kitID As Integer = 0
        Dim pd As New paraData
        Dim ta As New dsCustomerDocumentTableAdapters.Customer_kit_with_documentsTableAdapter
        Dim dt As New dsCustomerDocument.Customer_kit_with_documentsDataTable

        iMaxIndx = rcd.Rows.Count
        indx = 0
        Do While indx < iMaxIndx


            If rcd.Rows(indx).RowState <> DataRowState.Deleted Then
                Dim intPrintMethod As Integer = 0
                With rcd.Rows(indx)
                    intPrintMethod = qa.getDocumentPrintMethod(.Item("ItmID").ToString())

                    If intPrintMethod <> 3 Then
                        Return False
                    End If



                End With

            End If

            indx = indx + 1
        Loop

        'check for kits as well
        'added by Ted to deal with issue of address being set to N/A
        If kitrcd IsNot Nothing Then
            iMaxIndx = kitrcd.Rows.Count
            indx = 0
            Do While indx < iMaxIndx

                If kitrcd.Rows(indx).RowState <> DataRowState.Deleted Then
                    Dim intPrintMethod As Integer = 0

                    With kitrcd.Rows(indx)

                        kitID = .Item("KitID").ToString()

                        ta.Fill(dt, kitID)

                        If dt.Rows.Count > 0 Then
                            For Each dr As dsCustomerDocument.Customer_kit_with_documentsRow In dt.Rows

                                intPrintMethod = qa.getDocumentPrintMethod(dr.DocumentID)

                                If intPrintMethod <> 3 Then
                                    Return False
                                End If
                            Next
                        End If
                    End With

                End If

                indx = indx + 1
            Loop
        End If


        'If indx = 0 Then
        '    Return False
        'Else
            Return blnDownloadOnly
        'End If



    End Function

    Function CheckIfItemInCart(rcd As System.Data.DataTable, iItmID As Integer) As Boolean


        Dim indx As Integer
        Dim iMaxIndx As Integer
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
        Dim blnDownloadOnly As Boolean = True

        iMaxIndx = rcd.Rows.Count
        indx = 0
        Do While indx < iMaxIndx

          
                With rcd.Rows(indx)


                If iItmID.ToString = .Item("ItmID").ToString() Then
                    Return True
                End If


                End With


            indx = indx + 1
        Loop

        Return False

    End Function

    Function CheckIfHasNormalItems(rcd As System.Data.DataTable) As Boolean

        Dim indx As Integer
        Dim iMaxIndx As Integer
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
        Dim blnDownloadOnly As Boolean = False

        iMaxIndx = rcd.Rows.Count
        indx = 0
        Do While indx < iMaxIndx


            If rcd.Rows(indx).RowState <> DataRowState.Deleted Then
                Dim intPrintMethod As Integer = 0
                With rcd.Rows(indx)
                    intPrintMethod = qa.getDocumentPrintMethod(.Item("ItmID").ToString())

                    If intPrintMethod <> 3 Then
                        Return True
                    End If



                End With

            End If

            indx = indx + 1
        Loop

        Return blnDownloadOnly
    End Function

    Function CheckForDownloadOnlyExistingOrder(iOrderID As Integer) As Boolean

        Dim blnDownloadOnly As Boolean = True

        Dim ta As New dsOrdersTableAdapters.Order_Document1TableAdapter
        For Each dr As dsOrders.Order_Document1Row In ta.GetDataByOrderID(iOrderID).Rows

            If dr.PrintMethod <> 2 Then
                Return False
            End If
        Next

    End Function

    Function CheckIfHasDownloadItems(rcd As System.Data.DataTable) As Boolean

        Dim indx As Integer
        Dim iMaxIndx As Integer
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
        Dim blnDownloadOnly As Boolean = False

        iMaxIndx = rcd.Rows.Count
        indx = 0
        Do While indx < iMaxIndx


            If rcd.Rows(indx).RowState <> DataRowState.Deleted Then
                Dim intPrintMethod As Integer = 0
                With rcd.Rows(indx)
                    intPrintMethod = qa.getDocumentPrintMethod(.Item("ItmID").ToString())

                    If intPrintMethod = 3 Then
                        Return True
                    End If



                End With

            End If

            indx = indx + 1
        Loop

        Return blnDownloadOnly
    End Function

    Function ActivateDownloadableItems(iOrderID As Integer, iAccessCode As Integer) As String
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        Dim blnDownloadOnly As Boolean = True
        Dim ta As New dsOrdersTableAdapters.Order_Document1TableAdapter
        Dim strEmailText As String = ""
        qa.ActivateDownloadableItems(iOrderID)

        For Each dr As dsOrders.Order_Document1Row In ta.GetDataByOrderID(iOrderID)
            If dr.PrintMethod <> 2 Then
                blnDownloadOnly = False
            End If
        Next

        If blnDownloadOnly Then
            'switch order status to shipped: //3
            qa.SetOrderShipped(iOrderID)
        End If
        qa.SetDownloadPrice(iOrderID)

        strEmailText = "<table><tr><td>You have requested the following digital files from the " & qa.GetCustomerNameByOrderID(iOrderID) & " collateral fulfillment site. Please click on the links below and follow all appropriate directions to retrieve your files. Note that these links will only remain active for a period of 48 hours from the time your request was placed." & _
            "</td></tr></table>" & GetDownloadLinks(iOrderID, iAccessCode)


        LogActivity("    ActivateDownloadableItems Email text..." & strEmailText, msLogFile)
        Return strEmailText
    End Function

    Function GetDownloadLinks(intOrderID As Integer, iAccessCode As Integer) As String
        Dim ta As New dsOrdersTableAdapters.Order_Document1TableAdapter
        Dim taD As New dsOrdersTableAdapters.Order_Document_DownloadTableAdapter
        Dim drD As dsOrders.Order_Document_DownloadRow
        Dim dt As New dsOrders.Order_Document_DownloadDataTable
        Dim strLinks As String = "<table>"
        Dim qa As New dsCustomerTableAdapters.QueriesTableAdapter



        For Each dr As dsOrders.Order_Document1Row In ta.GetDataByOrderID(intOrderID)
            If dr.PrintMethod = 2 Then
                drD = dt.NewOrder_Document_DownloadRow
                Dim MyGuid As Guid = Guid.NewGuid()
                With drD

                    .ExpirationDate = Now.AddDays(3)
                    .guid = MyGuid.ToString
                    .OrderDocumentID = dr.ID
                    .CustomerID = qa.GetCustomerIDFromAccessCodeID(iAccessCode)
                End With
                dt.Rows.Add(drD)
                taD.Update(dt)

                strLinks += GetAllLinksForItem(dr.DocumentID, MyGuid.ToString, iAccessCode, dr.OrderID)

            End If
        Next


        strLinks += "</table>"
        Return strLinks
    End Function


    Function GetAllLinksForItem(iDocumentID As Integer, strGuid As String, iAccessCode As Integer, iOrderNumber As Integer) As String
        Dim strLinks As String = ""
        Dim ta As New dsOrdersTableAdapters.Customer_Document_DownloadableTableAdapter
        Dim strLinkText As String = ""

        For Each dr As dsOrders.Customer_Document_DownloadableRow In ta.GetDataByDocIDAccessCode(iDocumentID, iAccessCode).Rows
            If strLinkText = "" Then
                strLinkText = dr.ReferenceNo
            End If
            strLinks += "<tr><td><a href=""" & _
                ConfigurationSettings.AppSettings("HomeAddress") & "/" & ConfigurationSettings.AppSettings("SiteLocation") & "/ordReviewLanding.aspx?RNUMB=" & iOrderNumber & "&id=" & strGuid & "&fid=" & dr.FormatID & _
                    """>" & dr.FormatName & "</a></td></tr>"
        Next
        Return "<tr><th>" & strLinkText & "</th></tr>" & strLinks

    End Function

End Class
