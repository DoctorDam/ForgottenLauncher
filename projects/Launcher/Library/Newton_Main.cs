/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Forgotten_Land_Launcher.Library
{
    public partial class Newton_Main
    {
        #region EMULATOR ID RESPONSE

        public class EmulatorIDResponse
        {
            [JsonProperty("emulator_id")]
            public int EmulatorID { get; set; }

            public static EmulatorIDResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<EmulatorIDResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetEmulatorIDResponse()
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "36" },
            });
        }

        #endregion

        #region LOGIN RESPONSE

        public class LoginResponse
        {
            [JsonProperty("Logged")]
            public bool Logged { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            [JsonProperty("Token")]
            public string Token { get; set; }

            [JsonProperty("AccountInfo")]
            public AccountInfo AccInfo { get; set; }

            public static LoginResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<LoginResponse>(json, Converter.Settings);
        }

        public class AccountInfo
        {
            [JsonProperty("Id")]
            public int Id { get; set; }

            [JsonProperty("IsActive")]
            public bool IsActive { get; set; }

            [JsonProperty("Username")]
            public string Username { get; set; }

            [JsonProperty("Email")]
            public string Email { get; set; }

            [JsonProperty("RasarKey")]
            public string RasarKey { get; set; }

            [JsonProperty("RasarIV")]
            public string RasarIV { get; set; }

            [JsonProperty("RankName")]
            public string RankName { get; set; }

            [JsonProperty("RankColor")]
            public string RankColor { get; set; }

            [JsonProperty("GMLevel")]
            public int GMLevel { get; set; }

            [JsonProperty("VotePoints")]
            public int VotePoints { get; set; }

            [JsonProperty("DonatePoints")]
            public int DonatePoints { get; set; }

            [JsonProperty("BattlePayCredits")]
            public int BattlePayCredits { get; set; }

            [JsonProperty("AvatarUrl")]
            public string AvatarUrl { get; set; }

            [JsonProperty("Nickname")]
            public string Nickname { get; set; }

            [JsonProperty("LastLogin")]
            public string LastLogin { get; set; }

            [JsonProperty("LastIP")]
            public string LastIP { get; set; }
        }

        public static async Task<string> GetLoginResponse(string usernameOrEmail, string password)
        {
            string json;
            int emulator_id = 0;
            bool isBattlenetEnabled = false;

            try
            {
                json = await GetBattlenetEnableStatusResponse();
                isBattlenetEnabled = BattlenetEnableStatusResponse.FromJson(json)?.BattlenetEnabled ?? false;
                json = await GetEmulatorIDResponse();
                emulator_id = EmulatorIDResponse.FromJson(json).EmulatorID;
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }

            if (Extensions.IsValidEmail(usernameOrEmail) && isBattlenetEnabled)
            {
                return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
                {
                    { "execute", "2" },
                    { "battlenet_who", Codex_Battlenet.ToBattlenetWho(usernameOrEmail) },
                    { "battlenet_token", Codex_Battlenet.ToBattlenetToken(usernameOrEmail, password) },
                    { "rasar_key", Cache.AccountInfo.RasarKey },
                    { "rasar_iv", Cache.AccountInfo.RasarIV }
                });
            }
            else if (emulator_id == 18) // exception legioncore
            {
                return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
                {
                    { "execute", "1" },
                    { "vendetta_who", Codex_Standard.ToVendettaWho(usernameOrEmail) },
                    { "vendetta_token", Codex_Battlenet.ToBattlenetToken(usernameOrEmail, password) },
                    { "rasar_key", Cache.AccountInfo.RasarKey },
                    { "rasar_iv", Cache.AccountInfo.RasarIV }
                });
            }
            else
            {
                return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
                {
                    { "execute", "1" },
                    { "vendetta_who", Codex_Standard.ToVendettaWho(usernameOrEmail) },
                    { "vendetta_token", Codex_Standard.ToVendettaToken(usernameOrEmail, password) },
                    { "rasar_key", Cache.AccountInfo.RasarKey },
                    { "rasar_iv", Cache.AccountInfo.RasarIV }
                });
            }
        }

        #endregion

        #region REGISTER RESPONSE

        public class RegisterResponse
        {
            [JsonProperty("Registered")]
            public bool Registered { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static RegisterResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<RegisterResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetRegisterResponse(string username, string email, string password1, string password2)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "3" },
                { "username", Codex_Standard.ToRegObscure(username) },
                { "email", Codex_Standard.ToRegObscure(email) },
                { "password1", Codex_Standard.ToRegObscure(password1) },
                { "password2", Codex_Standard.ToRegObscure(password2) },
            });
        }

        #endregion

        #region AUTH TOKEN RESPONSE

        public class AuthTokenResponse
        {
            [JsonProperty("Authorized")]
            public bool Authorized { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            [JsonProperty("AccountInfo")]
            public AccountInfo AccInfo { get; set; }

            public static AuthTokenResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<AuthTokenResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetAuthTokenResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "4" },
                { "token", token },
                { "md5username", md5username },
            });
        }

        #endregion

        #region PASSWORD RECOVERY RESPONSE

        public static async Task SendPasswordRecoveryRequest(string email)
        {
            await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "5" },
                { "email", email },
            });
        }

        #endregion

        #region PASSWORD CHANGE BY CODE RESPONSE

        public class PasswordChangeByCodeResponse
        {
            [JsonProperty("Changed")]
            public bool Changed { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static PasswordChangeByCodeResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<PasswordChangeByCodeResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetPasswordChangeByCodeResponse(string username, string code, string password1, string password2)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "6" },
                { "username", Codex_Standard.ToRegObscure(username) },
                { "recovery_code", Codex_Standard.ToRegObscure(code) },
                { "password1", Codex_Standard.ToRegObscure(password1) },
                { "password2", Codex_Standard.ToRegObscure(password2) },
            });
        }

        #endregion

        #region ACCOUNT CURRENCIES RESPONSE

        public class AccountCurrenciesResponse
        {
            [JsonProperty("Authorized")]
            public bool Authorized { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            [JsonProperty("VotePoints")]
            public int VotePoints { get; set; }

            [JsonProperty("DonatePoints")]
            public int DonatePoints { get; set; }

            public static AccountCurrenciesResponse FromJson(string json) =>
                JsonConvert.DeserializeObject<AccountCurrenciesResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetAccountCurrenciesResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "7" },
                { "token", token },
                { "md5username", md5username },
            });
        }

        #endregion

        #region BATTLENET ENABLE STATUS RESPONSE

        public class BattlenetEnableStatusResponse
        {
            [JsonProperty("BattlenetEnabled")]
            public bool BattlenetEnabled { get; set; }

            public static BattlenetEnableStatusResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<BattlenetEnableStatusResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetBattlenetEnableStatusResponse()
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "102" },
            });
        }

        #endregion

        #region BATTLE PAY CREDITS ENABLE STATUS RESPONSE

        public class BattlePayCreditsEnableStatusResponse
        {
            [JsonProperty("BattlePayCreditsAsDP")]
            public bool BattlePayCreditsAsDP { get; set; }

            public static BattlePayCreditsEnableStatusResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<BattlePayCreditsEnableStatusResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetBattlePayCreditsEnableStatusResponse()
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "104" },
            });
        }

        #endregion

        #region REALMLIST RESPONSE

        public class RealmlistResponse
        {
            [JsonProperty("realmlist_address")]
            public string RealmlistAddress { get; set; }

            public static RealmlistResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<RealmlistResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetRealmlistResponse()
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "108" },
            });
        }

        #endregion

        #region SEND ISSUE TO DISCORD

        public static async Task<string> SendIssueToDiscord(string report_by, string message)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "105" },
                { "username", report_by },
                { "message", message },
            });
        }

        #endregion

        #region REGISTRATION TOS

        public class RegistrationTOSResponse
        {
            [JsonProperty("text")]
            public string Text { get; set; }

            public static RegistrationTOSResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<RegistrationTOSResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetRegistrationTOSResponse()
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "100" }
            });
        }

        #endregion

        #region DISCORD SERVER INFO RESPONSE

        public class DiscordServerInfoResponse
        {
            [JsonProperty("name")]
            public string ServerName { get; set; }

            [JsonProperty("presence_count")]
            public long MembersCount { get; set; }

            [JsonProperty("instant_invite")]
            public string InviteUrl { get; set; }

            [JsonProperty("members")]
            public List<DiscordMember> Members { get; set; }

            public static DiscordServerInfoResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<DiscordServerInfoResponse>(json, Converter.Settings);
        }

        public class DiscordMember
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("avatar_url")]
            public string AvatarUrl { get; set; }

            [JsonProperty("game", NullValueHandling = NullValueHandling.Ignore)]
            public Game Game { get; set; }
        }

        public class Game
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public static async Task<string> GetDiscordServerInfoResponse()
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "101" }
            });
        }

        #endregion

        #region UPDATE AVATAR RESPONSE

        public class AvatarUpdateResponse
        {
            [JsonProperty("Updated")]
            public bool Updated { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static AvatarUpdateResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<AvatarUpdateResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetAvatarUpdateResponse(string token, string md5username, string image_url)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "8" },
                { "token", token },
                { "md5username", md5username },
                { "image_url", image_url }
            });
        }

        #endregion

        #region SLIDER RESPONSE

        public class SliderResponse
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("image_url")]
            public string ImageUrl { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("redirect_url")]
            public string RedirectUrl { get; set; }

            public static List<SliderResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<SliderResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetSliderResponse()
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "103" },
            });
        }

        #endregion

        #region WEBSITE ARTICLES RESPONSE

        public class WebsiteArticlesResponse
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("date")]
            public string Date { get; set; }

            [JsonProperty("author")]
            public string Author { get; set; }

            [JsonProperty("image_url")]
            public string PictureUrl { get; set; }

            [JsonProperty("headline_en")]
            public string Headline_EN { get; set; }

            [JsonProperty("content_en")]
            public string Content_EN { get; set; }

            [JsonProperty("headline_de")]
            public string Headline_DE { get; set; }

            [JsonProperty("content_de")]
            public string Content_DE { get; set; }

            [JsonProperty("headline_es")]
            public string Headline_ES { get; set; }

            [JsonProperty("content_es")]
            public string Content_ES { get; set; }

            [JsonProperty("headline_fr")]
            public string Headline_FR { get; set; }

            [JsonProperty("content_fr")]
            public string Content_FR { get; set; }

            [JsonProperty("headline_no")]
            public string Headline_NO { get; set; }

            [JsonProperty("content_no")]
            public string Content_NO { get; set; }

            [JsonProperty("headline_ro")]
            public string Headline_RO { get; set; }

            [JsonProperty("content_ro")]
            public string Content_RO { get; set; }

            [JsonProperty("headline_se")]
            public string Headline_SE { get; set; }

            [JsonProperty("content_se")]
            public string Content_SE { get; set; }

            [JsonProperty("headline_ru")]
            public string Headline_RU { get; set; }

            [JsonProperty("content_ru")]
            public string Content_RU { get; set; }

            [JsonProperty("headline_zh")]
            public string Headline_ZH { get; set; }

            [JsonProperty("content_zh")]
            public string Content_ZH { get; set; }

            [JsonProperty("headline_ko")]
            public string Headline_KO { get; set; }

            [JsonProperty("content_ko")]
            public string Content_KO { get; set; }

            [JsonProperty("headline_fa")]
            public string Headline_FA { get; set; }

            [JsonProperty("content_fa")]
            public string Content_FA { get; set; }

            [JsonProperty("redirect_url")]
            public string RedirectUrl { get; set; }

            public static List<WebsiteArticlesResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<WebsiteArticlesResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetWebsiteArticlesResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "9" },
                { "token", token },
                { "md5username", md5username },
            });
        }

        #endregion

        #region LAUNCHER EVENTS RESPONSE

        public class LauncherEventsResponse
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("picture_url")]
            public string PictureUrl { get; set; }

            [JsonProperty("redirect_url")]
            public string RedirectUrl { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("content")]
            public string Content { get; set; }

            [JsonProperty("expiry_date")]
            public string ExpiryDate { get; set; }

            public static List<LauncherEventsResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<LauncherEventsResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetLauncherEventsResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "10" },
                { "token", token },
                { "md5username", md5username },
            });
        }

        #endregion

        #region WEBSITE CHANGELOGS RESPONSE

        public class WebsiteChangelogsResponse
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("category_name")]
            public string CategoryName { get; set; }

            [JsonProperty("changelog")]
            public string Description { get; set; }

            [JsonProperty("date_timestamp")]
            public long DateTimestamp { get; set; }

            public static List<WebsiteChangelogsResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<WebsiteChangelogsResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetWebsiteChangelogsResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "11" },
                { "token", token },
                { "md5username", md5username },
            });
        }

        #endregion

        #region LAUNCHER FAQ RESPONSE

        public class LauncherFaqResponse
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("text")]
            public string Text { get; set; }

            public static List<LauncherFaqResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<LauncherFaqResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetLauncherFaqResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "31" },
                { "token", token },
                { "md5username", md5username },
            });
        }

        #endregion

        #region LOGIN REWARDS RESPONSE

        public class LoginRewardsResponse
        {
            [JsonProperty("reward_id")]
            public int Id { get; set; }

            [JsonProperty("day")]
            public int Day { get; set; }

            [JsonProperty("month")]
            public int Month { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("picture_url")]
            public string PictureUrl { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("requires_player")]
            public bool RequiresPlayer { get; set; }

            [JsonProperty("requires_input")]
            public bool RequiresInput { get; set; }

            [JsonProperty("claimed")]
            public bool Claimed { get; set; }

            public static List<LoginRewardsResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<LoginRewardsResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetLoginRewardsResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "12" },
                { "token", token },
                { "md5username", md5username },
            });
        }

        #endregion

        #region CLAIM LOGIN REWARD RESPONSE

        public class ClaimLoginRewardResponse
        {
            [JsonProperty("Claimed")]
            public bool Claimed { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static ClaimLoginRewardResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<ClaimLoginRewardResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetClaimLoginRewardResponse(string token, string md5username, string month, string day)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "13" },
                { "token", token },
                { "md5username", md5username },
                { "month", month },
                { "day", day }
            });
        }

        #endregion

        #region ACCOUNT INVENTORY RESPONSE

        public class AccountInventoryResponse
        {
            [JsonProperty("reward_id")]
            public int Id { get; set; }

            [JsonProperty("picture_url")]
            public string PictureUrl { get; set; }

            [JsonProperty("acquired_on")]
            public string AcquiredOn { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("requires_player")]
            public bool RequiresPlayer { get; set; }

            [JsonProperty("requires_input")]
            public bool RequiresInput { get; set; }

            public static List<AccountInventoryResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<AccountInventoryResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetAccountInventoryResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "14" },
                { "token", token },
                { "md5username", md5username },
            });
        }

        #endregion

        #region CHARACTERS LIST RESPONSE

        public class CharactersListResponse
        {
            [JsonProperty("guid")]
            public int Guid { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("race")]
            public int Race { get; set; }

            [JsonProperty("class")]
            public int Class { get; set; }

            [JsonProperty("gender")]
            public int Gender { get; set; }

            [JsonProperty("level")]
            public int Level { get; set; }

            [JsonProperty("realmid")]
            public int RealmId { get; set; }

            [JsonProperty("realmname")]
            public string RealmName { get; set; }

            public static List<CharactersListResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<CharactersListResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetCharactersListResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "15" },
                { "token", token },
                { "md5username", md5username },
            });
        }

        #endregion

        #region USE INVENTORY ITEM RESPONSE

        public class UseInventoryItemResponse
        {
            [JsonProperty("Used")]
            public bool Used { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static UseInventoryItemResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<UseInventoryItemResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetUseInventoryItemResponse(string token, string md5username, string guid, string reward_id, string realm_id)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "16" },
                { "token", token },
                { "md5username", md5username },
                { "guid", guid },
                { "reward_id", reward_id },
                { "realm_id", realm_id }
            });
        }

        #endregion

        #region GAME FILES LIST RESPONSE

        public class GameFilesListResponse
        {
            [JsonProperty("Name")]
            public string Name { get; set; }

            [JsonProperty("Size")]
            public long Size { get; set; }

            [JsonProperty("Timestamp")]
            public int Timestamp { get; set; }

            [JsonProperty("IsBase")]
            public bool IsBase { get; set; }

            [JsonProperty("IsHD")]
            public bool IsHD { get; set; }

            [JsonProperty("IsUpdate")]
            public bool IsUpdate { get; set; }

            [JsonProperty("IsProgramData")]
            public bool IsProgramData { get; set; }

            [JsonProperty("TargetPath")]
            public string TargetPath { get; set; }

            [JsonProperty("Url")]
            public string Url { get; set; }

            public static List<GameFilesListResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<GameFilesListResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetGameFilesListResponse()
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "106" },
            });
        }

        #endregion

        #region ADDONS LIST RESPONSE

        public class AddonsListResponse
        {
            [JsonProperty("Name")]
            public string Name { get; set; }

            [JsonProperty("Description")]
            public string Description { get; set; }

            [JsonProperty("PictureUrl")]
            public string PictureUrl { get; set; }

            [JsonProperty("TotalSize")]
            public long TotalSize { get; set; }

            [JsonProperty("Files")]
            public List<AddonFile> Files { get; set; }

            public static List<AddonsListResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<AddonsListResponse>>(json, Converter.Settings);
        }

        public class AddonFile
        {

            [JsonProperty("size")]
            public long Size { get; set; }

            [JsonProperty("url")]
            public string DownloadUrl { get; set; }

            [JsonProperty("target")]
            public string LocalTarget { get; set; }
        }

        public static async Task<string> GetAddonsListResponse()
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "107" },
            });
        }

        #endregion

        #region LAUNCHER FILES LIST RESPONSE

        public class LauncherUpdateFilesListResponse
        {
            [JsonProperty("Name")]
            public string Name { get; set; }

            [JsonProperty("Size")]
            public long Size { get; set; }

            [JsonProperty("Timestamp")]
            public int Timestamp { get; set; }

            [JsonProperty("MD5_Hash")]
            public string MD5Hash { get; set; }

            [JsonProperty("TargetPath")]
            public string TargetPath { get; set; }

            [JsonProperty("Url")]
            public string Url { get; set; }

            public static List<LauncherUpdateFilesListResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<LauncherUpdateFilesListResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetLauncherUpdateFilesListResponseResponse()
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "109" },
            });
        }

        #endregion

        #region UPDATE PUBLIC NICKNAME RESPONSE

        public class UpdatePublicNicknameResponse
        {
            [JsonProperty("Updated")]
            public bool Updated { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static UpdatePublicNicknameResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<UpdatePublicNicknameResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetUpdatePublicNicknameResponse(string token, string md5username, string nickname)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "17" },
                { "token", token },
                { "md5username", md5username },
                { "nickname", nickname }
            });
        }

        #endregion

        #region PUBLIC NICKNAME RESPONSE

        public class PublicNicknameResponse
        {
            [JsonProperty("Authorized")]
            public bool Authorized { get; set; }

            [JsonProperty("Nickname")]
            public string Nickname { get; set; }

            public static PublicNicknameResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<PublicNicknameResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetPublicNicknameResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "18" },
                { "token", token },
                { "md5username", md5username }
            });
        }

        #endregion

        #region UNSTUCK CHARACTER RESPONSE

        public class CharacterUnstuckResponse
        {
            [JsonProperty("Unstucked")]
            public bool Unstucked { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static CharacterUnstuckResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<CharacterUnstuckResponse>(json, Converter.Settings);
        }
            
        public static async Task<string> GetCharacterUnstuckResponse(string token, string md5username, string guid, string realm_id)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "19" },
                { "token", token },
                { "md5username", md5username },
                { "guid", guid },
                { "realm_id", realm_id }
            });
        }

        #endregion

        #region TELEPORT LIST RESPONSE

        public class TeleportListResponse
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("dp_or_bpc_price")]
            public int DPorBPCprice { get; set; }

            [JsonProperty("vp_price")]
            public int VPrice { get; set; }

            public static List<TeleportListResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<TeleportListResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetTeleportListResponse()
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "20" }
            });
        }

        #endregion

        #region CHARACTER TELEPORT RESPONSE

        public class CharacterTeleportResponse
        {
            [JsonProperty("Teleported")]
            public bool Teleported { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static CharacterTeleportResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<CharacterTeleportResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetCharacterTeleportResponse(string token, string md5username, string guid, string realm_id, string teleport_id)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "21" },
                { "token", token },
                { "md5username", md5username },
                { "guid", guid },
                { "realm_id", realm_id },
                { "teleport_id", teleport_id }
            });
        }

        #endregion

        #region CHARACTERS TICKETS LIST RESPONSE

        public class CharactersTicketsListResponse
        {
            [JsonProperty("ticketId")]
            public int TicketId { get; set; }

            [JsonProperty("playerGuid")]
            public int PlayerGuid { get; set; }

            [JsonProperty("playerRace")]
            public int PlayerRace { get; set; }

            [JsonProperty("playerGender")]
            public int PlayerGender { get; set; }

            [JsonProperty("playerName")]
            public string PlayerName { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("lastModifiedTime")]
            public string LastModifiedTime { get; set; }

            [JsonProperty("closed")]
            public bool Closed { get; set; }

            [JsonProperty("completed")]
            public bool Completed { get; set; }

            [JsonProperty("viewed")]
            public bool Viewed { get; set; }

            [JsonProperty("realmId")]
            public int RealmId { get; set; }

            [JsonProperty("realmName")]
            public string RealmName { get; set; }

            public static List<CharactersTicketsListResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<CharactersTicketsListResponse>>(json, Converter.Settings);

        }

        public static async Task<string> GetCharactersTicketsListResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "22" },
                { "token", token },
                { "md5username", md5username }
            });
        }

        #endregion

        #region LADDERBOARD RESPONSE

        public class LadderboardResponse
        {
            [JsonProperty("playerName")]
            public string PlayerName { get; set; }

            [JsonProperty("playerRace")]
            public int PlayerRace { get; set; }

            [JsonProperty("playerGender")]
            public int PlayerGender { get; set; }

            [JsonProperty("playerClass")]
            public int PlayerClass { get; set; }

            [JsonProperty("todayKills")]
            public int TodayKills { get; set; }

            [JsonProperty("yesterdayKills")]
            public int YesterdayKills { get; set; }

            [JsonProperty("totalKills")]
            public int TotalKills { get; set; }

            [JsonProperty("realmId")]
            public int RealmId { get; set; }

            [JsonProperty("realmName")]
            public string RealmName { get; set; }

            public static List<LadderboardResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<LadderboardResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetLadderboardResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "34" },
                { "token", token },
                { "md5username", md5username }
            });
        }

        #endregion

        #region PRIVATE MESSAGES LIST RESPONSE

        public class PrivateMessagesListResponse
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("parent_id")]
            public int ParentId { get; set; }

            [JsonProperty("sender_id")]
            public int SenderId { get; set; }

            [JsonProperty("sender_nickname")]
            public string SenderNickname { get; set; }

            [JsonProperty("sender_avatar_url")]
            public string SenderAvatarUrl { get; set; }

            [JsonProperty("receiver_id")]
            public int ReceiverId { get; set; }

            [JsonProperty("receiver_nickname")]
            public string ReceiverNickname { get; set; }

            [JsonProperty("receiver_avatar_url")]
            public string ReceiverAvatarUrl { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("date_edited")]
            public string DateEdited { get; set; }

            [JsonProperty("seen")]
            public bool Seen { get; set; }

            [JsonProperty("date_seen")]
            public string DateSeen { get; set; }

            public static List<PrivateMessagesListResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<PrivateMessagesListResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetPrivateMessagesListResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "23" },
                { "token", token },
                { "md5username", md5username },
            });
        }

        #endregion

        #region NEW PRIVATE MESSAGE RESPONSE

        public class NewPrivateMessageResponse
        {
            [JsonProperty("Sent")]
            public bool Sent { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static NewPrivateMessageResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<NewPrivateMessageResponse>(json, Converter.Settings);
        }
            
        public static async Task<string> GetNewPrivateMessageResponse(string token, string md5username, string receiver, string title, string message)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "24" },
                { "token", token },
                { "md5username", md5username },
                { "receiver", receiver },
                { "title", title },
                { "message", message }
            });
        }

        #endregion

        #region PRIVATE MESSAGE THREAD RESPONSE

        public class PrivateMessageThreadResponse
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("parent_id")]
            public int ParentId { get; set; }

            [JsonProperty("sender_id")]
            public int SenderId { get; set; }

            [JsonProperty("sender_nickname")]
            public string SenderNickname { get; set; }

            [JsonProperty("sender_avatar_url")]
            public string SenderAvatarUrl { get; set; }

            [JsonProperty("receiver_id")]
            public int ReceiverId { get; set; }

            [JsonProperty("receiver_nickname")]
            public string ReceiverNickname { get; set; }

            [JsonProperty("receiver_avatar_url")]
            public string ReceiverAvatarUrl { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("date_edited")]
            public string DateEdited { get; set; }

            [JsonProperty("seen")]
            public bool Seen { get; set; }

            [JsonProperty("date_seen")]
            public string DateSeen { get; set; }
            public static List<PrivateMessageThreadResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<PrivateMessageThreadResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetPrivateMessageThreadResponse(string token, string md5username, string message_id)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "25" },
                { "token", token },
                { "md5username", md5username },
                { "id", message_id },
            });
        }

        #endregion

        #region NEW MESSAGE REPLY RESPONSE

        public class NewMessageReplyResponse
        {
            [JsonProperty("Sent")]
            public bool Sent { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static NewMessageReplyResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<NewMessageReplyResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetNewMessageReplyResponse(string token, string md5username, string message_id, string message)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "26" },
                { "token", token },
                { "md5username", md5username },
                { "message_id", message_id },
                { "message", message }
            });
        }

        #endregion

        #region DELETE PRIVATE MESSAGE RESPONSE

        public class DeletePrivateMessageResponse
        {
            [JsonProperty("Deleted")]
            public bool Deleted { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static DeletePrivateMessageResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<DeletePrivateMessageResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetDeletePrivateMessageResponse(string token, string md5username, string message_id)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "27" },
                { "token", token },
                { "md5username", md5username },
                { "message_id", message_id },
            });
        }

        #endregion

        #region REDEEM GIFT CODE RESPONSE

        public class RedeemGiftCodeResponse
        {
            [JsonProperty("Redeemed")]
            public bool Redeemed { get; set; }

            [JsonProperty("PictureUrl")]
            public string PictureUrl { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static RedeemGiftCodeResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<RedeemGiftCodeResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetRedeemGiftCodeResponse(string token, string md5username, string gift_code)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "28" },
                { "token", token },
                { "md5username", md5username },
                { "gift_code", gift_code },
            });
        }

        #endregion

        #region GIFT PREVIEW

        public class GiftPreview
        {
            [JsonProperty("IsValid")]
            public bool IsValid { get; set; }

            [JsonProperty("PictureUrl")]
            public string PictureUrl { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static GiftPreview FromJson(string json) => 
                JsonConvert.DeserializeObject<GiftPreview>(json, Converter.Settings);
        }

        public static async Task<string> GetGiftPreview(string token, string md5username, string gift_code)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "43" },
                { "token", token },
                { "md5username", md5username },
                { "gift_code", gift_code },
            });
        }

        #endregion

        #region SHOP LIST RESPONSE

        public class ShopListResponse
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("category")]
            public string Category { get; set; }

            [JsonProperty("picture_url")]
            public string PictureUrl { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("dp_or_bpc_price")]
            public int DPorBPCprice { get; set; }

            [JsonProperty("vp_price")]
            public int VPrice { get; set; }

            public static List<ShopListResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<ShopListResponse>>(json, Converter.Settings);
        }
            

        public static async Task<string> GetShopListResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "29" },
                { "token", token },
                { "md5username", md5username },
            });
        }

        #endregion

        #region SHOP PURCHASE RESPONSE

        public class ShopPurchaseResponse
        {
            [JsonProperty("Purchased")]
            public bool Purchased { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static ShopPurchaseResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<ShopPurchaseResponse>(json, Converter.Settings);
        }
            

        public static async Task<string> GetShopPurchaseResponse(string token, string md5username, string article_id)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "30" },
                { "token", token },
                { "md5username", md5username },
                { "article_id", article_id },
            });
        }

        #endregion

        #region VOTE SITES LIST RESPONSE

        public class VoteSitesResponse
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("vote_url")]
            public string VoteUrl { get; set; }

            [JsonProperty("image_url")]
            public string ImageUrl { get; set; }

            [JsonProperty("points_reward")]
            public int PointsReward { get; set; }

            [JsonProperty("seconds_left")]
            public int SecondsLeft { get; set; }

            public static List<VoteSitesResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<VoteSitesResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetVoteSitesResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "32" },
                { "token", token },
                { "md5username", md5username },
            });
        }

        #endregion

        #region VOTE RESPONSE

        public class VoteResponse
        {
            [JsonProperty("Voted")]
            public bool Voted { get; set; }

            [JsonProperty("Points")]
            public int Points { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static VoteResponse FromJson(string json) => 
                JsonConvert.DeserializeObject<VoteResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetVoteResponse(string token, string md5username, string site_id)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "33" },
                { "token", token },
                { "md5username", md5username },
                { "site_id", site_id },
            });
        }

        #endregion

        #region REALMS STATUS RESPONSE

        public class RealmsStatusResponse
        {
            [JsonProperty("RealmId")]
            public int RealmId { get; set; }

            [JsonProperty("RealmName")]
            public string RealmName { get; set; }

            [JsonProperty("OnlineStatus")]
            public bool OnlineStatus { get; set; }

            public static List<RealmsStatusResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<RealmsStatusResponse>>(json, Converter.Settings);
        } 

        public static async Task<string> GetRealmsStatusResponse()
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "35" }
            });
        }

        #endregion

        #region PAGES OPTIONS

        public class PagesOptions
        {
            [JsonProperty("account_inventory")]
            public PageOption AccountInventory { get; set; }
            
            [JsonProperty("private_messages")]
            public PageOption PrivateMessages { get; set; }
            
            [JsonProperty("login_rewards")]
            public PageOption LoginRewards { get; set; }
            
            [JsonProperty("news")]
            public PageOption News { get; set; }
            
            [JsonProperty("events")]
            public PageOption Events { get; set; }
            
            [JsonProperty("discord")]
            public PageOption Discord { get; set; }
            
            [JsonProperty("changelogs")]
            public PageOption Changelogs { get; set; }
            
            [JsonProperty("shop")]
            public PageOption Shop { get; set; }
            
            [JsonProperty("characters_market")]
            public PageOption CharactersMarket { get; set; }
            
            [JsonProperty("addons")]
            public PageOption Addons { get; set; }
            
            [JsonProperty("vote")]
            public PageOption Vote { get; set; }
            
            [JsonProperty("online_players")]
            public PageOption OnlinePlayers { get; set; }
            
            [JsonProperty("ladderboard")]
            public PageOption Ladderboard { get; set; }
            
            [JsonProperty("faq")]
            public PageOption Faq { get; set; }

            public static PagesOptions FromJson(string json) => 
                JsonConvert.DeserializeObject<PagesOptions>(json, Converter.Settings);
        }

        public class PageOption
        {
            [JsonProperty("button_visible")]
            public bool ButtonVisible { get; set; }

            [JsonProperty("page_enabled")]
            public bool PageEnabled { get; set; }
        }

        public static async Task<string> GetPagesOptions()
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "110" },
            });
        }

        #endregion

        #region CHARACTERS MARKET: LIST

        public class CharactersMarketList
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("character_guid")]
            public int CharacterGuid { get; set; }

            [JsonProperty("owner_account_id")]
            public int OwnerAccountId { get; set; }

            [JsonProperty("realm_id")]
            public int RealmId { get; set; }

            [JsonProperty("realm_name")]
            public string RealmName { get; set; }

            [JsonProperty("allow_bidding")]
            public bool AllowBidding { get; set; }

            [JsonProperty("price")]
            public int Price { get; set; }

            [JsonProperty("date_added")]
            public string DateAdded { get; set; }

            [JsonProperty("seconds_left")]
            public int SecondsLeft { get; set; }

            [JsonProperty("char_info")]
            public CharInfo CharInfo { get; set; }

            [JsonProperty("professions")]
            public List<Profession> Professions { get; set; }

            public static List<CharactersMarketList> FromJson(string json) =>
                JsonConvert.DeserializeObject<List<CharactersMarketList>>(json, Converter.Settings);
        }

        public class CharInfo
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("race")]
            public int Race { get; set; }

            [JsonProperty("class")]
            public int Class { get; set; }

            [JsonProperty("gender")]
            public int Gender { get; set; }

            [JsonProperty("level")]
            public int Level { get; set; }

            [JsonProperty("money")]
            public int Money { get; set; }

            [JsonProperty("totalkills")]
            public int Totalkills { get; set; }
        }

        public class Profession
        {
            [JsonProperty("skill_id")]
            public int SkillId { get; set; }

            [JsonProperty("level")]
            public int Level { get; set; }

            [JsonProperty("max")]
            public int Max { get; set; }
        }

        public static async Task<string> GetCharactersMarketList(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "37" },
                { "token", token },
                { "md5username", md5username },
            });
        }

        #endregion

        #region CHARACTERS MARKET: BIDS NOTIFICATIONS

        public class CharactersMarketBidsNotifications
        {
            [JsonProperty("character_name")]
            public string CharacterName { get; set; }

            [JsonProperty("character_race")]
            public int CharacterRace { get; set; }

            [JsonProperty("character_class")]
            public int CharacterClass { get; set; }

            [JsonProperty("character_level")]
            public int CharacterLevel { get; set; }

            [JsonProperty("character_gender")]
            public int CharacterGender { get; set; }

            public static List<CharactersMarketBidsNotifications> FromJson(string json) =>
                JsonConvert.DeserializeObject<List<CharactersMarketBidsNotifications>>(json, Converter.Settings);
        }

        public static async Task<string> GetCharactersMarketBidsNotifications(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "45" },
                { "token", token },
                { "md5username", md5username },
            });
        }

        #endregion

        #region CHARACTERS MARKET: SETTINGS

        public class CharactersMarketSettings
        {
            [JsonProperty("minimum_price")]
            public int MinimumPrice { get; set; }

            [JsonProperty("maximum_price")]
            public int MaximumPrice { get; set; }

            [JsonProperty("commission_percent")]
            public int CommissionPercent { get; set; }

            [JsonProperty("hours_durations")]
            public List<int> HoursDurations { get; set; }

            [JsonProperty("minimum_player_level")]
            public int MinimumPlayerLevel { get; set; }

            [JsonProperty("minimum_player_gold")]
            public int MinimumPlayerGold { get; set; }

            [JsonProperty("armory_url")]
            public string ArmoryUrl { get; set; }

            public static CharactersMarketSettings FromJson(string json) =>
                JsonConvert.DeserializeObject<CharactersMarketSettings>(json, Converter.Settings);
        }

        public static async Task<string> GetCharactersMarketSettings()
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "111" },
            });
        }

        #endregion

        #region CHARACTERS MARKET: SELL

        public class CharactersMarketSellResponse
        {
            [JsonProperty("Authorized")]
            public bool Authorized { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static CharactersMarketSellResponse FromJson(string json) =>
                JsonConvert.DeserializeObject<CharactersMarketSellResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetCharactersMarketSellResponse(string token, string md5username, string character_guid, string realm_id, string duration, string allow_bidding, string price)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "38" },
                { "token", token },
                { "md5username", md5username },
                { "character_guid", character_guid },
                { "realm_id", realm_id },
                { "duration", duration },
                { "allow_bidding", allow_bidding },
                { "price", price },
            });
        }

        #endregion

        #region CHARACTERS MARKET: CANCEL

        public class CharactersMarketCancelResponse
        {
            [JsonProperty("Authorized")]
            public bool Authorized { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static CharactersMarketCancelResponse FromJson(string json) =>
                JsonConvert.DeserializeObject<CharactersMarketCancelResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetCharactersMarketCancelResponse(string token, string md5username, string sale_id)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "39" },
                { "token", token },
                { "md5username", md5username },
                { "sale_id", sale_id }
            });
        }

        #endregion

        #region CHARACTERS MARKET: BUY

        public class CharactersMarketBuyResponse
        {
            [JsonProperty("Authorized")]
            public bool Authorized { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static CharactersMarketBuyResponse FromJson(string json) =>
                JsonConvert.DeserializeObject<CharactersMarketBuyResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetCharactersMarketBuyResponse(string token, string md5username, string sale_id)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "40" },
                { "token", token },
                { "md5username", md5username },
                { "sale_id", sale_id }
            });
        }

        #endregion

        #region CHARACTERS MARKET: BID

        public class CharactersMarketBidResponse
        {
            [JsonProperty("Authorized")]
            public bool Authorized { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            public static CharactersMarketBidResponse FromJson(string json) =>
                JsonConvert.DeserializeObject<CharactersMarketBidResponse>(json, Converter.Settings);
        }

        public static async Task<string> GetCharactersMarketBidResponse(string token, string md5username, string sale_id, string bid_amount)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "44" },
                { "token", token },
                { "md5username", md5username },
                { "sale_id", sale_id },
                { "bid_amount", bid_amount }
            });
        }

        #endregion

        #region ACCOUNT BAN STATUS

        public class AccountBanStatus
        {
            [JsonProperty("Authorized")]
            public bool Authorized { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            [JsonProperty("IsBanned")]
            public bool IsBanned { get; set; }

            [JsonProperty("BanDuration")]
            public string BanDuration { get; set; }

            public static AccountBanStatus FromJson(string json) =>
                JsonConvert.DeserializeObject<AccountBanStatus>(json, Converter.Settings);
        }

        public static async Task<string> GetAccountBanStatus(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "41" },
                { "token", token },
                { "md5username", md5username }
            });
        }

        #endregion

        #region ACCOUNT ACTIVE STATUS

        public class AccountActiveStatus
        {
            [JsonProperty("Authorized")]
            public bool Authorized { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            [JsonProperty("IsActive")]
            public bool IsActive { get; set; }

            public static AccountActiveStatus FromJson(string json) =>
                JsonConvert.DeserializeObject<AccountActiveStatus>(json, Converter.Settings);
        }

        public static async Task<string> GetAccountActiveStatus(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "112" },
                { "token", token },
                { "md5username", md5username }
            });
        }

        #endregion

        #region DISCORD RPC: ONLINE CHARACTER

        public class DiscordRPCOnlineCharacter
        {
            [JsonProperty("Message")]
            public string Message { get; set; }

            [JsonProperty("playerName")]
            public string Name { get; set; }

            [JsonProperty("playerRace")]
            public int Race { get; set; }

            [JsonProperty("playerGender")]
            public int Gender { get; set; }

            [JsonProperty("playerClass")]
            public int Class { get; set; }

            [JsonProperty("playerLevel")]
            public int Level { get; set; }

            [JsonProperty("zoneName")]
            public string ZoneName { get; set; }

            [JsonProperty("realmId")]
            public int RealmId { get; set; }

            [JsonProperty("realmName")]
            public string RealmName { get; set; }

            public static DiscordRPCOnlineCharacter FromJson(string json) =>
                JsonConvert.DeserializeObject<DiscordRPCOnlineCharacter>(json, Converter.Settings);
        }

        public static async Task<string> GetDiscordRPCOnlineCharacter(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "46" },
                { "token", token },
                { "md5username", md5username }
            });
        }

        #endregion

        #region ONLINE PLAYERS RESPONSE

        public class OnlinePlayersResponse
        {
            [JsonProperty("total_players")]
            public long TotalPlayers { get; set; }

            [JsonProperty("players")]
            public List<OnlinePlayer> Players { get; set; }

            public static OnlinePlayersResponse FromJson(string json) =>
                JsonConvert.DeserializeObject<OnlinePlayersResponse>(json, Converter.Settings);
        }

        public class OnlinePlayer
        {
            [JsonProperty("playerName")]
            public string Name { get; set; }

            [JsonProperty("playerRace")]
            public int Race { get; set; }

            [JsonProperty("playerGender")]
            public int Gender { get; set; }

            [JsonProperty("playerClass")]
            public int Class { get; set; }

            [JsonProperty("playerLevel")]
            public int Level { get; set; }

            [JsonProperty("zoneName")]
            public string Zone { get; set; }

            [JsonProperty("realmId")]
            public int RealmId { get; set; }

            [JsonProperty("realmName")]
            public string RealmName { get; set; }
        }

        public static async Task<string> GetOnlinePlayersResponse(string token, string md5username)
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "42" },
                { "token", token },
                { "md5username", md5username }
            });
        }

        #endregion
    }
}
