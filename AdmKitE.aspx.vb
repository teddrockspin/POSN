''''''''''''''''''''
'mw - 09-14-2009
'mw - 05-18-2009
'mw - 02-04-2009
'mw - 01-14-2008
'mw - 05-17-2008
'mw - 04-25-2007
'mw - 03-30-2007
'mw - 02-02-2007
''''''''''''''''''''


Imports System.Text

Partial Class AdmKitE
  Inherits paraPageBase


  Private mlID As String
  Protected WithEvents txtSubmit As System.Web.UI.WebControls.TextBox
  Protected WithEvents lblQty As System.Web.UI.WebControls.Label
  Private mbIsBase As Boolean


#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

  End Sub
  Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button

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
    PageTitle = "Kit Edit"
    'MyBase.Page_Load(sender, e)
    MyBase.PageMessage = ""

    If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
      MyBase.PageDirect()
      'ElseIf Not ((MyBase.CurrentUser.CanViewKits = True) Or _
      '            (MyBase.CurrentUser.CanEditKits = True)) Then
    ElseIf Not (MyBase.CurrentUser.CanViewKits = True) Then
      MyBase.PageDirect()
    End If

    If (Page.IsPostBack = False) Then
      MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri

      ''mw - 01-31-2009
      'Dim pScript As New StringBuilder
      'pScript.Append("<script>" & _
      '"function CheckSubmit () " & _
      '"{ " & _
      '"  if (confirm('Do you wish to add this kit to the current order at this time?') == true)" & _
      '"  {  document.frmAdmKitE.txtSubmit.value='Y';}" & _
      '"  else " & _
      '"  {  document.frmAdmKitE.txtSubmit.value='N';}" & _
      '"  return true;" & _
      '"} " & _
      '"</script>")
      'Me.RegisterStartupScript("Startup", pScript.ToString)
      'cmdSave.Attributes.Add("onclick", "return CheckSubmit(); ")
      ''''pScript.Append("<script language=""vbscript"" type=""text/vbscript"">" & _
      '''' "function CheckSubmit () as boolean " & _
      '''' "begin " & _
      '''' "  if (msgbox(""Do you wish to add this kit to the current order at this time?"",vbyesno,""Add to Order"") = 6) then " & _
      '''' "    document.frmAdmKitE.txtSubmit.value=""Y"" " & _
      '''' "  else " & _
      '''' "   document.frmAdmKitE.txtSubmit.value=""N"" " & _
      '''' "  return true " & _
      '''' "end function " & _
      '''' "</script>")
      ''''Me.RegisterStartupScript("Startup", pScript.ToString)
      ''''cmdSave.Attributes.Add("onclick", "return CheckSubmit() ")

    End If 'End PostBack
  End Sub

  Private Function CurrentCategoryID() As Long
    Dim lID As Long = 0

    If Not (cboCat.SelectedItem Is Nothing) Then
      lID = cboCat.SelectedItem.Value()
    End If

    Return lID
  End Function

  Private Function ShowControls()
    Dim bLOwn As Boolean = False
    Dim bEdit As Boolean = False

    bLOwn = (MyBase.CurrentKit.AccessCodeID = 0) Or _
            (MyBase.CurrentKit.AccessCodeID = MyBase.CurrentUser.AccessCodeID)
    'mw - 01-25-2009
    'bEdit = ((MyBase.CurrentUser.KitEditPermissions = MyBase.MyData.PERM_LOCAL) And bLOwn) Or _
    '        (MyBase.CurrentUser.KitEditPermissions = MyBase.MyData.PERM_GLOBAL) Or _
    bEdit = ((MyBase.CurrentUser.KitEditPermissions = MyBase.MyData.PERM_LOCAL) And bLOwn) Or _
            (MyBase.CurrentUser.KitEditPermissions = MyBase.MyData.PERM_GLOBAL) Or _
            MyBase.CurrentKit.IsTemp = True
    '

    'cmdItem.Enabled = bEdit
    'cmdSave.Enabled = bEdit
    'cmdDelete.Enabled = bEdit And (MyBase.CurrentKit.ID > 0)
    cmdItem.Visible = bEdit
    cmdSave.Visible = bEdit
    cmdDelete.Visible = bEdit And (MyBase.CurrentKit.ID > 0)
    If (MyBase.CurrentUser.KitEditPermissions = MyBase.MyData.PERM_GLOBAL) Then
      chkGlobal.Enabled = True
    End If

    If MyBase.CurrentKit.IsTemp Then
      lblNoteTmp.Visible = True
      cmdDelete.Visible = False
      'cboCat.Enabled = False
      'txtRefNo.Enabled = False
      'chkGlobal.Enabled = False
      'chkEnModify.Enabled = False
      'txtNote.Enabled = False
      lblCat.Visible = False
      lblRefNo.Visible = False
      lblGlobal.Visible = False
      lblAbleToModify.Visible = False
      cboCat.Visible = False
      txtRefNo.Visible = False
      chkGlobal.Visible = False
      chkEnModify.Visible = False
      txtNote.Visible = False
    End If
  End Function

    Private Function LoadData()
        Dim lCatID As Long = 0

        mlID = Val(Request.QueryString("ED").ToString())
        'mw - 01-25-2009
        mbIsBase = Val(Request.QueryString("Base").ToString()) = 1

        cmdAdd.Attributes.Add("onclick", "return confirm('You are about to save the current kit specification and add to the current order.  Do you wish to continue?'); ")
        cmdSave.Attributes.Add("onclick", "return confirm('You are about to save the current kit specification.  Do you wish to continue?'); ")
        cmdDelete.Attributes.Add("onclick", "return confirm('You are about to delete the current kit specification.  Do you wish to continue?'); ")

        If (mlID = -1) Then
            'from List Screen - Add
            MyBase.CurrentKit.Init()
            MyBase.CurrentKit.AccessCodeID = MyBase.CurrentUser.AccessCodeID
        ElseIf (mlID = 0) Then
            'from session edit
        ElseIf (mlID > 0) Then
            'Must be Loading from Database - from List Screen - Edit)
            'mw - 01-25-2009
            'LoadKitInSession()
            LoadKitInSession(mbIsBase)
        End If
        'MyBase.CurrentKit.status = MyData.PERM_LOCAL
        'If (mybase.CurrentUser.KitEditPermissions = mybase.MyData.PERM_GLOBAL Then mybase.CurrentKit.Status = mybase.mydata.PERM_GLOBAL

        lCatID = MyBase.CurrentKit.CategoryID
        LoadList(BuildSQLCategory(), cboCat, lCatID)

        txtRefNo.Text = MyBase.CurrentKit.ReferenceNumber
        txtRefName.Text = MyBase.CurrentKit.ReferenceDescription
        If (MyBase.CurrentKit.Status = MyBase.MyData.PERM_GLOBAL) Then
            chkGlobal.Checked = True
        Else
            chkGlobal.Checked = False
        End If
        chkEnModify.Checked = MyBase.CurrentKit.MyUseAsBase
        txtNote.Text = MyBase.CurrentKit.Note
        txtAssemblyInstructions.Text = MyBase.CurrentKit.AssemblyInstructions
        LoadControls()
        ShowControls()
    End Function

  'mw - 01-25-2009
  'Private Function LoadKitInSession() As Boolean
  '  Dim cnn As OleDb.OleDbConnection
  '  Dim cmd As OleDb.OleDbCommand
  '  Dim dr As OleDb.OleDbDataReader
  '  Dim bSuccess As Boolean = False
  '  Dim sSQL As String
  '  Dim iQty As Integer = 0
  '  Dim lID As Integer = 0
  '  Dim sName As String = String.Empty
  '  Dim sDesc As String = String.Empty
  '  Dim iStatus As String = 0
  '  Dim sSeq As String = 0
  '  Dim sColor As String = String.Empty

  '  Try
  '    MyBase.CurrentKit.Clear()

  '    sSQL = BuildSQLBase()
  '    If (sSQL.Length > 0) Then
  '      MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
  '      If dr.Read() Then
  '        MyBase.CurrentKit.ID = dr("ID").ToString()
  '        MyBase.CurrentKit.CategoryID = Val(dr("CategoryID") & "".ToString())
  '        MyBase.CurrentKit.ReferenceNumber = dr("RefNo").ToString()
  '        MyBase.CurrentKit.ReferenceDescription = dr("RefName").ToString()
  '        MyBase.CurrentKit.KitType = Not (Convert.ToBoolean(dr("LocalOwn")))
  '        MyBase.CurrentKit.Status = dr("Status")
  '        MyBase.CurrentKit.AccessCodeID = dr("AccessCodeID")
  '        MyBase.CurrentKit.Note = dr("KitNote") & "".ToString()
  '      End If
  '      MyBase.MyData.ReleaseReader2(cnn, cmd, dr)

  '      sSQL = BuildSQLItems()
  '      If (sSQL.Length > 0) Then
  '        MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
  '        Do While dr.Read()
  '          iQty = dr("Qty")
  '          lID = dr("ID")
  '          sName = dr("RefNo").ToString()
  '          sDesc = dr("RefName").ToString()
  '          sSeq = dr("Seq")
  '          If (Convert.ToBoolean(dr("OnBackOrder"))) Then
  '            sColor = MyBase.MyData.COLOR_BACK
  '          ElseIf Not (Convert.ToBoolean(dr("Active"))) Then
  '            sColor = MyBase.MyData.COLOR_INACTIVE
  '          Else
  '            sColor = MyBase.MyData.COLOR_NORMAL
  '          End If
  '          MyBase.CurrentKit.ItmSave(iQty, lID, sName, sDesc, sSeq, sColor)
  '        Loop
  '        MyBase.MyData.ReleaseReader2(cnn, cmd, dr)
  '      End If

  '      bSuccess = True
  '    End If

  '  Catch ex As Exception

  '  End Try
  '  Return bSuccess
  'End Function
    Private Function LoadKitInSession(ByVal bIsBase As Boolean) As Boolean
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim dr As OleDb.OleDbDataReader
        Dim bSuccess As Boolean = False
        Dim sSQL As String
        Dim bIsTemp As Boolean = False
        Dim iQty As Integer = 0
        Dim lID As Integer = 0
        Dim sName As String = String.Empty
        Dim sDesc As String = String.Empty
        Dim iStatus As String = 0
        Dim sSeq As String = ""
        Dim sColor As String = String.Empty

        Try
            MyBase.CurrentKit.Clear()

            sSQL = BuildSQLBase()
            If (sSQL.Length > 0) Then
                MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
                If dr.Read() Then
                    MyBase.CurrentKit.Status = dr("Status")
                    MyBase.CurrentKit.CategoryID = Val(dr("CategoryID") & "".ToString())

                    bIsTemp = MyBase.CurrentKit.Status = MyData.PERM_TEMP
                    MyBase.CurrentKit.IsTemp = bIsBase Or bIsTemp

                    'If Not mbIsBase Or (mbIsBase And bIsTemp) Then
                    If Not mbIsBase Then
                        MyBase.CurrentKit.ID = dr("ID").ToString()
                        MyBase.CurrentKit.ReferenceNumber = dr("RefNo").ToString()
                        MyBase.CurrentKit.ReferenceDescription = dr("RefName").ToString()
                        'MyBase.CurrentKit.Status = dr("Status")
                        MyBase.CurrentKit.AccessCodeID = dr("AccessCodeID")
                        MyBase.CurrentKit.KitType = Not (Convert.ToBoolean(dr("LocalOwn")))
                        MyBase.CurrentKit.MyUseAsBase = (dr("BaseKit") = 1)
                        MyBase.CurrentKit.Note = dr("KitNote") & "".ToString()
                        MyBase.CurrentKit.AssemblyInstructions = dr("AssemblyInstructions").ToString()
                    End If

                    'New Temporary Kit
                    If (mbIsBase And MyBase.CurrentKit.ID = 0) Then
                        MyBase.CurrentKit.ReferenceNumber = "Tmp" & Format(Now, "MMddyyhhmmss")
                        MyBase.CurrentKit.ReferenceDescription = String.Empty
                        MyBase.CurrentKit.Status = MyData.PERM_TEMP
                        MyBase.CurrentKit.AccessCodeID = MyBase.CurrentUser.AccessCodeID
                        'mw - NOT SURE OF THIS ONE ***  NEED TO CHECK ON
                        'MyBase.CurrentKit.KitType = Not (Convert.ToBoolean(dr("LocalOwn")))
                        MyBase.CurrentKit.KitType = False
                        MyBase.CurrentKit.MyUseAsBase = False
                        '
                        MyBase.CurrentKit.Note = "Temp:  Created " & Format(Now, "MM-dd-yy hh:mm:ss") & " from " & dr("RefNo").ToString()
                    End If
                End If

                MyBase.MyData.ReleaseReader2(cnn, cmd, dr)

                sSQL = BuildSQLItems()
                If (sSQL.Length > 0) Then
                    MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
                    Do While dr.Read()
                        iQty = dr("Qty")
                        lID = dr("ID")
                        sName = dr("RefNo").ToString()
                        sDesc = dr("RefName").ToString()
                        sSeq = dr("Seq").ToString()
                        If (Convert.ToBoolean(dr("OnBackOrder"))) Then
                            sColor = MyBase.MyData.COLOR_BACK
                        ElseIf Not (Convert.ToBoolean(dr("Active"))) Then
                            sColor = MyBase.MyData.COLOR_INACTIVE
                        Else
                            sColor = MyBase.MyData.COLOR_NORMAL
                        End If
                        MyBase.CurrentKit.ItmSave(iQty, lID, sName, sDesc, sSeq, sColor)
                    Loop
                    MyBase.MyData.ReleaseReader2(cnn, cmd, dr)
                End If

                bSuccess = True
            End If

        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        End Try
        Return bSuccess
    End Function
  '
  Private Function BuildSQLBase() As String
    Dim sSQL As String = String.Empty
    Dim sSelect As String = String.Empty
    Dim sWhere As String = String.Empty
    Dim sOrder As String = String.Empty

    'mw - 08-20-2009
    'sSelect = "SELECT Customer_Kit.ID, Customer_Kit.AccessCodeID, Customer_Kit.CategoryID, Customer_Kit.ReferenceNo AS RefNo, Customer_Kit.Description AS RefName, BaseKit, Customer_Kit.Note AS KitNote, InActiveDocCnt, BackOrderDocCnt " & _
    '          ", Customer_Kit.Status, iif(Customer_Kit.AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ",True,False) AS LocalOwn " & _
    '          "FROM (Customer_Kit LEFT JOIN qryKitDocSum ON Customer_Kit.ID = qryKitDocSum.KitID) " & _
    '          "INNER JOIN Customer_AccessCode ON Customer_Kit.AccessCodeID = Customer_AccessCode.ID "
        sSelect = "SELECT Customer_Kit.ID, Customer_Kit.AccessCodeID, k.CategoryID, Customer_Kit.ReferenceNo AS RefNo, Customer_Kit.Description AS RefName, BaseKit, Customer_Kit.Note AS KitNote, Customer_Kit.AssemblyInstructions, InActiveDocCnt, BackOrderDocCnt " & _
              ", Customer_Kit.Status, iif(Customer_Kit.AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ",True,False) AS LocalOwn " & _
              "FROM ((Customer_Kit LEFT JOIN qryKitDocSum ON Customer_Kit.ID = qryKitDocSum.KitID) " & _
              "INNER JOIN Customer_AccessCode ON Customer_Kit.AccessCodeID = Customer_AccessCode.ID " & _
              ") LEFT JOIN " & _
              " (SELECT kkw.KitID, kkw.KeywordID AS CategoryID FROM Customer_Kit_Keyword kkw " & _
              "  LEFT JOIN (Customer_Keyword ckw LEFT JOIN Customer_Key ck ON ckw.KeyID = ck.ID) ON kkw.KeywordID = ckw.ID " & _
              "  WHERE(ck.CustomerID = " & MyBase.CurrentUser.CustomerID & " )" & _
              "  ) k ON k.KitID = Customer_Kit.ID "
    '
    sWhere = "(Customer_Kit.ID = " & mlID & ") "
    If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

    sSQL = sSelect + sWhere + sOrder

    Return sSQL
  End Function

  Private Function BuildSQLItems() As String
    Dim sSQL As String = String.Empty
    Dim sSelect As String = String.Empty
    Dim sWhere As String = String.Empty
    Dim sOrder As String = String.Empty

        sSelect = "SELECT Customer_Document.ID, iif(Customer_Document.Status >" & MyBase.MyData.STATUS_INAC & ",True,False) AS Active, " & _
                  "iif(Status=" & MyData.STATUS_BACK & ",True,False) AS OnBackOrder, " & _
                  "Customer_Document.ReferenceNo AS RefNo, Customer_Document.Description AS RefName, " & _
                  "Customer_Kit_Document.Qty, Customer_Kit_Document.Seq AS Seq " & _
                  "FROM Customer_Kit_Document INNER JOIN Customer_Document ON Customer_Kit_Document.DocumentID = Customer_Document.ID "

    sWhere = "(Customer_Kit_Document.KitID = " & mlID & ") "
    If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

    sOrder = "ORDER BY Customer_Kit_Document.Seq, Customer_Document.ReferenceNo "

    sSQL = sSelect + sWhere + sOrder

    Return sSQL
  End Function

  Private Function BuildSQLCategory(Optional ByVal sCatIDs As String = "") As String
    Dim sSQL As String = String.Empty
    Dim sSelect As String = String.Empty
    Dim sWhere As String = String.Empty
    Dim sOrder As String = String.Empty

    'mw - 08-20-2009
    ''sSelect = "SELECT [Customer_Category].ID AS ID,  [Customer_Category].Name AS RefName, [Customer_Category].Description AS RefDescr " & _
    ''          "FROM [Customer_Category]"
    'sSelect = "SELECT [Customer_Category].ID AS ID,  [Customer_Category].Name AS ListText " & _
    '          "FROM [Customer_Category]"
    'sWhere = "([Customer_Category].CustomerID = " & MyBase.CurrentUser.CustomerID & ") " & _
    '     "AND ([Customer_Category].Status > " & MyBase.MyData.STATUS_INAC & ") "
    'If (Len(sCatIDs) > 0) Then
    '  sWhere = sWhere & " AND (Customer_Category.ParentID IN (" & sCatIDs & ")) "
    'Else
    '  sWhere = sWhere & " AND (ParentID = 0)"
    'End If
    'If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

    'sOrder = " ORDER BY [Customer_Category].Description "
    sSelect = "SELECT ck.[Name] AS Lbl, ckw.ID, ckw.Description AS ListText FROM Customer_Key ck LEFT JOIN Customer_Keyword ckw ON ck.ID = ckw.KeyID "
    sWhere = "(ck.CustomerID = " & MyBase.CurrentUser.CustomerID & ") " & _
             " AND (ck.Seq = 1) "
    If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere
    sOrder = " ORDER BY ckw.Description "
    '
    sSQL = sSelect + sWhere + sOrder

    Return sSQL
  End Function

  Private Function LoadList(ByVal sSQL As String, ByRef cbo As DropDownList, ByVal lID As Long)
    Dim cnn As OleDb.OleDbConnection
    Dim cmd As OleDb.OleDbCommand
    Dim dr As OleDb.OleDbDataReader
    Dim bSuccess As Boolean = False
    Dim sID As String
    Dim sText As String
    Dim sLbl As String

    sLbl = lblCat.Text
    cbo.Items.Clear()

    cbo.Items.Add(New ListItem("None", 0))
    cbo.Items(cbo.Items.Count - 1).Attributes.Add("style", "color:" & MyBase.MyData.COLOR_NORMAL)

    If (sSQL.Length > 0) Then
            MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)

            If dr IsNot Nothing AndAlso dr.FieldCount > 0 Then
                Do While dr.Read()
                    sLbl = dr("Lbl").ToString()
                    sID = dr("ID").ToString()
                    sText = Trim(dr("ListText"))

                    cbo.Items.Add(New ListItem(sText, sID))
                    cbo.Items(cbo.Items.Count - 1).Attributes.Add("style", "color:" & MyBase.MyData.COLOR_NORMAL)

                    If (sID = lID) Then
                        cbo.Items(cbo.Items.Count - 1).Selected = True
                    End If
                Loop
            End If

      MyBase.MyData.ReleaseReader2(cnn, cmd, dr)

      'SELECT
      Dim indx As Integer
      Dim iMax As Integer

      iMax = cbo.Items.Count - 1
      For indx = 0 To iMax
        If (cbo.Items(indx).Value = lID) Then
          cbo.SelectedIndex = indx
        End If
      Next

      lblCat.Text = sLbl & ":"

      bSuccess = True
    End If

    Return (bSuccess)
  End Function

    Private Function LoadControls() As Boolean
        Dim tRow As TableRow
        Dim tCell As TableCell
        Dim lLabel As New Label
        Dim tText As New TextBox
        Dim sSeqID As String
        Dim sQtyID As String
        Dim vReq As RequiredFieldValidator
        Dim vCmp As CompareValidator

        Dim rcd As System.Data.DataTable
        Dim indx As Integer
        Dim iMaxIndx As Integer
        Dim bSuccess As Boolean = False

        Dim sID As String
        Dim sName As String
        Dim sDesc As String
        Dim sColor As String
        Dim iQty As Integer
        Dim sSeq As String

        tRow = New TableRow

        'Space (ID)
        tCell = New TableCell
        tCell.Visible = False
        tRow.Cells.Add(tCell)
        'Label
        tCell = New TableCell
        tCell.HorizontalAlign = HorizontalAlign.Left
        lLabel = New Label
        lLabel.CssClass = "lblShort"
        lLabel.Font.Bold = True
        lLabel.ID = "lblSeq"
        lLabel.Text = "Sequence"
        tCell.Controls.Add(lLabel)
        tRow.Cells.Add(tCell)
        ''Space
        'tCell = New TableCell
        'tCell.Visible = False
        'tRow.Cells.Add(tCell)
        'Label
        tCell = New TableCell
        tCell.HorizontalAlign = HorizontalAlign.Left
        lLabel = New Label
        lLabel.CssClass = "lblShort"
        lLabel.Font.Bold = True
        lLabel.ID = "lblQty"
        lLabel.Text = "Quantity"
        tCell.Controls.Add(lLabel)
        tRow.Cells.Add(tCell)
        ''Space
        'tCell = New TableCell
        'tCell.Text = "&nbsp"
        'tRow.Cells.Add(tCell)
        'Label
        tCell = New TableCell
        tCell.HorizontalAlign = HorizontalAlign.Left
        tCell.ColumnSpan = 4
        lLabel = New Label
        lLabel.CssClass = "lblShort"
        lLabel.Font.Bold = True
        lLabel.ID = "lblDesc"
        lLabel.Text = "Description"
        tCell.Controls.Add(lLabel)
        tRow.Cells.Add(tCell)

        tblQty.Rows.Add(tRow)

        rcd = MyBase.CurrentKit.ItmCart
        iMaxIndx = rcd.Rows.Count
        indx = 0
        Do While indx < iMaxIndx
            With rcd.Rows(indx)
                sID = .Item("ItmID").ToString()
                sName = .Item("ItmName").ToString()
                sDesc = .Item("ItmDesc").ToString()
                sColor = .Item("ItmColor").ToString()
                iQty = Convert.ToInt16(.Item("ItmQty").ToString())
                sSeq = .Item("ItmSeq").ToString()
            End With
            tRow = New TableRow

            'ID
            tCell = New TableCell
            tCell.Visible = False
            tCell.Text = sID
            tRow.Cells.Add(tCell)
            'Sequence
            tCell = New TableCell
            tCell.HorizontalAlign = HorizontalAlign.Center
            sSeqID = "txtS" & indx
            tText = New TextBox
            tText.CssClass = "txtNumber"
            tText.ID = sSeqID
            tText.Text = sSeq
            tText.ToolTip = "Enter number for sequencing - C represents a Container Item - 0 represents no sequence"
            tText.MaxLength = 6
            tCell.Controls.Add(tText)
            'Validator Req
            vReq = New RequiredFieldValidator
            vReq.ID = "vS" & indx
            vReq.ControlToValidate = sSeqID
            vReq.ErrorMessage = "*"
            tCell.Controls.Add(vReq)
            ''Validator Cmp
            'vCmp = New CompareValidator
            'vCmp.ID = "vcS" & indx
            'vCmp.ControlToValidate = sSeqNoID
            'vCmp.Operator = ValidationCompareOperator.DataTypeCheck
            'vCmp.Type = ValidationDataType.Integer
            'vCmp.ErrorMessage = "*"
            'tCell.Controls.Add(vCmp)
            tRow.Cells.Add(tCell)
            ''Space
            'tCell = New TableCell
            'tCell.Text = "&nbsp"
            'tRow.Cells.Add(tCell)
            'Quantity
            tCell = New TableCell
            tCell.HorizontalAlign = HorizontalAlign.Center
            sQtyID = "txtQ" & indx
            tText = New TextBox
            tText.CssClass = "txtNumber"
            tText.ID = sQtyID
            tText.Text = iQty
            tCell.Controls.Add(tText)
            'Validator Req
            vReq = New RequiredFieldValidator
            vReq.ID = "vQ" & indx
            vReq.ControlToValidate = sQtyID
            vReq.ErrorMessage = "*"
            tCell.Controls.Add(vReq)
            'Validator Cmp
            vCmp = New CompareValidator
            vCmp.ID = "vcQ" & indx
            vCmp.ControlToValidate = sQtyID
            vCmp.[Operator] = ValidationCompareOperator.DataTypeCheck
            vCmp.Type = ValidationDataType.Integer
            vCmp.ErrorMessage = "*"
            tCell.Controls.Add(vCmp)
            tRow.Cells.Add(tCell)
            ''Space
            'tCell = New TableCell
            'tCell.Text = "&nbsp"
            'tRow.Cells.Add(tCell)
            'Description
            tCell = New TableCell
            tCell.HorizontalAlign = HorizontalAlign.Left
            tCell.ColumnSpan = 4
            lLabel = New Label
            If (sColor = MyBase.MyData.COLOR_BACK) Then
                lLabel.CssClass = "lblXLongBack"
            ElseIf (sColor = MyBase.MyData.COLOR_INACTIVE) Then
                lLabel.CssClass = "lblXLongInActive"
            Else
                lLabel.CssClass = "lblXLong"
            End If
            lLabel.ID = "lbl" & indx
            lLabel.Text = sName & " : " & sDesc
            tCell.Controls.Add(lLabel)
            tRow.Cells.Add(tCell)

            tblQty.Rows.Add(tRow)

            indx = indx + 1
        Loop

        bSuccess = True
        Return (bSuccess)
    End Function

    Private Function SaveData() As Boolean
        Dim rcd As System.Data.DataTable
        Dim indx As Integer
        Dim iMaxIndx As Integer

        Dim lID As Int32
        Dim sName As String
        Dim sDesc As String
        Dim iQty As Integer
        Dim sSeq As String
        Dim sTmp As String
        Dim ipos As Integer
        Dim tText As New TextBox

        Dim bSuccess As Boolean = False

        Try
            MyBase.CurrentKit.CategoryID = CurrentCategoryID()
            MyBase.CurrentKit.ReferenceNumber = txtRefNo.Text.ToString.Trim
            MyBase.CurrentKit.ReferenceDescription = txtRefName.Text.ToString.Trim
            If (MyBase.CurrentKit.IsTemp = True) Then
                MyBase.CurrentKit.Status = MyData.PERM_TEMP
            ElseIf (MyBase.CurrentUser.KitEditPermissions = MyBase.MyData.PERM_GLOBAL) Then
                MyBase.CurrentKit.Status = IIf(chkGlobal.Checked = True, MyData.PERM_GLOBAL, MyData.PERM_LOCAL)
            Else
                MyBase.CurrentKit.Status = MyData.PERM_LOCAL
            End If
            MyBase.CurrentKit.MyUseAsBase = chkEnModify.Checked
            MyBase.CurrentKit.Note = txtNote.Text.Trim
            MyBase.CurrentKit.AssemblyInstructions = txtAssemblyInstructions.Text.Trim

            'Note:  Could change saving...
            ' to completely remove DataTable.Rows and add all new DataTable.Rows
            iMaxIndx = tblQty.Rows.Count
            indx = 0 : indx = indx + 1 'Account for adding Header row
            Do While indx < iMaxIndx
                lID = 0
                sName = ""
                sDesc = ""
                iQty = 0

                With tblQty.Rows(indx)
                    lID = Val(.Cells(0).Text)

                    tText = .Cells(1).Controls(0)
                    sSeq = tText.Text.ToString.Trim
                    tText = .Cells(2).Controls(0)
                    iQty = Val(tText.Text)

                    MyBase.CurrentKit.ItmSave(iQty, lID, sName, sDesc, sSeq)
                End With
                indx = indx + 1
            Loop
            tText.Dispose()
            tText = Nothing

            MyBase.SessionStore("AdmKitE")

            bSuccess = True

        Catch ex As Exception
            MyBase.PageMessage = "Error attempting to Save Data..."
        End Try

        Return (bSuccess)
    End Function

  Private Function ValidQuantity() As Boolean
    Dim bValid As Boolean : bValid = False

    bValid = ((Convert.ToInt16(Val(txtQty.Text)) <= MyBase.CurrentOrder.MaxQtyPerLineItem)) Or _
             ((Convert.ToInt16(Val(txtQty.Text)) > MyBase.CurrentOrder.MaxQtyPerLineItem) And MyBase.CurrentUser.CanExtendQtyLI)
    If Not bValid Then
      MyBase.PageMessage = "Quantity to add exceeds maximum limit set at " & MyBase.CurrentOrder.MaxQtyPerLineItem
    End If

    Return bValid
  End Function


  'Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  '  Dim sDestin As String = paraPageBase.PAG_AdmKitList.ToString()
  '  MyBase.CurrentKit.Clear()
  '  MyBase.PageDirect(sDestin, 0, 0)
  '  'False here may help with lost session vars - states to not end request early
  '  'Response.Redirect(MyBase.CurrentUser.PreviousPage, False)
  'End Sub
  Private Overloads Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click
    'mw - 01-30-2009
    'Dim sDestin As String = paraPageBase.PAG_AdmKitList.ToString()
    Dim sDestin As String

    'If MyBase.CurrentKit.ID > 0 Then
    '  sDestin = paraPageBase.PAG_AdmKitList.ToString()
    'ElseIf (MyBase.CurrentKit.ID = 0 And MyBase.CurrentKit.IsTemp = True) Then
    '  sDestin = paraPageBase.PAG_OrdKitSelect.ToString()
    'End If
    If (MyBase.CurrentKit.ID = 0 And MyBase.CurrentKit.IsTemp = True) Then
      sDestin = paraPageBase.PAG_OrdKitSelect.ToString()
    Else
      sDestin = paraPageBase.PAG_AdmKitList.ToString()
    End If
    '
    MyBase.CurrentKit.Clear()
    MyBase.PageDirect(sDestin, 0, 0)
  End Sub

  'Private Sub cmdSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  '  Dim sDestin As String = paraPageBase.PAG_AdmKitItemSelect.ToString()

  '  If (SaveData()) Then
  '    MyBase.PageDirect(sDestin, 0, 0)
  '  ElseIf (MyBase.PageMessage.Length = 0) Then
  '    MyBase.PageMessage = "Record NOT updated..."
  '  End If
  'End Sub
  Private Sub cmdItem_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdItem.Click
    Dim sDestin As String = paraPageBase.PAG_AdmKitItemSelect.ToString()

    If (SaveData()) Then
      MyBase.PageDirect(sDestin, 0, 0)
    ElseIf (MyBase.PageMessage.Length = 0) Then
      MyBase.PageMessage = "Record NOT updated..."
    End If
  End Sub

  'Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  '  Dim sDestin As String = paraPageBase.PAG_AdmKitList.ToString()

  '  If (MyBase.CurrentKit.Delete) Then
  '    ' MyBase.PageMessage = "Record deleted!"
  '    MyBase.CurrentKit.Clear()
  '    MyBase.PageDirect(sDestin, 0, 0)
  '  ElseIf (MyBase.PageMessage.Length = 0) Then
  '    MyBase.PageMessage = "Unable to Delete..."
  '  End If
  'End Sub
  Private Overloads Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdDelete.Click
    Dim sDestin As String = paraPageBase.PAG_AdmKitList.ToString()

    If (MyBase.CurrentKit.Delete) Then
      ' MyBase.PageMessage = "Record deleted!"
      MyBase.CurrentKit.Clear()
      MyBase.PageDirect(sDestin, 0, 0)
    ElseIf (MyBase.PageMessage.Length = 0) Then
      MyBase.PageMessage = "Unable to Delete..."
    End If
  End Sub

  'Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  '  Dim sDestin As String = paraPageBase.PAG_AdmKitList.ToString()

  '  If (SaveData()) Then
  '    If (MyBase.CurrentKit.Store()) Then
  '      'MyBase.PageMessage = "Record updated!"
  '      MyBase.PageDirect(sDestin, 0, 0)
  '    Else
  '      MyBase.PageMessage = "Record NOT updated..."
  '    End If
  '  ElseIf (MyBase.PageMessage.Length = 0) Then
  '    MyBase.PageMessage = "Unable to Save Data..."
  '  End If
  'End Sub

  'Private Function SaveKit() As Boolean
  '  Dim sDestin As String
  '  Dim iQty As Int16
  '  Dim lID As Int32
  '  Dim sName As String
  '  Dim sDesc As String

  '  If (MyBase.CurrentKit.ID = 0 And MyBase.CurrentKit.IsTemp = True) Then
  '    sDestin = paraPageBase.PAG_OrdKitSelect.ToString()
  '  Else
  '    sDestin = paraPageBase.PAG_AdmKitList.ToString()
  '  End If

  '  If (SaveData()) Then
  '    lID = MyBase.CurrentKit.Store()
  '    If (lID > 0) Then
  '      '<asp:textbox id="txtSubmit" runat="server" width="0" text=""></asp:textbox>
  '      'If (txtSubmit.Text = "Y") Then
  '      '  'Save Kit to Order
  '      '  iQty = 1
  '      '  sName = MyBase.CurrentKit.ReferenceNumber
  '      '  sDesc = MyBase.CurrentKit.ReferenceDescription
  '      '  MyBase.CurrentOrder.KitSave(1, lID, sName, sDesc)
  '      '  MyBase.SessionStore("AdmKitE")
  '      'End If

  '      'MyBase.PageMessage = "Record updated!"
  '      MyBase.PageDirect(sDestin, 0, 0)
  '    Else
  '      MyBase.PageMessage = "Record NOT updated..."
  '    End If
  '  ElseIf (MyBase.PageMessage.Length = 0) Then
  '    MyBase.PageMessage = "Unable to Save Data..."
  '  End If
  'End Function

  Private Sub SaveClose(ByVal bAdd As Boolean)
    Dim sDestin As String
    Dim iQty As Int16
    Dim lID As Int32
    Dim sName As String
    Dim sDesc As String

    If (MyBase.CurrentKit.ID = 0 And MyBase.CurrentKit.IsTemp = True) Then
      sDestin = paraPageBase.PAG_OrdKitSelect.ToString()
    Else
      sDestin = paraPageBase.PAG_AdmKitList.ToString()
    End If

    If (SaveData()) Then
      lID = MyBase.CurrentKit.Store()
      If (lID > 0) Then
        If bAdd Then
          '<asp:textbox id="txtSubmit" runat="server" width="0" text=""></asp:textbox>
          'If (txtSubmit.Text = "Y") Then
          'Save Kit to Order
          If ValidQuantity() Then
            iQty = txtQty.Text.ToString
          Else
            iQty = 1
          End If
          sName = MyBase.CurrentKit.ReferenceNumber
          sDesc = MyBase.CurrentKit.ReferenceDescription
          MyBase.CurrentOrder.KitSave(iQty, lID, sName, sDesc)
          MyBase.SessionStore("AdmKitE")
        End If

        'MyBase.PageMessage = "Record updated!"
        MyBase.PageDirect(sDestin, 0, 0)
        MyBase.PageDirect(sDestin, 0, 0)
      Else
        MyBase.PageMessage = "Record NOT updated..."
      End If
    ElseIf (MyBase.PageMessage.Length = 0) Then
      MyBase.PageMessage = "Unable to Save Data..."
    End If
  End Sub

  Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdSave.Click
    SaveClose(False)
  End Sub

  Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdAdd.Click
    SaveClose(True)
  End Sub
End Class
