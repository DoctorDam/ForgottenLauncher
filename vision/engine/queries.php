<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

require_once('sql_queries.php');

class QUERIES
{
    protected $database;
    protected $realmid;

    public function __construct($database, $realmid)
    {
        $this->database = $database;
        $this->realmid = $realmid;
    }

    public function PrepareQuery($connection, string $name, bool $cms = false)
    {
        try
        {
            $id = $cms ? $this->database['cms']['cms_id'] : $this->database['auth']['emulator_id'];
            return $connection->prepare(SQL_QUERIES::GET($name, $id));
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return null;
    }

    public function Is_Valid_Sha_Pass_Hash($auth_db_connection, string $username, string $sha_pass_hash): bool
    {
        $valid = false;

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_VERIFY_SHA_PASS_HASH');
            $stmt->bind_param('ss', $username, $sha_pass_hash);
            $stmt->execute();
            $stmt->bind_result($valid);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $valid;
    }

    public function Get_Salt_Verifier_Using_Username($auth_db_connection, string $username): array
    {
        $salt = "";
        $verifier = "";

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_SALT_VERIFIER_BY_USERNAME');
            $stmt->bind_param('s', $username);
            $stmt->execute();
            $stmt->bind_result($salt, $verifier);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return [$salt, $verifier];
    }

    public function Get_Account_ID_Using_Username($auth_db_connection, string $username): int
    {
        $id = 0;

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_ACCOUNT_ID_BY_USERNAME');
            $stmt->bind_param('s', $username);
            $stmt->execute();
            $stmt->bind_result($id);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $id;
    }

    public function Get_Account_Is_Active($auth_db_connection, string $username): bool
    {
        $active = false;

        try 
        {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_ACCOUNT_IS_ACTIVE_BY_USERNAME');
            $stmt->bind_param('s', $username);
            $stmt->execute();
            $stmt->bind_result($active);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } 
        catch (\Exception $e) 
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $active;
    }

    public function Get_Account_ID_Using_Email($auth_db_connection, string $email): int
    {
        $id = 0;

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_ACCOUNT_ID_BY_EMAIL');
            $stmt->bind_param('s', $email);
            $stmt->execute();
            $stmt->bind_result($id);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $id;
    }

    public function Get_Username_Using_Account_ID($auth_db_connection, int $id): string
    {
        $username = "";

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_ACCOUNT_NAME_BY_ID');
            $stmt->bind_param('i', $id);
            $stmt->execute();
            $stmt->bind_result($username);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $username;
    }

    public function Get_Username_Using_Email($auth_db_connection, string $email): string
    {
        $username = "";

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_ACCOUNT_NAME_BY_EMAIL');
            $stmt->bind_param('s', $email);
            $stmt->execute();
            $stmt->bind_result($username);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $username;
    }

    public function Get_Last_Login_Timestamp_Using_Account_ID($auth_db_connection, int $id): string
    {
        $last_login = "";

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_ACCOUNT_LAST_LOGIN_BY_ID');
            $stmt->bind_param('i', $id);
            $stmt->execute();
            $stmt->bind_result($last_login);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $last_login === null || $last_login === "" ? "Unknown" : $last_login;
    }

    public function Get_Last_Login_IP_Using_Account_ID($auth_db_connection, int $id): string
    {
        $last_login_ip = "";

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_ACCOUNT_LAST_LOGIN_IP_BY_ID');
            $stmt->bind_param('i', $id);
            $stmt->execute();
            $stmt->bind_result($last_login_ip);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $last_login_ip;
    }

    public function Get_Account_Rank_Using_Account_ID($auth_db_connection, int $id): int
    {
        $rank = 0;

        try {
            $result = null;
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_ACCOUNT_RANK_BY_ID');
            $stmt->bind_param('i', $id);
            $stmt->execute();
            $stmt->bind_result($result);
            $stmt->fetch();
            $stmt->close();
            $rank = ($result !== null) ? $result : 0;
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $rank;
    }

    public function Get_Email_Using_MD5_Username($auth_db_connection, string $md5username): string
    {
        $email = "";

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_EMAIL_USING_MD5_USERNAME');
            $stmt->bind_param('s', $md5username);
            $stmt->execute();
            $stmt->bind_result($email);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $email;
    }

    public function Get_Username_Using_MD5_Username($auth_db_connection, string $md5username): string
    {
        $username = "";

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_USERNAME_BY_MD5HASH');
            $stmt->bind_param('s', $md5username);
            $stmt->execute();
            $stmt->bind_result($username);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $username;
    }

    public function Get_Email_Using_MD5_Email($auth_db_connection, string $md5email): string
    {
        $email = "";

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_EMAIL_BY_MD5HASH');
            $stmt->bind_param('s', $md5email);
            $stmt->execute();
            $stmt->bind_result($email);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $email;
    }

    public function Get_Email_Using_Username($auth_db_connection, string $username): string
    {
        $email = "";

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_EMAIL_BY_USERNAME');
            $stmt->bind_param('s', $username);
            $stmt->execute();
            $stmt->bind_result($email);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $email === null || $email === "" ? "?" : $email;
    }

    public function Get_Active_Banned_Using_ID($auth_db_connection, int $id): int
    {
        $active = 0;

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_BANNED_ACTIVE_BY_ID');
            $stmt->bind_param('i', $id);
            $stmt->execute();
            $stmt->bind_result($active);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $active;
    }

    public function Get_Account_Ban_Duration_String($auth_db_connection, int $id): string
    {
        $duration = "unknown";

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_ACCOUNT_BAN_DURATION_STRING');
            $stmt->bind_param('i', $id);
            $stmt->execute();
            $stmt->bind_result($duration);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $duration;
    }

    public function Get_Battle_Pay_Credits_Using_Email($auth_db_connection, string $email): int
    {
        $credits = 0;

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_BATTLEPAYCREDITS_BY_EMAIL');
            $stmt->bind_param('s', $email);
            $stmt->execute();
            $stmt->bind_result($credits);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $credits;
    }

    public function Get_Battlenet_ID_Using_Email($auth_db_connection, string $email): int
    {
        $id = 0;

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_BATTLENET_ID_BY_EMAIL');
            $stmt->bind_param('s', $email);
            $stmt->execute();
            $stmt->bind_result($id);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $id;
    }

    public function Get_Battlenet_ID_Using_Email_and_ShaPassHash($auth_db_connection, string $email, string $sha_pass_hash): int
    {
        $id = 0;

        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_GET_BATTLENET_ID_BY_EMAIL_AND_SHA_PASS_HASH');
            $stmt->bind_param('ss', $email, $sha_pass_hash);
            $stmt->execute();
            $stmt->bind_result($id);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $id;
    }

    public function Get_Characters_List_Using_Account_Id($char_db_connection, int $accountID): array
    {
        $characters_list = [];

        try {
            $guid       = 0;
            $name       = null;
            $race       = 0;
            $class      = 0;
            $gender     = 0;
            $level      = 0;
            $realmid    = (int)$this->realmid;
            $realmname  = $this->database['realms'][$this->realmid]['realm_name'];

            $stmt = $this->PrepareQuery($char_db_connection, 'CHAR_GET_CHARACTERS_LIST_BY_ACCOUNT_ID');
            $stmt->bind_param('i', $accountID);
            $stmt->execute();
            $stmt->bind_result($guid, $name, $race, $class, $gender, $level);

            while ($stmt->fetch()) {
                $characters_list[] =
                    [
                        'guid'      => $guid,
                        'name'      => $name,
                        'race'      => $race,
                        'class'     => $class,
                        'gender'    => $gender,
                        'level'     => $level,
                        'realmid'   => $realmid,
                        'realmname' => $realmname,
                    ];
            }

            $stmt->close();
            $char_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $characters_list;
    }

    public function Get_Username_Using_Auth_Token_and_Username($launcher_db_connection, string $MD5token, string $MD5username)
    {
        $username = null;

        try {
            $stmt = $launcher_db_connection->prepare("SELECT account FROM account_data WHERE ? = md5(access_token) AND ? = md5(LOWER(account)) AND UNIX_TIMESTAMP(NOW()) < token_valid_until");
            $stmt->bind_param('ss', $MD5token, $MD5username);
            $stmt->execute();
            $stmt->bind_result($username);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $username;
    }

    public function Character_Market_Sale_Exists($launcher_db_connection, string $character_guid, string $realm_id)
    {
        $exists = false;

        try {
            $stmt = $launcher_db_connection->prepare("SELECT id FROM characters_market WHERE character_guid = ? AND realm_id = ?");
            $stmt->bind_param('ii', $character_guid, $realm_id);
            $stmt->execute();
            $stmt->bind_result($exists);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $exists;
    }

    public function Get_Password_Recovery_Code_Using_Username_IP_Address($launcher_db_connection, string $username, string $ip_address): string
    {
        $code = '';

        try {
            $stmt = $launcher_db_connection->prepare("SELECT `reset_code` FROM `password_recovery` WHERE `username` = ? AND `ip_address` = ?");
            $stmt->bind_param('ss', $username, $ip_address);
            $stmt->execute();
            $stmt->bind_result($code);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $code;
    }

    public function Generate_Password_Recovery_Code($launcher_db_connection, string $username, string $recovery_code, string $valid_until): bool
    {
        try {
            $guest_ip = isset($_SERVER["HTTP_CF_CONNECTING_IP"]) ? $_SERVER["HTTP_CF_CONNECTING_IP"] : $_SERVER["REMOTE_ADDR"];
            $stmt = $launcher_db_connection->prepare("REPLACE INTO `password_recovery` (`username`, `reset_code`, `ip_address`, `valid_until`) VALUES (?, ?, ?, ?)");
            $stmt->bind_param('sssi', $username, $recovery_code, $guest_ip, $valid_until);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Get_Launcher_Slider_Image_Urls($launcher_db_connection): array
    {
        $images = [];

        try {
            $id             = 0;
            $image_url      = null;
            $title          = null;
            $description    = null;
            $redirect_url   = null;

            $stmt = $launcher_db_connection->prepare("SELECT id, image_url, title, description, redirect_url FROM slider ORDER BY id ASC");
            $stmt->execute();
            $stmt->bind_result($id, $image_url, $title, $description, $redirect_url);

            while ($stmt->fetch()) {
                $images[] =
                    [
                        'id'            => $id,
                        'image_url'     => $image_url,
                        'title'         => $title,
                        'description'   => $description,
                        'redirect_url'  => $redirect_url
                    ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $images;
    }

    // public function Get_Launcher_Articles($launcher_db_connection, int $position): array
    // {
    //     $articles = [];

    //     try {
    //         $id             = 0;
    //         $author         = null;
    //         $picture_url    = null;
    //         $redirect_url   = null;
    //         $title          = null;
    //         $content        = null;
    //         $date           = null;

    //         $stmt = $launcher_db_connection->prepare("SELECT a.id, (SELECT `account` FROM `account_data` WHERE id = a.author_id) AS author, a.picture_url, a.redirect_url, a.title, a.content, 
    //             DATE_FORMAT(a.date, '%e %M %Y') AS `date` FROM articles a WHERE a.POSITION = ? ORDER BY id DESC");
    //         $stmt->bind_param('i', $position);
    //         $stmt->execute();
    //         $stmt->bind_result($id, $author, $picture_url, $redirect_url, $title, $content, $date);

    //         while ($stmt->fetch()) {
    //             $articles[] =
    //                 [
    //                     'id'            => $id,
    //                     'author'        => $author,
    //                     'picture_url'   => $picture_url,
    //                     'redirect_url'  => $redirect_url,
    //                     'title'         => $title,
    //                     'content'       => $content,
    //                     'date'          => $date,
    //                 ];
    //         }

    //         $stmt->close();
    //         $launcher_db_connection->close();
    //     } catch (\Exception $e) {
    //         (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
    //     }

    //     return $articles;
    // }

    public function Get_Launcher_Events($launcher_db_connection): array
    {
        $events = [];

        try {
            $id             = 0;
            $picture_url    = null;
            $redirect_url   = null;
            $title          = null;
            $content        = null;
            $expiry_date    = null;

            $stmt = $launcher_db_connection->prepare("SELECT id, picture_url, redirect_url, title, content, DATE_FORMAT(`expiry_date`, '%e %M %Y') AS `expiry_date` FROM `events` WHERE `expiry_date` > NOW() ORDER BY id DESC");
            $stmt->execute();
            $stmt->bind_result($id, $picture_url, $redirect_url, $title, $content, $expiry_date);

            while ($stmt->fetch()) {
                $events[] =
                    [
                        'id'            => $id,
                        'picture_url'   => $picture_url,
                        'redirect_url'  => $redirect_url,
                        'title'         => $title,
                        'content'       => $content,
                        'expiry_date'   => $expiry_date,
                    ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $events;
    }

    public function Get_Launcher_Private_Messages_Using_AccountID($launcher_db_connection, int $account_id): array
    {
        $messages = [];

        try {
            $id                     = 0;
            $parent_id              = 0;
            $sender_id              = 0;
            $sender_nickname        = null;
            $sender_avatar_url      = null;
            $receiver_id            = 0;
            $receiver_nickname      = null;
            $receiver_avatar_url    = null;
            $title                  = null;
            $message                = null;
            $date_edited            = null;
            $seen                   = false;
            $date_seen              = null;

            $stmt = $launcher_db_connection->prepare("SELECT m.id, m.parent_id, 
            m.sender_id, ad_sender.public_nickname AS sender_nickname, av_sender.image_url AS sender_avatar_url,
            m.receiver_id, ad_receiver.public_nickname AS receiver_nickname, av_receiver.image_url AS receiver_avatar_url,
            m.title, m.message, m.date_edited, m.seen, m.date_seen
            FROM messages m
                JOIN account_data ad_sender ON m.sender_id = ad_sender.id
                JOIN account_data ad_receiver ON m.receiver_id = ad_receiver.id
                LEFT JOIN account_avatars av_sender ON m.sender_id = av_sender.account_id
                LEFT JOIN account_avatars av_receiver ON m.receiver_id = av_receiver.account_id
                    WHERE ? IN (m.sender_id, m.receiver_id) AND m.parent_id = 0 
                            AND NOT EXISTS (
                                SELECT 1
                                FROM messages_deleted md
                                WHERE md.message_id = m.id AND md.for_user_id = ?
                            )
                            ORDER BY m.date_edited DESC
            ");

            $stmt->bind_param('ii', $account_id, $account_id);
            $stmt->execute();
            $stmt->bind_result($id, $parent_id, $sender_id, $sender_nickname, $sender_avatar_url, $receiver_id, $receiver_nickname, $receiver_avatar_url, $title, $message, $date_edited, $seen, $date_seen);

            while ($stmt->fetch()) {
                $messages[] =
                    [
                        'id'                    => $id,
                        'parent_id'             => $parent_id,
                        'sender_id'             => $sender_id,
                        'sender_nickname'       => $sender_nickname,
                        'sender_avatar_url'     => $sender_avatar_url,
                        'receiver_id'           => $receiver_id,
                        'receiver_nickname'     => $receiver_nickname,
                        'receiver_avatar_url'   => $receiver_avatar_url,
                        'title'                 => $title,
                        'message'               => $message,
                        'date_edited'           => $date_edited,
                        'seen'                  => $seen,
                        'date_seen'             => $date_seen,
                    ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $messages;
    }

    public function Get_Launcher_Private_Message_Thread($launcher_db_connection, int $message_id): array
    {
        $messages = [];

        try {
            $id                     = 0;
            $parent_id              = 0;
            $sender_id              = 0;
            $sender_nickname        = null;
            $sender_avatar_url      = null;
            $receiver_id            = 0;
            $receiver_nickname      = null;
            $receiver_avatar_url    = null;
            $title                  = null;
            $message                = null;
            $date_edited            = null;
            $seen                   = false;
            $date_seen              = null;

            $stmt = $launcher_db_connection->prepare("SELECT m.id, m.parent_id, 
                    m.sender_id, ad_sender.public_nickname AS sender_nickname, av_sender.image_url AS sender_avatar_url,
                    m.receiver_id, ad_receiver.public_nickname AS receiver_nickname, av_receiver.image_url AS receiver_avatar_url,
                    m.title, m.message, m.date_edited, m.seen, m.date_seen 
                FROM messages m
                JOIN account_data ad_sender ON m.sender_id = ad_sender.id
                    LEFT JOIN account_data ad_receiver ON m.receiver_id = ad_receiver.id
                    LEFT JOIN account_avatars av_sender ON m.sender_id = av_sender.account_id
                    LEFT JOIN account_avatars av_receiver ON m.receiver_id = av_receiver.account_id
                WHERE m.id = ? OR m.parent_id = ? ORDER BY m.id ASC
            ");

            $stmt->bind_param('ii', $message_id, $message_id);
            $stmt->execute();
            $stmt->bind_result($id, $parent_id, $sender_id, $sender_nickname, $sender_avatar_url, $receiver_id, $receiver_nickname, $receiver_avatar_url, $title, $message, $date_edited, $seen, $date_seen);

            while ($stmt->fetch()) {
                $messages[] =
                    [
                        'id'                    => $id,
                        'parent_id'             => $parent_id,
                        'sender_id'             => $sender_id,
                        'sender_nickname'       => $sender_nickname,
                        'sender_avatar_url'     => $sender_avatar_url,
                        'receiver_id'           => $receiver_id,
                        'receiver_nickname'     => $receiver_nickname,
                        'receiver_avatar_url'   => $receiver_avatar_url,
                        'title'                 => $title,
                        'message'               => $message,
                        'date_edited'           => $date_edited,
                        'seen'                  => $seen,
                        'date_seen'             => $date_seen,
                    ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $messages;
    }

    public function Message_Receiver_Exists($launcher_db_connection, string $receiver): bool
    {
        $receiver_exists = false;

        try {
            $stmt = $launcher_db_connection->prepare("SELECT 1 FROM account_data WHERE public_nickname = ?");
            $stmt->bind_param('s', $receiver);
            $stmt->execute();
            $stmt->bind_result($receiver_exists);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $receiver_exists;
    }

    public function Is_In_Message_Party($launcher_db_connection, int $message_id, int $account_id): bool
    {
        $receiver_exists = false;

        try {
            $stmt = $launcher_db_connection->prepare("SELECT 1 FROM messages WHERE id = ? AND (sender_id = ? OR receiver_id = ?)");
            $stmt->bind_param('iii', $message_id, $account_id, $account_id);
            $stmt->execute();
            $stmt->bind_result($receiver_exists);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $receiver_exists;
    }

    public function Insert_New_Private_Message($launcher_db_connection, int $sender_id, string $receiver_nickname, string $title, string $message): bool
    {
        try {
            $stmt = $launcher_db_connection->prepare("INSERT INTO `messages` (`sender_id`, `receiver_id`, `title`, `message`, `date_edited`) 
                    VALUES (?, (SELECT id FROM account_data WHERE public_nickname = ?), ?, ?, NOW())");
            $stmt->bind_param('isss', $sender_id, $receiver_nickname, $title, $message);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Insert_New_Private_Message_Reply($launcher_db_connection, int $message_id, int $sender_id, string $message): bool
    {
        try {
            $stmt = $launcher_db_connection->prepare("INSERT INTO `messages` (`parent_id`, `sender_id`, `message`, `date_edited`) VALUES (?, ?, ?, NOW())");
            $stmt->bind_param('iis', $message_id, $sender_id, $message);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Delete_Private_Message_Using_Account_ID($launcher_db_connection, int $message_id, int $account_id): bool
    {
        try {
            $stmt = $launcher_db_connection->prepare("INSERT INTO `messages_deleted` (`message_id`, `for_user_id`) VALUES (?, ?)");
            $stmt->bind_param('ii', $message_id, $account_id);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Account_Data_Exists_By_ID($launcher_db_connection, int $account_id): bool
    {
        $exists = false;

        try {
            $stmt = $launcher_db_connection->prepare("SELECT 1 FROM account_data WHERE id = ?");
            $stmt->bind_param('i', $account_id);
            $stmt->execute();
            $stmt->bind_result($exists);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $exists;
    }

    public function Insert_New_Account_Data($launcher_db_connection, int $account_id, string $username, string $email, string $last_ip, string $access_token, int $token_time): bool
    {
        try {
            $stmt = $launcher_db_connection->prepare("INSERT INTO `account_data` (`id`, `account`, `email`, `last_ip_address`, `access_token`, `token_valid_until`, `public_nickname`) 
                VALUES (?, ?, ?, ?, ?, ?, 'not set..')");
            $stmt->bind_param('issssi', $account_id, $username, $email, $last_ip, $access_token, $token_time);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Update_Account_Data_Access_Token($launcher_db_connection, int $account_id, string $username, string $ip, string $access_token, int $token_time): bool
    {
        try {
            $stmt = $launcher_db_connection->prepare("UPDATE `account_data` SET `account` = ?, `access_token` = ?, `last_ip_address` = ?, `token_valid_until` = ? WHERE id = ?");
            $stmt->bind_param('sssii', $username, $access_token, $ip, $token_time, $account_id);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Get_Launcher_Login_Rewards($launcher_db_connection, int $accountID): array
    {
        try {
            $target_month       = date('n');
            $login_rewards      = [];

            $day                = 0;
            $month              = 0;
            $reward_id          = 0;
            $title              = null;
            $picture_url        = null;
            $description        = null;
            $requires_player    = false;
            $requires_input     = false;
            $claimed            = false;

            $stmt = $launcher_db_connection->prepare("SELECT lr.`day`, lr.`month`, lr.`reward_id`, r.`title`, r.`picture_url`, r.`description`, r.`requires_player`, r.`requires_input`,
                COALESCE((SELECT 1 FROM login_claimed_rewards WHERE account_id = ? AND `month` = lr.`month` AND `day` = lr.`day`), 0) AS claimed_reward
                FROM `login_rewards` lr
                JOIN `rewards` r ON lr.`reward_id` = r.`id` WHERE lr.`month` = ?
            ");

            $stmt->bind_param('ii', $accountID, $target_month);
            $stmt->execute();
            $stmt->bind_result($day, $month, $reward_id, $title, $picture_url, $description, $requires_player, $requires_input, $claimed);

            while ($stmt->fetch()) {
                $login_rewards[] =
                    [
                        'day'               => $day,
                        'month'             => $month,
                        'reward_id'         => $reward_id,
                        'title'             => $title,
                        'picture_url'       => $picture_url,
                        'description'       => $description,
                        'requires_player'   => $requires_player,
                        'requires_input'    => $requires_input,
                        'claimed'           => $claimed,
                    ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $login_rewards;
    }

    public function Get_Launcher_Account_Inventory($launcher_db_connection, int $accountID): array
    {
        $user_inventory = [];

        try {
            $reward_id          = 0;
            $acquired_on        = null;
            $picture_url        = null;
            $title              = null;
            $description        = null;
            $requires_player    = false;
            $requires_input     = false;

            $stmt = $launcher_db_connection->prepare("SELECT ai.reward_id, DATE_FORMAT(ai.acquired_on, '%e %M %Y at %H:%i') AS acquired_on, r.picture_url, r.title, r.description, r.requires_player, r.requires_input
                                          FROM account_inventory ai
                                          JOIN rewards r ON ai.reward_id = r.id WHERE ai.account_id = ?");

            $stmt->bind_param('i', $accountID);
            $stmt->execute();
            $stmt->bind_result($reward_id, $acquired_on, $picture_url, $title, $description, $requires_player, $requires_input);

            while ($stmt->fetch()) {
                $user_inventory[] =
                    [
                        'reward_id'         => $reward_id,
                        'acquired_on'       => $acquired_on,
                        'picture_url'       => $picture_url,
                        'title'             => $title,
                        'description'       => $description,
                        'requires_player'   => $requires_player,
                        'requires_input'    => $requires_input,
                    ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $user_inventory;
    }

    public function Claimed_Login_Reward($launcher_db_connection, int $accountID, int $month, int $day): int
    {
        $reward_id = 0;

        try {
            $stmt = $launcher_db_connection->prepare("SELECT 1 FROM login_claimed_rewards WHERE account_id = ? AND `month` = ? AND `day` = ?");
            $stmt->bind_param('iii', $accountID, $month, $day);
            $stmt->execute();
            $stmt->bind_result($reward_id);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();

            return $reward_id;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $reward_id;
    }

    public function Redeemed_Gift_Code($launcher_db_connection, int $accountID, int $gift_id): bool
    {
        $redeemed = false;

        try{
            $stmt = $launcher_db_connection->prepare("SELECT 1 FROM gift_redeems WHERE user_id = ? AND `gift_id` = ?");
            $stmt->bind_param('ii', $accountID, $gift_id);
            $stmt->execute();
            $stmt->bind_result($redeemed);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $redeemed;
    }

    public function Gift_Code_Expired($launcher_db_connection, string $code): bool
    {
        $expired = false;

        try{
            $stmt = $launcher_db_connection->prepare("SELECT 1 FROM gift_codes WHERE code = ? AND NOW() > valid_until");
            $stmt->bind_param('s', $code);
            $stmt->execute();
            $stmt->bind_result($expired);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $expired;
    }

    public function Owns_Inventory_Reward($launcher_db_connection, int $accountID, int $reward_id): bool
    {
        $reward_idExists = false;

        try {
            $stmt = $launcher_db_connection->prepare("SELECT 1 FROM account_inventory WHERE account_id = ? AND `reward_id` = ?");
            $stmt->bind_param('ii', $accountID, $reward_id);
            $stmt->execute();
            $stmt->bind_result($reward_idExists);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $reward_idExists;
    }

    public function Get_Character_Name_By_Guid($char_db_connection, int $guid): string
    {
        $character_name = '';

        try{
            $stmt = $this->PrepareQuery($char_db_connection, 'CHAR_GET_CHARACTER_NAME_BY_GUID');
            $stmt->bind_param('i', $guid);
            $stmt->execute();
            $stmt->bind_result($character_name);
            $stmt->fetch();
            $stmt->close();
            $char_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $character_name;
    }

    public function Get_Character_Guid_By_Name($char_db_connection, string $name): int
    {
        $character_guid = '';

        try {
            $stmt = $this->PrepareQuery($char_db_connection, 'CHAR_GET_CHARACTER_GUID_BY_NAME');
            $stmt->bind_param('s', $name);
            $stmt->execute();
            $stmt->bind_result($character_guid);
            $stmt->fetch();
            $stmt->close();
            $char_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $character_guid;
    }

    public function Get_Character_Info_By_Guid($char_db_connection, int $guid): array
    {
        $info = [];

        try
        {
            $name       = null;
            $race       = 0;
            $class      = 0;
            $gender     = 0;
            $level      = 0;
            $money      = 0;
            $totalkills = 0;
            $online     = false;

            $stmt = $this->PrepareQuery($char_db_connection, "CHAR_GET_CHARACTER_INFO_BY_GUID");
            $stmt->bind_param('i', $guid);
            $stmt->execute();
            $stmt->bind_result($name, $race, $class, $gender, $level, $money, $totalkills, $online);

            while ($stmt->fetch())
            {
                $info =
                [
                    'name'          => $name,
                    'race'          => $race,
                    'class'         => $class,
                    'gender'        => $gender,
                    'level'         => $level,
                    'money'         => $money,
                    'totalkills'    => $totalkills,
                    'online'        => $online,
                ];
            }

            $stmt->close();
            $char_db_connection->close();
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $info;
    }

    public function Get_Character_Professions_By_Guid($char_db_connection, int $guid): array
    {
        $professions = [];

        try {
            $skill_id   = 0;
            $level      = 0;
            $max        = 0;

            $stmt = $this->PrepareQuery($char_db_connection, "CHAR_GET_CHARACTER_PROFESSIONS_BY_GUID");
            $stmt->bind_param('i', $guid);
            $stmt->execute();
            $stmt->bind_result($skill_id, $level, $max);

            while ($stmt->fetch()) {
                $professions[] =
                    [
                        'skill_id'  => $skill_id,
                        'level'     => $level,
                        'max'       => $max,
                    ];
            }

            $stmt->close();
            $char_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $professions;
    }

    public function Get_Market_Sales($launcher_db_connection): array
    {
        $sales = [];

        try
        {
            $id                 = 0;
            $character_guid     = 0;
            $owner_account_id   = 0;
            $realm_id           = 0;
            $allow_bidding      = 0;
            $price              = 0;
            $date_added         = 0;
            $seconds_left       = 0;

            $stmt = $launcher_db_connection->prepare("SELECT id, character_guid, owner_account_id, realm_id, allow_bidding, price, `date_added`, 
                                                        UNIX_TIMESTAMP(`expires_on`) - UNIX_TIMESTAMP(NOW()) AS seconds_left FROM characters_market");
            $stmt->execute();
            $stmt->bind_result($id, $character_guid, $owner_account_id, $realm_id, $allow_bidding, $price, $date_added, $seconds_left);

            while ($stmt->fetch())
            {
                $sales[] =
                [
                    'id'                => $id,
                    'character_guid'    => $character_guid,
                    'owner_account_id'  => $owner_account_id,
                    'realm_id'          => $realm_id,
                    'realm_name'        => $this->database['realms'][$realm_id]['realm_name'],
                    'allow_bidding'     => $allow_bidding,
                    'price'             => $price,
                    'date_added'        => $date_added,
                    'seconds_left'      => $seconds_left,
                ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $sales;
    }

    public function Get_Market_Sale_Info($launcher_db_connection, $sale_id): array
    {
        $sale_info = [];

        try {
            $id                 = 0;
            $character_guid     = 0;
            $owner_account_id   = 0;
            $realm_id           = 0;
            $price              = 0;
            $date_added         = 0;
            $seconds_left       = 0;

            $stmt = $launcher_db_connection->prepare("SELECT id, character_guid, owner_account_id, realm_id, price, `date_added`, 
                                                        UNIX_TIMESTAMP(`expires_on`) - UNIX_TIMESTAMP(NOW()) AS seconds_left FROM characters_market WHERE id = ?");
            $stmt->bind_param('i', $sale_id);
            $stmt->execute();
            $stmt->bind_result($id, $character_guid, $owner_account_id, $realm_id, $price, $date_added, $seconds_left);

            while ($stmt->fetch()) {
                $sale_info =
                    [
                        'id'                => $id,
                        'character_guid'    => $character_guid,
                        'owner_account_id'  => $owner_account_id,
                        'realm_id'          => $realm_id,
                        'realm_name'        => $this->database['realms'][$realm_id]['realm_name'],
                        'price'             => $price,
                        'date_added'        => $date_added,
                        'seconds_left'      => $seconds_left,
                    ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $sale_info;
    }

    public function Get_Market_Bids_For_Sale($launcher_db_connection, $sale_id): array
    {
        $bids = [];

        try
        {
            $id         = 0;
            $buyer_id   = 0;
            $bid_amount   = 0;

            $stmt = $launcher_db_connection->prepare("SELECT id, buyer_id, bid_amount FROM characters_market_bids WHERE sale_id = ?");
            $stmt->bind_param('i', $sale_id);
            $stmt->execute();
            $stmt->bind_result($id, $buyer_id, $bid_amount);

            while ($stmt->fetch())
            {
                $bids[] =
                [
                    'id'            => $id,
                    'buyer_id'      => $buyer_id,
                    'bid_amount'    => $bid_amount,
                ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $bids;
    }

    public function Get_Market_Unread_Notifications_For($launcher_db_connection, $account_id): array
    {
        $notifications = [];

        try
        {
            $character_name     = 0;
            $character_race     = 0;
            $character_class    = 0;
            $character_level    = 0;
            $character_gender   = 0;

            $stmt = $launcher_db_connection->prepare("SELECT character_name, character_race, character_class, character_level, character_gender FROM characters_market_bids_won WHERE winner_id = ? AND notification_read = 0");
            $stmt->bind_param('i', $account_id);
            $stmt->execute();
            $stmt->bind_result($character_name, $character_race, $character_class, $character_level, $character_gender);

            while ($stmt->fetch())
            {
                $notifications[] =
                [
                    'character_name'    => $character_name,
                    'character_race'    => $character_race,
                    'character_class'   => $character_class,
                    'character_level'   => $character_level,
                    'character_gender'  => $character_gender,
                ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $notifications;
    }

    public function Get_Characters_Market_Highest_Bid_For($launcher_db_connection, $sale_id): array
    {
        $highest_bid = [];

        try
        {
            $id         = 0;
            $buyer_id   = 0;
            $bid_amount = 0;

            $stmt = $launcher_db_connection->prepare("SELECT id, buyer_id, bid_amount FROM characters_market_bids WHERE sale_id = ? ORDER BY bid_amount DESC LIMIT 1");
            $stmt->bind_param('i', $sale_id);
            $stmt->execute();
            $stmt->bind_result($id, $buyer_id, $bid_amount);

            while ($stmt->fetch())
            {
                $highest_bid =
                [
                    'id'            => $id,
                    'buyer_id'      => $buyer_id,
                    'bid_amount'    => $bid_amount,
                ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $highest_bid;
    }

    public function Get_Character_Race_By_Guid($char_db_connection, int $guid): int
    {
        $character_race = '';

        try {
            $stmt = $this->PrepareQuery($char_db_connection, 'CHAR_GET_CHARACTER_RACE_BY_GUID');
            $stmt->bind_param('i', $guid);
            $stmt->execute();
            $stmt->bind_result($character_race);
            $stmt->fetch();
            $stmt->close();
            $char_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $character_race;
    }

    public function Owns_Character_Guid($char_db_connection, int $accountID, int $guid): bool
    {
        $owns = false;

        try {
            $stmt = $this->PrepareQuery($char_db_connection, 'CHAR_GET_CHARACTER_BY_ACCOUNT_ID_AND_GUID');
            $stmt->bind_param('ii', $accountID, $guid);
            $stmt->execute();
            $stmt->bind_result($owns);
            $stmt->fetch();
            $stmt->close();
            $char_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $owns;
    }

    public function Character_Guid_Is_Banned($char_db_connection, int $guid): bool
    {
        $banned = false;

        try {
            $stmt = $this->PrepareQuery($char_db_connection, 'CHAR_IS_CHARACTER_GUID_BANNED');
            $stmt->bind_param('i', $guid);
            $stmt->execute();
            $stmt->bind_result($banned);
            $stmt->fetch();
            $stmt->close();
            $char_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $banned;
    }

    public function Character_Update_Account_ID($char_db_connection, int $guid, int $newAccountId): bool
    {
        try {
            $stmt = $this->PrepareQuery($char_db_connection, 'CHAR_UPDATE_CHARACTER_ACCOUNT_ID');
            $stmt->bind_param('ii', $newAccountId, $guid);
            $exec = $stmt->execute();
            $stmt->close();
            $char_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Is_Character_Guid_Online($char_db_connection, int $guid): bool
    {
        $is_online = false;

        try {
            $stmt = $this->PrepareQuery($char_db_connection, 'CHAR_GET_CHARACTER_ONLINE_BY_GUID');
            $stmt->bind_param('i', $guid);
            $stmt->execute();
            $stmt->bind_result($is_online);
            $stmt->fetch();
            $stmt->close();
            $char_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $is_online;
    }

    public function Set_Character_Location_m_x_y_z_o_Using_Guid($char_db_connection, int $guid, int $map, float $dest_x, float $dest_y, float $dest_z, float $orientation): bool
    {
        $success = false;

        try {
            $stmt = $this->PrepareQuery($char_db_connection, 'CHAR_UPDATE_CHARACTER_LOCATION_BY_GUID');
            $stmt->bind_param('iddddi', $map, $dest_x, $dest_y, $dest_z, $orientation, $guid);
            $stmt->execute();

            if ($stmt->affected_rows > 0) {
                $success = true;
            }

            $stmt->close();
            $char_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $success;
    }

    public function Get_Characters_Tickets_List_Using_Account_ID($char_db_connection, int $account_id, int $realmId, string $realmName): array
    {
        $tickets = [];

        try {
            $ticketId           = 0;
            $playerGuid         = 0;
            $playerRace         = 0;
            $playerGender       = 0;
            $playerName         = null;
            $message            = null;
            $lastModifiedTime   = null;
            $closed             = 0;
            $completed          = 0;
            $viewed             = 0;

            $stmt = $this->PrepareQuery($char_db_connection, "CHAR_GET_CHARACTERS_TICKETS_BY_ACCOUNT_ID");
            $stmt->bind_param('i', $account_id);
            $stmt->execute();
            $stmt->bind_result($ticketId, $playerGuid, $playerRace, $playerGender, $playerName, $message, $lastModifiedTime, $closed, $completed, $viewed);

            while ($stmt->fetch()) {
                $tickets[] =
                    [
                        'ticketId'          => $ticketId,
                        'playerGuid'        => $playerGuid,
                        'playerRace'        => $playerRace,
                        'playerGender'      => $playerGender,
                        'playerName'        => $playerName,
                        'message'           => $message,
                        'lastModifiedTime'  => $lastModifiedTime,
                        'closed'            => $closed,
                        'completed'         => $completed,
                        'viewed'            => $viewed,
                        'realmId'           => $realmId,
                        'realmName'         => $realmName,
                    ];
            }

            $stmt->close();
            $char_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $tickets;
    }

    public function Get_Realm_Ladderboard($char_db_connection, int $realmId, string $realmName): array
    {
        $ladderboard = [];

        try
        {
            $playerName     = null;
            $playerRace     = 0;
            $playerGender   = 0;
            $playerClass    = 0;
            $todayKills     = 0;
            $yesterdayKills = 0;
            $totalKills     = 0;

            $stmt = $this->PrepareQuery($char_db_connection, "CHAR_GET_CHARACTERS_LADDERBOARD");
            $stmt->execute();
            $stmt->bind_result($playerName, $playerRace, $playerGender, $playerClass, $todayKills, $yesterdayKills, $totalKills);

            while ($stmt->fetch())
            {
                $ladderboard[] =
                [
                    'playerName'        => $playerName,
                    'playerRace'        => $playerRace,
                    'playerGender'      => $playerGender,
                    'playerClass'       => $playerClass,
                    'todayKills'        => $todayKills,
                    'yesterdayKills'    => $yesterdayKills,
                    'totalKills'        => $totalKills,
                    'realmId'           => $realmId,
                    'realmName'         => $realmName,
                ];
            }

            $stmt->close();
            $char_db_connection->close();
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $ladderboard;
    }

    public function Get_Realm_Online_Players($char_db_connection, $config, int $realmId, string $realmName): array
    {
        $online_players = [];

        try
        {
            $account        = 0;
            $playerName     = null;
            $playerRace     = 0;
            $playerGender   = 0;
            $playerClass    = 0;
            $playerLevel    = 0;
            $zone_id        = 0;

            $max_rows = $config['pages']['online_players']['max_player_rows'];
            $stmt = $this->PrepareQuery($char_db_connection, "CHAR_GET_CHARACTERS_ONLINE");
            $stmt->bind_param('i', $max_rows);
            $stmt->execute();
            $stmt->bind_result($account, $playerName, $playerRace, $playerGender, $playerClass, $playerLevel, $zone_id);

            while ($stmt->fetch())
            {
                $online_players[] =
                [
                    'account'       => $account,
                    'playerName'    => $playerName,
                    'playerRace'    => $playerRace,
                    'playerGender'  => $playerGender,
                    'playerClass'   => $playerClass,
                    'playerLevel'   => $playerLevel,
                    'zoneName'      => ZONES::Get_Name($zone_id),
                    'realmId'       => $realmId,
                    'realmName'     => $realmName,
                ];
            }

            $stmt->close();
            $char_db_connection->close();
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $online_players;
    }

    public function Get_Realm_Online_Players_Count($char_db_connection): int
    {
        try
        {
            $count = 0;

            $stmt = $this->PrepareQuery($char_db_connection, "CHAR_GET_CHARACTERS_ONLINE_COUNT");
            $stmt->execute();
            $stmt->bind_result($count,);

            while ($stmt->fetch())
            {
                return $count;
            }

            $stmt->close();
            $char_db_connection->close();
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return 0;
    }

    public function Teleport_Destination_Is_Faction_Allowed($launcher_db_connection, int $id, bool $isAlliance): bool
    {
        $allowed = false;

        try {
            $factionColumn = $isAlliance ? 'alliance_allowed' : 'horde_allowed';

            $stmt = $launcher_db_connection->prepare("SELECT 1 FROM teleport_list WHERE id = ? AND $factionColumn = 1");
            $stmt->bind_param('i', $id);
            $stmt->execute();
            $stmt->bind_result($allowed);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();

            return $allowed;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $allowed;
    }

    public function Reward_ID_Exists_By_Month_Day($launcher_db_connection, int $month, int $day): int
    {
        $reward_id = 0;

        try {
            $stmt = $launcher_db_connection->prepare("
                SELECT lr.reward_id
                FROM login_rewards lr
                JOIN rewards r ON lr.reward_id = r.id
                WHERE lr.month = ? AND lr.day = ?;
            ");

            $stmt->bind_param('ii', $month, $day);
            $stmt->execute();
            $stmt->bind_result($reward_id);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();

            return $reward_id;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $reward_id;
    }

    public function Reward_Exists_Using_ID($launcher_db_connection, int $reward_id): bool
    {
        $reward_idExists = false;

        try {
            $stmt = $launcher_db_connection->prepare("SELECT 1 FROM rewards WHERE id = ?");
            $stmt->bind_param('i', $reward_id);
            $stmt->execute();
            $stmt->bind_result($reward_idExists);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $reward_idExists;
    }

    public function Get_Gift_Data_Using_Code($launcher_db_connection, string $gift_code): array
    {
        $gift_reward = [];

        try
        {
            $gift_id                = 0;
            $reward_id              = 0;
            $redeems_allowed        = null;
            $min_gm_level_allowed   = null;
            $max_gm_level_allowed   = null;
            $req_exact_gm_level     = null;
            $valid_until            = null;

            $stmt = $launcher_db_connection->prepare("SELECT id, reward_id, redeems_allowed, min_gm_level_allowed, max_gm_level_allowed, req_exact_gm_level, 
                valid_until FROM gift_codes WHERE code = ?");

            $stmt->bind_param('s', $gift_code);
            $stmt->execute();
            $stmt->bind_result($gift_id, $reward_id, $redeems_allowed, $min_gm_level_allowed, $max_gm_level_allowed, $req_exact_gm_level, $valid_until);

            while ($stmt->fetch())
            {
                $gift_reward =
                    [
                        'gift_id'               => $gift_id,
                        'reward_id'             => $reward_id,
                        'redeems_allowed'       => $redeems_allowed,
                        'min_gm_level_allowed'  => $min_gm_level_allowed,
                        'max_gm_level_allowed'  => $max_gm_level_allowed,
                        'req_exact_gm_level'    => $req_exact_gm_level,
                        'valid_until'           => $valid_until,
                    ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $gift_reward;
    }

    public function Get_Reward_Data_Using_ID($launcher_db_connection, int $reward_id): array
    {
        $reward_data = [];

        try
        {
            $id                 = 0;
            $title              = null;
            $picture_url        = null;
            $description        = null;
            $soap_command       = null;
            $auth_db_query      = null;
            $char_db_query      = null;
            $web_db_query       = null;
            $vision_db_query    = null;
            $requires_player    = false;
            $requires_input     = false;

            $stmt = $launcher_db_connection->prepare("SELECT id, title, picture_url, description, soap_command, auth_db_query, char_db_query, web_db_query, 
                vision_db_query, requires_player, requires_input FROM rewards WHERE id = ?");

            $stmt->bind_param('s', $reward_id);
            $stmt->execute();
            $stmt->bind_result($id, $title, $picture_url, $description, $soap_command, $auth_db_query, $char_db_query, $web_db_query, $vision_db_query, 
                $requires_player, $requires_input);

            while ($stmt->fetch())
            {
                $reward_data =
                [
                    'id'                => $id,
                    'title'             => $title,
                    'picture_url'       => $picture_url,
                    'description'       => $description,
                    'soap_command'      => $soap_command,
                    'auth_db_query'     => $auth_db_query,
                    'char_db_query'     => $char_db_query,
                    'web_db_query'      => $web_db_query,
                    'vision_db_query'   => $vision_db_query,
                    'requires_player'   => $requires_player,
                    'requires_input'    => $requires_input,
                ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $reward_data;
    }

    public function Nickname_exists($launcher_db_connection, int $account_id, string $nickname): bool
    {
        $nickname_exists = false;

        try {
            $stmt = $launcher_db_connection->prepare("SELECT 1 FROM account_data WHERE public_nickname = ? AND id != ?");
            $stmt->bind_param('si', $nickname, $account_id);
            $stmt->execute();
            $stmt->bind_result($nickname_exists);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $nickname_exists;
    }

    public function Characters_Market_Add($launcher_db_connection, int $character_guid, int $owner_account_id, int $realm_id, int $allow_bidding, int $price, int $duration_seconds): bool
    {
        try {
            $stmt = $launcher_db_connection->prepare("INSERT INTO characters_market (character_guid, owner_account_id, realm_id, allow_bidding, price, expires_on) 
                                                        VALUES (?, ?, ?, ?, ?, DATE_ADD(NOW(), INTERVAL ? SECOND))");
            $stmt->bind_param('iiiiii', $character_guid, $owner_account_id, $realm_id, $allow_bidding, $price, $duration_seconds);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Characters_Market_Delete_Expired_Non_Auctions($launcher_db_connection): bool
    {
        try
        {
            $stmt = $launcher_db_connection->prepare("DELETE FROM characters_market WHERE expires_on < NOW() AND `allow_bidding` = 0");
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Characters_Market_Delete_Sale($launcher_db_connection, $sale_id): bool
    {
        try {
            $stmt = $launcher_db_connection->prepare("DELETE FROM characters_market WHERE id = ?");
            $stmt->bind_param('i', $sale_id);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Characters_Market_Set_As_Expired($launcher_db_connection, $sale_id, $account_id): bool
    {
        try
        {
            $stmt = $launcher_db_connection->prepare("UPDATE characters_market SET expires_on = DATE_SUB(`expires_on`, INTERVAL 10000 DAY) WHERE id = ? AND owner_account_id = ?");
            $stmt->bind_param('ii', $sale_id, $account_id);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Characters_Market_Mark_Notifications_As_Read_For($launcher_db_connection, $account_id): bool
    {
        try
        {
            $stmt = $launcher_db_connection->prepare("UPDATE characters_market_bids_won SET notification_read = 1 WHERE winner_id = ?");
            $stmt->bind_param('i', $account_id);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Get_CMS_Changelog($cms_db_connection): array
    {
        $changelogs = [];

        try {
            $id             = 0;
            $category_name  = null;
            $changelog      = null;
            $date_timestamp = null;

            $stmt = $this->PrepareQuery($cms_db_connection, "CMS_GET_CHANGELOGS", true);
            $stmt->execute();
            $stmt->bind_result($id, $category_name, $changelog, $date_timestamp);

            while ($stmt->fetch()) {
                $changelogs[] =
                    [
                        'id'                => $id,
                        'category_name'     => $category_name,
                        'changelog'         => $changelog,
                        'date_timestamp'    => $date_timestamp,
                    ];
            }

            $stmt->close();
            $cms_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $changelogs;
    }

    public function Get_CMS_Articles($cms_db_connection, $website_url): array
    {
        $articles = [];

        try{
            $id         = 0;
            $date       = null;
            $author     = "Staff";
            $image_url  = "";
            $slug       = "";

            $headline_en = "";
            $headline_de = "";
            $headline_es = "";
            $headline_fr = "";
            $headline_no = "";
            $headline_ro = "";
            $headline_se = "";
            $headline_ru = "";
            $headline_zh = "";
            $headline_ko = "";
            $headline_fa = "";

            $content_en = "";
            $content_de = "";
            $content_es = "";
            $content_fr = "";
            $content_no = "";
            $content_ro = "";
            $content_se = "";
            $content_ru = "";
            $content_zh = "";
            $content_ko = "";
            $content_fa = "";

            $stmt = $this->PrepareQuery($cms_db_connection, "CMS_GET_ARTICLES", true);
            $stmt->execute();

            switch ($this->database['cms']['cms_id']) {
                case CMS_FUSION_GEN: {
                        $stmt->bind_result(
                            $id,
                            $date,
                            $author,
                            $image_url,
                            $headline_en,
                            $content_en,
                            $headline_de,
                            $content_de,
                            $headline_es,
                            $content_es,
                            $headline_fr,
                            $content_fr,
                            $headline_no,
                            $content_no,
                            $headline_ro,
                            $content_ro,
                            $headline_se,
                            $content_se,
                            $headline_ru,
                            $content_ru,
                            $headline_zh,
                            $content_zh,
                            $headline_ko,
                            $content_ko
                        );

                        while ($stmt->fetch()) {
                            if (str_starts_with($image_url, '["') && str_ends_with($image_url, '"]')) {
                                $image_url = $website_url . '/uploads/news/' . substr($image_url, 2, -2);
                            } elseif (str_starts_with($image_url, 'https://') || str_starts_with($image_url, 'http://')) {
                                $image_url = $image_url;
                            } else {
                                $image_url = $website_url . "/" . $image_url;
                            }

                            $articles[] =
                                [
                                    'id'            => $id,
                                    'date'          => $date,
                                    'author'        => $author,
                                    'image_url'     => $image_url,
                                    'headline_en'   => $headline_en,
                                    'content_en'    => $content_en,
                                    'headline_de'   => $headline_de,
                                    'content_de'    => $content_de,
                                    'headline_es'   => $headline_es,
                                    'content_es'    => $content_es,
                                    'headline_fr'   => $headline_fr,
                                    'content_fr'    => $content_fr,
                                    'headline_no'   => $headline_no,
                                    'content_no'    => $content_no,
                                    'headline_ro'   => $headline_ro,
                                    'content_ro'    => $content_ro,
                                    'headline_se'   => $headline_se,
                                    'content_se'    => $content_se,
                                    'headline_ru'   => $headline_ru,
                                    'content_ru'    => $content_ru,
                                    'headline_zh'   => $headline_zh,
                                    'content_zh'    => $content_zh,
                                    'headline_ko'   => $headline_ko,
                                    'content_ko'    => $content_ko,
                                    'headline_fa'   => $headline_fa,
                                    'content_fa'    => $content_fa,
                                    'redirect_url'  => $website_url . '/news/view/' . $id,
                                ];
                        }
                    }
                    break;
                case CMS_BLIZZCMS: {
                        $stmt->bind_result($id, $date, $headline_en, $content_en, $image_url);

                        while ($stmt->fetch()) {
                            $articles[] =
                                [
                                    'id'            => $id,
                                    'date'          => $date,
                                    'author'        => $author,
                                    'image_url'     => $website_url . '/assets/images/news/' . $image_url,
                                    'headline_en'   => $headline_en,
                                    'content_en'    => $content_en,
                                    'headline_de'   => $headline_de,
                                    'content_de'    => $content_de,
                                    'headline_es'   => $headline_es,
                                    'content_es'    => $content_es,
                                    'headline_fr'   => $headline_fr,
                                    'content_fr'    => $content_fr,
                                    'headline_no'   => $headline_no,
                                    'content_no'    => $content_no,
                                    'headline_ro'   => $headline_ro,
                                    'content_ro'    => $content_ro,
                                    'headline_se'   => $headline_se,
                                    'content_se'    => $content_se,
                                    'headline_ru'   => $headline_ru,
                                    'content_ru'    => $content_ru,
                                    'headline_zh'   => $headline_zh,
                                    'content_zh'    => $content_zh,
                                    'headline_ko'   => $headline_ko,
                                    'content_ko'    => $content_ko,
                                    'headline_fa'   => $headline_fa,
                                    'content_fa'    => $content_fa,
                                    'redirect_url'  => $website_url . '/news/' . $id . '/' . $slug,
                                ];
                        }
                    }
                    break;
                case CMS_WARCRY: {
                        $stmt->bind_result($id, $date, $author, $headline_en, $content_en, $image_url);

                        while ($stmt->fetch()) {
                            $articles[] =
                                [
                                    'id'            => $id,
                                    'date'          => $date,
                                    'author'        => $author,
                                    'image_url'     => $website_url . '/uploads/' . $image_url,
                                    'headline_en'   => $headline_en,
                                    'content_en'    => $content_en,
                                    'headline_de'   => $headline_de,
                                    'content_de'    => $content_de,
                                    'headline_es'   => $headline_es,
                                    'content_es'    => $content_es,
                                    'headline_fr'   => $headline_fr,
                                    'content_fr'    => $content_fr,
                                    'headline_no'   => $headline_no,
                                    'content_no'    => $content_no,
                                    'headline_ro'   => $headline_ro,
                                    'content_ro'    => $content_ro,
                                    'headline_se'   => $headline_se,
                                    'content_se'    => $content_se,
                                    'headline_ru'   => $headline_ru,
                                    'content_ru'    => $content_ru,
                                    'headline_zh'   => $headline_zh,
                                    'content_zh'    => $content_zh,
                                    'headline_ko'   => $headline_ko,
                                    'content_ko'    => $content_ko,
                                    'headline_fa'   => $headline_fa,
                                    'content_fa'    => $content_fa,
                                    'redirect_url'  => $website_url,
                                ];
                        }
                    }
                    break;
                case CMS_FUSION_WOW_CMS: {
                        $stmt->bind_result($id, $date, $author, $image_url, $headline_en, $content_en);

                        while ($stmt->fetch()) {
                            // contains a json array
                            $headline = json_decode($headline_en, true);
                            $content = json_decode($content_en, true);

                            if (str_starts_with($image_url, '["') && str_ends_with($image_url, '"]')) {
                                $image_url = $website_url . '/writable/uploads/news/' . substr($image_url, 2, -2);
                            } elseif (str_starts_with($image_url, 'https://') || str_starts_with($image_url, 'http://')) {
                                $image_url = $image_url;
                            } else {
                                $image_url = $website_url . "/" . $image_url;
                            }

                            $articles[] =
                                [
                                    'id'            => $id,
                                    'date'          => $date,
                                    'author'        => $author,
                                    'image_url'     => $image_url,
                                    'headline_en'   => $headline['english'],
                                    'content_en'    => $content['english'],
                                    'headline_de'   => $headline_de,
                                    'content_de'    => $content_de,
                                    'headline_es'   => $headline['spanish'],
                                    'content_es'    => $content['spanish'],
                                    'headline_fr'   => $headline_fr,
                                    'content_fr'    => $content_fr,
                                    'headline_no'   => $headline_no,
                                    'content_no'    => $content_no,
                                    'headline_ro'   => $headline_ro,
                                    'content_ro'    => $content_ro,
                                    'headline_se'   => $headline_se,
                                    'content_se'    => $content_se,
                                    'headline_ru'   => $headline_ru,
                                    'content_ru'    => $content_ru,
                                    'headline_zh'   => $headline_zh,
                                    'content_zh'    => $content_zh,
                                    'headline_ko'   => $headline_ko,
                                    'content_ko'    => $content_ko,
                                    'headline_fa'   => $headline['persian'],
                                    'content_fa'    => $content['persian'],
                                    'redirect_url'  => $website_url . '/news/view/' . $id,
                                ];
                        }
                    }
                    break;
                case CMS_ACORE_CMS: {
                        $stmt->bind_result($id, $date, $headline_en, $content_en, $image_url, $author);

                        while ($stmt->fetch()) {
                            $articles[] =
                                [
                                    'id'            => $id,
                                    'date'          => $date,
                                    'author'        => $author,
                                    'image_url'     => $image_url,
                                    'headline_en'   => $headline_en,
                                    'content_en'    => $content_en,
                                    'headline_de'   => $headline_de,
                                    'content_de'    => $content_de,
                                    'headline_es'   => $headline_es,
                                    'content_es'    => $content_es,
                                    'headline_fr'   => $headline_fr,
                                    'content_fr'    => $content_fr,
                                    'headline_no'   => $headline_no,
                                    'content_no'    => $content_no,
                                    'headline_ro'   => $headline_ro,
                                    'content_ro'    => $content_ro,
                                    'headline_se'   => $headline_se,
                                    'content_se'    => $content_se,
                                    'headline_ru'   => $headline_ru,
                                    'content_ru'    => $content_ru,
                                    'headline_zh'   => $headline_zh,
                                    'content_zh'    => $content_zh,
                                    'headline_ko'   => $headline_ko,
                                    'content_ko'    => $content_ko,
                                    'headline_fa'   => $headline_fa,
                                    'content_fa'    => $content_fa,
                                    'redirect_url'  => $website_url . '/news/view/' . $id,
                                ];
                        }
                    }
                    break;
            }

            $stmt->close();
            $cms_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $articles;
    }

    public function Get_Launcher_Faq($launcher_db_connection): array
    {
        $faq = [];

        try {
            $id     = 0;
            $title  = null;
            $text   = null;

            $stmt = $launcher_db_connection->prepare("SELECT `id`, `title`, `text` FROM `faq` ORDER BY `id` DESC");
            $stmt->execute();
            $stmt->bind_result($id, $title, $text);

            while ($stmt->fetch()) {
                $faq[] =
                    [
                        'id'    => $id,
                        'title' => $title,
                        'text'  => $text,
                    ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $faq;
    }

    public function Get_Launcher_Shop_List($launcher_db_connection): array
    {
        $shop_list = [];

        try {
            $id                 = 0;
            $category           = null;
            $picture_url        = null;
            $title              = null;
            $description        = null;
            $dp_or_bpc_price    = 0;
            $vp_price           = 0;

            $stmt = $launcher_db_connection->prepare("SELECT s.id, s.category, r.picture_url, r.title, r.description, s.dp_or_bpc_price, s.vp_price
                FROM shop s LEFT JOIN rewards r ON s.reward_id = r.id ORDER BY s.id DESC");
            $stmt->execute();
            $stmt->bind_result($id, $category, $picture_url, $title, $description, $dp_or_bpc_price, $vp_price);

            while ($stmt->fetch()) {
                $shop_list[] =
                    [
                        'id'                => $id,
                        'category'          => $category,
                        'picture_url'       => $picture_url,
                        'title'             => $title,
                        'description'       => $description,
                        'dp_or_bpc_price'   => $dp_or_bpc_price,
                        'vp_price'          => $vp_price,
                    ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $shop_list;
    }

    public function Get_Launcher_Teleport_List($launcher_db_connection): array
    {
        $teleport_list = [];

        try {
            $id                 = 0;
            $name               = null;
            $dp_or_bpc_price    = 0;
            $vp_price           = 0;

            $stmt = $launcher_db_connection->prepare("SELECT `id`, `name`, `dp_or_bpc_price`, `vp_price` FROM `teleport_list`");
            $stmt->execute();
            $stmt->bind_result($id, $name, $dp_or_bpc_price, $vp_price);

            while ($stmt->fetch()) {
                $teleport_list[] =
                    [
                        'id'                => $id,
                        'name'              => $name,
                        'dp_or_bpc_price'   => $dp_or_bpc_price,
                        'vp_price'          => $vp_price,
                    ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $teleport_list;
    }

    public function Get_Launcher_Teleport_Coordinates_Using_ID($launcher_db_connection, int $tele_id): array
    {
        $coordinates = [];

        try {
            $id             = 0;
            $name           = null;
            $map            = 0;
            $dest_x         = 0;
            $dest_y         = 0;
            $dest_z         = 0;
            $orientation    = 0;

            $stmt = $launcher_db_connection->prepare("SELECT `id`, `name`, `map`, `dest_x`, `dest_y`, `dest_z`, `orientation` FROM `teleport_list` WHERE id = ?");
            $stmt->bind_param('i', $tele_id);
            $stmt->execute();
            $stmt->bind_result($id, $name, $map, $dest_x, $dest_y, $dest_z, $orientation);

            while ($stmt->fetch()) {
                $coordinates =
                    [
                        'id'            => $id,
                        'name'          => $name,
                        'map'           => $map,
                        'dest_x'        => $dest_x,
                        'dest_y'        => $dest_y,
                        'dest_z'        => $dest_z,
                        'orientation'   => $orientation,
                    ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $coordinates;
    }

    public function Get_Launcher_Teleport_Costs_Using_ID($launcher_db_connection, int $tele_id): array
    {
        $costs = [];

        try {
            $dp_or_bpc_price    = 0;
            $vp_price           = 0;

            $stmt = $launcher_db_connection->prepare("SELECT `dp_or_bpc_price`, `vp_price` FROM `teleport_list` WHERE id = ?");
            $stmt->bind_param('i', $tele_id);
            $stmt->execute();
            $stmt->bind_result($dp_or_bpc_price, $vp_price);

            while ($stmt->fetch()) {
                $costs =
                    [
                        'dp_or_bpc_price'   => $dp_or_bpc_price,
                        'vp_price'          => $vp_price,
                    ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $costs;
    }

    public function Get_Launcher_Shop_Article_Costs_Using_ID($launcher_db_connection, int $article_id): array
    {
        $costs = [];

        try {
            $reward_id          = 0;
            $dp_or_bpc_price    = 0;
            $vp_price           = 0;

            $stmt = $launcher_db_connection->prepare("SELECT `dp_or_bpc_price`, `vp_price`, `reward_id` FROM `shop` WHERE id = ?");
            $stmt->bind_param('i', $article_id);
            $stmt->execute();
            $stmt->bind_result($dp_or_bpc_price, $vp_price, $reward_id);

            while ($stmt->fetch()) {
                $costs =
                    [
                        'dp_or_bpc_price'   => $dp_or_bpc_price,
                        'vp_price'          => $vp_price,
                        'reward_id'         => $reward_id,
                    ];
            }

            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $costs;
    }

    public function Get_User_Avatar_Url_Using_ID($launcher_db_connection, int $id): string
    {
        $url = "?";

        try {
            $stmt = $launcher_db_connection->prepare("SELECT image_url FROM account_avatars WHERE account_id = ?");
            $stmt->bind_param('i', $id);
            $stmt->execute();
            $stmt->bind_result($url);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $url;
    }

    public function Update_User_Avatar_Url_Using_ID($launcher_db_connection, int $id, string $url): bool
    {
        try {
            $stmt = $launcher_db_connection->prepare("REPLACE INTO `account_avatars` (`account_id`, `image_url`) VALUES (?, ?)");
            $stmt->bind_param('is', $id, $url);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Get_User_Public_Nickname_Using_ID($launcher_db_connection, int $id): string
    {
        $public_nickname = "?";

        try {
            $stmt = $launcher_db_connection->prepare("SELECT public_nickname FROM account_data WHERE id = ?");
            $stmt->bind_param('i', $id);
            $stmt->execute();
            $stmt->bind_result($public_nickname);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $public_nickname;
    }

    public function Delete_Password_Recovery_Code($launcher_db_connection, string $username): bool
    {
        try {
            $stmt = $launcher_db_connection->prepare("DELETE FROM `password_recovery` WHERE username = ?");
            $stmt->bind_param('s', $username);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Insert_V_S_Account($auth_db_connection, string $username, string $salt, string $verifier, string $email): bool
    {
        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_INSERT_V_S_ACCOUNT');
            $stmt->bind_param('ssss', $username, $salt, $verifier, $email);
            $exec = $stmt->execute();
            $auth_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Insert_SPH_Account($auth_db_connection, string $username, string $sha_pass_hash, string $email): bool
    {
        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_INSERT_SHA_PASS_HASH_ACCOUNT');
            $stmt->bind_param('sss', $username, $sha_pass_hash, $email);
            $exec = $stmt->execute();
            $auth_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Insert_SHA_Battlenet_Account($auth_db_connection, string $email, string $sha_pass_hash): bool
    {
        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_INSERT_SHA_BATTLENET_ACCOUNT');
            $stmt->bind_param('ss', $email, $sha_pass_hash);
            $exec = $stmt->execute();
            $auth_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Insert_S_V_Battlenet_Account($auth_db_connection, string $email, string $salt, string $verifier): bool
    {
        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_INSERT_S_V_BATTLENET_ACCOUNT');
            $stmt->bind_param('sss', $email, $salt, $verifier);
            $exec = $stmt->execute();
            $auth_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Insert_New_V_S_Password($auth_db_connection, string $salt, string $verifier, string $username): bool
    {
        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_UPDATE_S_V_PASSWORD');
            $stmt->bind_param('sss', $salt, $verifier, $username);
            $exec = $stmt->execute();
            $auth_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Insert_New_SPH_Password($auth_db_connection, string $sha_pass_hash, string $username): bool
    {
        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_UPDATE_SHA_PASS_HASH_PASSWORD');
            $stmt->bind_param('ss', $sha_pass_hash, $username);
            $exec = $stmt->execute();
            $auth_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Insert_New_Battlenet_Password($auth_db_connection, string $sha_pass_hash, string $email): bool
    {
        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_INSERT_BATTLENET_PASSWORD');
            $stmt->bind_param('ss', $sha_pass_hash, $email);
            $exec = $stmt->execute();
            $auth_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Link_Battlenet_Account($auth_db_connection, string $email): bool
    {
        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_LINK_BATTLENET_ACCOUNT');
            $stmt->bind_param('s', $email);
            $exec = $stmt->execute();
            $auth_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Get_Vote_Points_Using_AccountID_Or_Username($cms_db_connection, int $accountID, string $orUsername): int
    {
        $vp = 0;

        try
        {
            if ($this->database['cms']['cms_id'] == CMS_ACORE_CMS)
            {
                $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_GET_VOTE_POINTS_BY_USERNAME', true);
                $stmt->bind_param('s', $orUsername);
            }
            else
            {
                $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_GET_VOTE_POINTS_BY_USER_ID', true);
                $stmt->bind_param('i', $accountID);
            }

            $stmt->execute();
            $stmt->bind_result($vp);
            $stmt->fetch();
            $stmt->close();
            $cms_db_connection->close();
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $vp;
    }

    public function Get_Donate_Points_Using_AccountID_Or_Username($cms_db_connection, int $accountID, string $orUsername): int
    {
        $dp = 0;

        try
        {
            if ($this->database['cms']['cms_id'] == CMS_ACORE_CMS)
            {
                $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_GET_DONATE_POINTS_BY_USERNAME', true);
                $stmt->bind_param('s', $orUsername);
            }
            else
            {
                $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_GET_DONATE_POINTS_BY_USER_ID', true);
                $stmt->bind_param('i', $accountID);
            }

            $stmt->execute();
            $stmt->bind_result($dp);
            $stmt->fetch();
            $stmt->close();
            $cms_db_connection->close();
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $dp;
    }

    public function LegionCore_Get_Vote_Points($auth_db_connection, int $accountID): int
    {
        $vp = 0;

        try {
            $stmt = $auth_db_connection->prepare("SELECT amount FROM account_tokens WHERE account_id = ? AND tokenType = 2");
            $stmt->bind_param('i', $accountID);
            $stmt->execute();
            $stmt->bind_result($vp);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $vp;
    }

    public function LegionCore_Get_Donate_Points($auth_db_connection, int $accountID): int
    {
        $dp = 0;

        try {
            $stmt = $auth_db_connection->prepare("SELECT amount FROM account_tokens WHERE account_id = ? AND tokenType = 1");
            $stmt->bind_param('i', $accountID);
            $stmt->execute();
            $stmt->bind_result($dp);
            $stmt->fetch();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $dp;
    }

    public function Get_Last_Time_Voted_Unixtimestamp($cms_db_connection, int $site_id, int $accountID): int
    {
        $last_time = time();

        try {
            $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_GET_LAST_TIME_VOTED_UNIXTIMESTAMP', true);
            $stmt->bind_param('ii', $site_id, $accountID);
            $stmt->execute();
            $stmt->bind_result($last_time);
            $stmt->fetch();
            $stmt->close();
            $cms_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $last_time;
    }

    public function Get_Vote_Site_Cooldown($cms_db_connection, int $site_id): int
    {
        $cooldown_in_seconds = 0;

        try {
            $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_GET_VOTE_SITE_COOLDOWN_IN_SECONDS', true);
            $stmt->bind_param('i', $site_id);
            $stmt->execute();
            $stmt->bind_result($cooldown_in_seconds);
            $stmt->fetch();
            $stmt->close();
            $cms_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $cooldown_in_seconds;
    }

    public function Get_Vote_Site_Points($cms_db_connection, int $site_id): int
    {
        $points = 0;

        try {
            $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_GET_VOTE_SITE_POINTS', true);
            $stmt->bind_param('i', $site_id);
            $stmt->execute();
            $stmt->bind_result($points);
            $stmt->fetch();
            $stmt->close();
            $cms_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $points;
    }

    public function Remove_Vote_Points_Using_AccountID_Or_Username($cms_db_connection, int $accountID, int $amount, string $orUsername): bool
    {
        try
        {
            if ($this->database['cms']['cms_id'] == CMS_ACORE_CMS)
            {
                $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_REMOVE_VOTE_POINTS_BY_USERNAME', true);
                $stmt->bind_param('is', $amount, $orUsername);
            }
            else
            {
                $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_REMOVE_VOTE_POINTS_BY_USER_ID', true);
                $stmt->bind_param('ii', $amount, $accountID);
            }
            
            $exec = $stmt->execute();
            $cms_db_connection->close();

            return $exec;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Remove_Donate_Points_Using_AccountID_Or_Username($cms_db_connection, int $accountID, int $amount, string $orUsername): bool
    {
        try 
        {
            if ($this->database['cms']['cms_id'] == CMS_ACORE_CMS)
            {
                $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_REMOVE_DONATE_POINTS_BY_USERNAME', true);
                $stmt->bind_param('is', $amount, $orUsername);
            }
            else
            {
                $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_REMOVE_DONATE_POINTS_BY_USER_ID', true);
                $stmt->bind_param('ii', $amount, $accountID);
            }

            $exec = $stmt->execute();
            $cms_db_connection->close();

            return $exec;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Add_Vote_Points_Using_AccountID_Or_Username($cms_db_connection, int $accountID, int $amount, string $orUsername): bool
    {
        try
        {
            if ($this->database['cms']['cms_id'] == CMS_ACORE_CMS)
            {
                $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_ADD_VOTE_POINTS_BY_USERNAME', true);
                $stmt->bind_param('is', $amount, $orUsername);
            }
            else
            {
                $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_ADD_VOTE_POINTS_BY_USER_ID', true);
                $stmt->bind_param('ii', $amount, $accountID);
            }

            $exec = $stmt->execute();
            $cms_db_connection->close();

            return $exec;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Add_Donate_Points_Using_AccountID_Or_Username($cms_db_connection, int $accountID, int $amount, string $orUsername): bool
    {
        try
        {
            if ($this->database['cms']['cms_id'] == CMS_ACORE_CMS)
            {
                $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_ADD_DONATE_POINTS_BY_USERNAME', true);
                $stmt->bind_param('is', $amount, $orUsername);
            }
            else
            {
                $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_ADD_DONATE_POINTS_BY_USER_ID', true);
                $stmt->bind_param('ii', $amount, $accountID);
            }

            $exec = $stmt->execute();
            $cms_db_connection->close();

            return $exec;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function LegionCore_Add_Vote_Points($auth_db_connection, int $accountID, int $amount): bool
    {
        try {
            $stmt = $auth_db_connection->prepare("UPDATE account_tokens SET amount = amount + ? WHERE account_id = ? AND tokenType = 2");
            $stmt->bind_param('ii', $amount, $accountID);
            $exec = $stmt->execute();
            $auth_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function LegionCore_Add_Donate_Points($auth_db_connection, int $accountID, int $amount): bool
    {
        try {
            $stmt = $auth_db_connection->prepare("UPDATE account_tokens SET amount = amount + ? WHERE account_id = ? AND tokenType = 1");
            $stmt->bind_param('ii', $amount, $accountID);
            $exec = $stmt->execute();
            $auth_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Insert_Vote_Log($cms_db_connection, int $siteID, int $accountID): bool
    {
        try {
            $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_INSERT_VOTE_LOG', true);
            $stmt->bind_param('ii', $siteID, $accountID);
            $exec = $stmt->execute();
            $cms_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Insert_Characters_Market_Log($launcher_db_connection, int $sale_id, int $character_guid, int $old_account_id, int $new_account_id, int $realm_id): bool
    {
        try
        {
            $stmt = $launcher_db_connection->prepare("INSERT INTO characters_market_logs (sale_id, character_guid, old_account_id, new_account_id, realm_id) VALUES (?, ?, ?, ?, ?)");
            $stmt->bind_param('iiiii', $sale_id, $character_guid, $old_account_id, $new_account_id, $realm_id);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Insert_To_Characters_Market_Bids_Won($launcher_db_connection, int $buyer_id, string $character_name, int $character_race, int $character_class, int $character_level, int $character_gender): bool
    {
        try
        {
            $stmt = $launcher_db_connection->prepare("INSERT INTO characters_market_bids_won (winner_id, character_name, character_race, character_class, character_level, character_gender) VALUES (?, ?, ?, ?, ?, ?)");
            $stmt->bind_param('isiiii', $buyer_id, $character_name, $character_race, $character_class, $character_level, $character_gender);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Insert_Characters_Market_Bid($launcher_db_connection, int $buyer_id, int $sale_id, int $bid_amount): bool
    {
        try
        {
            $stmt = $launcher_db_connection->prepare("INSERT INTO characters_market_bids (buyer_id, sale_id, bid_amount) VALUES (?, ?, ?)");
            $stmt->bind_param('iii', $buyer_id, $sale_id, $bid_amount);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Delete_Characters_Market_Bid($launcher_db_connection, int $buyer_id, int $sale_id): bool
    {
        try
        {
            $stmt = $launcher_db_connection->prepare("DELETE FROM characters_market_bids WHERE buyer_id = ? AND sale_id = ?");
            $stmt->bind_param('ii', $buyer_id, $sale_id);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Update_Characters_Market_Sale_Price($launcher_db_connection, int $sale_id, int $price): bool
    {
        try
        {
            $stmt = $launcher_db_connection->prepare("UPDATE characters_market SET price = ? WHERE id = ?");
            $stmt->bind_param('ii', $price, $sale_id);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Get_Characters_Market_Bidded_Amount_For($launcher_db_connection, int $buyer_id, int $sale_id): int
    {
        $bidded_amount = 0;

        try
        {
            $stmt = $launcher_db_connection->prepare("SELECT bid_amount FROM characters_market_bids WHERE buyer_id = ? AND sale_id = ?");
            $stmt->bind_param('ii', $buyer_id, $sale_id);
            $stmt->execute();
            $stmt->bind_result($bidded_amount);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
            return $bidded_amount;
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $bidded_amount;
    }

    public function Delete_All_Characters_Market_Bids_For($launcher_db_connection, int $market_id)
    {
        try
        {
            $stmt = $launcher_db_connection->prepare("DELETE FROM characters_market_bids WHERE sale_id = ?");
            $stmt->bind_param('i', $market_id);
            $stmt->execute();
            $launcher_db_connection->close();
        }
        catch (\Exception $e)
        {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }
    }

    public function Insert_Website_Account_Data($cms_db_connection, int $accountID, string $username, string $email, string $pass): bool
    {
        try {
            switch ($this->database['cms']['cms_id']) {
                case CMS_FUSION_GEN: {
                        $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_INSERT_ACCOUNT_DATA', true);
                        $stmt->bind_param('is', $accountID, $username);
                        $exec = $stmt->execute();
                        $cms_db_connection->close();

                        return $exec;
                    }
                    break;
                case CMS_BLIZZCMS: {
                        $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_INSERT_ACCOUNT_DATA', true);
                        $stmt->bind_param('iss', $accountID, $username, $email);
                        $exec = $stmt->execute();
                        $cms_db_connection->close();

                        return $exec;
                    }
                    break;
                case CMS_ACORE_CMS: {
                        $stmt = $cms_db_connection->prepare('INSERT INTO wp_usermeta (user_id, meta_key, meta_value) VALUES
                                                (?, "wp_user_level", "0"),
                                                (?, "wp_capabilities", "a:1:{s:13:\"sp_player\";b:1;}")');
                        $stmt->bind_param('ii', $accountID, $accountID);

                        if ($stmt->execute()) {
                            $stmt = $cms_db_connection->prepare('INSERT INTO wp_users (user_login, user_pass, user_nicename, user_email, user_status, display_name, user_registered) VALUES (?, MD5(?), ?, ?, "0", ?, NOW())');
                            $stmt->bind_param('sssss', $username, $pass, $username, $email, $username);
                            $exec = $stmt->execute();
                            $cms_db_connection->close();

                            return $exec;
                        }

                        $cms_db_connection->close();
                    }
                    break;
                case CMS_WARCRY: {
                        $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_INSERT_ACCOUNT_DATA', true);
                        $stmt->bind_param('is', $accountID, $username);
                        $exec = $stmt->execute();
                        $cms_db_connection->close();

                        return $exec;
                    }
                    break;
                case CMS_FUSION_WOW_CMS: {
                        $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_INSERT_ACCOUNT_DATA', true);
                        $stmt->bind_param('is', $accountID, $username);
                        $exec = $stmt->execute();
                        $cms_db_connection->close();

                        return $exec;
                    }
                    break;
            }
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function LegionCore_Remove_Vote_Points($auth_db_connection, int $accountID, int $amount): bool
    {
        try {
            $stmt = $auth_db_connection->prepare("UPDATE account_tokens SET amount = amount - ? WHERE account_id = ? AND tokenType = 2");
            $stmt->bind_param('ii', $amount, $accountID);
            $exec = $stmt->execute();
            $auth_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function LegionCore_Remove_Donate_Points($auth_db_connection, int $accountID, int $amount): bool
    {
        try {
            $stmt = $auth_db_connection->prepare("UPDATE account_tokens SET amount = amount - ? WHERE account_id = ? AND tokenType = 1");
            $stmt->bind_param('ii', $amount, $accountID);
            $exec = $stmt->execute();
            $auth_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Get_Vote_Sites_List($cms_db_connection, int $accountID): array
    {
        $sites = [];

        try {
            $id             = 0;
            $title          = null;
            $vote_url       = null;
            $image_url      = null;
            $points_reward  = 0;
            $seconds_left   = 0;

            $site_cooldown = 0;
            $last_time = 0;

            $stmt = $this->PrepareQuery($cms_db_connection, 'CMS_GET_VOTE_SITES', true);
            $stmt->bind_param('i', $accountID);
            $stmt->execute();
            $stmt->bind_result($id, $title, $vote_url, $image_url, $points_reward, $site_cooldown, $last_time);

            while ($stmt->fetch()) {
                if ($last_time > 0) {
                    if ($site_cooldown > (time() - $last_time)) {
                        $seconds_left = $site_cooldown - (time() - $last_time);
                    }
                } else {
                    $seconds_left   = 0;
                }

                $sites[] =
                    [
                        'id'                => $id,
                        'title'             => $title,
                        'vote_url'          => $vote_url,
                        'image_url'         => $image_url,
                        'points_reward'     => $points_reward,
                        'seconds_left'      => $seconds_left,
                    ];
            }

            $stmt->close();
            $cms_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $sites;
    }

    public function Remove_BattlePayCredits_Using_Email($auth_db_connection, int $email, int $amount)
    {
        try {
            $stmt = $this->PrepareQuery($auth_db_connection, 'AUTH_REMOVE_BATTLEPAYCREDITS_BY_EMAIL', true);
            $stmt->bind_param('is', $amount, $email);
            $stmt->execute();
            $stmt->close();
            $auth_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }
    }

    public function Insert_Soap_Command_Log($launcher_db_connection, string $username, int $realmId, string $command)
    {
        try {
            $stmt = $launcher_db_connection->prepare("INSERT INTO `soap_logs` (`username`, `realm_id`, `command`) VALUES (?, ?, ?)");
            $stmt->bind_param('sis', $username, $realmId, $command);
            $stmt->execute();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }
    }

    public function Insert_Login_Claimed_Rewards_Log($launcher_db_connection, int $accountID, int $month, string $day)
    {
        try {
            $stmt = $launcher_db_connection->prepare("INSERT INTO `login_claimed_rewards` (`account_id`, `month`, `day`) VALUES (?, ?, ?)");
            $stmt->bind_param('iii', $accountID, $month, $day);
            $stmt->execute();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }
    }

    public function Insert_Gift_Redeem_Log($launcher_db_connection, int $accountID, int $gift_id)
    {
        try {
            $stmt = $launcher_db_connection->prepare("INSERT INTO `gift_redeems` (`user_id`, `gift_id`) VALUES (?, ?)");
            $stmt->bind_param('ii', $accountID, $gift_id);
            $stmt->execute();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }
    }

    public function Insert_Claimed_Reward_To_Account_Inventory($launcher_db_connection, int $accountID, int $reward_id)
    {
        try {
            $stmt = $launcher_db_connection->prepare("INSERT INTO `account_inventory` (`account_id`, `reward_id`, `acquired_on`) VALUES (?, ?, NOW())");
            $stmt->bind_param('ii', $accountID, $reward_id);
            $stmt->execute();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }
    }

    public function Delete_Used_Inventory_Item_From_Account_Id($launcher_db_connection, int $accountID, int $reward_id)
    {
        try {
            $stmt = $launcher_db_connection->prepare("DELETE FROM `account_inventory` WHERE account_id = ? AND reward_id = ? LIMIT 1"); // LIMIT 1 because user might have multiple of same reward
            $stmt->bind_param('ii', $accountID, $reward_id);
            $stmt->execute();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }
    }

    public function Get_Rasar_Key_From_Account_Id($launcher_db_connection, $account_id)
    {
        $key = null;

        try {
            $stmt = $launcher_db_connection->prepare("SELECT `rasar_key` FROM `account_data` WHERE `id` = ?");
            $stmt->bind_param('i', $account_id);
            $stmt->execute();
            $stmt->bind_result($key);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $key;
    }

    public function Get_Rasar_IV_From_Account_Id($launcher_db_connection, $account_id)
    {
        $iv = null;

        try {
            $stmt = $launcher_db_connection->prepare("SELECT `rasar_iv` FROM `account_data` WHERE `id` = ?");
            $stmt->bind_param('i', $account_id);
            $stmt->execute();
            $stmt->bind_result($iv);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $iv;
    }

    public function Update_Rasar_Key_For_Account_Id($launcher_db_connection, $key, $account_id): bool
    {
        try {
            $stmt = $launcher_db_connection->prepare("UPDATE account_data SET rasar_key = ? WHERE id = ?");
            $stmt->bind_param('si', $key, $account_id);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Update_Rasar_IV_For_Account_Id($launcher_db_connection, $iv, $account_id): bool
    {
        try {
            $stmt = $launcher_db_connection->prepare("UPDATE account_data SET rasar_iv = ? WHERE id = ?");
            $stmt->bind_param('si', $iv, $account_id);
            $exec = $stmt->execute();
            $launcher_db_connection->close();

            return $exec;
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return false;
    }

    public function Insert_Rate_Limiter_For($launcher_db_connection, string $ip, int $type)
    {
        try {
            $stmt = $launcher_db_connection->prepare("INSERT INTO rate_limiter (ip_address, `type`, `count`) VALUES (?, ?, 1) ON DUPLICATE KEY UPDATE `count` = `count` + 1");
            $stmt->bind_param('si', $ip, $type);
            $stmt->execute();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }
    }

    public function Count_Rate_Limiter_Logs_For($launcher_db_connection, $config, string $ip_address, int $type): int
    {
        $count = 0;

        try {
            $this->Delete_Expired_Rate_Limiter_Logs($launcher_db_connection, $config);

            $stmt = $launcher_db_connection->prepare("SELECT `count` FROM `rate_limiter` WHERE `ip_address` = ? AND `type` = ? ORDER BY date DESC LIMIT 1");
            $stmt->bind_param('si', $ip_address, $type);
            $stmt->execute();
            $stmt->bind_result($count);
            $stmt->fetch();
            $stmt->close();
            // $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $count;
    }

    public function Get_Rate_Limiter_Cooldown_Left_For($launcher_db_connection, $config, string $ip_address, int $reference): int
    {
        $time_left = 0;

        $config_wait_time = $config['rate_limiter'][$reference]['wait_time'];
        $config_wait_time = isset($config_wait_time) ? $config_wait_time : 60;

        try {
            $stmt = $launcher_db_connection->prepare("SELECT NOW()-`date` FROM `rate_limiter` WHERE `ip_address` = ? AND `type` = ? AND NOW() - `date` < ? ORDER BY `date` DESC LIMIT 1");
            $stmt->bind_param('sii', $ip_address, $reference, $config_wait_time);
            $stmt->execute();
            $stmt->bind_result($time_left);
            $stmt->fetch();
            $stmt->close();
            $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }

        return $config_wait_time - $time_left;
    }

    public function Delete_Expired_Rate_Limiter_Logs($launcher_db_connection, $config)
    {
        try {
            foreach ($config['rate_limiter'] as $key => $value) {
                if (isset($value['wait_time'])) {
                    $stmt = $launcher_db_connection->prepare("DELETE FROM `rate_limiter` WHERE `type` = ? AND NOW() - `date` > ?");
                    $stmt->bind_param('ii', $key, $value['wait_time']);
                    $stmt->execute();
                    $stmt->close();
                }
            }
            // $launcher_db_connection->close();
        } catch (\Exception $e) {
            (new LOGGER())->AppendMysqlError(__FUNCTION__ . " : " . $e->getMessage());
        }
    }
}
