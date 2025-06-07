<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

require_once('./vendor/autoload.php');

class CHAR
{
    protected $config;
    protected $database;
    protected $realmid;

    public function __construct($config, $database, $realmid)
    {
        $this->config = $config;
        $this->database = $database;
        $this->realmid = $realmid;
    }

    private function DB_CONNECTION()
    {
        try
        {
            $char = $this->database['realms'][$this->realmid];
    
            $connection = mysqli_connect(
                $char['mysql_hostname'],
                $char['mysql_user'],
                $char['mysql_pass'],
                $char['mysql_database'],
                $char['mysql_port']
            );
    
            mysqli_set_charset($connection, "utf8");
    
            return $connection;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError($e->getMessage());
        }

        return null;
    }

    public function TEST_DB_CONNECTION()
    {
        try
        {
            $char = $this->database['realms'][$this->realmid];
    
            $connection = mysqli_connect(
                $char['mysql_hostname'],
                $char['mysql_user'],
                $char['mysql_pass'],
                $char['mysql_database'],
                $char['mysql_port']
            );
        
            if (mysqli_connect_errno()) 
            {
                if ($connection) 
                {
                    mysqli_close($connection);
                }
                return "REALM ID ".$this->realmid." CHAR DB CONNECTION FAILED..";
            }
            else
            {
                mysqli_close($connection);
                return "REALM ID ".$this->realmid." CHAR DB CONNECTION OK";
            }
        }
        catch (\Exception $e)
        {
            return "REALM ID ".$this->realmid." CHAR DB ERROR: ".$e->getMessage();
        }
    }

    public function RUN_DB_QUERY($query)
    {
        $db_connection = $this->DB_CONNECTION();

        try
        {
            $stmt = $db_connection->prepare($query);
            $exec = $stmt->execute();
            $stmt->close();
            $db_connection->close();

            return $exec;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    private function AUTH()
    { 
        return new AUTH($this->config, $this->database);
    }

    private function LAUNCHER()
    {
        return new LAUNCHER($this->config, $this->database);
    }

    private function DB_QUERY(int $realmid = 0)
    {
        return new QUERIES($this->database, $realmid);
    }

    /*
    * Returns true or false if account id is character owner
    *
    * @param int $accountID
    * @param int $guid
    * @return bool
    */
    public function Is_Character_Owner($accountID, $guid)
    {
        return $this->DB_QUERY($this->realmid)->Owns_Character_Guid($this->DB_CONNECTION(), $accountID, $guid);
    }

    /*
    * Returns true or false if character is banned
    *
    * @param int $guid
    * @return bool
    */
    public function Is_Character_Banned($guid)
    {
        return $this->DB_QUERY($this->realmid)->Character_Guid_Is_Banned($this->DB_CONNECTION(), $guid);
    }

    /*
    * Returns true or false if character is online
    *
    * @param int $guid
    * @return bool
    */
    public function Is_Character_Online($guid)
    {
        return $this->DB_QUERY($this->realmid)->Is_Character_Guid_Online($this->DB_CONNECTION(), $guid);
    }

    /*
    * Returns true or false if character account id changed
    *
    * @param int $guid
    * @param int $newAccountId
    * @return bool
    */
    public function Change_Account_ID($guid, $newAccountId)
    {
        return $this->DB_QUERY($this->realmid)->Character_Update_Account_ID($this->DB_CONNECTION(), $guid, $newAccountId);
    }

    /*
    * Returns character name
    *
    * @param int $guid
    * @return string
    */
    public function Get_Character_Name($guid)
    {
        return $this->DB_QUERY($this->realmid)->Get_Character_Name_By_Guid($this->DB_CONNECTION(), $guid);
    }

    /*
    * Returns character guid
    *
    * @param String $name
    * @return int
    */
    public function Get_Character_Guid($name)
    {
        return $this->DB_QUERY($this->realmid)->Get_Character_Guid_By_Name($this->DB_CONNECTION(), $name);
    }

    /*
    * Returns character info array
    *
    * @param int $guid
    * @return array
    */
    public function Get_Character_Info($guid)
    {
        return $this->DB_QUERY($this->realmid)->Get_Character_Info_By_Guid($this->DB_CONNECTION(), $guid);
    }

    /*
    * Returns online characters array
    *
    * @return array
    */
    public function Get_Online_Characters()
    {
        return $this->DB_QUERY($this->realmid)->Get_Realm_Online_Players($this->DB_CONNECTION(), $this->config, 
            $this->realmid, $this->database['realms'][$this->realmid]['realm_name']);
    }

    /*
    * Returns character professions array
    *
    * @param int $guid
    * @return array
    */
    public function Get_Character_Professions($guid)
    {
        return $this->DB_QUERY($this->realmid)->Get_Character_Professions_By_Guid($this->DB_CONNECTION(), $guid);
    }

    /*
    * Returns account's character list as json
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Account_Characters_List($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Message' => Lang_Error["not_authorized"]
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username)) 
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            $accountID = $this->AUTH()->Get_Account_ID_By_Username($username);

            return json_encode($this->DB_QUERY($this->realmid)->Get_Characters_List_Using_Account_Id($this->DB_CONNECTION(), $accountID), JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns account's characters tickets list as json
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Account_Characters_Tickets_List($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Message' => Lang_Error["not_authorized"]
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username)) 
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            $accountID = $this->AUTH()->Get_Account_ID_By_Username($username);

            return json_encode($this->DB_QUERY($this->realmid)->Get_Characters_Tickets_List_Using_Account_ID($this->DB_CONNECTION(), 
                $accountID, $this->realmid, $this->database['realms'][$this->realmid]['realm_name']), JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }
    }

    /*
    * Returns ladderboard as json
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Ladderboard($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Message' => Lang_Error["not_authorized"]
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username)) 
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);

            return json_encode($this->DB_QUERY($this->realmid)->Get_Realm_Ladderboard($this->DB_CONNECTION(), 
            $this->realmid, $this->database['realms'][$this->realmid]['realm_name']), JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }
    }

    /*
    * Returns online players count
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return int
    */
    public function Get_Online_Players_Count($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Message' => Lang_Error["not_authorized"]
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username)) 
        {
            return json_encode($this->DB_QUERY($this->realmid)->Get_Realm_Online_Players_Count($this->DB_CONNECTION()), JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }
    }

    /*
    * Returns online players as json
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Online_Players_List($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Message' => Lang_Error["not_authorized"]
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username)) 
        {
            return json_encode($this->Get_Online_Characters(), JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }
    }
}
