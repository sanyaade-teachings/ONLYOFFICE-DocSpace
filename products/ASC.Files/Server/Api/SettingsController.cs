﻿// (c) Copyright Ascensio System SIA 2010-2022
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

using Module = ASC.Api.Core.Module;

namespace ASC.Files.Api;

public class SettingsController : ApiControllerBase
{
    private readonly FileStorageService<string> _fileStorageServiceString;
    private readonly FilesSettingsHelper _filesSettingsHelper;
    private readonly TenantManager _tenantManager;
    private readonly ProductEntryPoint _productEntryPoint;

    public SettingsController(
        FileStorageService<string> fileStorageServiceString,
        FilesSettingsHelper filesSettingsHelper,
        TenantManager tenantManager,
        ProductEntryPoint productEntryPoint)
    {
        _fileStorageServiceString = fileStorageServiceString;
        _filesSettingsHelper = filesSettingsHelper;
        _tenantManager = tenantManager;
        _productEntryPoint = productEntryPoint;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="set"></param>
    /// <returns></returns>
    [Update(@"thirdparty")]
    public bool ChangeAccessToThirdpartyFromBody([FromBody] SettingsRequestDto inDto)
    {
        return _fileStorageServiceString.ChangeAccessToThirdparty(inDto.Set);
    }

    [Update(@"thirdparty")]
    [Consumes("application/x-www-form-urlencoded")]
    public bool ChangeAccessToThirdpartyFromForm([FromForm] SettingsRequestDto inDto)
    {
        return _fileStorageServiceString.ChangeAccessToThirdparty(inDto.Set);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="set"></param>
    /// <returns></returns>
    [Update(@"changedeleteconfrim")]
    public bool ChangeDeleteConfrimFromBody([FromBody] SettingsRequestDto inDto)
    {
        return _fileStorageServiceString.ChangeDeleteConfrim(inDto.Set);
    }

    [Update(@"changedeleteconfrim")]
    [Consumes("application/x-www-form-urlencoded")]
    public bool ChangeDeleteConfrimFromForm([FromForm] SettingsRequestDto inDto)
    {
        return _fileStorageServiceString.ChangeDeleteConfrim(inDto.Set);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="set"></param>
    /// <category>Settings</category>
    /// <returns></returns>
    [Update(@"settings/downloadtargz")]
    public ICompress ChangeDownloadZipFromBody([FromBody] DisplayRequestDto inDto)
    {
        return _fileStorageServiceString.ChangeDownloadTarGz(inDto.Set);
    }

    [Update(@"settings/downloadtargz")]
    public ICompress ChangeDownloadZipFromForm([FromForm] DisplayRequestDto inDto)
    {
        return _fileStorageServiceString.ChangeDownloadTarGz(inDto.Set);
    }

    /// <summary>
    /// Display favorite folder
    /// </summary>
    /// <param name="set"></param>
    /// <category>Settings</category>
    /// <returns></returns>
    [Update(@"settings/favorites")]
    public bool DisplayFavoriteFromBody([FromBody] DisplayRequestDto inDto)
    {
        return _fileStorageServiceString.DisplayFavorite(inDto.Set);
    }

    [Update(@"settings/favorites")]
    [Consumes("application/x-www-form-urlencoded")]
    public bool DisplayFavoriteFromForm([FromForm] DisplayRequestDto inDto)
    {
        return _fileStorageServiceString.DisplayFavorite(inDto.Set);
    }

    /// <summary>
    /// Display recent folder
    /// </summary>
    /// <param name="set"></param>
    /// <category>Settings</category>
    /// <returns></returns>
    [Update(@"displayRecent")]
    public bool DisplayRecentFromBody([FromBody] DisplayRequestDto inDto)
    {
        return _fileStorageServiceString.DisplayRecent(inDto.Set);
    }

    [Update(@"displayRecent")]
    [Consumes("application/x-www-form-urlencoded")]
    public bool DisplayRecentFromForm([FromForm] DisplayRequestDto inDto)
    {
        return _fileStorageServiceString.DisplayRecent(inDto.Set);
    }

    /// <summary>
    /// Display template folder
    /// </summary>
    /// <param name="set"></param>
    /// <category>Settings</category>
    /// <returns></returns>
    [Update(@"settings/templates")]
    public bool DisplayTemplatesFromBody([FromBody] DisplayRequestDto inDto)
    {
        return _fileStorageServiceString.DisplayTemplates(inDto.Set);
    }

    [Update(@"settings/templates")]
    [Consumes("application/x-www-form-urlencoded")]
    public bool DisplayTemplatesFromForm([FromForm] DisplayRequestDto inDto)
    {
        return _fileStorageServiceString.DisplayTemplates(inDto.Set);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="set"></param>
    /// <returns></returns>
    [Update(@"forcesave")]
    public bool ForcesaveFromBody([FromBody] SettingsRequestDto inDto)
    {
        return _fileStorageServiceString.Forcesave(inDto.Set);
    }

    [Update(@"forcesave")]
    [Consumes("application/x-www-form-urlencoded")]
    public bool ForcesaveFromForm([FromForm] SettingsRequestDto inDto)
    {
        return _fileStorageServiceString.Forcesave(inDto.Set);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Read(@"settings")]
    public FilesSettingsHelper GetFilesSettings()
    {
        return _filesSettingsHelper;
    }

    [Read("info")]
    public Module GetModule()
    {
        _productEntryPoint.Init();
        return new Module(_productEntryPoint);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="save"></param>
    /// <visible>false</visible>
    /// <returns></returns>
    [Update(@"hideconfirmconvert")]
    public bool HideConfirmConvertFromBody([FromBody] HideConfirmConvertRequestDto inDto)
    {
        return _fileStorageServiceString.HideConfirmConvert(inDto.Save);
    }

    [Update(@"hideconfirmconvert")]
    [Consumes("application/x-www-form-urlencoded")]
    public bool HideConfirmConvertFromForm([FromForm] HideConfirmConvertRequestDto inDto)
    {
        return _fileStorageServiceString.HideConfirmConvert(inDto.Save);
    }

    [Read("@privacy/available")]
    public bool IsAvailablePrivacyRoomSettings()
    {
        return PrivacyRoomSettings.IsAvailable(_tenantManager);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="set"></param>
    /// <returns></returns>
    [Update(@"storeforcesave")]
    public bool StoreForcesaveFromBody([FromBody] SettingsRequestDto inDto)
    {
        return _fileStorageServiceString.StoreForcesave(inDto.Set);
    }

    [Update(@"storeforcesave")]
    [Consumes("application/x-www-form-urlencoded")]
    public bool StoreForcesaveFromForm([FromForm] SettingsRequestDto inDto)
    {
        return _fileStorageServiceString.StoreForcesave(inDto.Set);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="set"></param>
    /// <returns></returns>
    [Update(@"storeoriginal")]
    public bool StoreOriginalFromBody([FromBody] SettingsRequestDto inDto)
    {
        return _fileStorageServiceString.StoreOriginal(inDto.Set);
    }

    [Update(@"storeoriginal")]
    [Consumes("application/x-www-form-urlencoded")]
    public bool StoreOriginalFromForm([FromForm] SettingsRequestDto inDto)
    {
        return _fileStorageServiceString.StoreOriginal(inDto.Set);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="set"></param>
    /// <returns></returns>
    [Update(@"updateifexist")]
    public bool UpdateIfExistFromBody([FromBody] SettingsRequestDto inDto)
    {
        return _fileStorageServiceString.UpdateIfExist(inDto.Set);
    }

    [Update(@"updateifexist")]
    [Consumes("application/x-www-form-urlencoded")]
    public bool UpdateIfExistFromForm([FromForm] SettingsRequestDto inDto)
    {
        return _fileStorageServiceString.UpdateIfExist(inDto.Set);
    }
}