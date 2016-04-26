(function () {
    'use strict';

    var controllerId = 'wip';
    angular.module('app').
        controller(controllerId,
        ['$location', '$scope', '$rootScope', '$window',
            'bootstrap.dialog', 'common', 'config', 'datacontext',
            clientdetail]);

    function clientdetail($location, $scope, $rootScope, $window,
         bsDialog, common, config, datacontext) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var logError = common.logger.getLogFn(controllerId, 'error');

        vm.title = 'Work In Progress';
        vm.wips = [];
        vm.activate = activate;
        vm.gotoWip = gotoWip;
        vm.deleteWip = deleteWip;
     
        activate();

        function activate() {
            common.activateController([getWips()], controllerId)
                                .then(function () { log('Activated Wip View'); });
        }

        function deleteWip(wip) {
            return bsDialog.deleteDialog('Wip').
                then(confirmDelete);

            function confirmDelete() {
                 datacontext.localStorage.remove(wip);
                //.then(success, failed);

                //function success() { }

                //function failed(error) { }
            }
        }

        function getWips() {
             vm.wips = datacontext.localStorage.getAll()
            return vm.wips;
            //.then(function (data) {
            //    vm.wips = data;
            //}, function (error) {
            //    logError('Unable to get Wips ');
            //});
        }

        function gotoWip(wip) {
            $location.path(wip.route + '/' + wip.id);
        }
    }
})();