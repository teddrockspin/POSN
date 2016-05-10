<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="videoHandler.aspx.vb" Inherits="POSN.videoHandler" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="flowplayer/flowplayer-3.2.12.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>

			<!-- this A tag is where your Flowplayer will be placed. it can be anywhere -->
        
		<a href="<%=VideoLoc%>"
			 style="display:block;width:520px;height:330px"  
			 id="player"> 
		</a> 
	
		<!-- this will install flowplayer inside previous A- tag. -->
		<script>
		    flowplayer("player", "flowplayer/flowplayer-3.2.16.swf");
		</script>
	
			
	</div>
    </form>
</body>
</html>
