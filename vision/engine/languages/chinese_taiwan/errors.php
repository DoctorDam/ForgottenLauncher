<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com
Translated by kuramajssken

*/

return
[
    "not_authorized"                        => "沒有權限",
    "invalid_login_info"                    => "無法核實賬戶信息",
    "account_is_banned"                     => "您的帳戶被禁用..",
    "invalid_token"                         => "驗證無效或過期, 請重新登陸..",
    "attempts_cooldown"                     => "嘗試次數過多，請稍後再試..",
    "profile_id_taken"                      => "個人資料 ID 已被佔用，請選擇其他..",
    "invalid_reset_code"                    => "重置密碼無效或過期..",
    "password_not_changed"                  => "密碼未更改!",
    "invalid_receiver"                      => "收件人名稱不存在!",
    "message_title_criteria"                => "標題長度必須大於3字節小於50字節\r\n 內容長度必須大於10字節小2500字節.",
    "message_text_criteria"                 => "信息長度必須大於10字節小於2500字節.",
    "not_enough_currency"                   => "vp/dp/bpc點數不足!",
    "already_claimed_reward"                => "已領取此獎勵!",
    "already_redeemed_gift"                 => "已兌換此禮品代碼!",
    "invalid_gift_code"                     => "無效或過期的禮品代碼..",
    "error_gift_no_reward_defined"          => "輸入的禮品碼有效，但沒有獎勵",
    "error_gift_min_gm_rank"                => "您的 GM 等級低於最小允許值",
    "error_gift_max_gm_rank"                => "您的 GM 等級高於最大允許值",
    "error_gift_exact_gm_rank"              => "您的 GM 等級與所需等級不相等",
    "vote_cooldown"                         => "您的投票時間已過..",
    "registration_failed"                   => "由於未知原因註冊失敗..",
    "password_change_failed"                => "由於未知原因，密碼更改失敗..",
    "username_empty"                        => "用戶名不能為空!",
    "email_empty"                           => "電子郵件地址不能為空!",
    "password_empty"                        => "密碼不能為空!",
    "invalid_email_format"                  => "輸入的電子郵件格式無效!",
    "username_min_length"                   => "用戶名長度必須至少為 6 個字符!",
    "password_min_length"                   => "密碼長度至少為 6 個字符!",
    "passwords_no_match"                    => "密碼不一致!",
    "username_taken"                        => "用戶名已被註冊!",
    "email_taken"                           => "電子郵件已被使用!",
    "username_no_exist"                     => "輸入的用戶名不存在!",
    "character_require_online"              => "角色必須在線!",
    "character_require_offline"             => "角色必須離線!",
    "not_character_owner"                   => "該角色不屬於妳!",
    "teleport_destination_forbidden"        => "您所在陣營禁止的傳送目的地!",
    "reward_id_not_found"                   => "獎勵 ID 不存在",
    "soap_unauthorized"                     => "未授權的請求。",
    "characters_market_exists"              => "角色已在銷售中。",
    "characters_market_missing"             => "未找到販售角色。",
    "characters_market_buyself"             => "無法購買自己的角色。",
    "characters_market_min_price"           => "最低販售價格必須為",
    "characters_market_max_price"           => "最高販售價格必須低於",
    "characters_market_min_level"           => "需要最低玩家等級",
    "characters_market_min_gold"            => "需要最低擁有的金幣",
    "characters_market_invalid_duration"    => "所選的持續時間無效！",
    "characters_market_cancel_fail"         => "取消市場銷售失敗！",
    "characters_market_buy_fail"            => "從市場購買此角色失敗！",
    "characters_market_bid_fail"            => "競標此銷售失敗！",
    "characters_market_bid_fail_not_enough" => "由於較高的出價金額而失敗：{0}\r\n請重試..",
    "character_is_banned"                   => "角色已被禁止！",
    "character_account_id_change_fail"      => "角色所有權更改無效，資料庫中沒有受影響的行！",
    "characters_market_insert_fail"         => "角色市場添加無效，資料庫中沒有受影響的行！",
];