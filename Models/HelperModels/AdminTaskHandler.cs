using System;
using System.CodeDom;
using System.Data.Common;
using eventsBook.Controllers;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace eventsBook.Models.HelperModels;

public class AdminTaskHandler
{
    IWebHostEnvironment env;
    AppDbContext db;
    ILogger<AdminController> logger;
    public AdminTaskHandler(AppDbContext db, IWebHostEnvironment env, ILogger<AdminController> logger)
    {
        this.env = env;
        this.logger = logger;
        this.db = db;
    }
    public bool SaveImage(IFormFile file, out string imgName)
    {
        string[] allowedExtensions = [".jpg", ".jpeg", ".png"];
        string extension = Path.GetExtension(file.FileName).ToLower();
        imgName = "IMG" + DateTime.Now.Ticks + extension;
        if (!allowedExtensions.Contains(extension))//this may include . if not okay else I will ad the . in allowed extension
            return false;

        string imagesFolder = Path.Combine(env.WebRootPath, "images\\" + imgName);
        FileStream image = new(imagesFolder, FileMode.Create);
        try
        {
            file.CopyTo(image);
            image.Close();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return false;
        }
    }
    public bool verifyInput(EventInput input, bool AllowNullImages = false)
    {
        var properties = input.GetType().GetProperties();
        foreach (var prop in properties)
        {
            var value = prop.GetValue(input);
            if (prop.Name == "Category")
                continue;
            if (AllowNullImages && prop.Name == "ImageFile")
                continue;
            if (value is string)
            {
                if (string.IsNullOrEmpty(value as string) || string.IsNullOrWhiteSpace(value as string))
                    return false;
            }
            else if (value == null)
                return false;
        }
        return true;
    }
    public bool createEvent(EventInput input)
    {

        if (!verifyInput(input))
            return false;
        List<bool> uploaded = new();
        List<Images> imgsNames = new();
        input.ImageFile.ForEach(img =>
        {
            uploaded.Add(SaveImage(img, out string imgName));
            imgsNames.Add(new()
            {
                Url = "/images/" + imgName
            });
        });
        if (uploaded.TrueForAll(e => e == true))
        {
            Events ev = new()
            {
                CategoryId = input.CategoryId,
                Date = input.EventDate,
                Name = input.Name,
                Description = input.Description,
                Price = input.Price,
                Venue = input.Venue,
                Images = new(),
            };
            imgsNames.ForEach(img =>
            {
                ev.Images.Add(img);
            });
            db.Events.Add(ev);
            db.SaveChanges();
            return true;
        }
        return false;
    }
    public bool removeImages(string path)
    {
        //path should be like this /images/IMG123234234.png
        if (string.IsNullOrEmpty(path))
            return false;
        try
        {
            File.Delete(Path.Combine(env.WebRootPath, path));
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return false;
        }
    }

    public bool Edit(EventInput input)
    {
        if (!verifyInput(input, true))
            return false;
        List<bool> uploaded = new();
        List<Images> imgsNames = new();
        Events ev = db.Events.Include(e => e.Images).FirstOrDefault(eve => eve.Id == input.Id)!;
        if (input.ImageFile != null)
        {
            ev.Images.ForEach(img =>
            {
                if (removeImages(img.Url))
                    ev.Images.Remove(img);
            });
            db.Events.Update(ev);
            db.SaveChanges();
            input.ImageFile.ForEach(img =>
            {
                uploaded.Add(SaveImage(img, out string imgName));
                imgsNames.Add(new()
                {
                    Url = "/images/" + imgName
                });
            });
        }
        if (uploaded.TrueForAll(e => e == true) || input.ImageFile.Count == 0)
        {
            ev.CategoryId = input.CategoryId;
            ev.Date = input.EventDate;
            ev.Name = input.Name;
            ev.Description = input.Description;
            ev.Price = input.Price;
            ev.Venue = input.Venue;
            if (input.ImageFile != null)
            {
                ev.Images = new();
                imgsNames.ForEach(img =>
                {
                    ev.Images.Add(img);
                });
            }
            db.Events.Update(ev);
            db.SaveChanges();
            return true;
        }
        return false;
    }

}
