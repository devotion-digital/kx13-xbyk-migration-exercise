# kx13-xbyk-migration-exercise

Sample project for evaluating a candidate's ability to migrate a **Kentico 13** site to **Xperience by Kentico (XbyK)** using the official **Kentico Migration Tool**.

The repo provides the K13 source site (admin + Core MVC delivery), an XbyK target project for candidates to work in, a pre-customised copy of the migration tool, and database backups, all pre-pinned to compatible versions so a candidate can focus on running and customising the migration rather than setting up infrastructure.

> **For candidates:** see **[INSTRUCTIONS.md](INSTRUCTIONS.md)** for the task brief, deliverable, and step-by-step setup.

## Layout

```
kx13-xbyk-migration-exercise/
├── README.md                       Repo overview
├── INSTRUCTIONS.md                 Candidate brief
│
├── test-website-kx13/              K13 source site
│   ├── CMS/                        K13 admin app (.NET Framework 4.8, hybrid Web Forms + Portal Engine)
│   ├── TestWebsite/                K13 Core MVC delivery site (.NET 8), reference for the rendered site
│   ├── Lib/, packages/, …
│   ├── WebApp.sln                  K13 admin alone
│   └── TestWebsite.sln             K13 admin + Core MVC delivery
│
├── test-website-kx13.bacpac        K13 source DB with seeded test content (the one candidates restore)
├── test-website-kx13_clean.bacpac  K13 baseline, fresh install, no seeded content
├── test-website-xbyk.bacpac        Your maintainer-built migrated XbyK DB (reference for evaluating)
│
├── test-website-xbyk/              Candidate's working folder, pre-built XbyK 31.2.0 project
│   ├── TW.Web.sln                  with controllers, view components, widgets, sections, views,
│   └── TW.Web/                     and codegen output already wired up to consume migrated data
│
├── test-website-xbyk_clean/        Emergency reset, empty XbyK boilerplate
│   ├── TW.Web.sln                  in case the candidate (or you) wants to rebuild the XbyK
│   └── TW.Web/                     delivery side from scratch. Most candidates won't need it.
│
└── migration-tool/                 Kentico Migration Tool clone (master) + a custom mapping
    ├── Migration.Tool.sln
    ├── Migration.Tool.CLI/
    └── Migration.Tool.Extensions/ClassMappings/TwReusableSchemaMappings.cs   ← custom mapping (see below)
```

## Versions pinned

| Component | Version | Notes |
| --- | --- | --- |
| Kentico 13 (source) | 13.0.197 | Hotfix shipped with the K13 install; bacpac is at this version |
| K13 Core MVC delivery (`Kentico.Xperience.AspNetCore.WebApp`) | 13.0.197 | In `test-website-kx13/TestWebsite/` |
| Xperience by Kentico (target) | **31.2.0** | All `Kentico.Xperience.*` packages, both clean and reference |
| Kentico Migration Tool | v4.2.0-equivalent | Cloned from `master`; expects XbyK 31.2.0 |
| Kentico Xperience DB Manager | 31.2.0 | Local `dotnet tool` in both XbyK projects |
| .NET SDK | **8.0** or newer | All Core MVC projects target `net8.0` |
| SQL Server | local instance (e.g. `(local)\MSSQL2025`) | Used by both the K13 source DB and the XbyK target DB |

## What's already customised in the migration tool

The migration tool is **not** vanilla. There's one custom class mapping registered at [`migration-tool/Migration.Tool.Extensions/ClassMappings/TwReusableSchemaMappings.cs`](migration-tool/Migration.Tool.Extensions/ClassMappings/TwReusableSchemaMappings.cs) and wired in [`Migration.Tool.Extensions/ServiceCollectionExtensions.cs`](migration-tool/Migration.Tool.Extensions/ServiceCollectionExtensions.cs) via `services.AddTwReusableSchemaMappings()`.

It converts the K13 `TW.PageMeta` base page type (used as a class-inheritance parent of `TW.ContentPage` and `TW.HomePage`) into an XbyK **reusable field schema** that both child content types reference. Without this, K13's inheritance flattens into duplicated fields on each child in XbyK.

Full details in [INSTRUCTIONS.md → Customisations already in the migration toolkit](INSTRUCTIONS.md#customisations-already-in-the-migration-toolkit).

## Local config (gitignored)

Connection strings and hash salts live in environment-override files so they don't pollute the candidate-facing config:

| File | Contains | Tracked? |
| --- | --- | --- |
| `test-website-xbyk/TW.Web/appsettings.Development.json` | XbyK target connection string + hash salt | gitignored |
| `test-website-kx13/TestWebsite/appsettings.Development.json` | K13 Core MVC delivery connection string + salt + `CMSCIRepositoryPath` | gitignored |
| `migration-tool/Migration.Tool.CLI/appsettings.local.json` | Migration tool source/target connection strings + folder paths | gitignored |
| `test-website-xbyk_clean/TW.Web/appsettings.Development.json` | (none yet, candidates create this themselves) | n/a |
| `test-website-kx13/CMS/Web.config` | Empty `CMSConnectionString` element | tracked (blank by design) |

The committed `appsettings.json` files have empty connection strings and (for the migration tool) `[TODO]` placeholders.

## Key links

- Migration Tool repo & version matrix, https://github.com/Kentico/xperience-by-kentico-kentico-migration-tool
- Migration toolkit docs, https://docs.kentico.com/documentation/developers-and-admins/upgrade-to-xperience-by-kentico/migration-toolkit
- Xperience by Kentico product docs, https://docs.kentico.com/

## Notes

- `test-website-kx13/TestWebsite/` is the K13 Core MVC delivery site (.NET 8, `Kentico.Xperience.AspNetCore.WebApp` 13.0.197). It renders the K13 content and is your source of truth for what the migrated XbyK site should look like. Candidates re-implement this side in XbyK (the migration tool does **not** migrate the delivery code).
- `test-website-kx13/packages/` and all `bin/obj/` are gitignored.
- The migration tool was cloned from upstream's `master` branch (no release tag was current for XbyK 31.2.0 at clone time). If you upgrade, re-clone and re-apply `TwReusableSchemaMappings.cs` + the `ServiceCollectionExtensions.cs` registration.
