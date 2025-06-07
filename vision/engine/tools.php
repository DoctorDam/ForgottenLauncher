<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class TOOLS
{
    public static function FilterNewAccount(\vISION\AUTH $AUTH, &$json, string $username, string $email, string $password1, string $password2)
    {
        if (empty($username))
        {
            $json->Message = Lang_Error["username_empty"];
            return false;
        }
        elseif (empty($email))
        {
            $json->Message = Lang_Error["email_empty"];
            return false;
        }
        elseif (empty($password1) || empty($password2))
        {
            $json->Message = Lang_Error["password_empty"];
            return false;
        }
        elseif (!filter_var($email, FILTER_VALIDATE_EMAIL))
        {
            $json->Message = Lang_Error["invalid_email_format"];
            return false;
        }
        elseif (strlen($username) < 6)
        {
            $json->Message = Lang_Error["username_min_length"];
            return false;
        }
        elseif (strlen($password1) < 6 || strlen($password2) < 6)
        {
            $json->Message = Lang_Error["password_min_length"];
            return false;
        }
        elseif ($password1 != $password2)
        {
            $json->Message = Lang_Error["passwords_no_match"];
            return false;
        }
        elseif ($AUTH->Account_Exists($username))
        {
            $json->Message = Lang_Error["username_taken"];
            return false;
        }
        elseif ($AUTH->Email_Exists($email))
        {
            $json->Message = Lang_Error["email_taken"];
            return false;
        }

        return true;
    }

    public static function FilterNewPassword(\vISION\AUTH $AUTH, &$json, string $username, string $password1, string $password2)
    {
        if (empty($username))
        {
            $json->Message = Lang_Error["username_empty"];
            return false;
        }
        elseif (empty($password1) || empty($password2))
        {
            $json->Message = Lang_Error["password_empty"];
            return false;
        }
        elseif (strlen($password1) < 6 || strlen($password2) < 6)
        {
            $json->Message = Lang_Error["password_min_length"];
            return false;
        }
        elseif ($password1 != $password2)
        {
            $json->Message = Lang_Error["passwords_no_match"];
            return false;
        }
        elseif (!$AUTH->Account_Exists($username))
        {
            $json->Message = Lang_Error["username_no_exist"];
            return false;
        }

        return true;
    }
}
