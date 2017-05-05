angular.module("PostCardApp")
    .controller('MainController', ["$scope", "$state", '$timeout', 'AuthenticateService', 'AUTH_EVENTS',  function ($scope, $state, $timeout, AuthenticateService, AUTH_EVENTS) {
        $scope.username = AuthenticateService.username();
        $scope.isAuthenticated = AuthenticateService.isAuthenticated();

        $scope.$on(AUTH_EVENTS.authStatusChanged, function (evt, data) {
            $scope.isAuthenticated = AuthenticateService.isAuthenticated();
            $scope.username = AuthenticateService.username();
        })
        $scope.$on(AUTH_EVENTS.notAuthenticated, AUTH_EVENTS.notAuthorized, function (event, data) {
            console.log("Not Authenticated");
        })
        $scope.setCurrentUsername = function (name) {
            $scope.username = name;
        };
        $scope.logout = function () {
            AuthenticateService.logout();
            $timeout(function () {
                $state.go("Login");
            })
        }

    }]);
