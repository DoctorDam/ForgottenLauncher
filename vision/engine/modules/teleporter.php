<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class TELEPORTER
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

    private function LAUNCHER()
    { 
        return new LAUNCHER($this->config, $this->database);
    }

    private function AUTH()
    { 
        return new AUTH($this->config, $this->database);
    }

    private function CHAR()
    {
        return new CHAR($this->config, $this->database, $this->realmid);
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
    * Returns character set location result as json
    *
    * @return json
    */
    public function Teleport_Character($MD5token, $MD5Username, $guid, $teleport_id)
    {
        $json = (object)
        [
            'Teleported' => false,
            'Message'    => Lang_Error["not_authorized"]
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
            $email = $this->AUTH()->Get_Email_By_Username($username);

            if (!$this->CHAR()->Is_Character_Owner($accountID, $guid))
            {
                $json->Message = Lang_Error["not_character_owner"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            if ($this->CHAR()->Is_Character_Online($guid))
            {
                $json->Message = Lang_Error["character_require_offline"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            $isAlliance = in_array($this->DB_QUERY($this->realmid)->Get_Character_Race_By_Guid($this->DB_CONNECTION(), $guid), [1, 3, 4, 7, 11]);

            if (!$this->LAUNCHER()->Is_Teleport_Destination_Faction_Allowed($teleport_id, $isAlliance))
            {
                $json->Message = Lang_Error["teleport_destination_forbidden"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            $coordinates = $this->LAUNCHER($this->realmid)->Get_Launcher_Teleport_Coordinates($teleport_id);
            $costs = $this->LAUNCHER($this->realmid)->Get_Launcher_Teleport_Costs($teleport_id);

            $current_vp = ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA) ? 
                $this->AUTH()->Get_LegionCore_Vote_Points($accountID) : $this->CMS()->Get_Vote_Points($accountID, $username);

            $current_dp_or_bpc = $this->database['auth']['battlepay_credits_as_dp'] ? $this->AUTH()->Get_Battle_Pay_Credits_By_Email($email) : 
                (($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA) ? 
                    $this->AUTH()->Get_LegionCore_Donate_Points($accountID) : $this->CMS()->Get_Donate_Points($accountID, $username));

            // checks if has enough dp, battle pay credits or vp
            if ($current_vp >= $costs['vp_price'] && $current_dp_or_bpc >= $costs['dp_or_bpc_price'] )
            {
                if ($this->DB_QUERY($this->realmid)->Set_Character_Location_m_x_y_z_o_Using_Guid($this->DB_CONNECTION(), $guid,
                    $coordinates['map'], $coordinates['dest_x'], $coordinates['dest_y'], $coordinates['dest_z'], $coordinates['orientation']))
                {
                    // removes vote points if necessary
                    if ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA)
                    {
                        $this->AUTH()->Remove_LegionCore_Vote_Points($accountID, $costs['vp_price']);
                    }
                    else
                    {
                        $this->CMS()->Remove_Vote_Points($accountID, $costs['vp_price'], $username);
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
                            $this->AUTH()->Remove_LegionCore_Donate_Points($accountID, $costs['dp_or_bpc_price']);
                        }
                        else
                        {
                            $this->CMS()->Remove_Donate_Points($accountID, $costs['dp_or_bpc_price'], $username);
                        }
                    }

                    $json->Teleported = true;
                    $json->Message = Lang_Success["character_teleport_ok"];
                }
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