using Otlob.Core.Entites;
using Otlob.Core.Entites.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Otlob.Repository.Data
{
    public static class StoreContextSeed
    {

        //Seeding
        public static async Task SeedAsync(StoreContext dbContext)
        {

            if (!dbContext.ProductBrands.Any())
            {
                var BrandData = File.ReadAllText("../Otlob.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);
                if (Brands?.Count() > 0)
                {
                    foreach (var Brand in Brands)
                    {
                        await dbContext.Set<ProductBrand>().AddAsync(Brand);

                    }
                    await dbContext.SaveChangesAsync();

                }

            }




            //Seeding Types
            if (!dbContext.ProductTypes.Any())
            {

                var TypesData = File.ReadAllText("../Otlob.Repository/Data/DataSeed/types.json");

                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);

                if (Types?.Count() > 0)
                {
                    foreach (var Type in Types)
                    {
                        await dbContext.Set<ProductType>().AddAsync(Type);

                    }
                    await dbContext.SaveChangesAsync();

                }


            }

            //Products seeding


            if (!dbContext.Products.Any())
            {

                var ProductsData = File.ReadAllText("../Otlob.Repository/Data/DataSeed/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                if (products?.Count() > 0)
                {
                    foreach (var product in products)
                    {
                        await dbContext.Set<Product>().AddAsync(product);

                    }

                }


            }



            if (!dbContext.DeliveryMethods.Any())
            {

                var DeliveryMethodsData = File.ReadAllText("../Otlob.Repository/Data/DataSeed/delivery.json");

                var DeliveryMethods  = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);

                if (DeliveryMethods?.Count() > 0)
                {
                    foreach (var DeliveryMethod in DeliveryMethods)
                    {
                        await dbContext.Set<DeliveryMethod>().AddAsync(DeliveryMethod);

                    }

                }


            }




            await dbContext.SaveChangesAsync();

        }


    }
}
