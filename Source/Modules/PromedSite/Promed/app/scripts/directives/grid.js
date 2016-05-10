'use strict';

/**
 * @ngdoc directive
 * @name promedApp.directive:grid
 * @description
 * # grid
 */
angular.module('promedApp')
  .directive('grid', function () {
    return {
      template: '<div></div>',
      restrict: 'E',
      link: function postLink(scope, element, attrs) {
        element.text('this is the grid directive');
      }
    };
  });
