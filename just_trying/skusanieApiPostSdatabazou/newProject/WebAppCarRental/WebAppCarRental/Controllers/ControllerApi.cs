using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCarRental.DTO;
using WebAppCarRental.Email;
using WebAppCarRental.MakeToken;
using WebAppCarRental.Models;

namespace WebAppCarRental.Controllers
{
    [ApiController]
    [Route("user")]
    public class ControllerApi : ControllerBase
    {
        //localhost:44353/user/tokenAndDate
        [HttpGet]
        [Route("tokenAndDate")]
        public IActionResult getToken()
        {
            Token token = new Token();

            return Ok(token.createTokenWithDateAndTimeAsJson());
        }


        //zistenie Loginov usrov vsetkych
        //volanie cez: localhost:44535/user/create
        //localhost:44353/user/all
        [HttpGet]
        [Route("all")]
        public IActionResult getAllUser()
        {
            using Data.ContosoUserContext contosoUserContext = new Data.ContosoUserContext();
            List<string> list = new List<string>();
            foreach(var row in contosoUserContext.Users)
            {
                list.Add(row.Login);
            }
            return Ok(list);
        }

        //vytvorenie uctu
        //localhost:44353/user/create
        //body: {"Login":"Janko","Password":"123456","Email":"user@gmail.com"}
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
            using Data.ContosoUserContext contosoUserContext = new Data.ContosoUserContext();
            //List<User> list = new List<User>();
            foreach (var row in contosoUserContext.Users)
            {
                if (login == row.Login)
                {
                    return BadRequest("Login already exist!!!");
                }
            }
            //ak je vsetko OK zapis do databazy AllUsers
            User user = new User()
            {
                Login = login,
                Password = password,
                Email = email
            };
            contosoUserContext.Add(user);
            contosoUserContext.SaveChanges();
            return Ok();
        }


        //prihlasenie a vygenerovanie tokenu s casom aktualnym
        //localhost:44353/user/login
        //body: {"Login":"Janko","Password":"123456"}
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AccessDTO>> PostLogin([FromBody] AccessDTO accessDTO)
        {
            string login = accessDTO.Login;
            string password = accessDTO.Password;
            if (login == null || password == null || login == "" || password == "") {
                return BadRequest("Wrong data!!!");
            }
            //overenie ci login a heslo su spravne
            using Data.ContosoUserContext contosoUserContext = new Data.ContosoUserContext();
            
            foreach (var row in contosoUserContext.Users)
            {
                if (login == row.Login && password == row.Password)
                {
                    
                    //generovanie tokenu a casu a aktualiyacia do databazy!!!
                    int id = row.Id;
                    string json = new Token().createTokenWithDateAndTimeAsJson();
                    using (var context = new Data.ContosoUserContext())
                    {
                        var entity = context.Users.FirstOrDefault(item => item.Id == id);
                        entity.Token = json;
                        context.Users.Update(entity);
                        context.SaveChanges();
                    }
                    //

                    return Ok("Bol vygenerovany token kluc s aktualnym casom: " + json);
                }
            }
            return BadRequest("Wrong data!!!"); ;
        }


        ////skusam email
        //localhost:44353/user/send
        [HttpGet]
        [Route("send")]
        public IActionResult sendEm()
        {
            SendEmail sendEmail = new SendEmail();
            sendEmail.sendEmailToCustomer();

            return Ok();
        }

    }
}
