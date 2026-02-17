using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using DTO;

namespace Services
{
    public class AutoMapper:Profile
    {

        public AutoMapper() {


            CreateMap<User, UserDTO>();

           

            CreateMap<RegisterUserDTO, User>()
                .ForMember(
                dest=>dest.Password,
                opts=>opts.MapFrom(src=>src.UserPassword));

            CreateMap<LoginUserDTO, User>().ForMember(
                dest => dest.Password,
                opts => opts.MapFrom(src => src.UserPassward));

            CreateMap<CategoryToUpdateDTO, Category>();

            CreateMap<MainCategory, MainCategoriesDTO>().ReverseMap();

            CreateMap<GeminiPrompt, GeminiPromptDTO>().ReverseMap();

            CreateMap<ManegerMainCategoryDTO, MainCategory>();

            CreateMap<UpdateUserDTO, User>();

            CreateMap<Category, CategoryDTO>().ReverseMap();

            CreateMap<AddCategoryDTO, Category>()
                
            .ForMember(dest=>dest.ImgUrl,
            opts=>opts.MapFrom(src=>src.ImgUrl.FileName));

            CreateMap<Platform, PlatformsDTO>().ReverseMap();

            CreateMap<AddPlatformDTO, Platform>();

            CreateMap<Product, ProductDTO>()
            .ForMember(
            dest => dest.CategoryName,
            opts => opts.MapFrom(src => src.Category.CategoryName))
             .ForMember(
            dest => dest.ImgUrl,
            opts => opts.MapFrom(src => src.Category.ImgUrl));

            CreateMap<ProductDTO, Product>();

            CreateMap<AddProductDTO, Product>();

            CreateMap<UpdateProductDTO, Product>()
           .ForMember(
            dest => dest.ProductsId,
            opts => opts.MapFrom(src => src.ProductID));

            CreateMap<BasicSite, BasicSiteDTO>();


            CreateMap<AddBasicSiteDTO, BasicSite>();


            CreateMap<UpdateBasicSiteDTO, BasicSite>();

            CreateMap<Order, OrdersDTO>().ReverseMap();

            CreateMap<FullOrderDTO, Order>().ReverseMap();

            CreateMap<Order, OrderItemDTO>();

            CreateMap<AddReviewDTO, Review>();

            CreateMap<Review, ReviewDTO>().ReverseMap();


            CreateMap<SiteType, SiteTypeDTO>().ReverseMap();

            CreateMap<CartItem, CartItemDTO>().ReverseMap();

            CreateMap<AddToCartDTO, CartItem>();

            


        }
   
    }
}
