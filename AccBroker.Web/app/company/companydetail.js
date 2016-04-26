(function () {
    'use strict';

    var controllerId = 'companydetail';
    angular.module('app').
        controller(controllerId,
        ['$routeParams', '$location', '$scope', '$rootScope', '$window',
            'bootstrap.dialog', 'common', 'config', 'datacontext','$q',
            companydetail]);

    function companydetail($routeParams, $location, $scope, $rootScope, $window,
        bsDialog, common, config, datacontext, $q) {
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
        vm.checkCode = checkCode;

        vm.addAddress = addAddress;
        vm.removeAddress = removeAddress;

        vm.addContact = addContact;
        vm.removeContact = removeContact;

        vm.guid;

        Object.defineProperty(vm, 'canSave', {
            get: canSave
        });

        function canSave() { return vm.hasChanges && !vm.isSaving; }

        activate();

        function activate() {
            common.activateController([getRequestedCompany()], controllerId)
                                .then(function () { log('Activated Companydetail View'); });

        }

        vm.states = ['NSW', 'ACT', 'SA', 'WA', 'TAS', 'QND', 'VIC'];

        vm.addressTypes = [{ 'id': 1, 'type': 'home' }, { 'id': 2, 'type': 'work' }]

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
                vm.guid = common.createGuid();
                return vm.newcompany = true;
            }

            var wip = datacontext.localStorage.get(
                { entity: 'company', id: val });

            if (wip) {
                if (wip.state == 'Add') {
                    vm.guid = wip.id;
                    vm.newcompany = true;
                }
                else {
                    vm.newcompany = false;
                }
                vm.company = wip.current;
                vm.originalCompany = wip.original;
                bindData(vm.company);
                return vm.hasChanges = true;
            }

            return datacontext.company.getCompany(val)
            .then(function (data) {
                vm.company = angular.copy(data.data);
                vm.originalCompany = angular.copy(data.data);
                bindData(vm.company);
                vm.newcompany = false;
            }, function (error) {
                logError('Unable to get company ' + val);
                gotoCompanys();
            });
        }

        function bindData(data) {
            vm.addresses = data.addresses;
            vm.contacts = data.contacts;
        }

        function cancel() {
            vm.company = angular.copy(vm.originalCompany);
            removeFromStore();
           // gotoCompanys();
        }

        function gotoCompanys() {
            $location.path('/companys');
        }

        function goBack() { $window.history.back(); }


        function save() {
            SaveCompany();
           // if (vm.company.id == null) {
           //     vm.newcompany = true;
           //     return SaveCompany();
           // }
           // return datacontext.company.getCompany(vm.company.id)
           //.then(function (data) {
           //    if (data != null)
           //    { vm.newcompany = false; }
           //    else
           //    { vm.newcompany = true; }

           //    SaveCompany();
           //},
           //    function (error) {
           //        if (error.status == 404) {
           //            vm.newcompany = true;
           //            SaveCompany();
           //        }
           //    })

        }

        function SaveCompany() {
            vm.isSaving = true;
            if (vm.newcompany === true) {
                return datacontext.company.saveCompany(vm.company)
                    .then(function (saveResult) {
                        vm.company.id = saveResult.data.id;
                        removeFromStore();
                        vm.originalCompany = angular.copy(vm.company);
                        vm.newcompany = false;
                        vm.hasChanges = false;
                    }, function (error) {
                      
                    }).finally(function () {
                        vm.isSaving = false;
                    })
            }
            else {
                return datacontext.company.updateCompany(vm.company.id, vm.company)
                           .then(function (saveResult) {
                               removeFromStore();
                               vm.originalClient = angular.copy(vm.client);
                               vm.hasChanges = false;
                           }, function (error) {
                             
                           }).finally(function () {
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
                if (vm.hasChanges) {
                    addToStore();
                }
            }
        }, true);

        function addToStore() {
            var obj = {
                entity: 'company',
                id: vm.newcompany == true ? vm.guid : vm.company.id,
                orginal: vm.originalCompany,
                current: vm.company,
                description: vm.company.name,
                route: '/company',
                state: vm.newcompany == true ? 'Add' : 'update',
                date: new Date()
            };
            datacontext.localStorage.add(obj);
        }

        function removeFromStore() {
            datacontext.localStorage.remove({ entity: 'company', id: vm.newcompany == true ? vm.guid : vm.company.id });
        }

        function checkCode(code) {
            if (code) {
                return $q(function (resolve, reject) {
                    datacontext.company.codeAvailable(vm.company !=  null ? vm.company.id : null  , code)
                                            //.$promise
                                            .then(function (result) {
                                                if (result.data) {
                                                    if (result.data.codeAvailable == true) {
                                                         resolve(result);
                                                    } else {
                                                        reject(result);
                                                    }
                                                } else {
                                                    reject(result);
                                                }
                                            },
                                            function (error) {
                                                    resolve("unexpected error");
                                            });
                });
            } else {
                return $q(function (resolve) {
                    resolve();
                });
            }
    
        }
    }
})();