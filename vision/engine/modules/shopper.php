<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class SHOPPER
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

    private function CMS()
    {
        return new CMS($this->config, $this->database);
    }

    private function DB_QUERY(int $realmid = 0)
    {
        return new QUERIES($this->database, $realmid);
    }

    /*
    * Returns shop list as json
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Launcher_Shop_List($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Authorized' => false,
            'Message' => Lang_Error["not_authorized"]
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username))
        {
            return json_encode($this->DB_QUERY()->Get_Launcher_Shop_List($this->DB_CONNECTION()), JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Purchase Shop Article returns json
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @param int $article_id
    * @return json
    */
    public function Purchase_Shop_Article($MD5token, $MD5Username, $article_id)
    {
        $json = (object)
        [
            'Purchased' => false,
            'Message'   => Lang_Error["not_authorized"],
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
            $email = $this->AUTH()->Get_Email_By_Username($username);
            $costs = $this->DB_QUERY()->Get_Launcher_Shop_Article_Costs_Using_ID($this->DB_CONNECTION(), $article_id);

            $current_vp = ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA) ? 
                $this->AUTH()->Get_LegionCore_Vote_Points($account_id) : $this->CMS()->Get_Vote_Points($account_id, $username);

            $current_dp_or_bpc = $this->database['auth']['battlepay_credits_as_dp'] ? $this->AUTH()->Get_Battle_Pay_Credits_By_Email($email) : 
                (($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA) ? 
                    $this->AUTH()->Get_LegionCore_Donate_Points($account_id) : $this->CMS()->Get_Donate_Points($account_id, $username));

            // checks if has enough dp, battle pay credits or vp
            if ($current_vp >= $costs['vp_price'] &&  $current_dp_or_bpc >= $costs['dp_or_bpc_price'] )
            {
                $this->DB_QUERY()->Insert_Claimed_Reward_To_Account_Inventory($this->DB_CONNECTION(), $account_id, $costs['reward_id']);

                // removes vote points if necessary
                if ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA)
                {
                    $this->AUTH()->Remove_LegionCore_Vote_Points($account_id, $costs['vp_price']);
                }
                else
                {
                    $this->CMS()->Remove_Vote_Points($account_id, $costs['vp_price'], $username);
                }

                // removes donate points if necessary
                if ($this->database['auth']['battlepay_credits_as_dp'])
                {
                    $this->AUTH()->Remove_Battle_Pay_Credits_By_Email($email, $costs['dp_or_bpc_price']);
                }
                else
                {
                    if ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA)
                    {
                        $this->AUTH()->Remove_LegionCore_Donate_Points($account_id, $costs['dp_or_bpc_price']);
                    }
                    else
                    {
                        $this->CMS()->Remove_Donate_Points($account_id, $costs['dp_or_bpc_price'], $username);
                    }
                }

                $json->Purchased = true;
                $json->Message = Lang_Success["shop_purchased"];
            }
            else
            {
                $json->Message = Lang_Error["not_enough_currency"];
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