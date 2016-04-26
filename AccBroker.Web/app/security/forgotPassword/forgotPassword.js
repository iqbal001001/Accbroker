(function () {
    'use strict';
    var controllerId = 'forgotPassword';
    angular.module('app').
        controller(controllerId,
        ["common", "security",
            forgotPassword]);

    function forgotPassword(common, security) {
        var vm = this;

        vm.title = 'Forgot Password';
        vm.user =  {
            email: "",
    };
        vm.reset = function () {
            if (!vm.forgotPasswordForm.$valid)
                return;
            vm.message = "Processing Request...";
            security.forgotPassword(angular.copy(vm.user)).then(function (data) {
                vm.logSuccess(data.message);
                security.authenticate();
            }, function (data) {
                if (data.modelState) {
                    angular.forEach(data.modelState, function (errors, key) {
                        vm.logError(errors);
                    });
                }
                vm.message = null;
            });
        };
        vm.schema = [
            { label: '', property: 'email', type: 'email', attr: { required: true }, msgs: { required: "" } }
        ];

        var getLogFn = common.logger.getLogFn;
        vm.log = getLogFn(forgotPassword.controllerId);
        vm.logError = getLogFn(forgotPassword.controllerId, 'error');
        vm.logSuccess = getLogFn(forgotPassword.controllerId, 'success');

        activate([]);

        function activate(promises) {
            common.activateController([security.redirectAuthenticated('/')], forgotPassword.controllerId).then(function () {
                vm.log("Activated Login View");
            });
        }
    }
})();
