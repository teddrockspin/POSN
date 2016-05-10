<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PDFpreview.aspx.vb" Inherits="POSN.PDFpreview" %>

<!DOCTYPE html>
<!-- saved from url=(0047)http://mozilla.github.io/pdf.js/web/viewer.html -->
<html dir="ltr" mozdisallowselectionprint="" moznomarginboxes="">
<script id="tinyhippos-injected">if (window.top.ripple) { window.top.ripple("bootstrap").inject(window, document); }</script>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <title>Preview Document</title>


    <link rel="stylesheet" href="http://mozilla.github.io/pdf.js/web/viewer.css">
    <script src="./js/jquery.min.js"></script>
    <script type="text/javascript" src="./js/compatibility.js"></script>
    <style type="text/css"></style>



    <!-- This snippet is used in production, see Makefile -->
    <!--<link rel="resource" type="application/l10n" href="js/locale.properties">-->
    <script type="text/javascript" src="./js/l10n.js"></script>
    <script type="text/javascript" src="./js/pdf.js"></script>


    <script type="text/javascript" src="./js/debugger.js"></script>
    <%--<script type="text/javascript" src="./js/viewer.js"></script>--%>
    <script>
        var DEFAULT_URL = '<%=pdfurl%>';
        

        $(function () {
            $("#closethiswindow").on("click", function (e) {
                window.close();;
                e.preventDefault();
            });
        });
    </script>
    <style type="text/css">
        .textButton {
            -webkit-transition-property: background-color, border-color, box-shadow;
            -webkit-transition-duration: 150ms;
            -webkit-transition-timing-function: ease;
            -moz-transition-property: background-color, border-color, box-shadow;
            -moz-transition-duration: 150ms;
            -moz-transition-timing-function: ease;
            -ms-transition-property: background-color, border-color, box-shadow;
            -ms-transition-duration: 150ms;
            -ms-transition-timing-function: ease;
            -o-transition-property: background-color, border-color, box-shadow;
            -o-transition-duration: 150ms;
            -o-transition-timing-function: ease;
            transition-property: background-color, border-color, box-shadow;
            transition-duration: 150ms;
            transition-timing-function: ease;
            color: #ffffff;
            padding: 5px;
            position:absolute;
            left:450px;
        }
    </style>
    <script type="text/javascript" src="./js/viewer.js"></script>
    <style id="PDFJS_FONT_STYLE_TAG"></style>
</head>

<body tabindex="1">
    <a href="#" class="textButton" id="closethiswindow">Close this window</a>
    <iframe id="viewer" src = "./Viewer.js/#../<%=pdfurl %>" width='100%' height='100%' allowfullscreen webkitallowfullscreen>
    
</body>
</html>
