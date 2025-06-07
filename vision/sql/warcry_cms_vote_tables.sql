CREATE TABLE `vote_sites` (
	`id` INT(10) NOT NULL AUTO_INCREMENT,
	`vote_sitename` VARCHAR(50) NULL DEFAULT 'WarCry' COLLATE 'utf8mb4_unicode_ci',
	`vote_url` VARCHAR(255) NULL DEFAULT 'http://' COLLATE 'utf8mb4_unicode_ci',
	`vote_image` VARCHAR(255) NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',
	`hour_interval` INT(10) NOT NULL DEFAULT '12',
	`points_per_vote` TINYINT(3) NOT NULL DEFAULT '1',
	`callback_enabled` INT(10) NOT NULL DEFAULT '0',
	PRIMARY KEY (`id`) USING BTREE
) COLLATE='utf8mb4_unicode_ci' ENGINE=InnoDB ROW_FORMAT=DYNAMIC;