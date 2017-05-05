angular.module("PostCardApp")
.constant("REGEX", {
    Email: /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i
})
.directive("loginPage", ["AuthenticateService", "REGEX", "$state", function (AuthenticateService, REGEX, $state) {
    return {
        restrict: 'EA',
        templateUrl: "Angular/Pages/Login.Page.html",
        link: function (scope, element, attributes) {
            //element.addClass('login-widget');
            scope.data = {};
            focus('useremail');

            function ShowError(message) {
                scope.error = true;
                scope.errormsg = message;
            }
            scope.login = function (data) {
                scope.error = false;
                if (data.username == undefined) {
                    ShowError(' Please enter in your email');
                    focus('useremail');
                }
                else if (!REGEX.Email.test(data.username)) {
                    ShowError(' Please enter a valid email address');
                    focus('useremail');
                }
                else if (data.password == undefined || data.password == '') {
                    ShowError(' Please enter in your password');
                    focus('password');
                }

                else {
                    AuthenticateService.login(data.username, data.password).then(
                        function (authenticated) {
                            scope.checking = false;
                            scope.error = false;
                            $state.go('PostCard', {}, { reload: true });
                            //$scope.setCurrentUsername(data.username);
                        },
                        function (err) {
                            ShowError('You email and password are not found');
                        }
                    );
                }
            }
            scope.keypress = function (event) {
                //if Enter pressed
                if (event.which == 13) {
                    scope.login(scope.data);
                }

            }
        }
    }


}]);
