cmsdefine(["require", "exports", 'angular', 'CMS.ApplicationDashboard/Directives/SingleObjectTileDirective', 'CMS.ApplicationDashboard/Directives/ApplicationTileDirective', 'CMS.ApplicationDashboard/Directives/XbykNewsTileDirective', 'CMS.ApplicationDashboard/Directives/TileIconDirective'], function (cmsrequire, exports, angular, singleObjectTileDirective, applicationTileDirective, xbykNewsTileDirective, tileIconDirective) {
    var ModuleName = 'cms.dashboard.directives';

    angular.module(ModuleName, []).directive('singleObjectTile', singleObjectTileDirective.Directive).directive('applicationTile', applicationTileDirective.Directive).directive('xbykNewsTile', xbykNewsTileDirective.Directive).directive('tileIcon', tileIconDirective);

    exports.Module = ModuleName;
});
