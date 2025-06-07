/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System.Collections.Generic;
using System.Reflection;

namespace Forgotten_Land_Launcher.Library
{
    internal class WoW_Definitions
    {
        #region ENTRIES

        public enum PlayerRace
        {
            Human               = 1,
            Orc                 = 2,
            Dwarf               = 3,
            Nightelf            = 4,
            Undead              = 5,
            Tauren              = 6,
            Gnome               = 7,
            Troll               = 8,
            Goblin              = 9,
            Bloodelf            = 10,
            Draenei             = 11,
            Worgen              = 22,
            Pandaren            = 24,
            Nightborne          = 27,
            HighmountainTauren  = 28,
            Voidelf             = 29,
            LightforgedDraenei  = 30,
            ZandalariTroll      = 31,
            KulTiran            = 32,
            DarkIronDwarf       = 34,
            Vulpera             = 35,
            MagharOrc           = 36,
            MechaGnome          = 37,
            Dracthyr            = 52,
            Dracthyr2           = 70
        }

        public enum PlayerClass
        {
            Warrior         = 1,
            Paladin         = 2,
            Hunter          = 3,
            Rogue           = 4,
            Priest          = 5,
            DeathKnight     = 6,
            Shaman          = 7,
            Mage            = 8,
            Warlock         = 9,
            Monk            = 10,
            Druid           = 11,
            DemonHunter     = 12,
            Evoker          = 13
        }

        public enum Gender
        {
            Male,
            Female
        }

        public enum Profession
        {
            FirstAid        = 129,
            Blacksmithing   = 164,
            Leatherworking  = 165,
            Alchemy         = 171,
            Herbalism       = 182,
            Cooking         = 185,
            Mining          = 186,
            Tailoring       = 197,
            Engineering     = 202,
            Enchanting      = 333,
            Fishing         = 356,
            Skinning        = 393,
            Jewelcrafting   = 755,
            Inscription     = 773,
        }

        #endregion

        #region NAMES AND COLORS

        private static readonly Dictionary<PlayerClass, string> PlayerClassNames = new Dictionary<PlayerClass, string>
        {
            { PlayerClass.Warrior,     "Warrior" },
            { PlayerClass.Paladin,     "Paladin" },
            { PlayerClass.Hunter,      "Hunter" },
            { PlayerClass.Rogue,       "Rogue" },
            { PlayerClass.Priest,      "Priest" },
            { PlayerClass.DeathKnight, "DeathKnight" },
            { PlayerClass.Shaman,      "Shaman" },
            { PlayerClass.Mage,        "Mage" },
            { PlayerClass.Warlock,     "Warlock" },
            { PlayerClass.Monk,        "Monk" },
            { PlayerClass.Druid,       "Druid" },
            { PlayerClass.DemonHunter, "DemonHunter" },
            { PlayerClass.Evoker,      "Evoker" }
        };

        private static readonly Dictionary<PlayerClass, string> PlayerClassHexColors = new Dictionary<PlayerClass, string>
        {
            { PlayerClass.Warrior,     "c69b6d" },
            { PlayerClass.Paladin,     "f48cba" },
            { PlayerClass.Hunter,      "aad372" },
            { PlayerClass.Rogue,       "fff468" },
            { PlayerClass.Priest,      "ffffff" },
            { PlayerClass.DeathKnight, "c41e3a" },
            { PlayerClass.Shaman,      "0070dd" },
            { PlayerClass.Mage,        "3fc7eb" },
            { PlayerClass.Warlock,     "8788ee" },
            { PlayerClass.Monk,        "00ff98" },
            { PlayerClass.Druid,       "ff7c0a" },
            { PlayerClass.DemonHunter, "a330c9" },
            { PlayerClass.Evoker,      "33937f" }
        };

        private static readonly Dictionary<PlayerRace, string> PlayerRaceNames = new Dictionary<PlayerRace, string>
        {
            { PlayerRace.Human,                 "Human" },
            { PlayerRace.Orc,                   "Orc" },
            { PlayerRace.Dwarf,                 "Dwarf" },
            { PlayerRace.Nightelf,              "Nightelf" },
            { PlayerRace.Undead,                "Undead" },
            { PlayerRace.Tauren,                "Tauren" },
            { PlayerRace.Gnome,                 "Gnome" },
            { PlayerRace.Troll,                 "Troll" },
            { PlayerRace.Goblin,                "Goblin" },
            { PlayerRace.Bloodelf,              "Bloodelf" },
            { PlayerRace.Draenei,               "Draenei" },
            { PlayerRace.Worgen,                "Worgen" },
            { PlayerRace.Pandaren,              "Pandaren" },
            { PlayerRace.Nightborne,            "Nightborne" },
            { PlayerRace.HighmountainTauren,    "Highmountain Tauren" },
            { PlayerRace.Voidelf,               "Voidelf" },
            { PlayerRace.LightforgedDraenei,    "Lightforged Draenei" },
            { PlayerRace.ZandalariTroll,        "Zandalari Troll" },
            { PlayerRace.KulTiran,              "Kul Tiran" },
            { PlayerRace.DarkIronDwarf,         "Dark Iron Dwarf" },
            { PlayerRace.Vulpera,               "Vulpera" },
            { PlayerRace.MagharOrc,             "Maghar Orc" },
            { PlayerRace.MechaGnome,            "Mecha Gnome" },
            { PlayerRace.Dracthyr,              "Dracthyr" },
            { PlayerRace.Dracthyr2,             "Dracthyr" }
        };

        private static readonly Dictionary<Profession, string> Professions = new Dictionary<Profession, string>
        {
            { Profession.FirstAid,          "First Aid" },
            { Profession.Blacksmithing,     "Blacksmithing" },
            { Profession.Leatherworking,    "Leatherworking" },
            { Profession.Alchemy,           "Alchemy" },
            { Profession.Herbalism,         "Herbalism" },
            { Profession.Cooking,           "Cooking" },
            { Profession.Mining,            "Mining" },
            { Profession.Tailoring,         "Tailoring" },
            { Profession.Engineering,       "Engineering" },
            { Profession.Enchanting,        "Enchanting" },
            { Profession.Fishing,           "Fishing" },
            { Profession.Skinning,          "Skinning" },
            { Profession.Jewelcrafting,     "Jewelcrafting" },
            { Profession.Inscription,       "Inscription" },
        };

        #endregion

        #region ASSETS PATHS

        private static readonly Dictionary<PlayerClass, string> PlayerClassIcons = new Dictionary<PlayerClass, string>
        {
            { PlayerClass.Warrior,      $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/class_warrior.jpg" },
            { PlayerClass.Paladin,      $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/class_paladin.jpg" },
            { PlayerClass.Hunter,       $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/class_hunter.jpg" },
            { PlayerClass.Rogue,        $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/class_rogue.jpg" },
            { PlayerClass.Priest,       $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/class_priest.jpg" },
            { PlayerClass.DeathKnight,  $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/class_deathknight.jpg" },
            { PlayerClass.Shaman,       $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/class_shaman.jpg" },
            { PlayerClass.Mage,         $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/class_mage.jpg" },
            { PlayerClass.Warlock,      $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/class_warlock.jpg" },
            { PlayerClass.Monk,         $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/class_monk.jpg" },
            { PlayerClass.Druid,        $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/class_druid.jpg" },
            { PlayerClass.DemonHunter,  $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/class_demonhunter.jpg" },
            { PlayerClass.Evoker,       $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/class_evoker.jpg" }
        };

        private static readonly Dictionary<PlayerClass, string> PlayerClassCards = new Dictionary<PlayerClass, string>
        {
            { PlayerClass.Warrior,      $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/cards/warrior.jpg" },
            { PlayerClass.Paladin,      $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/cards/paladin.jpg" },
            { PlayerClass.Hunter,       $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/cards/hunter.jpg" },
            { PlayerClass.Rogue,        $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/cards/rogue.jpg" },
            { PlayerClass.Priest,       $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/cards/priest.jpg" },
            { PlayerClass.DeathKnight,  $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/cards/deathknight.jpg" },
            { PlayerClass.Shaman,       $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/cards/shaman.jpg" },
            { PlayerClass.Mage,         $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/cards/mage.jpg" },
            { PlayerClass.Warlock,      $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/cards/warlock.jpg" },
            { PlayerClass.Monk,         $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/cards/monk.jpg" },
            { PlayerClass.Druid,        $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/cards/druid.jpg" },
            { PlayerClass.DemonHunter,  $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/cards/demonhunter.jpg" },
            { PlayerClass.Evoker,       $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/classes/cards/evoker.jpg" }
        };

        private static readonly Dictionary<PlayerRace, string> PlayerMaleRaceIcons = new Dictionary<PlayerRace, string>
        {
            { PlayerRace.Human,                 $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_human_male.jpg" },
            { PlayerRace.Orc,                   $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_orc_male.jpg" },
            { PlayerRace.Dwarf,                 $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_dwarf_male.jpg" },
            { PlayerRace.Nightelf,              $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_nightelf_male.jpg" },
            { PlayerRace.Undead,                $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_scourge_male.jpg" },
            { PlayerRace.Tauren,                $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_tauren_male.jpg" },
            { PlayerRace.Gnome,                 $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_gnome_male.jpg" },
            { PlayerRace.Troll,                 $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_troll_male.jpg" },
            { PlayerRace.Goblin,                $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_goblin_male.jpg" },
            { PlayerRace.Bloodelf,              $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_bloodelf_male.jpg" },
            { PlayerRace.Draenei,               $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_draenei_male.jpg" },
            { PlayerRace.Worgen,                $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_worgen_male.jpg" },
            { PlayerRace.Pandaren,              $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_pandaren_male.jpg" },
            { PlayerRace.Nightborne,            $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_nightborne_male.jpg" },
            { PlayerRace.HighmountainTauren,    $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_highmountaintauren_male.jpg" },
            { PlayerRace.Voidelf,               $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_voidelf_male.jpg" },
            { PlayerRace.LightforgedDraenei,    $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_lightforgeddraenei_male.jpg" },
            { PlayerRace.ZandalariTroll,        $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_zandalaritroll_male.jpg" },
            { PlayerRace.KulTiran,              $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_kultiran_male.jpg" },
            { PlayerRace.DarkIronDwarf,         $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_darkirondwarf_male.jpg" },
            { PlayerRace.Vulpera,               $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_vulpera_male.jpg" },
            { PlayerRace.MagharOrc,             $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_magharorc_male.jpg" },
            { PlayerRace.MechaGnome,            $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_mechagnome_male.jpg" },
            { PlayerRace.Dracthyr,              $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_dracthyr_male.jpg" },
            { PlayerRace.Dracthyr2,             $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_dracthyr_male.jpg" }
        };

        private static readonly Dictionary<PlayerRace, string> PlayerFemaleRaceIcons = new Dictionary<PlayerRace, string>
        {
            { PlayerRace.Human,                 $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_human_female.jpg" },
            { PlayerRace.Orc,                   $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_orc_female.jpg" },
            { PlayerRace.Dwarf,                 $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_dwarf_female.jpg" },
            { PlayerRace.Nightelf,              $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_nightelf_female.jpg" },
            { PlayerRace.Undead,                $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_scourge_female.jpg" },
            { PlayerRace.Tauren,                $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_tauren_female.jpg" },
            { PlayerRace.Gnome,                 $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_gnome_female.jpg" },
            { PlayerRace.Troll,                 $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_troll_female.jpg" },
            { PlayerRace.Goblin,                $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_goblin_female.jpg" },
            { PlayerRace.Bloodelf,              $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_bloodelf_female.jpg" },
            { PlayerRace.Draenei,               $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_draenei_female.jpg" },
            { PlayerRace.Worgen,                $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_worgen_female.jpg" },
            { PlayerRace.Pandaren,              $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_pandaren_female.jpg" },
            { PlayerRace.Nightborne,            $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_nightborne_female.jpg" },
            { PlayerRace.HighmountainTauren,    $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_highmountaintauren_female.jpg" },
            { PlayerRace.Voidelf,               $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_voidelf_female.jpg" },
            { PlayerRace.LightforgedDraenei,    $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_lightforgeddraenei_female.jpg" },
            { PlayerRace.ZandalariTroll,        $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_zandalaritroll_female.jpg" },
            { PlayerRace.KulTiran,              $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_kultiran_female.jpg" },
            { PlayerRace.DarkIronDwarf,         $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_darkirondwarf_female.jpg" },
            { PlayerRace.Vulpera,               $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_vulpera_female.jpg" },
            { PlayerRace.MagharOrc,             $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_magharorc_female.jpg" },
            { PlayerRace.MechaGnome,            $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_mechagnome_female.jpg" },
            { PlayerRace.Dracthyr,              $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_dracthyr_female.jpg" },
            { PlayerRace.Dracthyr2,             $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/races/race_dracthyr_female.jpg" }
        };

        private static readonly Dictionary<Profession, string> ProfessionIcons = new Dictionary<Profession, string>
        {
            { Profession.FirstAid,          $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/Professions/spell_holy_sealofsacrifice.jpg" },
            { Profession.Blacksmithing,     $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/Professions/trade_blacksmithing.jpg" },
            { Profession.Leatherworking,    $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/Professions/inv_misc_armorkit_17.jpg" },
            { Profession.Alchemy,           $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/Professions/trade_alchemy.jpg" },
            { Profession.Herbalism,         $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/Professions/spell_nature_naturetouchgrow.jpg" },
            { Profession.Cooking,           $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/Professions/inv_misc_food_15.jpg" },
            { Profession.Mining,            $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/Professions/trade_mining.jpg" },
            { Profession.Tailoring,         $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/Professions/trade_tailoring.jpg" },
            { Profession.Engineering,       $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/Professions/trade_engineering.jpg" },
            { Profession.Enchanting,        $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/Professions/trade_engraving.jpg" },
            { Profession.Fishing,           $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/Professions/trade_fishing.jpg" },
            { Profession.Skinning,          $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/Professions/inv_misc_pelt_wolf_01.jpg" },
            { Profession.Jewelcrafting,     $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/Professions/inv_misc_gem_01.jpg" },
            { Profession.Inscription,       $"/{Assembly.GetCallingAssembly().GetName().Name};component/Assets/Professions/inv_inscription_tradeskill01.jpg" },
        };

        #endregion

        public static string GetPlayerClassName(int class_id)
        {
            return PlayerClassNames[(PlayerClass)class_id];
        }

        public static string GetPlayerClassHexColor(int class_id)
        {
            return PlayerClassHexColors[(PlayerClass)class_id];
        }

        public static string GetPlayerRaceName(int race_id)
        {
            return PlayerRaceNames[(PlayerRace)race_id];
        }

        public static string GetProfessionName(int skill_id)
        {
            return Professions[(Profession)skill_id];
        }

        public static string GetProfessionIcon(int skill_id)
        {
            return ProfessionIcons[(Profession)skill_id];
        }

        public static string GetPlayerClassIcon(int class_id)
        {
            return PlayerClassIcons[(PlayerClass)class_id];
        }

        public static string GetPlayerClassCard(int class_id)
        {
            return PlayerClassCards[(PlayerClass)class_id];
        }

        public static string GetPlayerRaceIcon(int race_id, int gender)
        {
            if (gender == (int)Gender.Male)
            {
                return PlayerMaleRaceIcons[(PlayerRace)race_id];
            }
            else if (gender == (int)Gender.Female)
            {
                return PlayerFemaleRaceIcons[(PlayerRace)race_id];
            }
            else
            {
                return PlayerMaleRaceIcons[(PlayerRace)race_id];
            }
        }

        public static string GetPlayerFactionName(int race_id)
        {
            switch (race_id)
            {
                case 1:
                case 3:
                case 4:
                case 7:
                case 11:
                    return "Alliance";
                case 2:
                case 5:
                case 6:
                case 8:
                case 9:
                case 10:
                case 12:
                    return "Horde";
                default:
                    return "Neutral";
            }
        }

        public static string GetPlayerFactionHexColor(int race_id)
        {
            switch (race_id)
            {
                case 1:
                case 3:
                case 4:
                case 7:
                case 11:
                    return "0090FF";
                case 2:
                case 5:
                case 6:
                case 8:
                case 9:
                case 10:
                case 12:
                    return "E25555";
                default:
                    return "55E291";
            }
        }

        public static string ConvertCopperToGold(int copper)
        {
            int goldAmount = copper / 10000;
            string formattedGold = goldAmount.ToString("N0");
            return formattedGold;
        }
    }
}
