var app = angular.module('app', ['ngMaterial', 'ngMessages'])
.config(function ($mdThemingProvider) {
    $mdThemingProvider.theme('default')
      .primaryPalette('orange')
      .accentPalette('green')
     .dark();
});