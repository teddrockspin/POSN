''''''''''''''''''''
'mw - 09-01-2009
'mw - 04-02-2009
'mw - 08-27-2008
'mw - 06-18-2008
'mw - 05-21-2008
'mw - 04-06-2008
'mw - 03-07-2008
'mw - 06-28-2007
'mw - 06-17-2007
'mw - 04-25-2007
''''''''''''''''''''


Imports System.Text

Partial Class OrdCst
  Inherits paraPageBase

#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

  End Sub
  Protected WithEvents vCVID As System.Web.UI.WebControls.RequiredFieldValidator
  Protected WithEvents grd As System.Web.UI.WebControls.DataGrid
  Protected WithEvents TextBoxesHere As System.Web.UI.WebControls.PlaceHolder
  Protected WithEvents Requiredfieldvalidator1 As System.Web.UI.WebControls.RequiredFieldValidator
  Protected WithEvents cmdCS As System.Web.UI.WebControls.ImageButton
  'Protected WithEvents txtSubmit As System.Web.UI.WebControls.TextBox
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

  Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    PageTitle = "Order Customize"
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
            If po.CheckForDownloadOnly(MyBase.CurrentOrder.ItmCart, MyBase.CurrentOrder.KitCart) Then
                CurrentOrder.CoverSheetID = cboCVID.SelectedValue
                CurrentOrder.CoverSheetText = cboCVID.SelectedItem.Text
                Response.Redirect("OrdSve.aspx")
            End If

            If CurrentOrder.EditOrder = True Then
                cmdEditBack.Visible = True
                cmdNext.Visible = False
            End If

            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri.ToString ': MyBase.SessionStore()
            'txtTest.Text = Request.Url.AbsoluteUri.ToString & "    -    " & MyBase.CurrentUser.PreviousPage & "  -  " & MyBase.CurrentUser.CurrentPage

            ''mw - 03-29-2008 
            ''   - Do not want to submit now - so no longer need this section
            MyBase.CurrentOrder.VisitMark(MyBase.MyData.PRO_CST)
            If (MyBase.CurrentOrder.VisitAll = True) Then
                Dim pScript As New StringBuilder
                'mw - 06-18-2008 - go directly to submit, if ready
                'pScript.Append("<script>" & _
                '"function CheckSubmit () " & _
                '"{ " & _
                '"  if (confirm('Do you wish to attempt to submit the current order at this time?') == true)" & _
                '"  {  document.frmOrdCst.txtSubmit.value='Y';}" & _
                '"  else " & _
                '"  {  document.frmOrdCst.txtSubmit.value='N';}" & _
                '"  return true;" & _
                '"} " & _
                '"</script>")
                pScript.Append("<script>" & _
                "function CheckSubmit () " & _
                "{ " & _
                "  document.frmOrdCst.txtSubmit.value='Y';" & _
                "  return true;" & _
                "} " & _
                "</script>")
                Me.RegisterStartupScript("Startup", pScript.ToString)
                cmdNext.Attributes.Add("onclick", "return CheckSubmit(); ")
            End If
        End If 'End PostBack

    'If (Page.IsPostBack = False) Then
    '  If (LoadData() = True) Then
    '    'ShowControls()
    '  Else
    '    Session("PMsg") = "Unable to Load Data..."
    '  End If 'End PostBack
    'End If
  End Sub

  Private Function LoadData() As Boolean
    Dim bSuccess As Boolean = False

    Try
      LoadControls()

      With MyBase.CurrentOrder
        LoadListCoverSheet(.CoverSheetID)
        txtCVText.Text = .CoverSheetText
      End With

      ShowControls()

      bSuccess = True
    Catch ex As Exception
      Session("PMsg") = "Unable to Load Data..."
    End Try

    Return (bSuccess)
  End Function

  Private Function ShowControls()
    Dim bValid As Boolean : bValid = False

    txtCVText.Visible = MyBase.CurrentUser.CanEditCoverSheet
        lblCVText.Visible = txtCVText.Visible

        Dim blnDisableAllQuantityLimits As Boolean


        If CurrentOrder.EditOrder Then
            Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
            blnDisableAllQuantityLimits = qa.GetDisableAllQuantityLimitsByAccessCodeID(CurrentOrder.AccessCodeID)
        Else
            blnDisableAllQuantityLimits = CurrentUser.DisableAllQuantityLimits
        End If
        bValid = MyBase.CurrentOrder.IsValid(blnDisableAllQuantityLimits)
        'cmdSubmit.Visible = bValid
        'cmdSubmit.Enabled = bValid
        'If Not bValid Then
        'Me.PageMessage = MyBase.CurrentOrder.ProblemMsg
        'Else
        '  Me.PageMessage = String.Empty
        'End If
        'cmdCS.Visible = True '(MyBase.CurrentUser.CanDownloadImages = True)
        lkCS.Visible = True '(MyBase.CurrentUser.CanDownloadImages = True)
  End Function

  Private Function LoadControls() As Boolean
    Dim tRow As TableRow
    Dim tCell As TableCell
    Dim lLabel As New Label
    Dim tText As New TextBox
    Dim cCbo As New DropDownList
    'Dim vReq As New RequiredFieldValidator
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

    'rcd = MyBase.CurrentCstCart
    rcd = MyBase.CurrentOrder.CstCart
    iMaxIndx = rcd.Rows.Count
    indx = 0
    Do While indx < iMaxIndx
      If (rcd.Rows(indx).Item("FieldType") = MyBase.MyData.CST_DET) Then
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
        'If bRequire Then
        'lLabel.Text = "* " & sLabel & ":"
        'Else
        lLabel.Text = sLabel & ":"
        'End If
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
          cCbo.ID = sValID
          LoadListCF(cCbo, sID, bEntry, sValue)

          'If not a selection - then add entered text
          If Not bEntry Or _
             (bEntry And (cCbo.SelectedIndex > 0)) Then
            sValue = ""
          End If

          tCell.Controls.Add(cCbo)
        End If
        'tRow.Cells.Add(tCell)

        ''Validator
        'If bRequire Then
        '  vReq = New RequiredFieldValidator
        '  vReq.ID = "v" & indx
        '  vReq.ControlToValidate = sValID
        '  vReq.ErrorMessage = "*"
        'End If

        'New Entry Cbo / Text - CELL 4
        If bSelect And bEntry Then
          'tCell = New TableCell
          'tCell.HorizontalAlign = HorizontalAlign.Left
          sValID = "txt" & indx
          tText = New TextBox
          tText.CssClass = "txtMedium"
          tText.ID = sValID
          tText.Text = sValue
          tCell.Controls.Add(tText)
          'tRow.Cells.Add(tCell)
        End If

        ''Validator
        'If bRequire Then
        '  tCell.Controls.Add(vReq)
        'End If
        If bRequire Then
          lLabel = New Label
          lLabel.CssClass = "lblRequired"
          lLabel.ID = "lblR" & indx
          lLabel.Text = " * "
          tCell.Controls.Add(lLabel)
        End If
        tRow.Cells.Add(tCell)

        '        'New Entry Cbo / Text - CELL 6
        tCell = New TableCell
        tCell.ColumnSpan = 2
        'If bSelect And bEntry Then
        ''tCell = New TableCell
        'tCell.HorizontalAlign = HorizontalAlign.Left : tCell.BackColor = Color.Coral
        'sValID = "txt" & indx
        'tText = New TextBox
        'tText.CssClass = "txtMedium"
        'tText.ID = sValID
        'tText.Text = sValue
        'tCell.Controls.Add(tText)
        ''tRow.Cells.Add(tCell)
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

  'Private Sub LoadCbo(ByRef Cbo As DropDownList, ByVal bEntry As Boolean, ByVal sValue As String)
  '  'TO DO...

  '  Dim sID As String
  '  Dim sText As String

  '  If (bEntry = True) Then
  '    sID = 1
  '    sText = "NEW ENTRY"
  '    Cbo.Items.Add(New ListItem(sText, sID))
  '    Cbo.Items(Cbo.Items.Count - 1).Attributes.Add("style", "color:" & MyBase.MyData.COLOR_NORMAL)
  '  End If

  '  sID = 2
  '  sText = "A"
  '  Cbo.Items.Add(New ListItem(sText, sID))
  '  Cbo.Items(Cbo.Items.Count - 1).Attributes.Add("style", "color:" & MyBase.MyData.COLOR_NORMAL)
  '  If (sText = sValue) Then
  '    Cbo.Items(Cbo.Items.Count - 1).Selected = True
  '  End If

  '  sID = 3
  '  sText = "B"
  '  Cbo.Items.Add(New ListItem(sText, sID))
  '  Cbo.Items(Cbo.Items.Count - 1).Attributes.Add("style", "color:" & MyBase.MyData.COLOR_NORMAL)
  '  If (sText = sValue) Then
  '    Cbo.Items(Cbo.Items.Count - 1).Selected = True
  '  End If

  '  sID = 4
  '  sText = "C"
  '  Cbo.Items.Add(New ListItem(sText, sID))
  '  Cbo.Items(Cbo.Items.Count - 1).Attributes.Add("style", "color:" & MyBase.MyData.COLOR_NORMAL)
  '  If (sText = sValue) Then
  '    Cbo.Items(Cbo.Items.Count - 1).Selected = True
  '  End If
  'End Sub
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

  Private Function BuildSQLCoverSheet(Optional ByVal bShowActiveOnly As Boolean = True) As String
    Dim sSQL As String = String.Empty
    Dim sSelect As String = String.Empty
    Dim sWhere As String = String.Empty
    Dim sOrder As String = String.Empty

    sSelect = "SELECT Customer_CoverSheet.ID, Customer_CoverSheet.Name AS RefName " & _
              "FROM Customer_CoverSheet "

    sWhere = " (Customer_CoverSheet.CustomerID = " & MyBase.CurrentUser.CustomerID & ") "
    If (Len(MyBase.CurrentUser.CoverSheetLevels) > 0) Then
      sWhere = sWhere & _
               " AND  ([Level] IN (" & MyBase.CurrentUser.CoverSheetLevels & "))"
    Else
      sWhere = sWhere & _
               " AND  ([Level] IN (''))"
    End If
    If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

    sOrder = sOrder & " ORDER BY Customer_CoverSheet.Name "

    sSQL = sSelect + sWhere + sOrder

    Return sSQL
  End Function

  Private Function LoadListCoverSheet(Optional ByVal lID As Long = 0) As Long
    Dim cnn As OleDb.OleDbConnection
    Dim cmd As OleDb.OleDbCommand
    Dim dr As OleDb.OleDbDataReader
    Dim bSuccess As Boolean = False
    Dim sSQL As String
    Dim sID As String
    Dim sText As String

    cboCVID.Items.Clear()

        'check if there's new way of setting cover sheet for access codes
        Dim qa As New dsCustomerTableAdapters.QueriesTableAdapter

        If qa.CheckIfThereCustomAccessCodeCoverSheet(MyBase.CurrentUser.AccessCodeID) > 0 Then
            Dim ta As New dsAccessCodesTableAdapters.DataTable1TableAdapter
            cboCVID.DataSource = ta.GetDataByAccessCode(CurrentUser.AccessCodeID)
            cboCVID.DataTextField = "Name"
            cboCVID.DataValueField = "CoverSheetID"
            cboCVID.DataBind()
            bSuccess = True
        Else

            sSQL = BuildSQLCoverSheet()
            If (sSQL.Length > 0) Then
                MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
                Do While dr.Read()
                    sID = dr("ID").ToString()
                    sText = Trim(dr("RefName"))

                    cboCVID.Items.Add(New ListItem(sText, sID))
                    cboCVID.Items(cboCVID.Items.Count - 1).Attributes.Add("style", "color:" & MyBase.MyData.COLOR_NORMAL)

                    If (sID = lID) Then
                        cboCVID.Items(cboCVID.Items.Count - 1).Selected = True
                        'cboShip.SelectedIndex = cboShip.Items.Count - 1
                    End If
                Loop
                MyBase.MyData.ReleaseReader2(cnn, cmd, dr)

                'SELECT
                Dim indx As Integer
                Dim iMax As Integer

                iMax = cboCVID.Items.Count - 1
                For indx = 0 To iMax
                    If (cboCVID.Items(indx).Value = lID) Then
                        cboCVID.SelectedIndex = indx
                    End If
                Next

                LoadCSLink()
                bSuccess = True
            End If

        End If
        Return (bSuccess)
    End Function

  'Private Function GetSelectCVID() As Int32
  '  Dim sID As Int32 = 0
  '  '  Dim indx As Integer
  '  '  Dim iMax As Integer

  '  'iMax = cboShip.Items.Count - 1
  '  'For indx = 0 To iMax
  '  'If lst.Items(indx).Selected Then
  '  'If (Len(sCats) > 0) Then sCats = sCats & ","
  '  'sID = cboShip.Items(indx).Value
  '  'End If
  '  'Next
  '  sID = cboCVID.Items(cboCVID.SelectedIndex).Value

  '  Return sID
  'End Function

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
      bValid = (cboCVID.SelectedItem.Value > 0)
      If Not bValid Then sMsg = "Cover Sheet Required..."

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
    'Dim bRequire As Boolean
    'Dim bEntry As Boolean
    'Dim sLabel As String
    Dim sValue As String
    Dim tText As TextBox
    Dim cCbo As DropDownList

    Dim bSuccess As Boolean = False

    Try
      ''Note:  Could change saving...
      '' to completely remove DataTable.Rows and add all new DataTable.Rows
      ''
      ''rcd = MyBase.CurrentCstCart
      'rcd = MyBase.CurrentOrder.CstCart
      'iMaxIndx = rcd.Rows.Count
      'indx = 0
      'Do While indx < iMaxIndx
      '  sValue = ""
      '  With rcd.Rows(indx)
      '    sID = .Item("FieldID").ToString()
      '    sLabel = .Item("FieldName").ToString()
      '    If (tblCst.Rows(indx).Cells(4).Controls(0).GetType.ToString = "System.Web.UI.WebControls.TextBox") Then
      '      tText = tblCst.Rows(indx).Cells(4).Controls(0)
      '      sValue = tText.Text
      '    ElseIf (tblCst.Rows(indx).Cells(4).Controls(0).GetType.ToString = "System.Web.UI.WebControls.DropDownList") Then
      '      cCbo = tblCst.Rows(indx).Cells(4).Controls(0)
      '      If (cCbo.SelectedIndex >= 0) Then
      '        If (cCbo.SelectedItem.Text = "NEW ENTRY") Then
      '          tText = tblCst.Rows(indx).Cells(6).Controls(0)
      '          sValue = tText.Text
      '        Else
      '          sValue = cCbo.SelectedItem.Text
      '        End If
      '      End If
      '    End If
      '    .Item("FieldValue") = sValue
      '  End With
      '  indx = indx + 1
      'Loop
      'MyBase.CurrentOrder.CstCart = rcd
      'Note:  Could change saving...
      ' to completely remove DataTable.Rows and add all new DataTable.Rows
      '
      'rcd = MyBase.CurrentCstCart
      '
      'rcd = MyBase.CurrentOrder.CstCart
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

      MyBase.CurrentOrder.CoverSheetID = cboCVID.SelectedItem.Value
      MyBase.CurrentOrder.CoverSheetDesc = cboCVID.SelectedItem.Text
      MyBase.CurrentOrder.CoverSheetText = Left(Trim$(txtCVText.Text), 400)
      MyBase.SessionStore("OrdCst")

      bSuccess = True

    Catch ex As Exception
      MyBase.PageMessage = "Error attempting to Save Data..."
    End Try

    Return (bSuccess)
  End Function

  'Private Function GetSelectItem() As String
  '  Dim sItem As String = String.Empty

  '  'TO DO...  will need to look up reference # or name of report to use for PDF file
  '  sItem = cboCVID.SelectedItem.Text
  '  'Dim indx As Integer
  '  'Dim iMax As Integer
  '  'Dim bDone As Boolean : bDone = False

  '  'iMax = cboCVID.Items.Count - 1
  '  'For indx = 0 To iMax
  '  '  If (Not bDone) And lstItem.Items(indx).Selected Then
  '  '    'If (Len(sCats) > 0) Then sCats = sCats & ","
  '  '    sItem = lstItem.Items(indx).Text
  '  '    sRef = Left(sItem, sItem.IndexOf(":")).Trim
  '  '    sDes = Mid(sItem, sItem.IndexOf(":") + 2).Trim
  '  '    sFile = GetFileName(sRef)
  '  '    bDone = True
  '  '  End If
  '  'Next

  '  Return sItem
  'End Function

  Private Function GetSelectCS(ByVal lID As Integer) As String
    Dim cnn As OleDb.OleDbConnection
    Dim cmd As OleDb.OleDbCommand
    Dim dr As OleDb.OleDbDataReader
    Dim bSuccess As Boolean = False
    Dim sSQL As String
    Dim sFile As String = String.Empty

    sSQL = "SELECT Customer_CoverSheet.Name, Customer_CoverSheet.ReportName " & _
           "FROM Customer_CoverSheet " & _
           "WHERE (Customer_CoverSheet.ID = " & lID & ") "
    MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
    If dr.Read() Then
      sFile = dr("ReportName").ToString()
    End If
    MyBase.MyData.ReleaseReader2(cnn, cmd, dr)

    Return sFile
  End Function

  'Private Overloads Sub cmdCS_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdCS.Click
  '  'Dim sDestin As String = paraPageBase.PAG_OrdKits.ToString()
  '  'MyBase.PageDirect(sDestin, 0, 0)
  '  Dim sCS As String
  '  Dim sFile As String
  '  Dim LogLib As New Log

  '  sCS = GetSelectCS(cboCVID.SelectedItem.Value)
  '  sFile = "PDFs/CSs/" & sCS & ".pdf"
  '  If (LogLib.FileExists(Server.MapPath(sFile))) Then
  '    'Response.Redirect(sFile, False)
  '    Response.Write("<script language='javascript'> window.open('" & sFile & "','Preview','location=0,resizable=1,height=600,width=600');window.focus();</script>")
  '  Else
  '    MyBase.PageMessage = "No preview available for the selected cover sheet..."
  '  End If

  '  LogLib = Nothing
  'End Sub

  'Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
  '  'Dim sDestin As String = paraPageBase.PAG_OrdKits.ToString()
  '  'MyBase.PageDirect(sDestin, 0, 0)
  '  'False here may help with lost session vars - states to not end request early
  '  Response.Redirect(MyBase.CurrentUser.PreviousPage, False)
  'End Sub

  'Private Sub cmdNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  '  Dim sDestin As String = paraPageBase.PAG_OrdRequest.ToString()

  '  SaveData()
  '  If (txtSubmit.Text = "Y") Then
  '    sDestin = paraPageBase.PAG_OrdSubmit.ToString()
  '  End If
  '  MyBase.PageDirect(sDestin, 0, 0)
  'End Sub
  Private Overloads Sub cmdNext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdNext.Click
    Dim sDestin As String = paraPageBase.PAG_OrdItemSelect.ToString()

    If ValidData() Then
      SaveData()
      'If (txtSubmit.Text = "Y") Then
            'If (txtSubmit.Value = "Y") Then
            sDestin = paraPageBase.PAG_OrdSubmit.ToString()
            'End If

            '  Response.Write("sDestin" & sDestin & "<BR>")

            MyBase.PageDirect(sDestin, 0, 0)
        End If
  End Sub

  'Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  '  Dim sDestin As String = paraPageBase.PAG_OrdSubmit.ToString()

  '  SaveData()
  '  MyBase.PageDirect(sDestin, 0, 0)
  'End Sub

  Private Sub LoadCSLink()
    Dim lID As Long
    Dim sCS As String
    Dim sFile As String
    Dim LogLib As New Log

    lID = cboCVID.SelectedItem.Value
    sCS = GetSelectCS(cboCVID.SelectedItem.Value)
    sFile = "PDFs/CSs/" & sCS & ".pdf"
    If (LogLib.FileExists(Server.MapPath(sFile))) Then
      'lkCV.Text = "<image=" & sFile & ">"
      lkCS.NavigateUrl = sFile
      'lkCS.NavigateUrl = "PDFVwr.aspx?ID=" & lID & "&TP=CS"
      lkCS.Target = "_blank"
    Else
      MyBase.PageMessage = "No preview available for the selected cover sheet..."
      lkCS.Text = ""
      lkCS.NavigateUrl = ""
    End If

    LogLib = Nothing
  End Sub

  Private Sub cboCVID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCVID.SelectedIndexChanged
    LoadCSLink()
  End Sub


  '<asp:imagebutton id="cmdCS" ToolTip="Preview Sample" Visible="False" ImageURL="images/btnPreview.png"
  '														Runat="server"></asp:imagebutton>

  '									<td><asp:textbox id="txtSubmit" runat="server" text="" width="0"></asp:textbox></td>
    Protected Sub cmdEditBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdEditBack.Click


       
        CurrentOrder.CoverSheetID = cboCVID.SelectedValue
        CurrentOrder.CoverSheetDesc = cboCVID.SelectedItem.Text
        CurrentOrder.CoverSheetText = txtCVText.Text



        'save custom fields
        Dim rcd As System.Data.DataTable
        Dim indx As Integer = 0
        Dim iMaxIndx As Integer
        Dim indx2 As Integer = 0



        rcd = MyBase.CurrentOrder.CstCart
        iMaxIndx = rcd.Rows.Count


        Do While indx < iMaxIndx
            If (rcd.Rows(indx).Item("FieldType") = MyBase.MyData.CST_DET) Then

                indx2 = 0
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
End Class
