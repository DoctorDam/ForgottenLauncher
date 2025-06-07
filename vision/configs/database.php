<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

/* DATABASE SETTINGS */
$database['auth'] = 
[
    'mysql_hostname'            => 'logon.forgotteland.fr',
    'mysql_port'                => '3306',
    'mysql_user'                => 'cherix',
    'mysql_pass'                => 'papours17',
    'mysql_database'            => 'update_auth',
    'emulator_id'               => '14', // See /engine/definitions.php
    'enable_battlenet'          => false,
    'battlepay_credits_as_dp'   => false,
    'holds_vp_dp'               => false, // Used by LEGIONCORE_735_SHA emulator id
];

$database['launcher'] = 
[
    'mysql_hostname'            => 'logon.forgotteland.fr',
    'mysql_port'                => '3306',
    'mysql_user'                => 'cherix',
    'mysql_pass'                => 'papours17',
    'mysql_database'            => 'launcher',
];

$database['cms'] = 
[
    'mysql_hostname'            => 'logon.forgotteland.fr',
    'mysql_port'                => '3306',
    'mysql_user'                => 'cherix',
    'mysql_pass'                => 'papours17',
    'mysql_database'            => 'fusiongen2',
    'cms_id'                    => '1', // See /engine/definitions.php
];

$database['realms'] = 
[
    1 => [ // realm id must be correct
        'realm_name'            => 'Forgotten Land',
        'mysql_hostname'        => 'logon.forgotteland.fr',
        'mysql_port'            => '3306',
        'mysql_user'            => 'cherix',
        'mysql_pass'            => 'papours17',
        'mysql_database'        => 'upadte_charecters',
        'soap' => 
        [ // SOAP SETTINGS
            'hostaddress'       => 'logon.forgotteland.fr',
            'port'              => '7878',
            'account'           => 'website', // must be minimum gm level 3
            'password'          => 'website',
        ], 
        // REALM SETTINGS
        'realmlist'             => 'logon.forgotteland.fr',
        'realm_port'            => '8085',
    ],
];