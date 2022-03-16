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
 * Pursuant to Section 7 � 3(b) of the GNU GPL you must retain the original ONLYOFFICE logo which contains 
 * relevant author attributions when distributing the software. If the display of the logo in its graphic 
 * form is not reasonably feasible for technical reasons, you must include the words "Powered by ONLYOFFICE" 
 * in every copy of the program you distribute. 
 * Pursuant to Section 7 � 3(e) we decline to grant you any rights under trademark law for use of our trademarks.
 *
*/

namespace ASC.AuditTrail;

[Scope]
public class AuditReportCreator
{
    private readonly GlobalFolderHelper _globalFolderHelper;
    private readonly FileUploader _fileUploader;
    private readonly FilesLinkUtility _filesLinkUtility;
    private readonly CommonLinkUtility _commonLinkUtility;
    private readonly ILog _logger;

    public AuditReportCreator(
        GlobalFolderHelper globalFolderHelper,
        IOptionsMonitor<ILog> options,
        FileUploader fileUploader,
        FilesLinkUtility filesLinkUtility,
        CommonLinkUtility commonLinkUtility)
    {
        _globalFolderHelper = globalFolderHelper;
        _logger = options.CurrentValue;
        _fileUploader = fileUploader;
        _filesLinkUtility = filesLinkUtility;
        _commonLinkUtility = commonLinkUtility;
    }

    public string CreateCsvReport<TEvent>(IEnumerable<TEvent> events, string reportName) where TEvent : BaseEvent
    {
        try
        {
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, Encoding.UTF8);
            using var csv = new CsvWriter(writer, CultureInfo.CurrentCulture);

            csv.Context.RegisterClassMap(new BaseEventMap<TEvent>());

            csv.WriteHeader<TEvent>();
            csv.NextRecord();
            csv.WriteRecords(events);
            writer.Flush();

                    var file = _fileUploader.ExecAsync(_globalFolderHelper.FolderMy, reportName, stream.Length, stream, true).Result;
                    var fileUrl = _commonLinkUtility.GetFullAbsolutePath(_filesLinkUtility.GetFileWebEditorUrl(file.ID));

            fileUrl += string.Format("&options={{\"codePage\":{0}}}", Encoding.UTF8.CodePage);

            return fileUrl;
        }
        catch (Exception ex)
        {
            _logger.Error("Error while generating login report: " + ex);
            throw;
        }
    }
}