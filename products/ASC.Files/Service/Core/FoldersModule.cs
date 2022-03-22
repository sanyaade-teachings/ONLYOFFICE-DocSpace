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

using FeedModule = ASC.Feed.Aggregator.Modules.FeedModule;

namespace ASC.Files.Service.Core;

public class FoldersModule : FeedModule
{
    public override Guid ProductID => WebItemManager.DocumentsProductID;
    public override string Name => Constants.FoldersModule;
    public override string Product => "documents";
    protected override string DbId => Constants.FilesDbId;

    private const string _folderItem = "folder";
    private const string _sharedFolderItem = "sharedFolder";

    private readonly FileSecurity _fileSecurity;
    private readonly FilesLinkUtility _filesLinkUtility;
    private readonly IFolderDao<int> _folderDao;
    private readonly UserManager _userManager;

    public FoldersModule(
        TenantManager tenantManager,
        UserManager userManager,
        WebItemSecurity webItemSecurity,
        FilesLinkUtility filesLinkUtility,
        FileSecurity fileSecurity,
        IDaoFactory daoFactory)
        : base(tenantManager, webItemSecurity)
    {
        _userManager = userManager;
        _filesLinkUtility = filesLinkUtility;
        _fileSecurity = fileSecurity;
        _folderDao = daoFactory.GetFolderDao<int>();
    }

    public override bool VisibleFor(Feed.Aggregator.Feed feed, object data, Guid userId)
    {
        if (!WebItemSecurity.IsAvailableForUser(ProductID, userId))
        {
            return false;
        }

        var tuple = (Tuple<Folder<int>, SmallShareRecord>)data;
        var folder = tuple.Item1;
        var shareRecord = tuple.Item2;

        bool targetCond;
        if (feed.Target != null)
        {
            if (shareRecord != null && shareRecord.ShareBy == userId)
            {
                return false;
            }

            var owner = (Guid)feed.Target;
            var groupUsers = _userManager.GetUsersByGroup(owner).Select(x => x.Id).ToList();
            if (groupUsers.Count == 0)
            {
                groupUsers.Add(owner);
            }

            targetCond = groupUsers.Contains(userId);
        }
        else
        {
            targetCond = true;
        }

        return targetCond && _fileSecurity.CanReadAsync(folder, userId).Result;
    }

    public override IEnumerable<int> GetTenantsWithFeeds(DateTime fromTime)
    {
        return _folderDao.GetTenantsWithFeedsForFoldersAsync(fromTime).Result;
    }

    public override IEnumerable<Tuple<Feed.Aggregator.Feed, object>> GetFeeds(FeedFilter filter)
    {
        var folders = _folderDao.GetFeedsForFoldersAsync(filter.Tenant, filter.Time.From, filter.Time.To).Result
                    .Where(f => f.Item1.RootFolderType != FolderType.TRASH && f.Item1.RootFolderType != FolderType.BUNCH)
                    .ToList();

        var parentFolderIDs = folders.Select(r => r.Item1.FolderID).ToList();
        var parentFolders = _folderDao.GetFoldersAsync(parentFolderIDs, checkShare: false).ToListAsync().Result;

        return folders.Select(f => new Tuple<Feed.Aggregator.Feed, object>(ToFeed(f, parentFolders.FirstOrDefault(r => r.ID.Equals(f.Item1.FolderID))), f));
    }

    private Feed.Aggregator.Feed ToFeed((Folder<int>, SmallShareRecord) tuple, Folder<int> rootFolder)
    {
        var folder = tuple.Item1;
        var shareRecord = tuple.Item2;

        if (shareRecord != null)
        {
            var feed = new Feed.Aggregator.Feed(shareRecord.ShareBy, shareRecord.ShareOn, true)
            {
                Item = _sharedFolderItem,
                ItemId = string.Format("{0}_{1}", folder.ID, shareRecord.ShareTo),
                ItemUrl = _filesLinkUtility.GetFileRedirectPreviewUrl(folder.ID, false),
                Product = Product,
                Module = Name,
                Title = folder.Title,
                ExtraLocation = rootFolder.FolderType == FolderType.DEFAULT ? rootFolder.Title : string.Empty,
                ExtraLocationUrl = rootFolder.FolderType == FolderType.DEFAULT ? _filesLinkUtility.GetFileRedirectPreviewUrl(folder.FolderID, false) : string.Empty,
                Keywords = folder.Title,
                HasPreview = false,
                CanComment = false,
                Target = shareRecord.ShareTo,
                GroupId = GetGroupId(_sharedFolderItem, shareRecord.ShareBy, folder.FolderID.ToString())
            };

            return feed;
        }

        return new Feed.Aggregator.Feed(folder.CreateBy, folder.CreateOn)
        {
            Item = _folderItem,
            ItemId = folder.ID.ToString(),
            ItemUrl = _filesLinkUtility.GetFileRedirectPreviewUrl(folder.ID, false),
            Product = Product,
            Module = Name,
            Title = folder.Title,
            ExtraLocation = rootFolder.FolderType == FolderType.DEFAULT ? rootFolder.Title : string.Empty,
            ExtraLocationUrl = rootFolder.FolderType == FolderType.DEFAULT ? _filesLinkUtility.GetFileRedirectPreviewUrl(folder.FolderID, false) : string.Empty,
            Keywords = folder.Title,
            HasPreview = false,
            CanComment = false,
            Target = null,
            GroupId = GetGroupId(_folderItem, folder.CreateBy, folder.FolderID.ToString())
        };
    }
}
