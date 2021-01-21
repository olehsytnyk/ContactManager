using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactManager.Data;
using ContactManager.Models;
using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Web;
using Grpc.Core;

namespace ContactManager.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ContactManagerContext _context;

        public ContactsController(ContactManagerContext context)
        {
            _context = context;
        }

        /*[HttpPost]
        public ActionResult Index(HttpPostedFileBase FileUpload)
        {

            DataTable dt = new DataTable();


            if (FileUpload.ContentLength > 0)
            {

                string fileName = Path.GetFileName(FileUpload.FileName);
                string path = Path.Combine(Server.MapPath("~/uploads"),fileName);


                try
                {
                    FileUpload.SaveAs(path);

                    dt = ProcessCSV(path);


                    ViewData["Feedback"] = ProcessBulkCopy(dt);
                }
                catch (Exception ex)
                {

                    ViewData["Feedback"] = ex.Message;
                }
            }
            else
            {

                ViewData["Feedback"] = "Please select a file";
            }


            dt.Dispose();

            return View("Index", ViewData["Feedback"]);
        }

        private static DataTable ProcessCSV(string fileName)
        {

            string Feedback = string.Empty;
            string line = string.Empty;
            string[] strArray;
            DataTable dt = new DataTable();
            DataRow row;


            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");


            StreamReader sr = new StreamReader(fileName);


            line = sr.ReadLine();
            strArray = r.Split(line);


            Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn()));



            while ((line = sr.ReadLine()) != null)
            {
                row = dt.NewRow();


                row.ItemArray = r.Split(line);
                dt.Rows.Add(row);
            }


            sr.Dispose();


            return dt;


        }


        private static String ProcessBulkCopy(DataTable dt)
        {
            string Feedback = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["ContactManagerContext"].ConnectionString;


            using (SqlConnection conn = new SqlConnection(connString))
            {

                using (var copy = new SqlBulkCopy(conn))
                {


                    conn.Open();


                    copy.DestinationTableName = "ImportDetails";
                    copy.BatchSize = dt.Rows.Count;
                    try
                    {

                        copy.WriteToServer(dt);
                        Feedback = "Upload complete";
                    }
                    catch (Exception ex)
                    {
                        Feedback = ex.Message;
                    }
                }
            }

            return Feedback;
        }*/

        // GET: Contacts
        public async Task<IActionResult> Index(string searchString)
        {

            var title = from m in _context.Contacts
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                title = title.Where(s => s.Name.Contains(searchString)
                                    || s.Phone.Contains(searchString)
                                    || s.Salary.Equals(searchString));
            }

            return View(await title.ToListAsync());
        }


        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DateOfBirth,Married,Phone,Salary")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,Married,Phone,Salary")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
