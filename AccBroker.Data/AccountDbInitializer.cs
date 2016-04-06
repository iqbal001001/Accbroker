using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccBroker.Domain;

namespace AccBroker.Data
{
    public class AccountDbInitializer : DropCreateDatabaseAlways<AccountDBContext> // DropCreateDatabaseIfModelChanges<AccountDBContext>//CreateDatabaseIfNotExists, DropCreateDatabaseIfModelChanges, AlwaysRecreateDatabase
    {
        protected override void Seed(AccountDBContext context)
        {
            var address = new Address { AddressLine1 = "line1", AddressLine2 = "line2", Suburb = "Sub", State = "NNN", PostCode = "0000", AddressType = 1, ChangeDate = DateTime.Now };

            var companies = new List<Company> { 
                new Company{ Name = "Company A", ABN = "12345", ChangeDate = DateTime.Now, Concurrency = Guid.NewGuid(), Addresses = new List<Address> { address }},
                new Company{ Name = "Company B", ABN = "23456", ChangeDate = DateTime.Now, Concurrency = Guid.NewGuid(), Addresses = new List<Address> { address }},
                new Company{ Name = "Company C", ABN = "34567", ChangeDate = DateTime.Now, Concurrency = Guid.NewGuid(), Addresses = new List<Address> { address }}
            };

            var clients = new List<Client> { 
                new Client{ Name = "Client A", ABN = "45678", ChangeDate = DateTime.Now, Concurrency = Guid.NewGuid(), Addresses = new List<Address> { address }},
                new Client{ Name = "Client B", ABN = "56789", ChangeDate = DateTime.Now, Concurrency = Guid.NewGuid(), Addresses = new List<Address> { address }},
                new Client{ Name = "Client C", ABN = "67890", ChangeDate = DateTime.Now, Concurrency = Guid.NewGuid(), Addresses = new List<Address> { address }}
            };

            companies.ForEach(company => context.Companies.Add(company));
            clients.ForEach(client => context.Clients.Add(client));

            var product1 = new Product { ProductCode = "P1", ProductName = "Prod 1", ProductDescription = "Prod 1 D", CostPrice = 1, SellPrice = 2, ChangeDate = DateTime.Now, Concurrency = Guid.NewGuid() };
            var product2 = new Product { ProductCode = "P2", ProductName = "Prod 2", ProductDescription = "Prod 2 D", CostPrice = 10, SellPrice = 20, ChangeDate = DateTime.Now, Concurrency = Guid.NewGuid() };
            var product3 = new Product { ProductCode = "P3", ProductName = "Prod 3", ProductDescription = "Prod 3 D", CostPrice = 15, SellPrice = 30, ChangeDate = DateTime.Now, Concurrency = Guid.NewGuid() };

            var products = new List<Product> { product1, product2, product3 };

            products.ForEach(product => context.Products.Add(product));

            var prodInvoice1Item1 = new ProductInvoiceItem
            {
                ProductCode = product1.ProductCode,
                ProductName = product1.ProductName,
                Product = product1,
                Quantity = 1,
                UnitPrice = product1.SellPrice
            };

            var invoice1 = new Invoice
            {
                Client = clients.First(c => c.Name == "Client A"),
                Company = companies.First(c => c.Name == "Company A"),
                BillingAddress = 1,
                InvoiceNo = "100",
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now,
                InvoiceType = 1,
                GST = 10,
                Amount = 100,
                ChangeDate = DateTime.Now,
                Concurrency = Guid.NewGuid(),
                InvoiceDescription = "desc",
                ProductInvoiceItems = new List<ProductInvoiceItem> {
                    prodInvoice1Item1
                },
                InvoiceItems = new List<InvoiceItem> { 
                      new InvoiceItem { InvocieType = 1,
                                        Discount = 0,
                                        GST = 10,
                                        Amount = 100,
                                        ChangeDate = DateTime.Now,
                                        Description = "desc",
                                        ProductInvoiceItem = prodInvoice1Item1}
                  }
            };

            var prodInvoice2Item1 = new ProductInvoiceItem
            {
                ProductCode = product1.ProductCode,
                ProductName = product1.ProductName,
                Product = product1,
                Quantity = 1,
                UnitPrice = product1.SellPrice
            };

            var prodInvoice2Item2 = new ProductInvoiceItem
            {
                ProductCode = product2.ProductCode,
                ProductName = product2.ProductName,
                Product = product2,
                Quantity = 1,
                UnitPrice = product2.SellPrice
            };

            var invoice2 = new Invoice
            {
                Client = clients.First(c => c.Name == "Client B"),
                Company = companies.First(c => c.Name == "Company A"),
                BillingAddress = 1,
                InvoiceNo = "200",
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now,
                InvoiceType = 1,
                GST = 40,
                Amount = 400,
                ChangeDate = DateTime.Now,
                Concurrency = Guid.NewGuid(),
                InvoiceDescription = "desc",
                ProductInvoiceItems = new List<ProductInvoiceItem> {
                    prodInvoice2Item1, 
                    prodInvoice2Item2
                },
                InvoiceItems = new List<InvoiceItem> { 
                          new InvoiceItem { InvocieType = 1,
                                            Discount = 0,
                                            GST = 20,
                                            Amount = 200,
                                            ChangeDate = DateTime.Now,
                                            Description = "desc",
                                            ProductInvoiceItem = prodInvoice2Item1},
                          new InvoiceItem { InvocieType = 1,
                                            Discount = 0,
                                            GST = 20,
                                            Amount = 200,
                                            ChangeDate = DateTime.Now,
                                            Description = "desc",
                                            ProductInvoiceItem = prodInvoice2Item2}
                      }
            };

            var prodInvoice3Item1 = new ProductInvoiceItem
            {
                ProductCode = product1.ProductCode,
                ProductName = product1.ProductName,
                Product = product1,
                Quantity = 1,
                UnitPrice = product1.SellPrice
            };

            var prodInvoice3Item2 = new ProductInvoiceItem
            {
                ProductCode = product1.ProductCode,
                ProductName = product1.ProductName,
                Product = product1,
                Quantity = 1,
                UnitPrice = product1.SellPrice
            };
            var prodInvoice3Item3 = new ProductInvoiceItem
            {
                ProductCode = product3.ProductCode,
                ProductName = product3.ProductName,
                Product = product3,
                Quantity = 1,
                UnitPrice = product3.SellPrice
            };

            var invoice3 = new Invoice
            {
                Client = clients.First(c => c.Name == "Client C"),
                Company = companies.First(c => c.Name == "Company A"),
                BillingAddress = 1,
                InvoiceNo = "300",
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now,
                InvoiceType = 1,
                GST = 50,
                Amount = 500,
                ChangeDate = DateTime.Now,
                Concurrency = Guid.NewGuid(),
                InvoiceDescription = "desc",
                ProductInvoiceItems = new List<ProductInvoiceItem> {
                    prodInvoice3Item1, prodInvoice3Item2, prodInvoice3Item3
                },
                InvoiceItems = new List<InvoiceItem> { 
                          new InvoiceItem { InvocieType = 1,
                                            Discount = 0,
                                            GST = 20,
                                            Amount = 200,
                                            ChangeDate = DateTime.Now,
                                            Description = "desc",
                                            ProductInvoiceItem = prodInvoice3Item1},
                          new InvoiceItem { InvocieType = 1,
                                            Discount = 0,
                                            GST = 20,
                                            Amount = 200,
                                            ChangeDate = DateTime.Now,
                                            Description = "desc",
                                            ProductInvoiceItem = prodInvoice3Item2},
                          new InvoiceItem { InvocieType = 1,
                                            Discount = 0,
                                            GST = 10,
                                            Amount = 100,
                                            ChangeDate = DateTime.Now,
                                            Description = "desc",
                                            ProductInvoiceItem = prodInvoice3Item3}
                      }
            };

            var prodInvoice4Item1 =  new ProductInvoiceItem { ProductCode = product1.ProductCode, 
                                                                                          ProductName = product1.ProductName, 
                                                                                          Product = product1 ,
                                                                                          Quantity = 1,
                                                                                          UnitPrice = product1.SellPrice};

            var prodInvoice4Item2 =  new ProductInvoiceItem { ProductCode = product2.ProductCode, 
                                                                                          ProductName = product2.ProductName, 
                                                                                          Product = product2 ,
                                                                                          Quantity = 1,
                                                                                          UnitPrice = product2.SellPrice};

            var invoice4 = new Invoice
            {
                Client = clients.First(c => c.Name == "Client B"),
                Company = companies.First(c => c.Name == "Company A"),
                BillingAddress = 1,
                InvoiceNo = "400",
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now,
                InvoiceType = 1,
                GST = 20,
                Amount = 200,
                ChangeDate = DateTime.Now,
                Concurrency = Guid.NewGuid(),
                InvoiceDescription = "desc",
                ProductInvoiceItems = new List<ProductInvoiceItem> {
                    prodInvoice4Item1, prodInvoice4Item2
                },
                InvoiceItems = new List<InvoiceItem> { 
                          new InvoiceItem { InvocieType = 1,
                                            Discount = 0,
                                            GST = 10,
                                            Amount = 100,
                                            ChangeDate = DateTime.Now,
                                            Description = "desc",
                                            ProductInvoiceItem =prodInvoice4Item1},
                          new InvoiceItem { InvocieType = 1,
                                            Discount = 0,
                                            GST = 10,
                                            Amount = 100,
                                            ChangeDate = DateTime.Now,
                                            Description = "desc",
                                            ProductInvoiceItem =prodInvoice4Item2}
                      }
            };

            var invoices = new List<Invoice> { invoice1, invoice2, invoice3, invoice4 };

            invoices.ForEach(invoice => context.Invoices.Add(invoice));

            var payment1 = new Payment
            {
                Client = clients.First(c => c.Name == "Client A"),
                Company = companies.First(c => c.Name == "Company A"),
                Concurrency = Guid.NewGuid(),
                GST = 10,
                Amount = 100,
                ChangeDate = DateTime.Now,
                Description = "desc",
                PaymentNo = "101",
                PaymentType = 1,
                PaymentItems = new List<PaymentItem> {
                    new PaymentItem 
                    {
                        Invoice = invoice1,
                        InvoiceNo = invoice1.InvoiceNo,
                        Description = "desc",
                        ChangeDate = DateTime.Now,
                        GST = 0,
                        Amount = 110
                    }
                }
            };

            var payment2 = new Payment
            {
                Client = clients.First(c => c.Name == "Client B"),
                Company = companies.First(c => c.Name == "Company A"),
                Concurrency = Guid.NewGuid(),
                GST = 0,
                Amount = 220,
                ChangeDate = DateTime.Now,
                Description = "desc",
                PaymentNo = "102",
                PaymentType = 1,
                PaymentItems = new List<PaymentItem> {
                    new PaymentItem 
                    {
                        Invoice = invoice2,
                        InvoiceNo = invoice2.InvoiceNo,
                        Description = "desc",
                        ChangeDate = DateTime.Now,
                        GST = 0,
                        Amount = 110
                    },
                    new PaymentItem 
                    {
                        Invoice = invoice4,
                        InvoiceNo = invoice4.InvoiceNo,
                        Description = "desc",
                        ChangeDate = DateTime.Now,
                        GST = 0,
                        Amount = 110
                    }

                }
            };

            var payment3 = new Payment
            {
                Client = clients.First(c => c.Name == "Client B"),
                Company = companies.First(c => c.Name == "Company A"),
                Concurrency = Guid.NewGuid(),
                GST = 0,
                Amount = 300,
                ChangeDate = DateTime.Now,
                Description = "desc",
                PaymentNo = "103",
                PaymentType = 1,
                PaymentItems = new List<PaymentItem> {
                    new PaymentItem 
                    {
                        Invoice = invoice2,
                        InvoiceNo = invoice2.InvoiceNo,
                        Description = "desc",
                        ChangeDate = DateTime.Now,
                        GST = 0,
                        Amount = 200
                    },
                    new PaymentItem 
                    {
                        Invoice = invoice4,
                        InvoiceNo = invoice4.InvoiceNo,
                        Description = "desc",
                        ChangeDate = DateTime.Now,
                        GST = 0,
                        Amount = 100
                    }

                }
            };

            var payment4 = new Payment
            {
                Client = clients.First(c => c.Name == "Client B"),
                Company = companies.First(c => c.Name == "Company A"),
                Concurrency = Guid.NewGuid(),
                GST = 0,
                Amount = 300,
                ChangeDate = DateTime.Now,
                Description = "desc",
                PaymentNo = "104",
                PaymentType = 1,
                PaymentItems = new List<PaymentItem> {
                     new PaymentItem 
                    {
                        Invoice = invoice2,
                        InvoiceNo = invoice2.InvoiceNo,
                        Description = "desc",
                        ChangeDate = DateTime.Now,
                        GST = 0,
                        Amount = 100
                    },
                    new PaymentItem 
                    {
                        Invoice = invoice3,
                        InvoiceNo = invoice3.InvoiceNo,
                        Description = "desc",
                        ChangeDate = DateTime.Now,
                        GST = 0,
                        Amount = 200
                    },
                    new PaymentItem 
                    {
                        Invoice = invoice4,
                        InvoiceNo = invoice4.InvoiceNo,
                        Description = "desc",
                        ChangeDate = DateTime.Now,
                        GST = 0,
                        Amount = 100
                    }
                }
            };

            var payments = new List<Payment> { payment1, payment2, payment3, payment4 };

            payments.ForEach(payment => context.Payments.Add(payment));

        }

    }
}
