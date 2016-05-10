''''''''''''''''''''
'mw - 08-27-2008
'mw - 06-18-2008
'mw - 05-21-2008
'mw - 03-30-2008
'mw - 06-28-2007
'mw - 06-17-2007
'mw - 04-25-2007
''''''''''''''''''''


Imports System.Text

Partial Class OrdReq
  Inherits paraPageBase

#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

  End Sub
  Protected WithEvents txtTitle As System.Web.UI.WebControls.TextBox
  Protected WithEvents lblFName As System.Web.UI.WebControls.Label
  Protected WithEvents txtFName As System.Web.UI.WebControls.TextBox
  Protected WithEvents lblLName As System.Web.UI.WebControls.Label
  Protected WithEvents txtLName As System.Web.UI.WebControls.TextBox
  Protected WithEvents lblEmail As System.Web.UI.WebControls.Label
  Protected WithEvents txtEmail As System.Web.UI.WebControls.TextBox
  Protected WithEvents lblRTitle As System.Web.UI.WebControls.Label
  Protected WithEvents lblRqTitle As System.Web.UI.WebControls.Label
  Protected WithEvents txtRqTitle As System.Web.UI.WebControls.TextBox
  Protected WithEvents vRqLName As System.Web.UI.WebControls.RequiredFieldValidator
  Protected WithEvents vRqFName As System.Web.UI.WebControls.RequiredFieldValidator
  Protected WithEvents vRqEmail As System.Web.UI.WebControls.RequiredFieldValidator
  Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
  Protected WithEvents tblCstM As System.Web.UI.WebControls.Table
  Protected WithEvents txtTest As System.Web.UI.WebControls.TextBox

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        PageTitle = "Order Requestor Information"
        MyBase.PageMessage = "Fill in all information accurately.  Required fields indicated with an asterisk."

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then

            Dim po As New paraOrder
            If po.CheckIfHasDownloadItems(MyBase.CurrentOrder.ItmCart) Then
                trConfirmEmail.Visible = True
            Else
                trConfirmEmail.Visible = False
            End If


            If CurrentOrder.EditOrder = True Then
                cmdEditBack.Visible = True
                cmdNext.Visible = False
            End If

            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri.ToString  ': MyBase.SessionStore()

            MyBase.CurrentOrder.VisitMark(MyBase.MyData.PRO_REQ)
            If (MyBase.CurrentOrder.VisitAll = True) Then
                Dim pScript As New StringBuilder

                pScript.Append("<script>" &
                "function CheckSubmit () " &
                "{ " &
                "  document.frmOrdReq.txtSubmit.value='Y';" &
                "  return true;" &
                "} " &
                "</script>")
                Me.RegisterStartupScript("Startup", pScript.ToString)
                cmdNext.Attributes.Add("onclick", "return CheckSubmit(); ")
            End If

        End If 'End PostBack
    End Sub

    Private Function LoadData() As Boolean
        Dim bSuccess As Boolean = False

        Try
            LoadControls()

            With MyBase.CurrentOrder
                txtRqFName.Text = .RequestorFirstName
                txtRqLName.Text = .RequestorLastName
                txtRqEmail.Text = .RequestorEmail
            End With

            ShowControls()

            bSuccess = True
        Catch ex As Exception
            Session("PMsg") = "Unable to Load Data..."
        End Try

        Return (bSuccess)
    End Function

    Private Function ShowControls()
        Dim bSearch As Boolean : bSearch = False

        bSearch = MyBase.CurrentUser.CanLookupOrder
        If (bSearch = True) Then
            lblRqLName.CssClass = "lblMediumLongSearch"
            cmdSearch.Visible = True
        End If
    End Function

    Private Function LoadControls() As Boolean
        Dim tRow As TableRow
        Dim tCell As TableCell
        Dim lLabel As New Label
        Dim tText As New TextBox
        Dim cCbo As New DropDownList
        Dim vReq As New RequiredFieldValidator
        Dim sValID As String

        Dim rcd As System.Data.DataTable
        Dim indx As Integer
        Dim iMaxIndx As Integer
        Dim bSuccess As Boolean = False

        Dim sID As String
        Dim bRequire As Boolean
        Dim bEntry As Boolean
        Dim sLabel As String
        Dim sValue As String
        Dim bSelect As Boolean
        Dim bType As Boolean

        rcd = MyBase.CurrentOrder.CstCart
        iMaxIndx = rcd.Rows.Count
        indx = 0
        Do While indx < iMaxIndx
            If (rcd.Rows(indx).Item("FieldType") = MyBase.MyData.CST_REQ) Then
                With rcd.Rows(indx)
                    sID = .Item("FieldID").ToString()
                    bRequire = .Item("FieldRequire").ToString()
                    bEntry = .Item("FieldEntry").ToString()
                    sLabel = .Item("FieldName").ToString()
                    sValue = .Item("FieldValue").ToString()
                    bSelect = (.Item("UserEntry").ToString() = "Select")
                    bType = (.Item("UserEntry").ToString() = "Type")
                End With
                tRow = New TableRow

                'Spacing - CELL 0
                tCell = New TableCell
                tCell.ColumnSpan = 3
                tRow.Cells.Add(tCell)
                'ID - CELL 1
                tCell = New TableCell
                tCell.Visible = False
                tCell.Text = sID
                tRow.Cells.Add(tCell)
                'Required - CELL 2
                tCell = New TableCell
                tCell.HorizontalAlign = HorizontalAlign.Left
                tCell.Visible = False
                tCell.Text = Convert.ToInt16(bRequire)
                tRow.Cells.Add(tCell)
                'Entry - CELL 3
                tCell = New TableCell
                tCell.Visible = False
                tCell.Text = Convert.ToInt16(bEntry)
                tRow.Cells.Add(tCell)

                'Label - CELL 4
                tCell = New TableCell
                tCell.ColumnSpan = 1
                tCell.HorizontalAlign = HorizontalAlign.Right
                lLabel = New Label
                lLabel.CssClass = "lblMediumLong"
                lLabel.ID = "lbl" & indx
                lLabel.Text = sLabel & ":"
                tCell.Controls.Add(lLabel)
                tRow.Cells.Add(tCell)

                'Textbox - CELL 5
                tCell = New TableCell
                tCell.ColumnSpan = 1
                tCell.HorizontalAlign = HorizontalAlign.Left
                If bType Then
                    sValID = "txt" & indx
                    tText = New TextBox
                    tText.CssClass = "txtMediumLong"
                    tText.ID = sValID
                    tText.Text = sValue
                    tText.MaxLength = 50
                    tCell.Controls.Add(tText)
                ElseIf bSelect Then
                    sValID = "cbo" & indx
                    cCbo = New DropDownList
                    cCbo.CssClass = "cboShort"
                    cCbo.ID = sValID
                    LoadListCF(cCbo, sID, bEntry, sValue)

                    If Not bEntry Or _
                       (bEntry And (cCbo.SelectedIndex > 0)) Then
                        sValue = ""
                    End If

                    tCell.Controls.Add(cCbo)
                End If

                'New Entry Cbo / Text - CELL 4
                If bSelect And bEntry Then
                    sValID = "txt" & indx
                    tText = New TextBox
                    tText.CssClass = "txtMediumLong"
                    tText.ID = sValID
                    tText.Text = sValue
                    tCell.Controls.Add(tText)
                End If

                lLabel = New Label
                lLabel.CssClass = "lblRequired"
                lLabel.ID = "lblR" & indx
                lLabel.Text = String.Empty
                If bRequire Then lLabel.Text = " * "
                tCell.Controls.Add(lLabel)
                tRow.Cells.Add(tCell)

                'New Entry Cbo / Text
                tCell = New TableCell
                tCell.ColumnSpan = 2
                If Not (bSelect And bEntry) Then
                    tCell.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                End If
                tRow.Cells.Add(tCell)

                tblCst.Rows.Add(tRow)
            End If
            indx = indx + 1
        Loop

        'mw - 06-21-2007
        cmdSearch.Visible = MyBase.CurrentUser.CanLookupOrder
        '

        bSuccess = True
        Return (bSuccess)
    End Function

    Private Function LoadListCF(ByRef Cbo As DropDownList, ByVal sCFID As String, ByVal bEntry As Boolean, ByVal sValue As String) As Boolean
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim dr As OleDb.OleDbDataReader
        Dim bSuccess As Boolean = False
        Dim sSQL As String
        Dim sText As String
        Dim indx As Integer
        Dim iMax As Integer

        indx = 0
        Cbo.Items.Clear()

        If (bEntry = True) Then
            indx = indx + 1
            sText = "NEW ENTRY"
            Cbo.Items.Add(New ListItem(sText, indx))
            Cbo.Items(Cbo.Items.Count - 1).Attributes.Add("style", "color:" & MyBase.MyData.COLOR_NORMAL)
        End If

        sSQL = BuildSQLEntry(sCFID)
        If (sSQL.Length > 0) Then
            MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
            Do While dr.Read()
                indx = indx + 1
                sText = Trim(dr("Entry"))

                Cbo.Items.Add(New ListItem(sText, indx))
                Cbo.Items(Cbo.Items.Count - 1).Attributes.Add("style", "color:" & MyBase.MyData.COLOR_NORMAL)

                If (sValue = sText) Then
                    Cbo.Items(Cbo.Items.Count - 1).Selected = True
                End If
            Loop
            MyBase.MyData.ReleaseReader2(cnn, cmd, dr)

            'SELECT
            indx = 0
            iMax = Cbo.Items.Count - 1
            For indx = 0 To iMax
                If (Cbo.Items(indx).Text = sValue) Then
                    Cbo.SelectedIndex = indx
                End If
            Next

            bSuccess = True
        End If

        Return (bSuccess)
    End Function

    Private Function BuildSQLEntry(ByVal sFieldID As String) As String
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty

        sSelect = "SELECT Lkp_CustomField.Value AS Entry FROM Lkp_CustomField"

        sWhere = "CustomFieldID = " & sFieldID
        If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

        sOrder = " ORDER BY Lkp_CustomField.Value"

        sSQL = sSelect + sWhere + sOrder

        Return sSQL
    End Function

    Private Function ValidData() As Boolean
        Dim bValid As Boolean = False
        Dim sMsg As String = String.Empty

        Dim indx As Integer
        Dim iMaxIndx As Integer

        Dim lID As Long
        Dim sValue As String
        Dim bReq As Boolean
        Dim lLabel As Label
        Dim tText As TextBox
        Dim lLabelReq As Label
        Dim cCbo As DropDownList

        Try
            bValid = (Len(txtRqFName.Text.ToString) > 0)
            If Not bValid Then sMsg = "First Name Required..."
            If bValid Then
                bValid = (Len(txtRqLName.Text.ToString) > 0)
                If Not bValid Then sMsg = "Last Name Required..."
            End If
            If bValid Then
                If bValid Then bValid = (Len(txtRqEmail.Text.ToString) > 0)
                If Not bValid Then sMsg = "Email Required..."
            End If

            If bValid Then
                iMaxIndx = tblCst.Rows.Count
                indx = 0
                Do While indx < iMaxIndx And bValid
                    sValue = ""
                    With tblCst.Rows(indx)
                        lID = Val(.Cells(1).Text)
                        If (lID > 0) Then
                            lLabel = .Cells(4).Controls(0)
                            If (.Cells(5).Controls(0).GetType.ToString = "System.Web.UI.WebControls.TextBox") Then
                                tText = .Cells(5).Controls(0)
                                sValue = tText.Text
                            ElseIf (.Cells(5).Controls(0).GetType.ToString = "System.Web.UI.WebControls.DropDownList") Then
                                cCbo = .Cells(5).Controls(0)
                                If (cCbo.SelectedIndex >= 0) Then
                                    If (cCbo.SelectedItem.Text = "NEW ENTRY") Then
                                        tText = .Cells(5).Controls(1)
                                        sValue = tText.Text
                                    Else
                                        sValue = cCbo.SelectedItem.Text
                                    End If
                                End If
                            End If
                            lLabelReq = .Cells(5).Controls(1)
                            bReq = (lLabelReq.Text.ToString.Trim = "*")
                            If bReq Then
                                If bValid Then bValid = (Len(sValue) > 0)
                                If Not bValid Then sMsg = Left(lLabel.Text.ToString.Trim, Len(lLabel.Text.ToString.Trim) - 1) & " Required..."
                            End If
                        End If
                    End With
                    indx = indx + 1
                Loop
            End If

        Catch ex As Exception
            MyBase.PageMessage = "Error attempting to Save Data..."
        End Try

        If Not bValid Then MyBase.PageMessage = sMsg
        Return (bValid)
    End Function


    Private Function SaveData() As Boolean
        Dim rcd As System.Data.DataTable
        Dim indx As Integer
        Dim iMaxIndx As Integer

        Dim lID As Long
        Dim sValue As String
        Dim tText As TextBox
        Dim cCbo As DropDownList

        Dim bSuccess As Boolean = False


        Try

            With MyBase.CurrentOrder
                .RequestorFirstName = txtRqFName.Text
                .RequestorLastName = txtRqLName.Text
                .RequestorEmail = txtRqEmail.Text
            End With
            MyBase.SessionStore("OrdReq")

            iMaxIndx = tblCst.Rows.Count
            indx = 0
            Do While indx < iMaxIndx
                sValue = ""
                With tblCst.Rows(indx)
                    If (.Cells.Count > 1) Then
                        lID = Val(.Cells(1).Text)
                        If (lID > 0) Then
                            If (.Cells(5).Controls(0).GetType.ToString = "System.Web.UI.WebControls.TextBox") Then
                                tText = .Cells(5).Controls(0)
                                sValue = tText.Text
                            ElseIf (.Cells(5).Controls(0).GetType.ToString = "System.Web.UI.WebControls.DropDownList") Then
                                cCbo = .Cells(5).Controls(0)
                                If (cCbo.SelectedIndex >= 0) Then
                                    If (cCbo.SelectedItem.Text = "NEW ENTRY") Then
                                        tText = .Cells(5).Controls(1)
                                        sValue = tText.Text
                                    Else
                                        sValue = cCbo.SelectedItem.Text
                                    End If
                                End If
                            End If
                            MyBase.CurrentOrder.CstSave(lID, sValue)
                        End If
                    End If
                End With
                indx = indx + 1
            Loop
            MyBase.SessionStore("OrdCst")

            bSuccess = True

        Catch ex As Exception
            MyBase.PageMessage = "Error attempting to Save Data..."
        End Try

        Return (bSuccess)
    End Function

    Private Function ClearData() As Boolean
        Dim rcd As System.Data.DataTable
        Dim indx As Integer
        Dim iMaxIndx As Integer

        Dim lID As Long
        Dim tText As TextBox
        Dim cCbo As DropDownList

        Dim bSuccess As Boolean = False

        Try
            txtRqFName.Text = String.Empty
            txtRqLName.Text = String.Empty
            txtRqEmail.Text = String.Empty

            iMaxIndx = tblCst.Rows.Count
            indx = 0
            Do While indx < iMaxIndx
                With tblCst.Rows(indx)
                    lID = Val(.Cells(1).Text)
                    If (lID > 0) Then
                        If (.Cells(5).Controls(0).GetType.ToString = "System.Web.UI.WebControls.TextBox") Then
                            tText = .Cells(5).Controls(0)
                            tText.Text = String.Empty
                        ElseIf (.Cells(5).Controls(0).GetType.ToString = "System.Web.UI.WebControls.DropDownList") Then
                            cCbo = .Cells(5).Controls(0)
                            If (cCbo.SelectedIndex >= 0) Then
                                If (cCbo.SelectedItem.Text = "NEW ENTRY") Then
                                    tText = .Cells(5).Controls(1)
                                    tText.Text = String.Empty
                                Else
                                    cCbo.SelectedIndex = -1
                                End If
                            End If
                        End If
                    End If
                End With
                indx = indx + 1
            Loop
            bSuccess = True

        Catch ex As Exception
            MyBase.PageMessage = "Error attempting to Clear Data..."
        End Try
    End Function

    Private Overloads Sub cmdNext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdNext.Click
        Dim sDestin As String = paraPageBase.PAG_OrdShip.ToString()
        If Page.IsValid Then
            If ValidData() Then
                SaveData()
                'If (txtSubmit.Text = "Y") Then
                If (txtSubmit.Value = "Y") Then
                    sDestin = paraPageBase.PAG_OrdSubmit.ToString()
                End If
                MyBase.PageDirect(sDestin, 0, 0)
            End If
        End If

    End Sub

    Private Overloads Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdSearch.Click
        Dim sName As String
        sName = txtRqLName.Text.ToString.Trim

        Response.Redirect("OrdSReq.aspx?N=" & sName)
    End Sub
  
    Private Overloads Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdClear.Click
        ClearData()
    End Sub


    Protected Sub cmdEditBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdEditBack.Click
        'save custom fields

        Dim rcd As System.Data.DataTable
        Dim indx As Integer = 0
        Dim iMaxIndx As Integer
        Dim indx2 As Integer = 0



        rcd = MyBase.CurrentOrder.CstCart
        iMaxIndx = rcd.Rows.Count


        Do While indx < iMaxIndx
            If (rcd.Rows(indx).Item("FieldType") = MyBase.MyData.CST_REQ) Then

                indx2 = 2
                For Each r As TableRow In tblCst.Rows
                    For Each c As Control In r.Controls

                        For Each tc As Control In c.Controls
                            If TypeOf tc Is TextBox And IsNumeric(Replace(tc.ID, "txt", "")) Then
                                If indx = indx2 Then

                                    rcd.Rows(indx).Item("FieldValue") = CType(tc, TextBox).Text

                                End If
                                indx2 = indx2 + 1

                            End If

                        Next
                    Next

                Next

            End If
            indx = indx + 1
        Loop

       

        CurrentOrder.RequestorFirstName = Me.txtRqFName.Text
        CurrentOrder.RequestorLastName = Me.txtRqLName.Text
        CurrentOrder.RequestorEmail = Me.txtRqEmail.Text

        Response.Redirect("OrdSve.aspx")






    End Sub
End Class
