angular.module("PostCardApp")
.directive("compareTo",  function () {
    return {
        require: "ngModel",
        scope: {
            otherModelValue: "=compareTo"
        },
        link: function (scope, element, attributes, ngModel) {
            ngModel.$validators.compareTo = function (modelValue) {
                return modelValue == scope.otherModelValue;
            };

            scope.$watch("otherModelValue", function () {
                ngModel.$validate();
            });
        }
    };
})
.directive("registerPage", ["AuthenticateService", "REGEX", "$state", function (AuthenticateService, REGEX, $state) {
    return {
        restrict: 'EA',
        templateUrl: "Angular/Pages/Register.Page.html",
        link: function (scope, element, attributes) {
            scope.emailRegex = REGEX.Email;
            scope.submitForm = function (userForm) {
                // check to make sure the form is completely valid
                scope.checked = true;
                console.log(userForm);
                if (userForm.$valid) {
                    AuthenticateService.register(userForm.name.$viewValue, userForm.email.$viewValue, userForm.password.$viewValue).then(
                        function (response) {
                            $state.transitionTo('Home');
                        },
                        function (error) {

                            scope.showServerError = true;
                            scope.serverError = error;
                        }
                    )
                }
                else {
                    console.log("Invalid");
                }

            };
        }
    }


}]);
