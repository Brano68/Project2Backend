using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppCarRental.CodePassword;
using WebAppCarRental.Data;
using WebAppCarRental.DTO;
using WebAppCarRental.Email;
using WebAppCarRental.MakeJson;
using WebAppCarRental.Models;

namespace WebAppCarRental.Controllers
{
    [Authorize]
    [ApiController]
    [Route("userrr")]
    public class ControllerUser : ControllerBase
    {
        private readonly Interfaces.IJwtAuthManager jwtAuthManager;
        public ControllerUser(Interfaces.IJwtAuthManager jwtAuthManager)
        {
            this.jwtAuthManager = jwtAuthManager;
        }
        //vytvorenie uctu
        //https://localhost:44353/userrr/create
        //body: {"Login":"Brano","Password":"123456","Email":"branislav.nebus@kosickaaademia.com"}
        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<UserDTO>> PostUser([FromBody] UserDTO userDTO)
        {
            string login = userDTO.Login;
            string password = userDTO.Password;
            string email = userDTO.Email;
            if (login == null || password == null || email == null
                || login == "" || password == "" || email == "")
            {
                Message message = new Message("Wrong data!!!", 400);
                return BadRequest(message);
            }
            //overenie ci login taky uz sa nenachadza
            ContosoUserAdminContext contosoUserAdminContext = new Data.ContosoUserAdminContext();
            foreach (var row in contosoUserAdminContext.Users)
            {
                if (login == row.Login)
                {
                    Message message = new Message("Login already exist!!!", 400);
                    return BadRequest(message);
                }
            }
            //ak je vsetko OK zapis do databazy AllMembers tabulky Users
            var token = jwtAuthManager.Authenticate(login, password);
            if (token == null || token.Equals("")) return Unauthorized(new Message("Error occured during authentication", 401));
            User user = new User()
            {
                Login = login,
                //heslo zakodujeme
                //Password = password,
                Password = MakeCode.MD5Hash(password),
                Email = email,
                Token = token
            };
            contosoUserAdminContext.Add(user);
            contosoUserAdminContext.SaveChanges();

            return Ok(new JsonWithToken(token, 200, "user", "User has been created!"));
        }



        //prihlasenie a vygenerovanie tokenu s casom aktualnym
        //https://localhost:44353/userrr/login
        //body: {"Login":"Brano","Password":"123456"}
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserLoginDTO>> PostLogin([FromBody] UserLoginDTO userLoginDTO)
        {
            string login = userLoginDTO.Login;
            string password = userLoginDTO.Password;
            if (login == null || password == null || login == "" || password == "")
            {
                Message message = new Message("Wrong data!!!", 400);
                return BadRequest(message);
            }
            //kodujeme heslo
            password = MakeCode.MD5Hash(password);

            //overenie ci login a heslo su spravne
            ContosoUserAdminContext contosoUserAdminContext = new Data.ContosoUserAdminContext();
            foreach (var row in contosoUserAdminContext.Users)
            {
                if (login == row.Login && password == row.Password)
                {
                    //generovanie tokenu a casu a aktualizacia do databazy!!!
                    int id = row.Id;
                    string email = row.Email;
                    using (var context = new Data.ContosoUserAdminContext())
                    {
                        var entity = context.Users.FirstOrDefault(item => item.Id == id);
                        var token = jwtAuthManager.Authenticate(login, password);
                        if (token == null || token.Equals("")) return Unauthorized(new Message("Error occured during authentication", 401));
                        else
                        {
                            entity.Token = token;
                            context.Users.Update(entity);
                            context.SaveChanges();

                            //overime ci ma vyplnene udaje Meno priezvisko a tieto veci
                            ContosoUserAdminContext contosoUserAdminContext1 = new ContosoUserAdminContext();
                            foreach(var row1 in contosoUserAdminContext1.InformationAboutCustomers)
                            {
                                if(id == row1.UserId)
                                {
                                    MakeJsonWithUserData makeJsonWithUserData = new MakeJsonWithUserData(token, new Message("Login succesful", 200),"user",row1.UserFname,
                                        row1.UserLname, row1.PhoneNumber, row1.State, row1.City, row1.Adress,
                                        row1.PostCode, row1.DriverLicenceNumber, row1.IdNumber,email);
                                    return Ok(makeJsonWithUserData);

                                }
                            }
                            return Ok(new JsonWithToken(token, 200, "user", "Login successful!"));
                        }
                    }
                }
            }
            return BadRequest(new Message("Wrong data!!", 400)); 
        }


        //tato metoda sluzi na update a vyplnenie udajov usera ak este neboli vyplnene
        [HttpPost]
        [Route("fillData")]
        public async Task<ActionResult<UserDataFillDTO>> PostData([FromBody] UserDataFillDTO userDataFillDTO)
        {
            //vyberame udaje z body
            string login = userDataFillDTO.Login;
            string fname = userDataFillDTO.UserFname;
            string lname = userDataFillDTO.UserLname;
            string phoneNumber = userDataFillDTO.PhoneNumber;
            string state = userDataFillDTO.State;
            string city = userDataFillDTO.City;
            string adress = userDataFillDTO.Adress;
            string postCode = userDataFillDTO.PostCode;
            string driverLicenceNumber = userDataFillDTO.DriverLicenceNumber;
            int idNumber = userDataFillDTO.IdNumber;

            //overime token
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var loginClaim = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault();
            if (!loginClaim.Value.Equals(login)) return Unauthorized(new Message("Invalid user-token relationship", 401));

            //zistime idecko na login co bude cudzi kluc v tabulke InformationAboutCustomers
            int idUser = 0;
            ContosoUserAdminContext contosoUserAdminContext = new ContosoUserAdminContext();
            foreach(var row in contosoUserAdminContext.Users)
            {
                if(row.Login == login)
                {
                    idUser = row.Id;
                    break;
                }
            }

            //ak najdeme idecko urobime insert v pripade ze uz sa idecko nachadza urobime update

            foreach (var row in contosoUserAdminContext.InformationAboutCustomers)
            {
                //ak idecko uz tam take je znamena to ze user uz udaje ma nahrate
                //cize urobime update
                if(row.UserId == idUser)
                {
                    using (var context = new Data.ContosoUserAdminContext())
                    {
                        var entity = context.InformationAboutCustomers.FirstOrDefault(item => item.Id == row.Id);
                        entity.UserId = idUser;
                        entity.UserFname = fname;
                        entity.UserLname = lname;
                        entity.PhoneNumber = phoneNumber;
                        entity.State = state;
                        entity.City = city;
                        entity.Adress = adress;
                        entity.PostCode = postCode;
                        entity.DriverLicenceNumber = driverLicenceNumber;
                        entity.IdNumber = idNumber;
                        context.InformationAboutCustomers.Update(entity);
                        context.SaveChanges();
                        return Ok(new Message("Data has been saved", 201));
                    }
                }
            }

            //ak este nie su v tabulke yiadne informacie o userovi
            //tak ich zapiseme do tabulky
            InformationAboutCustomer informationAboutCustomer = new InformationAboutCustomer()
            {
                UserId = idUser,
                UserFname = fname,
                UserLname = lname,
                PhoneNumber = phoneNumber,
                State = state,
                City = city,
                Adress = adress,
                PostCode = postCode,
                DriverLicenceNumber = driverLicenceNumber,
                IdNumber = idNumber,
            };

            contosoUserAdminContext.Add(informationAboutCustomer);
            contosoUserAdminContext.SaveChanges();

            return Ok(new Message("Data has been saved", 200));
        }




            //odhlasenie usra token sa vymaze z databazy
            //https://localhost:44353/userrr/logout
            //body: {"Login":"Brano","Token":"mUZcwc6gm85CF1UvM3xdp6Fku"}
            [HttpPost]
        [Route("logout")]
        public async Task<ActionResult<UserLogoutDTO>> PostLogout([FromBody] UserLogoutDTO userLogoutDTO)
        {
            Message message = new Message("Wrong data", 400);
            string login = userLogoutDTO.Login;
            if (login == null || login == "")
            {
                return BadRequest(message);
            }
            //overime ci login aj token sa nachadza v tabulke
            ContosoUserAdminContext contosoUserAdminContext = new ContosoUserAdminContext();
            foreach (var row in contosoUserAdminContext.Users)
            {
                //ak sa Login nachadza v tabulke
                if (row.Login == login)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    IEnumerable<Claim> claim = identity.Claims;
                    var loginClaim = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault();
                    if (!loginClaim.Value.Equals(login)) return Unauthorized(new Message("Invalid user-token relationship", 401));
                    //vyberieme token patriaci k Loginu   
                    //ak sa token aj login zhoduju s tym ktory pride cez Frontend
                    //tak Userovi vymazeme cely Json Token                    
                    int id = row.Id;
                    using (var context = new Data.ContosoUserAdminContext())
                    {
                        var entity = context.Users.FirstOrDefault(item => item.Id == id);
                        entity.Token = "";
                        context.Users.Update(entity);
                        context.SaveChanges();
                    }
                    Message message1 = new Message("User has been looged out!!!", 200);
                    return Ok(message1);
                }
            }
            return BadRequest(message);
        }

        //vypozicanie auta
        [HttpPost]
        [Route("borrow")]
        public async Task<ActionResult<BorrowCarDTO>> PostBorrow([FromBody] BorrowCarDTO borrowCarDTO)
        {
            string login = borrowCarDTO.Login;
            string password = borrowCarDTO.Password;
            string from = borrowCarDTO.From;
            string to = borrowCarDTO.To;
            string plate = borrowCarDTO.Plate;
            //overenie ci data su OK
            if(login == null || password == null || from == null || to == null || plate == null
                || login == "" || password == "" || from == "" || to == "" || plate == "")
            {
                Message message = new Message("Wrong data!!!", 400);
                return BadRequest(message);
            }
            //kodujeme heslo
            password = MakeCode.MD5Hash(password);
            //zistime si user a heslo cu su spravne a ak ano tak ake Id ma user
            int idUser = 0, idCar = 0;
            string email = "";
            ContosoUserAdminContext contosoUserAdminContext = new Data.ContosoUserAdminContext();
            double pricePerDay = 0;
            //zistime idecko usra
            foreach (var row in contosoUserAdminContext.Users)
            {
                if (login == row.Login && password == row.Password)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    IEnumerable<Claim> claim = identity.Claims;
                    var loginClaim = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault();
                    if (!loginClaim.Value.Equals(login)) return Unauthorized(new Message("Invalid user-token relationship", 401));

                    idUser = row.Id;
                    email = row.Email;
                    //teraz zistime Idecko auta
                    ContosoCarReservationContext contosoCarReservationContext = new ContosoCarReservationContext();
                    foreach(var row2 in contosoCarReservationContext.Cars)
                    {
                        if(row2.Plate == plate)
                        {
                            pricePerDay = row2.Price;
                            idCar = row2.Id;
                        }
                    }
                }
            }
            //teraz overime ci idecko usra a idecko auta je ine ako 0 ked hej pokracujeme ked ne vraciame BadRequest
            if(idCar == 0 || idUser == 0)
            {
                Message message = new Message("Car or user was not found!!!", 400);
                return BadRequest(message);
            }
            //teraz overime datum ci auto je volne na nas prijaty datum
            //cize prejdem databazu a overujem kde je idCar a checkujem datum ci sa neprekryva!!!
            //a ked sa nic neprekryva vyberiem email a zavolam funkciu nech posle email a vrati 200 -> OK
            if(checkIfDateIsAvailable(from, to, idCar))
            {
                //datum neni obsadeny posielam email
                //email posiela Illia v jeho funkcii
                /*
                SendEmail sendEmail = new SendEmail();
                sendEmail.sendEmailToCustomer(email);
                */
                //a zapisujem do tabulky
                ContosoCarReservationContext contosoCarReservationContext2 = new ContosoCarReservationContext();        
                Reservation reservation = new Reservation()
                {
                    UserLogin = login,
                    From = from,
                    To = to,
                    CarId = idCar,
                    UserId = idUser,
                    //metoda na zistenie kolko dni a vynasobime cenou vozidla ale este pripocitame raz cenu vozidla
                    //lebo pri pocitani dni vrati o jeden menej
                    PricePerDays = howManyDays(from, to) * pricePerDay + pricePerDay,
                    
                };
                /* Illia zapisuje aj do tabulky ked je vsetko ok po platbe
                contosoCarReservationContext2.Add(reservation);
                contosoCarReservationContext2.SaveChanges();  
                */
                //return Ok(new JsonWithPrice("You should get email", reservation.PricePerDays, 200));
                InformationAboutCustomer informationAboutCustomer = null;

                foreach (var row3 in contosoUserAdminContext.InformationAboutCustomers)
                {
                    if(idUser == row3.UserId)
                    {
                        informationAboutCustomer = new InformationAboutCustomer()
                        {
                            Id = row3.Id,
                            UserId = row3.UserId,
                            UserFname = row3.UserFname,
                            UserLname = row3.UserLname,
                            PhoneNumber = row3.PhoneNumber,
                            State = row3.State,
                            City = row3.City,
                            Adress = row3.Adress,
                            PostCode = row3.PostCode,
                            DriverLicenceNumber = row3.DriverLicenceNumber,
                            IdNumber = row3.IdNumber,
                        };
                        //return Ok(new JsonWithPrice("Data ok. You have to pay now.", reservation.PricePerDays, 200, reservation.CarId, reservation.UserId, reservation.UserLogin, reservation.From, reservation.To, informationAboutCustomer));
                    }
                }
                return Ok(new JsonWithPrice("Data ok. You have to pay now.", reservation.PricePerDays, 200, reservation.CarId, reservation.UserId, reservation.UserLogin, reservation.From, reservation.To, informationAboutCustomer));
                //return Ok(new JsonWithPrice("Data ok. You have to pay now.", reservation.PricePerDays, 200, reservation.CarId, reservation.UserId, reservation.UserLogin, reservation.From, reservation.To));
            }
            else
            {
                Message message = new Message("Date is taken already!!!", 400);
                return BadRequest(message);
            }
        }


        //pomocna metoda na overovanie ci auto neni obsadene!!!
        private Boolean checkIfDateIsAvailable(string dateFrom, string dateTo, int idCar)
        {
            DateTime dateTimeFrom = DateTime.Parse(dateFrom);
            DateTime dateTimeTo = DateTime.Parse(dateTo);
            //ak datum vratenia je mensi ako pozicania vrati false
            if(dateTimeTo < dateTimeFrom)
            {
                return false;
            }

            //teraz prejde tabulku Reservations
            ContosoCarReservationContext contosoCarReservationContext = new ContosoCarReservationContext();
            foreach (var row in contosoCarReservationContext.Reservations)
            {
                if(row.CarId == idCar)
                {
                    DateTime dtFromDB = DateTime.Parse(row.From);
                    DateTime dtToDB = DateTime.Parse(row.To);
                    //ak zadany datum je vacsi ako dtToDb nech pokracuje
                    if (dateTimeFrom > dtToDB)
                    {
                        continue;
                    }
                    else if (dateTimeTo < dtFromDB)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                
            }
            return true;
        }


        //pomocna metoda na vratenie kolko dni
        private double howManyDays(string from, string to)
        {
            DateTime dateFrom = DateTime.Parse(from);
            DateTime dateTo = DateTime.Parse(to);
            double days = (dateTo - dateFrom).TotalDays;
            return days;
        }


        ////3.5.2021
        ///https://localhost:44353/userrr/reservation
        [HttpGet]
        [Route("reservation")]
        public IActionResult showReservations([FromQuery(Name = "LoginOfUser")] string loginOfUser)
        {
            if(loginOfUser == null)
            {
                return BadRequest(new Message("Login is empty",401));
            }
            //overenie tokenu
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var loginClaim = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault();
            if (!loginClaim.Value.Equals(loginOfUser)) return Unauthorized(new Message("Invalid user-token relationship", 401));

            //ak token prejde
            //mapujeme tabulku rezervacie
            ContosoCarReservationContext contosoCarReservationContext = new ContosoCarReservationContext();
            ContosoCarReservationContext contosoCarReservationContext2 = new ContosoCarReservationContext();
            List<Order> orders = new List<Order>();
            foreach(var row in contosoCarReservationContext.Reservations)
            {
                if(row.UserLogin == loginOfUser)
                {
                    foreach(var row2 in contosoCarReservationContext2.Cars)
                    {
                        if(row.CarId == row2.Id)
                        {
                            Order order = new Order(row2.BrandOfCar, row2.Model, row2.Plate, row.PricePerDays, row.From, row.To);
                            orders.Add(order);
                        }
                    }
                    
                }
            }
            MyOrders myOrders = new MyOrders(orders);
            return Ok(myOrders);
        }

        [HttpPost]
        [Route("definitely")]
        public async Task<ActionResult<ReceiverDTO>> PostDefinitelly([FromBody] ReceiverDTO receiverDTO)
        {
            string login = receiverDTO.Login;
            int carID = receiverDTO.CarID;
            int userID = receiverDTO.UserID;
            string from = receiverDTO.From;
            string to = receiverDTO.To;
            double pricePerDays = receiverDTO.PricePerDays;
            //overime token
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var loginClaim = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault();
            if (!loginClaim.Value.Equals(login)) return Unauthorized(new Message("Invalid user-token relationship", 401));


            //zistime email a login
            string email = "";
           

            ContosoUserAdminContext contosoUserAdminContext = new ContosoUserAdminContext();
            foreach(var row in contosoUserAdminContext.Users)
            {
                if(row.Id == userID)
                {
                    email = row.Email;
                }
            }

            //posleme email
            new SendEmail().sendEmailToCustomer(email);

            //zapiseme ho este do tabulky REZERVACII
            Reservation reservation = new Reservation()
            {
                CarId = carID,
                UserId = userID,
                UserLogin = login,
                From = from,
                To = to,
                PricePerDays = pricePerDays,

            };
            ContosoCarReservationContext contosoCarReservationContext = new ContosoCarReservationContext();
            contosoCarReservationContext.Add(reservation);
            contosoCarReservationContext.SaveChanges();

            return Ok(new Message("You should get an email", 200));
        }


        }
}
