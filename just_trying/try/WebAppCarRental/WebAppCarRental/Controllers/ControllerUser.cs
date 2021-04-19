using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCarRental.Data;
using WebAppCarRental.DTO;
using WebAppCarRental.Email;
using WebAppCarRental.MakeToken;
using WebAppCarRental.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
                return BadRequest("Wrong data!!!");
            }
            //overenie ci login taky uz sa nenachadza
            ContosoUserAdminContext contosoUserAdminContext = new Data.ContosoUserAdminContext();
            foreach (var row in contosoUserAdminContext.Users)
            {
                if (login == row.Login)
                {
                    return BadRequest("Login already exist!!!");
                }
            }
            //ak je vsetko OK zapis do databazy AllMembers tabulky Users
            User user = new User()
            {
                Login = login,
                Password = password,
                Email = email
            };
            contosoUserAdminContext.Add(user);
            contosoUserAdminContext.SaveChanges();
            return Ok();
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
                return BadRequest("Wrong data!!!");
            }
            //overenie ci login a heslo su spravne
            ContosoUserAdminContext contosoUserAdminContext = new Data.ContosoUserAdminContext();
            foreach (var row in contosoUserAdminContext.Users)
            {
                if (login == row.Login && password == row.Password)
                {
                    //generovanie tokenu a casu a aktualizacia do databazy!!!
                    int id = row.Id;
                    //string json = new Token().createTokenWithDateAndTimeAsJson();
                    using (var context = new Data.ContosoUserAdminContext())
                    {
                        var entity = context.Users.FirstOrDefault(item => item.Id == id);
                        var token = jwtAuthManager.Authenticate(login, password);
                        if (token == null || token.Equals("")) return Unauthorized();
                        else
                        {
                            entity.Token = token;
                            context.Users.Update(entity);
                            context.SaveChanges();
                            return Ok("Signed up! " +token);
                        }
                        
                    }
                    //return Ok("You have just been signed up " );
                }
            }
            return BadRequest("Wrong data!!!"); ;
        }


        //odhlasenie usra token sa vymaze z databazy
        //https://localhost:44353/userrr/logout
        //body: {"Login":"Brano","Token":"mUZcwc6gm85CF1UvM3xdp6Fku"}
        [HttpPost]
        [Route("logout")]
        public async Task<ActionResult<UserLogoutDTO>> PostLogout([FromBody] UserLogoutDTO userLogoutDTO)
        {
            string login = userLogoutDTO.Login;
            string token = userLogoutDTO.Token;
            if (login == null || token == null || login == "" || token == "")
            {
                return BadRequest("Wrong Data");
            }
            //overime ci login aj token sa nachadza v tabulke
            ContosoUserAdminContext contosoUserAdminContext = new ContosoUserAdminContext();
            foreach (var row in contosoUserAdminContext.Users)
            {
                //ak sa Login nachadza v tabulke
                if (row.Login == login)
                {
                    //vyberieme token patriaci k Loginu
                    string tokenWithDateFromDatabase = row.Token;
                    var details = JObject.Parse(tokenWithDateFromDatabase);
                    string tokenWithoutDate = details["Token"].ToString();
                    //ak sa token aj login zhoduju s tym ktory pride cez Frontend
                    //tak Userovi vymazeme cely Json Token
                    if (token == tokenWithoutDate)
                    {
                        int id = row.Id;
                        using (var context = new Data.ContosoUserAdminContext())
                        {
                            var entity = context.Users.FirstOrDefault(item => item.Id == id);
                            entity.Token = "";
                            context.Users.Update(entity);
                            context.SaveChanges();
                        }
                        return Ok("User has been looged out!!!");
                    }
                }
            }
            return BadRequest("Wrong data!!!");
        }

        [HttpGet("try")]
        public string Try()
        {
            return "ahaa ideeee";
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
                return BadRequest("Wrong data!!!");
            }
            //zistime si user a heslo cu su spravne a ak ano tak ake Id ma user
            int idUser = 0, idCar = 0;
            string email = "";
            ContosoUserAdminContext contosoUserAdminContext = new Data.ContosoUserAdminContext();
            //zistime idecko usra
            foreach (var row in contosoUserAdminContext.Users)
            {
                
                if (login == row.Login && password == row.Password)
                {
                    idUser = row.Id;
                    email = row.Email;
                    //teraz zistime Idecko auta
                    ContosoCarReservationContext contosoCarReservationContext = new ContosoCarReservationContext();
                    foreach(var row2 in contosoCarReservationContext.Cars)
                    {
                        if(row2.Plate == plate)
                        {
                            idCar = row2.Id;
                        }
                    }
                }
            }
            //teraz overime ci idecko usra a idecko auta je ine ako 0 ked hej pokracujeme ked ne vraciame BadRequest
            if(idCar == 0 || idUser == 0)
            {
                return BadRequest("Car or user was not found!!!");
            }
            //teraz overime datum ci auto je volne na nas prijaty datum
            //cize prejdem databazu a overujem kde je idCar a checkujem datum ci sa neprekryva!!!
            //a ked sa nic neprekryva vyberiem email a zavolam funkciu nech posle email a vrati 200 -> OK
            if(checkIfDateIsAvailable(from, to))
            {
                //datum neni obsadeny posielam email
                SendEmail sendEmail = new SendEmail();
                sendEmail.sendEmailToCustomer(email);
                //a zapisujem do tabulky
                ContosoCarReservationContext contosoCarReservationContext2 = new ContosoCarReservationContext();
                Reservation reservation = new Reservation()
                {
                    UserLogin = login,
                    From = from,
                    To = to,
                    CarId = idCar,
                    UserId = idUser,
                };
                contosoCarReservationContext2.Add(reservation);
                contosoCarReservationContext2.SaveChanges();

                return Ok();
            }
            else
            {
                return BadRequest("Date is taken already!!!");
            }
        }


        //pomocna metoda na overovanie ci auto neni obsadene!!!
        private Boolean checkIfDateIsAvailable(string dateFrom, string dateTo)
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
                DateTime dtFromDB = DateTime.Parse(row.From);
                DateTime dtToDB = DateTime.Parse(row.To);
                //ak zadany datum je vacsi ako dtToDb nech pokracuje
                if(dateTimeFrom > dtToDB)
                {
                    continue;
                }else if(dateTimeTo < dtFromDB)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

    }
}
