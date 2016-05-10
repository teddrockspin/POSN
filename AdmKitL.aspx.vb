''''''''''''''''''''
'mw - 02-04-2009
'mw - 04-25-2007
'mw - 01-20-2007
''''''''''''''''''''


Partial Class AdmKitL
    Inherits paraPageBase

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
        LoadData()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PageTitle = "Kit List"
        'MyBase.Page_Load(sender, e)
        MyBase.PageMessage = "Click on Detail to view/edit definition of existing kit."

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        ElseIf Not ((MyBase.CurrentUser.CanViewKits = True) Or _
                    (MyBase.CurrentUser.CanEditKits = True)) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri
        End If 'End PostBack
    End Sub


    Private Function LoadData()
        LoadGrd()
    End Function

    Private Function LoadGrd() As Boolean
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim dr As OleDb.OleDbDataReader
        Dim bSuccess As Boolean = False
        Dim sSQL As String

        sSQL = BuildSQL()
        If (sSQL.Length > 0) Then
            MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)


            '--
            Dim dsUsage As New DataSet("Usage")
            Dim daUsage As New OleDb.OleDbDataAdapter(sSQL, cnn)
            daUsage.Fill(dsUsage, "Usage")
            '--

            MyBase.MyData.ReleaseReader2(cnn, cmd, dr)

            RemoveExcludedCategories(dsUsage)
            RemoveExcludedItems(dsUsage)

            grd.DataSource = dsUsage
            grd.DataBind()
            daUsage.Dispose()
            bSuccess = True
        End If

        Return (bSuccess)
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
    'Private Function BuildSQL() As String
    '  Dim sSQL As String = String.Empty
    '  Dim sSelect As String = String.Empty
    '  Dim sWhere As String = String.Empty
    '  Dim sOrder As String = String.Empty

    '  sSelect = "SELECT Customer_Kit.ID, Customer_Kit.ReferenceNo AS RefNo, Customer_Kit.Description AS RefName, " & _
    '            "IIf(qryKitDocSum.InActiveDocCnt is NULL,0,qryKitDocSum.InActiveDocCnt) AS InActiveDocCnt, " & _
    '            "IIf(qryKitDocSum.BackOrderDocCnt is NULL,0,qryKitDocSum.BackOrderDocCnt) AS BackOrderDocCnt " & _
    '            ", Customer_Kit.Status, iif(Customer_Kit.AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ",True,False) AS LocalOwn " & _
    '            "FROM (Customer_Kit LEFT JOIN qryKitDocSum ON Customer_Kit.ID = qryKitDocSum.KitID) " & _
    '            "INNER JOIN Customer_AccessCode ON Customer_Kit.AccessCodeID = Customer_AccessCode.ID "

    '  sWhere = " (Customer_Kit.Status > " & MyData.STATUS_INAC & ") "
    '  If (MyBase.CurrentUser.KitViewPermissions = MyData.PERM_LOCAL) Then
    '    sWhere = sWhere & " AND (Customer_Kit.AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ") "
    '  End If
    '  If (MyBase.CurrentUser.KitViewPermissions = MyData.PERM_GLOBAL) Then
    '    sWhere = sWhere & " AND (([Status] = " & MyData.PERM_LOCAL & ") AND (Customer_Kit.AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ")) " & _
    '             "OR (([Status] = " & MyData.PERM_GLOBAL & ") AND (Customer_AccessCode.CustomerID = " & MyBase.CurrentUser.CustomerID & ")) "
    '  End If
    '  If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

    '  sOrder = " ORDER BY Customer_Kit.ReferenceNo "

    '  sSQL = sSelect + sWhere + sOrder

    '  Return sSQL
    'End Function
    Private Function BuildSQL() As String
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty

        sSelect = "SELECT Customer_Kit.ID, Customer_Kit.ReferenceNo AS RefNo, Customer_Kit.Description AS RefName, " & _
                  "IIf(qryKitDocSum.InActiveDocCnt is NULL,0,qryKitDocSum.InActiveDocCnt) AS InActiveDocCnt, " & _
                  "IIf(qryKitDocSum.BackOrderDocCnt is NULL,0,qryKitDocSum.BackOrderDocCnt) AS BackOrderDocCnt " & _
                  ", Customer_Kit.Status, iif(Customer_Kit.AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ",True,False) AS LocalOwn " & _
                  "FROM (Customer_Kit LEFT JOIN qryKitDocSum ON Customer_Kit.ID = qryKitDocSum.KitID) " & _
                  "INNER JOIN Customer_AccessCode ON Customer_Kit.AccessCodeID = Customer_AccessCode.ID "

        sWhere = " (Customer_Kit.Status > " & MyData.STATUS_INAC & ") "
        If (MyBase.CurrentUser.KitViewPermissions = MyData.PERM_LOCAL) Then
            sWhere = sWhere & " AND (Customer_Kit.AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ") "
        End If
        If (MyBase.CurrentUser.KitViewPermissions = MyData.PERM_GLOBAL) Then
            sWhere = sWhere & _
                " AND (" & _
                "     (([Status] = " & MyData.PERM_TEMP & ") AND (Customer_Kit.AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ")) " & _
                "  OR (([Status] = " & MyData.PERM_LOCAL & ") AND (Customer_Kit.AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ")) " & _
                "  OR (([Status] = " & MyData.PERM_GLOBAL & ") AND (Customer_AccessCode.CustomerID = " & MyBase.CurrentUser.CustomerID & ")) " & _
                ")"
        End If
        If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

        sOrder = " ORDER BY Customer_Kit.ReferenceNo "

        sSQL = sSelect + sWhere + sOrder

        Return sSQL
    End Function

    Private Function ShowControls() As Boolean
        Dim bShowEdit As Boolean = False

        bShowEdit = (MyBase.CurrentUser.KitEditPermissions = MyBase.MyData.PERM_LOCAL) Or _
                    (MyBase.CurrentUser.KitEditPermissions = MyBase.MyData.PERM_GLOBAL)
        cmdAdd.Visible = bShowEdit
    End Function

    Sub grd_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grd.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or _
            e.Item.ItemType = ListItemType.AlternatingItem Or _
            e.Item.ItemType = ListItemType.SelectedItem Or _
            e.Item.ItemType = ListItemType.EditItem) Then
            Dim sDestin As String = paraPageBase.PAG_AdmKitEdit.ToString()
            Dim sID As String = e.Item.Cells(0).Text.ToString()
            Dim sLinks As String = String.Empty

            Dim iStatus As Int16 = Convert.ToInt16(e.Item.Cells(3).Text.ToString)
            'Dim sType As String = IIf(iStatus = MyBase.MyData.PERM_GLOBAL, "G", "L")
            Dim sType As String = IIf(iStatus = MyBase.MyData.PERM_GLOBAL, "Global", "Local")
            Dim bLOwn As Boolean = Convert.ToBoolean(Convert.ToInt16(e.Item.Cells(4).Text.ToString))
            Dim iInacCnt As Int16 = Convert.ToInt16(e.Item.Cells(5).Text.ToString)
            Dim iBackCnt As Int16 = Convert.ToInt16(e.Item.Cells(6).Text.ToString)
            Dim sNo As String = e.Item.Cells(7).Text.ToString
            Dim sDes As String = e.Item.Cells(8).Text.ToString

            If ((MyBase.CurrentUser.KitViewPermissions = MyBase.MyData.PERM_LOCAL) And bLOwn) Or _
               (MyBase.CurrentUser.KitViewPermissions = MyBase.MyData.PERM_GLOBAL) Then

                'mw - 01-25-2009
                'sLinks = "./AdmKitE.aspx?ED=" & sID
                sLinks = "./AdmKitE.aspx?ED=" & sID & "&Base=0"
                '
                sLinks = "<a class='grid' href='" & sLinks & "'>Detail</a>"

                e.Item.Cells(1).Text = sLinks
            End If
            e.Item.Cells(2).Text = sType

            'Coloring
            If (iInacCnt > 0) Then
                'Inactive Docs
                e.Item.ForeColor = Color.Red
            ElseIf (iBackCnt > 0) Then
                'BackOrder Docs
                'e.Item.ForeColor = Color.Green
                'e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#bd5304") 'Orange
                e.Item.ForeColor = Color.FromArgb(189, 83, 4) 'Orange
            ElseIf (bLOwn = 0) Then
                'Local Owner
                e.Item.ForeColor = Color.Blue
            End If
        End If
    End Sub

    'Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '  Dim sDestin As String = paraPageBase.PAG_AdmKitEdit.ToString()

    '  MyBase.PageDirect(sDestin, 0, -1)
    'End Sub
    Private Overloads Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdAdd.Click
        'Dim sDestin As String = paraPageBase.PAG_AdmKitEdit.ToString()
        'MyBase.PageDirect(sDestin, 0, -1)
        Response.Redirect("AdmKitE.aspx?ED=-1&Base=0'", False)
    End Sub

    'Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '  'Dim sDestin As String = paraPageBase.PAG_Main.ToString()
    '  'MyBase.PageDirect(sDestin, 0, 0)
    '  'False here may help with lost session vars - states to not end request early
    '  Response.Redirect(MyBase.CurrentUser.PreviousPage, False)
    'End Sub
    Private Overloads Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click
        Response.Redirect(MyBase.CurrentUser.PreviousPage, False)
    End Sub

End Class
