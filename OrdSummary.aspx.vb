Imports System.Text

Partial Public Class OrdSummary
    Inherits paraPageBase
    Private Const COL_ID = 0
    Private Const COL_CHK = 1

    Private Const COL_KEY = 2

    Private Const COL_ISTAT = 2
    Private Const COL_ISYM = 3
    Private Const COL_INO = 4
    Private Const COL_IDES = 5

    Private Const MSG_Lev0 = "No items exist with the current category selections."
    Private Const MSG_Lev1 = "Highlight the tags within each category and click ""VIEW SELECTIONS"" to filter the items which appear in the Item Selection Window."
    Private Const MSG_Lev2 = "Click on any line item in the Item Selection Window to view.  Check the box if the item is to be added to the cart."




    Dim intCustomHeadercount As Integer = 0
   



    Private Sub OrdSummary_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MyBase.Page_Init(sender, e)
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        PageTitle = "Order Summary"
        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then
            MyBase.PageDirect()

            'MyBase.Page_Load(sender, e)
            MyBase.PageMessage = ""
        ElseIf Not (MyBase.CurrentUser.CurrentUserReportHistoryVisibility = True) Then
            MyBase.PageDirect()
        End If

        If (Page.IsPostBack = False) Then
            MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri
        End If 'End PostBack


        If Page.IsPostBack = False Then
            hfSortField.Value = Session("Sort")
            Session("dt") = Nothing
            Dim paraD As New paraData
            paraD.CleanUp(CurrentUser, CurrentOrder)


            txtDate1.Text = Format(Now.Date.AddMonths(-1), "d")
            txtDate2.Text = Format(Now.Date, "d")
            Me.PopulateAccessCodes()
            Me.PopulateStatusCodes()
            'load filter settings
            If Not Request.Cookies("Filter") Is Nothing Then
                If IsDate(Request.Cookies("Filter").Values("DateFrom").ToString) Then
                    Dim d1, d2 As Date
                    d1 = Convert.ToDateTime(Request.Cookies("Filter").Values("DateFrom").ToString)
                    d2 = Convert.ToDateTime(Request.Cookies("Filter").Values("DateTo").ToString)

                    txtDate1.Text = Format(d1, "d")
                    txtDate2.Text = Format(d2, "d")

                    ddlAccessCode.SelectedValue = Request.Cookies("Filter").Values("Code").ToString
                    ddlStatus.SelectedValue = Request.Cookies("Filter").Values("Status").ToString
                End If
            End If



            'Me.divEditReviewOrder.Visible = False
            Me.divGrid.Visible = True
            grd.Columns(grd.Columns.Count - 1).Visible = CurrentUser.ShowPrice

            'Else


            '    DataBind()


        End If


        DataBindGrid(, grd.CurrentPageIndex)

    End Sub

    Sub PopulateAccessCodes()
        If CurrentUser.CurrentUserReportHistoryVisibility = "2" Then
            Dim taAccessCode As New dsOrdersTableAdapters.Customer_AccessCodeTableAdapter
            ddlAccessCode.AppendDataBoundItems = True
            Dim li As New ListItem
            li.Value = "%"
            li.Text = "All"
            li.Selected = True
            ddlAccessCode.Items.Add(li)
            ddlAccessCode.DataSource = taAccessCode.GetAccessCodesByCustomerID(CurrentUser.CustomerID)
            ddlAccessCode.DataTextField = "Code"
            ddlAccessCode.DataValueField = "ID"

            ddlAccessCode.DataBind()
            'If CurrentUser.AccessCode <> "" Then
            '    ddlAccessCode.Items.FindByText(CurrentUser.AccessCode).Selected = True
            'End If
        ElseIf CurrentUser.CurrentUserReportHistoryVisibility = "1" Then
            ddlAccessCode.Items.Clear()
            Dim li As New ListItem
            li.Selected = True
            li.Text = CurrentUser.AccessCode
            li.Value = CurrentUser.AccessCodeID
            ddlAccessCode.Items.Add(li)
        Else
            ddlAccessCode.Items.Clear()
        End If

    End Sub

    Private Sub PopulateStatusCodes()
        Dim taStatuses As New dsStatusesTableAdapters.StatusTableAdapter
        ddlStatus.AppendDataBoundItems = True
        Dim li As New ListItem
        li.Value = "%"
        li.Text = "All"
        li.Selected = True
        ddlStatus.Items.Add(li)
        Me.ddlStatus.DataSource = taStatuses.GetData
        Me.ddlStatus.DataTextField = "Description"
        Me.ddlStatus.DataValueField = "Id"
        Me.ddlStatus.DataBind()
    End Sub

    Sub DataBindGrid(Optional ByVal sSortField As String = "", Optional ByVal intNewPage As Integer = 0, Optional ByVal blnDesc As Boolean = False, Optional ByVal blnForceRefresh As Boolean = False)
        Dim ta As New dsOrdersTableAdapters.dsOrdersWithAccessCodesTableAdapter
        Dim dt As New dsOrders.dsOrdersWithAccessCodesDataTable
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter

        grd.CurrentPageIndex = intNewPage

        Dim intAccessCode As Integer = 0
        'If IsNumeric(ddlAccessCode.SelectedValue) Then intAccessCode = CInt(ddlAccessCode.SelectedValue)
        Dim d1, d2 As Date
        Dim decOrderCost As Decimal = 0
        d1 = txtDate1.Text 'Convert.ToDateTime(txtDate1.Text)
        d2 = txtDate2.Text 'Convert.ToDateTime(txtDate2.Text)

        dt = ta.GetOrdersByCustomerID(ddlAccessCode.SelectedValue, ddlStatus.SelectedValue, CurrentUser.CustomerID, d1, d2)



        '----add custom fields
        Dim iStartCell As Integer = 3
        Dim taCustom As New dsOrdersTableAdapters.Customer_CustomFieldTableAdapter

        Dim blnColumnExists As Boolean = False
        Dim blnCustomField As Boolean = False

        For Each dr As dsOrders.Customer_CustomFieldRow In taCustom.GetDisplayCustomFields(CurrentUser.CustomerID)

            Dim dc As New System.Data.DataColumn
            dc.ColumnName = dr.Name
            dc.DefaultValue = dr.ID
            dc.Unique = False
            dc.Caption = "CustomField"
            dt.Columns.Add(dc)

            blnColumnExists = CheckIfColumnExists(dr.Name)
            blnCustomField = True

            If blnColumnExists = False Then
                Dim dgc As New BoundColumn
                dgc.HeaderText = dr.Name
                dgc.SortExpression = "[" & dr.Name & "]"
                dgc.DataField = dr.Name

                grd.Columns.AddAt(3, dgc)
            End If


        Next

        hfCustomField.value = blnCustomField

        If IsNothing(Session("dt")) Or blnForceRefresh = True Then

            intCustomHeadercount = iStartCell - 1
            '---------------------
            Dim blnRebind As Boolean = True
            If blnCustomField = False Then
                blnRebind = True
            ElseIf Page.IsPostBack = False Then
                blnRebind = True
            ElseIf blnColumnExists Then
                blnRebind = True
            Else
                blnRebind = False
            End If

            If blnRebind Then
                For Each dr As dsOrders.dsOrdersWithAccessCodesRow In dt.Rows
                    Dim dr1 As DataRow = dr

                    decOrderCost = qa.GetOrderTotal(dr.ID)
                    If decOrderCost > 0 And dr.StatusID = 3 Then
                        dr.Cost = decOrderCost
                    Else
                        dr.Cost = 0
                    End If

                    dr.ShippingMethod = qa.GetShippingMethodName(CurrentUser.CustomerID, dr.ActualShipID) & " " & qa.GetShippingMethodDescription(dr.ActualShipID)


                    For Each dc As DataColumn In dr.Table.Columns
                        If dc.Caption = "CustomField" Then
                            Dim strVal As String = qa.GetCustomFieldValue(dr.ID, CInt(dc.DefaultValue))
                            dr.Item(dc.ColumnName) = strVal

                        End If
                    Next
                Next
            End If


            Session("dt") = dt
        Else
            dt = Session("dt")
        End If




        Dim dv As New DataView(dt)



        'If (sSortField <> "") Then
        '    'If (sSortField = Session("Sort")) And blnDesc = False And InStr(sSortField, " DESC") = 0 Then
        '    '    dv.Sort = sSortField & " DESC"
        '    'Else
        '    If hfSort.Value = 2 Then
        '        dv.Sort = sSortField & " DESC"
        '    Else
        '        dv.Sort = sSortField
        '    End If

        '    'End If
        'End If

        If hfSortField.Value <> "" Then
            dv.Sort = hfSortField.Value

        End If

        Session("Sort") = hfSortField.Value

        grd.DataSource = dv

        grd.DataBind()
        If blnCustomField = False Then
            Session("Sort") = dv.Sort
        ElseIf blnColumnExists Then
            Session("Sort") = dv.Sort
        End If


        'save settings
        Dim ckFilter As New HttpCookie("Filter")

        ckFilter.Values("DateFrom") = d1.ToString
        ckFilter.Values("DateTo") = d2.ToString
        ckFilter.Values("Code") = ddlAccessCode.SelectedValue
        ckFilter.Values("Status") = ddlStatus.SelectedValue

        Response.Cookies.Add(ckFilter)
        lblTest.Text = dv.Sort

    End Sub



    Private Sub grd_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grd.PageIndexChanged
        If Not IsNothing(Session("Sort")) Then
            DataBindGrid(Session("Sort").ToString, e.NewPageIndex, True)
        Else
            DataBindGrid("", e.NewPageIndex, True)
        End If

    End Sub

    Function CheckIfColumnExists(ByVal strHeaderText As String) As Boolean
        Dim bln As Boolean = False
        For Each ctl As DataGridColumn In grd.Columns
            If ctl.HeaderText = strHeaderText Then
                bln = True
                Exit For
            End If
        Next

        Return bln
    End Function
    Private Sub grd_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grd.ItemCommand
        Select Case e.CommandName
            Case "EditItem"



                'divGrid.Visible = False
                'divEditReviewOrder.Visible = True
                PopulateOrderDetails(e.CommandArgument)
                'Me.txtSelectedOrder.Text = e.CommandArgument
            Case "Sort"
                'If hfCustomField.value Then
                '    If Not IsNothing(Session("Sort")) Then
                'DataBind(Session("Sort").ToString, grd.CurrentPageIndex, False)
                If hfSortField.Value.ToString.Contains(e.CommandArgument) = False Then
                    hfSortField.Value = e.CommandArgument
                    hfSort.Value = 1
                End If

                Select Case hfSort.value
                    Case "1"
                        hfSortField.Value = e.CommandArgument
                        hfSort.Value = 2
                    Case "2"
                        hfSortField.Value = e.CommandArgument & " DESC"
                        hfSort.Value = 1
                End Select


                DataBindGrid(e.CommandArgument, grd.CurrentPageIndex, False)
                '    End If
                'End If

        End Select
    End Sub



    Sub PopulateOrderDetails(ByVal intOrderID As Integer)
        Dim ta As New dsOrdersTableAdapters.dsOrdersWithAccessCodesTableAdapter
        Dim taCS As New dsOrdersTableAdapters.Customer_CoverSheetTableAdapter
        Dim taKits As New dsOrdersTableAdapters.dsOrderKitDetailsTableAdapter
        Dim taDetails As New dsOrdersTableAdapters.dsOrderDetailsCustomTableAdapter
        Dim taOrderCustomFields As New dsOrdersTableAdapters.dtOrderCustomFieldsTableAdapter
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        Dim strWebSet As String = ""
        Dim pOrder As New paraOrder
        'gvOrderItems.DataSource = taDetails.GetByOrderID(intOrderID)

        'gvOrderItems.DataBind()

        'hfOrderID.Value = intOrderID
        For Each dr As dsOrders.dsOrdersWithAccessCodesRow In ta.GetByOrderID(intOrderID)
            With MyBase.CurrentOrder

                For Each drCS As dsOrders.Customer_CoverSheetRow In taCS.GetByCoverSheetID(dr.CoverSheetID)

                    .CoverSheetDesc = drCS.Name
                    .CoverSheetID = dr.CoverSheetID
                    .CoverSheetText = drCS.Note
                Next

                .CustomerEmail = dr.Email
                .RequestorEmail = dr.Requestor_Email
                .RequestorFirstName = dr.Requestor_FirstName
                .RequestorLastName = dr.Requestor_LastName

                Try

                Catch ex As Exception
                    .RequestorTitle = dr.Requestor_Title
                End Try
                Try
                    .ShipAddress1 = dr.ShipTo_Address1
                Catch ex As Exception

                End Try
                Try
                    .ShipAddress2 = dr.ShipTo_Address2
                Catch ex As Exception

                End Try

                .ShipCity = dr.ShipTo_City
                .ShipCompany = dr.ShipTo_Name
                .ShipContactFirstName = dr.ShipTo_ContactFirstName
                .ShipContactLastName = dr.ShipTo_ContactLastName
                .ShipCountry = dr.ShipTo_Country

                If dr.IsRecipientEmailNull = False Then
                    .RecipientEmail = dr.RecipientEmail
                End If
                If dr.IsRecipientPhoneNull = False Then
                    .RecipientPhone = dr.RecipientPhone
                End If
    

                Try
                    .ShipNote = dr.ShipNote
                Catch ex As Exception

                End Try

                .ShipState = dr.ShipTo_State
                .ShipZip = dr.ShipTo_ZipCode
                .ShipPrefID = dr.PreferredShipID
                .ShipPrefDesc = qa.GetShippingMethodName(CurrentUser.CustomerID, dr.PreferredShipID)


                .EditOrder = True
                .ItmCart = taDetails.GetData(intOrderID)
                .KitCart = taKits.GetData(intOrderID)
                .OrderID = intOrderID
                Try
                    .CoverSheetText = dr.CS_Content()
                Catch ex As Exception

                End Try

                .StatusID = dr.StatusID
                .CstCart = pOrder.GetCustomFields(dr.CustomerID, True, intOrderID) 'taOrderCustomFields.GetData(intOrderID)
                .AccessCodeID = dr.AccessCodeID
                .AccessCode = qa.GetAccessCode(dr.AccessCodeID)
                .DisableAllQuantityLimits = qa.GetDisableAllQuantityLimitsByAccessCodeID(dr.AccessCodeID)

                strWebSet = qa.GetWebSet(dr.AccessCodeID)

                If .DisableAllQuantityLimits Then
                    .CanExtendQtyLI = True
                    .CanExtendQtyOR = True
                Else
                    If (Len(strWebSet) > 0) Then
                        .CanExtendQtyLI = (Mid(strWebSet, 6, 1) = 1)
                        .CanExtendQtyOR = (Mid(strWebSet, 7, 1) = 1)
                    End If
                End If


                Try
                    If dr("RequiredDate") Is DBNull.Value = False Then
                        .ScheduledDelivery = dr.RequiredDate
                    End If



                Catch ex As Exception

                End Try

                .MaxQtyPerOrder = qa.GetMaxQtyPerOrder(dr.AccessCodeID)
                .MaxQtyPerLineItem = qa.GetMaxQtyPerLineItem(dr.AccessCodeID)
            End With

        Next
        'Dim o As New paraOrder
        'o.PopulateOrderData(intOrderID, CurrentUser)


        Response.Redirect("OrdSve.aspx")



    End Sub

    Function GetOrderKitDetails(ByVal intOrderID As Integer) As DataTable
        'order kits


        'sID = .Item("KitID").ToString()
        'sName = .Item("KitName").ToString()
        'sDesc = .Item("KitDesc").ToString()
        'sColor = .Item("KitColor").ToString()
        ''MyBase.MyData.GetKitDetail(MyBase.CurrentUser.CustomerID, Val(sID), sName, sDesc)
        'lQty = Val(.Item("KitQty").ToString())


        Dim objDT As System.Data.DataTable
        Dim objDR As System.Data.DataRow

        objDT = New System.Data.DataTable("CstCart")
        objDT.Columns.Add("ID", GetType(Integer))
        objDT.Columns("ID").AutoIncrement = True
        objDT.Columns("ID").AutoIncrementSeed = 1

        objDT.Columns.Add("FieldID", GetType(Integer))
        objDT.Columns.Add("FieldRequire", GetType(Integer))
        objDT.Columns.Add("FieldEntry", GetType(Integer))
        objDT.Columns.Add("FieldName", GetType(String))
        objDT.Columns.Add("FieldValue", GetType(String))
        objDT.Columns.Add("UserEntry", GetType(String))
        objDT.Columns.Add("FieldType", GetType(String))


        Dim myData As paraData
        'Dim cnn As OleDb.OleDbConnection
        'Dim cmd As OleDb.OleDbCommand
        'Dim dr As OleDb.OleDbDataReader
        Dim sSQL As String
        Dim indx As Integer
        Dim sID As String
        Dim sName As String
        Dim bRequire As Boolean
        Dim bEntry As Boolean
        Dim sEntry As String
        Dim iType As Int16
        Dim taDetails As New dsOrdersTableAdapters.DataTable1TableAdapter

        For Each dr As dsOrders.DataTable1Row In taDetails.GetDetailsData(CurrentUser.CustomerID).Rows
            indx = indx + 1

            sID = dr("ID").ToString()
            sName = dr("Name").ToString()
            bRequire = Convert.ToBoolean(dr("Require").ToString())
            bEntry = Convert.ToBoolean(dr("Entry").ToString())
            sEntry = IIf(dr("SelectCnt") > 0, "Select", "Type")
            iType = dr("CustomType")

            objDR = objDT.NewRow()
            objDR("FieldID") = sID
            objDR("FieldRequire") = bRequire
            objDR("FieldEntry") = bEntry
            objDR("FieldName") = sName
            objDR("FieldValue") = ""
            objDR("UserEntry") = sEntry
            objDR("FieldType") = iType

            objDT.Rows.Add(objDR)
        Next


        Return objDT

    End Function

    Function GetOrderDetails(ByVal intOrderID As Integer) As DataTable
        'order details
        Dim objDT As System.Data.DataTable
        Dim objDR As System.Data.DataRow

        objDT = New System.Data.DataTable("CstCart")
        objDT.Columns.Add("ID", GetType(Integer))
        objDT.Columns("ID").AutoIncrement = True
        objDT.Columns("ID").AutoIncrementSeed = 1

        objDT.Columns.Add("FieldID", GetType(Integer))
        objDT.Columns.Add("FieldRequire", GetType(Integer))
        objDT.Columns.Add("FieldEntry", GetType(Integer))
        objDT.Columns.Add("FieldName", GetType(String))
        objDT.Columns.Add("FieldValue", GetType(String))
        objDT.Columns.Add("UserEntry", GetType(String))
        objDT.Columns.Add("FieldType", GetType(String))


        Dim myData As paraData
        'Dim cnn As OleDb.OleDbConnection
        'Dim cmd As OleDb.OleDbCommand
        'Dim dr As OleDb.OleDbDataReader
        Dim sSQL As String
        Dim indx As Integer
        Dim sID As String
        Dim sName As String
        Dim bRequire As Boolean
        Dim bEntry As Boolean
        Dim sEntry As String
        Dim iType As Int16
        Dim taDetails As New dsOrdersTableAdapters.DataTable1TableAdapter

        For Each dr As dsOrders.DataTable1Row In taDetails.GetDetailsData(CurrentUser.CustomerID).Rows
            indx = indx + 1

            sID = dr("ID").ToString()
            sName = dr("Name").ToString()
            bRequire = Convert.ToBoolean(dr("Require").ToString())
            bEntry = Convert.ToBoolean(dr("Entry").ToString())
            sEntry = IIf(dr("SelectCnt") > 0, "Select", "Type")
            iType = dr("CustomType")

            objDR = objDT.NewRow()
            objDR("FieldID") = sID
            objDR("FieldRequire") = bRequire
            objDR("FieldEntry") = bEntry
            objDR("FieldName") = sName
            objDR("FieldValue") = ""
            objDR("UserEntry") = sEntry
            objDR("FieldType") = iType

            objDT.Rows.Add(objDR)
        Next


        Return objDT

    End Function


    Private Sub grd_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grd.ItemDataBound



        'e(-griditem)



        'Dim dgc As New BoundColumn
        'dgc = CType(sender, DataGrid).Columns.Item(4)
        'If dgc.DataField = "Program Name" Then
        '    Response.Write(dgc.SortExpression & "<BR>")
        'End If

        'Select Case e.Item.ItemType



        '    Case ListItemType.Header
        '        Dim bln As Boolean = True

        '        'For Each tc As TableCell In e.Item.Cells

        '        '    'If tc.Text = "ID" Then



        '        '    'End If
        '        '    'Response.Write("<HR>")
        '        '    'If tc.Controls.Count > 1 Then
        '        '    '    For Each ctl As Control In tc.Controls
        '        '    '        Response.Write(ctl.GetType.ToString & "<BR>")
        '        '    '    Next
        '        '    '    bln = False
        '        '    'End If

        '        '    tc.Visible = bln

        '        '    intCustomHeadercount += 1
        '        'Next

        '    Case Else
        '        'For Each tc As TableCell In e.Item.Cells
        '        '    If tc.Text = "ID" Then
        '        '        tc.Visible = False
        '        '    End If

        '        'Next
        'End Select


    End Sub



    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGo.Click
        If Page.IsValid() Then
            DataBindGrid("", 0, False, True)
        End If
    End Sub

    Private Sub grd_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles grd.SortCommand
        DataBindGrid(e.SortExpression)
    End Sub




    
    Function CheckVisibleDetails(ByVal intStatusID As Integer) As Boolean

        Select Case intStatusID
            Case 3
                Return True
            Case Else

                Return False
        End Select
    End Function

    Function GetOrderTotal(ByVal intOrderID As Integer) As Decimal
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        Return qa.GetOrderTotal(intOrderID)
    End Function
    Function getShipMethodName(ByVal intShipID As Integer) As String
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        Return qa.GetShippingMethodName(CurrentUser.CustomerID, intShipID)

    End Function
    Function GetPieces(ByVal intOrderID As Integer) As Integer
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        Dim obj As Object = qa.GetItemQtyInOrder(intOrderID)
        If IsNothing(obj) Then
            Return 0
        Else
            Return obj.ToString
        End If

    End Function
    Function GetKitQty(ByVal intOrderID As Integer) As Integer
        Dim ta As New dsOrdersTableAdapters.Order_DocumentTableAdapter
        Dim intTemp As Integer = 0


        For Each dr As dsOrders.Order_DocumentRow In ta.GetDataByOrderID(intOrderID)
            intTemp += dr.QtyOrdered
        Next

        Return intTemp
    End Function
End Class
