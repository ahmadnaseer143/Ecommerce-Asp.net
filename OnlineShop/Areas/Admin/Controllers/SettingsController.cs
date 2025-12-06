using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models.Db;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingsController : Controller
    {
        private readonly OnlineShopContext _context;

        public SettingsController(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: Admin/Settings/Edit/5
        // GET: Admin/Settings/Edit
        public async Task<IActionResult> Edit()
        {
            // Change FirstAsync() to FirstOrDefaultAsync()
            var setting = await _context.Settings.FirstOrDefaultAsync();

            if (setting == null)
            {
                // If no settings exist, create a default one, save it, and then return it to the view.
                setting = new OnlineShop.Models.Db.Setting()
                {
                    // Initialize with default/placeholder values
                    Title = "Default Shop Title",
                    Shipping = 0,
                    CopyRight = "© 2025 All Rights Reserved.",
                    Logo = "default.png" // Use a placeholder image name
                };

                _context.Settings.Add(setting);
                await _context.SaveChangesAsync();

                TempData["message"] = "Initial settings record created. Please complete the fields.";
            }

            // Always return the setting (either existing or newly created) to the view
            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
    [Bind("Id,Shipping,Title,Address,Email, Phone,CopyRight,Instagram,Facebook,GooglePlus,Youtube,Twitter,Logo")]
       Setting setting, IFormFile? newLogo)
        {
            if (id != setting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (newLogo != null)
                    {

                        string d = Directory.GetCurrentDirectory();
                        string path = d + "\\wwwroot\\images\\" + setting.Logo;
                        //------------------------------------------------
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        //------------------------------------------------
                        setting.Logo = Guid.NewGuid() + Path.GetExtension(newLogo.FileName);
                        path = d + "\\wwwroot\\images\\" + setting.Logo;
                        //------------------------------------------------

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            newLogo.CopyTo(stream);
                        }
                    }

                    _context.Update(setting);
                    await _context.SaveChangesAsync();

                    TempData["message"] = "Setting saved";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettingExists(setting.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return Redirect($"/admin/Settings/Edit");
        }


        private bool SettingExists(int id)
        {
            return _context.Settings.Any(e => e.Id == id);
        }
    }
}
