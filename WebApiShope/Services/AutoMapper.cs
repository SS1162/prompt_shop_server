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
    public class AutoMapper : Profile
    {

        public AutoMapper()
        {


            CreateMap<User, UserDTO>();



            CreateMap<RegisterUserDTO, User>()
                .ForMember(
                dest => dest.Password,
                opts => opts.MapFrom(src => src.UserPassword));

            CreateMap<LoginUserDTO, User>().ForMember(
                dest => dest.Password,
                opts => opts.MapFrom(src => src.UserPassward));

            CreateMap<CategoryToUpdateDTO, Category>();

            CreateMap<MainCategory, MainCategoriesDTO>().ReverseMap();

            CreateMap<GeminiPrompt, GeminiPromptDTO>().ReverseMap();

            CreateMap<ManegerMainCategoryDTO, MainCategory>();

            CreateMap<UpdateUserDTO, User>();

            CreateMap<Category, CategoryDTO>().ReverseMap();

            CreateMap<OrdersItem, AddToCartDTO>()
            .ForMember(dest => dest.UserID, opts => opts.MapFrom(src => src.Order != null ? src.Order.UserId : 0))
            .ForMember(dest => dest.ProductsID, opts => opts.MapFrom(src => src.ProductsId))
            .ForMember(dest => dest.PlatformsID, opts => opts.MapFrom(src => src.BasicSitesPlatforms))
            .ForMember(dest => dest.UserDescription, opts => opts.MapFrom(src => src.UserDescription))
            .ForMember(dest => dest.ProductName, opts => opts.MapFrom(src => src.Products != null ? src.Products.ProductsName : string.Empty))
            .ForMember(dest => dest.Price, opts => opts.MapFrom(src => src.Products != null ? src.Products.Price : 0));

            CreateMap<Order, OrderDetielsDTO>()
            .ForMember(dest => dest.OrderID, opts => opts.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.UserID, opts => opts.MapFrom(src => src.UserId))
            .ForMember(dest => dest.ReviewId, opts => opts.MapFrom(src => src.ReviewId))
            .ForMember(dest => dest.ReviewImg, opts => opts.MapFrom(src => src.Review != null ? src.Review.ReviewImg : string.Empty))
            .ForMember(dest => dest.Stars, opts => opts.MapFrom(src => src.Review != null ? src.Review.Stars : 0))
            .ForMember(dest => dest.ReviewNote, opts => opts.MapFrom(src => src.Review != null ? src.Review.ReviewText : string.Empty))
            .ForMember(dest => dest.SiteName, opts => opts.MapFrom(src => src.Basic.SiteName))
            .ForMember(dest => dest.SiteTypeName, opts => opts.MapFrom(src => src.Basic.SiteType != null ? src.Basic.SiteType.SiteTypeName : string.Empty))
            .ForMember(dest => dest.SiteTypeDescreption, opts => opts.MapFrom(src => src.Basic.SiteType != null ? src.Basic.SiteType.SiteTypeDescreption : string.Empty))
            .ForMember(dest => dest.Prompt, opts => opts.MapFrom(src => src.FinalPrompt))
            .ForMember(dest => dest.Products, opts => opts.MapFrom(src => src.OrdersItems));


            CreateMap<AddCategoryDTO, Category>()

            .ForMember(dest => dest.ImgUrl,
            opts => opts.MapFrom(src => src.ImgUrl.FileName));

            CreateMap<Platform, PlatformsDTO>().ReverseMap();

            CreateMap<AddPlatformDTO, Platform>();

            CreateMap<Product, ProductDTO>()
            .ForMember(
            dest => dest.CategoryName,
            opts => opts.MapFrom(src => src.Category != null ? src.Category.CategoryName : string.Empty))
             .ForMember(
            dest => dest.ImgUrl,
            opts => opts.MapFrom(src => src.Category != null ? src.Category.ImgUrl : string.Empty));

            CreateMap<ProductDTO, Product>();

            CreateMap<AddProductDTO, Product>();

            CreateMap<UpdateProductDTO, Product>()
           .ForMember(
            dest => dest.ProductsId,
            opts => opts.MapFrom(src => src.ProductID));

            CreateMap<BasicSite, BasicSiteDTO>()
            .ForMember(
            dest => dest.PlatformID,
            opts => opts.MapFrom(src => src.BasicSitesPlatforms))
            .ForMember(
            dest => dest.PlatformName,
            opts => opts.MapFrom(src => src.BasicSitesPlatformsNavigation.PlatformName))
            .ForMember(
            dest => dest.SiteTypeName,
            opts => opts.MapFrom(src => src.SiteType.SiteTypeName))
            .ForMember(
            dest => dest.SiteTypeDescreption,
            opts => opts.MapFrom(src => src.SiteType.SiteTypeDescreption))
            .ForMember(
            dest => dest.UserDescreption,
            opts => opts.MapFrom(src => src.UserDescriptionNavigation != null ? src.UserDescriptionNavigation.Prompt : null))
            .ForMember(
            dest => dest.GeminiPromptId,
            opts => opts.MapFrom(src => src.UserDescription));


            CreateMap<AddBasicSiteDTO, BasicSite>()
             .ForMember(
            dest => dest.BasicSitesPlatforms,
            opts => opts.MapFrom(src => src.PlatformID));


            CreateMap<UpdateBasicSiteDTO, BasicSite>()
            .ForMember(
            dest => dest.BasicSiteId,
            opts => opts.MapFrom(src => src.BasicSiteID))
            .ForMember(
            dest => dest.BasicSitesPlatforms,
            opts => opts.MapFrom(src => src.PlatformID))
            .ForMember(
            dest => dest.UserDescription,
            opts => opts.MapFrom(src => src.UserDescreption));

            CreateMap<Order, OrdersDTO>();

            CreateMap<OrdersDTO, Order>()
            .ForMember(dest => dest.OrdersItems, opts => opts.MapFrom(src => src.Products))
            .ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.UserID))
            .ForMember(dest => dest.BasicId, opts => opts.MapFrom(src => src.BasicID));

            CreateMap<AddToCartDTO, OrdersItem>()
            .ForMember(dest => dest.ProductsId, opts => opts.MapFrom(src => src.ProductsID))
            .ForMember(dest => dest.BasicSitesPlatforms, opts => opts.MapFrom(src => src.PlatformsID))
            .ForMember(dest => dest.UserDescription, opts => opts.MapFrom(src => src.UserDescription));

            CreateMap<Order, FullOrderDTO>()
            .ForMember(dest => dest.SiteName, opts => opts.MapFrom(src => src.Basic.SiteName))
            .ForMember(dest => dest.ProductCount, opts => opts.MapFrom(src => src.OrdersItems.Count))
            .ForMember(dest => dest.StatusName, opts => opts.MapFrom(src => src.Status.StatusName))
            .ForMember(dest => dest.ReviewId, opts => opts.MapFrom(src => src.ReviewId));

            CreateMap<FullOrderDTO, Order>();

            CreateMap<Order, OrderItemDTO>();

            CreateMap<AddReviewDTO, Review>()
            .ForMember(
            dest => dest.Stars,
            opts => opts.MapFrom(src => src.Score))
            .ForMember(
            dest => dest.ReviewText,
            opts => opts.MapFrom(src => src.Note))
            .ForMember(
            dest => dest.ReviewImg,
            opts => opts.MapFrom(src => src.ReviewImg != null ? src.ReviewImg.FileName : null));

            CreateMap<Review, ReviewDTO>().ReverseMap();


            CreateMap<CartItemDTO, CartItem>();
            CreateMap<SiteType, SiteTypeDTO>().ReverseMap();

            CreateMap<CartItem, CartItemDTO>()
                .ForMember(
                    dest => dest.CartID,
                    opts => opts.MapFrom(src => src.CartId))
                .ForMember(
                    dest => dest.ProductsName,
                    opts => opts.MapFrom(src => src.Products.ProductsName))
                .ForMember(
                    dest => dest.Price,
                    opts => opts.MapFrom(src => src.Products.Price))
                .ForMember(
                    dest => dest.CategoryName,
                    opts => opts.MapFrom(src => src.Products.Category.CategoryName))
                .ForMember(
                    dest => dest.ImgUrl,
                    opts => opts.MapFrom(src => src.Products.Category.ImgUrl))
                .ForMember(
                    dest => dest.CategoryDescreption,
                    opts => opts.MapFrom(src => src.Products.Category.CategoryDescreption))
                .ForMember(
                    dest => dest.PlatformName,
                    opts => opts.MapFrom(src => src.BasicSitesPlatformsNavigation.PlatformName))
                .ForMember(
                    dest => dest.ProductID,
                    opts => opts.MapFrom(src => src.ProductsId))
                .ForMember(
                    dest => dest.UserDescreptionID,
                    opts => opts.MapFrom(src => src.UserDescription))
                .ForMember(
                    dest => dest.UserDescreption,
                    opts => opts.MapFrom(src => src.UserDescriptionNavigation != null ? src.UserDescriptionNavigation.Prompt : null))
                .ForMember(
                    dest => dest.PlatformID,
                    opts => opts.MapFrom(src => src.BasicSitesPlatforms));

            CreateMap<AddToCartDTO, CartItem>()
            .ForMember(
            dest => dest.BasicSitesPlatforms,
            opts => opts.MapFrom(src => src.PlatformsID))
            
            .ForMember(
            dest => dest.UserId,
            opts => opts.MapFrom(src => src.UserID))
            .ForMember(
            dest => dest.ProductsId,
            opts => opts.MapFrom(src => src.ProductsID));




        }

    }
}
