<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

// HOLDS EMULATORS IDS
define('TRINITYCORE_WOTLK',         1);
define('TRINITYCORE_CATA',          2);
define('TRINITYCORE_DRAGONFLIGHTS', 3);
define('MANGOS_ONE',                4);
define('MANGOS_TWO',                5);
define('MANGOS_THREE',              6);
define('MANGOS_FOUR',               7);
define('MANGOS_FIVE',               8);
define('CMANGOS_CLASSIC',           9);
define('CMANGOS_TBC',               10);
define('CMANGOS_WOTLK',             11);
define('CMANGOS_CATA',              12);
define('VMANGOS_CLASSIC',           13);
define('AZEROTHCORE_WOTLK',         14);
define('SKYFIRE_MOP',               15);
define('ASHMANE_SHADOWLANDS',       16);
define('ATLANTISS_CATA',            17);
define('LEGIONCORE_735_SHA',        18);
define('TRINITYCORE_WOTLK_2016',    19);

// HOLDS CMS IDS
define('CMS_FUSION_GEN',        1);
define('CMS_BLIZZCMS',          2);
define('CMS_ACORE_CMS',         3);
define('CMS_WARCRY',            4);
define('CMS_FUSION_WOW_CMS',    5);

// DO NOT EDIT THESE, WE ARE NOT RESPONSIBLE FOR ACTIONS
define('Lang_Error', include_once("languages/".$config['language']."/errors.php"));
define('Lang_Success', include_once("languages/".$config['language']."/success.php"));

define('RATE_LIMITER_LOGIN',                1);
define('RATE_LIMITER_REGISTER',             2);
define('RATE_LIMITER_PASSWORD_RECOVERY',    3);
define('RATE_LIMITER_PASSWORD_RESET',       4);
define('RATE_LIMITER_DISCORD_INFO',         5);
define('RATE_LIMITER_ACCOUNT_INFO',         6);
define('RATE_LIMITER_DISCORD_INGAME_RPC',   7);