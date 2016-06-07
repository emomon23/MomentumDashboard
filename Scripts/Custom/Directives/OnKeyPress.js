var onKeyPress = function () {
    return {
        scope: true,
        link: function (scope, element, attributes, ngModel) {

            var scopeMethodToCall = attributes.onKeyPress;

            $(element).bind('keypress', elementKeyPress);
            
            function elementKeyPress(event) {
                var functionPtr = scope[scopeMethodToCall];
                functionPtr(event);
            }

            scope.$on("$destroy", function() {
                $(element).unbind('keypress', elementKeyPress);
            });
        }
      };
};

var onKeyUp = function () {
    return {
        scope: true,
        link: function (scope, element, attributes, ngModel) {

            var scopeMethodToCall = attributes.onKeyUp;

            $(element).bind('keyup', elementKeyUp);

            function elementKeyUp(event) {
                var functionPtr = scope[scopeMethodToCall];
                functionPtr(event);
            }

            scope.$on("$destroy", function () {
                $(element).unbind('keyup', elementKeyUp);
            });
        }
    };
};

angular.module("app")
    .directive("onKeyPress", onKeyPress)
    .directive("onKeyUp", onKeyUp);