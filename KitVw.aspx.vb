''''''''''''''''''''
'mw - 04-01-2009
'mw - 03-17-2007
'mw - 01-25-2007
''''''''''''''''''''


Imports System.Text

Partial Class KitVw

  Inherits paraPageBase

  Private mlID As String

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

  Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    PageTitle = "Kit View"
    'MyBase.Page_Load(sender, e)
    MyBase.PageMessage = ""

    If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
      MyBase.PageDirect()
    ElseIf Not (MyBase.CurrentUser.CanViewKits = True) Then
      MyBase.PageDirect()
    End If

        If (Page.IsPostBack = False) Then
            GetShowStockSetting()
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri

            mlID = Val(Request.QueryString("ID").ToString())

            'Dim sScript As StringBuilder = New StringBuilder(200)
            'sScript.Append("<script language='javascript'>" & _
            '               "   function Closer()" & _
            '               "   {" & _
            '               "     window.close();" & _
            '               "     return true;" & _
            '               "  }" & _
            '               "</script>")
            'Me.RegisterStartupScript("Startup", sScript.ToString)
            'cmdBack.Attributes.Add("onclick", "Closer();")

            LoadData()

            'Required - if decide to open in separate window
            'Me.RegisterStartupScript("Startup", "<script language='javascript'>window.focus();</script>")
            '
        End If 'End PostBack
  End Sub

  Private Sub LoadData()
    If (mlID = 0) Then
      MyBase.CurrentKit.Init()
    ElseIf (mlID > 0) Then
      'Must be Loading from Database
      LoadKitInSession()
    End If

    lblRefNo.Text = MyBase.CurrentKit.ReferenceNumber
    lblRefName.Text = MyBase.CurrentKit.ReferenceDescription
    LoadControls()
  End Sub

    Private Function LoadKitInSession() As Boolean
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim dr As OleDb.OleDbDataReader
        Dim bSuccess As Boolean = False
        Dim sSQL As String
        Dim iQty As Integer = 0
        Dim lID As Integer = 0
        Dim sName As String = String.Empty
        Dim sDesc As String = String.Empty
        Dim iStatus As String = 0
        Dim sSeq As String = 0
        Dim sColor As String = String.Empty


        Try
            MyBase.CurrentKit.Clear()

            sSQL = BuildSQLBase()
            If (sSQL.Length > 0) Then
                MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
                If dr.Read() Then
                    MyBase.CurrentKit.ReferenceNumber = dr("RefNo").ToString()
                    MyBase.CurrentKit.ReferenceDescription = dr("RefName").ToString()
                    MyBase.CurrentKit.KitType = Not (Convert.ToBoolean(dr("LocalOwn")))
                    MyBase.CurrentKit.Status = dr("Status")
                    MyBase.CurrentKit.AccessCodeID = dr("AccessCodeID")
                End If
                MyBase.MyData.ReleaseReader2(cnn, cmd, dr)

                sSQL = BuildSQLItems()
                If (sSQL.Length > 0) Then
                    MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
                    Do While dr.Read()
                        iQty = dr("Qty")
                        lID = dr("ID")
                        sName = dr("RefNo").ToString() ' & " stock: " & getShowStock(dr("ID"))
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

        End Try
        Return bSuccess
    End Function

  Private Function BuildSQLBase() As String
    Dim sSQL As String = String.Empty
    Dim sSelect As String = String.Empty
    Dim sWhere As String = String.Empty
    Dim sOrder As String = String.Empty

    sSelect = "SELECT Customer_Kit.ID, Customer_Kit.AccessCodeID, Customer_Kit.ReferenceNo AS RefNo, Customer_Kit.Description AS RefName, InActiveDocCnt, BackOrderDocCnt " & _
              ", Customer_Kit.Status, iif(Customer_Kit.AccessCodeID = " & MyBase.CurrentUser.AccessCodeID & ",True,False) AS LocalOwn " & _
              "FROM (Customer_Kit LEFT JOIN qryKitDocSum ON Customer_Kit.ID = qryKitDocSum.KitID) " & _
              "INNER JOIN Customer_AccessCode ON Customer_Kit.AccessCodeID = Customer_AccessCode.ID "

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
                  "Customer_Kit_Document.Qty, Customer_Kit_Document.Seq " & _
                  "FROM Customer_Kit_Document INNER JOIN Customer_Document ON Customer_Kit_Document.DocumentID = Customer_Document.ID "

    sWhere = "(Customer_Kit_Document.KitID = " & mlID & ") "
    If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

    sOrder = "ORDER BY Customer_Kit_Document.Seq, Customer_Document.ReferenceNo "

    sSQL = sSelect + sWhere + sOrder

    Return sSQL
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
        Dim blnShowStock As Boolean = hfShowStock.Value
        Dim pd As New paraData
        Dim sStock As String = ""

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

        If blnShowStock Then
            tCell = New TableCell
            tCell.HorizontalAlign = HorizontalAlign.Left
            tCell.ColumnSpan = 4
            lLabel = New Label
            lLabel.CssClass = "lblShort"
            lLabel.Font.Bold = True
            lLabel.ID = "lblStock"
            lLabel.Text = "Stock Qty"
            tCell.Controls.Add(lLabel)
            tRow.Cells.Add(tCell)
        End If


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
      lLabel = New Label
      lLabel.CssClass = "lblShort"
      lLabel.ID = sSeqID
      lLabel.Text = sSeq
      'tText.ToolTip = "Enter number for sequencing - C represents a Container Item - 0 represents no sequence"
      tCell.Controls.Add(lLabel)
      tRow.Cells.Add(tCell)
      'Quantity
      tCell = New TableCell
      tCell.HorizontalAlign = HorizontalAlign.Center
      sQtyID = "txtQ" & indx
      lLabel = New Label
      lLabel.CssClass = "lblShort"
      lLabel.ID = sQtyID
      lLabel.Text = iQty
      tCell.Controls.Add(lLabel)
      tRow.Cells.Add(tCell)
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

            If blnShowStock Then
                sStock = pd.getShowStock(sID, True)
                tCell = New TableCell
                tCell.HorizontalAlign = HorizontalAlign.Center
                lLabel = New Label
                lLabel.CssClass = "lblShort"
                lLabel.Text = sStock
                tCell.Controls.Add(lLabel)
                tRow.Cells.Add(tCell)
            End If

      indx = indx + 1
    Loop

    bSuccess = True
    Return (bSuccess)
  End Function

  'Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  '  'False here may help with lost session vars - states to not end request early
  '  Response.Redirect(MyBase.CurrentUser.PreviousPage, False)
  'End Sub
  Private Overloads Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click
    Response.Redirect(MyBase.CurrentUser.PreviousPage, False)
  End Sub
    Sub GetShowStockSetting()
        Dim ta As New dsAccessCodesTableAdapters.Customer_AccessCode1TableAdapter
        Dim dr As dsAccessCodes.Customer_AccessCode1Row
        dr = ta.GetDataByAccessCodeID(CurrentUser.AccessCodeID).Rows(0)
        hfShowStock.Value = dr.ShowStock
    End Sub
  'Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdClose.Click
  '  Response.Write("<script>window.close();</script>")
    'End Sub

    'Function getShowStock(intItemID As Integer) As String
    '    Dim strTmp As String = ""

    '    Dim pd As New paraData
    '    strTmp = pd.getShowStock(intItemID, True)

    '    Return strTmp
    'End Function
End Class
