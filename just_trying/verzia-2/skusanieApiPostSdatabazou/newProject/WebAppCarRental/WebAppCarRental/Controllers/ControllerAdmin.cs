using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCarRental.Data;
using WebAppCarRental.DTO;
using WebAppCarRental.MakeJson;
using WebAppCarRental.MakeToken;
using WebAppCarRental.Models;

namespace WebAppCarRental.Controllers
{
    [ApiController]
    [Route("admin")]
    public class ControllerAdmin : ControllerBase
    {
        //globalne heslo bez ktoreho nie je mozne vytvorit ADMINA!!!
        private const string globalPasswordAdmin = "112233AA";

        //tato metoda sluzi na vytvorenie Admina
        //localhost:44353/admin/create
        //body: {"Login":"Admin1","Password":"11111111","Email":"admin1@gmail.com","GlobalPassword":"112233AA"}
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<AdminDTO>> PostAdmin([FromBody] AdminDTO adminDTO)
        {
            string login = adminDTO.Login;
            string password = adminDTO.Password;
            string email = adminDTO.Email;
            string globalPassword = adminDTO.GlobalPassword;
            //overenie ci data nie su null alebo prazdny string!!!
            if (login == null || password == null || email == null || globalPassword == null)
            {
                Message message = new Message("Wrong data all data must be filled!!!",400);
                return BadRequest(message);
                //return BadRequest("Wrong data all data must be filled!!!");
            }else if(login == "" || password == "" || email == "" || globalPassword == "")
            {
                Message message = new Message("Data must be filled!!!", 400);
                return BadRequest(message);
                //return BadRequest("Data must be filled!!!");
            }
            //overime ci login admin sa uz nenachadza v tabulke!!!
            ContosoUserAdminContext contosoUserAdminContext = new ContosoUserAdminContext();
            foreach (var row in contosoUserAdminContext.Admins)
            {
                if(row.Login == login)
                {
                    Message message = new Message("Login already exist!!! Change it!!!", 400);
                    return BadRequest(message);
                    //return BadRequest("Login already exist!!! Change it!!!");
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
                Admin admin = new Admin()
                {
                    Login = login,
                    Password = password,
                    Email = email,
                };
                contosoUserAdminContext.Add(admin);
                contosoUserAdminContext.SaveChanges();
                Message message = new Message("Admin has been created.", 201);
                return Ok(message);
            }
                
        }


        //tato metoda sluzi na prihlasenie ADMINA!!!
        //localhost:44353/admin/login
        //body: {"Login":"Janko","Password":"123456"}
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
                    string json = new Token().createTokenWithDateAndTimeAsJson();
                    using (var context = new Data.ContosoUserAdminContext())
                    {
                        var entity = context.Admins.FirstOrDefault(item => item.Id == id);
                        entity.Token = json;
                        context.Admins.Update(entity);
                        context.SaveChanges();
                    }
                    return Ok("Token with time has been generated " + json);
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
            string token = adminLogoutDTO.Token;
            Message message = new Message("Wrong Data!!!", 400);
            if (login == null || token == null || login == "" || token == "")
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
                    string tokenWithDateFromDatabase = row.Token;
                    var details = JObject.Parse(tokenWithDateFromDatabase);
                    string tokenWithoutDate = details["Token"].ToString();
                    //ak sa token aj login zhoduju s tym ktory pride cez Frontend
                    //tak Adminovi vymazeme cely Json Token
                    if(token == tokenWithoutDate)
                    {
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
            string password = carAddDTO.Password;
            string model = carAddDTO.Model;
            string brandOfCar = carAddDTO.BrandOfCar;
            string plate = carAddDTO.Plate;
            double price = carAddDTO.Price;
            //overime ci prisli vsetke udaje
            if(login == null || password == null || model == null || brandOfCar == null || plate == null || price == null
                || login == "" || password == "" || model == "" || brandOfCar == "" || plate == "")
            {
                return BadRequest(message);
            }
            //overime ci Admin existuje!!!
            ContosoUserAdminContext contosoUserAdminContext = new ContosoUserAdminContext();
            foreach (var row in contosoUserAdminContext.Admins)
            {
                if (row.Login == login && row.Password == password)
                {
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
