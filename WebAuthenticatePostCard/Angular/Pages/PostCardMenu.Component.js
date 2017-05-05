angular.module("PostCardApp").directive("postCardMenu",function () {
    return {
        restrict:'EA',
        template: "<h1>{{text}}</h1>",
        link: function (scope, element, attributes) {
            scope.text = "Angular build successfully"

        }
    }


});
