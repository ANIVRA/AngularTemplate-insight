(function () {
    angular.module('app')
        // Path: /
        .controller('menuController', ['$scope', '$state', '$stateParams', 'authService', function ($scope, $state, $stateParams, authSvc) {
            $scope.authentication = authSvc.authentication;
            $scope.logout = authSvc.logout;
        }])
})();