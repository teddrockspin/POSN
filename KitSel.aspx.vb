''''''''''''''''''''
'mw - 09-01-2009
'mw - 08-15-2009
'mw - 08-12-2009
'mw - 07-15-2009
''''''''''''''''''''
'
'Used from ...
'  OrdKSel
'
'''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''


Imports System.Text

Partial Class KitSel
  Inherits paraPageBase

  Private Const COL_ID = 0
  Private Const COL_CHK = 1

  Private Const COL_KEY = 2

  Private Const COL_ISTAT = 2
  Private Const COL_ISYM = 3
  Private Const COL_INO = 4
  Private Const COL_IDES = 5
  Private Const COL_INOTE = 6
  Private Const COL_IDOCIN = 7
  Private Const COL_IDOCBK = 8
  Private Const COL_IBASE = 9
  
  Private Const MSG_Lev0 = "No kits exist with the current category selections."
    Private Const MSG_Lev1 = "Highlight the tags within each category and click ""VIEW SELECTIONS"" to filter the items which appear in the Kit Selection Window."
  Private Const MSG_Lev2 = "Click on any line item in the Kit Selection Window to view.  Check the box if the kit is to be added to the cart."

  Protected WithEvents ibtnK1SelectAll As System.Web.UI.WebControls.ImageButton
  Protected WithEvents ibtnK1RemoveAll As System.Web.UI.WebControls.ImageButton
  Protected WithEvents ibtnItmSelectAll As System.Web.UI.WebControls.ImageButton
  Protected WithEvents ibtnItmRemoveAll As System.Web.UI.WebControls.ImageButton
  Protected WithEvents chkItmSelectAll As System.Web.UI.WebControls.CheckBox
  Protected WithEvents chkK1Select As System.Web.UI.HtmlControls.HtmlInputHidden
  Protected WithEvents txtTestK1 As System.Web.UI.WebControls.TextBox
  

#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

  End Sub

  'NOTE: The following placeholder declaration is required by the Web Form Designer.
  'Do not delete or move it.
  Private designerPlaceholderDeclaration As System.Object

  Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
    'CODEGEN: This method call is required by the Web Form Designer
    'Do not modify it using the code editor.
    InitializeComponent()

    MyBase.Page_Init(sender, e)
  End Sub




#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        PageTitle = "Kit Selection"
        If (Page.IsPostBack = False) Then MyBase.PageMessage = MSG_Lev1

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri

        End If

        If (Page.IsPostBack = False) Then
            GetShowStockSetting()
            'txtItmSelFrom.Text = Request.QueryString("C") & ""
            txtItmSelFrom.Value = Request.QueryString("C") & ""
            MyBase.CurrentOrder.VisitMark(MyBase.MyData.PRO_KIT)
        End If

        If Not Page.IsPostBack Then
            SetScrolls()
            LoadData()
        Else
            MyBase.CurrentUser.CurrentKitSels = GetSelectedGridIDs_Chk(grdItm, "Itm")
            SaveScrolls()
        End If
    End Sub


    Private Function LoadData() As Boolean
        Dim sType As String = String.Empty
        Dim sQty As String = String.Empty
        Dim bSuccess As Boolean = False

        Try
            sType = txtItmSelFrom.Value
            If Not Page.IsPostBack Then
                Select Case sType
                    Case "O"
                        If (MyBase.CurrentUser.CanExtendQtyLI) Then
                            sQty = 32767
                        Else
                            sQty = MyBase.CurrentOrder.MaxQtyPerLineItem
                        End If
                        vMaxQty.Text = "Quantity cannot exceed " & sQty
                        vMaxQty.ValueToCompare = sQty
                        vMaxQty.ErrorMessage = "Quantity cannot exceed the limit set at " & sQty & " per line item."
                        vMaxQty.CssClass = "lblInactive"

                        If Not (MyBase.CurrentUser.CanExtendQtyLI) Then
                            ibtnAddOrd.Attributes.Add("onclick", _
                            "if(parseInt(txtItmSelCnt.value) > 0) " & _
                            "{" & _
                            " if(parseInt(txtQtyOrd.value) > " & sQty & ") " & _
                            " { return(alert('Quantity added cannot exceed the limit set at " & sQty & " per line item.')==0); } " & _
                            " else { if (confirm('You are about to add selected kits to the current order.  Do you wish to continue?')) {CheckOrderQuantities();} else return false;} " & _
                            "} " & _
                            "else { alert('No selections have been made... Please select items requested to be added to cart.'); return 0; }; " & _
                            "")
                        ElseIf (MyBase.CurrentUser.CanExtendQtyLI) Then
                            ibtnAddOrd.Attributes.Add("onclick", _
                            "if(parseInt(txtItmSelCnt.value) > 0) " & _
                            "{" & _
                            " if(parseInt(txtQtyOrd.value) > " & sQty & ") " & _
                            " { return(alert('Quantity added cannot exceed the limit set at " & sQty & " per line item.')==0); } " & _
                            " else " & _
                            " { if(parseInt(txtQtyOrd.value) > " & MyBase.CurrentOrder.MaxQtyPerLineItem & ") " & _
                            "   { return confirm('Quantity added exceeding the limit set at " & MyBase.CurrentOrder.MaxQtyPerLineItem & " per line item will require authorization.  Do you wish to continue?'); } " & _
                            "   else { if (confirm('You are about to add selected kits to the current order.  Do you wish to continue?')) {CheckOrderQuantities();} else return false;} " & _
                            " } " & _
                            "} " & _
                            "else {alert('No selections have been made... Please select items requested to be added to cart.'); return 0;}; " & _
                            "")
                        End If
                        'Order - End
                        '''''''''''''''
                End Select

                ibtnEditKit.Attributes.Add("onclick", _
                                          "if(parseInt(txtItmSelCnt.value) == 0) " & _
                                          " {alert('No selections have been made... Please select a kit to edit.'); return false;} " & _
                                          "")
                ibtnViewKit.Attributes.Add("onclick", _
                                          "if(parseInt(txtItmSelCnt.value) == 0) " & _
                                          " {alert('No selections have been made... Please select a kit to view.'); return false;} " & _
                                          "")

                ibtnK1Select.Attributes.Add("onclick", "SelectAllKey(document.getElementById('frmKitSel'), 'K1'); return false;")

                bSuccess = LoadKeywords()
                SetKeySelections()
                SearchItems_ByKeyword()
                SetItmSelections()
                ShowControls()
            Else
                bSuccess = True
            End If
        Catch ex As Exception
            Session("PMsg") = "Unable to Load Data..."
        End Try

        Return (bSuccess)
    End Function

    Private Function LoadKeywords() As Boolean
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim dr As OleDb.OleDbDataReader
        Dim sSQL As String = String.Empty
        Dim bSuccess As Boolean = False
        Dim iSeq As Int16
        Dim sType As String
        Dim bVisible As Boolean

        Try
            lblK1.Text = String.Empty

            sSQL = BuildSQL_KeywordType()
            If (sSQL.Length > 0) Then
                MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
                Do While dr.Read()
                    iSeq = Val(dr("Seq").ToString & "")
                    sType = dr("KeywordType").ToString & ""
                    If (iSeq = 1) Then
                        lblK1.Text = sType
                    End If
                Loop
                MyBase.MyData.ReleaseReader2(cnn, cmd, dr)
                bSuccess = True
            End If

            bVisible = (Len(lblK1.Text) > 0)
            If bVisible Then
                pnlK1.Visible = bVisible
                sSQL = BuildSQL_Keyword(1)
                bSuccess = LoadGrid(sSQL, grdK1)
            End If

        Catch ex As Exception

        End Try

        Return bSuccess
    End Function

    Private Function LoadGrid(ByVal sSQL As String, ByRef grd As System.Web.UI.WebControls.DataGrid) As Boolean
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim ds As DataSet
        Dim bSuccess As Boolean = False

        If (sSQL.Length > 0) Then
            grd.SelectedIndex = -1
            MyBase.MyData.GetDataSet(cnn, cmd, ds, sSQL)
            If (grd.ID = "grdK1") Then
                RemoveExcludedCategories(ds)
            Else
                RemoveExcludedItems(ds)
            End If
            grd.DataSource = ds
            If (grd.PageCount < grd.CurrentPageIndex) Then grd.CurrentPageIndex = 0
            grd.DataBind()
            bSuccess = True
        End If

        Return (bSuccess)
    End Function

    Private Sub RemoveExcludedCategories(ByVal ds As DataSet)
        Dim taExclusions As New dsExclusionsTableAdapters.GetExclusionsForKitsTableAdapter
        Dim dtExclusions As New dsExclusions.GetExclusionsForKitsDataTable
        Dim dRow As DataRow

        For Each dRow In ds.Tables(0).Rows
            taExclusions.Fill(dtExclusions, dRow("Id"), CurrentUser.AccessCodeID, dRow("Id"), CurrentUser.CustomerID)
            If (dtExclusions.Rows.Count > 0) Then
                dRow.Delete()
            End If
        Next
    End Sub

    Private Function ClearGrid(ByRef grd As System.Web.UI.WebControls.DataGrid) As Boolean
        MyBase.PageMessage = MSG_Lev1
        grd.DataSource = Nothing
        grd.DataBind()
    End Function

    Private Function LoadItem(ByVal sK1IDs As String) As Boolean
        Dim bSuccess As Boolean = False
        Dim sSQL As String
        Dim ds As DataSet
        Dim lRcdCnt As Long = 0

        txtItmRow.Value = -1
        txtItmSelCnt.Value = 0

        sSQL = BuildSQL_Itm(sK1IDs)
        bSuccess = LoadGrid(sSQL, grdItm)

        ds = grdItm.DataSource

        lRcdCnt = ds.Tables(0).Rows.Count
        If (lRcdCnt > 0) Then
            If (Val(txtItmSelCnt.Value) = 0) And (MyBase.CurrentOrder.ItmCart.Rows.Count = 0) Then MyBase.PageMessage = MSG_Lev2
        Else
            MyBase.PageMessage = MSG_Lev0 '& "  " & MSG_Lev1
        End If

        MyBase.CurrentUser.CurrentK1KitSels = GetSelectedGridIDs(grdK1, "K1")
        SaveScrolls()
    End Function

    Private Sub RemoveExcludedItems(ByVal ds As DataSet)
        Dim taExclusions As New dsExclusionsTableAdapters.ExclusionsByTypeTableAdapter
        Dim dtExclusions As New dsExclusions.ExclusionsByTypeDataTable
        Dim drExclusions() As dsExclusions.ExclusionsByTypeRow
        Dim dRow As DataRow
        Dim iCounter As Integer

        taExclusions.Fill(dtExclusions, 7, CurrentUser.AccessCodeID, 7, CurrentUser.CustomerID)

        For iCounter = 0 To ds.Tables(0).Rows.Count - 1
            dRow = ds.Tables(0).Rows(iCounter)
            Try
                drExclusions = dtExclusions.Select(String.Format("Description={0}", dRow("Id")))
                If (drExclusions.GetUpperBound(0) > -1) Then
                    dRow.Delete()
                End If
            Catch ex As Exception
            End Try
        Next
        ds.AcceptChanges()
    End Sub

    Private Function SetKeySelections() As String
        SetGridSelections(grdK1, "K1", MyBase.CurrentUser.CurrentK1KitSels)
    End Function

    Private Function SetItmSelections() As String
        SetGridSelections(grdItm, "Itm", MyBase.CurrentUser.CurrentKitSels)
    End Function

    Private Sub ClearScrolls()
        Dim s As String
        Dim a() As String
        Try
            s = MyBase.CurrentUser.CurrentKitScrolls
            a = s.Split("|")
            ScrollXPosDoc.Value = 0
            ScrollYPosDoc.Value = 0
            ScrollXPosItm.Value = 0
            ScrollXPosK1.Value = 0
        Catch ex As Exception
        End Try
    End Sub

    Private Sub SetScrolls()
        Dim s As String
        Dim a() As String
        Try
            s = MyBase.CurrentUser.CurrentKitScrolls
            a = s.Split("|")
            ScrollXPosDoc.Value = Val(a(1))
            ScrollYPosDoc.Value = Val(a(2))
            ScrollXPosItm.Value = Val(a(3))
            ScrollXPosK1.Value = Val(a(4))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub SaveScrolls()
        Dim s As String
        Try
            s = "|" & Val(ScrollXPosDoc.Value) & "|" & _
                      Val(ScrollYPosDoc.Value) & "|" & _
                      Val(ScrollXPosItm.Value) & "|" & _
                      Val(ScrollXPosK1.Value) & "|"
            MyBase.CurrentUser.CurrentKitScrolls = s
        Catch ex As Exception
        End Try
    End Sub

    Private Function SetGridSelections(ByRef grd As System.Web.UI.WebControls.DataGrid, ByVal sGrid As String, ByVal sSetIDs As String) As String
        'Note:  ID is Column 0
        'Note:  Check is Column 1
        'Note:  Buttons are Column 2
        Dim indx As Integer
        Dim iMax As Integer
        Dim sID As String = String.Empty
        Dim sSelIDs As String = String.Empty
        Dim sShpIDs As String = String.Empty
        Dim iPosSel As Integer
        Dim iPosShp As Integer
        Dim chk As WebControls.CheckBox
        Dim bSuccess As Boolean = False

        Try
            sSelIDs = "!" & Replace(sSetIDs, ",", "!") & "!"
            If (sGrid = "Itm") Then sShpIDs = "!" & Replace(MyBase.CurrentOrder.KitCartString, ",", "!") & "!"
            iMax = grd.Items.Count - 1
            For indx = 0 To iMax
                sID = "!" & Val(grd.Items(indx).Cells(COL_ID).Text.ToString()) & "!"
                iPosSel = InStr(sSelIDs, sID)
                iPosShp = InStr(sShpIDs, sID)
                If (sGrid = "Itm") And (iPosShp > 0) Then
                    SelectRow_Itm(grd.Items(indx), sGrid, 3)
                ElseIf (iPosSel > 0) Then
                    chk = grd.Items(indx).Cells(COL_CHK).FindControl("chk" & sGrid & "Select")
                    If (sGrid = "Itm") Then
                        'txtItmSelCnt.Text = Val(txtItmSelCnt.Text) + 1
                        txtItmSelCnt.Value = Val(txtItmSelCnt.Value) + 1
                        SelectRow_Itm(grd.Items(indx), sGrid, 2)
                    Else
                        SelectRow_Key(grd.Items(indx), sGrid, 1)
                    End If
                End If
            Next indx
            bSuccess = True

        Catch ex As Exception

        End Try
        chk = Nothing

        Return bSuccess
    End Function

    Private Function IsInShoppingCart(ByVal sID As String) As Boolean
        Dim sShpIDs As String = String.Empty
        Dim iPosShp As Integer
        Dim bInCart As Boolean = False

        Try
            sShpIDs = "!" & Replace(MyBase.CurrentOrder.KitCartString, ",", "!") & "!"
            iPosShp = InStr(sShpIDs, sID)
            If (iPosShp > 0) Then
                bInCart = True
            End If
            Return bInCart

        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function BuildSQL_KeywordType() As String
        Dim bShowActiveOnly As Boolean : bShowActiveOnly = True
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty

        sSelect = "SELECT ck.ID, ck.Seq AS Seq, ck.Name AS KeywordType " & _
                  "FROM Customer_Key ck "
        sWhere = " (ck.CustomerID = " & MyBase.CurrentUser.CustomerID & ") "

        sWhere = sWhere & " AND (ck.Status >" & MyData.STATUS_INAC & ") "
        If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

        sOrder = sOrder & " ORDER BY ck.Seq, ck.Name "

        sSQL = sSelect + sWhere + sOrder

        Return sSQL
    End Function

    Private Function BuildSQL_Keyword(ByVal iSeq As Int16) As String
        Dim bShowActiveOnly As Boolean : bShowActiveOnly = True
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty

        sSelect = "SELECT w.ID, k.Name AS KeywordType, w.Description AS Keyword " & _
                  "FROM Customer_Key k INNER JOIN Customer_Keyword w " & _
                  "ON k.ID = w.KeyID "

        sWhere = " (k.CustomerID = " & MyBase.CurrentUser.CustomerID & ") " & _
             " AND (k.Seq = " & iSeq & ")"

        If (bShowActiveOnly) Then
            sWhere = sWhere & " AND (k.Status >" & MyData.STATUS_INAC & ") "
        End If
        If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

        sOrder = sOrder & " ORDER BY w.Description "

        sSQL = sSelect + sWhere + sOrder

        Return sSQL
    End Function

    Private Function BuildSQL_Itm(Optional ByVal sKeyword1IDs As String = "", Optional ByVal sRefNo As String = "") As String
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

    Private Sub ClearNote()
        lblKitRef.Text = String.Empty
        lblKitDes.Text = String.Empty
        lblKitNote.Text = String.Empty
    End Sub

    Protected Sub grdK1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdK1.ItemDataBound
        grdKey_ItemDataBound("K1", sender, e)
    End Sub

    Protected Sub grdKey_ItemDataBound(ByVal sGrid As String, ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        Dim btn As WebControls.Button

        If (e.Item.ItemType = ListItemType.Item Or _
            e.Item.ItemType = ListItemType.AlternatingItem Or _
            e.Item.ItemType = ListItemType.SelectedItem Or _
            e.Item.ItemType = ListItemType.EditItem) Then

            btn = e.Item.Cells(COL_KEY).FindControl("btnKeyword")
            btn.Attributes("onClick") = "javascript:SelectRowKey(document.getElementById('" & btn.ClientID & "'));return false;"

        End If
        btn = Nothing
    End Sub

    Protected Sub grdItm_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdItm.ItemDataBound
        Dim iStatus As Int16
        Dim iDocCntInac As Int16
        Dim iDocCntBack As Int16
        Dim bIsBaseKit As Boolean
        Dim btnNo As WebControls.Button
        Dim btnName As WebControls.Button
        Dim btnStock As WebControls.Button

        Dim chk As WebControls.CheckBox
        Dim sSym As String = String.Empty
        Dim sID As String = String.Empty

        If (e.Item.ItemType = ListItemType.Item Or _
            e.Item.ItemType = ListItemType.AlternatingItem Or _
            e.Item.ItemType = ListItemType.SelectedItem Or _
            e.Item.ItemType = ListItemType.EditItem) Then

            sID = e.Item.Cells(COL_ID).Text
            iStatus = Convert.ToInt16(e.Item.Cells(COL_ISTAT).Text.ToString)
            iDocCntInac = Convert.ToInt16(e.Item.Cells(COL_IDOCIN).Text.ToString)
            iDocCntBack = Convert.ToInt16(e.Item.Cells(COL_IDOCBK).Text.ToString)
            bIsBaseKit = Convert.ToInt16(e.Item.Cells(COL_IBASE).Text.ToString)

            chk = e.Item.Cells(COL_CHK).FindControl("chkSelect")
            btnNo = e.Item.Cells(COL_INO).FindControl("btnItmNo")
            btnName = e.Item.Cells(COL_INO).FindControl("btnItmName")
            btnStock = e.Item.Cells(COL_INO).FindControl("btnStock")

            If (MyBase.CurrentUser.CanExtendQtyLI) Then
                chk.Attributes("onclick") = String.Format("javascript:SelectRowItm(document.getElementById('{0}'));", chk.ClientID)
            Else
                chk.Attributes("onclick") = String.Format("{0}; if (CheckComponentQuantityLimit(components, limits, arrayupperlimit, document.getElementById('{1}').value, document.getElementById('{2}'))) {{SelectRowItm(document.getElementById('{3}'))}}", Me.ComponentQuantityLimitArrays(sID), Me.txtQtyOrd.ClientID, chk.ClientID, chk.ClientID)
            End If

            btnNo.Attributes("onClick") = "javascript:HighlighRowItm(document.getElementById('" & btnNo.ClientID & "')); return false;"
            btnName.Attributes("onClick") = "javascript:HighlighRowItm(document.getElementById('" & btnName.ClientID & "')); return false;"
            btnStock.Attributes("onClick") = "javascript:HighlighRowItm(document.getElementById('" & btnName.ClientID & "')); return false;"

            If (iDocCntInac > 0) Then
                e.Item.Cells(COL_ISYM).Text = " - "
                e.Item.ForeColor = Color.Red                    'MyData.COLOR_INAC
                btnNo.ForeColor = Color.Red                     'MyData.COLOR_INAC
                btnName.ForeColor = Color.Red                   'MyData.COLOR_INAC
                btnStock.ForeColor = Color.Red

            ElseIf (iDocCntBack > 0) Then

                e.Item.Cells(COL_ISYM).Text = " * "
                e.Item.ForeColor = Color.FromArgb(189, 83, 4)   'MyData.COLOR_BACK
                btnNo.ForeColor = Color.FromArgb(189, 83, 4)    'MyData.COLOR_BACK
                btnName.ForeColor = Color.FromArgb(189, 83, 4)  'MyData.COLOR_BACK
                btnStock.ForeColor = Color.FromArgb(189, 83, 4)

            ElseIf (bIsBaseKit = True) Then
                e.Item.Cells(COL_ISYM).Text = " + "
                e.Item.ForeColor = Color.Purple                 'MyData.COLOR_BaseKit
                btnNo.ForeColor = Color.Purple                  'MyData.COLOR_BaseKit
                btnName.ForeColor = Color.Purple                'MyData.COLOR_BaseKit
                btnStock.ForeColor = Color.Purple
            Else
                e.Item.ForeColor = Color.Black                  'MyBase.MyData.COLOR_NORMAL
                btnNo.ForeColor = Color.Black                   'MyBase.MyData.COLOR_NORMAL
                btnName.ForeColor = Color.Black                 'MyBase.MyData.COLOR_NORMAL
                btnStock.ForeColor = Color.Black
            End If

            If (IsInShoppingCart(sID) = True) Then
                SelectRow_Itm(e.Item, "Itm", 3)
            End If

        End If

        chk = Nothing
        btnNo = Nothing
        btnName = Nothing
    End Sub

    Private Function ComponentQuantityLimitArrays(ByVal iKitID As Integer) As String
        Dim Components As New StringBuilder
        Dim sComponents As String
        Dim Limits As New StringBuilder
        Dim sLimits As String
        Dim ds As New dsInventoryTableAdapters.ComponentQuantityLimitTableAdapter
        Dim dt As New dsInventory.ComponentQuantityLimitDataTable
        Dim dr As dsInventory.ComponentQuantityLimitRow
        Dim iCounter As Integer = -1

        ds.Fill(dt, iKitID)
        For Each dr In dt.Rows
            Components.Append(String.Format("'{0}', ", dr.DocumentID.Replace("'", "").Replace(",", "")))
            Dim lng1 As Long = dr.OrderQuantityLimit1
            Dim lng2 As Long = dr.Qty

            Limits.Append(String.Format("{0}, ", lng2 * lng1))
            iCounter += 1
        Next

        sComponents = Components.ToString
        sLimits = Limits.ToString

        If sComponents.EndsWith(", ") Then
            sComponents = sComponents.Substring(0, Len(sComponents) - 2)
            sLimits = sLimits.Substring(0, Len(sLimits) - 2)
        End If

        Return String.Format("var components=[{0}]; var limits=[{1}]; var arrayupperlimit={2}", sComponents, sLimits, iCounter)
    End Function

    Protected Sub grdItm_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdItm.ItemCommand
        'grdItem_ItemCommand("Itm", source, e)
    End Sub

    Private Sub SelectRow_Key(ByRef e As Object, ByVal sGrid As String, ByVal bSelect As Byte)
        'Yellow - #FFFF80
        Dim btn As WebControls.Button
        Dim ctl As System.Web.UI.HtmlControls.HtmlInputHidden

        Try
            ctl = e.Cells(COL_CHK).FindControl("chkSelect")
            If (bSelect = 1) Then 'Checked
                e.BackColor = Color.FromArgb(255, 255, 128) 'Light Yellow
                If (ctl.Value = "0") Then ctl.Value = "1"
            ElseIf (bSelect = 0) Then ' UnChecked
                e.BackColor = Color.White
                If (ctl.Value = "1") Then ctl.Value = "0"
            End If

            btn = e.Cells(COL_KEY).FindControl("btnKeyword")
            btn.BackColor = e.BackColor

        Catch ex As Exception

        End Try
        btn = Nothing
        ctl = Nothing
    End Sub

    Private Sub SelectRow_Itm(ByRef e As Object, ByVal sGrid As String, ByVal bSelect As Byte)
        'Green - #3dbc45 '#008400
        'Light Green - #92ee8e
        'Yellow - #FFFF80
        Dim btn As WebControls.Button
        Dim chk As WebControls.CheckBox

        Try
            If (bSelect = 1) Then 'Checked
                e.BackColor = Color.FromArgb(255, 255, 128) 'Light Yellow
                chk = e.Cells(COL_CHK).FindControl("chkSelect")
                If chk.Checked = False Then chk.Checked = True
            ElseIf (bSelect = 2) Then 'Checked but secondary (Item Green)
                e.BackColor = Color.FromArgb(61, 188, 69) 'Green
                chk = e.Cells(COL_CHK).FindControl("chkSelect")
                If chk.Checked = False Then chk.Checked = True
            ElseIf (bSelect = 3) Then 'Already in the Cart (Item Light Green)
                e.BackColor = Color.FromArgb(146, 238, 142) 'Light Green
                chk = e.Cells(COL_CHK).FindControl("chkSelect")
                If chk.Checked = True Then chk.Checked = False
                If chk.Enabled = True Then chk.Enabled = False
            ElseIf (bSelect = 0) Then ' UnChecked
                e.BackColor = Color.White
            End If

            btn = e.Cells(COL_INO).FindControl("btnItmNo")
            btn.BackColor = e.BackColor
            btn = e.Cells(COL_IDES).FindControl("btnItmName")
            btn.BackColor = e.BackColor

        Catch ex As Exception

        End Try
        chk = Nothing
        btn = Nothing
    End Sub

    Private Function GridSelectUpdate(ByRef grd As System.Web.UI.WebControls.DataGrid, ByVal sGrid As String, ByVal bChecked As Byte, Optional ByVal bUpdate As Boolean = True)
        'Note:  ID is Column 0
        'Note:  Check is Column 1
        Dim sID As String = String.Empty
        Dim indx As Integer
        Dim iMax As Integer

        iMax = grd.Items.Count - 1
        For indx = 0 To iMax
            sID = grd.Items(indx).Cells(COL_ID).Text
            If (sGrid = "Itm") And (IsInShoppingCart(sID) = True) Then
                SelectRow_Itm(grd.Items(indx), sGrid, 3)
            Else
                If bChecked And (Val(grd.Items(indx).Cells(COL_CHK).Text) = 0) Then
                    grd.Items(indx).Cells(COL_CHK).Text = 1
                ElseIf Not bChecked And (Val(grd.Items(indx).Cells(COL_CHK).Text) = 1) Then
                    grd.Items(indx).Cells(COL_CHK).Text = 0
                End If
                SelectRow_Key(grd.Items(indx), sGrid, bChecked)
            End If
        Next indx

        If (bUpdate = True) Then SearchItems_ByKeyword()
    End Function

    Private Function GetSelectedGridIDs(ByRef grd As System.Web.UI.WebControls.DataGrid, ByVal sGrid As String) As String
        'Note:  ID is Column 0
        'Note:  Check is Column 1
        'Note:  Buttons are Column 2
        Dim indx As Integer
        Dim iMax As Integer
        Dim sIDs As String = String.Empty
        Dim ctl As System.Web.UI.HtmlControls.HtmlInputHidden

        Try
            iMax = grd.Items.Count - 1
            For indx = 0 To iMax
                ctl = grd.Items(indx).Cells(COL_CHK).FindControl("chkSelect")
                If (Val(ctl.Value) = 1) Then
                    If (Len(sIDs) > 0) Then sIDs = sIDs & ","
                    sIDs = sIDs & Val(grd.Items(indx).Cells(COL_ID).Text.ToString())
                End If
            Next indx
        Catch ex As Exception

        End Try

        ctl = Nothing
        Return sIDs
    End Function

    Private Function GetSelectedGridIDs_Chk(ByRef grd As System.Web.UI.WebControls.DataGrid, ByVal sGrid As String) As String
        'Note:  ID is Column 0
        'Note:  Check is Column 1
        'Note:  Buttons are Column 2
        Dim indx As Integer
        Dim iMax As Integer
        Dim sIDs As String = String.Empty
        Dim chk As WebControls.CheckBox

        Try
            iMax = grd.Items.Count - 1
            For indx = 0 To iMax
                chk = grd.Items(indx).Cells(COL_CHK).FindControl("chkSelect")
                If (chk.Checked = True) Then
                    If (Len(sIDs) > 0) Then sIDs = sIDs & ","
                    sIDs = sIDs & Val(grd.Items(indx).Cells(COL_ID).Text.ToString())
                End If
            Next indx
        Catch ex As Exception

        End Try

        chk = Nothing
        Return sIDs
    End Function

    Private Sub SearchItems_ByKeyword()
        Dim sK1IDs As String
        Dim sK2IDs As String
        Dim sK3IDs As String
        Dim sK4IDs As String

        ClearNote()
        sK1IDs = GetSelectedGridIDs(grdK1, "K1")

        If (Len(sK1IDs) > 0) Then
            LoadItem(sK1IDs)
        Else
            ClearGrid(grdItm)
        End If
    End Sub

    Private Function ShowControls() As Boolean
        Dim bShowEdit As Boolean = False

        Select Case txtItmSelFrom.Value
            Case "I"
                If (pnlBtnOrd.Visible = True) Then pnlBtnOrd.Visible = False

            Case "K"
                If (pnlBtnOrd.Visible = True) Then pnlBtnOrd.Visible = False

            Case "O"
                If (pnlBtnOrd.Visible = False) Then pnlBtnOrd.Visible = True

                bShowEdit = True
                ibtnAddOrd.Visible = bShowEdit

        End Select

        Return True
    End Function

    Private Sub ibtnGet_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnGet.Click
        MyBase.CurrentUser.KitSelsInit()
        SearchItems_ByKeyword()
    End Sub

    Private Sub ibtnRestart_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRestart.Click
        ClearScrolls()
        GridSelectUpdate(grdK1, "K1", 0, False)
        ClearGrid(grdItm)
        SearchItems_ByKeyword()
        If (Val(txtItmSelCnt.Value) = 0) And (MyBase.CurrentOrder.KitCart.Rows.Count = 0) Then MyBase.PageMessage = MSG_Lev2
    End Sub

    Private Sub ibtnViewKit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnViewKit.Click
        Dim lID As Long
        Dim bIsBase As Boolean

        lID = Val(CurrentItemID(bIsBase))
        If (lID > 0) Then
            Response.Write("<script language='javascript'>" & _
                            "  location.href = 'KitVw.aspx?ID=" & lID & "';" & _
                            "</script>")
        End If
    End Sub

    Private Sub ibtnEditKit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnEditKit.Click
        Dim lID As Long
        Dim bIsBase As Boolean

        lID = Val(CurrentItemID(bIsBase))
        If (lID > 0) And (bIsBase = True) Then
            Response.Write("<script language='javascript'>" & _
                            "  location.href = 'AdmKitE.aspx?ED=" & lID & "&Base=1';" & _
                            "</script>")
        End If
    End Sub


  ''''''''''''''''''''''''''''''
  'Ord - Item Selections
    Private Function ValidQuantity_Ord() As Boolean
        Dim bValid As Boolean : bValid = False

        bValid = ((Convert.ToInt16(Val(txtQtyOrd.Text)) <= MyBase.CurrentOrder.MaxQtyPerLineItem)) Or _
                 ((Convert.ToInt16(Val(txtQtyOrd.Text)) > MyBase.CurrentOrder.MaxQtyPerLineItem) And MyBase.CurrentUser.CanExtendQtyLI)
        If Not bValid Then
            MyBase.PageMessage = "Quantity to add exceeds maximum limit set at " & MyBase.CurrentOrder.MaxQtyPerLineItem
        End If

        Return bValid
    End Function

    Private Function SaveSelections_Ord() As Boolean
        Dim iCnt As Integer : iCnt = 0
        Dim indx As Integer
        Dim iMax As Integer
        Dim btn As WebControls.Button
        Dim chk As WebControls.CheckBox
        Dim iQty As Int16
        Dim lID As Int32
        Dim sName As String
        Dim sDesc As String
        Dim sColor As String
        Dim ipos As Integer

        Try
            iMax = grdItm.Items.Count - 1
            For indx = 0 To iMax
                With grdItm.Items(indx)
                    chk = .Cells(COL_CHK).FindControl("chkSelect")
                    If Val(chk.Checked = True) Then
                        iCnt = iCnt + 1
                        iQty = txtQtyOrd.Text.ToString
                        lID = Val(.Cells(COL_ID).Text.ToString())
                        btn = .Cells(COL_INO).FindControl("btnItmNo")
                        sName = btn.Text.ToString()
                        btn = .Cells(COL_IDES).FindControl("btnItmName")
                        sDesc = btn.Text.ToString()
                        'No Longer Needed 
                        '      sColor = lstItem.Items(indx).Attributes.Item("Style")

                        If checkIfItemInKitIsBackordered(lID) Then
                            sColor = MyData.COLOR_BACK
                        Else
                            sColor = ""
                        End If


                        MyBase.CurrentOrder.KitSave(iQty, lID, sName, sDesc, sColor)
                    End If
                End With
            Next
            MyBase.CurrentUser.CurrentKitSels = String.Empty
            MyBase.SessionStore("KitSel?C=O")

        Catch ex As Exception
            iCnt = 0
        End Try
        btn = Nothing

        Return (iCnt > 0)
    End Function

    Private Overloads Sub ibtnAddOrd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnAddOrd.Click
        If ValidQuantity_Ord() Then
            If SaveSelections_Ord() Then
                MyBase.PageMessage = "Kits Added - " & MyBase.CurrentOrder.KitCart.Rows.Count & " kits currently in cart."
                'Remove all Item Selections
                GridSelectUpdate(grdItm, "Itm", 0, False)
            End If
        End If
    End Sub

    Private Sub ibtnItmOrd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnItmOrd.Click
        Dim sDestin As String = paraPageBase.PAG_OrdItemSelect.ToString()
        MyBase.PageDirect(sDestin, 0, 0)
    End Sub

    Private Sub ibtnCartOrd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCartOrd.Click
        Dim sDestin As String = paraPageBase.PAG_OrdCart.ToString()
        MyBase.PageDirect(sDestin, 0, 0)
    End Sub

    Private Function CurrentItemID(Optional ByRef bIsBase As Boolean = False) As Long
        Dim sID As String = String.Empty
        Dim indx As Integer
        Dim iMax As Integer
        Dim bDone As Boolean : bDone = False
        Dim chk As WebControls.CheckBox

        iMax = grdItm.Items.Count - 1
        For indx = 0 To iMax
            With grdItm.Items(indx)
                chk = .Cells(COL_CHK).FindControl("chkSelect")
                If Val(chk.Checked = True) Then
                    'If Not bDone And Val(.Cells(COL_CHK).Text) = 1 Then
                    sID = Val(.Cells(COL_ID).Text.ToString())
                    bIsBase = (Val(.Cells(COL_IBASE).Text.ToString()) = 1)
                    bDone = True
                End If
            End With
        Next

        chk = Nothing
        Return Val(sID)
    End Function
    Sub GetShowStockSetting()
        Dim ta As New dsAccessCodesTableAdapters.Customer_AccessCode1TableAdapter
        Dim dr As dsAccessCodes.Customer_AccessCode1Row
        dr = ta.GetDataByAccessCodeID(CurrentUser.AccessCodeID).Rows(0)
        hfShowStock.Value = dr.ShowStock
        If dr.ShowStock = False Then
            grdItm.Columns(11).HeaderText = ""
        End If

    End Sub
    Function getShowStock(intKitID As Integer) As String
        If hfShowStock.Value Then
            Dim o As New paraData
            Return o.GetKitStock(True, intKitID)
        Else
            Return ""
        End If

    End Function

    Function checkIfItemInKitIsBackordered(iKitID As Integer) As Boolean

        Dim ta As New dsCustomerDocumentTableAdapters.Customer_kit_with_documentsTableAdapter
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter

        For Each dr As dsCustomerDocument.Customer_kit_with_documentsRow In ta.GetDataByKitID(iKitID).Rows
            If qa.GetDocumentStatusIDByDocumentID(dr.DocumentID) = 2 Then
                Return True
            End If
        Next
        Return False
    End Function

End Class
