using ContactManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.Data
{
    public class ContactManagerContext : DbContext
    {
        public ContactManagerContext (DbContextOptions<ContactManagerContext> options) : base(options)
        { }

        public DbSet<Contact> Contacts { get; set; }
    }
}
