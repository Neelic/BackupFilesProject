# BackupFilesProject

Тестовый проект для резервного копирования файлов и папок по расписанию cron-выражения.

В аргументы принимает путь к JSON-файлу, пример данного файла находится в корневой папке репозитория.

Копирует файлы и папки на основе даты изменения.

Формат cron-выражений: <https://www.quartz-scheduler.net/documentation/quartz-3.x/how-tos/crontrigger.html#format>

Для ведения логов используется стандартный ILogger
