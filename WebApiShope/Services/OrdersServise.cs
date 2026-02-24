using AutoMapper;
using DTO;
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrdersServise : IOrdersServise
    {

        private readonly IOrdersReposetory _orderRepository;
        private readonly IMapper _mapper;
        private readonly IUsersReposetory _usersReposetory;
        private readonly IBasicSitesReposetory _basicSitesReposetory;
        private readonly IProductsReposetory _productsReposetory;
        private readonly IStatusesReposetory _statusesReposetory;
        private readonly ICreatePrompt _createPrompt;


        public OrdersServise(IOrdersReposetory orderRepository, IMapper mapper,
            IUsersReposetory usersReposetory, IBasicSitesReposetory basicSitesReposetory,
            IProductsReposetory productsReposetory, IStatusesReposetory statusesReposetory, ICreatePrompt createPrompt)
        {
            this._orderRepository = orderRepository;
            this._mapper = mapper;
            this._usersReposetory = usersReposetory;
            this._basicSitesReposetory = basicSitesReposetory;
            this._productsReposetory = productsReposetory;
            this._statusesReposetory = statusesReposetory;
            _createPrompt = createPrompt;
        }

        public async Task<OrderDetielsDTO> GetByIdOrderServise(long id)
        {
            Order order = await _orderRepository.GetOrderByIdReposetory(id);
            return _mapper.Map<OrderDetielsDTO>(order);
        }

        public async Task<IEnumerable<FullOrderDTO>> GetByUserIdOrderServise(long id)
        {
           IEnumerable<Order> order = await _orderRepository.getOrdersByUserId(id);
            return _mapper.Map<IEnumerable<FullOrderDTO>> (order);
        }


        public async Task<IEnumerable<FullOrderDTO>> GetAllOrders()
        {
            IEnumerable<Order> order = await _orderRepository.GetAllOrders();
            return _mapper.Map<IEnumerable<FullOrderDTO>>(order);
        }
        
        public async Task<Resulte<FullOrderDTO>> AddOrderServise(OrdersDTO dto)
        {
            User? checkIfUserInsist = await _usersReposetory.GetByIDUsersRepositories(dto.UserID);
            if (checkIfUserInsist == null)
            {
                Resulte<FullOrderDTO>.Failure("The user id not insist");
            }

            BasicSite? checkIfIBasicSitensist = await _basicSitesReposetory.GetByIDBasicSiteReposetory(dto.BasicID);
            if (checkIfIBasicSitensist == null)
            {
                Resulte<FullOrderDTO>.Failure("The basic site id not insist");
            }

            double sum = 0;
            for (int i = 0; i < dto.Products.Count(); i++)
            {

                Product? checkProductPrice = await _productsReposetory.GetByIDProductsReposetory(dto.Products[i].ProductsID);
                if (checkProductPrice == null)
                {
                    return Resulte<FullOrderDTO>.Failure("The product list is incorect");
                }
                sum += checkProductPrice.Price;
            }

            if (dto.OrderSum != sum)
            {
                return Resulte<FullOrderDTO>.Failure("The sum is incorect");
            }

             Order orderToReposetory = _mapper.Map<Order>(dto);
            orderToReposetory.StatusId = 1;
            Order orderFromReposetory = await _orderRepository.AddOrderReposetory(orderToReposetory);
            checkIfUserInsist.BasicId = null;
            await _usersReposetory.UpdateUsersRepositories(checkIfUserInsist.UserId, checkIfUserInsist);
            orderFromReposetory.OrderDate= DateOnly.FromDateTime(DateTime.Now);
            string prompt=await _createPrompt.Prompt(orderFromReposetory.OrderId);
            orderFromReposetory.FinalPrompt = prompt;
            return Resulte<FullOrderDTO>.Success(_mapper.Map<FullOrderDTO>(orderFromReposetory));
        }

        public async Task<Resulte<FullOrderDTO>> UpdateStatusServise(long id, FullOrderDTO order)
        {
            if (id != order.OrderID)
            {
                return Resulte<FullOrderDTO>.Failure("The ids are diffrent");
            }
            Order checkIfIOrderinsist = await _orderRepository.GetOrderByIdReposetory(id);
            if (checkIfIOrderinsist == null)
            {
                return Resulte<FullOrderDTO>.Failure("The order ids isn't exist");
            }

            //User? checkIfUserInsist = await _usersReposetory.GetByIDUsersRepositories(order.UserID);
            //if (checkIfUserInsist == null)
            //{
            //    Resulte<FullOrderDTO>.Failure("The user id not insist");
            //}

            //BasicSite? checkIfIBasicSitensist = await _basicSitesReposetory.GetByIDBasicSiteReposetory(order.BasicID);
            //if (checkIfIBasicSitensist == null)
            //{
            //    Resulte<FullOrderDTO>.Failure("The basic site id not insist");
            //}

            //Status? checkIfIStatusensist = await _statusesReposetory.GetStatusByID(order.StatusId);
            //if (checkIfIBasicSitensist == null)
            //{
            //    Resulte<FullOrderDTO>.Failure("The basic site id not insist");
            //}

            //double sum = 0;
            //for (int i = 0; i < order.Products.Count(); i++)
            //{

            //    Product? checkProductPrice = await _productsReposetory.GetByIDProductsReposetory(order.Products[i].ProductsID);
            //    if (checkProductPrice == null)
            //    {
            //        return Resulte<FullOrderDTO>.Failure("The product list is incorect");
            //    }
            //    sum += checkProductPrice.Price;
            //}

            //if (order.OrderSum != sum)
            //{
            //    return Resulte<FullOrderDTO>.Failure("The sum is incorect");
            //}

            Order orderToReposetory = _mapper.Map<Order>(order);
            await _orderRepository.UpdateStatusReposetory(id, orderToReposetory);
            return Resulte<FullOrderDTO>.Success(null);
        }

        public async Task<Resulte<IEnumerable<OrderItemDTO>>> GetOrderItemsServise(long orderId)
        {
            Order checkIfIOrderinsist = await _orderRepository.GetOrderByIdReposetory(orderId);
            if (checkIfIOrderinsist == null)
            {
                return Resulte<IEnumerable<OrderItemDTO>>.Failure("The order ids isn't exist");
            }

            IEnumerable<OrdersItem> orderItems = await _orderRepository.GetOrderItemsReposetory(orderId);

            return Resulte<IEnumerable<OrderItemDTO>>.Success(_mapper.Map<IEnumerable<OrderItemDTO>>(orderItems));
        }


    }

}

