using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Repositiory.Data
{
    public static class StoreContextSeed
    {

        public static async Task SeedAsync(StoreContext dbcontext)
        {
            //Product brand seed
            if (!dbcontext.Set<ProductBrand>().Any())
            {
                var BrandsData = File.ReadAllText("../Talabat.Repositiory/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);

                if (Brands?.Count() > 0)
                {
                    foreach (var Brand in Brands)
                    {
                        await dbcontext.Set<ProductBrand>().AddAsync(Brand);
                    }

                }
            }
                //product types seed
                if (!dbcontext.Set<ProductType>().Any())
                {
                    var TypesData = File.ReadAllText("../Talabat.Repositiory/Data/DataSeed/types.json");
                    var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                    if (Types?.Count() > 0)
                    {
                        foreach (var type in Types)
                        {
                            await dbcontext.Set<ProductType>().AddAsync(type);
                        }

                    }
                }

                //products seed
                if (!dbcontext.Set<Product>().Any())
                {
                    var ProductsData = File.ReadAllText("../Talabat.Repositiory/Data/DataSeed/products.json");
                    var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                    if (Products?.Count() > 0)
                    {
                        foreach (var product in Products)
                        {
                            await dbcontext.Set<Product>().AddAsync(product);
                        }

                    }
                }
                //DelivertMethods Seed
                if (!dbcontext.DeliveryMethods.Any())
                {
                    var DeliveryMethodsData = File.ReadAllText("../Talabat.Repositiory/Data/DataSeed/delivery.json");
                    var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                    if (DeliveryMethods?.Count() > 0)
                    {
                        foreach (var delivery in DeliveryMethods)
                        {
                            await dbcontext.Set<DeliveryMethod>().AddAsync(delivery);
                        }

                    }
                   
                }
                await dbcontext.SaveChangesAsync();
        }
    }
}

