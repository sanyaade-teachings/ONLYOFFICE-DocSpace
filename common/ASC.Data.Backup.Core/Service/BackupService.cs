/*
 *
 * (c) Copyright Ascensio System Limited 2010-2020
 *
 * This program is freeware. You can redistribute it and/or modify it under the terms of the GNU 
 * General Public License (GPL) version 3 as published by the Free Software Foundation (https://www.gnu.org/copyleft/gpl.html). 
 * In accordance with Section 7(a) of the GNU GPL its Section 15 shall be amended to the effect that 
 * Ascensio System SIA expressly excludes the warranty of non-infringement of any third-party rights.
 *
 * THIS PROGRAM IS DISTRIBUTED WITHOUT ANY WARRANTY; WITHOUT EVEN THE IMPLIED WARRANTY OF MERCHANTABILITY OR
 * FITNESS FOR A PARTICULAR PURPOSE. For more details, see GNU GPL at https://www.gnu.org/copyleft/gpl.html
 *
 * You can contact Ascensio System SIA by email at sales@onlyoffice.com
 *
 * The interactive user interfaces in modified source and object code versions of ONLYOFFICE must display 
 * Appropriate Legal Notices, as required under Section 5 of the GNU GPL version 3.
 *
 * Pursuant to Section 7 § 3(b) of the GNU GPL you must retain the original ONLYOFFICE logo which contains 
 * relevant author attributions when distributing the software. If the display of the logo in its graphic 
 * form is not reasonably feasible for technical reasons, you must include the words "Powered by ONLYOFFICE" 
 * in every copy of the program you distribute. 
 * Pursuant to Section 7 § 3(e) we decline to grant you any rights under trademark law for use of our trademarks.
 *
*/

namespace ASC.Data.Backup.Services;

[Scope]
public class BackupService : IBackupService
{
    private readonly ILog _logger;
    private readonly BackupStorageFactory _backupStorageFactory;
    private readonly BackupWorker _backupWorker;
    private readonly BackupRepository _backupRepository;
    private readonly ConfigurationExtension _configuration;

    public BackupService(
        ILog logger,
        BackupStorageFactory backupStorageFactory,
        BackupWorker backupWorker,
        BackupRepository backupRepository,
        ConfigurationExtension configuration)
    {
        _logger = logger;
        _backupStorageFactory = backupStorageFactory;
        _backupWorker = backupWorker;
        _backupRepository = backupRepository;
        _configuration = configuration;
    }

    public void StartBackup(StartBackupRequest request)
    {
        var progress = _backupWorker.StartBackup(request);
        if (!string.IsNullOrEmpty(progress.Error))
        {
            throw new FaultException();
        }
    }

    public void DeleteBackup(Guid id)
    {
        var backupRecord = _backupRepository.GetBackupRecord(id);
        _backupRepository.DeleteBackupRecord(backupRecord.Id);

        var storage = _backupStorageFactory.GetBackupStorage(backupRecord);
        if (storage == null)
        {
            return;
        }

        storage.Delete(backupRecord.StoragePath);
    }

    public void DeleteAllBackups(int tenantId)
    {
        foreach (var backupRecord in _backupRepository.GetBackupRecordsByTenantId(tenantId))
        {
            try
            {
                _backupRepository.DeleteBackupRecord(backupRecord.Id);
                var storage = _backupStorageFactory.GetBackupStorage(backupRecord);
                if (storage == null)
                {
                    continue;
                }

                storage.Delete(backupRecord.StoragePath);
            }
            catch (Exception error)
            {
                _logger.Warn("error while removing backup record: {0}", error);
            }
        }
    }

    public List<BackupHistoryRecord> GetBackupHistory(int tenantId)
    {
        var backupHistory = new List<BackupHistoryRecord>();
        foreach (var record in _backupRepository.GetBackupRecordsByTenantId(tenantId))
        {
            var storage = _backupStorageFactory.GetBackupStorage(record);
            if (storage == null)
            {
                continue;
            }

            if (storage.IsExists(record.StoragePath))
            {
                backupHistory.Add(new BackupHistoryRecord
                {
                    Id = record.Id,
                    FileName = record.Name,
                    StorageType = record.StorageType,
                    CreatedOn = record.CreatedOn,
                    ExpiresOn = record.ExpiresOn
                });
            }
            else
            {
                _backupRepository.DeleteBackupRecord(record.Id);
            }
        }
        return backupHistory;
    }

    public void StartTransfer(StartTransferRequest request)
    {
        var progress = _backupWorker.StartTransfer(request.TenantId, request.TargetRegion, request.BackupMail, request.NotifyUsers);
        if (!string.IsNullOrEmpty(progress.Error))
        {
            throw new FaultException();
        }
    }

    public void StartRestore(StartRestoreRequest request)
    {
        if (request.StorageType == BackupStorageType.Local)
        {
            if (string.IsNullOrEmpty(request.FilePathOrId) || !File.Exists(request.FilePathOrId))
            {
                throw new FileNotFoundException();
            }
        }

        if (!request.BackupId.Equals(Guid.Empty))
        {
            var backupRecord = _backupRepository.GetBackupRecord(request.BackupId);
            if (backupRecord == null)
            {
                throw new FileNotFoundException();
            }

            request.FilePathOrId = backupRecord.StoragePath;
            request.StorageType = backupRecord.StorageType;
            request.StorageParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(backupRecord.StorageParams);
        }

        var progress = _backupWorker.StartRestore(request);
        if (!string.IsNullOrEmpty(progress.Error))
        {
            throw new FaultException();
        }
    }

    public BackupProgress GetBackupProgress(int tenantId)
    {
        return _backupWorker.GetBackupProgress(tenantId);
    }

    public BackupProgress GetTransferProgress(int tenantId)
    {
        return _backupWorker.GetTransferProgress(tenantId);
    }

    public BackupProgress GetRestoreProgress(int tenantId)
    {
        return _backupWorker.GetRestoreProgress(tenantId);
    }

    public string GetTmpFolder()
    {
        return _backupWorker.TempFolder;
    }

    public List<TransferRegion> GetTransferRegions()
    {
        var settings = _configuration.GetSetting<BackupSettings>("backup");

        return settings.WebConfigs.Elements.Select(configElement =>
        {
            var config = Utils.ConfigurationProvider.Open(PathHelper.ToRootedConfigPath(configElement.Path));
            var baseDomain = config.AppSettings.Settings["core:base-domain"].Value;

            return new TransferRegion
            {
                Name = configElement.Region,
                BaseDomain = baseDomain,
                IsCurrentRegion = configElement.Region.Equals(settings.WebConfigs.CurrentRegion, StringComparison.InvariantCultureIgnoreCase)
            };
        })
        .ToList();
    }

    public void CreateSchedule(CreateScheduleRequest request)
    {
        _backupRepository.SaveBackupSchedule(
            new BackupSchedule()
            {
                TenantId = request.TenantId,
                Cron = request.Cron,
                BackupMail = request.BackupMail,
                BackupsStored = request.NumberOfBackupsStored,
                StorageType = request.StorageType,
                StorageBasePath = request.StorageBasePath,
                StorageParams = JsonConvert.SerializeObject(request.StorageParams)
            });
    }

    public void DeleteSchedule(int tenantId)
    {
        _backupRepository.DeleteBackupSchedule(tenantId);
    }

    public ScheduleResponse GetSchedule(int tenantId)
    {
        var schedule = _backupRepository.GetBackupSchedule(tenantId);
        if (schedule != null)
        {
            var tmp = new ScheduleResponse
            {
                StorageType = schedule.StorageType,
                StorageBasePath = schedule.StorageBasePath,
                BackupMail = schedule.BackupMail,
                NumberOfBackupsStored = schedule.BackupsStored,
                Cron = schedule.Cron,
                LastBackupTime = schedule.LastBackupTime,
                StorageParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(schedule.StorageParams)
            };

            return tmp;
        }
        else
        {
            return null;
        }
    }
}
