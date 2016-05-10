
'mw - 09-01-2009
'mw - 08-15-2009
'mw - 07-15-2009
'mw - 05-27-2009
''''''''''''''''''''
'
'Used from ...
'  OrdISel
'  AdmISel
'  AdmKitISel
'
'''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''


Imports System.Text
Imports System.IO
Imports System.Web.Services
Imports System.Web.Script.Services

Partial Class ItmSel
    Inherits paraPageBase

    Private Const COL_ID = 0
    Private Const COL_CHK = 1

    'Private Const COL_KEY = 2
    'Private Const COL_ISTAT = 2
    'Private Const COL_ISYM = 3
    'Private Const COL_INO = 4
    'Private Const COL_IDES = 5

    Private Const COL_KEY = 3
    Private Const COL_ISTAT = 3
    Private Const COL_ISYM = 4
    Private Const COL_INO = 5
    Private Const COL_IDES = 6

    Private Const MSG_Lev0 = "No items exist with the current category selections."
    Private Const MSG_Lev1 = "Highlight the tags within each category and click ""VIEW SELECTIONS"" to filter the items which appear in the Item Selection Window."
    Private Const MSG_Lev2 = "Click on any line item in the Item Selection Window to view. If you want the item added to your cart, check the box and press Add To Cart at the bottom of the screen."

    Protected WithEvents ibtnK2SelectAll As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ibtnK2RemoveAll As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ibtnK3SelectAll As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ibtnK3RemoveAll As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ibtnK4SelectAll As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ibtnK4RemoveAll As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ibtnItmSelectAll As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ibtnItmRemoveAll As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ibtnAdd As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ibtnKit As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ibtnCart As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdBackKit As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblItmRow As System.Web.UI.WebControls.Label
    Protected WithEvents lblItmRCnt As System.Web.UI.WebControls.Label
    Protected WithEvents searchHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents chkK1Select As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents chkK2Select As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents chkK3Select As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents chkK4Select As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected qoEnabled As Boolean = False
    Protected hasDnldItems As Boolean = False


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.ImageButton
    Protected WithEvents txtQty As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblQty As System.Web.UI.WebControls.Label
    Protected WithEvents cmdKit As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdCart As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblImgRef As System.Web.UI.WebControls.Label
    Protected WithEvents lblImgDes As System.Web.UI.WebControls.Label
    Protected WithEvents imgItem As System.Web.UI.WebControls.Image
    Protected WithEvents lkHigh As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHighSize As System.Web.UI.WebControls.Label
    Protected WithEvents lkBase As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblBaseSize As System.Web.UI.WebControls.Label
    Protected WithEvents lkLow As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblLowSize As System.Web.UI.WebControls.Label
    Protected WithEvents lblCat As System.Web.UI.WebControls.Label
    Protected WithEvents cmdAllCat As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblSub1 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdAllSub1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblSub2 As System.Web.UI.WebControls.Label
    Protected WithEvents frmOrdISel As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents fraKeyWrd As System.Web.UI.HtmlControls.HtmlGenericControl

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()

        MyBase.Page_Init(sender, e)

        If AjaxRequest Then
            If Request.Params("type") = "validateitems" Then
                Response.Clear()
                Response.ContentType = "text/HTML"
                Response.Write(GetAddToCartMessage(Request.Params("data"), Request.Params("qty")))

                Response.End()

            End If
        End If
    End Sub

#End Region

    Public bCanOrderDownloadItems As Boolean = False

    Private startTime As DateTime = DateTime.Now

    Public ReadOnly Property AjaxRequest As Boolean
        Get
            If String.IsNullOrEmpty(Request.Params("ajax")) Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Protected Overloads Overrides Sub OnPreRender(ByVal e As EventArgs)
        MyBase.OnPreRender(e)
        Dim dblLoadTime As Double = (DateTime.Now - startTime).TotalSeconds
        Me.LoadTime.Text = dblLoadTime
        If dblLoadTime > 20 Then
            'write to even long
            Dim pb As New paraPageBase
            pb.writeToEvenLog("ItemSel.aspx load time (sec):" & dblLoadTime.ToString, Diagnostics.EventLogEntryType.Warning)

        End If
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckIfShowGoogleDocs()



        Dim ca As New dsCustomerTableAdapters.CustomerQuickOrderDLTableAdapter
        If (Not Boolean.TryParse(ca.GetData(Convert.ToInt32(CurrentUser.CustomerID)).Rows(0)("AllowQuickOrderDl").ToString(), qoEnabled)) Then
            qoEnabled = False
        End If


        Dim qa As New dsAccessCodesTableAdapters.QueriesTableAdapter
        If Not Convert.ToBoolean(qa.CheckShowStatus(CurrentUser.AccessCode, CurrentUser.CustomerID)) Then
            setStockVisable()
        End If

        If Not qa.CheckIfAllowedPurchaseDownloadableItems(CurrentUser.AccessCodeID) Then
            setDnldClnInvisable()
        End If

        Try
            bCanOrderDownloadItems = qa.CheckIfAllowedPurchaseDownloadableItems(CurrentUser.AccessCodeID)
        Catch ex As Exception
            Response.Redirect("login.aspx")
        End Try

        If bCanOrderDownloadItems = False Then
            grdItm.Columns(2).HeaderText = ""
        End If


        PageTitle = "Item Selection"
        If (Page.IsPostBack = False) Then MyBase.PageMessage = MSG_Lev1

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri
        End If

        If (Page.IsPostBack = False) Then
            txtItmSelFrom.Value = Request.QueryString("C") & ""
            MyBase.CurrentOrder.VisitMark(MyBase.MyData.PRO_ITM)

        End If

        If Not Page.IsPostBack Then
            GetShowStockSetting()
            SetScrolls()
            LoadData()

        Else
            MyBase.CurrentUser.CurrentItmSels(grdItm.CurrentPageIndex) = GetSelectedGridIDs_Chk(grdItm, "Itm")
            SaveScrolls()
        End If
    End Sub

    Private Function GetAddToCartMessage(ByVal SelectedItems As String, ByVal QtyToOrder As Integer) As String
        Dim sScript, sSQL As String
        Dim cnn As SqlClient.SqlConnection
        Dim cmd As SqlClient.SqlCommand
        Dim ds As DataSet
        Dim dRow As DataRow
        Dim BackOrdered As Boolean = False
        Dim PartNumbers As String = ""

        If SelectedItems <> "" Then
            SelectedItems = SelectedItems.Remove(SelectedItems.Length - 1)
        End If

        sSQL = BuildSQL_Itm("", "", "", "", "", SelectedItems)
        MyBase.MyData.GetDataSetSql(cnn, cmd, ds, sSQL)

        For Each dRow In ds.Tables(0).Rows
            If CBool(dRow("OnBackOrder")) = True Or (CInt(dRow("CurrentStock") < QtyToOrder And CInt(dRow("PrintMethod")) = 0)) Then
                BackOrdered = True
                PartNumbers &= dRow("RefNo") & ","
            End If
        Next

        If PartNumbers <> "" Then
            PartNumbers = PartNumbers.Remove(PartNumbers.Length - 1)
        End If

        If BackOrdered Then ' if one or more selected items is backordered then use this script
            sScript = "if(parseInt(txtItmSelCnt.value) > 0) " & _
           "{" & _
            "if (confirm('Of the items you are about to add to the current order, the following are currently on Backorder or will be Backordered as a result of your request. Part Number(s) (" & PartNumbers & "). All items selected will be added to your current order. Do you wish to continue?')) {return true;} else {return false} }" & _
            "else {alert('No selections have been made... Please select items requested to be added to cart.'); return false;}; "

        Else
            sScript = "if(parseInt(txtItmSelCnt.value) > 0) " & _
            "{" & _
            "if (confirm('You are about to add selected items to the current order.  Do you wish to continue?')) {return true;} else {return false} }" & _
            "else {alert('No selections have been made... Please select items requested to be added to cart.'); return false;}; "
        End If

        'If item being added to cart is in Backorder mode or if the quantity being requested exceeds current inventory level (“FUL” only items) the dialog box should read: 
        '“Of the items you are about to add to the current order, the following are currently on Backorder or will be Backordered as a result of your request. Part Number ### piece in inventory 
        'All items selected will be added to your current order. Do you wish to continue?"



        Return sScript
    End Function

    Sub setStockVisable()

        Dim itemStyle As New HtmlGenericControl("style")
        itemStyle.Attributes.Add("type", "text/css")
        itemStyle.InnerHtml = ".hdn{display:none;}"
        Page.Controls.Add(itemStyle)
    End Sub

    Sub setDnldClnInvisable()

        Dim itemStyle As New HtmlGenericControl("style")
        itemStyle.Attributes.Add("type", "text/css")
        itemStyle.InnerHtml = ".hdnDnld{display:none;}"
        Page.Controls.Add(itemStyle)
    End Sub

    Sub GetShowStockSetting()
        Dim ta As New dsAccessCodesTableAdapters.Customer_AccessCode1TableAdapter
        Dim dr As dsAccessCodes.Customer_AccessCode1Row
        Try
            dr = ta.GetDataByAccessCodeID(CurrentUser.AccessCodeID).Rows(0)
            hfShowStock.Value = dr.ShowStock
            If dr.ShowStock = False Then
                grdItm.Columns(8).HeaderText = ""
            End If
        Catch ex As Exception
            hfShowStock.Value = False
        End Try

    End Sub

    Private Function LoadData() As Boolean
        Dim sType As String = String.Empty
        Dim sQty As String = String.Empty
        Dim bSuccess As Boolean = False
        Dim sScript As String

        Try
            sType = txtItmSelFrom.Value
            If Not Page.IsPostBack Then
                Select Case sType
                    Case "O"
                        'If (MyBase.CurrentUser.CanExtendQtyLI) Then
                        sQty = 100000000
                        'Else
                        'sQty = MyBase.CurrentOrder.MaxQtyPerLineItem
                        'End If
                        vMaxQty.Text = "Quantity cannot exceed " & sQty
                        vMaxQty.ValueToCompare = sQty
                        vMaxQty.ErrorMessage = "Quantity cannot exceed the limit set at " & sQty & " per line item."
                        vMaxQty.CssClass = "lblInactive"

                        If Not (MyBase.CurrentUser.CanExtendQtyLI) Then
                            'sScript = "if(parseInt(txtItmSelCnt.value) > 0) " & _
                            '"{" & _
                            '" if(parseInt(txtQtyOrd.value) > " & sQty & ") " & _
                            '" { return(alert('Quantity added cannot exceed the limit set at " & sQty & " per line item.')==0);} " & _
                            '" else {if (confirm('You are about to add selected items to the current order.  Do you wish to continue?')) {CheckOrderQuantities();} else {return false} }" & _
                            '"} " & _
                            '"else {alert('No selections have been made... Please select items requested to be added to cart.'); return false;}; " & _
                            '""


                            'If item being added to cart is in Backorder mode or if the quantity being requested exceeds current inventory level (“FUL” only items) the dialog box should read: 
                            '“Of the items you are about to add to the current order, the following are currently on Backorder or will be Backordered as a result of your request. Part Number ### piece in inventory 
                            'All items selected will be added to your current order. Do you wish to continue?"

                            sScript = "if(parseInt(txtItmSelCnt.value) > 0) " & _
                            "{" & _
                            "if (confirm('You are about to add selected items to the current order.  Do you wish to continue?')) {return true;} else {return false} }" & _
                            "else {alert('No selections have been made... Please select items requested to be added to cart.'); return false;}; " & _
                            ""

                            ibtnAddOrd.Attributes.Add("onclick", sScript)
                        ElseIf (MyBase.CurrentUser.CanExtendQtyLI) Then
                            'sScript = "if(parseInt(txtItmSelCnt.value) > 0) " & _
                            '"{" & _
                            '" if(parseInt(txtQtyOrd.value) > " & sQty & ") " & _
                            '" { return(alert('Quantity added cannot exceed the limit set at " & sQty & " per line item.')==0);} " & _
                            '" else {if (confirm('You are about to add selected items to the current order.  Do you wish to continue?')) {return true;} else {return false} }" & _
                            '"} " & _
                            '"else {alert('No selections have been made... Please select items requested to be added to cart.'); return false;}; " & _
                            '""

                            sScript = "if(parseInt(txtItmSelCnt.value) > 0) " & _
                           "{" & _
                           "if (confirm('You are about to add selected items to the current order.  Do you wish to continue?')) {return true;} else {return false} }" & _
                           "else {alert('No selections have been made... Please select items requested to be added to cart.'); return false;}; " & _
                           ""

                            ibtnAddOrd.Attributes.Add("onclick", sScript)
                        End If
                        'Order - End
                        '''''''''''''''

                    Case "K"
                        '''''''''''''''
                        'Kit - Start
                        vMaxQty.ValueToCompare = 32767
                        ibtnAddKit.Attributes.Add("onclick", "return confirm('You are about to add selected items to the current kit.  Do you wish to continue?'); ")
                        'Kit - End
                        '''''''''''''''

                    Case "I"
                        '''''''''''''''
                        'Item - Start
                        vMaxQty.ValueToCompare = 32767
                        'ibtnAddKit.Attributes.Add("onclick", "return confirm('You are about to add selected items to the current kit.  Do you wish to continue?'); ")
                        'Item - End
                        '''''''''''''''
                    Case Else
                        vMaxQty.ValueToCompare = 32767
                End Select

                ibtnK1Select.Attributes.Add("onclick", "SelectAllKey(document.getElementById('frmItmSel'), 'K1'); return false;")
                ibtnK2Select.Attributes.Add("onclick", "SelectAllKey(document.getElementById('frmItmSel'), 'K2'); return false;")
                ibtnK3Select.Attributes.Add("onclick", "SelectAllKey(document.getElementById('frmItmSel'), 'K3'); return false;")
                ibtnK4Select.Attributes.Add("onclick", "SelectAllKey(document.getElementById('frmItmSel'), 'K4'); return false;")
                ibtnGet.Attributes("onClick") = "javascript:LoadImage(0); return true;"

                bSuccess = LoadKeywords()
                SetKeySelections()
                PagerInit(Val(ScrollYPosItm.Value.ToString))
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

    Private Function btnAddOrdScript1(ByVal sQty As String) As String
        Dim sScript As New StringBuilder

        sScript.AppendLine("if(parseInt(txtItmSelCnt.value) > 0) ")
        sScript.AppendLine("{")
        sScript.AppendLine(String.Format("if (parseInt(txtQtyOrd.value) > {0}) ", sQty))
        sScript.AppendLine(String.Format("{ return(alert('Quantity added cannot exceed the limit set at {0} per line item.')==0);} ", sQty))
        sScript.AppendLine("else {")
        sScript.AppendLine("if (confirm('You are about to add selected items to the current order.  Do you wish to continue?'))")
        sScript.AppendLine("{")
        sScript.AppendLine("CheckOrderQuantities();")
        sScript.AppendLine("return true;")
        sScript.AppendLine("} ")
        sScript.AppendLine("else {")
        sScript.AppendLine(" alert('No selections have been made... Please select items requested to be added to cart.');")
        sScript.AppendLine("return false;};")

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
            lblK2.Text = String.Empty
            lblK3.Text = String.Empty
            lblK4.Text = String.Empty

            sSQL = BuildSQL_KeywordType()
            If (sSQL.Length > 0) Then
                MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
                Do While dr.Read()
                    iSeq = Val(dr("Seq").ToString & "")
                    sType = dr("KeywordType").ToString & ""
                    If (iSeq = 1) Then
                        lblK1.Text = sType
                    ElseIf (iSeq = 2) Then
                        lblK2.Text = sType
                    ElseIf (iSeq = 3) Then
                        lblK3.Text = sType
                    ElseIf (iSeq = 4) Then
                        lblK4.Text = sType
                    End If
                Loop
                MyBase.MyData.ReleaseReader2(cnn, cmd, dr)
                bSuccess = True
            End If

            bVisible = (Len(lblK1.Text) > 0)
            If bVisible Then
                pnlK1.Visible = bVisible
                'sSQL = BuildSQL_Keyword(1)
                bSuccess = LoadGrid(1, grdK1)
            End If

            bVisible = (Len(lblK2.Text) > 0)
            If bVisible Then
                pnlK2.Visible = bVisible
                'sSQL = BuildSQL_Keyword(2)
                bSuccess = LoadGrid(2, grdK2)
            End If

            bVisible = (Len(lblK3.Text) > 0)
            If bVisible Then
                pnlK3.Visible = bVisible
                'sSQL = BuildSQL_Keyword(3)
                bSuccess = LoadGrid(3, grdK3)
            End If

            bVisible = (Len(lblK4.Text) > 0)
            If bVisible Then
                pnlK4.Visible = bVisible
                'sSQL = BuildSQL_Keyword(4)
                bSuccess = LoadGrid(4, grdK4)
            End If

        Catch ex As Exception

        End Try

        Return bSuccess
    End Function

    Private Function LoadGridItm(ByVal sSQL As String, ByRef grd As System.Web.UI.WebControls.DataGrid) As Boolean
        Dim cnn As SqlClient.SqlConnection
        Dim cmd As SqlClient.SqlCommand
        Dim ds As DataSet
        Dim dRow As DataRow
        Dim bSuccess As Boolean = False


        If (sSQL.Length > 0) Then

            grd.SelectedIndex = -1
            MyBase.MyData.GetDataSetSql(cnn, cmd, ds, sSQL)
            RemoveExcludedItems(ds)

            'Dim qa As New dsAccessCodesTableAdapters.QueriesTableAdapter
            'If qa.CheckIfAllowedPurchaseDownloadableItems(CurrentUser.AccessCodeID) Then
            '    AddDownloadableItems(ds)
            'End If

            grd.DataSource = ds
            If (grd.PageCount < grd.CurrentPageIndex) Then grd.CurrentPageIndex = 0
            grd.DataBind()

            bSuccess = True
        End If

        Return (bSuccess)
    End Function

    Private Function LoadGrid(ByVal iSeq As String, ByRef grd As System.Web.UI.WebControls.DataGrid) As Boolean
        Dim taExclusions As New dsExclusionsTableAdapters.AccessCodeExclusionsByDescriptionTableAdapter
        Dim dtExclusions As New dsExclusions.AccessCodeExclusionsByDescriptionDataTable

        Dim taCatagories As New dsCategoriesTableAdapters.CatagoriesTableAdapter
        Dim dtCatagories As New dsCategories.CatagoriesDataTable
        Dim rCatagories As dsCategories.CatagoriesRow

        taCatagories.Fill(dtCatagories, iSeq, CurrentUser.CustomerID)
        taExclusions.ClearBeforeFill = True
        For Each rCatagories In dtCatagories.Rows
            taExclusions.Fill(dtExclusions, iSeq, CurrentUser.AccessCodeID, rCatagories("Id"), iSeq, rCatagories("Id"), CurrentUser.CustomerID)
            If (dtExclusions.Rows.Count > 0) Then
                rCatagories.Delete()
            End If
        Next

        grd.DataSource = dtCatagories
        grd.DataBind()

        Return True
    End Function

    Private Sub RemoveExcludedItems(ByVal ds As DataSet)
        Dim taExclusions As New dsExclusionsTableAdapters.ExclusionsByTypeTableAdapter
        Dim dtExclusions As New dsExclusions.ExclusionsByTypeDataTable
        Dim drExclusions() As dsExclusions.ExclusionsByTypeRow

        Dim taExclusionsKeyword As New dsExclusionsTableAdapters.GetExclusionsByKeywordTableAdapter
        Dim dtExclusionsKeyword As New dsExclusions.GetExclusionsByKeywordDataTable
        Dim drExclusionsKeyword() As dsExclusions.GetExclusionsByKeywordRow

        Dim dRow As DataRow
        Dim iCounter As Integer

        taExclusions.Fill(dtExclusions, 5, CurrentUser.AccessCodeID, 5, CurrentUser.CustomerID)
        taExclusionsKeyword.Fill(dtExclusionsKeyword, CurrentUser.CustomerID, CurrentUser.CustomerID, CurrentUser.AccessCodeID)

        For iCounter = 0 To ds.Tables(0).Rows.Count - 1
            dRow = ds.Tables(0).Rows(iCounter)
            Try
                drExclusions = dtExclusions.Select(String.Format("Description={0}", dRow("Id")))
                If (drExclusions.GetUpperBound(0) > -1) Then
                    dRow.Delete()
                Else
                    drExclusionsKeyword = dtExclusionsKeyword.Select(String.Format("Id={0}", dRow("Id")))
                    If (drExclusionsKeyword.GetUpperBound(0) > -1) Then
                        dRow.Delete()
                    End If
                End If
            Catch ex As Exception
            End Try
        Next
        ds.Tables(0).AcceptChanges()
    End Sub
    Private Sub AddDownloadableItems(ByRef ds As DataSet)
        Dim qa As New dsDownloadsTableAdapters.QueriesTableAdapter
        Dim dsNew As New DataSet
        Dim dtCopy As DataTable = ds.Tables(0).Copy()

        dsNew.Tables.Add(dtCopy)
        dsNew.Tables(0).Rows.Clear()
        Dim taDownload As New dsCustomerDocumentTableAdapters.Customer_Document_DownloadTableAdapter
        Dim drDownload As dsCustomerDocument.Customer_Document_DownloadRow
        Dim i As Integer = 0
        Dim obj As Object
        For Each dr As DataRow In ds.Tables(0).Rows
            dsNew.Tables(0).ImportRow(dr)
            'obj = -1
            Try
                obj = qa.CheckIfDocumenthasDownloadableItem(dr("ID"))
            Catch
            End Try

            If obj IsNot Nothing Then
                Try
                    drDownload = taDownload.GetDataByDocumentID(obj).Rows(0)
                    dsNew.Tables(0).ImportRow(drDownload)
                Catch
                End Try
            End If
            i += 1
        Next

        ds.Tables(0).Rows.Clear()
        ds = dsNew
    End Sub
    Private Function ClearGrid(ByRef grd As System.Web.UI.WebControls.DataGrid) As Boolean
        MyBase.PageMessage = MSG_Lev1
        lblItmPCnt.Text = String.Empty
        grd.DataSource = Nothing
        grd.DataBind()
    End Function

    Private Function LoadItem(Optional ByVal sK1IDs As String = "", Optional ByVal sK2IDs As String = "", Optional ByVal sK3IDs As String = "", Optional ByVal sK4IDs As String = "", _
                            Optional ByVal sText As String = "") As Boolean
        Dim bSuccess As Boolean = False
        Dim sSQL As String
        Dim ds As DataSet
        Dim lRcdCnt As Long = 0
        Dim qa As New dsAccessCodesTableAdapters.QueriesTableAdapter

        txtItmRow.Value = -1
        txtItmSelCnt.Value = 0

        sSQL = BuildSQL_Itm(sK1IDs, sK2IDs, sK3IDs, sK4IDs, sText)
        bSuccess = LoadGridItm(sSQL, grdItm)

        ds = grdItm.DataSource
        RemoveExcludedItems(ds)
        lRcdCnt = ds.Tables(0).Rows.Count
        If qa.CheckIfAllowedPurchaseDownloadableItems(CurrentUser.AccessCodeID) Then
            AddDownloadableItems(ds)
        End If


        If (lRcdCnt > 0) Then
            ShowItmPaging(True)
            lblItmPCnt.Text = "Page " & IIf(grdItm.CurrentPageIndex >= 0, grdItm.CurrentPageIndex + 1, 0) & " of " & _
                              IIf(lRcdCnt > 0, Math.Ceiling(lRcdCnt / grdItm.PageSize), 0).ToString()
            If (Val(txtItmSelCnt.Value) = 0) And (MyBase.CurrentOrder.ItmCart.Rows.Count = 0) Then MyBase.PageMessage = MSG_Lev2
        Else
            ShowItmPaging(False)
            MyBase.PageMessage = MSG_Lev0 '& "  " & MSG_Lev1
        End If

        MyBase.CurrentUser.CurrentK1Sels = GetSelectedGridIDs(grdK1, "K1")
        MyBase.CurrentUser.CurrentK2Sels = GetSelectedGridIDs(grdK2, "K2")
        MyBase.CurrentUser.CurrentK3Sels = GetSelectedGridIDs(grdK3, "K3")
        MyBase.CurrentUser.CurrentK4Sels = GetSelectedGridIDs(grdK4, "K4")
        SaveScrolls()
    End Function

    Private Sub ShowItmPaging(ByVal bShow As Boolean)
        lblItmPCnt.Text = String.Empty
        If (bShow = False) Then grdItm.CurrentPageIndex = 0
        If (grdItm.Visible <> bShow) Then grdItm.Visible = bShow
        If (lbtnItmFirst.Visible <> bShow) Then lbtnItmFirst.Visible = bShow
        If (lbtnItmPrev.Visible <> bShow) Then lbtnItmPrev.Visible = bShow
        If (lbtnItmNext.Visible <> bShow) Then lbtnItmNext.Visible = bShow
        If (lbtnItmLast.Visible <> bShow) Then lbtnItmLast.Visible = bShow
    End Sub

    Private Sub SetKeySelections()
        Try
            SetGridSelections(grdK1, "K1", MyBase.CurrentUser.CurrentK1Sels)
            SetGridSelections(grdK2, "K2", MyBase.CurrentUser.CurrentK2Sels)
            SetGridSelections(grdK3, "K3", MyBase.CurrentUser.CurrentK3Sels)
            SetGridSelections(grdK4, "K4", MyBase.CurrentUser.CurrentK4Sels)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub SetItmSelections()
        Try
            SetGridSelections(grdItm, "Itm", MyBase.CurrentUser.CurrentItmSels(grdItm.CurrentPageIndex))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ClearScrolls()
        Dim s As String
        Dim a() As String
        Try
            s = MyBase.CurrentUser.CurrentItmScrolls
            a = s.Split("|")
            ScrollXPosDoc.Value = 0
            ScrollYPosDoc.Value = 0
            ScrollXPosItm.Value = 0
            ScrollYPosItm.Value = 0
            ScrollXPosK1.Value = 0
            ScrollXPosK2.Value = 0
            ScrollXPosK3.Value = 0
            ScrollXPosK4.Value = 0

        Catch ex As Exception
        End Try
    End Sub

    Private Sub SetScrolls()
        Dim s As String
        Dim a() As String
        Try
            s = MyBase.CurrentUser.CurrentItmScrolls
            a = s.Split("|")
            ScrollXPosDoc.Value = Val(a(1))
            ScrollYPosDoc.Value = Val(a(2))
            ScrollXPosItm.Value = Val(a(3))
            ScrollYPosItm.Value = Val(a(4))
            ScrollXPosK1.Value = Val(a(5))
            ScrollXPosK2.Value = Val(a(6))
            ScrollXPosK3.Value = Val(a(7))
            ScrollXPosK4.Value = Val(a(8))

        Catch ex As Exception
        End Try
    End Sub

    Private Sub SaveScrolls()
        Dim s As String
        Try
            s = "|" & Val(ScrollXPosDoc.Value) & "|" & _
                      Val(ScrollYPosDoc.Value) & "|" & _
                      Val(ScrollXPosItm.Value) & "|" & _
                      Val(ScrollYPosItm.Value) & "|" & _
                      Val(ScrollXPosK1.Value) & "|" & _
                      Val(ScrollXPosK2.Value) & "|" & _
                      Val(ScrollXPosK3.Value) & "|" & _
                      Val(ScrollXPosK4.Value) & "|"

            MyBase.CurrentUser.CurrentItmScrolls = s
        Catch ex As Exception
        End Try
    End Sub

    Private Function SetGridSelections(ByRef grd As System.Web.UI.WebControls.DataGrid, ByVal sGrid As String, ByVal sSetIDs As String) As String
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
            If (sGrid = "Itm") Then sShpIDs = "!" & Replace(MyBase.CurrentOrder.ItmCartString, ",", "!") & "!"
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
                        txtItmSelCnt.Value = Val(txtItmSelCnt.Value) + 1
                        SelectRow_Itm(grd.Items(indx), sGrid, 2)
                    Else
                        SelectRow_Key(grd.Items(indx), sGrid, 1)
                    End If
                End If
            Next indx
            bSuccess = True

        Catch ex As Exception
            bSuccess = False
        End Try
        chk = Nothing

        Return bSuccess
    End Function

    Private Function IsInShoppingCart(ByVal sID As String) As Boolean
        Dim sShpIDs As String = String.Empty
        Dim iPosShp As Integer
        Dim bInCart As Boolean = False

        Try
            sShpIDs = "!" & Replace(MyBase.CurrentOrder.ItmCartString, ",", "!") & "!"
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

    Private Function BuildSQL_Itm(Optional ByVal sKeyword1IDs As String = "", Optional ByVal sKeyword2IDs As String = "", Optional ByVal sKeyword3IDs As String = "", Optional ByVal sKeyword4IDs As String = "", _
                                Optional ByVal sSearch As String = "", Optional ByVal sIDs As String = "") As String
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
        Dim qa As New dsAccessCodesTableAdapters.QueriesTableAdapter

        sSelect = "SELECT DISTINCT d.ID, d.[Status], d.ReferenceNo AS RefNo, d.[Description] AS RefName, " & _
                  "CASE d.[Status] WHEN " & MyBase.MyData.STATUS_BACK & " THEN 'True' ELSE 'False' End AS OnBackOrder, " & _
                  "CASE LEN(d.Prefix) WHEN 0 THEN 'ZZZZ' ELSE d.Prefix END as Prefix1, " & _
                  "OrderQuantityLimit, " & _
                  "ISNULL((Select (QtyOHLocP + QtyOHLocS + QtyOHLocW) from Customer_Document_Fill where DocumentID = d.ID), 999999) as CurrentStock, " & _
                  "d.PrintMethod "
        sFrom = "FROM Customer_Document d " & _
                "LEFT OUTER JOIN Customer_Document_DownloadableConnection ON d.ID = Customer_Document_DownloadableConnection.DocumentID_Downloadable " & _
                "INNER JOIN Customer_Document_Keyword dk ON d.ID = dk.DocumentID "

        If qa.CheckIfAllowedPurchaseDownloadableItems(CurrentUser.AccessCodeID) Then
            sWhere = " (d.CustomerID = " & MyBase.CurrentUser.CustomerID & " AND Customer_Document_DownloadableConnection.DocumentID_Main is null)"
        Else
            sWhere = " (d.CustomerID = " & MyBase.CurrentUser.CustomerID & " and d.PrintMethod<>3)"
        End If

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
            sFrom = "FROM Customer_Document d " & _
                "LEFT OUTER JOIN Customer_Document_DownloadableConnection ON d.ID = Customer_Document_DownloadableConnection.DocumentID_Downloadable " & _
                "INNER JOIN (" & sKeyFrom & ") dk ON d.ID = dk.DocumentID "
        End If

        If (sSearch.Length > 0) Then
            sWhere = sWhere & "AND ( (d.ReferenceNo LIKE " & MyBase.MyData.SQLString("%" & sSearch & "%") & ") OR (d.[Description] LIKE " & MyBase.MyData.SQLString("%" & sSearch & "%") & ")) "
        End If

        If (bShowActiveOnly) Then
            sWhere = sWhere & " AND (d.[Status] >" & MyData.STATUS_INAC & ") "
        End If

        If sIDs.Length > 0 Then
            sWhere &= " AND (d.ID in (" & sIDs & "))"
        End If

        If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

        sOrder = sOrder & " ORDER BY Prefix1, d.ReferenceNo "

        sSQL = sSelect + sFrom & sWhere + sOrder

        'Response.Write(sSQL)

        Return sSQL
    End Function

    'Private Function BuildSQL_Itm(Optional ByVal sKeyword1IDs As String = "", Optional ByVal sKeyword2IDs As String = "", Optional ByVal sKeyword3IDs As String = "", Optional ByVal sKeyword4IDs As String = "", _
    '                           Optional ByVal sSearch As String = "") As String
    '    Dim bShowActiveOnly As Boolean : bShowActiveOnly = True
    '    Dim sSQL As String = String.Empty
    '    Dim sSelect As String = String.Empty
    '    Dim sFrom As String = String.Empty
    '    Dim sKey1From As String = String.Empty
    '    Dim sKey2From As String = String.Empty
    '    Dim sKey3From As String = String.Empty
    '    Dim sKey4From As String = String.Empty
    '    Dim sKeyFrom As String = String.Empty
    '    Dim sWhere As String = String.Empty
    '    Dim sOrder As String = String.Empty
    '    Dim qa As New dsAccessCodesTableAdapters.QueriesTableAdapter

    '    sSelect = "SELECT DISTINCT d.ID, d.Status, d.ReferenceNo AS RefNo, d.Description AS RefName, " & _
    '              "iif(d.Status=" & MyBase.MyData.STATUS_BACK & ",True,False) AS OnBackOrder, " & _
    '              "iif(LEN(d.Prefix & '')=0,'ZZZZ',FORMAT(d.Prefix,'0000')), OrderQuantityLimit "
    '    sFrom = "FROM Customer_Document d INNER JOIN Customer_Document_Keyword dk ON d.ID = dk.DocumentID "

    '    If qa.CheckIfAllowedPurchaseDownloadableItems(CurrentUser.AccessCodeID) Then
    '        sWhere = " (d.CustomerID = " & MyBase.CurrentUser.CustomerID & ")"
    '    Else
    '        sWhere = " (d.CustomerID = " & MyBase.CurrentUser.CustomerID & " and d.PrintMethod<>3)"
    '    End If

    '    'By Keyword Tags
    '    If (Len(sKeyword1IDs) > 0) Or (Len(sKeyword2IDs) > 0) Or (Len(sKeyword3IDs) > 0) Or (Len(sKeyword4IDs) > 0) Then
    '        If (Len(sKeyword1IDs) > 0) Then
    '            sKey1From = "(SELECT DocumentID FROM Customer_Document_Keyword WHERE KeywordID IN (" & sKeyword1IDs & ")) K1 "
    '        Else
    '            sKey1From = "(SELECT DocumentID FROM Customer_Document_Keyword) K1 "
    '        End If
    '        If (Len(sKeyword2IDs) > 0) Then
    '            sKey2From = "(SELECT DocumentID FROM Customer_Document_Keyword WHERE KeywordID IN (" & sKeyword2IDs & ")) K2 "
    '        Else
    '            sKey2From = "(SELECT DocumentID FROM Customer_Document_Keyword) K2 "
    '        End If
    '        If (Len(sKeyword3IDs) > 0) Then
    '            sKey3From = "(SELECT DocumentID FROM Customer_Document_Keyword WHERE KeywordID IN (" & sKeyword3IDs & ")) K3 "
    '        Else
    '            sKey3From = "(SELECT DocumentID FROM Customer_Document_Keyword) K3 "
    '        End If
    '        If (Len(sKeyword4IDs) > 0) Then
    '            sKey4From = "(SELECT DocumentID FROM Customer_Document_Keyword WHERE KeywordID IN (" & sKeyword4IDs & ")) K4 "
    '        Else
    '            sKey4From = "(SELECT DocumentID FROM Customer_Document_Keyword) K4 "
    '        End If
    '        sKeyFrom = "SELECT DISTINCT K1.DocumentID FROM " & _
    '                    sKey1From & " INNER JOIN " & _
    '                    "(" & sKey2From & " INNER JOIN " & _
    '                    "(" & sKey3From & " INNER JOIN " & _
    '                    "" & sKey4From & _
    '                    " ON K3.DocumentID = K4.DocumentID " & _
    '                    ")ON K2.DocumentID = K3.DocumentID " & _
    '                    ")ON K1.DocumentID = K2.DocumentID " & _
    '                    ""
    '        sFrom = "FROM Customer_Document d INNER JOIN (" & sKeyFrom & ") dk ON d.ID = dk.DocumentID "
    '    End If

    '    If (sSearch.Length > 0) Then
    '        sWhere = sWhere & "AND ( (d.ReferenceNo LIKE " & MyBase.MyData.SQLString("%" & sSearch & "%") & ") OR (d.Description LIKE " & MyBase.MyData.SQLString("%" & sSearch & "%") & ")) "
    '    End If

    '    If (bShowActiveOnly) Then
    '        sWhere = sWhere & " AND (d.Status >" & MyData.STATUS_INAC & ") "
    '    End If
    '    If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

    '    sOrder = sOrder & "ORDER BY iif(LEN(d.Prefix & '')=0,'ZZZZ',FORMAT(d.Prefix,'0000')), d.ReferenceNo "

    '    sSQL = sSelect + sFrom & sWhere + sOrder

    '    Response.Write(sSQL)

    '    Return sSQL
    'End Function

    Protected Sub grdK1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdK1.ItemDataBound
        grdKey_ItemDataBound("K1", sender, e)
    End Sub

    Protected Sub grdK1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdK1.ItemCommand
    End Sub

    Protected Sub grdK2_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdK2.ItemDataBound
        grdKey_ItemDataBound("K2", sender, e)
    End Sub

    Protected Sub grdK2_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdK2.ItemCommand
    End Sub

    Protected Sub grdK3_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdK3.ItemDataBound
        grdKey_ItemDataBound("K3", sender, e)
    End Sub

    Protected Sub grdK3_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdK3.ItemCommand
    End Sub

    Protected Sub grdK4_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdK4.ItemDataBound
        grdKey_ItemDataBound("K4", sender, e)
    End Sub

    Protected Sub grdK4_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdK4.ItemCommand
    End Sub

    Protected Sub grdKey_ItemDataBound(ByVal sGrid As String, ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        Dim btn As WebControls.Button
        Dim chk As CheckBox

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
        Dim btnNo As WebControls.Button
        Dim btnName As WebControls.Button
        Dim btnStock As WebControls.Button
        Dim chk As WebControls.CheckBox
        Dim chkSelectDownload As WebControls.CheckBox
        Dim lblOrderQuantityLimit As Label
        Dim lblQDAvail As Label
        Dim sSym As String = String.Empty
        Dim sID As String = String.Empty
        Dim sScript As String
        Dim ltDnldText As Literal
        Dim obj As Object
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter
        Dim qaD As New dsDownloadsTableAdapters.QueriesTableAdapter

        Dim hf As HiddenField
        Dim po As New paraOrder


        If (e.Item.ItemType = ListItemType.Item Or _
            e.Item.ItemType = ListItemType.AlternatingItem Or _
            e.Item.ItemType = ListItemType.SelectedItem Or _
            e.Item.ItemType = ListItemType.EditItem) Then

            sID = e.Item.Cells(COL_ID).Text
            iStatus = Convert.ToInt16(e.Item.Cells(COL_ISTAT).Text.ToString)
            chk = e.Item.Cells(COL_CHK).FindControl("chkSelect")
            chkSelectDownload = e.Item.FindControl("chkSelectDownload")
            btnNo = e.Item.Cells(COL_INO).FindControl("btnItmNo")
            btnName = e.Item.Cells(COL_INO).FindControl("btnItmName")
            btnStock = e.Item.Cells(COL_INO).FindControl("btnStock")
            lblOrderQuantityLimit = e.Item.FindControl("lblOrderQuantityLimit")
            ltDnldText = e.Item.FindControl("ltDownloadables")
            hf = e.Item.FindControl("hfDownloadID")
            Dim intPrintMethod As Integer = qa.getDocumentPrintMethod(sID)
            obj = qaD.CheckIfDocumenthasDownloadableItem(sID)
            lblQDAvail = e.Item.FindControl("lblQDAvail")

            If bCanOrderDownloadItems AndAlso obj IsNot Nothing Then
                ltDnldText.Text = GetDownloadDetails(obj.ToString)
                If ltDnldText.Text = "" Then
                    lblQDAvail.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                    chkSelectDownload.Visible = False
                Else
                    lblQDAvail.Text = "&nbsp;Available&nbsp;"
                    chkSelectDownload.Visible = True
                End If

                chkSelectDownload.Attributes.Add("OnClick", String.Format("javascript:SelectRowItmDnld(document.getElementById('{0}'));", chkSelectDownload.ClientID))
                e.Item.Cells(COL_INO + 1).HorizontalAlign = HorizontalAlign.Left
                hf.Value = obj.ToString
                e.Item.Cells(COL_INO).VerticalAlign = VerticalAlign.Top
                'check if item in cart:
                If po.CheckIfItemInCart(MyBase.CurrentOrder.ItmCart, obj.ToString) Then
                    chkSelectDownload.Checked = True
                    chkSelectDownload.Enabled = False
                End If
            Else
                chkSelectDownload.Visible = False
                chkSelectDownload.CssClass = "ui-helper-hidden-accessible"
            End If

            'if customer cant see stock hide column




            If intPrintMethod = 3 Then
                'e.Item.Cells(1).VerticalAlign = VerticalAlign.Top
                'e.Item.Cells(COL_INO).VerticalAlign = VerticalAlign.Top

                'e.Item.Cells(COL_INO).Text &= "<div class='accordion'><h3>Downloadable Items</h3><div>" & _
                '"<p>Mauris mauris ante, blandit et, ultrices a, suscipit eget, quam. Integer ut neque. Vivamus nisi metus, molestie vel, gravida in, condimentum sit amet, nunc. Nam a nibh. Donec suscipit eros. Nam mi. Proin viverra leo ut odio. Curabitur malesuada. Vestibulum a velit eu ante scelerisque vulputate.</p>" & _
                '"</div>" & _
                '"</div>"
                chk.Visible = False
                chk.CssClass = "ui-helper-hidden-accessible"
            End If
            'download:
            'chk.Enabled = False
            'chk.Style.Item("display") = "none"
            'chk.Attributes("onClick") = "javascript:HighlighRowItm(document.getElementById('" & chk.ClientID & "')); return false;"
            'Else

            'If (Me.txtQtyOrd.Visible) Then
            If (MyBase.CurrentUser.CanExtendQtyLI) Then
                chk.Attributes.Add("OnClick", String.Format("javascript:SelectRowItm(document.getElementById('{0}'));", chk.ClientID))
            Else
                sScript = txtOrdQtyJavascript(lblOrderQuantityLimit.Text, Me.txtQtyOrd.ClientID, chk.ClientID, btnNo.Text, btnName.Text.Trim)
                chk.Attributes.Add("OnClick", sScript)
            End If
            'End If
            'End If

            btnNo.Attributes("onClick") = "javascript:HighlighRowItm(document.getElementById('" & btnNo.ClientID & "')); return false;"
            btnName.Attributes("onClick") = "javascript:HighlighRowItm(document.getElementById('" & btnName.ClientID & "')); return false;"
            btnStock.Attributes("onClick") = "javascript:HighlighRowItm(document.getElementById('" & btnName.ClientID & "')); return false;"

            If (iStatus = MyData.STATUS_BACK) Then
                btnNo.Font.Size = 8
                e.Item.Cells(COL_ISYM).Text = " * " + btnNo.Text + "&nbsp;&nbsp;"
                e.Item.ForeColor = Color.FromArgb(189, 83, 4)   'MyData.COLOR_BACK
                btnNo.ForeColor = Color.FromArgb(189, 83, 4)    'MyData.COLOR_BACK
                btnName.ForeColor = Color.FromArgb(189, 83, 4)  'MyData.COLOR_BACK
                btnStock.ForeColor = Color.FromArgb(189, 83, 4)

            ElseIf (iStatus = MyData.STATUS_INAC) Then
                btnNo.Font.Size = 8
                e.Item.Cells(COL_ISYM).Text = " - " + btnNo.Text + "&nbsp;&nbsp;"
                e.Item.ForeColor = Color.Red                    'MyData.COLOR_INAC
                btnNo.ForeColor = Color.Red                     'MyData.COLOR_INAC
                btnName.ForeColor = Color.Red                   'MyData.COLOR_INAC
                btnStock.ForeColor = Color.Red
            Else
                btnNo.Font.Size = 8
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


    Private Function txtOrdQtyJavascript(ByVal lblOrderQuantityLimitText As String, ByVal txtOrderQuantityClientId As String, ByVal chkSelectClientId As String, ByVal btnNoText As String, ByVal btnNameText As String)
        Dim sScript As New StringBuilder

        btnNameText = btnNameText.Replace(vbCrLf, "")

        Dim strName As String = btnNameText.ToString

        strName = strName.Replace("""", "\""")
        strName = strName.Replace("'", "\'")

        sScript.AppendLine(String.Format("if (CheckOrderQuantityLimit({0}, document.getElementById('{1}').value, document.getElementById('{2}'), '{3} - {4}'))", lblOrderQuantityLimitText, txtOrderQuantityClientId, chkSelectClientId, btnNoText, strName))
        sScript.AppendLine("{")
        sScript.AppendLine(String.Format("javascript:SelectRowItm(document.getElementById('{0}'))", chkSelectClientId))
        sScript.AppendLine("}")


        Return sScript.ToString
    End Function



    Sub PagerInit(ByVal iPage As Integer)
        Try
            grdItm.CurrentPageIndex = iPage
            SearchItems_ByKeyword()
        Catch ex As Exception
        End Try
    End Sub

    Sub PagerMove(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnItmNext.Click
        Dim arg As String = sender.CommandArgument

        Try
            ScrollXPosItm.Value = 0

            Select Case arg
                Case "next" 'The next Button was Clicked
                    If (grdItm.CurrentPageIndex < (grdItm.PageCount - 1)) Then
                        grdItm.CurrentPageIndex += 1
                    End If
                Case "prev" 'The prev button was clicked
                    If (grdItm.CurrentPageIndex > 0) Then
                        grdItm.CurrentPageIndex -= 1
                    End If
                Case "last" 'The Last Page button was clicked
                    grdItm.CurrentPageIndex = (grdItm.PageCount - 1)
                Case Else 'The First Page button was clicked
                    grdItm.CurrentPageIndex = Convert.ToInt32(arg)
            End Select

            ScrollYPosItm.Value = grdItm.CurrentPageIndex

            'Determine the Current Search Type
            If (inpPageType.Value = "K") Then
                SearchItems_ByKeyword()
            Else
                SearchItems_ByText()
            End If
            SetItmSelections()

        Catch ex As Exception
            Session("PMsg") = "Unable to Get Page Data..."
        End Try
    End Sub

    Sub Prev_Buttons()
        Dim PrevSet As String
        Dim ds As DataSet
        Dim lRcdCnt As Long

        ds = grdItm.DataSource
        lRcdCnt = ds.Tables(0).Rows.Count

        If grdItm.CurrentPageIndex + 1 <> 1 And lRcdCnt <> -1 Then
            PrevSet = grdItm.PageSize
            lbtnItmPrev.Text = ("< Prev " & PrevSet)

            If grdItm.CurrentPageIndex + 1 = grdItm.PageCount Then
                lbtnItmFirst.Text = ("<< 1st Page")
            End If
        End If
    End Sub

    Sub Next_Buttons()
        Dim NextSet As String
        Dim ds As DataSet
        Dim lRcdCnt As Long

        ds = grdItm.DataSource
        lRcdCnt = ds.Tables(0).Rows.Count

        If grdItm.CurrentPageIndex + 1 < grdItm.PageCount Then
            NextSet = grdItm.PageSize
            lbtnItmNext.Text = ("Next " & NextSet & " >")
        End If
        If grdItm.CurrentPageIndex + 1 = grdItm.PageCount - 1 Then
            Dim EndCount As Integer
            EndCount = lRcdCnt - (grdItm.PageSize * (grdItm.CurrentPageIndex + 1))
            lbtnItmNext.Text = ("Next " & EndCount & " >")
        End If
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

    Private Sub SelectRow_Itm(ByRef e As DataGridItem, ByVal sGrid As String, ByVal bSelect As Byte)
        'Green - #3dbc45 '#008400
        'Light Green - #92ee8e
        'Yellow - #FFFF80
        Dim btn As WebControls.Button
        Dim chk As WebControls.CheckBox
        Dim chkD As WebControls.CheckBox

        Dim sID As String = e.Cells(COL_ID).Text

        'Dim qa As New dsDownloadsTableAdapters.QueriesTableAdapter
        Dim ta As New dsCustomerDocumentTableAdapters.Customer_DocumentTableAdapter
        Dim dr As dsCustomerDocument.Customer_DocumentRow = ta.GetDataByDocumentID(sID).Rows(0)
        Dim iPrintMethod = dr.PrintMethod

        If iPrintMethod = 3 Then
            e.Cells(COL_INO + 1).Text = GetDownloadDetails(sID)
            'Else
            '    With e.Cells(COL_INO)
            '        If .FindControl("btnItmNo") Is Nothing Then
            '            Dim btnRefNo As New Button
            '            btnRefNo.Text = dr.ReferenceNo
            '            btnRefNo.CssClass = "btnItmNoList"
            '            btnRefNo.ID = "btnItmNo"
            '            .Controls.Add(btnRefNo)
            '            .HorizontalAlign = HorizontalAlign.Right
            '            .ID = "skip"
            '        End If

            '    End With

        End If

        Dim i As Integer = 0
        'For Each cl As TableCell In e.Cells
        '    cl.BackColor = Color.White
        '    For Each ctl As Control In cl.Controls
        '        If TypeOf ctl Is WebControl Then
        '            CType(ctl, WebControl).BackColor = Color.White
        '        End If
        '    Next
        'Next


        'Try
        If (bSelect = 1) Then 'Checked
            e.BackColor = Color.FromArgb(255, 255, 128) 'Light Yellow
            chk = e.Cells(COL_CHK).FindControl("chkSelect")
            If chk.Checked = False Then chk.Checked = True

            chkD = e.Cells(COL_CHK + 1).FindControl("chkSelectDownload")
            'If chkD.Checked = False Then chkD.Checked = True

        ElseIf (bSelect = 2) Then 'Checked but secondary (Item Green)
            e.BackColor = Color.FromArgb(61, 188, 69) 'Green
            chk = e.Cells(COL_CHK).FindControl("chkSelect")
            If chk.Checked = False Then chk.Checked = True

            chkD = e.Cells(COL_CHK + 1).FindControl("chkSelectDownload")
            'If chkD.Checked = False Then chkD.Checked = True

        ElseIf (bSelect = 3) Then 'Already in the Cart (Item Light Green)


            e.Style("background-color") = "#92EE8E"



            chk = e.Cells(COL_CHK).FindControl("chkSelect")
            If chk.Checked = True Then chk.Checked = False
            If chk.Enabled = True Then chk.Enabled = False

            'chkD = e.Cells(COL_CHK + 1).FindControl("chkSelectDownload")
            'If chkD.Enabled = True Then chkD.Enabled = False
            i = 0

            If iPrintMethod = 3 Then
                For Each cl As TableCell In e.Cells
                    cl.BackColor = Color.FromArgb(146, 238, 142)
                    e.BackColor = Color.FromArgb(146, 238, 142) 'Light Green
                    If cl.ID <> "skip" Then
                        For Each ctl As Control In cl.Controls
                            If TypeOf ctl Is WebControl Then
                                'CType(ctl, WebControl).BackColor = Color.FromArgb(146, 238, 142)
                                CType(ctl, WebControl).Style("background-color") = "rgb(146, 238, 142)"
                            End If

                            If ctl.ID = "btnItmNo" And iPrintMethod = 3 Then
                                cl.Controls.Remove(ctl)
                            End If
                        Next
                    End If


                Next
            Else
                CType(e.FindControl("btnItmName"), WebControl).Style("background-color") = "rgb(146, 238, 142)"
            End If



        ElseIf (bSelect = 0) Then ' UnChecked
            e.BackColor = Color.White
        End If
        If btn IsNot Nothing Then
            btn = e.Cells(COL_INO).FindControl("btnItmNo")
            btn.BackColor = e.BackColor
        End If

        btn = e.Cells(COL_IDES).FindControl("btnItmName")

        If btn IsNot Nothing Then
            btn.BackColor = e.BackColor
        Else
            e.Cells(COL_IDES).BackColor = e.BackColor

        End If


        'Catch ex As Exception

        'End Try
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


        'download items:
        iMax = grd.Items.Count - 1
        Dim chk As CheckBox
        Dim hf As HiddenField


        For indx = 0 To iMax
            Try
                If grdItm.Items(indx).FindControl("chkSelectDownload") IsNot Nothing Then

                    chk = grdItm.Items(indx).FindControl("chkSelectDownload")
                    hf = grdItm.Items(indx).FindControl("hfDownloadID")

                    If IsInShoppingCart(hf.Value) Then
                        chk.Enabled = False
                    End If
                End If
            Catch ex As Exception

            End Try


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
        Dim chkD As WebControls.CheckBox

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


    Private Function GetSelectedGridIDs_ChkDownload(ByRef grd As System.Web.UI.WebControls.DataGrid, ByVal sGrid As String) As String
        'Note:  ID is Column 0
        'Note:  Check is Column 1
        'Note:  Buttons are Column 2
        Dim indx As Integer
        Dim iMax As Integer
        Dim sIDs As String = String.Empty
        Dim chk As WebControls.CheckBox
        Dim chkD As WebControls.CheckBox

        Try
            iMax = grd.Items.Count - 1
            For indx = 0 To iMax

                chkD = grd.Items(indx).Cells(COL_CHK).FindControl("chkSelectDownload")
                If (chkD.Checked = True) Then
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

        sK1IDs = GetSelectedGridIDs(grdK1, "K1")
        sK2IDs = GetSelectedGridIDs(grdK2, "K2")
        sK3IDs = GetSelectedGridIDs(grdK3, "K3")
        sK4IDs = GetSelectedGridIDs(grdK4, "K4")

        If (Len(sK1IDs) > 0) Or (Len(sK2IDs) > 0) Or (Len(sK3IDs) > 0) Or (Len(sK4IDs) > 0) Then
            LoadItem(sK1IDs, sK2IDs, sK3IDs, sK4IDs, "")
        Else
            ClearGrid(grdItm)
        End If
    End Sub

    Private Sub SearchItems_ByText()
        Dim sText As String

        sText = txtSearch.Text.ToString
        LoadItem("", "", "", "", sText)
    End Sub

    Private Sub ibtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSearch.Click
        Dim grd As WebControls.DataGrid

        inpPageType.Value = "T"

        grd = Me.FindControl("grdK1")
        If Not (grd Is Nothing) Then GridSelectUpdate(grdK1, "K1", 0)
        grd = Me.FindControl("grdK2")
        If Not (grd Is Nothing) Then GridSelectUpdate(grdK2, "K2", 0)
        grd = Me.FindControl("grdK3")
        If Not (grd Is Nothing) Then GridSelectUpdate(grdK3, "K3", 0)
        grd = Me.FindControl("grdK4")
        If Not (grd Is Nothing) Then GridSelectUpdate(grdK4, "K4", 0)

        SearchItems_ByText()
    End Sub


    Private Function ShowControls() As Boolean
        Dim bShowEdit As Boolean = False
        Dim bLOwn As Boolean = False
        Dim bEdit As Boolean = False

        Select Case txtItmSelFrom.Value
            Case "I"
                If (pnlBtnItm.Visible = False) Then pnlBtnItm.Visible = True
                If (pnlBtnKit.Visible = True) Then pnlBtnKit.Visible = False
                If (pnlBtnOrd.Visible = True) Then pnlBtnOrd.Visible = False
                ibtnEditItm.Visible = (MyBase.CurrentUser.CanRestockPlan = True)

            Case "K"
                If (pnlBtnItm.Visible = True) Then pnlBtnItm.Visible = False
                If (pnlBtnKit.Visible = False) Then pnlBtnKit.Visible = True
                If (pnlBtnOrd.Visible = True) Then pnlBtnOrd.Visible = False

            Case "O"
                If (pnlBtnItm.Visible = True) Then pnlBtnItm.Visible = False
                If (pnlBtnKit.Visible = True) Then pnlBtnKit.Visible = False
                If (pnlBtnOrd.Visible = False) Then pnlBtnOrd.Visible = True
                ibtnAddKit.Visible = (MyBase.CurrentUser.CanViewKits = True)
        End Select

        Return True
    End Function

    Private Sub ibtnGet_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnGet.Click
        inpPageType.Value = "K"
        MyBase.CurrentUser.ItmSelsInit()
        ShowItmPaging(False)
        SearchItems_ByKeyword()
        'INJECT JS:
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myKey", "accord();", True)
    End Sub

    Private Sub ibtnRestart_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRestart.Click
        ClearScrolls()
        GridSelectUpdate(grdK1, "K1", 0, False)
        GridSelectUpdate(grdK2, "K2", 0, False)
        GridSelectUpdate(grdK3, "K3", 0, False)
        GridSelectUpdate(grdK4, "K4", 0, False)
        ClearGrid(grdItm)
        ShowItmPaging(False)
        SearchItems_ByKeyword()
        If (Val(txtItmSelCnt.Value) = 0) And (MyBase.CurrentOrder.ItmCart.Rows.Count = 0) Then MyBase.PageMessage = MSG_Lev2
    End Sub

    Private Function ValidQuantity_Ord() As Boolean
        Dim bValid As Boolean : bValid = False

        bValid = True
        '((Convert.ToInt32(Val(txtQtyOrd.Text)) <= MyBase.CurrentOrder.MaxQtyPerLineItem)) Or _
        '             ((Convert.ToInt32(Val(txtQtyOrd.Text)) > MyBase.CurrentOrder.MaxQtyPerLineItem) And MyBase.CurrentUser.CanExtendQtyLI)
        If Not bValid Then
            MyBase.PageMessage = "Quantity to add exceeds maximum limit set at " & MyBase.CurrentOrder.MaxQtyPerLineItem
        End If

        Return bValid
    End Function

    Private Function SaveSelections_Ord() As Boolean
        Dim iCnt As Integer : iCnt = 0
        Dim indx As Integer
        Dim iMax As Integer
        Dim lQty As Long
        Dim lID As Long
        Dim ipos As Integer
        Dim sIDs As String = String.Empty
        Dim sName As String = String.Empty
        Dim sDesc As String = String.Empty
        Dim qa As New dsInventoryTableAdapters.QueriesTableAdapter

        Try
            MyBase.CurrentUser.CurrentItmSels(grdItm.CurrentPageIndex) = GetSelectedGridIDs_Chk(grdItm, "Itm")

            iMax = MyBase.CurrentUser.CurrentItmSelPageCnt
            For indx = 0 To iMax
                sIDs = MyBase.CurrentUser.CurrentItmSels(indx)
                lID = GetNextID(sIDs)
                Do While (lID > 0)
                    iCnt = iCnt + 1
                    lQty = txtQtyOrd.Text.ToString
                    MyBase.MyData.GetItmDetail(MyBase.CurrentUser.CustomerID, lID, sName, sDesc)

                    Dim sColor As String = ""
                    Select Case qa.GetDocumentStatusIDByDocumentID(lID)
                        Case 0
                            sColor = MyData.COLOR_INACTIVE
                        Case 1
                            sColor = ""
                        Case 2
                            sColor = MyData.COLOR_BACK
                    End Select

                    MyBase.CurrentOrder.ItmSave(lQty, lID, sName, sDesc, sColor)
                    lID = GetNextID(sIDs)
                Loop
            Next

            'add downloadable items:
            iCnt += AddDownloadableItemsToCart()

            MyBase.CurrentUser.ItmSelsInit()
            MyBase.SessionStore("ItmSel?C=O")
            txtItmsSelected.Value = ""

        Catch ex As Exception
            iCnt = 0
        End Try

        Return (iCnt > 0)
    End Function

    Function AddDownloadableItemsToCart() As Integer

        Dim indx As Integer
        Dim iMax As Integer
        Dim lID As Long
        Dim ipos As Integer
        Dim sIDs As String = String.Empty
        Dim sName As String = String.Empty
        Dim sDesc As String = String.Empty
        Dim chk As WebControls.CheckBox
        Dim iRet As Integer = 0

        Dim hf As HiddenField

        iMax = grdItm.Items.Count - 1
        For indx = 0 To iMax

            chk = grdItm.Items(indx).FindControl("chkSelectDownload")
            hf = grdItm.Items(indx).FindControl("hfDownloadID")

            If chk.Checked Then
                lID = hf.Value
                'Response.Write(chk.ID & " " & hf.Value & "<BR>")
                MyBase.MyData.GetItmDetail(MyBase.CurrentUser.CustomerID, lID, sName, sDesc)
                MyBase.CurrentOrder.ItmSave(1, lID, sName, sDesc, "")
                iRet += 1

            End If




        Next indx

        Return iRet
    End Function


    Private Function GetNextID(ByRef sIDs As String) As Long
        Dim lID As Long = 0
        Dim ipos As Integer

        Try
            If (Len(sIDs) > 0) Then
                ipos = InStr(sIDs, ",")
                If (ipos > 0) Then
                    lID = Val(Mid(sIDs, 1, ipos))
                    sIDs = Mid(sIDs, ipos + 1)
                Else
                    lID = Val(sIDs)
                    sIDs = String.Empty
                End If
            End If
        Catch ex As Exception

        End Try

        Return lID
    End Function

    Private Overloads Sub ibtnAddOrd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnAddOrd.Click
        If ValidQuantity_Ord() Then
            If SaveSelections_Ord() Then
                MyBase.PageMessage = "Items Added - " & MyBase.CurrentOrder.ItmCart.Rows.Count & " items currently in cart."
                'Remove all Item Selections
                GridSelectUpdate(grdItm, "Itm", 0, False)
            End If
        End If
    End Sub

    Private Sub ibtnKitOrd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnKitOrd.Click
        Dim sDestin As String = paraPageBase.PAG_OrdKitSelect.ToString()
        MyBase.PageDirect(sDestin, 0, 0)
    End Sub

    Private Sub ibtnCartOrd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCartOrd.Click
        Dim sDestin As String = paraPageBase.PAG_OrdCart.ToString()
        MyBase.PageDirect(sDestin, 0, 0)
    End Sub

    Private Function CurrentItemID() As Long
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
                    sID = Val(.Cells(COL_ID).Text.ToString())
                    bDone = True
                End If
            End With
        Next

        chk = Nothing
        Return Val(sID)
    End Function

    Private Overloads Sub ibtnBackItm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnBackItm.Click
        Response.Redirect(MyBase.CurrentUser.PreviousPage, False)
    End Sub

    Private Overloads Sub ibtnEditItm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnEditItm.Click
        If (Val(CurrentItemID) > 0) Then
            Response.Redirect("./AdmItmE.aspx" & _
                              "?ED=" & CurrentItemID() _
                              , False)
        End If
    End Sub

    Private Function SaveSelections_Kit() As Boolean
        Dim iCnt As Integer = 0
        Dim indx As Integer
        Dim iMax As Integer
        Dim btn As WebControls.Button
        Dim chk As WebControls.CheckBox
        Dim lQty As Long
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
                        lQty = txtQtyKit.Text.ToString
                        lID = Val(.Cells(COL_ID).Text.ToString())
                        btn = .Cells(COL_INO).FindControl("btnItmNo")
                        sName = btn.Text.ToString()
                        btn = .Cells(COL_IDES).FindControl("btnItmName")
                        sDesc = btn.Text.ToString()

                        MyBase.CurrentKit.ItmSave(lQty, lID, sName, sDesc, 0, sColor)
                    End If
                End With
            Next

        Catch ex As Exception
            iCnt = 0
        End Try
        chk = Nothing
        btn = Nothing

        Return (iCnt > 0)
    End Function

    Private Overloads Sub ibtnBackKit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnBackKit.Click
        Response.Redirect(MyBase.CurrentUser.PreviousPage, False)
    End Sub

    Private Overloads Sub ibtnAddKit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnAddKit.Click
        Dim sDestin As String = paraPageBase.PAG_AdmKitEdit.ToString()

        If (SaveSelections_Kit()) Then
            Response.Redirect("./AdmKitE.aspx?ED=0&Base=0", False)
        End If
    End Sub
    ''''''''''''''''''''''''''''''

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Function getShowStock(intItemID As Integer) As String
        Dim strTmp As String = ""
        If hfShowStock.Value Then
            Dim pd As New paraData
            strTmp = pd.getShowStock(intItemID, True)
        End If
        Return strTmp
    End Function

    Function GetDownloadDetails(intDocumentID As Integer) As String
        Dim ta As New dsCustomerDocumentTableAdapters.Customer_Document_Downloadable1TableAdapter
        Dim dt As New dsCustomerDocument.Customer_Document_Downloadable1DataTable
        Dim itemsExist As New Boolean
        itemsExist = False
        ta.FillByAccessCodeDoc(dt, intDocumentID, CurrentUser.AccessCodeID)
        ' style=""display:none""
        Dim sRet As String = "<div class=""accordion"" id=""accordion" & intDocumentID & """><div><table class=""downloadinfo""><tr><td><h3 class='accordionheader'>Downloadable Formats for this item</h3></td></tr>"
        For Each dr As dsCustomerDocument.Customer_Document_Downloadable1Row In dt.Rows
            Dim FormatName As String = dr.FormatName
            If qoEnabled Then
                Dim sName As String = FormatName
                FormatName = "<a href='GetFile.aspx?docID=" & dr.DocumentID & "&f=" & dr.FormatID & "'>" & sName & "</a>"
            End If
            sRet = sRet & "<tr><td class='tdformatname'>" & FormatName & "</td><td align=center><img src=""" & ResolveUrl("~/icons/") & dr.Icon & """ /></td><td><span class=""lblDSmall"">(" & GetFileSize(dr.FileName, intDocumentID) & ")</span></td></tr>"
            itemsExist = True
        Next

        If (Not qoEnabled) Then
            Dim sPath As String = "/" & ConfigurationManager.AppSettings("SiteLocation") & "/PDFS/" & intDocumentID & "/preview/" & intDocumentID & ".pdf"
            Dim sVidPath As String = "/" & ConfigurationManager.AppSettings("SiteLocation") & "/PDFS/" & intDocumentID & "/preview/" & intDocumentID & ".flv"
            If File.Exists(Server.MapPath(sPath)) Then
                If CheckIfShowGoogleDocs() Then
                    Dim pdfurl As String = "http://docs.google.com/viewer?url=" & _
                       Server.UrlEncode(ConfigurationSettings.AppSettings("HomeAddress") & "/" & ConfigurationSettings.AppSettings("SiteLocation") & "/pdfs/" & intDocumentID & "/preview/" & intDocumentID & ".pdf")
                    sRet += "<tr><td colspan=3 align=center><a href=""#"" data-href=""" & pdfurl & """  class=""newWindow"" ><img src='./images/view.gif' border='0'> preview this document</a></td></tr>"
                Else
                    sRet += "<tr><td colspan=3 align=center><a href=""#"" data-href=""pdfpreview.aspx?id=" & intDocumentID & """  class=""newWindow"" ><img src='./images/view.gif' border='0'> preview this document</a></td></tr>"

                End If

            ElseIf File.Exists(Server.MapPath(sVidPath)) Then
                sRet += "<tr><td colspan=3 align=center><a href='" & ConfigurationManager.AppSettings("HomeAddress") & "/" & ConfigurationManager.AppSettings("SiteLocation") & "/videoHandler.aspx?itemID=" & intDocumentID & "' onclick=""window.open('" & ConfigurationManager.AppSettings("HomeAddress") & "/" & ConfigurationManager.AppSettings("SiteLocation") & "/videoHandler.aspx?itemID=" & intDocumentID & "', 'newwindow', 'width=550, height=400, '); return false;"" ><img src='./images/view.gif' border='0'> Preview this document</a></td></tr>"
            End If

        End If
        sRet = sRet & "</table></div></div>"

        If Not itemsExist Then
            sRet = ""
        End If
        Return sRet
    End Function

    Function CheckIfShowGoogleDocs() As Boolean

        Dim strBrowser As String = Request.Browser.Browser
        Dim intVersion As Integer = Request.Browser.MajorVersion

        If strBrowser.ToString.ToUpper() = "IE" And intVersion <= 9 Then
            Return True
        Else
            Return False
        End If

    End Function

    Function GetFileSize(sFile As String, mlid As Integer) As String
        Dim sSize As String = String.Empty
        Dim strPath As String = ConfigurationManager.AppSettings("dampath") & "\" & mlid & "\"
        Dim LogLib As New Log
        LogLib.FileExists(strPath & sFile, sSize)
        Return sSize
    End Function

End Class

