(function () {
    'use strict';
    var controllerId = 'joinController';
    angular.module('app').
        controller(controllerId,
        ['common', 'security', '$modal',
            joinController]);

    function joinController(common, security, $modal) {
        var vm = this;
        
        var getLogFn = common.logger.getLogFn;
        vm.log = getLogFn(joinController.controllerId);
        vm.logError = getLogFn(joinController.controllerId, 'error');
        vm.logSuccess = getLogFn(joinController.controllerId, 'success');
        vm.log = common.logger.getLogFn(joinController.controllerId);


        var User = function () {
            return {
                email: '',
                password: '',
                confirmPassword: ''
            }
        }

        vm.user = new User();
        vm.join = function () {
            if (!vm.joinForm.$valid) return;
            vm.message = "Processing Registration...";
            security.register(angular.copy(vm.user)).then(function () {
                //Success
            }, function (data) {
                vm.message = null;
            })
        };
        vm.schema = [
		    { label: 'Email Address', property: 'email', type: 'email', attr: { required: true } },
		    { property: 'password', type: 'password', attr: { required: true } },
		    { property: 'confirmPassword', label: 'Confirm Password', type: 'password', attr: { confirmPassword: 'vm.user.password', required: true } }
        ];

        activate([]);

        function activate(promise) {
            common.activateController([security.redirectAuthenticated('/')], joinController.controllerId).then(function () {
                vm.log("Activated Login View");
            });
        };
    }
})();


