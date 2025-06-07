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
    "not_authorized"                        => "Non autorisé à faire cela.",
    "invalid_login_info"                    => "Nous n'avons pas pu vérifier ces informations de compte.",
    "account_is_banned"                     => "Votre compte est banni.",
    "invalid_token"                         => "Jeton invalide ou expiré, veuillez vous reconnecter.",
    "attempts_cooldown"                     => "Trop de tentatives, veuillez réessayer plus tard.",
    "profile_id_taken"                      => "L'identifiant de profil est déjà utilisé, veuillez en choisir un autre.",
    "invalid_reset_code"                    => "Code de réinitialisation invalide ou expiré.",
    "password_not_changed"                  => "Le mot de passe n'a PAS été modifié!",
    "invalid_receiver"                      => "Le nom du destinataire n'existe pas!",
    "message_title_criteria"                => "Le titre doit être >= 3 et <= 50 caractères ou\r\n le message doit être >= 10 et <= 2500 caractères.",
    "message_text_criteria"                 => "Le message doit être >= 10 et <= 2500 caractères.",
    "not_enough_currency"                   => "Pas assez de points VP ou DP/BPC!",
    "already_claimed_reward"                => "Cette récompense a déjà été réclamée!",
    "already_redeemed_gift"                 => "Ce code cadeau a déjà été utilisé!",
    "invalid_gift_code"                     => "Code cadeau invalide ou expiré.",
    "error_gift_no_reward_defined"          => "Le code cadeau saisi est valide mais ne récompense rien",
    "error_gift_min_gm_rank"                => "Votre rang de GM est inférieur au minimum autorisé",
    "error_gift_max_gm_rank"                => "Votre rang de GM est supérieur au maximum autorisé",
    "error_gift_exact_gm_rank"              => "Votre rang de GM n'est pas égal au rang requis",
    "vote_cooldown"                         => "Vous êtes en période de refroidissement pour le vote.",
    "registration_failed"                   => "L'inscription a échoué pour des raisons inconnues.",
    "password_change_failed"                => "La modification du mot de passe a échoué pour des raisons inconnues.",
    "username_empty"                        => "Le nom d'utilisateur ne peut pas être vide!",
    "email_empty"                           => "L'adresse e-mail ne peut pas être vide!",
    "password_empty"                        => "Le mot de passe ne peut pas être vide!",
    "invalid_email_format"                  => "L'e-mail saisi n'est pas un format valide!",
    "username_min_length"                   => "Le nom d'utilisateur doit comporter au moins 6 caractères!",
    "password_min_length"                   => "Le mot de passe doit comporter au moins 6 caractères!",
    "passwords_no_match"                    => "Les mots de passe ne correspondent pas!",
    "username_taken"                        => "Le nom d'utilisateur est déjà pris!",
    "email_taken"                           => "L'adresse e-mail est déjà prise!",
    "username_no_exist"                     => "Le nom d'utilisateur saisi n'existe pas!",
    "character_require_online"              => "Le personnage doit être en ligne!",
    "character_require_offline"             => "Le personnage doit être hors ligne!",
    "not_character_owner"                   => "Vous ne possédez pas ce personnage!",
    "teleport_destination_forbidden"        => "La destination de téléportation est interdite pour votre faction!",
    "reward_id_not_found"                   => "L'ID de récompense n'existe pas.",
    "soap_unauthorized"                     => "Requête non autorisée.",
    "characters_market_exists"              => "Le personnage existe en vente.",
    "characters_market_missing"             => "Personnage non trouvé en vente.",
    "characters_market_buyself"             => "Impossible d'acheter vos propres personnages.",
    "characters_market_min_price"           => "Le prix de vente minimum doit être",
    "characters_market_max_price"           => "Le prix de vente maximum doit être inférieur à",
    "characters_market_min_level"           => "Niveau minimum du joueur requis",
    "characters_market_min_gold"            => "Or possédé minimum requis",
    "characters_market_invalid_duration"    => "Durée sélectionnée invalide !",
    "characters_market_cancel_fail"         => "Échec de l'annulation de la vente sur le marché !",
    "characters_market_buy_fail"            => "Échec de l'achat de ce personnage sur le marché !",
    "characters_market_bid_fail"            => "Échec de l'enchère de cette vente !",
    "characters_market_bid_fail_not_enough" => "Échec de l'enchère en raison d'un montant d'enchère plus élevé : {0}\r\nVeuillez réessayer..",
    "character_is_banned"                   => "Le personnage est banni !",
    "character_account_id_change_fail"      => "Le changement de propriétaire du personnage n'a eu aucun effet, aucune ligne de base de données affectée !",
    "characters_market_insert_fail"         => "L'ajout du personnage sur le marché n'a eu aucun effet, aucune ligne de base de données affectée !",
];
