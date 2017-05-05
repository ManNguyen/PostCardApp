// My routing machanic for single page application using UI-Router
// Before access to any page, I check if the page need to be authenticated or not:
//    If it's true then I check if there is valid token stored in client or not to grant access to that page
//    It it's false, simply move to that page
angular.module("PostCardApp")
.run(['$rootScope', '$state', '$transitions', 'AuthenticateService', function ($rootScope, $state, $transitions, AuthenticateService) {
    $transitions.onStart({}, function ($transition$) {
        toState = $transition$.$to();
        if (toState.authenticate && !AuthenticateService.isAuthenticated()) {
            $state.transitionTo("UnauthorizedAccess");
            event.preventDefault();
        }
    });
}]);
angular.module("PostCardApp")
.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    //default State
    $urlRouterProvider.otherwise('/Home');
    $stateProvider
        .state('Home', {
            url: '/Home',
            templateUrl: 'Angular/Pages/Home.Page.html'
        })
        .state('PostCard', {
            authenticate:true,/*This mean to access this page you have to be aunthenticated*/
            url: '/PostCard',
            template: '<div post-card-page></div>'
        })
        .state('UnauthorizedAccess', {
            url: '/UnauthorizedAccess',
            template: '<div class="container"><h1> You cannot access this page without an account </h1></div>'
        })
        .state('Login', {
            url: '/Login',
            template: '<div login-page></div>'
        })
        .state('Register', {
            url: '/Register',
            template: '<div register-page></div>'
        })
        
}]);