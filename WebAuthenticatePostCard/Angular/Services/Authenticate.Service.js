angular.module("PostCardApp")
    ///Constant to identify authentication
 .constant('AUTH_EVENTS', {
     notAuthenticated: 'auth-not-authenticated',
     notAuthorized: 'auth-not-authorized',
     authStatusChanged: 'AuthenticateChange'
 })
.service('AuthenticateService', ['$q', '$http', '$rootScope', 'AUTH_EVENTS', '$state', function ($q, $http, $rootScope, AUTH_EVENTS, $state) {
    const AUTHENTICATE_URL = '/api/Authenticate';
    var LOCAL_TOKEN_KEY = 'yourTokenKey';
    var username = '';
    var userid = ""
    var expireTime;
    var isAuthenticated = false;
    var isExpired = false;
    $rootScope.$on(AUTH_EVENTS.notAuthenticated, function (event, data) {
        isAuthenticated = false;
        isExpired = true;
        $state.go('Login');
    })
    // Load Jwt Token from local computer
    function loadUserCredentials() {
        var token = window.localStorage.getItem(LOCAL_TOKEN_KEY);
        if (token) {
            useCredentials(token);
        }
        else {
            console.log("Token not found");
        }
    }
    // Save Jwt Token to local computer
    function storeUserCredentials(token) {
        window.localStorage.setItem(LOCAL_TOKEN_KEY, token);
        useCredentials(token);
    }
    // Destroy Jwt Token
    function destroyUserCredentials() {
        authToken = undefined;
        username = '';
        isAuthenticated = false;
        $http.defaults.headers.common['Authorization'] = undefined;
        window.localStorage.removeItem(LOCAL_TOKEN_KEY);
    }
    // Read the token to get basic infor such as name and id,
    // then set token as default value of authorization header so that the server can use it to authenticate upcoming request
    function useCredentials(token) {
        var decodedJwt = parseJwt(token);
        username = decodedJwt["unique_name"];
        userid = decodedJwt["nameid"];
        expireTime = parseInt(decodedJwt["exp"]);
        //Date.now() return timestamp in miliseconds  so I have to devide by 1000
        var now = Math.floor(Date.now() / 1000);
        if (expireTime < now) {
            console.log('expired');
            isAuthenticated = false;
            isExpired = true;
        }
        else {
            isAuthenticated = true;
            isExpired = false;
            authToken = token;
            // Set the token parameters and schema as default value of Authorization header for upcoming requests
            $http.defaults.headers.common['Authorization'] = "Basic " + token;
        }
        //broadcast AuthenticateChange event to change ui
        $rootScope.$broadcast(AUTH_EVENTS.authStatusChanged);
    }

    function parseJwt(token) {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace('-', '+').replace('_', '/');
        return JSON.parse(window.atob(base64));
    };

    var login = function (name, pw) {
        return $q(function (resolve, reject) {
            //Communicate with back end on this function
            var password = pw;
            var encoded = window.btoa(name + '|' + password);
            $http({
                method: 'GET',
                url: AUTHENTICATE_URL,
                headers: {
                    'Authorization': 'Basic ' + encoded,
                }
            }).then(
            function (response) {
                //Pass the authentication 
                storeUserCredentials(response.headers('AuthenticateToken'));
                resolve('Login success.');
            }, function (error) {
                reject("Cannot Login");
            })
        });
    };
    var logout = function () {
        destroyUserCredentials();
        $rootScope.$broadcast(AUTH_EVENTS.authStatusChanged);
    };
    var register = function (name, email, password) {
        return $q(function (resolve, reject) {
            console.log('Register Service');
            $http({
                method: 'GET',
                url: AUTHENTICATE_URL+"?name="+name+"&email="+email+"&password="+password
            }).then(function (response) {
                console.log(response);
                resolve('Register Success.');
            }, function (error) {
                console.log(error.data);
                reject(error.data);
            })
        });
    };

    //Try to load Local Credential when app turn on
    loadUserCredentials();

    return {
        login: login,
        logout: logout,
        register:register,
        isAuthenticated: function () { return isAuthenticated; },
        isExpired: function () { return isExpired; },
        username: function () { return username; },
        userID: function () { return userid; },
    };
}])
.factory('AuthInterceptor', ['$rootScope', '$q', 'AUTH_EVENTS', function ($rootScope, $q, AUTH_EVENTS) {
    return {
        responseError: function (response) {
            $rootScope.$broadcast({
                401: AUTH_EVENTS.notAuthenticated,
                403: AUTH_EVENTS.notAuthorized
            }[response.status], response);
            return $q.reject(response);
        }
    };
}])

.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('AuthInterceptor');
}]);