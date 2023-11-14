using Application.Common.Factory;
using Application.Product.Commands;
using Application.Stock.LocalStock.Commands;
using Domain;
using Application.Stock.Interfaces;
using System;

namespace BusinessImpl.Stock
{
    public class LocalStockAddHandler : BaseStockAddHandler, IStockAddHandler
    {
        public IProductInsertIfNotExistsCommand ProductInsertIfNotExistsCommand { private get; set; }
        public ILocalStockUpdateCommand StockUpdateCommand { private get; set; }

        public LocalStockAddHandler(ILocalStockUpdateCommand localStockUpdateCommand, IProductInsertIfNotExistsCommand productInsertIfNotExistsCommand) : base()
        {
            StockUpdateCommand = localStockUpdateCommand;
            ProductInsertIfNotExistsCommand = productInsertIfNotExistsCommand;
        }

        public void AddProduct(ProductEntity product, StockEntity stock, float productQuantity)
        {
            if (!(stock is LocalStockEntity))
            {
                throw new ArgumentException("Invalid type");
            }
            ProductInsertIfNotExistsCommand.Execute(product);
            AddProductUpdateStock(product, stock, productQuantity);
            StockUpdateCommand.Execute((LocalStockEntity)stock);
        }
    }
}
