''''''''''''''''''''
'mw - 09-01-2009
'mw - 08-27-2008
'mw - 06-18-2008
'mw - 06-07-2008
'mw - 05-23-2008
'mw - 04-03-2008
'mw - 08-24-2007
'mw - 08-18-2007
'mw - 06-28-2007
'mw - 06-17-2007
'mw - 04-25-2007
''''''''''''''''''''


Imports System.Text
Imports System.Data.SqlClient
Imports POSN.dsCustomerTableAdapters

Partial Class OrdShp
    Inherits paraPageBase
    Private intPShowpScheduledDelivery As Integer = -1

    Public Property blnShowpScheduledDelivery() As Boolean
        Get
            If CurrentOrder.EditOrder Then
                Return False
            ElseIf intPShowpScheduledDelivery >= 0 Then
                If intPShowpScheduledDelivery > 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Dim ta As New dsAccessCodesTableAdapters.Customer_AccessCode1TableAdapter
                For Each dr As dsAccessCodes.Customer_AccessCode1Row In ta.GetDataByCustomerIDandCode(Me.CurrentUser.CustomerID, CurrentUser.AccessCode)
                    If dr.IsScheduledDeliveryNull Then
                        intPShowpScheduledDelivery = 0
                    ElseIf dr.ScheduledDelivery > 0 Then
                        intPShowpScheduledDelivery = dr.ScheduledDelivery
                    Else
                        intPShowpScheduledDelivery = 0
                    End If
                Next

                If intPShowpScheduledDelivery > 0 Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblRqTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtRqTitle As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblRqFName As System.Web.UI.WebControls.Label
    Protected WithEvents txtRqFName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblRqLName As System.Web.UI.WebControls.Label
    Protected WithEvents txtRqLName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblRqEmail As System.Web.UI.WebControls.Label
    Protected WithEvents txtRqEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblFMame As System.Web.UI.WebControls.Label
    Protected WithEvents vFName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vRqLName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vCompany As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vAddress1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vCity As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vState As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vZip As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vShipID As System.Web.UI.WebControls.RequiredFieldValidator
    '  Protected WithEvents Dropdownlist1 As System.Web.UI.WebControls.DropDownList

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
        PageTitle = "Order Shipping Information"
        'MyBase.Page_Load(sender, e)
        MyBase.PageMessage = "Fill in all information accurately.  Required fields indicated with an asterisk."

        'If (MyBase.UState = MyBase.MyData.STATE_LoggedOut) Then
        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            'If (MyBase.UState = MyBase.MyData.STATE_None) Then
            MyBase.PageDirect()
            'Response.Write("<H2>User State=" & MyBase.CurrentUser.State & "</H2>")
            'Response.Write("<H2>Cust Name=" & MyBase.CurrentUser.CustomerName & "</H2>")
        End If

        If (Page.IsPostBack = False) Then

            Dim po As New paraOrder

            If po.CheckForDownloadOnly(MyBase.CurrentOrder.ItmCart, CurrentOrder.KitCart) Then
                SetDownloadOnlyShipMethod()
                Response.Redirect("OrdCst.aspx")
            End If

            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri.ToString

            If CurrentOrder.EditOrder = True Then
                cmdEditBack.Visible = True
                cmdNext.Visible = False
            End If
            If blnShowpScheduledDelivery = False Then
                trScheduledDelivery.Visible = False
            End If
            'txtTest.Text = sender.ToString & " - " & Request.Url.AbsoluteUri.ToString & "    -    " & MyBase.CurrentUser.PreviousPage & "  -  " & MyBase.CurrentUser.CurrentPage

            ''mw - 03-29-2008 
            ''   - Do not want to submit now - so no longer need this section
            MyBase.CurrentOrder.VisitMark(paraData.PRO_SHP)
            If (MyBase.CurrentOrder.VisitAll = True) Then
                Dim pScript As New StringBuilder
                pScript.Append("<script>" & _
                "function CheckSubmit () " & _
                "{ " & _
                "  document.frmOrdShp.txtSubmit.value='Y';" & _
                "  return true;" & _
                "} " & _
                "</script>")
                Me.RegisterStartupScript("Startup", pScript.ToString)
                cmdNext.Attributes.Add("onclick", "return CheckSubmit(); ")

                ChangePrefShip()
            End If
            CheckRecipientEmailPhoneReq()
            ChangePrefShip()
            CheckControls()
        End If 'End PostBack
    End Sub

    Private Function LoadData() As Boolean
        'Dim oMyData As paraOrder = New paraOrder
        Dim bSuccess As Boolean = False

        Try

            PopulateCountries()
            With MyBase.CurrentOrder
                txtFName.Text = .ShipContactFirstName
                txtLName.Text = .ShipContactLastName
                txtCompany.Text = .ShipCompany
                txtAddress1.Text = .ShipAddress1
                txtAddress2.Text = .ShipAddress2
                txtCity.Text = .ShipCity
                txtState.Text = .ShipState
                txtZip.Text = .ShipZip
                txtPhoneNumber.Text = .RecipientPhone
                txtEmail.Text = .RecipientEmail
                chkSendConfirmEmail.Checked = .SendRecipientConfirmEmail

                'txtCountry.Text = .ShipCountry
                Dim strCountry As String = .ShipCountry

                Select Case strCountry.ToUpper
                    Case "USA", "US"
                        strCountry = "United States"
                End Select

                For Each li As ListItem In txtCountryDDL.Items
                    If li.Value = strCountry Then
                        li.Selected = True
                        Exit For
                    End If
                Next

                CheckCountry()
                LoadListShip(.ShipPrefID)
                LoadControls()

                If (Page.IsPostBack = False) Then GetSelectShipNote()
                txtNote.Text = .ShipNote
                If (MyBase.CurrentOrder.RequiresAuthorShip() = True) Then
                    lblAuth.Text = "Shipping via " & Mid(MyBase.CurrentOrder.ShipPrefDesc, 2) & ", will require authorization."
                Else
                    lblAuth.Text = ""
                End If
            End With

            ShowControls()

            bSuccess = True
        Catch ex As Exception
            Session("PMsg") = "Unable to Load Data..."
        End Try

        Return (bSuccess)
    End Function

    Private Function ShowControls()
        'Dim bValid As Boolean : bValid = False
        Dim bSearch As Boolean : bSearch = False

        'bValid = MyBase.CurrentOrder.IsValid
        'cmdSubmit.Visible = bValid
        'cmdSubmit.Enabled = bValid
        'If Not bValid Then
        'Me.PageMessage = MyBase.CurrentOrder.ProblemMsg
        'Else
        '  Me.PageMessage = String.Empty
        'End If

        bSearch = MyBase.CurrentUser.CanLookupOrder
        If (bSearch = True) Then
            lblLName.CssClass = "lblMediumLongSearch"
            lblCompany.CssClass = "lblMediumLongSearch"
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
            If (rcd.Rows(indx).Item("FieldType") = paraData.CST_SHP) Then 'And CheckIfItemShows(cboShip.SelectedValue, rcd.Rows(indx).Item("FieldID"))
                'check if field visible based on shipping method:

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
                    tText.CssClass = "txtMedium"
                    tText.ID = sValID
                    tText.Text = sValue
                    tText.MaxLength = 50
                    tCell.Controls.Add(tText)
                ElseIf bSelect Then
                    sValID = "cbo" & indx
                    cCbo = New DropDownList
                    cCbo.CssClass = "cboShort"
                    cCbo.ForeColor = Color.Black
                    cCbo.ID = sValID
                    LoadListCF(cCbo, sID, bEntry, sValue)

                    'If not a selection - then add entered text
                    If Not bEntry Or _
                       (bEntry And (cCbo.SelectedIndex > 0)) Then
                        sValue = ""
                    End If

                    tCell.Controls.Add(cCbo)
                End If

                ''Validator
                'If bRequire Then
                '  vReq = New RequiredFieldValidator
                '  vReq.ID = "v" & indx
                '  vReq.ControlToValidate = sValID
                '  vReq.ErrorMessage = "*"
                'End If

                'New Entry Cbo / Text - CELL 4
                If bSelect And bEntry Then
                    sValID = "txt" & indx
                    tText = New TextBox
                    tText.CssClass = "txtMedium"
                    tText.ID = sValID
                    tText.Text = sValue
                    tCell.Controls.Add(tText)
                End If

                ''Validator
                'If bRequire Then
                '  tCell.Controls.Add(vReq)
                'End If
                'tRow.Cells.Add(tCell)
                If bRequire Then
                    lLabel = New Label
                    lLabel.CssClass = "lblRequired"
                    lLabel.ID = "lblR" & indx
                    lLabel.Text = " * "
                    tCell.Controls.Add(lLabel)
                End If
                tRow.Cells.Add(tCell)

                'SPACING - CELL 6
                tCell = New TableCell
                tCell.ColumnSpan = 2
                If Not (bSelect And bEntry) Then
                    tCell.Text = "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp"
                End If
                tRow.Cells.Add(tCell)

                tblCst.Rows.Add(tRow)
            End If
            indx = indx + 1
        Loop

        bSuccess = True
        Return (bSuccess)
    End Function

    Private Function CheckControls() As Boolean
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
        Dim paraorder As New paraOrder

        rcd = MyBase.CurrentOrder.CstCart
        iMaxIndx = rcd.Rows.Count
        indx = 0
        Do While indx < iMaxIndx
            If (rcd.Rows(indx).Item("FieldType") = paraData.CST_SHP) Then
                'check if field visible based on shipping method:
                Dim strControlID As String = ""
                Dim strControlIDLabel As String = ""
                Dim strLnlReqID As String = ""

                With rcd.Rows(indx)
                    sID = .Item("FieldID").ToString()
                    bRequire = .Item("FieldRequire").ToString()
                    bEntry = .Item("FieldEntry").ToString()
                    sLabel = .Item("FieldName").ToString()
                    sValue = .Item("FieldValue").ToString()
                    bSelect = (.Item("UserEntry").ToString() = "Select")
                    bType = (.Item("UserEntry").ToString() = "Type")
                End With

                strControlIDLabel = "lbl" & indx

                If bType Then
                    strControlID = "txt" & indx
                ElseIf bSelect Then
                    strControlID = "cbo" & indx
                End If
                If bRequire Then

                    strLnlReqID = "lblR" & indx

                End If

                Dim blnShow As Boolean = paraorder.CheckIfCustomItemShows(cboShip.SelectedValue, rcd.Rows(indx).Item("FieldID"))

                For Each r As TableRow In tblCst.Rows
                    For Each c As TableCell In r.Cells
                        For Each ctl As Control In c.Controls
                            If ctl.ID = strControlID Or ctl.ID = strControlIDLabel Or ctl.ID = strLnlReqID Then

                                ctl.Visible = blnShow

                                If blnShow = False Then
                                    If TypeOf ctl Is TextBox Then
                                        CType(ctl, TextBox).Text = ""
                                    End If
                                    If TypeOf ctl Is DropDownList Then
                                        CType(ctl, DropDownList).SelectedIndex = 0
                                    End If
                                End If
                            End If
                        Next
                    Next
                Next

            End If
            indx = indx + 1
        Loop

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

        'Dim rcd As System.Data.DataTable
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
            bValid = (Len(txtFName.Text.ToString) > 0)
            If Not bValid Then sMsg = "First Name Required..."
            If bValid Then
                bValid = (Len(txtLName.Text.ToString) > 0)
                If Not bValid Then sMsg = "Last Name Required..."
            End If
            If bValid Then
                If bValid Then bValid = (Len(txtAddress1.Text.ToString) > 0)
                If Not bValid Then sMsg = "Address Line 1 Required..."
            End If
            If bValid Then
                If bValid Then bValid = (Len(txtCity.Text.ToString) > 0)
                If Not bValid Then sMsg = "City Required..."
            End If
            If bValid Then
                If bValid Then bValid = (Len(txtState.Text.ToString) > 0)
                If Not bValid Then sMsg = "State Required..."
            End If
            If bValid Then
                If bValid Then bValid = (Len(txtZip.Text.ToString) > 0)
                If Not bValid Then sMsg = "Postal Code Required..."
            End If
            If bValid Then
                If bValid Then bValid = (Len(txtCountryDDL.SelectedValue.ToString) > 0)
                If Not bValid Then sMsg = "Country Required..."
            End If
            If bValid Then
                If bValid Then bValid = (GetSelectShipID() > 0)
                If Not bValid Then sMsg = "Preferred Shipping Method Required..."
            End If

            If blnShowpScheduledDelivery Then
                If bValid Then
                    If bValid Then bValid = (IsDate(txtScheduledDate.Text))
                    If Not bValid Then sMsg = "Scheduled Date Required"
                End If
            End If
            If blnShowpScheduledDelivery Then
                If bValid Then
                    If bValid Then bValid = (DateDiff(DateInterval.Day, CDate(txtScheduledDate.Text), Now.Date) <= 0)
                    If Not bValid Then sMsg = "Scheduled Date Must Not Be In The Past"
                End If
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
                            If bReq And lLabelReq.Visible = True Then
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
        'Dim bRequire As Boolean
        'Dim bEntry As Boolean
        'Dim sLabel As String
        Dim sValue As String
        Dim tText As TextBox
        Dim cCbo As DropDownList

        Dim bSuccess As Boolean = False

        Try
        

            With MyBase.CurrentOrder
                .ShipContactFirstName = txtFName.Text
                .ShipContactLastName = txtLName.Text
                .ShipCompany = txtCompany.Text
                .ShipAddress1 = txtAddress1.Text
                .ShipAddress2 = txtAddress2.Text
                .ShipCity = txtCity.Text
                .ShipState = txtState.Text
                .ShipZip = txtZip.Text
                .ShipCountry = txtCountryDDL.SelectedValue
                .ShipPrefID = GetSelectShipID()
                .ShipPrefDesc = cboShip.SelectedItem.Text
                .ShipNote = Left$(Trim$(txtNote.Text), 200)
                .RecipientEmail = txtEmail.Text
                .RecipientPhone = txtPhoneNumber.Text
                .SendRecipientConfirmEmail = chkSendConfirmEmail.Checked
                If blnShowpScheduledDelivery Then
                    .ScheduledDelivery = txtScheduledDate.Text
                End If
            End With
            MyBase.SessionStore("OrdShp")

            'Reset custom fields before saving:


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
            txtFName.Text = String.Empty
            txtLName.Text = String.Empty
            txtCompany.Text = String.Empty
            txtAddress1.Text = String.Empty
            txtAddress2.Text = String.Empty
            txtCity.Text = String.Empty
            txtState.Text = String.Empty
            txtZip.Text = String.Empty
            'txtCountry.Text = String.Empty

            txtCountryDDL.SelectedIndex = 0
            cboShip.SelectedIndex = -1
            txtNote.Text = String.Empty

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

    Private Function GetSelectShipID() As Int32
        Dim sID As Int32 = 0
        '  Dim indx As Integer
        '  Dim iMax As Integer

        'iMax = cboShip.Items.Count - 1
        'For indx = 0 To iMax
        'If lst.Items(indx).Selected Then
        'If (Len(sCats) > 0) Then sCats = sCats & ","
        'sID = cboShip.Items(indx).Value
        'End If
        'Next
        sID = cboShip.Items(cboShip.SelectedIndex).Value

        Return sID
    End Function

    Private Function GetSelectShipNote() As String
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim dr As OleDb.OleDbDataReader
        Dim bSuccess As Boolean = False
        Dim sSQL As String
        Dim sID As String
        Dim sText As String

        'sText = lblShipTypeNote.Text
        sID = GetSelectShipID()
        '    If bLoad Or (sID <> MyBase.CurrentOrder.ShipPrefID) Then
        sSQL = "SELECT Note FROM Customer_ShippingMethod WHERE ID = " & sID
        MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
        If dr.Read() Then
            sText = dr("Note")
        End If
        MyBase.MyData.ReleaseReader2(cnn, cmd, dr)
        'End If

        Return sText
    End Function


    Private Function BuildSQLShip(Optional ByVal bShowActiveOnly As Boolean = True) As String
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty



        sSelect = "SELECT Customer_ShippingMethod.ID, Customer_ShippingMethod.Code AS RefName, Customer_ShippingMethod.[Description]  AS RefDescr, CASE Permit	WHEN 2 Then '*'	ELSE '' END as Symbol " & _
                  " FROM Customer_ShippingMethod INNER JOIN Customer_ShippingPerm ON Customer_ShippingMethod.ID = Customer_ShippingPerm.ShippingMethodID  "


        'sSelect = "SELECT Customer_ShippingMethod.ID, Customer_ShippingMethod.Code AS RefName, Customer_ShippingMethod.Description AS RefDescr, iif(Permit=2,'*','') AS Symbol " & _
        '          "FROM Customer_ShippingMethod INNER JOIN Customer_ShippingPerm ON Customer_ShippingMethod.ID = Customer_ShippingPerm.ShippingMethodID "

        sWhere = "(Customer_ShippingPerm.AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ") AND (Customer_ShippingPerm.Permit > 0) AND Customer_ShippingMethod.Code<>'DOWNLOAD'"

        If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

        sOrder = " ORDER BY  Customer_ShippingMethod.SeqNo, Customer_ShippingMethod.Code, Customer_ShippingMethod.Description "

        sSQL = sSelect + sWhere + sOrder

        Return sSQL
    End Function

    Private Function LoadListShip(Optional ByVal lID As Long = 0) As Long
        Dim cnn As SqlConnection
        Dim cmd As SqlCommand
        Dim dr As SqlDataReader
        Dim bSuccess As Boolean = False
        Dim sSQL As String
        Dim sID As String
        Dim sText As String

        cboShip.Items.Clear()

        sSQL = BuildSQLShip()
        If (sSQL.Length > 0) Then
            MyBase.MyData.GetReaderSQL(cnn, cmd, dr, sSQL)
            Do While dr.Read()
                sID = dr("ID").ToString()
                sText = Trim(dr("Symbol")) & "  " & dr("RefName") & " " & dr("RefDescr")

                cboShip.Items.Add(New ListItem(sText, sID))
                cboShip.Items(cboShip.Items.Count - 1).Attributes.Add("style", "color:" & paraData.COLOR_NORMAL)

                If (sID = lID) Then
                    cboShip.Items(cboShip.Items.Count - 1).Selected = True
                    'cboShip.SelectedIndex = cboShip.Items.Count - 1
                End If
            Loop
            MyBase.MyData.ReleaseReaderSQL(cnn, cmd, dr)

            'SELECT
            Dim indx As Integer
            Dim iMax As Integer

            iMax = cboShip.Items.Count - 1
            For indx = 0 To iMax
                If (cboShip.Items(indx).Value = lID) Then
                    cboShip.SelectedIndex = indx
                End If
            Next

            bSuccess = True
        End If

        Return (bSuccess)
    End Function

    'Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
    '  'Dim sDestin As String = paraPageBase.PAG_OrdRequest.ToString()
    '  'MyBase.PageDirect(sDestin, 0, 0)
    '  'False here may help with lost session vars - states to not end request early
    '  Response.Redirect(MyBase.CurrentUser.PreviousPage, False)
    'End Sub

    'Private Sub cmdNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '  Dim sDestin As String = paraPageBase.PAG_OrdItemSelect.ToString()

    '  SaveData()
    '  If (txtSubmit.Text = "Y") Then
    '    sDestin = paraPageBase.PAG_OrdSubmit.ToString()
    '  End If
    '  MyBase.PageDirect(sDestin, 0, 0)
    'End Sub
    Private Overloads Sub cmdNext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdNext.Click
        Dim sDestin As String = paraPageBase.PAG_OrdCustom.ToString()


        If ValidData() And Page.IsValid Then
            SaveData()
            'If (txtSubmit.Text = "Y") Then
            If (txtSubmit.Value = "Y") Then
                sDestin = paraPageBase.PAG_OrdSubmit.ToString()
            End If
            MyBase.PageDirect(sDestin, 0, 0)
        End If
    End Sub

    'Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '  Dim sDestin As String = paraPageBase.PAG_OrdSubmit.ToString()

    '  SaveData()
    '  MyBase.PageDirect(sDestin, 0, 0)
    'End Sub

    ''mw - 06-28-2007
    'Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
    '  'Search for similar orders and populate selections
    '  'Display selections and populate button

    '  Dim sName As String = String.Empty
    '  Dim sCompany As String = String.Empty

    '  sName = txtLName.Text.ToString.Trim
    '  sCompany = txtCompany.Text.ToString.Trim

    '  Response.Redirect("OrdSShp.aspx?N=" & sName & "&C=" & sCompany)
    'End Sub
    Private Overloads Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdSearch.Click
        'Search for similar orders and populate selections
        'Display selections and populate button

        Dim sName As String = String.Empty
        Dim sCompany As String = String.Empty

        sName = txtLName.Text.ToString.Trim
        sCompany = txtCompany.Text.ToString.Trim

        Response.Redirect("OrdSShp.aspx?N=" & sName & "&C=" & sCompany)
    End Sub

    'Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
    '  'Clear all Shipping Data currently displayed
    '  ClearData()
    'End Sub
    Private Overloads Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdClear.Click
        '  'Clear all Shipping Data currently displayed
        ClearData()
    End Sub

    Private Sub cboShip_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboShip.SelectedIndexChanged
        ChangePrefShip()
        CheckControls()
    End Sub
    Sub ChangePrefShip()
        Dim sShipText As String = String.Empty

        lblShipTypeNote.Text = GetSelectShipNote()
        If (cboShip.SelectedIndex >= 0) Then
            sShipText = cboShip.SelectedItem.Text
            If (Left(sShipText, 1) = "*") Then
                lblAuth.Text = "Shipping via " & Mid(sShipText, 2) & ", will require authorization."
            End If
        End If
    End Sub
    Private Sub txtCountry_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCountryDDL.SelectedIndexChanged 'txtCountry.TextChanged
        CheckCountry()
    End Sub



    Private Sub CheckCountry()
        Dim bOnExList As Boolean
        Dim sMsg As String = String.Empty

        bOnExList = MyBase.CurrentOrder.CountryExclusionCheck(MyBase.MyData, MyBase.CurrentUser.CustomerID, txtCountryDDL.SelectedValue.ToString, sMsg)

        If bOnExList = True Then
            lblCountryMsg.Text = sMsg
        Else
            lblCountryMsg.Text = ""
        End If
    End Sub
    Protected Sub cmdEditBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdEditBack.Click

        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        With CurrentOrder
            .RecipientPhone = txtPhoneNumber.Text
            .RecipientEmail = txtEmail.Text
        End With
        CurrentOrder.ShipContactFirstName = txtFName.Text


        CurrentOrder.ShipContactLastName = txtLName.Text


        CurrentOrder.ShipCompany = txtCompany.Text


        CurrentOrder.ShipAddress1 = txtAddress1.Text

        CurrentOrder.ShipAddress2 = txtAddress2.Text


        CurrentOrder.ShipCity = txtCity.Text


        CurrentOrder.ShipState = txtState.Text


        CurrentOrder.ShipZip = txtZip.Text



        CurrentOrder.ShipCountry = txtCountryDDL.SelectedValue



        CurrentOrder.ShipPrefID = cboShip.SelectedValue
        CurrentOrder.ShipPrefDesc = qa.GetShippingMethodName(CurrentUser.CustomerID, cboShip.SelectedValue)

        CurrentOrder.ShipNote = txtNote.Text


        'save custom fields
        Dim rcd As System.Data.DataTable
        Dim indx As Integer = 0
        Dim iMaxIndx As Integer
        Dim indx2 As Integer = 0



        rcd = MyBase.CurrentOrder.CstCart
        iMaxIndx = rcd.Rows.Count


        Do While indx < iMaxIndx
            If (rcd.Rows(indx).Item("FieldType") = MyBase.MyData.CST_SHP) Then

                indx2 = 3
                For Each r As TableRow In tblCst.Rows
                    For Each c As Control In r.Controls

                        For Each tc As Control In c.Controls
                            If TypeOf tc Is TextBox And IsNumeric(Replace(tc.ID, "txt", "")) Then
                                If indx = indx2 Then

                                    rcd.Rows(indx).Item("FieldValue") = CType(tc, TextBox).Text
                                    Response.Write(CType(tc, TextBox).Text & "<BR>")
                                End If
                                indx2 = indx2 + 1

                            End If

                        Next
                    Next

                Next

            End If
            indx = indx + 1
        Loop


        Response.Redirect("OrdSve.aspx")
    End Sub

    Sub PopulateCountries()
        Dim taMatch As New dsCustomerTableAdapters.Customer_MatchTableAdapter
        Dim strCustomerID As String = CurrentUser.CustomerID
        Dim dt As dsCustomer.Customer_MatchDataTable


        If CurrentOrder.EditOrder Then
            Dim qa As New dsCustomerTableAdapters.QueriesTableAdapter
            strCustomerID = qa.GetCustomerIDFromAccessCodeID(CurrentOrder.AccessCodeID)
        End If

        dt = taMatch.GetDataByCustomer(strCustomerID)

        Dim strCountries(dt.Rows.Count) As String



        Dim ta As New dsOrdersTableAdapters.ListsTableAdapter

        Dim liUS As New ListItem
        liUS.Value = "United States"
        liUS.Text = "United States"
        liUS.Selected = False
        txtCountryDDL.Items.Add(liUS)

        For Each dr As dsOrders.ListsRow In ta.GetCountries.Rows
            Dim li As New ListItem
            li.Value = dr.Text
            li.Text = dr.Text
            txtCountryDDL.Items.Add(li)
        Next
        Dim liUS2 As New ListItem
        liUS2.Value = "United States"
        liUS2.Text = "United States"
        liUS2.Selected = False
        txtCountryDDL.Items.Add(liUS2)
    End Sub
    Sub CheckRecipientEmailPhoneReq()
        Dim dr As dsCustomer.CustomerRow
        Dim ta As New dsCustomerTableAdapters.CustomerTableAdapter
        Try
            dr = ta.GetByCustomerID(CurrentUser.CustomerID).Rows(0)
            RequiredFieldValidatorEmail.Enabled = dr.RequireRecipientEmail
            RequiredFieldValidatorPhone.Enabled = dr.RequireRecipientPhone
            LabelEmail.Visible = dr.RequireRecipientEmail
            LabelPhone.Visible = dr.RequireRecipientPhone
        Catch ex As Exception
            RequiredFieldValidatorEmail.Enabled = False
            RequiredFieldValidatorPhone.Enabled = False
            LabelEmail.Visible = False
            LabelPhone.Visible = False
        End Try

    End Sub


    Sub SetDownloadOnlyShipMethod()
        Dim qa As New dsCustomerTableAdapters.QueriesTableAdapter
        If qa.CheckIfCustomerHasDownloadMethod(CurrentUser.CustomerID) <= 0 Then
            Dim ta As New dsCustomerTableAdapters.Customer_ShippingMethodTableAdapter
            Dim dt As New dsCustomer.Customer_ShippingMethodDataTable
            Dim dr As dsCustomer.Customer_ShippingMethodRow
            dr = dt.NewCustomer_ShippingMethodRow
            With dr
                .CustomerID = CurrentUser.CustomerID
                .Code = "DOWNLOAD"
                .Description = "Digital Download Only"
                .AcctNo = ""
                .HdChg = 0
                .Note = ""
                .SeqNo = 99

            End With

            dt.Rows.Add(dr)
            ta.Update(dt)
        End If
        With MyBase.CurrentOrder
            .ShipPrefID = qa.getDownloadMethodID(CurrentUser.CustomerID)
            .ShipContactFirstName = .RequestorFirstName
            .ShipContactLastName = .RequestorLastName
            .ShipAddress1 = "N/A"
            .ShipCity = "N/A"
            .ShipCity = "N/A"
            .ShipState = "N/A"
            .ShipZip = "N/A"
            .ShipCountry = "N/A"

        End With
    End Sub
End Class
