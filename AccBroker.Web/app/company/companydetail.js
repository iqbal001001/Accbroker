(function () {
    'use strict';

    var controllerId = 'companydetail';
    angular.module('app').
        controller(controllerId,
        ['$routeParams', '$location', '$scope', '$rootScope', '$window',
            'bootstrap.dialog', 'common', 'config', 'datacontext',
            companydetail]);

    function companydetail($routeParams, $location, $scope, $rootScope, $window,
        bsDialog, common, config, datacontext) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var logError = common.logger.getLogFn(controllerId, 'error');


        vm.activate = activate;
        vm.companyIdParameter = $routeParams.id;
        vm.cancel = cancel;
        vm.goBack = goBack;
        vm.save = save;
        vm.newcompany;
        vm.deleteCompany = deleteCompany;
        vm.hasChanges = false;
        vm.isSaving = false;

        vm.addAddress = addAddress;
        vm.removeAddress = removeAddress;

        vm.addContact = addContact;
        vm.removeContact = removeContact;

        Object.defineProperty(vm, 'canSave', {
            get: canSave
        });

        function canSave() { return vm.hasChanges && !vm.isSaving; }

        activate();

        function activate() {
            common.activateController([getRequestedCompany()], controllerId)
                                .then(function () { log('Activated Companydetail View'); });

        }

        function deleteCompany() {
            return bsDialog.deleteDialog('Company').
                then(confirmDelete);

            function confirmDelete() {
                datacontext.company.deleteCompany(vm.company.id)
                .then(success, failed);

                function success() {
                    gotoCompanys();
                }

                function failed(error) { cancel(); }
            }

        }




        function getRequestedCompany() {
            var val = $routeParams.id;
            if (val === 'new') {
                return vm.newcompany = true;
            }

            return datacontext.company.getCompany(val)
            .then(function (data) {
                vm.company = angular.copy(data.data);
                vm.originalCompany = angular.copy(data.data);
                vm.addresses = vm.company.addresses;
                vm.contacts = vm.company.contacts;
                vm.newcompany = false;
            }, function (error) {
                logError('Unable to get company ' + val);
                gotoCompanys();
            });
        }



        function cancel() {
            gotoCompanys();
        }

        function gotoCompanys() {
            $location.path('/companys');
        }

        function goBack() { $window.history.back(); }


        function save() {
            if (vm.company.id == null) {
                vm.newcompany = true;
                return SaveCompany();
            }
            return datacontext.company.getCompany(vm.company.id)
           .then(function (data) {
               if (data != null)
               { vm.newcompany = false; }
               else
               { vm.newcompany = true; }

               SaveCompany();
           },
               function (error) {
                   if (error.status == 404) {
                       vm.newcompany = true;
                       SaveCompany();
                   }
               })

        }

        function SaveCompany() {
            vm.isSaving = true;
            if (vm.newcompany === true) {
                return datacontext.company.saveCompany(vm.company)
                    .then(function (saveResult) {
                        vm.isSaving = false;
                    }, function (error) {
                        vm.isSaving = false;
                    })
            }
            else {
                return datacontext.company.updateCompany(vm.company.id, vm.company)
                           .then(function (saveResult) {
                               vm.isSaving = false;
                           }, function (error) {
                               vm.isSaving = false;
                           })
            }
        }

        function addAddress() {
                var newaddress =
                    {
                        "ID": 0,
                        "addressLine1": '',
                        "addressLine2": '',
                        "suburb": '',
                        "state": '',
                        "postCode": '',
                        "addressType": ''
                    }
                vm.addresses.push(newaddress);
        }

        function removeAddress(idx) {
            vm.addresses.splice(idx, 1);
        }

        function addContact() {
            var newcontact =
                {
                    "ID": 0,
                    "name": '',
                    "position": '',
                    "homePhone": '',
                    "workPhone": '',
                    "mobile": '',
                    "contactType": ''
                }
            vm.contacts.push(newcontact);
        }

        function removeContact(idx) {
            vm.contacts.splice(idx, 1);
        }

        $scope.$watch('vm.company', function (newValue, oldValue) {
            if (newValue != oldValue) {
                vm.hasChanges = !angular.equals(vm.company, vm.originalCompany);
            }
        }, true);
    }
})();