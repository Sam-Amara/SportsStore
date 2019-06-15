using SportsStore.Domain.Entities;
using System.Collections.Generic;


namespace SportsStore.Domain.Abstract
{
    public interface IProductsRepository
    {
        IEnumerable<Product> Products { get; }

        void SaveProduct(Product product);
    }
}
