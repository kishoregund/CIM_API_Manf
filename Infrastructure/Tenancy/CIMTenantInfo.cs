﻿using Finbuckle.MultiTenant.Abstractions;

namespace Infrastructure.Tenancy
{
    public class CIMTenantInfo : ITenantInfo
    {
        public string Id { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string AdminEmail { get; set; }
        public DateTime ValidUpTo { get; set; }
        public bool IsActive { get; set; }
        public bool SubscribedBy { get; set; }        
    }
}
