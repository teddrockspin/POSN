Public Partial Class emptyOrders
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ta As New dsOrdersTableAdapters.OrderTableAdapter

        gvEmptyOrders.DataSource = ta.GetEmptyOrders
        gvEmptyOrders.DataBind()

        'PopulateOrderPO()
      
    End Sub


    Sub PopulateOrderPO()
        Dim ta As New dsOrdersTableAdapters.OrderTableAdapter
        Dim qa As New dsOrdersTableAdapters.QueriesTableAdapter
        For Each dr As dsOrders.OrderRow In ta.GetAllOrdersWithPOs
            Dim strPO As String = qa.GetPurchaseReferenceByOrderNumber(dr.ID)

            dr.PurchaseReference = strPO

            ta.Update(dr)
        Next

        Response.Write("done updating PO")
    End Sub
End Class