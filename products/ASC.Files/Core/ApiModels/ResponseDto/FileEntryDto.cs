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

namespace ASC.Files.Core.ApiModels.ResponseDto;

public abstract class FileEntryDto
{
    public string Title { get; set; }
    public FileShare Access { get; set; }
    public bool Shared { get; set; }
    public ApiDateTime Created { get; set; }
    public EmployeeDto CreatedBy { get; set; }

    private ApiDateTime _updated;
    public ApiDateTime Updated
    {
        get => _updated < Created ? Created : _updated;
        set => _updated = value;
    }

    public FolderType RootFolderType { get; set; }
    public EmployeeDto UpdatedBy { get; set; }
    public bool? ProviderItem { get; set; }
    public string ProviderKey { get; set; }
    public int? ProviderId { get; set; }

    protected FileEntryDto(FileEntry entry, EmployeeDtoHelper employeeWraperHelper, ApiDateTimeHelper apiDateTimeHelper)
    {
        Title = entry.Title;
        Access = entry.Access;
        Shared = entry.Shared;
        Created = apiDateTimeHelper.Get(entry.CreateOn);
        CreatedBy = employeeWraperHelper.Get(entry.CreateBy);
        Updated = apiDateTimeHelper.Get(entry.ModifiedOn);
        UpdatedBy = employeeWraperHelper.Get(entry.ModifiedBy);
        RootFolderType = entry.RootFolderType;
        ProviderItem = entry.ProviderEntry.NullIfDefault();
        ProviderKey = entry.ProviderKey;
        ProviderId = entry.ProviderId.NullIfDefault();
    }

    protected FileEntryDto() { }
}

public abstract class FileEntryWrapper<T> : FileEntryDto
{
    public T Id { get; set; }
    public T RootFolderId { get; set; }
    public bool CanShare { get; set; }
    public bool CanEdit { get; set; }

    protected FileEntryWrapper(FileEntry<T> entry, EmployeeDtoHelper employeeWraperHelper, ApiDateTimeHelper apiDateTimeHelper)
        : base(entry, employeeWraperHelper, apiDateTimeHelper)
    {
        Id = entry.ID;
        RootFolderId = entry.RootFolderId;
    }

    protected FileEntryWrapper() { }
}

[Scope]
public class FileEntryDtoHelper
{
    private readonly ApiDateTimeHelper _apiDateTimeHelper;
    private readonly EmployeeDtoHelper _employeeWraperHelper;
    public readonly FileSharingHelper _fileSharingHelper;
    public readonly FileSecurity _fileSecurity;

    public FileEntryDtoHelper(
        ApiDateTimeHelper apiDateTimeHelper,
        EmployeeDtoHelper employeeWraperHelper,
        FileSharingHelper fileSharingHelper, FileSecurity fileSecurity
        )
    {
        _apiDateTimeHelper = apiDateTimeHelper;
        _employeeWraperHelper = employeeWraperHelper;
        _fileSharingHelper = fileSharingHelper;
        _fileSecurity = fileSecurity;
    }

    protected internal async Task<T> GetAsync<T, TId>(FileEntry<TId> entry) where T : FileEntryWrapper<TId>, new()
    {
        return new T
        {
            Id = entry.ID,
            Title = entry.Title,
            Access = entry.Access,
            Shared = entry.Shared,
            Created = _apiDateTimeHelper.Get(entry.CreateOn),
            CreatedBy = _employeeWraperHelper.Get(entry.CreateBy),
            Updated = _apiDateTimeHelper.Get(entry.ModifiedOn),
            UpdatedBy = _employeeWraperHelper.Get(entry.ModifiedBy),
            RootFolderType = entry.RootFolderType,
            RootFolderId = entry.RootFolderId,
            ProviderItem = entry.ProviderEntry.NullIfDefault(),
            ProviderKey = entry.ProviderKey,
            ProviderId = entry.ProviderId.NullIfDefault(),
            CanShare = await _fileSharingHelper.CanSetAccessAsync(entry),
            CanEdit = await _fileSecurity.CanEditAsync(entry)
        };
    }
}
