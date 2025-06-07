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
    "not_authorized"                        => "没有权限",
    "invalid_login_info"                    => "无法核实账户信息",
    "account_is_banned"                     => "您的帐户被禁用..",
    "invalid_token"                         => "验证无效或过期, 请重新登陆..",
    "attempts_cooldown"                     => "尝试次数过多，请稍后再试..",
    "profile_id_taken"                      => "个人资料 ID 已被占用，请选择其他..",
    "invalid_reset_code"                    => "重置密码无效或过期..",
    "password_not_changed"                  => "密码未更改!",
    "invalid_receiver"                      => "收件人名称不存在!",
    "message_title_criteria"                => "标题长度必须大于3字节小于50字节\r\n 内容长度必须大于10字节小2500字节.",
    "message_text_criteria"                 => "信息长度必须大于10字节小于2500字节.",
    "not_enough_currency"                   => "vp/dp/bpc点数不足!",
    "already_claimed_reward"                => "已领取此奖励!",
    "already_redeemed_gift"                 => "已兑换此礼品代码!",
    "invalid_gift_code"                     => "无效或过期的礼品代码..",
    "error_gift_no_reward_defined"          => "输入的礼品码有效，但没有奖励",
    "error_gift_min_gm_rank"                => "您的 GM 等级低于最小允许值",
    "error_gift_max_gm_rank"                => "您的 GM 等级高于最大允许值",
    "error_gift_exact_gm_rank"              => "您的 GM 等级与所需等级不相等",
    "vote_cooldown"                         => "您的投票时间已过..",
    "registration_failed"                   => "由于未知原因注册失败..",
    "password_change_failed"                => "由于未知原因，密码更改失败..",
    "username_empty"                        => "用户名不能为空!",
    "email_empty"                           => "电子邮件地址不能为空!",
    "password_empty"                        => "密码不能为空!",
    "invalid_email_format"                  => "输入的电子邮件格式无效!",
    "username_min_length"                   => "用户名长度必须至少为 6 个字符!",
    "password_min_length"                   => "密码长度至少为 6 个字符!",
    "passwords_no_match"                    => "密码不一致!",
    "username_taken"                        => "用户名已被注册!",
    "email_taken"                           => "电子邮件已被使用!",
    "username_no_exist"                     => "输入的用户名不存在!",
    "character_require_online"              => "角色必须在线!",
    "character_require_offline"             => "角色必须离线!",
    "not_character_owner"                   => "该角色不属于你!",
    "teleport_destination_forbidden"        => "您所在阵营禁止的传送目的地!",
    "reward_id_not_found"                   => "奖励 ID 不存在",
    "soap_unauthorized"                     => "未经授权的请求。",
    "characters_market_exists"              => "角色已在销售中。",
    "characters_market_missing"             => "未找到销售角色。",
    "characters_market_buyself"             => "无法购买自己的角色。",
    "characters_market_min_price"           => "最低销售价格必须为",
    "characters_market_max_price"           => "最高销售价格必须低于",
    "characters_market_min_level"           => "需要最低玩家等级",
    "characters_market_min_gold"            => "需要最低拥有的金币",
    "characters_market_invalid_duration"    => "所选的持续时间无效！",
    "characters_market_cancel_fail"         => "取消市场销售失败！",
    "characters_market_buy_fail"            => "从市场购买此角色失败！",
    "characters_market_bid_fail"            => "竞标此销售失败！",
    "characters_market_bid_fail_not_enough" => "由于较高的出价金额而失败：{0}\r\n请重试..",
    "character_is_banned"                   => "角色被禁止！",
    "character_account_id_change_fail"      => "角色所有权更改未生效，数据库中没有受影响的行！",
    "characters_market_insert_fail"         => "角色市场添加未生效，数据库中没有受影响的行！",
];
