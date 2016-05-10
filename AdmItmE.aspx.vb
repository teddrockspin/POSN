''''''''''''''''''''
'mw - 07-19-2008
'mw - 06-07-2008
'mw - 05-03-2008
'mw - 09-16-2007
''''''''''''''''''''


Partial Class AdmItmE
  Inherits paraPageBase

    Private mlID As String
    Private Const cMaxOrderLimit = 999999

#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

  End Sub
  Protected WithEvents lblRefName As System.Web.UI.WebControls.Label
  Protected WithEvents txtRefName As System.Web.UI.WebControls.Label
  Protected WithEvents Attr3 As System.Web.UI.WebControls.Label
  Protected WithEvents Attr4 As System.Web.UI.WebControls.Label
  Protected WithEvents vScedObsDate As System.Web.UI.WebControls.RangeValidator
  Protected WithEvents Rangevalidator1 As System.Web.UI.WebControls.RangeValidator

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
        PageTitle = "Item Edit"
        'MyBase.Page_Load(sender, e)
        MyBase.PageMessage = ""

        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()
        ElseIf Not (MyBase.CurrentUser.CanRestockPlan = True) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            AddScripts()
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri
            SetStockWarningLabel()
        End If 'End PostBack
    End Sub

    Private Sub SetStockWarningLabel()
        Dim taSWT As New dsCustomerTableAdapters.CustomerStockWarningTypeTableAdapter
        Dim dtSWT As New dsCustomer.CustomerStockWarningTypeDataTable

        dtSWT = taSWT.GetData(CurrentUser.CustomerID)
        If (dtSWT.Rows.Count > 0) Then
            If dtSWT(0).StockWarningType = 0 Then
                lblWarnLevel.Text = "Stock Warning Level (Wks):"
            Else
                lblWarnLevel.Text = "Stock Warning Level (Pcs):"
            End If
        End If
    End Sub

    Private Sub AddScripts()
        Me.chkNoOrderLimit.Attributes.Add("onClick", String.Format("ShowOrderQuantityLimit({0}, {1}, {2}, {3})", Me.txtOrderQuantityLimit.ClientID, Me.lblOrderQuantityLimit.ClientID, Me.chkNoOrderLimit.ClientID, cMaxOrderLimit))
    End Sub

    Private Sub ShowOrderQuantityLimit(ByVal bValue As Boolean)
        If (bValue) Then
            Me.txtOrderQuantityLimit.Attributes.Add("style", "display: '';")
            Me.lblOrderQuantityLimit.Attributes.Add("style", "display: '';")
        Else
            Me.txtOrderQuantityLimit.Attributes.Add("style", "display: 'none';")
            Me.lblOrderQuantityLimit.Attributes.Add("style", "display: 'none';")
        End If

        If (Trim(Me.txtOrderQuantityLimit.Text) <> "") Then
            If (Me.txtOrderQuantityLimit.Text = cMaxOrderLimit) Then
                Me.txtOrderQuantityLimit.Text = ""
            End If
        End If
    End Sub

    Private Function CurrentItemTypeID() As Long
        Dim lID As Long = 0

        If Not (cboItmType.SelectedItem Is Nothing) Then
            lID = cboItmType.SelectedItem.Value()
        End If

        Return lID
    End Function

    Private Function CurrentVendorID() As Long
        Dim lID As Long = 0

        If Not (cboVendor.SelectedItem Is Nothing) Then
            lID = cboVendor.SelectedItem.Value()
        End If

        Return lID
    End Function

    Private Function CurrentAttribute1ID() As Long
        Dim lID As Long = 0

        If Not (cboAttr1.SelectedItem Is Nothing) Then
            lID = cboAttr1.SelectedItem.Value()
        End If

        Return lID
    End Function

    Private Function CurrentAttribute2ID() As Long
        Dim lID As Long = 0

        If Not (cboAttr2.SelectedItem Is Nothing) Then
            lID = cboAttr2.SelectedItem.Value()
        End If

        Return lID
    End Function

    Private Function CurrentAttribute3ID() As Long
        Dim lID As Long = 0

        If Not (cboAttr3.SelectedItem Is Nothing) Then
            lID = cboAttr3.SelectedItem.Value()
        End If

        Return lID
    End Function

    Private Function CurrentAttribute4ID() As Long
        Dim lID As Long = 0

        If Not (cboAttr4.SelectedItem Is Nothing) Then
            lID = cboAttr4.SelectedItem.Value()
        End If

        Return lID
    End Function

    Private Function LoadData()
        mlID = Val(Request.QueryString("ED").ToString())

        LoadItemDetails()

        cmdSave.Attributes.Add("onclick", "return confirm('You are about to save the current item specifications.  Do you wish to continue?'); ")

        If (Me.txtOrderQuantityLimit.Text = cMaxOrderLimit) Then
            Me.chkNoOrderLimit.Checked = True
        Else
            Me.chkNoOrderLimit.Checked = False
        End If
        ShowOrderQuantityLimit(Not Me.txtOrderQuantityLimit.Text = CType(cMaxOrderLimit, String))
    End Function

    Private Function BuildSQLBase() As String
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty

        sSelect = "SELECT Customer_Document.ID, " & _
                  "Customer_Document.PrintMethod AS PrintMethod, " & _
                  "Customer_Document.ReferenceNo AS RefNo, " & _
                  "Customer_Document.Description AS RefDesc, " & _
                  "Customer_Document.Prefix & '' AS Prefix, " & _
                  "iif(Customer_Document_Fill.Cost IS NULL,'n/a',Customer_Document_Fill.Cost) AS LastPurPrice, " & _
                  "iif(Customer_Document.VendorID IS NULL,0,Customer_Document.VendorID) AS VendorID, " & _
                  "iif(Customer_Document.InActPrjDate IS NULL,'',Customer_Document.InActPrjDate) AS SchedObsDate, " & _
                  "iif(Customer_Document_Fill.StockWarn IS NULL,'n/a',Customer_Document_Fill.StockWarn) AS WarnLevel, " & _
                  "iif(Customer_Document.ItemTypeID IS NULL,0,Customer_Document.ItemTypeID) AS ItmTypeID, " & _
                  "iif(Customer_Document.OrderQuantityLimit IS NULL, 0, Customer_Document.OrderQuantityLimit) as OrderQuantityLimit, " & _
                  "iif(Customer_Document.Attr1ID IS NULL,0,Customer_Document.Attr1ID) AS Attr1ID, " & _
                  "iif(Customer_Document.Attr2ID IS NULL,0,Customer_Document.Attr2ID) AS Attr2ID, " & _
                  "iif(Customer_Document.Attr3ID IS NULL,0,Customer_Document.Attr3ID) AS Attr3ID, " & _
                  "iif(Customer_Document.Attr4ID IS NULL,0,Customer_Document.Attr4ID) AS Attr4ID, " & _
                  "Customer_Document.Attr5 " & _
                  "FROM Customer_Document LEFT JOIN Customer_Document_Fill ON Customer_Document.ID = Customer_Document_Fill.DocumentID "

        sWhere = "(Customer_Document.ID = " & mlID & ") "
        If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

        sSQL = sSelect + sWhere + sOrder

        Return sSQL
    End Function

    Private Function LoadItemDetails() As Boolean
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim rcd As OleDb.OleDbDataReader
        Dim bSuccess As Boolean = False
        Dim sSQL As String

        Dim sRefNo As String = String.Empty
        Dim sRefDesc As String = String.Empty
        Dim sPrefix As String = String.Empty
        Dim sPrice As String = "0.00"
        Dim lVendorID As Long = 0
        Dim sDate As String = String.Empty
        Dim sWarnLev As String = "0"
        Dim sOrderQuantityLimit As String = "0"
        Dim lItmTypeID As Long = 0
        Dim lAttr1ID As Long = 0
        Dim lAttr2ID As Long = 0
        Dim lAttr3ID As Long = 0
        Dim lAttr4ID As Long = 0
        Dim sAttr5 As String = String.Empty

        sSQL = BuildSQLBase()
        If (sSQL.Length > 0) Then
            MyBase.MyData.GetReader2(cnn, cmd, rcd, sSQL)
            If rcd.Read() Then
                sRefNo = rcd("RefNo")
                sRefDesc = rcd("RefDesc")
                sPrefix = rcd("Prefix")
                sPrice = rcd("LastPurPrice")
                lVendorID = rcd("VendorID")
                sDate = rcd("SchedObsDate")
                sWarnLev = rcd("WarnLevel")
                lItmTypeID = rcd("ItmTypeID")
                lAttr1ID = rcd("Attr1ID")
                lAttr2ID = rcd("Attr2ID")
                lAttr3ID = rcd("Attr3ID")
                lAttr4ID = rcd("Attr4ID")
                sAttr5 = (rcd("Attr5").ToString())
                If (rcd("PrintMethod") = 1) Then
                    txtWarnLevel.Enabled = False
                    txtLastPurPrice.Enabled = False
                End If
                sOrderQuantityLimit = rcd("OrderQuantityLimit")
            End If
            MyBase.MyData.ReleaseReader2(cnn, cmd, rcd)

            bSuccess = True
        End If

        txtRefNo.Text = sRefNo
        txtRefDesc.Text = sRefDesc
        txtPrefix.Text = sPrefix
        txtLastPurPrice.Text = sPrice
        LoadList(BuildSQLVendor(), cboVendor, lVendorID)
        txtSchedObsDate.Text = sDate
        txtWarnLevel.Text = sWarnLev
        Me.txtOrderQuantityLimit.Text = sOrderQuantityLimit
        LoadList(BuildSQLItemType(lItmTypeID), cboItmType, lItmTypeID)
        LoadAttributes(lItmTypeID)
        LoadList(BuildSQLAttribute(lItmTypeID, 1), cboAttr1, lAttr1ID)
        LoadList(BuildSQLAttribute(lItmTypeID, 2), cboAttr2, lAttr2ID)
        LoadList(BuildSQLAttribute(lItmTypeID, 3), cboAttr3, lAttr3ID)
        LoadList(BuildSQLAttribute(lItmTypeID, 4), cboAttr4, lAttr4ID)
        txtAttr5.Text = sAttr5

        Return bSuccess
    End Function

    Private Function BuildSQLVendor() As String
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty

        sSelect = "SELECT ID, Name AS ListText " & _
                  "FROM Customer_Vendor "

        sWhere = "(Customer_Vendor.CustomerID = " & MyBase.CurrentUser.CustomerID & ") "
        If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

        sOrder = " ORDER BY [Name]"

        sSQL = sSelect + sWhere + sOrder

        Return sSQL
    End Function

    Private Function BuildSQLItemType(ByVal lItemTypeID As Long) As String
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty

        sSelect = "SELECT ID, ReferenceNo + ' - ' + Description AS ListText " & _
                  "FROM Customer_ItemType "

        sWhere = "(Customer_ItemType.CustomerID = " & MyBase.CurrentUser.CustomerID & ") "
        If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

        sOrder = " ORDER BY ReferenceNo, Description"

        sSQL = sSelect + sWhere + sOrder

        Return sSQL
    End Function

    Private Function BuildSQLAttribute(ByVal lItemTypeID As Long, ByVal iAttrNo As Integer) As String
        Dim sSQL As String = String.Empty
        Dim sSelect As String = String.Empty
        Dim sWhere As String = String.Empty
        Dim sOrder As String = String.Empty

        sSelect = "SELECT ID, Description AS ListText " & _
                  "FROM Customer_ItemType_Attribute "

        sWhere = "(Customer_ItemType_Attribute.ItemTypeID = " & lItemTypeID & ") " & _
            " AND (Customer_ItemType_Attribute.AttrNo = " & iAttrNo & ") "
        If (sWhere.Length > 0) Then sWhere = " WHERE " + sWhere

        sOrder = " ORDER BY Description"

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

        cbo.Items.Clear()

        cbo.Items.Add(New ListItem("None", 0))
        cbo.Items(cbo.Items.Count - 1).Attributes.Add("style", "color:" & MyBase.MyData.COLOR_NORMAL)

        If (sSQL.Length > 0) Then
            MyBase.MyData.GetReader2(cnn, cmd, dr, sSQL)
            Do While dr.Read()
                sID = dr("ID").ToString()
                sText = Trim(dr("ListText"))

                cbo.Items.Add(New ListItem(sText, sID))
                cbo.Items(cbo.Items.Count - 1).Attributes.Add("style", "color:" & MyBase.MyData.COLOR_NORMAL)

                If (sID = lID) Then
                    cbo.Items(cbo.Items.Count - 1).Selected = True
                End If
            Loop
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

            bSuccess = True
        End If

        Return (bSuccess)
    End Function

    Private Function LoadAttributes(ByVal lID As Long)
        Dim cnn As OleDb.OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim rcd As OleDb.OleDbDataReader
        Dim sSQL As String = String.Empty
        Dim bSuccess As Boolean = False
        Dim sAttr1 As String = "Atribute 1:"
        Dim sAttr2 As String = "Atribute 2:"
        Dim sAttr3 As String = "Atribute 3:"
        Dim sAttr4 As String = "Atribute 4:"

        Try
            sSQL = "SELECT " & _
                   "Customer_ItemType.Attr1 AS Attr1, " & _
                   "Customer_ItemType.Attr2 AS Attr2, " & _
                   "Customer_ItemType.Attr3 AS Attr3, " & _
                   "Customer_ItemType.Attr4 AS Attr4 " & _
                   "FROM Customer_ItemType " & _
                   "WHERE (ID = " & lID & ")"
            MyBase.MyData.GetReader2(cnn, cmd, rcd, sSQL)
            If rcd.Read() Then
                If (Trim(rcd("Attr1").ToString).Length > 0) Then sAttr1 = Trim(rcd("Attr1").ToString) & ":"
                If (Trim(rcd("Attr2").ToString).Length > 0) Then sAttr2 = Trim(rcd("Attr2").ToString) & ":"
                If (Trim(rcd("Attr3").ToString).Length > 0) Then sAttr3 = Trim(rcd("Attr3").ToString) & ":"
                If (Trim(rcd("Attr4").ToString).Length > 0) Then sAttr4 = Trim(rcd("Attr4").ToString) & ":"
                MyBase.MyData.ReleaseReader2(cnn, cmd, rcd)
            End If
            lblAttr1.Text = sAttr1
            lblAttr2.Text = sAttr2
            lblAttr3.Text = sAttr3
            lblAttr4.Text = sAttr4

            bSuccess = True

        Catch ex As Exception
            MyBase.PageMessage = "Error attempting to Load Attributes..."
        End Try

        Return (bSuccess)
    End Function


    Private Function SaveData() As Boolean
        Dim cnn As OleDb.OleDbConnection
        Dim rcd As OleDb.OleDbDataReader
        Dim sSQL As String = String.Empty
        Dim bSuccess As Boolean = False
        Dim sPrefix As String = String.Empty
        Dim sPrice As String = "0.00"
        Dim sVendorID As String = "NULL"
        Dim sDate As String = String.Empty
        Dim sWarnLev As String = "0"
        Dim sOrderQuantityLimit As String = "0"
        Dim sItmTypeID As String = "NULL"
        Dim sAttr1ID As String = "NULL"
        Dim sAttr2ID As String = "NULL"
        Dim sAttr3ID As String = "NULL"
        Dim sAttr4ID As String = "NULL"
        Dim sAttr5 As String = String.Empty

        Try
            sPrefix = Trim(txtPrefix.Text)
            sPrice = Convert.ToSingle(Val(Trim(txtLastPurPrice.Text)))
            sVendorID = CurrentVendorID() : sVendorID = IIf(Val(sVendorID) > 0, sVendorID, "NULL")
            sDate = Trim(txtSchedObsDate.Text)
            sWarnLev = Convert.ToByte(Val(Trim(txtWarnLevel.Text)))
            If (chkNoOrderLimit.Checked) Or (Me.txtOrderQuantityLimit.Text = "") Then
                Me.txtOrderQuantityLimit.Text = cMaxOrderLimit
            End If
            sOrderQuantityLimit = Me.txtOrderQuantityLimit.Text

            sItmTypeID = CurrentItemTypeID() : sItmTypeID = IIf(Val(sItmTypeID) > 0, sItmTypeID, "NULL")
            sAttr1ID = CurrentAttribute1ID() : sAttr1ID = IIf(Val(sAttr1ID) > 0, sAttr1ID, "NULL")
            sAttr2ID = CurrentAttribute2ID() : sAttr2ID = IIf(Val(sAttr2ID) > 0, sAttr2ID, "NULL")
            sAttr3ID = CurrentAttribute3ID() : sAttr3ID = IIf(Val(sAttr3ID) > 0, sAttr3ID, "NULL")
            sAttr4ID = CurrentAttribute4ID() : sAttr4ID = IIf(Val(sAttr4ID) > 0, sAttr4ID, "NULL")
            sAttr5 = Trim(txtAttr5.Text)

            MyBase.MyData.GetConnection(cnn)
            sSQL = "UPDATE Customer_Document SET " & _
                   "Prefix = " & MyBase.MyData.SQLString(sPrefix) & ", " & _
                   "VendorID = " & sVendorID & ", " & _
                   "InActPrjDate = " & IIf(Len(sDate) > 0, MyBase.MyData.SQLString(sDate), "NULL") & ", " & _
                   "ItemTypeID = " & sItmTypeID & ", " & _
                   "OrderQuantityLimit = " & sOrderQuantityLimit & ", " & _
                   "Attr1ID = " & sAttr1ID & ", " & _
                   "Attr2ID = " & sAttr2ID & ", " & _
                   "Attr3ID = " & sAttr3ID & ", " & _
                   "Attr4ID = " & sAttr4ID & ", " & _
                   "Attr5 = " & MyBase.MyData.SQLString(sAttr5) & " " & _
                   "WHERE Customer_Document.ID = " & mlID
            bSuccess = (MyBase.MyData.SQLExecute(cnn, sSQL) = True)
            If (txtWarnLevel.Text <> "n/a") Then
                sSQL = "UPDATE Customer_Document_Fill SET " & _
                       "Cost = " & sPrice & ", " & _
                       "StockWarn = " & sWarnLev & " " & _
                       "WHERE Customer_Document_Fill.DocumentID = " & mlID
                bSuccess = bSuccess And MyBase.MyData.SQLExecute(cnn, sSQL)
            End If

        Catch ex As Exception
            MyBase.PageMessage = "Error attempting to Save Data..."
        End Try
        If Not bSuccess Then
            MyBase.PageMessage = "Error attempting to Save Data..."
        End If

        Return (bSuccess)
    End Function

    Private Sub cboItmType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboItmType.SelectedIndexChanged
        Dim lItmTypeID As Long = 0

        lItmTypeID = CurrentItemTypeID()
        LoadAttributes(lItmTypeID)
        LoadList(BuildSQLAttribute(lItmTypeID, 1), cboAttr1, 0)
        LoadList(BuildSQLAttribute(lItmTypeID, 2), cboAttr2, 0)
        LoadList(BuildSQLAttribute(lItmTypeID, 3), cboAttr3, 0)
        LoadList(BuildSQLAttribute(lItmTypeID, 4), cboAttr4, 0)
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdSave.Click
        Dim sDestin As String = paraPageBase.PAG_AdmItemSelect.ToString()

        If SaveData() Then
            MyBase.PageDirect(sDestin, 0, 0)
        End If
    End Sub

    Private Overloads Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click
        Dim sDestin As String = paraPageBase.PAG_AdmItemSelect.ToString()
        MyBase.PageDirect(sDestin, 0, 0)
    End Sub

End Class
