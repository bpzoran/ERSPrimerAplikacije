﻿using Application.Order.Queries;
using RepoInMemory.Common.Queries;
using Domain;

namespace RepoInMemory.Order.Queries
{
    public class OrderFindByIdQuery : BaseFindByIdQuery<OrderEntity>, IOrderFindByIdQuery { }
}
