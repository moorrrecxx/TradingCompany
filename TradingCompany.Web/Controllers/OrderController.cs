using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using TradingCompany.BLL.Concreate;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DALEF.Concrete;
using TradingCompany.DTO;

namespace TradingCompany.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderManager orderManager;
        private readonly IMapper mapper;
        private readonly ILogger<OrderController> logger;
        public OrderController(ILogger<OrderController> logger, IOrderManager orderManager, IMapper mapper)
        {
            this.logger = logger;
            this.orderManager = orderManager;
            this.mapper = mapper;
        }
        // GET: OrderController
        public ActionResult Index()
        {
            var Order = orderManager.GetAllOrders();
            return View(Order);
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            var statuses = orderManager.GetAllStatuses();

            ViewBag.StatusId = new SelectList(statuses, "StatusId", "StatusName");

            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderDTO orderDTO)
        {
            try
            {
                ValidateOrder(orderDTO);
                if (ModelState.IsValid)
                {
                    if (orderDTO.CreatedAt == default)
                    {
                        orderDTO.CreatedAt = DateTime.Now;
                    }

                    orderManager.CreateOrder(orderDTO);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating order");
                ModelState.AddModelError("", "Unable to save changes. Try again.");
            }
            var statuses = orderManager.GetAllStatuses();
            ViewBag.StatusId = new SelectList(statuses, "StatusId", "StatusName");
            return View(orderDTO);
            }   

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            var order = orderManager.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            var statuses = orderManager.GetAllStatuses();

            ViewBag.StatusId = new SelectList(statuses, "StatusId", "StatusName");

            return View(order);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OrderDTO orderDTO)
        {
            if (id != orderDTO.OrderId)
            {
                return BadRequest();
            }

            try
            {
                ValidateOrder(orderDTO);
                if (ModelState.IsValid)
                {
                    orderManager.UpdateOrder(orderDTO);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating order");
                ModelState.AddModelError("", "Unable to save changes. Try again.");
            }

            var statuses = orderManager.GetAllStatuses();
            ViewBag.StatusId = new SelectList(statuses, "StatusId", "StatusName");

            return View(orderDTO);
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            var order = orderManager.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, OrderDTO orderDTO)
        {
            try
            {
                orderManager.DeleteOrder(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting order");
                var order = orderManager.GetOrderById(id);
                ModelState.AddModelError("", "Deletion failed. Note: You cannot delete an order if it has associated shipments.");
                return View(order);
            }
        }
        private void ValidateOrder(OrderDTO orderDTO)
        {
            if (string.IsNullOrWhiteSpace(orderDTO.Phone))
            {
                ModelState.AddModelError("Phone", "Номер телефону є обов'язковим.");
            }
            else
            {
                orderDTO.Phone = orderDTO.Phone.Trim();

                if (orderDTO.Phone.Length != 13 || !orderDTO.Phone.StartsWith("+380"))
                {
                    ModelState.AddModelError("Phone", "Номер повинен мати рівно 13 символів і починатися з +380 (наприклад: +380671234567).");
                }
                else
                {
                    var allOrders = orderManager.GetAllOrders();

                    bool isDuplicate = allOrders.Any(o =>
                        o.Phone != null &&
                        o.Phone.Trim() == orderDTO.Phone &&
                        o.OrderId != orderDTO.OrderId);

                    if (isDuplicate)
                    {
                        ModelState.AddModelError("Phone", "Замовлення з таким номером телефону вже існує.");
                    }
                }
            }
        }

    }
}
