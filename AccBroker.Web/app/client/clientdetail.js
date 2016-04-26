(function () {
    'use strict';

    var controllerId = 'clientdetail';
    angular.module('app').
        controller(controllerId,
        ['$routeParams', '$location', '$scope', '$rootScope', '$window',
            'bootstrap.dialog', 'common', 'config', 'datacontext', '$q',
            'helper',
            clientdetail]);

    function clientdetail($routeParams, $location, $scope, $rootScope, $window,
        bsDialog, common, config, datacontext, $q, helper) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var logError = common.logger.getLogFn(controllerId, 'error');


        vm.activate = activate;
        vm.clientIdParameter = $routeParams.id;
        vm.cancel = cancel;
        vm.goBack = goBack;
        vm.save = save;
        vm.checkCode = checkCode;
        vm.newclient;
        vm.deleteClient = deleteClient;
        vm.hasChanges = false;
        vm.isSaving = false;
        vm.guid;

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
            common.activateController([getRequestedClient()], controllerId)
                                .then(function () { log('Activated Clientdetail View'); });

        }

        vm.states = ['NSW', 'ACT', 'SA', 'WA', 'TAS', 'QND', 'VIC'];

        vm.addressTypes = [{ 'id': 1, 'type': 'home' }, { 'id': 2, 'type': 'work' }]

        function deleteClient() {
            return bsDialog.deleteDialog('Client').
                then(confirmDelete);

            function confirmDelete() {
                datacontext.client.deleteClient(vm.client.id)
                .then(success, failed);

                function success() {
                    gotoClients();
                }

                function failed(error) { cancel(); }
            }

        }

        function getRequestedClient() {
            var val = $routeParams.id;
            if (val === 'new') {
                vm.guid = common.createGuid();
                return vm.newclient = true;
            }

            var wip = datacontext.localStorage.get({ entity: 'client',  id: val });

            if (wip) {
                if (wip.state == 'Add') {
                    vm.guid = wip.id;
                    vm.newclient = true;
                }
                else {
                    vm.newclient = false;
                }
                vm.client = wip.current;
                vm.originalClient = wip.original;
                bindData(vm.client);
               return vm.hasChanges = true;
            }

            return datacontext.client.getClient(val)
            .then(function (data) {
                vm.client = angular.copy(data.data);
                vm.originalClient = angular.copy(data.data);
                bindData(vm.client);
                vm.newclient = false;
            }, function (error) {
                logError('Unable to get client ' + val);
                gotoClients();
            });
        }

        function bindData(data) {
            vm.addresses = data.addresses;
            vm.contacts = data.contacts;
        }



        function cancel() {
            vm.client = angular.copy(vm.originalClient);
            removeFromStore();
            //gotoClients();
        }

        function gotoClients() {
            $location.path('/clients');
        }

        function goBack() { $window.history.back(); }


        function save() {
            SaveClient();
           // if (vm.client.id == null || vm.client.id == vm.guid) {
           //     //vm.client.id = null;
           //     vm.newclient = true;
           //     return SaveClient();
           // }
           // return datacontext.client.getClient(vm.client.id)
           //.then(function (data) {
           //    if (data != null)
           //    { vm.newclient = false; }
           //    else
           //    { vm.newclient = true; }

           //    SaveClient();
           //},
           //    function (error) {
           //        if (error.status == 404) {
           //            vm.newclient = true;
           //            SaveClient();
           //        }
           //    })
        }

        function SaveClient() {
            vm.isSaving = true;
            if (vm.newclient === true) {
                return datacontext.client.saveClient(vm.client)
                    .then(function (saveResult) {
                        vm.client.id = saveResult.data.id;
                        window.location.pathname.replace(vm.guid, saveResult.data.id); //helper.replaceLocationUrlGuidWithId(saveResult.data.id);
                        removeFromStore();
                        vm.originalClient = angular.copy(vm.client);
                        vm.newclient = false;
                        vm.hasChanges = false;
                    }, function (error) {
                        //todo : error display
                    }).finally(function () {
                        vm.isSaving = false;
                    })
            }
            else {
                return datacontext.client.updateClient(vm.client.id, vm.client)
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

        $scope.$watch('vm.client', function(newValue, oldValue) {
            if(newValue != oldValue) {
                vm.hasChanges = !angular.equals(vm.client, vm.originalClient);
                if (vm.hasChanges) {
                    addToStore();
                }
            }
        }, true);

        function addToStore() {
            var obj = {
                entity: 'client',
                id: vm.newclient == true ? vm.guid : vm.client.id,
                orginal: vm.originalClient,
                current: vm.client,
                description: vm.client.name,
                route: '/client',
                state: vm.newclient == true ? 'Add' : 'update',
                date: new Date()
            };
            datacontext.localStorage.add(obj);
        }

        function removeFromStore() {
            datacontext.localStorage.remove({ entity: 'client', id: vm.newclient == true ? vm.guid : vm.client.id });
        }

        function checkCode(code) {
            if (code) {
                return $q(function (resolve, reject) {
                    datacontext.client.codeAvailable(vm.client !=  null ? vm.client.id : null, code)
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