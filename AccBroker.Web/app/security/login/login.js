var LoginModel = (function () {
    function LoginModel() {
        this.username = "";
        this.password = "";
        this.rememberMe = false;
    }
    return LoginModel;
})();

(function () {
    'use strict';
    var controllerId = 'login';
    angular.module('app').
        controller(controllerId,
        ['common', 'security',
            login]);


    function login(common, security) {
        var vm = this;
        vm.common = common;
        vm.security = security;
        vm.title = "Login";
        vm.user = {
                username: 'acc@broker.com.au',
                password: '@Accbroker2016',
                rememberMe: false
            };
       
        vm.login = function () {
            if (!vm.loginForm.$valid)
                return;
            vm.message = "Processing Login...";
            vm.security.login(angular.copy(vm.user)).then(function (data) {
                vm.logSuccess("Successfully logged in");
            }, function (data) {
                var msg = data.error_description;
                vm.logError(msg);
            });
            vm.message = null;
        };
        //autofields.js to make login form
        vm.schema = [
            { label: '', placeholder: 'Email', property: 'username', type: 'email', attr: { required: false }, msgs: { required: '' } },
            { label: '', property: 'password', type: 'password', attr: { required: false }, msgs: { required: '' } },
            { property: 'rememberMe', label: 'Keep me logged in', type: 'checkbox' }
        ];
        vm.options = {
            validation: {
                enabled: false
            }
        };


       

        var getLogFn = common.logger.getLogFn;
        vm.log = getLogFn(login.controllerId);
        vm.logError = getLogFn(login.controllerId, 'error');
        vm.logSuccess = getLogFn(login.controllerId, 'success');

        activate([]);

        function activate(promise) {
            vm.common.activateController([vm.security.redirectAuthenticated('/')], login.controllerId).then(function () {
                vm.log("Activated Login View");
            });
        };

       
    }
    
})();

