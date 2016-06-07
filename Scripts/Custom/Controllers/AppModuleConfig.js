angular.module("app")
.config(function ($routeProvider) {
    $routeProvider.when("/checkout",
        { templateUrl: "/html/CartSummary.html" });

    $routeProvider.when('/products',
        { templateUrl: "/html/ListOfProducts.html" });

    $routeProvider.when('/applications',
       { templateUrl: "/html/applicationlist.html" });

    $routeProvider.when('/clients',
      { templateUrl: "/html/clientlist.html" });

    $routeProvider.when('/testHistory',
        { templateUrl: '/html/TestHistoryList.html' });

    $routeProvider.otherwise({
         
    });
});