(function () {
    'use strict';

    var controllerId = 'clientdetail';
    angular.module('app').
        controller(controllerId,
        ['$routeParams', '$location', '$scope', '$rootScope', '$window',
            'bootstrap.dialog', 'common', 'config', 'datacontext',
            clientdetail]);

    function clientdetail($routeParams, $location, $scope, $rootScope, $window,
        bsDialog, common, config, datacontext) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var logError = common.logger.getLogFn(controllerId, 'error');


        vm.activate = activate;
        vm.clientIdParameter = $routeParams.id;
        vm.cancel = cancel;
        vm.goBack = goBack;
        vm.save = save;
        vm.newclient;
        vm.deleteClient = deleteClient;
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
            common.activateController([getRequestedClient()], controllerId)
                                .then(function () { log('Activated Clientdetail View'); });

        }

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
                return vm.newclient = true;
            }

            return datacontext.client.getClient(val)
            .then(function (data) {
                vm.client = angular.copy(data.data);
                vm.originalClient = angular.copy(data.data);
                vm.addresses = vm.client.addresses;
                vm.contacts = vm.client.contacts;
                vm.newclient = false;
            }, function (error) {
                logError('Unable to get client ' + val);
                gotoClients();
            });
        }



        function cancel() {
            gotoClients();
        }

        function gotoClients() {
            $location.path('/clients');
        }

        function goBack() { $window.history.back(); }


        function save() {
            if (vm.client.id == null) {
                vm.newclient = true;
                return SaveClient();
            }
            return datacontext.client.getClient(vm.client.id)
           .then(function (data) {
               if (data != null)
               { vm.newclient = false; }
               else
               { vm.newclient = true; }

               SaveClient();
           },
               function (error) {
                   if (error.status == 404) {
                       vm.newclient = true;
                       SaveClient();
                   }
               })

        }

        function SaveClient() {
            vm.isSaving = true;
            if (vm.newclient === true) {
                return datacontext.client.saveClient(vm.client)
                    .then(function (saveResult) {
                        vm.isSaving = false;
                    }, function (error) {
                        vm.isSaving = false;
                    })
            }
            else {
                return datacontext.client.updateClient(vm.client.id, vm.client)
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

        $scope.$watch('vm.client', function(newValue, oldValue) {
            if(newValue != oldValue) {
                vm.hasChanges = !angular.equals(vm.client, vm.originalClient);
            }
        },true);
    }
})();