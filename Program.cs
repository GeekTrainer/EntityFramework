using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFW1
{
    class Program
    {
        static void Main(string[] args)
        {
            DeleteRecord();
            Console.ReadLine();
           
        }


        static void ModifyField()
        {
            using (MMAEntities context = new MMAEntities())
            {
                int val = 10 ;
                var query = from c in context.Customers
                            where c.CustomerID == val
                            select c;

                var customer = query.FirstOrDefault();


                if (customer != null)
                {
                    customer.ZipCode = "95310";

                    context.SaveChanges();
                    Console.WriteLine("update done");
                }
            }

        }
        

        static void AddRecord()
        {
            using (MMAEntities context = new MMAEntities())
            {
                Customers customer = new Customers();
                customer.CustomerID = 11;
                customer.Name = "Flavour, SALT";
                customer.Address = "Madison Square";
                customer.City = "Manathan";
                customer.State = "New York";
                customer.ZipCode = "100001";
                context.Customers.Add(customer);

                context.SaveChanges();
                Console.WriteLine("record added");
            }

        }


        static void DeleteRecord()
        {
            using (MMAEntities context = new MMAEntities())
            {
                var customer = context.Customers.Find("Antony, Abdul");

                context.Customers.Remove(customer);

                context.SaveChanges();
                Console.WriteLine("record deleted");
            }

        }

               
        static void Exemple1()
        {
            using (MMAEntities context = new MMAEntities())
            {
                var query = from c in context.Customers select c;

                var allCustomers = query.ToList();

                foreach (var customer in allCustomers)
                    Console.WriteLine(customer.CustomerID + " " + customer.Name);
            }

        }

       
        static void ExempleGroupBy()
        {
            using (MMAEntities context = new MMAEntities())
            {
                var query = from p in context.Invoices
                            group p by new { p.InvoiceID, p.CustomerID } into productGroupbyCategory
                            select new
                            {
                                InvoiceID = productGroupbyCategory.Key.InvoiceID,
                                CustomerID = productGroupbyCategory.Key.CustomerID
                            };


                foreach (var p in query.ToList())
                    Console.WriteLine(" Group " + p.CustomerID + " " + p.InvoiceID);
                   
            }

        }

       
        static void ExempleFiltre()
        {
            String filtre = "P";

            using (MMAEntities context = new MMAEntities())
            {
                var q = from c in context.Customers
                        where c.City.StartsWith(filtre)
                        select c;

                foreach (var customer in q.ToList())                
                Console.WriteLine(" Filtrage " + customer.CustomerID + " " + customer.City);
            }
        }
               

        
        static void ExempleFirst()
        {
            using (MMAEntities context = new MMAEntities())
            {
                var query = from c in context.Customers
                            where c.Name.Contains("Johnson")
                            select c;

                var customer = query.FirstOrDefault();

                if (customer != null)
                    Console.WriteLine(customer.CustomerID + " " + customer.Name);
            }

        }
        
       
        static void ExempleOrderBy()
        {
            using (MMAEntities context = new MMAEntities())
            {
                var query = from c in context.Customers
                            orderby c.Name ascending
                            select c;

                var allCustomers = query.ToList();

                foreach (var customer in allCustomers)
                    Console.WriteLine(customer.CustomerID + " " + customer.Name);
            }
        }

        
        static void ExempleCount()
        {
            using (MMAEntities context = new MMAEntities())
            {
                var query = from lineitem in context.InvoiceLineItems
                            group lineitem by lineitem.ProductCode into groupProd
                            join prod in context.Products on groupProd.Key equals prod.ProductCode
                            orderby prod.Description  ascending
                            select new
                            {
                                ProductCode = groupProd.Key,
                                ProductName = prod.Description,
                                Quantity = groupProd.Count()
                            };

                foreach (var p in query.ToList())
                    Console.WriteLine(p.ProductName + " = " + p.Quantity);

            }
        }

      
        static void ExempleMaximum()
        {
            using (MMAEntities context = new MMAEntities())
            {
                var query = from lineitem in context.InvoiceLineItems
                            group lineitem by lineitem.ProductCode into groupProd
                            select new
                            {
                                ProductId = groupProd.Key,
                                MaxQuantity = groupProd.Max(od => od.Quantity)
                            };

                foreach (var p in query.ToList())
                    Console.WriteLine(p.ProductId + " " + p.MaxQuantity);

            }
        }


        static void ExempleAverage()
        {
            using (MMAEntities context = new MMAEntities())
            {
                var query = from inv in context.Invoices
                            group inv by inv.CustomerID into groupCust
                            select new
                            {
                                CustomerID = groupCust.Key,
                                AverageQuantity =  groupCust.Average(od => od.InvoiceTotal)
                            };

                foreach (var p in query.ToList())
                    Console.WriteLine(p.CustomerID + " = " + p.AverageQuantity);

            }
        }



        static void ExempleMultipleSelect()
        {
            using (MMAEntities context = new MMAEntities())
            {
               
                var query =  from cos in context.Customers 
                             join inv in context.Invoices on cos.CustomerID equals inv.CustomerID
                             join line in context.InvoiceLineItems on inv.InvoiceID equals line.InvoiceID                          
                              where inv.InvoiceID == 47
                           select inv;
                          
                var list = query.ToList();

                foreach (var cust in list)
                    Console.WriteLine(cust.Customers.Name + "  >>>  " + cust.Customers.Address + " " );
            }
        }





    }
}
