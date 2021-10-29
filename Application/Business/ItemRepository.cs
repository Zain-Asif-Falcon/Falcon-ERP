using Application.Business.IAppServices;
using Application.Interface.Repositories;
using Domain.Contracts.V1;
using Domain.Entities;
using Domain.ViewModel;
using Domain.ViewModel.API;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Application.Business
{
    public class ItemRepository : Repository<Item>, IItemsRepository
    {
        private readonly ERPDBContext db;

        public ItemRepository(ERPDBContext _db, ILogger logger) : base(_db, logger)
        {
            this.db = _db;
        }
        public new async Task<bool> Update(Item itm)
        {
            try
            {
                bool updateStatus = false;
                var objFromDb = await db.Items.FirstOrDefaultAsync(c => c.ItemsId == itm.ItemsId);
                objFromDb.Code = itm.Code;
                objFromDb.Description = itm.Description;
                objFromDb.Price = itm.Price;
                objFromDb.PurchasePrice = itm.PurchasePrice;
                objFromDb.Weight = itm.Weight;
               // objFromDb.IsActive = itm.IsActive;
                objFromDb.updated_at = DateTime.Now;
                //db.Update(itm);
                var create = await db.SaveChangesAsync();
                if (create > 0)
                {
                    updateStatus = true;
                }
                _logger.LogInformation("Log message in the {Repo} method for update", typeof(ItemRepository));
                return updateStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} function error for update", typeof(ItemRepository));
                return false;
            }            
        }
        
        public async Task<IEnumerable<SelectListItem>> GetListItemsForDropDown()
        {
            return await db.Items.Where(x => x.IsActive == true).Select(x => new SelectListItem()
            {
                Text = x.Code + "-" + x.Description,
                Value = x.ItemsId.ToString()
            }).ToListAsync();
        }
        //public async Task<Item> GetItemInfo(int ItemId)
        //{
        //    Item itm = new Item();
        //    itm =await db.Items.Where(a => a.ItemsId == ItemId && a.IsActive == true).FirstOrDefaultAsync(); ;
        //    return itm;
        //}

        //public async Task<bool> Create(Item item)
        //{
        //    bool updateStatus = false;
        //    Item itm = new Item();
        //    itm.Code = item.Code;
        //    itm.Description = item.Description;
        //    itm.Price = item.Price;
        //    itm.Weight = itm.Weight;
        //    itm.IsActive = true;
        //    itm.created_at = DateTime.Now;
        //    Add(itm);
        //    var create = await db.SaveChangesAsync();
        //    if (create > 0)
        //    {
        //        updateStatus = true;
        //    }
        //    return updateStatus;
        //}
        public async Task<bool> SetRecordAsDeleted(int Id)
        {
            int IsDeleted = 0;
            try
            {
                var objFromDb = await db.Items.FirstOrDefaultAsync(c => c.ItemsId == Id);
                if (objFromDb != null)
                {
                    objFromDb.deleted_at = DateTime.Now;
                    objFromDb.IsActive = false;
                    IsDeleted = db.SaveChanges();
                }
                _logger.LogInformation("Log message in the {Repo} method", typeof(ItemRepository));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete function error", typeof(ItemRepository));
                return false;
            }
            
            return (IsDeleted > 0) ? true : false;
        }

        public async Task<ItemsViewModel> GetItemInfo(int id)
        {
            var itemInfo = await db.Items.Select(a => new ItemsViewModel { stock = a.ItemStocks.Where(b => b.ItemId == a.ItemsId).Select(j => j.Qty).FirstOrDefault(), Itms = new Item { Code = a.Code, created_at = a.created_at, deleted_at = a.deleted_at, Description = a.Description, IsActive = a.IsActive, Weight = a.Weight, Price = a.Price, PurchasePrice = a.PurchasePrice, IsDeletable = a.IsDeletable, ItemsId = a.ItemsId, updated_at = a.updated_at } }).Where(a => a.Itms.ItemsId == id).FirstOrDefaultAsync();
            return itemInfo;
        }
        public async Task<bool> GetExixtingCode(string Code)
        {
            bool exist = false;
            var itemInfo = await db.Items.Where(a => a.Code == Code).FirstOrDefaultAsync();
            if(itemInfo == null)
            {
                exist = false;
            }  
            else
            {
                exist = true;
            }
            return exist;
        }        
    }
}
