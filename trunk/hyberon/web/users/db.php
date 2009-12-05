<?php  
	include_once("conf/config.php");
	
	class Database
	{
		var $server;
		var $database;
		
		function Database ( $srv, $db, $user, $pass )
		{
			$this->server = mysql_pconnect($srv,$user,$pass);
			if (!$this->server)
				return FALSE;

			$this->database = $db;
			$result = mysql_select_db($db);
		}
		
		function SetCurrent()
		{
			mysql_select_db($this->database,$this->server);
		}
		
		function Error ( $query )
		{
			$this->SetCurrent();
			echo "SQL ERROR: " . mysql_error() . "</br>";
			echo "SQL ERROR Query: " . $query . "</br>";
		}
		
		function Get ( $query )
		{
			$this->SetCurrent();
			$result = mysql_query($query);
			if (!$result && $result != 0 && mysql_num_rows($result) > 0)
				$this->Error($query);
				
			return $result;
		}
		
		function Set ( $query )
		{
			$this->SetCurrent();
			$result = mysql_query($query);
			if (!$result)
			{
				$this->Error($query);
				return FALSE;
			}
				
			return TRUE;
		}
		
		function QueryResult ( $result, $field )
		{
			if (!$result)
				return FALSE;
			
			$this->SetCurrent();
			
			$list = array(); 
			$count = mysql_num_rows($result);
			for ($i = 0; $i < $count; $i += 1)
			{
				$row = mysql_fetch_array($result);
				$list[] = $row[$field];
			}
			
			return $list;
		}
		
		function QueryResults ( $result )
		{
			if (!$result)
				return FALSE;
			
			$list = array(); 
			$count = mysql_num_rows($result);
			for ($i = 0; $i < $count; $i += 1)
			{
				$row = mysql_fetch_array($result);

				$thisRow = array();
				foreach ( $row as $key => $val)
				{
					if (!is_numeric($key))
						$thisRow[] = $this->Unsanitize($val);
				}
				
				$list[] = $thisRow;
			}
			
			return $list;
		}
		
		function ResultCount ( $results )
		{
			return mysql_num_rows($results);
		}
		
		function GetResult ( $results )
		{
			return mysql_fetch_array($results);
		}
		
		function GetField( $keyName, $key, $table, $field )
		{
			$query = "SELECT " . $field . " FROM ". $table ." WHERE " . $keyName . "='" .$key . "'";
			$results = $this->QueryResult($this->Get($query),$field );
			if (!$results)
				return FALSE;
							
			return $this->Unsanitize($results[0]);
		}
		
		function GetSortString ( $sort, $dec )
		{
			if ($sort)
			{
				if ($dec)
					return " ORDER BY ". $sort ." DESC";
				else
					return " ORDER BY ". $sort ." ASC";	
			}
			
			return "";
		}
		
		function GetFields( $keyName, $key, $table, $fields, $sort, $dec )
		{
			$query = "SELECT ";

			for ($i = 0; $i < count($fields); $i += 1)
			{
				$query = $query . $fields[$i];
				if ($i < count($fields)-1)
					$query = $query . ", ";
			}
			$query = $query .  " FROM ". $table ." WHERE " . $keyName . "='" .$key . "'" . $this->GetSortString($sort,$dec);
			
			$results = $this->QueryResults($this->Get($query));
			if (!$results)
				return FALSE;
				
			return $results;
		}
		
		function SetField ( $keyName, $key, $table, $field, $value )
		{
			$query = "UPDATE " . $table ." SET " . $field . "='" .$value."' WHERE " . $keyName ."='" .$key. "'";
			return $this->Set($query); 
		}
		
		function Sanitize ( $value )
		{
			$this->SetCurrent();
			return mysql_real_escape_string(addslashes($value));	
		}
		
		function Unsanitize ( $value )
		{
			$this->SetCurrent();
			return stripslashes($value);	
		}
	}
	
	
	
?>