using BookStore.Models;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class ChatBoxesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ChatBoxes
        public async Task<ActionResult> Index()
        {
            return View(await db.ChatBoxes.ToListAsync());
        }

        public ActionResult Announcement()
        {
            var chatBox = db.ChatBoxes.ToListAsync();
            return View(chatBox);
        }

        // GET: ChatBoxes/Details/5
        public async Task<ActionResult> Announcement(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatBox chatBox = await db.ChatBoxes.FindAsync(id);
            if (chatBox == null)
            {
                return HttpNotFound();
            }
            return View(chatBox);
        }

        // GET: ChatBoxes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChatBoxes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Chat_Id,Subject,ChatPost,Posted_Date")] ChatBox chatBox)
        {
            chatBox.Posted_Date = DateTime.Now;

            {
                if (ModelState.IsValid)
                {
                    db.ChatBoxes.Add(chatBox);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return View(chatBox);
            }
        }
        // GET: ChatBoxes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatBox chatBox = await db.ChatBoxes.FindAsync(id);
            if (chatBox == null)
            {
                return HttpNotFound();
            }
            return View(chatBox);
        }

        // POST: ChatBoxes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Chat_Id,Subject,ChatPost,Posted_Date")] ChatBox chatBox)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chatBox).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(chatBox);
        }

        // GET: ChatBoxes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatBox chatBox = await db.ChatBoxes.FindAsync(id);
            if (chatBox == null)
            {
                return HttpNotFound();
            }
            return View(chatBox);
        }

        // POST: ChatBoxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ChatBox chatBox = await db.ChatBoxes.FindAsync(id);
            db.ChatBoxes.Remove(chatBox);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
