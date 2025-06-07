<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class GIFT_CODES
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
    * Returns gift data as array
    *
    * @param string $code
    * @return array
    */
    private function Get_Gift_Data($code)
    {
        return $this->DB_QUERY()->Get_Gift_Data_Using_Code($this->DB_CONNECTION(), $code);
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
    * Returns true or false if user already redeemed this code
    *
    * @param int $user_id
    * @param string $code
    * @return bolean
    */
    private function Already_Redeemed($user_id, $gift_id)
    {
        return $this->DB_QUERY()->Redeemed_Gift_Code($this->DB_CONNECTION(), $user_id, $gift_id);
    }

    /*
    * Returns true or false if code expired
    *
    * @param string $code
    * @return bolean
    */
    private function Code_Expired($code)
    {
        return $this->DB_QUERY()->Gift_Code_Expired($this->DB_CONNECTION(), $code);
    }

    /*
    * Returns json result for gift code preview
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @param string $code
    * @return json
    */
    public function Get_Gift_Preview($MD5token, $MD5Username, $code)
    {
        $json = (object)
        [
            'IsValid'    => false,
            'PictureUrl' => null,
            'Message'    => Lang_Error["not_authorized"],
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
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->Code_Expired($code))
        {
            $json->Message = Lang_Error["invalid_gift_code"]; 
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
        $user_id = $this->AUTH()->Get_Account_ID_By_Username($username);

        $gift_data = $this->Get_Gift_Data($code);

        if ($gift_data == null || empty($gift_data))
        {
            $json->Message = Lang_Error["invalid_gift_code"]; 
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        $reward_data = $this->Get_Reward_Data($gift_data['reward_id']);

        if ($this->Already_Redeemed($user_id, $gift_data['gift_id']))
        {
            $json->Message = Lang_Error["already_redeemed_gift"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($gift_data['reward_id'] <= 0)
        {
            $json->Message = Lang_Error["error_gift_no_reward_defined"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if($this->AUTH()->Get_Account_Rank_By_ID($user_id) < $gift_data['min_gm_level_allowed'] && $gift_data['min_gm_level_allowed'] != 0)
        {
            $json->Message = Lang_Error["error_gift_min_gm_rank"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if($this->AUTH()->Get_Account_Rank_By_ID($user_id) < $gift_data['max_gm_level_allowed'] && $gift_data['max_gm_level_allowed'] != 0)
        {
            $json->Message = Lang_Error["error_gift_max_gm_rank"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if($this->AUTH()->Get_Account_Rank_By_ID($user_id) != $gift_data['req_exact_gm_level'] && $gift_data['req_exact_gm_level'] != 0)
        {
            $json->Message = Lang_Error["error_gift_exact_gm_rank"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }
        
        $json->IsValid = true;
        $json->PictureUrl = $reward_data['picture_url'];
        $json->Message = $reward_data['title'];

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns json result for redeem gift code
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @param string $code
    * @return json
    */
    public function Redeem_Gift_Code($MD5token, $MD5Username, $code)
    {
        $json = (object)
        [
            'Redeemed'   => false,
            'PictureUrl' => null,
            'Message'    => Lang_Error["not_authorized"],
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
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->Code_Expired($code))
        {
            $json->Message = Lang_Error["invalid_gift_code"]; 
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
        $user_id = $this->AUTH()->Get_Account_ID_By_Username($username);

        $gift_data = $this->Get_Gift_Data($code);

        if ($gift_data == null || empty($gift_data))
        {
            $json->Message = Lang_Error["invalid_gift_code"]; 
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        $reward_data = $this->Get_Reward_Data($gift_data['reward_id']);

        if ($this->Already_Redeemed($user_id, $gift_data['gift_id']))
        {
            $json->Message = Lang_Error["already_redeemed_gift"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($gift_data['reward_id'] <= 0)
        {
            $json->Message = Lang_Error["error_gift_no_reward_defined"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if($this->AUTH()->Get_Account_Rank_By_ID($user_id) < $gift_data['min_gm_level_allowed'] && $gift_data['min_gm_level_allowed'] != 0)
        {
            $json->Message = Lang_Error["error_gift_min_gm_rank"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if($this->AUTH()->Get_Account_Rank_By_ID($user_id) < $gift_data['max_gm_level_allowed'] && $gift_data['max_gm_level_allowed'] != 0)
        {
            $json->Message = Lang_Error["error_gift_max_gm_rank"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if($this->AUTH()->Get_Account_Rank_By_ID($user_id) != $gift_data['req_exact_gm_level'] && $gift_data['req_exact_gm_level'] != 0)
        {
            $json->Message = Lang_Error["error_gift_exact_gm_rank"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        $this->DB_QUERY()->Insert_Claimed_Reward_To_Account_Inventory($this->DB_CONNECTION(), $user_id, $gift_data['reward_id']);
        $this->DB_QUERY()->Insert_Gift_Redeem_Log($this->DB_CONNECTION(), $user_id, $gift_data['gift_id']);
        
        $json->Redeemed = true;
        $json->PictureUrl = $reward_data['picture_url'];
        $json->Message = Lang_Success["gift_code_redeem"];

        return json_encode($json, JSON_PRETTY_PRINT);
    }
}