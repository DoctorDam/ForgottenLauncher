<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class LOGIN_REWARDS
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
    * Checks if reward id exists
    *
    * @param int $reward_id
    * @return bool
    */
    public function Reward_Exists($reward_id)
    {
        return $this->DB_QUERY()->Reward_Exists_Using_ID($this->DB_CONNECTION(), $reward_id);
    }

    /*
    * Returns month's login rewards list as json
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Login_Rewards_List($MD5token, $MD5Username)
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

            return json_encode($this->DB_QUERY()->Get_Launcher_Login_Rewards($this->DB_CONNECTION(), $accountID), JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns json result for claim login reward
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @param int $month
    * @param int $day
    * @return json
    */
    public function Claim_Login_Reward($MD5token, $MD5Username, $month, $day)
    {
        $json = (object)
        [
            'Claimed' => false,
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

            // check if user did not already claimed this login reward
            if (!(bool)$this->DB_QUERY()->Claimed_Login_Reward($this->DB_CONNECTION(), $accountID, $month, $day))
            {
                // hack-check: if user is not trying to claim other day
                if (date('n') == $month && date('j') == $day)
                {
                    // check if login reward exists
                    if ($reward_id = $this->DB_QUERY()->Reward_ID_Exists_By_Month_Day($this->DB_CONNECTION(), $month, $day))
                    {
                        // add to login claimed rewards table
                        $this->DB_QUERY()->Insert_Login_Claimed_Rewards_Log($this->DB_CONNECTION(), $accountID, $month, $day);
                        $this->DB_QUERY()->Insert_Claimed_Reward_To_Account_Inventory($this->DB_CONNECTION(), $accountID, $reward_id);

                        $json->Claimed = true;
                        $json->Message = Lang_Success["reward_claimed"];
                    }
                }
            }
            else
            {
                $json->Message = Lang_Error["already_claimed_reward"]; 
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