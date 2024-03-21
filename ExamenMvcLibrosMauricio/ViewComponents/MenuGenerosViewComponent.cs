﻿using ExamenMvcLibrosMauricio.Models;
using ExamenMvcLibrosMauricio.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExamenMvcLibrosMauricio.ViewComponents
{
    [ViewComponent(Name = "MenuGeneros")]
    public class MenuGenerosViewComponent:ViewComponent
    {
        private RepositoryLibros repo;

        public MenuGenerosViewComponent(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = await this.repo.GetAllGenerosAsync();
            return View(generos);
        }
    }
}
