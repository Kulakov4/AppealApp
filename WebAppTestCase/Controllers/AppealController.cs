using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppTestCase.Configuration;
using WebAppTestCase.Extensions;
using WebAppTestCase.Helpers;
using WebAppTestCase.Interfaces;
using WebAppTestCase.Models;
using WebAppTestCase.ViewModels;

namespace WebAppTestCase.Controllers
{
    public class AppealController : Controller
    {
        private readonly IAppealService service;

        private int pageSize = 10;   // количество элементов на странице возьмём из настроек

        public AppealController(IAppealService appealService, IOptions<AppealConfig> config)
        {
            this.service = appealService;
            if (config.Value.PageSize > 0)
                pageSize = config.Value.PageSize;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)  // список всех обращений
        {
            var skipCount = (page - 1) * pageSize;
            var count = await service.GetAll().CountAsync(); 
            var items = await service.GetPageItems(page, pageSize).ToListAsync();
            var pageViewModel = new PageViewModel(count, page, pageSize);

            var viewModel = new CollectionViewModel<Appeal> { Items = items, ItemNo = skipCount + 1, PageViewModel = pageViewModel };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Page(int appealId)
        {
            if (appealId <= 0)
                return BadRequest();

            var ourAppeal = await service.Get(appealId);

            // Заявки нет в БД
            if (ourAppeal == null)
                return RedirectToAction(nameof(Index), new { page = 1 });

            // Количество всех заявок
            var count = await service.GetAll().CountAsync();
            // Количество заявок с датой создания больше чем у нашей
            var skipItemCount = await service.GetSkipItemCount(ourAppeal.CreatedAt);
            // Вычисляем номер страницы
            var thePage = (skipItemCount / pageSize) + 1;

            return RedirectToAction(nameof(Index), new { page = thePage });
        }

        [HttpGet]
        public IActionResult Create(int page)  // Добавление нового обращения
        {
            return View(nameof(Edit), new AppealViewModel { IsEditing = false, Page = page } );
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int page)  // Редактирование обращения
        {
            if (id <= 0)
                return BadRequest();

            var appeal = await service.Get(id);

            if (appeal == null)
                return NotFound();

            var viewModel = new AppealViewModel { IsEditing = true, Page = page };
            viewModel.CopyProperties(appeal);
            viewModel.Phone = appeal.Phone.AsPhone(PhoneNumberFormat.NATIONAL);

            return View(nameof(Edit), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(AppealViewModel model)   // Сохранение обращения
        {
            if (model == null)
                return BadRequest();

            var appeal = (model.Id > 0) ? await service.Get(model.Id) : new Appeal();

            // Если в БД обращения с таким идентификатором уже нет
            if (model.Id > 0 && appeal == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return View(nameof(Edit), model);
            }

            try
            {
                // Если происходит создание новой записи
                if (model.Id == 0)
                {
                    appeal.CopyProperties(model);
                    appeal.Phone = model.Phone.AsPhone(PhoneNumberFormat.E164);
                    appeal.CreatedAt = DateTime.Now;
                    appeal = await service.Insert(appeal);
                }
                else
                {
                    // Редактировать разрешено только сообщение
                    appeal.Message = model.Message;
                }

                await service.Save();  // тут в appeal.Id записывается новый идентификатор

                if (appeal.Id == 0)
                    return BadRequest();

                return RedirectToAction(nameof(Page), new { appealId = appeal.Id });
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(AppealViewModel model)   // Удаление сообщения
        {
            if (model == null || model.Id == 0)
                return BadRequest();
            try
            {
                var result = await service.Delete(model.Id);
                if (!result)
                    return NotFound();

                await service.Save();

                return RedirectToAction(nameof(Index), new { page = model.Page });
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(AppealViewModel model)
        {
            if (model == null)
                return BadRequest();

            if (model.Page > 0)
                return RedirectToAction(nameof(Index), new { page = model.Page });
            else
                return RedirectToAction("Index", "Home");
        }
    }
}
