(function () {
    'use strict';

    var serviceId = 'localStorage';
    angular.module('app').factory(serviceId,
        ['$localStorage', localStorage]);

    function localStorage($localStorage) {

        $localStorage = $localStorage.$default({
            things: []
        });

        function Ctor() {
            //Exposed data access functions
            this.getAll = getAll,
            this.get = get,
            this.add = add,
            this.remove = remove,
            this.clear = clear
        }

        return new Ctor;


        function getAll() {
            return $localStorage.things;
        };

        function get(thing) {
            var index = getIndex(thing);
            return $localStorage.things[index];
        };

        function add(thing) {

            //var index = $localStorage.things.indexOf(thing);
            var index = getIndex(thing);
            if( index > -1) {
                $localStorage.things.splice(index, 1);
            }
            $localStorage.things.push(thing);

            broadcast($localStorage.things);
        }

        function remove(thing) {
            var index = getIndex(thing);
            if (index > -1) {
                $localStorage.things.splice(index, 1);
            }

            broadcast($localStorage.things);
        }

        function clear() {
            $localStorage.things = [];
        }

        function getIndex(thing) {
            return findPositionByProperty($localStorage.things, 'entity', thing.entity, 'id', thing.id);
        }

        function findPositionByProperty(source, prop, value, prop2, value2) {
            if (source) {
                for (var i = 0; i < source.length; i++) {
                    var x = source[i][prop];
                    if (source[i][prop] == value && source[i][prop2] == value2) {
                        return i;
                    }
                }
            }
            //throw "Couldn't find object with id: " + id;
            return -1;
        }

        function broadcast(data) {
            //common.$broadcast(config.events.storage.wipChanged, data);
        }
       

    }

})()
