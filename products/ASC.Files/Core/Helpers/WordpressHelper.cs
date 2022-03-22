// (c) Copyright Ascensio System SIA 2010-2022
//
// This program is a free software product.
// You can redistribute it and/or modify it under the terms
// of the GNU Affero General Public License (AGPL) version 3 as published by the Free Software
// Foundation. In accordance with Section 7(a) of the GNU AGPL its Section 15 shall be amended
// to the effect that Ascensio System SIA expressly excludes the warranty of non-infringement of
// any third-party rights.
//
// This program is distributed WITHOUT ANY WARRANTY, without even the implied warranty
// of MERCHANTABILITY or FITNESS FOR A PARTICULAR  PURPOSE. For details, see
// the GNU AGPL at: http://www.gnu.org/licenses/agpl-3.0.html
//
// You can contact Ascensio System SIA at Lubanas st. 125a-25, Riga, Latvia, EU, LV-1021.
//
// The  interactive user interfaces in modified source and object code versions of the Program must
// display Appropriate Legal Notices, as required under Section 5 of the GNU AGPL version 3.
//
// Pursuant to Section 7(b) of the License you must retain the original Product logo when
// distributing the program. Pursuant to Section 7(e) we decline to grant you any rights under
// trademark law for use of our trademarks.
//
// All the Product's GUI elements, including illustrations and icon sets, as well as technical writing
// content are licensed under the terms of the Creative Commons Attribution-ShareAlike 4.0
// International. See the License terms at http://creativecommons.org/licenses/by-sa/4.0/legalcode

namespace ASC.Web.Files.Helpers;

[Scope]
public class WordpressToken
{
    public ILog Logger { get; set; }
    private readonly TokenHelper _tokenHelper;
    public ConsumerFactory ConsumerFactory { get; }

        private readonly OAuth20TokenHelper _oAuth20TokenHelper;

    public const string AppAttr = "wordpress";

        public WordpressToken(IOptionsMonitor<ILog> optionsMonitor, TokenHelper tokenHelper, ConsumerFactory consumerFactory, OAuth20TokenHelper oAuth20TokenHelper)
    {
        Logger = optionsMonitor.CurrentValue;
        _tokenHelper = tokenHelper;
        ConsumerFactory = consumerFactory;
            _oAuth20TokenHelper = oAuth20TokenHelper;
    }

    public OAuth20Token GetToken()
    {
        return _tokenHelper.GetToken(AppAttr);
    }

    public void SaveToken(OAuth20Token token)
    {
        ArgumentNullException.ThrowIfNull(token);

        _tokenHelper.SaveToken(new Token(token, AppAttr));
    }

    public OAuth20Token SaveTokenFromCode(string code)
    {
            var token = _oAuth20TokenHelper.GetAccessToken<WordpressLoginProvider>(ConsumerFactory, code);
        ArgumentNullException.ThrowIfNull(token);

        _tokenHelper.SaveToken(new Token(token, AppAttr));

        return token;
    }

    public void DeleteToken(OAuth20Token token)
    {
        ArgumentNullException.ThrowIfNull(token);

        _tokenHelper.DeleteToken(AppAttr);
    }
}

[Singletone]
public class WordpressHelper
{
    public ILog Logger { get; set; }
    public RequestHelper RequestHelper { get; }

    public enum WordpressStatus
    {
        draft = 0,
        publish = 1
    }

    public WordpressHelper(IOptionsMonitor<ILog> optionsMonitor, RequestHelper requestHelper)
    {
        Logger = optionsMonitor.CurrentValue;
        RequestHelper = requestHelper;
    }

    public string GetWordpressMeInfo(string token)
    {
        try
        {
                return WordpressLoginProvider.GetWordpressMeInfo(RequestHelper, token);
        }
        catch (Exception ex)
        {
            Logger.Error("Get Wordpress info about me ", ex);

            return string.Empty;
        }

    }

    public bool CreateWordpressPost(string title, string content, int status, string blogId, OAuth20Token token)
    {
        try
        {
            var wpStatus = ((WordpressStatus)status).ToString();
                WordpressLoginProvider.CreateWordpressPost(RequestHelper, title, content, wpStatus, blogId, token);

            return true;
        }
        catch (Exception ex)
        {
            Logger.Error("Create Wordpress post ", ex);

            return false;
        }
    }
}
