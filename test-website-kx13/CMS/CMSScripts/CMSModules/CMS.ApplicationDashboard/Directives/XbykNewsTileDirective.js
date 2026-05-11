cmsdefine(["require", "exports", 'CMS/EventHub'], function (cmsrequire, exports, eventHub) {
    exports.Directive = [
        'cms.resource.xbykNewsTile', function (__xbykNewsTile) {
            var tile = {
                restrict: 'A',
                scope: {},
                link: function ($scope) {
                    $scope.model = {};
                    $scope.model.visible = false;

                    const toggleXbykNewsTile = (value) => $scope.$emit('toggleXbykNewsTile', value);

                    __xbykNewsTile.get(function (xbykData) {
                        $scope.model.header = xbykData.Header;
                        $scope.model.description = xbykData.Description;
                        $scope.model.ctaLabel = xbykData.CtaLabel;
                        $scope.model.ctaUrl = xbykData.CtaUrl;

                        // User settings indicate the XbyK news tile visibility
                        toggleXbykNewsTile(xbykData.Visible);
                    });

                    $scope.model.hide = () => {
                        toggleXbykNewsTile(false);

                        // Store the hidden XbyK news tile state in the user settings
                        __xbykNewsTile.update({ visible: false });
                    }
                },
                templateUrl: 'xbykNewsTemplate.html'
            };

            return tile;
        }
    ];
});