Public Partial Class ordReviewLanding
    Inherits paraPageBase
    Private Sub OrdSummary_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then

        If Page.IsPostBack = False Then
            If Request.QueryString("RNUMB") <> "" AndAlso IsNumeric(Request.QueryString("RNUMB")) Then
                CheckAuth(Request.QueryString("RNUMB"))
            End If

            If Request.QueryString("RN") <> "" AndAlso IsNumeric(Request.QueryString("RN")) Then
                CheckAuth(Request.QueryString("RN"))
            End If
        End If


        'MyBase.PageDirectWithTarget("login.aspx?target=" & Server.UrlEncode("ordReviewLanding.aspx?" & Request.ServerVariables("QUERY_STRING")))

        'MyBase.Page_Load(sender, e)
        MyBase.PageMessage = ""
        'Else
        'If (Page.IsPostBack = False) Then
        '    MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri
        'End If
        'If IsNumeric(Request.QueryString("RNUMB")) Then
        '    PopulateOrderDetails(Request.QueryString("RNUMB"))
        'End If
        'If IsNumeric(Request.QueryString("RN")) Then
        '    PopulateOrderDetails(Request.QueryString("RN"))
        'End If
        'End If
        MyBase.Page_Init(sender, e)
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Request.QueryString("RN").ToString <> "" Then

        PageTitle = "Order Summary"


  


        If (MyBase.CurrentUser.State = MyBase.MyData.STATE_LoggedOut) Then

            If Page.IsPostBack = False Then
                If Request.QueryString("RNUMB") <> "" AndAlso IsNumeric(Request.QueryString("RNUMB")) Then
                    CheckAuth(Request.QueryString("RNUMB"))
                End If

                If Request.QueryString("RN") <> "" AndAlso IsNumeric(Request.QueryString("RN")) Then
                    CheckAuth(Request.QueryString("RN"))
                End If
            End If


            'MyBase.PageDirectWithTarget("login.aspx?target=" & Server.UrlEncode("ordReviewLanding.aspx?" & Request.ServerVariables("QUERY_STRING")))

            'MyBase.Page_Load(sender, e)
            MyBase.PageMessage = ""
        Else
            If Request.QueryString("id") <> "" Then
                Response.Redirect("GetFile.aspx?id=" & Request.QueryString("id") & "&fid=" & Request.QueryString("fid"), False)
            Else

                If (Page.IsPostBack = False) Then
                    MyBase.CurrentUser.CurrentPage = Request.Url.AbsoluteUri
                End If
                If IsNumeric(Request.QueryString("RNUMB")) Then
                    PopulateOrderDetails(Request.QueryString("RNUMB"))
                End If
                If IsNumeric(Request.QueryString("RN")) Then
                    PopulateOrderDetails(Request.QueryString("RN"))
                End If
            End If
        End If



        'End If

    End Sub

    Sub CheckAuth(ByVal intOrderNumber As Integer)

        Dim objTmp As Object
        Dim qa As New dsAccessCodesTableAdapters.QueriesTableAdapter


        objTmp = qa.GetAuthAccesCode(intOrderNumber)

        If IsNothing(objTmp) = False AndAlso objTmp.ToString <> "" Then

            Dim sAccCode As String = objTmp.ToString
            Dim ta As New dsOrdersTableAdapters.dsOrdersWithAccessCodesTableAdapter
            Dim dr As dsOrders.dsOrdersWithAccessCodesRow
            Dim taAC As New dsAccessCodesTableAdapters.Customer_AccessCode1TableAdapter

            Dim drAC As dsAccessCodes.Customer_AccessCode1Row
            Dim sMSG As String = ""

            Try
                dr = ta.GetByOrderID(intOrderNumber).Rows(0)
                drAC = taAC.GetDataByCustomerIDandCode(dr.CustomerID, sAccCode).Rows(0)

                If MyBase.MyData.VerifyUser(drAC.CustomerCode, drAC.Pwd, sAccCode, sMSG) Then
                    dr = Nothing
                    ta.Dispose()
                    drAC = Nothing
                    taAC.Dispose()
                    qa.Dispose()
                    Dim strTmp As String = Request.ServerVariables("QUERY_STRING")
                    Response.Redirect("ordReviewLanding.aspx?" & strTmp, False)
                    'Server.Transfer("ordReviewLanding.aspx?" & strTmp)



                    ' PopulateOrderDetails(intOrderNumber)
                Else
                    dr = Nothing
                    ta.Dispose()
                    drAC = Nothing
                    taAC.Dispose()
                    qa.Dispose()
                End If

            Catch ex As Exception
                Response.Redirect("Login.aspx?errmsg=" & ex.Message)
                'Response.Redirect("OrdSve.aspx")
            End Try



        End If
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
        Dim strOrderMsg As String = ""
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
                Try
                    .ShipNote = dr.ShipNote
                Catch ex As Exception

                End Try

                .ShipState = dr.ShipTo_State
                .ShipZip = dr.ShipTo_ZipCode
                .ShipPrefID = dr.PreferredShipID
                .ShipPrefDesc = qa.GetShippingMethodName(CurrentUser.CustomerID, dr.PreferredShipID)
                If dr.IsRecipientEmailNull = False Then
                    .RecipientEmail = dr.RecipientEmail
                End If
                If dr.IsRecipientPhoneNull = False Then
                    .RecipientPhone = dr.RecipientPhone
                End If



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


                .MaxQtyPerOrder = qa.GetMaxQtyPerOrder(dr.AccessCodeID)
                .MaxQtyPerLineItem = qa.GetMaxQtyPerLineItem(dr.AccessCodeID)
            End With


            If dr.StatusID <> 6 Then
                strOrderMsg = "Please note: this order has already been activated."
                '<option value="9">Back Ordered Items</option>
                '<option value="4">Cancelled</option>
                '<option value="2">Completed</option>
                '<option value="7">Future Shipment</option>
                '<option selected="selected" value="6">Hold</option>
                '<option value="5">Partial</option>
                '<option value="3">Shipped</option>
                '<option value="8">Stand By</option>

                Select Case dr.StatusID
                    Case 9, 2, 7, 5, 8
                        strOrderMsg = "Please note: this order has already been released."
                    Case 4
                        strOrderMsg = "Please note: this order has been cancelled."
                    Case 3
                        strOrderMsg = "Please note: this order has already been released and shipped."
                End Select

            End If
        Next
        'Dim o As New paraOrder
        'o.PopulateOrderData(intOrderID, CurrentUser)
       

        Response.Redirect("OrdSve.aspx?ordermsg=" & strOrderMsg)


    End Sub

  
End Class