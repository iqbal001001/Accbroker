

(function () {
 'use strict';
    var controllerId = 'confrimEmail';
    angular.module('app').
        controller(controllerId,
        ['$route', 'common', 'security', 'alert',
            confrimEmail]);

    function confrimEmail($route, common, security, alert) {
        var vm = this;

        vm.title = 'Confirm Email';
        var getLogFn = common.logger.getLogFn;
        vm.log = getLogFn(confrimEmail.controllerId);
        vm.logError = getLogFn(confrimEmail.controllerId, 'error');
        vm.logSuccess = getLogFn(confrimEmail.controllerId, 'success');

        vm.activate([]);

        function activate(promises) {
            var vm = vm;
            common.activateController([security.redirectAuthenticated('/')], confrimEmail.controllerId).then(function () {
                vm.log("Activated Confirm Email View"),
                vm.secure();
            });
        }
        function secure() {
            var vm = vm;
            security.confirmEmail($route.current.params).then(function (data) {
                alert('success', data.message);
                security.authenticate('/');
            });
        }
    }
})();

