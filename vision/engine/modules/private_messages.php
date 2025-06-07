<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class PRIVATE_MESSAGES
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

    private function LAUNCHER()
    { 
        return new LAUNCHER($this->config, $this->database);
    }

    private function AUTH()
    { 
        return new AUTH($this->config, $this->database);
    }

    private function DB_QUERY(int $realmid = 0)
    {
        return new QUERIES($this->database, $realmid);
    }

    /*
    * Returns private messages as json
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Launcher_Private_Messages($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Message' => Lang_Error["not_authorized"],
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
            return json_encode($this->DB_QUERY()->Get_Launcher_Private_Messages_Using_AccountID($this->DB_CONNECTION(), $accountID), JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns private message thread as json
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @param int $message_id
    * @return json
    */
    public function Get_Launcher_Private_Message_Thread($MD5token, $MD5Username, $message_id)
    {
        $json = (object)
        [
            'Message' => Lang_Error["not_authorized"],
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username))
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            $account_id = $this->AUTH()->Get_Account_ID_By_Username($username);

            if ($this->DB_QUERY()->Is_In_Message_Party($this->DB_CONNECTION(), $message_id, $account_id))
            {
                return json_encode($this->DB_QUERY()->Get_Launcher_Private_Message_Thread($this->DB_CONNECTION(), $message_id), JSON_PRETTY_PRINT);
            }
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Inserts new private message
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @param string $receiver_nickname
    * @param string $title
    * @param string $message
    * @return json
    */
    public function Send_New_Launcher_Private_Message($MD5token, $MD5Username, $receiver_nickname, $title, $message)
    {
        $json = (object)
        [
            'Sent'    => false,
            'Message' => Lang_Error["not_authorized"],
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

            if (strlen($message) >= 10 && strlen($message) <= 2500 && strlen($title) >= 3 && strlen($title) <= 50)
            {
                if ($this->DB_QUERY()->Message_Receiver_Exists($this->DB_CONNECTION(), $receiver_nickname))
                {
                    if ($this->DB_QUERY()->Insert_New_Private_Message($this->DB_CONNECTION(), $accountID, $receiver_nickname, $title, $message))
                    {
                        $json->Sent = true;
                        $json->Message = Lang_Success["message_sent"];
                    }
                }
                else
                {
                    $json->Message = Lang_Error["invalid_receiver"];
                }
            }
            else
            {
                $json->Message = Lang_Error["message_title_criteria"];
            }
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Inserts new private message reply
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @param int $message_id
    * @param string $message
    * @return json
    */
    public function Send_New_Launcher_Private_Message_Reply($MD5token, $MD5Username, $message_id, $message)
    {
        $json = (object)
        [
            'Sent'    => false,
            'Message' => Lang_Error["not_authorized"],
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username))
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            $account_id = $this->AUTH()->Get_Account_ID_By_Username($username);

            if (strlen($message) >= 10 && strlen($message) <= 2500)
            {
                if ($this->DB_QUERY()->Is_In_Message_Party($this->DB_CONNECTION(), $message_id, $account_id))
                {
                    if ($this->DB_QUERY()->Insert_New_Private_Message_Reply($this->DB_CONNECTION(), $message_id, $account_id, $message))
                    {
                        $json->Sent = true;
                        $json->Message = Lang_Success["reply_sent"];
                    }
                }
            }
            else
            {
                $json->Message = Lang_Error["message_text_criteria"];
            }
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Deleted private message for account id
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @param int $message_id
    * @return json
    */
    public function Delete_Launcher_Private_Message_For($MD5token, $MD5Username, $message_id)
    {
        $json = (object) 
        [
            'Deleted' => false,
            'Message' => Lang_Error["not_authorized"],
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username))
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            $account_id = $this->AUTH()->Get_Account_ID_By_Username($username);

            if ($this->DB_QUERY()->Is_In_Message_Party($this->DB_CONNECTION(), $message_id, $account_id))
            {
                if ($this->DB_QUERY()->Delete_Private_Message_Using_Account_ID($this->DB_CONNECTION(), $message_id, $account_id))
                {
                    $json->Deleted = true;
                    $json->Message = Lang_Success["message_deleted"];
                }
            }
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }
}