using BookStore.DTOs.orderDTO;
using BookStore.Models;
using BookStore.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        UnitWork unit;
        UserManager<IdentityUser> userManager;
        public OrderController(UnitWork unit, UserManager<IdentityUser> userManager)
        {
            this.unit = unit;
            this.userManager = userManager;
        }


        [HttpPost]
        public IActionResult createOrder(AddOrderDTO orderDTO)
        {
            Order order = new Order()
            {
                customer_id = orderDTO.customer_id,
                orderDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                status = "create"

            };
            decimal totalPrice = 0;
            foreach (var item in orderDTO.books)
            {
                Book book = unit.Generic_BookRepo.selectById(item.Book_Id);
                totalPrice = totalPrice + (book.Price * item.Quantity);
                OrderDetails details = new OrderDetails()
                {
                    order_id = order.Id,
                    book_id = book.Id,
                    quantity = item.Quantity,
                    unitPrice = book.Price,
                };
                if (book.stock > 0 && (book.stock - item.Quantity >= 0))
                {
                    order.orderDetails.Add(details);
                    book.stock -= item.Quantity;
                    unit.Generic_BookRepo.update(book);
                }
                else
                {
                    return BadRequest("Invalid quantity");
                }
            }

            order.totalPrice = totalPrice;
            unit.Generic_OrderRepo.add(order);
            unit.Generic_OrderRepo.save();

            return Ok();



        }

        [HttpGet]
        [Authorize(Roles = "admin")]

        public IActionResult getAllOrders()
        {
            var orders = unit.Generic_OrderRepo.selectAll();

            if (!orders.Any()) { return NotFound(); }
            else
            {
                List<SelectOrderDTO> listOrders = new List<SelectOrderDTO>();
                foreach (var order in orders)
                {
                    SelectOrderDTO selectOrderDTO = new SelectOrderDTO()
                    {
                        OrderStatus = order.status,
                        OrderId = order.Id,
                        CustomerId = order.customer_id,
                        totalPrice = order.totalPrice,
                    };
                    List<EditOrderDetailsDTO> orderDetailsDTOs = new List<EditOrderDetailsDTO>();
                    foreach (var item in order.orderDetails)
                    {
                        EditOrderDetailsDTO orderDetailsDTO = new EditOrderDetailsDTO()
                        {
                            Book_Id = item.book_id,
                            Quantity = item.quantity,
                            unitPrice = item.unitPrice
                        };
                        orderDetailsDTOs.Add(orderDetailsDTO);
                    }
                    selectOrderDTO.OrderDetails = orderDetailsDTOs;
                    listOrders.Add(selectOrderDTO);

                }
                return Ok(listOrders);
            }


        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]

        public IActionResult getOrderById(int id)
        {
            Order order = unit.Generic_OrderRepo.selectById(id);

            if (order == null) { return NotFound(); }
            else
            {
                SelectOrderDTO selectOrderDTO = new SelectOrderDTO()
                {
                    OrderStatus = order.status,
                    OrderId = order.Id,
                    CustomerId = order.customer_id,
                    totalPrice = order.totalPrice,
                };
                List<EditOrderDetailsDTO> orderDetailsDTOs = new List<EditOrderDetailsDTO>();
                foreach (var item in order.orderDetails)
                {
                    EditOrderDetailsDTO orderDetailsDTO = new EditOrderDetailsDTO()
                    {
                        Book_Id = item.book_id,
                        Quantity = item.quantity,
                        unitPrice = item.unitPrice
                    };
                    orderDetailsDTOs.Add(orderDetailsDTO);
                }
                selectOrderDTO.OrderDetails = orderDetailsDTOs;
                return Ok(selectOrderDTO);
            }


        }
        [HttpGet]
        public IActionResult getOrderByUser()
        {
            var user = User.Identity.Name;
            var _user = userManager.FindByNameAsync(user).Result;
                

            if (_user == null) { return NotFound(); }
            else
            {
                Order order = unit.Generic_OrderRepo.selectAll().Where(o => o.customer_id == _user.Id).FirstOrDefault();

                SelectOrderDTO selectOrderDTO = new SelectOrderDTO()
                {
                    OrderStatus = order.status,
                    OrderId = order.Id,
                    CustomerId = order.customer_id,
                    totalPrice = order.totalPrice,
                };
                List<EditOrderDetailsDTO> orderDetailsDTOs = new List<EditOrderDetailsDTO>();
                foreach (var item in order.orderDetails)
                {
                    EditOrderDetailsDTO orderDetailsDTO = new EditOrderDetailsDTO()
                    {
                        Book_Id = item.book_id,
                        Quantity = item.quantity,
                        unitPrice = item.unitPrice
                    };
                    orderDetailsDTOs.Add(orderDetailsDTO);
                }
                selectOrderDTO.OrderDetails = orderDetailsDTOs;
                return Ok(selectOrderDTO);
            }


        }

        //[HttpPut]
        ////[Authorize(Roles = "admin")]

        //public IActionResult editOrder(EditOrderDTO editOrderDTO) 
        //{
        //    if(ModelState.IsValid) 
        //    {
        //        Order order = unit.Generic_OrderRepo.selectById(editOrderDTO.OrderId);
        //        if (order == null) { return NotFound(); }
        //        else
        //        {

        //            order.orderDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //            order.status = editOrderDTO.OrderStatus;
        //            order.customer_id = editOrderDTO.CustomerId;


        //            List<OrderDetails> orderDetails = new List<OrderDetails>();
        //            decimal totalPrice = 0;
        //            foreach (var item in editOrderDTO.OrderDetails)
        //            {
        //                Book book = unit.Generic_BookRepo.selectById(item.Book_Id);
        //                totalPrice = totalPrice + (book.Price * item.Quantity);
        //                OrderDetails newOrderDetails = new OrderDetails()
        //                {
        //                    book_id = item.Book_Id,
        //                    order_id = editOrderDTO.OrderId,
        //                    quantity = item.Quantity,
        //                    unitPrice = item.unitPrice
        //                };
        //                orderDetails.Add(newOrderDetails);
        //                if (book.stock > 0 && (book.stock - item.Quantity >= 0))
        //                {

        //                    order.totalPrice = totalPrice;
        //                    book.stock -= item.Quantity;
        //                    unit.Generic_BookRepo.update(book);
        //                    order.orderDetails = orderDetails;

        //                }
        //                else
        //                {
        //                    return BadRequest("Invalid quantity");
        //                }

        //            }
        //            unit.Generic_OrderRepo.update(order);
        //            unit.Save();
        //            return Ok();
        //        }
        //    }
        //    else
        //    { 
        //        return BadRequest(ModelState);
        //    }  

        //}

        [HttpPut]
        [Authorize(Roles = "admin")]

        public IActionResult editOrder(EditOrderDTO editOrderDTO)
        {
            //bool flagAddOrOverwrite = editOrderDTO.flagAddOrOverwrite; // false : add , true; overwrite
            if (ModelState.IsValid)
            {
                Order order = unit.Generic_OrderRepo.selectById(editOrderDTO.OrderId);
                if (order == null)
                {
                    return NotFound();
                }
                else
                {
                    order.orderDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    order.status = editOrderDTO.OrderStatus;
                    order.customer_id = editOrderDTO.CustomerId;
                    if (editOrderDTO.flagAddOrOverwrite) {

                        order.orderDetails.Clear();
                    }
                    decimal totalPrice = 0;


                    foreach (var item in editOrderDTO.OrderDetails)
                    {
                        Book book = unit.Generic_BookRepo.selectById(item.Book_Id);
                        if (book == null)
                        {
                            return BadRequest($"Book with ID {item.Book_Id} not found.");
                        }

                        if (book.stock <= 0 || book.stock - item.Quantity < 0)
                        {
                            return BadRequest($"Invalid quantity for Book ID {item.Book_Id}");
                        }

                        var existingOrderDetail = order.orderDetails.FirstOrDefault(od => od.book_id == item.Book_Id);

                        if (existingOrderDetail != null)
                        {
                            existingOrderDetail.quantity = item.Quantity;
                            existingOrderDetail.unitPrice = item.unitPrice;
                        }
                        else
                        {
                            OrderDetails newOrderDetails = new OrderDetails()
                            {
                                book_id = item.Book_Id,
                                order_id = editOrderDTO.OrderId,
                                quantity = item.Quantity,
                                unitPrice = item.unitPrice
                            };
                            order.orderDetails.Add(newOrderDetails);
                        }
                        book.stock -= item.Quantity;

                        unit.Generic_BookRepo.update(book);
                    }
                    foreach (var item in order.orderDetails) {

                        totalPrice += item.unitPrice * item.quantity;

                    }

                    order.totalPrice = totalPrice;

                    unit.Generic_OrderRepo.update(order);
                    unit.Save();
                    return Ok();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]

        public IActionResult deleteOrder(int id) 
        { 
            var order = unit.Generic_OrderRepo.selectById(id);
            if (order == null) return NotFound();
            unit.Generic_OrderRepo.remove(id);
            unit.Save();
            return Ok();
        
        }

    }



        
        
        
        
}
