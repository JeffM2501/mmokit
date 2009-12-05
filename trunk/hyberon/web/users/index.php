<?php
	include_once('conf/config.php');
	include_once('common.php');
	include_once('db.php');
	
	function SiteDB()
	{
		global $CONFIG_DATABASE_SERVER;
		global $CONFIG_DATABASE_DATABSE;
		global $CONFIG_DATABASE_USER;
		global $CONFIG_DATABASE_PASS;

		$DB = new Database($CONFIG_DATABASE_SERVER,$CONFIG_DATABASE_DATABSE,$CONFIG_DATABASE_USER,$CONFIG_DATABASE_PASS);

		return $DB ;
	}
	
	function RegisterPage()
	{
		dumpHeader();
		
		echo "<div id=\"SectionHeader\">Register</div>
		<form id=\"LoginForm\" action=\"" . $_SERVER['SCRIPT_NAME'] . "\" method=\"POST\">
		<input type=\"hidden\" name=\"action\" value=\"register\">
		<p>Username <input type=\"text\" name=\"username\"></p>
		<p>Password <input type=\"password\" name=\"password\"></p>
		<p>Email <input type=\"text\" name=\"email\"></p>
		<p><input type=\"submit\" value=\"Register\"></p>
		</form>";
		dumpFooter();	
	}
	
	function RegisterError( $error )
	{
		dumpHeader();
		echo "<div id=\"SectionHeader\">Error</div>
		<div id=\"Error\">Error: $error </div>";
		dumpFooter();
	}
	
	function Register()
	{
		$db = SiteDB();
		
		$username = $db->Sanitize(GetRequest('username'));
		if (!$username)
		{
			RegisterError("Must enter username");
			return;
		}
		
		$uid = $db->GetField("username",$username, "users","ID");
		if ($uid)
		{
			RegisterError("User Exists");
			return;
		}
		
		$password = $db->Sanitize(GetRequest('password'));
		if (!$password)
		{
			RegisterError("Must enter password");
			return;
		}
		
		$passhash = md5($password);
		
		$email = $db->Sanitize(GetRequest('email'));
		if (!$email)
		{
			RegisterError("Must enter email");
			return;
		}
		
		$query = "INSERT INTO users (Username, Password, Email, Access) VALUES ('$username', '$passhash', '$email', 1)";
		if (!$db->Set($query))
		{
			RegisterError("DB fail");
			return;
		}
		
		$uid = $db->GetField("username",$username, "users","ID");
		if (!$uid)
		{
			RegisterError("DB fail");
			return;
		}

		dumpHeader();
		echo "<div id=\"SectionHeader\">Registration complete($uid)</div>
		<div id=\"Error\">Thanks for registering, have fun!</div>";
		dumpFooter();
	}

	session_start();
				
	$action = "";
	if (isset($_REQUEST['action']) && $_REQUEST['action'])
		$action = $_REQUEST['action'];
	
	if ($action == "register")
		Register();
	else
	RegisterPage();

?>