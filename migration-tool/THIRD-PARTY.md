# Third-party notice

This directory is a vendored copy of the open-source **Kentico Migration Tool**, distributed under the MIT License. The original license is preserved at [`LICENSE.md`](LICENSE.md).

- Upstream repository: https://github.com/Kentico/xperience-by-kentico-kentico-migration-tool
- Vendored from the `master` branch (the version current at the time of inclusion; see the upstream repository's version compatibility matrix for the corresponding Xperience by Kentico release this build targets).
- Vendoring approach: source cloned and the inner `.git` directory removed, so this folder is plain files inside the parent repository.

## Local modifications

The only local addition is one custom class mapping:

- [`Migration.Tool.Extensions/ClassMappings/TwReusableSchemaMappings.cs`](Migration.Tool.Extensions/ClassMappings/TwReusableSchemaMappings.cs), wired in [`Migration.Tool.Extensions/ServiceCollectionExtensions.cs`](Migration.Tool.Extensions/ServiceCollectionExtensions.cs) via `services.AddTwReusableSchemaMappings()`.

It maps the K13 `TW.PageMeta` base page type to an XbyK reusable field schema referenced by the inheriting content types `TW.ContentPage` and `TW.HomePage`. See `INSTRUCTIONS.md` in the parent repository for context.

All other files in this directory are from the upstream project, unmodified.

## License

Copyright (c) 2022 Kentico, licensed under the MIT License (see [`LICENSE.md`](LICENSE.md)).
