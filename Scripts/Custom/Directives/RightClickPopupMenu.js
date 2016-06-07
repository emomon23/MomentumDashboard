
angular.module("app")
    .directive('rightClickPopupMenu', function () {
        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                //Cancel the browsers context menu pop up for this element
                element.on("contextmenu", function (e) {
                    e.preventDefault();
                });

                element.on('mousedown', (function (e) {
                    if (e.button == 2) { //The right button
                        if (attrs['prePopupCall']) {
                            scope.$eval(attrs['prePopupCall']);
                        }

                        scope.$apply();
                        
                        // If the document is clicked somewhere
                        $(document).unbind().bind("mousedown", documentMouseDownDirectiveHandler);

                        $(".custom-menu li").unbind().click(function (e) {
                            $('.custom-menu').hide(100);
                        });


                        $(".custom-menu").toggle(100).
                                                    css({
                                                        top: e.pageY + "px",
                                                        left: e.pageX + "px"
                                                    });


                    }
                }));
                             
                scope.$on('$destroy', function () {
                    //Make sure to release any event handlers to avoid memory leaks
                    if ($(document).isBound('mousedown', documentMouseDownDirectiveHandler)) {
                        $('.custom-menu').unbind();
                        $(document).unbind('mousedown', documentMouseDownDirectiveHandler);
                    }
                });

                function documentMouseDownDirectiveHandler(e) {
                    var isOnRunItem = e.target.className.indexOf(attrs['class']) >= 0;
                    var isOnPopupMenu = $(e.target).parents(".custom-menu").length > 0;

                    // If the clicked element is not the menu itself
                    if (!(isOnRunItem || isOnPopupMenu)) {
                        // Hide it
                        $(".custom-menu").hide(100);
                    }
                }
                
                $.fn.isBound = function (type, fn) {
                    var result = false;
                    var checkElement = this;

                    if (checkElement) {
                        var eventHandlers = checkElement.data('events');
                        if (eventHandlers) {

                            eventHandlers = eventHandlers[type];

                            if (angular.isArray(eventHandlers)) {
                                for (var i = 0; i < eventHandlers.length; i++) {
                                    if (eventHandlers[i].handler.name == fn.name) {
                                        result = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    return result;
                };
            }
              
        };
       
    });