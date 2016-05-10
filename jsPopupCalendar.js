var popCalDstFld;
var temp;
var popCalWin;

function popupCal()
{
	var tmpDate         = new Date();
	var tmpString       = "";
	var tmpNum          = 0;
	var popCalDateVal;
	var dstWindowName   = "";

	//Initialize the window to an empty object.
	popCalWin = new Object();
	
	dstWindowName = popupCal.arguments[0];
	temp = popupCal.arguments[1];
		
	popCalDstFld = document.getElementById(popupCal.arguments[1]);

	popCalDstFmt = "M/d/yyyy";
	popCalMonths = "January,February,March,April,May,June,July,August,September,October,November,December";
 	popCalDays = "Sun,Mon,Tue,Wed,Thu,Fri,Sat";
	popCalToday = "Today";
	popCalClose = "Close";
	popCalTitle = "Calendar";
	popCalFirstDayWeek = 0;
 
	tmpString = new String(popCalDstFld.value);  
	
	//If tmpString is empty (meaning that the field is empty) 
	//use todays date as the starting point
	if(tmpString == "")
		popCalDateVal = new Date()
	else
	{
		//Make sure the century is included, if it isn't, add this 
		//century to the value that was in the field
		tmpNum = tmpString.lastIndexOf( "/" );
		if ( (tmpString.length - tmpNum) == 3 )
		{
			tmpString = tmpString.substring(0,tmpNum + 1)+"20"+tmpString.substr(tmpNum+1);
			popCalDateVal = new Date(getFirstDate(tmpString));
			alert(popCalDateVal);
		}
		else
		{
			popCalDateVal = new Date(getFirstDate(tmpString));
		}
	}
	
	if( popCalDateVal.toString() == "NaN" || popCalDateVal.toString() == "0")
	{
		popCalDateVal = new Date();
		popCalDstFld.value = "";
	}
			
	var dateString = String(popCalDateVal.getMonth()+1) + "/" + String(popCalDateVal.getDate()) + "/" + String(popCalDateVal.getFullYear());

	//Call the routine to draw the initial calendar
	reloadCalPopup(dateString, dstWindowName);
	
	return void(0);
}
 
function closeCalPopup()
{
	popCalWin.close();
	return void(0);
}
 
function reloadCalPopup() //[0]dateString, [1]dstWindowName
{
	//Set the window's features here

	var windowFeatures = "toolbar=no, location=no, status=no, menubar=no, scrollbars=no, resizable=no, height=250, width=270, top=" + ((screen.height - 270)/2).toString()+",left="+((screen.width - 270)/2).toString();
	var tmpDate = new Date( reloadCalPopup.arguments[0] );
	
	if (tmpDate.toString() == "Invalid Date")
	    tmpDate = new Date();
	
	tmpDate.setDate(1);
	
	//Get the calendar data
	var popCalData = calPopupSetData(tmpDate,reloadCalPopup.arguments[1]);
 
	//Check to see if the window has been initialized, create it if it hasn't been
	if( popCalWin.toString() == "[object Object]" )
	{
		popCalWin = window.open("",reloadCalPopup.arguments[1],windowFeatures);
		popCalWin.opener = self;
		// Window im Vordergrund
		popCalWin.focus();
	}
	else 
	{
    popCalWin.document.close();
		popCalWin.document.clear();
  }
	
	//this is the line with the big problem
  popCalWin.document.write(popCalData);
	return void(1);
}
 
function calPopupSetData(firstDay,dstWindowName)
{
	var popCalData = "";
    var lastDate = 0;
	var fnt = new Array( "<FONT SIZE=\"1\">", "<B><FONT SIZE=\"2\">", "<FONT SIZE=\"2\" COLOR=\"#EF741D\"><B>");
	var dtToday = new Date();
	var thisMonth = firstDay.getMonth();
	var thisYear = firstDay.getFullYear();
	var nPrevMonth = (thisMonth == 0 ) ? 11 : (thisMonth - 1);
	var nNextMonth = (thisMonth == 11 ) ? 0 : (thisMonth + 1);
	var nPrevMonthYear = (nPrevMonth == 11) ? (thisYear - 1): thisYear;
	var nNextMonthYear = (nNextMonth == 0) ? (thisYear + 1): thisYear;
	var sToday = String((dtToday.getMonth()+1) + "/01/" + dtToday.getFullYear());
	var sPrevMonth = String((nPrevMonth+1) + "/01/" + nPrevMonthYear);
	var sNextMonth = String((nNextMonth+1) + "/01/" + nNextMonthYear);
	var sPrevYear1 = String((thisMonth+1) + "/01/" + (thisYear - 1));
	var sNextYear1 = String((thisMonth+1) + "/01/" + (thisYear + 1));
 	var tmpDate = new Date( sNextMonth );
	
	tmpDate = new Date( tmpDate.valueOf() - 1001 );
	lastDate = tmpDate.getDate();

	if (this.popCalMonths.split) // javascript 1.1 defensive code
	{
		var monthNames = this.popCalMonths.split(",");
		var dayNames = this.popCalDays.split(",");
	}
	else  // Need to build a js 1.0 split algorithm, default English for now
	{
		var monthNames = new Array("January","February","March","April","May","June","July","August","September","October","November","December");
		var dayNames = new Array("Sun","Mon","Tue","Wed","Thu","Fri","Sat")
	}

 	var styles = "<style><!-- body{font-family:Arial,Helvetica,sans-serif;font-size:9pt}; td {  font-family: Arial, Helvetica, sans-serif; font-size: 9pt; color: #666666}; A { text-decoration: none; };TD.day { border-bottom: solid black; border-width: 0px; }--></style>"
	var cellAttribs = "align=\"center\" class=\"day\" BGCOLOR=\"#F1F1F1\"onMouseOver=\"temp=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC';\" onMouseOut=\"this.style.backgroundColor=temp;\""
	var cellAttribs2 = "align=\"center\" BGCOLOR=\"#F1F1F1\" onMouseOver=\"temp=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC';\" onMouseOut=\"this.style.backgroundColor=temp;\""
	var htmlHead = "<HTML><HEAD><TITLE>"+popCalTitle+"</TITLE>" + styles + "</HEAD><BODY BGCOLOR=\"#F1F1F1\" TEXT=\"#000000\" LINK=\"#364180\" ALINK=\"#FF8100\" VLINK=\"#424282\">";
	var htmlTail = "</BODY></HTML>";
	var closeAnchor = "<CENTER><input type=button value=\""+popCalClose+"\" onClick=\"javascript:window.opener.closeCalPopup()\"></CENTER>";            
	var todayAnchor = "<A HREF=\"javascript:window.opener.reloadCalPopup('"+sToday+"','"+dstWindowName+"');\">"+popCalToday+"</A>";
	var prevMonthAnchor = "<A HREF=\"javascript:window.opener.reloadCalPopup('"+sPrevMonth+"','"+dstWindowName+"');\">" + monthNames[nPrevMonth] + "</A>";
	var nextMonthAnchor = "<A HREF=\"javascript:window.opener.reloadCalPopup('"+sNextMonth+"','"+dstWindowName+"');\">" + monthNames[nNextMonth] + "</A>";
	var prevYear1Anchor = "<A HREF=\"javascript:window.opener.reloadCalPopup('"+sPrevYear1+"','"+dstWindowName+"');\">"+(thisYear-1)+"</A>";
	var nextYear1Anchor = "<A HREF=\"javascript:window.opener.reloadCalPopup('"+sNextYear1+"','"+dstWindowName+"');\">"+(thisYear+1)+"</A>";
		
	popCalData += (htmlHead + fnt[1]);
	popCalData += ("<DIV align=\"center\">");
	popCalData += ("<TABLE BORDER=\"0\" cellspacing=\"0\" callpadding=\"0\" width=\"250\"><TR><TD width=\"45\">&nbsp</TD>");
	popCalData += ("<TD width=\"45\" align=\"center\" " + cellAttribs2);
	popCalData += (" >");
	popCalData += (fnt[0]+prevYear1Anchor+"</FONT></TD>");
	popCalData += ("<TD width=\"70\" align=\"center\" "+cellAttribs2);
	popCalData += (" >");
	popCalData += (fnt[0]+todayAnchor+"</FONT></TD>");
	popCalData += ("<TD width=\"45\" align=\"center\" "+cellAttribs2);
	popCalData += (" >");
	popCalData += (fnt[0]+nextYear1Anchor+"</FONT></TD><TD width=\"45\">&nbsp</TD>");
	popCalData += ("</TR></TABLE>");
	popCalData += ("<TABLE BORDER=\"0\" cellspacing=\"0\" callpadding=\"0\" width=\"250\">");          
	popCalData += ("<TR><TD width=\"55\" align=\"center\" "+cellAttribs2);
	popCalData += (" >");
	popCalData += (fnt[0] + prevMonthAnchor + "</FONT></TD>");
	popCalData += ("<TD width=\"140\" align=\"center\">");
	popCalData += ("&nbsp;&nbsp;"+fnt[1]+"<FONT COLOR=\"#000000\">" + monthNames[thisMonth] + ", " + thisYear + "&nbsp;&nbsp;</FONT></TD>");
	popCalData += ("<TD width=\"55\" align=\"center\" "+cellAttribs2);
	popCalData += (" >");
	popCalData += (fnt[0]+nextMonthAnchor+"</FONT></TD></TR></TABLE><BR>");       
	popCalData += ("<TABLE BORDER=\"0\" cellspacing=\"2\" cellpadding=\"1\"  width=\"245\">" );
	popCalData += ("");
	popCalData += ("<TR>");
	
	var xday = 0;
	for (xday = 0; xday < 7; xday++)
	{
		popCalData += ("<TD width=\"35\" align=\"center\">"+fnt[1]+"<FONT COLOR=\"#000000\">"+dayNames[(xday+popCalFirstDayWeek)%7]+"</FONT></TD>");
	};
	popCalData += ("</TR>");
	
	var calDay = 0;
	var monthDate = 1;
	var weekDay = firstDay.getDay();
	do
	{
		popCalData += ("<TR>");
		for (calDay = 0; calDay < 7; calDay++ )
		{
			if(((weekDay+7-popCalFirstDayWeek)%7 != calDay) || (monthDate > lastDate))
			{
				popCalData += ("<TD width=\"35\">"+fnt[1]+"&nbsp;</FONT></TD>");
				continue;
			}
			else
			{
				anchorVal = "<A HREF=\"javascript:window.opener.calPopupSetDate(window.opener.popCalDstFld,'" + (thisMonth+1) + "/" + monthDate + "/" + thisYear + "');window.opener.closeCalPopup()\">";
				jsVal = "javascript:window.opener.calPopupSetDate(window.opener.popCalDstFld,'" + constructDate(monthDate,thisMonth+1,thisYear) + "');window.opener.closeCalPopup()";

				popCalData += ("<TD width=\"35\" "+cellAttribs+" onClick=\""+jsVal+"\">");
				
				if ((firstDay.getMonth() == dtToday.getMonth()) && (monthDate == dtToday.getDate()) && (thisYear == dtToday.getFullYear()) )
					popCalData += (anchorVal+fnt[2]+monthDate+"</A></FONT></TD>");
				else
					popCalData += (anchorVal+fnt[1]+monthDate+"</A></FONT></TD>");
				
				weekDay++;
				monthDate++;
			}
		}
		weekDay = popCalFirstDayWeek;
		popCalData += ("</TR>");
	} while( monthDate <= lastDate );
	
	popCalData += ("</TABLE></DIV><BR>");
 
	popCalData += (closeAnchor+"</FONT>"+htmlTail);
	return( popCalData );
}

function getFirstDate(theDate)
{
	var arr = theDate.split(" ");
	var len = arr.length;
	var temp;
	
	if (len == 1)
		return arr[0];
	else
	{
		for( var i = 0; i < len; i++)
		{
			temp = new Date(arr[i]);
			
			if (temp.toString() != 'NaN')
				return arr[i];
		}
	}
	
	return '';
}
 
function calPopupSetDate()
{
	var val = calPopupSetDate.arguments[0].value;
	var arr = val.split(" ");
	var len = arr.length;
	var theDate;
	var found = 0;
	var result = "";
	
	if (len == 1)
		calPopupSetDate.arguments[0].value = calPopupSetDate.arguments[1];
	else
	{
		for( var i = 0; i < len; i++)
		{
				theDate = new Date(arr[i]);
				
				if (theDate != 'NaN')
				{
					found = i;
					break;
				}
		}
		
		arr[found] = calPopupSetDate.arguments[1];
		
		for( var x = 0; x < len; x++ )
			result = result + arr[x] + " ";
			
		calPopupSetDate.arguments[0].value = result;
	}
}

// utility function
function padZero(num)
{
  return ((num <= 9) ? ("0" + num) : num);
}

// Format short date
function constructDate(d,m,y)
{
  var fmtDate = this.popCalDstFmt
  fmtDate = fmtDate.replace ('dd', padZero(d))
  fmtDate = fmtDate.replace ('d', d)
  fmtDate = fmtDate.replace ('MM', padZero(m))
  fmtDate = fmtDate.replace ('M', m)
  fmtDate = fmtDate.replace ('yyyy', y)
  fmtDate = fmtDate.replace ('yy', padZero(y%100))
  return fmtDate;
}