(function () {
    'use strict';
    var controllerId = 'resetPasswordExternal';
    angular.module('app').
        controller(controllerId,
        ['$route', 'common', 'security',
            resetPasswordExternal]);

    function resetPasswordExternal($route, common, security) {
        var _this = this;
        this.$route = $route;
        this.common = common;
        this.security = security;
        this.title = "Reset Password";
        this.user = this.externalUser();
        this.reset = function () {
            if (!_this.resetForm.$valid)
                return;
            _this.message = "Processing Request...";
            _this.user.code = _this.$route.current.params.code;
            _this.resetPassword(angular.copy(_this.user)).then(function (data) {
                _this.security.authenticate();
                _this.logSuccess("Password reset successfully");
            }, function (data) {
                if (data.modelState) {
                    angular.forEach(data.modelState, function (errors, key) {
                        _this.logError(errors);
                    });
                }
                if (data.exceptionMessage) {
                    _this.logError(data.exceptionMessage);
                }
                if (data.error_description) {
                    _this.logError(data.error_description);
                }
                _this.message = null;
            });
        };
        this.schema = [
            { label: '', property: 'email', type: 'email', attr: { required: true } },
            {
                property: 'password', label: '', type: 'password', attr: { required: true, ngPattern: "/(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z])(?=.*[0-9])/", ngMinlength: 6 }, msgs: { required: 'Must contain 6 characters with one number, uppercase letter, lowercase letter and non alphanumeric character' } },
            { property: 'confirmPassword', label: '', type: 'password', attr: { confirmPassword: 'user.password', required: true, ngMinlength: 6 }, msgs: { match: 'Your passwords need to match' } }];
        var getLogFn = common.logger.getLogFn;
        this.log = getLogFn(resetParesetPasswordExternalssword.controllerId);
        this.logError = getLogFn(resetPasswordExternal.controllerId, 'error');
        this.logSuccess = getLogFn(resetPasswordExternal.controllerId, 'success');

        this.activate([]);

        this.externalUser = function () {
            return {
                email: '',
                password: '',
                confirmPassword: '',
                code: decodeURIComponent(_this.$route.current.params.code)
            };
        };
    }
    resetPasswordExternal.prototype.activate = function (promises) {
        var _this = this;
        this.common.activateController([this.security.redirectAuthenticated('/')], resetPasswordExternal.controllerId).then(function () {
            _this.log("Activated Login View");
        });
    };
})();

