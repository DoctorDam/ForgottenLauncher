/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP TABLE IF EXISTS `account_avatars`;
CREATE TABLE IF NOT EXISTS `account_avatars` (
  `account_id` int NOT NULL,
  `image_url` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  UNIQUE KEY `account_id` (`account_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

DROP TABLE IF EXISTS `account_data`;
CREATE TABLE IF NOT EXISTS `account_data` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `account` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
  `email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `last_ip_address` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `access_token` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `token_valid_until` int NOT NULL DEFAULT (0),
  `public_nickname` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `rasar_key` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `rasar_iv` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `account` (`account`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

DROP TABLE IF EXISTS `account_inventory`;
CREATE TABLE IF NOT EXISTS `account_inventory` (
  `account_id` int NOT NULL,
  `reward_id` int NOT NULL,
  `acquired_on` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

DROP TABLE IF EXISTS `characters_market`;
CREATE TABLE IF NOT EXISTS `characters_market` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `character_guid` int NOT NULL,
  `owner_account_id` int NOT NULL,
  `realm_id` tinyint NOT NULL DEFAULT (0),
  `allow_bidding` tinyint(1) NOT NULL DEFAULT '0',
  `price` int NOT NULL,
  `date_added` datetime NOT NULL DEFAULT (now()) ON UPDATE CURRENT_TIMESTAMP,
  `expires_on` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

DROP TABLE IF EXISTS `characters_market_bids`;
CREATE TABLE IF NOT EXISTS `characters_market_bids` (
  `id` int NOT NULL AUTO_INCREMENT,
  `buyer_id` int NOT NULL,
  `sale_id` int NOT NULL,
  `bid_amount` int NOT NULL DEFAULT '0',
  `date` datetime NOT NULL DEFAULT (now()) ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `buyer_id` (`buyer_id`,`sale_id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

DROP TABLE IF EXISTS `characters_market_bids_won`;
CREATE TABLE IF NOT EXISTS `characters_market_bids_won` (
  `id` int NOT NULL AUTO_INCREMENT,
  `winner_id` int NOT NULL,
  `character_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `character_race` int NOT NULL,
  `character_class` int NOT NULL,
  `character_level` tinyint NOT NULL,
  `character_gender` tinyint(1) NOT NULL,
  `date` datetime NOT NULL DEFAULT (now()) ON UPDATE CURRENT_TIMESTAMP,
  `notification_read` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

DROP TABLE IF EXISTS `characters_market_logs`;
CREATE TABLE IF NOT EXISTS `characters_market_logs` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `sale_id` int unsigned NOT NULL DEFAULT '0',
  `character_guid` int NOT NULL DEFAULT '0',
  `old_account_id` int NOT NULL DEFAULT '0',
  `new_account_id` int NOT NULL DEFAULT '0',
  `realm_id` tinyint NOT NULL DEFAULT (0),
  `date` datetime NOT NULL DEFAULT (now()) ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

DROP TABLE IF EXISTS `events`;
CREATE TABLE IF NOT EXISTS `events` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `picture_url` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `redirect_url` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `title` char(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `content` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `expiry_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

INSERT INTO `events` (`id`, `picture_url`, `redirect_url`, `title`, `content`, `expiry_date`) VALUES
	(1, 'https://bnetcmsus-a.akamaihd.net/cms/carousel_header/vx/VXVVDLBPA6P11699047219782.png', 'https://eu.shop.battle.net/en-us/product/world-of-warcraft-the-war-within', 'Descend into the depths of Azeroth', 'Pre-purchase² World of Warcraft: The War Within and unlock a slew of benefits to aid you in your coming adventures! All editions include an Enhanced Level 70 Character Boost and the critically-acclaimed Dragonflight expansion³. In addition, the Epic Edition also guarantees Beta Access and Early Access⁴ to The War Within. All in-game items are available immediately unless otherwise noted.', '2024-11-04 20:32:43'),
	(2, 'https://bnetcmsus-a.akamaihd.net/cms/blog_thumbnail/jo/JOPTS4Z17S0S1707173888701.png', 'https://worldofwarcraft.blizzard.com/en-us/news/24064536/ride-into-celestial-escapades-with-the-lunar-pack', 'Ride into Celestial Escapades with the Lunar Pack\r\n', 'Ride off into celestial escapades with The Lunar Pack! Get all six mounts* or fill out your Collection with the ones you have yet to add at an interstellar discount. No matter which you choose to ride astride on your adventures across Azeroth, each is prepared to endow you with good fortune, courage, joy, wit, and a plethora of luck.', '2025-12-04 20:38:33');

DROP TABLE IF EXISTS `faq`;
CREATE TABLE IF NOT EXISTS `faq` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `text` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

INSERT INTO `faq` (`id`, `title`, `text`) VALUES
	(1, 'How to clear cache manually?', 'Close your World of Warcraft game.\r\n\r\nGo to your World of Warcraft folder.\r\n\r\nFind "cache" folder then right-click delete.\r\n\r\nStart World of Warcraft game again.'),
	(2, 'Can I use my World of Warcraft Trading Card Game codes in WoW Classic?', 'Codes from the TCG won’t be usable in WoW Classic.'),
	(3, 'Will there be an Armory for WoW Classic?', 'There was no Armory for World of Warcraft until the Burning Crusade. If you wanted to know more about a player’s gear before you invited them to your party, you had to meet them in the world and physically inspect them (and you couldn’t inspect players of the opposite faction until 2.4). You couldn’t see their spec or talent builds when you inspected them, either. To maintain that same dynamic, we won’t have an Armory or profile pages for WoW Classic at launch.'),
	(4, 'Will I be able to play all the content that was available in 1.12 when WoW Classic launches?', 'The content for WoW Classic will be rolled out across six patches, to allow for increasing power progression and to better deliver an experience closer to what it felt like to play in original WoW. We haven’t yet determined exactly when phases 2-6 will occur, but we’ll keep you updated as they draw closer.'),
	(5, 'How many characters can I create in WoW Classic?', 'You’ll be able to create a maximum of 10 characters per WoW Classic realm, with a total maximum of 50 characters across all realms in your region. You’ll also be restricted to one faction (Horde or Alliance) on PvP realms. WoW Classic name reservations are available now and allow you to create up to three characters per WoW Account before WoW Classic releases.'),
	(6, 'Can I use WoW Tokens in WoW Classic?', 'You won’t be able to buy and sell WoW Tokens in WoW Classic, but you can buy and sell them in World of Warcraft and use them toward your game time.');

DROP TABLE IF EXISTS `gift_codes`;
CREATE TABLE IF NOT EXISTS `gift_codes` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `code` varchar(25) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `reward_id` int NOT NULL DEFAULT (0),
  `redeems_allowed` int NOT NULL DEFAULT '0',
  `min_gm_level_allowed` int NOT NULL DEFAULT '0',
  `max_gm_level_allowed` int NOT NULL DEFAULT '0',
  `req_exact_gm_level` int NOT NULL DEFAULT '0',
  `valid_until` datetime NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `code` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

DROP TABLE IF EXISTS `gift_redeems`;
CREATE TABLE IF NOT EXISTS `gift_redeems` (
  `user_id` int NOT NULL,
  `gift_id` int NOT NULL,
  UNIQUE KEY `user_id_gift_id` (`user_id`,`gift_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


DROP TABLE IF EXISTS `login_claimed_rewards`;
CREATE TABLE IF NOT EXISTS `login_claimed_rewards` (
  `account_id` int NOT NULL,
  `month` int NOT NULL,
  `day` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

DROP TABLE IF EXISTS `login_rewards`;
CREATE TABLE IF NOT EXISTS `login_rewards` (
  `month` tinyint NOT NULL DEFAULT '0',
  `day` tinyint NOT NULL DEFAULT '0',
  `reward_id` int NOT NULL DEFAULT '0',
  UNIQUE KEY `month` (`month`,`day`),
  KEY `FK_login_rewards_rewards` (`reward_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

INSERT INTO `login_rewards` (`month`, `day`, `reward_id`) VALUES
	(4, 1, 1),
	(4, 29, 2),
	(4, 28, 3),
	(4, 6, 4),
	(4, 27, 6),
	(4, 8, 7),
	(4, 30, 8),
	(4, 7, 9),
	(4, 26, 13),
	(4, 5, 25);

DROP TABLE IF EXISTS `messages`;
CREATE TABLE IF NOT EXISTS `messages` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `parent_id` int NOT NULL DEFAULT (0),
  `sender_id` int NOT NULL DEFAULT (0),
  `receiver_id` int NOT NULL DEFAULT (0),
  `title` char(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
  `message` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `date_edited` datetime NOT NULL DEFAULT (now()) ON UPDATE CURRENT_TIMESTAMP,
  `seen` tinyint NOT NULL DEFAULT '0',
  `date_seen` datetime DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

DROP TABLE IF EXISTS `messages_deleted`;
CREATE TABLE IF NOT EXISTS `messages_deleted` (
  `message_id` int NOT NULL,
  `for_user_id` int NOT NULL,
  UNIQUE KEY `message_id_for_user_id` (`message_id`,`for_user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

DROP TABLE IF EXISTS `password_recovery`;
CREATE TABLE IF NOT EXISTS `password_recovery` (
  `username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `reset_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ip_address` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `valid_until` int NOT NULL,
  PRIMARY KEY (`username`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci ROW_FORMAT=DYNAMIC;

DROP TABLE IF EXISTS `rate_limiter`;
CREATE TABLE IF NOT EXISTS `rate_limiter` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `ip_address` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `type` tinyint NOT NULL DEFAULT (0),
  `count` int NOT NULL DEFAULT (0),
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `ip_address` (`ip_address`,`type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

DROP TABLE IF EXISTS `rewards`;
CREATE TABLE IF NOT EXISTS `rewards` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `picture_url` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `soap_command` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `auth_db_query` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `char_db_query` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `web_db_query` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `vision_db_query` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `requires_player` tinyint(1) NOT NULL DEFAULT '0',
  `requires_input` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

INSERT INTO `rewards` (`id`, `title`, `picture_url`, `description`, `soap_command`, `auth_db_query`, `char_db_query`, `web_db_query`, `vision_db_query`, `requires_player`, `requires_input`) VALUES
	(1, 'Bulwark of Kings', 'https://th.bing.com/th/id/OIP.VUI6DRY08bZBaIwVz8trNgAAAA?pid=ImgDet&rs=1', 'Claim [Bulwark of Kings] and [Herald of Woe] for free!!', 'send items {PLAYER_NAME} "Daily Login Rewards" "Congratulations for claiming your daily rewards" 19357:1 28484:1', NULL, NULL, NULL, NULL, 1, 0),
	(2, 'Ashes of Al\'ar', 'https://th.bing.com/th/id/OIP.iHHR2aTy28UlbWOgkvzpTAHaE1?pid=ImgDet&rs=1', 'Claim [Ashes of Al\'ar] for free!!', 'send items {PLAYER_NAME} "Daily Login Rewards" "Congratulations for claiming your daily rewards" 32458:1', NULL, NULL, NULL, NULL, 1, 0),
	(3, 'Faction Change', 'https://bnetcmsus-a.akamaihd.net/cms/blog_header/f7/F775CYTPUG171578677980075.jpg', 'Claim free one-time faction change service.', 'character changefaction {PLAYER_NAME}', NULL, NULL, NULL, NULL, 1, 0),
	(4, 'Character Rename', 'https://th.bing.com/th/id/R.b5b676b683994dd0ddbfa3ff098126a0?rik=5G%2bdbJED66%2fXvA&pid=ImgRaw&r=0', 'Claim for free one-time character rename service.', 'character rename {PLAYER_NAME}', NULL, NULL, NULL, NULL, 1, 0),
	(5, 'Account GM Level 4 Reward', 'https://i.ytimg.com/vi/esSX8zPhXSY/maxresdefault.jpg', 'Updates your account gm level to 4', NULL, 'REPLACE INTO account_access (AccountID, SecurityLevel, RealmID) VALUES ({USER_ID}, 4, -1)', NULL, NULL, NULL, 0, 0),
	(6, 'Swift Flying Broom', 'https://vignette.wikia.nocookie.net/wowwiki/images/0/0e/Wroombroom.jpg/revision/latest?cb=20080319135929', 'Claim a free [Swift Flying Broom] one-time!!', 'send items {PLAYER_NAME} "Daily Login Rewards" "Congratulations for claiming your daily rewards" 33182:1', NULL, NULL, NULL, NULL, 1, 0),
	(7, 'Badges of Justice', 'https://wowvendor.com/app/uploads/2021/05/buy-badges-of-justice-farm-boost-carry-service.jpg', 'Claim 100x [Badge of Justice] for free!', 'send items {PLAYER_NAME} "Daily Login Rewards" "Congratulations for claiming your daily rewards" 29434:100', NULL, NULL, NULL, NULL, 1, 0),
	(8, 'Warglaive of Azzinoth', 'https://th.bing.com/th/id/OIP.SGqJ8NQKzvnwEWHx1JRWaQHaEq?rs=1&pid=ImgDetMain', 'Warglaive of Azzinoth Main-Hand', 'send items {PLAYER_NAME} "Daily Login Rewards" "Congratulations for claiming your daily rewards" 32838:1', NULL, NULL, NULL, NULL, 1, 0),
	(9, 'Bulwark of Azzinoth', 'https://th.bing.com/th/id/OIP.F4osUnHRqGuOIFSXIT6dCwHaGA?pid=ImgDet&rs=1', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec vitae sem nisi. Nulla congue mollis posuere. In maximus scelerisque nunc porta porttitor. Etiam pulvinar est orci, non tempus mi elementum sit amet. Ut bibendum sollicitudin sapien non hendrerit. Cras nec gravida erat, sed hendrerit metus. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Nam mollis eros ante, vitae auctor sapien feugiat ac. Phasellus at suscipit risus. Nunc mollis sagittis mi id scelerisque. Phasellus finibus vel arcu eget pellentesque. Etiam ut tincidunt nisl. Sed ultricies justo vel dolor aliquet auctor. Vestibulum eget orci risus.', 'send items {PLAYER_NAME} "Daily Login Rewards" "Congratulations for claiming your daily rewards" 32375:1', NULL, NULL, NULL, NULL, 1, 0),
	(10, '100x Perpetual Purple Firework', 'https://wotlk.evowow.com/static/images/wow/icons/large/inv_misc_missilesmall_purple.jpg', 'Shoots a firework into the air that bursts into a thousand purple stars. (30 seconds cooldown)', 'send items {PLAYER_NAME} "Daily Login Rewards" "Congratulations for claiming your daily rewards" 49703:100', NULL, NULL, NULL, NULL, 1, 0),
	(11, '10x Red Streaks Firework', 'https://wotlk.evowow.com/static/images/wow/icons/large/spell_fire_flare.jpg', 'Shoots a firework into the air that bursts into red streaks.', 'send items {PLAYER_NAME} "Daily Login Rewards" "Congratulations for claiming your daily rewards" 5740:10', NULL, NULL, NULL, NULL, 1, 0),
	(12, '10x Red, White and Blue Firework', 'https://wotlk.evowow.com/static/images/wow/icons/large/spell_holy_mindvision.jpg', 'Shoots a firework into the air that bursts into red, white and blue stars.', 'send items {PLAYER_NAME} "Daily Login Rewards" "Congratulations for claiming your daily rewards" 9317:10', NULL, NULL, NULL, NULL, 1, 0),
	(13, 'Screaming Torchfiend\'s Brutality', 'https://th.bing.com/th?id=OIF.Hv8N3%2bg0sjTQOHVBnI707w&rs=1&pid=ImgDetMain', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus vitae faucibus massa. Suspendisse vel dolor lacus. Maecenas et ex eu elit sagittis auctor et sed orci. Maecenas eleifend sagittis quam, molestie auctor massa imperdiet in. Maecenas pulvinar ante quis enim ullamcorper tempus. Pellentesque rhoncus posuere sapien, vitae lobortis metus sollicitudin ut. Vivamus lobortis porttitor odio sit amet ornare. Mauris vitae lacinia leo, vel pharetra purus. Curabitur sed interdum velit. Quisque suscipit est nec pharetra laoreet. Nam quis feugiat elit. Etiam sem sem, viverra iaculis tincidunt et, posuere vel nibh. Fusce sed erat tincidunt, convallis lacus sed, sollicitudin ante. In at tortor mi.', 'send items {PLAYER_NAME} "Shop Purchase" "Congratulations for your purchase" 207261:1 207262:1 207263:1 207264:1 207266:1', NULL, NULL, NULL, NULL, 1, 0),
	(14, 'Werynkeeper\'s Timeless Vigil', 'https://wow.zamimg.com/modelviewer/live/webthumbs/item-set/1/1/24/1560.webp', 'Nam volutpat dolor eu hendrerit pretium. Pellentesque arcu urna, fringilla nec urna non, aliquet consectetur est. Mauris et odio eleifend, cursus lorem eget, ultrices dui. Aenean velit velit, cursus et magna et, lacinia bibendum augue. Integer urna nibh, finibus nec massa et, pretium tempus tellus. Vestibulum tristique, leo ac mollis scelerisque, augue urna iaculis diam, vitae pulvinar enim urna nec nisl. Nulla sit amet egestas sem. Sed vitae tristique sapien. Maecenas aliquet et ipsum ut mattis. Suspendisse dictum pretium molestie. Curabitur felis nulla, porttitor vel efficitur ut, consectetur id risus. Phasellus at consequat lectus, sed malesuada eros. Nulla scelerisque tempus augue quis maximus. Mauris et sapien malesuada, faucibus nunc eget, condimentum libero. Ut imperdiet sed ipsum eget euismod.', 'send items {PLAYER_NAME} "Shop Purchase" "Congratulations for your purchase" 207225:1 207226:1 207227:1 207228:1 207240:1', NULL, NULL, NULL, NULL, 1, 0),
	(15, 'Haunted Frostbrood Remains', 'https://wow.zamimg.com/modelviewer/live/webthumbs/item-set/1/1/246/1526.webp', 'Sed vel rutrum purus. Nunc nisl nibh, tincidunt eu est sit amet, placerat interdum elit. Nulla id dolor non justo elementum aliquet non eget justo. Integer justo nulla, molestie quis blandit ac, fringilla vitae ligula. Nullam non elit at nibh porta efficitur. Phasellus velit turpis, lacinia id scelerisque et, bibendum eu sapien. Nunc dapibus bibendum placerat. Maecenas ipsum diam, tristique at nunc vitae, tempor tempus justo. Suspendisse potenti. Ut at tristique eros. Nam at augue eu nisl ultrices placerat eget in nisl.', 'send items {PLAYER_NAME} "Shop Purchase" "Congratulations for your purchase" 207225:1 207226:1 207227:1 207228:1 207240:1', NULL, NULL, NULL, NULL, 1, 0),
	(16, 'Fyr\'alath the Dreamrender', 'https://wow.zamimg.com/uploads/screenshots/normal/1135186-fyralath-the-dreamrender-fyralath-pose.jpg', 'Mauris justo lectus, tempor pretium hendrerit at, vehicula non nibh. Duis feugiat rhoncus malesuada. Vestibulum ut turpis dapibus, scelerisque nibh tincidunt, egestas lacus. Vivamus convallis justo nec fermentum euismod. Suspendisse a felis nec sapien hendrerit volutpat vitae dictum eros. Donec ornare sed risus ut elementum. Vivamus elit nulla, congue lobortis tempor at, porta ut nisl. Vivamus non erat augue. Integer vehicula felis erat, et lobortis nisi consequat at. Nam eget sem a tellus venenatis porttitor. Proin consectetur, ex nec dapibus scelerisque, augue ante pharetra enim, vel commodo dolor lorem sed diam. Maecenas ut iaculis augue, vitae molestie mauris. Donec egestas, lorem sed aliquet imperdiet, dolor odio dapibus enim, ac tristique felis lectus at ex.', 'send items {PLAYER_NAME} "Shop Purchase" "Congratulations for your purchase" 206448:1', NULL, NULL, NULL, NULL, 1, 0),
	(17, 'Thunderfury, Blessed Blade of the Windseeker', 'https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/5b4509be-2966-42a6-acb7-ddb9c80b2ba4/dd13bli-67ea24f4-2cb1-4849-a942-84fef153f034.jpg/v1/fill/w_1024,h_1024,q_70,strp/thunderfury__blessed_blade_of_the_windseeker_by_streltsoff_dd13bli-fullview.jpg', 'Duis vitae enim aliquam, tempor mi quis, gravida lorem. Donec dapibus elit sed placerat faucibus. Integer euismod risus sed tellus suscipit ultricies. Morbi et sagittis magna. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Cras a imperdiet mauris. Aenean congue tortor quis leo porttitor semper. Fusce at pellentesque nibh. Duis a magna eu justo varius mollis. Donec lobortis, dui in lobortis malesuada, quam libero molestie tortor, ut euismod lacus risus in lacus.', 'send items {PLAYER_NAME} "Shop Purchase" "Congratulations for your purchase" 19019:1', NULL, NULL, NULL, NULL, 1, 0),
	(18, 'Dragonwrath, Tarecgosa\'s Rest', 'https://wow.zamimg.com/uploads/screenshots/normal/246599-dragonwrath-tarecgosas-rest.jpg', 'Quisque non pretium nibh. Integer euismod et nunc sit amet feugiat. Maecenas non bibendum est, eu vestibulum nisl. Fusce dapibus ac tellus commodo lacinia. Sed luctus interdum dui non volutpat. Curabitur lacinia tincidunt pharetra. Pellentesque a bibendum lectus. Quisque porttitor scelerisque laoreet. Vestibulum sapien nisi, vehicula nec sapien nec, tincidunt venenatis arcu.', 'send items {PLAYER_NAME} "Shop Purchase" "Congratulations for your purchase" 71086:1', NULL, NULL, NULL, NULL, 1, 0),
	(19, 'Warglaive of Azzinoth - Main Hand', 'https://wow.zamimg.com/uploads/screenshots/normal/53756-warglaive-of-azzinoth.jpg', 'Integer dui mi, pharetra nec nisi rhoncus, pellentesque maximus odio. Donec eu leo lorem. Vestibulum quis bibendum arcu. Suspendisse eget sem dictum, volutpat tortor eget, tincidunt lorem. Praesent nec pulvinar magna. Nunc ante dolor, ullamcorper vitae viverra non, accumsan eget velit. Quisque et vehicula tellus, vestibulum scelerisque risus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Morbi volutpat ex a viverra vulputate.', 'send items {PLAYER_NAME} "Shop Purchase" "Congratulations for your purchase" 32837:1', NULL, NULL, NULL, NULL, 1, 0),
	(20, 'Shadowmourne', 'https://vignette2.wikia.nocookie.net/wowwiki/images/2/27/Shadowmourne.jpg/revision/latest?cb=20090822180728', 'Pellentesque non sagittis orci, sed cursus lacus. Sed ut orci tristique, maximus tortor et, vulputate lectus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. In hac habitasse platea dictumst. Donec cursus, magna at lacinia placerat, nisl purus commodo magna, et porttitor tortor massa eget nibh. Donec luctus libero massa, et maximus massa tincidunt et. Duis eget lacinia ante, posuere varius nibh. Pellentesque ac porta felis. Aenean ut commodo nulla. Mauris ultrices blandit justo sed dignissim. Cras molestie aliquam varius. Morbi tempor congue ligula, eget interdum dolor efficitur vitae. Etiam in massa vitae risus maximus facilisis et eu lacus. Integer non dapibus nulla, non pretium enim. Cras hendrerit interdum turpis.', 'send items {PLAYER_NAME} "Shop Purchase" "Congratulations for your purchase" 49623:1', NULL, NULL, NULL, NULL, 1, 0),
	(21, 'Lingering Echo of Tarecgosa', 'https://th.bing.com/th/id/R.7160099b49bb2581497a5e7f87106504?rik=7qT1yBYzlFWMnQ&riu=http%3a%2f%2f2.bp.blogspot.com%2f-CeC32gsFG08%2fUyCDCQrs_PI%2fAAAAAAAAAiA%2fT2YhpCKsYmw%2fs1600%2fBraeleiAzureDrake20140309.jpg&ehk=%2fiy1jgowXMHgQRviFwIoC3pT7eCccLO5vRkaCwUXweA%3d&risl=&pid=ImgRaw&r=0', 'Donec at laoreet turpis, at hendrerit justo. Donec quam mauris, consectetur malesuada mauris eu, luctus interdum nisi. Praesent tempus elit ut erat lobortis, ut tempor nisl dictum. Nullam facilisis interdum fermentum. Donec congue ex sed nisi dictum suscipit. Aenean bibendum ex a neque scelerisque fermentum. Donec semper placerat tellus, et cursus felis condimentum at. Mauris efficitur sed libero vitae tincidunt. Mauris vehicula id risus quis dictum. Maecenas condimentum metus ac pulvinar dictum. Maecenas non est sed nunc finibus imperdiet a vel orci. Suspendisse et risus purus.', 'send items {PLAYER_NAME} "Shop Purchase" "Congratulations for your purchase" 206162:1', NULL, NULL, NULL, NULL, 1, 0),
	(22, 'Azure Worldchiller', 'https://buyboost.com/data/products/2929/Azure_Worldchiller1.webp', 'Vivamus bibendum risus et justo semper iaculis. Integer pulvinar eros ultrices erat posuere dapibus quis quis massa. Nulla posuere lorem urna, vitae hendrerit tortor pulvinar et. Nunc egestas mi vitae lobortis interdum. Nulla interdum lectus non nisl molestie, eget commodo dolor fringilla. Pellentesque est ligula, condimentum a massa eu, elementum egestas est. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi laoreet ornare dui, in dignissim ex.', 'send items {PLAYER_NAME} "Shop Purchase" "Congratulations for your purchase" 208572:1', NULL, NULL, NULL, NULL, 1, 0),
	(23, 'Ashes of Al\'ar', 'https://wow.zamimg.com/uploads/screenshots/normal/858108-ashes-of-alar-after-years-of-trying-i-finally-got-this-mount.jpg', 'Proin tempus massa at velit commodo, vel pellentesque sapien placerat. Nam et eros ultrices, sagittis nunc vel, mattis nibh. Phasellus lorem ipsum, pretium id porttitor vel, tempor vel leo. Nullam id ipsum leo. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aliquam sagittis mauris felis, sed venenatis sapien pellentesque at. Suspendisse a interdum metus. Sed urna leo, egestas at efficitur a, fermentum sed turpis. In hac habitasse platea dictumst. In hac habitasse platea dictumst. Integer lacinia a ex sit amet tempor. Suspendisse cursus elit in finibus vestibulum.', 'send items {PLAYER_NAME} "Shop Purchase" "Congratulations for your purchase" 32458:1', NULL, NULL, NULL, NULL, 1, 0),
	(24, 'Grotto Netherwing Drake', 'https://wow.zamimg.com/uploads/screenshots/normal/1113587-grotto-netherwing-drake.jpg', 'Nullam metus ante, aliquet eget pulvinar et, ultricies nec lectus. Nulla cursus fringilla felis, rutrum iaculis ante lobortis at. Praesent dui sapien, ornare non vulputate sed, aliquet quis dui. Proin sed elit sed metus consectetur pulvinar sit amet sit amet leo. Praesent a quam mattis urna luctus tincidunt. Etiam eu mi ac magna iaculis interdum sit amet vel mauris. Vestibulum rhoncus neque varius nisi pretium, et ultricies metus ultricies. Mauris facilisis mi nisl, faucibus vulputate massa facilisis eu. Ut egestas, arcu nec rhoncus tempus, velit nibh dignissim neque, ut ullamcorper tellus dui et nisi. Etiam pellentesque maximus feugiat.', 'send items {PLAYER_NAME} "Shop Purchase" "Congratulations for your purchase" 206156:1', NULL, NULL, NULL, NULL, 1, 0),
	(25, '4x "Gigantique" Bag', 'https://wow.zamimg.com/uploads/guide/seo/8015.jpg?1557462595', 'Curabitur ullamcorper suscipit ante nec molestie. Suspendisse laoreet rutrum nibh, dapibus consectetur mi auctor ut. Maecenas fringilla consequat molestie. Vestibulum eu egestas erat. Suspendisse finibus purus vitae dui tempus, a elementum ligula posuere. Aenean lobortis mattis suscipit. Praesent posuere ornare neque sed maximus. Donec efficitur sem id ante efficitur blandit. Cras at pharetra quam. Quisque a auctor neque. Sed sed nibh metus. In blandit, orci vitae placerat vulputate, mi leo venenatis augue, sed iaculis eros lacus ac dui. Donec sollicitudin urna in facilisis mollis. Nulla volutpat sed quam maximus accumsan. Duis interdum a lacus malesuada semper.', 'send items {PLAYER_NAME} "Shop Purchase" "Congratulations for your purchase" 38082:4', NULL, NULL, NULL, NULL, 1, 0);

DROP TABLE IF EXISTS `shop`;
CREATE TABLE IF NOT EXISTS `shop` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `category` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT 'Uncategorized',
  `reward_id` int NOT NULL DEFAULT '0',
  `dp_or_bpc_price` int NOT NULL DEFAULT '0',
  `vp_price` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

INSERT INTO `shop` (`id`, `category`, `reward_id`, `dp_or_bpc_price`, `vp_price`) VALUES
	(1, 'Armor Sets', 13, 100, 0),
	(2, 'Armor Sets', 14, 100, 0),
	(3, 'Armor Sets', 15, 100, 0),
	(4, 'Weapons', 16, 250, 0),
	(5, 'Weapons', 17, 0, 30),
	(6, 'Weapons', 18, 50, 0),
	(7, 'Weapons', 19, 50, 0),
	(8, 'Weapons', 20, 50, 0),
	(9, 'Mounts', 21, 150, 0),
	(10, 'Mounts', 22, 150, 0),
	(11, 'Mounts', 23, 150, 0),
	(12, 'Mounts', 24, 150, 0),
	(13, 'Containers', 25, 1000, 0);

DROP TABLE IF EXISTS `slider`;
CREATE TABLE IF NOT EXISTS `slider` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `image_url` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `title` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `redirect_url` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

INSERT INTO `slider` (`id`, `image_url`, `title`, `description`, `redirect_url`) VALUES
	(1, 'https://bnetcmsus-a.akamaihd.net/cms/blog_thumbnail/ll/LL0673UQ5OQK1699029727486.png', 'Cataclysm Classic Beta Now Live', 'Cataclysm Classic is now in closed beta. It\'s not too late to reignite the fire and help usher in a new Azeroth when you sign up to join the World of Warcraft: Cataclysm Classic™ beta before it hits the servers in 2024.', 'https://worldofwarcraft.blizzard.com/en-us/news/24073137/cataclysm-classic-beta-now-live'),
	(2, 'https://bnetcmsus-a.akamaihd.net/cms/blog_thumbnail/tr/TRIZHKNS9P0C1554180046164.jpg', 'The Darkmoon Faire Returns', 'The Darkmoon Faire celebrates the wondrous, exotic, and mysterious from around Azeroth! This mist-shrouded island is a conundrum wrapped in an enigma, accessible for one week only at the beginning of each month.', 'https://worldofwarcraft.blizzard.com/en-us/news/23785338/the-darkmoon-faire-returns'),
	(3, 'https://bnetcmsus-a.akamaihd.net/cms/blog_thumbnail/hq/HQQJ3RPRMP111709073896222.png', 'Seize Two Sets of the Dreadlord\'s Regalia!', 'Beware the deep shadows, for the dreadlords are the masters of dark sorcery, preferring to manipulate and undermine their enemies from the darkest places. The Unseen are cruel and cunning schemers, adept in illusion and disguise. Now, you can clothe yourself in their raiment when you acquire two color variants in one pack.', 'https://worldofwarcraft.blizzard.com/en-us/news/24064544/seize-two-sets-of-the-dreadlords-regalia'),
	(4, 'https://bnetcmsus-a.akamaihd.net/cms/blog_thumbnail/1c/1C6ZMMEW9YKV1709078385844.png', 'Herald in the Heavens Astride Ash\'Adar, Harbinger of the Dawn', 'Herald in the heavens astride the ethereal, color-shifting Ash\'adar, Harbinger of the Dawn* now available for a limited time** through the in-game shop.', 'https://worldofwarcraft.blizzard.com/en-us/news/24064543/herald-in-the-heavens-astride-ashadar-harbinger-of-the-dawn');

DROP TABLE IF EXISTS `soap_logs`;
CREATE TABLE IF NOT EXISTS `soap_logs` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `username` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT 'Unknown',
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `realm_id` int NOT NULL DEFAULT '0',
  `command` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci ROW_FORMAT=DYNAMIC;

DROP TABLE IF EXISTS `teleport_list`;
CREATE TABLE IF NOT EXISTS `teleport_list` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `map` int NOT NULL DEFAULT (0),
  `dest_x` float NOT NULL,
  `dest_y` float NOT NULL,
  `dest_z` float NOT NULL,
  `orientation` float NOT NULL,
  `dp_or_bpc_price` int NOT NULL DEFAULT (0),
  `vp_price` int NOT NULL DEFAULT (0),
  `alliance_allowed` tinyint NOT NULL DEFAULT (0),
  `horde_allowed` tinyint NOT NULL DEFAULT (0),
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

INSERT INTO `teleport_list` (`id`, `name`, `map`, `dest_x`, `dest_y`, `dest_z`, `orientation`, `dp_or_bpc_price`, `vp_price`, `alliance_allowed`, `horde_allowed`) VALUES
	(1, 'Orgrimmar', 1, 1283.54, -4395.35, 26.2977, 0.293688, 0, 5, 0, 1),
	(2, 'Silvermoon', 530, 9319.67, -7276.46, 13.2359, 6.25117, 0, 5, 0, 1),
	(3, 'Undercity', 0, 1913.4, 235.633, 51.5175, 3.06872, 0, 5, 0, 1),
	(4, 'Thunderbluff', 1, -1304.15, 205.728, 68.6814, 5.09316, 0, 5, 0, 1),
	(5, 'Stormwind', 0, -9175.68, 340.18, 83.9863, 0.751192, 0, 5, 1, 0),
	(6, 'Ironforge', 0, -5048.04, -777.14, 493.71, 5.06219, 0, 5, 1, 0),
	(7, 'Exodar', 530, -4071.93, -12013.8, -1.27883, 1.30768, 0, 5, 1, 0),
	(8, 'Darnassus', 1, 9984.75, 1952.17, 1325.59, 1.58802, 0, 5, 1, 0),
	(9, 'Shattrath', 530, -1847.6, 5407.51, -12.428, 2.18807, 1, 0, 1, 1),
	(10, 'Dalaran', 571, 5804.15, 624.771, 647.767, 1.164, 1, 0, 1, 1),
	(11, 'Booty Bay', 0, -14297.2, 530.993, 8.77916, 3.98863, 0, 5, 1, 1),
	(12, 'Ratchet', 1, -986.219, -3802.88, 5.31985, 4.14216, 0, 5, 1, 1);

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
