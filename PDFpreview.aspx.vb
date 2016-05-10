Public Class PDFpreview
    Inherits System.Web.UI.Page


    ' ConfigurationManager.AppSettings("sitpath") & "\PDFS\" & ItemID & "\preview\"
    'Public pdfurl = "/PDFs/" & Request.QueryString("id") & "/AC 0005ABR_L.pdf/preview/" & Request.QueryString("id") & ".pdf"
    Public pdfurl As String = ""
    Public pdfurlgoogle As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNumeric(Request.QueryString("id")) Then
            'retrieve preview pdf:
            'pdfurl = ConfigurationSettings.AppSettings("HomeAddress") & "/" & ConfigurationSettings.AppSettings("SiteLocation") & "/pdfs/" & Request.QueryString("id") & "/preview/" & Request.QueryString("id") & ".pdf"  'fetchpdf.aspx?id=" & Request.QueryString("id")
            pdfurl = "pdfs/" & Request.QueryString("id") & "/preview/" & Request.QueryString("id") & ".pdf"
            pdfurlgoogle = "https://docs.google.com/viewer?url=" & Server.UrlEncode(pdfurl)
        End If
    End Sub
    
End Class