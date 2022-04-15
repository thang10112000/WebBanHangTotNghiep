using Common;
using Model.DAO;
using Model.EF;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebShopOnline.Models;
using WebShopOnline.OrderMomo;

namespace WebShopOnline.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        private const string CartSession = "CartSession";

        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Product.ID == id);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public ActionResult AddItem(long productId, int quantity)
        {
            var product = new ProductDao().ViewItemCart(productId);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                //Gán vào session
                Session[CartSession] = list;
            }
            else
            {
                //tạo mới đối tượng cart item
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //Gán vào session
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address, string email, string content, string productname)
        {
            var order = new Order();

            order.CreatedDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipMobile = mobile;
            order.ShipName = shipName;
            order.ShipEmail = email;
            order.Content = content;
            order.Product = productname;
            try
            {
                var id = new OrderDao().Insert(order);

                var cart = (List<CartItem>)Session[CartSession];
                var detailDao = new Model.DAO.OrderDetailDao();
                decimal total = 0;
                var productName = "";
                var quanlity = 0;
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity); // nè đoạn này tính tổng tiền
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;
                    orderDetail.Price = total;
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.Name = item.Product.Name;

                    detailDao.Insert(orderDetail);

                    productName = item.Product.Name;
                    quanlity += (item.Quantity);
                }
                string contents = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Client/template/neworder.html"));

                contents = contents.Replace("{{CustomerName}}", shipName);
                contents = contents.Replace("{{Products}}", productName);
                contents = contents.Replace("{{Quanlity}}", quanlity.ToString("NO"));
                contents = contents.Replace("{{Phone}}", mobile);
                contents = contents.Replace("{{Email}}", email);
                contents = contents.Replace("{{Address}}", address);
                contents = contents.Replace("{{Total}}", total.ToString("N0"));
                contents = contents.Replace("{{Content}}", content);

                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(email, "Đơn hàng mới từ Shop", contents);
                new MailHelper().SendMail(toEmail, "Đơn hàng mới từ Shop", contents);
            }
            catch (Exception)
            {
                //ghi log
                return Redirect("/loi-thanh-toan");
            }
            return Redirect("/hoan-thanh");
        }

        //[HttpGet]
        //public ActionResult PaymentMomo()
        //{
        //    var cart = Session[CartSession];
        //    var list = new List<CartItem>();
        //    if (cart != null)
        //    {
        //        list = (List<CartItem>)cart;
        //    }
        //    return View(list);
        //}

        //[HttpPost]
        public ActionResult PaymentMomo()
        {
            var cart = (List<CartItem>)Session[CartSession];
            decimal total = 0;
            foreach (var item in cart)
            {
                total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity); // nè đoạn này tính tổng tiền
            }
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOQPV620220414";
            string accessKey = "P6c6TUXasoaoXDpt";
            string serectkey = "9zvvqk80KjI843mDdroa4py9A8MvDoXA";
            string orderInfo = "Đơn mới";
            string returnUrl = "https://localhost:44302/Cart/Success";
            string notifyurl = "http://ba1adf48beba.ngrok.io/Cart/Payment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = total.ToString();
            string orderid = DateTime.Now.Ticks.ToString();
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }
            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult ErrorSuccess()
        {
            return View();
        }
    }
}