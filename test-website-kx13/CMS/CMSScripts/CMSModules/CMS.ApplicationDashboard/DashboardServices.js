cmsdefine(["require", "exports", 'angular', 'CMS.ApplicationDashboard/Services/TileResource', 'CMS.ApplicationDashboard/Services/XbykNewsTileResource', 'CMS.ApplicationDashboard/Services/LiveTilePushService'], function (cmsrequire, exports, angular, tileResource, xbykNewsTileResource, liveTilePushService) {
    var ModuleName = 'cms.dashboard.services';

    angular.module(ModuleName, []).factory('cms.resource.tile', tileResource.Resource).factory('cms.resource.xbykNewsTile', xbykNewsTileResource.Resource).service('cms.service.liveTilePushService', liveTilePushService.Service);

    exports.Module = ModuleName;
});
