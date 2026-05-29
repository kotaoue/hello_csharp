# hello_efcore

Hello World sample for Entity Framework Core (EF Core) with SQLite.

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/download)

## Run locally

```bash
cd hello_efcore
dotnet restore
dotnet run
```

When it runs, it creates a local SQLite database file (`hello_efcore.db`), inserts sample data, and prints saved records.

## After first run

### Inspect the database

```bash
sqlite3 hello_efcore.db
.tables
SELECT * FROM Messages;
.exit
```

### Run again

```bash
dotnet run
```

The sample inserts seed data only when the table is empty.

### Reset and recreate the local database

```bash
rm hello_efcore.db
dotnet run
```
