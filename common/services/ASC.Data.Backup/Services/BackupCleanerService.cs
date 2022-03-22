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

namespace ASC.Data.Backup.Services;

[Singletone]
internal sealed class BackupCleanerService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly ILog _logger;
    private readonly TimeSpan _period;

    private readonly IServiceScopeFactory _scopeFactory;

    public BackupCleanerService(
        ConfigurationExtension configuration,
        IOptionsMonitor<ILog> options,
        IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _logger = options.CurrentValue;
        _period = configuration.GetSetting<BackupSettings>("backup").Cleaner.Period;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Info("starting backup cleaner service...");

        _timer = new Timer(DeleteExpiredBackups, null, TimeSpan.Zero, _period);

        _logger.Info("backup cleaner service started");

        return Task.CompletedTask;
    }

    public void DeleteExpiredBackups(object state)
    {
        using var serviceScope = _scopeFactory.CreateScope();

        var backupRepository = serviceScope.ServiceProvider.GetRequiredService<BackupRepository>();
        var backupStorageFactory = serviceScope.ServiceProvider.GetRequiredService<BackupStorageFactory>();

        _logger.Debug("started to clean expired backups");

        var backupsToRemove = backupRepository.GetExpiredBackupRecords();

        _logger.DebugFormat("found {0} backups which are expired", backupsToRemove.Count);

        foreach (var scheduledBackups in backupRepository.GetScheduledBackupRecords().GroupBy(r => r.TenantId))
        {
            var schedule = backupRepository.GetBackupSchedule(scheduledBackups.Key);

            if (schedule != null)
            {
                var scheduledBackupsToRemove = scheduledBackups.OrderByDescending(r => r.CreatedOn).Skip(schedule.BackupsStored).ToList();
                if (scheduledBackupsToRemove.Any())
                {
                    _logger.DebugFormat("only last {0} scheduled backup records are to keep for tenant {1} so {2} records must be removed", schedule.BackupsStored, schedule.TenantId, scheduledBackupsToRemove.Count);
                    backupsToRemove.AddRange(scheduledBackupsToRemove);
                }
            }
            else
            {
                backupsToRemove.AddRange(scheduledBackups);
            }
        }

        foreach (var backupRecord in backupsToRemove)
        {
            try
            {
                var backupStorage = backupStorageFactory.GetBackupStorage(backupRecord);
                if (backupStorage == null)
                {
                    continue;
                }

                backupStorage.Delete(backupRecord.StoragePath);

                backupRepository.DeleteBackupRecord(backupRecord.Id);
            }
            catch (ProviderInfoArgumentException error)
            {
                _logger.Warn("can't remove backup record " + backupRecord.Id, error);

                if (DateTime.UtcNow > backupRecord.CreatedOn.AddMonths(6))
                {
                    backupRepository.DeleteBackupRecord(backupRecord.Id);
                }
            }
            catch (Exception error)
            {
                _logger.Warn("can't remove backup record: " + backupRecord.Id, error);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.Info("stopping backup cleaner service...");

        _timer?.Change(Timeout.Infinite, 0);
        _logger.Info("backup cleaner service stopped");

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
