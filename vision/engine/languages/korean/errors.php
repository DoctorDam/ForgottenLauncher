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
    "not_authorized"                        => "그렇게 할 권한이 없습니다.",
    "invalid_login_info"                    => "그 계정 정보를 확인할 수 없었습니다..",
    "account_is_banned"                     => "계정이 금지되어 있습니다..",
    "invalid_token"                         => "잘못되거나 만료된 토큰입니다. 다시 로그인하십시오..",
    "attempts_cooldown"                     => "너무 많은 시도를 하였습니다. 나중에 다시 시도해주세요..",
    "profile_id_taken"                      => "프로필 ID가 이미 사용중입니다. 다른걸 선택해주세요..",
    "invalid_reset_code"                    => "잘못되거나 만료된 재설정 코드입니다..",
    "password_not_changed"                  => "비밀번호가 변경되지 않았습니다!",
    "invalid_receiver"                      => "수신자 이름이 없습니다!",
    "message_title_criteria"                => "제목 길이는 3 ~ 50 이어야하고 또는 메시지는 10 ~ 2500 길이여야 합니다.",
    "message_text_criteria"                 => "메시지는 10 ~ 2500 길이여야 합니다.",
    "not_enough_currency"                   => "vp 또는 dp/bpc 포인트가 부족합니다!",
    "already_claimed_reward"                => "이미 이 보상을 요구했습니다!",
    "already_redeemed_gift"                 => "이 선물 코드를 이미 환불했습니다!",
    "invalid_gift_code"                     => "잘못되거나 만료된 선물 코드입니다..",
    "error_gift_no_reward_defined"          => "입력한 선물 코드는 유효하지만 보상이 없습니다",
    "error_gift_min_gm_rank"                => "당신의 GM 등급이 허용되는 최소값보다 낮습니다",
    "error_gift_max_gm_rank"                => "당신의 GM 등급이 최대 허용치보다 높습니다",
    "error_gift_exact_gm_rank"              => "GM 등급이 필요한 등급과 동일하지 않습니다",
    "vote_cooldown"                         => "당신은 투표를 하고 있습니다..",
    "registration_failed"                   => "알 수 없는 이유로 등록에 실패했습니다..",
    "password_change_failed"                => "알 수 없는 이유로 인해 암호 변경에 실패했습니다..",
    "username_empty"                        => "사용자 이름은 비워 둘 수 없습니다!",
    "email_empty"                           => "이메일 주소는 비워둘 수 없습니다!",
    "password_empty"                        => "비밀번호는 비워둘 수 없습니다!",
    "invalid_email_format"                  => "입력한 이메일은 올바른 형식이 아닙니다!",
    "username_min_length"                   => "사용자 이름의 길이는 6자 이상이어야 합니다!",
    "password_min_length"                   => "비밀번호는 6자 이상이어야 합니다!",
    "passwords_no_match"                    => "비밀번호가 일치하지 않았습니다!",
    "username_taken"                        => "사용자 이름이 이미 사용되었습니다!",
    "email_taken"                           => "이메일은 이미 쓰고있습니다!",
    "username_no_exist"                     => "입력한 사용자 이름이 없습니다!",
    "character_require_online"              => "캐릭터가 온라인 상태여야 합니다!",
    "character_require_offline"             => "캐릭터가 오프라인 상태여야 합니다!",
    "not_character_owner"                   => "당신은 그 캐릭터를 소유하고 있지 않습니다!",
    "teleport_destination_forbidden"        => "선택한 목적지는 진영 문제로 금지합니다!",
    "reward_id_not_found"                   => "보상 ID가 존재하지 않습니다.",
    "soap_unauthorized"                     => "비정상적인 요청입니다.",
    "characters_market_exists"              => "캐릭터가 판매중입니다.",
    "characters_market_missing"             => "판매중인 캐릭터를 찾을 수 없습니다.",
    "characters_market_buyself"             => "자신의 캐릭터를 살 수 없습니다.",
    "characters_market_min_price"           => "최소한의 가격은...",
    "characters_market_max_price"           => "최대 판매 가격은 이 가격보다 적어야합니다. ",
    "characters_market_min_level"           => "최소 플레이어 레벨 필요합니다.",
    "characters_market_min_gold"            => "최소 소유금이 필요합니다.",
    "characters_market_invalid_duration"    => "선택한 기간이 잘못되었습니다!",
    "characters_market_cancel_fail"         => "시중판매 취소 실패!",
    "characters_market_buy_fail"            => "시장에서 이 캐릭터를 구입하지 못했습니다!",
    "characters_market_bid_fail"            => "이 매물을 입찰하지 못했습니다!",
    "characters_market_bid_fail_not_enough" => "입찰금액이 높아져 입찰에 실패하였습니다: {0}\r\n다시 시도해 주세요..",
    "character_is_banned"                   => "캐릭터가 밴되어있습니다!",
    "character_account_id_change_fail"      => "문자 소유권 변경은 영향을 미치지 않았고, 데이터베이스 행은 영향을 받지 않았습니다!",
    "characters_market_insert_fail"         => "캐릭터 마켓 추가는 효과가 없었고, 데이터베이스 행은 영향을 받지 않았습니다!",
];
