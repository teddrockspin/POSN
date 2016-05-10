Public Class videoHandler
    Inherits System.Web.UI.Page
    Public VideoLoc As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim itemID As String = Request.QueryString("ItemID").ToString()
        VideoLoc = ConfigurationManager.AppSettings("HomeAddress") & "/" & ConfigurationManager.AppSettings("SiteLocation") & "/PDFS/" & itemID & "/preview/" & itemID & ".flv"

    End Sub

End Class