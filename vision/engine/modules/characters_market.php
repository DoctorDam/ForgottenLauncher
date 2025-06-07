<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class CHARACTERS_MARKET
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

    private function CHAR($realm_id)
    { 
        return new CHAR($this->config, $this->database, $realm_id);
    }

    private function CMS()
    {
        return new CMS($this->config, $this->database);
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
    * Returns market sales info array
    *
    * @return array
    */
    public function Get_Market_Sales()
    {
        return $this->DB_QUERY()->Get_Market_Sales($this->DB_CONNECTION());
    }

    /*
    * Returns market bids info array
    *
    * @param int $sale_id
    * @return array
    */
    public function Get_Market_Bids_For($sale_id)
    {
        return $this->DB_QUERY()->Get_Market_Bids_For_Sale($this->DB_CONNECTION(), $sale_id);
    }

    /*
    * Returns market bids notifications
    *
    * @param int $account_id
    * @return array
    */
    public function Get_Market_Bids_Notifications($account_id)
    {
        return $this->DB_QUERY()->Get_Market_Unread_Notifications_For($this->DB_CONNECTION(), $account_id);
    }

    /*
    * Returns true or false if any notifications were marked as read
    *
    * @param int $account_id
    * @return bool
    */
    public function Mark_Bids_Notifications_As_Read($account_id)
    {
        return $this->DB_QUERY()->Characters_Market_Mark_Notifications_As_Read_For($this->DB_CONNECTION(), $account_id);
    }

    /*
    * Returns specific market sale info array
    *
    * @return array
    */
    public function Get_Market_Sale_Info($sale_id)
    {
        return $this->DB_QUERY()->Get_Market_Sale_Info($this->DB_CONNECTION(), $sale_id);
    }

    /*
    * Returns true or false if character is already listed for sale
    *
    * @param int $characterGUID
    * @param int $realmID
    * @return bool
    */
    public function Is_Character_On_Sale($characterGUID, $realmID)
    {
        return $this->DB_QUERY($realmID)->Character_Market_Sale_Exists($this->DB_CONNECTION(), $characterGUID, $realmID);
    }

    /*
    * Returns true or false if character added successfuly to market list
    *
    * @param int $characterGUID
    * @param int $ownerAccountId
    * @param int $allow_bidding
    * @param int $price
    * @param int $realmID
    * @param int $duration_seconds
    * @return bool
    */
    public function Add_To_Sale($characterGUID, $ownerAccountId, $allow_bidding, $price, $realmID, $duration_seconds)
    {
        return $this->DB_QUERY()->Characters_Market_Add($this->DB_CONNECTION(), $characterGUID, $ownerAccountId, $realmID, $allow_bidding, $price, $duration_seconds);
    }

    /*
    * Returns true or false if successfuly added market log
    *
    * @param int $sale_id
    * @param int $character_guid
    * @param int $old_account_id
    * @param int $new_account_id
    * @param int $realm_id
    * @return bool
    */
    public function Add_Market_Log($sale_id, $character_guid, $old_account_id, $new_account_id, $realm_id)
    {
        return $this->DB_QUERY()->Insert_Characters_Market_Log($this->DB_CONNECTION(), $sale_id, $character_guid, $old_account_id, $new_account_id, $realm_id);
    }

    /*
    * Returns true or false if successfuly added market bids won table
    *
    * @param int $buyer_id
    * @param int $character_name
    * @param int $character_race
    * @param int $character_class
    * @param int $character_level
    * @return bool
    */
    public function Add_To_Market_Bids_Won($buyer_id, $name, $race, $class, $level, $gender)
    {
        return $this->DB_QUERY()->Insert_To_Characters_Market_Bids_Won($this->DB_CONNECTION(), $buyer_id, $name, $race, $class, $level, $gender);
    }

    /*
    * Adds bid to market sale
    *
    * @param int $buyer_id
    * @param int $market_id
    * @param int $bid_amount
    * @return bool
    */
    public function Add_Market_Bid($buyer_id, $market_id, $bid_amount)
    {
        return $this->DB_QUERY()->Insert_Characters_Market_Bid($this->DB_CONNECTION(), $buyer_id, $market_id, $bid_amount);
    }

    /*
    * Delete market sale bid
    *
    * @param int $buyer_id
    * @param int $sale_id
    * @return bool
    */
    public function Delete_Market_Bid($buyer_id, $sale_id)
    {
        return $this->DB_QUERY()->Delete_Characters_Market_Bid($this->DB_CONNECTION(), $buyer_id, $sale_id);
    }

    /*
    * Update market sale price
    *
    * @param int $sale_id
    * @param int $price
    * @return bool
    */
    public function Update_Market_Price($sale_id, $price)
    {
        return $this->DB_QUERY()->Update_Characters_Market_Sale_Price($this->DB_CONNECTION(), $sale_id, $price);
    }

    /*
    * Returns any current bidded amount made by user for a specific sale
    *
    * @param int $buyer_id
    * @param int $sale_id
    * @return int
    */
    public function Get_Current_Market_Bidded_Amount($buyer_id, $sale_id)
    {
        return $this->DB_QUERY()->Get_Characters_Market_Bidded_Amount_For($this->DB_CONNECTION(), $buyer_id, $sale_id);
    }

    /*
    * Returns current sale highest bid
    *
    * @param int $sale_id
    * @return int
    */
    public function Get_Current_Market_Highest_Bid_For($sale_id)
    {
        return $this->DB_QUERY()->Get_Characters_Market_Highest_Bid_For($this->DB_CONNECTION(), $sale_id);
    }

    /*
    * Delete market bid for a specific sale
    *
    * @param int $sale_id
    *
    */
    public function Delete_All_Market_Bids_For($sale_id)
    {
        $this->DB_QUERY()->Delete_All_Characters_Market_Bids_For($this->DB_CONNECTION(), $sale_id);
    }

    /*
    * Deletes a specific market sale from the market
    *
    * @param int $sale_id
    * @return bool
    */
    public function Delete_Market_Sale($sale_id)
    {
        return $this->DB_QUERY()->Characters_Market_Delete_Sale($this->DB_CONNECTION(), $sale_id);
    }

    /*
    * Returns true or false if sale expired
    *
    * @param int $sale_id
    * @return bool
    */
    public function Sale_Expired($sale_id)
    {
        $sales = $this->Get_Market_Sales();
        
        foreach($sales as $key => $sale)
        {
            if ($sale['id'] == $sale_id)
            {
                return $sale["seconds_left"] <= 0;
            }
        }

        return false;
    }

    /*
    * Restores all bids points to their owners
    *
    * @param int $sale_id
    *
    */
    public function Restore_Market_Bids_Points($sale_id)
    {
        $bids = $this->Get_Market_Bids_For($sale_id);

        foreach($bids as $key => $bid)
        {
            $account_id = $bid['buyer_id'];
            $username = $this->AUTH()->Get_Username_By_Account_ID($account_id);

            if ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA)
            {
                $this->AUTH()->Add_Legion_Core_Donate_Points($account_id, $bid['bid_amount']);
            }
            else
            {
                $this->CMS()->Add_Donate_Points($account_id, $bid['bid_amount'], $username);
            }
        }
    }

    /*
    * Returns json response for characters market list
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Market_List($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Authorized' => false,
            'Message'    => Lang_Error["not_authorized"]
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->DB_QUERY()->Get_Username_Using_Auth_Token_and_Username($this->DB_CONNECTION(), $MD5token, $MD5Username))
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            $account_id = $this->AUTH()->Get_Account_ID_By_Username($username);

            $this->On_Expired_Auction_Sales(); // must be placed as first
            $this->On_Expired_Non_Auction_Sales(); // must be placed as second

            $sales = $this->Get_Market_Sales();

            foreach($sales as $key => $value)
            {
                $char_info = $this->CHAR($value['realm_id'])->Get_Character_Info($value['character_guid']);
                $char_professions = $this->CHAR($value['realm_id'])->Get_Character_Professions($value['character_guid']);

                $sales[$key]['char_info'] = $char_info;
                $sales[$key]['professions'] = $char_professions;
            }
    
            return json_encode($sales, JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns json response when someone is adding a character on sale
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @param int $characterGUID
    * @param int $realmID
    * @return json
    */
    public function Add_To_Market($MD5token, $MD5Username, $characterGUID, $realmID, $duration, $allow_bidding, $price)
    {
        $json = (object)
        [
            'Authorized' => false,
            'Message'    => Lang_Error["not_authorized"]
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        // check if user is validated
        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username))
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            $account_id = $this->AUTH()->Get_Account_ID_By_Username($username);
            $allow_bidding = filter_var($allow_bidding, FILTER_VALIDATE_BOOLEAN);

            // check if character is listed for sale
            if ($this->Is_Character_On_Sale($characterGUID, $realmID))
            {
                $json->Message = Lang_Error["characters_market_exists"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // check if owner of the character
            if (!$this->CHAR($realmID)->Is_Character_Owner($account_id, $characterGUID))
            {
                $json->Message = Lang_Error["not_character_owner"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // check if character is banned
            if ($this->CHAR($realmID)->Is_Character_Banned($characterGUID))
            {
                $json->Message = Lang_Error["character_is_banned"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // check for minimum price
            $min_price = $this->config['characters_market']['minimum_price'];
            if ($price < $min_price)
            {
                $json->Message = Lang_Error["characters_market_min_price"]." ".$min_price;
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // check for maximum price
            $max_price = $this->config['characters_market']['maximum_price'];
            if ($price > $max_price)
            {
                $json->Message = Lang_Error["characters_market_max_price"]." ".$max_price;
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            $char_info = $this->CHAR($realmID)->Get_Character_Info($characterGUID);

            // check for minimum player level
            $min_player_level = $this->config['characters_market']['minimum_player_level'];
            if ($char_info['level'] < $min_player_level)
            {
                $json->Message = Lang_Error["characters_market_min_level"]." ".$min_player_level;
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // check for minimum player gold
            $min_player_gold = $this->config['characters_market']['minimum_player_gold'];
            if ($char_info['money'] < $min_player_gold)
            {
                $json->Message = Lang_Error["characters_market_min_gold"]." ".$min_player_gold."g";
                return json_encode($json, JSON_PRETTY_PRINT);
            }
            
            // check if duration selected is valid
            if (isset($this->config['characters_market']['hours_durations'][$duration]))
            {
                $json->Message = Lang_Error["characters_market_invalid_duration"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // check if character is online
            if ($char_info['online'])
            {
                $soap_result = json_decode
                (
                    $this->SOAP_MASTER()->Send_Command("kick ".$this->CHAR($realmID)->Get_Character_Name($characterGUID), $realmID, $username)
                );

                // kick character in case is online, check if soap command succeeded
                if (!$soap_result->Success)
                {
                    $json->Message = $soap_result->Message;
                    return json_encode($json, JSON_PRETTY_PRINT);
                }
            }

            // wait
            sleep(1);
            
            // remove character from old account id
            if (!$this->CHAR($realmID)->Change_Account_ID($characterGUID, 0))
            {
                $json->Message = Lang_Error["character_account_id_change_fail"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // if character added to market list
            if($this->Add_To_Sale($characterGUID, $account_id, (int)$allow_bidding, $price, $realmID, $duration*60*60))
            {
                $json->Authorized = true;
                $json->Message = Lang_Success["characters_market_added"];
            }
            else
            {
                // restore ownership
                $this->CHAR($realmID)->Change_Account_ID($characterGUID, $account_id);

                $json->Message = Lang_Error["characters_market_insert_fail"];
                return json_encode($json, JSON_PRETTY_PRINT);
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
    * Returns json response for character cancel sale request
    * @param string $MD5token
    * @param string $MD5Username
    * @param int $sale_id
    * @return json
    */
    public function Cancel_Sale($MD5token, $MD5Username, $sale_id)
    {
        $json = (object)
        [
            'Authorized' => false,
            'Message'    => Lang_Error["not_authorized"]
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        // check if user is validated
        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username))
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            $account_id = $this->AUTH()->Get_Account_ID_By_Username($username);

            if($this->DB_QUERY()->Characters_Market_Set_As_Expired($this->DB_CONNECTION(), $sale_id, $account_id))
            {
                $json->Authorized = true;
                $json->Message = Lang_Success["characters_market_cancelled"];
                $this->On_Expired_Non_Auction_Sales();
                $this->Restore_Market_Bids_Points($sale_id);
                $this->Delete_All_Market_Bids_For($sale_id);
            }
            else
            {
                $json->Message = Lang_Error["characters_market_cancel_fail"]; // not owner or sale / query failed / no rows found
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
    * Returns json string for character buy sale request
    * @param string $MD5token
    * @param string $MD5Username
    * @param int $sale_id
    * @return json
    */
    public function Buy_Sale($MD5token, $MD5Username, $sale_id)
    {
        $json = (object)
        [
            'Authorized' => false,
            'Message'    => Lang_Error["not_authorized"]
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        // check if user is validated
        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username))
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            $account_id = $this->AUTH()->Get_Account_ID_By_Username($username);
            $email = $this->AUTH()->Get_Email_By_Username($username);
            $sale_info = $this->Get_Market_Sale_Info($sale_id);

            $purchased = false;

            // check if character is listed for sale
            if (!$this->Is_Character_On_Sale($sale_info['character_guid'], $sale_info['realm_id']))
            {
                $json->Message = Lang_Error["characters_market_missing"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // check if owner of the character
            if ($sale_info['owner_account_id'] == $account_id)
            {
                $json->Message = Lang_Error["characters_market_buyself"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // check if character is banned
            if ($this->CHAR($sale_info['realm_id'])->Is_Character_Banned($sale_info['character_guid']))
            {
                $json->Message = Lang_Error["character_is_banned"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            $current_dp = ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA) ? 
                    $this->AUTH()->Get_LegionCore_Donate_Points($account_id) : $this->CMS()->Get_Donate_Points($account_id, $username);

            // check if has enough dp
            if ($current_dp < $sale_info['price'])
            {
                $json->Message = Lang_Error["not_enough_currency"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // removes donate points or battlepaycredits
            if ($this->database['auth']['battlepay_credits_as_dp'])
            {
                $this->AUTH()->Remove_Battle_Pay_Credits_By_Email($email, $sale_info['price']);
            }
            else
            {
                if ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA)
                {
                    if ($this->AUTH()->Remove_LegionCore_Donate_Points($account_id, $sale_info['price']))
                    {
                        $json->Authorized = true;
                        $purchased = $json->Authorized;
                    }
                }
                else
                {
                    if ($this->CMS()->Remove_Donate_Points($account_id, $sale_info['price'], $username))
                    {
                        $json->Authorized = true;
                        $purchased = $json->Authorized;
                    }
                }
            }

            // on successful purchase
            if ($purchased)
            {
                $json->Message = Lang_Success["characters_market_purchased"];

                $this->Add_Market_Log($sale_id, $sale_info['character_guid'], $sale_info['owner_account_id'], $account_id, $sale_info['realm_id']);

                $this->CHAR($sale_info['realm_id'])->Change_Account_ID($sale_info['character_guid'], $account_id);

                $seller_points_reward = $sale_info['price'] - ($sale_info['price'] * ($this->config['characters_market']['commission_percent']/100));

                if ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA)
                {
                    // adds dp to seller
                    $this->AUTH()->Add_Legion_Core_Donate_Points($sale_info['owner_account_id'], $seller_points_reward);
                }
                else
                {
                    // adds dp to seller
                    $this->CMS()->Add_Donate_Points($sale_info['owner_account_id'], $seller_points_reward, $username);
                }
                
                // deletes sale from the market
                $this->Delete_Market_Sale($sale_id);
            }
            else
            {
                $json->Message = Lang_Error["characters_market_buy_fail"]; // not owner or sale / query failed / no rows found
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
    * Returns json string for character sale bidding
    * @param string $MD5token
    * @param string $MD5Username
    * @param int $sale_id
    * @return json
    */
    public function Bid_Sale($MD5token, $MD5Username, $sale_id, $bid_amount)
    {
        $json = (object)
        [
            'Authorized' => false,
            'Message'    => Lang_Error["not_authorized"]
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        // check if user is validated
        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username))
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            $account_id = $this->AUTH()->Get_Account_ID_By_Username($username);
            $email = $this->AUTH()->Get_Email_By_Username($username);
            $sale_info = $this->Get_Market_Sale_Info($sale_id);

            $bidded = false;

            // checks if sale info exists
            if (empty($sale_info))
            {
                $json->Message = Lang_Error["characters_market_missing"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // check if character is listed for sale
            if (!$this->Is_Character_On_Sale($sale_info['character_guid'], $sale_info['realm_id']))
            {
                $json->Message = Lang_Error["characters_market_missing"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // check if owner of the character
            if ($sale_info['owner_account_id'] == $account_id)
            {
                $json->Message = Lang_Error["characters_market_buyself"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // check if character is banned
            if ($this->CHAR($sale_info['realm_id'])->Is_Character_Banned($sale_info['character_guid']))
            {
                $json->Message = Lang_Error["character_is_banned"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            $current_dp = ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA) ? 
                    $this->AUTH()->Get_LegionCore_Donate_Points($account_id) : $this->CMS()->Get_Donate_Points($account_id, $username);

            $any_current_bidded_amount = $this->Get_Current_Market_Bidded_Amount($account_id, $sale_id);

            // check if has enough dp
            if ($current_dp < ($bid_amount - $any_current_bidded_amount))
            {
                $json->Message = Lang_Error["not_enough_currency"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // check if bid amount is lower than current highest amount
            if ($bid_amount < $sale_info['price'])
            {
                $json->Message = str_replace("{0}", $sale_info['price'], Lang_Error["characters_market_bid_fail_not_enough"]);
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // removes donate points or battlepaycredits
            if ($this->database['auth']['battlepay_credits_as_dp'])
            {
                $this->AUTH()->Remove_Battle_Pay_Credits_By_Email($email, $sale_info['price']);

                $json->Authorized = true;
                $bidded = $json->Authorized;
            }
            else
            {
                if ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA)
                {
                    if ($this->AUTH()->Remove_LegionCore_Donate_Points($account_id, ($bid_amount - $any_current_bidded_amount)))
                    {
                        $json->Authorized = true;
                        $bidded = $json->Authorized;
                    }
                }
                else
                {
                    if ($this->CMS()->Remove_Donate_Points($account_id, ($bid_amount - $any_current_bidded_amount), $username))
                    {
                        $json->Authorized = true;
                        $bidded = $json->Authorized;
                    }
                }
            }

            // on successful bid
            if ($bidded)
            {
                $json->Message = Lang_Success["characters_market_bid_added"];

                $this->Delete_Market_Bid($account_id, $sale_id);

                $this->Add_Market_Bid($account_id, $sale_id, $bid_amount);

                $this->Update_Market_Price($sale_id, $bid_amount);
            }
            else
            {
                $json->Message = Lang_Error["characters_market_bid_fail"]; // could not remove donation points
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
    * Handles expired non auctions
    */
    public function On_Expired_Non_Auction_Sales()
    {
        $sales = $this->Get_Market_Sales();
        
        foreach($sales as $key => $sale)
        {
            if ($sale["seconds_left"] <= 0)
            {
                $this->CHAR($sale["realm_id"])->Change_Account_ID($sale["character_guid"], $sale["owner_account_id"]);
            }
        }

        $this->DB_QUERY()->Characters_Market_Delete_Expired_Non_Auctions($this->DB_CONNECTION());
    }

    /*
    * Handles expired auctions
    */
    public function On_Expired_Auction_Sales()
    {
        $sales = $this->Get_Market_Sales();

        foreach($sales as $key => $sale)
        {
            $allow_bidding = filter_var($sale['allow_bidding'], FILTER_VALIDATE_BOOLEAN);

            // checks if sale is an auction
            if ($allow_bidding)
            {
                $sale_id = $sale['id'];

                // check if sale expired
                if ($this->Sale_Expired($sale_id))
                {
                    // gets the highest bid info
                    $highest_bid = $this->Get_Current_Market_Highest_Bid_For($sale_id);
    
                    // restore all bid points made by everyone includding winner
                    $this->Restore_Market_Bids_Points($sale_id);
    
                    // if there is any highest bid
                    if (!empty($highest_bid))
                    {
                        $winner_account_id = $highest_bid['buyer_id'];
                        $seller_account_id = $sale['owner_account_id'];
    
                        $winner_username = $this->AUTH()->Get_Username_By_Account_ID($winner_account_id);
                        $seller_username = $this->AUTH()->Get_Username_By_Account_ID($seller_account_id);

                        $winner_email = $this->AUTH()->Get_Email_By_Username($winner_username);
    
                        $won_char_info = $this->CHAR($sale['realm_id'])->Get_Character_Info($sale['character_guid']);
    
                        // logs this sale
                        $this->Add_Market_Log($sale_id, $sale['character_guid'], $seller_account_id , $winner_account_id, $sale['realm_id']);

                        // adds to market notifications
                        $this->Add_To_Market_Bids_Won($winner_account_id,  $won_char_info['name'], $won_char_info['race'], $won_char_info['class'], $won_char_info['level'], $won_char_info['gender']);
    
                        // adds character to winner account
                        $this->CHAR($sale['realm_id'])->Change_Account_ID($sale['character_guid'], $winner_account_id);
    
                        // removes donate points or battlepaycredits
                        if ($this->database['auth']['battlepay_credits_as_dp'])
                        {
                            $this->AUTH()->Remove_Battle_Pay_Credits_By_Email($winner_email, $sale['price']);
                        }
                        else
                        {
                            if ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA)
                            {
                                $this->AUTH()->Remove_LegionCore_Donate_Points($winner_account_id, $sale['price']);
                            }
                            else
                            {
                                $this->CMS()->Remove_Donate_Points($winner_account_id, $sale['price'], $winner_username);
                            }
                        }
    
                        $seller_points_reward = $sale['price'] - ($sale['price'] * ($this->config['characters_market']['commission_percent']/100));
    
                        if ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA)
                        {
                            // adds dp to seller
                            $this->AUTH()->Add_Legion_Core_Donate_Points($seller_account_id , $seller_points_reward);
                        }
                        else
                        {
                            // adds dp to seller
                            $this->CMS()->Add_Donate_Points($seller_account_id , $seller_points_reward, $seller_username);
                        }
                    }
    
                    // remove all market bids for this sale
                    $this->Delete_All_Market_Bids_For($sale_id);
                    
                    // deletes sale from the market
                    $this->Delete_Market_Sale($sale_id);
                }
            }
        }
    }

    /*
    * Returns json fpor market bids won notifications
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Market_Notifications($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Authorized' => false,
            'Message'    => Lang_Error["not_authorized"]
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        $notifications = [];

        // check if user is validated
        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username))
        {
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            $account_id = $this->AUTH()->Get_Account_ID_By_Username($username);

            $notifications = $this->Get_Market_Bids_Notifications($account_id);

            if (!empty($notifications))
            {
                $this->Mark_Bids_Notifications_As_Read($account_id);
            }
        }

        return json_encode($notifications, JSON_PRETTY_PRINT);
    }
}