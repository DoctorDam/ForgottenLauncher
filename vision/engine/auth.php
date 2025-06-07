<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

use Laizerox\Wowemu\SRP\UserClient;

require_once('./vendor/autoload.php');
require_once('srp6.php');

class AUTH
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
            $auth = $this->database['auth'];
    
            $connection = mysqli_connect
            (
                $auth['mysql_hostname'],
                $auth['mysql_user'],
                $auth['mysql_pass'],
                $auth['mysql_database'],
                $auth['mysql_port']
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

    public function TEST_DB_CONNECTION()
    {
        try
        {
            $auth = $this->database['auth'];
        
            $connection = mysqli_connect(
                $auth['mysql_hostname'],
                $auth['mysql_user'],
                $auth['mysql_pass'],
                $auth['mysql_database'],
                $auth['mysql_port']
            );
        
            if (mysqli_connect_errno()) 
            {
                if ($connection) 
                {
                    mysqli_close($connection);
                }
                return "AUTH DB CONNECTION FAILED..";
            }
            else
            {
                mysqli_close($connection);
                return "AUTH DB CONNECTION OK";
            }
        }
        catch (\Exception $e)
        {
            return "AUTH DB ERROR: ".$e->getMessage();
        }
    }

    public function RUN_DB_QUERY($query)
    {
        $db_connection = $this->DB_CONNECTION();

        try
        {
            $stmt = $db_connection->prepare($query);
            $exec = $stmt->execute();
            $stmt->close();
            $db_connection->close();

            return $exec;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    private function LAUNCHER()
    {
        return new LAUNCHER($this->config, $this->database);
    }

    private function CMS()
    {
        return new CMS($this->config, $this->database);
    }

    private function SRP6()
    {
        return new SRP6();
    }

    private function DB_QUERY(int $realmid = 0)
    {
        return new QUERIES($this->database, $realmid);
    }

    /*
    * Checks if account exists
    *
    * @param string $username
    * @return bool
    */
    public function Account_Exists(string $username)
    {
        return (bool)$this->DB_QUERY()->Get_Account_ID_Using_Username($this->DB_CONNECTION(), $username);
    }

    /*
    * Checks if account is active
    *
    * @param string $username
    * @return bool
    */
    public function Account_is_Active(string $username)
    {
        return $this->DB_QUERY()->Get_Account_Is_Active($this->DB_CONNECTION(), $username);
    }

    /*
    * Checks if battlenet account exists
    *
    * @param string $email
    * @return bool
    */
    private function Battlenet_Account_Exists(string $email)
    {
        return (bool)$this->DB_QUERY()->Get_Battlenet_ID_Using_Email($this->DB_CONNECTION(), $email);
    }

    /*
    * Checks if email exists
    *
    * @param string $email
    * @return bool
    */
    public function Email_Exists(string $email)
    {
        return (bool)$this->DB_QUERY()->Get_Account_ID_Using_Email($this->DB_CONNECTION(), $email);
    }

    /*
    * Checks if account is banned
    *
    * @param string $username
    * @return bool
    */
    private function Account_is_Banned(string $username)
    {
        return (bool)$this->DB_QUERY()->Get_Active_Banned_Using_ID($this->DB_CONNECTION(), $this->Get_Account_ID_By_Username($username));
    }

    /*
    * Returns account ban duration string
    *
    * @param string $md5hash
    * @return string
    */
    public function Get_Account_Ban_Duration_String(int $accountID)
    {
        return $this->DB_QUERY()->Get_Account_Ban_Duration_String($this->DB_CONNECTION(), $accountID);
    }

    /*
    * Returns username from md5 hash
    *
    * @param string $md5hash
    * @return string
    */
    public function Get_Username_From_MD5Hash(string $md5hash)
    {
        return $this->DB_QUERY()->Get_Username_Using_MD5_Username($this->DB_CONNECTION(), $md5hash);
    }

    /*
    * Returns salt and verifier
    *
    * @param string $md5hash
    * @return array
    */
    private function Get_User_Salt_And_Verifier(string $username)
    {
        return $this->DB_QUERY()->Get_Salt_Verifier_Using_Username($this->DB_CONNECTION(), $username);
    }

    /*
    * Returns email from md5 hash
    *
    * @param string $md5hash
    * @return string
    */
    private function Get_Email_From_MD5Hash(string $md5hash)
    {
        return $this->DB_QUERY()->Get_Email_Using_MD5_Email($this->DB_CONNECTION(), $md5hash);
    }

    /*
    * Returns username from account id
    *
    * @param int $accountID
    * @return string
    */
    public function Get_Username_By_Account_ID(int $accountID)
    {
        return $this->DB_QUERY()->Get_Username_Using_Account_ID($this->DB_CONNECTION(), $accountID);
    }

    /*
    * Returns account id from username
    *
    * @param string $username
    * @return int
    */
    public function Get_Account_ID_By_Username(string $username)
    {
        return $this->DB_QUERY()->Get_Account_ID_Using_Username($this->DB_CONNECTION(), $username);
    }

    /*
    * Returns account gm level from account id
    *
    * @param int $accountID
    * @return int
    */
    public function Get_Account_Rank_By_ID(int $accountID)
    {
        return $this->DB_QUERY()->Get_Account_Rank_Using_Account_ID($this->DB_CONNECTION(), $accountID);
    }

    /*
    * Returns username from email
    *
    * @param string $email
    * @return string
    */
    public function Get_Username_By_Email(string $email)
    {
        return $this->DB_QUERY()->Get_Username_Using_Email($this->DB_CONNECTION(), $email);
    }

    /*
    * Returns last login date timestamp
    *
    * @param int $id
    * @return string
    */
    public function Get_Last_Login_Date_Timestamp_By_ID(int $accountID)
    {
        return $this->DB_QUERY()->Get_Last_Login_Timestamp_Using_Account_ID($this->DB_CONNECTION(), $accountID);
    }

    /*
    * Returns last login ip
    *
    * @param int $id
    * @return string
    */
    public function Get_Last_Login_IP_By_ID(int $accountID)
    {
        return $this->DB_QUERY()->Get_Last_Login_IP_Using_Account_ID($this->DB_CONNECTION(), $accountID);
    }

    /*
    * Returns battle pay credits
    *
    * @param string $email
    * @return int
    */
    public function Get_Battle_Pay_Credits_By_Email(int $email)
    {
        return $this->DB_QUERY()->Get_Battle_Pay_Credits_Using_Email($this->DB_CONNECTION(), $email);
    }

    /*
    * Removes battle pay credits from email
    *
    * @param string $email
    *
    */
    public function Remove_Battle_Pay_Credits_By_Email(string $email, int $amount)
    {
        $this->DB_QUERY()->Remove_BattlePayCredits_Using_Email($this->DB_CONNECTION(), $email, $amount);
    }

    /*
    * Returns email from username
    *
    * @param string $username
    * @return string
    */
    public function Get_Email_By_Username(string $username)
    {
        return $this->DB_QUERY()->Get_Email_Using_Username($this->DB_CONNECTION(), $username);
    }

    /*
    * Returns legioncore vote points
    *
    * @param int $accountID
    * @return int
    */
    public function Get_LegionCore_Vote_Points(int $accountID)
    {
        return $this->DB_QUERY()->LegionCore_Get_Vote_Points($this->DB_CONNECTION(), $accountID);
    }

    /*
    * Returns legioncore donate points
    *
    * @param int $accountID
    * @return int
    */
    public function Get_LegionCore_Donate_Points(int $accountID)
    {
        return $this->DB_QUERY()->LegionCore_Get_Donate_Points($this->DB_CONNECTION(), $accountID);
    }

    /*
    * Adds legioncore vote points
    *
    * @param int $accountID
    * @return bool
    */
    public function Add_Legion_Core_Vote_Points(int $accountID, int $amount)
    {
        return $this->DB_QUERY()->LegionCore_Add_Vote_Points($this->DB_CONNECTION(), $accountID, $amount);
    }

    /*
    * Adds legioncore donate points
    *
    * @param int $accountID
    * @return bool
    */
    public function Add_Legion_Core_Donate_Points(int $accountID, int $amount)
    {
        return $this->DB_QUERY()->LegionCore_Add_Donate_Points($this->DB_CONNECTION(), $accountID, $amount);
    }

    /*
    * Removes legioncore vote points
    *
    * @param int $accountID
    * @param int $amount
    *
    */
    public function Remove_LegionCore_Vote_Points(int $accountID, int $amount)
    {
        return $this->DB_QUERY()->LegionCore_Remove_Vote_Points($this->DB_CONNECTION(), $accountID, $amount);
    }

    /*
    * Removes legioncore donate points
    *
    * @param int $accountID
    * @param int $amount
    *
    */
    public function Remove_LegionCore_Donate_Points(int $accountID, int $amount)
    {
        return $this->DB_QUERY()->LegionCore_Remove_Donate_Points($this->DB_CONNECTION(), $accountID, $amount);
    }

    /*
    * Checks if battlenet credentials are valid
    *
    * @param string $username
    * @param string $sha_pass_hash
    * @return bool
    */
    private function Validate_Battlenet_Login($email, $sha_pass_hash)
    {
        return (bool)$this->DB_QUERY()->Get_Battlenet_ID_Using_Email_and_ShaPassHash($this->DB_CONNECTION(), $email, $sha_pass_hash);
    }

    /*
    * Returns account info object
    *
    * @param string $username
    * @return object
    */
    public function Get_Account_Info($username)
    {
        $account = new \stdClass();

        $id                     = $this->Get_Account_ID_By_Username($username);
        $email                  = $this->Get_Email_By_Username($username);
        $gmlevel                = $this->Get_Account_Rank_By_ID($id);
        $rasar_key              = $this->LAUNCHER()->Get_Rasar_Key($id);
        $rasar_iv               = $this->LAUNCHER()->Get_Rasar_IV($id);

        $account->Authorized    = true;
        $account->IsActive      = $this->Account_is_Active($username);
        $account->Username      = $username;
        $account->Email         = $email;
        $account->RasarKey      = $rasar_key;
        $account->RasarIV       = $rasar_iv;
        $account->RankName      = isset($this->config['display_ranks']['names'][$gmlevel]) ? $this->config['display_ranks']['names'][$gmlevel] : "Undefined!";
        $account->RankColor     = isset($this->config['display_ranks']['colors'][$gmlevel]) ? $this->config['display_ranks']['colors'][$gmlevel] : "FFFFFF";
        $account->GMLevel       = $gmlevel;
        $account->Id            = $id;

        if ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA)
        {
            $account->VotePoints   = $this->Get_LegionCore_Vote_Points($id);
            $account->DonatePoints = $this->Get_LegionCore_Donate_Points($id);
        }
        else
        {
            $account->VotePoints   = $this->CMS()->Get_Vote_Points($id, $username);
            $account->DonatePoints = $this->CMS()->Get_Donate_Points($id, $username);
        }

        $account->BattlePayCredits = $this->database['auth']['battlepay_credits_as_dp'] ? $this->DB_QUERY()->Get_Battle_Pay_Credits_Using_Email($this->DB_CONNECTION(), $email) : 0;
        $account->AvatarUrl        = $this->LAUNCHER()->Get_User_Avatar_Url($id);
        $account->Nickname         = $this->LAUNCHER()->Get_User_Public_Nickname_Only_By_ID($id);
        $account->LastLogin        = $this->Get_Last_Login_Date_Timestamp_By_ID($id);
        $account->LastIP           = $this->Get_Last_Login_IP_By_ID($id);

        return $account;
    }

    /*
    * Returns account currencies
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Account_Currencies($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Authorized' => false,
            'Message' => Lang_Error["not_authorized"],
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username))
        {
            $username = $this->Get_Username_From_MD5Hash($MD5Username);
            $accountID = $this->Get_Account_ID_By_Username($username);

            $json->Authorized = true;

            if ($this->database['auth']['holds_vp_dp'] && $this->database['auth']['emulator_id'] == LEGIONCORE_735_SHA)
            {
                $json->VotePoints   = $this->Get_LegionCore_Vote_Points($accountID);
                $json->DonatePoints = $this->Get_LegionCore_Donate_Points($accountID);
            }
            else
            {
                $json->VotePoints   = $this->CMS()->Get_Vote_Points($accountID, $username);
                $json->DonatePoints = $this->CMS()->Get_Donate_Points($accountID, $username);
            }

            return json_encode($json, JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns account ban status
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Account_Ban_Status($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Authorized' => false,
            'Message' => Lang_Error["not_authorized"],
            'IsBanned' => false,
            'BanDuration' => "Unknown",
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username))
        {
            $username = $this->Get_Username_From_MD5Hash($MD5Username);
            $accountID = $this->Get_Account_ID_By_Username($username);

            $json->Authorized = true;

            if($this->Account_is_Banned($username))
            {
                $json->IsBanned = true;
                $json->BanDuration = $this->Get_Account_Ban_Duration_String($accountID);
            }

            return json_encode($json, JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns account active status
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Account_Active_Status($MD5token, $MD5Username)
    {
        $json = (object)
        [
            'Authorized' => false,
            'Message' => Lang_Error["not_authorized"],
            'IsActive' => false,
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if ($this->LAUNCHER()->Validate_Login_Token($MD5token, $MD5Username))
        {
            $username = $this->Get_Username_From_MD5Hash($MD5Username);

            $json->Authorized = true;
            $json->IsActive = $this->Account_is_Active($username);

            return json_encode($json, JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns json response for standard login challenge
    *
    * @param string $vendettaWho
    * @param string $vendettaToken
    * @return json
    */
    public function Standard_Login_Challenge(string $vendettaWho, string $vendettaToken, string $rasar_key, string $rasar_iv)
    {
        $json = (object)
        [
            'Logged'        => false,
            'Message'       => Lang_Error["not_authorized"],
            'Token'         => null,
            'AccountInfo'   => null,
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        $username = $this->Get_Username_From_MD5Hash($vendettaWho);

        if ($this->Account_is_Banned($username))
        {
            $json->Message = Lang_Error["account_is_banned"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        if (!$this->Account_Exists($username))
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_login_info"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        $accountID              = $this->Get_Account_ID_By_Username($username);
        $unhexToken             = $vendettaToken;
        $vendettaToken          = hex2bin($vendettaToken);

        switch($this->database['auth']['emulator_id'])
        {
            case TRINITYCORE_WOTLK:
            case AZEROTHCORE_WOTLK:
            case ASHMANE_SHADOWLANDS:
            case TRINITYCORE_DRAGONFLIGHTS:
            case ATLANTISS_CATA:
            {
                list($salt, $verifier)  = $this->Get_User_Salt_And_Verifier($username);
                if ($this->SRP6()->IsAVendetta($vendettaToken, $salt, $verifier))
                {
                    $json->Logged       = true;
                    $json->Message      = Lang_Success["login_ok"];
                    $json->Token        = $this->LAUNCHER()->Update_AccountData($accountID, $username, $this->Get_Email_By_Username($username));
                    $this->LAUNCHER()->Update_Rasar_Key($rasar_key, $accountID);
                    $this->LAUNCHER()->Update_Rasar_IV($rasar_iv, $accountID);
                    $json->AccountInfo  = $this->Get_Account_Info($username);
                }
                else
                {
                    $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
                    $json->Message = Lang_Error["invalid_login_info"];
                }
            }
            break;
            case CMANGOS_CLASSIC:
            case CMANGOS_TBC:
            case CMANGOS_WOTLK:
            case CMANGOS_CATA:
            case VMANGOS_CLASSIC:
            {
                list($salt, $verifier)  = $this->Get_User_Salt_And_Verifier($username);
                $cMaNGOS = new UserClient($username, $salt);
                if (strtoupper($verifier) === strtoupper($cMaNGOS->GetVendettaVerifier($vendettaToken)))
                {
                    $json->Logged       = true;
                    $json->Message      = Lang_Success["login_ok"];
                    $json->Token        = $this->LAUNCHER()->Update_AccountData($accountID, $username, $this->Get_Email_By_Username($username));
                    $this->LAUNCHER()->Update_Rasar_Key($rasar_key, $accountID);
                    $this->LAUNCHER()->Update_Rasar_IV($rasar_iv, $accountID);
                    $json->AccountInfo  = $this->Get_Account_Info($username);
                }
                else
                {
                    $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
                    $json->Message = Lang_Error["invalid_login_info"];
                }
            }
            break;
            case TRINITYCORE_CATA:
            case MANGOS_ONE:
            case MANGOS_TWO:
            case MANGOS_THREE:
            case MANGOS_FOUR:
            case MANGOS_FIVE:
            case SKYFIRE_MOP:
            case TRINITYCORE_WOTLK_2016:
            {
                if ($this->DB_QUERY()->Is_Valid_Sha_Pass_Hash($this->DB_CONNECTION(), $username, $unhexToken))
                {
                    $json->Logged       = true;
                    $json->Message      = Lang_Success["login_ok"];
                    $json->Token        = $this->LAUNCHER()->Update_AccountData($accountID, $username, $this->Get_Email_By_Username($username));
                    $this->LAUNCHER()->Update_Rasar_Key($rasar_key, $accountID);
                    $this->LAUNCHER()->Update_Rasar_IV($rasar_iv, $accountID);
                    $json->AccountInfo  = $this->Get_Account_Info($username);
                }
                else
                {
                    $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
                    $json->Message = Lang_Error["invalid_login_info"];
                }
            }
            break;
            case LEGIONCORE_735_SHA:
            {
                $sha256_pass_hash = strtoupper(bin2hex(strrev($vendettaToken)));
                if ($this->DB_QUERY()->Is_Valid_Sha_Pass_Hash($this->DB_CONNECTION(), $username, $sha256_pass_hash))
                {
                    $json->Logged       = true;
                    $json->Message      = Lang_Success["login_ok"];
                    $json->Token        = $this->LAUNCHER()->Update_AccountData($accountID, $username, $this->Get_Email_By_Username($username));
                    $this->LAUNCHER()->Update_Rasar_Key($rasar_key, $accountID);
                    $this->LAUNCHER()->Update_Rasar_IV($rasar_iv, $accountID);
                    $json->AccountInfo  = $this->Get_Account_Info($username);
                }
                else
                {
                    $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
                    $json->Message = Lang_Error["invalid_login_info"];
                }
            }
            break;
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns json response for battlenet login challenge by email and password
    *
    * @param string $battlenetWho
    * @param string $battlenetToken
    * @return json
    */
    function Battlenet_Login_Challenge(string $battlenetWho, string $battlenetToken)
    {
        $json = (object)
        [
            'Logged'        => false,
            'Message'       => Lang_Error["not_authorized"],
            'Token'         => null,
            'AccountInfo'   => null,
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_LOGIN))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        $email = $this->Get_Email_From_MD5Hash($battlenetWho);

        if (!$this->Battlenet_Account_Exists($email))
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_login_info"];
        }

        $username = $this->Get_Username_By_Email($email);

        if ($this->Account_is_Banned($username))
        {
            $json->Message = Lang_Error["account_is_banned"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        $accountID      = $this->Get_Account_ID_By_Username($username);
        $battlenetToken = strtoupper(bin2hex(strrev(hex2bin($battlenetToken))));

        switch($this->database['auth']['emulator_id'])
        {
            case TRINITYCORE_CATA:
            case TRINITYCORE_DRAGONFLIGHTS:
            case SKYFIRE_MOP:
            case ASHMANE_SHADOWLANDS:
            case LEGIONCORE_735_SHA:
            {
                if ($this->Validate_Battlenet_Login($email, $battlenetToken))
                {
                    if (!$this->Account_is_Banned($username))
                    {
                        $json->LoggedAs = $username;
                        $json->Logged = true;
                        $json->Message = 'Logged in with success.';
                        $json->Token = $this->LAUNCHER()->Update_AccountData($accountID, $username, $this->Get_Email_By_Username($username));
                        $json->AccountInfo  = $this->Get_Account_Info($username);
                    }
                    else
                    {
                        $json->Message = 'Login failed, account is banned!';
                    }
                }
                else
                {
                    $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
                    $json->Message = Lang_Error["invalid_login_info"];
                }
            }
            break;
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns json response for account registration
    *
    * @param string $username
    * @param string $email
    * @param string $password1
    * @param string $password2
    * @return json
    */
    function Create_Standard_Account(string $username, string $email, string $password1, string $password2)
    {
        $json = (object)
        [
            'Registered' => false,
            'Message'    => Lang_Error["registration_failed"],
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_REGISTER))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        $final_password = null;
        $register_successful = false;

        $username   = hex2bin($username);
        $email      = hex2bin($email);
        $password1  = hex2bin($password1);
        $password2  = hex2bin($password2);
        $s = null;
        $v = null;

        $final_password = $password2;

        if(!TOOLS::FilterNewAccount($this, $json, $username, $email, $password1, $password2))
        {
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        switch($this->database['auth']['emulator_id'])
        {
            case TRINITYCORE_WOTLK:
            case TRINITYCORE_DRAGONFLIGHTS:
            case AZEROTHCORE_WOTLK:
            case ASHMANE_SHADOWLANDS:
            case ATLANTISS_CATA:
            {
                list($s, $v) = $this->SRP6()->Get_Registration_Data($username, $final_password);

                if ($this->DB_QUERY()->Insert_V_S_Account($this->DB_CONNECTION(), $username, $s, $v, $email))
                {
                    $json->Registered = true;
                    $json->Message = Lang_Success["register_ok"];
                    $register_successful = true; // used by battlenet
                }
            }
            break;
            case CMANGOS_CLASSIC:
            case CMANGOS_TBC:
            case CMANGOS_WOTLK:
            case CMANGOS_CATA:
            case VMANGOS_CLASSIC:
            {
                $cMaNGOS = new UserClient($username);

                $s = $cMaNGOS->generateSalt();
                $v = $cMaNGOS->generateVerifier($final_password);

                if ($this->DB_QUERY()->Insert_V_S_Account($this->DB_CONNECTION(), $username, $s, $v, $email))
                {
                    $json->Registered = true;
                    $json->Message = Lang_Success["register_ok"];
                    $register_successful = true; // used by battlenet
                }
            }
            break;
            case MANGOS_ONE:
            case MANGOS_TWO:
            case MANGOS_THREE:
            case MANGOS_FOUR:
            case MANGOS_FIVE:
            case TRINITYCORE_CATA:
            case SKYFIRE_MOP:
            case TRINITYCORE_WOTLK_2016:
            {
                $sha1_pass_hash = sha1(strtoupper($username).':'.strtoupper($final_password));

                if ($this->DB_QUERY()->Insert_SPH_Account($this->DB_CONNECTION(), $username, $sha1_pass_hash, $email))
                {
                    $json->Registered = true;
                    $json->Message = Lang_Success["register_ok"];
                    $register_successful = true; // used by battlenet
                }
            }
            break;
            case LEGIONCORE_735_SHA:
            {
                $sha256_pass_hash = strtoupper(bin2hex(strrev(hex2bin(strtoupper(hash('sha256', strtoupper(hash('sha256', strtoupper($username)) . ':' . strtoupper($final_password)))))))) ;

                if ($this->DB_QUERY()->Insert_SPH_Account($this->DB_CONNECTION(), $username, $sha256_pass_hash, $email))
                {
                    $json->Registered = true;
                    $json->Message = Lang_Success["register_ok"];
                    $register_successful = true; // used by battlenet
                }
            }
            break;
        }

        // battlenet registration
        if ($register_successful)
        {
            $this->CMS()->Insert_CMS_Account_Data($this->Get_Account_ID_By_Username($username), $username, $email, $final_password);
            
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_REGISTER);

            if ($this->database['auth']['enable_battlenet'])
            {
                if ($this->Create_Battlenet_Account($email, $final_password, $s, $v))
                {
                    $json->Message = Lang_Success["register_ok_battlenet"];
                }
            }
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns true or false for battlenet account creation
    *
    * @param string $email
    * @param string $password
    * @return bool
    */
    private function Create_Battlenet_Account(string $email, string $plain_password, string $salt, string $verifier)
    {
        $sha256_pass_hash = strtoupper(bin2hex(strrev(hex2bin(strtoupper(hash("sha256",strtoupper(hash("sha256", strtoupper($email)).":".strtoupper($plain_password))))))));

        if (!$this->Battlenet_Account_Exists($email))
        {
            switch($this->database['auth']['emulator_id'])
            {
                case TRINITYCORE_DRAGONFLIGHTS:
                case LEGIONCORE_735_SHA:
                {
                    $battlenet_registered = $this->DB_QUERY()->Insert_S_V_Battlenet_Account($this->DB_CONNECTION(), $email, $salt, $verifier);
                    $battlenet_linked = $this->DB_QUERY()->Link_Battlenet_Account($this->DB_CONNECTION(), $email);
                    return $battlenet_linked;
                }
                break;
                case TRINITYCORE_CATA:
                case ASHMANE_SHADOWLANDS:
                {
                    $battlenet_registered = $this->DB_QUERY()->Insert_SHA_Battlenet_Account($this->DB_CONNECTION(), $email, $sha256_pass_hash);
                    $battlenet_linked = $this->DB_QUERY()->Link_Battlenet_Account($this->DB_CONNECTION(), $email);
                    return $battlenet_linked;
                }
                break;
            }
        }

        return false;
    }

    /*
    * Returns json response for password change
    *
    * @param string $username
    * @param string $password1
    * @param string $password2
    * @return json
    */
    public function Change_Password(string $username, string $password1, string $password2)
    {
        $json = (object)
        [
            'Changed' => false,
            'Message' => Lang_Error["password_change_failed"],
        ];

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_PASSWORD_RESET))
        {
            $json->Message = Lang_Error["attempts_cooldown"];
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        $final_password     = null;
        $change_successful  = false;
        $email              = $this->Get_Email_By_Username($username);
        $final_password     = $password2;

        if (!TOOLS::FilterNewPassword($this, $json, $username, $password1, $password2))
        {
            return json_encode($json, JSON_PRETTY_PRINT);
        }

        switch($this->database['auth']['emulator_id'])
        {
            case TRINITYCORE_WOTLK:
            case TRINITYCORE_DRAGONFLIGHTS:
            case ASHMANE_SHADOWLANDS:
            case ATLANTISS_CATA:
            {
                list($s, $v) = $this->SRP6()->Get_Registration_Data($username, $final_password);

                if ($this->DB_QUERY()->Insert_New_V_S_Password($this->DB_CONNECTION(), $s, $v, $username))
                {
                    $json->Changed = true;
                    $json->Message = Lang_Success["password_changed"];
                    $change_successful = true; // used by battlenet
                }
            }
            break;
            case CMANGOS_CLASSIC:
            case CMANGOS_TBC:
            case CMANGOS_WOTLK:
            case CMANGOS_CATA:
            case VMANGOS_CLASSIC:
            {
                $cMaNGOS = new UserClient($username);

                $s = $cMaNGOS->generateSalt();
                $v = $cMaNGOS->generateVerifier($final_password);

                if ($this->DB_QUERY()->Insert_New_V_S_Password($this->DB_CONNECTION(), $s, $v, $username))
                {
                    $json->Changed = true;
                    $json->Message = Lang_Success["password_changed"];
                    $change_successful = true; // used by battlenet
                }
            }
            break;
            case TRINITYCORE_CATA:
            case MANGOS_ONE:
            case MANGOS_TWO:
            case MANGOS_THREE:
            case MANGOS_FOUR:
            case MANGOS_FIVE:
            case SKYFIRE_MOP:
            case LEGIONCORE_735_SHA:
            case TRINITYCORE_WOTLK_2016:
            {
                $sha256_pass_hash = strtoupper(bin2hex(strrev(hex2bin(strtoupper(hash('sha256', strtoupper(hash('sha256', strtoupper($username)) . ':' . strtoupper($final_password)))))))) ;
                
                if ($this->DB_QUERY()->Insert_New_SPH_Password($this->DB_CONNECTION(), $sha256_pass_hash, $username))
                {
                    $json->Changed = true;
                    $json->Message = Lang_Success["password_changed"];
                    $change_successful = true; // used by battlenet
                }
            }
            break;
        }

        // battlenet password change
        if ($change_successful)
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_PASSWORD_RESET);

            if ($this->database['auth']['enable_battlenet'])
            {
                if ($this->Change_Battlenet_Password($email, $final_password))
                {
                    $json->Message = Lang_Success["password_changed_battlenet"];
                }
            }
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns true or false for battlenet password change
    *
    * @param string $email
    * @param string $password
    * @return bool
    */
    private function Change_Battlenet_Password(string $email, string $password)
    {
        $sha256_pass_hash = strtoupper(bin2hex(strrev(hex2bin(strtoupper(hash("sha256",strtoupper(hash("sha256", strtoupper($email)).":".strtoupper($password))))))));

        if ($this->Battlenet_Account_Exists($email))
        {
            switch($this->database['auth']['emulator_id'])
            {
                case TRINITYCORE_CATA:
                case TRINITYCORE_DRAGONFLIGHTS:
                case ASHMANE_SHADOWLANDS:
                {
                    $battlenet_registered = $this->DB_QUERY()->Insert_New_Battlenet_Password($this->DB_CONNECTION(), $sha256_pass_hash, $email);
                    return $battlenet_registered;
                }
                break;
            }
        }

        return false;
    }
}
