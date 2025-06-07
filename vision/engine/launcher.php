<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class LAUNCHER
{
    protected $config;
    protected $database;

    public function __construct($config, $database)
    {
        $this->config = $config;
        $this->database = $database;
    }

    private function DB_CONNECTION()
    {
        try
        {
            $launcher = $this->database['launcher'];
    
            $connection = mysqli_connect
            (
                $launcher['mysql_hostname'],
                $launcher['mysql_user'],
                $launcher['mysql_pass'],
                $launcher['mysql_database'],
                $launcher['mysql_port']
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
            $launcher = $this->database['launcher'];
    
            $connection = mysqli_connect
            (
                $launcher['mysql_hostname'],
                $launcher['mysql_user'],
                $launcher['mysql_pass'],
                $launcher['mysql_database'],
                $launcher['mysql_port']
            );
        
            if (mysqli_connect_errno()) 
            {
                if ($connection) 
                {
                    mysqli_close($connection);
                }
                return "LAUNCHER DB CONNECTION FAILED..";
            }
            else
            {
                mysqli_close($connection);
                return "LAUNCHER DB CONNECTION OK";
            }
        }
        catch (\Exception $e)
        {
            return "LAUNCHER DB ERROR: ".$e->getMessage();
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

    private function MAILER()
    {
        return new MAILER($this->config);
    }

    private function DB_QUERY(int $realmid = 0)
    {
        return new QUERIES($this->database, $realmid);
    }

    /*
    * Checks if guest has rate limiter cooldown
    *
    * @param string $ip
    * @return bool
    */
    public function Has_Rate_Limiter_Cooldown($RATE_LIMITER_REFERENCE)
    {
        $guest_ip = isset($_SERVER["HTTP_CF_CONNECTING_IP"]) ? $_SERVER["HTTP_CF_CONNECTING_IP"]: $_SERVER["REMOTE_ADDR"];
        $guest_ip = $RATE_LIMITER_REFERENCE == RATE_LIMITER_DISCORD_INFO ? "0" : $guest_ip;
        $guest_ip = $RATE_LIMITER_REFERENCE == RATE_LIMITER_DISCORD_INGAME_RPC ? "0" : $guest_ip;
        
        $max_attempts = $this->config['rate_limiter'][$RATE_LIMITER_REFERENCE]['max_attempts'];

        return $this->DB_QUERY()->Count_Rate_Limiter_Logs_For($this->DB_CONNECTION(), $this->config, $guest_ip, $RATE_LIMITER_REFERENCE) >= $max_attempts;
    }

    /*
    * Adds attempts to rate limiter
    *
    * @param string $ip
    *
    */
    public function Update_Rate_Limiter_Attempts($RATE_LIMITER_REFERENCE)
    {
        $guest_ip = isset($_SERVER["HTTP_CF_CONNECTING_IP"]) ? $_SERVER["HTTP_CF_CONNECTING_IP"]: $_SERVER["REMOTE_ADDR"];
        $guest_ip = $RATE_LIMITER_REFERENCE == RATE_LIMITER_DISCORD_INFO ? "0" : $guest_ip;
        $guest_ip = $RATE_LIMITER_REFERENCE == RATE_LIMITER_DISCORD_INGAME_RPC ? "0" : $guest_ip;

        $this->DB_QUERY()->Insert_Rate_Limiter_For($this->DB_CONNECTION(), $guest_ip, $RATE_LIMITER_REFERENCE);
    }

    /*
    * Returns rate limiter cooldown in seconds
    *
    * @param string $ip
    * @return int
    */
    public function Get_Rate_Limiter_Cooldown($RATE_LIMITER_REFERENCE)
    {
        $guest_ip = isset($_SERVER["HTTP_CF_CONNECTING_IP"]) ? $_SERVER["HTTP_CF_CONNECTING_IP"]: $_SERVER["REMOTE_ADDR"];
        $guest_ip = $RATE_LIMITER_REFERENCE == RATE_LIMITER_DISCORD_INFO ? "0" : $guest_ip;
        $guest_ip = $RATE_LIMITER_REFERENCE == RATE_LIMITER_DISCORD_INGAME_RPC ? "0" : $guest_ip;

        return $this->DB_QUERY()->Get_Rate_Limiter_Cooldown_Left_For($this->DB_CONNECTION(), $this->config, $guest_ip, $RATE_LIMITER_REFERENCE);
    }

    /*
    * Adds soap command to logs
    *
    * @param string $username
    * @param int $realmId
    * @param string $command
    *
    */
    public function Add_Soap_Command_Log(string $username, int $realmId, string $command)
    {
        $this->DB_QUERY()->Insert_Soap_Command_Log($this->DB_CONNECTION(), $username, $realmId, $command);
    }

    /*
    * Returns user avatar url
    *
    * @param int $accountID
    * @return string
    */
    public function Get_User_Avatar_Url($accountID)
    {
        return $this->DB_QUERY()->Get_User_Avatar_Url_Using_ID($this->DB_CONNECTION(), $accountID);
    }

    /*
    * Returns true if user avatar url was updated
    *
    * @param int $accountID
    * @param string $url
    * @return bool
    */
    public function Update_User_Avatar_Url($accountID, $url)
    {
        return $this->DB_QUERY()->Update_User_Avatar_Url_Using_ID($this->DB_CONNECTION(), $accountID, $url);
    }

    /*
    * Returns user public nickname
    *
    * @param int $accountID
    * @return string
    */
    public function Get_User_Public_Nickname_Only_By_ID($accountID)
    {
        return $this->DB_QUERY()->Get_User_Public_Nickname_Using_ID($this->DB_CONNECTION(), $accountID);
    }
    /*
    * Returns articles as json
    *
    * @return json
    */
    public function Get_Slider_Images()
    {
        return json_encode($this->DB_QUERY()->Get_Launcher_Slider_Image_Urls($this->DB_CONNECTION()), JSON_PRETTY_PRINT);
    }

    /*
    * Returns events as json
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Launcher_Events_List($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Authorized' => false,
            'Message' => Lang_Error["not_authorized"]
        ];

        if ($this->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->Validate_Login_Token($MD5token, $MD5Username))
        {
            return json_encode($this->DB_QUERY()->Get_Launcher_Events($this->DB_CONNECTION()), JSON_PRETTY_PRINT);
        }
        else
        {
            $this->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns faq as json
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Launcher_Faq_List($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Authorized' => false,
            'Message' => Lang_Error["not_authorized"]
        ];

        if ($this->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->Validate_Login_Token($MD5token, $MD5Username))
        {
            return json_encode($this->DB_QUERY()->Get_Launcher_Faq($this->DB_CONNECTION()), JSON_PRETTY_PRINT);
        }
        else
        {
            $this->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns teleport list as json
    *
    * @return json
    */
    public function Get_Launcher_Teleport_List()
    {
        return json_encode($this->DB_QUERY()->Get_Launcher_Teleport_List($this->DB_CONNECTION()), JSON_PRETTY_PRINT);
    }

    /*
    * Returns teleport coordinates as array
    *
    * @param int $id
    * @return array
    */
    public function Get_Launcher_Teleport_Coordinates($id)
    {
        return $this->DB_QUERY()->Get_Launcher_Teleport_Coordinates_Using_ID($this->DB_CONNECTION(), $id);
    }

    /*
    * Returns teleport costs as array
    *
    * @param int $id
    * @return array
    */
    public function Get_Launcher_Teleport_Costs($id)
    {
        return $this->DB_QUERY()->Get_Launcher_Teleport_Costs_Using_ID($this->DB_CONNECTION(), $id);
    }

    /*
    * Returns teleport destination allowed as bolean
    *
    * @param int $teleport_id
    * @param bool $isAlliance
    * @return bool
    */
    public function Is_Teleport_Destination_Faction_Allowed(int $teleport_id, bool $isAlliance) : bool
    {
        return $this->DB_QUERY()->Teleport_Destination_Is_Faction_Allowed($this->DB_CONNECTION(), $teleport_id, $isAlliance);
    }

    /*
    * Checks if login token is valid
    *
    * @param int $accountID
    * @return bool
    */
    public function Validate_Login_Token($MD5token, $MD5Username)
    {
        return (bool)$this->DB_QUERY()->Get_Username_Using_Auth_Token_and_Username($this->DB_CONNECTION(), $MD5token, $MD5Username);
    }

    /*
    * Checks if password recovery code is valid
    *
    * @param string $code
    * @param string $username
    * @return bool
    */
    private function Validate_Password_Recovery_Code($code, $username)
    {
        $guest_ip = isset($_SERVER["HTTP_CF_CONNECTING_IP"])? $_SERVER["HTTP_CF_CONNECTING_IP"]: $_SERVER["REMOTE_ADDR"];
        $recovery_code = $this->DB_QUERY()->Get_Password_Recovery_Code_Using_Username_IP_Address($this->DB_CONNECTION(), $username, $guest_ip);
    
        return strlen($recovery_code) > 0 && $recovery_code == $code;
    }

    /*
    * Returns account's rasar key
    *
    * @param int $account_id
    * @return string
    */
    public function Get_Rasar_Key($account_id)
    {
        return $this->DB_QUERY()->Get_Rasar_Key_From_Account_Id($this->DB_CONNECTION(), $account_id);
    }

    /*
    * Returns account's rasar iv
    *
    * @param int $account_id
    * @return string
    */
    public function Get_Rasar_IV($account_id)
    {
        return $this->DB_QUERY()->Get_Rasar_IV_From_Account_Id($this->DB_CONNECTION(), $account_id);
    }

    /*
    * Updates account's rasar key
    *
    * @param string $key
    * @param int $account_id
    * @return bool
    */
    public function Update_Rasar_Key($key, $account_id)
    {
        return $this->DB_QUERY()->Update_Rasar_Key_For_Account_Id($this->DB_CONNECTION(), $key, $account_id);
    }  

    /*
    * Updates account's rasar iv
    *
    * @param string $iv
    * @param int $account_id
    * @return bool
    */
    public function Update_Rasar_IV($key, $account_id)
    {
        return $this->DB_QUERY()->Update_Rasar_IV_For_Account_Id($this->DB_CONNECTION(), $key, $account_id);
    }  

    /*
    * Returns json response for token challenge authentication
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Token_Challenge($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Authorized'    => false,
            'Message'       => Lang_Error["not_authorized"],
            'AccountInfo'   => null,
        ];

        if ($this->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->Validate_Login_Token($MD5token, $MD5Username))
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            $json->Authorized = true;
            $json->Message = Lang_Success["authorized"];
            $json->AccountInfo  = $this->AUTH()->Get_Account_Info($username);
        }
        else
        {
            $this->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns access token
    *
    * @param int $accountID
    * @param string $username
    * @param string $email
    * @return string
    */
    public function Update_AccountData($accountID, $username, $email)
    {
        $guest_ip = isset($_SERVER["HTTP_CF_CONNECTING_IP"])? $_SERVER["HTTP_CF_CONNECTING_IP"]: $_SERVER["REMOTE_ADDR"];
        $access_token = substr(md5(openssl_random_pseudo_bytes(20)),-32);
        $token_time = time() + $this->config['access_token']['valid_time'];

        if ($this->DB_QUERY()->Account_Data_Exists_By_ID($this->DB_CONNECTION(), $accountID))
        {
            $this->DB_QUERY()->Update_Account_Data_Access_Token($this->DB_CONNECTION(), $accountID, $username, $guest_ip, $access_token, $token_time);
        }
        else
        {
            $this->DB_QUERY()->Insert_New_Account_Data($this->DB_CONNECTION(), $accountID, $username, $email, $guest_ip, $access_token, $token_time);
        }

        return md5($access_token);
    }

    /*
    * Returns json response for update avatar url
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @param string $image_url
    * @return json
    */
    public function Update_AvatarUrl($MD5token, $MD5Username, $image_url)
    {
        $json = (object)
        [
            'Updated' => false,
            'Message' => Lang_Error["not_authorized"]
        ];

        if ($this->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->Validate_Login_Token($MD5token, $MD5Username))
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            if ($this->Update_User_Avatar_Url($this->AUTH()->Get_Account_ID_By_Username($username), $image_url))
            {
                $json->Updated = true;
                $json->Message = Lang_Success["avatar_updated"];
            }
        }
        else
        {
            $this->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns json response for update public nickname
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @param string $nickname
    * @return json
    */
    public function Update_Public_Profile_Nickname($MD5token, $MD5Username, $nickname)
    {
        $json = (object)
        [
            'Updated' => false,
            'Message' => Lang_Error["not_authorized"]
        ];

        if ($this->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->Validate_Login_Token($MD5token, $MD5Username))
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            $accountID = $this->AUTH()->Get_Account_ID_By_Username($username);

            if (!$this->DB_QUERY()->Nickname_exists($this->DB_CONNECTION(), $accountID, $nickname))
            {
                $connection = $this->DB_CONNECTION();

                if ($stmt = $connection->prepare('UPDATE account_data SET public_nickname = ? WHERE id = ?'))
                {
                    $stmt->bind_param('si', $nickname, $accountID);
                    $stmt->execute();
                    $stmt->close();

                    $json->Updated = true;
                    $json->Message = Lang_Success["profile_updated"];
                }

                $connection->close();
            }
            else
            {
                $json->Message = Lang_Error["profile_id_taken"];
            }
        }
        else
        {
            $this->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns json response for get public nickname
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_User_Public_Nickname_By_ID($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Authorized' => false,
            'Nickname'   => null
        ];

        if ($this->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->Validate_Login_Token($MD5token, $MD5Username))
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            $accountID = $this->AUTH()->Get_Account_ID_By_Username($username);
            $json->Authorized = true;
            $json->Nickname = $this->DB_QUERY()->Get_User_Public_Nickname_Using_ID($this->DB_CONNECTION(), $accountID);
        }
        else
        {
            $this->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Sends password recovery code request
    *
    * @param string $email
    *
    */
    public function Send_Password_Recovery_Code($email)
    {
        if (filter_var($email, FILTER_VALIDATE_EMAIL) && !$this->Has_Rate_Limiter_Cooldown(RATE_LIMITER_PASSWORD_RECOVERY)) 
        {
            if ($username = $this->AUTH()->Get_Username_By_Email($email)) 
            {
                $recovery_code = md5(mt_rand());

                $valid_until = time() + $this->config['recovery_code']['valid_time'];

                if ($this->DB_QUERY()->Generate_Password_Recovery_Code($this->DB_CONNECTION(), $username, $recovery_code, $valid_until)) 
                {
                    $this->MAILER()->SendPasswordResetCode($email, $username, $recovery_code);
                }
            }
            else
            {
                sleep(2); // don't give a clue if success or not
            }

            $this->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
        }

        ob_clean();
    }

    /*
    * Returns json response for change account password using reset code
    *
    * @param string $username
    * @param string $recovery_code
    * @param string $password1
    * @param string $password2
    * @return json
    */
    public function Change_Account_Password_Using_Reset_Code($username, $recovery_code, $password1, $password2)
    {
        $json = (object)
        [
            'Changed' => false,
            'Message' => Lang_Error["password_not_changed"],
        ];

        if ($this->Has_Rate_Limiter_Cooldown(RATE_LIMITER_PASSWORD_RESET))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        $username       = hex2bin($username);
        $recovery_code  = hex2bin($recovery_code);
        $password1      = hex2bin($password1);
        $password2      = hex2bin($password2);

        if ($this->Validate_Password_Recovery_Code($recovery_code, $username))
        {
            $cp_obj = json_decode($this->AUTH()->Change_Password($username, $password1, $password2));
            $json->Changed = $cp_obj->Changed;
            $json->Message = $cp_obj->Message;

            if ($json->Changed)
            {
                $this->DB_QUERY()->Delete_Password_Recovery_Code($this->DB_CONNECTION(), $username);
            }
        }
        else
        {
            $this->Update_Rate_Limiter_Attempts(RATE_LIMITER_PASSWORD_RESET);
            $json->Message = Lang_Error["invalid_reset_code"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns realm status as json result
    *
    * @param int $realmid
    * @return json
    */
    public function GetRealmStatus($realmid)
    {
        error_reporting(0);

        $realm      = array();
        $RealmName  = $this->database['realms'][$realmid]['realm_name'];

        if ($fp = fsockopen($this->database['realms'][$realmid]['realmlist'], $this->database['realms'][$realmid]['realm_port'], $errno, $errstr, 3))
        {
            $realm[] = array
            (
                'RealmId'       => $realmid,
                'RealmName'     => $RealmName,
                'OnlineStatus'  => true,
            );
            fclose($fp);
        }
        else
        {
            $realm[] = array
            (
                'RealmId'       => $realmid,
                'RealmName'     => $RealmName,
                'OnlineStatus'  => false,
            );
        }

        return json_encode($realm, JSON_PRETTY_PRINT);
    }
}