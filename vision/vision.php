<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

header('Content-Type: application/json; charset=UTF-8');

// CONFIGS
require_once('configs/config.php');
require_once('configs/database.php');
require_once('configs/discord.php');

// ENGINE
include_once('engine/auth.php');
include_once('engine/char.php');
include_once('engine/cms.php');
include_once('engine/definitions.php');
include_once('engine/discord.php');
include_once('engine/downloads.php');
include_once('engine/launcher.php');
include_once('engine/logger.php');
include_once('engine/mailer.php');
include_once('engine/queries.php');
include_once('engine/soap.php');
include_once('engine/tools.php');

// MODULES
include_once('engine/modules/account_inventory.php');
include_once('engine/modules/characters_market.php');
include_once('engine/modules/discord_rpc.php');
include_once('engine/modules/gift_codes.php');
include_once('engine/modules/login_rewards.php');
include_once('engine/modules/private_messages.php');
include_once('engine/modules/shopper.php');
include_once('engine/modules/teleporter.php');
include_once('engine/modules/unstucker.php');
include_once('engine/modules/voter.php');

// RESOURCES
include_once('engine/resources/zones.php');

if ($_SERVER['REQUEST_METHOD'] === 'POST')
{
    $api = new APIHandle($config, $database, $discord);
    $api->handle($_POST);
}
elseif ($_SERVER['REQUEST_METHOD'] === 'GET' && isset($_GET['execute']))
{
    $api = new APIHandle($config, $database, $discord);
    $api->handle($_GET);
}
else
{
    http_response_code(400);
    echo json_encode(array('Hello' => 'Welcome to Vision API.'));
}

class APIHandle
{
    public $config;
    private $database;
    private $discord;

    public function __construct($config, $database, $discord)
    {
        $this->config = $config;
        $this->database = $database;
        $this->discord = $discord;
    }

    public function handle($VAR)
    {
        $caseFunctions = 
        [
            1   => 'handleStandardAuth',
            2   => 'handleBattlenetAuth',
            3   => 'handleRegistration',
            4   => 'handleAuthTokenData',
            5   => 'handlePasswordRecovery',
            6   => 'handleChangePasswordByCode',
            7   => 'handleAccountCurrencies',
            8   => 'handleUpdateAvatarUrl',
            9   => 'handleWebsiteArticles',
            10  => 'handleLauncherEvents',
            11  => 'handleWebsiteChangelogs',
            12  => 'handleLoginRewards',
            13  => 'handleClaimLoginReward',
            14  => 'handleAccountInventory',
            15  => 'handleAccountCharactersList',
            16  => 'handleUseInventoryItem',
            17  => 'handleUpdatePublicNickname',
            18  => 'handleGetPublicNickname',
            19  => 'handleUnstuckCharacter',
            20  => 'handleTeleportList',
            21  => 'handleTeleportCharacter',
            22  => 'handleCharactersTicketsList',
            23  => 'handleMessagesList',
            24  => 'handleSendMessage',
            25  => 'handleMessageThread',
            26  => 'handleSendMessageReply',
            27  => 'handleDeleteMessage',
            28  => 'handleRedeemGiftCode',
            29  => 'handleShopList',
            30  => 'handleShopPurchase',
            31  => 'handleLauncherFaqList',
            32  => 'handleVoteSitesList',
            33  => 'handleVote',
            34  => 'handleLadderboard',
            35  => 'handleRealmStatus',
            36  => 'handleEmulatorID',
            37  => 'handleCharactersMarketList',
            38  => 'handleCharactersMarketSell',
            39  => 'handleCharactersMarketCancel',
            40  => 'handleCharactersMarketBuy',
            41  => 'handleAccountBanStatus',
            42  => 'handleOnlinePlayers',
            43  => 'handleGiftPreview',
            44  => 'handleCharactersMarketBid',
            45  => 'handleCharactersMarketBidsWon',
            46  => 'handleDiscordRPCOnlineCharacter',
            100 => 'handleRegistrationTOS',
            101 => 'handleDiscordServerInfo',
            102 => 'isBattlenetEnabled',
            103 => 'handleSliderImages',
            104 => 'isBattlePayCreditsAsDP',
            105 => 'handleSendIssueToDiscord',
            106 => 'handleListGameFiles',
            107 => 'handleListAddonsFiles',
            108 => 'handleRealmListAddress',
            109 => 'handleListLauncherUpdateFiles',
            110 => 'handlePagesEnabled',
            111 => 'handleCharactersMarketSettings',
            112 => 'handleAccountIsActive',
            'testdbconnections' => 'testdbconnections',
            'debugwebserverinfo' => 'debugwebserverinfo',
        ];

        if (isset($caseFunctions[$VAR['execute']]))
        {
            $functionName = $caseFunctions[$VAR['execute']];
            $this->$functionName($VAR);
        }
        elseif ($VAR['execute'] == 999)
        {

        }
    }

    private function handleStandardAuth($VAR)
    {
        $auth = new AUTH($this->config, $this->database);
        echo $auth->Standard_Login_Challenge($VAR['vendetta_who'], $VAR['vendetta_token'], $VAR['rasar_key'], $VAR['rasar_iv']);
    }

    private function handleBattlenetAuth($VAR)
    {
        $auth = new AUTH($this->config, $this->database);
        echo $auth->Battlenet_Login_Challenge($VAR['battlenet_who'], $VAR['battlenet_token']);
    }

    private function handleRegistration($VAR)
    {
        $auth = new AUTH($this->config, $this->database);
        echo $auth->Create_Standard_Account($VAR['username'], $VAR['email'], $VAR['password1'], $VAR['password2']);
    }

    private function handleRegistrationTOS($VAR)
    {
        $tos_file_path = "docs/registration_terms_of_service.txt";

        $tos_file = fopen($tos_file_path, "r") or die("Unable to open file!");

        $tos = fread($tos_file, filesize($tos_file_path));

        fclose($tos_file);

        $json = (object)
        [
            'text' => $tos,
        ];

        echo json_encode($json, JSON_PRETTY_PRINT);;
    }

    private function handleAuthTokenData($VAR)
    {
        $launcher = new LAUNCHER($this->config, $this->database);
        echo $launcher->Token_Challenge($VAR['token'], $VAR['md5username']);
    }

    private function handleUpdateAvatarUrl($VAR)
    {
        sleep(1);
        $launcher = new LAUNCHER($this->config, $this->database);
        echo $launcher->Update_AvatarUrl($VAR['token'], $VAR['md5username'], $VAR['image_url']);
    }

    private function handleUpdatePublicNickname($VAR)
    {
        sleep(1);
        $launcher = new LAUNCHER($this->config, $this->database);
        echo $launcher->Update_Public_Profile_Nickname($VAR['token'], $VAR['md5username'], $VAR['nickname']);
    }

    private function handleGetPublicNickname($VAR)
    {
        $launcher = new LAUNCHER($this->config, $this->database);
        echo $launcher->Get_User_Public_Nickname_By_ID($VAR['token'], $VAR['md5username']);
    }

    private function handleSliderImages()
    {
        $launcher = new LAUNCHER($this->config, $this->database);
        echo $launcher->Get_Slider_Images();
    }

    private function handleWebsiteArticles($VAR)
    {
        if ($this->config['pages']['news']['page_enabled'])
        {
            $cms = new CMS($this->config, $this->database);
            echo $cms->Get_Articles_List($VAR['token'], $VAR['md5username']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleLauncherEvents($VAR)
    {
        if ($this->config['pages']['events']['page_enabled'])
        {
            $launcher = new LAUNCHER($this->config, $this->database);
            echo $launcher->Get_Launcher_Events_List($VAR['token'], $VAR['md5username']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleWebsiteChangelogs($VAR)
    {
        if ($this->config['pages']['changelogs']['page_enabled'])
        {
            $cms = new CMS($this->config, $this->database);
            echo $cms->Get_Changelog_List($VAR['token'], $VAR['md5username']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleLauncherFaqList($VAR)
    {
        if ($this->config['pages']['faq']['page_enabled'])
        {
            $launcher = new LAUNCHER($this->config, $this->database);
            echo $launcher->Get_Launcher_Faq_List($VAR['token'], $VAR['md5username']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleShopList($VAR)
    {
        if ($this->config['pages']['shop']['page_enabled'])
        {
            $shopper = new SHOPPER($this->config, $this->database);
            echo $shopper->Get_Launcher_Shop_List($VAR['token'], $VAR['md5username']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleShopPurchase($VAR)
    {
        if ($this->config['pages']['shop']['page_enabled'])
        {
            $shopper = new SHOPPER($this->config, $this->database);
            echo $shopper->Purchase_Shop_Article($VAR['token'], $VAR['md5username'], $VAR['article_id']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleLoginRewards($VAR)
    {
        if ($this->config['pages']['login_rewards']['page_enabled'])
        {
            $login_rewards = new LOGIN_REWARDS($this->config, $this->database);
            echo $login_rewards->Get_Login_Rewards_List($VAR['token'], $VAR['md5username']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleClaimLoginReward($VAR)
    {
        if ($this->config['pages']['login_rewards']['page_enabled'])
        {
            $login_rewards = new LOGIN_REWARDS($this->config, $this->database);
            echo $login_rewards->Claim_Login_Reward($VAR['token'], $VAR['md5username'], $VAR['month'], $VAR['day']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleAccountInventory($VAR)
    {
        if ($this->config['pages']['account_inventory']['page_enabled'])
        {
            $account_inventory = new ACCOUNT_INVENTORY($this->config, $this->database, 0);
            echo $account_inventory->Get_Account_Inventory_List($VAR['token'], $VAR['md5username']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleMessagesList($VAR)
    {
        if ($this->config['pages']['private_messages']['page_enabled'])
        {
            $private_messages = new PRIVATE_MESSAGES($this->config, $this->database);
            echo $private_messages->Get_Launcher_Private_Messages($VAR['token'], $VAR['md5username']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleSendMessage($VAR)
    {
        if ($this->config['pages']['private_messages']['page_enabled'])
        {
            $private_messages = new PRIVATE_MESSAGES($this->config, $this->database);
            echo $private_messages->Send_New_Launcher_Private_Message($VAR['token'], $VAR['md5username'], $VAR['receiver'], $VAR['title'], $VAR['message']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleSendMessageReply($VAR)
    {
        if ($this->config['pages']['private_messages']['page_enabled'])
        {
            $private_messages = new PRIVATE_MESSAGES($this->config, $this->database);
            echo $private_messages->Send_New_Launcher_Private_Message_Reply($VAR['token'], $VAR['md5username'], $VAR['message_id'], $VAR['message']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleDeleteMessage($VAR)
    {
        if ($this->config['pages']['private_messages']['page_enabled'])
        {
            $private_messages = new PRIVATE_MESSAGES($this->config, $this->database);
            echo $private_messages->Delete_Launcher_Private_Message_For($VAR['token'], $VAR['md5username'], $VAR['message_id']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleRedeemGiftCode($VAR)
    {
        sleep(1);
        $gift_codes = new GIFT_CODES($this->config, $this->database);
        echo $gift_codes->Redeem_Gift_Code($VAR['token'], $VAR['md5username'], $VAR['gift_code']);
    }

    private function handleGiftPreview($VAR)
    {
        sleep(1);
        $gift_codes = new GIFT_CODES($this->config, $this->database);
        echo $gift_codes->Get_Gift_Preview($VAR['token'], $VAR['md5username'], $VAR['gift_code']);
    }

    private function handleMessageThread($VAR)
    {
        if ($this->config['pages']['private_messages']['page_enabled'])
        {
            $private_messages = new PRIVATE_MESSAGES($this->config, $this->database);
            echo $private_messages->Get_Launcher_Private_Message_Thread($VAR['token'], $VAR['md5username'], $VAR['id']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleVoteSitesList($VAR)
    {
        if ($this->config['pages']['vote']['page_enabled'])
        {
            $voter = new VOTER($this->config, $this->database);
            echo $voter->Get_Vote_Sites($VAR['token'], $VAR['md5username']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleVote($VAR)
    {
        if ($this->config['pages']['vote']['page_enabled'])
        {
            $voter = new VOTER($this->config, $this->database);
            echo $voter->Vote_For_Site_ID($VAR['token'], $VAR['md5username'], $VAR['site_id']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleAccountCharactersList($VAR)
    {
        sleep(1);

        $characters = []; // Initialize an empty array to store characters
    
        foreach ($this->database['realms'] as $realmId => $realmData)
        {
            $char = new CHAR($this->config, $this->database, $realmId);
            $characters = array_merge($characters, json_decode($char->Get_Account_Characters_List($VAR['token'], $VAR['md5username'])));
        }

        echo json_encode($characters, JSON_PRETTY_PRINT);
    }

    private function handleUnstuckCharacter($VAR)
    {
        sleep(1);
        $unstucker = new UNSTUCKER($this->config, $this->database, $VAR['realm_id']);
        echo $unstucker->Unstuck_Character($VAR['token'], $VAR['md5username'], $VAR['guid']);
    }

    private function handleTeleportList()
    {
        $launcher = new LAUNCHER($this->config, $this->database);
        echo $launcher->Get_Launcher_Teleport_List();
    }

    private function handleTeleportCharacter($VAR)
    {
        sleep(1);
        $teleporter = new TELEPORTER($this->config, $this->database, $VAR['realm_id']);
        echo $teleporter->Teleport_Character($VAR['token'], $VAR['md5username'], $VAR['guid'], $VAR['teleport_id']);
    }

    private function handleCharactersTicketsList($VAR)
    {
        sleep(1);

        $tickets = []; // Initialize an empty array to store tickets
    
        foreach ($this->database['realms'] as $realmId => $realmData)
        {
            $char = new CHAR($this->config, $this->database, $realmId);
            $tickets = array_merge($tickets, json_decode($char->Get_Account_Characters_Tickets_List($VAR['token'], $VAR['md5username'])));
        }

        echo json_encode($tickets, JSON_PRETTY_PRINT);
    }

    private function handleLadderboard($VAR)
    {
        if ($this->config['pages']['ladderboard']['page_enabled'])
        {
            sleep(1);
    
            $ladderboard = []; // Initialize an empty array to store ladderboard
        
            foreach ($this->database['realms'] as $realmId => $realmData)
            {
                $char = new CHAR($this->config, $this->database, $realmId);
                $ladderboard = array_merge($ladderboard, json_decode($char->Get_Ladderboard($VAR['token'], $VAR['md5username'])));
            }
    
            echo json_encode($ladderboard, JSON_PRETTY_PRINT);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleOnlinePlayers($VAR)
    {
        if ($this->config['pages']['online_players']['page_enabled'])
        {
            sleep(1);

            $main_array = [];
    
            $online_stats = [ 'total_players' => 0 ];
            $online_players = [];
        
            foreach ($this->database['realms'] as $realmId => $realmData)
            {
                $char = new CHAR($this->config, $this->database, $realmId);
                $online_players = array_merge($online_players, json_decode($char->Get_Online_Players_List($VAR['token'], $VAR['md5username'])));
                $online_stats['total_players'] += $char->Get_Online_Players_Count($VAR['token'], $VAR['md5username']);
            }

            $main_array = array_merge($main_array, $online_stats);
            $main_array['players'] = $online_players;
    
            echo json_encode($main_array, JSON_PRETTY_PRINT);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleUseInventoryItem($VAR)
    {
        if ($this->config['pages']['account_inventory']['page_enabled'])
        {
            $account_inventory = new ACCOUNT_INVENTORY($this->config, $this->database, $VAR['realm_id']);
            echo $account_inventory->Use_Inventory_Item($VAR['token'], $VAR['md5username'], $VAR['guid'], $VAR['reward_id']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleCharactersMarketList($VAR)
    {
        if ($this->config['pages']['characters_market']['page_enabled'])
        {
            $market = new CHARACTERS_MARKET($this->config, $this->database);
            echo $market->Get_Market_List($VAR['token'], $VAR['md5username']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleCharactersMarketSell($VAR)
    {
        if ($this->config['pages']['characters_market']['page_enabled'])
        {
            $market = new CHARACTERS_MARKET($this->config, $this->database);
            echo $market->Add_To_Market($VAR['token'], $VAR['md5username'], $VAR['character_guid'], $VAR['realm_id'], $VAR['duration'], $VAR['allow_bidding'], $VAR['price']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleCharactersMarketCancel($VAR)
    {
        if ($this->config['pages']['characters_market']['page_enabled'])
        {
            $market = new CHARACTERS_MARKET($this->config, $this->database);
            echo $market->Cancel_Sale($VAR['token'], $VAR['md5username'], $VAR['sale_id']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleCharactersMarketBuy($VAR)
    {
        if ($this->config['pages']['characters_market']['page_enabled'])
        {
            $market = new CHARACTERS_MARKET($this->config, $this->database);
            echo $market->Buy_Sale($VAR['token'], $VAR['md5username'], $VAR['sale_id']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleCharactersMarketBid($VAR)
    {
        if ($this->config['pages']['characters_market']['page_enabled'])
        {
            $market = new CHARACTERS_MARKET($this->config, $this->database);
            echo $market->Bid_Sale($VAR['token'], $VAR['md5username'], $VAR['sale_id'], $VAR['bid_amount']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleCharactersMarketBidsWon($VAR)
    {
        if ($this->config['pages']['characters_market']['page_enabled'])
        {
            $market = new CHARACTERS_MARKET($this->config, $this->database);
            echo $market->Get_Market_Notifications($VAR['token'], $VAR['md5username']);
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleDiscordRPCOnlineCharacter($VAR)
    {
        $discord_rpc = new DISCORD_RPC($this->config, $this->database);
        echo $discord_rpc->Get_Online_Character($VAR['token'], $VAR['md5username']);
    }

    private function handleAccountBanStatus($VAR)
    {
        $auth = new AUTH($this->config, $this->database);
        echo $auth->Get_Account_Ban_Status($VAR['token'], $VAR['md5username']);
    }

    private function handleAccountIsActive($VAR)
    {
        $auth = new AUTH($this->config, $this->database);
        echo $auth->Get_Account_Active_Status($VAR['token'], $VAR['md5username']);
    }

    private function handleRealmStatus()
    {
        sleep(1);

        $realms = []; // Initialize an empty array to store realms
    
        foreach ($this->database['realms'] as $realmId => $realmData)
        {
            $launcher = new LAUNCHER($this->config, $this->database, $realmId);
            $realms = array_merge($realms, json_decode($launcher->GetRealmStatus($realmId)));
        }

        echo json_encode($realms, JSON_PRETTY_PRINT);
    }

    private function handleEmulatorID()
    {
        $json = new \stdClass();
        $json->emulator_id = $this->database['auth']['emulator_id'];
        echo json_encode($json, JSON_PRETTY_PRINT);
    }

    private function handlePasswordRecovery($VAR)
    {
        $launcher = new LAUNCHER($this->config, $this->database);
        $launcher->Send_Password_Recovery_Code($VAR['email']);
    }

    private function handleChangePasswordByCode($VAR)
    {
        sleep(1);
        $launcher = new LAUNCHER($this->config, $this->database);
        echo $launcher->Change_Account_Password_Using_Reset_Code($VAR['username'], $VAR['recovery_code'], $VAR['password1'], $VAR['password2']);
    }

    private function handleAccountCurrencies($VAR)
    {
        sleep(3);
        $auth = new AUTH($this->config, $this->database);
        echo $auth->Get_Account_Currencies($VAR['token'], $VAR['md5username']);
    }

    private function handleDiscordServerInfo()
    {
        if ($this->config['pages']['discord']['page_enabled'])
        {
            sleep(1);
            $discord_lib = new DISCORD_LIB($this->config, $this->discord, $this->database);
            echo $discord_lib->GetServerInfo();
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function handleSendIssueToDiscord($VAR)
    {
        sleep(1);
        $discord_lib = new DISCORD_LIB($this->config, $this->discord, $this->database);
        $discord_lib->SendReport($VAR['username'], $VAR['message']);
    }

    private function handleListGameFiles()
    {
        $updater = new DOWNLOADS($this->config);
        echo $updater->ListGameFiles();
    }

    private function handleListLauncherUpdateFiles()
    {
        $updater = new DOWNLOADS($this->config);
        echo $updater->ListLauncherUpdateFiles();
    }

    private function handleListAddonsFiles()
    {
        if ($this->config['pages']['addons']['page_enabled'])
        {
            sleep(1);
            $updater = new DOWNLOADS($this->config);
            echo $updater->ListAddons();
        }
        else
        {
            $json = new \stdClass();
            $json->Message = "Feature is currently locked, please come back later..";
            echo json_encode($json, JSON_PRETTY_PRINT);
        }
    }

    private function isBattlenetEnabled()
    {
        echo json_encode(['BattlenetEnabled' => $this->database['auth']['enable_battlenet']], JSON_PRETTY_PRINT);
    }

    private function handleRealmListAddress()
    {
        echo json_encode(['realmlist_address' => $this->config['realmlist']], JSON_PRETTY_PRINT);
    }

    private function isBattlePayCreditsAsDP()
    {
        echo json_encode(['BattlePayCreditsAsDP' => $this->database['auth']['battlepay_credits_as_dp']], JSON_PRETTY_PRINT);
    }

    private function handlePagesEnabled()
    {
        echo json_encode($this->config['pages'], JSON_PRETTY_PRINT);
    }

    private function handleCharactersMarketSettings()
    {
        echo json_encode($this->config['characters_market'], JSON_PRETTY_PRINT);
    }

    private function testdbconnections()
    {
        $auth = new AUTH($this->config, $this->database);
        echo $auth->TEST_DB_CONNECTION();

        echo "\r\n";
        echo "\r\n";

        foreach ($this->database['realms'] as $realmId => $realmData)
        {
            $char = new CHAR($this->config, $this->database, $realmId);
            echo $char->TEST_DB_CONNECTION();
        }

        echo "\r\n";
        echo "\r\n";

        $cms = new CMS($this->config, $this->database);
        echo $cms->TEST_DB_CONNECTION();

        echo "\r\n";
        echo "\r\n";

        $launcher = new LAUNCHER($this->config, $this->database);
        echo $launcher->TEST_DB_CONNECTION();
    }

    private function debugwebserverinfo()
    {
        header('Content-Type: html; charset=UTF-8');
        phpinfo();
    }
}
