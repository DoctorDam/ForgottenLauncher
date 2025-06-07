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
    "not_authorized"                        => "No está autorizado para hacer eso.",
    "invalid_login_info"                    => "No pudimos verificar esa información de la cuenta.",
    "account_is_banned"                     => "Tu cuenta está prohibida.",
    "invalid_token"                         => "Token inválido o expirado, por favor inicie sesión nuevamente.",
    "attempts_cooldown"                     => "Demasiados intentos, por favor intente nuevamente más tarde.",
    "profile_id_taken"                      => "El ID de perfil ya está en uso, elija otro.",
    "invalid_reset_code"                    => "Código de restablecimiento inválido o expirado.",
    "password_not_changed"                  => "¡La contraseña NO fue cambiada!",
    "invalid_receiver"                      => "¡El nombre del receptor no existe!",
    "message_title_criteria"                => "El título debe tener una longitud >= 3 y <= 50 o\r\n el mensaje debe tener una longitud >= 10 y <= 2500.",
    "message_text_criteria"                 => "El mensaje debe tener una longitud >= 10 y <= 2500.",
    "not_enough_currency"                   => "¡No hay suficientes puntos de VP o DP/BPC!",
    "already_claimed_reward"                => "¡Ya has reclamado esta recompensa!",
    "already_redeemed_gift"                 => "¡Ya has canjeado este código de regalo!",
    "invalid_gift_code"                     => "Código de regalo inválido o expirado.",
    "error_gift_no_reward_defined"          => "El código de regalo introducido es válido pero no otorga ninguna recompensa",
    "error_gift_min_gm_rank"                => "Tu rango de GM es menor que el mínimo permitido",
    "error_gift_max_gm_rank"                => "Tu rango de GM es mayor que el máximo permitido",
    "error_gift_exact_gm_rank"              => "Tu rango de GM no es igual al rango requerido",
    "vote_cooldown"                         => "Estás en enfriamiento de voto.",
    "registration_failed"                   => "El registro falló debido a razones desconocidas.",
    "password_change_failed"                => "El cambio de contraseña falló debido a razones desconocidas.",
    "username_empty"                        => "¡El nombre de usuario no puede estar vacío!",
    "email_empty"                           => "¡La dirección de correo electrónico no puede estar vacía!",
    "password_empty"                        => "¡La contraseña no puede estar vacía!",
    "invalid_email_format"                  => "¡El correo electrónico ingresado no tiene un formato válido!",
    "username_min_length"                   => "¡El nombre de usuario debe tener al menos 6 caracteres!",
    "password_min_length"                   => "¡La contraseña debe tener al menos 6 caracteres!",
    "passwords_no_match"                    => "¡Las contraseñas no coinciden!",
    "username_taken"                        => "¡El nombre de usuario ya está en uso!",
    "email_taken"                           => "¡El correo electrónico ya está en uso!",
    "username_no_exist"                     => "¡El nombre de usuario ingresado no existe!",
    "character_require_online"              => "¡El personaje debe estar en línea!",
    "character_require_offline"             => "¡El personaje debe estar fuera de línea!",
    "not_character_owner"                   => "¡No eres el dueño de ese personaje!",
    "teleport_destination_forbidden"        => "¡Destino de teletransporte prohibido para tu facción!",
    "reward_id_not_found"                   => "El ID de recompensa no existe",
    "soap_unauthorized"                     => "Solicitud no autorizada.",
    "characters_market_exists"              => "El personaje existe en venta.",
    "characters_market_missing"             => "Personaje no encontrado en venta.",
    "characters_market_buyself"             => "No se pueden comprar personajes propios.",
    "characters_market_min_price"           => "El precio de venta mínimo debe ser",
    "characters_market_max_price"           => "El precio de venta máximo debe ser menor que",
    "characters_market_min_level"           => "Se requiere un nivel mínimo de jugador",
    "characters_market_min_gold"            => "Se requiere un mínimo de oro poseído",
    "characters_market_invalid_duration"    => "¡Duración seleccionada es inválida!",
    "characters_market_cancel_fail"         => "¡Falló al cancelar la venta en el mercado!",
    "characters_market_buy_fail"            => "¡Falló al comprar este personaje en el mercado!",
    "characters_market_bid_fail"            => "¡Error al ofertar esta venta!",
    "characters_market_bid_fail_not_enough" => "Error al ofertar debido a una cantidad de oferta más alta: {0}\r\nPor favor, inténtelo de nuevo..",
    "character_is_banned"                   => "¡El personaje está baneado!",
    "character_account_id_change_fail"      => "El cambio de propiedad del personaje no tuvo efecto, ¡ninguna fila de la base de datos se vio afectada!",
    "characters_market_insert_fail"         => "La adición del personaje al mercado no tuvo efecto, ¡ninguna fila de la base de datos se vio afectada!",
];
