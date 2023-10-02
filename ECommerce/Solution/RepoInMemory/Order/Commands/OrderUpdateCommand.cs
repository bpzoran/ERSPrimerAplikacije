﻿using Application.Order.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Order.Commands
{
    public class OrderUpdateCommand : BaseUpdateCommand<OrderEntity>, IOrderUpdateCommand { }
}
