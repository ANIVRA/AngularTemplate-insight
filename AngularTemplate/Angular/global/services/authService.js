'use strict';
(function () {
    angular.module('app').factory('authService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

        var serviceBase = '/';
        var authServiceFactory = {};

        var _authentication = {
            isAuth: false,
            name: "",
            username: "",
            token: "",
            roles: []
        };

        var _register = function (registration) {

            _logOut();

            return $http.post('/api/account/register', registration).then(function (response) {
                return response;
            });

        };

        var _login = function (loginData) {

            var data = "grant_type=password&username=" + loginData.Username + "&password=" + loginData.Password;

            var deferred = $q.defer();

            $http.post('/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                _authentication.isAuth = true;
                _authentication.username = response.Username;
                _authentication.token = response.access_token;
                _authentication.name = response.Name;
                _authentication.roles = response.Roles;

                localStorageService.set('authorizationData', _authentication);

                deferred.resolve(response);

            }).error(function (err, status) {
                _logOut();
                deferred.reject(err);
            });

            return deferred.promise;

        };

        var _logOut = function () {

            localStorageService.remove('authorizationData');

            _authentication.isAuth = false;
            _authentication.username = "";
            _authentication.name = "";
            _authentication.token = "";
            _authentication.roles = [];

        };

        var _fillAuthData = function () {

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                _authentication.isAuth = true;
                _authentication.username = authData.userName;
                _authentication.name = authData.name;
                _authentication.token = authData.token;
                _authentication.roles = authData.roles;
            }

        }

        authServiceFactory.register = _register;
        authServiceFactory.login = _login;
        authServiceFactory.logout = _logOut;
        authServiceFactory.fillAuthData = _fillAuthData;
        authServiceFactory.authentication = _authentication;

        return authServiceFactory;
    }]);


})();