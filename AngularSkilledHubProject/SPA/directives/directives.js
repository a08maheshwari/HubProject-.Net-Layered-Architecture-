﻿(function(){
    'use strict'

    angular.module('App').directive('myCurrentTime', ['$interval', 'dateFilter',

    function ($interval, dateFilter) {
          // return the directive link function. (compile function not needed)
          return function (scope, element, attrs) {
              var format,  // date format
                  stopTime; // so that we can cancel the time updates

              // used to update the UI
              function updateTime() {
                  element.text(dateFilter(new Date(), format));
              }

              // watch the expression, and update the UI on change.
              scope.$watch(attrs.myCurrentTime, function (value) {
                  format = value;
                  updateTime();
              });

              stopTime = $interval(updateTime, 1000);

              // listen on DOM destroy (removal) event, and cancel the next UI update
              // to prevent updating time after the DOM element was removed.
              element.on('$destroy', function () {
                  $interval.cancel(stopTime);
              });
          }
      }]);
}());