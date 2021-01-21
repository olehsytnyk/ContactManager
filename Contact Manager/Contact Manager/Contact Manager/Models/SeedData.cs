using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ContactManager.Data;
using System;
using System.Linq;

namespace ContactManager.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ContactManagerContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ContactManagerContext>>()))
            {
                // Look for any movies.
                if (context.Contacts.Any())
                {
                    return;   // DB has been seeded
                }

                context.Contacts.AddRange(
                    new Contact
                    {
                        Name = "Andriy",
                        DateOfBirth = DateTime.Parse("1989-2-12"),
                        Married = true,
                        Phone = "123456789",
                        Salary = 1200
                    },

                    new Contact
                    {
                        Name = "Semen",
                        DateOfBirth = DateTime.Parse("1999-3-22"),
                        Married = false,
                        Phone = "987654321",
                        Salary = 400
                    },

                    new Contact
                    {
                        Name = "Vitalii",
                        DateOfBirth = DateTime.Parse("1988-10-15"),
                        Married = true,
                        Phone = "654321789",
                        Salary = 1500
                    },

                    new Contact
                    {
                        Name = "Mykola",
                        DateOfBirth = DateTime.Parse("2000-7-30"),
                        Married = false,
                        Phone = "789065432",
                        Salary = 200
                    }
                );
                context.SaveChanges();
            }
        }
    }
}