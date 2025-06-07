<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class DISCORD_RPC
{
    protected $config;
    protected $database;

    private $cache_folder_path = './cache';
    private $cache_file_path = "./cache/realms_online_players.json";

    public function __construct($config, $database)
    {
        $this->config = $config;
        $this->database = $database;
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

    /*
    * Returns true or false if cache folder exists
    *
    * @return bool
    */
    private function cache_folder_exist()
    {
        $path = realpath($this->cache_folder_path);

        return ($path !== false AND is_dir($path)) ? $path : false;
    }

    /*
    * Returns true or false if cache file exists
    *
    * @return bool
    */
    private function cache_file_exists()
    {
        return file_exists($this->cache_file_path);
    }

    /*
    * Returns true or false if cache is a valid json format
    *
    * @return bool
    */
    private function is_valid_cache_file()
    {
        try
        {
            if ($this->cache_file_exists())
            {
                $contents = file_get_contents($this->cache_file_path);

                $json = json_decode($contents);

                if ($json !== null)
                {
                    return true;
                }
            }
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendOtherError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    /*
    * Returns cache json
    *
    * @return json
    */
    private function get_cache_json()
    {
        $json = (object) [];

        try
        {
            if ($this->cache_file_exists())
            {
                $contents = file_get_contents($this->cache_file_path);

                $json = json_decode($contents);

                if ($json !== null)
                {
                    return json_encode($json, JSON_PRETTY_PRINT);
                }
            }
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendOtherError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $json;
    }

    /*
    * Updates cache file
    */
    public function Update_Cache_File()
    {
        try
        {
            if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_DISCORD_INGAME_RPC))
            {
                return; // skips everything
            }
    
            if (!$this->cache_folder_exist()) 
            {
                mkdir($this->cache_folder_path);
            }
            
            $online_characters = [];
    
            foreach ($this->database['realms'] as $realmId => $realmData)
            {
                $online_characters = $this->CHAR($realmId)->Get_Online_Characters();
            }
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendOtherError(__FUNCTION__ . " : " . $e->getMessage());
        }
        finally
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_DISCORD_INGAME_RPC);
        }

        file_put_contents($this->cache_file_path, json_encode($online_characters, JSON_PRETTY_PRINT));
    }

    /*
    * Returns account's character list as json
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Online_Character($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Message' => Lang_Error["not_authorized"]
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username)) 
        {
            $this->Update_Cache_File();

            sleep(1);
            
            $username = $this->AUTH()->Get_Username_From_MD5Hash($MD5Username);
            $account_id = $this->AUTH()->Get_Account_ID_By_Username($username);

            if ($this->is_valid_cache_file())
            {
                $decoded_json = json_decode($this->get_cache_json());

                foreach ($decoded_json as $character)
                {
                    if ($character->account == $account_id)
                    {
                        return json_encode($character, JSON_PRETTY_PRINT);
                        break;
                    }
                }
            }
            else
            {
                $json->Message = Lang_Error["invalid_token"];
                return json_encode($json, JSON_PRETTY_PRINT);
            }
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }
    }
}