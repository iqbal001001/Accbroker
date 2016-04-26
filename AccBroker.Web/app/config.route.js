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
            }, {
                url: '/workinprogress',
                config: {
                    title: 'workinprogress',
                    templateUrl: 'app/wip/wip.html',
                    settings: {
                        content: '<i class="glyphicon glyphicon-asterisk"></i> Work In Progress'
                    }
                }
            }, {
                url: '/login',
                config: {
                    title: 'login',
                    templateUrl: 'app/security/login/login.html',
                    settings: {
                        nav: 0,
                        content: 'Login'
                    }
                }
            }, {
                url: '/join',
                config: {
                    title: 'join',
                    templateUrl: 'app/security/join/join.html',
                    settings: {
                        nav: 0,
                        content: 'Join'
                    }
                }
            }, {
                url: '/forgotpassword',
                config: {
                    title: 'forgot password',
                    templateUrl: 'app/security/forgotPassword/forgotPassword.html',
                    settings: {
                        nav: 0,
                        content: 'Forgot Password'
                    }
                }
            },
            {
                url: '/manage',
                config: {
                    title: 'manage',
                    templateUrl: 'app/security/manage/manage.html',
                    settings: {
                        nav: 0,
                        content: 'Manage'
                    }
                }
            }, {
                url: '/confirmemail',
                config: {
                    title: 'confirm email',
                    templateUrl: 'app/security/confirmEmail/confirmEmail.html',
                    settings: {
                        nav: 0,
                        content: 'Confirm Email'
                    }
                }
            }, {
                url: '/resetpassword',
                config: {
                    title: 'reset password',
                    templateUrl: 'app/security/resetPassword/resetPassword.html',
                    settings: {
                        nav: 0,
                        content: 'Reset Password'
                    }
                }
            }, {
                url: '/registerExternal',
                config: {
                    title: 'register external',
                    templateUrl: 'app/security/registerExternal/registerexternal.html',
                    settings: {
                        nav: 0,
                        content: 'Register External'
                    }
                }
            }

        ];
    }
})();