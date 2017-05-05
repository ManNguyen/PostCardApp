angular.module("PostCardApp").directive("postCardPage",["$http","$sce", "AuthenticateService",function ($http,$sce,AuthenticateService) {
    return {
        restrict: 'EA',
        templateUrl: "Angular/Pages/PostCard.Page.html",
     
        link: function (scope, element, attributes) {
            scope.Name = AuthenticateService.username();
            $http({
                method: 'GET',
                url: 'api/PostCard',
                responseType: 'arraybuffer',
                Accept: 'application/jpeg',
           }).then(function (response) {
                var blob = new Blob([response.data], { type: "application/jpeg" });
                var fileURL = URL.createObjectURL(blob);
                scope.JpgImageSrc = $sce.trustAsResourceUrl(fileURL);
                scope.errorMsg =undefined
            },
            function (error) {
                console.log(error.statusText);
                scope.JpgImageSrc = undefined;
                scope.errorMsg = error.statusText
            })
        }
    }


}]);
