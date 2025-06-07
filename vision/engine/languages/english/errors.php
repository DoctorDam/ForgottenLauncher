<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

return
[
    "not_authorized"                        => "Not authorized to do that.",
    "invalid_login_info"                    => "We could not verify that account information..",
    "account_is_banned"                     => "Your account is banned..",
    "invalid_token"                         => "Invalid or expired token, please login again..",
    "attempts_cooldown"                     => "Too many attempts, please try again later..",
    "profile_id_taken"                      => "Profile id already used, chose other..",
    "invalid_reset_code"                    => "Invalid or expired reset code..",
    "password_not_changed"                  => "Password was NOT changed!",
    "invalid_receiver"                      => "Receiver name doesn't exist!",
    "message_title_criteria"                => "Title must be >= 3 and <= 50 length or\r\n message must be >= 10 and <= 2500 length.",
    "message_text_criteria"                 => "Message must be >= 10 and <= 2500 length.",
    "not_enough_currency"                   => "Not enough vp or dp/bpc points!",
    "already_claimed_reward"                => "Already claimed this reward!",
    "already_redeemed_gift"                 => "Already redeemed this gift code!",
    "invalid_gift_code"                     => "Invalid or expired gift code..",
    "error_gift_no_reward_defined"          => "The gift code entered is valid but doesn't reward anything",
    "error_gift_min_gm_rank"                => "Your gm rank is lower than the minimum allowed",
    "error_gift_max_gm_rank"                => "Your gm rank is higher than the maximum allowed",
    "error_gift_exact_gm_rank"              => "Your gm rank is not equal with the required rank",
    "vote_cooldown"                         => "You are on vote cooldown..",
    "registration_failed"                   => "Registration failed due to unknown reasons..",
    "password_change_failed"                => "Password change failed due to unknown reasons..",
    "username_empty"                        => "Username can't be empty!",
    "email_empty"                           => "Email address can't be empty!",
    "password_empty"                        => "Password can't be empty!",
    "invalid_email_format"                  => "The entered email is not a valid format!",
    "username_min_length"                   => "Username must be at least 6 characters long!",
    "password_min_length"                   => "Password must be at least 6 characters long!",
    "passwords_no_match"                    => "Passwords didn't match!",
    "username_taken"                        => "Username is already taken!",
    "email_taken"                           => "Email is already taken!",
    "username_no_exist"                     => "Username entered doesn't exist!",
    "character_require_online"              => "Character must be online!",
    "character_require_offline"             => "Character must be offline!",
    "not_character_owner"                   => "You don't own that character!",
    "teleport_destination_forbidden"        => "Teleport destination forbidden for your faction!",
    "reward_id_not_found"                   => "Reward id doesn't exist",
    "soap_unauthorized"                     => "Unauthorized request.",
    "characters_market_exists"              => "Character exists on sale.",
    "characters_market_missing"             => "Character not found on sale.",
    "characters_market_buyself"             => "Can't buy own characters.",
    "characters_market_min_price"           => "Minimum sale price must be",
    "characters_market_max_price"           => "Maximum sale price must be less than",
    "characters_market_min_level"           => "Requires minimum player level",
    "characters_market_min_gold"            => "Requires minimum owned gold",
    "characters_market_invalid_duration"    => "Duration selected is invalid!",
    "characters_market_cancel_fail"         => "Failed to cancel the market sale!",
    "characters_market_buy_fail"            => "Failed to buy this character from the market!",
    "characters_market_bid_fail"            => "Failed to bid this sale!",
    "characters_market_bid_fail_not_enough" => "Failed to bid due to higher bid amount: {0}\r\nPlease try again..",
    "character_is_banned"                   => "Character is banned!",
    "character_account_id_change_fail"      => "Character ownership change had no effect, no database rows affected!",
    "characters_market_insert_fail"         => "Character market adding had no effect, no database rows affected!",
];
