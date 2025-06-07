<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class UNSTUCKER
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

    private function SOAP_MASTER()
    {
        return new SOAP_MASTER($this->config, $this->database);
    }

    /*
    * Returns character unstuck result as json
    *
    * @return json
    */
    public function Unstuck_Character($MD5token, $MD5Username, $guid)
    {
        $json = (object)
        [
            'Unstucked' => false,
            'Message'   => Lang_Error["not_authorized"]
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

            if (!$this->CHAR()->Is_Character_Owner($accountID, $guid))
            {
                $json->Message = Lang_Error["not_character_owner"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            if (!$this->CHAR()->Is_Character_Online($guid))
            {
                $json->Message = Lang_Error["character_require_online"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }

            // revive
            $this->SOAP_MASTER()->Send_Command("revive ".$this->CHAR()->Get_Character_Name($guid), $this->realmid, $username);

            // unstuck
            $soap_command = "unstuck ".$this->CHAR()->Get_Character_Name($guid)." inn";
            $result = json_decode($this->SOAP_MASTER()->Send_Command($soap_command, $this->realmid, $username));
            $success = $result->Success;

            if ($success)
            {
                $json->Unstucked = true;
                $json->Message = Lang_Success["character_unstuck_ok"];
            }
            else
            {
                $json->Message = $result->Message;
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