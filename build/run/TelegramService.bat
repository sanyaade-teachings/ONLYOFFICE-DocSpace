echo "RUN ASC.TelegramService"
call dotnet run --project ..\..\common\services\ASC.TelegramService\ASC.TelegramService.csproj --no-build --$STORAGE_ROOT=..\..\Data  --log__dir=..\..\Logs --log__name=telegram