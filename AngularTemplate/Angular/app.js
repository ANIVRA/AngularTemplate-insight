(function () {
    angular.module('app', ['ui.router', 'ui.bootstrap', 'LocalStorageModule'])
        .config(['$stateProvider', '$locationProvider', '$httpProvider', '$urlRouterProvider', function ($stateProvider, $locationProvider, $httpProvider, $urlRouterProvider) {
            // UI States, URL Routing & Mapping. For more info see: https://github.com/angular-ui/ui-router
            // ------------------------------------------------------------------------------------------------------------

            $urlRouterProvider
                   // The `when` method says if the url is ever the 1st param, then redirect to the 2nd param
                   // Here we are just setting up some convenience urls.
                   // If the url is ever invalid, e.g. '/asdf', then redirect to '/' aka the home state
                   .otherwise('/');

            $stateProvider
                .state('dashboard', {
                    url: '/',
                    templateUrl: '/Angular/dashboard/views/dashboard.html',
                    data: {
                        Authorize: "All"
                    }

                })
                .state('about', {
                    url: '/about',
                    templateUrl: '/Angular/about/views/about.html',
                    data: {
                        Authorize: "All"
                    },
                    abstract: true
                })
                .state('about.overview', {
                    url: '/',
                    templateUrl: '/Angular/about/views/about.overview.html',
                    data: {
                        Authorize: "All"
                    },
                })
                .state('about.pricing', {
                    url: '/pricing',
                    templateUrl: '/Angular/about/views/about.pricing.html',
                    data: {
                        Authorize: "All"
                    },
                })

                .state('register', {
                    url: '/account/register',
                    templateUrl: '/Angular/account/views/register.html',
                    data: {
                        Authorize: "Anonymous"
                    }

                })
                .state('login', {
                    url: '/account/login',
                    templateUrl: '/Angular/account/views/login.html',
                    data: {
                        Authorize: "Anonymous"
                    }

                })
            //$locationProvider.html5Mode({
            //    enabled: true,
            //    requireBase: false
            //});
            $httpProvider.defaults.withCredentials = true;
            $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';

            $httpProvider.interceptors.push('authInterceptorService');

        }])

    // Gets executed after the injector is created and are used to kickstart the application. Only instances and constants
    // can be injected here. This is to prevent further system configuration during application run time.
    .run(['$templateCache', '$rootScope', '$state', '$stateParams', 'authService', function ($templateCache, $rootScope, $state, $stateParams, authService) {
        // Allows to retrieve UI Router state information from inside templates
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;

        authService.fillAuthData();

        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            //For later improved security
            var authorized = false;

            if (toState.data.Authorize.indexOf("Anonymous") > -1)
                authorized = true
            else {
                if (authService.authentication.isAuth) {

                    if (toState.data.Authorize.indexOf("All") > -1)
                        authorized = true;
                    else {
                        angular.forEach(authService.authentication.Roles, function (value, key) {
                            if (toState.Authorize.data.indexOf(value))
                                authorized = true;
                        });
                    }
                }
            }
            if (authorized == false) {
                event.preventDefault();
                authService.logout();
                $state.go('login');
            }
        });
        

    }]);
})();