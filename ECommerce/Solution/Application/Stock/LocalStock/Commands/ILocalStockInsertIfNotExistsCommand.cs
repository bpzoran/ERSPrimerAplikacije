﻿using Application.Common.Commands;
using Domain;

namespace Application.Stock.LocalStock.Commands
{
    public interface ILocalStockInsertIfNotExistsCommand: IInsertIfNotExistsCommand<LocalStockEntity>
    {
    }
}
