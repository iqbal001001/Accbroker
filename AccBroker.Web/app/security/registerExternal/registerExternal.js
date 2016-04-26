
(function () {
    'use strict';
    var controllerId = 'registerExternal';
    angular.module('app').
        controller(controllerId,
        ["common", "security",
            registerExternal]);

    function registerExternal(common, security) {
        var vm = this;

        vm.title = "Register External";
        vm.registerExternalUser = function () {
            if (!vm.registerForm.$valid)
                return;
            vm.message = "Processing Login...";
            security.registerExternal().then(function () {
                vm.logSuccess("You have successfully joined");
            }, function (data) {
                if (data.modelState) {
                    angular.forEach(data.modelState, function (errors, key) {
                        var error1 = errors[0];
                        var stringshortened = error1.slice(0, 4);
                        if (stringshortened === "Name") {
                            errors[0] = null;
                        }
                        vm.logError(errors);
                    });
                    vm.message = null;
                }
                ;
            });
        };
        vm.schema = [
            { property: 'userName', placeholder: 'Email', label: '', type: 'email', attr: { required: true, msg: '' } },
            { property: 'firstName', placeholder: 'First Name', label: '', type: 'text', attr: { required: true } },
            { property: 'lastName', placeholder: 'Last Name', label: '', type: 'text', attr: { required: true } }
        ];
        var getLogFn = common.logger.getLogFn;
        vm.log = getLogFn(registerExternal.controllerId);
        vm.logError = getLogFn(registerExternal.controllerId, 'error');
        vm.logSuccess = getLogFn(registerExternal.controllerId, 'success');

        vm.activate([]);

        function activate(promises) {
            var vm = vm;
            common.activateController([security.redirectAuthenticated('/')], registerExternal.controllerId).then(function () {
                vm.log("Activated Login View");
            });
        }
    }
})();

