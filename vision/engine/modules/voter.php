<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class VOTER
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
            $cms = $this->database['cms'];
    
            $connection = mysqli_connect
            (
                $cms['mysql_hostname'],
                $cms['mysql_user'],
                $cms['mysql_pass'],
                $cms['mysql_database'],
                $cms['mysql_port']
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

    private function CMS()
    {
        return new CMS($this->config, $this->database);
    }

    private function DB_QUERY(int $realmid = 0)
    {
        return new QUERIES($this->database, $realmid);
    }

    /*
    * Returns vote points reward amount for site id
    *
    * @param int $siteID
    * @return int
    */
    public function Get_Vote_Site_Reward_Amount(int $siteID)
    {
        return $this->DB_QUERY()->Get_Vote_Site_Points($this->DB_CONNECTION(), $siteID);
    }

    /*
    * Logs vote action
    *
    * @param int $siteID
    * @param int $accountID
    *
    */
    public function Insert_Vote_Log(int $siteID, int $accountID)
    {
        return $this->DB_QUERY()->Insert_Vote_Log($this->DB_CONNECTION(), $siteID, $accountID);
    }

    /*
    * Returns vote sites list array
    *
    * @param int $accountID
    * @return array
    */
    public function Get_Vote_Sites_List(int $accountID)
    {
        return $this->DB_QUERY()->Get_Vote_Sites_List($this->DB_CONNECTION(), $accountID);
    }

    /*
    * Returns last time voted as unix timestamp
    *
    * @param int $siteID
    * @param int $accountID
    * @return int
    */
    public function Get_Last_Time_Voted_Unixtimestamp(int $siteID, int $accountID)
    {
        return $this->DB_QUERY()->Get_Last_Time_Voted_Unixtimestamp($this->DB_CONNECTION(), $siteID, $accountID);
    }

    /*
    * Returns vote site's defined cooldown in seconds
    *
    * @param int $siteID
    * @return int
    */
    public function Get_Vote_Site_Cooldown(int $siteID)
    {
        return $this->DB_QUERY()->Get_Vote_Site_Cooldown($this->DB_CONNECTION(), $siteID);
    }

    /*
    * Returns json result for vote sites
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Vote_Sites($MD5token, $MD5Username)
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

            return json_encode($this->Get_Vote_Sites_List($accountID), JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns json result for vote click
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @param int $site_id
    * @return json
    */
    public function Vote_For_Site_ID($MD5token, $MD5Username, $site_id)
    {
        $json = (object)
        [
            'Voted'   => false,
            'Points'  => 0,
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

            $site_cooldown_seconds = $this->Get_Vote_Site_Cooldown($site_id);
            $last_vote_unixtimestamp = $this->Get_Last_Time_Voted_Unixtimestamp($site_id, $accountID);
            $last_vote_seconds_past = time() - $last_vote_unixtimestamp;

            $on_cooldown = $last_vote_unixtimestamp == 0 ? false : $last_vote_seconds_past < $site_cooldown_seconds;

            if ($on_cooldown)
            {
                $json->Message = Lang_Error["vote_cooldown"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            $points = $this->Get_Vote_Site_Reward_Amount($site_id);

            $points_added = ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA) ?
                $this->AUTH()->Add_Legion_Core_Vote_Points($accountID, $points) : $this->CMS()->Add_Vote_Points($accountID, $points, $username);

            if ($points_added)
            {
                if ($this->Insert_Vote_Log($site_id, $accountID))
                {
                    $json->Voted = true;
                    $json->Points = $points;
                    $json->Message = Lang_Success["vote_success"];
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