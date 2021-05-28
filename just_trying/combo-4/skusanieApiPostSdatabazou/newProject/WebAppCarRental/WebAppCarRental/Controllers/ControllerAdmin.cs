using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppCarRental.Data;
using WebAppCarRental.DTO;
using WebAppCarRental.MakeJson;
using WebAppCarRental.Models;

namespace WebAppCarRental.Controllers
{
    [Authorize]
    [ApiController]
    [Route("admin")]
    public class ControllerAdmin : ControllerBase
    {
        //globalne heslo bez ktoreho nie je mozne vytvorit ADMINA!!!
        private const string globalPasswordAdmin = "112233AA";
        private readonly Interfaces.IJwtAuthManager jwtAuthManager;
        public ControllerAdmin(Interfaces.IJwtAuthManager jwtAuthManager)
        {
            this.jwtAuthManager = jwtAuthManager;
        }

        //tato metoda sluzi na vytvorenie Admina
        //localhost:44353/admin/create
        //body: {"Login":"Admin1","Password":"11111111","Email":"admin1@gmail.com","GlobalPassword":"112233AA"}
        //moze ho vytvorit iba ADMIN!!! s uthorizaciou
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<AdminDTO>> PostAdmin([FromBody] AdminDTO adminDTO)
        {
            //admin ktory vytvara
            string loginWhoIsCreating = adminDTO.LoginWhoIsCreating;

            string login = adminDTO.Login;
            string password = adminDTO.Password;
            string email = adminDTO.Email;
            string globalPassword = adminDTO.GlobalPassword;
            //overenie ci data nie su null alebo prazdny string!!!
            if (login == null || password == null || email == null || globalPassword == null)
            {
                Message message = new Message("Wrong data all data must be filled!!!",400);
                return BadRequest(message);
            }else if(login == "" || password == "" || email == "" || globalPassword == "")
            {
                Message message = new Message("Data must be filled!!!", 400);
                return BadRequest(message);            
            }
            //overime token admina ktory ho vytvara
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var loginClaim = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault();
            if (!loginClaim.Value.Equals(loginWhoIsCreating)) return Unauthorized(new Message("Invalid user-token relationship", 401));

            //overime ci login admin sa uz nenachadza v tabulke!!!
            ContosoUserAdminContext contosoUserAdminContext = new ContosoUserAdminContext();
            foreach (var row in contosoUserAdminContext.Admins)
            {
                if(row.Login == login)
                {
                    Message message = new Message("Login already exist!!! Change it!!!", 400);
                    return BadRequest(message);
                }
            }
            //po uspesnom prejdeni tabulky Admins a ak dany login este neexistuje
            //tak vytvorime daneho Admina
            //no musi vediet Globalne HESLO!!! ak je nespravne tak ho nevytvorime!!!
            if(globalPassword != globalPasswordAdmin)
            {
                Message message = new Message("You do not know correct password. You are not one of the Admin!!!", 400);
                return BadRequest(message);
                //return BadRequest("You do not know correct password. You are not one of the Admin!!!");
            }
            else
            {
                //ak je heslo spravne vytvorime Admina a ulozime do tabulky
                //hned ho prihlasi pojde prec pri tomto
                //var token = jwtAuthManager.Authenticate(login, password);
                //if (token == null || token.Equals("")) return Unauthorized(new Message("Error occured during authentication", 401));
                Admin admin = new Admin()
                {
                    Login = login,
                    Password = password,
                    Email = email,
                    //Token = token
                };
                contosoUserAdminContext.Add(admin);
                contosoUserAdminContext.SaveChanges();
                
                return Ok(new Message("Admin has been created!", 200));
            }
                
        }

        //tato metoda sluzi na prihlasenie ADMINA!!!
        //localhost:44353/admin/login
        //body: {"Login":"Janko","Password":"123456"}
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AdminLoginDTO>> PostLogin([FromBody] AdminLoginDTO adminLoginDTO)
        {
            string login = adminLoginDTO.Login;
            string password = adminLoginDTO.Password;
            //overenie ci login alebo heslo nie su null alebo prazdny string
            Message message = new Message("Wrong Data!!!", 400);
            if (login == null || password == null || login == "" || password == "")
            {
                return BadRequest(message);
            }
            //overenie ci login a heslo su spravne!!!
            ContosoUserAdminContext contosoUserAdminContext = new ContosoUserAdminContext();
            foreach (var row in contosoUserAdminContext.Admins)
            {
                //ak su udaje prihlasovacie spravne generuje token
                if (row.Login == login && row.Password == password)
                {
                    int id = row.Id;
                    using (var context = new Data.ContosoUserAdminContext())
                    {
                        var entity = context.Admins.FirstOrDefault(item => item.Id == id);
                        var token = jwtAuthManager.Authenticate(login, password);
                        if (token == null || token.Equals("")) return Unauthorized(new Message("Error occured during authentication", 401));
                        else
                        {
                            entity.Token = token;
                            context.Admins.Update(entity);
                            context.SaveChanges();
                            return Ok(new JsonWithToken(token, 200, "admin", "Login successful!"));
                        }
                    }
                }
            }
            return BadRequest(message);
        }

        
        //tato metoda sluzi na odhlasenie ADMINA!!!
        //localhost:44353/admin/logout
        //body: {"Login":"Admin1","Token":"T2dihB6TPW8jvUVCwMuKKN2oN"}
        [HttpPost]
        [Route("logout")]
        public async Task<ActionResult<AdminLogoutDTO>> PostLogout([FromBody] AdminLogoutDTO adminLogoutDTO)
        {
            string login = adminLogoutDTO.Login;
            Message message = new Message("Wrong Data!!!", 400);
            if (login == null || login == "")
            {
                return BadRequest(message);
            }
            //overime ci login aj token sa nachadza v tabulke
            ContosoUserAdminContext contosoUserAdminContext = new ContosoUserAdminContext();
            foreach (var row in contosoUserAdminContext.Admins)
            {
                //ak sa Login nachadza v tabulke
                if(row.Login == login)
                {
                    //vyberieme token patriaci k Loginu
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    IEnumerable<Claim> claim = identity.Claims;
                    var loginClaim = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault();
                    if (!loginClaim.Value.Equals(login)) return Unauthorized(new Message("Invalid user-token relationship", 401));
                    //ak sa token aj login zhoduju s tym ktory pride cez Frontend
                    //tak Adminovi vymazeme cely Json Token 
                    int id = row.Id;
                    using (var context = new Data.ContosoUserAdminContext())
                    {
                        var entity = context.Admins.FirstOrDefault(item => item.Id == id);
                        entity.Token = "";
                        context.Admins.Update(entity);
                        context.SaveChanges();
                    }
                    Message message1 = new Message("Admin has been looged out!!!", 200);
                    return Ok(message1);                  
                }
            }
                return BadRequest(message);
        }



        //tato metoda sluzi na vlozenie auta a moze ho vlozit iba Admin!!!
        //localhost:44353/admin/addCar
        //body: {"Login":"Admin1","Password":"11111111","BrandOfCar":"Toyota","Model":"Aygo","Plate":"KE222DE"}
        [HttpPost]
        [Route("addCar")]
        public async Task<ActionResult<CarAddDTO>> PostAddCar([FromBody] CarAddDTO carAddDTO)
        {
            Message message = new Message("Wrong Data!!!", 400);
            string login = carAddDTO.Login;
            string model = carAddDTO.Model;
            string brandOfCar = carAddDTO.BrandOfCar;
            string plate = carAddDTO.Plate;
            double price = carAddDTO.Price;
            //overime ci prisli vsetke udaje
            if(login == null || model == null || brandOfCar == null || plate == null || price == null
                || login == "" || model == "" || brandOfCar == "" || plate == "")
            {
                return BadRequest(message);
            }
            //overime ci Admin existuje!!!
            ContosoUserAdminContext contosoUserAdminContext = new ContosoUserAdminContext();
            foreach (var row in contosoUserAdminContext.Admins)
            {
                if (row.Login == login)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    IEnumerable<Claim> claim = identity.Claims;
                    var loginClaim = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault();
                    if (!loginClaim.Value.Equals(login)) return Unauthorized(new Message("Invalid user-token relationship", 401));

                    //overime ci SPZ auta sa uz nenachadza v tabulke Cars
                    ContosoCarReservationContext contosoCarReservationContext = new ContosoCarReservationContext();
                    foreach (var row2 in contosoCarReservationContext.Cars)
                    {
                        if(row2.Plate == plate)
                        {
                            Message message1 = new Message("Car is already in the table Cars!!!", 400);
                            return BadRequest(message1);
                        }
                    }
                    //ak sa SPZ nebude nachadzat v tabulke Cars tak auto pridame do databazy
                    Car car = new Car()
                    {
                        Model = model,
                        BrandOfCar = brandOfCar,
                        Plate =plate,
                        Price = price,
                    };
                    contosoCarReservationContext.Add(car);
                    contosoCarReservationContext.SaveChanges();
                    Message message2 = new Message("The car has been added!!!", 201);
                    return Ok(message2);
                }
            }
            return BadRequest(message);
        }
    }
}
