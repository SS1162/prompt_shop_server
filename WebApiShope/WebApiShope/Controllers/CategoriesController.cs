using Microsoft.AspNetCore.Mvc;
using Services;
using DTO;
using Microsoft.AspNetCore.Http.HttpResults;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesServise _categoriesServise;
        public CategoriesController(ICategoriesServise categoriesServise)
        {
            this._categoriesServise = categoriesServise;
        }
        // GET: api/<CategoryController>
        [HttpGet]
        async public Task<ActionResult<ResponePage<CategoryDTO>>> GetCategoriesByMainCategoryID(int numberOfPages, long mainCategoryID, int pageSize, string? search)
        {
            Resulte<ResponePage<CategoryDTO>> respone = await _categoriesServise.GetCategoriesServise(numberOfPages, mainCategoryID, pageSize, search);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }
            if (!respone.Data.Data.Any())
            {
                return NoContent();
            }

            return Ok(respone.Data);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        async public Task<ActionResult<CategoryDTO>> GetCategoryByCategoryID(long id)
        {
            CategoryDTO category = await _categoriesServise.GetByIDCategoriesServise(id);
            if (category == null)
            {
                return NoContent();
            }
            return Ok(category);
        }

        // POST api/<CategoryController>
        [HttpPost("admin")]
        async public Task<ActionResult<CategoryDTO>> AddCategory([FromForm] AddCategoryDTO category)
        {
            Resulte<CategoryDTO> categoryConstructedObject = await _categoriesServise.AddCategoriesServise(category);
            if (!categoryConstructedObject.IsSuccess)
            {
                return BadRequest(categoryConstructedObject.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetCategoryByCategoryID), new { id = categoryConstructedObject.Data.CategoryID }, categoryConstructedObject.Data);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("admin/{id}")]
        async public Task<ActionResult> UpdateCategory(long id, [FromForm] CategoryToUpdateDTO category)
        {
            Resulte<CategoryDTO> respone = await _categoriesServise.UpdateCategoriesServise(id, category);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }
            return Ok();
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("admin/{id}")]
        async public Task<ActionResult> DeleteCategoty(long id)
        {
            Resulte<CategoryDTO> respone = await _categoriesServise.DeleteIDCategoriesServise(id);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }
            return Ok();
        }




    }
}

//import { HttpClient, HttpParams } from '@angular/common/http';
//import { inject, Injectable } from '@angular/core';
//import { environment } from '../../../environments/environment';
//import { Observable, tap } from 'rxjs';
//import { CategoryModel, ResponePageModel } from '../../Models/CategoryModel';

//@Injectable({
//  providedIn: 'root',
//})
//export class CategoryServise {
//  private http = inject(HttpClient);
//  private readonly BASIC_URL: string = `${environment.apiUrl}/Categories`;

//  // GET: api/Categories (קבלת דף קטגוריות לפי קטגוריה ראשית)
//  getCategoriesByMainID(
//    numberOfPages: number,
//    mainCategoryID: number,
//    pageSize: number,
//    search?: string
//  ): Observable<ResponePageModel<CategoryModel>> {
//    let params = new HttpParams()
//      .set('numberOfPages', numberOfPages)
//      .set('mainCategoryID', mainCategoryID)
//      .set('pageSize', pageSize);

//    if (search) params = params.set('search', search);

//    return this.http.get<ResponePageModel<CategoryModel>>(this.BASIC_URL, { params });
//  }

//  // GET: api/Categories/{id} (לפי מזהה ספציפי)
//  getCategoryById(id: number): Observable<CategoryModel> {
//    return this.http.get<CategoryModel>(`${this.BASIC_URL}/${id}`);
//  }

//  // POST: api/Categories/admin (הוספה עם FormData בגלל ה-[FromForm])
//  addCategory(formData: FormData): Observable<CategoryModel> {
//    return this.http.post<CategoryModel>(`${this.BASIC_URL}/admin`, formData);
//  }

//  // PUT: api/Categories/admin/{id} (עדכון עם FormData)
//  updateCategory(id: number, formData: FormData): Observable<void> {
//    return this.http.put<void>(`${this.BASIC_URL}/admin/${id}`, formData);
//  }

//  // DELETE: api/Categories/admin/{id}
//  deleteCategory(id: number): Observable<void> {
//    return this.http.delete<void>(`${this.BASIC_URL}/admin/${id}`);
//  }
//}