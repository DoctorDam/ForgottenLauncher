<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class CMS
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

    public function TEST_DB_CONNECTION()
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
        
            if (mysqli_connect_errno()) 
            {
                if ($connection) 
                {
                    mysqli_close($connection);
                }
                return "CMS DB CONNECTION FAILED..";
            }
            else
            {
                mysqli_close($connection);
                return "CMS DB CONNECTION OK";
            }
        }
        catch (\Exception $e)
        {
            return "CMS DB ERROR: ".$e->getMessage();
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

    private function DB_QUERY(int $realmid = 0)
    {
        return new QUERIES($this->database, $realmid);
    }

    /*
    * Inserts account information in cms database
    *
    * @param int $accountID
    * @param string $username
    * @param string $email
    * @param string $pass
    * @return bool
    */
    public function Insert_CMS_Account_Data(int $accountID, string $username, string $email, string $pass)
    {
        return $this->DB_QUERY()->Insert_Website_Account_Data($this->DB_CONNECTION(), $accountID, $username, $email, $pass);
    }

    /*
    * Returns articles as json
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Articles_List($MD5token, $MD5Username)
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
            return json_encode($this->DB_QUERY()->Get_CMS_Articles($this->DB_CONNECTION(), $this->config['website_url']), JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns changelogs as json
    *
    * @param string $MD5token
    * @param string $MD5Username
    * @return json
    */
    public function Get_Changelog_List($MD5token, $MD5Username)
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
            return json_encode($this->DB_QUERY()->Get_CMS_Changelog($this->DB_CONNECTION()), JSON_PRETTY_PRINT);
        }
        else
        {
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_LOGIN);
            $json->Message = Lang_Error["invalid_token"];
        }

        return json_encode($json, JSON_PRETTY_PRINT);
    }

    /*
    * Returns account's current vote points amount
    *
    * @param int $accountID
    * @param string $orUsername
    * @return int
    */
    public function Get_Vote_Points(int $accountID, string $orUsername)
    {
        return $this->DB_QUERY()->Get_Vote_Points_Using_AccountID_Or_Username($this->DB_CONNECTION(), $accountID, $orUsername);
    }

    /*
    * Returns account's current donate points amount
    *
    * @param int $accountID
    * @param string $orUsername
    * @return int
    */
    public function Get_Donate_Points(int $accountID, string $orUsername)
    {
        return $this->DB_QUERY()->Get_Donate_Points_Using_AccountID_Or_Username($this->DB_CONNECTION(), $accountID, $orUsername);
    }

    /*
    * Removes amount of vote points from account
    *
    * @param int $accountID
    * @param int $amount
    * @param string $orUsername
    *
    */
    public function Remove_Vote_Points(int $accountID, int $amount, string $orUsername)
    {
        return $this->DB_QUERY()->Remove_Vote_Points_Using_AccountID_Or_Username($this->DB_CONNECTION(), $accountID, $amount, $orUsername);
    }

    /*
    * Removes amount of donate points from account
    *
    * @param int $accountID
    * @param int $amount
    * @param string $orUsername
    *
    */
    public function Remove_Donate_Points(int $accountID, int $amount, string $orUsername)
    {
        return $this->DB_QUERY()->Remove_Donate_Points_Using_AccountID_Or_Username($this->DB_CONNECTION(), $accountID, $amount, $orUsername);
    }

    /*
    * Adds amount of vote points to account
    *
    * @param int $accountID
    * @param int $amount
    * @param string $orUsername
    *
    */
    public function Add_Vote_Points(int $accountID, int $amount, string $orUsername)
    {
        return $this->DB_QUERY()->Add_Vote_Points_Using_AccountID_Or_Username($this->DB_CONNECTION(), $accountID, $amount, $orUsername);
    }

    /*
    * Adds amount of donation points to account
    *
    * @param int $accountID
    * @param int $amount
    * @param string $orUsername
    *
    */
    public function Add_Donate_Points(int $accountID, int $amount, string $orUsername)
    {
        return $this->DB_QUERY()->Add_Donate_Points_Using_AccountID_Or_Username($this->DB_CONNECTION(), $accountID, $amount, $orUsername);
    }
}