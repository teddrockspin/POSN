Partial Class Exclusions
    Inherits paraPageBase

    Private Const COL_KEY As Integer = 2
    Private Const ColorHighlight As Integer = &HFFFF80

    Private Enum ScreenType
        items
        kits
    End Enum

    Private Sub Exclusions_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If (Not IsPostBack) Then
            LoadMemoryTables()
        End If
        MyBase.Page_Init(sender, e)
        GridHeaderLabels()
    End Sub

    Private Sub GridHeaderLabels()
        Dim taKey As New dsKeyTableAdapters.Customer_KeyTableAdapter
        Dim dtKey As New dsKey.Customer_KeyDataTable

        dtKey = taKey.GetData(CurrentUser.CustomerID)

        Try
            Me.grdK1Rad.Columns(1).HeaderText = dtKey(0).Name
        Catch ex As Exception
            Me.grdK1Rad.Visible = False
        End Try
        Try
            Me.grdK2Rad.Columns(1).HeaderText = dtKey(1).Name
        Catch ex As Exception
            Me.grdK2Rad.Visible = False
        End Try
        Try
            Me.grdK3Rad.Columns(1).HeaderText = dtKey(2).Name
        Catch ex As Exception
            Me.grdK3Rad.Visible = False
        End Try
    End Sub

    Private Overloads Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not IsPostBack) Then
            FillDropDownLists()
            ScreenState(ScreenType.items)
        End If
    End Sub

    Private Sub LoadMemoryTables()
        Dim dt1 As New dsCategories.CatagoryIdsDataTable
        Dim dt2 As New dsCategories.CatagoryIdsDataTable
        Dim dt3 As New dsCategories.CatagoryIdsDataTable
        Dim dt5 As New dsCategories.CatagoryIdsDataTable
        Dim dt6 As New dsCategories.CatagoryIdsDataTable

        Session("SelectedGrid1") = dt1
        Session("SelectedGrid2") = dt2
        Session("SelectedGrid3") = dt3
        Session("SelectedGrid5") = dt5
        Session("SelectedGrid6") = dt6
    End Sub

    Private Sub FillDropDownLists()
        Dim taAccessCode As New dsAccessCodesTableAdapters.Customer_AccessCodeTableAdapter
        Dim dtAccessCode As New dsAccessCodes.Customer_AccessCodeDataTable

        taAccessCode.ClearBeforeFill = False

        If (CurrentUser.CurrentUserExclusionsAccess = 2) Then
            dtAccessCode = taAccessCode.GetData(CurrentUser.CustomerID)
            ddlAccessCode.DataTextField = "Code"
            ddlAccessCode.DataValueField = "ID"

            ddlAccessCode.DataSource = dtAccessCode
            ddlAccessCode.DataBind()
        Else
            ddlAccessCode.Items.Add(New ListItem(CurrentUser.AccessCode, CurrentUser.AccessCodeID))
        End If


        Me.ddlAccessCode.Items(0).Selected = True
    End Sub

    Protected Sub ddlAccessCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAccessCode.SelectedIndexChanged
        LoadMemoryTables()
        LoadGrids()
    End Sub

    Private Sub LoadScreen()
        Select Case Me.lstItemTypes.SelectedIndex
            Case ScreenType.items
                LoadGrids()
            Case ScreenType.kits
                LoadGridsKits()
        End Select
    End Sub

    Private Sub LoadGridsKits()
        LoadGridKits(1, Me.grdKitCategories)
        LoadItemKits(SelectedID(Session("SelectedGrid6")))
        LoadExclusionsGrid(ScreenType.kits)
    End Sub

    Private Sub LoadGrids()
        LoadGrid(1, Me.grdK1Rad)
        LoadGrid(2, Me.grdK2Rad)
        LoadGrid(3, Me.grdK3Rad)
        LoadItem(SelectedID(Session("SelectedGrid1")), SelectedID(Session("SelectedGrid2")), SelectedID(Session("SelectedGrid3")), "", "")
        LoadExclusionsGrid(ScreenType.items)
    End Sub

    Private Sub LoadExclusionsGrid(ByVal sState As ScreenType)
        Dim taCatagories As New dsExclusionsTableAdapters.CatagoriesByAccessCodeTableAdapter
        Dim dt As New dsExclusions.CatagoriesByAccessCodeDataTable

        Dim taCategoriesKits As New dsExclusionsTableAdapters.CategoriesByAccessCodeKitsTableAdapter
        Dim dtCategoriesKits As New dsExclusions.CategoriesByAccessCodeKitsDataTable

        If (sState = ScreenType.items) Then
            taCatagories.Fill(dt, Me.ddlAccessCode.SelectedValue, CurrentUser.CustomerID, CurrentUser.CustomerID, Me.ddlAccessCode.SelectedValue, CurrentUser.CustomerID)
            Me.grdExclusionsRad.DataSource = dt
        Else
            taCategoriesKits.Fill(dtCategoriesKits, Me.ddlAccessCode.SelectedValue, CurrentUser.CustomerID, Me.ddlAccessCode.SelectedValue, CurrentUser.CustomerID)
            Me.grdExclusionsRad.DataSource = dtCategoriesKits
        End If

        Me.grdExclusionsRad.DataBind()
    End Sub

    Private Function LoadGrid(ByVal iSeq As Integer, ByRef grd As Telerik.Web.UI.RadGrid) As Boolean
        Dim taExclusionsAccessID As New dsExclusionsTableAdapters.GetExclusionByDescriptionTableAdapter
        Dim dtExclusionsAccessID As New dsExclusions.GetExclusionByDescriptionDataTable

        Dim taCatagories As New dsCategoriesTableAdapters.CatagoriesTableAdapter
        Dim dtCatagories As New dsCategories.CatagoriesDataTable
        Dim rCatagories As dsCategories.CatagoriesRow

        taCatagories.Fill(dtCatagories, iSeq, CurrentUser.CustomerID)
        taExclusionsAccessID.ClearBeforeFill = True
        For Each rCatagories In dtCatagories.Rows
            taExclusionsAccessID.Fill(dtExclusionsAccessID, rCatagories.ID, Me.ddlAccessCode.SelectedValue, rCatagories.ID, CurrentUser.CustomerID)
            If (dtExclusionsAccessID.Rows.Count > 0) Then
                rCatagories.Delete()
            End If
        Next

        grd.DataSource = dtCatagories
        grd.DataBind()

        Return True
    End Function

    Private Function LoadGridKits(ByVal iSeq As Integer, ByRef grd As Telerik.Web.UI.RadGrid) As Boolean
        Dim taExclusionsAccessID As New dsExclusionsTableAdapters.GetExclusionsForKitsTableAdapter
        Dim dtExclusionsAccessID As New dsExclusions.GetExclusionsForKitsDataTable

        Dim taCatagories As New dsCategoriesTableAdapters.CatagoriesTableAdapter
        Dim dtCatagories As New dsCategories.CatagoriesDataTable
        Dim rCatagories As dsCategories.CatagoriesRow

        taCatagories.Fill(dtCatagories, iSeq, CurrentUser.CustomerID)
        taExclusionsAccessID.ClearBeforeFill = True
        For Each rCatagories In dtCatagories.Rows
            taExclusionsAccessID.Fill(dtExclusionsAccessID, rCatagories.ID, Me.ddlAccessCode.SelectedValue, rCatagories.ID, CurrentUser.CustomerID)
            If (dtExclusionsAccessID.Rows.Count > 0) Then
                rCatagories.Delete()
            End If
        Next

        grd.DataSource = dtCatagories
        grd.DataBind()

        Return True
    End Function

    Private Sub CatagorySelected(ByVal iSeq As Integer, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Dim sTable As String = String.Format("SelectedGrid{0}", iSeq)
        Dim dt As dsCategories.CatagoryIdsDataTable = Session(sTable)
        Dim dRow As dsCategories.CatagoryIdsRow
        Dim lblColor As Label

        dRow = dt.Rows.Find(e.CommandArgument)
        If (dRow Is Nothing) Then
            Try
                dRow = dt.Rows.Add
                dRow.ID = e.CommandArgument
                e.Item.BackColor = Color.FromArgb(ColorHighlight)
            Catch ex As Exception
            End Try
        Else
            dRow.Delete()
            lblColor = e.Item.FindControl("lblColor")
            If (e.Item.ItemType = Telerik.Web.UI.GridItemType.AlternatingItem) Then
                e.Item.BackColor = Me.grdK1Rad.AlternatingItemStyle.BackColor
            Else
                e.Item.BackColor = Me.grdK1Rad.ItemStyle.BackColor
            End If
        End If

        Session(sTable) = dt
        LoadItem(SelectedID(Session("SelectedGrid1")), SelectedID(Session("SelectedGrid2")), SelectedID(Session("SelectedGrid3")), "", "")
    End Sub

    Private Sub CatagorySelectedRad(ByVal iSeq As Integer, ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim sTable As String = String.Format("SelectedGrid{0}", iSeq)
        Dim dt As dsCategories.CatagoryIdsDataTable = Session(sTable)
        Dim dRow As dsCategories.CatagoryIdsRow
        Dim lblColor As Label

        dRow = dt.Rows.Find(e.CommandArgument)
        If (dRow Is Nothing) Then
            Try
                dRow = dt.Rows.Add
                dRow.ID = e.CommandArgument
                e.Item.BackColor = Color.FromArgb(ColorHighlight)
            Catch ex As Exception
            End Try
        Else
            dRow.Delete()
            lblColor = e.Item.FindControl("lblColor")
            If (e.Item.ItemType = Telerik.Web.UI.GridItemType.AlternatingItem) Then
                e.Item.BackColor = Me.grdK1Rad.AlternatingItemStyle.BackColor
            Else
                e.Item.BackColor = Me.grdK1Rad.ItemStyle.BackColor
            End If
        End If

        Session(sTable) = dt
        If (lstItemTypes.SelectedIndex = ScreenType.items) Then
            LoadItem(SelectedID(Session("SelectedGrid1")), SelectedID(Session("SelectedGrid2")), SelectedID(Session("SelectedGrid3")), "", "")
        Else
            LoadItemKits(SelectedID(Session("SelectedGrid6")))
        End If

    End Sub

    Private Function LoadGridItm(ByVal sSQL As String, ByRef grd As telerik.Web.UI.RadGrid) As Boolean
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim ds As DataSet
        Dim dRow As DataRow
        Dim bSuccess As Boolean = False

        Dim taExclusions As New dsExclusionsTableAdapters.AccessCodeExclusionsByDescriptionTableAdapter
        Dim dtExclusions As New dsExclusions.AccessCodeExclusionsByDescriptionDataTable

        Dim iType As Integer

        If lstItemTypes.SelectedIndex = ScreenType.items Then
            iType = 5
        Else
            iType = 7
        End If

        If (sSQL.Length > 0) Then
            'grd.SelectedIndex = -1
            MyBase.MyData.GetDataSet(cnn, cmd, ds, sSQL)
            For Each dRow In ds.Tables(0).Rows
                dtExclusions = taExclusions.GetData(iType, Me.ddlAccessCode.SelectedValue, dRow("Id"), iType, dRow("Id"), CurrentUser.CustomerID)
                If (dtExclusions.Rows.Count > 0) Then
                    dRow.Delete()
                End If
            Next
            grd.DataSource = ds
            If (grd.PageCount < grd.CurrentPageIndex) Then grd.CurrentPageIndex = 0
            grd.DataBind()
            bSuccess = True
        End If

        Return (bSuccess)
    End Function

    Private Function SelectedID(ByVal dt As dsCategories.CatagoryIdsDataTable) As String
        Dim sId() As String
        Dim dRow As dsCategories.CatagoryIdsRow
        Dim iCounter As Integer

        iCounter = 0
        For Each dRow In dt.Rows
            ReDim Preserve sId(iCounter)
            sId(iCounter) = dRow.ID
            iCounter += 1
        Next

        If (iCounter > 0) Then
            Return Join(sId, ",")
        Else
            If (lstItemTypes.SelectedIndex = ScreenType.items) Then
                Return ""
            Else
                Return "0"
            End If
        End If
    End Function

    Private Function LoadItem(Optional ByVal sK1IDs As String = "", Optional ByVal sK2IDs As String = "", Optional ByVal sK3IDs As String = "", Optional ByVal sK4IDs As String = "", _
                            Optional ByVal sText As String = "") As Boolean
        Dim bSuccess As Boolean = False
        Dim sSQL As String
        Dim lRcdCnt As Long = 0

        If (sK1IDs = "") And (sK2IDs = "") And (sK3IDs = "") And (sK4IDs = "") And (sText = "") Then
            sK1IDs = "0"
        End If

        sSQL = BuildSQL_Itm(sK1IDs, sK2IDs, sK3IDs, sK4IDs, sText)
        bSuccess = LoadGridItm(sSQL, grdItmRad)
    End Function

    Private Function LoadItemKits(ByVal sK1IDs As String) As Boolean
        Dim bSuccess As Boolean = False
        Dim sSQL As String
        Dim lRcdCnt As Long = 0

        sSQL = BuildSQL_Kit(sK1IDs)
        bSuccess = LoadGridItm(sSQL, grdItmRad)

        Return bSuccess
    End Function

    Private Function BuildSQL_Itm(Optional ByVal sKeyword1IDs As String = "", Optional ByVal sKeyword2IDs As String = "", Optional ByVal sKeyword3IDs As String = "", Optional ByVal sKeyword4IDs As String = "", _
                            Optional ByVal sSearch As String = "") As String
        Dim bShowActiveOnly As Boolean : bShowActiveOnly = True
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sFrom As String = String.Empty
        Dim sKey1From As String = String.Empty
        Dim sKey2From As String = String.Empty
        Dim sKey3From As String = String.Empty
        Dim sKey4From As String = String.Empty
        Dim sKeyFrom As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty

        sSelect = "SELECT DISTINCT d.ID, d.Status, d.ReferenceNo AS RefNo, d.Description AS RefName, " & _
                  "iif(d.Status=" & MyBase.MyData.STATUS_BACK & ",True,False) AS OnBackOrder, " & _
                  "iif(LEN(d.Prefix & '')=0,'ZZZZ',FORMAT(d.Prefix,'0000')), OrderQuantityLimit "
        sFrom = "FROM Customer_Document d INNER JOIN Customer_Document_Keyword dk ON d.ID = dk.DocumentID "
        sWhere = " (d.CustomerID = " & MyBase.CurrentUser.CustomerID & ")"

        'By Keyword Tags
        If (Len(sKeyword1IDs) > 0) Or (Len(sKeyword2IDs) > 0) Or (Len(sKeyword3IDs) > 0) Or (Len(sKeyword4IDs) > 0) Then
            If (Len(sKeyword1IDs) > 0) Then
                sKey1From = "(SELECT DocumentID FROM Customer_Document_Keyword WHERE KeywordID IN (" & sKeyword1IDs & ")) K1 "
            Else
                sKey1From = "(SELECT DocumentID FROM Customer_Document_Keyword) K1 "
            End If
            If (Len(sKeyword2IDs) > 0) Then
                sKey2From = "(SELECT DocumentID FROM Customer_Document_Keyword WHERE KeywordID IN (" & sKeyword2IDs & ")) K2 "
            Else
                sKey2From = "(SELECT DocumentID FROM Customer_Document_Keyword) K2 "
            End If
            If (Len(sKeyword3IDs) > 0) Then
                sKey3From = "(SELECT DocumentID FROM Customer_Document_Keyword WHERE KeywordID IN (" & sKeyword3IDs & ")) K3 "
            Else
                sKey3From = "(SELECT DocumentID FROM Customer_Document_Keyword) K3 "
            End If
            If (Len(sKeyword4IDs) > 0) Then
                sKey4From = "(SELECT DocumentID FROM Customer_Document_Keyword WHERE KeywordID IN (" & sKeyword4IDs & ")) K4 "
            Else
                sKey4From = "(SELECT DocumentID FROM Customer_Document_Keyword) K4 "
            End If
            sKeyFrom = "SELECT DISTINCT K1.DocumentID FROM " & _
                        sKey1From & " INNER JOIN " & _
                        "(" & sKey2From & " INNER JOIN " & _
                        "(" & sKey3From & " INNER JOIN " & _
                        "" & sKey4From & _
                        " ON K3.DocumentID = K4.DocumentID " & _
                        ")ON K2.DocumentID = K3.DocumentID " & _
                        ")ON K1.DocumentID = K2.DocumentID " & _
                        ""
            sFrom = "FROM Customer_Document d INNER JOIN (" & sKeyFrom & ") dk ON d.ID = dk.DocumentID "
        End If

        If (sSearch.Length > 0) Then
            sWhere = sWhere & "AND ( (d.ReferenceNo LIKE " & MyBase.MyData.SQLString("%" & sSearch & "%") & ") OR (d.Description LIKE " & MyBase.MyData.SQLString("%" & sSearch & "%") & ")) "
        End If

        If (bShowActiveOnly) Then
            sWhere = sWhere & " AND (d.Status >" & MyData.STATUS_INAC & ") "
        End If
        If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

        sOrder = sOrder & "ORDER BY iif(LEN(d.Prefix & '')=0,'ZZZZ',FORMAT(d.Prefix,'0000')), d.ReferenceNo "

        sSQL = sSelect + sFrom & sWhere + sOrder

        Return sSQL
    End Function

    Private Sub AddExclusion(ByVal Id As Integer, ByVal Type As Integer)
      
        'Dim del As New dsExclusionsTableAdapters.QueriesTableAdapter
        'del.DeleteIndividualExclusions(CurrentUser.CustomerID, Id, Type)

        Dim ta As New dsExclusionsTableAdapters.Customer_AccessCodeExclusionsTableAdapter
        Dim dt As New dsExclusions.Customer_AccessCodeExclusionsDataTable

        dt.AddCustomer_AccessCodeExclusionsRow(ddlAccessCode.SelectedValue, Id, Type, CurrentUser.CustomerID)
        ta.Update(dt)
    End Sub

    Private Function isCatagorySelected(ByVal iId As Integer, ByVal dt As dsCategories.CatagoryIdsDataTable) As Boolean
        Dim dRow As dsCategories.CatagoryIdsRow

        dRow = dt.FindByID(iId)
        Return Not dRow Is Nothing
    End Function

    Private Sub CheckIfSelected(ByVal iGrid As Integer, ByVal e As Telerik.Web.UI.GridItemEventArgs)
        Dim dtSelected As New dsCategories.CatagoryIdsDataTable
        Dim drSelected As dsCategories.CatagoryIdsRow
        Dim lId As Label

        lId = e.Item.FindControl(String.Format("Labelk{0}", iGrid))

        dtSelected = Session(String.Format("SelectedGrid{0}", iGrid))
        drSelected = dtSelected.FindByID(lId.Text)
        If (Not drSelected Is Nothing) Then
            e.Item.BackColor = Color.FromArgb(ColorHighlight)
        End If
    End Sub

    Protected Sub lstItemTypes_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstItemTypes.SelectedIndexChanged
        ScreenState(Me.lstItemTypes.SelectedIndex)
    End Sub

    Private Sub grdK1Rad_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdK1Rad.ItemCommand
        Select Case e.CommandName
            Case "Add"
                AddExclusion(e.CommandArgument, 1)
                LoadGrid(1, grdK1Rad)
                LoadExclusionsGrid(ScreenType.items)
            Case "SelectCatagory"
                CatagorySelectedRad(1, e)
        End Select
    End Sub

    Private Sub grdK2Rad_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdK2Rad.ItemCommand
        Select Case e.CommandName
            Case "Add"
                AddExclusion(e.CommandArgument, 2)
                LoadGrid(2, grdK2Rad)
                LoadExclusionsGrid(ScreenType.items)
            Case "SelectCatagory"
                CatagorySelectedRad(2, e)
        End Select
    End Sub

    Private Sub grdK3Rad_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdK3Rad.ItemCommand
        Select Case e.CommandName
            Case "Add"
                AddExclusion(e.CommandArgument, 3)
                LoadGrid(3, grdK3Rad)
                LoadExclusionsGrid(ScreenType.items)
            Case "SelectCatagory"
                CatagorySelectedRad(3, e)
        End Select
    End Sub

    Private Sub grdK1Rad_ItemDataBound1(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdK1Rad.ItemDataBound
        Dim lEdit As LinkButton

        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.AlternatingItem, Telerik.Web.UI.GridItemType.Item
                CheckIfSelected(1, e)
                If (Not CurrentUser.CurrentUserExclusionsAllowEdit) Then
                    lEdit = e.Item.FindControl("lnkEdit")
                    If Not lEdit Is Nothing Then
                        lEdit.Enabled = False
                    End If

                End If
        End Select
    End Sub

    Private Sub grdK2Rad_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdK2Rad.ItemDataBound
        Dim lEdit As LinkButton

        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.AlternatingItem, Telerik.Web.UI.GridItemType.Item
                CheckIfSelected(2, e)
                If (Not CurrentUser.CurrentUserExclusionsAllowEdit) Then
                    lEdit = e.Item.FindControl("lnkEdit")
                    If Not lEdit Is Nothing Then
                        lEdit.Enabled = False
                    End If

                End If
        End Select
    End Sub

    Private Sub grdK3Rad_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdK3Rad.ItemDataBound
        Dim lEdit As LinkButton

        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.AlternatingItem, Telerik.Web.UI.GridItemType.Item
                CheckIfSelected(3, e)
                If (Not CurrentUser.CurrentUserExclusionsAllowEdit) Then
                    lEdit = e.Item.FindControl("lnkEdit")
                    If Not lEdit Is Nothing Then
                        lEdit.Enabled = False
                    End If
                End If
        End Select
    End Sub

    Private Sub grdItmRad_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdItmRad.ItemCommand
        Select Case e.CommandName
            Case "Add"
                Dim iType As Integer
                If lstItemTypes.SelectedIndex = ScreenType.items Then
                    iType = 5
                Else
                    iType = 7
                End If
                AddExclusion(e.CommandArgument, iType)
                LoadScreen()
        End Select
    End Sub

    Private Sub grdItmRad_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdItmRad.ItemDataBound
        Dim lEdit As LinkButton

        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.AlternatingItem, Telerik.Web.UI.GridItemType.Item
                If (Not CurrentUser.CurrentUserExclusionsAllowEdit) Then
                    lEdit = e.Item.FindControl("lnkEdit")
                    lEdit.Enabled = False
                End If
        End Select
    End Sub

    Private Sub grdExclusionsRad_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdExclusionsRad.ItemCommand
        Select Case e.CommandName
            Case "Remove"
                Dim ta As New dsExclusionsTableAdapters.ExclusionsByIdTableAdapter
                Dim dt As New dsExclusions.ExclusionsByIdDataTable

                dt = ta.GetData(e.CommandArgument)
                If (dt.Rows.Count = 1) Then
                    dt.Rows(0).Delete()
                    ta.Update(dt)
                    Me.LoadScreen()
                End If
        End Select
    End Sub

    Private Sub grdExclusionsRad_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdExclusionsRad.ItemDataBound
        Dim lAccessCodeId As Label
        Dim lnkRemove As LinkButton

        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.AlternatingItem, Telerik.Web.UI.GridItemType.Item
                lAccessCodeId = e.Item.FindControl("lblAccessCode")
                lnkRemove = e.Item.FindControl("lnkRemove")
                If (lAccessCodeId.Text = "0") And (Me.ddlAccessCode.SelectedValue <> 0) Then
                    lnkRemove.Enabled = False
                    e.Item.BackColor = Color.FromArgb(ColorHighlight)
                End If
                If (Not CurrentUser.CurrentUserExclusionsAllowEdit) Then
                    lnkRemove.Enabled = False
                End If
        End Select
    End Sub

    Private Sub ScreenState(ByVal sState As ScreenType)
        Me.lstItemTypes.SelectedIndex = sState

        Select Case sState
            Case ScreenType.items
                Me.pnlItems.Visible = True
                Me.pnlKits.Visible = False
                Me.lblDocuments.Text = "Items"
                LoadGrids()
            Case ScreenType.kits
                Me.pnlItems.Visible = False
                Me.pnlKits.Visible = True
                Me.lblDocuments.Text = "Kits"
                LoadGridsKits()
                LoadItemKits(SelectedID(Session("SelectedGrid6")))
        End Select
    End Sub

    Private Sub grdKitCategories_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdKitCategories.ItemCommand
        Select Case e.CommandName
            Case "Add"
                AddExclusion(e.CommandArgument, 6)
                Me.LoadGridsKits()
            Case "SelectCatagory"
                CatagorySelectedRad(6, e)
        End Select
    End Sub

    Private Function BuildSQL_Kit(Optional ByVal sKeyword1IDs As String = "", Optional ByVal sRefNo As String = "") As String
        Dim bShowActiveOnly As Boolean : bShowActiveOnly = True
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sFrom As String = String.Empty
        Dim sKey1From As String = String.Empty
        Dim sKeyFrom As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty

        sSelect = "SELECT k.ID, k.Status, k.ReferenceNo AS RefNo, k.Description AS RefName, k.[Note] AS KitNote, InActiveDocCnt, BackOrderDocCnt " & _
                  ",  k.BaseKit, iif(k.AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ",True,False) AS LocalOwn "
        sFrom = "(Customer_Kit k LEFT JOIN qryKitDocSum ON k.ID = qryKitDocSum.KitID) " & _
                "INNER JOIN Customer_AccessCode ca ON k.AccessCodeID = ca.ID "

        sWhere = " (k.Status > " & MyBase.MyData.STATUS_INAC & ") AND " & _
                 " (InActiveDocCnt = 0) "
        If (MyBase.CurrentUser.KitViewPermissions = MyData.PERM_LOCAL) Then
            sWhere = sWhere & _
                 " AND (k.AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ") "
        End If
        If (MyBase.CurrentUser.KitViewPermissions = MyData.PERM_GLOBAL) Then
            sWhere = sWhere & _
                 " AND (" & _
                 "     (([Status] = " & MyData.PERM_TEMP & ") AND (k.AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ")) " & _
                 "  OR (([Status] = " & MyData.PERM_LOCAL & ") AND (k.AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ")) " & _
                 "  OR (([Status] = " & MyData.PERM_GLOBAL & ") AND (ca.CustomerID = " & MyBase.CurrentUser.CustomerID & ")) " & _
                 ")"
        End If

        'By Keyword Tags
        If (Len(sKeyword1IDs) > 0) Then
            sKey1From = "(SELECT KitID FROM Customer_Kit_Keyword WHERE KeywordID IN (" & sKeyword1IDs & ")) K1 "
        Else
            sKey1From = "(SELECT KitID FROM Customer_Kit_Keyword) K1 "
        End If
        sKeyFrom = "SELECT DISTINCT K1.KitID FROM " & _
                    sKey1From
        sFrom = "(" & sKeyFrom & ") kk INNER JOIN (" & _
                    sFrom & _
                    ") ON kk.KitID = k.ID"
        '

        'By Search Ref No
        If (sRefNo.Length > 0) Then
            sWhere = sWhere & _
              " AND (k.[ReferenceNo] = " & MyData.SQLString(sRefNo) & ") "
        End If
        '

        If (bShowActiveOnly) Then
            sWhere = sWhere & " AND (k.Status >" & MyData.STATUS_INAC & ") "
        End If
        If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

        sOrder = sOrder & "ORDER BY k.ReferenceNo "

        sFrom = " FROM " & sFrom
        sSQL = sSelect + sFrom & sWhere + sOrder

        Return sSQL
    End Function

    Private Sub grdKitCategories_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdKitCategories.ItemDataBound
        Dim lEdit As LinkButton

        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.AlternatingItem, Telerik.Web.UI.GridItemType.Item
                CheckIfSelected(6, e)
                If (Not CurrentUser.CurrentUserExclusionsAllowEdit) Then
                    If lEdit IsNot Nothing Then
                        lEdit = e.Item.FindControl("lnkEdit")
                        lEdit.Enabled = False
                    End If
                  
                End If
        End Select
    End Sub

    Protected Sub grdItmRad_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdItmRad.NeedDataSource

    End Sub

    Protected Sub grdExclusionsRad_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdExclusionsRad.NeedDataSource

    End Sub

    Protected Sub grdK1Rad_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdK1Rad.NeedDataSource

    End Sub

    Function GetAccessCode(ByVal intAccessCodeID As Integer) As String
        Dim ta As New dsOrdersTableAdapters.QueriesTableAdapter
        Dim strTmp As String
        If intAccessCodeID = 0 Then
            strTmp = "All"
        Else
            strTmp = ta.GetAccessCode(intAccessCodeID)
        End If

        If strTmp = "" Then strTmp = intAccessCodeID

        Return strTmp
    End Function
End Class