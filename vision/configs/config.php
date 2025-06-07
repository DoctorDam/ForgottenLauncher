<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

$config['realmlist'] = "logon.forgottenland.fr";

$config['website_url'] = "http://forgottenland.fr"; // do NOT add "/" at the end and make sure your url starts with correct protocol http or https !!

$config['language'] = "english"; // See /engine/languages available folders

$config['smtp'] = 
[
    'host'              => 'smtp.gmail.com',
    'username'          => 'your smtp email/username',
    'password'          => 'your smtp password',
    'port'              => '465',
    'from_name'         => 'account recovery',
    'reply_to_email'    => 'no-reply@yourserver.com',
    'reply_to_name'     => 'no-reply',
];

$config['pages'] =
[
    'account_inventory' => [ 'button_visible' => false, 'page_enabled' => false ],
    'private_messages'  => [ 'button_visible' => true, 'page_enabled' => true ],
    'login_rewards'     => [ 'button_visible' => true, 'page_enabled' => false ],
    'news'              => [ 'button_visible' => true, 'page_enabled' => true ],
    'events'            => [ 'button_visible' => false, 'page_enabled' => false ],
    'discord'           => [ 'button_visible' => true, 'page_enabled' => true ],
    'changelogs'        => [ 'button_visible' => true, 'page_enabled' => false ],
    'shop'              => [ 'button_visible' => true, 'page_enabled' => false ],
    'characters_market' => [ 'button_visible' => false, 'page_enabled' => false ],
    'addons'            => [ 'button_visible' => false, 'page_enabled' => false ],
    'vote'              => [ 'button_visible' => true, 'page_enabled' => true ],
    'online_players'    => [ 'button_visible' => true, 'page_enabled' => true, 'max_player_rows' => 250 ],
    'ladderboard'       => [ 'button_visible' => false, 'page_enabled' => false ],
    'faq'               => [ 'button_visible' => false, 'page_enabled' => false ],
];

$config['characters_market'] = 
[
    'minimum_price'         => 20,
    'maximum_price'         => 10000,
    'commission_percent'    => 10,
    'hours_durations'       => [ 12, 24, 48 ], // 12h, 24h, 48h, etc..
    'minimum_player_level'  => 1,
    'minimum_player_gold'   => 0,
    'armory_url'            => 'https://forgottenland.fr/armory',
];

$config['display_ranks'] = 
[
    'names' => 
    [
        0 => 'Player',
        1 => 'Beta Tester',
        2 => 'Game Master',
        3 => 'Administrator',
        4 => 'Owner',
    ],
    'colors' => 
    [
        0 => 'FFFFFF',
        1 => '00FF9B',
        2 => '00FF0C',
        3 => 'FF3535',
        4 => '00E0FF',
    ],
];

$config['access_token'] = 
[
    'valid_time'        => 2592000,
];

$config['rate_limiter'] =
[
    1 /*RATE_LIMITER_LOGIN*/ =>
    [
        'max_attempts'      => 10,
        'wait_time'         => 900, // seconds
    ],

    2 /*RATE_LIMITER_REGISTER*/ =>
    [
        'max_attempts'      => 1,
        'wait_time'         => 300, // seconds
    ],

    3 /*RATE_LIMITER_PASSWORD_RECOVERY*/ =>
    [
        'max_attempts'      => 1,
        'wait_time'         => 120, // seconds
    ],

    4 /*RATE_LIMITER_PASSWORD_RESET*/ =>
    [
        'max_attempts'      => 1,
        'wait_time'         => 10, // seconds
        'valid_time'        => 120, // seconds
    ],

    5 /*RATE_LIMITER_DISCORD_INFO*/ =>
    [
        'max_attempts'      => 3,
        'wait_time'         => 120, // seconds
    ],

    6 /*RATE_LIMITER_ACCOUNT_INFO*/ =>
    [
        'max_attempts'      => 5,
        'wait_time'         => 30, // seconds
    ],

    7 /*RATE_LIMITER_DISCORD_INGAME_RPC*/ =>
    [
        'max_attempts'      => 1,
        'wait_time'         => 20, // seconds
    ],  
];