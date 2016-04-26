(function () {
            'use strict';
            var controllerId = 'PageView';
            angular.module('app').
                controller(controllerId,
                ['common',
                    PageViewService]);

            function PageViewService(common) {
                this.common = common;

                this.prototype.toggleScanList = function (showMode) {
                    var vm = this;
                    vm.showUrlEntry = true;
                    vm.showScanList = showMode;
                    vm.showAdvancedOptions = !showMode;
                };

                this.prototype.showAlert = function () {

                    //alert("hello");
                };

                this.prototype.getLoginModalDialog = function () {
                    return this.loginModalDialog;
                };

                this.prototype.getScanUrlEnteredFromHomepage = function () {
                    return this.scanUrlEnteredFromHomepage;
                };
                //PageViewService.serviceId = "PageView";
                //return PageViewService;
            }
        })();
        

