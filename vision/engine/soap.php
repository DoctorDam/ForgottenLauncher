<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class SOAP_MASTER
{
    protected $config;
    protected $database;

    public function __construct($config, $database)
    {
        $this->config = $config;
        $this->database = $database;
    }

    private function LAUNCHER()
    {
        return new LAUNCHER($this->config, $this->database);
    }

    /*
    * Returns json response based on the soap request
    *
    * @param string $admin_user
    * @param string $admin_pass
    * @param string $ingame_command
    * @param int $realmId
    * @param string $target_user
    * @return json
    */
    public function Send_Command(string $ingame_command, int $realmId, string $by_user)
    {
        $jsonObj = (object)
        [
            'Success' => false,
            'Message' => Lang_Error["not_authorized"],
        ];

        $emulatorId = $this->database['auth']['emulator_id'];

        $emulatorMappings = 
        [
            TRINITYCORE_WOTLK           => 'urn:TC',
            TRINITYCORE_CATA            => 'urn:TC',
            TRINITYCORE_DRAGONFLIGHTS   => 'urn:TC',
            MANGOS_ONE                  => 'urn:MaNGOS',
            MANGOS_TWO                  => 'urn:MaNGOS',
            MANGOS_THREE                => 'urn:MaNGOS',
            MANGOS_FOUR                 => 'urn:MaNGOS',
            MANGOS_FIVE                 => 'urn:MaNGOS',
            CMANGOS_CLASSIC             => 'urn:MaNGOS',
            CMANGOS_TBC                 => 'urn:MaNGOS',
            CMANGOS_WOTLK               => 'urn:MaNGOS',
            CMANGOS_CATA                => 'urn:MaNGOS',
            VMANGOS_CLASSIC             => 'urn:MaNGOS',
            AZEROTHCORE_WOTLK           => 'urn:AC',
            SKYFIRE_MOP                 => 'urn:SF',
            ASHMANE_SHADOWLANDS         => 'urn:SF',
            ATLANTISS_CATA              => 'urn:TC',
            LEGIONCORE_735_SHA          => 'urn:TC',
        ];

        $soap_urn = $emulatorMappings[$emulatorId] ?? 'urn:MaNGOS';  

        $client = new \SoapClient(NULL,
        [
            "location" => "http://".$this->database['realms'][$realmId]['soap']['hostaddress'].":".$this->database['realms'][$realmId]['soap']['port']."/",
            "uri" => $soap_urn,
            "style" => SOAP_RPC,
            'login' => $this->database['realms'][$realmId]['soap']['account'],
            'password' => $this->database['realms'][$realmId]['soap']['password']
        ]);

        try 
        {
            $result = $client->executeCommand(new \SoapParam($ingame_command, "command"));
            // echo $result;

            $jsonObj->Message = Lang_Success["soap_successful_request"];
            $jsonObj->Success = true;

            if (!empty($by_user))
            {
                $this->LAUNCHER()->Add_Soap_Command_Log($by_user, $realmId, $ingame_command);
            }
            else
            {
                $this->LAUNCHER()->Add_Soap_Command_Log($this->database['realms'][$realmId]['soap']['account'], $realmId, $ingame_command);
            }
        }
        catch (\Exception $e)
        {
            // echo $e->getMessage();
            $jsonObj->Message = Lang_Error["soap_unauthorized"]." Reason: ".$e->getMessage();
        }

        return json_encode($jsonObj, JSON_PRETTY_PRINT);
    }
}