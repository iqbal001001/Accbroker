using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Reflection;
using System.Web;
using AccBroker.Domain;

namespace AccBroker.WebAPI
{
    public static class ProductExtension
    {
        public static ProductDTO ToDTO(this Product product)
        {
            return new ProductDTO
            {
                ID = product.ID,
                ProductCode = product.ProductCode != null ? product.ProductCode.Trim() : null,
                ProductName = product.ProductName != null ? product.ProductName.Trim() : null,
                ProductDescription = product.ProductDescription != null ? product.ProductDescription.Trim() : null,
                CostPrice = product.CostPrice,
                SellPrice = product.SellPrice,
                Concurrency = product.Concurrency,
                CreateDate = product.CreateDate,
                ChangeDate = product.ChangeDate
            };
        }

        public static object ToDataShapeObject(this ProductDTO product, List<string> lstOfFields)
        {
            if (!lstOfFields.Any())
            {
                return product;
            }
            else
            {
                ExpandoObject objectToReturn = new ExpandoObject();
                foreach (var field in lstOfFields)
                {
                    var fieldValue = product.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(product, null);

                    ((IDictionary<string, object>)objectToReturn).Add(field, fieldValue);

                }
                return objectToReturn;
            }

        }

        public static Product ToDomain(this ProductDTO product, Product originalProduct = null)
        {
            if (originalProduct != null && originalProduct.ID == product.ID)
            {
                originalProduct.ProductCode = product.ProductCode;
                originalProduct.ProductName = product.ProductName;
                originalProduct.ProductDescription = product.ProductDescription;
                originalProduct.CostPrice = product.CostPrice;
                originalProduct.SellPrice = product.SellPrice;
                originalProduct.Concurrency = product.Concurrency;
                originalProduct.CreateDate = product.CreateDate;
                originalProduct.ChangeDate = product.ChangeDate;

                return originalProduct;
            }
            return new Product
            {
                ID = product.ID,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                CostPrice = product.CostPrice,
                SellPrice = product.SellPrice,
                Concurrency = product.Concurrency,
                CreateDate = product.CreateDate,
                ChangeDate = product.ChangeDate
            };
        }

      
    }
}