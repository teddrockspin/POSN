''''''''''''''''''''
'mw - 08-20-2009
'mw - 02-01-2009
'mw - 01-14-2009
'mw - 05-17-2008
'mw - 04-28-2007
'mw - 11-19-2006
''''''''''''''''''''


'<Serializable()> _
Public Class paraKit

    Private pID As Long = 0
    Private pAccessCodeID As Long = 0
    Private pStatus As Integer = 0
    Private pIsTemp As Boolean = False
    Private pCategoryID As Long = 0
    Private pRefNo As String = String.Empty
    Private pRefDescr As String = String.Empty
    Private pType As Integer = 0
    Private pCanBeBase As Boolean = False
    Private pNote As String = String.Empty
    Private pAssemblyInstructions As String = String.Empty
    Private pCurrentItmCart As New System.Data.DataTable


    Friend Property ID() As Integer
        Get
            Return pID
        End Get
        Set(ByVal Value As Integer)
            pID = Value
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

    Friend Property Status() As Integer
        Get
            Return pStatus
        End Get
        Set(ByVal Value As Integer)
            pStatus = Value
        End Set
    End Property

    Friend Property IsTemp() As Boolean
        Get
            Return pIsTemp
        End Get
        Set(ByVal Value As Boolean)
            pIsTemp = Value
        End Set
    End Property

    Friend Property CategoryID() As Integer
        Get
            Return pCategoryID
        End Get
        Set(ByVal Value As Integer)
            pCategoryID = Value
        End Set
    End Property

    Friend Property ReferenceNumber() As String
        Get
            Return pRefNo
        End Get
        Set(ByVal Value As String)
            pRefNo = Value
        End Set
    End Property

    Friend Property ReferenceDescription() As String
        Get
            Return pRefDescr
        End Get
        Set(ByVal Value As String)
            pRefDescr = Value
        End Set
    End Property

    Friend Property KitType() As Integer
        Get
            Return pType
        End Get
        Set(ByVal Value As Integer)
            pType = Value
        End Set
    End Property

    Friend Property MyUseAsBase() As Boolean
        Get
            Return pCanBeBase
        End Get
        Set(ByVal Value As Boolean)
            pCanBeBase = Value
        End Set
    End Property

    Friend Property Note() As String
        Get
            Return pNote
        End Get
        Set(ByVal Value As String)
            pNote = Value
        End Set
    End Property

    Friend Property AssemblyInstructions() As String
        Get
            Return pAssemblyInstructions
        End Get
        Set(value As String)
            pAssemblyInstructions = value
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

    Friend ReadOnly Property ItmIDs() As String
        Get
            Const COL_ITMID = 2

            Dim sItmID As String = String.Empty
            Dim sVals = String.Empty
            Dim objDT As System.Data.DataTable
            Dim indx As Integer : indx = 0
            Dim iMaxIndx As Integer : iMaxIndx = 0

            objDT = pCurrentItmCart

            iMaxIndx = objDT.Rows.Count - 1
            For indx = 0 To iMaxIndx
                sItmID = objDT.Rows(indx).Item(COL_ITMID)
                'sVals = sVals & "|" & sItmID
                'If (Len(sVals) = 0) Then
                '  sVals = "|"
                'End If
                sVals = sVals & sItmID & "|"
            Next indx
            sVals = "|" & sVals

            objDT = Nothing
            Return sVals
        End Get
    End Property

    Friend ReadOnly Property ItmQtys() As String
        Get
            Const COL_QTY = 1

            Dim objDT As System.Data.DataTable
            Dim iItmQty As Integer = 0
            Dim sVals = String.Empty
            Dim indx As Integer : indx = 0
            Dim iMaxIndx As Integer : iMaxIndx = 0

            objDT = pCurrentItmCart

            iMaxIndx = objDT.Rows.Count - 1
            For indx = 0 To iMaxIndx
                iItmQty = objDT.Rows(indx).Item(COL_QTY)
                'sVals = sVals & "|" & iItmQty
                'If (Len(sVals) = 0) Then
                '  sVals = "|"
                'End If
                sVals = sVals & iItmQty & "|"
            Next indx
            sVals = "|" & sVals

            objDT = Nothing
            Return sVals
        End Get
    End Property

    Friend Sub Init()
        pID = 0
        pAccessCodeID = 0
        pStatus = 0
        pIsTemp = False
        pCategoryID = 0
        pRefNo = String.Empty
        pRefDescr = String.Empty
        pType = 0
        pCanBeBase = 0
        pNote = String.Empty
        pAssemblyInstructions = String.Empty

        ItmCartPrep()
    End Sub

    Private Sub ItmCartPrep()
        Dim objDT As System.Data.DataTable
        Dim objDR As System.Data.DataRow

        objDT = New System.Data.DataTable("ItmCart")
        objDT.Columns.Add("ID", GetType(Integer))
        objDT.Columns("ID").AutoIncrement = True
        objDT.Columns("ID").AutoIncrementSeed = 1

        objDT.Columns.Add("ItmQty", GetType(Integer))
        objDT.Columns.Add("ItmID", GetType(String))
        objDT.Columns.Add("ItmName", GetType(String))
        objDT.Columns.Add("ItmDesc", GetType(String))
        objDT.Columns.Add("ItmSeq", GetType(String))
        objDT.Columns.Add("ItmColor", GetType(String))

        pCurrentItmCart = objDT
    End Sub

    Friend Sub ItmSave(ByVal iQty As Integer, ByVal lID As Long, ByVal sName As String, ByVal sDesc As String, ByVal sSeq As String, Optional ByVal sColor As String = "NORMAL")
        Const COL_ID = 0
        Const COL_QTY = 1
        Const COL_ITMID = 2
        Const COL_NAME = 3
        Const COL_DESC = 4
        Const COL_SEQ = 5
        Const COL_COLOR = 6

        Dim objDT As System.Data.DataTable
        Dim objDR As System.Data.DataRow
        Dim indx As Integer : indx = 0
        Dim iMaxIndx As Integer : iMaxIndx = 0
        Dim iRow As Integer : iRow = -1

        objDT = pCurrentItmCart

        iMaxIndx = objDT.Rows.Count - 1
        For indx = 0 To iMaxIndx
            If (lID = objDT.Rows(indx).Item(COL_ITMID)) Then
                iRow = indx
            End If
        Next indx

        If (iQty <= 0) And (iRow >= 0) Then
            'Delete
            objDT.Rows(iRow).Delete()

        ElseIf (lID > 0) And (iQty > 0) And (iRow < 0) Then
            'Add
            objDR = objDT.NewRow()
            objDR.Item(COL_QTY) = iQty
            objDR.Item(COL_ITMID) = lID
            objDR.Item(COL_NAME) = sName
            objDR.Item(COL_DESC) = sDesc
            objDR.Item(COL_SEQ) = sSeq
            objDR.Item(COL_COLOR) = sColor
            objDT.Rows.Add(objDR)
            objDR = Nothing

        Else
            'Update
            objDT.Rows(iRow).Item(COL_QTY) = iQty
            objDT.Rows(iRow).Item(COL_SEQ) = sSeq
        End If

        pCurrentItmCart = objDT

        objDT = Nothing
    End Sub

    Friend Sub Clear()
        Dim objDT As System.Data.DataTable

        pID = 0
        pAccessCodeID = 0
        pStatus = 0
        pIsTemp = False
        pCategoryID = 0
        pRefNo = String.Empty
        pRefDescr = String.Empty
        pType = 0
        pCanBeBase = 0
        pNote = String.Empty
        pAssemblyInstructions = String.Empty

        objDT = pCurrentItmCart

        Do While (objDT.Rows.Count > 0)
            'Delete
            objDT.Rows(0).Delete()
        Loop
    End Sub

    'Friend Function Store() As Boolean
    Friend Function Store() As Long
        Dim myData As paraData
        Dim lKitID As Long = 0
        Dim bSuccess As Boolean = False
        Dim sMsg As String = String.Empty

        Try
            myData = New paraData
            lKitID = CreateKit(myData, sMsg)
            'mw - 08-20-2009
            If (lKitID > 0) Then
                bSuccess = SaveKitKeyword(myData, lKitID, pCategoryID)
                '
                If (lKitID > 0) Then
                    bSuccess = SaveKitItems(myData, lKitID)
                End If
            End If

        Catch ex As Exception
            'Page Message Here for Status to user...
            'moLog.Entry(msLogFile, "  ERROR:  " & moLog.NewLine & ex.Message.ToString & moLog.NewLine)
        End Try
        myData = Nothing

        'Return bSuccess
        Return lKitID
    End Function

    Private Function CreateKit(ByRef myData As paraData, ByRef sMsg As String) As Long
        Dim cnnE As OleDb.OleDbConnection
        Dim cmdE As OleDb.OleDbCommand
        Dim sSQLE As String
        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim lKitID As Long = 0
        Dim lExistID As Long = 0
        Dim lExistAnotherID As Long = 0
        Dim sOrgSpec As String = String.Empty
        Dim sNewSpec As String = String.Empty
        Dim sOrgRefNo As String = String.Empty
        Dim sNewRefNo As String = String.Empty
        Dim iRevNo As Integer = 1
        Dim sItmIDs As String = String.Empty
        Dim sItmQtys As String = String.Empty
        Dim bNeedToRemove As Boolean = False
        Dim sCurrentTime As String = String.Empty
        Dim sText As String = String.Empty
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String



        Try
            If (pID = 0) Then
                'Check to see if an Active Kit exists with same RefNo
                lExistID = LkpKit(myData, pAccessCodeID, pRefNo)
             
            Else
                lExistID = pID
                'Check for another with the newer refno
                lExistAnotherID = LkpKit(myData, pAccessCodeID, pRefNo)
            End If

            If (lExistID > 0) Then
                If (KitOrderPresent(myData, lExistID)) Then
                    'Check what has changed
                    GetKitSpec(myData, lExistID, sItmIDs, sItmQtys)
                    sOrgSpec = sItmIDs & "!" & sItmQtys
                    sNewSpec = ItmIDs & "!" & ItmQtys
                    If (LCase(sOrgSpec) = LCase(sNewSpec)) Then
                        'smsg="ExistID =" & lExistID & " Kit Order Present - Spec NOT Changed"
                        'Items Same Check Name Change
                        sOrgRefNo = GetKitRefNo(myData, lExistID)
                        sNewRefNo = pRefNo
                        If (LCase(sOrgRefNo) = LCase(sNewRefNo)) Then
                            'smsg="ExistID =" & lExistID & " Kit Order Present - Spec Changed - Name NOT Changed"
                            'Only Description has changed - use same ID
                            lKitID = lExistID
                        Else
                            'smsg="ExistID =" & lExistID & " Kit Order Present - Spec Changed - Name Changed"
                            bNeedToRemove = True
                        End If
                        '
                    Else
                        'smsg="ExistID =" & lExistID & " Kit Order Present - Spec Changed   " & LCASE(sOrgSpec) & " ,,, " & LCASE(sNewSpec)
                        bNeedToRemove = True
                    End If

                    If bNeedToRemove Then
                        RemoveKit(myData, lExistID)
                        iRevNo = GetKitRevNo(myData, lExistID) + 1
                    End If
                    '
                Else
                    'smsg="ExistID =" & lExistID & " Kit Order NOT Present"
                    'Update Current
                    lKitID = lExistID
                End If
            Else
                iRevNo = 1
            End If

            sCurrentTime = Now()
            If (lKitID = 0) Then
                'Create New
                'mw - 08-20-2009
                ''mw - 02-01-2009
                '''sSQLE = "INSERT INTO Customer_Kit (Status, AccessCodeID, CategoryID, ReferenceNo, RevNo, Description, ModifiedDate) " & _
                '''        "VALUES (" & pStatus & ", " & pAccessCodeID & ", " & IIf(pCategoryID > 0, pCategoryID, "NULL") & ", " & myData.SQLString(pRefNo) & ", " & iRevNo & ", " & myData.SQLString(pRefDescr) & ", #" & sCurrentTime & "#) "
                ''sSQLE = "INSERT INTO Customer_Kit (Status, AccessCodeID, CategoryID, ReferenceNo, RevNo, Description, [Note], ModifiedDate) " & _
                ''        "VALUES (" & pStatus & ", " & pAccessCodeID & ", " & IIf(pCategoryID > 0, pCategoryID, "NULL") & ", " & myData.SQLString(pRefNo) & ", " & iRevNo & ", " & myData.SQLString(pRefDescr) & ", " & myData.SQLString(pNote) & ", #" & sCurrentTime & "#) "
                'sSQLE = "INSERT INTO Customer_Kit (Status, AccessCodeID, CategoryID, ReferenceNo, RevNo, Description, BaseKit, [Note], ModifiedDate) " & _
                '        "VALUES (" & pStatus & ", " & pAccessCodeID & ", " & IIf(pCategoryID > 0, pCategoryID, "NULL") & ", " & myData.SQLString(pRefNo) & ", " & iRevNo & ", " & myData.SQLString(pRefDescr) & ", " & IIf(pCanBeBase = True, 1, 0) & ", " & myData.SQLString(pNote) & ", #" & sCurrentTime & "#) "
                sSQLE = "INSERT INTO Customer_Kit (Status, AccessCodeID, CategoryID, ReferenceNo, RevNo, Description, BaseKit, [Note], ModifiedDate, AssemblyInstructions) " & _
                "VALUES (" & pStatus & ", " & pAccessCodeID & ", NULL, " & myData.SQLString(pRefNo) & ", " & iRevNo & ", " & myData.SQLString(pRefDescr) & ", " & IIf(pCanBeBase = True, 1, 0) & ", " & myData.SQLString(pNote) & ", #" & sCurrentTime & "#" & "," & myData.SQLString(pAssemblyInstructions) & ") "
                '
                myData.GetConnection(cnnE)
                If Not (cnnE Is Nothing) Then
                    myData.SQLExecute(cnnE, sSQLE)
                End If
                myData.ReleaseConnection(cnnE)

                sSQLR = "SELECT ID AS KitID FROM Customer_Kit WHERE (" & _
                        "(AccessCodeID = " & pAccessCodeID & ") AND " & _
                        "(ReferenceNo = " & myData.SQLString(pRefNo) & ") AND (RevNo = " & iRevNo & ") AND " & _
                        "(ModifiedDate = #" & sCurrentTime & "#))"
                myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
                If rcdR.Read() Then
                    lKitID = rcdR("KitID")
                End If
                myData.ReleaseReader2(cnnR, cmdR, rcdR)

            Else
                'Update Existing
                'If Unique Reference Number Then can change - otherwise cannot change that part
                If (lExistAnotherID = lExistID) Then lExistAnotherID = 0
                If (lExistAnotherID = 0) Then
                    sText = "ReferenceNo = " & myData.SQLString(pRefNo) & ","
                Else
                    sMsg = "Unable to change reference number.  There is another active kit already using this reference number."
                End If
                'mw - 08-20-2009
                ''mw - 02-01-2009
                '''sSQLE = "UPDATE Customer_Kit SET Status = " & pStatus & ", " & sText & " CategoryID = " & IIf(pCategoryID > 0, pCategoryID, "NULL") & ", " & " Description = " & myData.SQLString(pRefDescr) & _
                '''        ", ModifiedDate = #" & Now() & "# " & _
                '''        " WHERE (ID = " & lKitID & ")"
                ''sSQLE = "UPDATE Customer_Kit SET Status = " & pStatus & ", " & sText & " CategoryID = " & IIf(pCategoryID > 0, pCategoryID, "NULL") & ", " & " Description = " & myData.SQLString(pRefDescr) & ", " & " [Note] = " & myData.SQLString(pNote) & _
                ''        ", ModifiedDate = #" & Now() & "# " & _
                ''        " WHERE (ID = " & lKitID & ")"
                'sSQLE = "UPDATE Customer_Kit SET Status = " & pStatus & ", " & sText & " CategoryID = " & IIf(pCategoryID > 0, pCategoryID, "NULL") & ", " & " Description = " & myData.SQLString(pRefDescr) & ", " & "BaseKit = " & IIf(pCanBeBase = True, 1, 0) & ", " & " [Note] = " & myData.SQLString(pNote) & _
                '        ", ModifiedDate = #" & Now() & "# " & _
                '        " WHERE (ID = " & lKitID & ")"
                sSQLE = "UPDATE Customer_Kit SET Status = " & pStatus & ", " & sText & " CategoryID = NULL, " & " Description = " & myData.SQLString(pRefDescr) & ", " & "BaseKit = " & IIf(pCanBeBase = True, 1, 0) & ", " & " [Note] = " & myData.SQLString(pNote) & _
                        ", ModifiedDate = #" & Now() & "# " & ", " & " AssemblyInstructions = " & myData.SQLString(pAssemblyInstructions) & _
                        " WHERE (ID = " & lKitID & ")"
                '
                myData.GetConnection(cnnE)
                If Not (cnnE Is Nothing) Then
                    myData.SQLExecute(cnnE, sSQLE)
                End If
                myData.ReleaseConnection(cnnE)
            End If

        Catch ex As Exception
            myData.ReleaseConnection(cnnE)
            myData.ReleaseReader2(cnnR, cmdR, rcdR)
        End Try

        Return lKitID
    End Function

    Private Function LkpKit(ByRef myData As paraData, ByVal lAccessCodeID As Long, ByVal sRefNo As String) As Long
        'NOTE:  looks up Active Kits
        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String
        Dim lID As Long = 0

        Try
            sSQLR = "SELECT ID AS KitID FROM Customer_Kit WHERE (Status > " & myData.STATUS_INAC & ") " & _
                    "AND (AccessCodeID = " & lAccessCodeID & ") AND (ReferenceNo = " & myData.SQLString(sRefNo) & ")"
            myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
            If rcdR.Read() Then
                lID = rcdR("KitID")
            End If
            myData.ReleaseReader2(cnnR, cmdR, rcdR)

        Catch ex As Exception
            myData.ReleaseReader2(cnnR, cmdR, rcdR)
        End Try

        Return lID
    End Function

    Private Function KitOrderPresent(ByRef myData As paraData, ByVal lKitID As Long) As Boolean
        'NOTE:  looks up Active Kits
        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String
        Dim lID As Long = 0
        Dim bPresent As Boolean = False

        Try
            If (lKitID > 0) Then
                sSQLR = "SELECT KitID FROM Order_Document WHERE (KitID = " & lKitID & ")"
                myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
                If rcdR.Read() Then
                    bPresent = True
                End If
                myData.ReleaseReader2(cnnR, cmdR, rcdR)
            End If

        Catch ex As Exception
            myData.ReleaseReader2(cnnR, cmdR, rcdR)
        End Try

        Return bPresent
    End Function

    Private Function GetKitSpec(ByRef myData As paraData, ByVal lKitID As Long, ByRef sItmID As String, ByRef sItmQty As String) As Boolean
        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String
        Dim indx As Integer = 0
        Dim bSuccess As Boolean = False

        Try
            sItmID = ""
            sItmQty = ""

            sSQLR = "SELECT Customer_Document.ID, iif(Customer_Document.Status >" & myData.STATUS_INAC & ",True,False) AS Active, Customer_Kit_Document.Qty, Customer_Kit_Document.Seq " & _
                    "FROM Customer_Kit_Document INNER JOIN Customer_Document ON Customer_Kit_Document.DocumentID = Customer_Document.ID " & _
                    "WHERE (Customer_Kit_Document.KitID = " & lKitID & ") " & _
                    "ORDER BY Customer_Document.ReferenceNo "
            myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
            Do While rcdR.Read
                indx = indx + 1
                If (Len(sItmID) = 0) Then
                    sItmID = "|"
                    sItmQty = "|"
                End If
                sItmID = sItmID & rcdR("ID") & "|"
                If (rcdR("Active") = True) Then
                    sItmQty = sItmQty & rcdR("Qty") & "|"
                Else
                    sItmQty = sItmQty & -(rcdR("Qty")) & "|"
                End If
            Loop
            myData.ReleaseReader2(cnnR, cmdR, rcdR)
            bSuccess = True

        Catch ex As Exception
            myData.ReleaseReader2(cnnR, cmdR, rcdR)
        End Try

        Return bSuccess
    End Function

    Private Function GetKitRefNo(ByVal myData As paraData, ByVal lKitID As Long) As String
        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String
        Dim lID As Long = 0
        Dim sRefNo As String = String.Empty

        Try
            sSQLR = "SELECT ReferenceNo FROM Customer_Kit WHERE (ID = " & lKitID & ")"
            myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
            If rcdR.Read() Then
                sRefNo = rcdR("ReferenceNo")
            End If
            myData.ReleaseReader2(cnnR, cmdR, rcdR)

        Catch ex As Exception
            myData.ReleaseReader2(cnnR, cmdR, rcdR)
        End Try

        Return sRefNo
    End Function

    Private Function GetKitRevNo(ByRef myData As paraData, ByVal lKitID As Long) As Integer
        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String
        Dim iRevNo As Integer = iRevNo = 0

        Try
            sSQLR = "SELECT RevNo FROM Customer_Kit WHERE (ID = " & lKitID & ")"
            myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
            If rcdR.Read() Then
                iRevNo = rcdR("RevNo")
            End If
            myData.ReleaseReader2(cnnR, cmdR, rcdR)

        Catch ex As Exception
            myData.ReleaseReader2(cnnR, cmdR, rcdR)
        End Try

        Return iRevNo
    End Function

    Friend Function RemoveKit(ByRef myData As paraData, ByVal lKitID As Long) As Boolean
        'Customer_Kit Lookup Existing - if Orders already processed
        Dim cnnE As OleDb.OleDbConnection
        Dim cmdE As OleDb.OleDbCommand
        Dim sSQLE As String
        Dim iRevNo As Integer = iRevNo = 0
        Dim bSuccess As Boolean = False

        Try
            If (lKitID > 0) Then
                myData.GetConnection(cnnE)
                If Not (cnnE Is Nothing) Then
                    If (KitOrderPresent(myData, lKitID)) Then
                        sSQLE = "UPDATE Customer_Kit SET Status=" & myData.STATUS_INAC & " , ModifiedDate=now() " & _
                               "WHERE (ID = " & lKitID & ")"
                    Else
                        sSQLE = "DELETE * FROM Customer_Kit WHERE (ID = " & lKitID & ")"
                    End If
                    myData.SQLExecute(cnnE, sSQLE)
                End If
                myData.ReleaseConnection(cnnE)
            End If
            bSuccess = True

        Catch ex As Exception
            myData.ReleaseConnection(cnnE)
        End Try

        Return bSuccess
    End Function

    Private Function SaveKitKeyword(ByRef myData As paraData, ByVal lKitID As Long, ByVal lKeywordID As Long) As Boolean
        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String
        Dim cnnE As OleDb.OleDbConnection
        Dim sSQLE As String
        Dim bAdd As Boolean = True
        Dim bUpdate As Boolean = False
        Dim bSuccess As Boolean = False

        Try
            If (lKeywordID > 0) Then
                sSQLR = "SELECT ID AS KitID, KeywordID FROM Customer_Kit_Keyword WHERE (KitID = " & lKitID & ") "
                myData.GetReader2(cnnR, cmdR, rcdR, sSQLR)
                If rcdR.Read() Then
                    bUpdate = (lKeywordID <> Val(rcdR("KeywordID")))
                    bAdd = False
                End If
                myData.ReleaseReader2(cnnR, cmdR, rcdR)

                If bAdd Then
                    sSQLE = "INSERT INTO Customer_Kit_Keyword (KitID, KeywordID) VALUES (" & lKitID & ", " & lKeywordID & ")"
                ElseIf bUpdate Then
                    sSQLE = "UPDATE Customer_Kit_Keyword SET KeywordID = " & lKeywordID & " WHERE KitID = " & lKitID
                End If

                If bAdd Or bUpdate Then
                    myData.GetConnection(cnnE)
                    myData.SQLExecute(cnnE, sSQLE)
                    myData.ReleaseConnection(cnnE)
                    bSuccess = True
                Else
                    bSuccess = True
                End If
            End If

        Catch ex As Exception
            myData.ReleaseConnection(cnnE)
            myData.ReleaseReader2(cnnR, cmdR, rcdR)
        End Try

        Return bSuccess
    End Function

    Private Function SaveKitItems(ByRef myData As paraData, ByVal lKitID As Long) As Boolean
        Const COL_ID = 0
        Const COL_QTY = 1
        Const COL_ITMID = 2
        Const COL_NAME = 3
        Const COL_DESC = 4
        Const COL_SEQ = 5
        Const COL_COLOR = 6

        Dim cnnR As OleDb.OleDbConnection
        Dim cmdR As OleDb.OleDbCommand
        Dim rcdR As OleDb.OleDbDataReader
        Dim sSQLR As String
        Dim cnnE As OleDb.OleDbConnection
        Dim cmdE As OleDb.OleDbCommand
        Dim sSQLE As String
        Dim objDT As System.Data.DataTable
        Dim indx As Integer : indx = 0
        Dim iMaxIndx As Integer : iMaxIndx = 0
        Dim sCurrentTime As String = String.Empty
        Dim lItmID As Long = 0
        Dim iItmQty As Integer = 0
        Dim iItmSeq As String = ""
        Dim bFound As Boolean = False
        Dim bSuccess As Boolean = False

        Try
            objDT = pCurrentItmCart
            myData.GetConnection(cnnE)
            sCurrentTime = Now()

            iMaxIndx = objDT.Rows.Count - 1
            For indx = 0 To iMaxIndx
                lItmID = objDT.Rows(indx).Item(COL_ITMID)
                iItmQty = objDT.Rows(indx).Item(COL_QTY)
                'iItmSeq = IIf(objDT.Rows(indx).Item(COL_SEQ) = "C" Or objDT.Rows(indx).Item(COL_SEQ) = "c", -1, objDT.Rows(indx).Item(COL_SEQ))
                iItmSeq = objDT.Rows(indx).Item(COL_SEQ)

                If (iItmQty > 0) Then
                    sSQLR = "SELECT ID FROM Customer_Kit_Document " & _
                            "WHERE ((KitID = " & lKitID & ") AND (DocumentID = " & lItmID & "))"


                    myData.GetReader2(cnnE, cmdR, rcdR, sSQLR)
                    bFound = rcdR.Read()
                    'myData.ReleaseReader2(cnnR, cmdR, rcdR)
                    rcdR.Close()

                    If bFound Then
                        sSQLE = "UPDATE Customer_Kit_Document SET Qty=" & iItmQty & ", Seq='" & iItmSeq & "', ModifiedDate=#" & sCurrentTime & "# " & _
                                  "WHERE ((KitID = " & lKitID & ") AND (DocumentID = " & lItmID & "))"
                        myData.SQLExecute(cnnE, sSQLE)
                    Else
                        sSQLE = "INSERT INTO Customer_Kit_Document (KitID, DocumentID, Qty, Seq, ModifiedDate) VALUES " & _
                              "(" & lKitID & ", " & lItmID & ", " & iItmQty & ", '" & iItmSeq & "', #" & sCurrentTime & "#)"
                        myData.SQLExecute(cnnE, sSQLE)
                    End If
                End If
            Next indx

            'Remove any left over Docs - those still remaining should have had date and qty updated
            sSQLE = "DELETE * FROM Customer_Kit_Document WHERE (KitID = " & lKitID & ") AND (ModifiedDate < #" & sCurrentTime & "#)"
            myData.SQLExecute(cnnE, sSQLE)

            myData.ReleaseConnection(cnnE)
            bSuccess = True

        Catch ex As Exception
            myData.ReleaseReader2(cnnR, cmdR, rcdR)
            myData.ReleaseConnection(cnnE)
        End Try
        objDT = Nothing

        Return bSuccess
    End Function

    Friend Function Delete() As Boolean
        Dim myData As paraData
        Dim bSuccess As Boolean = False

        myData = New paraData

        If (pID > 0) Then bSuccess = RemoveKit(myData, pID)

        myData = Nothing

        Return bSuccess
    End Function

End Class
