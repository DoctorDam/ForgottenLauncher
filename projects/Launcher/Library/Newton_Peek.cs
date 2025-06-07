/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace Forgotten_Land_Launcher.Library
{
    internal class Newton_Peek
    {
        public static async Task<List<Newton_Main.SliderResponse>> SliderResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetSliderResponse();
                return Newton_Main.SliderResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.LoginResponse> LoginResponse(string username_or_email, string password)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetLoginResponse(username_or_email, password);
                return Newton_Main.LoginResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.AuthTokenResponse> AuthTokenResponse(string token, string md5username)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetAuthTokenResponse(token, md5username);
                return Newton_Main.AuthTokenResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.RegisterResponse> RegisterResponse(string username, string email, string password1, string password2)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetRegisterResponse(username, email, password1, password2);
                return Newton_Main.RegisterResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task SendPasswordRecoveryRequest(string email)
        {
            try
            {
                await Newton_Main.SendPasswordRecoveryRequest(email);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), "method doesn't have a return", null, ex, true);
            }
        }

        public static async Task<Newton_Main.PasswordChangeByCodeResponse> PasswordChangeByCodeResponse(string username, string code, string password1, string password2)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetPasswordChangeByCodeResponse(username, code, password1, password2);
                return Newton_Main.PasswordChangeByCodeResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.AccountCurrenciesResponse> AccountCurrenciesResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetAccountCurrenciesResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.AccountCurrenciesResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.BattlePayCreditsEnableStatusResponse> BattlePayCreditsEnableStatusResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetBattlePayCreditsEnableStatusResponse();
                return Newton_Main.BattlePayCreditsEnableStatusResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.EmulatorIDResponse> EmulatorIDResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetEmulatorIDResponse();
                return Newton_Main.EmulatorIDResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.RealmlistResponse> RealmlistResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetRealmlistResponse();
                return Newton_Main.RealmlistResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.RegistrationTOSResponse> RegistrationTOSResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetRegistrationTOSResponse();
                return Newton_Main.RegistrationTOSResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.DiscordServerInfoResponse> DiscordServerInfoResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetDiscordServerInfoResponse();
                return Newton_Main.DiscordServerInfoResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, false);
            }

            return null;
        }

        public static async Task<Newton_Main.AvatarUpdateResponse> AvatarUpdateResponse(string image_url)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetAvatarUpdateResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    image_url
                );
                return Newton_Main.AvatarUpdateResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.WebsiteArticlesResponse>> LauncherArticlesResponse(int position)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetWebsiteArticlesResponse
                (
                    Cache.AuthToken,
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.WebsiteArticlesResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.LauncherEventsResponse>> LauncherEventsResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetLauncherEventsResponse
                (
                    Cache.AuthToken,
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.LauncherEventsResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.WebsiteChangelogsResponse>> LauncherChangelogsResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetWebsiteChangelogsResponse
                (
                    Cache.AuthToken,
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.WebsiteChangelogsResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.LauncherFaqResponse>> LauncherFaqResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetLauncherFaqResponse
                (
                    Cache.AuthToken,
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.LauncherFaqResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.LoginRewardsResponse>> LoginRewardsResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetLoginRewardsResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.LoginRewardsResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.ClaimLoginRewardResponse> ClaimLoginRewardResponse(int month, int day)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetClaimLoginRewardResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    month.ToString(), 
                    day.ToString()
                );
                return Newton_Main.ClaimLoginRewardResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.AccountInventoryResponse>> AccountInventoryResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetAccountInventoryResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.AccountInventoryResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.CharactersListResponse>> CharactersListResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetCharactersListResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.CharactersListResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.UseInventoryItemResponse> UseInventoryItemResponse(int guid, int reward_id, int realm_id)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetUseInventoryItemResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    guid.ToString(), 
                    reward_id.ToString(), 
                    realm_id.ToString()
                );
                return Newton_Main.UseInventoryItemResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.GameFilesListResponse>> GameFilesListResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetGameFilesListResponse();
                return Newton_Main.GameFilesListResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.AddonsListResponse>> AddonsListResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetAddonsListResponse();
                return Newton_Main.AddonsListResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.LauncherUpdateFilesListResponse>> LauncherUpdateFilesListResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetLauncherUpdateFilesListResponseResponse();
                return Newton_Main.LauncherUpdateFilesListResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.UpdatePublicNicknameResponse> UpdatePublicNicknameResponse(string nickname)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetUpdatePublicNicknameResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    nickname
                );
                return Newton_Main.UpdatePublicNicknameResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.PublicNicknameResponse> PublicNicknameResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetPublicNicknameResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.PublicNicknameResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.CharacterUnstuckResponse> CharacterUnstuckResponse(int guid, int realm_id)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetCharacterUnstuckResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    guid.ToString(), 
                    realm_id.ToString()
                );
                return Newton_Main.CharacterUnstuckResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.TeleportListResponse>> TeleportListResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetTeleportListResponse();
                return Newton_Main.TeleportListResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.CharacterTeleportResponse> CharacterTeleportResponse(int guid, int realm_id, int teleport_id)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetCharacterTeleportResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    guid.ToString(), 
                    realm_id.ToString(), 
                    teleport_id.ToString()
                );
                return Newton_Main.CharacterTeleportResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.CharactersTicketsListResponse>> CharactersTicketsListResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetCharactersTicketsListResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.CharactersTicketsListResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.LadderboardResponse>> LadderboardResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetLadderboardResponse
                (
                    Cache.AuthToken,
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.LadderboardResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.PrivateMessagesListResponse>> PrivateMessagesListResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetPrivateMessagesListResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.PrivateMessagesListResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.NewPrivateMessageResponse> NewPrivateMessageResponse(string receiver, string title, string content)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetNewPrivateMessageResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    receiver, 
                    title, 
                    content
                );
                return Newton_Main.NewPrivateMessageResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.PrivateMessageThreadResponse>> PrivateMessageThreadResponse(int message_id)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetPrivateMessageThreadResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    message_id.ToString()
                );
                return Newton_Main.PrivateMessageThreadResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.NewMessageReplyResponse> NewMessageReplyResponse(int message_id, string content)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetNewMessageReplyResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    message_id.ToString(), 
                    content
                );
                return Newton_Main.NewMessageReplyResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.DeletePrivateMessageResponse> DeletePrivateMessageResponse(int message_id)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetDeletePrivateMessageResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    message_id.ToString()
                );
                return Newton_Main.DeletePrivateMessageResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.RedeemGiftCodeResponse> RedeemGiftCodeResponse(string gift_code)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetRedeemGiftCodeResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    gift_code
                );
                return Newton_Main.RedeemGiftCodeResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.GiftPreview> GiftPreview(string gift_code)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetGiftPreview
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    gift_code
                );
                return Newton_Main.GiftPreview.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.ShopListResponse>> ShopListResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetShopListResponse
                (
                    Cache.AuthToken,
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.ShopListResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.ShopPurchaseResponse> ShopPurchaseResponse(int id)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetShopPurchaseResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    id.ToString()
                );
                return Newton_Main.ShopPurchaseResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.VoteSitesResponse>> VoteSitesResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetVoteSitesResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.VoteSitesResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.VoteResponse> VoteResponse(int site_id)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetVoteResponse
                (
                    Cache.AuthToken,
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    site_id.ToString()
                );
                return Newton_Main.VoteResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.RealmsStatusResponse>> RealmsStatusResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetRealmsStatusResponse();
                return Newton_Main.RealmsStatusResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.PagesOptions> PagesOptions()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetPagesOptions();
                return Newton_Main.PagesOptions.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.CharactersMarketList>> CharactersMarketList()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetCharactersMarketList
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.CharactersMarketList.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<List<Newton_Main.CharactersMarketBidsNotifications>> CharactersMarketBidsNotifications()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetCharactersMarketBidsNotifications
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.CharactersMarketBidsNotifications.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.CharactersMarketSettings> CharactersMarketSettings()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetCharactersMarketSettings();
                return Newton_Main.CharactersMarketSettings.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.CharactersMarketSellResponse> CharactersMarketSellResponse(int char_guid, int realm_id, int price, bool allow_bidding, int duration)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetCharactersMarketSellResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username),
                    char_guid.ToString(),
                    realm_id.ToString(),
                    price.ToString(),
                    allow_bidding.ToString(),
                    duration.ToString()
                );
                return Newton_Main.CharactersMarketSellResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.CharactersMarketCancelResponse> CharactersMarketCancelResponse(int sale_id)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetCharactersMarketCancelResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    sale_id.ToString()
                );
                return Newton_Main.CharactersMarketCancelResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.CharactersMarketBuyResponse> CharactersMarketBuyResponse(int sale_id)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetCharactersMarketBuyResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    sale_id.ToString()
                );

                return Newton_Main.CharactersMarketBuyResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.CharactersMarketBidResponse> CharactersMarketBidResponse(int sale_id, int bid_amount)
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetCharactersMarketBidResponse
                (
                    Cache.AuthToken, 
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username), 
                    sale_id.ToString(),
                    bid_amount.ToString()
                );

                return Newton_Main.CharactersMarketBidResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.AccountBanStatus> AccountBanStatus()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetAccountBanStatus
                (
                    Cache.AuthToken,
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.AccountBanStatus.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.AccountActiveStatus> AccountActiveStatus()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetAccountActiveStatus
                (
                    Cache.AuthToken,
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.AccountActiveStatus.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.DiscordRPCOnlineCharacter> DiscordRPCOnlineCharacter()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetDiscordRPCOnlineCharacter
                (
                    Cache.AuthToken,
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.DiscordRPCOnlineCharacter.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }

        public static async Task<Newton_Main.OnlinePlayersResponse> OnlinePlayersResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetOnlinePlayersResponse
                (
                    Cache.AuthToken,
                    Codex_Standard.ToVendettaWho(Cache.AccountInfo.Username)
                );
                return Newton_Main.OnlinePlayersResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, json, ex, true);
            }

            return null;
        }
    }
}
