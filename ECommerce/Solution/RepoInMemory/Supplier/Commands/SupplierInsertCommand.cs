﻿using Application.Supplier.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Supplier.Commands
{
    public class SupplierInsertCommand : BaseInsertCommand<SupplierEntity>, ISupplierInsertCommand
    {
    }
}
