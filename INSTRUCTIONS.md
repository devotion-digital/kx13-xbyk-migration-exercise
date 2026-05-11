# Candidate Instructions, K13 → XbyK Migration Exercise

## Your task

Use the official **Kentico Migration Tool** to migrate the Kentico 13 source site (provided in this repo) into the supplied Xperience by Kentico target site. The XbyK target already has controllers, view components, widgets, sections, and views wired up to render the migrated content, your job is to get the **migration tool itself** to produce data that those XbyK components can consume cleanly.

There is **no expected outcome** for this exercise. We are interested in how far you can get, what decisions you make along the way, and how you approach problems you haven't seen before, not in a perfectly migrated site.

You do **not** need to run the Kentico 13 admin itself. You only need the K13 **database** restored (the migration tool reads directly from it) and you can read the K13 Core MVC delivery code as a reference for what the rendered site should look like.

## Where you'll be working

| Folder | Purpose |
| --- | --- |
| **`test-website-xbyk/TW.Web/`** | **Your working folder.** Pre-built XbyK 31.2.0 project with controllers, view components, widgets, sections, and codegen output already in place. Open this in your IDE. |
| `test-website-xbyk_clean/TW.Web/` | Emergency reset, an *empty* XbyK boilerplate, in case you want to start the XbyK delivery side from scratch. Most candidates won't need it. |
| `test-website-kx13/` | K13 source (admin app + Core MVC delivery). Read it for reference; you don't need to build or run it. |
| `migration-tool/` | The migration tool, with one custom mapping pre-applied (see below). This is where most of your work happens. |

## Deliverable

When you're finished, return:

1. A **zip of the repo** with your changes in place. Just zip the whole thing (you can leave the bacpacs out to keep the size down, since we already have the source one). Anything you edited, configured, or added (especially under `migration-tool/Migration.Tool.Extensions/` and `test-website-xbyk/`) comes along automatically.
2. A **`.bacpac`** (or `.bak`) export of the XbyK target database produced by your migration run.

That's the whole submission.

> **Submission**: see the email / message you received with this exercise, it contains the upload link.

## What you need installed

- **.NET 8 SDK** or newer (we use 8.0.x; 9.0.x and 10.0.x also work). Download: https://dotnet.microsoft.com/download
- **SQL Server**, Express, Developer, or full edition. LocalDB also works. You'll need two empty databases (one for the K13 source restore, one for the XbyK target).
- **Visual Studio 2022** (Community is fine) **or JetBrains Rider**, either works for building and running the projects.
- A SQL client (**SSMS** or **Azure Data Studio**) for restoring the bacpac and exporting the result.

You do **not** need a Kentico 13 install or Visual Studio's .NET Framework 4.8 workload, the K13 admin is not run as part of this exercise.

## What's in the K13 source

The K13 site uses **ASP.NET Core MVC** as the delivery layer (the K13 admin is the headless CMS). It contains:

- **Page types**
  - **`TW.PageMeta`**, a *base* page type holding shared SEO meta fields (`PageMetaTitle`, `PageMetaDescription`). Other page types inherit from it via K13's class inheritance.
  - **`TW.HomePage`**, routable home page (inherits from `TW.PageMeta`). Fields: `HomePageName`, `HomePageSubheading`, `HomePageHeroImage` (media-library reference).
  - **`TW.ContentPage`**, routable content page (inherits from `TW.PageMeta`). Fields: `ContentPageName`, `ContentPageSubheading`.
  - **`TW.SiteConfiguration`**, *data-only* page type (no URL), holding the site logo and footer text; lives under root.
- **Page builder widgets** registered in the K13 Core MVC delivery site:
  - **Call to action** (`TestWebsite.CallToAction`), heading, supporting text, button text, button-target-page selector.
  - **Image card** (`TestWebsite.ImageCard`), image (media-library file), alt text, caption.
  - **Child pages** (`TestWebsite.ChildPages`), lists child pages of the current/selected page as tiles.
- **Page builder section**, `TestWebsite.DefaultSection` (single-column).
- **View components** (in the K13 delivery site, baked into the layout, not page-builder widgets):
  - Header (reads logo from Site Settings)
  - Footer (reads logo + footer text from Site Settings)
  - Home Hero (image-backed; reads the HomePage's hero image)
  - Content Hero (text-only)
  - Top navigation (lists top-level routable pages, excluding home, clicked via logo)
- **Media library** holding the logo and hero images, referenced by various page types.

The XbyK target already mirrors all of this. The migration tool's job is to populate XbyK with K13's content (page types, pages, media, etc.) so the XbyK code renders. The migration tool will **not** move the delivery code, that's already done for you in `test-website-xbyk/TW.Web/`.

## High-level steps

The detail of each step lives in the official Kentico documentation, links are in the next section. Use these steps as a checklist; consult the docs for the actual commands and configuration.

1. **Restore the K13 source database.**
   In SSMS / Azure Data Studio, restore `test-website-kx13.bacpac` to your local SQL Server as a new database (e.g. `test-website-kx13`). Take a note of the connection string.

2. **Restore the empty XbyK target database** from `test-website-xbyk.bacpac`, same instance, restore as a new database (e.g. `test-website-xbyk`).

   This bacpac is the result of running the Kentico Xperience database manager, schema is in place, an `administrator` account exists with a blank password, and the hash salt that's already baked in matches the value committed to [`test-website-xbyk/TW.Web/appsettings.json`](test-website-xbyk/TW.Web/appsettings.json). You **don't** need to run `dotnet kentico-xperience-dbmanager` yourself.

   The only thing you need to supply is the connection string. Easiest: create `test-website-xbyk/TW.Web/appsettings.Development.json` (gitignored) with:
   ```json
   {
     "ConnectionStrings": {
       "CMSConnectionString": "Data Source=<your server>;Initial Catalog=test-website-xbyk;Integrated Security=True;Encrypt=False;"
     }
   }
   ```

3. **Run the XbyK target site once** to confirm the target works:
   ```powershell
   cd test-website-xbyk\TW.Web
   dotnet run
   ```
   Open the admin UI at `https://localhost:<port>/admin`, log in as `administrator` (blank password, set a real one immediately if you keep using the site). You'll see the site renders but there's no content yet. Stop the site before moving on.

   > If you'd rather start the XbyK delivery side from scratch and run dbmanager yourself, see the alternative path in `test-website-xbyk_clean/`, but be aware nothing's wired up over there, so you'll need to build controllers/widgets/views yourself.

4. **Configure the Migration Tool.**
   Edit `migration-tool/Migration.Tool.CLI/appsettings.json` (or create `appsettings.local.json` next to it, it's loaded as an override and gitignored). The four `[TODO]` values to fill in:
   - **`Settings.KxConnectionString`**, your **K13 source** database (from step 1)
   - **`Settings.KxCmsDirPath`**, absolute path to `test-website-kx13/CMS/`
   - **`Settings.XbyKDirPath`**, absolute path to `test-website-xbyk/TW.Web/`
   - **`Settings.XbyKApiSettings.ConnectionStrings.CMSConnectionString`**, your **XbyK target** database (from step 2)

   The tool's [Usage Guide](https://github.com/Kentico/xperience-by-kentico-kentico-migration-tool/blob/master/docs/Usage-Guide.md) documents every other setting.

5. **Build and run the Migration Tool CLI.**
   ```powershell
   cd migration-tool
   dotnet build Migration.Tool.sln -c Release
   cd Migration.Tool.CLI\bin\Release\net8.0
   .\Migration.Tool.CLI.exe migrate --help
   ```
   The `migrate` master command accepts chained subcommands. A reasonable full sweep:
   ```powershell
   .\Migration.Tool.CLI.exe migrate --sites --custom-modules --users --page-types --media-libraries --pages --forms
   ```
   Dependency order is handled internally, the CLI flag order doesn't matter. Re-running on the same target DB is destructive; back up your empty post-dbmanager state if you want a clean do-over.

6. **Run the XbyK site again** and inspect the migrated content in the admin UI and on the front end. Iterate, re-run the migration with different flags, edit mappings under `migration-tool/Migration.Tool.Extensions/`, rebuild, restore the empty DB baseline, re-run.

7. **Export the resulting XbyK database** as a `.bacpac` or `.bak` and zip up the `test-website-xbyk/` folder for submission.

## Customisations already in the migration toolkit

The Migration Tool is **pre-configured** with a custom mapping that handles the K13 `TW.PageMeta` base page type. You don't need to write this, but you should understand what it does, because you'll likely need to write similar mappings for other situations.

**File:** [`migration-tool/Migration.Tool.Extensions/ClassMappings/TwReusableSchemaMappings.cs`](migration-tool/Migration.Tool.Extensions/ClassMappings/TwReusableSchemaMappings.cs)
**Registered via:** `services.AddTwReusableSchemaMappings()` in [`Migration.Tool.Extensions/ServiceCollectionExtensions.cs`](migration-tool/Migration.Tool.Extensions/ServiceCollectionExtensions.cs)

### What it does

In K13, `TW.PageMeta` is a base page type. `TW.ContentPage` and `TW.HomePage` *inherit* from it, picking up its `PageMetaTitle` / `PageMetaDescription` fields automatically. XbyK has no class inheritance, the equivalent concept is a **reusable field schema** referenced by content types.

The custom mapping does three things:

1. **Suppresses migration of `TW.PageMeta` as a content type.** A `ReusableSchemaBuilder` registered for that source class causes the page-type migrator to skip it. So you won't see a `TW.PageMeta` content type in XbyK, only the schema.
2. **Creates a reusable field schema named `TW.PageMeta`**, deriving its fields (`PageMetaTitle`, `PageMetaDescription`) from the K13 source class via `ConvertFrom`.
3. **Maps `TW.ContentPage` and `TW.HomePage` as content types that reference the schema** using `MultiClassMapping.UseResusableSchema("TW.PageMeta")` and explicit source-to-schema-field mappings so the data flows through.

### Why this couldn't be done with `appsettings.json` alone

The tool has a `CreateReusableFieldSchemaForClasses` config setting, but it only converts the *named* class, children that inherit from it in K13 still get their own copies of the inherited fields in XbyK rather than referencing the new schema. The C# mapping is the only way to get the children to actually reference the shared schema. (The two approaches are mutually exclusive, the tool will refuse to start if you set both.)

### What you'll see post-migration

- `TW.ContentPage` and `TW.HomePage` exist as content types in admin → **Content types**.
- `TW.PageMeta` exists as a reusable field schema, **not** a content type, find it in admin → **Content types** → **Reusable field schemas**.
- The XbyK target's generated class for `TW.PageMeta` (already shipped in `test-website-xbyk/TW.Web/Generated/ReusableFieldSchemas/`) is an *interface* (`ITWPageMeta`), implemented by both page-type classes.

### Treat it as a worked example

When you hit the "*Things you'll hit along the way*" issues below, you may want to write your own `IWidgetPropertyMigration` or additional class mapping. The TwReusableSchemaMappings is your reference pattern.

## Things you'll hit along the way

A few wrinkles you'll discover, surfaced here so you know they're expected, not bugs in this repo:

- **Source instance API discovery warning.** On startup, the migration tool prints:
  > `Source instance API discovery feature is disabled, capabilities of Migration Tool to migrate widgets, page urls will be limited.`

  This feature would let the tool query the running K13 site over HTTP to learn each widget property's form-component metadata. Without it, two built-in property migrations, `WidgetPageSelectorMigration` and `WidgetFileMigration`, **skip silently**. The result: widgets migrate, plain-text properties (heading, button text, alt text) come across fine, but **page selectors and media file selectors keep their K13-format JSON values** (e.g. `{"nodeGuid":"..."}` and `{"fileGuid":"..."}`) that XbyK's matching form components don't understand. In the admin you'll see "deleted item" in the widget property selectors after migration.

  You have three reasonable options:
  1. **Re-pick the page / image in admin** for each affected widget instance, fastest for a small site, no code needed.
  2. **Enable source instance API discovery** in the migration tool's `appsettings.json` (`OptInFeatures.QuerySourceInstanceApi`) and run the K13 Core MVC delivery site so the tool can call it. See the tool's [Usage Guide](https://github.com/Kentico/xperience-by-kentico-kentico-migration-tool/blob/master/docs/Usage-Guide.md).
  3. **Write a custom `IWidgetPropertyMigration`** in `migration-tool/Migration.Tool.Extensions/` that rewrites the raw JSON shape from K13 to XbyK. Doesn't need API discovery.

  We'd love to see how you reason through this, there's no single right answer.

- **Page builder enablement and content-type registration.** The XbyK project's `Program.cs` lists which content types have the page builder UI via `PageBuilderOptions.ContentTypeNames`. We've enabled it for `TW.HomePage` and `TW.ContentPage` already. If you add or rename page types, update that list.

- **Section / widget identifiers** in the migrated page-builder JSON refer to whatever K13 used (e.g., `TestWebsite.CallToAction`, `TestWebsite.DefaultSection`). The XbyK widget/section classes in `test-website-xbyk/TW.Web/` already have matching `Identifier` constants. If the migration data references identifiers we haven't covered, you'll see *"Sections / widgets with the following identifiers are not registered. Saving the page will result in lost data."*

- **Page templates vs controllers.** K13's delivery site uses controller-based routing without page templates, so the XbyK controllers return plain `View(model)` instead of `TemplateResult(model)`. If you change this to template-driven, controllers need to return `new TemplateResult(model)` and pages need a template assigned.

- **Tree-path filtering of children.** `PathMatch.Children("/", nestingLevel: 1)` doesn't reliably limit to depth 1 in XbyK 31.2.0 when the parent is the root. The `NavigationViewComponent` filters by `WebPageItemTreePath.Count(c => c == '/') == 1` in C# instead.

## Required reading

- **Migration Tool repo (start here)**: https://github.com/Kentico/xperience-by-kentico-kentico-migration-tool
  - **Usage Guide**: https://github.com/Kentico/xperience-by-kentico-kentico-migration-tool/blob/master/docs/Usage-Guide.md
  - **Version compatibility matrix** (we've pinned versions for you, but worth reading): https://github.com/Kentico/xperience-by-kentico-kentico-migration-tool#library-version-matrix
- **Official docs entry-point**: https://docs.kentico.com/documentation/developers-and-admins/upgrade-to-xperience-by-kentico/migration-toolkit
- **Xperience by Kentico product docs**: https://docs.kentico.com/

## Versions (pinned, don't change them)

| Component | Version |
| --- | --- |
| Xperience by Kentico (target) | **31.2.0** |
| Kentico Migration Tool | v4.2.0-equivalent (cloned from master) |
| Kentico 13 source DB | restored as-is from the supplied bacpac (13.0.197) |
| K13 Core MVC delivery | `Kentico.Xperience.AspNetCore.WebApp` 13.0.197 (.NET 8) |
| .NET SDK | 8.0 or newer |

## Bonus, hotfix to the latest XbyK

Optional, attempt only if you've already got the migration running end-to-end. Upgrade the XbyK target from the pinned **31.2.0** to the latest stable XbyK release.

What's involved:

1. Read the **[migration tool's version compatibility matrix](https://github.com/Kentico/xperience-by-kentico-kentico-migration-tool#library-version-matrix)** to see which XbyK versions the cloned migration tool supports. If the latest XbyK is newer than what the tool supports, you have a decision: stay on the highest tool-compatible XbyK, or pull a newer migration tool release and merge our `TwReusableSchemaMappings.cs` customisation into it.
2. Bump the package versions in `test-website-xbyk/TW.Web/TW.Web.csproj` and the `kentico.xperience.dbmanager` version in `test-website-xbyk/TW.Web/.config/dotnet-tools.json`.
3. Restore tools, restore packages, rebuild.
4. Run `dotnet kentico-xperience-dbmanager` against your migrated XbyK DB to apply schema migrations between versions.
5. Re-run the site, smoke-test the admin + front-end. Spot regressions in widgets, page builder, routing, search.

What we're looking for in this bonus:

- A clean upgrade with no broken pages, missing fields, or broken widget references.
- Notes in `NOTES.md` (or commit messages) on what versions you ended on, any compatibility decisions, anything you had to patch.
- If you upgraded the migration tool itself, a note on what changed in your `Migration.Tool.Extensions/` to keep the `TwReusableSchemaMappings.cs` working.

You don't need to re-run migration after the upgrade, the existing migrated data should still be valid. If it isn't, that's an interesting thing to write down.

## How we'll review your submission

We'll look at your zipped copy and your exported `.bacpac` to see:

- How successfully the migration tool ran, what configuration you used
- Any extensions or mappings you added to `migration-tool/Migration.Tool.Extensions/`
- How much of the source content made it across cleanly (page types, content tree, widgets, media)
- Decisions visible in your `appsettings.json` and any notes you leave

You're welcome (but not required) to include a short `NOTES.md` at the repo root (or anywhere obvious) describing what you tried, what worked, what didn't, and what you'd do next with more time. We genuinely value that more than a finished migration.

## Ground rules

- **Use of AI assistants** (Claude, Copilot, ChatGPT, etc.), **allowed**. Treat them like any other documentation source.
- **Use of community packages**, allowed.
- **Modifying the Migration Tool source** in `migration-tool/`, encouraged where needed. Note in your `NOTES.md` what you changed and why.
- **Modifying the XbyK target code** in `test-website-xbyk/TW.Web/`, allowed but not the focus. If you do, note what and why.
- **Time**, see the email/message you received with this exercise, it has the deadline.
- **Questions during the exercise**, see the email/message you received with this exercise for the contact and response window.

Good luck, we're more interested in your process than your result.
