''''''''''''''''''''
'mw - 08-27-2008
'mw - 05-24-2008
'mw - 08-18-2007
'mw - 03-17-2007
''''''''''''''''''''


Partial Class OrdSveP
  Inherits paraPageBase

#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

  End Sub
  Protected WithEvents frmOrdPrv As System.Web.UI.HtmlControls.HtmlForm
  Protected WithEvents lblTitle As System.Web.UI.WebControls.Label

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
        PageTitle = "Order Preview"
        'MyBase.Page_Load(sender, e)

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri

            LoadData()
        End If 'End PostBack
    End Sub

  Private Function LoadData()
    Dim sMsg As String = String.Empty

    'Get Review Data her...
    LoadControls()
  End Function

  Private Function LoadControls() As Boolean
    Dim rcd As System.Data.DataTable
    Dim indx As Integer
    Dim iMaxIndx As Integer
    Dim bRequiresLI As Boolean = False
    Dim bRequiresOR As Boolean = False
    Dim bSuccess As Boolean = False

    With MyBase.CurrentOrder
      AddHeaderRow("Order Preview")
      AddDetailRow("", "")
      AddDetailRow("Requestor Title:", .RequestorTitle)
      AddDetailRow("Requestor First Name:", .RequestorFirstName)
      AddDetailRow("Requestor Last Name:", .RequestorLastName)
      AddDetailRow("Requestor E-mail:", .RequestorEmail)
      AddDetailRow("", "")
      AddDetailRow("Shipping Contact First Name:", .ShipContactFirstName)
      AddDetailRow("Shipping Contact Last Name:", .ShipContactLastName)
      AddDetailRow("Shipping Contact Company:", .ShipCompany)
      AddDetailRow("Shipping Address:", .ShipAddress1)
      AddDetailRow("", .ShipAddress2)
      AddDetailRow("", .ShipCity & ", " & .ShipState & " " & .ShipZip & "  " & .ShipCountry)
      If (.CountryExclusionCheck(MyBase.MyData, MyBase.CurrentUser.CustomerID, .ShipCountry) = True) Then AddNoteRow("Shipping country requires authorization.")
      AddDetailRow("Preferred Shipping Method:", .ShipPrefDesc)
      If (.RequiresAuthorShip) Then AddNoteRow("Shipping method requires authorization.")
      AddDetailRow("Notes:", .ShipNote, 1)
      AddDetailRow("", "")
      AddDetailRow("Cover Sheet:", .CoverSheetDesc)
      AddDetailRow("Cover Sheet Detail:", .CoverSheetText, 2)
      AddDetailRow("", "")
      rcd = MyBase.CurrentOrder.CstCart
      iMaxIndx = rcd.Rows.Count
      indx = 0
      Do While indx < iMaxIndx
        AddDetailRow(rcd.Rows(indx).Item("FieldName").ToString() & ":", rcd.Rows(indx).Item("FieldValue").ToString())
        indx = indx + 1
      Loop
      AddDetailRow("", "")
      AddDetailRow("", "")
      AddSubHeaderRow("Details")
      rcd = MyBase.CurrentOrder.ItmCart
      iMaxIndx = rcd.Rows.Count
      indx = 0
      Do While indx < iMaxIndx
        AddDetailRow(rcd.Rows(indx).Item("ItmQty").ToString() & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", rcd.Rows(indx).Item("ItmName").ToString() & " - " & rcd.Rows(indx).Item("ItmDesc").ToString())
        indx = indx + 1
      Loop
      AddDetailRow("", "")
      rcd = MyBase.CurrentOrder.KitCart
      iMaxIndx = rcd.Rows.Count
      indx = 0
      Do While indx < iMaxIndx
        AddDetailRow(rcd.Rows(indx).Item("KitQty").ToString() & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", rcd.Rows(indx).Item("KitName").ToString() & " - " & rcd.Rows(indx).Item("KitDesc").ToString())
        indx = indx + 1
      Loop
      AddDetailRow("", "")
      'Authorization Note...
            .CurrentAuthorizationStatus(bRequiresLI, bRequiresOR, CurrentUser.DisableAllQuantityLimits)
            If (bRequiresLI) Then AddNoteRow("Quantity of line items requires authorization.")
      If (bRequiresOR) Then AddNoteRow("Total quantity of items ordered requires authorization.")
    End With
    rcd.Dispose() : rcd = Nothing
    bSuccess = True
    Return (bSuccess)
  End Function

  Private Sub AddHeaderRow(ByVal sLabel As String)
    Dim tRow As TableRow
    Dim tCell As TableCell
    Dim lLabel As Label

    tRow = New TableRow
    tCell = New TableCell
    tCell.ColumnSpan = 3
    tCell.HorizontalAlign = HorizontalAlign.Center
    tCell.VerticalAlign = VerticalAlign.Middle
    lLabel = New Label
    lLabel.CssClass = "lblXLong"
    lLabel.Font.Bold = True
    lLabel.Text = sLabel
    tCell.Controls.Add(lLabel)
    tRow.Cells.Add(tCell)

    tblLoad.Rows.Add(tRow)
  End Sub

  Private Sub AddSubHeaderRow(ByVal sLabel As String)
    Dim tRow As TableRow
    Dim tCell As TableCell
    Dim lLabel As Label

    tRow = New TableRow
    tCell = New TableCell
    tCell.ColumnSpan = 3
    tCell.HorizontalAlign = HorizontalAlign.Left
    tCell.VerticalAlign = VerticalAlign.Middle
    lLabel = New Label
    lLabel.CssClass = "lblXLong"
    lLabel.Font.Bold = True
    lLabel.Text = sLabel
    tCell.Controls.Add(lLabel)
    tRow.Cells.Add(tCell)

    tblLoad.Rows.Add(tRow)
  End Sub

  Private Sub AddNoteRow(ByVal sLabel As String)
    Dim tRow As TableRow
    Dim tCell As TableCell
    Dim lLabel As Label

    tRow = New TableRow
    tCell = New TableCell
    tCell.ColumnSpan = 3
    tCell.HorizontalAlign = HorizontalAlign.Center
    tCell.VerticalAlign = VerticalAlign.Middle
    lLabel = New Label
    lLabel.CssClass = "lblXLongDescriptor"
    lLabel.Text = "Note:  " & sLabel
    tCell.Controls.Add(lLabel)
    tRow.Cells.Add(tCell)

    tblLoad.Rows.Add(tRow)
  End Sub

  Private Sub AddDetailRow(ByVal sLabel As String, ByVal sText As String, Optional ByVal bMultiLine As Byte = 0)
    Dim tRow As TableRow
    Dim tCell As TableCell

    tRow = New TableRow
    tCell = New TableCell
    tCell.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
    tRow.Cells.Add(tCell)
    tCell = GetLabelCell(sLabel)
    tRow.Cells.Add(tCell)
    tCell = GetTextCell(sText, bMultiLine)
    tRow.Cells.Add(tCell)

    tblLoad.Rows.Add(tRow)
  End Sub

  Private Function GetLabelCell(ByVal sText As String) As TableCell
    Dim tCell As TableCell
    Dim lLabel As Label

    tCell = New TableCell
    tCell.HorizontalAlign = HorizontalAlign.Right
    tCell.VerticalAlign = VerticalAlign.Middle
    lLabel = New Label
    lLabel.CssClass = "lblMediumLong"
    'If (Right(sText, 1) = ":") Then lLabel.Font.Bold = True
    lLabel.Text = sText & "&nbsp;&nbsp;&nbsp;"
    tCell.Controls.Add(lLabel)

    Return tCell
  End Function

  Private Function GetTextCell(ByVal sText As String, Optional ByVal bMultiLine As Byte = 0) As TableCell
    'Dim tCell As TableCell
    'Dim lLabel As Label

    'tCell = New TableCell
    'tCell.HorizontalAlign = HorizontalAlign.Left
    'tCell.VerticalAlign = VerticalAlign.Middle
    'lLabel = New Label
    'lLabel.CssClass = "lblXLong"
    'lLabel.Text = sText
    'tCell.Controls.Add(lLabel)

    'Return tCell
    Dim tCell As TableCell
    Dim lLabel As Label
    Dim tText As TextBox

    tCell = New TableCell
    tCell.HorizontalAlign = HorizontalAlign.Left
    tCell.VerticalAlign = VerticalAlign.Middle
    If bMultiLine = 1 Or bMultiLine = 2 Then
      tText = New TextBox
      tText.TextMode = TextBoxMode.MultiLine
      tText.Style.Add("OVERFLOW", "HIDDEN")
      tText.ReadOnly = True
      tText.BorderStyle = BorderStyle.None
      If bMultiLine = 1 Then
        tText.CssClass = "lblXLongM"
      ElseIf bMultiLine = 2 Then
        tText.CssClass = "lblXLongL"
      End If
      tText.Text = sText
      tCell.Controls.Add(tText)
    Else
      lLabel = New Label
      lLabel.CssClass = "lblXLong"
      lLabel.Text = sText
      tCell.Controls.Add(lLabel)
    End If

    Return tCell
  End Function

  Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
    Dim sDestin As String = paraPageBase.PAG_OrdSubmit.ToString()

    MyBase.PageDirect(sDestin, 0, 0)
    End Sub



End Class
