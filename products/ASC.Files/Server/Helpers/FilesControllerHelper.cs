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

namespace ASC.Files.Helpers;

public class FilesControllerHelper<T> : FilesHelperBase<T>
{
    private readonly ILog _logger;
    private readonly ApiDateTimeHelper _apiDateTimeHelper;
    private readonly UserManager _userManager;
    private readonly DisplayUserSettingsHelper _displayUserSettingsHelper;
    private readonly IServiceProvider _serviceProvider;
    private readonly FileConverter _fileConverter;
    private readonly FileOperationDtoHelper _fileOperationDtoHelper;

    public FilesControllerHelper(
        FilesSettingsHelper filesSettingsHelper,
        FileUploader fileUploader,
        SocketManager socketManager,
        FileDtoHelper fileDtoHelper,
        ApiContext apiContext,
        FileStorageService<T> fileStorageService,
        FolderContentDtoHelper folderContentDtoHelper,
        IHttpContextAccessor httpContextAccessor,
        FolderDtoHelper folderDtoHelper,
        ILog logger,
        ApiDateTimeHelper apiDateTimeHelper,
        UserManager userManager,
        DisplayUserSettingsHelper displayUserSettingsHelper,
        IServiceProvider serviceProvider,
        FileConverter fileConverter,
        FileOperationDtoHelper fileOperationDtoHelper) 
        : base(
            filesSettingsHelper,
            fileUploader,
            socketManager,
            fileDtoHelper,
            apiContext,
            fileStorageService,
            folderContentDtoHelper,
            httpContextAccessor,
            folderDtoHelper)
    {
        _logger = logger;
        _apiDateTimeHelper = apiDateTimeHelper;
        _fileConverter = fileConverter;
        _userManager = userManager;
        _serviceProvider = serviceProvider;
        _displayUserSettingsHelper = displayUserSettingsHelper;
        _fileOperationDtoHelper = fileOperationDtoHelper;
    }

    public async Task<IEnumerable<FileDto<T>>> ChangeHistoryAsync(T fileId, int version, bool continueVersion)
    {
        var pair = await _fileStorageService.CompleteVersionAsync(fileId, version, continueVersion);
        var history = pair.Value;

        var result = new List<FileDto<T>>();

        foreach (var e in history)
        {
            result.Add(await _fileDtoHelper.GetAsync(e));
        }

        return result;
    }

    public async IAsyncEnumerable<ConversationResultDto<T>> CheckConversionAsync(CheckConversionRequestDto<T> cheqConversionRequestDto)
    {
        var checkConversaion = _fileStorageService.CheckConversionAsync(new List<CheckConversionRequestDto<T>>() { cheqConversionRequestDto }, cheqConversionRequestDto.Sync);

        await foreach (var r in checkConversaion)
        {
            var o = new ConversationResultDto<T>
            {
                Id = r.Id,
                Error = r.Error,
                OperationType = r.OperationType,
                Processed = r.Processed,
                Progress = r.Progress,
                Source = r.Source,
            };

            if (!string.IsNullOrEmpty(r.Result))
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        AllowTrailingCommas = true,
                        PropertyNameCaseInsensitive = true
                    };

                    var jResult = JsonSerializer.Deserialize<FileJsonSerializerData<T>>(r.Result, options);
                    o.File = await GetFileInfoAsync(jResult.Id, jResult.Version);
                }
                catch (Exception e)
                {
                    o.File = r.Result;
                    _logger.Error(e);
                }
            }

            yield return o;
        }
    }

    public async Task<FileDto<T>> CreateFileAsync(T folderId, string title, JsonElement templateId, bool enableExternalExt = false)
    {
        File<T> file;

        if (templateId.ValueKind == JsonValueKind.Number)
        {
            file = await _fileStorageService.CreateNewFileAsync(new FileModel<T, int> { ParentId = folderId, Title = title, TemplateId = templateId.GetInt32() }, enableExternalExt);
        }
        else if (templateId.ValueKind == JsonValueKind.String)
        {
            file = await _fileStorageService.CreateNewFileAsync(new FileModel<T, string> { ParentId = folderId, Title = title, TemplateId = templateId.GetString() }, enableExternalExt);
        }
        else
        {
            file = await _fileStorageService.CreateNewFileAsync(new FileModel<T, int> { ParentId = folderId, Title = title, TemplateId = 0 }, enableExternalExt);
        }

        return await _fileDtoHelper.GetAsync(file);
    }

    public Task<FileDto<T>> CreateHtmlFileAsync(T folderId, string title, string content)
    {
        ArgumentNullException.ThrowIfNull(title);

        return CreateFileAsync(folderId, title, content, ".html");
    }

    public Task<FileDto<T>> CreateTextFileAsync(T folderId, string title, string content)
    {
        ArgumentNullException.ThrowIfNull(title);

        //Try detect content
        var extension = ".txt";
        if (!string.IsNullOrEmpty(content))
        {
            if (Regex.IsMatch(content, @"<([^\s>]*)(\s[^<]*)>"))
            {
                extension = ".html";
            }
        }

        return CreateFileAsync(folderId, title, content, extension);
    }

    private async Task<FileDto<T>> CreateFileAsync(T folderId, string title, string content, string extension)
    {
        using var memStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var file = await _fileUploader.ExecAsync(folderId,
                          title.EndsWith(extension, StringComparison.OrdinalIgnoreCase) ? title : (title + extension),
                          memStream.Length, memStream);

        return await _fileDtoHelper.GetAsync(file);
    }

    public Task<EditHistoryDataDto> GetEditDiffUrlAsync(T fileId, int version = 0, string doc = null)
    {
        return _fileStorageService.GetEditDiffUrlAsync(fileId, version, doc);
    }

    public async Task<List<EditHistoryDto>> GetEditHistoryAsync(T fileId, string doc = null)
    {
        var result = await _fileStorageService.GetEditHistoryAsync(fileId, doc);

        return result.Select(r => new EditHistoryDto(r, _apiDateTimeHelper, _userManager, _displayUserSettingsHelper)).ToList();
    }

    public async Task<IEnumerable<FileDto<T>>> GetFileVersionInfoAsync(T fileId)
    {
        var files = await _fileStorageService.GetFileHistoryAsync(fileId);
        var result = new List<FileDto<T>>();

        foreach (var e in files)
        {
            result.Add(await _fileDtoHelper.GetAsync(e));
        }

        return result;
    }

    public async Task<FileDto<T>> LockFileAsync(T fileId, bool lockFile)
    {
        var result = await _fileStorageService.LockFileAsync(fileId, lockFile);

        return await _fileDtoHelper.GetAsync(result);
    }

    public async Task<List<EditHistoryDto>> RestoreVersionAsync(T fileId, int version = 0, string url = null, string doc = null)
    {
        var result = await _fileStorageService.RestoreVersionAsync(fileId, version, url, doc);

        return result.Select(r => new EditHistoryDto(r, _apiDateTimeHelper, _userManager, _displayUserSettingsHelper)).ToList();
    }

    public IAsyncEnumerable<ConversationResultDto<T>> StartConversionAsync(CheckConversionRequestDto<T> cheqConversionRequestDto)
    {
        cheqConversionRequestDto.StartConvert = true;

        return CheckConversionAsync(cheqConversionRequestDto);
    }

    public Task<string> UpdateCommentAsync(T fileId, int version, string comment)
    {
        return _fileStorageService.UpdateCommentAsync(fileId, version, comment);
    }

    public async Task<FileDto<T>> UpdateFileAsync(T fileId, string title, int lastVersion)
    {
        if (!string.IsNullOrEmpty(title))
        {
            await _fileStorageService.FileRenameAsync(fileId, title);
        }

        if (lastVersion > 0)
        {
            await _fileStorageService.UpdateToVersionAsync(fileId, lastVersion);
        }

        return await GetFileInfoAsync(fileId);
    }

    public async Task<FileDto<T>> UpdateFileStreamAsync(Stream file, T fileId, string fileExtension, bool encrypted = false, bool forcesave = false)
    {
        try
        {
            var resultFile = await _fileStorageService.UpdateFileStreamAsync(fileId, file, fileExtension, encrypted, forcesave);

            return await _fileDtoHelper.GetAsync(resultFile);
        }
        catch (FileNotFoundException e)
        {
            throw new ItemNotFoundException("File not found", e);
        }
    }

    public async Task<FileDto<TTemplate>> CopyFileAsAsync<TTemplate>(T fileId, TTemplate destFolderId, string destTitle, string password = null)
    {
        var service = _serviceProvider.GetService<FileStorageService<TTemplate>>();
        var controller = _serviceProvider.GetService<FilesControllerHelper<TTemplate>>();
        var file = await _fileStorageService.GetFileAsync(fileId, -1);
        var ext = FileUtility.GetFileExtension(file.Title);
        var destExt = FileUtility.GetFileExtension(destTitle);

        if (ext == destExt)
        {
            var newFile = await service.CreateNewFileAsync(new FileModel<TTemplate, T> { ParentId = destFolderId, Title = destTitle, TemplateId = fileId }, false);

            return await _fileDtoHelper.GetAsync(newFile);
        }

        using (var fileStream = await _fileConverter.ExecAsync(file, destExt, password))
        {
            return await controller.InsertFileAsync(destFolderId, fileStream, destTitle, true);
        }
    }

    public async Task<IEnumerable<FileOperationDto>> DeleteFileAsync(T fileId, bool deleteAfter, bool immediately)
    {
        var result = new List<FileOperationDto>();

        foreach (var e in _fileStorageService.DeleteFile("delete", fileId, false, deleteAfter, immediately))
        {
            result.Add(await _fileOperationDtoHelper.GetAsync(e));
        }

        return result;
    }
}
