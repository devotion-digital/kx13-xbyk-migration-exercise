using CMS.DataEngine;

using Microsoft.Extensions.DependencyInjection;

using Migration.Tool.Common.Abstractions;
using Migration.Tool.Common.Builders;

namespace Migration.Tool.Extensions.ClassMappings;

/// <summary>
/// Maps the K13 TW.* page-type inheritance hierarchy onto an XbyK reusable schema.
///
/// K13 design: TW.PageMeta is a base page type; TW.ContentPage and TW.HomePage inherit from it.
/// XbyK has no class inheritance — the equivalent is a reusable field schema referenced by content types.
///
/// Result of this mapping:
///   - TW.PageMeta is NOT created as a content type (the reusable schema builder excludes its source class).
///   - A reusable schema "TW.PageMeta" is created (note: empty unless non-system fields are added to TW.PageMeta in K13).
///   - TW.ContentPage and TW.HomePage become content types that reference the schema via UseResusableSchema(...).
/// </summary>
public static class TwReusableSchemaMappings
{
    private const string SchemaName = "TW.PageMeta";

    public static IServiceCollection AddTwReusableSchemaMappings(this IServiceCollection services)
    {
        var schema = new ReusableSchemaBuilder(SchemaName, "Base - Page Meta", "Shared page meta fields");
        schema.ConvertFrom("TW.PageMeta", x => x);

        var contentPage = new MultiClassMapping("TW.ContentPage", target =>
        {
            target.ClassName = "TW.ContentPage";
            target.ClassTableName = "TW_ContentPage";
            target.ClassDisplayName = "Content page";
            target.ClassType = ClassType.CONTENT_TYPE;
            target.ClassContentTypeType = ClassContentTypeType.WEBSITE;
            target.ClassWebPageHasUrl = true;
        });
        contentPage.BuildField("ContentPageID").AsPrimaryKey();
        contentPage.UseResusableSchema(SchemaName);
        // Inherited from TW.PageMeta in K13; map source columns to schema-named targets so data flows in.
        contentPage.BuildField("PageMetaTitle").SetFrom("TW.ContentPage", "PageMetaTitle");
        contentPage.BuildField("PageMetaDescription").SetFrom("TW.ContentPage", "PageMetaDescription");
        contentPage.BuildField("ContentPageName").SetFrom("TW.ContentPage", "ContentPageName", true);
        contentPage.BuildField("ContentPageSubheading").SetFrom("TW.ContentPage", "ContentPageSubheading", true);

        var homePage = new MultiClassMapping("TW.HomePage", target =>
        {
            target.ClassName = "TW.HomePage";
            target.ClassTableName = "TW_HomePage";
            target.ClassDisplayName = "Home page";
            target.ClassType = ClassType.CONTENT_TYPE;
            target.ClassContentTypeType = ClassContentTypeType.WEBSITE;
            target.ClassWebPageHasUrl = true;
        });
        homePage.BuildField("HomePageID").AsPrimaryKey();
        homePage.UseResusableSchema(SchemaName);
        // Inherited from TW.PageMeta in K13; map source columns to schema-named targets so data flows in.
        homePage.BuildField("PageMetaTitle").SetFrom("TW.HomePage", "PageMetaTitle");
        homePage.BuildField("PageMetaDescription").SetFrom("TW.HomePage", "PageMetaDescription");
        homePage.BuildField("HomePageName").SetFrom("TW.HomePage", "HomePageName", true);
        homePage.BuildField("HomePageSubheading").SetFrom("TW.HomePage", "HomePageSubheading", true);
        homePage.BuildField("HomePageHeroImage").SetFrom("TW.HomePage", "HomePageHeroImage", true);

        services.AddSingleton<IReusableSchemaBuilder>(schema);
        services.AddSingleton<IClassMapping>(contentPage);
        services.AddSingleton<IClassMapping>(homePage);

        return services;
    }
}
