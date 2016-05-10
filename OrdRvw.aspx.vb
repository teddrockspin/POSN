''''''''''''''''''''
'mw - 10-02-2008
'mw - 05-24-2008
'mw - 08-18-2007
'mw - 03-17-2007
'mw - 11-05-2006
''''''''''''''''''''
'
'Sample:  http://localhost/POSN/OrdRvw.aspx?KE=08182007-090225&RN=10847
'Sample:  http://localhost/POSN/OrdRvw.aspx?KE=05242008-093342&RN=12957
'Sample:  http://localhost/POSN/OrdRvw.aspx?KE=05242008-102252&RN=12964
'Sample:  http://localhost/POSN/OrdRvw.aspx?KE=09122008-150840&RN=16052
'

Partial Class OrdRvw
  Inherits paraPageBase

  Private mbDone As Boolean = False
  Private msKey As String = String.Empty
  Private mlOrderID As Long = 0
  Private miMaxLI As Integer = 0
  Private miSumLI As Long = 0

#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

  End Sub
  Protected WithEvents tblItm As System.Web.UI.WebControls.Table
  Protected WithEvents tblKit As System.Web.UI.WebControls.Table

  'NOTE: The following placeholder declaration is required by the Web Form Designer.
  'Do not delete or move it.
  Private designerPlaceholderDeclaration As System.Object

  Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
    'CODEGEN: This method call is required by the Web Form Designer
    'Do not modify it using the code editor.
    InitializeComponent()

    MyBase.Page_Init(sender, e)

    'To test.... http://localhost/POSN/OrdRvw.aspx?KE=08292005-060916&RN=4276
    msKey = Request.QueryString("KE")
    mlOrderID = Request.QueryString("RN")
    If (msKey.Length = 0) And (mlOrderID = 0) Then
      'For Overlap in systems
      msKey = Request.QueryString("Key")
      mlOrderID = Request.QueryString("RequestNo")
    End If
    '    If (Page.IsPostBack = False) Then
    cmdApprove.Attributes.Add("onclick", "return confirm('You are about to approve the current request.  Do you wish to continue?');")
    cmdRespond.Attributes.Add("onclick", "return confirm('You are about to send your response to the current request.  Do you wish to continue?');")
    Session("ReviewResponse") = 0
    LoadData(msKey, mlOrderID)
    '   End If
  End Sub

#End Region

  Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    PageTitle = "Approve / Respond"
    'MyBase.Page_Load(sender, e)
    'MyBase.PageMessage = ""
    'If (Page.IsPostBack = False) Then MyBase.PageMessage = ""

    'If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
    'MyBase.PageDirect()
    'End If

    'If (Page.IsPostBack = False) Then
    '  If (LoadData() = True) Then
    '    'ShowControls()
    '  Else
    '    Session("PMsg") = "Unable to Load Data..."
    '  End If 'End PostBack
    'End If
  End Sub

  Private Function LoadData(ByVal sKey As String, ByVal lOrderID As Long)
    Dim bValid As Boolean = False
    Dim bOnHold As Boolean = False
    Dim bSuccess As Boolean = False

    Try
      mbDone = (Session("ReviewResponse") = 1)
      bValid = LoadDetails(sKey, lOrderID, bOnHold)
      If (bValid = True) Then
        miSumLI = 0
        LoadItems(lOrderID)
        LoadKits(lOrderID)
        LoadSummary()
      End If

      ShowControls(bValid, bOnHold)

      bSuccess = True
    Catch ex As Exception
      Session("PMsg") = "Unable to Load Data..."
    End Try

    Return (bSuccess)
  End Function

  Private Function LoadDetails(ByVal sKey As String, ByVal lOrderID As Long, ByRef bOnHold As Boolean)
    Dim cnn As OleDb.OleDbConnection
    Dim cmd As OleDb.OleDbCommand
    Dim rcd As OleDb.OleDbDataReader
    Dim bSuccess As Boolean = False
    Dim sSQL As String
    Dim tRow As TableRow
    Dim tCell As TableCell
    Dim bKeyMatch As Boolean = False

    Try
      tblDetail.Rows.Clear()
      '
      sSQL = "SELECT [Order].*, FORMAT([Order].RequestDate,'mmddyyyy-hhnnss') AS OrderKey, Customer_ShippingMethod.Description AS Ship_PreferredName " & _
             "FROM [Order] INNER JOIN Customer_ShippingMethod ON [Order].PreferredShipID = Customer_ShippingMethod.ID " & _
             "WHERE [Order].ID = " & lOrderID
      MyBase.MyData.GetReader2(cnn, cmd, rcd, sSQL)
      If rcd.Read() Then
        bKeyMatch = (sKey = rcd("OrderKey"))
        bOnHold = (rcd("StatusID") = MyData.STATUS_HOLD)
        txtRCC.Text = rcd("Requestor_Email")

        If (bKeyMatch = True) Then
          tRow = New TableRow
          tCell = GetCell_Label("Review for Authorization on Request # " & lOrderID, "C", True)
          tCell.ColumnSpan = 7
          tRow.Cells.Add(tCell)
          tblDetail.Rows.Add(tRow)
          If (bOnHold = False) Then
            tRow = New TableRow
            tCell = GetCell_Label("This Request is no longer on hold.", "C", True)
            tCell.ColumnSpan = 7
            tRow.Cells.Add(tCell)
            tblDetail.Rows.Add(tRow)
          End If
          tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 7 : tRow.Cells.Add(tCell)
          tblDetail.Rows.Add(tRow)

          tRow = New TableRow
          tCell = New TableCell : tCell.Text = "&nbsp;"
          tRow.Cells.Add(tCell)
          tCell = GetCell_Label("Requestor", "C", "H")
          tRow.Cells.Add(tCell)
          tblDetail.Rows.Add(tRow)

          tRow = New TableRow
          tCell = GetCell_Label("Name:", "R")
          tCell.ColumnSpan = 2
          tRow.Cells.Add(tCell)
          tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 2 : tRow.Cells.Add(tCell)
          tCell = GetCell_Label(rcd("Requestor_FirstName") & " " & rcd("Requestor_LastName"), "L")
          tCell.ColumnSpan = 3
          tRow.Cells.Add(tCell)
          tblDetail.Rows.Add(tRow)

          tRow = New TableRow
          tCell = New TableCell : tCell.Text = "&nbsp;"
          tRow.Cells.Add(tCell)
          tCell = GetCell_Label("Recipient", "C", "H")
          tRow.Cells.Add(tCell)
          tblDetail.Rows.Add(tRow)

          tRow = New TableRow
          tCell = GetCell_Label("Name:", "R")
          tCell.ColumnSpan = 2
          tRow.Cells.Add(tCell)
          tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 2 : tRow.Cells.Add(tCell)
          tCell = GetCell_Label(rcd("ShipTo_ContactFirstName") & " " & rcd("ShipTo_ContactLastName"), "L")
          tCell.ColumnSpan = 3
          tRow.Cells.Add(tCell)
          tblDetail.Rows.Add(tRow)

          tRow = New TableRow
          tCell = GetCell_Label("Company:", "R")
          tCell.ColumnSpan = 2
          tRow.Cells.Add(tCell)
          tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 2 : tRow.Cells.Add(tCell)
          tCell = GetCell_Label(rcd("ShipTo_Name"), "L")
          tCell.ColumnSpan = 3
          tRow.Cells.Add(tCell)
          tblDetail.Rows.Add(tRow)

          tRow = New TableRow
          tCell = GetCell_Label("Address:", "R")
          tCell.ColumnSpan = 2
          tRow.Cells.Add(tCell)
          tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 2 : tRow.Cells.Add(tCell)
          tCell = GetCell_Label(rcd("ShipTo_Address1"), "L")
          tCell.ColumnSpan = 3
          tRow.Cells.Add(tCell)
          tblDetail.Rows.Add(tRow)

          tRow = New TableRow
          tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 4 : tRow.Cells.Add(tCell)
          tCell = GetCell_Label(rcd("ShipTo_Address2"), "L")
          tCell.ColumnSpan = 3
          tRow.Cells.Add(tCell)
          tblDetail.Rows.Add(tRow)

          tRow = New TableRow
          tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 4 : tRow.Cells.Add(tCell)
          tCell = GetCell_Label(rcd("ShipTo_City") & ", " & rcd("ShipTo_State") & " " & rcd("ShipTo_ZipCode"), "L")
          tCell.ColumnSpan = 3
          tRow.Cells.Add(tCell)
          tblDetail.Rows.Add(tRow)

          If (rcd("ShipTo_Country").ToString.Length > 0) Then
            tRow = New TableRow
            tCell = GetCell_Label("Address:", "R")
            tCell.ColumnSpan = 2
            tRow.Cells.Add(tCell)
            tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 2 : tRow.Cells.Add(tCell)
            tCell = GetCell_Label(rcd("ShipTo_Country"), "L")
            tCell.ColumnSpan = 3
            tRow.Cells.Add(tCell)
            tblDetail.Rows.Add(tRow)
          End If

          tRow = New TableRow
          tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 7 : tRow.Cells.Add(tCell)
          tblDetail.Rows.Add(tRow)

          tRow = New TableRow
          tCell = GetCell_Label("Shipping Method:", "R")
          tCell.ColumnSpan = 2
          tRow.Cells.Add(tCell)
          tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 2 : tRow.Cells.Add(tCell)
          tCell = GetCell_Label(rcd("Ship_PreferredName"), "L")
          tCell.ColumnSpan = 3
          tRow.Cells.Add(tCell)
          tblDetail.Rows.Add(tRow)

          tRow = New TableRow
          tCell = GetCell_Label("Notes:", "R")
          tCell.ColumnSpan = 2
          tRow.Cells.Add(tCell)
          tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 2 : tRow.Cells.Add(tCell)
          tCell = GetCell_Label(rcd("ShipNote"), "L", , 1)
          tCell.ColumnSpan = 3
          tRow.Cells.Add(tCell)
          tblDetail.Rows.Add(tRow)

          tCell.Dispose()
          tRow.Dispose()
        End If
      End If
      bSuccess = True
    Catch ex As Exception
      Session("PMsg") = "Unable to Load Order Detail Data..."
    End Try
    MyBase.MyData.ReleaseReader2(cnn, cmd, rcd)

    Return (bKeyMatch)
  End Function

  Private Function GetCell_Label(ByVal sText As String, ByVal sAlign As String, Optional ByVal sType As String = "L", Optional ByVal bMultiLine As Byte = 0) As TableCell
    Dim tCell As New TableCell
    Dim lbl As New Label
    Dim tText As New TextBox

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
      tText.Dispose()

    Else
      If (sType = "HL") Then
        lbl.CssClass = "lblTitle"
      ElseIf (sType = "H") Then
        lbl.CssClass = "lblTitleSub"
      ElseIf (sType = "L") Then
        lbl.CssClass = "lblLong"
      ElseIf (sType = "S") Then
        lbl.CssClass = "lblShort"
      End If
      If (sAlign = "L") Then
        tCell.HorizontalAlign = HorizontalAlign.Left
      ElseIf (sAlign = "R") Then
        tCell.HorizontalAlign = HorizontalAlign.Right
      ElseIf (sAlign = "C") Then
        tCell.HorizontalAlign = HorizontalAlign.Center
        lbl.CssClass = "lblTitle"
      End If
      lbl.Text = sText
      tCell.Controls.Add(lbl)
      lbl.Dispose()
    End If

    Return tCell
  End Function

  Private Function LoadItems(ByVal lOrderID As Long)
    Dim cnn As OleDb.OleDbConnection
    Dim cmd As OleDb.OleDbCommand
    Dim rcd As OleDb.OleDbDataReader
    Dim bSuccess As Boolean = False
    Dim sSQL As String
    Dim tRow As TableRow
    Dim tCell As TableCell
    Dim sItm As String = String.Empty
    'Dim iQty As String = 0
    Dim sQty As String = 0

    Try
      sSQL = "SELECT Customer_Document.ReferenceNo, Customer_Document.Description, Order_Document.QtyOrdered " & _
             "FROM Customer_Document RIGHT JOIN Order_Document ON Customer_Document.ID = Order_Document.DocumentID " & _
             "WHERE Order_Document.OrderID = " & lOrderID & " AND (VAL(Order_Document.KitID & '') = 0) " & _
             "ORDER BY Customer_Document.ReferenceNo, Customer_Document.Description "
      MyBase.MyData.GetReader2(cnn, cmd, rcd, sSQL)

      If (rcd.HasRows) Then
        tRow = New TableRow
        tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 7 : tRow.Cells.Add(tCell)
        tblDetail.Rows.Add(tRow)
        tRow = New TableRow
        tCell = New TableCell : tCell.Text = "&nbsp;"
        tRow.Cells.Add(tCell)
        tCell = GetCell_Label("Items", "C", "H")
        tRow.Cells.Add(tCell)
        tblDetail.Rows.Add(tRow)
        Do While rcd.Read()
          sItm = rcd("ReferenceNo") & " - " & rcd("Description")
          'iQty = rcd("QtyOrdered")
          'If (iQty > miMaxLI) Then miMaxLI = iQty
          'miSumLI = miSumLI + iQty
          sQty = rcd("QtyOrdered")
          If (sQty > miMaxLI) Then miMaxLI = sQty
          miSumLI = miSumLI + sQty

          tRow = New TableRow
          'tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 1 : tRow.Cells.Add(tCell)
          'tCell = GetCell_Label(iQty, "R", "S")
          tCell = GetCell_Label(sQty, "R", "S")
          tCell.ColumnSpan = 2
          tRow.Cells.Add(tCell)
          tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 1 : tRow.Cells.Add(tCell)
          tCell = GetCell_Label(sItm, "L")
          tCell.ColumnSpan = 3
          tRow.Cells.Add(tCell)
          tblDetail.Rows.Add(tRow)
        Loop
        tCell.Dispose()
        tRow.Dispose()
      End If

      bSuccess = True
    Catch ex As Exception
      Session("PMsg") = "Unable to Load Item Data..."
    End Try
    MyBase.MyData.ReleaseReader2(cnn, cmd, rcd)

    Return (bSuccess)
  End Function

  Private Function LoadKits(ByVal lOrderID As Long)
    Dim cnn As OleDb.OleDbConnection
    Dim cmd As OleDb.OleDbCommand
    Dim rcd As OleDb.OleDbDataReader
    Dim bSuccess As Boolean = False
    Dim sSQL As String
    Dim tRow As TableRow
    Dim tCell As TableCell
    Dim sCurrKit As String = String.Empty
    Dim sKit As String = String.Empty
    Dim sItm As String = String.Empty
    'Dim iQty As String = 0
    Dim sQty As String = 0

    Try
      sSQL = "SELECT Customer_Kit.ReferenceNo AS KitReferenceNo, Customer_Kit.Description AS KitDescription, " & _
             "Customer_Document.ReferenceNo AS DocReferenceNo, Customer_Document.Description AS DocDescription, " & _
             "Order_Document.QtyOrdered " & _
             "FROM (Customer_Document RIGHT JOIN Order_Document ON Customer_Document.ID = Order_Document.DocumentID) " & _
             "LEFT JOIN Customer_Kit ON Order_Document.KitID = Customer_Kit.ID " & _
             "WHERE Order_Document.OrderID = " & lOrderID & " AND (VAL(Order_Document.KitID & '') >0) " & _
             "ORDER BY Customer_Kit.ReferenceNo, Customer_Kit.Description, Customer_Document.ReferenceNo, Customer_Document.Description "
      MyBase.MyData.GetReader2(cnn, cmd, rcd, sSQL)

      If (rcd.HasRows) Then
        tRow = New TableRow
        tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 7 : tRow.Cells.Add(tCell)
        tblDetail.Rows.Add(tRow)
        tRow = New TableRow
        tCell = New TableCell : tCell.Text = "&nbsp;"
        tRow.Cells.Add(tCell)
        tCell = GetCell_Label("Kits", "C", "H")
        tRow.Cells.Add(tCell)
        tblDetail.Rows.Add(tRow)

        Do While rcd.Read()
          sKit = rcd("KitReferenceNo") & " - " & rcd("KitDescription")
          sItm = rcd("DocReferenceNo") & " - " & rcd("DocDescription")
          'iQty = rcd("QtyOrdered")
          'If (iQty > miMaxLI) Then miMaxLI = iQty
          'miSumLI = miSumLI + iQty
          sQty = rcd("QtyOrdered")
          If (sQty > miMaxLI) Then miMaxLI = sQty
          miSumLI = miSumLI + sQty

          tRow = New TableRow
          If (sKit <> sCurrKit) Then
            tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 1 : tRow.Cells.Add(tCell)
            tCell = GetCell_Label(sKit, "R")
            tCell.ColumnSpan = 1
            tRow.Cells.Add(tCell)
          Else
            tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 2 : tRow.Cells.Add(tCell)
          End If
          'tCell = GetCell_Label(iQty, "R", "S")
          tCell = GetCell_Label(sQty, "R", "S")
          tCell.ColumnSpan = 1
          tRow.Cells.Add(tCell)
          tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 1 : tRow.Cells.Add(tCell)
          tCell = GetCell_Label(sItm, "L")
          tCell.ColumnSpan = 3
          tRow.Cells.Add(tCell)
          tblDetail.Rows.Add(tRow)

          sCurrKit = sKit
        Loop
        tCell.Dispose()
        tRow.Dispose()
      End If
      bSuccess = True
    Catch ex As Exception
      Session("PMsg") = "Unable to Load Kit Data..."
    End Try
    MyBase.MyData.ReleaseReader2(cnn, cmd, rcd)

    Return (bSuccess)
  End Function

  Private Function LoadSummary()
    Dim bSuccess As Boolean = False
    Dim tRow As TableRow
    Dim tCell As TableCell

    Try
      'tRow = New TableRow
      'tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 7 : tRow.Cells.Add(tCell)
      'tblDetail.Rows.Add(tRow)

      tRow = New TableRow
      tCell = New TableCell : tCell.Text = "&nbsp;"
      tRow.Cells.Add(tCell)
      tblDetail.Rows.Add(tRow)

      tRow = New TableRow
      tCell = GetCell_Label("Highest Line Item Quantity:  " & miMaxLI, "C", "HL")
      tCell.ColumnSpan = 7
      tRow.Cells.Add(tCell)
      '      'tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 1 : tRow.Cells.Add(tCell)
      '      tCell = GetCell_Label(miMaxLI, "L", "H")
      '      tCell.ColumnSpan = 3
      'tRow.Cells.Add(tCell)
      tblDetail.Rows.Add(tRow)

      tRow = New TableRow
      tCell = GetCell_Label("Total Line Item Quantity:  " & miSumLI, "C", "HL")
      tCell.ColumnSpan = 7
      tRow.Cells.Add(tCell)
      '      'tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 1 : tRow.Cells.Add(tCell)
      '      tCell = GetCell_Label(miSumLI, "L", "H")
      '      tCell.ColumnSpan = 3
      '      tRow.Cells.Add(tCell)
      tblDetail.Rows.Add(tRow)

      tRow = New TableRow
      tCell = New TableCell : tCell.Text = "&nbsp;" : tCell.ColumnSpan = 7 : tRow.Cells.Add(tCell)
      tblDetail.Rows.Add(tRow)

      tCell.Dispose()
      tRow.Dispose()
      bSuccess = True
    Catch ex As Exception
      Session("PMsg") = "Unable to Load Summary Data..."
    End Try

    Return (bSuccess)
  End Function

  Private Function ShowControls(ByVal bValid As Boolean, ByVal bForApproval As Boolean)
    tblDetail.Visible = bValid

    'lblREmail.Visible = bForApproval
    'txtREmail.Visible = bForApproval
    'lblRContent.Visible = bForApproval
    'txtRContent.Visible = bForApproval

    'cmdApprove.Visible = bForApproval
    'cmdRespond.Visible = bForApproval

    lblREmail.Enabled = bForApproval And Not mbDone
    txtREmail.Enabled = bForApproval And Not mbDone
    lblRContent.Enabled = bForApproval And Not mbDone
    txtRContent.Enabled = bForApproval And Not mbDone

    'cmdApprove.Enabled = bForApproval And Not mbDone
    'cmdRespond.Enabled = bForApproval And Not mbDone
    cmdApprove.Visible = bForApproval And Not mbDone
    cmdRespond.Visible = bForApproval And Not mbDone

    If bForApproval And Not mbDone Then
      Me.PageMessage = "Please review and respond by approving as is or responding with instructions on how to proceed..."
    ElseIf Not bValid Then
      Me.PageMessage = "Unable to locate Request..."
    End If
  End Function

  'Private Sub cmdApprove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  '  If (mlOrderID > 0) Then
  '    If (MyBase.MyData.ApproveOrder(mlOrderID)) Then
  '      MyBase.PageMessage = "Request # " & mlOrderID & " has been approved."
  '      Session("ReviewResponse") = 1
  '    Else
  '      MyBase.PageMessage = "Unable to approve Request # " & mlOrderID & "."
  '    End If
  '  Else
  '    MyBase.PageMessage = "Unable to approve Request # " & mlOrderID & "."
  '  End If

  '  LoadData(msKey, mlOrderID)
  'End Sub
  Private Overloads Sub cmdApprove_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdApprove.Click
    If (mlOrderID > 0) Then
      If (MyBase.MyData.ApproveOrder(mlOrderID)) Then
        MyBase.PageMessage = "Request # " & mlOrderID & " has been approved."
        Session("ReviewResponse") = 1
      Else
        MyBase.PageMessage = "Unable to approve Request # " & mlOrderID & "."
      End If
    Else
      MyBase.PageMessage = "Unable to approve Request # " & mlOrderID & "."
    End If

    LoadData(msKey, mlOrderID)
  End Sub

  'Private Sub cmdRespond_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  '  Dim oMail As Mail
  '  Dim sTo As String = ConfigurationSettings.AppSettings("DefaultEmailAddress")
  '  Dim sFrom As String = txtREmail.Text.ToString
  '  Dim sSubject As String = "In Response to Request Number " & mlOrderID
  '  Dim sBody As String = txtRContent.Text.ToString
  '  Dim sCC As String = txtRCC.Text.ToString

  '  Try
  '    If (sTo.Length > 0) And (sFrom.Length > 0) And (sCC.Length > 0) And (sBody.Length > 0) Then
  '      oMail = New Mail
  '      If (oMail.SendMessage(sTo, sFrom, sSubject, sBody, sCC)) Then
  '        MyBase.PageMessage = "Response sucessfully sent!"
  '        Session("ReviewResponse") = 1
  '      Else
  '        MyBase.PageMessage = "Unable to send response..."
  '      End If
  '    Else
  '      MyBase.PageMessage = "Incomplete information to send response..."
  '    End If
  '  Catch ex As Exception
  '    MyBase.PageMessage = "Unable to send response..."
  '  End Try
  '  oMail = Nothing

  '  LoadData(msKey, mlOrderID)
  'End Sub
  Private Overloads Sub cmdRespond_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRespond.Click
    Dim oMail As Mail
    Dim sTo As String = ConfigurationSettings.AppSettings("DefaultEmailAddress")
    Dim sFrom As String = txtREmail.Text.ToString
    Dim sSubject As String = "In Response to Request Number " & mlOrderID
    Dim sBody As String = txtRContent.Text.ToString
    Dim sCC As String = txtRCC.Text.ToString

    Try
      MyBase.MyData.RejectOrder(mlOrderID)

      If (sTo.Length > 0) And (sFrom.Length > 0) And (sCC.Length > 0) And (sBody.Length > 0) Then
        oMail = New Mail
        If (oMail.SendMessage(sTo, sFrom, sSubject, sBody, sCC)) Then
          MyBase.PageMessage = "Response sucessfully sent!"
          Session("ReviewResponse") = 1
        Else
          MyBase.PageMessage = "Unable to send response..."
        End If
      Else
        MyBase.PageMessage = "Incomplete information to send response..."
      End If
    Catch ex As Exception
      MyBase.PageMessage = "Unable to send response..."
    End Try
    oMail = Nothing

    LoadData(msKey, mlOrderID)
  End Sub

  'Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  '  MyBase.PageDirect(MyBase.PAG_Main, 0, 0)
  'End Sub

End Class
