# Инструкция по работе с миграциями Entity Framework

## Создание миграции

Для создания новой миграции выполните следующую команду в терминале из корневой папки проекта:

```bash
dotnet ef migrations add InitialCreate --project DAL --startup-project Restraunt
```

Где:
- `InitialCreate` - имя миграции (можете использовать любое имя)
- `--project DAL` - проект, содержащий DbContext
- `--startup-project Restraunt` - стартовый проект (WPF приложение)

## Применение миграций к базе данных

### Автоматическое применение (рекомендуется)

Миграции применяются автоматически при запуске приложения через метод `InitializeDatabase()` в `App.xaml.cs`.

### Ручное применение

Если нужно применить миграции вручную, выполните:

```bash
dotnet ef database update --project DAL --startup-project Restraunt
```

## Удаление последней миграции

Если нужно отменить последнюю миграцию (еще не примененную к БД):

```bash
dotnet ef migrations remove --project DAL --startup-project Restraunt
```

## Просмотр списка миграций

```bash
dotnet ef migrations list --project DAL --startup-project Restraunt
```

## Создание скрипта SQL из миграции

Для создания SQL скрипта из миграций:

```bash
dotnet ef migrations script --project DAL --startup-project Restraunt --output migration.sql
```

## Важные замечания

1. Убедитесь, что PostgreSQL запущен и доступен
2. Проверьте строку подключения в `RestrauntContext.cs` и `DesignTimeDbContextFactory.cs`
3. База данных `RestrauntPO` будет создана автоматически при первом применении миграции
4. Все миграции хранятся в папке `DAL/Migrations/`
