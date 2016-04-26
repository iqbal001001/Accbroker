
var ChangePasswordModel = (function () {
    function ChangePasswordModel() {
        this.oldPassword = '';
        this.newPassword = '';
        this.confirmPassword = '';
    }
    return ChangePasswordModel;
})();

(function () {
    'use strict';
    var controllerId = 'manage';
    angular.module('app').
        controller(controllerId,
        ["common", "security",
            manage]);

    function manage(common, security) {
        var vm = this;

        vm.title = "Manage";
        // user = new ChangePasswordModel();
        vm.loadInfo = function () {
            vm.message = "Loading...";
            security.mangeInfo().then(function (data) {
                vm.manageInfo = data;
            }).finally(function () {
                vm.message = null;
            });
        };
        vm.changingPassword = null;
        vm.changePassword = function () {
            vm.changingPassword = new ChangePasswordModel();
        };
        vm.cancel = function () {
            vm.changingPassword = null;
        };
        vm.updatePassword = function () {
            if (!vm.manageForm.$valid)
                return;
            var newPassword = angular.copy(vm.changingPassword);
            vm.changingPassword = null;
            security.changePassword(newPassword).then(function () {
                vm.logSuccess("Password updated successfully");
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

                vm.changingPassword = newPassword;
            });
        };
        vm.associateExternal = function (login) {
            security.associateExternal(login, "/manage");
        };
        vm.removeExternal = function (userLogin) {
            userLogin.processing = true;
            security.removeLogin(userLogin).then(function () {
                vm.loadInfo();
            });
        };
        vm.userLogin = function (login) {
            var match = false;
            angular.forEach(vm.manageInfo.logins, function (userLogin) {
                if (match)
                    return;

                match = (login.name == userLogin.loginProvider) ? userLogin : false;
            });

            return match;
        };
        vm.changePasswordSchema = [
            { label: '', property: 'oldPassword', type: 'password', attr: { required: true, ngPattern: "/(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z])(?=.*[0-9])/", ngMinlength: 6 }, msgs: { required: '' } },
            {
                label: '',
                property: 'newPassword',
                type: 'password',
                attr: { required: true, ngPattern: "/(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z])(?=.*[0-9])/", ngMinlength: 6 },
                msgs: {
                    required: 'Must contain 6 characters with one number, uppercase letter, lowercase letter and non alphanumeric character'
                }
            },
            { property: 'confirmPassword', label: '', type: 'password', attr: { confirmPassword: 'vm.changingPassword.newPassword', required: true, ngMinlength: 6 }, msgs: { match: 'Your passwords need to match' } }
        ];
        var getLogFn = common.logger.getLogFn;
        vm.log = getLogFn(manage.controllerId);
        vm.logError = getLogFn(manage.controllerId, 'error');
        vm.logSuccess = getLogFn(manage.controllerId, 'success');
        vm.loadInfo();

        activate([]);

        function activate(promises) {
            var vm = vm;
            common.activateController([security.authenticate()], manage.controllerId).then(function () {
                vm.log("Activated Manage View");
            });
        }
    }

})();

