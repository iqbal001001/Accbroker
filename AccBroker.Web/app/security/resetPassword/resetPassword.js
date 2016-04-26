
(function () {
    'use strict';
    var controllerId = 'resetPassword';
    angular.module('app').
        controller(controllerId,
        ["$route", "common", "security",
            resetPassword]);

    function resetPassword($route, common, security) {
        var vm = vm;

        vm.title = "Reset Password";
        //user =  vm.externalUser();
        vm.reset = function () {
            if (!vm.resetForm.$valid)
                return;
            vm.message = "Processing Request...";
            vm.user.code = $route.current.params.code;
            vm.security.resetPassword(angular.copy(vm.user)).then(function (data) {
                vm.security.authenticate();
                vm.logSuccess("Password reset successfully");
            }, function (data) {
                if (data.modelState) {
                    angular.forEach(data.modelState, function (errors, key) {
                        vm.logError(errors);
                    });
                }
                if (data.exceptionMessage) {
                    vm.logError(data.exceptionMessage);
                }
                if (data.error_description) {
                    vm.logError(data.error_description);
                }
                vm.message = null;
            });
        };
        vm.schema = [
            { label: '', property: 'email', type: 'email', attr: { required: true } },
            { property: 'password', label: '', type: 'password', attr: { required: true, ngMinlength: 6 }, msgs: { required: 'Use at least 6 characters' } },
            { property: 'confirmPassword', label: '', type: 'password', attr: { confirmPassword: 'vm.user.password', required: true, ngMinlength: 6 }, msgs: { match: 'Your passwords need to match' } }];
        var getLogFn = common.logger.getLogFn;
        vm.log = getLogFn(resetPassword.controllerId);
        vm.logError = getLogFn(resetPassword.controllerId, 'error');
        vm.logSuccess = getLogFn(resetPassword.controllerId, 'success');

        vm.activate([]);

        function activate(promises) {
            var vm = vm;
            vm.common.activateController([vm.security.redirectAuthenticated('/')], resetPassword.controllerId).then(function () {
                vm.log("Activated Login View");
            });

            vm.externalUser = function () {
                return {
                    email: '',
                    password: '',
                    confirmPassword: '',
                    code: decodeURIComponent($route.current.params.code)
                };
            };
            vm.user = vm.externalUser();
        }
    }
})();
