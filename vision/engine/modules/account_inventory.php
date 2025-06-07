<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class ACCOUNT_INVENTORY
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

    private function CHAR(int $realm_id)
    {
        return new CHAR($this->config, $this->database, $realm_id);
    }

    private function CMS()
    {
        return new CMS($this->config, $this->database);
    }

    private function LOGIN_REWARDS()
    {
        return new LOGIN_REWARDS($this->config, $this->database);
    }

    private function SOAP_MASTER()
    {
        return new SOAP_MASTER($this->config, $this->database);
    }

    private function DB_QUERY(int $realmid = 0)
    {
        return new QUERIES($this->database, $realmid);
    }

    /*
    * Returns true or false if owns that reward in inventory
    *
    * @param int $account_id
    * @param int $reward_id
    * @return bool
    */
    private function Owns_Inventory_Reward($account_id, $reward_id)
    {
        return $this->DB_QUERY()->Owns_Inventory_Reward($this->DB_CONNECTION(), $account_id, $reward_id);
    }

    /*
    * Returns reward data as array
    *
    * @param int $reward_id
    * @return array
    */
    private function Get_Reward_Data($reward_id)
    {
        return $this->DB_QUERY()->Get_Reward_Data_Using_ID($this->DB_CONNECTION(), $reward_id);
    }

    /*
    * Deletes reward from account id
    *
    * @param int $account_id
    * @param int $reward_id
    * @return void
    */
    public function Delete_Reward_From_Account($account_id, $reward_id)
    {
        $this->DB_QUERY()->Delete_Used_Inventory_Item_From_Account_Id($this->DB_CONNECTION(), $account_id, $reward_id);
    }

    /*
    * Returns month's login rewards list as json
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Account_Inventory_List($MD5token, $MD5Username)
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

            return json_encode($this->DB_QUERY()->Get_Launcher_Account_Inventory($this->DB_CONNECTION(), $accountID), JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns item inventory use result as json
    *
    * @return json
    */
    public function Use_Inventory_Item($MD5token, $MD5Username, $guid, $reward_id)
    {
        $json = (object)
        [
            'Used'    => false,
            'Message' => Lang_Error["not_authorized"]
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if (!$this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username)) 
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
        $user_id = $this->AUTH()->Get_Account_ID_By_Username($username);

        if (!$this->Owns_Inventory_Reward($user_id, $reward_id))
        {
            $json->Message = Lang_Error["reward_id_not_found"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($guid != 0)
        {
            if (!$this->CHAR($this->realmid)->Is_Character_Owner($user_id, $guid))
            {
                $json->Message = Lang_Error["not_character_owner"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }
        }

        if (!$this->LOGIN_REWARDS()->Reward_Exists($reward_id))
        {
            $json->Message = Lang_Error["reward_id_not_found"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        $reward_data = $this->Get_Reward_Data($reward_id);

        $soap_command       = $reward_data['soap_command'];
        $auth_db_query      = $reward_data['auth_db_query'];
        $char_db_query      = $reward_data['char_db_query'];
        $web_db_query       = $reward_data['web_db_query'];
        $vision_db_query    = $reward_data['vision_db_query'];

        $rewards_types = 0;

        if ($soap_command != null && !empty($soap_command))
        {
            $rewards_types++;

            $soap_command = str_replace("{USERNAME}", $username, $soap_command);
            $soap_command = str_replace("{USER_ID}", $user_id, $soap_command);

            if ($guid != 0)
            {
                $character_name = $this->CHAR($this->realmid)->Get_Character_Name($guid);

                $soap_command = str_replace("{PLAYER_NAME}", $character_name, $soap_command);
                $soap_command = str_replace("{PLAYER_GUID}", $guid, $soap_command);
            }

            $result = json_decode($this->SOAP_MASTER()->Send_Command($soap_command, $this->realmid, $username));

            if (!$result->Success)
            {
                $json->Message = $result->Message;
                return json_encode($json, JSON_PRETTY_PRINT);
            }
        }

        if ($auth_db_query != null && !empty($auth_db_query))
        {
            $rewards_types++;

            $auth_db_query = str_replace("{USERNAME}", $username, $auth_db_query);
            $auth_db_query = str_replace("{USER_ID}", $user_id, $auth_db_query);

            if ($guid != 0)
            {
                $character_name = $this->CHAR($this->realmid)->Get_Character_Name($guid);
                
                $auth_db_query = str_replace("{PLAYER_NAME}", $character_name, $auth_db_query);
                $auth_db_query = str_replace("{PLAYER_GUID}", $guid, $auth_db_query);
            }

            if (!$this->AUTH()->RUN_DB_QUERY($auth_db_query))
            {
                $json->Message = "No auth table rows were affected..";
                return json_encode($json, JSON_PRETTY_PRINT);
            }
        }

        if ($char_db_query != null && !empty($char_db_query))
        {
            $rewards_types++;

            $char_db_query = str_replace("{USERNAME}", $username, $char_db_query);
            $char_db_query = str_replace("{USER_ID}", $user_id, $char_db_query);

            if ($guid != 0)
            {
                $character_name = $this->CHAR($this->realmid)->Get_Character_Name($guid);
                
                $char_db_query = str_replace("{PLAYER_NAME}", $character_name, $char_db_query);
                $char_db_query = str_replace("{PLAYER_GUID}", $guid, $char_db_query);
            }

            if (!$this->CHAR($this->realmid)->RUN_DB_QUERY($char_db_query))
            {
                $json->Message = "No char table rows were affected..";
                return json_encode($json, JSON_PRETTY_PRINT);
            }
        }

        if ($web_db_query != null && !empty($web_db_query))
        {
            $rewards_types++;

            $web_db_query = str_replace("{USERNAME}", $username, $web_db_query);
            $web_db_query = str_replace("{USER_ID}", $user_id, $web_db_query);

            if ($guid != 0)
            {
                $character_name = $this->CHAR($this->realmid)->Get_Character_Name($guid);
                
                $web_db_query = str_replace("{PLAYER_NAME}", $character_name, $web_db_query);
                $web_db_query = str_replace("{PLAYER_GUID}", $guid, $web_db_query);
            }

            if (!$this->CMS()->RUN_DB_QUERY($web_db_query))
            {
                $json->Message = "No web table rows were affected..";
                return json_encode($json, JSON_PRETTY_PRINT);
            }
        }

        if ($vision_db_query != null && !empty($vision_db_query))
        {
            $rewards_types++;

            $vision_db_query = str_replace("{USERNAME}", $username, $vision_db_query);
            $vision_db_query = str_replace("{USER_ID}", $user_id, $vision_db_query);

            if ($guid != 0)
            {
                $character_name = $this->CHAR($this->realmid)->Get_Character_Name($guid);
                
                $vision_db_query = str_replace("{PLAYER_NAME}", $character_name, $vision_db_query);
                $vision_db_query = str_replace("{PLAYER_GUID}", $guid, $vision_db_query);
            }

            if (!$this->LAUNCHER()->RUN_DB_QUERY($vision_db_query))
            {
                $json->Message = "No launcher table rows were affected..";
                return json_encode($json, JSON_PRETTY_PRINT);
            }
        }

        $json->Used = true;
        $json->Message = Lang_Success["inventory_use_ok"];
        $this->Delete_Reward_From_Account($user_id, $reward_id);

        return json_encode($json, JSON_PRETTY_PRINT);
    }
}