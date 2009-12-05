<?php  
	include_once("conf/config.php");
	
	function GetRequest( $item )
	{
		$return = "";
		if (isset($_REQUEST[$item]) && $_REQUEST[$item])
			$return = $_REQUEST[$item];
	
		return $return;
	}
	
	function dumpMeta()
	{
		echo
		"<meta http-equiv=\"Content-Typ\e\" content=\"text/html; charset=ISO-8859-1\">
		<meta name=\"robots\" content=\"index, follow\">
		<link href=\"site.css\" rel=\"stylesheet\" type=\"text/css\">";
	}
	
	function dumpHeader( $redirect = '', $wait = 0 )
	{	
		echo 
		"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">
		<html xmlns=\"http://www.w3.org/1999/xhtml\">
		<head>";
		
		if ($redirect)
		{
			echo "<meta HTTP-EQUIV=\"REFRESH\" content=\"" . $wait . "; url=" . $redirect . "\">";
		}
		echo "<title>Registration</title>";
		dumpMeta();
		
		echo 
			"</head>
			<body>
			<div id=\"PageFrame\">
			<!-- begin header -->
			<div id=\"Header\">
			Game User Registration(DEV)<img id=\"HeaderImage\" src=\"images/report_logo.png\">
			</div>
			<!-- end header -->
			";	
	}
	
	function dumpNavItem ( $name, $target, $selected )
	{
		if ($selected)
			$class = "NavBarItemSel";
		else
			$class = "NavbarItem";
			
		if ($target)
			echo "<a class=\"" . $class . "\" href=\"" . $target . "\">";
		else
			echo "<span class=\"" . $class . "\">";
			
		echo $name;
		if ($target)
			echo "</a>";
		else
			echo "</span>";
	}
	
	function dumpNavBar ( $item, $extra = "" )
	{
		echo "<div id=\"Navbar\">";
		
		dumpNavItem("Home","index.php",$item == 0);
	
		if ($extra)
			dumpNavItem($extra,"",TRUE);
			
		echo "</div>";
	}
	
	function dumpFooter()
	{
		echo"
		<!-- begin footer -->
		<div id=\"footer\">
		<div id=\"disclaimer\">Development</div>
		</div>
		<!-- end footer -->
		</div> <!-- end PageFrame -->
		</body>
		</html>";
	}
	
	function GetOSPathName ( $path )
	{
		return settingValue("root_path") . $path;
	}
?>