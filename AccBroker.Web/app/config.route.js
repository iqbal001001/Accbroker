(function () {
    'use strict';

    var app = angular.module('app');

    // Collect the routes
    app.constant('routes', getRoutes());
    
    // Configure the routes and route resolvers
    app.config(['$routeProvider', 'routes', routeConfigurator]);
    function routeConfigurator($routeProvider, routes) {

        routes.forEach(function (r) {
            $routeProvider.when(r.url, r.config);
        });
        $routeProvider.otherwise({ redirectTo: '/' });
    }

    // Define the routes 
    function getRoutes() {
        return [
            {
                url: '/',
                config: {
                    templateUrl: 'app/dashboard/dashboard.html',
                    title: 'dashboard',
                    settings: {
                        nav: 1,
                        content: '<i class="fa fa-dashboard"></i> Dashboard'
                    }
                }
            }, {
                url: '/admin',
                config: {
                    title: 'admin',
                    templateUrl: 'app/admin/admin.html',
                    settings: {
                        nav: 2,
                        content: '<i class="fa fa-lock"></i> Admin'
                    }
                }
            }, {
                url: '/company',
                config: {
                    title: 'company',
                    templateUrl: 'app/company/company.html',
                    settings: {
                        nav: 3,
                        content: '<i class="fa fa-lock"></i> Company'
                    }
                }
            }, {
                url: '/company/:id',
                config: {
                    title: 'company',
                    templateUrl: 'app/company/companydetail.html',
                    settings: {
                       
                    }
                }
            }, {
                url: '/companies/search/:search',
                config: {
                    title: 'companies search',
                    templateUrl: 'app/company/company.html',
                    settings: {

                    }
                }
            }, {
                url: '/client',
                config: {
                    title: 'client',
                    templateUrl: 'app/client/client.html',
                    settings: {
                        nav: 3,
                        content: '<i class="fa fa-lock"></i> Client'
                    }
                }
            }, {
                url: '/client/:id',
                config: {
                    title: 'client',
                    templateUrl: 'app/client/clientdetail.html',
                    settings: {

                    }
                }
            }, {
                url: '/clients/search/:search',
                config: {
                    title: 'clients search',
                    templateUrl: 'app/client/client.html',
                    settings: {

                    }
                }
            }, {
                url: '/invoice',
                config: {
                    title: 'invoice',
                    templateUrl: 'app/invoice/invoice.html',
                    settings: {
                        nav: 3,
                        content: '<i class="fa fa-lock"></i> Invoice'
                    }
                }
            }, {
                url: '/invoice/:id',
                config: {
                    title: 'invoice',
                    templateUrl: 'app/invoice/invoicedetail.html',
                    settings: {

                    }
                }
            }, {
                url: '/invoices/search/:search',
                config: {
                    title: 'Invoices search',
                    templateUrl: 'app/invoice/invoice.html',
                    settings: {

                    }
                }
            }, {
                url: '/payment',
                config: {
                    title: 'payment',
                    templateUrl: 'app/payment/payment.html',
                    settings: {
                        nav: 3,
                        content: '<i class="fa fa-lock"></i> Payment'
                    }
                }
            }, {
                url: '/payment/:id',
                config: {
                    title: 'payment',
                    templateUrl: 'app/payment/paymentdetail.html',
                    settings: {

                    }
                }
            }, {
                url: '/payments/search/:search',
                config: {
                    title: 'payments search',
                    templateUrl: 'app/payment/payment.html',
                    settings: {

                    }
                }
            }, {
                url: '/product',
                config: {
                    title: 'product',
                    templateUrl: 'app/product/product.html',
                    settings: {
                        nav: 3,
                        content: '<i class="fa fa-lock"></i> Product'
                    }
                }
            }, {
                url: '/product/:id',
                config: {
                    title: 'product',
                    templateUrl: 'app/product/productdetail.html',
                    settings: {

                    }
                }
            }, {
                url: '/products/search/:search',
                config: {
                    title: 'products search',
                    templateUrl: 'app/product/product.html',
                    settings: {

                    }
                }
            }

        ];
    }
})();